using System;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using TTI.TTF.Taxonomy.Model;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using TTI.TTF.Taxonomy.TypePrinters;

namespace TTI.TTF.Taxonomy
{
    public static class PrintController
    {
        internal static WordprocessingDocument Document;
        private static readonly ILog _log;
        private static string _outputFolder = "";
        private static string _filePath = "";

        static PrintController()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            #endregion
        }

        private static void InitWorkingDocument(string styleSource)
        {
            Document =
                WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
            _log.Info("Artifact file: " + _filePath + " created");
            // Add a main document part.     
            var mainPart = Document.AddMainDocumentPart();

            // Create the document structure 
            mainPart.Document = new Document();
            mainPart.Document.AppendChild(new Body());
            
            // Get the Styles part for this document.
            Utils.AddStylesPartToPackage(Document);
            var styles = Utils.ExtractStylesPart(styleSource);
            Utils.ReplaceStylesPart(Document, styles);
            Save();
        }

        public static void PrintBase(string fileName, string waterMark, string styleSource, Base baseToPrint)
        {
            _log.Info("Print Controller printing Base: " + baseToPrint.Artifact.Name);

            var trimName = ModelMap.GetBaseFolderName(baseToPrint.TokenType, baseToPrint.TokenUnit,
                baseToPrint.RepresentationType);
            _outputFolder = fileName +  ModelMap.BaseFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            BasePrinter.PrintTokenBase(Document, baseToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, baseToPrint.Artifact.Name);
            Save();
            
            Document.Close();
        }

        public static void PrintBehavior(string fileName, string waterMark, string styleSource, Model.Core.Behavior behaviorToPrint)
        {
            _log.Info("Print Controller printing Behavior: " + behaviorToPrint.Artifact.Name);

            var trimName = behaviorToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = fileName +  ModelMap.BehaviorFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }

            BehaviorPrinter.PrintBehavior(Document, behaviorToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, behaviorToPrint.Artifact.Name);
            Save();
            
            Document.Close();
        }

        public static void PrintBehaviorGroup(string fileName, string waterMark, string styleSource, BehaviorGroup behaviorGroupToPrint)
        {
            _log.Info("Print Controller printing Behavior Group: " + behaviorGroupToPrint.Artifact.Name);

            var trimName = behaviorGroupToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = fileName +  ModelMap.BehaviorGroupFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            // Add a main document part. 
            BehaviorGroupPrinter.PrintBehaviorGroup(Document, behaviorGroupToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, behaviorGroupToPrint.Artifact.Name);
            Save();
            Document.Close();
        }

        public static void PrintPropertySet(string fileName, string waterMark, string styleSource, PropertySet psToPrint)
        {
            _log.Info("Print Controller printing Property Set: " + psToPrint.Artifact.Name);

            var trimName = psToPrint.Artifact.Name.Replace(" ", "-").ToLower();
            _outputFolder = fileName +  ModelMap.PropertySetFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }

            PropertySetPrinter.AddPropertySetProperties(Document, psToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, psToPrint.Artifact.Name);
            Save();

            Document.Close();
        }

        public static void PrintFormula(string fileName, string waterMark, string styleSource, TemplateFormula formulaToPrint)
        {
            _log.Info("Print Controller printing Property Set: " + formulaToPrint.Artifact.Name);

            var trimName = formulaToPrint.Artifact.Name.Replace(" ", "");
            _outputFolder = fileName +  ModelMap.TemplateFormulasFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
           
            FormulaPrinter.PrintFormula(Document, formulaToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, formulaToPrint.Artifact.Name);
            Save();

            Document.Close();
        }
        
        public static void PrintDefinition(string fileName, string waterMark, string styleSource, TemplateDefinition definitionToPrint)
        {
            _log.Info("Print Controller printing Property Set: " + definitionToPrint.Artifact.Name);

            var trimName = definitionToPrint.Artifact.Name.Replace(" ", "-");
            _outputFolder = fileName +  ModelMap.TemplateDefinitionsFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
           
            DefinitionPrinter.PrintDefinition(Document, definitionToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, definitionToPrint.Artifact.Name);
            Save();

            Document.Close();
        }
        
        public static void PrintSpec(string fileName, string waterMark, string styleSource, TokenSpecification specification)
        {
            _log.Info("Print Controller printing Property Set: " + specification.Artifact.Name);

            var trimName = specification.Artifact.Name.Replace(" ", "-");
            _outputFolder = fileName +  ModelMap.SpecificationsFolder + ModelMap.FolderSeparator + trimName + 
                            ModelMap.FolderSeparator + ModelMap.Latest;
            _filePath = _outputFolder + ModelMap.FolderSeparator + trimName + "-spec.docx";
            try
            {
                Directory.CreateDirectory(_outputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            
            SpecificationPrinter.PrintSpecification(Document, specification, true);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, specification.Artifact.Name + " - " + specification.SpecificationHash);
            Save();

            Document.Close();
        }
        
        internal static void PrintAllArtifacts(string filePath, string waterMark, string styleSource)
        {
            _log.Info("Taxonomy Printer: Printing All Artifacts");

            //bases
            foreach (var baseToken in ModelManager.Taxonomy.BaseTokenTypes)
            {
                PrintBase(filePath, waterMark, styleSource, baseToken.Value);
            }

            foreach (var behavior in ModelManager.Taxonomy.Behaviors)
            {
                PrintBehavior(filePath, waterMark, styleSource, behavior.Value);
            }

            foreach (var bg in ModelManager.Taxonomy.BehaviorGroups)
            {
                PrintBehaviorGroup(filePath, waterMark, styleSource, bg.Value);
            }

            foreach (var ps in ModelManager.Taxonomy.PropertySets)
            {
                PrintPropertySet(filePath, waterMark, styleSource, ps.Value);
            }

            foreach (var f in ModelManager.Taxonomy.TemplateFormulas)
            {
                PrintFormula(filePath, waterMark, styleSource, f.Value);
            }

            foreach (var d in ModelManager.Taxonomy.TemplateDefinitions)
            {
                PrintDefinition(filePath, waterMark, styleSource, d.Value);
                var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
                {
                    DefinitionId = d.Key
                });

                if (spec == null) continue;
                if (spec.Artifact.Name.Contains("Error:"))
                {
                    _log.Error(spec.Artifact.Name);
                    return;
                }
                PrintSpec(filePath, waterMark, styleSource, spec);
            }
        }
        
        /// <summary>
        /// This will create a single OpenXML document.  After it is created, it should be opened and a new Table of Contents before printing to PDF.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="waterMark"></param>
        /// <param name="styleSource"></param>
        internal static void BuildTtfBook(string filePath, string waterMark, string styleSource)
		{
            _filePath = filePath + ModelMap.FolderSeparator + ".." + ModelMap.FolderSeparator + "TTF-Book.docx";
			try
			{
				InitWorkingDocument(styleSource);
                Document.MainDocumentPart.Document.AppendChild(new Body());
			}
			catch (Exception ex)
			{
				_log.Error("Artifact Output Folder: " + _outputFolder + " cannot be created.");
				_log.Error(ex);
				return;
			}

			CommonPrinter.AddTaxonomyInfo(Document, ModelManager.Taxonomy.Version);
			
			_log.Info("Adding Bases");
            CommonPrinter.AddSectionPage(Document, "Base Tokens");
			foreach (var b in ModelManager.Taxonomy.BaseTokenTypes.Values)
			{
				BasePrinter.PrintTokenBase(Document, b, true);
			}
			Save();
            
			_log.Info("Adding Behaviors");
            CommonPrinter.AddSectionPage(Document, "Behaviors");
			foreach (var b in ModelManager.Taxonomy.Behaviors.Values)
			{
                BehaviorPrinter.PrintBehavior(Document, b, true);
			}
			Save();
            
			_log.Info("Adding Behavior-Groups");
            CommonPrinter.AddSectionPage(Document, "Behavior Groups");
			foreach (var b in ModelManager.Taxonomy.BehaviorGroups.Values)
			{
				BehaviorGroupPrinter.PrintBehaviorGroup(Document, b, true);
			}
			Save();
            
            _log.Info("Adding Property-Sets");
            CommonPrinter.AddSectionPage(Document, "Property Sets");
            foreach (var ps in ModelManager.Taxonomy.PropertySets.Values)
			{
				PropertySetPrinter.AddPropertySetProperties(Document, ps, true);
			}
			Save();
            
			_log.Info("Adding Template Formulas");
            CommonPrinter.AddSectionPage(Document, "Template Formulas");
            foreach (var tf in ModelManager.Taxonomy.TemplateFormulas.Values)
			{
				FormulaPrinter.PrintFormula(Document, tf, true);
			}
			Save();
            
			_log.Info("Adding Template Definitions");
            CommonPrinter.AddSectionPage(Document, "Template Definitions");
            foreach (var td in ModelManager.Taxonomy.TemplateDefinitions.Values)
			{
				DefinitionPrinter.PrintDefinition(Document, td, true);
			}
			Save();
            
			_log.Info("Adding Specifications");
            CommonPrinter.AddSectionPage(Document, "Token Specifications");
            foreach (var tf in ModelManager.Taxonomy.TemplateDefinitions.Keys)
			{
				var spec = Printer.TaxonomyClient.GetTokenSpecification(new TokenTemplateId
				{
					DefinitionId = tf
				});
				if (spec == null) continue;
				SpecificationPrinter.PrintSpecification(Document, spec, false);
			}
			Save();
            
			Utils.InsertCustomWatermark(Document, waterMark);
			Utils.AddFooter(Document, "Token Taxonomy Framework" + " - " + ModelManager.Taxonomy.Version.Version + ": " + ModelManager.Taxonomy.Version.StateHash);
			Save();
            Document.Close();
		}
        
        internal static void Save(bool validate = false)
        {
            Document.MainDocumentPart.Document.Save();
            _log.Info("Document saved: " + _filePath);
            if (!validate) return;
            Document.Close();
            Document = WordprocessingDocument.Open(_filePath,false);
                
            if (!Utils.ValidateWordDocument(Document))
                Utils.ValidateCorruptedWordDocument(Document);
            Document.Close();
            Document = WordprocessingDocument.Open(_filePath, true);
        }

    }
}
