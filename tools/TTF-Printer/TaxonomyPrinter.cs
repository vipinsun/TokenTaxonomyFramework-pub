using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using TTI.TTF.Taxonomy.TypePrinters;
using Behavior = TTI.TTF.Taxonomy.Model.Core.Behavior;

namespace TTI.TTF.Taxonomy
{
	public class TaxonomyPrinter
	{
		private static WordprocessingDocument _document;
		private static readonly ILog _log;
		private static string _outputFolder = "";
		private static string _filePath = "";

		static TaxonomyPrinter()
		{
			#region logging

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			#endregion
		}

		//todo:Testing here
		internal async Task PrintAllArtifacts(string filePath, string waterMark, string styleSource)
		{
			_log.Info("Taxonomy Printer: Printing All Artifacts");

			//bases
			foreach (var baseToken in ModelManager.Taxonomy.BaseTokenTypes)
			{
				PrintController.PrintBase(filePath, waterMark, styleSource, baseToken.Value);
			}

			foreach (var behavior in ModelManager.Taxonomy.Behaviors)
			{
				PrintController.PrintBehavior(filePath, waterMark, styleSource, behavior.Value);
			}

			foreach (var bg in ModelManager.Taxonomy.BehaviorGroups)
			{
				PrintController.PrintBehaviorGroup(filePath, waterMark, styleSource, bg.Value);
			}

			foreach (var ps in ModelManager.Taxonomy.PropertySets)
			{
				PrintController.PrintPropertySet(filePath, waterMark, styleSource, ps.Value);
			}

			foreach (var f in ModelManager.Taxonomy.TemplateFormulas)
			{
				PrintController.PrintFormula(filePath, waterMark, styleSource, f.Value);
			}

			foreach (var d in ModelManager.Taxonomy.TemplateDefinitions)
			{
				PrintController.PrintDefinition(filePath, waterMark, styleSource, d.Value);
				var spec = await Printer.TaxonomyClient.GetTokenSpecificationAsync(new TokenTemplateId
				{
					DefinitionId = d.Key
				});

				if (spec == null) continue;
				if (spec.Artifact.Name.Contains("Error:"))
				{
					_log.Error(spec.Artifact.Name);
					return;
				}
				PrintController.PrintSpec(filePath, waterMark, styleSource, spec);
			}
		}
		
		internal static void InitWorkingDocument(string styleSource)
		{
			_document =
				WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
			_log.Info("TTF Book file: " + _filePath + " created");
			// Add a main document part.     
			var mainPart = _document.AddMainDocumentPart();

			// Create the document structure and add some text.
			mainPart.Document = new Document();

			// Get the Styles part for this document.
			Utils.AddStylesPartToPackage(_document);
			var styles = Utils.ExtractStylesPart(styleSource);
			Utils.ReplaceStylesPart(_document, styles);
			Save();
		}

		internal static void BuildTtfBook(string filePath, string waterMark, string styleSource)
		{
			var aPath = filePath + ModelMap.FolderSeparator;
			_filePath = filePath + ModelMap.FolderSeparator + ".." + ModelMap.FolderSeparator + "TTF-Book.docx";
			
			try
			{
				PrintController.InitWorkingDocument(styleSource);
			}
			catch (Exception ex)
			{
				_log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
				_log.Error(ex);
				return;
			}

			AddTaxonomyInfo(_document, ModelManager.Taxonomy.Version);
			
			if (Directory.Exists(aPath + ModelMap.BaseFolder))
			{
				_log.Info("Base Artifact Folder Found, loading to Base Token Types");
				var bases = new DirectoryInfo(aPath + ModelMap.BaseFolder);
				foreach (var ad in bases.EnumerateDirectories())
				{
					Base baseType;
					_log.Info("Loading " + ad.Name);
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
							_log.Error("Failed to load base token type: " + ad.Name);
							_log.Error(e);
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
				_log.Error("Base artifact folder NOT found, moving on to behaviors.");
			}

			if (Directory.Exists(aPath + ModelMap.BehaviorFolder))
			{

				_log.Info("Behavior Artifact Folder Found, loading to Behaviors");
				var behaviors = new DirectoryInfo(aPath + ModelMap.BehaviorFolder);

				foreach (var ad in behaviors.EnumerateDirectories())
				{
					Behavior behavior;
					_log.Info("Loading " + ad.Name);
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
							_log.Error("Failed to load Behavior: " + ad.Name);
							_log.Error(e);
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

				_log.Info("BehaviorGroup Artifact Folder Found, loading to BehaviorGroups");
				var behaviorGroups = new DirectoryInfo(aPath + ModelMap.BehaviorGroupFolder);
				
				foreach (var ad in behaviorGroups.EnumerateDirectories())
				{
					BehaviorGroup behaviorGroup;
					_log.Info("Loading " + ad.Name);
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
							_log.Error("Failed to load BehaviorGroup: " + ad.Name);
							_log.Error(e);
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

				_log.Info("PropertySet Artifact Folder Found, loading to PropertySets");
				var propertySets = new DirectoryInfo(aPath + ModelMap.PropertySetFolder);
				foreach (var ad in propertySets.EnumerateDirectories())
				{
					PropertySet propertySet;
					_log.Info("Loading " + ad.Name);
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
							_log.Error("Failed to load PropertySet: " + ad.Name);
							_log.Error(e);
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
				_log.Info("TemplateFormulas Folder Found, loading to TemplateFormulas");
				var formulaDirectory = new DirectoryInfo(aPath + ModelMap.TemplateFormulasFolder);
				var formulaFolders = formulaDirectory.EnumerateDirectories();
				foreach (var f in formulaFolders)
				{
					TemplateFormula formula;
					_log.Info("Loading " + f.Name);
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
							_log.Error("Failed to load TemplateFormula: " + f.Name);
							_log.Error(e);
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
				_log.Info("TemplateDefinitions Folder Found, loading to TemplateDefinitions");
				var definitionsFolder = new DirectoryInfo(aPath + ModelMap.TemplateDefinitionsFolder);
				var definitions = definitionsFolder.EnumerateDirectories();
				foreach (var t in definitions)
				{
					TemplateDefinition definition;
					_log.Info("Loading " + t.Name);
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
							_log.Error("Failed to load TemplateDefinition: " + t.Name);
							_log.Error(e);
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
		}
		
		public static void AddTaxonomyInfo(WordprocessingDocument document, TaxonomyVersion version)
        {
            _log.Info("Printing Artifact Reference: " + artifactReference.Id );

            var body = document.MainDocumentPart.Document.Body;
            
            var name = GetNameForId(artifactReference.Id, artifactReference.Type);
            
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text("Name: " + name) { Space = SpaceProcessingModeValues.Preserve });
           
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", para);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Id: " + artifactReference.Id));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", aDef);

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text("Reference Notes: " + artifactReference.ReferenceNotes));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bizBody);
            
            if (artifactReference.Values == null) return;
            
            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Reference Values"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", eDef);
            
            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fiPara);
            
            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(document, artifactReference.Values.ArtifactFiles));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffiPara);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cPara);

            var ccPara =
                body.AppendChild(
                    new Paragraph(new ParagraphProperties(new ParagraphStyleId {Val = "Heading2"})));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(document, artifactReference.Values.Maps.CodeReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ccPara);
                
            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pPara);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(document,
                artifactReference.Values.Maps.ImplementationReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ppPara);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", rPara);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(document, artifactReference.Values.Maps.Resources));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rrPara);
        }
		internal static void Save(bool validate = false)
		{
			_document.MainDocumentPart.Document.Save();
			_log.Info("Document saved: " + _filePath);
			if (!validate) return;
			_document.Close();
			_document = WordprocessingDocument.Open(_filePath,false);
                
			if (!Utils.ValidateWordDocument(_document))
				Utils.ValidateCorruptedWordDocument(_document);
			_document.Close();
			_document = WordprocessingDocument.Open(_filePath, true);
		}
	}
}