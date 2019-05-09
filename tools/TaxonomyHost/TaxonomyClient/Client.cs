using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf.Collections;
using Grpc.Core;
using log4net;
using Microsoft.Extensions.Configuration;
using TTI.TTF.Model.Artifact;
using TTI.TTF.Model.Core;
using TTI.TTF.Taxonomy;

namespace TaxonomyClient
{
	public static class Client
	{
		private static IConfigurationRoot _config;
		private static ILog _log;
		private static string _gRpcHost;
		private static int _gRpcPort;

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

			if (args.Length == 1||args.Length == 4)
			{
				_gRpcHost = _config["gRpcHost"];
				_gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
			
				_log.Info("Connection to TaxonomyService: " + _gRpcHost + " port: " + _gRpcPort);
				var taxonomyClient = new TaxonomyService.TaxonomyServiceClient(
					new Channel(_gRpcHost, _gRpcPort, ChannelCredentials.Insecure));
				if (args.Length == 1)
				{
					if (args[0] == "--f")
					{
						GetFullTaxonomy(taxonomyClient);
						return;
					}

					OutputUsage();
					return;
				}
				
				for (var i = 0; i < args.Length; i++)
				{
					var arg = args[i];
					switch (arg)
					{
						case "--s":
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
					Console.WriteLine("Missing Symbol for Taxonomy Query, include --s [TOOLING_SYMBOL]");
					return;
				}

				if (!artifactSet)
				{
					Console.WriteLine("Missing Artifact Type, include --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
					return;
				}

				var symbolQuery = new Symbol
				{
					ArtifactSymbol = symbol
				};
				
				switch (artifactType)
				{
					case ArtifactType.Base:
						var baseType = taxonomyClient.GetBaseArtifact(symbolQuery);
						OutputBaseType(symbol, baseType);
						return;
					case ArtifactType.Behavior:
						var behavior = taxonomyClient.GetBehaviorArtifact(symbolQuery);
						OutputBehavior(symbol, behavior);
						return;
					case ArtifactType.BehaviorGroup:
						var behaviorGroup = taxonomyClient.GetBehaviorGroupArtifact(symbolQuery);
						OutputBehaviorGroup(symbol, behaviorGroup);
						return;
					case ArtifactType.PropertySet:
						var propertySet = taxonomyClient.GetPropertySetArtifact(symbolQuery);
						OutputPropertySet(symbol, propertySet);
						return;
					case ArtifactType.TokenTemplate:
						var tokenTemplate = taxonomyClient.GetTokenTemplateArtifact(new TaxonomyFormula
						{
							Formula = symbol
						});
						OutputTemplate(symbol, tokenTemplate);
						return;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			OutputUsage();
		}

		private static void OutputUsage()
		{
			
				Console.WriteLine(
					"Usage: dotnet TaxonomyClient --f");
				Console.WriteLine("	Retrieves the entire Taxonomy and writes it to the console.");
				Console.WriteLine(
					"Usage: dotnet TaxonomyClient --s [TOOLING_SYMBOL] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate]");
				Console.WriteLine("	Retrieves the Taxonomy Artifact by Tooling Symbol and writes it to the console.");

		}

		private static void GetFullTaxonomy(TaxonomyService.TaxonomyServiceClient taxonomyClient)
		{
			_log.Info("Fetching 1.0 version of the Taxonomy");

			var taxonomy = taxonomyClient.GetFullTaxonomy(new TaxonomyVersion {Version = "1.0"});

			_log.Info("-----------------------------Taxonomy Start---------------------------------------");
			_log.Info("Taxonomy Version: " + taxonomy.Version);
			_log.Info("Taxonomy Base Token Types Count: " + taxonomy.BaseTokenTypes.Count);
			_log.Info("Taxonomy Behaviors Count: " + taxonomy.Behaviors.Count);
			_log.Info("Taxonomy BehaviorGroups Count: " + taxonomy.BehaviorGroups.Count);
			_log.Info("Taxonomy PropertySet Count: " + taxonomy.PropertySets.Count);
			_log.Info("Taxonomy TokenTemplate Count: " + taxonomy.TokenTemplates.Count);
			_log.Info("-> Base Token Types <-");
			foreach (var (symbol, baseType) in taxonomy.BaseTokenTypes)
			{
				OutputBaseType(symbol, baseType);
			}

			_log.Info("-> Base Token Types End <-");
			_log.Info("-> Behaviors <-");
			foreach (var (symbol, behavior) in taxonomy.Behaviors)
			{
				OutputBehavior(symbol, behavior);
			}

			_log.Info("-> Behaviors End <-");
			_log.Info("-> BehaviorGroups <-");

			foreach (var (symbol, bg) in taxonomy.BehaviorGroups)
			{
				OutputBehaviorGroup(symbol, bg);
			}

			_log.Info("-> BehaviorGroups End <-");

			_log.Info("-> PropertySets <-");
			foreach (var (symbol, propSet) in taxonomy.PropertySets)
			{
				OutputPropertySet(symbol, propSet);
			}

			_log.Info("-> PropertySets End <-");
			_log.Info("-> TokenTemplates <-");
			foreach (var (symbol, value) in taxonomy.TokenTemplates)
			{
				OutputTemplate(symbol, value);
			}

			_log.Info("-> TokenTemplates End <-");
			_log.Info("-----------------------------Taxonomy   End----------------------------------------");
		}

		private static void OutputPropertySet(string symbol, PropertySet propSet)
		{
			_log.Info("***PropertySet***");
			_log.Info(symbol);
			OutputArtifact(propSet.Artifact);
			foreach (var prop in propSet.Properties)
			{
				_log.Info("***Non-Behavioral Property***");
				_log.Info(prop.Name);
				_log.Info(prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				_log.Info("***Non-Behavioral Property End***");
			}

			_log.Info("***PropertySet End***");
		}

		private static void OutputInvocations(IEnumerable<Invocation> propPropertyInvocations)
		{
			foreach (var i in propPropertyInvocations)
			{
				_log.Info("***PropertyInvocation***");
				_log.Info(i.Name);
				_log.Info(i.Description);
				OutputInvocationRequest(i.Request);
				OutputInvocationResponse(i.Response);
				_log.Info("***PropertyInvocation End***");
			}
		}

		private static void OutputInvocationResponse(InvocationResponse iResponse)
		{
			_log.Info("[Response]");
			_log.Info(iResponse.Description);
			_log.Info(iResponse.ControlMessageName);
			_log.Info(" [Output] ");
			foreach (var p in iResponse.OutputParameters)
			{
				_log.Info("  [Parameter] ");
				_log.Info("   -Parameter Name: " + p.Name);
				_log.Info("   -Parameter Value Description: " + p.ValueDescription);
			}
		}

		private static void OutputInvocationRequest(InvocationRequest iRequest)
		{
			_log.Info("[Request]");
			_log.Info(iRequest.Description);
			_log.Info(iRequest.ControlMessageName);
			_log.Info(" [Input] ");
			foreach (var p in iRequest.InputParameters)
			{
				_log.Info("  [Parameter] ");
				_log.Info("   -Parameter Name: " + p.Name);
				_log.Info("   -Parameter Value Description: " + p.ValueDescription);
			}
		}

		private static void OutputBehaviorGroup(string symbol, BehaviorGroup bg)
		{
			_log.Info("***BehaviorGroup***");
			_log.Info(symbol);
			OutputArtifact(bg.Artifact);
			OutputBehaviorSymbols(bg.BehaviorSymbols);
			_log.Info("***BehaviorGroup End***");
		}

		private static void OutputBehaviorSymbols(IEnumerable<ArtifactSymbol> bgBehaviorSymbols)
		{
			_log.Info("***Behavior Symbols***");
			foreach (var s in bgBehaviorSymbols)
			{
				_log.Info("*Behavior Symbol*");
				_log.Info(s);
				_log.Info("*Behavior Symbol End*");
			}
			_log.Info("***Behavior Symbols End***");
		}

		private static void OutputBehavior(string symbol, Behavior behavior)
		{
			_log.Info("***Behavior***");
			_log.Info(symbol);
			OutputArtifact(behavior.Artifact);
			_log.Info(behavior.IsExternal);
			_log.Info(behavior.ConstructorName);
			_log.Info(behavior.Invocations);
			_log.Info(behavior.Properties);
			_log.Info("***Behavior End***");
		}

		private static void OutputBaseType(string symbol, Base baseType)
		{
			_log.Info("***Base Token Type***");
			_log.Info("Symbol: " + symbol);
			_log.Info("Name: " + baseType.Name);
			_log.Info("Type: " + baseType.TokenType);
			_log.Info("Details:");
			OutputArtifact(baseType.Artifact);
			_log.Info("Formula:");
			_log.Info(baseType.TokenFormulaCase);
			_log.Info("Constructor: " + baseType.ConstructorName);
			_log.Info("TokenSymbol: " + baseType.Symbol);
			_log.Info("Owner: " + baseType.Owner);
			_log.Info("Decimals: " + baseType.Decimals);
			_log.Info("Quantity: " + baseType.Quantity);
			_log.Info("***Properties***");
			foreach (var tp in baseType.TokenProperties)
			{
				_log.Info("  [Properties]");
				_log.Info("   " + tp);
			}

			_log.Info("***PropertiesEnd***");
			_log.Info("***Children***");
			foreach (var c in baseType.ChildTokens)
			{
				_log.Info("***Child Token***");
				OutputTemplate(c.Artifact.ArtifactSymbol.ToolingSymbol, c);
				_log.Info("***Child Token End***");
			}

			_log.Info("***ChildrenEnd***");
			_log.Info("***Base Token Type End***");
		}

		private static void OutputTemplate(string symbol, TokenTemplate template)
		{
			_log.Info("***TokenTemplate***");
			_log.Info("Formula: " + symbol);
			OutputArtifact(template.Artifact);
			OutputBaseType(template.Base.Artifact.ArtifactSymbol.ToolingSymbol, template.Base);
			_log.Info("***Behaviors***");
			foreach (var b in template.Behaviors)
			{
				OutputBehavior(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Info("***Behaviors End***");
			_log.Info("***BehaviorGroups ***");
			foreach (var b in template.BehaviorGroups)
			{
				OutputBehaviorGroup(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Info("***BehaviorGroups End***");
			_log.Info("***PropertySets***");
			foreach (var b in template.PropertySets)
			{
				OutputPropertySet(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Info("***PropertySets End***");
			_log.Info("***TokenTemplate End***");
		}

	
		private static void OutputArtifact(Artifact artifact)
		{
			_log.Info("Details:");
			_log.Info(artifact.Name);
			_log.Info(artifact.Type);
			_log.Info(artifact.ArtifactSymbol);
			_log.Info(artifact.Aliases);
			_log.Info(artifact.ArtifactDefinition);
			_log.Info(artifact.ControlUri);
			_log.Info(artifact.InfluencedBySymbols);
			_log.Info(artifact.IncompatibleWithSymbols);
			foreach (var f in artifact.ArtifactFiles)
			{
				_log.Info(f.Content);
				_log.Info(f.FileName);
				if(f.Content!=ArtifactContent.Other)
					_log.Info(f.FileData.ToStringUtf8());
			}
			_log.Info(artifact.Maps);
		}
	}
}