using System;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;

namespace TTF.TTI.TTFExplorer
{
  public static class Utils
  {
    public static void InitLog()
    {
      var xmlDocument = new XmlDocument();
      try
      {
        
          xmlDocument.Load(File.OpenRead(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                         "/log4net.config"));
      }
      catch (Exception)
      {
        
          xmlDocument.Load(File.OpenRead(
              Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/log4net.config"));
      }

      XmlConfigurator.Configure(
          LogManager.CreateRepository(Assembly.GetEntryAssembly(),
              typeof(log4net.Repository.Hierarchy.Hierarchy)), xmlDocument["log4net"]);
    }
  }
}
