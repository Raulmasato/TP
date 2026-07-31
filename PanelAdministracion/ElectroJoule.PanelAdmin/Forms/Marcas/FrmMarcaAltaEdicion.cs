using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Marcas
{
    public class FrmMarcaAltaEdicion : FrmBase
    {
        private readonly List<Marca> _existentes;
        private readonly bool _esEdicion;
        private TextBox _txtNombre, _txtOrigen;
        private Label _errNombre;
        private MensajePanel _mensaje;

        public Marca MarcaResultado { get; private set; }

        public FrmMarcaAltaEdicion(Marca existente, List<Marca> todas)
        {
            _existentes = todas;
            _esEdicion = existente != null;

            Text = _esEdicion ? "Editar Marca" : "Nueva Marca";
            StartPosition = FormStartPosition.CenterParent;
            Width = 400;
            Height = 270;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);

            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var lblNombre = new Label { Text = "Nombre *:", AutoSize = true, Location = new Point(20, 55) };
            _txtNombre = new TextBox { Location = new Point(120, 51), Width = 230 };
            _errNombre = new Label { AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 8F, FontStyle.Italic), Location = new Point(120, 74), Visible = false };

            var lblOrigen = new Label { Text = "Origen *:", AutoSize = true, Location = new Point(20, 105) };
            _txtOrigen = new TextBox { Location = new Point(120, 101), Width = 230 };

            Controls.Add(lblNombre);
            Controls.Add(_txtNombre);
            Controls.Add(_errNombre);
            Controls.Add(lblOrigen);
            Controls.Add(_txtOrigen);

            var btnGuardar = new Button { Text = "Guardar", Location = new Point(120, 160), Width = 100 };
            btnGuardar.Click += (s, e) => Guardar();
            var btnCancelar = new Button { Text = "Cancelar", Location = new Point(230, 160), Width = 100, DialogResult = DialogResult.Cancel };
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            CancelButton = btnCancelar;

            if (_esEdicion)
            {
                _txtNombre.Text = existente.Nombre;
                _txtOrigen.Text = existente.Origen;
            }
        }

        private void Guardar()
        {
            _mensaje.Ocultar();
            _errNombre.Visible = false;

            if (string.IsNullOrWhiteSpace(_txtNombre.Text) || string.IsNullOrWhiteSpace(_txtOrigen.Text))
            {
                _mensaje.MostrarError("Debe completar todos los campos obligatorios (*).");
                return;
            }

            if (_existentes.Any(m => m.Nombre.Equals(_txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                _errNombre.Text = "Ya existe una marca con este nombre.";
                _errNombre.Visible = true;
                return;
            }

            MarcaResultado = new Marca { Nombre = _txtNombre.Text.Trim(), Origen = _txtOrigen.Text.Trim(), Activa = true };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
