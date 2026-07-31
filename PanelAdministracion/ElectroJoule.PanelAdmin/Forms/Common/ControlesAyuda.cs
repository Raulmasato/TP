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

    public static class DataGridViewExtensions
    {
        /// <summary>
        /// Fuerza el pintado "clásico" (no delegado al tema de Windows) del encabezado de columnas.
        /// EnableHeadersVisualStyles=true (el valor por defecto) delega el dibujo del header a uxtheme.dll,
        /// lo que en ciertas combinaciones de Windows/tema/RDP puede pintarlo con altura o colores inconsistentes.
        /// Fijar el estilo explícitamente evita depender de ese renderizado.
        /// </summary>
        public static void AplicarEstiloBase(this DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 30;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 241, 245);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(35, 40, 50);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.RowTemplate.Height = 24;
        }

        /// <summary>Asegura que, tras poblar la grilla, quede posicionada desde la primera fila (sin scroll ni selección residual)
        /// y fuerza un repintado completo, para evitar que el primer pintado quede parcial (header/primera fila no dibujados).</summary>
        public static void NormalizarVista(this DataGridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                grid.ClearSelection();
                grid.CurrentCell = null;
                grid.FirstDisplayedScrollingRowIndex = 0;
            }
            grid.Invalidate(true);
            grid.Update();
        }
    }
}
