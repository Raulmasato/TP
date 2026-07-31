using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Devoluciones
{
    /// <summary>CU.015 Gestión de Devoluciones (parte Administrador).</summary>
    public class FrmDevoluciones : Form
    {
        private readonly BindingList<Devolucion> _devoluciones = new BindingList<Devolucion>(Mocks.Devoluciones);
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnVerDetalle;
        private CheckBox _chkSoloPendientes;

        public FrmDevoluciones()
        {
            Text = "Gestión de Devoluciones";
            StartPosition = FormStartPosition.CenterParent;
            Width = 900;
            Height = 560;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Load += (s, e) => Filtrar();
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            _chkSoloPendientes = new CheckBox { Text = "Mostrar solo solicitudes pendientes", AutoSize = true, Location = new Point(10, 18), Checked = true };
            _chkSoloPendientes.CheckedChanged += (s, e) => Filtrar();
            panelSuperior.Controls.Add(_chkSoloPendientes);
            Controls.Add(panelSuperior);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            _btnVerDetalle = new Button { Text = "Ver detalle / Aprobar / Rechazar", Location = new Point(10, 10), Width = 240, Enabled = false };
            _btnVerDetalle.Click += (s, e) => VerDetalle();
            panelBotones.Controls.Add(_btnVerDetalle);
            Controls.Add(panelBotones);

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _grid.Columns.Add("Numero", "Número");
            _grid.Columns.Add("VentaOrigen", "Venta de origen");
            _grid.Columns.Add("Cliente", "Cliente");
            _grid.Columns.Add("Fecha", "Fecha solicitud");
            _grid.Columns.Add("Estado", "Estado");
            _grid.CellDoubleClick += (s, e) => VerDetalle();
            _grid.SelectionChanged += (s, e) => _btnVerDetalle.Enabled = _grid.SelectedRows.Count > 0;

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Filtrar()
        {
            _mensaje.Ocultar();
            var lista = _chkSoloPendientes.Checked
                ? _devoluciones.Where(d => d.Estado == "Pendiente").ToList()
                : _devoluciones.ToList();

            _grid.Rows.Clear();
            foreach (var d in lista)
            {
                int i = _grid.Rows.Add(d.Numero, d.VentaOrigen, d.Cliente, d.Fecha.ToString("dd/MM/yyyy"), d.Estado);
                if (d.Estado == "Aprobada") _grid.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                if (d.Estado == "Rechazada") _grid.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
            }
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnVerDetalle.Enabled = false;
        }

        private void VerDetalle()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var numero = _grid.SelectedRows[0].Cells["Numero"].Value.ToString();
            var devolucion = _devoluciones.FirstOrDefault(d => d.Numero == numero);
            if (devolucion == null) return;

            using (var frm = new FrmDevolucionDetalle(devolucion))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    devolucion.Estado = frm.EstadoResultado;
                    _mensaje.MostrarExito($"La solicitud {devolucion.Numero} fue {(devolucion.Estado == "Aprobada" ? "aprobada" : "rechazada")}.");
                    Filtrar();
                }
            }
        }
    }
}
