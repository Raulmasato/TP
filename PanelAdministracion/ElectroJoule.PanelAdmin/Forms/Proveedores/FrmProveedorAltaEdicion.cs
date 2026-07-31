using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Proveedores
{
    public class FrmProveedorAltaEdicion : Form
    {
        private readonly List<Proveedor> _existentes;
        private readonly bool _esEdicion;
        private TextBox _txtNombre, _txtCuit, _txtContacto;
        private Label _errCuit;
        private MensajePanel _mensaje;

        public Proveedor ProveedorResultado { get; private set; }

        public FrmProveedorAltaEdicion(Proveedor existente, List<Proveedor> todos)
        {
            _existentes = todos;
            _esEdicion = existente != null;

            Text = _esEdicion ? "Editar Proveedor" : "Nuevo Proveedor";
            StartPosition = FormStartPosition.CenterParent;
            Width = 460;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);

            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var lblNombre = new Label { Text = "Nombre *:", AutoSize = true, Location = new Point(20, 55) };
            _txtNombre = new TextBox { Location = new Point(140, 51), Width = 260 };

            var lblCuit = new Label { Text = "CUIT *:", AutoSize = true, Location = new Point(20, 95) };
            _txtCuit = new TextBox { Location = new Point(140, 91), Width = 260 };
            if (_esEdicion) _txtCuit.ReadOnly = true;
            _errCuit = new Label { AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 8F, FontStyle.Italic), Location = new Point(140, 114), Visible = false };

            var lblContacto = new Label { Text = "Contacto *:", AutoSize = true, Location = new Point(20, 145) };
            _txtContacto = new TextBox { Location = new Point(140, 141), Width = 260 };

            Controls.Add(lblNombre);
            Controls.Add(_txtNombre);
            Controls.Add(lblCuit);
            Controls.Add(_txtCuit);
            Controls.Add(_errCuit);
            Controls.Add(lblContacto);
            Controls.Add(_txtContacto);

            var btnGuardar = new Button { Text = "Guardar", Location = new Point(140, 200), Width = 100 };
            btnGuardar.Click += (s, e) => Guardar();
            var btnCancelar = new Button { Text = "Cancelar", Location = new Point(250, 200), Width = 100, DialogResult = DialogResult.Cancel };
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            CancelButton = btnCancelar;

            if (_esEdicion)
            {
                _txtNombre.Text = existente.Nombre;
                _txtCuit.Text = existente.Cuit;
                _txtContacto.Text = existente.Contacto;
            }
        }

        private void Guardar()
        {
            _mensaje.Ocultar();
            _errCuit.Visible = false;

            if (string.IsNullOrWhiteSpace(_txtNombre.Text) || string.IsNullOrWhiteSpace(_txtCuit.Text) || string.IsNullOrWhiteSpace(_txtContacto.Text))
            {
                _mensaje.MostrarError("Debe completar todos los campos obligatorios (*).");
                return;
            }

            if (!_esEdicion && _existentes.Any(p => p.Cuit == _txtCuit.Text.Trim()))
            {
                _errCuit.Text = "Ya existe un proveedor registrado con este CUIT.";
                _errCuit.Visible = true;
                return;
            }

            ProveedorResultado = new Proveedor
            {
                Nombre = _txtNombre.Text.Trim(),
                Cuit = _txtCuit.Text.Trim(),
                Contacto = _txtContacto.Text.Trim(),
                Activo = true
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
