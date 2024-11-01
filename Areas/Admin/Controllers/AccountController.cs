using Microsoft.AspNetCore.Mvc;

namespace QLBN.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
