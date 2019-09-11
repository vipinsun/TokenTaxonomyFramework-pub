using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;
using Paragraph = DocumentFormat.OpenXml.Drawing.Paragraph;

namespace TTI.TTF.Taxonomy
{
    class Printer
    {
        private static WordprocessingDocument _document;

        static void Main(string[] args)
        {
            Console.WriteLine("TTF-Printer printing with options: " + args);
            var jsonPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                           @"\json\fractional-fungible.json";
            var wordPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                           @"\templates\baseToken.docx";
            var artifact = Base.Parser.ParseJson(File.ReadAllText(jsonPath));


            _document = WordprocessingDocument.Open(wordPath, true);

            var bookMarks = GetAllBookmarks();
            ReplaceBookmarksForArtifact(bookMarks, artifact.Artifact);
            ReplaceBookmarksForBase(bookMarks, artifact);
            UpdateHeaderFooter(artifact.Artifact.Name);


            _document.MainDocumentPart.Document.Save();
        }

        public static List<BookmarkStart> GetAllBookmarks()
        {
            return _document.MainDocumentPart.RootElement.Descendants<BookmarkStart>().ToList();
        }

        public static void UpdateHeaderFooter(string artifactName)
        {

            foreach (var footerPart in _document.MainDocumentPart.FooterParts)
            {
                //Gets the text in headers
                foreach (var currentText in footerPart.RootElement.Descendants<Text>())
                {
                    currentText.Text = currentText.Text.Replace("artifactName", artifactName);
                }
                footerPart.Footer.Save();
            }
            
        }


        public static void ReplaceBookmarksForArtifact(IEnumerable<BookmarkStart> bookmarks, Artifact artifact)
        {
            //switch on artifactType for non-artifact stuff to find replace
            foreach (var bookmark in bookmarks)
            {
                var replacement = bookmark.NextSibling<Run>();
                if (artifact != null && replacement != null)
                {
                    switch (bookmark.Name.Value)
                    {
                        case "ArtifactName":
                            replacement.GetFirstChild<Text>().Text = artifact.Name;
                            break;
                        case "Id":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactSymbol.Id;
                            break;
                        case "ArtifactType":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactSymbol.Type.ToString();
                            break;
                        case "Visual":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactSymbol.Visual;
                            break;
                        case "Tooling":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactSymbol.Tooling;
                            break;
                        case "Version":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactSymbol.Version;
                            break;
                        case "BusinessDescription":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactDefinition.BusinessDescription;
                            break;
                        case "BusinessExample":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactDefinition.BusinessExample;
                            break;
                        case "Comments":
                            replacement.GetFirstChild<Text>().Text = artifact.ArtifactDefinition.Comments;
                            break;
                        case "ControlURI":
                            replacement.GetFirstChild<Text>().Text = artifact.ControlUri;
                            break;
                    }
                    BuildDependencyTable(artifact.Dependencies);
                    BuildIncompatibleTable(artifact.IncompatibleWithSymbols);
                    BuildInfluencesTable(artifact.InfluencedBySymbols);
                    BuildFilesTable(artifact.ArtifactFiles);
                    BuildCodeTable(artifact.Maps.CodeReferences);
                    BuildImplementationTable(artifact.Maps.ImplementationReferences);
                    BuildReferenceTable(artifact.Maps.Resources);
                }
            }

        }

        private static void BuildReferenceTable(IEnumerable<MapResourceReference> resources)
        {
            //resourcesMapTable table should be the 10th table
            var referenceMapTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(9);
            if (referenceMapTable == null) return;
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
        }

        private static void BuildImplementationTable(IEnumerable<MapReference> references)
        {
            //implementationMapTable table should be the 9th table
            var implementationMapTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(8);
            if (implementationMapTable == null) return;
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
        }

        private static void BuildCodeTable(IEnumerable<MapReference> references)
        {
            //codeMaps table should be the 8th table
            var codeMapTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(7);
            if (codeMapTable == null) return;
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
        }

        private static void BuildFilesTable(IEnumerable<ArtifactFile> files)
        {
            //files table should be the 7th table
            var filesTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(6);
            if (filesTable == null) return;
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
        }

        private static void BuildInfluencesTable(IEnumerable<SymbolInfluence> influences)
        {
            //influences table should be the 6th table
            var influenceTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(5);
            if (influenceTable == null) return;
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
        }

        private static void BuildIncompatibleTable(IEnumerable<ArtifactSymbol> incompatibles)
        {
            //incompatibilities table should be the 5th table
            var incompatibleTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(4);
            if (incompatibleTable == null) return;
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
        }

        private static void BuildDependencyTable(IEnumerable<SymbolDependency> dependencies)
        {
            //dependencies table should be the 4th table
            var dependencyTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(3);
            if (dependencyTable == null) return;
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
        }

        public static void BuildAnalogies(IEnumerable<ArtifactAnalogy> analogies)
        {
            var analogyTable =
                _document.MainDocumentPart.Document.Body.Elements<Table>().ElementAtOrDefault(1);
            if (analogyTable == null) return;
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

        }

        public static void ReplaceBookmarksForBase(IEnumerable<BookmarkStart> bookmarks, Base tokenBase)
        {
            foreach (var bookmark in bookmarks)
            {
                var replacement = bookmark.NextSibling<Run>();
                if (tokenBase != null && replacement != null)
                {
                    switch (bookmark.Name.Value)
                    {
                        case "TokenName":

                            replacement.GetFirstChild<Text>().Text = tokenBase.Name;

                            break;
                        case "TokenType":

                            replacement.GetFirstChild<Text>().Text = tokenBase.TokenType.ToString();

                            break;

                        case "RepresentationType":

                            replacement.GetFirstChild<Text>().Text = tokenBase.RepresentationType.ToString();

                            break;
                        case "ValueType":

                            replacement.GetFirstChild<Text>().Text = tokenBase.ValueType.ToString();

                            break;
                        case "TokenUnit":

                            replacement.GetFirstChild<Text>().Text = tokenBase.TokenUnit.ToString();

                            break;
                        case "Symbol":

                            replacement.GetFirstChild<Text>().Text = tokenBase.Symbol;

                            break;
                        case "Owner":

                            replacement.GetFirstChild<Text>().Text = tokenBase.Owner;

                            break;
                        case "Quantity":

                            replacement.GetFirstChild<Text>().Text = tokenBase.Quantity.ToString();

                            break;
                        case "Decimals":

                            replacement.GetFirstChild<Text>().Text = tokenBase.Decimals.ToString();

                            break;
                        case "Properties":

                            replacement.GetFirstChild<Text>().Text = tokenBase.TokenProperties.ToString();

                            break;
                        case "ConstructorName":

                            replacement.GetFirstChild<Text>().Text = tokenBase.ConstructorName;

                            break;
                        case "Constructor":
                            replacement.GetFirstChild<Text>().Text = tokenBase.Constructor != null ? 
                                tokenBase.Constructor.ToString() : "constructor TBD";
                            break;
                            
                    }
                }
            }
        }
    }
}
