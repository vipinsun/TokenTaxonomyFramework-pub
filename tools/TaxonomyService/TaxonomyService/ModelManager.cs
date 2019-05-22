using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using log4net;
using TTI.TTF.Taxonomy.Controllers;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy
{
	public static class ModelManager
	{
		private static readonly ILog _log;
		private static Model.Taxonomy Taxonomy { get; set; }
		static ModelManager()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void Init()
		{
			_log.Info("ModelManager Init");
			Taxonomy = TaxonomyController.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version, Taxonomy, DateTime.Now.AddDays(1));
		}

		internal static Model.Taxonomy GetFullTaxonomy(TaxonomyVersion version)
		{
			_log.Info("GetFullTaxonomy version " + version.Version);
			return Taxonomy.Version == version.Version ? Taxonomy : TaxonomyCache.GetFromCache(version.Version);
		}
		
		internal static Model.Taxonomy RefreshTaxonomy(TaxonomyVersion version)
		{
			_log.Info("RefreshTaxonomy version " + version.Version);
			Taxonomy = TaxonomyController.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version, Taxonomy, DateTime.Now.AddDays(1));
			return Taxonomy;
		}

		public static Base GetBaseArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBaseArtifact Symbol " + symbol.ToolingSymbol);
			return Taxonomy.BaseTokenTypes.Single(e => e.Key == symbol.ToolingSymbol).Value;
		}

		public static Behavior GetBehaviorArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorArtifact Symbol " + symbol.ToolingSymbol);
			return Taxonomy.Behaviors.Single(e => e.Key == symbol.ToolingSymbol).Value;
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorGroupArtifact Symbol " + symbol.ToolingSymbol);
			return Taxonomy.BehaviorGroups.Single(e => e.Key == symbol.ToolingSymbol).Value;
		}

		public static PropertySet GetPropertySetArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetPropertySetArtifact Symbol " + symbol.ToolingSymbol);
			return Taxonomy.PropertySets.Single(e => e.Key == symbol.ToolingSymbol).Value;		
		}

		public static TokenTemplate GetTokenTemplateArtifact(TaxonomyFormula formula)
		{
			_log.Info("GetTokenTemplateArtifact Formula: " + formula.Formula);
			return Taxonomy.TokenTemplates.Single(e => e.Key == formula.Formula).Value;		
		}

		public static NewArtifactResponse CreateArtifact(NewArtifactRequest artifactRequest)
		{
			_log.Info("CreateArtifact: " + artifactRequest.Type);
			return TaxonomyController.CreateArtifact(artifactRequest);
		}

		public static UpdateArtifactResponse UpdateArtifact(UpdateArtifactRequest artifactRequest)
		{
			_log.Info("UpdateArtifact: " + artifactRequest.Type);
			return TaxonomyController.UpdateArtifact(artifactRequest);
		}

		public static DeleteArtifactResponse DeleteArtifact(DeleteArtifactRequest artifactRequest)
		{
			_log.Info("DeleteArtifact: " + artifactRequest.ArtifactSymbol.ToolingSymbol);
			return TaxonomyController.DeleteArtifact(artifactRequest);
		}

		public static bool AddOrUpdateInMemoryArtifact(ArtifactType type, Any artifact)
		{
			switch (type)
			{
				case ArtifactType.Base:
					var baseType = artifact.Unpack<Base>();
					try
					{
						Taxonomy.BaseTokenTypes.Remove(baseType.Artifact.ArtifactSymbol.ToolingSymbol);
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.ToolingSymbol, baseType);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + baseType.Artifact.ArtifactSymbol.ToolingSymbol);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.ToolingSymbol, baseType);
					}
					return true;
				case ArtifactType.Behavior:
					var behavior = artifact.Unpack<Behavior>();
					try
					{
						Taxonomy.Behaviors.Remove(behavior.Artifact.ArtifactSymbol.ToolingSymbol);
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.ToolingSymbol, behavior);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behavior.Artifact.ArtifactSymbol.ToolingSymbol);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.ToolingSymbol, behavior);
					}
					return true;
				case ArtifactType.BehaviorGroup:
					var behaviorGroup = artifact.Unpack<BehaviorGroup>();
					try
					{
						Taxonomy.BehaviorGroups.Remove(behaviorGroup.Artifact.ArtifactSymbol.ToolingSymbol);
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.ToolingSymbol, behaviorGroup);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behaviorGroup.Artifact.ArtifactSymbol.ToolingSymbol);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.ToolingSymbol, behaviorGroup);
					}
					return true;
				case ArtifactType.PropertySet:
					var propertySet = artifact.Unpack<PropertySet>();
					try
					{
						Taxonomy.PropertySets.Remove(propertySet.Artifact.ArtifactSymbol.ToolingSymbol);
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.ToolingSymbol, propertySet);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + propertySet.Artifact.ArtifactSymbol.ToolingSymbol);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.ToolingSymbol, propertySet);
					}
					return true;
				case ArtifactType.TokenTemplate:
					var tokenTemplate = artifact.Unpack<TokenTemplate>();
					try
					{
						Taxonomy.TokenTemplates.Remove(tokenTemplate.Artifact.ArtifactSymbol.ToolingSymbol);
						Taxonomy.TokenTemplates.Add(tokenTemplate.Artifact.ArtifactSymbol.ToolingSymbol, tokenTemplate);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + tokenTemplate.Artifact.ArtifactSymbol.ToolingSymbol);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.TokenTemplates.Add(tokenTemplate.Artifact.ArtifactSymbol.ToolingSymbol, tokenTemplate);
					}
					return true;
				default:
					return false;
			}
		}
		
		internal static string GetArtifactFolderNameBySymbol(ArtifactType artifactType, string toolingSymbol)
		{
			_log.Info("GetArtifactFolderNameBySymbol: " + artifactType +": " + toolingSymbol);
			try
			{
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseFolder = Taxonomy.BaseTokenTypes.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == toolingSymbol);
						return baseFolder.Value.Artifact.Name;
					case ArtifactType.Behavior:
						var behaviorFolder = Taxonomy.Behaviors.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == toolingSymbol);
						return behaviorFolder.Value.Artifact.Name;
					case ArtifactType.BehaviorGroup:
						var behaviorGroupFolder = Taxonomy.BehaviorGroups.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == toolingSymbol);
						return behaviorGroupFolder.Value.Artifact.Name;
					case ArtifactType.PropertySet:
						var propertySetFolder = Taxonomy.PropertySets.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == toolingSymbol);
						return propertySetFolder.Value.Artifact.Name;
					case ArtifactType.TokenTemplate:
						var tokenTemplateFolder = Taxonomy.TokenTemplates.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == toolingSymbol);
						return tokenTemplateFolder.Value.Artifact.Name;
					default:
						throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
				}
			}
			catch (Exception)
			{
				_log.Info("No matching artifact folder of type: " + artifactType + " with symbol: " + toolingSymbol);
				return "";
			}
		}
		
		internal static bool CheckForUniqueArtifact(ArtifactType artifactType, Artifact artifact)
		{
			var name = artifact.Name;
			_log.Info("CheckForUniqueArtifact: " + artifactType +": " + name);
			try
			{
				if(string.IsNullOrEmpty(GetArtifactFolderNameBySymbol(artifactType, artifact.ArtifactSymbol.ToolingSymbol)))
					throw new Exception("Tooling Symbol Found.");
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseFolder = Taxonomy.BaseTokenTypes.Single(e =>
							e.Value.Artifact.Name == name);
						break;
					case ArtifactType.Behavior:
						var behaviorFolder = Taxonomy.Behaviors.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == name);
						break;
					case ArtifactType.BehaviorGroup:
						var behaviorGroupFolder = Taxonomy.BehaviorGroups.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == name);
						break;
					case ArtifactType.PropertySet:
						var propertySetFolder = Taxonomy.PropertySets.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == name);
						break;
					case ArtifactType.TokenTemplate:
						var tokenTemplateFolder = Taxonomy.TokenTemplates.Single(e =>
							e.Value.Artifact.ArtifactSymbol.ToolingSymbol == name);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		
		internal static Artifact MakeUniqueArtifact(Artifact artifact)
		{
			var newArtifact = artifact.Clone();
			var newIdentifiers =
				Utils.GetRandomArtifactFromArtifact(artifact.Name, artifact.ArtifactSymbol.VisualSymbol, artifact.ArtifactSymbol.ToolingSymbol);
			newArtifact.Name = newIdentifiers.Name;
			newArtifact.ArtifactSymbol.VisualSymbol = newIdentifiers.VisualSymbol;
			newArtifact.ArtifactSymbol.ToolingSymbol = newIdentifiers.ToolingSymbol;
			return newArtifact;
		}
	}
}