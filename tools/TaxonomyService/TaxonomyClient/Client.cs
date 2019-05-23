using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using log4net;
using Microsoft.Extensions.Configuration;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy
{
	public static class Client
	{
		private static IConfigurationRoot _config;
		private static ILog _log;
		private static string _gRpcHost;
		private static int _gRpcPort;
		internal static TaxonomyService.TaxonomyServiceClient TaxonomyClient;
		private static bool _saveArtifact;
		
		private static void Main(string[] args)
		{
			#region config

			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables()
				.Build();

			#endregion

			#region logging

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

			#endregion

			var symbol = "";
			var artifactType = ArtifactType.Base;
			var artifactSet = false;
			var create = false;
			var newArtifactName = "";
			var newSymbol = "";

			if (args.Length > 0 &&  args.Length < 7 )
			{
				_gRpcHost = _config["gRpcHost"];
				_gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
			
				_log.Info("Connection to TaxonomyService: " + _gRpcHost + " port: " + _gRpcPort);
				TaxonomyClient = new TaxonomyService.TaxonomyServiceClient(
					new Channel(_gRpcHost, _gRpcPort, ChannelCredentials.Insecure));
				switch (args.Length)
				{
					case 1 when args[0] == "--f":
						GetFullTaxonomy(TaxonomyClient);
						return;
					case 1:
						OutputUsage();
						return;
					case 2:
						OutputUsage();
						return;
					case 3:
						OutputUsage();
						return;
					case 4:
						var artifactFolder = "";
						for (var i = 0; i < args.Length; i++)
						{
							switch (args[i])
							{
								case "--u":
									i++;
									artifactFolder = args[i];
									continue;
								case "--t":
									i++;
									var t = Convert.ToInt32(args[i]);
									artifactType = (ArtifactType) t;
									artifactSet = true;
									continue;
								default:
									continue;
							}
						}
						if(artifactSet && !string.IsNullOrEmpty(artifactFolder))
							UpdateArtifact(artifactType, artifactFolder);
						else
							GetArtifact(args, symbol, artifactType);
						return;
					case 5:
						foreach (var t in args)
						{
							switch (t)
							{
								case "--s":
									_saveArtifact = true;
									GetArtifact(args, symbol, artifactType);
									return;
								
								default:
									continue;
							}
						}
						OutputUsage();
						return;
					case 6:
					{
						for (var i = 0; i < args.Length; i++)
						{
							var arg = args[i];
							switch (arg)
							{
								case "--c":
									i++;
									create = true;
									newSymbol = args[1];
									continue;
								case "--t":
									i++;
									var t = Convert.ToInt32(args[i]);
									artifactType = (ArtifactType) t;
									artifactSet = true;
									continue;
								case "--n":
									i++;
									newArtifactName = args[i];
									continue;
								default:
									continue;
							}
						}

						if (string.IsNullOrEmpty(newArtifactName))
						{
							Console.WriteLine("Missing New Artifact Name, include --n [NEW_ARTIFACT_NAME]");
							return;
						}
						
						if (string.IsNullOrEmpty(newSymbol))
						{
							Console.WriteLine("Missing New Artifact Symbol, include --c [NEW_ARTIFACT_SYMBOL]");
							return;
						}

						if (!artifactSet)
						{
							Console.WriteLine(
								"Missing New Artifact Type, include --ts [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
							return;
						}
						
						if (!create)
						{
							Console.WriteLine(
								"Missing Create Switch, include --c to create a new artifact.");
							return;
						}

						if (artifactType == ArtifactType.Base)
						{
							Console.WriteLine("This tool does not support creating a new base token type or --t 0");
							return;
						}
						
						var newArtifactRequest = new NewArtifactRequest
						{
							Type = artifactType
						};
						
						Any newType;
						switch (artifactType)
						{
							case ArtifactType.Base:
								return;
							case ArtifactType.Behavior:
								var newBehavior = new Behavior
								{
									Artifact = new Artifact
									{
										Name = newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											ToolingSymbol = newSymbol.ToLower(),
											VisualSymbol = "<i>" + newSymbol.ToLower()+"</i>"
										}
									}
								};
								newType = Any.Pack(newBehavior);
								break;
							case ArtifactType.BehaviorGroup:
								var newBehaviorGroup = new BehaviorGroup
								{
									Artifact = new Artifact
									{
										Name = newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											ToolingSymbol = newSymbol.ToUpper(),
											VisualSymbol = "<i>" + newSymbol.ToUpper()+"</i>"
										}
									}
								};
								newType = Any.Pack(newBehaviorGroup);
								break;
							case ArtifactType.PropertySet:
								var newPropertySet = new PropertySet
								{
									Artifact = new Artifact
									{
										Name = newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											ToolingSymbol = "ph" + newSymbol,
											VisualSymbol = "&phi;" + newSymbol
										}
									}
								};
								newType = Any.Pack(newPropertySet);
								break;
							case ArtifactType.TokenTemplate:
								var newTokenTemplate = new TokenTemplate
								{
									Artifact = new Artifact
									{
										Name = newArtifactName,
										ArtifactSymbol = new ArtifactSymbol
										{
											ToolingSymbol = newSymbol,
											VisualSymbol = newSymbol
										}
									}
								};
								newType = Any.Pack(newTokenTemplate);
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						newArtifactRequest.Artifact = newType;
						
						_log.Error("Create New Artifact of Type: " + artifactType);
						if (artifactType == ArtifactType.TokenTemplate)
						{
							_log.Warn("	& Token Tooling Formula = " + symbol);
						}
						else
						{
							_log.Warn("	& Tooling Symbol" + symbol);
						}
						_log.Info("-----------------------------------------");

						var newArtifactResponse = TaxonomyClient.CreateArtifact(newArtifactRequest); 
						_log.Error("New Artifact Response");
						switch (newArtifactResponse.Type)
						{
							case ArtifactType.Base:
								break;
							case ArtifactType.Behavior:
								var newBehavior = newArtifactResponse.ArtifactTypeObject.Unpack<Behavior>();
								OutputLib.OutputBehavior(newBehavior.Artifact.ArtifactSymbol.ToolingSymbol, newBehavior);
								return;
							case ArtifactType.BehaviorGroup:
								var newBehaviorGroup = newArtifactResponse.ArtifactTypeObject.Unpack<BehaviorGroup>();
								OutputLib.OutputBehaviorGroup(newBehaviorGroup.Artifact.ArtifactSymbol.ToolingSymbol, newBehaviorGroup);
								return;
							case ArtifactType.PropertySet:
								var newPropertySet = newArtifactResponse.ArtifactTypeObject.Unpack<PropertySet>();
								OutputLib.OutputPropertySet(newPropertySet.Artifact.ArtifactSymbol.ToolingSymbol, newPropertySet);
								return;
							case ArtifactType.TokenTemplate:
								var newTokenTemplate = newArtifactResponse.ArtifactTypeObject.Unpack<TokenTemplate>();
								OutputLib.OutputTemplate(newTokenTemplate.Artifact.ArtifactSymbol.ToolingSymbol, newTokenTemplate);
								return;
							default:
								throw new ArgumentOutOfRangeException();
						}
						return;
					}
					default:
						OutputUsage();
						return;
				}
			}
			OutputUsage();
		}

		private static void UpdateArtifact(ArtifactType type, string updateFolder)
		{
			OutputLib.UpdateArtifact(type, updateFolder);
		}

		private static void OutputUsage()
		{
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --f");
			Console.WriteLine("	Retrieves the entire Taxonomy and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --ts [TOOLING_SYMBOL] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Retrieves the Taxonomy Artifact by Tooling Symbol and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --ts [TOOLING_SYMBOL] --s --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Retrieves the Taxonomy Artifact by Tooling Symbol and SAVES it locally in a folder and writes it to the console.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --u [UPDATE_ARTIFACT_FOLDER] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Updates an artifact edited locally if updated from a saved query using --s.");
			Console.WriteLine(
				"Usage: dotnet TaxonomyClient --c [NEW_ARTIFACT_SYMBOL] --n [NEW_ARTIFACT_NAME] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
			Console.WriteLine("	 Creates a new Taxonomy Artifact and returns it after creation.");
		}

		private static void GetFullTaxonomy(TaxonomyService.TaxonomyServiceClient taxonomyClient)
		{
			_log.Warn("Fetching 1.0 version of the Taxonomy");
			Model.Taxonomy taxonomy;
			try
			{
				taxonomy = taxonomyClient.GetFullTaxonomy(new TaxonomyVersion {Version = "1.0"});
			}
			catch (Exception e)
			{
				_log.Error(e.Message);
				_log.Error("Cannot connect to Taxonomy Service: " + _gRpcHost + " on port: " +_gRpcPort);
				return;
			}

			_log.Warn("-----------------------------Taxonomy Start---------------------------------------");
			_log.Info("-Taxonomy Version: " + taxonomy.Version);
			_log.Info("-Taxonomy Base Token Types Count: " + taxonomy.BaseTokenTypes.Count);
			_log.Info("-Taxonomy Behaviors Count: " + taxonomy.Behaviors.Count);
			_log.Info("-Taxonomy BehaviorGroups Count: " + taxonomy.BehaviorGroups.Count);
			_log.Info("-Taxonomy PropertySet Count: " + taxonomy.PropertySets.Count);
			_log.Info("-Taxonomy TokenTemplate Count: " + taxonomy.TokenTemplates.Count);
			_log.Error("-> Base Token Types <-");
			foreach (var (symbol, baseType) in taxonomy.BaseTokenTypes)
			{
				OutputLib.OutputBaseType(symbol, baseType);
			}

			_log.Error("-> Base Token Types End <-");
			_log.Warn("-> Behaviors <-");
			foreach (var (symbol, behavior) in taxonomy.Behaviors)
			{
				OutputLib.OutputBehavior(symbol, behavior);
			}

			_log.Warn("-> Behaviors End <-");
			_log.Error("-> BehaviorGroups <-");

			foreach (var (symbol, bg) in taxonomy.BehaviorGroups)
			{
				OutputLib.OutputBehaviorGroup(symbol, bg);
			}

			_log.Error("-> BehaviorGroups End <-");

			_log.Warn("-> PropertySets <-");
			foreach (var (symbol, propSet) in taxonomy.PropertySets)
			{
				OutputLib.OutputPropertySet(symbol, propSet);
			}

			_log.Warn("-> PropertySets End <-");
			_log.Error("-> TokenTemplates <-");
			foreach (var (symbol, value) in taxonomy.TokenTemplates)
			{
				OutputLib.OutputTemplate(symbol, value);
			}

			_log.Warn("-> TokenTemplates End <-");
			_log.Warn("-----------------------------Taxonomy   End----------------------------------------");
		}
		
		private static void GetArtifact(IReadOnlyList<string> args, string symbol, ArtifactType artifactType)
		{
			var artifactSet = false;
			for (var i = 0; i < args.Count; i++)
			{
				var arg = args[i];
				switch (arg)
				{
					case "--ts":
						i++;
						symbol = args[i];
						continue;
					case "--t":
						i++;
						var t = Convert.ToInt32(args[i]);
						artifactType = (ArtifactType) t;
						artifactSet = true;
						continue;
					default:
						continue;
				}
			}

			if (string.IsNullOrEmpty(symbol))
			{
				Console.WriteLine("Missing Symbol for Taxonomy Query, include --ts [TOOLING_SYMBOL]");
				return;
			}

			if (!artifactSet)
			{
				Console.WriteLine(
					"Missing Artifact Type, include --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
				return;
			}

			var symbolQuery = new ArtifactSymbol
			{
				ToolingSymbol = symbol
			};

			_log.Error("Taxonomy Artifact Symbol Query for Type: " + artifactType);
			if (artifactType == ArtifactType.TokenTemplate)
			{
				_log.Warn("	& Token Tooling Formula = " + symbol);
			}
			else
			{
				_log.Warn("	& Tooling Symbol" + symbol);
			}

			_log.Info("-----------------------------------------");
			
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));

			switch (artifactType)
			{
				case ArtifactType.Base:
					var baseType = TaxonomyClient.GetBaseArtifact(symbolQuery);
					OutputLib.OutputBaseType(symbol, baseType);
					if (!_saveArtifact) return;
					var typedJson = jsf.Format(baseType);
					OutputLib.SaveArtifact(artifactType, baseType.Artifact.Name, typedJson);
					return;
				case ArtifactType.Behavior:
					var behavior = TaxonomyClient.GetBehaviorArtifact(symbolQuery);
					OutputLib.OutputBehavior(symbol, behavior);
					if (!_saveArtifact) return;
					var typedBehaviorJson = jsf.Format(behavior);
					OutputLib.SaveArtifact(artifactType, behavior.Artifact.Name, typedBehaviorJson);
					return;
				case ArtifactType.BehaviorGroup:
					var behaviorGroup = TaxonomyClient.GetBehaviorGroupArtifact(symbolQuery);
					OutputLib.OutputBehaviorGroup(symbol, behaviorGroup);
					if (!_saveArtifact) return;
					var typedBehaviorGroupJson = jsf.Format(behaviorGroup);
					OutputLib.SaveArtifact(artifactType, behaviorGroup.Artifact.Name, typedBehaviorGroupJson);
					return;
				case ArtifactType.PropertySet:
					var propertySet = TaxonomyClient.GetPropertySetArtifact(symbolQuery);
					OutputLib.OutputPropertySet(symbol, propertySet);
					if (!_saveArtifact) return;
					var typedPropertySetJson = jsf.Format(propertySet);
					OutputLib.SaveArtifact(artifactType, propertySet.Artifact.Name, typedPropertySetJson);
					return;
				case ArtifactType.TokenTemplate:
					var tokenTemplate = TaxonomyClient.GetTokenTemplateArtifact(new TaxonomyFormula
					{
						Formula = symbol
					});
					OutputLib.OutputTemplate(symbol, tokenTemplate);
					if (!_saveArtifact) return;
					var typedTemplateJson = jsf.Format(tokenTemplate);
					OutputLib.SaveArtifact(artifactType, tokenTemplate.Artifact.Name, typedTemplateJson);
					return;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

	}
}