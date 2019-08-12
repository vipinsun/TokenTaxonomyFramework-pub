using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using log4net;
using Newtonsoft.Json.Linq;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.Controllers
{
	public static class TaxonomyController
	{

		private static readonly string ArtifactPath;
		private static readonly string FolderSeparator = TxService.FolderSeparator;
		private static readonly string Latest = TxService.Latest;
		private static readonly ILog Log;

		static TaxonomyController()
		{
			Utils.InitLog();
			Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			ArtifactPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + FolderSeparator +
			               TxService.ArtifactPath;
		}

		#region load
		internal static Model.Taxonomy Load()
		{
			if (!Directory.Exists(ArtifactPath))
			{
				var err = "Artifact Path not found: " + ArtifactPath;
				Log.Error(err);
				throw new Exception(err);
			}

			Log.Info("Artifact Folder Found, loading to Taxonomy.");
			var root = new DirectoryInfo(ArtifactPath);
			Model.Taxonomy taxonomy;
			
			var rJson = root.GetFiles("Taxonomy.json");
			var fJson = root.GetFiles("FormulaGrammar.json");
			try
			{
				taxonomy = GetArtifact<Model.Taxonomy>(rJson[0]);
				taxonomy.FormulaGrammar = GetArtifact<FormulaGrammar>(fJson[0]);
				Log.Info("Loaded Taxonomy Version: " + taxonomy.Version);
				Log.Info("Taxonomy Formula Grammar loaded");
			}
			catch (Exception e)
			{
				Log.Error("Failed to load Taxonomy: " + e);
				throw;
			}

			var aPath = ArtifactPath + FolderSeparator;
			if (Directory.Exists(aPath + ModelMap.BaseFolder))
			{
				Log.Info("Base Artifact Folder Found, loading to Base Token Types");
				var bases = new DirectoryInfo(aPath + ModelMap.BaseFolder);
				foreach (var ad in bases.EnumerateDirectories())
				{
					Base baseType;
					Log.Info("Loading " + ad.Name);
					var versions = ad.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							baseType = GetArtifact<Base>(bJson[0]);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load base token type: " + ad.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}

					baseType.Artifact = GetArtifactFiles(ad, baseType.Artifact);
					taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.Id, baseType);
				}
			}
			else
			{
				Log.Error("Base artifact folder NOT found, moving on to behaviors.");
			}

			if (Directory.Exists(aPath + ModelMap.BehaviorFolder))
			{

				Log.Info("Behavior Artifact Folder Found, loading to Behaviors");
				var behaviors = new DirectoryInfo(aPath + ModelMap.BehaviorFolder);

				foreach (var ad in behaviors.EnumerateDirectories())
				{
					Behavior behavior;
					Log.Info("Loading " + ad.Name);
					var versions = ad.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							behavior = GetArtifact<Behavior>(bJson[0]);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load Behavior: " + ad.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}

					behavior.Artifact = GetArtifactFiles(ad, behavior.Artifact);
					taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.Id, behavior);
				}
			}

			if (Directory.Exists(aPath + ModelMap.BehaviorGroupFolder))
			{

				Log.Info("BehaviorGroup Artifact Folder Found, loading to BehaviorGroups");
				var behaviorGroups = new DirectoryInfo(aPath + ModelMap.BehaviorGroupFolder);
				
				foreach (var ad in behaviorGroups.EnumerateDirectories())
				{
					BehaviorGroup behaviorGroup;
					Log.Info("Loading " + ad.Name);
					var versions = ad.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							behaviorGroup = GetArtifact<BehaviorGroup>(bJson[0]);

						}
						catch (Exception e)
						{
							Log.Error("Failed to load BehaviorGroup: " + ad.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}

					behaviorGroup.Artifact = GetArtifactFiles(ad, behaviorGroup.Artifact);
					taxonomy.BehaviorGroups.Add(behaviorGroup.Artifact.ArtifactSymbol.Id, behaviorGroup);
				}
			}
			
			if(Directory.Exists(aPath + ModelMap.PropertySetFolder))
			{

				Log.Info("PropertySet Artifact Folder Found, loading to PropertySets");
				var propertySets = new DirectoryInfo(aPath + ModelMap.PropertySetFolder);
				foreach (var ad in propertySets.EnumerateDirectories())
				{
					PropertySet propertySet;
					Log.Info("Loading " + ad.Name);
					var versions = ad.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							propertySet = GetArtifact<PropertySet>(bJson[0]);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load PropertySet: " + ad.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}
					propertySet.Artifact = GetArtifactFiles(ad, propertySet.Artifact);
					taxonomy.PropertySets.Add(propertySet.Artifact.ArtifactSymbol.Id, propertySet);
				}
			}

			if (Directory.Exists(aPath + ModelMap.TemplateFormulasFolder))
			{
				Log.Info("TemplateFormulas Folder Found, loading to TemplateFormulas");
				var formulaDirectory = new DirectoryInfo(aPath + ModelMap.TemplateFormulasFolder);
				var formulaFolders = formulaDirectory.EnumerateDirectories();
				foreach (var f in formulaFolders)
				{
					TemplateFormula formula;
					Log.Info("Loading " + f.Name);
					var versions = f.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							formula = GetArtifact<TemplateFormula>(bJson[0]);
						}
						catch (Exception e)
						{
							Log.Error("Failed to load TemplateFormula: " + f.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}
					formula.Artifact = GetArtifactFiles(f, formula.Artifact);
					taxonomy.TemplateFormulas.Add(formula.Artifact.ArtifactSymbol.Id,formula);
				}
				
			}

			if (Directory.Exists(aPath + ModelMap.TemplateDefinitionsFolder))
			{
				Log.Info("TemplateDefinitions Folder Found, loading to TemplateDefinitions");
				var definitionsFolder = new DirectoryInfo(aPath + ModelMap.TemplateDefinitionsFolder);
				var definitions = definitionsFolder.EnumerateDirectories();
				foreach (var t in definitions)
				{
					TemplateDefinition definition;
					Log.Info("Loading " + t.Name);
					var versions = t.GetDirectories("latest");
					var bJson = versions.FirstOrDefault()?.GetFiles("*.json");
					if (bJson != null)
					{
						try
						{
							definition = GetArtifact<TemplateDefinition>(bJson[0]);

						}
						catch (Exception e)
						{
							Log.Error("Failed to load TemplateDefinition: " + t.Name);
							Log.Error(e);
							continue;
						}
					}
					else
					{
						continue;
					}
					definition.Artifact = GetArtifactFiles(t, definition.Artifact);
					taxonomy.TemplateDefinitions.Add(definition.Artifact.ArtifactSymbol.Id,
						definition);
				}
			}

			BuildHierarchy(ref taxonomy);
			
			return taxonomy;
		}
		
		private static void BuildHierarchy(ref Model.Taxonomy taxonomy)
		{
			Log.Info("Building Template Hierarchy");
			var hybrids = new HybridBranch
			{
				Fungible = new FungibleBranch()
				{
					Fractional = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.Hybrid,
								TokenType = TokenType.Fungible,
								TokenUnit = TokenUnit.Fractional
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Hybrid-FractionalFungibleParent",
						Templates = new TokenTemplates()
					},
					Whole = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.Hybrid,
								TokenType = TokenType.Fungible,
								TokenUnit = TokenUnit.Whole
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Hybrid-WholeFungibleParent",
						Templates = new TokenTemplates()
					}
				},
				NonFungible = new NonFungibleBranch
				{
					Fractional = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.Hybrid,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Fractional
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Hybrid-FractionalFungibleParent",
						Templates = new TokenTemplates()
					},
					Whole = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.Hybrid,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Whole
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Hybrid-WholeFungibleParent",
						Templates = new TokenTemplates()
					},
					Singleton = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.Hybrid,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Singleton
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Hybrid-Singleton",
						Templates = new TokenTemplates()
					}
				}
			};
			
			var hierarchy = new Hierarchy
			{
				Hybrids = hybrids,
				Fungibles = new FungibleBranch()
				{
					Fractional = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.SingleToken,
								TokenType = TokenType.Fungible,
								TokenUnit = TokenUnit.Fractional
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "FractionalFungibleParent",
						Templates = new TokenTemplates()
					},
					Whole = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.SingleToken,
								TokenType = TokenType.Fungible,
								TokenUnit = TokenUnit.Whole
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "WholeFungibleParent",
						Templates = new TokenTemplates()
					}
				},
				NonFungibles = new NonFungibleBranch
				{
					Fractional = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.SingleToken,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Fractional
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "FractionalFungibleParent",
						Templates = new TokenTemplates()
					},
					Whole = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.SingleToken,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Whole
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "WholeFungibleParent",
						Templates = new TokenTemplates()
					},
					Singleton = new BranchRoot
					{
						BranchIdentifier = new BranchIdentifier
						{
							Classification = new Classification
							{
								TemplateType = TemplateType.SingleToken,
								TokenType = TokenType.NonFungible,
								TokenUnit = TokenUnit.Singleton
							},
							FormulaId = Guid.Empty.ToString()
						},
						Name = "Singleton",
						Templates = new TokenTemplates()
					}
				}
				
			};
			
			var templates = BuildTemplates(ref taxonomy);
			foreach (var (id, template) in templates.Template)
			{
				var baseType =
					taxonomy.BaseTokenTypes.SingleOrDefault(e => e.Key == template.Formula.TokenBase.Base.Id).Value;
				if (baseType != null)
				{
					switch (template.Formula.TemplateType)
					{
						case TemplateType.SingleToken:
							switch (baseType.TokenUnit)
							{
								case TokenUnit.Fractional:
									if (baseType.TokenType == TokenType.Fungible)
										hierarchy.Fungibles.Fractional.Templates.Template.Add(id, template);
									else if (baseType.TokenType == TokenType.NonFungible)
										hierarchy.NonFungibles.Fractional.Templates.Template
											.Add(id, template);
									Log.Info("Template: " + id + " added to Single/Fungibles");
									break;
								case TokenUnit.Whole:
									if (baseType.TokenType == TokenType.Fungible)
										hierarchy.Fungibles.Whole.Templates.Template.Add(id, template);
									else if (baseType.TokenType == TokenType.NonFungible)
										hierarchy.NonFungibles.Whole.Templates.Template
											.Add(id, template);
									Log.Info("Template: " + id + " added to Single/NonFungibles");
									break;
								case TokenUnit.Singleton:
									hierarchy.NonFungibles.Singleton.Templates.Template
										.Add(id, template);
									Log.Info("Template: " + id + " added to Single/Singletons");
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}
							break;
						case TemplateType.Hybrid:
							switch (baseType.TokenUnit)
							{
								case TokenUnit.Fractional:
									if (baseType.TokenType == TokenType.Fungible)
										hierarchy.Hybrids.Fungible.Fractional.Templates.Template.Add(id, template);
									else if (baseType.TokenType == TokenType.NonFungible)
										hierarchy.Hybrids.NonFungible.Fractional.Templates.Template
											.Add(id, template);
									Log.Info("Template: " + id + " added to Hybrid/Fungibles");
									break;
								case TokenUnit.Whole:
									if (baseType.TokenType == TokenType.Fungible)
										hierarchy.Hybrids.Fungible.Whole.Templates.Template.Add(id, template);
									else if (baseType.TokenType == TokenType.NonFungible)
										hierarchy.Hybrids.NonFungible.Whole.Templates.Template
											.Add(id, template);
									Log.Info("Template: " + id + " added to Hybrid/NonFungibles");
									break;
								case TokenUnit.Singleton:
									hierarchy.Hybrids.NonFungible.Singleton.Templates.Template
										.Add(id, template);
									Log.Info("Template: " + id + " added to Hybrid/Singletons");
									break;
								default:
									throw new ArgumentOutOfRangeException();
							}
							break;
					}
				}

				taxonomy.TokenTemplateHierarchy = hierarchy;
			}
		}
		
		private static TokenTemplates BuildTemplates(ref Model.Taxonomy taxonomy)
		{
			var retVal = new TokenTemplates();

			foreach (var (templateId, definition) in taxonomy.TemplateDefinitions)
			{
				var dFormula = taxonomy.TemplateFormulas.SingleOrDefault(e => e.Key == definition.FormulaReference.Id)
					.Value;
				if (dFormula != null)
				{
					retVal.Template.Add(templateId, new TokenTemplate
					{
						Definition = definition,
						Formula = dFormula
					});
				}
				else
				{
					Log.Error("Token Template with definition id: " + templateId + " does not have a paired formula or it cannot be located.");
				}
			}

			TokenTemplateCache.ResetCache();
			TokenTemplateCache.LoadTemplates(retVal);
			return retVal;
		}
		

		#endregion
		
		#region Create, Update, Delete
		
		public static NewArtifactResponse CreateArtifact(NewArtifactRequest artifactRequest)
		{
			var artifactJson = "";
			DirectoryInfo outputFolder = null;
		
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));
			var artifactType = artifactRequest.Type;
			var artifactName = "";
			var retVal = new NewArtifactResponse
			{
				Type = artifactType
			};
			
			switch (artifactType)
			{
				case ArtifactType.Base:
					Log.Info("CreateArtifact was requested to create a new base token type, which is not supported.");
					break;
				case ArtifactType.Behavior:
					var newBehavior = artifactRequest.Artifact.Unpack<Behavior>();
					if (!ModelManager.CheckForUniqueArtifact(ArtifactType.Behavior, newBehavior.Artifact))
					{
						newBehavior.Artifact = ModelManager.MakeUniqueArtifact(newBehavior.Artifact);
					}

					if (string.IsNullOrEmpty(newBehavior.Artifact.ArtifactSymbol.Id))
						newBehavior.Artifact.ArtifactSymbol.Id = Guid.NewGuid().ToString().ToLower();
					artifactName = newBehavior.Artifact.Name.ToLower();
					
					//unique invocations
					foreach (var i in newBehavior.Invocations)
					{
						i.Id = Guid.NewGuid().ToString().ToLower();
					}

					//unique invocations
					foreach (var p in newBehavior.Properties)
					{
						foreach (var pi in p.PropertyInvocations)
							pi.Id = Guid.NewGuid().ToString().ToLower();
					}
					
					outputFolder = GetArtifactFolder(artifactType, artifactName);
					if(newBehavior.Artifact.ArtifactFiles.Count > 0)
						CreateArtifactFiles(newBehavior.Artifact.ArtifactFiles, outputFolder, artifactName);
					else
					{
						AddArtifactFiles(outputFolder, artifactName, "Behaviors", artifactType);
					}
					artifactJson = jsf.Format(newBehavior);
					retVal.ArtifactTypeObject= Any.Pack(newBehavior);
					break;
				case ArtifactType.BehaviorGroup:
					var newBehaviorGroup = artifactRequest.Artifact.Unpack<BehaviorGroup>();
					if (!ModelManager.CheckForUniqueArtifact(ArtifactType.BehaviorGroup, newBehaviorGroup.Artifact))
					{
						newBehaviorGroup.Artifact = ModelManager.MakeUniqueArtifact(newBehaviorGroup.Artifact);
					}
					artifactName = newBehaviorGroup.Artifact.Name.ToLower();
					outputFolder = GetArtifactFolder(artifactType, artifactName);
					if(newBehaviorGroup.Artifact.ArtifactFiles.Count > 0)
						CreateArtifactFiles(newBehaviorGroup.Artifact.ArtifactFiles, outputFolder, artifactName);
					else
					{
						AddArtifactFiles(outputFolder, artifactName, "BehaviorGroups", artifactType);
					}
					artifactJson = jsf.Format(newBehaviorGroup);
					retVal.ArtifactTypeObject= Any.Pack(newBehaviorGroup);
					break;
				case ArtifactType.PropertySet:
					var newPropertySet = artifactRequest.Artifact.Unpack<PropertySet>();
					if (!ModelManager.CheckForUniqueArtifact(ArtifactType.PropertySet, newPropertySet.Artifact))
					{
						newPropertySet.Artifact = ModelManager.MakeUniqueArtifact(newPropertySet.Artifact);
					}
					artifactName = newPropertySet.Artifact.Name.ToLower();
					
					//unique invocations
					foreach (var p in newPropertySet.Properties)
					{
						foreach (var pi in p.PropertyInvocations)
							pi.Id = Guid.NewGuid().ToString().ToLower();
					}
					
					outputFolder = GetArtifactFolder(artifactType, artifactName);
					if(newPropertySet.Artifact.ArtifactFiles.Count > 0)
						CreateArtifactFiles(newPropertySet.Artifact.ArtifactFiles, outputFolder, artifactName);
					else
					{
						AddArtifactFiles(outputFolder, artifactName, "PropertySets", artifactType);
					}
					retVal.ArtifactTypeObject= Any.Pack(newPropertySet);
					artifactJson = jsf.Format(newPropertySet);
					break;
				case ArtifactType.TemplateFormula:
					var templateFormula = artifactRequest.Artifact.Unpack<TemplateFormula>();
					if (!ModelManager.CheckForUniqueTemplateFormula(templateFormula.Artifact.Name, templateFormula.Artifact.ArtifactSymbol.Tooling))
					{
						var (newName, artifactSymbol) = ModelManager.MakeUniqueTokenFormula(templateFormula.Artifact.Name, templateFormula.Artifact.ArtifactSymbol);
						templateFormula.Artifact.Name = newName;
						templateFormula.Artifact.ArtifactSymbol = artifactSymbol;
					}
					artifactName = Utils.FirstToUpper(templateFormula.Artifact.Name);
					templateFormula.Artifact.Name = artifactName;
					if (string.IsNullOrEmpty(templateFormula.Artifact.ArtifactSymbol.Id))
						templateFormula.Artifact.ArtifactSymbol.Id = Guid.NewGuid().ToString().ToLower();
					outputFolder = GetArtifactFolder(artifactType, artifactName);
					if(templateFormula.Artifact.ArtifactFiles.Count > 0)
						CreateArtifactFiles(templateFormula.Artifact.ArtifactFiles, outputFolder, artifactName);
					else
					{
						AddArtifactFiles(outputFolder, artifactName, ModelMap.TemplateFormulasFolder, artifactType);
					}
					retVal.ArtifactTypeObject= Any.Pack(templateFormula);
					artifactJson = jsf.Format(templateFormula);
					break;
				case ArtifactType.TemplateDefinition:
					var templateDefinition = artifactRequest.Artifact.Unpack<TemplateDefinition>();
					if (ModelManager.CheckForUniqueTemplateFormula(templateDefinition.Artifact.Name, templateDefinition.Artifact.ArtifactSymbol.Tooling))
					{
						var newName = ModelManager.MakeUniqueDefinitionName(templateDefinition.Artifact.Name);
						templateDefinition.Artifact.Name = newName;
					}
					artifactName = Utils.FirstToUpper(templateDefinition.Artifact.Name);
					templateDefinition.Artifact.Name = artifactName;
					
					if (string.IsNullOrEmpty(templateDefinition.Artifact.ArtifactSymbol.Id))
						templateDefinition.Artifact.ArtifactSymbol.Id = Guid.NewGuid().ToString().ToLower();
					
					outputFolder = GetArtifactFolder(artifactType, artifactName);
					if(templateDefinition.Artifact.ArtifactFiles.Count > 0)
						CreateArtifactFiles(templateDefinition.Artifact.ArtifactFiles, outputFolder, artifactName);
					else
					{
						AddArtifactFiles(outputFolder, artifactName, ModelMap.TemplateFormulasFolder, artifactType);
					}
					retVal.ArtifactTypeObject= Any.Pack(templateDefinition);
					artifactJson = jsf.Format(templateDefinition);
					break;
				case ArtifactType.TokenTemplate:
					break;
				default:
					Log.Error("No matching type found for: " + artifactType);
					throw new ArgumentOutOfRangeException();
			}

			Log.Info("Artifact Destination: " + ArtifactPath + FolderSeparator + artifactRequest.Type + " folder");
			var formattedJson = JToken.Parse(artifactJson).ToString();
			
			//test to make sure formattedJson will Deserialize.
			try
			{
				switch (artifactRequest.Type)
				{
					case ArtifactType.Base:
						var testBase = JsonParser.Default.Parse<Base>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testBase.Artifact.Name);
						break;
					case ArtifactType.Behavior:
						var testBehavior = JsonParser.Default.Parse<Behavior>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testBehavior.Artifact.Name);
						break;
					case ArtifactType.BehaviorGroup:
						var testBehaviorGroup = JsonParser.Default.Parse<BehaviorGroup>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testBehaviorGroup.Artifact.Name);
						break;
					case ArtifactType.PropertySet:
						var testPropertySet = JsonParser.Default.Parse<PropertySet>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testPropertySet.Artifact.Name);
						break;
					case ArtifactType.TemplateFormula:
						var testFormula = JsonParser.Default.Parse<TemplateFormula>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testFormula.Artifact.Name);
						break;
					case ArtifactType.TemplateDefinition:
						var testDefinition = JsonParser.Default.Parse<TemplateDefinition>(formattedJson);
						Log.Info("Artifact type: " + artifactType + " successfully deserialized: " +
						          testDefinition.Artifact.Name);
						break;
					case ArtifactType.TokenTemplate:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception e)
			{
				Log.Error("Json failed to deserialize back into: " + artifactType);
				Log.Error(e);
				return new NewArtifactResponse();
			}

			Log.Info("Creating Artifact: " + formattedJson);
			if (outputFolder != null)
			{
				var artifactStream = File.CreateText(outputFolder.FullName + FolderSeparator + artifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();
			}
			
			Log.Info("Complete");
			return retVal;
		}
		
		public static UpdateArtifactResponse UpdateArtifact(UpdateArtifactRequest artifactRequest)
		{
			string artifactJson;
			
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));
			var artifactType = artifactRequest.Type;
	
			var retVal = new UpdateArtifactResponse
			{
				Type = artifactType
			};

			string artifactName;
			string existingVersion;
			switch (artifactType)
			{
				case ArtifactType.Base:
					Log.Info("UpdateArtifact was requested to update a base token type, which is not supported.");
					break;
				case ArtifactType.Behavior:
					var updateBehavior = artifactRequest.ArtifactTypeObject.Unpack<Behavior>();

					var existingBehavior = ModelManager.GetBehaviorArtifact(new ArtifactSymbol
					{
						Tooling = updateBehavior.Artifact.ArtifactSymbol.Tooling
					});
					existingVersion = existingBehavior.Artifact.ArtifactSymbol.Version;
					existingBehavior.MergeFrom(updateBehavior);
					artifactName = updateBehavior.Artifact.Name.ToLower();
					artifactJson = jsf.Format(existingBehavior);
					var (outcomeB, messageB) = VersionArtifact(ModelMap.BehaviorFolder, artifactName,
						existingVersion, artifactJson, artifactType);
					if (outcomeB)
					{
						ModelManager.AddOrUpdateInMemoryArtifact(artifactRequest.Type,
							Any.Pack(existingBehavior));
						retVal.ArtifactTypeObject = Any.Pack(existingBehavior);
						retVal.Updated = true;
					}
					else
					{
						retVal.Updated = false;
						Log.Error(messageB);
					}
					Log.Info("TOM and Artifact updated.");
					return retVal;
				case ArtifactType.BehaviorGroup:
					var updateBehaviorGroup = artifactRequest.ArtifactTypeObject.Unpack<BehaviorGroup>();

					var existingBehaviorGroup = ModelManager.GetBehaviorGroupArtifact(new ArtifactSymbol
					{
						Tooling = updateBehaviorGroup.Artifact.ArtifactSymbol.Tooling
					});
					existingVersion = existingBehaviorGroup.Artifact.ArtifactSymbol.Version;
					existingBehaviorGroup.MergeFrom(updateBehaviorGroup);
					artifactName = updateBehaviorGroup.Artifact.Name.ToLower();

					artifactJson = jsf.Format(existingBehaviorGroup);
					var (outcomeBg, messageBg) = VersionArtifact(ModelMap.BehaviorGroupFolder, artifactName,
						existingVersion, artifactJson, artifactType);
					if (outcomeBg)
					{
						ModelManager.AddOrUpdateInMemoryArtifact(artifactRequest.Type,
							Any.Pack(existingBehaviorGroup));
						retVal.ArtifactTypeObject = Any.Pack(existingBehaviorGroup);
						retVal.Updated = true;
					}
					else
					{
						retVal.Updated = false;
						Log.Error(messageBg);
					}
					Log.Info("TOM and Artifact updated.");
					return retVal;
				case ArtifactType.PropertySet:
					var updatePropertySet = artifactRequest.ArtifactTypeObject.Unpack<PropertySet>();

					var existingPropertySet = ModelManager.GetPropertySetArtifact(new ArtifactSymbol
					{
						Tooling = updatePropertySet.Artifact.ArtifactSymbol.Tooling
					});
					existingVersion = existingPropertySet.Artifact.ArtifactSymbol.Version;
					existingPropertySet.MergeFrom(updatePropertySet);
					artifactName = updatePropertySet.Artifact.Name.ToLower();
					
					artifactJson = jsf.Format(existingPropertySet);
					var (outcomePs, messagePs) = VersionArtifact(ModelMap.PropertySetFolder, artifactName,
						existingVersion, artifactJson, artifactType);
					if (outcomePs)
					{
						ModelManager.AddOrUpdateInMemoryArtifact(artifactRequest.Type,
							Any.Pack(existingPropertySet));
						retVal.ArtifactTypeObject = Any.Pack(existingPropertySet);
						retVal.Updated = true;
					}
					else
					{
						retVal.Updated = false;
						Log.Error(messagePs);
					}
					Log.Info("TOM and Artifact updated.");
					return retVal;
				case ArtifactType.TemplateFormula:
					var updateTokenTemplate = artifactRequest.ArtifactTypeObject.Unpack<TemplateFormula>();

					var templateFormulaArtifact =
						ModelManager.GetTemplateFormulaArtifact(updateTokenTemplate.Artifact.ArtifactSymbol);
					existingVersion = templateFormulaArtifact.Artifact.ArtifactSymbol.Version;
					templateFormulaArtifact.MergeFrom(updateTokenTemplate);
					templateFormulaArtifact.Artifact.ArtifactSymbol.Id = Guid.NewGuid().ToString();
					artifactName = updateTokenTemplate.Artifact.Name.ToLower();
					
					artifactJson = jsf.Format(templateFormulaArtifact);
					var (outcomeT, messageT) = VersionArtifact(ModelMap.TemplateFormulasFolder, artifactName,
						existingVersion, artifactJson, artifactType);
					if (outcomeT)
					{
						ModelManager.AddOrUpdateInMemoryArtifact(artifactRequest.Type,
							Any.Pack(templateFormulaArtifact));
						retVal.ArtifactTypeObject = Any.Pack(templateFormulaArtifact);
						retVal.Updated = true;
					}
					else
					{
						retVal.Updated = false;
						Log.Error(messageT);
					}
					Log.Info("TOM and Artifact updated.");
					return retVal;
				case ArtifactType.TemplateDefinition:
					var updateTemplateDefinition = artifactRequest.ArtifactTypeObject.Unpack<TemplateDefinition>();

					var templateDefinitionArtifact =
						ModelManager.GetTemplateDefinitionArtifact(updateTemplateDefinition.Artifact.ArtifactSymbol);
					existingVersion = templateDefinitionArtifact.Artifact.ArtifactSymbol.Version;
					templateDefinitionArtifact.MergeFrom(updateTemplateDefinition);
					templateDefinitionArtifact.Artifact.ArtifactSymbol.Id = Guid.NewGuid().ToString();
					artifactName = updateTemplateDefinition.Artifact.Name.ToLower();
					
					artifactJson = jsf.Format(templateDefinitionArtifact);
					var (outcomeD, messageD) = VersionArtifact(ModelMap.TemplateDefinitionsFolder, artifactName,
						existingVersion, artifactJson, artifactType);
					if (outcomeD)
					{
						ModelManager.AddOrUpdateInMemoryArtifact(artifactRequest.Type,
							Any.Pack(templateDefinitionArtifact));
						retVal.ArtifactTypeObject = Any.Pack(templateDefinitionArtifact);
						retVal.Updated = true;
					}
					else
					{
						retVal.Updated = false;
						Log.Error(messageD);
					}
					Log.Info("TOM and Artifact updated.");
					return retVal;
				default:
					Log.Error("No matching type found for: " + artifactType);
					throw new ArgumentOutOfRangeException();
			}

			return retVal;
		}

		private static bool SaveArtifact(ArtifactType type, string artifactName, string artifactJson,
			FileSystemInfo outputFolder)
		{
			Log.Info("Artifact Destination: " + ArtifactPath + FolderSeparator + type + " folder");
			var formattedJson = JToken.Parse(artifactJson).ToString();

			//test to make sure formattedJson will Deserialize.
			try
			{
				switch (type)
				{
					case ArtifactType.Base:
						var testBase = JsonParser.Default.Parse<Base>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBase.Artifact.Name);
						break;
					case ArtifactType.Behavior:
						var testBehavior = JsonParser.Default.Parse<Behavior>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBehavior.Artifact.Name);
						break;
					case ArtifactType.BehaviorGroup:
						var testBehaviorGroup = JsonParser.Default.Parse<BehaviorGroup>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBehaviorGroup.Artifact.Name);
						break;
					case ArtifactType.PropertySet:
						var testPropertySet = JsonParser.Default.Parse<PropertySet>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testPropertySet.Artifact.Name);
						break;
					case ArtifactType.TemplateFormula:
						var templateFormula = JsonParser.Default.Parse<TemplateFormula>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          templateFormula.Artifact.Name);
						break;
					case ArtifactType.TemplateDefinition:
						var templateDefinition = JsonParser.Default.Parse<TemplateDefinition>(formattedJson);
						Log.Info("Artifact type: " + type + " successfully deserialized: " +
						          templateDefinition.Artifact.Name);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception e)
			{
				Log.Error("Json failed to deserialize back into: " + type);
				Log.Error(e);
				return false;
			}

			Log.Info("Saving Artifact: " + formattedJson);
			if (outputFolder != null)
			{
				var artifactStream = File.CreateText(outputFolder.FullName + FolderSeparator + artifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();
			}

			Log.Info("Complete");
			return true;
		}

		internal static DeleteArtifactResponse DeleteArtifact(DeleteArtifactRequest artifactRequest)
		{
			Log.Info("DeleteArtifact of type: " + artifactRequest.ArtifactSymbol.Type + " with Tooling symbol: " + artifactRequest.ArtifactSymbol);
			DeleteArtifactResponse response;
			var artifactFolderName =
				ModelManager.GetArtifactFolderNameBySymbol(artifactRequest.ArtifactSymbol.Type, artifactRequest.ArtifactSymbol
					.Tooling);
			try
			{
				switch (artifactRequest.ArtifactSymbol.Type)
				{
					case ArtifactType.Base:
						DeleteArtifactFolder(ModelMap.BaseFolder, artifactFolderName);
						break;
					case ArtifactType.Behavior:
						DeleteArtifactFolder(ModelMap.BehaviorFolder, artifactFolderName);
						break;
					case ArtifactType.BehaviorGroup:
						DeleteArtifactFolder(ModelMap.BehaviorGroupFolder, artifactFolderName);
						break;
					case ArtifactType.PropertySet:
						DeleteArtifactFolder(ModelMap.PropertySetFolder, artifactFolderName);
						break;
					case ArtifactType.TemplateFormula:
						DeleteArtifactFolder(ModelMap.TemplateFormulasFolder, artifactFolderName);
						break;
					case ArtifactType.TemplateDefinition:
						DeleteArtifactFolder(ModelMap.TemplateDefinitionsFolder, artifactFolderName);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				response = new DeleteArtifactResponse
				{
					Deleted = true
				};
			}
			catch (Exception e)
			{
				Log.Error("Error attempting to delete artifact of type: " + artifactRequest.ArtifactSymbol.Type + " with Tooling symbol: " + artifactRequest.ArtifactSymbol);
				Log.Error(e);
				response = new DeleteArtifactResponse
				{
					Deleted = false
				};
			}

			return response;
		}
		
		#endregion
		
		#region Artifact Utils

		private static DirectoryInfo GetArtifactFolder(ArtifactType type, string artifactName)
		{
			string typeFolderName;
			switch (type)
			{
				case ArtifactType.Base:
					typeFolderName = ModelMap.BaseFolder;
					break;
				case ArtifactType.Behavior:
					typeFolderName = ModelMap.BehaviorFolder;
					break;
				case ArtifactType.BehaviorGroup:
					typeFolderName = ModelMap.BehaviorGroupFolder;
					break;
				case ArtifactType.PropertySet:
					typeFolderName = ModelMap.PropertySetFolder;
					break;
				case ArtifactType.TemplateFormula:
					typeFolderName = ModelMap.TemplateFormulasFolder;
					break;
				case ArtifactType.TemplateDefinition:
					typeFolderName = ModelMap.TemplateDefinitionsFolder;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}

			var path = ArtifactPath + FolderSeparator + typeFolderName + FolderSeparator + artifactName + Latest;
			return Directory.Exists(path) ? new DirectoryInfo(ArtifactPath + FolderSeparator + typeFolderName + FolderSeparator + artifactName + Latest) : Directory.CreateDirectory(path);
		}
		
		private static void DeleteArtifactFolder(string artifactTypeFolderName, string artifactFolderName)
		{
			try
			{
				Directory.Delete(
					ArtifactPath + FolderSeparator + artifactTypeFolderName + FolderSeparator +
					artifactFolderName, true);
			} 
			catch (Exception e) 
			{
				Log.Error("Unable to Delete Artifact Folder: " + artifactTypeFolderName + FolderSeparator + artifactFolderName);
				Log.Error(e);
			} 
		}
		
		private static void CreateArtifactFiles(IEnumerable<ArtifactFile> artifactArtifactFiles, FileSystemInfo outputFolder, string artifactName)
		{
			foreach (var af in artifactArtifactFiles)
			{
				switch (af.Content)
				{
					case ArtifactContent.Uml:
					{
						Log.Info("Creating Artifact MD UML File");
						var md  = File.CreateText(outputFolder.FullName + FolderSeparator + artifactName+".md");
						md.Write(af.FileData.ToStringUtf8());
						md.Close();
						break;
					}
					case ArtifactContent.Other:
					{
						Log.Info("Creating Artifact Other File");
						var other  = File.Create(outputFolder.FullName + FolderSeparator + af.FileName);
						other.Write(af.FileData.ToByteArray());
						other.Close();
						break;
					}
					case ArtifactContent.Definition:
						break;
					case ArtifactContent.Control:
						Log.Info("Creating Artifact Proto Control");
						var proto  = File.CreateText(outputFolder.FullName + FolderSeparator + artifactName+".proto");
						proto.Write(af.FileData.ToStringUtf8());
						proto.Close();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		
		private static TemplateFormula GetParentFormula(string fullPath, string folderSeparator, string artifactId)
		{
			var baseFile = File.OpenText(fullPath + ModelMap.TemplateFormulasFolder + folderSeparator + artifactId + folderSeparator + TxService.Latest + folderSeparator + artifactId +".json");
			var json = baseFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			var retVal = JsonParser.Default.Parse<TemplateFormula>(formattedJson);
			return retVal;
		}

		private static FormulaGrammar GenerateFormula()
		{
			var formula = new FormulaGrammar();

			var singleToken = new SingleTokenGrammar
			{
				
				BaseTokenToolingSymbol = "",
				Behaviors = new BehaviorList
				{
					ListStart = "{",
					BehaviorToolingSymbols = {"","" },
					ListEnd = "}"
				},
				GroupStart = "[",
				GroupEnd = "]"
			};
			
			var psli = new PropertySetList
			{
				ListStart = "+",
				PropertySets = { new PropertySetListItem
				{
					PropertySetSymbol = ""
				}}
			};
			singleToken.PropertySets = psli;

			formula.SingleTokenGrammar = singleToken;

			var hybrid = new HybridTokenGrammar
			{
				ChildrenStart = "(",
				ChildrenEnd = ")",
				Parent = singleToken
			};
			hybrid.ChildTokens.Add(singleToken);
			hybrid.ChildTokens.Add(singleToken);

			formula.HybridGrammar = hybrid;
			
			var hybridHybrids = new HybridTokenWithHybridChildrenGrammar
			{
				HybridChildrenStart = "(",
				HybridChildrenEnd = ")",
				Parent = singleToken
			};
			hybridHybrids.HybridChildTokens.Add(hybrid);
			hybridHybrids.HybridChildTokens.Add(hybrid);
			formula.HybridWithHybridsGrammar = hybridHybrids;
			
			return formula;
		}

		private static Base GetTokenTypeBase(TokenType tokenType, TokenUnit tokenUnit)
		{
            const string typeFolder = "base";
			var classFolder = ModelMap.GetBaseFolderName(tokenType, tokenUnit);
			var baseFile = File.OpenText(ArtifactPath + typeFolder + FolderSeparator + classFolder 
			                             + FolderSeparator + TxService.Latest + FolderSeparator + classFolder+".json");
			var json = baseFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			var baseType = JsonParser.Default.Parse<Base>(formattedJson);
			return baseType;
		}
		
		private static Artifact GetArtifactFiles(DirectoryInfo ad, Artifact artifact)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto", StringComparison.CurrentCulture))
				{
					var protoFile = GetArtifactText(af);
					artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
					continue;
				}

				if (af.Name.EndsWith("md", StringComparison.CurrentCulture))
				{
					var mdFile = GetArtifactText(af);

					artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
					continue;
				}

				if (af.Name.EndsWith("json", StringComparison.CurrentCulture))
				{
					continue;
				}

				var otherFile = GetArtifactBytes(af);

				var other = new ArtifactFile
				{
					FileName = af.Name,
					Content = ArtifactContent.Other
				};
				if (!af.Name.EndsWith(".DS_Store", StringComparison.CurrentCulture))
					other.FileData = ByteString.CopyFrom(otherFile);
				artifact.ArtifactFiles.Add(other);
			}

			return artifact;
		}

		private static T GetArtifact<T>(FileInfo artifact) where T : IMessage, new()
		{
			var typeFile = artifact.OpenText();
			var json = typeFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			return JsonParser.Default.Parse<T>(formattedJson);
		}

		private static string GetArtifactText(FileInfo artifactFile)
		{
			var typeFile = artifactFile.OpenText();
			return typeFile.ReadToEnd();
		}

		private static byte[] GetArtifactBytes(FileInfo artifactFile)
		{
			using (var ms = new MemoryStream())
			{
				var f = artifactFile.OpenRead();
				f.CopyTo(ms);
				return  ms.ToArray();
			}
		}

		private static (bool, string) VersionArtifact(string artifactTypeFolder, string artifactName, string version,
				string artifactJson, ArtifactType artifactType)
		{
			try
			{
				var latestPath = ArtifactPath + FolderSeparator + artifactTypeFolder + FolderSeparator +
				                 artifactName + Latest;

				if (string.IsNullOrEmpty(version))
					version = "1.0";
				var oldLatestPath = ArtifactPath + FolderSeparator + artifactTypeFolder + FolderSeparator +
				                    artifactName + FolderSeparator + version;
				var (outcome, message) = CreateVersion(latestPath, oldLatestPath);
				if (!outcome) return (false, message);
				var outputFolder =
					new DirectoryInfo(latestPath);

				return (SaveArtifact(artifactType, artifactName, artifactJson, outputFolder), "latest");
			}
			catch (Exception e)
			{
				Log.Error(e);
				return (false, e.Message);
			}
		}
		
		private static (bool, string) CreateVersion(string sourceDirName, string destDirName)
		{
			// Get the subdirectories for the specified directory.
			var dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				var err = "New Version could not be created, source directory does not exist or could not be found: "
					+ sourceDirName;
				Log.Error(err);
				return (false, err);
			}

			
			// If the destination directory doesn't exist, create it.
			if (Directory.Exists(destDirName))
			{
				Log.Error(destDirName + " already exists, creating a backup.");
				destDirName = Utils.Randomize(destDirName);
			}
			if (!Directory.Exists(destDirName))
			{

				Directory.CreateDirectory(destDirName);
				Log.Error(destDirName + " is previous Version.");
			}
        
			// Get the files in the directory and copy them to the new location.
			var files = dir.GetFiles();
			foreach (var file in files)
			{
				var tempPath = Path.Combine(destDirName, file.Name);
				file.CopyTo(tempPath, false);
			}

			return (true, "");
		}
		
		private static void AddArtifactFiles(DirectoryInfo outputFolder, string artifactName, string nameSpaceAdd, ArtifactType artifactType)
		{
			try
			{
				CreateMarkdown(outputFolder, artifactName, artifactType);
				CreateProto(outputFolder, artifactName, nameSpaceAdd);
			}
			catch (Exception e)
			{
				Log.Error("Error creating new artifact files");
				Log.Error(e);
			}
		}
		private static void CreateMarkdown(DirectoryInfo outputFolder, string artifactName, ArtifactType artifactType)
		{
			Log.Info("Creating Artifact Markdown");
			var md = File.CreateText(outputFolder + FolderSeparator + artifactName + ".md");
			md.Write("# " + artifactName + " a TTF " + artifactType);
			md.Close();

		}
		
		private static void CreateProto(DirectoryInfo outputFolder, string artifactName, string nameSpaceAdd)
		{
			Log.Info("Creating Artifact Proto");
			var pFile = outputFolder + FolderSeparator + artifactName + ".proto";
			
			var proto  = File.CreateText(pFile);
			var templateProto =
				File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
				                 FolderSeparator + "templates" + FolderSeparator + "artifact.proto");
			
			var ns = templateProto.Replace("BASE", nameSpaceAdd);
			ns = ns.Replace("NAME", artifactName);
			ns = ns.Replace("BASE", nameSpaceAdd.ToLower());
			proto.Write(ns.Replace("ARTIFACT", artifactName));
			proto.Close();
		}
		#endregion
	}
}