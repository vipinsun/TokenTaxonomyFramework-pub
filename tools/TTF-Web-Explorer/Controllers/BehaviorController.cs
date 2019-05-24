using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	public class BehaviorController : Controller
	{
		// GET
		public IActionResult GetBehavior(string symbol)
		{
			return View();
		}
	}
}