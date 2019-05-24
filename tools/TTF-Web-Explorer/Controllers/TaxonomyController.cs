using Microsoft.AspNetCore.Mvc;

namespace TTI.TTF.WebExplorer.Controllers
{
	[Route("[controller]")]
	public class TaxonomyController : Controller
	{
		[HttpGet("~/")]
		[HttpGet("")]
		public IActionResult GetTaxonomy()
		{
			ViewData["Version"] = Host.Taxonomy.Version;
			ViewData["Bases"] = Host.Taxonomy.BaseTokenTypes;
			ViewData["Behaviors"] = Host.Taxonomy.Behaviors;
			ViewData["BehaviorGroups"] = Host.Taxonomy.BehaviorGroups;
			ViewData["PropertySets"] = Host.Taxonomy.PropertySets;
			ViewData["TokenTemplates"] = Host.Taxonomy.TokenTemplates;
			return View(Host.Taxonomy);
		}
	}
}