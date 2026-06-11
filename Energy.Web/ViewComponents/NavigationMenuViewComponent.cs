using Energy.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.ViewComponents;

public sealed class NavigationMenuViewComponent : ViewComponent
{
    private readonly INavigationService _navigationService;

    public NavigationMenuViewComponent(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = await _navigationService.GetMenuForUserAsync(
            HttpContext.User,
            HttpContext.RequestAborted);

        return View(items);
    }
}

