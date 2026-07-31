using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectroJoule.PanelAdmin.Forms.Bitacora;
using ElectroJoule.PanelAdmin.Forms.Categorias;
using ElectroJoule.PanelAdmin.Forms.Clientes;
using ElectroJoule.PanelAdmin.Forms.Componentes;
using ElectroJoule.PanelAdmin.Forms.Devoluciones;
using ElectroJoule.PanelAdmin.Forms.Errores;
using ElectroJoule.PanelAdmin.Forms.Marcas;
using ElectroJoule.PanelAdmin.Forms.Pedidos;
using ElectroJoule.PanelAdmin.Forms.Proveedores;
using ElectroJoule.PanelAdmin.Forms.Reportes;
using ElectroJoule.PanelAdmin.Modelos;

namespace ElectroJoule.PanelAdmin.Forms.Principal
{
    public class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            Text = "ElectroJoule - Panel de Administración";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1024;
            Height = 680;
            Font = new Font("Segoe UI", 9F);

            var menu = ConstruirMenu();
            MainMenuStrip = menu;
            Controls.Add(menu);

            var status = new StatusStrip();
            status.Items.Add(new ToolStripStatusLabel("Usuario: admin") { Spring = false });
            status.Items.Add(new ToolStripStatusLabel { Spring = true });
            status.Items.Add(new ToolStripStatusLabel($"ElectroJoule v1.0 · Conectado · {DateTime.Now:dd/MM/yyyy}"));
            Controls.Add(status);

            Controls.Add(ConstruirPanelPrincipal());
        }

        private MenuStrip ConstruirMenu()
        {
            var menu = new MenuStrip();

            var catalogo = new ToolStripMenuItem("&Catálogo");
            catalogo.DropDownItems.Add("Componentes", null, (s, e) => new FrmComponentes().Show());
            catalogo.DropDownItems.Add("Categorías", null, (s, e) => new FrmCategorias().Show());
            catalogo.DropDownItems.Add("Marcas", null, (s, e) => new FrmMarcas().Show());

            var ventas = new ToolStripMenuItem("&Ventas");
            ventas.DropDownItems.Add("Pedidos", null, (s, e) => new FrmPedidos().Show());
            ventas.DropDownItems.Add("Devoluciones", null, (s, e) => new FrmDevoluciones().Show());

            var contactos = new ToolStripMenuItem("Con&tactos");
            contactos.DropDownItems.Add("Clientes", null, (s, e) => new FrmClientes().Show());
            contactos.DropDownItems.Add("Proveedores", null, (s, e) => new FrmProveedores().Show());

            var reportes = new ToolStripMenuItem("&Reportes");
            reportes.DropDownItems.Add("Ventas por período", null, (s, e) => new FrmVentasPorPeriodo().Show());
            reportes.DropDownItems.Add("Movimientos de inventario", null, (s, e) => new FrmMovimientosInventario().Show());

            var sistema = new ToolStripMenuItem("&Sistema");
            sistema.DropDownItems.Add("Bitácora", null, (s, e) => new FrmBitacora().Show());
            sistema.DropDownItems.Add("Errores", null, (s, e) => new FrmErrores().Show());
            sistema.DropDownItems.Add(new ToolStripSeparator());
            sistema.DropDownItems.Add("Salir", null, (s, e) => Close());

            menu.Items.Add(catalogo);
            menu.Items.Add(ventas);
            menu.Items.Add(contactos);
            menu.Items.Add(reportes);
            menu.Items.Add(sistema);
            return menu;
        }

        private Panel ConstruirPanelPrincipal()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 248), Padding = new Padding(30) };

            var lblTitulo = new Label
            {
                Text = "Bienvenido a ElectroJoule",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 45, 70),
                AutoSize = true,
                Location = new Point(30, 20)
            };
            var lblSub = new Label
            {
                Text = "Sistema de gestión comercial para venta de componentes electrónicos",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(30, 58)
            };
            panel.Controls.Add(lblTitulo);
            panel.Controls.Add(lblSub);

            var tarjetas = new FlowLayoutPanel
            {
                Location = new Point(30, 100),
                Size = new Size(940, 140),
                FlowDirection = FlowDirection.LeftToRight
            };
            tarjetas.Controls.Add(CrearTarjeta("Componentes activos", Mocks.Componentes.Count(c => c.Activo).ToString(), Color.FromArgb(52, 152, 219)));
            tarjetas.Controls.Add(CrearTarjeta("Pedidos pendientes", Mocks.Pedidos.Count(p => p.Estado == "Pendiente").ToString(), Color.FromArgb(243, 156, 18)));
            tarjetas.Controls.Add(CrearTarjeta("Devoluciones pendientes", Mocks.Devoluciones.Count(d => d.Estado == "Pendiente").ToString(), Color.FromArgb(231, 76, 60)));
            tarjetas.Controls.Add(CrearTarjeta("Clientes activos", Mocks.Clientes.Count(c => c.Activo).ToString(), Color.FromArgb(39, 174, 96)));
            panel.Controls.Add(tarjetas);

            var lblAccesos = new Label
            {
                Text = "Accesos rápidos",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 45, 70),
                AutoSize = true,
                Location = new Point(30, 260)
            };
            panel.Controls.Add(lblAccesos);

            var accesos = new FlowLayoutPanel { Location = new Point(30, 295), Size = new Size(940, 250) };
            accesos.Controls.Add(CrearBotonAcceso("Componentes", () => new FrmComponentes().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Categorías", () => new FrmCategorias().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Marcas", () => new FrmMarcas().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Pedidos", () => new FrmPedidos().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Devoluciones", () => new FrmDevoluciones().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Clientes", () => new FrmClientes().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Proveedores", () => new FrmProveedores().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Ventas por período", () => new FrmVentasPorPeriodo().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Mov. de inventario", () => new FrmMovimientosInventario().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Bitácora", () => new FrmBitacora().Show()));
            accesos.Controls.Add(CrearBotonAcceso("Errores", () => new FrmErrores().Show()));
            panel.Controls.Add(accesos);

            return panel;
        }

        private Panel CrearTarjeta(string titulo, string valor, Color color)
        {
            var panel = new Panel { Size = new Size(215, 110), Margin = new Padding(0, 0, 15, 0), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            var barra = new Panel { Dock = DockStyle.Top, Height = 6, BackColor = color };
            var lblValor = new Label { Text = valor, Font = new Font("Segoe UI", 22F, FontStyle.Bold), ForeColor = color, AutoSize = true, Location = new Point(15, 30) };
            var lblTitulo = new Label { Text = titulo, Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true, Location = new Point(15, 80) };
            panel.Controls.Add(barra);
            panel.Controls.Add(lblValor);
            panel.Controls.Add(lblTitulo);
            return panel;
        }

        private Button CrearBotonAcceso(string texto, Action accion)
        {
            var boton = new Button
            {
                Text = texto,
                Size = new Size(150, 60),
                Margin = new Padding(0, 0, 15, 15),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            boton.FlatAppearance.BorderColor = Color.FromArgb(210, 214, 220);
            boton.Click += (s, e) => accion();
            return boton;
        }
    }
}
