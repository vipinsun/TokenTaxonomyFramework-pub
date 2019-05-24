using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	public class TokenTemplateController : Controller
	{
		// GET
		public IActionResult GetTokenTemplate()
		{
			return View();
		}
	}
}