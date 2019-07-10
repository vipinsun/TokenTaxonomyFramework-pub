using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using log4net;
using Newtonsoft.Json.Linq;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy
{
	public static class OutputLib
	{
		private static readonly ILog _log;
		private static readonly string _folderSeparator = Os.IsWindows() ? "\\" : "/";
		private static readonly string _artifactPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		static OutputLib()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static void OutputPropertySet(string symbol, PropertySet propSet)
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

		internal static void OutputBehaviorGroup(string symbol, BehaviorGroup bg)
		{
			_log.Warn("	***BehaviorGroup***");
			_log.Info("			-Symbol: " + symbol);
			if (bg.Artifact != null)
			{
				OutputArtifact(bg.Artifact);
				OutputBehaviorSymbols(bg.BehaviorSymbols);
			}

			_log.Warn("	***BehaviorGroup End***");
		}

		private static void OutputBehaviorSymbols(IEnumerable<ArtifactSymbol> bgBehaviorSymbols)
		{
			_log.Warn("		***Behavior Symbols***");
			foreach (var s in bgBehaviorSymbols)
			{
				_log.Error("		----------------------------------------------");
				_log.Info("			tooling: " + s.tooling);
				_log.Error("		----------------------------------------------");
			}
			_log.Warn("		***Behavior Symbols End***");
		}

		internal static void OutputBehavior(string symbol, Behavior behavior)
		{
			_log.Warn("		***Behavior***");
			_log.Info(" 			-Symbol: " + symbol);
			if (behavior.Artifact != null)
			{
				OutputArtifact(behavior.Artifact);
				_log.Info(" 			-IsExternal: " + behavior.IsExternal);
				_log.Info(" 			-Constructor: " + behavior.ConstructorName);
				OutputInvocations(behavior.Invocations);
				OutputBehaviorProperties(behavior.Properties);
			}
			_log.Warn("		***Behavior End***");
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

		internal static void OutputBaseType(string symbol, Base baseType)
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
				OutputTemplate(c.Artifact.ArtifactSymbol.tooling, c);
				_log.Info("			***Child Token End***");
			}

			_log.Info("		***ChildrenEnd***");
			_log.Warn("	***Base Token Type End***");
		}

		internal static void OutputTemplate(string symbol, TokenTemplate template)
		{
			_log.Warn("	***TokenTemplate***");
			_log.Info("		-Formula: " + symbol);
			OutputArtifact(template.Artifact);
			OutputBaseType(template.Base.Base.Artifact.ArtifactSymbol.tooling, template.Base.Base);
			_log.Error("		***Behaviors***");
			foreach (var b in template.Behaviors)
			{
				OutputBehavior(b.Symbol.tooling, b.Behavior);
			}
			_log.Error("		***Behaviors End***");
			_log.Warn("		***BehaviorGroups ***");
			foreach (var b in template.BehaviorGroups)
			{
				OutputBehaviorGroup(b.Symbol.tooling, b.BehaviorGroup);
			}
			_log.Warn("		***BehaviorGroups End***");
			_log.Error("		***PropertySets***");
			foreach (var b in template.PropertySets)
			{
				OutputPropertySet(b.Symbol.tooling, b.PropertySet);
			}
			_log.Error("		***PropertySets End***");
			_log.Warn("	***TokenTemplate End***");
		}

	
		private static void OutputArtifact(Artifact artifact)
		{
			if (artifact == null) return;
			_log.Error("		[Details]:");
			_log.Info("			-ArtifactName: " + artifact.Name);
			_log.Info("			-Type: " + artifact.ArtifactSymbol.Type);
			OutputSymbol(artifact.ArtifactSymbol);
			_log.Info("			-Aliases: " + artifact.Aliases);
			OutputArtifactDefinition(artifact.ArtifactDefinition);
			_log.Info("			-ControlUri: " + artifact.ControlUri);
			OutputDependencies(artifact.Dependencies);
			OutputInfluencedBy(artifact.InfluencedBySymbols);
			OutputIncompatible(artifact.IncompatibleWithSymbols);
			_log.Warn("			-ArtifactFiles:");
			OutputArtifactFiles("			", artifact.ArtifactFiles);
			OutputMaps(artifact.Maps);
		}

		private static void OutputDependencies(IEnumerable<SymbolDependency> artifactDependencies)
		{
			_log.Warn("			-Dependencies: ");
               foreach (var i in artifactDependencies)
               {
               	_log.Error("				---------------------------------------------------");
               	_log.Info("				-Symbol: " + i.Symbol);
               	_log.Info("				-Description: " + i.Description);
               	_log.Error("				---------------------------------------------------");
               }
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
				_log.Info("				-Symbol: " + i.tooling);
				_log.Error("				---------------------------------------------------");
			}
		}

		private static void OutputSymbol(ArtifactSymbol symbol)
		{
			_log.Warn("			-Symbol: ");
			_log.Error("				---------------------------------------------------");
			_log.Info("				-Type: " + symbol.Type);
			_log.Info("				-Tooling: " + symbol.tooling);
			_log.Info("				-Visual: " + symbol.visual);
			_log.Info("				-Version: " + symbol.version);
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
		
		internal static void SaveArtifact(ArtifactType type, string artifactName, string artifactJson)
		{
			_log.Info("Artifact Destination: " + _artifactPath + _folderSeparator + type + " folder");
			var formattedJson = JToken.Parse(artifactJson).ToString();

			//test to make sure formattedJson will Deserialize.
			try
			{
				switch (type)
				{
					case ArtifactType.Base:
						var testBase = JsonParser.Default.Parse<Base>(formattedJson);
						_log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBase.Artifact.Name);
						break;
					case ArtifactType.Behavior:
						var testBehavior = JsonParser.Default.Parse<Behavior>(formattedJson);
						_log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBehavior.Artifact.Name);
						break;
					case ArtifactType.BehaviorGroup:
						var testBehaviorGroup = JsonParser.Default.Parse<BehaviorGroup>(formattedJson);
						_log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testBehaviorGroup.Artifact.Name);
						break;
					case ArtifactType.PropertySet:
						var testPropertySet = JsonParser.Default.Parse<PropertySet>(formattedJson);
						_log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testPropertySet.Artifact.Name);
						break;
					case ArtifactType.TokenTemplate:
						var testTemplate = JsonParser.Default.Parse<TokenTemplate>(formattedJson);
						_log.Info("Artifact type: " + type + " successfully deserialized: " +
						          testTemplate.Artifact.Name);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception e)
			{
				_log.Error("Json failed to deserialize back into: " + type);
				_log.Error(e);
				return;
			}

			_log.Info("Saving Artifact: " + artifactName);
			if (_artifactPath != null)
			{
				var savedTo =Directory.CreateDirectory(_artifactPath + _folderSeparator + artifactName);
				var artifactStream = File.CreateText(_artifactPath + _folderSeparator + artifactName 
				                                     + _folderSeparator + artifactName + ".json");
				artifactStream.Write(formattedJson);
				artifactStream.Close();
				_log.Info("Saved to folder: " + savedTo.Name);
			}

			_log.Info("Local Artifact Save Complete");
		}

		internal static void UpdateArtifact(ArtifactType type, string folderName)
		{
			_log.Error("Updating: " + type + " name = " + folderName);
			var path = _artifactPath + _folderSeparator + folderName;
			if (Directory.Exists(path))
			{
				var artifactFolder = new DirectoryInfo(path);
				_log.Info("Loading Updated Artifact From: " + artifactFolder.Name);
				var bJson = artifactFolder.GetFiles("*.json");
				//eventually load the proto and md as well.
				var updateArtifactRequest = new UpdateArtifactRequest
				{
					Type = type
				};
				
				switch (type)
				{
					case ArtifactType.Base:
						try
						{
							var baseType = GetArtifact<Base>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(baseType);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							_log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							_log.Error("Failed to load base token type: " + artifactFolder.Name);
							_log.Error(e);
						}
						break;
					case ArtifactType.Behavior:
						try
						{
							var typeFromJson = GetArtifact<Behavior>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							_log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							_log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							_log.Error(e);
						}
						break;
					case ArtifactType.BehaviorGroup:
						try
						{
							var typeFromJson = GetArtifact<BehaviorGroup>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							_log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							_log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							_log.Error(e);
						}
						break;
					case ArtifactType.PropertySet:
						try
						{
							var typeFromJson = GetArtifact<PropertySet>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							_log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							_log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							_log.Error(e);
						}
						break;
					case ArtifactType.TokenTemplate:
						try
						{
							var typeFromJson = GetArtifact<TokenTemplate>(bJson[0]);
							updateArtifactRequest.ArtifactTypeObject = Any.Pack(typeFromJson);
							var response = Client.TaxonomyClient.UpdateArtifact(updateArtifactRequest);
							_log.Info("Updated: " + response.Updated);
						}
						catch (Exception e)
						{
							_log.Error("Failed to load/update type: " + type + " name = " + artifactFolder.Name);
							_log.Error(e);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(type), type, null);
				}
			}
			else
			{
				_log.Warn("Local Artifact Folder: " + path + " NOT FOUND.");
			}
		}
		private static T GetArtifact<T>(FileInfo artifact) where T : IMessage, new()
		{
			var typeFile = artifact.OpenText();
			var json = typeFile.ReadToEnd();
			var formattedJson = JToken.Parse(json).ToString();
			return JsonParser.Default.Parse<T>(formattedJson);
		}
	}
}