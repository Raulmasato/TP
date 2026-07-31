using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Bitacora
{
    /// <summary>CU.Arq.002 Consulta de Bitácora.</summary>
    public class FrmBitacora : FrmBase
    {
        private DateTimePicker _dtDesde, _dtHasta;
        private TextBox _txtUsuario;
        private ComboBox _cmbFuncionalidad;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;

        public FrmBitacora()
        {
            Text = "Consulta de Bitácora";
            StartPosition = FormStartPosition.CenterParent;
            Width = 950;
            Height = 580;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Shown += (s, e) => Buscar();
        }

        private void ConstruirUI()
        {
            var panelFiltros = new Panel { Dock = DockStyle.Top, Height = 65, Padding = new Padding(10) };
            panelFiltros.Controls.Add(new Label { Text = "Desde:", AutoSize = true, Location = new Point(10, 15) });
            _dtDesde = new DateTimePicker { Location = new Point(60, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 1) };
            panelFiltros.Controls.Add(new Label { Text = "Hasta:", AutoSize = true, Location = new Point(200, 15) });
            _dtHasta = new DateTimePicker { Location = new Point(250, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 31) };
            panelFiltros.Controls.Add(new Label { Text = "Usuario:", AutoSize = true, Location = new Point(390, 15) });
            _txtUsuario = new TextBox { Location = new Point(450, 11), Width = 120 };
            panelFiltros.Controls.Add(new Label { Text = "Funcionalidad:", AutoSize = true, Location = new Point(590, 15) });
            _cmbFuncionalidad = new ComboBox { Location = new Point(680, 11), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbFuncionalidad.Items.Add("(Todas)");
            _cmbFuncionalidad.Items.AddRange(Mocks.Bitacora.Select(b => b.Funcionalidad).Distinct().Cast<object>().ToArray());
            _cmbFuncionalidad.SelectedIndex = 0;

            panelFiltros.Controls.Add(_dtDesde);
            panelFiltros.Controls.Add(_dtHasta);
            panelFiltros.Controls.Add(_txtUsuario);
            panelFiltros.Controls.Add(_cmbFuncionalidad);
            Controls.Add(panelFiltros);

            var panelBotones = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(10, 0, 10, 0) };
            var btnBuscarReal = new Button { Text = "Buscar", Location = new Point(10, 5), Width = 90 };
            btnBuscarReal.Click += (s, e) => Buscar();
            panelBotones.Controls.Add(btnBuscarReal);
            Controls.Add(panelBotones);

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _grid.AplicarEstiloBase();
            _grid.Columns.Add("Fecha", "Fecha y hora");
            _grid.Columns.Add("Usuario", "Usuario");
            _grid.Columns.Add("Funcionalidad", "Funcionalidad");
            _grid.Columns.Add("Accion", "Acción registrada");

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 70, 0, 0) };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Buscar()
        {
            var query = Mocks.Bitacora.Where(b => b.Fecha.Date >= _dtDesde.Value.Date && b.Fecha.Date <= _dtHasta.Value.Date);
            if (!string.IsNullOrWhiteSpace(_txtUsuario.Text))
                query = query.Where(b => b.Usuario.IndexOf(_txtUsuario.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            if (_cmbFuncionalidad.SelectedIndex > 0)
                query = query.Where(b => b.Funcionalidad == _cmbFuncionalidad.SelectedItem.ToString());

            var lista = query.OrderByDescending(b => b.Fecha).ToList();
            _grid.Rows.Clear();
            foreach (var b in lista)
                _grid.Rows.Add(b.Fecha.ToString("dd/MM/yyyy HH:mm"), b.Usuario, b.Funcionalidad, b.Accion);

            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
        }
    }
}
