using System;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Principal;

namespace ElectroJoule.PanelAdmin
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}
