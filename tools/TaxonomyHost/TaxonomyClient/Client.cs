using System;
using System.Collections.Generic;
using System.Reflection;
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
			_log.Warn("Fetching 1.0 version of the Taxonomy");

			var taxonomy = taxonomyClient.GetFullTaxonomy(new TaxonomyVersion {Version = "1.0"});

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
				OutputBaseType(symbol, baseType);
			}

			_log.Error("-> Base Token Types End <-");
			_log.Warn("-> Behaviors <-");
			foreach (var (symbol, behavior) in taxonomy.Behaviors)
			{
				OutputBehavior(symbol, behavior);
			}

			_log.Warn("-> Behaviors End <-");
			_log.Error("-> BehaviorGroups <-");

			foreach (var (symbol, bg) in taxonomy.BehaviorGroups)
			{
				OutputBehaviorGroup(symbol, bg);
			}

			_log.Error("-> BehaviorGroups End <-");

			_log.Warn("-> PropertySets <-");
			foreach (var (symbol, propSet) in taxonomy.PropertySets)
			{
				OutputPropertySet(symbol, propSet);
			}

			_log.Warn("-> PropertySets End <-");
			_log.Error("-> TokenTemplates <-");
			foreach (var (symbol, value) in taxonomy.TokenTemplates)
			{
				OutputTemplate(symbol, value);
			}

			_log.Warn("-> TokenTemplates End <-");
			_log.Warn("-----------------------------Taxonomy   End----------------------------------------");
		}

		private static void OutputPropertySet(string symbol, PropertySet propSet)
		{
			_log.Error("	***PropertySet***");
			_log.Info("		-Symbol: " + symbol);
			OutputArtifact(propSet.Artifact);
			foreach (var prop in propSet.Properties)
			{
				_log.Warn("		***Non-Behavioral Property***");
				_log.Info("			-Name: " + prop.Name);
				_log.Info("			-ValueDescription: " + prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				_log.Warn("		***Non-Behavioral Property End***");
			}

			_log.Error("	***PropertySet End***");
		}

		private static void OutputInvocations(IEnumerable<Invocation> propPropertyInvocations)
		{
			foreach (var i in propPropertyInvocations)
			{
				_log.Error("				-Invocation");
				_log.Info("					-Name: " + i.Name);
				_log.Info("					-Description: " + i.Description);
				OutputInvocationRequest(i.Request);
				OutputInvocationResponse(i.Response);
				_log.Error("					-----------------------------------------------------");
			}
		}

		private static void OutputInvocationResponse(InvocationResponse iResponse)
		{
			_log.Warn("					[Response]");
			_log.Info("						-Name: " + iResponse.ControlMessageName);
			_log.Info("						-Description: " + iResponse.Description);
			_log.Info(" 						[Output] ");
			foreach (var p in iResponse.OutputParameters)
			{
				_log.Info("  						[Parameter] ");
				_log.Info("   							-Parameter Name: " + p.Name);
				_log.Info("   							-Parameter Value Description: " + p.ValueDescription);
			}
		}

		private static void OutputInvocationRequest(InvocationRequest iRequest)
		{
			_log.Warn("					[Request]");
			_log.Info("						-Name: " + iRequest.ControlMessageName);
			_log.Info("						-Description: " + iRequest.Description);
			_log.Info(" 						[Input] ");
			foreach (var p in iRequest.InputParameters)
			{
				_log.Info("  						[Parameter] ");
				_log.Info("   							-Parameter Name: " + p.Name);
				_log.Info("   							-Parameter Value Description: " + p.ValueDescription);
			}
		}

		private static void OutputBehaviorGroup(string symbol, BehaviorGroup bg)
		{
			_log.Info("	***BehaviorGroup***");
			_log.Info("			-Symbol: " + symbol);
			OutputArtifact(bg.Artifact);
			OutputBehaviorSymbols(bg.BehaviorSymbols);
			_log.Info("	***BehaviorGroup End***");
		}

		private static void OutputBehaviorSymbols(IEnumerable<ArtifactSymbol> bgBehaviorSymbols)
		{
			_log.Info("		***Behavior Symbols***");
			foreach (var s in bgBehaviorSymbols)
			{
				_log.Error("		----------------------------------------------");
				_log.Info("			ToolingSymbol: " + s.ToolingSymbol);
				_log.Error("		----------------------------------------------");
			}
			_log.Info("		***Behavior Symbols End***");
		}

		private static void OutputBehavior(string symbol, Behavior behavior)
		{
			_log.Info("		***Behavior***");
			_log.Info(" 			-Symbol: " + symbol);
			OutputArtifact(behavior.Artifact);
			_log.Info(" 			-IsExternal: " + behavior.IsExternal);
			_log.Info(" 			-Constructor: " + behavior.ConstructorName);
			OutputInvocations(behavior.Invocations);
			OutputBehaviorProperties(behavior.Properties);
			_log.Info("		***Behavior End***");
		}

		private static void OutputBehaviorProperties(IEnumerable<Property> behaviorProperties)
		{
			foreach (var prop in behaviorProperties)
			{
				_log.Warn("		***Behavioral Property***");
				_log.Info(" 			-Name: " + prop.Name);
				_log.Info("			-ValueDescription: " + prop.ValueDescription);
				OutputInvocations(prop.PropertyInvocations);
				_log.Warn("		***Behavioral Property End***");
			}
		}

		private static void OutputBaseType(string symbol, Base baseType)
		{
			_log.Warn("	***Base Token Type***");
			_log.Info("		-Symbol: " + symbol);
			_log.Info("		-Name: " + baseType.Name);
			_log.Info("		-Type: " + baseType.TokenType);
			
			OutputArtifact(baseType.Artifact);
			_log.Warn("		-Formula:");
			_log.Info("			" + baseType.TokenFormulaCase);
			_log.Info("		-Constructor: " + baseType.ConstructorName);
			_log.Info("		-TokenSymbol: " + baseType.Symbol);
			_log.Info("		-Owner: " + baseType.Owner);
			_log.Info("		-Decimals: " + baseType.Decimals);
			_log.Info("		-Quantity: " + baseType.Quantity);
			_log.Error("		***Properties***");
			foreach (var tp in baseType.TokenProperties)
			{
				_log.Info("  		[Properties]");
				_log.Info("   			" + tp);
			}

			_log.Info("		***PropertiesEnd***");
			_log.Info("		***Children***");
			foreach (var c in baseType.ChildTokens)
			{
				_log.Info("			***Child Token***");
				OutputTemplate(c.Artifact.ArtifactSymbol.ToolingSymbol, c);
				_log.Info("			***Child Token End***");
			}

			_log.Info("		***ChildrenEnd***");
			_log.Warn("	***Base Token Type End***");
		}

		private static void OutputTemplate(string symbol, TokenTemplate template)
		{
			_log.Warn("	***TokenTemplate***");
			_log.Info("		-Formula: " + symbol);
			OutputArtifact(template.Artifact);
			OutputBaseType(template.Base.Artifact.ArtifactSymbol.ToolingSymbol, template.Base);
			_log.Error("		***Behaviors***");
			foreach (var b in template.Behaviors)
			{
				OutputBehavior(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Error("		***Behaviors End***");
			_log.Warn("		***BehaviorGroups ***");
			foreach (var b in template.BehaviorGroups)
			{
				OutputBehaviorGroup(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Warn("		***BehaviorGroups End***");
			_log.Error("		***PropertySets***");
			foreach (var b in template.PropertySets)
			{
				OutputPropertySet(b.Artifact.ArtifactSymbol.ToolingSymbol, b);
			}
			_log.Error("		***PropertySets End***");
			_log.Warn("	***TokenTemplate End***");
		}

	
		private static void OutputArtifact(Artifact artifact)
		{
			_log.Error("		[Details]:");
			_log.Info("			-ArtifactName: " + artifact.Name);
			_log.Info("			-Type: " + artifact.Type);
			OutputSymbol(artifact.ArtifactSymbol);
			_log.Info("			-Aliases: " + artifact.Aliases);
			OutputArtifactDefinition(artifact.ArtifactDefinition);
			_log.Info("			-ControlUri: " + artifact.ControlUri);
			OutputInfluencedBy(artifact.InfluencedBySymbols);
			OutputIncompatible(artifact.IncompatibleWithSymbols);
			_log.Warn("			-ArtifactFiles:");
			OutputArtifactFiles("			", artifact.ArtifactFiles);
			OutputMaps(artifact.Maps);
		}

		private static void OutputArtifactFiles(string buffer, IEnumerable<ArtifactFile> artifactFiles)
		{
			foreach (var f in artifactFiles)
			{
				_log.Info(buffer + "-FileContent: " + f.Content);
				_log.Info(buffer + "-FileName: " + f.FileName);
				_log.Info(buffer + "-FileContents:");
				_log.Error(buffer + "---------------------------------------------------------------------------------------");
				
				if (f.Content != ArtifactContent.Other)
					_log.Info(f.FileData.ToStringUtf8());
				_log.Error(buffer + "---------------------------------------------------------------------------------------");
			}
		}

		private static void OutputMaps(Maps artifactMaps)
		{
			_log.Error("			[Maps]:");
			_log.Warn("				-Implementation Maps:");
			foreach (var m in artifactMaps.ImplementationReferences)
			{
				_log.Error("				---------------------------------------------------");
				_log.Info("				-Name: " + m.Name);
				_log.Info("				-Type: " + m.MappingType);
				_log.Info("				-Supported Platform: " + m.Platform);
				_log.Info("				-Reference: " + m.ReferencePath);
				_log.Error("				---------------------------------------------------");
			}
			_log.Warn("				-Code Maps:");
			foreach (var m in artifactMaps.CodeReferences)
			{
				_log.Error("				---------------------------------------------------");
				_log.Info("				-Name: " + m.Name);
				_log.Info("				-Type: " + m.MappingType);
				_log.Info("				-Supported Platform: " + m.Platform);
				_log.Info("				-Reference: " + m.ReferencePath);
				_log.Error("				---------------------------------------------------");
			}
			_log.Warn("				-Resource Maps:");
			foreach (var m in artifactMaps.Resources)
			{
				_log.Error("				---------------------------------------------------");
				_log.Info("				-Name: " + m.Name);
				_log.Info("				-Type: " + m.MappingType);
				_log.Info("				-Description: " + m.Description);
				_log.Info("				-Reference: " + m.ResourcePath);
				_log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputIncompatible(IEnumerable<ArtifactSymbol> artifactIncompatibleWithSymbols)
		{
			_log.Warn("			-Incompatible With: ");
			foreach (var i in artifactIncompatibleWithSymbols)
			{
				_log.Error("				---------------------------------------------------");
				_log.Info("				-Symbol: " + i.ToolingSymbol);
				_log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputSymbol(ArtifactSymbol symbol)
		{
			_log.Warn("			-Symbol: ");
			_log.Error("				---------------------------------------------------");
			_log.Info("				-Tooling: " + symbol.ToolingSymbol);
			_log.Info("				-Visual: " + symbol.VisualSymbol);
			_log.Error("				---------------------------------------------------");
		}
		private static void OutputInfluencedBy(IEnumerable<SymbolInfluence> artifactInfluencedBySymbols)
		{
			_log.Warn("			-Influenced By: ");
			foreach (var i in artifactInfluencedBySymbols)
			{
				_log.Error("				---------------------------------------------------");
				_log.Info("				-Symbol: " + i.Symbol);
				_log.Info("				-Description: " + i.Description);
				_log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputArtifactDefinition(ArtifactDefinition artifactArtifactDefinition)
		{
			_log.Warn("			-Definition: ");
			_log.Info("				-Description: " + artifactArtifactDefinition.BusinessDescription);
			_log.Info("");
			_log.Info("				-Example: " + artifactArtifactDefinition.BusinessExample);
			_log.Info("");
			_log.Warn("				-Analogies: ");
			foreach (var a in artifactArtifactDefinition.Analogies)
			{
				_log.Info("				-Analogy: " + a.Name);
				_log.Info("				-Description: " + a.Description);
			}
			_log.Info("				-Comments: " + artifactArtifactDefinition.Comments);
		}
	}
}