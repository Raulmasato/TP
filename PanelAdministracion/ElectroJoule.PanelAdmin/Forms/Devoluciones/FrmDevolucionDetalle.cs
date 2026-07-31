using System.Drawing;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Devoluciones
{
    public class FrmDevolucionDetalle : FrmBase
    {
        private readonly Devolucion _devolucion;
        private MensajePanel _mensaje;

        public string EstadoResultado { get; private set; }

        public FrmDevolucionDetalle(Devolucion devolucion)
        {
            _devolucion = devolucion;
            Text = $"Detalle de Devolución {devolucion.Numero}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 560;
            Height = 480;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F);

            ConstruirUI();
        }

        private void ConstruirUI()
        {
            _mensaje = new MensajePanel();
            Controls.Add(_mensaje);

            var panelInfo = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(15, 45, 15, 5) };
            panelInfo.Controls.Add(new Label { Text = $"Venta de origen: {_devolucion.VentaOrigen}", AutoSize = true, Location = new Point(0, 0), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            panelInfo.Controls.Add(new Label { Text = $"Cliente: {_devolucion.Cliente}", AutoSize = true, Location = new Point(0, 25) });
            panelInfo.Controls.Add(new Label { Text = $"Fecha de solicitud: {_devolucion.Fecha:dd/MM/yyyy}", AutoSize = true, Location = new Point(0, 50) });
            panelInfo.Controls.Add(new Label { Text = $"Estado actual: {_devolucion.Estado}", AutoSize = true, Location = new Point(0, 75) });
            Controls.Add(panelInfo);

            var lblMotivo = new Label { Text = $"Motivo: {_devolucion.Motivo}", AutoSize = false, Dock = DockStyle.Top, Height = 58, Padding = new Padding(15, 0, 15, 0) };
            Controls.Add(lblMotivo);

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            grid.AplicarEstiloBase();
            grid.Columns.Add("Componente", "Componente a devolver");
            grid.Columns.Add("Cantidad", "Cantidad");
            foreach (var item in _devolucion.Detalle)
                grid.Rows.Add(item.Componente, item.Cantidad);
            grid.NormalizarVista();

            var panelInferior = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(15, 10, 15, 10) };
            var esPendiente = _devolucion.Estado == "Pendiente";

            var btnAprobar = new Button { Text = "Aprobar", Location = new Point(0, 5), Width = 110, Enabled = esPendiente, BackColor = Color.FromArgb(230, 245, 233) };
            btnAprobar.Click += (s, e) => Resolver("Aprobada");
            var btnRechazar = new Button { Text = "Rechazar", Location = new Point(120, 5), Width = 110, Enabled = esPendiente, BackColor = Color.FromArgb(253, 236, 234) };
            btnRechazar.Click += (s, e) => Resolver("Rechazada");
            var btnCerrar = new Button { Text = "Cerrar", Location = new Point(340, 5), Width = 100, DialogResult = DialogResult.Cancel };

            panelInferior.Controls.Add(btnAprobar);
            panelInferior.Controls.Add(btnRechazar);
            panelInferior.Controls.Add(btnCerrar);
            Controls.Add(panelInferior);
            Controls.Add(grid);
            CancelButton = btnCerrar;

            if (!esPendiente)
                _mensaje.MostrarAdvertencia($"Esta solicitud ya fue resuelta como '{_devolucion.Estado}'. No admite nuevas acciones.");
        }

        private void Resolver(string estado)
        {
            EstadoResultado = estado;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
