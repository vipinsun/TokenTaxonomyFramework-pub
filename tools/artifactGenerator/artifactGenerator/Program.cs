using System;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using log4net;
using TTF.Tokens.Model.Artifact;
using TTF.Tokens.Model.Core;

namespace ArtifactGenerator
{
	class Program{
	
		private static ILog _log;
		private static string ArtifactName { get; set; }
		private static string ArtifactPath { get; set; }
		private static ArtifactType ArtifactType { get; set; }
		public static void Main(string[] args)
		{
			if (args.Length != 6)
			{
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
				Type = ArtifactType
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
					var artifactPropertSet = new PropertySet
					{
						Artifact = AddArtifactFiles(outputFolder, folderSeparator, artifact)
					};
					artifactJson = jsf.Format(artifactPropertSet);
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
			_log.Info("Creating Artifact: " + artifactJson);
			var artifactStream = File.CreateText(outputFolder.FullName + folderSeparator + ArtifactName + ".json");
			artifactStream.Write(artifactJson);
			artifactStream.Close();
			
			
			_log.Info("Complete");
		}

		private static Artifact AddArtifactFiles(DirectoryInfo outputFolder, string folderSeparator, Artifact parent)
		{
			var md = CreateMarkdown(outputFolder, folderSeparator, parent);
			var proto = CreateProto(outputFolder, folderSeparator, parent);
			var retArtifact = parent.Clone();
			
			retArtifact.ArtifactFiles.Add(proto);
			retArtifact.ArtifactFiles.Add(md);
			return retArtifact;
		}
		private static ArtifactFile CreateMarkdown(DirectoryInfo outputFolder, string folderSeparator, Artifact parent)
		{
			_log.Info("Creating Artifact Markdown");
			var md = File.CreateText(outputFolder + folderSeparator + ArtifactName + ".md");
			md.Write("# " + ArtifactName + " a TTF " + ArtifactType);
			md.Close();
			return new ArtifactFile
			{
				FileName = ArtifactName + ".md",
				Artifact = parent,
				Content = ArtifactContent.Uml
			};
		}
		
		private static ArtifactFile CreateProto(DirectoryInfo outputFolder, string folderSeparator, Artifact parent)
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
				Artifact = parent,
				Content = ArtifactContent.Control
			};
		}
		
	}
}