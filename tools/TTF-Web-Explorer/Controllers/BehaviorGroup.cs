using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	public class BehaviorGroup : Controller
	{
		// GET
		public IActionResult GetBehaviorGroup()
		{
			return View();
		}
	}
}