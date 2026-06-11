using FluentValidation;

namespace Energy.Application.Home.Queries.GetHomeDashboard;

public sealed class GetHomeDashboardQueryValidator : AbstractValidator<GetHomeDashboardQuery>
{
    public GetHomeDashboardQueryValidator()
    {
        RuleFor(query => query.QuickLinkCount).InclusiveBetween(0, 12);

        When(query => query.IncludeQuickLinks, () =>
        {
            RuleFor(query => query.QuickLinkCount)
                .GreaterThan(0)
                .WithMessage("QuickLinkCount must be greater than 0 when quick links are enabled.");
        });
    }
}
