using AUTOS_AJH.Views;
using System;
using System.Windows.Forms;

namespace AUTOS_AJH
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}