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
    internal class Printer
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

            if (args.Length != 4 || args.Length == 1)
            {
                _log.Error(GetUsage());
                return;
            }

            _log.Info("TTF-Printer printing with options: " + args);

            _gRpcHost = _config["gRpcHost"];
            _gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
            _printToPath = _config["printToPath"];

            _log.Info("Connection to TaxonomyService: " + _gRpcHost + " port: " + _gRpcPort);
            TaxonomyClient = new Service.ServiceClient(
                new Channel(_gRpcHost, _gRpcPort, ChannelCredentials.Insecure));

            var waterMark = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                            @"\images\TTF-bw.jpg";

            var styleSource = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                            @"\templates\savon.docx";

            var filePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + ModelMap.FolderSeparator +
                           _printToPath + ModelMap.FolderSeparator;

            switch (args.Length)
            {
                case 1 when args[0] == "-a":
                    var ttf = TaxonomyClient.GetFullTaxonomy(new Model.TaxonomyVersion { Version = "1.0" });
                    PrintController.PrintTTF(ttf);
                    return;
                case 1:
                    _log.Error(GetUsage());
                    return;
                case 2:
                    _log.Error(GetUsage());
                    return;
                case 3:
                    _log.Error(GetUsage());
                    return;
                case 4:
                    var id = "";
                    ArtifactType artifactType = ArtifactType.Base;
                    var  artifactSet = false;
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
                                artifactType = (ArtifactType)t;
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
                                var b = TaxonomyClient.GetBaseArtifact(new ArtifactSymbol
                                {
                                    Id = id
                                });

                                if (b != null)
                                {
                                    PrintController.PrintBase(filePath, waterMark, styleSource, b);
                                    return;
                                }
                                break;
                            case ArtifactType.Behavior:
                                var behavior = TaxonomyClient.GetBehaviorArtifact(new ArtifactSymbol
                                {
                                    Id = id
                                });

                                if (behavior != null)
                                {
                                    PrintController.PrintBehavior(filePath, waterMark, styleSource, behavior);
                                    return;
                                }
                                break;
                            case ArtifactType.BehaviorGroup:
                                var behaviorGroup = TaxonomyClient.GetBehaviorGroupArtifact(new ArtifactSymbol
                                {
                                    Id = id
                                });

                                if (behaviorGroup != null)
                                {
                                    PrintController.PrintBehaviorGroup(filePath, waterMark, styleSource, behaviorGroup);
                                    return;
                                }
                                break;
                            case ArtifactType.PropertySet:
                                var propertySet = TaxonomyClient.GetPropertySetArtifact(new ArtifactSymbol
                                {
                                    Id = id
                                });

                                if (propertySet != null)
                                {
                                    PrintController.PrintPropertySet(filePath, waterMark, styleSource, propertySet);
                                    return;
                                }
                                break;
                            case ArtifactType.TemplateFormula:
                                break;
                            case ArtifactType.TemplateDefinition:
                                break;
                            case ArtifactType.TokenTemplate:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }

        }

        private static string GetUsage()
        {
            var u = "Artifact Printer Syntax - " + " 'TTF-Printer -t <TypeId> -id <Id>'";
            var s = "       'TTF-Printer -a' will generate the entire TTF.";
            var t = "       Types - 0=Base, 1=Behavior, 2-BehaviorGroup, 3=PropertySet, 4=TemplateFormula, 5=TemplateDefinition, 6=TokenTemplate/Specification";
            return u + s +  t;
        }
    }
}