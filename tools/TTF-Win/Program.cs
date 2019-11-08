using System;
using System.Windows.Forms;
using TTF_Win.Controller;

namespace TTF_Win
{
    internal static class Program
    {

        [STAThread]
        private static void Main()
        {
            TaxonomyServices.Load();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}