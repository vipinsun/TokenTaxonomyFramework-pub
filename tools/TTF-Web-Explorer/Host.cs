using System;
using System.Reflection;
using Grpc.Core;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TTI.TTF.Taxonomy;
using TTI.TTF.Taxonomy.Model;

namespace TTI.TTF.WebExplorer
{
	public static class Host
	{
		internal static TaxonomyService.TaxonomyServiceClient TaxonomyClient { get; set; }
		private static IConfigurationRoot _config;
		private static ILog _log;
		internal static Taxonomy.Model.Taxonomy Taxonomy { get; set; }
		
		public static void Main(string[] args)
		{
			#region logging

			Utils.InitLog();
			_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

			#endregion
			
			#region config

			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.AddEnvironmentVariables()
				.Build();
			var gRpcHost = _config["gRpcHost"];
			var gRpcPort = Convert.ToInt32(_config["gRpcPort"]);
			
			#endregion
			
			_log.Info("Connection to TaxonomyService: " + gRpcHost + " port: " + gRpcPort);
			TaxonomyClient = new TaxonomyService.TaxonomyServiceClient(
				new Channel(gRpcHost, gRpcPort, ChannelCredentials.Insecure));
			Taxonomy = TaxonomyClient.GetFullTaxonomy(new TaxonomyVersion {Version = "1.0"});
			_log.Info("Taxonomy Version: " + Taxonomy.Version + " loaded.");
			
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
		
	}
}