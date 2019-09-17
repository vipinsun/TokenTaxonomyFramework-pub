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
        internal static WordprocessingDocument DocumentClone;
        private static readonly ILog Log;
        internal static string OutputFolder = "";
        internal static string FilePath = "";
        public static StylesPart StylesPart { get; set; }
        static PrintController()
        {
            #region logging

            Utils.InitLog();
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            #endregion
        }

        private static void InitWorkingDocument(string styleSource)
        {
            _document =
                WordprocessingDocument.Create(FilePath, WordprocessingDocumentType.Document, true);

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
            Log.Info("Print Controller printing Base: " + baseToPrint.Artifact.Name);

            var trimName = baseToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BaseFolder + ModelMap.FolderSeparator +
                           trimName;
            FilePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                Log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                Log.Error(ex);
                return;
            }

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
            Log.Info("Print Controller printing Behavior: " + behaviorToPrint.Artifact.Name);

            string trimName = behaviorToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BehaviorFolder + ModelMap.FolderSeparator +
                trimName;
            FilePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                Log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                Log.Error(ex);
                return;
            }

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
            Log.Info("Print Controller printing Behavior Group: " + behaviorGroupToPrint.Artifact.Name);

            string trimName = behaviorGroupToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.BehaviorGroupFolder + ModelMap.FolderSeparator +
                trimName;
            FilePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                Log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                Log.Error(ex);
                return;
            }
            // Add a main document part. 
            ArtifactPrinter.AddArtifactContent(_document, behaviorGroupToPrint.Artifact, true);
            BehaviorGroupPrinter.AddBehaviorGroupProperties(_document, behaviorGroupToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, behaviorGroupToPrint.Artifact.Name);
            Save();

            if (!Utils.ValidateWordDocument(_document))
                Utils.ValidateCorruptedWordDocument(_document);
            _document.Close();
        }

        public static void PrintPropertySet(string fileName, string waterMark, string styleSource, PropertySet psToPrint)
        {
            Log.Info("Print Controller printing Property Set: " + psToPrint.Artifact.Name);

            string trimName = psToPrint.Artifact.Name.Replace(" ", "");
            OutputFolder = fileName + ModelMap.FolderSeparator + ModelMap.PropertySetFolder + ModelMap.FolderSeparator +
                trimName;
            FilePath = OutputFolder + ModelMap.FolderSeparator + trimName + ".docx";
            try
            {
                Directory.CreateDirectory(OutputFolder);
                InitWorkingDocument(styleSource);
            }
            catch (Exception ex)
            {
                Log.Error("Artifact Output Folder: " + OutputFolder + " cannot be created.");
                Log.Error(ex);
                return;
            }
           
            ArtifactPrinter.AddArtifactContent(_document, psToPrint.Artifact, true);
            PropertySetPrinter.AddPropertySetProperties(_document, psToPrint);
            Utils.InsertCustomWatermark(_document, waterMark);
            Utils.AddFooter(_document, psToPrint.Artifact.Name);
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
