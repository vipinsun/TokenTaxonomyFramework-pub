using System;
using System.IO;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using TTI.TTF.Taxonomy.Model.Artifact;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy
{
    internal class Printer
    {

        private static void Main(string[] args)
        {
            Console.WriteLine("TTF-Printer printing with options: " + args);
            var jsonPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                           @"\json\fractional-fungible.json";
    
            var artifact = Base.Parser.ParseJson(File.ReadAllText(jsonPath));
            var waterMark = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                            @"\images\TTF.jpg";


            var filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                           @"\artifact.docx";

            ArtifactPrinter.PrintArtifact(filePath, waterMark, ArtifactType.Base, Any.Pack(artifact));
            Console.WriteLine("Press Any Key");
            Console.ReadLine();
        }
    }
}