using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Componentes
{
    /// <summary>CU.001 Alta, CU.002 Modificación, CU.003 Baja de Componentes.</summary>
    public class FrmComponentes : FrmBase
    {
        private readonly BindingList<Componente> _componentes = new BindingList<Componente>(Modelos.Mocks.Componentes);
        private TextBox _txtBuscar;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnEditar;
        private Button _btnEliminar;

        public FrmComponentes()
        {
            Text = "Gestión de Catálogo - Componentes";
            StartPosition = FormStartPosition.CenterParent;
            Width = 980;
            Height = 620;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Shown += (s, e) => CargarGrid(_componentes.ToList());
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            var lblBuscar = new Label { Text = "Buscar por código o nombre:", AutoSize = true, Location = new Point(10, 18) };
            _txtBuscar = new TextBox { Location = new Point(195, 14), Width = 260 };
            _txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Buscar(); };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(465, 12), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(560, 12), Width = 90 };
            btnLimpiar.Click += (s, e) => { _txtBuscar.Text = ""; CargarGrid(_componentes.ToList()); };

            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(_txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnLimpiar);
            Controls.Add(panelSuperior);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            var btnNuevo = new Button { Text = "Nuevo", Location = new Point(10, 10), Width = 100 };
            btnNuevo.Click += (s, e) => AbrirAltaEdicion(null);
            _btnEditar = new Button { Text = "Editar", Location = new Point(120, 10), Width = 100, Enabled = false };
            _btnEditar.Click += (s, e) => AbrirAltaEdicion(ComponenteSeleccionado());
            _btnEliminar = new Button { Text = "Eliminar", Location = new Point(230, 10), Width = 100, Enabled = false };
            _btnEliminar.Click += (s, e) => EliminarSeleccionado();
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
            _grid.Columns.Add("Codigo", "Código");
            _grid.Columns.Add("Nombre", "Nombre");
            _grid.Columns.Add("Precio", "Precio ($)");
            _grid.Columns.Add("Stock", "Stock");
            _grid.Columns.Add("Categoria", "Categoría");
            _grid.Columns.Add("Marca", "Marca");
            _grid.Columns.Add("Estado", "Estado");
            _grid.SelectionChanged += (s, e) =>
            {
                bool haySeleccion = _grid.SelectedRows.Count > 0;
                _btnEditar.Enabled = haySeleccion;
                _btnEliminar.Enabled = haySeleccion;
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
                ? _componentes.ToList()
                : _componentes.Where(c => c.Codigo.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0
                                        || c.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            CargarGrid(resultado);
        }

        private void CargarGrid(System.Collections.Generic.List<Componente> lista)
        {
            _grid.Rows.Clear();
            foreach (var c in lista)
            {
                int i = _grid.Rows.Add(c.Codigo, c.Nombre, c.Precio.ToString("N2"), c.Stock, c.Categoria, c.Marca, c.Activo ? "Activo" : "Inactivo");
                if (c.Stock == 0)
                    _grid.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                if (!c.Activo)
                    _grid.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }
            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnEditar.Enabled = false;
            _btnEliminar.Enabled = false;
        }

        private Componente ComponenteSeleccionado()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            var codigo = _grid.SelectedRows[0].Cells["Codigo"].Value.ToString();
            return _componentes.FirstOrDefault(c => c.Codigo == codigo);
        }

        private void AbrirAltaEdicion(Componente componente)
        {
            using (var frm = new FrmComponenteAltaEdicion(componente, _componentes.ToList()))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    _mensaje.MostrarExito(componente == null
                        ? $"Componente '{frm.ComponenteResultado.Nombre}' dado de alta correctamente."
                        : $"Componente '{frm.ComponenteResultado.Nombre}' actualizado correctamente.");
                    if (componente == null)
                        _componentes.Add(frm.ComponenteResultado);
                    Buscar();
                }
            }
        }

        private void EliminarSeleccionado()
        {
            var c = ComponenteSeleccionado();
            if (c == null) return;

            var confirmacion = MessageBox.Show(this,
                $"¿Confirma la baja lógica del componente '{c.Nombre}' ({c.Codigo})?",
                "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes) return;

            if (!c.Activo)
            {
                _mensaje.MostrarError($"El componente '{c.Nombre}' ya se encuentra dado de baja.");
                return;
            }

            c.Activo = false;
            _mensaje.MostrarExito($"Componente '{c.Nombre}' dado de baja correctamente.");
            Buscar();
        }
    }
}
