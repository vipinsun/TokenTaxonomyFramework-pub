using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	public class PropertySetController : Controller
	{
		// GET
		public IActionResult GetPropertySet()
		{
			return View();
		}
	}
}