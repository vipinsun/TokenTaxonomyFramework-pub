using DocumentFormat.OpenXml.Packaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TTI.TTF.Taxonomy.Model.Core;

namespace TTI.TTF.Taxonomy.TypePrinters
{
    internal static class FormulaPrinter
    {
        private static readonly ILog Log;
        static FormulaPrinter()
        {
            #region logging

            Utils.InitLog();
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            #endregion
        }

        public static void AddFormulaProperties(WordprocessingDocument document, TemplateFormula formula)
        {

        }
    }
}
