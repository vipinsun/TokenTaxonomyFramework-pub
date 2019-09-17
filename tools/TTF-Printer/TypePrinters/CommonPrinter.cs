using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Collections.Generic;
using System.Reflection;
using TTI.TTF.Taxonomy.Model.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.Collections;
using System;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    static class CommonPrinter
    {
        private static readonly ILog _log;
        static CommonPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void BuildInvocationsTable(WordprocessingDocument document, IEnumerable<Invocation> invocations)
        {
            _log.Info("Printing InvocationsTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text("Invocations"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", invokes, JustificationValues.Center);

            foreach (var i in invocations) {

                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text(i.Name));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", exBody, JustificationValues.Center);

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Id: " + i.Id));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text(i.Description));
                Utils.ApplyStyleToParagraph(document, "Quote", "Quote", iDescription);

                var requestBody = body.AppendChild(new Paragraph());
                var requestRun = requestBody.AppendChild(new Run());
                requestRun.AppendChild(new Text("Request"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", requestBody);

                var reqBody = body.AppendChild(new Paragraph());
                var reqRun = reqBody.AppendChild(new Run());
                reqRun.AppendChild(new Text("Control Message: " + i.Request.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", reqBody);

                var dBody = body.AppendChild(new Paragraph());
                var dRun = dBody.AppendChild(new Run());
                dRun.AppendChild(new Text("Description: " + i.Request.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", dBody);

                var paramsBody = body.AppendChild(new Paragraph());
                var paramsRun = paramsBody.AppendChild(new Run());
                paramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", paramsBody);

                var invokePara = body.AppendChild(new Paragraph());
                var invokeRun = invokePara.AppendChild(new Run());
                invokeRun.AppendChild(GetParamsTable(document, i.Request.InputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", invokePara);

                //response
                var responseBody = body.AppendChild(new Paragraph());
                var responseRun = responseBody.AppendChild(new Run());
                responseRun.AppendChild(new Text("Response"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", responseBody);

                var resBody = body.AppendChild(new Paragraph());
                var resRun = resBody.AppendChild(new Run());
                resRun.AppendChild(new Text("Control Message: " + i.Response.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", resBody);

                var rBody = body.AppendChild(new Paragraph());
                var rRun = rBody.AppendChild(new Run());
                rRun.AppendChild(new Text("Description: " + i.Response.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rBody);

                var ramsBody = body.AppendChild(new Paragraph());
                var ramsRun = ramsBody.AppendChild(new Run());
                ramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", ramsBody);

                var rinvokePara = body.AppendChild(new Paragraph());
                var rinvokeRun = rinvokePara.AppendChild(new Run());
                rinvokeRun.AppendChild(GetParamsTable(document, i.Response.OutputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rinvokePara);

            }
        }

        public static void BuildInvocationTable(WordprocessingDocument document, Invocation invocation)
        {
            _log.Info("Printing InvocationTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text(invocation.Name));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", invokes, JustificationValues.Center);


                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Id: " + invocation.Id));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text(invocation.Description));
                Utils.ApplyStyleToParagraph(document, "Quote", "Quote", iDescription);

                var requestBody = body.AppendChild(new Paragraph());
                var requestRun = requestBody.AppendChild(new Run());
                requestRun.AppendChild(new Text("Request"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", requestBody);

                var reqBody = body.AppendChild(new Paragraph());
                var reqRun = reqBody.AppendChild(new Run());
                reqRun.AppendChild(new Text("Control Message: " + invocation.Request.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", reqBody);

                var dBody = body.AppendChild(new Paragraph());
                var dRun = dBody.AppendChild(new Run());
                dRun.AppendChild(new Text("Description: " + invocation.Request.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", dBody);

                var paramsBody = body.AppendChild(new Paragraph());
                var paramsRun = paramsBody.AppendChild(new Run());
                paramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", paramsBody);

                var invokePara = body.AppendChild(new Paragraph());
                var invokeRun = invokePara.AppendChild(new Run());
                invokeRun.AppendChild(GetParamsTable(document, invocation.Request.InputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", invokePara);

                //response
                var responseBody = body.AppendChild(new Paragraph());
                var responseRun = responseBody.AppendChild(new Run());
                responseRun.AppendChild(new Text("Response"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", responseBody);

                var resBody = body.AppendChild(new Paragraph());
                var resRun = resBody.AppendChild(new Run());
                resRun.AppendChild(new Text("Control Message: " + invocation.Response.ControlMessageName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", resBody);

                var rBody = body.AppendChild(new Paragraph());
                var rRun = rBody.AppendChild(new Run());
                rRun.AppendChild(new Text("Description: " + invocation.Response.Description));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rBody);

                var ramsBody = body.AppendChild(new Paragraph());
                var ramsRun = ramsBody.AppendChild(new Run());
                ramsRun.AppendChild(new Text("Parameters"));
                Utils.ApplyStyleToParagraph(document, "Heading4", "Heading4", ramsBody);

                var rinvokePara = body.AppendChild(new Paragraph());
                var rinvokeRun = rinvokePara.AppendChild(new Run());
                rinvokeRun.AppendChild(GetParamsTable(document, invocation.Response.OutputParameters));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rinvokePara);
        }

        internal static void BuildInfluenceBindings(WordprocessingDocument document, IEnumerable<InfluenceBinding> influenceBindings)
        {
            _log.Info("Printing InvocationsTable");
            var body = document.MainDocumentPart.Document.Body;

            var invokes = body.AppendChild(new Paragraph());
            var ivRun = invokes.AppendChild(new Run());
            ivRun.AppendChild(new Text("Influence Bindings"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", invokes, JustificationValues.Center);

            foreach (var i in influenceBindings)
            {

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Influneced Id: " + i.InfluencedId));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var influencedName = "Replace with Influenced name";
                var indBody = body.AppendChild(new Paragraph());
                var indRun = indBody.AppendChild(new Run());
                indRun.AppendChild(new Text("Influneced Name: " + influencedName));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", indBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Influenced Invocation Id: " + i.InfluencedInvocationId));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                var requestBody = body.AppendChild(new Paragraph());
                var requestRun = requestBody.AppendChild(new Run());
                requestRun.AppendChild(new Text("Influence Type: " + i.InfluenceType.ToString()));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", requestBody);

                var reqBody = body.AppendChild(new Paragraph());
                var reqRun = reqBody.AppendChild(new Run());
                reqRun.AppendChild(new Text("Influencing Invocation: "));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", reqBody, JustificationValues.Center);

                BuildInvocationTable(document, i.InfluencingInvocation);

                var resBody = body.AppendChild(new Paragraph());
                var resRun = resBody.AppendChild(new Run());
                resRun.AppendChild(new Text("Influenced Invocation: "));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", resBody, JustificationValues.Center);

                BuildInvocationTable(document, i.InfluencedInvocation);
            }
        }

        internal static void BuildPropertiesTable(WordprocessingDocument document, RepeatedField<Property> properties)
        {
            _log.Info("Printing InvocationsTable");
            var body = document.MainDocumentPart.Document.Body;
            var propertPara = body.AppendChild(new Paragraph());
            var ivRun = propertPara.AppendChild(new Run());
            ivRun.AppendChild(new Text("Properties"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", propertPara, JustificationValues.Center);

            foreach (var p in properties)
            {
                var exBody = body.AppendChild(new Paragraph());
                var exRun = exBody.AppendChild(new Run());
                exRun.AppendChild(new Text(p.Name));
                Utils.ApplyStyleToParagraph(document, "Subtitle", "Subtitle", exBody, JustificationValues.Center);

                var idBody = body.AppendChild(new Paragraph());
                var idRun = idBody.AppendChild(new Run());
                idRun.AppendChild(new Text("Value Description: " + p.ValueDescription));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", idBody);

                var iDescription = body.AppendChild(new Paragraph());
                var iRun = iDescription.AppendChild(new Run());
                iRun.AppendChild(new Text("Template Value: " + p.TemplateValue));
                Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iDescription);

                BuildInvocationsTable(document, p.PropertyInvocations);
                BuildPropertiesTable(document, p.Properties);

            }
        }

        public static Table GetParamsTable(WordprocessingDocument document, IEnumerable<InvocationParameter> parameters)
        {
            //dependencies table should be the 4th table
            var paramsTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "35" }));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "65" }));
            tr1.Append(name1);
            tr1.Append(description1);
            paramsTable.Append(tr1);
            foreach (var d in parameters)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(d.Name))));
                description.Append(new Paragraph(new Run(new Text(d.ValueDescription))));
         
                tr.Append(name);
                tr.Append(description);
                paramsTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", paramsTable);
            return paramsTable;
        }
    }
}
