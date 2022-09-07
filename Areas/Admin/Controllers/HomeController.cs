using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}