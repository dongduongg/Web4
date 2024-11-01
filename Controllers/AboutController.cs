using Microsoft.AspNetCore.Mvc;
//using QLBN.Models.Authentication;

namespace QLBN.Controllers
{
    
    public class AboutController : Controller
	{
		public IActionResult Index()
		{
            ViewData["ActivePage"] = "About";
			return View();
		}
	}
}
