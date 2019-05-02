using System.ComponentModel.DataAnnotations;
using TTI.TTF.Taxonomy.Model;

namespace TaxonomyHost.factories
{
	public static class TaxonomyFactory
	{
		internal static Taxonomy Load()
		{
			var tax = new Taxonomy();

			return tax;
		}
		
	}
}