using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Pedidos
{
    /// <summary>CU.013 Gestión de Pedidos (parte Administrador).</summary>
    public class FrmPedidos : FrmBase
    {
        private readonly BindingList<Pedido> _pedidos = new BindingList<Pedido>(Mocks.Pedidos);
        private ComboBox _cmbEstado;
        private TextBox _txtCliente;
        private DateTimePicker _dtDesde, _dtHasta;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnVerDetalle;

        public FrmPedidos()
        {
            Text = "Gestión de Pedidos";
            StartPosition = FormStartPosition.CenterParent;
            Width = 1000;
            Height = 620;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Load += (s, e) => CargarGrid(_pedidos.ToList());
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelFiltros = new Panel { Dock = DockStyle.Top, Height = 65, Padding = new Padding(10) };

            panelFiltros.Controls.Add(new Label { Text = "Estado:", AutoSize = true, Location = new Point(10, 15) });
            _cmbEstado = new ComboBox { Location = new Point(65, 11), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbEstado.Items.Add("(Todos)");
            _cmbEstado.Items.AddRange(new object[] { "Pendiente", "Confirmado", "En preparación", "Despachado", "Entregado", "Cancelado" });
            _cmbEstado.SelectedIndex = 0;

            panelFiltros.Controls.Add(new Label { Text = "Cliente:", AutoSize = true, Location = new Point(230, 15) });
            _txtCliente = new TextBox { Location = new Point(285, 11), Width = 180 };

            panelFiltros.Controls.Add(new Label { Text = "Desde:", AutoSize = true, Location = new Point(480, 15) });
            _dtDesde = new DateTimePicker { Location = new Point(525, 11), Width = 110, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 1) };
            panelFiltros.Controls.Add(new Label { Text = "Hasta:", AutoSize = true, Location = new Point(645, 15) });
            _dtHasta = new DateTimePicker { Location = new Point(688, 11), Width = 110, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 31) };

            var btnBuscar = new Button { Text = "Buscar", Location = new Point(810, 9), Width = 80 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(895, 9), Width = 80 };
            btnLimpiar.Click += (s, e) => LimpiarFiltros();

            panelFiltros.Controls.Add(_cmbEstado);
            panelFiltros.Controls.Add(_txtCliente);
            panelFiltros.Controls.Add(_dtDesde);
            panelFiltros.Controls.Add(_dtHasta);
            panelFiltros.Controls.Add(btnBuscar);
            panelFiltros.Controls.Add(btnLimpiar);
            Controls.Add(panelFiltros);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            _btnVerDetalle = new Button { Text = "Ver detalle / Actualizar estado", Location = new Point(10, 10), Width = 220, Enabled = false };
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
            _grid.Columns.Add("Cliente", "Cliente");
            _grid.Columns.Add("Fecha", "Fecha");
            _grid.Columns.Add("Estado", "Estado");
            _grid.Columns.Add("Total", "Total ($)");
            _grid.CellDoubleClick += (s, e) => VerDetalle();
            _grid.SelectionChanged += (s, e) => _btnVerDetalle.Enabled = _grid.SelectedRows.Count > 0;

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void LimpiarFiltros()
        {
            _cmbEstado.SelectedIndex = 0;
            _txtCliente.Text = "";
            _dtDesde.Value = new DateTime(2026, 7, 1);
            _dtHasta.Value = new DateTime(2026, 7, 31);
            CargarGrid(_pedidos.ToList());
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var query = _pedidos.AsEnumerable();

            if (_cmbEstado.SelectedIndex > 0)
                query = query.Where(p => p.Estado == _cmbEstado.SelectedItem.ToString());

            if (!string.IsNullOrWhiteSpace(_txtCliente.Text))
                query = query.Where(p => p.Cliente.IndexOf(_txtCliente.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            query = query.Where(p => p.Fecha.Date >= _dtDesde.Value.Date && p.Fecha.Date <= _dtHasta.Value.Date);

            CargarGrid(query.ToList());
        }

        private void CargarGrid(System.Collections.Generic.List<Pedido> lista)
        {
            _grid.Rows.Clear();
            foreach (var p in lista)
            {
                int i = _grid.Rows.Add(p.Numero, p.Cliente, p.Fecha.ToString("dd/MM/yyyy"), p.Estado, p.Total.ToString("N2"));
                _grid.Rows[i].DefaultCellStyle.ForeColor = ColorEstado(p.Estado);
            }
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnVerDetalle.Enabled = false;
        }

        private Color ColorEstado(string estado)
        {
            switch (estado)
            {
                case "Pendiente": return Color.FromArgb(230, 168, 0);
                case "Cancelado": return Color.FromArgb(192, 57, 43);
                case "Entregado": return Color.FromArgb(39, 174, 96);
                default: return Color.Black;
            }
        }

        private void VerDetalle()
        {
            if (_grid.SelectedRows.Count == 0) return;
            var numero = _grid.SelectedRows[0].Cells["Numero"].Value.ToString();
            var pedido = _pedidos.FirstOrDefault(p => p.Numero == numero);
            if (pedido == null) return;

            using (var frm = new FrmPedidoDetalle(pedido))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    pedido.Estado = frm.NuevoEstado;
                    _mensaje.MostrarExito($"El pedido {pedido.Numero} fue actualizado al estado '{pedido.Estado}'.");
                    Buscar();
                }
            }
        }
    }
}
