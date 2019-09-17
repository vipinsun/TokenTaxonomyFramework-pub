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
        private static WordprocessingDocument _document;
        private static readonly ILog _log;
        internal static string OutputFolder = "";
        internal static string filePath = "";
        static PrintController()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void PrintBase(string fileName, string waterMark, string styleSource, Base baseToPrint)
        {
     
            _log.Info("Print Controller printing Base: " + baseToPrint.Artifact.Name);

            string trimName = baseToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BaseFolder + ModelMap.FolderSeparator +
                trimName;
            filePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                _document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            }
            catch(Exception ex)
            {
                _log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            // Add a main document part. 
            var mainPart = _document.AddMainDocumentPart();

            mainPart.Document = new Document();
            // Get the Styles part for this document.
            Utils.AddStylesPartToPackage(_document);
            var styles = Utils.ExtractStylesPart(styleSource);
            Utils.ReplaceStylesPart(_document, styles);

            Save();
            ArtifactPrinter.AddArtifactContent(_document, baseToPrint.Artifact, true);
            BasePrinter.AddBaseProperties(_document, baseToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, baseToPrint.Artifact.Name);
            Save();

            if (!Utils.ValidateWordDocument(_document))
                Utils.ValidateCorruptedWordDocument(_document);
            _document.Close();
        }

        public static void PrintBehavior(string fileName, string waterMark, string styleSource, Model.Core.Behavior behaviorToPrint)
        {
            _log.Info("Print Controller printing Behavior: " + behaviorToPrint.Artifact.Name);

            string trimName = behaviorToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BehaviorFolder + ModelMap.FolderSeparator +
                trimName;
            filePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                _document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            // Add a main document part. 
            var mainPart = _document.AddMainDocumentPart();

            mainPart.Document = new Document();
            // Get the Styles part for this document.
            Utils.AddStylesPartToPackage(_document);
            var styles = Utils.ExtractStylesPart(styleSource);
            Utils.ReplaceStylesPart(_document, styles);

            Save();
            ArtifactPrinter.AddArtifactContent(_document, behaviorToPrint.Artifact, true);
            BehaviorPrinter.AddBehaviorProperties(_document, behaviorToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, behaviorToPrint.Artifact.Name);
            Save();

            if (!Utils.ValidateWordDocument(_document))
                Utils.ValidateCorruptedWordDocument(_document);
            _document.Close();
        }

        public static void PrintBehaviorGroup(string fileName, string waterMark, string styleSource, BehaviorGroup behaviorGroupToPrint)
        {
            _log.Info("Print Controller printing Behavior Group: " + behaviorGroupToPrint.Artifact.Name);

            string trimName = behaviorGroupToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BehaviorGroupFolder + ModelMap.FolderSeparator +
                trimName;
            filePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                _document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            }
            catch (Exception ex)
            {
                _log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                _log.Error(ex);
                return;
            }
            // Add a main document part. 
            var mainPart = _document.AddMainDocumentPart();

            mainPart.Document = new Document();
            // Get the Styles part for this document.
            Utils.AddStylesPartToPackage(_document);
            var styles = Utils.ExtractStylesPart(styleSource);
            Utils.ReplaceStylesPart(_document, styles);

            Save();
            ArtifactPrinter.AddArtifactContent(_document, behaviorGroupToPrint.Artifact, true);
            BehaviorGroupPrinter.AddBehaviorGroupProperties(_document, behaviorGroupToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, behaviorGroupToPrint.Artifact.Name);
            Save();

            if (!Utils.ValidateWordDocument(_document))
                Utils.ValidateCorruptedWordDocument(_document);
            _document.Close();
        }

        internal static void PrintTTF(Model.Taxonomy ttf)
        {
            throw new NotImplementedException();
        }

        internal static void Save()
        {
            _document.MainDocumentPart.Document.Save();
        }

    }
}
