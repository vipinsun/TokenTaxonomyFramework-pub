using System;
using System.Reflection;
using log4net;
using TaxonomyHost.factories;
using TTF.Tokens.Model.Artifact;
using TTF.Tokens.Model.Core;
using TTI.TTF.Taxonomy.Model;
using TTT.TTF.Taxonomy;

namespace TaxonomyHost
{
	public static class ModelManager
	{
		private static readonly ILog Log;
		private static Taxonomy Taxonomy { get; set; }

		static ModelManager()
		{
			Utils.InitLog();
			Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void Init()
		{
			Log.Info("ModelManager Init");
			Taxonomy = TaxonomyFactory.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version, Taxonomy, DateTime.Now.AddDays(1));
		}

		internal static Taxonomy GetFullTaxonomy(TaxonomyVersion version)
		{
			Log.Info("GetFullTaxonomy version " + version.Version);
			return Taxonomy.Version == version.Version ? Taxonomy : TaxonomyCache.GetFromCache(version.Version);
		}

		public static Base GetBaseArtifact(Symbol symbol)
		{
			throw new NotImplementedException();
		}

		public static Behavior GetBehaviorArtifact(Symbol symbol)
		{
			throw new NotImplementedException();
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(Symbol symbol)
		{
			throw new NotImplementedException();
		}

		public static PropertySet GetPropertySetArtifact(Symbol symbol)
		{
			throw new NotImplementedException();
		}

		public static TokenTemplate GetTokenTemplateArtifact(TaxonomyFormula formula)
		{
			throw new NotImplementedException();
		}

		public static NewArtifactResponse CreateArtifact(NewArtifactRequest artifactRequest)
		{
			throw new NotImplementedException();
		}

		public static UpdateArtifactResponse UpdateArtifact(UpdateArtifactRequest artifactRequest)
		{
			throw new NotImplementedException();
		}

		public static DeleteArtifactResponse DeleteArtifact(DeleteArtifactRequest artifactRequest)
		{
			throw new NotImplementedException();
		}
	}
}