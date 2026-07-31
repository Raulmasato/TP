using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Componentes
{
    public class FrmComponenteAltaEdicion : Form
    {
        private readonly Componente _original;
        private readonly List<Componente> _existentes;
        private readonly bool _esEdicion;

        private TextBox _txtCodigo, _txtNombre, _txtDescripcion, _txtPrecio, _txtStock;
        private ComboBox _cmbCategoria, _cmbMarca;
        private Label _errCodigo, _errStock, _errPrecio;
        private MensajePanel _mensaje;

        public Componente ComponenteResultado { get; private set; }

        public FrmComponenteAltaEdicion(Componente existente, List<Componente> todos)
        {
            _original = existente;
            _existentes = todos;
            _esEdicion = existente != null;

            Text = _esEdicion ? "Editar Componente" : "Nuevo Componente";
            StartPosition = FormStartPosition.CenterParent;
            Width = 480;
            Height = 560;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            if (_esEdicion) CargarDatos(existente);
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(20, 50, 20, 20),
                AutoSize = true
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _txtCodigo = new TextBox { Width = 260 };
            if (_esEdicion) _txtCodigo.ReadOnly = true;
            _errCodigo = CrearLabelError();

            _txtNombre = new TextBox { Width = 260 };
            _txtDescripcion = new TextBox { Width = 260, Height = 60, Multiline = true };
            _txtPrecio = new TextBox { Width = 260 };
            _errPrecio = CrearLabelError();
            _txtStock = new TextBox { Width = 260 };
            _errStock = CrearLabelError();

            _cmbCategoria = new ComboBox { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbCategoria.Items.AddRange(Mocks.CategoriasNombres.Cast<object>().ToArray());

            _cmbMarca = new ComboBox { Width = 260, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbMarca.Items.AddRange(Mocks.MarcasNombres.Cast<object>().ToArray());

            AgregarFila(panel, "Código *:", _txtCodigo, _errCodigo);
            AgregarFila(panel, "Nombre *:", _txtNombre, null);
            AgregarFila(panel, "Descripción:", _txtDescripcion, null);
            AgregarFila(panel, "Precio *:", _txtPrecio, _errPrecio);
            AgregarFila(panel, "Stock *:", _txtStock, _errStock);
            AgregarFila(panel, "Categoría *:", _cmbCategoria, null);
            AgregarFila(panel, "Marca *:", _cmbMarca, null);

            Controls.Add(panel);
            panel.BringToFront();

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 55, Padding = new Padding(10) };
            var btnGuardar = new Button { Text = "Guardar", Width = 100, Location = new Point(150, 12), DialogResult = DialogResult.None };
            btnGuardar.Click += (s, e) => Guardar();
            var btnCancelar = new Button { Text = "Cancelar", Width = 100, Location = new Point(260, 12), DialogResult = DialogResult.Cancel };
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnCancelar);
            Controls.Add(panelBotones);
            CancelButton = btnCancelar;
        }

        private Label CrearLabelError() => new Label
        {
            AutoSize = true,
            ForeColor = Color.FromArgb(192, 57, 43),
            Font = new Font("Segoe UI", 8F, FontStyle.Italic),
            Visible = false,
            Text = ""
        };

        private void AgregarFila(TableLayoutPanel panel, string etiqueta, Control control, Label error)
        {
            var lbl = new Label { Text = etiqueta, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 8, 0, 0) };
            var cont = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0, 3, 0, 3) };
            cont.Controls.Add(control);
            if (error != null) cont.Controls.Add(error);
            panel.Controls.Add(lbl);
            panel.Controls.Add(cont);
        }

        private void CargarDatos(Componente c)
        {
            _txtCodigo.Text = c.Codigo;
            _txtNombre.Text = c.Nombre;
            _txtDescripcion.Text = c.Descripcion;
            _txtPrecio.Text = c.Precio.ToString("0.00");
            _txtStock.Text = c.Stock.ToString();
            _cmbCategoria.SelectedItem = c.Categoria;
            _cmbMarca.SelectedItem = c.Marca;
        }

        private void Guardar()
        {
            _mensaje.Ocultar();
            _errCodigo.Visible = false;
            _errStock.Visible = false;
            _errPrecio.Visible = false;
            bool valido = true;

            if (string.IsNullOrWhiteSpace(_txtCodigo.Text) || string.IsNullOrWhiteSpace(_txtNombre.Text))
            {
                _mensaje.MostrarError("Debe completar todos los campos obligatorios (*).");
                return;
            }

            if (!_esEdicion && _existentes.Any(c => c.Codigo.Equals(_txtCodigo.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                _errCodigo.Text = "Ya existe un componente con este código.";
                _errCodigo.Visible = true;
                valido = false;
            }

            if (!decimal.TryParse(_txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                _errPrecio.Text = "Ingrese un precio válido mayor a 0.";
                _errPrecio.Visible = true;
                valido = false;
            }

            if (!int.TryParse(_txtStock.Text, out int stock) || stock < 0)
            {
                _errStock.Text = "El stock no puede ser negativo.";
                _errStock.Visible = true;
                valido = false;
            }

            if (_cmbCategoria.SelectedItem == null || _cmbMarca.SelectedItem == null)
            {
                _mensaje.MostrarError("Debe seleccionar categoría y marca.");
                valido = false;
            }

            if (!valido)
            {
                _mensaje.MostrarError("Corrija los errores marcados en rojo antes de guardar.");
                return;
            }

            ComponenteResultado = new Componente
            {
                Codigo = _txtCodigo.Text.Trim(),
                Nombre = _txtNombre.Text.Trim(),
                Descripcion = _txtDescripcion.Text.Trim(),
                Precio = precio,
                Stock = stock,
                Categoria = _cmbCategoria.SelectedItem.ToString(),
                Marca = _cmbMarca.SelectedItem.ToString(),
                Activo = true
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
