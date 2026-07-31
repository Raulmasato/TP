using System.Drawing;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Common;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Pedidos
{
    public class FrmPedidoDetalle : FrmBase
    {
        private readonly Pedido _pedido;
        private ComboBox _cmbEstado;
        private MensajePanel _mensaje;

        public string NuevoEstado { get; private set; }

        public FrmPedidoDetalle(Pedido pedido)
        {
            _pedido = pedido;
            Text = $"Detalle del Pedido {pedido.Numero}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 560;
            Height = 520;
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

            var panelInfo = new Panel { Dock = DockStyle.Top, Height = 125, Padding = new Padding(15, 45, 15, 5) };
            panelInfo.Controls.Add(new Label { Text = $"Cliente: {_pedido.Cliente}", AutoSize = true, Location = new Point(0, 0), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) });
            panelInfo.Controls.Add(new Label { Text = $"Fecha del pedido: {_pedido.Fecha:dd/MM/yyyy}", AutoSize = true, Location = new Point(0, 25) });
            Controls.Add(panelInfo);

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
            grid.Columns.Add("Componente", "Componente");
            grid.Columns.Add("Cantidad", "Cantidad");
            grid.Columns.Add("PrecioUnitario", "Precio unit. ($)");
            grid.Columns.Add("Subtotal", "Subtotal ($)");
            foreach (var item in _pedido.Detalle)
                grid.Rows.Add(item.Componente, item.Cantidad, item.PrecioUnitario.ToString("N2"), item.Subtotal.ToString("N2"));
            grid.NormalizarVista();

            var panelInferior = new Panel { Dock = DockStyle.Bottom, Height = 130, Padding = new Padding(15, 10, 15, 10) };
            var lblTotal = new Label
            {
                Text = $"Total: $ {_pedido.Total:N2}",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            panelInferior.Controls.Add(lblTotal);

            panelInferior.Controls.Add(new Label { Text = "Actualizar estado:", AutoSize = true, Location = new Point(0, 40) });
            _cmbEstado = new ComboBox { Location = new Point(130, 36), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbEstado.Items.AddRange(new object[] { "Pendiente", "Confirmado", "En preparación", "Despachado", "Entregado", "Cancelado" });
            _cmbEstado.SelectedItem = _pedido.Estado;
            panelInferior.Controls.Add(_cmbEstado);

            var btnActualizar = new Button { Text = "Actualizar estado", Location = new Point(130, 75), Width = 200 };
            btnActualizar.Click += (s, e) => ActualizarEstado();
            panelInferior.Controls.Add(btnActualizar);

            var btnCerrar = new Button { Text = "Cerrar", Location = new Point(340, 75), Width = 100, DialogResult = DialogResult.Cancel };
            panelInferior.Controls.Add(btnCerrar);
            CancelButton = btnCerrar;

            Controls.Add(panelInferior);
            Controls.Add(grid);
        }

        private void ActualizarEstado()
        {
            if (_cmbEstado.SelectedItem == null)
            {
                _mensaje.MostrarError("Debe seleccionar un estado.");
                return;
            }

            var nuevo = _cmbEstado.SelectedItem.ToString();
            if (nuevo == _pedido.Estado)
            {
                _mensaje.MostrarAdvertencia("El pedido ya se encuentra en este estado.");
                return;
            }

            if (_pedido.Estado == "Cancelado" || _pedido.Estado == "Entregado")
            {
                _mensaje.MostrarError($"No es posible modificar el estado de un pedido {_pedido.Estado.ToLower()}.");
                return;
            }

            NuevoEstado = nuevo;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
