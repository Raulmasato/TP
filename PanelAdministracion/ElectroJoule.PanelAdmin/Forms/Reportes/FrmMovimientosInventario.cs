using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Reportes
{
    /// <summary>CU.023 Reporte de Movimientos de Inventario, CU.024 Exportar a Excel.</summary>
    public class FrmMovimientosInventario : FrmBase
    {
        private DateTimePicker _dtDesde, _dtHasta;
        private TextBox _txtComponente;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;

        public FrmMovimientosInventario()
        {
            Text = "Reporte - Movimientos de Inventario";
            StartPosition = FormStartPosition.CenterParent;
            Width = 950;
            Height = 600;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Shown += (s, e) => Buscar();
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelFiltros = new Panel { Dock = DockStyle.Top, Height = 65, Padding = new Padding(10) };
            panelFiltros.Controls.Add(new Label { Text = "Desde:", AutoSize = true, Location = new Point(10, 15) });
            _dtDesde = new DateTimePicker { Location = new Point(60, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 1) };
            panelFiltros.Controls.Add(new Label { Text = "Hasta:", AutoSize = true, Location = new Point(200, 15) });
            _dtHasta = new DateTimePicker { Location = new Point(250, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 31) };
            panelFiltros.Controls.Add(new Label { Text = "Componente (opcional):", AutoSize = true, Location = new Point(390, 15) });
            _txtComponente = new TextBox { Location = new Point(530, 11), Width = 180 };

            var btnBuscar = new Button { Text = "Buscar", Location = new Point(730, 9), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnExportar = new Button { Text = "Exportar a Excel", Location = new Point(825, 9), Width = 110 };
            btnExportar.Click += (s, e) => Exportar();

            panelFiltros.Controls.Add(_dtDesde);
            panelFiltros.Controls.Add(_dtHasta);
            panelFiltros.Controls.Add(_txtComponente);
            panelFiltros.Controls.Add(btnBuscar);
            panelFiltros.Controls.Add(btnExportar);
            Controls.Add(panelFiltros);

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
            _grid.Columns.Add("Componente", "Componente");
            _grid.Columns.Add("Tipo", "Tipo de movimiento");
            _grid.Columns.Add("Cantidad", "Cantidad");
            _grid.Columns.Add("StockResultante", "Stock resultante");
            _grid.Columns.Add("Usuario", "Usuario");

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var query = Mocks.Movimientos.Where(m => m.Fecha.Date >= _dtDesde.Value.Date && m.Fecha.Date <= _dtHasta.Value.Date);
            if (!string.IsNullOrWhiteSpace(_txtComponente.Text))
                query = query.Where(m => m.Componente.IndexOf(_txtComponente.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            var lista = query.ToList();
            _grid.Rows.Clear();
            foreach (var m in lista)
            {
                int i = _grid.Rows.Add(m.Fecha.ToString("dd/MM/yyyy HH:mm"), m.Componente, m.TipoMovimiento, m.Cantidad, m.StockResultante, m.Usuario);
                _grid.Rows[i].DefaultCellStyle.ForeColor = m.Cantidad < 0 ? Color.FromArgb(192, 57, 43) : Color.FromArgb(39, 174, 96);
            }

            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
        }

        private void Exportar()
        {
            if (_grid.Rows.Count == 0)
            {
                _mensaje.MostrarError("No hay datos para exportar. Ajuste los filtros de búsqueda.");
                return;
            }

            bool hayFiltro = !string.IsNullOrWhiteSpace(_txtComponente.Text);
            _mensaje.MostrarExito($"Reporte exportado correctamente a Excel ({_grid.Rows.Count} fila(s), {(hayFiltro ? "listado filtrado" : "listado completo del período")}). Archivo: MovimientosInventario_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }
    }
}
