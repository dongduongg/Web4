using Microsoft.AspNetCore.Mvc;
//using QLBN.Models.Authentication;

namespace QLBN.Controllers
{
   
    public class ServiceController : Controller
	{
		public IActionResult Index()
		{
            ViewData["ActivePage"] = "Service";
            return View();
		}
	}
}
