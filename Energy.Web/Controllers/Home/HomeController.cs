using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Energy.Web.Models;

namespace Energy.Web.Controllers.Home;

public sealed class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Dashboard");

    [AllowAnonymous]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
}
