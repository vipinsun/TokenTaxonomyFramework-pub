using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Collections;
using log4net;
using Newtonsoft.Json.Linq;
using TTF.Tokens.Model.Artifact;
using TTF.Tokens.Model.Core;
using TTI.TTF.Taxonomy.Model;

namespace TaxonomyHost.factories
{
	public static class TaxonomyFactory
	{
		private static string _baseFolder = "base";
		private static string _behaviorFolder = "behaviors";
		private static string _behaviorGroupFolder = "behavior-groups";
		private static string _propertySetFolder = "property-sets";
		private static string _tokenTemplates = "token-templates";
		private static string _artifactPath = TaxonomyService.ArtifactPath;
		private static string _folderSeparator = TaxonomyService.FolderSeparator;
		private static readonly ILog _log;

		static TaxonomyFactory()
		{
			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		internal static Taxonomy Load()
		{
			if (!Directory.Exists(_artifactPath))
			{
				var err = "Artifact Path not found: " + _artifactPath;
				_log.Error(err);
				throw new Exception(err);
			}

			_log.Info("Artifact Folder Found, loading to Taxonomy.");
			var root = new DirectoryInfo(_artifactPath);
			Taxonomy taxonomy;
			var rJson = root.GetFiles("*.json");
			try
			{
				taxonomy = GetArtifact<Taxonomy>(rJson[0]);
				_log.Info("Loaded Taxonomy Version: " + taxonomy.Version);
			}
			catch (Exception e)
			{
				_log.Error("Failed to load Taxonomy: " + e);
				throw;
			}

			var aPath = _artifactPath + _folderSeparator;
			if (Directory.Exists(aPath + _baseFolder))
			{

				_log.Info("Base Artifact Folder Found, loading to Base Token Types");
				var bases = new DirectoryInfo(aPath + _baseFolder);
				foreach (var ad in bases.EnumerateDirectories())
				{
					Base baseType;
					_log.Info("Loading " + ad.Name);
					var bJson = ad.GetFiles("*.json");
					try
					{
						baseType = GetArtifact<Base>(bJson[0]);
						
					}
					catch (Exception e)
					{
						_log.Error("Failed to load base token type.");
						_log.Error(e);
						continue;
					}

					var baseArtifactWFiles = GetBaseArtifactFiles(ad, baseType);
					taxonomy.BaseTokenTypes.Add(baseType.Artifact.ArtifactSymbol.ToolingSymbol, baseArtifactWFiles);
				}
			}
			else
			{
				_log.Error("Base artifact folder NOT found, moving on to behaviors.");
			}

			if (Directory.Exists(aPath + _behaviorFolder))
			{

				_log.Info("Behavior Artifact Folder Found, loading to Behavior Types");
				var behaviors = new DirectoryInfo(aPath + _behaviorFolder);
				foreach (var ad in behaviors.EnumerateDirectories())
				{
					Behavior behavior;
					_log.Info("Loading " + ad.Name);
					var bJson = ad.GetFiles("*.json");
					try
					{
						behavior = GetArtifact<Behavior>(bJson[0]);
						
					}
					catch (Exception e)
					{
						_log.Error("Failed to load base token type.");
						_log.Error(e);
						continue;
					}

					var baseArtifactWFiles = GetBehaviorArtifactFiles(ad, behavior);
					taxonomy.Behaviors.Add(baseType.Artifact.ArtifactSymbol.ToolingSymbol, baseArtifactWFiles);
				}
			}


			return tax;
		}

		private static Base GetBaseArtifactFiles(DirectoryInfo ad, Base baseType)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto", ignoreCase: true))
				{
					var protoFile = GetArtifactText(af);
					baseType.Artifact.ControlUri = af.Name;
					baseType.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
				}

				if (af.Name.EndsWith("md", ignoreCase: true))
				{
					var mdFile = GetArtifactText(af);

					baseType.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
				}
				else
				{
					var otherFile = GetArtifactBytes(af);

					baseType.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFrom(otherFile),
						Content = ArtifactContent.Other
					});
				}
			}

			return baseType;
		}
		
		private static Behavior GetBehaviorArtifactFiles(DirectoryInfo ad, Behavior behavior)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto", ignoreCase: true))
				{
					var protoFile = GetArtifactText(af);
					behavior.Artifact.ControlUri = af.Name;
					behavior.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
				}

				if (af.Name.EndsWith("md", ignoreCase: true))
				{
					var mdFile = GetArtifactText(af);

					behavior.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
				}
				else
				{
					var otherFile = GetArtifactBytes(af);

					behavior.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFrom(otherFile),
						Content = ArtifactContent.Other
					});
				}
			}

			return behavior;
		}

		private static T GetArtifact<T>(FileInfo artifact) where T : new()
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
	}
}