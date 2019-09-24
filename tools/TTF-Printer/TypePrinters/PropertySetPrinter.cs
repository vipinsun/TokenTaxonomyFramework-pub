using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using System.Reflection;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    internal static class PropertySetPrinter
    {
        private static readonly ILog _log;
        static PropertySetPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddPropertySetProperties(WordprocessingDocument document, PropertySet ps, bool book, bool isForAppendix = false)
        {
            ArtifactPrinter.AddArtifactContent(document, ps.Artifact, book,isForAppendix);
            _log.Info("Printing Property Set Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Property Set"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            CommonPrinter.BuildPropertiesTable(document, ps.Properties, book);
            
            if(!book) return;
            var pageBreak = body.AppendChild(new Paragraph());
            var pbr = pageBreak.AppendChild(new Run());
            pbr.AppendChild(new Text(""));

            if (pageBreak.ParagraphProperties == null)
            {
                pageBreak.ParagraphProperties = new ParagraphProperties();
            }

            pageBreak.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }
        
        public static void AddPropertySetSpecification(WordprocessingDocument document, PropertySetSpecification ps)
        {
            ArtifactPrinter.AddArtifactContent(document, ps.Artifact, false, true);
            _log.Info("Printing Property Set Specification Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Property Set Details"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);

            CommonPrinter.BuildPropertySpecificationTable(document, ps.Properties);
        }
    }
}
