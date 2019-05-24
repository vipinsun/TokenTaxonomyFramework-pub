using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	[Route("controller")]
	public class BaseTokenController : Controller
	{
		[HttpGet("/base/{symbol}")]
		public IActionResult GetBaseToken(string symbol)
		{
			 var baseToken = Host.Taxonomy.BaseTokenTypes.FirstOrDefault(e=>e.Key == symbol).Value;
			 ViewData["Base"] = baseToken;
			return View(baseToken);
		}
	}
}