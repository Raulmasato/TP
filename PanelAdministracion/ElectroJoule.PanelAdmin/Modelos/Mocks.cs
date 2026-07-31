using System;
using System.Collections.Generic;

namespace ElectroJoule.PanelAdmin.Modelos
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ComponentesAsociados { get; set; }
        public bool Activa { get; set; } = true;
    }

    public class Marca
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; }
        public int ComponentesAsociados { get; set; }
        public bool Activa { get; set; } = true;
    }

    public class Componente
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Contacto { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class PedidoItem
    {
        public string Componente { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }

    public class Pedido
    {
        public string Numero { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public List<PedidoItem> Detalle { get; set; } = new List<PedidoItem>();
    }

    public class DevolucionItem
    {
        public string Componente { get; set; }
        public int Cantidad { get; set; }
    }

    public class Devolucion
    {
        public string Numero { get; set; }
        public string VentaOrigen { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public List<DevolucionItem> Detalle { get; set; } = new List<DevolucionItem>();
    }

    public class VentaReporte
    {
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Componente { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }

    public class MovimientoInventario
    {
        public DateTime Fecha { get; set; }
        public string Componente { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int StockResultante { get; set; }
        public string Usuario { get; set; }
    }

    public class RegistroBitacora
    {
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Funcionalidad { get; set; }
        public string Accion { get; set; }
    }

    public class RegistroError
    {
        public DateTime Fecha { get; set; }
        public string Funcionalidad { get; set; }
        public string Severidad { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>Datos de ejemplo hardcodeados para poblar las maquetas visuales (sin conexión real a datos).</summary>
    public static class Mocks
    {
        public static List<string> CategoriasNombres => new List<string> { "Resistencias", "Capacitores", "Semiconductores", "Microcontroladores", "Sensores", "Conectores" };
        public static List<string> MarcasNombres => new List<string> { "Texas Instruments", "STMicroelectronics", "Vishay", "Espressif", "Yageo", "Molex" };

        public static List<Categoria> Categorias => new List<Categoria>
        {
            new Categoria { Id = 1, Nombre = "Resistencias", ComponentesAsociados = 3, Activa = true },
            new Categoria { Id = 2, Nombre = "Capacitores", ComponentesAsociados = 2, Activa = true },
            new Categoria { Id = 3, Nombre = "Semiconductores", ComponentesAsociados = 4, Activa = true },
            new Categoria { Id = 4, Nombre = "Microcontroladores", ComponentesAsociados = 2, Activa = true },
            new Categoria { Id = 5, Nombre = "Sensores", ComponentesAsociados = 0, Activa = true },
            new Categoria { Id = 6, Nombre = "Conectores", ComponentesAsociados = 1, Activa = true },
        };

        public static List<Marca> Marcas => new List<Marca>
        {
            new Marca { Id = 1, Nombre = "Texas Instruments", Origen = "Estados Unidos", ComponentesAsociados = 3, Activa = true },
            new Marca { Id = 2, Nombre = "STMicroelectronics", Origen = "Suiza", ComponentesAsociados = 2, Activa = true },
            new Marca { Id = 3, Nombre = "Vishay", Origen = "Estados Unidos", ComponentesAsociados = 2, Activa = true },
            new Marca { Id = 4, Nombre = "Espressif", Origen = "China", ComponentesAsociados = 1, Activa = true },
            new Marca { Id = 5, Nombre = "Yageo", Origen = "Taiwán", ComponentesAsociados = 0, Activa = true },
            new Marca { Id = 6, Nombre = "Molex", Origen = "Estados Unidos", ComponentesAsociados = 1, Activa = true },
        };

        public static List<Componente> Componentes => new List<Componente>
        {
            new Componente { Codigo = "RES-1K-0805", Nombre = "Resistencia 1K Ohm 0805", Descripcion = "Resistencia SMD 1/8W tolerancia 5%", Precio = 15.50m, Stock = 1200, Categoria = "Resistencias", Marca = "Yageo" },
            new Componente { Codigo = "RES-10K-0805", Nombre = "Resistencia 10K Ohm 0805", Descripcion = "Resistencia SMD 1/8W tolerancia 5%", Precio = 15.50m, Stock = 980, Categoria = "Resistencias", Marca = "Yageo" },
            new Componente { Codigo = "RES-4K7-THT", Nombre = "Resistencia 4.7K Ohm THT", Descripcion = "Resistencia de 1/4W con terminales", Precio = 12.00m, Stock = 450, Categoria = "Resistencias", Marca = "Vishay" },
            new Componente { Codigo = "CAP-100N-CER", Nombre = "Capacitor Cerámico 100nF", Descripcion = "Capacitor cerámico multicapa X7R", Precio = 22.30m, Stock = 640, Categoria = "Capacitores", Marca = "Vishay" },
            new Componente { Codigo = "CAP-220U-ELEC", Nombre = "Capacitor Electrolítico 220uF", Descripcion = "Capacitor electrolítico 25V radial", Precio = 85.00m, Stock = 15, Categoria = "Capacitores", Marca = "Vishay" },
            new Componente { Codigo = "TRA-2N2222", Nombre = "Transistor 2N2222", Descripcion = "Transistor NPN de propósito general", Precio = 45.00m, Stock = 320, Categoria = "Semiconductores", Marca = "Texas Instruments" },
            new Componente { Codigo = "DIO-1N4148", Nombre = "Diodo 1N4148", Descripcion = "Diodo de conmutación rápida", Precio = 8.50m, Stock = 0, Categoria = "Semiconductores", Marca = "Texas Instruments" },
            new Componente { Codigo = "REG-7805", Nombre = "Regulador de voltaje 7805", Descripcion = "Regulador lineal 5V 1A TO-220", Precio = 95.00m, Stock = 210, Categoria = "Semiconductores", Marca = "STMicroelectronics" },
            new Componente { Codigo = "IC-LM358", Nombre = "Amplificador Operacional LM358", Descripcion = "Amplificador operacional dual", Precio = 120.00m, Stock = 8, Categoria = "Semiconductores", Marca = "STMicroelectronics" },
            new Componente { Codigo = "MCU-ESP32", Nombre = "Microcontrolador ESP32-WROOM", Descripcion = "Módulo WiFi/BT dual core", Precio = 3200.00m, Stock = 45, Categoria = "Microcontroladores", Marca = "Espressif" },
            new Componente { Codigo = "MCU-ATMEGA328", Nombre = "Microcontrolador ATmega328P", Descripcion = "Microcontrolador AVR de 8 bits", Precio = 2100.00m, Stock = 60, Categoria = "Microcontroladores", Marca = "Texas Instruments" },
            new Componente { Codigo = "CON-USB-C", Nombre = "Conector USB tipo C", Descripcion = "Conector USB-C hembra SMD 16 pines", Precio = 180.00m, Stock = 130, Categoria = "Conectores", Marca = "Molex" },
        };

        public static List<Cliente> Clientes => new List<Cliente>
        {
            new Cliente { Id = 1, Nombre = "Juan Pérez", Dni = "30111222", Correo = "juan.perez@gmail.com", Telefono = "011-4455-6677", Direccion = "Av. Corrientes 1234, CABA", Activo = true },
            new Cliente { Id = 2, Nombre = "María González", Dni = "28555111", Correo = "maria.gonzalez@hotmail.com", Telefono = "011-2233-4455", Direccion = "Calle San Martín 456, La Plata", Activo = true },
            new Cliente { Id = 3, Nombre = "Carlos Rodríguez", Dni = "33999444", Correo = "carlos.rodriguez@outlook.com", Telefono = "0341-555-1122", Direccion = "Bv. Oroño 890, Rosario", Activo = true },
            new Cliente { Id = 4, Nombre = "Electrónica del Sur SRL", Dni = "30-71234567-9", Correo = "compras@electronicadelsur.com.ar", Telefono = "0291-444-3322", Direccion = "Ruta 3 Km 12, Bahía Blanca", Activo = true },
            new Cliente { Id = 5, Nombre = "Lucía Fernández", Dni = "35222888", Correo = "lucia.fernandez@gmail.com", Telefono = "0261-333-9988", Direccion = "San Juan 234, Mendoza", Activo = false },
        };

        public static List<Proveedor> Proveedores => new List<Proveedor>
        {
            new Proveedor { Id = 1, Nombre = "Distribuidora Componentes SA", Cuit = "30-70111222-4", Contacto = "ventas@distcomponentes.com.ar", Activo = true },
            new Proveedor { Id = 2, Nombre = "ImportTech Argentina", Cuit = "30-70555888-1", Contacto = "info@importtech.com.ar", Activo = true },
            new Proveedor { Id = 3, Nombre = "Electrocomponentes del Plata", Cuit = "30-71333999-6", Contacto = "pedidos@ecplata.com.ar", Activo = true },
            new Proveedor { Id = 4, Nombre = "Global Semiconductors Corp.", Cuit = "30-71888444-2", Contacto = "sales@globalsemi.com", Activo = false },
        };

        public static List<Pedido> Pedidos => new List<Pedido>
        {
            new Pedido { Numero = "PED-0001", Cliente = "Juan Pérez", Fecha = new DateTime(2026, 7, 28), Estado = "Pendiente", Total = 4650.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "MCU-ESP32", Cantidad = 1, PrecioUnitario = 3200.00m }, new PedidoItem { Componente = "CON-USB-C", Cantidad = 8, PrecioUnitario = 180.00m } } },
            new Pedido { Numero = "PED-0002", Cliente = "María González", Fecha = new DateTime(2026, 7, 27), Estado = "Confirmado", Total = 1240.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "REG-7805", Cantidad = 8, PrecioUnitario = 95.00m }, new PedidoItem { Componente = "TRA-2N2222", Cantidad = 10, PrecioUnitario = 45.00m } } },
            new Pedido { Numero = "PED-0003", Cliente = "Electrónica del Sur SRL", Fecha = new DateTime(2026, 7, 25), Estado = "En preparación", Total = 21000.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "MCU-ATMEGA328", Cantidad = 10, PrecioUnitario = 2100.00m } } },
            new Pedido { Numero = "PED-0004", Cliente = "Carlos Rodríguez", Fecha = new DateTime(2026, 7, 20), Estado = "Despachado", Total = 850.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "CAP-220U-ELEC", Cantidad = 10, PrecioUnitario = 85.00m } } },
            new Pedido { Numero = "PED-0005", Cliente = "Juan Pérez", Fecha = new DateTime(2026, 7, 15), Estado = "Entregado", Total = 1550.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "RES-1K-0805", Cantidad = 100, PrecioUnitario = 15.50m } } },
            new Pedido { Numero = "PED-0006", Cliente = "María González", Fecha = new DateTime(2026, 7, 10), Estado = "Cancelado", Total = 960.00m,
                Detalle = new List<PedidoItem> { new PedidoItem { Componente = "IC-LM358", Cantidad = 8, PrecioUnitario = 120.00m } } },
        };

        public static List<Devolucion> Devoluciones => new List<Devolucion>
        {
            new Devolucion { Numero = "DEV-0001", VentaOrigen = "VTA-0512", Cliente = "Juan Pérez", Fecha = new DateTime(2026, 7, 29), Estado = "Pendiente", Motivo = "Componente con falla de fábrica (no enciende).",
                Detalle = new List<DevolucionItem> { new DevolucionItem { Componente = "MCU-ESP32", Cantidad = 1 } } },
            new Devolucion { Numero = "DEV-0002", VentaOrigen = "VTA-0498", Cliente = "Carlos Rodríguez", Fecha = new DateTime(2026, 7, 26), Estado = "Pendiente", Motivo = "Cantidad recibida distinta a la solicitada.",
                Detalle = new List<DevolucionItem> { new DevolucionItem { Componente = "RES-10K-0805", Cantidad = 50 } } },
            new Devolucion { Numero = "DEV-0003", VentaOrigen = "VTA-0470", Cliente = "María González", Fecha = new DateTime(2026, 7, 18), Estado = "Aprobada", Motivo = "Componente incorrecto enviado.",
                Detalle = new List<DevolucionItem> { new DevolucionItem { Componente = "REG-7805", Cantidad = 4 } } },
            new Devolucion { Numero = "DEV-0004", VentaOrigen = "VTA-0455", Cliente = "Electrónica del Sur SRL", Fecha = new DateTime(2026, 7, 12), Estado = "Rechazada", Motivo = "Devolución fuera del plazo permitido.",
                Detalle = new List<DevolucionItem> { new DevolucionItem { Componente = "MCU-ATMEGA328", Cantidad = 2 } } },
        };

        public static List<VentaReporte> VentasReporte => new List<VentaReporte>
        {
            new VentaReporte { Numero = "VTA-0512", Fecha = new DateTime(2026, 7, 29), Cliente = "Juan Pérez", Componente = "MCU-ESP32", Cantidad = 1, Total = 3200.00m },
            new VentaReporte { Numero = "VTA-0505", Fecha = new DateTime(2026, 7, 24), Cliente = "María González", Componente = "REG-7805", Cantidad = 8, Total = 760.00m },
            new VentaReporte { Numero = "VTA-0498", Fecha = new DateTime(2026, 7, 20), Cliente = "Carlos Rodríguez", Componente = "RES-10K-0805", Cantidad = 50, Total = 775.00m },
            new VentaReporte { Numero = "VTA-0470", Fecha = new DateTime(2026, 7, 14), Cliente = "Electrónica del Sur SRL", Componente = "CON-USB-C", Cantidad = 20, Total = 3600.00m },
            new VentaReporte { Numero = "VTA-0455", Fecha = new DateTime(2026, 7, 5), Cliente = "Lucía Fernández", Componente = "CAP-100N-CER", Cantidad = 30, Total = 669.00m },
        };

        public static List<MovimientoInventario> Movimientos => new List<MovimientoInventario>
        {
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 29, 10, 15, 0), Componente = "MCU-ESP32", TipoMovimiento = "Egreso (venta)", Cantidad = -1, StockResultante = 45, Usuario = "admin" },
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 28, 16, 40, 0), Componente = "CON-USB-C", TipoMovimiento = "Ingreso (compra)", Cantidad = 100, StockResultante = 130, Usuario = "admin" },
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 24, 9, 5, 0), Componente = "REG-7805", TipoMovimiento = "Egreso (venta)", Cantidad = -8, StockResultante = 210, Usuario = "vendedor1" },
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 20, 14, 22, 0), Componente = "RES-10K-0805", TipoMovimiento = "Egreso (venta)", Cantidad = -50, StockResultante = 980, Usuario = "vendedor1" },
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 18, 11, 0, 0), Componente = "IC-LM358", TipoMovimiento = "Ajuste (devolución)", Cantidad = 4, StockResultante = 8, Usuario = "admin" },
            new MovimientoInventario { Fecha = new DateTime(2026, 7, 10, 8, 30, 0), Componente = "DIO-1N4148", TipoMovimiento = "Egreso (venta)", Cantidad = -25, StockResultante = 0, Usuario = "vendedor2" },
        };

        public static List<RegistroBitacora> Bitacora => new List<RegistroBitacora>
        {
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 31, 9, 12, 0), Usuario = "admin", Funcionalidad = "Gestión de Componentes", Accion = "Alta de componente MCU-ESP32" },
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 30, 17, 5, 0), Usuario = "vendedor1", Funcionalidad = "Gestión de Pedidos", Accion = "Actualización de estado PED-0002 a Confirmado" },
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 30, 15, 48, 0), Usuario = "admin", Funcionalidad = "Gestión de Categorías", Accion = "Intento de baja de categoría 'Resistencias' (bloqueado)" },
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 29, 10, 20, 0), Usuario = "admin", Funcionalidad = "Gestión de Devoluciones", Accion = "Solicitud DEV-0001 registrada como pendiente" },
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 28, 8, 55, 0), Usuario = "vendedor2", Funcionalidad = "Gestión de Clientes", Accion = "Baja lógica de cliente Lucía Fernández" },
            new RegistroBitacora { Fecha = new DateTime(2026, 7, 27, 12, 30, 0), Usuario = "admin", Funcionalidad = "Reportes", Accion = "Exportación de reporte 'Ventas por período' a Excel" },
        };

        public static List<RegistroError> Errores => new List<RegistroError>
        {
            new RegistroError { Fecha = new DateTime(2026, 7, 31, 9, 40, 0), Funcionalidad = "Gestión de Componentes", Severidad = "Advertencia", Mensaje = "Intento de alta con código de componente duplicado: RES-1K-0805" },
            new RegistroError { Fecha = new DateTime(2026, 7, 30, 16, 2, 0), Funcionalidad = "Gestión de Componentes", Severidad = "Advertencia", Mensaje = "Intento de guardar stock negativo (-5) para CAP-220U-ELEC" },
            new RegistroError { Fecha = new DateTime(2026, 7, 29, 21, 15, 0), Funcionalidad = "Sitio de Ventas", Severidad = "Error", Mensaje = "Timeout al conectar con la API REST desde el sitio de ventas" },
            new RegistroError { Fecha = new DateTime(2026, 7, 28, 11, 3, 0), Funcionalidad = "Gestión de Categorías", Severidad = "Advertencia", Mensaje = "Intento de baja de categoría con componentes asociados: Resistencias" },
            new RegistroError { Fecha = new DateTime(2026, 7, 22, 3, 30, 0), Funcionalidad = "Reportes", Severidad = "Crítico", Mensaje = "Fallo al generar exportación a Excel: memoria insuficiente" },
        };
    }
}
