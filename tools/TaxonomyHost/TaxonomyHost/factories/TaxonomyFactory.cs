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
						_log.Error("Failed to load base token type: " + ad.Name);
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

				_log.Info("Behavior Artifact Folder Found, loading to Behaviors");
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
						_log.Error("Failed to load Behavior: " + ad.Name);
						_log.Error(e);
						continue;
					}

					var behaviorArtifactFiles = GetBehaviorArtifactFiles(ad, behavior);
					taxonomy.Behaviors.Add(behavior.Artifact.ArtifactSymbol.ToolingSymbol, behaviorArtifactFiles);
				}
			}

			if (Directory.Exists(aPath + _behaviorGroupFolder))
			{

				_log.Info("BehaviorGroup Artifact Folder Found, loading to BehaviorGroups");
				var behaviorGroups = new DirectoryInfo(aPath + _behaviorGroupFolder);
				foreach (var ad in behaviorGroups.EnumerateDirectories())
				{
					BehaviorGroup behaviorGroup;
					_log.Info("Loading " + ad.Name);
					var bJson = ad.GetFiles("*.json");
					try
					{
						behaviorGroup = GetArtifact<BehaviorGroup>(bJson[0]);
						
					}
					catch (Exception e)
					{
						_log.Error("Failed to load BehaviorGroup: " +ad.Name);
						_log.Error(e);
						continue;
					}

					var behaviorGroupArtifactWFiles = GetBehaviorGroupArtifactFiles(ad, behaviorGroup);
					taxonomy.BehaviorGroups.Add(behaviorGroupArtifactWFiles.Artifact.ArtifactSymbol.ToolingSymbol, behaviorGroupArtifactWFiles);
				}
			}
			
			if(Directory.Exists(aPath + _propertySetFolder))
			{

				_log.Info("PropertySet Artifact Folder Found, loading to PropertySets");
				var propertySets = new DirectoryInfo(aPath + _behaviorGroupFolder);
				foreach (var ad in propertySets.EnumerateDirectories())
				{
					PropertySet propertySet;
					_log.Info("Loading " + ad.Name);
					var bJson = ad.GetFiles("*.json");
					try
					{
						propertySet = GetArtifact<PropertySet>(bJson[0]);
						
					}
					catch (Exception e)
					{
						_log.Error("Failed to load PropertySet: " + ad.Name);
						_log.Error(e);
						continue;
					}

					var propertySetArtifactFiles = GetPropertySetArtifactFiles(ad, propertySet);
					taxonomy.PropertySets.Add(propertySetArtifactFiles.Artifact.ArtifactSymbol.ToolingSymbol, propertySetArtifactFiles);
				}
			}
			
			if(Directory.Exists(aPath + _tokenTemplates))
			{

				_log.Info("TokenTemplate Artifact Folder Found, loading to TokenTemplates");
				var tokenTemplates = new DirectoryInfo(aPath + _tokenTemplates);
				foreach (var ad in tokenTemplates.EnumerateDirectories())
				{
					TokenTemplate tokenTemplate;
					_log.Info("Loading " + ad.Name);
					var bJson = ad.GetFiles("*.json");
					try
					{
						tokenTemplate = GetArtifact<TokenTemplate>(bJson[0]);
						
					}
					catch (Exception e)
					{
						_log.Error("Failed to load TokenTemplate: " + ad.Name);
						_log.Error(e);
						continue;
					}

					var tokenTemplateArtifactFiles = GetTokenTemplateArtifactFiles(ad, tokenTemplate);
					taxonomy.TokenTemplates.Add(tokenTemplateArtifactFiles.Artifact.ArtifactSymbol.ToolingSymbol, tokenTemplateArtifactFiles);
				}
			}
			
			return taxonomy;
		}

		private static Base GetBaseArtifactFiles(DirectoryInfo ad, Base baseType)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto"))
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

				if (af.Name.EndsWith("md"))
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
				if (af.Name.EndsWith("proto"))
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

				if (af.Name.EndsWith("md"))
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

		private static BehaviorGroup GetBehaviorGroupArtifactFiles(DirectoryInfo ad, BehaviorGroup behaviorGroup)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto"))
				{
					var protoFile = GetArtifactText(af);
					behaviorGroup.Artifact.ControlUri = af.Name;
					behaviorGroup.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
				}

				if (af.Name.EndsWith("md"))
				{
					var mdFile = GetArtifactText(af);

					behaviorGroup.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
				}
				else
				{
					var otherFile = GetArtifactBytes(af);

					behaviorGroup.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFrom(otherFile),
						Content = ArtifactContent.Other
					});
				}
			}

			return behaviorGroup;
		}
		
		private static PropertySet GetPropertySetArtifactFiles(DirectoryInfo ad, PropertySet propertySet)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto"))
				{
					var protoFile = GetArtifactText(af);
					propertySet.Artifact.ControlUri = af.Name;
					propertySet.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
				}

				if (af.Name.EndsWith("md"))
				{
					var mdFile = GetArtifactText(af);

					propertySet.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
				}
				else
				{
					var otherFile = GetArtifactBytes(af);

					propertySet.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFrom(otherFile),
						Content = ArtifactContent.Other
					});
				}
			}

			return propertySet;
		}
		
		private static TokenTemplate GetTokenTemplateArtifactFiles(DirectoryInfo ad, TokenTemplate tokenTemplate)
		{
			foreach (var af in ad.EnumerateFiles())
			{
				if (af.Name.EndsWith("proto"))
				{
					var protoFile = GetArtifactText(af);
					tokenTemplate.Artifact.ControlUri = af.Name;
					tokenTemplate.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(protoFile),
						Content = ArtifactContent.Control
					});
				}

				if (af.Name.EndsWith("md"))
				{
					var mdFile = GetArtifactText(af);

					tokenTemplate.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFromUtf8(mdFile),
						Content = ArtifactContent.Uml
					});
				}
				else
				{
					var otherFile = GetArtifactBytes(af);

					tokenTemplate.Artifact.ArtifactFiles.Add(new ArtifactFile
					{
						FileName = af.Name,
						FileData = ByteString.CopyFrom(otherFile),
						Content = ArtifactContent.Other
					});
				}
			}

			return tokenTemplate;
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
	}
}