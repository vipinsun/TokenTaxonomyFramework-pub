using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using log4net;
using Newtonsoft.Json.Linq;
using TaxonomyHost.factories;
using TTF.Tokens.Model.Artifact;
using TTF.Tokens.Model.Core;
using TTI.TTF.Taxonomy.Model;
using TTT.TTF.Taxonomy;

namespace TaxonomyHost
{
	public static class ModelManager
	{
		private static readonly ILog _log;
		internal static Taxonomy Taxonomy { get; set; }

		static ModelManager()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void Init()
		{
			_log.Info("ModelManager Init");
			Taxonomy = TaxonomyFactory.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version, Taxonomy, DateTime.Now.AddDays(1));
		}

		internal static Taxonomy GetFullTaxonomy(TaxonomyVersion version)
		{
			_log.Info("GetFullTaxonomy version " + version.Version);
			return Taxonomy.Version == version.Version ? Taxonomy : TaxonomyCache.GetFromCache(version.Version);
		}

		public static Base GetBaseArtifact(Symbol symbol)
		{
			_log.Info("GetBaseArtifact Symbol " + symbol.ArtifactSymbol);
			return Taxonomy.BaseTokenTypes.Single(e => e.Key == symbol.ArtifactSymbol).Value;
		}

		public static Behavior GetBehaviorArtifact(Symbol symbol)
		{
			_log.Info("GetBehaviorArtifact Symbol " + symbol.ArtifactSymbol);
			return Taxonomy.Behaviors.Single(e => e.Key == symbol.ArtifactSymbol).Value;
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(Symbol symbol)
		{
			_log.Info("GetBehaviorGroupArtifact Symbol " + symbol.ArtifactSymbol);
			return Taxonomy.BehaviorGroups.Single(e => e.Key == symbol.ArtifactSymbol).Value;
		}

		public static PropertySet GetPropertySetArtifact(Symbol symbol)
		{
			_log.Info("GetPropertySetArtifact Symbol " + symbol.ArtifactSymbol);
			return Taxonomy.PropertySets.Single(e => e.Key == symbol.ArtifactSymbol).Value;		
		}

		public static TokenTemplate GetTokenTemplateArtifact(TaxonomyFormula formula)
		{
			_log.Info("GetTokenTemplateArtifact Formula: " + formula.Formula);
			return Taxonomy.TokenTemplates.Single(e => e.Key == formula.Formula).Value;		
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