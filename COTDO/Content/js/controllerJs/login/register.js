const form = $('.formRegistrar');

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
        clave: $('#txtPassword').val()
    };

    if (Object.values(inputs).some(value => !value.trim()) || !$('#txtRepeatPassword').val().trim()) {
        showAlert(e.target, "Debe completar todos los campos del formulario", "warning");
        return;
    }

    if (!validateDomains(inputs.correo)) {
        showAlert(e.target, "El dominio del correo no está permitido", "warning");
        return;
    }

    if (!validatePasswords(inputs.clave, $('#txtRepeatPassword').val().trim())) {
        showAlert(e.target, "Las contraseñas no coinciden", "warning");
        return;
    }

    $('#btnRegistrar').prop('disabled', true);
    createAccount(inputs, e.target);
}

function validatePasswords(pass1, pass2) {
    return pass1 === pass2;
}

function createAccount(obj, target) {
    showLoader();
    fetch(`${controller}/Register`, {
        method: 'POST',
        headers: {
            'Content-Type': "application/json"
        },
        body: JSON.stringify(obj)
    })
        .then(response => {
            if (!response.ok) {
                Swal.close();
                showAlert(target, "Ocurrió un error al registrar la cuenta. Inténtelo más tarde.", "error");
                $('#btnRegistrar').prop('disabled', false);
                return;
            }

            return response.json();
        })
        .then(result => {
            const { IsSuccess, Message } = result._response;

            if (!IsSuccess) {
                Swal.close();
                showAlert(target, Message, "warning");
                $('#btnRegistrar').prop('disabled', false);
                return;
            }

            showAlert(target, Message, "success");
            Swal.close();
            setTimeout(() => {
                window.location.href = `${controller}/Index`;
            }, 2400);
        })
        .catch(error => {
            Swal.close();
            console.error(`Error: ${error}`);
            showAlert(target, "Ocurrió un error al registrar la cuenta. Inténtelo más tarde.", "error");
            $('#btnRegistrar').prop('disabled', false);
        });
}