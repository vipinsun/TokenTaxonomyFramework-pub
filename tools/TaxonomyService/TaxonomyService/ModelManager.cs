using System;
using System.Linq;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using log4net;
using TTI.TTF.Taxonomy.Controllers;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using Enum = System.Enum;

namespace TTI.TTF.Taxonomy
{
	public static class ModelManager
	{
		private static readonly ILog _log;
		private static Model.Taxonomy Taxonomy { get; set; }
		private const string NameIndex = "Name";
		private const string FormulaIndex = "Formula";
		private const string IdIndex = "Id";
		
		static ModelManager()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void Init()
		{
			_log.Info("ModelManager Init");
			Taxonomy = TaxonomyController.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version.Id, Taxonomy, DateTime.Now.AddDays(1));
			BuildTemplateIndexes();
		}

		internal static Model.Taxonomy GetFullTaxonomy(TaxonomyVersion version)
		{
			_log.Info("GetFullTaxonomy version " + version.Version);
			return Taxonomy.Version.Version == version.Version ? Taxonomy : TaxonomyCache.GetFromCache(version.Version);
		}
		
		internal static Model.Taxonomy RefreshTaxonomy(TaxonomyVersion version)
		{
			_log.Info("RefreshTaxonomy version " + version.Version);
			Taxonomy = TaxonomyController.Load();
			TaxonomyCache.SaveToCache(Taxonomy.Version.Version, Taxonomy, DateTime.Now.AddDays(1));
			BuildTemplateIndexes();
			return Taxonomy;
		}

		public static Base GetBaseArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBaseArtifact Symbol " + symbol.Tooling);
			return Taxonomy.BaseTokenTypes.Single(e => e.Key == symbol.Tooling).Value;
		}

		public static Behavior GetBehaviorArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorArtifact Symbol " + symbol.Tooling);
			return Taxonomy.Behaviors.Single(e => e.Key == symbol.Tooling).Value;
		}

		public static BehaviorGroup GetBehaviorGroupArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetBehaviorGroupArtifact Symbol " + symbol.Tooling);
			return Taxonomy.BehaviorGroups.Single(e => e.Key == symbol.Tooling).Value;
		}

		public static PropertySet GetPropertySetArtifact(ArtifactSymbol symbol)
		{
			_log.Info("GetPropertySetArtifact Symbol " + symbol.Tooling);
			return Taxonomy.PropertySets.Single(e => e.Key == symbol.Tooling).Value;		
		}

		public static TokenTemplate GetTokenTemplateArtifact(TaxonomyFormula formula)
		{
			_log.Info("GetTokenTemplateArtifact Formula: " + formula.Formula);
			var formulaIndex = TemplateIndexes.GetFromCache(FormulaIndex);
			return formulaIndex.Templates.SingleOrDefault(e => e.Key == formula.Formula).Value;	
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
						var idIndex = TemplateIndexes.GetFromCache(IdIndex);
						if (idIndex.Templates.Count <= options.MaxItemReturn) //if max return is greater than the total count, just send back all of them.
						{
							tokenTemplates.Templates.AddRange(idIndex.Templates.Values);
							result.FirstItemIndex = 0;
							result.LastItemIndex = tokenTemplates.Templates.Count - 1;
						}
						else
						{
							tokenTemplates.Templates.AddRange(idIndex.Templates.Values.ToList()
								.GetRange(options.LastItemIndex, options.MaxItemReturn));
							result.ArtifactCollection = Any.Pack(tokenTemplates);
							result.LastItemIndex =
								options.LastItemIndex + tokenTemplates.Templates.Count - 1;
						}

						result.TotalItemsInCollection = tokenTemplates.Templates.Count;
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
			_log.Info("DeleteArtifact: " + artifactRequest.ArtifactSymbol.Tooling);
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
						Taxonomy.BaseTokenTypes.Remove(baseType.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.Tooling, baseType);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + baseType.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.Tooling, baseType);
					}
					return true;
				case ArtifactType.Behavior:
					var behavior = artifact.Unpack<Behavior>();
					try
					{
						Taxonomy.Behaviors.Remove(behavior.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.Tooling, behavior);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behavior.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.Tooling, behavior);
					}
					return true;
				case ArtifactType.BehaviorGroup:
					var behaviorGroup = artifact.Unpack<BehaviorGroup>();
					try
					{
						Taxonomy.BehaviorGroups.Remove(behaviorGroup.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.Tooling, behaviorGroup);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + behaviorGroup.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.Tooling, behaviorGroup);
					}
					return true;
				case ArtifactType.PropertySet:
					var propertySet = artifact.Unpack<PropertySet>();
					try
					{
						Taxonomy.PropertySets.Remove(propertySet.Artifact.ArtifactSymbol.Tooling);
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.Tooling, propertySet);
					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + propertySet.Artifact.ArtifactSymbol.Tooling);
						_log.Info("Adding artifact to Taxonomy.");
						Taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.Tooling, propertySet);
					}
					return true;
				case ArtifactType.TokenTemplate:
					var tokenTemplate = artifact.Unpack<TokenTemplate>();
					var branches = Enum.GetValues(typeof(ClassificationBranch));
					try
					{
						if (tokenTemplate.Parent.Base.TokenFormulaCase != Base.TokenFormulaOneofCase.SingleToken)
						{
							foreach (var b in branches)
							{
								var currentBranch = (ClassificationBranch) b;

								switch (currentBranch)
								{
									case ClassificationBranch.FractionalFungible:
										foreach (var t in Taxonomy.TokenTemplates.Hybrids.FractionalFungibles
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.Hybrids.FractionalFungibles
												.TokenTemplates.Remove(t);
											Taxonomy.TokenTemplates.Hybrids.FractionalFungibles
												.TokenTemplates.Add(tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.WholeFungible:
										foreach (var t in Taxonomy.TokenTemplates.Hybrids.WholeFungibles
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.Hybrids.WholeFungibles.TokenTemplates
												.Remove(t);
											Taxonomy.TokenTemplates.Hybrids.WholeFungibles.TokenTemplates
												.Add(tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.FractionalNonFungible:
										foreach (var t in Taxonomy.TokenTemplates.Hybrids
											.FractionalNonFungibles.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.Hybrids.FractionalNonFungibles
												.TokenTemplates.Remove(t);
											Taxonomy.TokenTemplates.Hybrids.FractionalNonFungibles
												.TokenTemplates.Add(tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.Singleton:
										foreach (var t in Taxonomy.TokenTemplates.Hybrids.Singletons
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.Hybrids.Singletons.TokenTemplates.Remove(
												t);
											Taxonomy.TokenTemplates.Hybrids.Singletons.TokenTemplates.Add(
												tokenTemplate);
											return true;
										}

										break;
									default:
										throw new ArgumentOutOfRangeException();
								}

							}

						}
						else
						{
							foreach (var b in branches)
							{
								var currentBranch = (ClassificationBranch) b;

								switch (currentBranch)
								{
									case ClassificationBranch.FractionalFungible:
										foreach (var t in Taxonomy.TokenTemplates.FractionalFungibles
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.FractionalFungibles.TokenTemplates
												.Remove(t);
											Taxonomy.TokenTemplates.FractionalFungibles.TokenTemplates.Add(
												tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.WholeFungible:
										foreach (var t in Taxonomy.TokenTemplates.WholeFungibles
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.WholeFungibles.TokenTemplates.Remove(t);
											Taxonomy.TokenTemplates.WholeFungibles.TokenTemplates.Add(
												tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.FractionalNonFungible:
										foreach (var t in Taxonomy.TokenTemplates.FractionalNonFungibles
											.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.FractionalNonFungibles.TokenTemplates
												.Remove(t);
											Taxonomy.TokenTemplates.FractionalNonFungibles.TokenTemplates
												.Add(tokenTemplate);
											return true;
										}

										break;
									case ClassificationBranch.Singleton:
										foreach (var t in Taxonomy.TokenTemplates.Singletons.TokenTemplates)
										{
											if (t.Parent.Formula.Id != tokenTemplate.Parent.Formula.Id)
												continue;
											Taxonomy.TokenTemplates.Singletons.TokenTemplates.Remove(t);
											Taxonomy.TokenTemplates.Singletons.TokenTemplates.Add(
												tokenTemplate);
											return true;
										}

										break;
									default:
										throw new ArgumentOutOfRangeException();
								}

							}
						}

					}
					catch (Exception)
					{
						_log.Info("AddOrUpdateInMemoryArtifact did not find an existing: " + type + " with a Tooling Symbol of: " + tokenTemplate.Parent.Formula);
						_log.Info("Adding artifact to Taxonomy.");
						
						AddTemplate(tokenTemplate);
					}
					return true;
				default:
					return false;
			}
		}

		private static bool AddTemplate(TokenTemplate template)
		{
			ClassificationBranch targetBranch;
			bool hybrid = false;
			try
			{

				switch (template.Parent.Base.TokenFormulaCase)
				{
					case Base.TokenFormulaOneofCase.None:
						throw new Exception("Token classification could not be determined.");
					case Base.TokenFormulaOneofCase.SingleToken:
						targetBranch = template.Parent.Base.SingleToken.ClassificationBranch;
						break;
					case Base.TokenFormulaOneofCase.Hybrid:
						targetBranch = template.Parent.Base.Hybrid.Parent.ClassificationBranch;
						hybrid = true;
						break;
					case Base.TokenFormulaOneofCase.HybridWithHybrids:
						targetBranch = template.Parent.Base.HybridWithHybrids.Parent.ClassificationBranch;
						hybrid = true;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
					

					switch (targetBranch)
					{
						case ClassificationBranch.FractionalFungible:
							if(hybrid)
								Taxonomy.TokenTemplates.Hybrids.FractionalFungibles.TokenTemplates.Add(template);
								//todo:add save to git
							else
							{
								Taxonomy.TokenTemplates.FractionalFungibles.TokenTemplates.Add(template);
							}
							break;
						case ClassificationBranch.WholeFungible:
							if(hybrid)
								Taxonomy.TokenTemplates.Hybrids.WholeFungibles.TokenTemplates.Add(template);
							//todo:add save to git
							else
							{
								Taxonomy.TokenTemplates.WholeFungibles.TokenTemplates.Add(template);
							}
							break;
						case ClassificationBranch.FractionalNonFungible:
							if(hybrid)
								Taxonomy.TokenTemplates.Hybrids.FractionalNonFungibles.TokenTemplates.Add(template);
							//todo:add save to git
							else
							{
								Taxonomy.TokenTemplates.FractionalNonFungibles.TokenTemplates.Add(template);
							}
							break;
						case ClassificationBranch.Singleton:
							if(hybrid)
								Taxonomy.TokenTemplates.Hybrids.Singletons.TokenTemplates.Add(template);
							//todo:add save to git
							else
							{
								Taxonomy.TokenTemplates.Singletons.TokenTemplates.Add(template);
							}
							break;
						default:
							throw new ArgumentOutOfRangeException();
					
				}
				return true;
			}
			catch (Exception e)
			{
				_log.Error("Failed to add template to taxonomy: " + e);
				return false;
			}

		}

		internal static string GetArtifactFolderNameBySymbol(ArtifactType artifactType, string tooling)
		{
			_log.Info("GetArtifactFolderNameBySymbol: " + artifactType +": " + tooling);
			try
			{
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseFolder = Taxonomy.BaseTokenTypes.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return baseFolder.Value.Artifact.Name;
					case ArtifactType.Behavior:
						var behaviorFolder = Taxonomy.Behaviors.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return behaviorFolder.Value.Artifact.Name;
					case ArtifactType.BehaviorGroup:
						var behaviorGroupFolder = Taxonomy.BehaviorGroups.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return behaviorGroupFolder.Value.Artifact.Name;
					case ArtifactType.PropertySet:
						var propertySetFolder = Taxonomy.PropertySets.Single(e =>
							e.Value.Artifact.ArtifactSymbol.Tooling == tooling);
						return propertySetFolder.Value.Artifact.Name;
					case ArtifactType.TokenTemplate:
						var nameIndex = TemplateIndexes.GetFromCache(NameIndex);
						var tokenTemplateFolder = nameIndex.Templates.Single(e =>
							e.Value.Parent.Formula.Tooling == tooling);
						return tokenTemplateFolder.Value.Parent.Name;
					default:
						throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
				}
			}
			catch (Exception)
			{
				_log.Info("No matching artifact folder of type: " + artifactType + " with symbol: " + tooling);
				return "";
			}
		}
		
		internal static bool CheckForUniqueArtifact(ArtifactType artifactType, Artifact artifact)
		{
			var name = artifact.Name;
			_log.Info("CheckForUniqueArtifact: " + artifactType +": " + name);
			try
			{
				if(!string.IsNullOrEmpty(GetArtifactFolderNameBySymbol(artifactType, artifact.ArtifactSymbol.Tooling)))
					throw new Exception("Tooling Symbol Found.");
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseFolder = Taxonomy.BaseTokenTypes.Single(e =>
							e.Value.Artifact.Name == name);
						break;
					case ArtifactType.Behavior:
						var behaviorFolder = Taxonomy.Behaviors.Single(e =>
							e.Value.Artifact.Name == name);
						break;
					case ArtifactType.BehaviorGroup:
						var behaviorGroupFolder = Taxonomy.BehaviorGroups.Single(e =>
							e.Value.Artifact.Name == name);
						break;
					case ArtifactType.PropertySet:
						var propertySetFolder = Taxonomy.PropertySets.Single(e =>
							e.Value.Artifact.Name == name);
						break;
					case ArtifactType.TokenTemplate:
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
		
		internal static bool CheckForUniqueTemplate(string formula, string name)
		{
			_log.Info("CheckForUniqueTemplate: " + name);
			try
			{
				if(!string.IsNullOrEmpty(GetArtifactFolderNameBySymbol(ArtifactType.TokenTemplate, formula)))
					throw new Exception("Tooling Symbol Found.");
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
			var (name, visual, tooling) = Utils.GetRandomArtifactFromArtifact(artifact.Name, artifact.ArtifactSymbol.Visual, artifact.ArtifactSymbol.Tooling);
			newArtifact.Name = name;
			newArtifact.ArtifactSymbol.Visual = visual;
			newArtifact.ArtifactSymbol.Tooling = tooling;
			newArtifact.ArtifactSymbol.Id = Guid.NewGuid().ToString();
			newArtifact.ArtifactSymbol.Version = "1.0";
			return newArtifact;
		}
		
		internal static (string, ArtifactSymbol) MakeUniqueTokenFormula(string name, ArtifactSymbol formula)
		{
			var newArtifact = formula.Clone();
			var (newName, visual, tooling) = Utils.GetRandomTemplate(name, formula.Visual, formula.Tooling);
			newArtifact.Id = Guid.NewGuid().ToString();
			newArtifact.Visual = visual;
			newArtifact.Tooling = tooling;
			newArtifact.Version = "1.0";
			return (newName, newArtifact);
		}
		
		
		private static void BuildTemplateIndexes()
		{
			var nameIndex = new TemplateIndex();
			var formulaIndex = new TemplateIndex();
			var idIndex = new TemplateIndex();
			var branches = Enum.GetValues(typeof(ClassificationBranch));
			foreach (var b in branches)
			{
				var currentBranch = (ClassificationBranch) b;

				switch (currentBranch)
				{
					case ClassificationBranch.FractionalFungible:
						foreach (var t in Taxonomy.TokenTemplates.FractionalFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}
						
						foreach (var t in Taxonomy.TokenTemplates.Hybrids.FractionalFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}

						break;
					case ClassificationBranch.WholeFungible:
						foreach (var t in Taxonomy.TokenTemplates.WholeFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}
						
						foreach (var t in Taxonomy.TokenTemplates.Hybrids.WholeFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}

						break;
					case ClassificationBranch.FractionalNonFungible:
						foreach (var t in Taxonomy.TokenTemplates.FractionalNonFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}
						
						foreach (var t in Taxonomy.TokenTemplates.Hybrids.FractionalNonFungibles.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}

						break;
					case ClassificationBranch.Singleton:
						foreach (var t in Taxonomy.TokenTemplates.Singletons.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}
						
						foreach (var t in Taxonomy.TokenTemplates.Hybrids.Singletons.TokenTemplates)
						{
							nameIndex.Templates.Add(t.Parent.Name, t);
							formulaIndex.Templates.Add(t.Parent.Formula.Tooling, t);
							idIndex.Templates.Add(t.Parent.Formula.Id, t);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			
				TemplateIndexes.SaveToCache(NameIndex, nameIndex, DateTime.Now.AddDays(1));
				TemplateIndexes.SaveToCache(FormulaIndex, formulaIndex, DateTime.Now.AddDays(1));
				TemplateIndexes.SaveToCache(IdIndex, idIndex, DateTime.Now.AddDays(1));
			}
		}
	}
}