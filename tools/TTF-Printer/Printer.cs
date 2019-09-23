using System;
using System.IO;
using System.Reflection;
using TTI.TTF.Taxonomy.Model.Artifact;
using log4net;
using Microsoft.Extensions.Configuration;
using Grpc.Core;
using TTI.TTF.Taxonomy.TypePrinters;
using TTI.TTF.Taxonomy.Model;

namespace TTI.TTF.Taxonomy
{
    internal static class Printer
    {
        private static IConfigurationRoot _config;
        private static ILog _log;
        private static string _gRpcHost;
        private static int _gRpcPort;
        internal static Service.ServiceClient TaxonomyClient;
        private static string _printToPath;

        private static void Main(string[] args)
        {

            #region config

            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            #endregion

            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion

            if (args.Length == 1 || args.Length == 4)
            {

                _log.Info("TTF-Printer printing with options: " + args);

                _gRpcHost = _config["gRpcHost"];
                _gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
                _printToPath = _config["printToPath"];

                _log.Info("Connection to TaxonomyService: " + _gRpcHost + " port: " + _gRpcPort);
                TaxonomyClient = new Service.ServiceClient(
                    new Channel(_gRpcHost, _gRpcPort, ChannelCredentials.Insecure));

                ModelManager.Taxonomy = (TaxonomyClient.GetFullTaxonomy(new TaxonomyVersion
                {
                    Version = "1.0"
                }));

                var waterMark = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator
                                                                                             + "images" +
                                                                                             ModelMap.FolderSeparator +
                                                                                             "TTF-bw.jpg";

                var styleSource =
                    Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator
                                                                                 + "templates" +
                                                                                 ModelMap.FolderSeparator +
                                                                                 "savon.docx";

                var filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator +
                               _printToPath + ModelMap.FolderSeparator;

                switch (args.Length)
                {
                    case 1:
                        switch (args[0])
                        {
                            case "-a":
                                TaxonomyPrinter.PrintAllArtifacts(filePath, waterMark, styleSource);
                                break;
                            case "-b":
                                TaxonomyPrinter.BuildTtfBook(filePath, waterMark, styleSource);
                                break;
                        }

                        return;
                    case 2:
                        _log.Error(GetUsage());
                        return;
                    case 3:
                        _log.Error(GetUsage());
                        return;
                    case 4:
                        var id = "";
                        var artifactType = ArtifactType.Base;
                        var artifactSet = false;
                        for (var i = 0; i < args.Length; i++)
                        {
                            switch (args[i])
                            {
                                case "-id":
                                    i++;
                                    id = args[i];
                                    continue;
                                case "-t":
                                    i++;
                                    var t = Convert.ToInt32(args[i]);
                                    artifactType = (ArtifactType) t;
                                    artifactSet = true;
                                    continue;
                                default:
                                    continue;
                            }

                        }

                        if (artifactSet && !string.IsNullOrEmpty(id))
                        {
                            switch (artifactType)
                            {
                                case ArtifactType.Base:
                                    var b = ModelManager.GetBaseArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (b != null)
                                    {
                                        PrintController.PrintBase(filePath, waterMark, styleSource, b);
                                    }

                                    break;
                                case ArtifactType.Behavior:
                                    var behavior = ModelManager.GetBehaviorArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (behavior != null)
                                    {
                                        PrintController.PrintBehavior(filePath, waterMark, styleSource, behavior);
                                    }

                                    break;
                                case ArtifactType.BehaviorGroup:
                                    var behaviorGroup = ModelManager.GetBehaviorGroupArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (behaviorGroup != null)
                                    {
                                        PrintController.PrintBehaviorGroup(filePath, waterMark, styleSource,
                                            behaviorGroup);
                                    }

                                    break;
                                case ArtifactType.PropertySet:
                                    var propertySet = ModelManager.GetPropertySetArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (propertySet != null)
                                    {
                                        PrintController.PrintPropertySet(filePath, waterMark, styleSource, propertySet);
                                    }

                                    break;
                                case ArtifactType.TemplateFormula:
                                    var formula = ModelManager.GetTemplateFormulaArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (formula != null)
                                    {
                                        PrintController.PrintFormula(filePath, waterMark, styleSource, formula);
                                    }

                                    break;
                                case ArtifactType.TemplateDefinition:
                                    var definition = ModelManager.GetTemplateDefinitionArtifact(new ArtifactSymbol
                                    {
                                        Id = id
                                    });

                                    if (definition != null)
                                    {
                                        PrintController.PrintDefinition(filePath, waterMark, styleSource, definition);
                                    }

                                    break;
                                case ArtifactType.TokenTemplate:
                                    var spec = TaxonomyClient.GetTokenSpecification(new TokenTemplateId
                                    {
                                        DefinitionId = id
                                    });

                                    if (spec != null)
                                    {
                                        if (spec.Artifact.Name.Contains("Error:"))
                                        {
                                            _log.Error(spec.Artifact.Name);
                                            return;
                                        }

                                        PrintController.PrintSpec(filePath, waterMark, styleSource, spec);
                                    }

                                    break;
                            }
                        }

                        break;
                }
            }
            {
                _log.Error(GetUsage());
            }
        }

        private static string GetUsage()
        {
            const string u = "Artifact Printer Syntax - " + " 'TTF-Printer -t <TypeId> -id <Id>'";
            const string s = "       '- TTF-Printer -a' will generate the entire TTF.";
            const string b = "       '- TTF-Printer -b will create a single TTF Book from all artifacts'";
            const string t = "Types - 0=Base, 1=Behavior, 2-BehaviorGroup, 3=PropertySet, 4=TemplateFormula, 5=TemplateDefinition, 6=TokenTemplate/Specification";
            return u + s + b + t;
        }
    }
}