using System;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using log4net;
using Newtonsoft.Json.Linq;
using TTF.Tokens.Model.Artifact;
using TTF.Tokens.Model.Core;

namespace ArtifactGenerator
{
	internal static class FactGen{
	
		private static ILog _log;
		private static string ArtifactName { get; set; }
		private static string ArtifactPath { get; set; }
		private static ArtifactType ArtifactType { get; set; }
		public static void Main(string[] args)
		{
			if (args.Length != 6)
			{
				if (args.Length == 0)
				{
					_log.Info("Usage: dotnet factgen --p [PATH_TO_ARTIFACTS FOLDER] --t [ARTIFACT_TYPE: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate --n [ARTIFACT_NAME] ");
					return;
				}
				_log.Error("Required arguments --p [path-to-artifact folder] --n [artifactName] --t [artifactType: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate");
				throw new Exception("Missing required parameters.");
			}

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
				}

				if (arg != "--t") continue;
				i++;
				var t = Convert.ToInt32(args[i]);
				ArtifactType = (ArtifactType) t;
			}

			if (string.IsNullOrEmpty(ArtifactPath) || string.IsNullOrEmpty(ArtifactName))
			{
				throw new Exception("Missing value for either --n or --p.");
			}

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

			
			string artifactTypeFolder;
			var jsf = new JsonFormatter(new JsonFormatter.Settings(true));
			
			string artifactJson;
			DirectoryInfo outputFolder;
			var artifact =  new Artifact
			{
				Name = ArtifactName,
				Type = ArtifactType,
				ArtifactSymbol = new ArtifactSymbol
				{
					ToolingSymbol = "",
					VisualSymbol = ""
				},
				ArtifactDefinition = new ArtifactDefinition
				{
					BusinessDescription = "This is a " + ArtifactName + " of type: " + ArtifactType,
					BusinessExample = "",
					Comments = "",
					Analogies = { new ArtifactAnalogy
					{
						Name = "Analogy 1",
						Description = ArtifactName + " analogy 1 description"
					}}
				},
				Maps = new Maps
				{
					CodeReferences = { new MapReference
					{
						MappingType = MappingType.SourceCode,
						Name = "Code 1",
						Platform = TargetPlatform.Daml,
						ReferencePath = ""
					}},
					ImplementationReferences = { new MapReference
					{
						MappingType = MappingType.Implementation,
						Name = "Implementation 1",
						Platform = TargetPlatform.ChaincodeGo,
						ReferencePath = ""
					}},
					Resources = { new MapResourceEntry
					{
						MappingType = MappingType.Resource,
						Name = "Regulation Reference 1",
						Description = "",
						ResourcePath = ""
					}}
				},
				IncompatibleWithSymbols = { new ArtifactSymbol
				{
					ToolingSymbol = "",
					VisualSymbol = ""
				}},
				Aliases = { "alias1", "alias2"}
				
			};
			switch (ArtifactType)
			{
				case ArtifactType.Base:
					
					artifactTypeFolder = "base";
					outputFolder = Directory.CreateDirectory(fullPath + artifactTypeFolder + folderSeparator + ArtifactName);
					var artifactBase = new Base
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactBase);
					break;
				case ArtifactType.Behavior:
					
					artifactTypeFolder = "behaviors";
					outputFolder = Directory.CreateDirectory(fullPath + artifactTypeFolder + folderSeparator + ArtifactName);
					var artifactBehavior = new Behavior
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactBehavior);
					break;
				case ArtifactType.BehaviorGroup:
					artifactTypeFolder = "behavior-groups";
					outputFolder = Directory.CreateDirectory(fullPath + artifactTypeFolder + folderSeparator + ArtifactName);
					var artifactBehaviorGroup = new BehaviorGroup
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactBehaviorGroup);
					break;
				case ArtifactType.PropertySet:
					artifactTypeFolder = "property-sets";
					outputFolder = Directory.CreateDirectory(fullPath + artifactTypeFolder + folderSeparator + ArtifactName);
					var artifactPropertySet = new PropertySet
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactPropertySet);
					break;
				case ArtifactType.TokenTemplate:
					artifactTypeFolder = "tokens";
					outputFolder = Directory.CreateDirectory(fullPath + artifactTypeFolder + folderSeparator + ArtifactName);
					var artifactTokenTemplate = new TokenTemplate
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactTokenTemplate);
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
						var testBehaviorGroup = JsonParser.Default.Parse<Behavior>(formattedJson);
						_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
						          testBehaviorGroup.Artifact.Name);
						break;
					case ArtifactType.PropertySet:
						var testPropertySet = JsonParser.Default.Parse<Behavior>(formattedJson);
						_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
						          testPropertySet.Artifact.Name);
						break;
					case ArtifactType.TokenTemplate:
						var testTemplate = JsonParser.Default.Parse<Behavior>(formattedJson);
						_log.Info("Artifact type: " + ArtifactType + " successfully deserialized: " +
						          testTemplate.Artifact.Name);
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
			var artifactStream = File.CreateText(outputFolder.FullName + folderSeparator + ArtifactName + ".json");
			artifactStream.Write(formattedJson);
			artifactStream.Close();
			
			
			_log.Info("Complete");
		}

		private static Artifact AddArtifactFiles(DirectoryInfo outputFolder, string folderSeparator, Artifact parent)
		{
			var md = CreateMarkdown(outputFolder, folderSeparator);
			var proto = CreateProto(outputFolder, folderSeparator);
			var retArtifact = parent.Clone();
			
			retArtifact.ArtifactFiles.Add(proto);
			retArtifact.ControlUri = ArtifactPath + folderSeparator + ArtifactName + folderSeparator + proto.FileName;
			retArtifact.ArtifactFiles.Add(md);
			return retArtifact;
		}
		private static ArtifactFile CreateMarkdown(DirectoryInfo outputFolder, string folderSeparator)
		{
			_log.Info("Creating Artifact Markdown");
			var md = File.CreateText(outputFolder + folderSeparator + ArtifactName + ".md");
			md.Write("# " + ArtifactName + " a TTF " + ArtifactType);
			md.Close();
			return new ArtifactFile
			{
				FileName = ArtifactName + ".md",
				Content = ArtifactContent.Uml
			};
		}
		
		private static ArtifactFile CreateProto(DirectoryInfo outputFolder, string folderSeparator)
		{
			_log.Info("Creating Artifact Proto");
			var proto  = File.CreateText(outputFolder + folderSeparator + ArtifactName + ".proto");
			var templateProto =
				File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
				                 folderSeparator + "templates" + folderSeparator + "artifact.proto");
			
			
			proto.Write(templateProto.Replace("ARTIFACT", ArtifactName));
			proto.Close();
			return new ArtifactFile
			{
				FileName = ArtifactName + ".proto",
				Content = ArtifactContent.Control
			};
		}
		
	}
}