# ElectroJoule - Maquetas visuales de pantallas

Maquetas visuales (prototipos de interfaz) del sistema ElectroJoule, generadas a partir de los 24 Casos de Uso de negocio, para capturar pantallas y documentarlas. **No hay lógica de negocio real ni conexión a base de datos/API**: todas las pantallas usan datos de ejemplo (mock) hardcodeados.

## Estructura del repositorio

```
PanelAdministracion/   Panel de Administración - Windows Forms (.NET Framework 4.7.2, C#)
SitioVentas/            Sitio de ventas - HTML / CSS / JS (orientado al Cliente)
```

### Panel de Administración (`PanelAdministracion/`)

Aplicación WinForms (`ElectroJoule.PanelAdmin.sln`). Abrir con Visual Studio y ejecutar `ElectroJoule.PanelAdmin` (form inicial `FrmPrincipal`).

Pantallas incluidas:
- **Catálogo**: Componentes, Categorías, Marcas (alta/edición/baja con validaciones y bloqueos según flujo alternativo).
- **Ventas**: Pedidos (filtro y actualización de estado), Devoluciones (aprobar/rechazar).
- **Contactos**: Clientes, Proveedores (alta/edición/baja lógica).
- **Reportes**: Ventas por período, Movimientos de inventario (con exportación a Excel simulada).
- **Sistema**: Bitácora, Errores.

Los datos de ejemplo están centralizados en `Forms/../Modelos/Mocks.cs`. Los mensajes de error/confirmación de los flujos alternativos se muestran mediante el panel reutilizable `MensajePanel` (`Forms/Common/ControlesAyuda.cs`), y las grillas de búsqueda muestran siempre un estado "sin resultados" cuando corresponde.

### Sitio de ventas (`SitioVentas/`)

Sitio estático HTML/CSS/JS. Abrir `index.html` en un navegador (no requiere servidor).

Pantallas incluidas:
- `index.html` — Catálogo / búsqueda de componentes.
- `carrito.html` — Carrito de compras.
- `checkout.html` — Registrar venta (métodos de pago).
- `comprobante.html` — Comprobante de venta.
- `mis-pedidos.html` — Armar pedido y listado de pedidos propios.
- `solicitar-devolucion.html` — Solicitud de devolución.

Los datos de ejemplo están en `js/mock-data.js`. El carrito persiste en `localStorage` del navegador para simular el flujo de compra entre pantallas.

## Alcance

Ambas interfaces son **maquetas visuales** pensadas para capturar pantallas y documentar los prototipos de cada Caso de Uso. La funcionalidad real (backend, base de datos, capas BLL/DAL, API REST con JWT) se implementará en una etapa posterior.
