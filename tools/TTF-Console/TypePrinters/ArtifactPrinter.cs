using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using TTI.TTF.Taxonomy.Model.Artifact;
using log4net;
using System.Reflection;
using static DocumentFormat.OpenXml.Wordprocessing.TableWidthUnitValues;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    internal static class ArtifactPrinter {
        private static readonly ILog Log;

        static ArtifactPrinter()
        {
            #region logging

            Utils.InitLog();
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddArtifactContent(WordprocessingDocument document, Artifact artifact,
            bool isTopArtifact)
        {
            Log.Info("Printing Artifact: " + artifact.Name + " is top artifact " + isTopArtifact);

            var body = document.MainDocumentPart.Document.AppendChild(new Body());

            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(artifact.Name) { Space = SpaceProcessingModeValues.Preserve });
            if (isTopArtifact)
                Utils.ApplyStyleToParagraph(document, "Title", "Title", para, JustificationValues.Center);
            else
                Utils.ApplyStyleToParagraph(document, "Heading1", "Heading1", para, JustificationValues.Center);

            GenerateArtifactSymbol(document, artifact.ArtifactSymbol);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Definition"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aDef);


            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
            Utils.ApplyStyleToParagraph(document, "Quote", "Quote", bizBody);

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Example"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", eDef);

            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", exBody);

            var aPara = body.AppendChild(new Paragraph());
            var aRun = aPara.AppendChild(new Run());
            aRun.AppendChild(new Text("Analogies"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", aPara);

            var arPara = body.AppendChild(new Paragraph());
            var arRun = arPara.AppendChild(new Run());
            arRun.AppendChild(BuildAnalogies(document, artifact.ArtifactDefinition.Analogies));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", arPara);

            var coPara = body.AppendChild(new Paragraph());
            var coRun = coPara.AppendChild(new Run());
            coRun.AppendChild(new Text("Comments"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", coPara);

            var cmBody = body.AppendChild(new Paragraph());
            var cmRun = cmBody.AppendChild(new Run());
            cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", cmBody);

            var dPara = body.AppendChild(new Paragraph());
            var dRun = dPara.AppendChild(new Run());
            dRun.AppendChild(new Text("Dependencies"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", dPara);

            var ddPara = body.AppendChild(new Paragraph());
            var ddRun = ddPara.AppendChild(new Run());
            ddRun.AppendChild(BuildDependencyTable(document, artifact.Dependencies));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ddPara);

            var iPara = body.AppendChild(new Paragraph());
            var iRun = iPara.AppendChild(new Run());
            iRun.AppendChild(new Text("Incompatible With"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", iPara);

            var iiPara = body.AppendChild(new Paragraph());
            var iiRun = iiPara.AppendChild(new Run());
            iiRun.AppendChild(BuildIncompatibleTable(document, artifact.IncompatibleWithSymbols));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", iiPara);

            var fPara = body.AppendChild(new Paragraph());
            var fRun = fPara.AppendChild(new Run());
            fRun.AppendChild(new Text("Influenced By"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fPara);

            var ffPara = body.AppendChild(new Paragraph());
            var ffRun = ffPara.AppendChild(new Run());
            ffRun.AppendChild(BuildInfluencesTable(document, artifact.InfluencedBySymbols));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffPara);

            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", fiPara);

            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(document, artifact.ArtifactFiles));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ffiPara);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", cPara);

            var ccPara = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" })));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(document, artifact.Maps.CodeReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ccPara);

            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", pPara);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(document, artifact.Maps.ImplementationReferences));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", ppPara);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            Utils.ApplyStyleToParagraph(document, "Heading2", "Heading2", rPara);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(document, artifact.Maps.Resources));
            Utils.ApplyStyleToParagraph(document, "Normal", "Normal", rrPara);

            PrintController.Save();
        }

        public static void GenerateArtifactSymbol(WordprocessingDocument document, ArtifactSymbol symbol)
        {
            var basicProps = new[,]
            {
                {"Type:", symbol.Type.ToString()},
                {"Id:", symbol.Id},
                {"Visual:", symbol.Visual},
                {"Tooling:", symbol.Tooling},
                {"Version:", symbol.Version}
            };
            Utils.AddTable(document, basicProps);
        }

        private static Table BuildReferenceTable(WordprocessingDocument document, IEnumerable<MapResourceReference> resources)
        {
            var referenceMapTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            platform1.Append(new Paragraph(new Run(new Text("Location"))));
            platform1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "20" }));
            link1.Append(new Paragraph(new Run(new Text("Description"))));
            link1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "50" }));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
            referenceMapTable.Append(tr1);

            foreach (var im in resources)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var link = new TableCell();
                var description = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(im.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(im.Name))));
                link.Append(new Paragraph(new Run(new Text(im.ResourcePath))));
                description.Append(new Paragraph(new Run(new Text(im.Description))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(link);
                tr.Append(description);
                referenceMapTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", referenceMapTable);
            return referenceMapTable;
        }

        private static Table BuildImplementationTable(WordprocessingDocument document, IEnumerable<MapReference> references)
        {
            var implementationMapTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            platform1.Append(new Paragraph(new Run(new Text("Platform"))));
            platform1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            link1.Append(new Paragraph(new Run(new Text("Location"))));
            link1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "55" }));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
            implementationMapTable.Append(tr1);

            foreach (var im in references)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var platform = new TableCell();
                var link = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(im.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(im.Name))));
                platform.Append(new Paragraph(new Run(new Text(im.Platform.ToString()))));
                link.Append(new Paragraph(new Run(new Text(im.ReferencePath))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(platform);
                tr.Append(link);
                implementationMapTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", implementationMapTable);
            return implementationMapTable;
        }

        private static Table BuildCodeTable(WordprocessingDocument document, IEnumerable<MapReference> references)
        {
            var codeMapTable = new Table();

            var trH = new TableRow();
            var contentTypeH = new TableCell();
            var nameH = new TableCell();
            var platformH = new TableCell();
            var linkH = new TableCell();

            contentTypeH.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentTypeH.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            nameH.Append(new Paragraph(new Run(new Text("Name"))));
            nameH.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            platformH.Append(new Paragraph(new Run(new Text("Platform"))));
            platformH.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            linkH.Append(new Paragraph(new Run(new Text("Location"))));
            linkH.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "55" }));
            trH.Append(contentTypeH);
            trH.Append(nameH);
            trH.Append(platformH);
            trH.Append(linkH);
            codeMapTable.Append(trH);
            foreach (var c in references)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var name = new TableCell();
                var platform = new TableCell();
                var link = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(c.MappingType.ToString()))));
                name.Append(new Paragraph(new Run(new Text(c.Name))));
                platform.Append(new Paragraph(new Run(new Text(c.Platform.ToString()))));
                link.Append(new Paragraph(new Run(new Text(c.ReferencePath))));
                tr.Append(contentType);
                tr.Append(name);
                tr.Append(platform);
                tr.Append(link);
                codeMapTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", codeMapTable);
            return codeMapTable;
        }

        private static Table BuildFilesTable(WordprocessingDocument document, IEnumerable<ArtifactFile> files)
        {
            var filesTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var fileName1 = new TableCell();
            var content1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Content Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "10" }));
            fileName1.Append(new Paragraph(new Run(new Text("File Name"))));
            fileName1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "25" }));
            content1.Append(new Paragraph(new Run(new Text("File Content"))));
            content1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "65" }));
            tr1.Append(contentType1);
            tr1.Append(fileName1);
            tr1.Append(content1);
            filesTable.Append(tr1);
            foreach (var f in files)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var fileName = new TableCell();
                var content = new TableCell();

                var data = f.FileData == null ? "pending" : f.FileData.ToStringUtf8();
                contentType.Append(new Paragraph(new Run(new Text(f.Content.ToString()))));
                fileName.Append(new Paragraph(new Run(new Text(f.FileName))));
                content.Append(new Paragraph(new Run(new Text(data))));
                tr.Append(contentType);
                tr.Append(fileName);
                tr.Append(content);
                filesTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", filesTable);
            return filesTable;
        }

        private static Table BuildInfluencesTable(WordprocessingDocument document, IEnumerable<SymbolInfluence> influences)
        {
            var influenceTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();


            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "10" }));
            name1.Append(new Paragraph(new Run(new Text("Description"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "75" }));
            description1.Append(new Paragraph(new Run(new Text("Applies To"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "15" }));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            influenceTable.Append(tr1);
            foreach (var i in influences)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                symbol.Append(new Paragraph(new Run(new Text(i.Symbol.Tooling))));
                name.Append(new Paragraph(new Run(new Text(i.Description))));
                description.Append(new Paragraph(new Run(new Text(i.AppliesTo.ToString()))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                influenceTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", influenceTable);
            return influenceTable;
        }

        private static Table BuildIncompatibleTable(WordprocessingDocument document, IEnumerable<ArtifactSymbol> incompatibles)
        {
            var incompatibleTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "45" }));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "10" }));
            description1.Append(new Paragraph(new Run(new Text("Id"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "45" }));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            incompatibleTable.Append(tr1);
            foreach (var i in incompatibles)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(i.Type.ToString()))));
                symbol.Append(new Paragraph(new Run(new Text(i.Tooling))));
                description.Append(new Paragraph(new Run(new Text(i.Id))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                incompatibleTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", incompatibleTable);
            return incompatibleTable;
        }

        private static Table BuildDependencyTable(WordprocessingDocument document, IEnumerable<SymbolDependency> dependencies)
        {
            //dependencies table should be the 4th table
            var dependencyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "35" }));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "10" }));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "55" }));
            tr1.Append(name1);
            tr1.Append(symbol1);
            tr1.Append(description1);
            dependencyTable.Append(tr1);
            foreach (var d in dependencies)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(d.Symbol.Type.ToString()))));
                symbol.Append(new Paragraph(new Run(new Text(d.Symbol.Tooling))));
                description.Append(new Paragraph(new Run(new Text(d.Description))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                dependencyTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", dependencyTable);
            return dependencyTable;
        }

        public static Table BuildAnalogies(WordprocessingDocument document, IEnumerable<ArtifactAnalogy> analogies)
        {
            var analogyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "25" }));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = Pct, Width = "75" }));
            tr1.Append(name1);
            tr1.Append(description1);
            analogyTable.Append(tr1);
            foreach (var a in analogies)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(a.Name))));
                description.Append(new Paragraph(new Run(new Text(a.Description))));
                tr.Append(name);
                tr.Append(description);
                analogyTable.Append(tr);
            }
            Utils.ApplyStyleTable(document, "GridTable4-Accent1", "GridTable4-Accent1", analogyTable);
            return analogyTable;
        }


    }
}