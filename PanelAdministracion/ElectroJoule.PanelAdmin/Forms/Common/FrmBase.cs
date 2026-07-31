using System.Windows.Forms;

namespace ElectroJoule.PanelAdmin.Forms.Common
{
    /// <summary>Etiqueta de build visible en el título de cada pantalla, para verificar a simple vista que se está
    /// ejecutando la versión compilada más reciente (útil mientras se ajustan las maquetas visuales).</summary>
    public static class VersionInfo
    {
        public const string Build = "2026-07-31.07";
    }

    public class FrmBase : Form
    {
        protected FrmBase()
        {
            Load += (s, e) => Text = $"{Text}   [build {VersionInfo.Build}]";
        }
    }
}
