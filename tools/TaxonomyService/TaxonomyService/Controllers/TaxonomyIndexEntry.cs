using System;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.Controllers
{
	
	public class TaxonomyIndexEntry<T> where T : class
	{
		public ArtifactType Type { get; }
		private readonly T indexRef;
		public TaxonomyIndexEntry(ArtifactType type, ref T indexRef)
		{
			Type = type;
			this.indexRef = indexRef;
		}

		public T GetReference() 
		{
			return indexRef as T;
		}

	}
}