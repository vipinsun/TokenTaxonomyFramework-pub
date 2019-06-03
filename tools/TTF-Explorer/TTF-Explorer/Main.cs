using System.Reflection;
using AppKit;
using Grpc.Core;
using log4net;
using TTF.TTI.TTFExplorer;
using TTI.TTF.Taxonomy;

namespace TTI.TTF.TTFExplorer
{


  static class MainClass
  {
    internal static ILog Log;
    private static string GRpcHost { get; set; }
    private static int GRpcPort { get; set; }
    internal static TaxonomyService.TaxonomyServiceClient TaxonomyContext;


    static void Main(string[] args)
    {
      NSApplication.Init();
      NSApplication.Main(args);

      #region logging

      Utils.InitLog();
      Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      #endregion

     
    }

    private static void ConnectContext()
    {
      Log.Info("Connection to TaxonomyService: " + GRpcHost + " port: " + GRpcPort);
      TaxonomyContext = new TaxonomyService.TaxonomyServiceClient(
          new Channel(GRpcHost, GRpcPort, ChannelCredentials.Insecure));

    }
  }
}
