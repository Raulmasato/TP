// Datos de ejemplo (mock) para poblar las maquetas visuales del sitio de ventas ElectroJoule.
// No hay conexión real a una API REST: todo el estado vive en memoria / localStorage del navegador.

const CATALOGO = [
  { codigo: "RES-1K-0805", nombre: "Resistencia 1K Ohm 0805", categoria: "Resistencias", marca: "Yageo", precio: 15.5, stock: 1200, icono: "🟤" },
  { codigo: "RES-10K-0805", nombre: "Resistencia 10K Ohm 0805", categoria: "Resistencias", marca: "Yageo", precio: 15.5, stock: 980, icono: "🟤" },
  { codigo: "RES-4K7-THT", nombre: "Resistencia 4.7K Ohm THT", categoria: "Resistencias", marca: "Vishay", precio: 12.0, stock: 450, icono: "🟤" },
  { codigo: "CAP-100N-CER", nombre: "Capacitor Cerámico 100nF", categoria: "Capacitores", marca: "Vishay", precio: 22.3, stock: 640, icono: "🔵" },
  { codigo: "CAP-220U-ELEC", nombre: "Capacitor Electrolítico 220uF", categoria: "Capacitores", marca: "Vishay", precio: 85.0, stock: 15, icono: "🔵" },
  { codigo: "TRA-2N2222", nombre: "Transistor 2N2222 NPN", categoria: "Semiconductores", marca: "Texas Instruments", precio: 45.0, stock: 320, icono: "⚫" },
  { codigo: "DIO-1N4148", nombre: "Diodo 1N4148", categoria: "Semiconductores", marca: "Texas Instruments", precio: 8.5, stock: 0, icono: "⚫" },
  { codigo: "REG-7805", nombre: "Regulador de voltaje 7805", categoria: "Semiconductores", marca: "STMicroelectronics", precio: 95.0, stock: 210, icono: "⚫" },
  { codigo: "IC-LM358", nombre: "Amplificador Operacional LM358", categoria: "Semiconductores", marca: "STMicroelectronics", precio: 120.0, stock: 8, icono: "🔲" },
  { codigo: "MCU-ESP32", nombre: "Microcontrolador ESP32-WROOM", categoria: "Microcontroladores", marca: "Espressif", precio: 3200.0, stock: 45, icono: "🖥️" },
  { codigo: "MCU-ATMEGA328", nombre: "Microcontrolador ATmega328P", categoria: "Microcontroladores", marca: "Texas Instruments", precio: 2100.0, stock: 60, icono: "🖥️" },
  { codigo: "CON-USB-C", nombre: "Conector USB tipo C", categoria: "Conectores", marca: "Molex", precio: 180.0, stock: 130, icono: "🔌" },
];

const CATEGORIAS = ["Resistencias", "Capacitores", "Semiconductores", "Microcontroladores", "Conectores"];
const MARCAS = ["Texas Instruments", "STMicroelectronics", "Vishay", "Espressif", "Yageo", "Molex"];

const MIS_PEDIDOS = [
  { numero: "PED-0001", fecha: "28/07/2026", estado: "Pendiente", total: 4650.0, items: [{ componente: "MCU-ESP32", cantidad: 1 }, { componente: "CON-USB-C", cantidad: 8 }] },
  { numero: "PED-0005", fecha: "15/07/2026", estado: "Entregado", total: 1550.0, items: [{ componente: "RES-1K-0805", cantidad: 100 }] },
  { numero: "PED-0006", fecha: "10/07/2026", estado: "Cancelado", total: 960.0, items: [{ componente: "IC-LM358", cantidad: 8 }] },
];

const MIS_VENTAS = [
  { numero: "VTA-0512", fecha: "29/07/2026", total: 3200.0, items: [{ componente: "MCU-ESP32", cantidad: 1 }] },
  { numero: "VTA-0470", fecha: "14/07/2026", total: 3600.0, items: [{ componente: "CON-USB-C", cantidad: 20 }] },
  { numero: "VTA-0455", fecha: "05/07/2026", total: 669.0, items: [{ componente: "CAP-100N-CER", cantidad: 30 }] },
];

const MIS_DEVOLUCIONES = [
  { numero: "DEV-0001", ventaOrigen: "VTA-0512", fecha: "29/07/2026", estado: "Pendiente", motivo: "Componente con falla de fábrica (no enciende)." },
  { numero: "DEV-0003", ventaOrigen: "VTA-0470", fecha: "18/07/2026", estado: "Aprobada", motivo: "Componente incorrecto enviado." },
  { numero: "DEV-0004", ventaOrigen: "VTA-0455", fecha: "12/07/2026", estado: "Rechazada", motivo: "Devolución fuera del plazo permitido." },
];

function formatearMoneda(valor) {
  return "$ " + valor.toLocaleString("es-AR", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

function obtenerCarrito() {
  try {
    return JSON.parse(localStorage.getItem("ej_carrito")) || [];
  } catch (e) {
    return [];
  }
}

function guardarCarrito(carrito) {
  localStorage.setItem("ej_carrito", JSON.stringify(carrito));
  actualizarBadgeCarrito();
}

function agregarAlCarrito(codigo, cantidad) {
  const carrito = obtenerCarrito();
  const existente = carrito.find((i) => i.codigo === codigo);
  if (existente) existente.cantidad += cantidad;
  else carrito.push({ codigo, cantidad });
  guardarCarrito(carrito);
}

function actualizarBadgeCarrito() {
  const badge = document.querySelector(".badge-carrito");
  if (!badge) return;
  const total = obtenerCarrito().reduce((acc, i) => acc + i.cantidad, 0);
  badge.textContent = total;
  badge.style.display = total > 0 ? "inline-block" : "none";
}

document.addEventListener("DOMContentLoaded", actualizarBadgeCarrito);
