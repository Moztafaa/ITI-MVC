
using Microsoft.AspNetCore.Mvc;
namespace NTier.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    // GET: HomeController
    public ActionResult Index()
    {
        return View();
    }

}
