using QLBN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using QLBN.Models.Authentication;

namespace QLBN.Controllers
{
	
	public class HomeController : Controller
	{
        
        public IActionResult Index()
		{
			//var lst
            ViewData["ActivePage"] = "Home";
            return View();
		}

		
	}
}