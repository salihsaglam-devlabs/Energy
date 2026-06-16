using Energy.Shared.Common;
using Energy.Application.Finance.Services;
using Energy.Domain.Modules.Budget;
using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Common;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Finance;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.ProgressPayments;
using Energy.Infrastructure.Finance.Services;
using Energy.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Finance iş kuralı testleri: ödeme/tahsilat parçalı kapama, over-allocation engeli,
/// puantaj maliyeti, hakediş→alacak ve bütçe aşımı bildirimi.
/// </summary>
public sealed class FinanceServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly FinanceService _finance;

    private Guid _currencyId, _companyId, _partnerId;

    public FinanceServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _finance = new FinanceService(_db, NullLogger<FinanceService>.Instance);

        var currency = new Currency { Id = Guid.NewGuid(), Code = "TRY", Name = "TRY", IsActive = true };
        _db.Currencies.Add(currency);
        _currencyId = currency.Id;
        var company = new Company { Id = Guid.NewGuid(), Code = "C1", Name = "Co", BaseCurrencyId = currency.Id, IsActive = true };
        _db.Companies.Add(company);
        _companyId = company.Id;
        var partner = new BusinessPartner { Id = Guid.NewGuid(), PartnerType = PartnerType.Customer, Code = "BP", Name = "Partner", IsActive = true };
        _db.BusinessPartners.Add(partner);
        _partnerId = partner.Id;
        _db.SaveChanges();
    }

    [Fact]
    public async Task Payment_allocation_partially_then_fully_closes_payable()
    {
        var payableId = await _finance.CreatePayableAsync(_partnerId, _currencyId, 1000m, DateTime.UtcNow, null, null, null);

        var payment = new Payment
        {
            Id = Guid.NewGuid(), PartnerId = _partnerId, CurrencyId = _currencyId,
            Amount = 1000m, PaymentDate = DateTime.UtcNow, PaymentNo = "PAY-1",
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        await _finance.AllocatePaymentAsync(payment.Id, new[] { new FinanceAllocationLine(payableId, 600m) });
        Assert.Equal(400m, (await _db.Payables.FindAsync(payableId))!.RemainingAmount);

        await _finance.AllocatePaymentAsync(payment.Id, new[] { new FinanceAllocationLine(payableId, 400m) });
        var payable = await _db.Payables.FindAsync(payableId);
        Assert.Equal(0m, payable!.RemainingAmount);
        Assert.True(payable.IsClosed);
    }

    [Fact]
    public async Task Payment_over_allocation_is_blocked()
    {
        var payableId = await _finance.CreatePayableAsync(_partnerId, _currencyId, 1000m, DateTime.UtcNow, null, null, null);
        var payment = new Payment
        {
            Id = Guid.NewGuid(), PartnerId = _partnerId, CurrencyId = _currencyId,
            Amount = 500m, PaymentDate = DateTime.UtcNow, PaymentNo = "PAY-2",
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _finance.AllocatePaymentAsync(payment.Id, new[] { new FinanceAllocationLine(payableId, 600m) }));
    }

    [Fact]
    public async Task Collection_allocation_closes_receivable()
    {
        var receivableId = await _finance.CreateReceivableAsync(_partnerId, _currencyId, 800m, DateTime.UtcNow, null, null, null);
        var collection = new Collection
        {
            Id = Guid.NewGuid(), PartnerId = _partnerId, CurrencyId = _currencyId,
            Amount = 800m, CollectionDate = DateTime.UtcNow, CollectionNo = "COL-1",
        };
        _db.Collections.Add(collection);
        await _db.SaveChangesAsync();

        await _finance.AllocateCollectionAsync(collection.Id, new[] { new FinanceAllocationLine(receivableId, 800m) });
        var receivable = await _db.Receivables.FindAsync(receivableId);
        Assert.True(receivable!.IsClosed);
    }

    [Fact]
    public async Task Timesheet_cost_posts_labour_expense()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(), CompanyId = _companyId, Code = "E1",
            FirstName = "A", LastName = "B", IsActive = true,
        };
        _db.Employees.Add(employee);
        var ts = new Timesheet { Id = Guid.NewGuid(), TimesheetNo = "TS-1", PeriodStart = DateTime.UtcNow, PeriodEnd = DateTime.UtcNow };
        _db.Timesheets.Add(ts);
        _db.TimesheetLines.Add(new TimesheetLine { Id = Guid.NewGuid(), TimesheetId = ts.Id, EmployeeId = employee.Id, WorkDate = DateTime.UtcNow, NormalHours = 8m, OvertimeHours = 0m, HourlyCost = 100m });
        _db.TimesheetLines.Add(new TimesheetLine { Id = Guid.NewGuid(), TimesheetId = ts.Id, EmployeeId = employee.Id, WorkDate = DateTime.UtcNow, NormalHours = 4m, OvertimeHours = 0m, HourlyCost = 120m });
        await _db.SaveChangesAsync();

        var txId = await _finance.PostTimesheetCostAsync(ts.Id, _currencyId);
        var transaction = await _db.FinancialTransactions.FindAsync(txId);
        Assert.Equal(1280m, transaction!.Amount);
        Assert.Equal(FinancialTransactionType.Expense, transaction.TransactionType);
    }

    [Fact]
    public async Task ProgressPayment_for_customer_creates_receivable()
    {
        var contract = new Contract
        {
            Id = Guid.NewGuid(), ContractType = ContractType.Customer, CurrencyId = _currencyId,
            ContractNo = "CT-1", Title = "Main", Status = DocumentStatus.Approved,
        };
        _db.Contracts.Add(contract);
        var pp = new ProgressPayment
        {
            Id = Guid.NewGuid(), ContractId = contract.Id, PartnerId = _partnerId,
            ProgressPaymentNo = "PP-1", PaymentPeriodStart = DateTime.UtcNow, PaymentPeriodEnd = DateTime.UtcNow,
            GrossAmount = 5000m, NetAmount = 5000m,
        };
        _db.ProgressPayments.Add(pp);
        await _db.SaveChangesAsync();

        var receivableId = await _finance.PostProgressPaymentAsync(pp.Id);
        var receivable = await _db.Receivables.FindAsync(receivableId);
        Assert.Equal(5000m, receivable!.Amount);
        Assert.Equal(_partnerId, receivable.PartnerId);
    }

    [Fact]
    public async Task Budget_overrun_raises_notification()
    {
        var costCenter = new CostCenter { Id = Guid.NewGuid(), Code = "CC1", Name = "CC", IsActive = true };
        _db.CostCenters.Add(costCenter);
        var budget = new Budget { Id = Guid.NewGuid(), CostCenterId = costCenter.Id, CurrencyId = _currencyId, Name = "B", Year = 2026, IsActive = true };
        _db.Budgets.Add(budget);
        _db.BudgetLines.Add(new BudgetLine { Id = Guid.NewGuid(), BudgetId = budget.Id, CostCenterId = costCenter.Id, Description = "L", PlannedAmount = 1000m });

        var transaction = new FinancialTransaction
        {
            Id = Guid.NewGuid(), TransactionType = FinancialTransactionType.Expense,
            CurrencyId = _currencyId, Amount = 1500m, TransactionDate = DateTime.UtcNow,
        };
        _db.FinancialTransactions.Add(transaction);
        _db.FinancialTransactionLines.Add(new FinancialTransactionLine { Id = Guid.NewGuid(), FinancialTransactionId = transaction.Id, CostCenterId = costCenter.Id, Amount = 1500m });
        await _db.SaveChangesAsync();

        var overrun = await _finance.CheckBudgetOverrunAsync(budget.Id);
        Assert.True(overrun);
        Assert.True(await _db.Notifications.AnyAsync(n => n.NotificationType == "BudgetOverrun"));
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}

