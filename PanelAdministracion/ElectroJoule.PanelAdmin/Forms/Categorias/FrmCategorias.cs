using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Categorias
{
    /// <summary>CU.004 Alta, CU.005 Modificación, CU.006 Baja de Categorías.</summary>
    public class FrmCategorias : FrmBase
    {
        private readonly BindingList<Categoria> _categorias = new BindingList<Categoria>(Mocks.Categorias);
        private TextBox _txtBuscar;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Button _btnEditar, _btnEliminar;

        public FrmCategorias()
        {
            Text = "Gestión de Catálogo - Categorías";
            StartPosition = FormStartPosition.CenterParent;
            Width = 700;
            Height = 560;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Shown += (s, e) => CargarGrid(_categorias.ToList());
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
            var lblBuscar = new Label { Text = "Buscar por nombre:", AutoSize = true, Location = new Point(10, 18) };
            _txtBuscar = new TextBox { Location = new Point(150, 14), Width = 220 };
            _txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) Buscar(); };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(380, 12), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnLimpiar = new Button { Text = "Limpiar", Location = new Point(475, 12), Width = 90 };
            btnLimpiar.Click += (s, e) => { _txtBuscar.Text = ""; CargarGrid(_categorias.ToList()); };
            panelSuperior.Controls.Add(lblBuscar);
            panelSuperior.Controls.Add(_txtBuscar);
            panelSuperior.Controls.Add(btnBuscar);
            panelSuperior.Controls.Add(btnLimpiar);
            Controls.Add(panelSuperior);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(10) };
            var btnNuevo = new Button { Text = "Nuevo", Location = new Point(10, 10), Width = 100 };
            btnNuevo.Click += (s, e) => AbrirAltaEdicion(null);
            _btnEditar = new Button { Text = "Editar", Location = new Point(120, 10), Width = 100, Enabled = false };
            _btnEditar.Click += (s, e) => AbrirAltaEdicion(CategoriaSeleccionada());
            _btnEliminar = new Button { Text = "Eliminar", Location = new Point(230, 10), Width = 100, Enabled = false };
            _btnEliminar.Click += (s, e) => Eliminar();
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
            _grid.Columns.Add("Nombre", "Nombre");
            _grid.Columns.Add("Componentes", "Componentes asociados");
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
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var texto = _txtBuscar.Text.Trim();
            var resultado = string.IsNullOrEmpty(texto)
                ? _categorias.ToList()
                : _categorias.Where(c => c.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            CargarGrid(resultado);
        }

        private void CargarGrid(System.Collections.Generic.List<Categoria> lista)
        {
            _grid.Rows.Clear();
            foreach (var c in lista)
                _grid.Rows.Add(c.Nombre, c.ComponentesAsociados);
            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _btnEditar.Enabled = false;
            _btnEliminar.Enabled = false;
        }

        private Categoria CategoriaSeleccionada()
        {
            if (_grid.SelectedRows.Count == 0) return null;
            var nombre = _grid.SelectedRows[0].Cells["Nombre"].Value.ToString();
            return _categorias.FirstOrDefault(c => c.Nombre == nombre);
        }

        private void AbrirAltaEdicion(Categoria categoria)
        {
            using (var frm = new FrmCategoriaAltaEdicion(categoria, _categorias.ToList()))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    if (categoria == null)
                        _categorias.Add(frm.CategoriaResultado);
                    else
                        categoria.Nombre = frm.CategoriaResultado.Nombre;

                    _mensaje.MostrarExito(categoria == null
                        ? $"Categoría '{frm.CategoriaResultado.Nombre}' dada de alta correctamente."
                        : $"Categoría '{frm.CategoriaResultado.Nombre}' actualizada correctamente.");
                    Buscar();
                }
            }
        }

        private void Eliminar()
        {
            var c = CategoriaSeleccionada();
            if (c == null) return;

            if (c.ComponentesAsociados > 0)
            {
                _mensaje.MostrarError($"No es posible eliminar la categoría '{c.Nombre}' porque tiene {c.ComponentesAsociados} componente(s) asociado(s). Reasigne o elimine primero esos componentes.");
                return;
            }

            var confirmacion = MessageBox.Show(this, $"¿Confirma la baja de la categoría '{c.Nombre}'?", "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            _categorias.Remove(c);
            _mensaje.MostrarExito($"Categoría '{c.Nombre}' eliminada correctamente.");
            Buscar();
        }
    }
}
