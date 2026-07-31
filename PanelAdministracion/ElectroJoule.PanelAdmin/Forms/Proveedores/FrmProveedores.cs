using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Proveedores
{
    /// <summary>CU.019 Alta, CU.020 Modificación, CU.021 Baja de Proveedores.</summary>
    public class FrmProveedores : FrmBase
    {
        private readonly BindingList<Proveedor> _proveedores = new BindingList<Proveedor>(Mocks.Proveedores);
        private TextBox _txtBuscar;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnEditar, _btnEliminar;

        public FrmProveedores()
        {
            Text = "Gestión de Proveedores";
            StartPosition = FormStartPosition.CenterParent;
            Width = 900;
            Height = 580;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Shown += (s, e) => CargarGrid(_proveedores.ToList());
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            var lblBuscar = new Label { Text = "Buscar por nombre o CUIT:", AutoSize = true, Location = new Point(10, 18) };
            _txtBuscar = new TextBox { Location = new Point(190, 14), Width = 260 };
            _txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Buscar(); };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(460, 12), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(555, 12), Width = 90 };
            btnLimpiar.Click += (s, e) => { _txtBuscar.Text = ""; CargarGrid(_proveedores.ToList()); };
            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(_txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnLimpiar);
            Controls.Add(panelSuperior);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            var btnNuevo = new Button { Text = "Nuevo", Location = new Point(10, 10), Width = 100 };
            btnNuevo.Click += (s, e) => AbrirAltaEdicion(null);
            _btnEditar = new Button { Text = "Editar", Location = new Point(120, 10), Width = 100, Enabled = false };
            _btnEditar.Click += (s, e) => AbrirAltaEdicion(ProveedorSeleccionado());
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
            _grid.AplicarEstiloBase();
            _grid.Columns.Add("Nombre", "Nombre / Razón social");
            _grid.Columns.Add("Cuit", "CUIT");
            _grid.Columns.Add("Contacto", "Contacto");
            _grid.Columns.Add("Estado", "Estado");
            _grid.SelectionChanged += (s, e) =>
            {
                _btnEditar.Enabled = _grid.SelectedRows.Count > 0;
                _btnEliminar.Enabled = _grid.SelectedRows.Count > 0;
            };

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 45, 0, 0) };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var texto = _txtBuscar.Text.Trim();
            var resultado = string.IsNullOrEmpty(texto)
                ? _proveedores.ToList()
                : _proveedores.Where(p => p.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0
                                        || p.Cuit.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            CargarGrid(resultado);
        }

        private void CargarGrid(System.Collections.Generic.List<Proveedor> lista)
        {
            _grid.Rows.Clear();
            foreach (var p in lista)
            {
                int i = _grid.Rows.Add(p.Nombre, p.Cuit, p.Contacto, p.Activo ? "Activo" : "Inactivo");
                if (!p.Activo) _grid.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }
            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnEditar.Enabled = false;
            _btnEliminar.Enabled = false;
        }

        private Proveedor ProveedorSeleccionado()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            var cuit = _grid.SelectedRows[0].Cells["Cuit"].Value.ToString();
            return _proveedores.FirstOrDefault(p => p.Cuit == cuit);
        }

        private void AbrirAltaEdicion(Proveedor proveedor)
        {
            using (var frm = new FrmProveedorAltaEdicion(proveedor, _proveedores.ToList()))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    if (proveedor == null)
                        _proveedores.Add(frm.ProveedorResultado);
                    else
                    {
                        proveedor.Nombre = frm.ProveedorResultado.Nombre;
                        proveedor.Contacto = frm.ProveedorResultado.Contacto;
                    }

                    _mensaje.MostrarExito(proveedor == null
                        ? $"Proveedor '{frm.ProveedorResultado.Nombre}' dado de alta correctamente."
                        : $"Proveedor '{frm.ProveedorResultado.Nombre}' actualizado correctamente.");
                    Buscar();
                }
            }
        }

        private void Inactivar()
        {
            var p = ProveedorSeleccionado();
            if (p == null) return;

            if (!p.Activo)
            {
                _mensaje.MostrarError($"El proveedor '{p.Nombre}' ya se encuentra inactivo.");
                return;
            }

            var confirmacion = MessageBox.Show(this, $"¿Confirma la baja lógica del proveedor '{p.Nombre}'?", "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            p.Activo = false;
            _mensaje.MostrarExito($"Proveedor '{p.Nombre}' inactivado correctamente.");
            Buscar();
        }
    }
}
