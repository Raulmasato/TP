using System.Drawing;
using System.Windows.Forms;

namespace ElectroJoule.PanelAdmin.Forms.Common
{
    /// <summary>Panel de mensajes de error/confirmación reutilizable, para representar en pantalla los flujos alternativos.</summary>
    public class MensajePanel : Panel
    {
        private readonly Label _lblIcono;
        private readonly Label _lblTexto;

        public MensajePanel()
        {
            Height = 40;
            Dock = DockStyle.Top;
            Visible = false;
            Padding = new Padding(10, 8, 10, 8);

            _lblIcono = new Label
            {
                AutoSize = false,
                Width = 24,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _lblTexto = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(_lblTexto);
            Controls.Add(_lblIcono);
        }

        public void MostrarError(string mensaje)
        {
            BackColor = Color.FromArgb(253, 236, 234);
            ForeColor = Color.FromArgb(146, 32, 22);
            _lblIcono.Text = "✕";
            _lblIcono.ForeColor = Color.FromArgb(192, 57, 43);
            _lblTexto.ForeColor = Color.FromArgb(146, 32, 22);
            _lblTexto.Text = mensaje;
            Visible = true;
        }

        public void MostrarExito(string mensaje)
        {
            BackColor = Color.FromArgb(230, 245, 233);
            ForeColor = Color.FromArgb(30, 105, 60);
            _lblIcono.Text = "✓";
            _lblIcono.ForeColor = Color.FromArgb(39, 174, 96);
            _lblTexto.ForeColor = Color.FromArgb(30, 105, 60);
            _lblTexto.Text = mensaje;
            Visible = true;
        }

        public void MostrarAdvertencia(string mensaje)
        {
            BackColor = Color.FromArgb(255, 244, 214);
            ForeColor = Color.FromArgb(133, 100, 4);
            _lblIcono.Text = "!";
            _lblIcono.ForeColor = Color.FromArgb(230, 168, 0);
            _lblTexto.ForeColor = Color.FromArgb(133, 100, 4);
            _lblTexto.Text = mensaje;
            Visible = true;
        }

        public void Ocultar()
        {
            Visible = false;
        }
    }

    /// <summary>Etiqueta de estado "sin resultados" para grillas de búsqueda, según lo pedido: nunca dejar una grilla vacía sin feedback.</summary>
    public class SinResultadosLabel : Label
    {
        public SinResultadosLabel()
        {
            Dock = DockStyle.Fill;
            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            ForeColor = Color.Gray;
            Text = "No se encontraron resultados para la búsqueda ingresada.";
            Visible = false;
        }
    }
}
