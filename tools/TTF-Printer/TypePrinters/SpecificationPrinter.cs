using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Reflection;
using DocumentFormat.OpenXml.Wordprocessing;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    internal static class SpecificationPrinter
    {
        private static readonly ILog _log;
        static SpecificationPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddSpecificationProperties(WordprocessingDocument document, TokenSpecification spec)
        {
            _log.Info("Printing Token Specification Properties: " + spec.Artifact.Name);
            var body = document.MainDocumentPart.Document.Body;

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Token Specification"));
            Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", aDef, JustificationValues.Center);
            
            BasePrinter.AddBaseSpecification(document, spec.TokenBase);

            var beDef = body.AppendChild(new Paragraph());
            var beRun = beDef.AppendChild(new Run());
            beRun.AppendChild(new Text("Behaviors"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", beDef, JustificationValues.Center);
            foreach (var b in spec.Behaviors)
            {
                BehaviorPrinter.AddBehaviorSpecification(document, b);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", bbDef, JustificationValues.Center);
            }
            
            var bgDef = body.AppendChild(new Paragraph());
            var bgRun = bgDef.AppendChild(new Run());
            bgRun.AppendChild(new Text("Behavior Groups"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bgDef, JustificationValues.Center);
            foreach (var bg in spec.BehaviorGroups)
            {
                ArtifactPrinter.AddArtifactReference(document, bg.Reference);
                foreach (var b in bg.BehaviorArtifacts)
                {
                    var bbeDef = body.AppendChild(new Paragraph());
                    var bbeRun = bbeDef.AppendChild(new Run());
                    bbeRun.AppendChild(new Text("Behavior"));
                    Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbeDef);

                    BehaviorPrinter.AddBehaviorReferenceProperties(document, b);
                }
            }
            
            var pDef = body.AppendChild(new Paragraph());
            var pRun = pDef.AppendChild(new Run());
            pRun.AppendChild(new Text("Property Sets"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pDef, JustificationValues.Center);
            foreach (var p in spec.PropertySets)
            {
                ArtifactPrinter.AddArtifactReference(document, p.Reference);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
                CommonPrinter.BuildPropertiesSpecificationTable(document, p.Properties);
            }
            
            var cDef = body.AppendChild(new Paragraph());
            var cRun = cDef.AppendChild(new Run());
            cRun.AppendChild(new Text("Child Tokens"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cDef, JustificationValues.Center);
            foreach (var c in spec.ChildTokens)
            {
                AddDefinitionProperties(document, c);
                var bbDef = body.AppendChild(new Paragraph());
                var bbRun = bbDef.AppendChild(new Run());
                bbRun.AppendChild(new Text(""));
                Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", bbDef, JustificationValues.Center);
            }
        }
    }
}
