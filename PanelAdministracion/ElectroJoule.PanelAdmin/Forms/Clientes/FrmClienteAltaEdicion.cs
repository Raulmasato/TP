using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Clientes
{
    public class FrmClienteAltaEdicion : FrmBase
    {
        private readonly List<Cliente> _existentes;
        private readonly bool _esEdicion;
        private TextBox _txtNombre, _txtDni, _txtCorreo, _txtTelefono, _txtDireccion;
        private Label _errDni, _errCorreo;
        private MensajePanel _mensaje;

        public Cliente ClienteResultado { get; private set; }

        public FrmClienteAltaEdicion(Cliente existente, List<Cliente> todos)
        {
            _existentes = todos;
            _esEdicion = existente != null;

            Text = _esEdicion ? "Editar Cliente" : "Nuevo Cliente";
            StartPosition = FormStartPosition.CenterParent;
            Width = 480;
            Height = 480;
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

            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(20, 50, 20, 20), AutoSize = true };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _txtNombre = new TextBox { Width = 260 };
            _txtDni = new TextBox { Width = 260 };
            if (_esEdicion) _txtDni.ReadOnly = true;
            _errDni = CrearError();
            _txtCorreo = new TextBox { Width = 260 };
            _errCorreo = CrearError();
            _txtTelefono = new TextBox { Width = 260 };
            _txtDireccion = new TextBox { Width = 260, Height = 50, Multiline = true };

            AgregarFila(panel, "Nombre *:", _txtNombre, null);
            AgregarFila(panel, "DNI *:", _txtDni, _errDni);
            AgregarFila(panel, "Correo *:", _txtCorreo, _errCorreo);
            AgregarFila(panel, "Teléfono:", _txtTelefono, null);
            AgregarFila(panel, "Dirección:", _txtDireccion, null);

            var panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 55, Padding = new Padding(10) };
            var btnGuardar = new Button { Text = "Guardar", Width = 100, Location = new Point(150, 12) };
            btnGuardar.Click += (s, e) => Guardar();
            var btnCancelar = new Button { Text = "Cancelar", Width = 100, Location = new Point(260, 12), DialogResult = DialogResult.Cancel };
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnCancelar);
            Controls.Add(panelBotones);
            Controls.Add(panel);
            CancelButton = btnCancelar;
        }

        private Label CrearError() => new Label { AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 8F, FontStyle.Italic), Visible = false };

        private void AgregarFila(TableLayoutPanel panel, string etiqueta, Control control, Label error)
        {
            var lbl = new Label { Text = etiqueta, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 8, 0, 0) };
            var cont = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0, 3, 0, 3) };
            cont.Controls.Add(control);
            if (error != null) cont.Controls.Add(error);
            panel.Controls.Add(lbl);
            panel.Controls.Add(cont);
        }

        private void CargarDatos(Cliente c)
        {
            _txtNombre.Text = c.Nombre;
            _txtDni.Text = c.Dni;
            _txtCorreo.Text = c.Correo;
            _txtTelefono.Text = c.Telefono;
            _txtDireccion.Text = c.Direccion;
        }

        private void Guardar()
        {
            _mensaje.Ocultar();
            _errDni.Visible = false;
            _errCorreo.Visible = false;

            if (string.IsNullOrWhiteSpace(_txtNombre.Text) || string.IsNullOrWhiteSpace(_txtDni.Text) || string.IsNullOrWhiteSpace(_txtCorreo.Text))
            {
                _mensaje.MostrarError("Debe completar todos los campos obligatorios (*).");
                return;
            }

            if (!_esEdicion && _existentes.Any(c => c.Dni == _txtDni.Text.Trim()))
            {
                _errDni.Text = "Ya existe un cliente registrado con este DNI/CUIT.";
                _errDni.Visible = true;
                return;
            }

            if (!_txtCorreo.Text.Contains("@") || !_txtCorreo.Text.Contains("."))
            {
                _errCorreo.Text = "Ingrese un correo electrónico válido.";
                _errCorreo.Visible = true;
                return;
            }

            ClienteResultado = new Cliente
            {
                Nombre = _txtNombre.Text.Trim(),
                Dni = _txtDni.Text.Trim(),
                Correo = _txtCorreo.Text.Trim(),
                Telefono = _txtTelefono.Text.Trim(),
                Direccion = _txtDireccion.Text.Trim(),
                Activo = true
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
