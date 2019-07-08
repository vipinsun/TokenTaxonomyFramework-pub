using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using log4net;
using Newtonsoft.Json.Linq;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using TTI.TTF.Taxonomy.Model;

namespace ArtifactGenerator
{
	internal static class FactGen{
	
		private static ILog _log;
		private static string ArtifactName { get; set; }
		private static string ArtifactPath { get; set; }
		private static ArtifactType ArtifactType { get; set; }
		private static TokenType BaseType { get; set; }

		private static ClassificationBranch Classification { get; set; }
		private static TaxonomyVersion TaxonomyVersion { get; set; }
		private static string Latest { get; set; }
		private static bool TemplateMode { get; set; }

		public static void Main(string[] args)
		{

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			if (args.Length == 6 || args.Length == 8 || args.Length == 10 || args.Length == 4)
			{
				for (var i = 0; i < args.Length; i++)
				{
					var arg = args[i];
					switch (arg)
					{
						case "--p":
							i++;
							ArtifactPath = args[i];
							continue;
						case "--n":
							i++;
							ArtifactName = args[i];
							continue;
						case "--t":
							i++;
							var t = Convert.ToInt32(args[i]);
							ArtifactType = (ArtifactType) t;
							continue;
						case "--b":
							i++;
							var b = Convert.ToInt32(args[i]);
							BaseType = (TokenType) b;
							continue;
						case "--c":
							i++;
							var c = Convert.ToInt32(args[i]);
							Classification = (ClassificationBranch) c;
							TemplateMode = true;
							continue;
						case "--v":
							i++;
							var v = args[i];
							TaxonomyVersion = new TaxonomyVersion
							{
								Version = v
							};
							continue;
						default:
							continue;
					}
				}
			}
			else
			{
				if (args.Length == 0)
				{
					_log.Info(
						"Usage: dotnet factgen --p [PATH_TO_ARTIFACTS FOLDER] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate] --n [ARTIFACT_NAME] (optional if artifactType is Base or TokenTemplate.");
					_log.Info(
						"--b [baseTokenType: 0 = fungible, 1 = non-fungible, 2 = hybrid-fungibleRoot, 3 = hybrid-non-fungibleRoot, 4 = hybrid-fungibleRoot-hybridChildren, 5 = hybrid-non-fungibleRoot-hybridChildren]");
					_log.Info(
						"--c [classification: 0 = fractionalFungible, 1 = wholeFungible, 2 = fractionalNonFungible, 3 = singleton] Is required for a Template Type");
					_log.Info(
						"To update the TaxonomyVersion: dotnet factgen --v [VERSION_STRING] --p [PATH_TO_ARTIFACTS FOLDER]");
					return;
				}

				_log.Error(
					"Required arguments --p [path-to-artifact folder] --n [artifactName] --t [artifactType: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate], (optional if artifactType is Base or TokenTemplate ");
				_log.Error(
					"--b [baseTokenType: 0 = fungible, 1 = non-fungible, 2 = hybrid-fungibleRoot, 3 = hybrid-non-fungibleRoot, 4 = hybrid-fungibleRoot-hybridChildren, 5 = hybrid-non-fungibleRoot-hybridChildren]");
				_log.Error(
					"To update the TaxonomyVersion: dotnet factgen --v [VERSION_STRING] --p [PATH_TO_ARTIFACTS FOLDER]");
				throw new Exception("Missing required parameters.");
			}

			_log.Info("Generating Artifact: " + ArtifactName + " of type: " + ArtifactType);

			var folderSeparator = "/";
			var fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (Os.IsWindows())
			{
				fullPath += "\\" + ArtifactPath + "\\";
				folderSeparator = "\\";
			}
			else
				fullPath += "/" + ArtifactPath + "/";

			Latest = folderSeparator + "latest";


			string artifactTypeFolder;
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));


			if (string.IsNullOrEmpty(ArtifactPath))
			{
				throw new Exception("Missing value for --p.");
			}

			if (TaxonomyVersion != null)
			{
				_log.Info("Updating Taxonomy Version Only.");
				var tx = new Taxonomy
				{
					Version = TaxonomyVersion.Version
				};
				var txVersionJson = jsf.Format(tx);

				var formattedTxJson = JToken.Parse(txVersionJson).ToString();

				//test to make sure formattedJson will Deserialize.
				try
				{
					JsonParser.Default.Parse<Taxonomy>(formattedTxJson);
					_log.Info("Taxonomy: " + TaxonomyVersion.Version + " successfully deserialized.");
				}
				catch (Exception e)
				{
					_log.Error("Json failed to deserialize back into Taxonomy");
					_log.Error(e);
					return;
				}

				_log.Info("Creating Taxonomy: " + formattedTxJson);
				var txStream = File.CreateText(fullPath + "Taxonomy.json");
				txStream.Write(formattedTxJson);
				txStream.Close();


				_log.Info("Creating Formula and Grammar Definitions");
				var formula = GenerateFormula();

				var formulaJson = jsf.Format(formula);

				var formattedFormulaJson = JToken.Parse(formulaJson).ToString();

				//test to make sure formattedJson will Deserialize.
				try
				{
					JsonParser.Default.Parse<FormulaGrammar>(formattedFormulaJson);
					_log.Info("FormulaGrammar successfully deserialized.");
				}
				catch (Exception e)
				{
					_log.Error("Json failed to deserialize back into FormulaGrammar.");
					_log.Error(e);
					return;
				}

				var formulaStream = File.CreateText(fullPath + "FormulaGrammar.json");
				formulaStream.Write(formattedFormulaJson);
				formulaStream.Close();

				return;
			}


			if (string.IsNullOrEmpty(ArtifactName))
			{
				throw new Exception("Missing value for  --n ");
			}

			if (!TemplateMode)
			{
				string artifactJson;
				DirectoryInfo outputFolder;
				var artifact = new Artifact
				{
					Name = ArtifactName,
					ArtifactSymbol = new ArtifactSymbol
					{
						Type = ArtifactType,
						ToolingSymbol = "",
						VisualSymbol = "",
						ArtifactVersion = "1.0"
					},
					ArtifactDefinition = new ArtifactDefinition
					{
						BusinessDescription = "This is a " + ArtifactName + " of type: " + ArtifactType,
						BusinessExample = "",
						Comments = "",
						Analogies =
						{
							new ArtifactAnalogy
							{
								Name = "Analogy 1",
								Description = ArtifactName + " analogy 1 description"
							}
						}
					},
					Maps = new Maps
					{
						CodeReferences =
						{
							new MapReference
							{
								MappingType = MappingType.SourceCode,
								Name = "Code 1",
								Platform = TargetPlatform.Daml,
								ReferencePath = ""
							}
						},
						ImplementationReferences =
						{
							new MapReference
							{
								MappingType = MappingType.Implementation,
								Name = "Implementation 1",
								Platform = TargetPlatform.ChaincodeGo,
								ReferencePath = ""
							}
						},
						Resources =
						{
							new MapResourceReference
							{
								MappingType = MappingType.Resource,
								Name = "Regulation Reference 1",
								Description = "",
								ResourcePath = ""
							}
						}
					},
					IncompatibleWithSymbols =
					{
						new ArtifactSymbol
						{
							Type = ArtifactType,
							ToolingSymbol = "",
							VisualSymbol = ""
						}
					},
					Dependencies =
					{
						new SymbolDependency
						{
							Description = "",
							Symbol = new ArtifactSymbol()
						}
					},
					Aliases = {"alias1", "alias2"}

				};
				switch (ArtifactType)
				{
					case ArtifactType.Base:
						var formula = GenerateFormula();
						artifactTypeFolder = "base";
						outputFolder =
							Directory.CreateDirectory(
								fullPath + artifactTypeFolder + folderSeparator + ArtifactName + Latest);
						var artifactBase = new Base
						{
							Artifact = AddArtifactFiles(outputFolder, artifactTypeFolder, folderSeparator,
								artifact, "Base"),
							TokenType = BaseType
						};
						artifactBase.Artifact.InfluencedBySymbols.Add(new SymbolInfluence
						{
							Description =
								"Whether or not the token class will be sub-dividable will influence the decimals value of this token. If it is non-sub-dividable, the decimals value should be 0.",
							Symbol = new ArtifactSymbol
							{
								Type = ArtifactType.Behavior,
								ToolingSymbol = "~d",
								VisualSymbol = "~d",
								ArtifactVersion = ""
							}
						});
						artifactBase.TokenType = BaseType;
						switch (BaseType)
						{
							case TokenType.Fungible:
								artifactBase.SingleToken = formula.SingleToken;
								break;
							case TokenType.NonFungible:
								artifactBase.SingleToken = formula.SingleToken;
								break;
							case TokenType.HybridFungibleRoot:
								artifactBase.Hybrid = formula.Hybrid;
								break;
							case TokenType.HybridNonFungibleRoot:
								artifactBase.Hybrid = formula.Hybrid;
								break;
							case TokenType.HybridFungibleRootHybridChildren:
								artifactBase.HybridWithHybrids = formula.HybridWithHybrids;
								break;
							case TokenType.HybridNonFungibleRootHybridChildren:
								artifactBase.HybridWithHybrids = formula.HybridWithHybrids;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						artifactJson = jsf.Format(artifactBase);
						break;
					case ArtifactType.Behavior:

						artifactTypeFolder = "behaviors";
						outputFolder =
							Directory.CreateDirectory(
								fullPath + artifactTypeFolder + folderSeparator + ArtifactName + Latest);
						var artifactBehavior = new Behavior
						{
							Artifact = AddArtifactFiles(outputFolder, artifactTypeFolder, folderSeparator,
								artifact, "Behaviors"),
							ConstructorName = "",
							Invocations =
							{
								new Invocation
								{
									Name = "InvocationRequest1",
									Description =
										"Describe the what the this invocation triggers in the behavior",
									Request = new InvocationRequest
									{
										ControlMessageName = "InvocationRequest",
										Description = "The request",
										InputParameters =
										{
											new InvocationParameter
											{
												Name = "Input Parameter 1",
												ValueDescription =
													"Contains some input data required for the invocation to work."
											}
										}
									},
									Response = new InvocationResponse
									{
										ControlMessageName = "InvocationResponse",
										Description = "The response",
										OutputParameters =
										{
											new InvocationParameter
											{
												Name = "Output Parameter 1",
												ValueDescription =
													"One of the values that the invocation should return."
											}
										}
									}
								}
							}
						};
						artifactBehavior.Properties.Add(new Property
						{
							Name = "Property1",
							ValueDescription = "Some Value",
							PropertyInvocations =
							{
								new Invocation
								{
									Name = "PropertyGetter",
									Description = "The property value.",
									Request = new InvocationRequest
									{
										ControlMessageName = "GetProperty1Request",
										Description = "",
										InputParameters =
										{
											new InvocationParameter
											{
												Name = "input1",
												ValueDescription = "value"
											}
										}
									},
									Response = new InvocationResponse
									{
										ControlMessageName = "GetProperty1Response",
										Description = "Return value",
										OutputParameters =
										{
											new InvocationParameter
											{
												Name = "Output1",
												ValueDescription = "value of property"
											}
										}
									}
								}
							}
						});
						artifactBehavior.Artifact.InfluencedBySymbols.Add(new SymbolInfluence
						{
							Description = "",
							Symbol = new ArtifactSymbol
							{
								ToolingSymbol = "",
								VisualSymbol = ""
							}
						});
						artifactBehavior.Artifact.Dependencies.Add(new SymbolDependency
						{
							Description = "",
							Symbol = new ArtifactSymbol
							{
								ToolingSymbol = "",
								VisualSymbol = ""
							}
						});
						artifact.IncompatibleWithSymbols.Add(new ArtifactSymbol());
						artifactJson = jsf.Format(artifactBehavior);
						break;
					case ArtifactType.BehaviorGroup:
						artifactTypeFolder = "behavior-groups";
						outputFolder =
							Directory.CreateDirectory(
								fullPath + artifactTypeFolder + folderSeparator + ArtifactName + Latest);
						var artifactBehaviorGroup = new BehaviorGroup
						{
							Artifact = AddArtifactFiles(outputFolder, artifactTypeFolder, folderSeparator,
								artifact, "BehaviorGroups")
						};
						artifactBehaviorGroup.BehaviorSymbols.Add(new ArtifactSymbol
						{
							ToolingSymbol = "<i>Symbol1</i>",
							VisualSymbol = "Symbol1"
						});
						artifactBehaviorGroup.BehaviorSymbols.Add(new ArtifactSymbol
						{
							ToolingSymbol = "<i>Symbol2</i>",
							VisualSymbol = "Symbol2"
						});
						artifactBehaviorGroup.BehaviorSymbols.Add(new ArtifactSymbol
						{
							ToolingSymbol = "<i>Symbol3<i>",
							VisualSymbol = "Symbol3"
						});
						artifactJson = jsf.Format(artifactBehaviorGroup);
						break;
					case ArtifactType.PropertySet:
						artifactTypeFolder = "property-sets";
						outputFolder =
							Directory.CreateDirectory(
								fullPath + artifactTypeFolder + folderSeparator + ArtifactName + Latest);
						var artifactPropertySet = new PropertySet
						{
							Artifact = AddArtifactFiles(outputFolder, artifactTypeFolder, folderSeparator,
								artifact, "PropertySets"),
							Properties =
							{
								new Property
								{
									Name = "Property1",
									ValueDescription =
										"This is the property required to be implemented and should be able to contain data of type X.",
									PropertyInvocations =
									{
										new Invocation
										{
											Name = "Property1 Getter",
											Description = "Request the value of the property",
											Request = new InvocationRequest
											{
												ControlMessageName = "GetProperty1Request",
												Description = "The request"
											},
											Response = new InvocationResponse
											{
												ControlMessageName = "GetProperty1Response",
												Description = "The response",
												OutputParameters =
												{
													new InvocationParameter
													{
														Name = "Property1.Value",
														ValueDescription =
															"Returning the value of the property."
													}
												}
											}
										},
										new Invocation
										{
											Name = "Property1 Setter",
											Description =
												"Set the Value of the Property, note if Roles should be applied to the Setter.",
											Request = new InvocationRequest
											{
												ControlMessageName = "SetProperty1Request",
												Description = "The request",
												InputParameters =
												{
													new InvocationParameter
													{
														Name = "New Value of Property",
														ValueDescription =
															"The data to set the property to."
													}
												}
											},
											Response = new InvocationResponse
											{
												ControlMessageName = "SetProperty1Response",
												Description = "The response",
												OutputParameters =
												{
													new InvocationParameter
													{
														Name = "Result, true or false",
														ValueDescription =
															"Returning the value of the set request."
													}
												}
											}
										}
									}
								}
							}
						};
						artifactPropertySet.Artifact.InfluencedBySymbols.Add(new SymbolInfluence
						{
							Description = "",
							Symbol = new ArtifactSymbol
							{
								ToolingSymbol = "",
								VisualSymbol = ""
							}
						});
						artifactJson = jsf.Format(artifactPropertySet);
						break;
					default:
						_log.Error("No matching type found for: " + ArtifactType);
						throw new ArgumentOutOfRangeException();
				}

				_log.Info("Artifact Destination: " + outputFolder);
				var formattedJson = JToken.Parse(artifactJson).ToString();

				//test to make sure formattedJson will Deserialize.
				try
				{
					switch (ArtifactType)
					{
						case ArtifactType.Base:

							var testBase = JsonParser.Default.Parse<Base>(formattedJson);
							_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
							          testBase.Artifact.Name);
							break;
						case ArtifactType.Behavior:
							var testBehavior = JsonParser.Default.Parse<Behavior>(formattedJson);
							_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
							          testBehavior.Artifact.Name);
							break;
						case ArtifactType.BehaviorGroup:
							var testBehaviorGroup = JsonParser.Default.Parse<BehaviorGroup>(formattedJson);
							_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
							          testBehaviorGroup.Artifact.Name);
							break;
						case ArtifactType.PropertySet:
							var testPropertySet = JsonParser.Default.Parse<PropertySet>(formattedJson);
							_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
							          testPropertySet.Artifact.Name);
							break;

						case ArtifactType.TokenTemplate:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception e)
				{
					_log.Error("Json failed to deserialize back into: " + ArtifactType);
					_log.Error(e);
					return;
				}

				_log.Info("Creating Artifact: " + formattedJson);
				var artifactStream =
					File.CreateText(outputFolder.FullName + folderSeparator + ArtifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();

				_log.Info("Complete");
			}
			else
			{
				//template mode
				artifactTypeFolder = "token-templates";
				var outputFolder =
					Directory.CreateDirectory(
						fullPath + artifactTypeFolder + folderSeparator + ArtifactName + Latest);

				var templateToken = new TokenTemplate();

				templateToken.Base = GetTokenTemplate(fullPath, folderSeparator);
				
				if (BaseType != TokenType.Fungible || BaseType != TokenType.NonFungible)
				{
					templateToken.ChildTokens.Add(GetTokenTemplate(fullPath, folderSeparator));
				}
				
				var artifactJson = jsf.Format(templateToken);
				var formattedJson = JToken.Parse(artifactJson).ToString();
				
				try
				{
					var testTemplate = JsonParser.Default.Parse<TokenTemplate>(formattedJson);
					_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
					          testTemplate.Base.Name);
				}
				catch (Exception e)
				{
					_log.Error("Json failed to deserialize back into: " + ArtifactType);
					_log.Error(e);
					return;
				}

				_log.Info("Creating Template: " + formattedJson);
				var artifactStream =
					File.CreateText(outputFolder.FullName + folderSeparator + ArtifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();

				_log.Info("Complete");
			}
		}

		private static TemplateToken GetTokenTemplate(string fullPath, string folderSeparator)
		{
			return new TemplateToken
			{
				Definition = AddDefinition(),
				Base = GetTokenTypeBase(fullPath, folderSeparator),
				Behaviors =
				{
					new TemplateBehavior
					{
						ImplementationDetails = "",
						Behavior = new Behavior(),
						Symbol = new ArtifactSymbol
						{
							Type = ArtifactType.Behavior
						}
							
					}
				},
				BehaviorGroups =
				{
					new TemplateBehaviorGroup
					{
						BehaviorGroup = new BehaviorGroup(),
						ImplementationDetails = "",
						Symbol = new ArtifactSymbol
						{
							Type = ArtifactType.BehaviorGroup
						}
					}
				},
				PropertySets =
				{
					new TemplatePropertySet
					{
						PropertySet = new PropertySet(),
						ImplementationDetails = "",
						Symbol = new ArtifactSymbol
						{
							Type = ArtifactType.PropertySet
						}
					}
				}
			};
		}

		private static FormulaGrammar GenerateFormula()
		{
			var formula = new FormulaGrammar();

			var singleToken = new SingleToken
			{
				BaseToken = new TokenBase
				{
					ArtifactSymbol = new ArtifactSymbol()
				},
				Behaviors = new BehaviorList
				{
					ListStart = "{",
					BehaviorSymbols = { new ArtifactSymbol
					{
						Type = ArtifactType.Behavior
					}},
					ListEnd = "}"
				},
				GroupStart = "[",
				GroupEnd = "]"
			};
			
			var psli = new PropertySetListItem
			{
				ListStart = "+",
				PropertySetSymbols = new ArtifactSymbol
				{
					Type = ArtifactType.PropertySet
				}
			};
			singleToken.PropertySets.Add(psli);

			formula.SingleToken = singleToken;

			var hybrid = new HybridTokenFormula
			{
				ChildrenStart = "(",
				ChildrenEnd = ")",
				Parent = singleToken
			};
			hybrid.ChildTokens.Add(singleToken);
			hybrid.ChildTokens.Add(singleToken);

			formula.Hybrid = hybrid;
			
			var hybridHybrids = new HybridTokenWithHybridChildrenFormula
			{
				HybridChildrenStart = "(",
				HybridChildrenEnd = ")",
				Parent = singleToken
			};
			hybridHybrids.HybridChildTokens.Add(hybrid);
			hybridHybrids.HybridChildTokens.Add(hybrid);
			formula.HybridWithHybrids = hybridHybrids;
			
			
			return formula;
		}

		private static Base GetTokenTypeBase(string fullPath,  string folderSeparator)
		{
			string baseName;
			const string typeFolder = "base";
			
			switch (BaseType)
			{
				case TokenType.Fungible:
					baseName = "fungible";
					break;
				case TokenType.NonFungible:
					baseName = "non-fungible";
					break;
				case TokenType.HybridFungibleRoot:
					baseName = "hybrid-fungibleRoot";
					break;
				case TokenType.HybridNonFungibleRoot:
					baseName = "hybrid-non-fungibleRoot";
					break;
				case TokenType.HybridFungibleRootHybridChildren:
					baseName = "hybrid-non-fungibleRoot-hybridChildren";
					break;
				case TokenType.HybridNonFungibleRootHybridChildren:
					baseName = "hybrid-non-fungibleRoot-hybridChildren";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			var baseFile = File.OpenText(fullPath + typeFolder + folderSeparator + baseName + folderSeparator + baseName+".json");
			var json = baseFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			var baseType = JsonParser.Default.Parse<Base>(formattedJson);
			
			//setting classification
			var classified = SetClassification(baseType);

			return classified;
		}

		private static Base SetClassification(Base baseToken)
		{
			var cloneBase = baseToken.Clone();

			var artifactSymbol = new ArtifactSymbol
			{
				Type = ArtifactType.Base
			};
			switch (BaseType)
			{

				case TokenType.Fungible:
					artifactSymbol.ToolingSymbol = "tF";
					artifactSymbol.VisualSymbol = "&tau;<sub>F</sub>";
					break;
				case TokenType.NonFungible:
					artifactSymbol.ToolingSymbol = "tN";
					artifactSymbol.VisualSymbol = "&tau;<sub>N</sub>";
					break;
				case TokenType.HybridFungibleRoot:
					artifactSymbol.ToolingSymbol = "tF()";
					artifactSymbol.VisualSymbol = "&tau;<sub>F</sub>()";
					break;
				case TokenType.HybridNonFungibleRoot:
					artifactSymbol.ToolingSymbol = "tN()";
					artifactSymbol.VisualSymbol = "&tau;<sub>N</sub>()";
					break;
				case TokenType.HybridFungibleRootHybridChildren:
					artifactSymbol.ToolingSymbol = "tF(t())";
					artifactSymbol.VisualSymbol = "&tau;<sub>F</sub>(t())";
					break;
				case TokenType.HybridNonFungibleRootHybridChildren:
					artifactSymbol.ToolingSymbol = "tN(t())";
					artifactSymbol.VisualSymbol = "&tau;<sub>N</sub>(t())";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (baseToken.TokenFormulaCase)
			{
				case Base.TokenFormulaOneofCase.SingleToken:
					cloneBase.SingleToken.ClassificationBranch = Classification;
					cloneBase.SingleToken.Behaviors = GetDividable(baseToken.SingleToken.Behaviors);
					break;
				case Base.TokenFormulaOneofCase.Hybrid:
					cloneBase.Hybrid.Parent.ClassificationBranch = Classification;
					cloneBase.Hybrid.Parent.Behaviors = GetDividable(baseToken.SingleToken.Behaviors);
					break;
				case Base.TokenFormulaOneofCase.HybridWithHybrids:
					cloneBase.HybridWithHybrids.Parent.ClassificationBranch = Classification;
					cloneBase.HybridWithHybrids.Parent.Behaviors = GetDividable(baseToken.SingleToken.Behaviors);
					break;
				case Base.TokenFormulaOneofCase.None:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return cloneBase;
		}

		private static BehaviorList GetDividable(BehaviorList list)
		{
			var retVal = list.Clone();
			if (Classification == ClassificationBranch.Singleton ||
			    Classification == ClassificationBranch.WholeFungible)
			{
				var notSub =
					list.BehaviorSymbols.SingleOrDefault(e => e.ToolingSymbol == "~d");
				if (notSub == null)
				{
					retVal.BehaviorSymbols.Add(new ArtifactSymbol
					{
						ToolingSymbol = "~d"
					});
				}
			}
			else
			{
				var sub =
					list.BehaviorSymbols.SingleOrDefault(e =>
						e.ToolingSymbol == "d");
				if (sub == null)
				{
					retVal.BehaviorSymbols.Add(new ArtifactSymbol
					{
						ToolingSymbol = "d"
					});
				}

			}

			return retVal;
		}

		//for templates only
		private static ArtifactDefinition AddDefinition()
		{
			
			return new ArtifactDefinition
			{
				BusinessDescription = "Template Token Description",
				BusinessExample = "Template Token Example",
				Comments = "template comments"
			};
			
		}
		
		private static Artifact AddArtifactFiles(DirectoryInfo outputFolder, string typeFolder, string folderSeparator, Artifact parent, string nameSpaceAdd)
		{
			var md = CreateMarkdown(outputFolder, folderSeparator);
			var proto = CreateProto(outputFolder, folderSeparator, nameSpaceAdd);
			var retArtifact = parent.Clone();
			
			retArtifact.ArtifactFiles.Add(proto);
			retArtifact.ControlUri = ArtifactPath + folderSeparator + typeFolder + folderSeparator + ArtifactName + folderSeparator + Latest + proto.FileName;
			retArtifact.ArtifactFiles.Add(md);
			return retArtifact;
		}
		private static ArtifactFile CreateMarkdown(DirectoryInfo outputFolder, string folderSeparator)
		{
			_log.Info("Creating Artifact Markdown");
			var mdFile = outputFolder + folderSeparator + ArtifactName + ".md";
			var t = File.Exists(mdFile);
			if (t)
				return new ArtifactFile
				{
					FileName = ArtifactName + ".md",
					Content = ArtifactContent.Uml
				};
			var md = File.CreateText(outputFolder + folderSeparator + ArtifactName + ".md");
			md.Write("# " + ArtifactName + " a TTF " + ArtifactType);
			md.Close();

			return new ArtifactFile
			{
				FileName = ArtifactName + ".md",
				Content = ArtifactContent.Uml
			};
		}
		
		private static ArtifactFile CreateProto(DirectoryInfo outputFolder, string folderSeparator, string nameSpaceAdd)
		{
			_log.Info("Creating Artifact Proto");
			var pFile = outputFolder + folderSeparator + ArtifactName + ".proto";
			var t = File.Exists(pFile);
			if (t)
				return new ArtifactFile
				{
					FileName = ArtifactName + ".proto",
					Content = ArtifactContent.Control
				};
			var proto  = File.CreateText(pFile);
			var templateProto =
				File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
				                 folderSeparator + "templates" + folderSeparator + "artifact.proto");
			
			var ns = templateProto.Replace("BASE", nameSpaceAdd);
			ns = ns.Replace("NAME", ArtifactName);
			proto.Write(ns.Replace("ARTIFACT", ArtifactName));
			proto.Close();
			return new ArtifactFile
			{
				FileName = ArtifactName + ".proto",
				Content = ArtifactContent.Control
			};
		}
		
	}
}