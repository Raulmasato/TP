using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Errores
{
    /// <summary>CU.Arq.007 Consulta de Errores.</summary>
    public class FrmErrores : FrmBase
    {
        private DateTimePicker _dtDesde, _dtHasta;
        private ComboBox _cmbFuncionalidad, _cmbSeveridad;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;

        public FrmErrores()
        {
            Text = "Consulta de Errores";
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
            panelFiltros.Controls.Add(new Label { Text = "Funcionalidad:", AutoSize = true, Location = new Point(390, 15) });
            _cmbFuncionalidad = new ComboBox { Location = new Point(480, 11), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbFuncionalidad.Items.Add("(Todas)");
            _cmbFuncionalidad.Items.AddRange(Mocks.Errores.Select(e => e.Funcionalidad).Distinct().Cast<object>().ToArray());
            _cmbFuncionalidad.SelectedIndex = 0;
            panelFiltros.Controls.Add(new Label { Text = "Severidad:", AutoSize = true, Location = new Point(680, 15) });
            _cmbSeveridad = new ComboBox { Location = new Point(755, 11), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbSeveridad.Items.AddRange(new object[] { "(Todas)", "Advertencia", "Error", "Crítico" });
            _cmbSeveridad.SelectedIndex = 0;

            panelFiltros.Controls.Add(_dtDesde);
            panelFiltros.Controls.Add(_dtHasta);
            panelFiltros.Controls.Add(_cmbFuncionalidad);
            panelFiltros.Controls.Add(_cmbSeveridad);
            Controls.Add(panelFiltros);

            var panelBotones = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(10, 0, 10, 0) };
            var btnBuscar = new Button { Text = "Buscar", Location = new Point(10, 5), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            panelBotones.Controls.Add(btnBuscar);
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
            _grid.Columns.Add("Funcionalidad", "Funcionalidad");
            _grid.Columns.Add("Severidad", "Severidad");
            _grid.Columns.Add("Mensaje", "Mensaje de error");

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 70, 0, 0) };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Buscar()
        {
            var query = Mocks.Errores.Where(e => e.Fecha.Date >= _dtDesde.Value.Date && e.Fecha.Date <= _dtHasta.Value.Date);
            if (_cmbFuncionalidad.SelectedIndex > 0)
                query = query.Where(e => e.Funcionalidad == _cmbFuncionalidad.SelectedItem.ToString());
            if (_cmbSeveridad.SelectedIndex > 0)
                query = query.Where(e => e.Severidad == _cmbSeveridad.SelectedItem.ToString());

            var lista = query.OrderByDescending(e => e.Fecha).ToList();
            _grid.Rows.Clear();
            foreach (var e in lista)
            {
                int i = _grid.Rows.Add(e.Fecha.ToString("dd/MM/yyyy HH:mm"), e.Funcionalidad, e.Severidad, e.Mensaje);
                _grid.Rows[i].DefaultCellStyle.ForeColor = ColorSeveridad(e.Severidad);
            }

            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
        }

        private Color ColorSeveridad(string severidad)
        {
            switch (severidad)
            {
                case "Crítico": return Color.FromArgb(192, 57, 43);
                case "Error": return Color.FromArgb(211, 84, 0);
                case "Advertencia": return Color.FromArgb(230, 168, 0);
                default: return Color.Black;
            }
        }
    }
}
