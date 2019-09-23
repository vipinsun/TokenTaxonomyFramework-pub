using System;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using TTI.TTF.Taxonomy.Model;
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

        internal static void InitWorkingDocument(string styleSource)
        {
            Document =
                WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
            _log.Info("Artifact file: " + _filePath + " created");
            // Add a main document part.     
            var mainPart = Document.AddMainDocumentPart();

            // Create the document structure and add some text.
            mainPart.Document = new Document();

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

            ArtifactPrinter.AddArtifactContent(Document, baseToPrint.Artifact, false, false);
            BasePrinter.AddBaseProperties(Document, baseToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, baseToPrint.Artifact.Name);
            Save();
            
            Document.Close();
        }

        public static void PrintBehavior(string fileName, string waterMark, string styleSource, Model.Core.Behavior behaviorToPrint)
        {
            _log.Info("Print Controller printing Behavior: " + behaviorToPrint.Artifact.Name);

            var trimName = behaviorToPrint.Artifact.Name.Replace(" ", "");
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

            ArtifactPrinter.AddArtifactContent(Document, behaviorToPrint.Artifact, false, false);
            BehaviorPrinter.AddBehaviorProperties(Document, behaviorToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, behaviorToPrint.Artifact.Name);
            Save();
            
            Document.Close();
        }

        public static void PrintBehaviorGroup(string fileName, string waterMark, string styleSource, BehaviorGroup behaviorGroupToPrint)
        {
            _log.Info("Print Controller printing Behavior Group: " + behaviorGroupToPrint.Artifact.Name);

            var trimName = behaviorGroupToPrint.Artifact.Name.Replace(" ", "");
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
            ArtifactPrinter.AddArtifactContent(Document, behaviorGroupToPrint.Artifact, false,false);
            BehaviorGroupPrinter.AddBehaviorGroupProperties(Document, behaviorGroupToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, behaviorGroupToPrint.Artifact.Name);
            Save();
            Document.Close();
        }

        public static void PrintPropertySet(string fileName, string waterMark, string styleSource, PropertySet psToPrint)
        {
            _log.Info("Print Controller printing Property Set: " + psToPrint.Artifact.Name);

            var trimName = psToPrint.Artifact.Name.Replace(" ", "");
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
           
            ArtifactPrinter.AddArtifactContent(Document, psToPrint.Artifact, false,false);
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
           
            ArtifactPrinter.AddArtifactContent(Document, formulaToPrint.Artifact,false, false);
            FormulaPrinter.AddFormulaProperties(Document, formulaToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, formulaToPrint.Artifact.Name);
            Save();

            Document.Close();
        }
        
        public static void PrintDefinition(string fileName, string waterMark, string styleSource, TemplateDefinition definitionToPrint)
        {
            _log.Info("Print Controller printing Property Set: " + definitionToPrint.Artifact.Name);

            var trimName = definitionToPrint.Artifact.Name.Replace(" ", "");
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
           
            ArtifactPrinter.AddArtifactContent(Document, definitionToPrint.Artifact, false,false);
            DefinitionPrinter.AddDefinitionProperties(Document, definitionToPrint, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, definitionToPrint.Artifact.Name);
            Save();

            Document.Close();
        }
        
        public static void PrintSpec(string fileName, string waterMark, string styleSource, TokenSpecification specification)
        {
            _log.Info("Print Controller printing Property Set: " + specification.Artifact.Name);

            var trimName = specification.Artifact.Name.Replace(" ", "");
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

            var classification = ModelMap.GetClassification(specification);
                
            ArtifactPrinter.AddArtifactSpecification(Document, specification.Artifact, classification, true);
            SpecificationPrinter.AddSpecificationProperties(Document, specification, false);
            Utils.InsertCustomWatermark(Document, waterMark);
            Utils.AddFooter(Document, specification.Artifact.Name + " - " + specification.SpecificationHash);
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
