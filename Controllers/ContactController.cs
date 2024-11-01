using Microsoft.AspNetCore.Mvc;
//using QLBN.Models.Authentication;

namespace QLBN.Controllers
{
    
    public class ContactController : Controller
	{
		public IActionResult Index()
		{
            ViewData["ActivePage"] = "Contact";
            return View();
		}
	}
}
