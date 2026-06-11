using Energy.Web.Services.Navigation;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.ViewComponents;

public sealed class NavigationMenuViewComponent : ViewComponent
{
    private readonly INavigationService _navigation;
    public NavigationMenuViewComponent(INavigationService navigation) { _navigation = navigation; }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var items = await _navigation.GetMyNavigationAsync(HttpContext.RequestAborted);
        return View(items);
    }
}
