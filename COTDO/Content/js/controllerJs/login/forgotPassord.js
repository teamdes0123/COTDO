const form = $('.formForgotPass');

$(document).ready(() => {
    $('#txtCedula').mask('000-0000000-0', {
        reverse: true
    });

    form.on('submit', validateForm);
});

function validateForm(e) {
    e.preventDefault();

    const inputs = {
        cedula: $('#txtCedula').val().trim().toLowerCase(),
        correo: $('#txtCorreo').val().trim().toLowerCase(),
    }

    if (Object.values(inputs).some(value => !value.trim())) {
        showAlert(e.target, "Debe completar todos los campos del formulario", "warning");
        return;
    }


    if (!validateDomains(inputs.correo)) {
        showAlert(e.target, "El dominio del correo no está permitido", "warning");
        return;
    }

    $('#btnRecuperar').prop('disabled', true);
    recuperarContraseña(inputs);

}
function logIn(obj) {
    const element = document.querySelector('.formForgotPass');
    showLoader();

    fetch(`${controller}/RecuperarContraseña`, {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(obj)
    })
        .then(response => {
            if (!response.ok) {
                Swal.close();
                showAlert(element, "Ocurrió un error al intentar iniciar sesión. Inténtelo más tarde.", "error");
                $('#btnRecuperar').prop('disabled', false);
                return;
            }

            return response.json();
        })
        .then(result => {
            const { IsSuccess, Message = "" } = result._response;

            if (!IsSuccess) {
                Swal.close();
                showAlert(element, Message, "warning");
                $('#btnRecuperar').prop('disabled', false);
                return;
            }

            Swal.close();
            $('#btnRecuperar').prop('disabled', false);
            setTimeout(() => {
                window.location.replace(`${controllerHome}/Index`);
                Swal.close();
            }, 200)
        })
        .catch(error => {
            Swal.close();
            showAlert(element, "Ocurrió un error al intentar iniciar sesión. Inténtelo más tarde.", "error");
            $('#btnRecuperar').prop('disabled', false);
            console.error(`Error: ${error}`);
        });
}