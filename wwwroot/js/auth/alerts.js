// Mostrar alertas de TempData
document.addEventListener('DOMContentLoaded', function () {
    const alertContainer = document.getElementById('alert-container');

    // Mostrar alerta de éxito
    if (tempData.successMessage) {
        showAlert('success', tempData.successMessage);
    }

    // Mostrar alerta de error
    if (tempData.errorMessage) {
        showAlert('danger', tempData.errorMessage);
    }

    // Cerrar automáticamente después de 5 segundos
    setTimeout(() => {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        });
    }, 5000);
});

// Función para mostrar alertas
function showAlert(type, message) {
    const alertContainer = document.getElementById('alert-container');
    const alert = document.createElement('div');

    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.role = 'alert';
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

    alertContainer.appendChild(alert);
}

// Objeto global con datos de TempData (simulado)
const tempData = {
    successMessage: '@TempData["SuccessMessage"]',
    errorMessage: '@TempData["ErrorMessage"]'
};