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
        cedula: $('#txtCedula').val(),
        correo: $('#txtCorreo').val(),
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

    createAccount(inputs);
}

function validatePasswords(pass1, pass2) {
    return pass1 === pass2;
}

function validateDomains(email) {
    const domains = [
        "gmail.com",
        "hotmail.com"
    ];

    const domain = email.split("@")[1];

    return domains.some(validDomain => validDomain === domain);
}

function createAccount(obj) {
    fetch(`${controller}/Register`, {
        method: 'POST',
        headers: {
            'Content-Type': "application/json"
        },
        body: JSON.stringify(obj)
    })
        .then(response => {
            if (!response.ok) {
                showAlert(e.target, "Ocurrió un error al registrar la cuenta. Inténtelo más tarde.", "warning");
                return;
            }

            return response.json();
        })
        .then(result => {
            console.log(result);
        })
        .catch(error => {
            console.error(`Error: ${error}`);
            showAlert(e.target, "Ocurrió un error al registrar la cuenta. Inténtelo más tarde.", "warning");
        });
}