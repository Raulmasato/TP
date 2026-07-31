using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Categorias
{
    public class FrmCategoriaAltaEdicion : Form
    {
        private readonly List<Categoria> _existentes;
        private readonly bool _esEdicion;
        private TextBox _txtNombre;
        private Label _errNombre;
        private MensajePanel _mensaje;

        public Categoria CategoriaResultado { get; private set; }

        public FrmCategoriaAltaEdicion(Categoria existente, List<Categoria> todas)
        {
            _existentes = todas;
            _esEdicion = existente != null;

            Text = _esEdicion ? "Editar Categoría" : "Nueva Categoría";
            StartPosition = FormStartPosition.CenterParent;
            Width = 400;
            Height = 230;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);

            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var lbl = new Label { Text = "Nombre *:", AutoSize = true, Location = new Point(20, 60) };
            _txtNombre = new TextBox { Location = new Point(120, 56), Width = 230 };
            _errNombre = new Label { AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 8F, FontStyle.Italic), Location = new Point(120, 80), Visible = false };
            Controls.Add(lbl);
            Controls.Add(_txtNombre);
            Controls.Add(_errNombre);

            var btnGuardar = new Button { Text = "Guardar", Location = new Point(120, 120), Width = 100 };
            btnGuardar.Click += (s, e) => Guardar();
            var btnCancelar = new Button { Text = "Cancelar", Location = new Point(230, 120), Width = 100, DialogResult = DialogResult.Cancel };
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            CancelButton = btnCancelar;

            if (_esEdicion) _txtNombre.Text = existente.Nombre;
        }

        private void Guardar()
        {
            _mensaje.Ocultar();
            _errNombre.Visible = false;

            if (string.IsNullOrWhiteSpace(_txtNombre.Text))
            {
                _errNombre.Text = "El nombre es obligatorio.";
                _errNombre.Visible = true;
                return;
            }

            if (_existentes.Any(c => c.Nombre.Equals(_txtNombre.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                _errNombre.Text = "Ya existe una categoría con este nombre.";
                _errNombre.Visible = true;
                return;
            }

            CategoriaResultado = new Categoria { Nombre = _txtNombre.Text.Trim(), Activa = true };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
