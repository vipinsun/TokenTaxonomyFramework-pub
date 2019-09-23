using System;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.TypePrinters;

namespace TTI.TTF.Taxonomy
{
	public static class TaxonomyPrinter
	{
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
		internal static async Task PrintAllArtifacts(string filePath, string waterMark, string styleSource)
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
		
		private static void InitWorkingDocument(string styleSource)
		{
			PrintController.Document =
				WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
			_log.Info("TTF Book file: " + _filePath + " created");
			// Add a main document part.     
			var mainPart = PrintController.Document.AddMainDocumentPart();

			// Create the document structure and add some text.
			mainPart.Document = new Document();
			mainPart.Document.AppendChild(new Body());
			// Get the Styles part for this document.
			Utils.AddStylesPartToPackage(PrintController.Document);
			var styles = Utils.ExtractStylesPart(styleSource);
			Utils.ReplaceStylesPart(PrintController.Document, styles);
			PrintController.Save();
		}

		internal static void BuildTtfBook(string filePath, string waterMark, string styleSource)
		{
			_filePath = filePath + ModelMap.FolderSeparator + ".." + ModelMap.FolderSeparator + "TTF-Book.docx";
			try
			{
				InitWorkingDocument(styleSource);
			}
			catch (Exception ex)
			{
				_log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
				_log.Error(ex);
				return;
			}

			AddTaxonomyInfo(PrintController.Document, ModelManager.Taxonomy.Version);
			
			_log.Info("Adding Bases");
			foreach (var b in ModelManager.Taxonomy.BaseTokenTypes.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, b.Artifact, true,false);
				BasePrinter.AddBaseProperties(PrintController.Document, b, true);
			}
			PrintController.Save();
			_log.Info("Adding Behaviors");
			foreach (var b in ModelManager.Taxonomy.Behaviors.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, b.Artifact, true,false);
				BehaviorPrinter.AddBehaviorProperties(PrintController.Document, b, true);
			}
			PrintController.Save();
			_log.Info("Adding Behavior-Groups");
			foreach (var b in ModelManager.Taxonomy.BehaviorGroups.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, b.Artifact, true, false);
				BehaviorGroupPrinter.AddBehaviorGroupProperties(PrintController.Document, b, true);
			}
			PrintController.Save();
			_log.Info("Adding Property-Sets");
			foreach (var ps in ModelManager.Taxonomy.PropertySets.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, ps.Artifact, true, false);
				PropertySetPrinter.AddPropertySetProperties(PrintController.Document, ps, true);
			}
			PrintController.Save();
			_log.Info("Adding Template Formulas");
			foreach (var tf in ModelManager.Taxonomy.TemplateFormulas.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, tf.Artifact, true,false);
				FormulaPrinter.AddFormulaProperties(PrintController.Document, tf, true);
			}
			PrintController.Save();
			_log.Info("Adding Template Definitions");
			foreach (var td in ModelManager.Taxonomy.TemplateDefinitions.Values)
			{
				ArtifactPrinter.AddArtifactContent(PrintController.Document, td.Artifact, true, false);
				DefinitionPrinter.AddDefinitionProperties(PrintController.Document, td, true);
			}
			PrintController.Save();
			_log.Info("Adding Specifications");
			foreach (var tf in ModelManager.Taxonomy.TemplateDefinitions.Keys)
			{
				var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
				{
					DefinitionId = tf
				});
				if (spec == null) continue;
				var classification = ModelMap.GetClassification(spec);
                
				ArtifactPrinter.AddArtifactSpecification(PrintController.Document, spec.Artifact, classification, true);
				SpecificationPrinter.AddSpecificationProperties(PrintController.Document, spec, true);
			}
			PrintController.Save();
			Utils.InsertCustomWatermark(PrintController.Document, waterMark);
			Utils.AddFooter(PrintController.Document, "Token Taxonomy Framework" + " - " + ModelManager.Taxonomy.Version.Version + ": " + ModelManager.Taxonomy.Version.StateHash);
			PrintController.Save();

			PrintController.Document.Close();
		}

		private static void AddTaxonomyInfo(WordprocessingDocument document, TaxonomyVersion version)
        {
            _log.Info("Printing Taxonomy Info: " + version.Id );

            var body = document.MainDocumentPart.Document.Body;
            
            var title = body.AppendChild(new Paragraph());
            var titleRun = title.AppendChild(new Run());
            titleRun.AppendChild(new Text("Token Taxonomy Framework") { Space = SpaceProcessingModeValues.Preserve });
           
            Utils.ApplyStyleToParagraph(document, "Title", "Title", title, JustificationValues.Center);
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
	            pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }
	}
}