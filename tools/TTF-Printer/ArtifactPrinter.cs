using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.WellKnownTypes;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using StringValue = DocumentFormat.OpenXml.StringValue;
using Style = DocumentFormat.OpenXml.Wordprocessing.Style;
using V = DocumentFormat.OpenXml.Vml;

namespace TTI.TTF.Taxonomy
{
    public static class ArtifactPrinter
    {
        private static WordprocessingDocument _document;

        public static void PrintArtifact(string fileName, string waterMark, ArtifactType artifactType, Any artifactToPrint)
        {
            _document = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document);

            // Add a main document part. 
            var mainPart = _document.AddMainDocumentPart();

            mainPart.Document = new Document();
            // Get the Styles part for this document.
            AddStyles();
     
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

        private static void AddStyles()
        {
            // Heading 1
            var styleRunPropertiesH1 = new StyleRunProperties();
            var color1 = new Color() { Val = "2F5496" };
            // Specify a 16 point size. 16x2 because it’s half-point size
            var fontSizeH1 = new FontSize {Val = new StringValue("32")};

            styleRunPropertiesH1.Append(color1);
            styleRunPropertiesH1.Append(fontSizeH1);
            AddStyleToDoc(_document.MainDocumentPart, "heading1", "heading 1", styleRunPropertiesH1);

            // Heading 2
            var styleRunPropertiesH2 = new StyleRunProperties();
            var color2 = new Color() { Val = "2F5496" };
            // Specify a 13 point size. 16x2 because it’s half-point size
            var fontSizeH2 = new FontSize {Val = new StringValue("26")};

            styleRunPropertiesH2.Append(color2);
            styleRunPropertiesH2.Append(fontSizeH2);
            AddStyleToDoc(_document.MainDocumentPart, "heading2", "heading 2", styleRunPropertiesH2);


            // Normal
            var styleRunPropertiesNormal = new StyleRunProperties();
            
            // Specify a 13 point size. 16x2 because it’s half-point size
            var fontSizeNormal = new FontSize { Val = new StringValue("13") };

            styleRunPropertiesNormal.Append(fontSizeNormal);
            AddStyleToDoc(_document.MainDocumentPart, "normal", "normal", styleRunPropertiesNormal);

            // Emphasis
            var styleRunPropertiesEmphasis = new StyleRunProperties();
            var colorE = new Color() { Val = "2F5496" };
            // Specify a 13 point size. 16x2 because it’s half-point size
            var fontSizeEmphasis = new FontSize { Val = new StringValue("16") };

            styleRunPropertiesEmphasis.Append(colorE);
            styleRunPropertiesEmphasis.Append(fontSizeEmphasis);
            AddStyleToDoc(_document.MainDocumentPart, "emphasis", "emphasis", styleRunPropertiesEmphasis);

        }

        private static void Save()
        {
            _document.MainDocumentPart.Document.Save();
        }

        #region artifact
        public static void AddArtifactContent(Artifact artifact)
        {

            var ppH1 = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId() { Val = "heading1" },
                SpacingBetweenLines = new SpacingBetweenLines() { After = "0" },
                Justification = new Justification { Val = JustificationValues.Center }
            };

            var ppH2 = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId() { Val = "heading2" },
                SpacingBetweenLines = new SpacingBetweenLines() { After = "0" },
                Justification = new Justification { Val = JustificationValues.Left }
            };

            var ppNormal = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId() { Val = "normal" },
                SpacingBetweenLines = new SpacingBetweenLines() { After = "0" },
                Justification = new Justification { Val = JustificationValues.Left }
            };

            var ppEmphasis = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId() { Val = "emphasis" },
                SpacingBetweenLines = new SpacingBetweenLines() { After = "0" },
                Justification = new Justification { Val = JustificationValues.Both }
            };

            var body = _document.MainDocumentPart.Document.AppendChild(new Body());

            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            run.AppendChild(new Text(artifact.Name) { Space = SpaceProcessingModeValues.Preserve });

            para.Append(ppH1);
 

            var basicProps = new[,]
            {
                {"Id:", artifact.ArtifactSymbol.Id},
                {"Type:", artifact.ArtifactSymbol.Type.ToString()},
                {"Visual:", artifact.ArtifactSymbol.Visual},
                {"Tooling:", artifact.ArtifactSymbol.Tooling},
                {"Version:", artifact.ArtifactSymbol.Version}
            };
            AddTable(basicProps);

            var aDef = body.AppendChild(new Paragraph());
            var adRun = aDef.AppendChild(new Run());
            adRun.AppendChild(new Text("Definition"));
            aDef.Append(ppH2);

            var bizBody = body.AppendChild(new Paragraph());
            var bRun = bizBody.AppendChild(new Run());
            bRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessDescription));
            bizBody.Append(ppEmphasis);

            var eDef = body.AppendChild(new Paragraph());
            var eRun = eDef.AppendChild(new Run());
            eRun.AppendChild(new Text("Example"));
            eDef.Append(ppH2);

            var exBody = body.AppendChild(new Paragraph());
            var exRun = exBody.AppendChild(new Run());
            exRun.AppendChild(new Text(artifact.ArtifactDefinition.BusinessExample));
            exBody.Append(ppNormal);

            var aPara = body.AppendChild(new Paragraph());
            var aRun = aPara.AppendChild(new Run());
            aRun.AppendChild(new Text("Analogies"));
            aPara.Append(ppH2);

            var arPara = body.AppendChild(new Paragraph());
            var arRun = arPara.AppendChild(new Run());
            arRun.AppendChild(BuildAnalogies(artifact.ArtifactDefinition.Analogies));
            arPara.Append(ppNormal);

            var coPara = body.AppendChild(new Paragraph());
            var coRun = coPara.AppendChild(new Run());
            coRun.AppendChild(new Text("Comments"));
            coPara.Append(ppH2);

            var cmBody = body.AppendChild(new Paragraph());
            var cmRun = cmBody.AppendChild(new Run());
            cmRun.AppendChild(new Text(artifact.ArtifactDefinition.Comments));
            cmBody.Append(ppNormal);

            var dPara = body.AppendChild(new Paragraph());
            var dRun = dPara.AppendChild(new Run());
            dRun.AppendChild(new Text("Dependencies"));
            dPara.Append(ppH2);

            var ddPara = body.AppendChild(new Paragraph());
            var ddRun = ddPara.AppendChild(new Run());
            ddRun.AppendChild(BuildDependencyTable(artifact.Dependencies));
            ddPara.Append(ppNormal);

            var iPara = body.AppendChild(new Paragraph());
            var iRun = iPara.AppendChild(new Run());
            iRun.AppendChild(new Text("Incompatible With"));
            iPara.Append(ppH2);

            var iiPara = body.AppendChild(new Paragraph());
            var iiRun = iiPara.AppendChild(new Run());
            iiRun.AppendChild(BuildIncompatibleTable(artifact.IncompatibleWithSymbols));
            iiPara.Append(ppNormal);

            var fPara = body.AppendChild(new Paragraph());
            var fRun = fPara.AppendChild(new Run());
            fRun.AppendChild(new Text("Influenced By"));
            fPara.Append(ppH2);

            var ffPara = body.AppendChild(new Paragraph());
            var ffRun = ffPara.AppendChild(new Run());
            ffRun.AppendChild(BuildInfluencesTable(artifact.InfluencedBySymbols));
            ffPara.Append(ppNormal);

            var fiPara = body.AppendChild(new Paragraph());
            var fiRun = fiPara.AppendChild(new Run());
            fiRun.AppendChild(new Text("Artifact Files"));
            fiPara.Append(ppH2);

            var ffiPara = body.AppendChild(new Paragraph());
            var ffiRun = ffiPara.AppendChild(new Run());
            ffiRun.AppendChild(BuildFilesTable(artifact.ArtifactFiles));
            ffiPara.Append(ppNormal);

            var cPara = body.AppendChild(new Paragraph());
            var cRun = cPara.AppendChild(new Run());
            cRun.AppendChild(new Text("Code Map"));
            cPara.Append(ppH2);

            var ccPara = body.AppendChild(new Paragraph(new ParagraphProperties(new ParagraphStyleId { Val = "Heading 2" })));
            var ccRun = ccPara.AppendChild(new Run());
            ccRun.AppendChild(BuildCodeTable(artifact.Maps.CodeReferences));
            ccPara.Append(ppNormal);

            var pPara = body.AppendChild(new Paragraph());
            var pRun = pPara.AppendChild(new Run());
            pRun.AppendChild(new Text("Implementation Map"));
            pPara.Append(ppH2);

            var ppPara = body.AppendChild(new Paragraph());
            var ppRun = ppPara.AppendChild(new Run());
            ppRun.AppendChild(BuildImplementationTable(artifact.Maps.ImplementationReferences));
            ppPara.Append(ppNormal);

            var rPara = body.AppendChild(new Paragraph());
            var rRun = rPara.AppendChild(new Run());
            rRun.AppendChild(new Text("Resource Map"));
            rPara.Append(ppH2);

            var rrPara = body.AppendChild(new Paragraph());
            var rrRun = rrPara.AppendChild(new Run());
            rrRun.AppendChild(BuildReferenceTable(artifact.Maps.Resources));
            rrPara.Append(ppNormal);

            Save();
        }

        private static Table BuildReferenceTable(IEnumerable<MapResourceReference> resources)
        {
            var referenceMapTable =new Table();
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

            return referenceMapTable;
        }

        private static Table BuildImplementationTable(IEnumerable<MapReference> references)
        {
            var implementationMapTable = new Table();
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

            return implementationMapTable;
        }

        private static Table BuildCodeTable(IEnumerable<MapReference> references)
        {
            var codeMapTable =new Table();
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

            return codeMapTable;
        }

        private static Table BuildFilesTable(IEnumerable<ArtifactFile> files)
        {
            var filesTable =new Table();
            foreach (var f in files)
            {
                var tr = new TableRow();
                var contentType = new TableCell();
                var fileName = new TableCell();
                var content = new TableCell();

                contentType.Append(new Paragraph(new Run(new Text(f.Content.ToString()))));
                fileName.Append(new Paragraph(new Run(new Text(f.FileName))));
                content.Append(new Paragraph(new Run(new Text(f.FileData.ToStringUtf8()))));
                tr.Append(contentType);
                tr.Append(fileName);
                tr.Append(content);
                filesTable.Append(tr);
            }

            return filesTable;
        }

        private static Table BuildInfluencesTable(IEnumerable<SymbolInfluence> influences)
        {
            //influences table should be the 6th table
            var influenceTable = new Table();
            foreach (var i in influences)
            {
                var tr = new TableRow();
                var name = new TableCell();
                var symbol = new TableCell();
                var description = new TableCell();

                name.Append(new Paragraph(new Run(new Text(i.Description))));
                symbol.Append(new Paragraph(new Run(new Text(i.Symbol.Tooling))));
                description.Append(new Paragraph(new Run(new Text(i.AppliesTo.ToString()))));
                tr.Append(name);
                tr.Append(symbol);
                tr.Append(description);
                influenceTable.Append(tr);
            }

            return influenceTable;
        }

        private static Table BuildIncompatibleTable(IEnumerable<ArtifactSymbol> incompatibles)
        {
            var incompatibleTable = new Table();
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

            return incompatibleTable;
        }

        private static Table BuildDependencyTable(IEnumerable<SymbolDependency> dependencies)
        {
            //dependencies table should be the 4th table
            var dependencyTable = new Table();
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

            return dependencyTable;
        }

        public static Table BuildAnalogies(IEnumerable<ArtifactAnalogy> analogies)
        {
            var analogyTable = new Table();
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
        // Apply a style to a paragraph.
        public static void AddStyleToDoc(MainDocumentPart mainPart, string styleid, string stylename, StyleRunProperties styleRunProperties)
        {

            // Get the Styles part for this document.
            var part =
                mainPart.StyleDefinitionsPart;

            // If the Styles part does not exist, add it and then add the style.
            if (part == null)
            {
                part = AddStylesPartToPackage(mainPart);
                AddNewStyle(part, styleid, stylename, styleRunProperties);
            }
            else
            {
                // If the style is not in the document, add it.
                if (IsStyleIdInDocument(mainPart, styleid) != true)
                {
                    // No match on styleid, so let's try style name.
                    var styleidFromName = GetStyleIdFromStyleName(stylename);
                    if (styleidFromName == null)
                    {
                        AddNewStyle(part, styleid, stylename, styleRunProperties);
                    }
                    else
                        styleid = styleidFromName;
                }
            }

        }

        // Add a StylesDefinitionsPart to the document.  Returns a reference to it.
        public static StyleDefinitionsPart AddStylesPartToPackage(MainDocumentPart mainPart)
        {
            StyleDefinitionsPart part;
            part = mainPart.AddNewPart<StyleDefinitionsPart>();
            var root = new Styles();
            root.Save(part);
            return part;
        }

        public static bool IsStyleIdInDocument(MainDocumentPart mainPart, string styleid)
        {
            // Get access to the Styles element for this document.
            var s = mainPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            var n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleid.
            var style = s.Elements<Style>()
                .Where(st => (st.StyleId == styleid) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
                return false;

            return true;
        }

        // Create a new style with the specified styleid and stylename and add it to the specified style definitions part.
        private static void AddNewStyle(StyleDefinitionsPart styleDefinitionsPart, string styleId, string styleName, StyleRunProperties styleRunProperties)
        {
            // Get access to the root element of the styles part.
            var styles = styleDefinitionsPart.Styles;

            // Create a new paragraph style and specify some of the properties.
            var style = new Style()
            {
                Type = StyleValues.Paragraph,
                StyleId = styleId,
                CustomStyle = true
            };
            style.Append(new StyleName() { Val = styleName });
            style.Append(new BasedOn() { Val = "Normal" });
            style.Append(new NextParagraphStyle() { Val = "Normal" });
            style.Append(new UIPriority() { Val = 900 });

            // Create the StyleRunProperties object and specify some of the run properties.


            // Add the run properties to the style.
            // --- Here we use the OuterXml. If you are using the same var twice, you will get an error. So to be sure just insert the xml and you will get through the error.
            style.Append(styleRunProperties);

            // Add the style to the styles part.
            styles.Append(style);
        }

        public static bool IsStyleIdInDocument(string styleId)
        {
            // Get access to the Styles element for this document.
            var s = _document.MainDocumentPart.StyleDefinitionsPart.Styles;

            // Check that there are styles and how many.
            var n = s.Elements<Style>().Count();
            if (n == 0)
                return false;

            // Look for a match on styleid.
            var style = s.Elements<Style>()
                .Where(st => (st.StyleId == styleId) && (st.Type == StyleValues.Paragraph))
                .FirstOrDefault();
            if (style == null)
                return false;

            return true;
        }

        
        public static string GetStyleIdFromStyleName(string styleName)
        {
            var stylePart = _document.MainDocumentPart.StyleDefinitionsPart;
            string styleId = stylePart.Styles.Descendants<StyleName>()
                .Where(s => s.Val.Value.Equals(styleName) &&
                    (((Style)s.Parent).Type == StyleValues.Paragraph))
                .Select(n => ((Style)n.Parent).StyleId).FirstOrDefault();
            return styleId;
        }


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

        #endregion
    }
}
