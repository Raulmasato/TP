using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Clientes
{
    /// <summary>CU.016 Alta, CU.017 Modificación, CU.018 Baja de Clientes.</summary>
    public class FrmClientes : Form
    {
        private readonly BindingList<Cliente> _clientes = new BindingList<Cliente>(Mocks.Clientes);
        private TextBox _txtBuscar;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnEditar, _btnEliminar;

        public FrmClientes()
        {
            Text = "Gestión de Clientes";
            StartPosition = FormStartPosition.CenterParent;
            Width = 1000;
            Height = 600;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            CargarGrid(_clientes.ToList());
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            var lblBuscar = new Label { Text = "Buscar por nombre, DNI o correo:", AutoSize = true, Location = new Point(10, 18) };
            _txtBuscar = new TextBox { Location = new Point(220, 14), Width = 260 };
            _txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Buscar(); };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(490, 12), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(585, 12), Width = 90 };
            btnLimpiar.Click += (s, e) => { _txtBuscar.Text = ""; CargarGrid(_clientes.ToList()); };
            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(_txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnLimpiar);
            Controls.Add(panelSuperior);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            var btnNuevo = new Button { Text = "Nuevo", Location = new Point(10, 10), Width = 100 };
            btnNuevo.Click += (s, e) => AbrirAltaEdicion(null);
            _btnEditar = new Button { Text = "Editar", Location = new Point(120, 10), Width = 100, Enabled = false };
            _btnEditar.Click += (s, e) => AbrirAltaEdicion(ClienteSeleccionado());
            _btnEliminar = new Button { Text = "Inactivar", Location = new Point(230, 10), Width = 100, Enabled = false };
            _btnEliminar.Click += (s, e) => Inactivar();
            panelBotones.Controls.Add(btnNuevo);
            panelBotones.Controls.Add(_btnEditar);
            panelBotones.Controls.Add(_btnEliminar);
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
            _grid.Columns.Add("Nombre", "Nombre");
            _grid.Columns.Add("Dni", "DNI / CUIT");
            _grid.Columns.Add("Correo", "Correo");
            _grid.Columns.Add("Telefono", "Teléfono");
            _grid.Columns.Add("Direccion", "Dirección");
            _grid.Columns.Add("Estado", "Estado");
            _grid.SelectionChanged += (s, e) =>
            {
                _btnEditar.Enabled = _grid.SelectedRows.Count > 0;
                _btnEliminar.Enabled = _grid.SelectedRows.Count > 0;
            };

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
            contenedorGrid.BringToFront();
            panelSuperior.BringToFront();
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var texto = _txtBuscar.Text.Trim();
            var resultado = string.IsNullOrEmpty(texto)
                ? _clientes.ToList()
                : _clientes.Where(c => c.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0
                                     || c.Dni.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0
                                     || c.Correo.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            CargarGrid(resultado);
        }

        private void CargarGrid(System.Collections.Generic.List<Cliente> lista)
        {
            _grid.Rows.Clear();
            foreach (var c in lista)
            {
                int i = _grid.Rows.Add(c.Nombre, c.Dni, c.Correo, c.Telefono, c.Direccion, c.Activo ? "Activo" : "Inactivo");
                if (!c.Activo) _grid.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnEditar.Enabled = false;
            _btnEliminar.Enabled = false;
        }

        private Cliente ClienteSeleccionado()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            var dni = _grid.SelectedRows[0].Cells["Dni"].Value.ToString();
            return _clientes.FirstOrDefault(c => c.Dni == dni);
        }

        private void AbrirAltaEdicion(Cliente cliente)
        {
            using (var frm = new FrmClienteAltaEdicion(cliente, _clientes.ToList()))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    if (cliente == null)
                        _clientes.Add(frm.ClienteResultado);
                    else
                    {
                        cliente.Nombre = frm.ClienteResultado.Nombre;
                        cliente.Correo = frm.ClienteResultado.Correo;
                        cliente.Telefono = frm.ClienteResultado.Telefono;
                        cliente.Direccion = frm.ClienteResultado.Direccion;
                    }

                    _mensaje.MostrarExito(cliente == null
                        ? $"Cliente '{frm.ClienteResultado.Nombre}' dado de alta correctamente."
                        : $"Cliente '{frm.ClienteResultado.Nombre}' actualizado correctamente.");
                    Buscar();
                }
            }
        }

        private void Inactivar()
        {
            var c = ClienteSeleccionado();
            if (c == null) return;

            if (!c.Activo)
            {
                _mensaje.MostrarError($"El cliente '{c.Nombre}' ya se encuentra inactivo.");
                return;
            }

            var confirmacion = MessageBox.Show(this, $"¿Confirma la baja lógica del cliente '{c.Nombre}'?", "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            c.Activo = false;
            _mensaje.MostrarExito($"Cliente '{c.Nombre}' inactivado correctamente.");
            Buscar();
        }
    }
}
