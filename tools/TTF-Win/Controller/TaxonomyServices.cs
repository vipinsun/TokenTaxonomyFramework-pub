using Grpc.Core;
using System;
using System.Reflection;
using log4net;
using Microsoft.Extensions.Configuration;
using TTI.TTF.Taxonomy;
using TTI.TTF.Taxonomy.Model;

namespace TTF_Win.Controller
{
    internal static class TaxonomyServices
    {
        internal static Service.ServiceClient TaxonomyClient;
        internal static PrinterService.PrinterServiceClient PrinterClient;

        public static Taxonomy Taxonomy { get; set; }

        public static void Load()
        {

            #region config

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            #endregion

            #region logging

            Utils.InitLog();
            var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion

            var gRpcHost = config["gRpcHost"];
            var gRpcPort = Convert.ToInt32(config["gRpcPort"]);

            var printHost = config["printHost"];
            var printPort = Convert.ToInt32(config["printPort"]);

            log.Info("Connection to TaxonomyService: " + gRpcHost + " port: " + gRpcPort);
            TaxonomyClient = new Service.ServiceClient(

                new Channel(gRpcHost, gRpcPort, ChannelCredentials.Insecure));

            log.Info("Connection to TTF-Printer: " + printHost + " port: " + printPort);
            PrinterClient = new PrinterService.PrinterServiceClient(

                new Channel(printHost, printPort, ChannelCredentials.Insecure));

            Taxonomy = TaxonomyClient.GetFullTaxonomy(new TaxonomyVersion
            {
                Version = "1.0"
            });
        }
    }
}
