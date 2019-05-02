using System.Collections.Generic;
using TTF.Tokens.Model.Core;

namespace TaxonomyHost.factories
{
	public class BaseFactory
	{
		internal IEnumerable<Base> Load()
		{
			var bases = new List<Base>();

			return bases;
		}
	}
}