using System;
using System.Reflection;
using Grpc.Core;
using log4net;
using Microsoft.Extensions.Configuration;

namespace TaxonomyHost
{
	internal static class TaxonomyService
	{
		private static IConfigurationRoot _config;
		private static ILog _log;
		internal static string ArtifactPath { get; set; }
		private static string _gRpcHost;
		private static int _gRpcPort;
		private static Server _apiServer;
		internal static string FolderSeparator { get; private set; }
		private static void Main()
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
			
			ArtifactPath = _config["artifactPath"];
			_gRpcHost = _config["gRpcHost"];
			_gRpcPort = Convert.ToInt32(_config["gRpcPort"]);

			FolderSeparator = Os.IsWindows() ? "\\" : "/";

			ModelManager.Init();
			
			_apiServer = new Server
			{
				Services = {TTI.TTF.Taxonomy.TaxonomyService.BindService(new Host())},
				Ports = {new ServerPort(_gRpcHost, _gRpcPort, ServerCredentials.Insecure)}
			};
			
			
			_apiServer.Start();
			_log.Info("Api open on port: " + _gRpcPort);
			Console.WriteLine("Taxonomy Ready");
			Console.WriteLine("Press \'q\' to close the Taxonomy.Service");
			while (Console.Read() != 'q')
			{
			}
			_apiServer.ShutdownAsync();
		}
	}
}