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

		public static QueryResult GetArtifactsOfType(QueryOptions options)
		{
			var result = new QueryResult
			{
				ArtifactType = options.ArtifactType,
				FirstItemIndex = options.LastItemIndex - 1
			};
			try
			{
				switch (options.ArtifactType)
				{
					case ArtifactType.Base:
						var bases = new Bases();
						bases.Base.AddRange(Taxonomy.BaseTokenTypes
							.Values); //unlikely to be more than a handful of these.	
						result.ArtifactCollection = Any.Pack(bases);
						result.FirstItemIndex = 0;
						result.LastItemIndex = bases.Base.Count - 1;
						result.TotalItemsInCollection = Taxonomy.BaseTokenTypes.Count;
						break;
					case ArtifactType.Behavior:
						var behaviors = new Behaviors();
						if (Taxonomy.Behaviors.Count <= options.MaxItemReturn
						) //if max return is greater than the total count, just send back all of them.
						{
							behaviors.Behavior.AddRange(Taxonomy.Behaviors.Values);
							result.FirstItemIndex = 0;
							result.LastItemIndex = behaviors.Behavior.Count - 1;
						}
						else
						{
							behaviors.Behavior.AddRange(behaviors.Behavior.ToList()
								.GetRange(options.LastItemIndex, options.MaxItemReturn));
							result.ArtifactCollection = Any.Pack(behaviors);
							result.LastItemIndex = options.LastItemIndex + behaviors.Behavior.Count - 1;
						}

						result.TotalItemsInCollection = behaviors.Behavior.Count;
						result.ArtifactCollection = Any.Pack(behaviors);
						break;
					case ArtifactType.BehaviorGroup:
						var behaviorGroups = new BehaviorGroups();
						if (Taxonomy.BehaviorGroups.Count <= options.MaxItemReturn
						) //if max return is greater than the total count, just send back all of them.
						{
							behaviorGroups.BehaviorGroup.AddRange(Taxonomy.BehaviorGroups.Values);
							result.FirstItemIndex = 0;
							result.LastItemIndex = behaviorGroups.BehaviorGroup.Count - 1;
						}
						else
						{
							behaviorGroups.BehaviorGroup.AddRange(behaviorGroups.BehaviorGroup.ToList()
								.GetRange(options.LastItemIndex, options.MaxItemReturn));
							result.ArtifactCollection = Any.Pack(behaviorGroups);
							result.LastItemIndex =
								options.LastItemIndex + behaviorGroups.BehaviorGroup.Count - 1;
						}

						result.TotalItemsInCollection = behaviorGroups.BehaviorGroup.Count;
						result.ArtifactCollection = Any.Pack(behaviorGroups);
						break;
					case ArtifactType.PropertySet:
						var propertySets = new PropertySets();
						if (Taxonomy.PropertySets.Count <= options.MaxItemReturn
						) //if max return is greater than the total count, just send back all of them.
						{
							propertySets.PropertySet.AddRange(Taxonomy.PropertySets.Values);
							result.FirstItemIndex = 0;
							result.LastItemIndex = propertySets.PropertySet.Count - 1;
						}
						else
						{
							propertySets.PropertySet.AddRange(propertySets.PropertySet.ToList()
								.GetRange(options.LastItemIndex, options.MaxItemReturn));
							result.ArtifactCollection = Any.Pack(propertySets);
							result.LastItemIndex = options.LastItemIndex + propertySets.PropertySet.Count - 1;
						}

						result.TotalItemsInCollection = propertySets.PropertySet.Count;
						result.ArtifactCollection = Any.Pack(propertySets);
						break;
					case ArtifactType.TokenTemplate:
						var tokenTemplates = new TokenTemplates();
						if (Taxonomy.TokenTemplates.Count <= options.MaxItemReturn
						) //if max return is greater than the total count, just send back all of them.
						{
							tokenTemplates.TokenTemplate.AddRange(Taxonomy.TokenTemplates.Values);
							result.FirstItemIndex = 0;
							result.LastItemIndex = tokenTemplates.TokenTemplate.Count - 1;
						}
						else
						{
							tokenTemplates.TokenTemplate.AddRange(tokenTemplates.TokenTemplate.ToList()
								.GetRange(options.LastItemIndex, options.MaxItemReturn));
							result.ArtifactCollection = Any.Pack(tokenTemplates);
							result.LastItemIndex =
								options.LastItemIndex + tokenTemplates.TokenTemplate.Count - 1;
						}

						result.TotalItemsInCollection = tokenTemplates.TokenTemplate.Count;
						result.ArtifactCollection = Any.Pack(tokenTemplates);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				return result;
			}
			catch (Exception ex)
			{
				_log.Error("Error retrieving artifact collection of type: " + options.ArtifactType);
				_log.Error(ex);
				return result;
			}
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
				if(!string.IsNullOrEmpty(GetArtifactFolderNameBySymbol(artifactType, artifact.ArtifactSymbol.ToolingSymbol)))
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
				return true;
			}
			return false;
		}
		
		internal static Artifact MakeUniqueArtifact(Artifact artifact)
		{
			var newArtifact = artifact.Clone();
			var (name, visualSymbol, toolingSymbol) = Utils.GetRandomArtifactFromArtifact(artifact.Name, artifact.ArtifactSymbol.VisualSymbol, artifact.ArtifactSymbol.ToolingSymbol);
			newArtifact.Name = name;
			newArtifact.ArtifactSymbol.VisualSymbol = visualSymbol;
			newArtifact.ArtifactSymbol.ToolingSymbol = toolingSymbol;
			return newArtifact;
		}
	}
}