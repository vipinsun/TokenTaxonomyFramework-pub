using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.WellKnownTypes;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;
using V = DocumentFormat.OpenXml.Vml;

namespace TTI.TTF.Taxonomy
{
    public static class ArtifactPrinter
    {
        private static WordprocessingDocument _document;

        public static void PrintArtifact(string fileName, string waterMark, string styleSource, ArtifactType artifactType, Any artifactToPrint)
        {
            _document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document);

            // Add a main document part. 
            var mainPart = _document.AddMainDocumentPart();

            mainPart.Document = new Document();
            // Get the Styles part for this document.
            AddStylesPartToPackage();
            var styles = ExtractStylesPart(styleSource);
            ReplaceStylesPart(styles);

            Save();

            dynamic artifact;
            switch (artifactType)
            {
                case ArtifactType.Base:
                    artifact = artifactToPrint.Unpack<Base>();
                    AddArtifactContent(artifact?.Artifact);
                    AddBaseProperties(artifact);
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    break;
                case ArtifactType.Behavior:
                    artifact = artifactToPrint.Unpack<Model.Core.Behavior>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddBehaviorProperties(artifact);
                    break;
                case ArtifactType.BehaviorGroup:
                    artifact = artifactToPrint.Unpack<BehaviorGroup>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddBehaviorGroupProperties(artifact);
                    break;
                case ArtifactType.PropertySet:
                    artifact = artifactToPrint.Unpack<PropertySet>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddPropSetProperties(artifact);
                    break;
                case ArtifactType.TemplateFormula:
                    artifact = artifactToPrint.Unpack<TemplateFormula>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddFormulaProperties(artifact);
                    break;
                case ArtifactType.TemplateDefinition:
                    artifact = artifactToPrint.Unpack<TemplateDefinition>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddDefinitionProperties(artifact);
                    break;
                case ArtifactType.TokenTemplate:
                    artifact = artifactToPrint.Unpack<TokenSpecification>();
                    InsertCustomWatermark(waterMark);
                    AddFooter(artifact?.Artifact.Name);
                    AddArtifactContent(artifact?.Artifact);
                    AddSpecProperties(artifact);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(artifactType), artifactType, null);
            }

            Save();
            if(!ValidateWordDocument())
                ValidateCorruptedWordDocument();
            _document.Close();
        }


        private static void Save()
        {
            _document.MainDocumentPart.Document.Save();
        }

        #region artifact
        public static void AddArtifactContent(Artifact artifact)
        {
            var body = _document.MainDocumentPart.Document.AppendChild(new Body());

            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(artifact.Name) { Space = SpaceProcessingModeValues.Preserve });
            ApplyStyleToParagraph("Title", "Title", para, JustificationValues.Center);
            
            
            var basicProps = new[,]
            {
                {"Type:", artifact.ArtifactSymbol.Type.ToString()},
                {"Id:", artifact.ArtifactSymbol.Id},
                {"Visual:", artifact.ArtifactSymbol.Visual},
                {"Tooling:", artifact.ArtifactSymbol.Tooling},
                {"Version:", artifact.ArtifactSymbol.Version}
            };
            AddTable(basicProps);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Definition"));
            ApplyStyleToParagraph("Heading1", "Heading1", aDef);
            

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
            ApplyStyleToParagraph("Strong Emphasis", "Strong Emphasis", bizBody);

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Example"));
            ApplyStyleToParagraph("Heading2", "Heading2", eDef);

            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
            ApplyStyleToParagraph("Normal", "Normal", exBody);

            var aPara = body.AppendChild(new Paragraph());
            var aRun = aPara.AppendChild(new Run());
            aRun.AppendChild(new Text("Analogies"));
            ApplyStyleToParagraph("Heading2", "Heading2", aPara);

            var arPara = body.AppendChild(new Paragraph());
            var arRun = arPara.AppendChild(new Run());
            arRun.AppendChild(BuildAnalogies(artifact.ArtifactDefinition.Analogies));
            ApplyStyleToParagraph("Normal", "Normal", arPara);

            var coPara = body.AppendChild(new Paragraph());
            var coRun = coPara.AppendChild(new Run());
            coRun.AppendChild(new Text("Comments"));
            ApplyStyleToParagraph("Heading2", "Heading2", coPara);

            var cmBody = body.AppendChild(new Paragraph());
            var cmRun = cmBody.AppendChild(new Run());
            cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
            ApplyStyleToParagraph("Normal", "Normal", cmBody);

            var dPara = body.AppendChild(new Paragraph());
            var dRun = dPara.AppendChild(new Run());
            dRun.AppendChild(new Text("Dependencies"));
            ApplyStyleToParagraph("Heading2", "Heading2", dPara);

            var ddPara = body.AppendChild(new Paragraph());
            var ddRun = ddPara.AppendChild(new Run());
            ddRun.AppendChild(BuildDependencyTable(artifact.Dependencies));
            ApplyStyleToParagraph("Normal", "Normal", ddPara);

            var iPara = body.AppendChild(new Paragraph());
            var iRun = iPara.AppendChild(new Run());
            iRun.AppendChild(new Text("Incompatible With"));
            ApplyStyleToParagraph("Heading2", "Heading2", iPara);

            var iiPara = body.AppendChild(new Paragraph());
            var iiRun = iiPara.AppendChild(new Run());
            iiRun.AppendChild(BuildIncompatibleTable(artifact.IncompatibleWithSymbols));
            ApplyStyleToParagraph("Normal", "Normal", iiPara);

            var fPara = body.AppendChild(new Paragraph());
            var fRun = fPara.AppendChild(new Run());
            fRun.AppendChild(new Text("Influenced By"));
            ApplyStyleToParagraph("Heading2", "Heading2", fPara);

            var ffPara = body.AppendChild(new Paragraph());
            var ffRun = ffPara.AppendChild(new Run());
            ffRun.AppendChild(BuildInfluencesTable(artifact.InfluencedBySymbols));
            ApplyStyleToParagraph("Normal", "Normal", ffPara);

            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            ApplyStyleToParagraph("Heading2", "Heading2", fiPara);

            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(artifact.ArtifactFiles));
            ApplyStyleToParagraph("Normal", "Normal", ffiPara);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            ApplyStyleToParagraph("Heading2", "Heading2", cPara);

            var ccPara = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" })));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(artifact.Maps.CodeReferences));
            ApplyStyleToParagraph("Normal", "Normal", ccPara);

            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            ApplyStyleToParagraph("Heading2", "Heading2", pPara);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(artifact.Maps.ImplementationReferences));
            ApplyStyleToParagraph("Normal", "Normal", ppPara);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            ApplyStyleToParagraph("Heading2", "Heading2", rPara);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(artifact.Maps.Resources));
            ApplyStyleToParagraph("Normal", "Normal", rrPara);

            Save();
        }

        private static Table BuildReferenceTable(IEnumerable<MapResourceReference> resources)
        {
            var referenceMapTable =new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            platform1.Append(new Paragraph(new Run(new Text("Location"))));
            platform1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "20" }));
            link1.Append(new Paragraph(new Run(new Text("Description"))));
            link1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "50" }));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", referenceMapTable);
            return referenceMapTable;
        }

        private static Table BuildImplementationTable(IEnumerable<MapReference> references)
        {
            var implementationMapTable = new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var name1 = new TableCell();
            var platform1 = new TableCell();
            var link1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            platform1.Append(new Paragraph(new Run(new Text("Platform"))));
            platform1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            link1.Append(new Paragraph(new Run(new Text("Location"))));
            link1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "55" }));
            tr1.Append(contentType1);
            tr1.Append(name1);
            tr1.Append(platform1);
            tr1.Append(link1);
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", implementationMapTable);
            return implementationMapTable;
        }

        private static Table BuildCodeTable(IEnumerable<MapReference> references)
        {
            var codeMapTable =new Table();

            var trH = new TableRow();
            var contentTypeH = new TableCell();
            var nameH = new TableCell();
            var platformH = new TableCell();
            var linkH = new TableCell();

            contentTypeH.Append(new Paragraph(new Run(new Text("Map Type"))));
            contentTypeH.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            nameH.Append(new Paragraph(new Run(new Text("Name"))));
            nameH.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            platformH.Append(new Paragraph(new Run(new Text("Platform"))));
            platformH.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
            linkH.Append(new Paragraph(new Run(new Text("Location"))));
            linkH.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "55" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", codeMapTable);
            return codeMapTable;
        }

        private static Table BuildFilesTable(IEnumerable<ArtifactFile> files)
        {
            var filesTable =new Table();
            var tr1 = new TableRow();
            var contentType1 = new TableCell();
            var fileName1 = new TableCell();
            var content1 = new TableCell();

            contentType1.Append(new Paragraph(new Run(new Text("Content Type"))));
            contentType1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "10" }));
            fileName1.Append(new Paragraph(new Run(new Text("File Name"))));
            fileName1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "25" }));
            content1.Append(new Paragraph(new Run(new Text("File Content"))));
            content1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "65" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", filesTable);
            return filesTable;
        }

        private static Table BuildInfluencesTable(IEnumerable<SymbolInfluence> influences)
        {
            var influenceTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

           
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "10" }));
            name1.Append(new Paragraph(new Run(new Text("Description"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "75" }));
            description1.Append(new Paragraph(new Run(new Text("Applies To"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "15" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", influenceTable);
            return influenceTable;
        }

        private static Table BuildIncompatibleTable(IEnumerable<ArtifactSymbol> incompatibles)
        {
            var incompatibleTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "45" }));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "10" }));
            description1.Append(new Paragraph(new Run(new Text("Id"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "45" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", incompatibleTable);
            return incompatibleTable;
        }

        private static Table BuildDependencyTable(IEnumerable<SymbolDependency> dependencies)
        {
            //dependencies table should be the 4th table
            var dependencyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var symbol1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Artifact Type"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "35" }));
            symbol1.Append(new Paragraph(new Run(new Text("Symbol"))));
            symbol1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "10" }));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "55" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", dependencyTable);
            return dependencyTable;
        }

        public static Table BuildAnalogies(IEnumerable<ArtifactAnalogy> analogies)
        {
            var analogyTable = new Table();
            var tr1 = new TableRow();
            var name1 = new TableCell();
            var description1 = new TableCell();

            name1.Append(new Paragraph(new Run(new Text("Name"))));
            name1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "25" }));
            description1.Append(new Paragraph(new Run(new Text("Description"))));
            description1.Append(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "75" }));
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
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", analogyTable);
            return analogyTable;
        }
        
        #endregion

        #region ttf categories

        public static void AddBaseProperties(Base tokenBase)
        {
            var baseProps = new[,]
            {
                {"Token Name:", tokenBase.Name},
                {"Token Type:", tokenBase.TokenType.ToString() },
                {"Representation Type:", tokenBase.RepresentationType.ToString()},
                {"Value Type:", tokenBase.ValueType.ToString()},
                {"Token Unit:",tokenBase.TokenUnit.ToString() },
                {"Symbol:", tokenBase.Symbol},
                {"Owner:", tokenBase.Owner},
                {"Quantity:", tokenBase.Quantity.ToString()},
                {"Decimals:", tokenBase.Decimals.ToString()},
                {"Constructor Name:",  tokenBase.ConstructorName}
            };

            AddTable(baseProps);
            /*
            tokenBase.TokenProperties.ToString();
            tokenBase.Constructor != null
                ? tokenBase.Constructor.ToString()
                : "constructor TBD";
                */        

        }

        private static void AddSpecProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        private static void AddDefinitionProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        private static void AddFormulaProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        private static void AddPropSetProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        private static void AddBehaviorGroupProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        private static void AddBehaviorProperties(object artifact)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region formatting

       
        // Take the data from a two-dimensional array and build a table at the 
        // end of the supplied document.
        public static void AddTable(string[,] data)
        {
            var table = new Table();
            GetFormattedTable(table);

            for (var i = 0; i <= data.GetUpperBound(0); i++)
            {
                var tr = new TableRow();
                for (var j = 0; j <= data.GetUpperBound(1); j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i, j]))));

                    if(IsLabel(data[i, j]))
                        tc.Append(new TableCellProperties(
                            new TableCellWidth {Type = TableWidthUnitValues.Pct, Width = "15"}));
                    else
                    {
                        tc.Append(new TableCellProperties(
                            new TableCellWidth { Type = TableWidthUnitValues.Pct, Width = "85" }));
                    }

                    tr.Append(tc);
                }

                table.Append(tr);
            }
            ApplyStyleTable("GridTable4-Accent1", "GridTable4-Accent1", table);
            _document.MainDocumentPart.Document.Body.Append(table);
        }

        private static bool IsLabel(string s)
        {
            return s.Contains(":");
        }


        private static void GetFormattedTable(Table table)
        {
            var props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new BottomBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new LeftBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new RightBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new InsideHorizontalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    },
                    new InsideVerticalBorder
                    {
                        Val = new EnumValue<BorderValues>(BorderValues.None),
                        Size = 12
                    }));

            table.AppendChild(props);
        }

        public static bool ValidateWordDocument()
        {
                try
                {
                    var validator = new OpenXmlValidator();
                    var count = 0;
                    foreach (var error in
                        validator.Validate(_document))
                    {
                        count++;
                        Console.WriteLine("Error " + count);
                        Console.WriteLine("Description: " + error.Description);
                        Console.WriteLine("ErrorType: " + error.ErrorType);
                        Console.WriteLine("Node: " + error.Node);
                        Console.WriteLine("Path: " + error.Path.XPath);
                        Console.WriteLine("Part: " + error.Part.Uri);
                        Console.WriteLine("-------------------------------------------");
                    }

                    Console.WriteLine("count={0}", count);
                    return true;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
        }

        public static void ValidateCorruptedWordDocument()
        {
            // Insert some text into the body, this would cause Schema Error
           
                // Insert some text into the body, this would cause Schema Error
                var body = _document.MainDocumentPart.Document.Body;
                var run = new Run(new Text("some text"));
                body.Append(run);

                try
                {
                    var validator = new OpenXmlValidator();
                    var count = 0;
                    foreach (var error in
                        validator.Validate(_document))
                    {
                        count++;
                        Console.WriteLine("Error " + count);
                        Console.WriteLine("Description: " + error.Description);
                        Console.WriteLine("ErrorType: " + error.ErrorType);
                        Console.WriteLine("Node: " + error.Node);
                        Console.WriteLine("Path: " + error.Path.XPath);
                        Console.WriteLine("Part: " + error.Part.Uri);
                        Console.WriteLine("-------------------------------------------");
                    }

                    Console.WriteLine("count={0}", count);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }
        
        private static void AddFooter(string name)
        {
            var footerPart = _document.MainDocumentPart.AddNewPart<FooterPart>();

            var footerPartId = _document.MainDocumentPart.GetIdOfPart(footerPart);


            var footer1 = new Footer();

            var paragraph1 = new Paragraph();

            var paragraphProperties1 = new ParagraphProperties();
            var paragraphStyleId1 = new ParagraphStyleId { Val = "Footer" };

            paragraphProperties1.Append(paragraphStyleId1);

            var run1 = new Run();
            var text1 = new Text { Text = name };

            run1.Append(text1);

            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);

            footer1.Append(paragraph1);

            footerPart.Footer = footer1;

            var sections = _document.MainDocumentPart.Document.Body.Elements<SectionProperties>();
            _document.MainDocumentPart.Document.Body.PrependChild(new FooterReference { Id = footerPartId });
            foreach (var section in sections)
            {
                section.RemoveAllChildren<FooterReference>();

                // Create the new header and footer reference node
                section.PrependChild(new FooterReference { Id = footerPartId });
            }
            footer1.Save();
            Save();
        }

        private static void InsertCustomWatermark(string imagePath)
        {
            SetWaterMarkPicture(imagePath);
            var mainDocumentPart1 = _document.MainDocumentPart;
            if (mainDocumentPart1 == null) return;
            mainDocumentPart1.DeleteParts(mainDocumentPart1.HeaderParts);
            var headPart1 = mainDocumentPart1.AddNewPart<HeaderPart>();
            GenerateHeaderPart1Content(headPart1);
            var rId = mainDocumentPart1.GetIdOfPart(headPart1);
            var image = headPart1.AddNewPart<ImagePart>("image/jpeg", "rId999");
            GenerateImagePart1Content(image);
            var sectPrs = mainDocumentPart1.Document.Body.Elements<SectionProperties>();
            mainDocumentPart1.Document.Body.PrependChild(new HeaderReference { Id = rId });
            foreach (var sectPr in sectPrs)
            {
                sectPr.RemoveAllChildren<HeaderReference>();
                sectPr.PrependChild(new HeaderReference { Id = rId });
            }

        }
        private static void GenerateHeaderPart1Content(HeaderPart headerPart1)
        {
            var header1 = new Header();
            var paragraph2 = new Paragraph();
            var run1 = new Run();
            var picture1 = new Picture();
            var shape1 = new V.Shape { Id = "WordPictureWatermark75517470", Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:456.15pt;height:456.15pt;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin", OptionalString = "_x0000_s2051", AllowInCell = false, Type = "#_x0000_t75" };
            var imageData1 = new V.ImageData { Gain = "19661f", BlackLevel = "22938f", Title = "水印", RelationshipId = "rId999" };
            shape1.Append(imageData1);
            picture1.Append(shape1);
            run1.Append(picture1);
            paragraph2.Append(run1);
            header1.Append(paragraph2);
            headerPart1.Header = header1;
            header1.Save();
        }
        private static void GenerateImagePart1Content(ImagePart imagePart1)
        {
            var data = GetBinaryDataStream(_imagePart1Data);
            imagePart1.FeedData(data);
            data.Close();
        }

        private static string _imagePart1Data = "";

        private static Stream GetBinaryDataStream(string base64String)
        {
            return new MemoryStream(Convert.FromBase64String(base64String));
        }

        public static void SetWaterMarkPicture(string file)
        {
            try
            {
                var inFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                var byteArray = new byte[inFile.Length];
                long byteRead = inFile.Read(byteArray, 0, (int)inFile.Length);
                inFile.Close();
                _imagePart1Data = Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        public static void ApplyStyleTable(string styleId, string styleName, Table t)
        {
            // If the paragraph has no ParagraphProperties object, create one.
            if (!t.Elements<TableProperties>().Any())
            {
                t.PrependChild(new TableProperties());
            }

            
            // Get the paragraph properties element of the paragraph.
            var tTr = t.Elements<TableProperties>().First();

            // Get the Styles part for this document.
            var part =
                _document.MainDocumentPart.StyleDefinitionsPart;

            // If the Styles part does not exist, add it and then add the style.
            if (part != null)
            {
                // If the style is not in the document, add it.
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(styleId, StyleValues.Table) != true)
                {
                    // No match on styleId, so let's try style name.
                    var fromStyleName = GetStyleIdFromStyleName(styleName, StyleValues.Table);
                    if (fromStyleName != null)
                    {
                        tTr.TableStyle = new TableStyle {Val = fromStyleName};
                        return;

                    }
                }

            }

            // Set the style of the paragraph.
            tTr.TableStyle = new TableStyle { Val = styleId };

        }

        public static void ApplyStyleToParagraph(string styleId, string styleName, Paragraph p, JustificationValues justification = JustificationValues.Left)
        {
            // If the paragraph has no ParagraphProperties object, create one.
            if (!p.Elements<ParagraphProperties>().Any())
            {
                p.PrependChild(new ParagraphProperties
                {
                    Justification = new Justification { Val = justification}
                });
            }

            // Get the paragraph properties element of the paragraph.
            var pPr = p.Elements<ParagraphProperties>().First();

            // Get the Styles part for this document.
            var part =
                _document.MainDocumentPart.StyleDefinitionsPart;

            // If the Styles part does not exist, add it and then add the style.
            if (part != null)
            {
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(styleId) != true)
                {
                    // No match on styleId, so let's try style name.
                    var fromStyleName = GetStyleIdFromStyleName(styleName);
                    if (fromStyleName != null)
                    
                        styleId = fromStyleName;
                }
            }

            // Set the style of the paragraph.
            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = styleId };
        }

        public static bool IsStyleIdInDocument(string styleId, StyleValues styleValues = StyleValues.Paragraph)
        {
            // Get access to the Styles element for this document.
            var s = _document.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            var n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleId.
            var style = s
                .Elements<Style>()
                .FirstOrDefault(st => (st.StyleId == styleId) && (st.Type == styleValues));
            return style != null;
        }


        // Return styleId that matches the styleName, or null when there's no match.
        public static string GetStyleIdFromStyleName(string styleName, StyleValues styleValues = StyleValues.Paragraph)
        {
            var stylePart = _document.MainDocumentPart.StyleDefinitionsPart;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                            ((Style)s.Parent).Type == styleValues)
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }

        
        //https://docs.microsoft.com/en-us/office/open-xml/how-to-replace-the-styles-parts-in-a-word-processing-document
        public static void ReplaceStylesPart(XDocument newStyles,
            bool setStylesWithEffectsPart = false)
        {

            // Get a reference to the main document part.
            var docPart = _document.MainDocumentPart;

            // Assign a reference to the appropriate part to the
            // stylesPart variable.
            StylesPart stylesPart = null;
            if (setStylesWithEffectsPart)
                stylesPart = docPart.StylesWithEffectsPart;
            else
                stylesPart = docPart.StyleDefinitionsPart;

            // If the part exists, populate it with the new styles.
            if (stylesPart != null)
            {
                newStyles.Save(new StreamWriter(stylesPart.GetStream(
                    FileMode.Create, FileAccess.Write)));
            }

        }

        public static StyleDefinitionsPart AddStylesPartToPackage()
        {
            var part = _document.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            var root = new Styles();
            root.Save(part);
            return part;
        }

        // Extract the styles or stylesWithEffects part from a 
        // word processing document as an XDocument instance.
        public static XDocument ExtractStylesPart(
          string fileName,
          bool getStylesWithEffectsPart = false)
        {
            // Declare a variable to hold the XDocument.
            XDocument styles = null;

            // Open the document for read access and get a reference.
            using (var document =
                WordprocessingDocument.Open(fileName, false))
            {
                // Get a reference to the main document part.
                var docPart = document.MainDocumentPart;

                // Assign a reference to the appropriate part to the
                // stylesPart variable.
                StylesPart stylesPart = null;
                if (getStylesWithEffectsPart)
                    stylesPart = docPart.StylesWithEffectsPart;
                else
                    stylesPart = docPart.StyleDefinitionsPart;

                // If the part exists, read it into the XDocument.
                if (stylesPart != null)
                {
                    using (var reader = XmlNodeReader.Create(
                      stylesPart.GetStream(FileMode.Open, FileAccess.Read)))
                    {
                        // Create the XDocument.
                        styles = XDocument.Load(reader);
                    }
                }
            }
            // Return the XDocument instance.
            return styles;
        }
        #endregion
    }
}
