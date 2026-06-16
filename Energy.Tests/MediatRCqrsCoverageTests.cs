using System.Reflection;
using Energy.Application;
using MediatR;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// MediatR / CQRS kapsam ve standart uyum testi (ProjeTasarımKuralları).
/// Coverage raporunda MediatR/CQRS kapsamı ayrıca belirtilmek üzere bu test sınıfı
/// kullanılır. Doğruladıkları:
///  - Energy.Application assembly'sinde MediatR request (Command/Query) ve handler'lar mevcut.
///  - Hiçbir IRequest/IRequestHandler GENERIC değil (GenericCrudHandler&lt;T&gt; yasağı).
///  - Her IRequest tipinin karşılık gelen bir IRequestHandler'ı var (yetim use-case yok).
///  - Handler'lar Application katmanında kalır (Infrastructure/Api/Web'e bağımlı değildir).
///  - Referans dikey kesit (Procurement/PurchaseOrder) eksiksiz tanımlıdır.
/// </summary>
public sealed class MediatRCqrsCoverageTests
{
    private static readonly Assembly ApplicationAssembly = typeof(DependencyInjection).Assembly;

    private static IReadOnlyList<Type> ConcreteTypes() => ApplicationAssembly
        .GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } || t.IsValueType)
        .ToList();

    private static bool ImplementsOpenInterface(Type type, Type openInterface) => type
        .GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openInterface);

    private static IReadOnlyList<Type> RequestTypes() => ConcreteTypes()
        .Where(t => ImplementsOpenInterface(t, typeof(IRequest<>)))
        .ToList();

    private static IReadOnlyList<Type> HandlerTypes() => ConcreteTypes()
        .Where(t => ImplementsOpenInterface(t, typeof(IRequestHandler<,>)))
        .ToList();

    [Fact]
    public void Application_Assembly_Has_MediatR_Requests_And_Handlers()
    {
        Assert.NotEmpty(RequestTypes());
        Assert.NotEmpty(HandlerTypes());
    }

    [Fact]
    public void No_Generic_Command_Query_Or_Handler_Is_Defined()
    {
        // Generic Command/Query/Handler mimarisi yasak (CreateEntityCommand<T>,
        // GenericCrudHandler<T> vb.). Tüm request ve handler'lar somut (non-generic) olmalı.
        var genericRequests = ConcreteTypes()
            .Where(t => t.IsGenericTypeDefinition && ImplementsOpenInterface(t, typeof(IRequest<>)))
            .Select(t => t.Name)
            .ToList();

        var genericHandlers = ConcreteTypes()
            .Where(t => t.IsGenericTypeDefinition && ImplementsOpenInterface(t, typeof(IRequestHandler<,>)))
            // Pipeline behavior'ları (IPipelineBehavior) bu listeye girmez; yalnızca handler'lar.
            .Select(t => t.Name)
            .ToList();

        Assert.True(genericRequests.Count == 0,
            "Generic Command/Query tanımları yasak: " + string.Join(", ", genericRequests));
        Assert.True(genericHandlers.Count == 0,
            "Generic Handler tanımları yasak: " + string.Join(", ", genericHandlers));
    }

    [Fact]
    public void Every_Request_Has_A_Handler()
    {
        var handledRequestTypes = HandlerTypes()
            .SelectMany(h => h.GetInterfaces())
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .Select(i => i.GetGenericArguments()[0])
            .ToHashSet();

        var orphanRequests = RequestTypes()
            .Where(r => !handledRequestTypes.Contains(r))
            .Select(r => r.FullName)
            .ToList();

        Assert.True(orphanRequests.Count == 0,
            "Handler'ı olmayan use-case'ler: " + string.Join(", ", orphanRequests));
    }

    [Fact]
    public void Handlers_Do_Not_Depend_On_Infrastructure_Web_Or_Api()
    {
        // Handler'lar Application katmanında kalmalı; Infrastructure/Api/Web assembly'lerine
        // referans vermemeli (constructor parametre tipleri üzerinden kontrol edilir).
        string[] forbidden = ["Energy.Infrastructure", "Energy.Api", "Energy.Web"];

        var violations = new List<string>();
        foreach (var handler in HandlerTypes())
        {
            var ctorParamTypes = handler.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Select(p => p.ParameterType.Assembly.GetName().Name ?? string.Empty);

            if (ctorParamTypes.Any(asm => forbidden.Contains(asm)))
            {
                violations.Add(handler.FullName!);
            }
        }

        Assert.True(violations.Count == 0,
            "Handler'lar yasak katmanlara bağımlı: " + string.Join(", ", violations));
    }

    [Theory]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Commands.CreatePurchaseOrder.CreatePurchaseOrderCommand")]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Commands.UpdatePurchaseOrder.UpdatePurchaseOrderCommand")]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Commands.DeletePurchaseOrder.DeletePurchaseOrderCommand")]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderById.GetPurchaseOrderByIdQuery")]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderList.GetPurchaseOrderListQuery")]
    [InlineData("Energy.Application.Modules.Procurement.PurchaseOrder.Queries.GetPurchaseOrderLookup.GetPurchaseOrderLookupQuery")]
    public void Reference_PurchaseOrder_Vertical_Slice_Is_Defined(string requestTypeName)
    {
        var requestType = ApplicationAssembly.GetType(requestTypeName);
        Assert.True(requestType is not null, $"Beklenen use-case tipi bulunamadı: {requestTypeName}");
        Assert.True(ImplementsOpenInterface(requestType!, typeof(IRequest<>)),
            $"{requestTypeName} bir IRequest<> değil.");
    }
}

