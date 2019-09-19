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

        public static void AddPropertySetProperties(WordprocessingDocument document, PropertySet ps)
        {
            _log.Info("Printing Property Set Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Property Set"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            CommonPrinter.BuildPropertiesTable(document, ps.Properties);
        }
        
        public static void AddPropertySetSpecification(WordprocessingDocument document, PropertySetSpecification ps)
        {
            _log.Info("Printing Property Set Properties: " + ps.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Property Set"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);

            CommonPrinter.BuildPropertiesTable(document, ps.Properties);
        }
    }
}
