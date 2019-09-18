using DocumentFormat.OpenXml.Packaging;
using log4net;
using System.Reflection;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    internal static class DefinitionPrinter
    {
        private static readonly ILog _log;
        static DefinitionPrinter()
        {
            #region logging

            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddDefinitionProperties(WordprocessingDocument document, TemplateDefinition definition)
        {

        }
    }
}
