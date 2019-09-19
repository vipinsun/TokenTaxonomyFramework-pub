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
        private static WordprocessingDocument _document;
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
            _document =
                WordprocessingDocument.Create(_filePath, WordprocessingDocumentType.Document);
            _log.Info("Artifact file: " + _filePath + " created");
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

        public static void PrintBase(string fileName, string waterMark, string styleSource, Base baseToPrint)
        {
            _log.Info("Print Controller printing Base: " + baseToPrint.Artifact.Name);

            var trimName = baseToPrint.Artifact.Name.Replace(" ", "");
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

            ArtifactPrinter.AddArtifactContent(_document, baseToPrint.Artifact, false);
            BasePrinter.AddBaseProperties(_document, baseToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, baseToPrint.Artifact.Name);
            Save();
            
            _document.Close();
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

            ArtifactPrinter.AddArtifactContent(_document, behaviorToPrint.Artifact, false);
            BehaviorPrinter.AddBehaviorProperties(_document, behaviorToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, behaviorToPrint.Artifact.Name);
            Save();
            
            _document.Close();
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
            ArtifactPrinter.AddArtifactContent(_document, behaviorGroupToPrint.Artifact, false);
            BehaviorGroupPrinter.AddBehaviorGroupProperties(_document, behaviorGroupToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, behaviorGroupToPrint.Artifact.Name);
            Save();
            _document.Close();
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
           
            ArtifactPrinter.AddArtifactContent(_document, psToPrint.Artifact, false);
            PropertySetPrinter.AddPropertySetProperties(_document, psToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, psToPrint.Artifact.Name);
            Save();

            _document.Close();
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
           
            ArtifactPrinter.AddArtifactContent(_document, formulaToPrint.Artifact, false);
            FormulaPrinter.AddFormulaProperties(_document, formulaToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, formulaToPrint.Artifact.Name);
            Save();

            _document.Close();
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
           
            ArtifactPrinter.AddArtifactContent(_document, definitionToPrint.Artifact, false);
            DefinitionPrinter.AddDefinitionProperties(_document, definitionToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, definitionToPrint.Artifact.Name);
            Save();

            _document.Close();
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
                
            ArtifactPrinter.AddArtifactSpecification(_document, specification.Artifact, classification, true);
            SpecificationPrinter.AddSpecificationProperties(_document, specification);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, specification.Artifact.Name + " - " + specification.SpecificationHash);
            Save();

            _document.Close();
        }

       

        internal static void PrintTtf()
        {
            throw new NotImplementedException();
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
