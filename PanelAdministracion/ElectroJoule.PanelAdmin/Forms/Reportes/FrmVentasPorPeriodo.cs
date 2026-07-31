using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Reportes
{
    /// <summary>CU.022 Reporte de Ventas por Período, CU.024 Exportar a Excel.</summary>
    public class FrmVentasPorPeriodo : FrmBase
    {
        private DateTimePicker _dtDesde, _dtHasta;
        private TextBox _txtCliente, _txtComponente;
        private DataGridView _grid;
        private SinResultadosLabel _lblSinResultados;
        private MensajePanel _mensaje;
        private Label _lblTotal;

        public FrmVentasPorPeriodo()
        {
            Text = "Reporte - Ventas por Período";
            StartPosition = FormStartPosition.CenterParent;
            Width = 950;
            Height = 600;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
            Load += (s, e) => Buscar();
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelFiltros = new Panel { Dock = DockStyle.Top, Height = 95, Padding = new Padding(10) };
            panelFiltros.Controls.Add(new Label { Text = "Desde:", AutoSize = true, Location = new Point(10, 15) });
            _dtDesde = new DateTimePicker { Location = new Point(60, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 1) };
            panelFiltros.Controls.Add(new Label { Text = "Hasta:", AutoSize = true, Location = new Point(200, 15) });
            _dtHasta = new DateTimePicker { Location = new Point(250, 11), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 7, 31) };

            panelFiltros.Controls.Add(new Label { Text = "Cliente (opcional):", AutoSize = true, Location = new Point(10, 50) });
            _txtCliente = new TextBox { Location = new Point(130, 46), Width = 180 };
            panelFiltros.Controls.Add(new Label { Text = "Componente (opcional):", AutoSize = true, Location = new Point(330, 50) });
            _txtComponente = new TextBox { Location = new Point(470, 46), Width = 180 };

            var btnBuscar = new Button { Text = "Buscar", Location = new Point(670, 44), Width = 90 };
            btnBuscar.Click += (s, e) => Buscar();
            var btnExportar = new Button { Text = "Exportar a Excel", Location = new Point(770, 44), Width = 130 };
            btnExportar.Click += (s, e) => Exportar();

            panelFiltros.Controls.Add(_dtDesde);
            panelFiltros.Controls.Add(_dtHasta);
            panelFiltros.Controls.Add(_txtCliente);
            panelFiltros.Controls.Add(_txtComponente);
            panelFiltros.Controls.Add(btnBuscar);
            panelFiltros.Controls.Add(btnExportar);
            Controls.Add(panelFiltros);

            var panelInferior = new Panel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(10) };
            _lblTotal = new Label { AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Location = new Point(0, 8) };
            panelInferior.Controls.Add(_lblTotal);
            Controls.Add(panelInferior);

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
            _grid.Columns.Add("Numero", "N° Venta");
            _grid.Columns.Add("Fecha", "Fecha");
            _grid.Columns.Add("Cliente", "Cliente");
            _grid.Columns.Add("Componente", "Componente");
            _grid.Columns.Add("Cantidad", "Cantidad");
            _grid.Columns.Add("Total", "Total ($)");

            _lblSinResultados = new SinResultadosLabel();
            var contenedorGrid = new Panel { Dock = DockStyle.Fill };
            contenedorGrid.Controls.Add(_grid);
            contenedorGrid.Controls.Add(_lblSinResultados);
            Controls.Add(contenedorGrid);
        }

        private void Buscar()
        {
            _mensaje.Ocultar();
            var query = Mocks.VentasReporte.Where(v => v.Fecha.Date >= _dtDesde.Value.Date && v.Fecha.Date <= _dtHasta.Value.Date);

            if (!string.IsNullOrWhiteSpace(_txtCliente.Text))
                query = query.Where(v => v.Cliente.IndexOf(_txtCliente.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(_txtComponente.Text))
                query = query.Where(v => v.Componente.IndexOf(_txtComponente.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            var lista = query.ToList();
            _grid.Rows.Clear();
            foreach (var v in lista)
                _grid.Rows.Add(v.Numero, v.Fecha.ToString("dd/MM/yyyy"), v.Cliente, v.Componente, v.Cantidad, v.Total.ToString("N2"));

            _grid.NormalizarVista();
            bool sinResultados = lista.Count == 0;
            _lblSinResultados.Visible = sinResultados;
            _grid.Visible = !sinResultados;
            _lblTotal.Text = $"Total del período filtrado: $ {lista.Sum(v => v.Total):N2}  ({lista.Count} venta(s))";
        }

        private void Exportar()
        {
            if (_grid.Rows.Count == 0)
            {
                _mensaje.MostrarError("No hay datos para exportar. Ajuste los filtros de búsqueda.");
                return;
            }

            bool hayFiltro = !string.IsNullOrWhiteSpace(_txtCliente.Text) || !string.IsNullOrWhiteSpace(_txtComponente.Text);
            _mensaje.MostrarExito($"Reporte exportado correctamente a Excel ({_grid.Rows.Count} fila(s), {(hayFiltro ? "listado filtrado" : "listado completo del período")}). Archivo: VentasPorPeriodo_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }
    }
}
