﻿const form = $('.formLogin');

$(document).ready(() => {
    $('#txtCedula').mask('000-0000000-0', {
        reverse: true
    });
    form.on('submit', validateForm);
});

function validateForm(e) {
    e.preventDefault();

    const inputs = {
        username: $('#txtCedula').val().trim().toLowerCase(),
        password: $('#txtPassword').val().trim()
    }

    if (Object.values(inputs).some(inputValue => !inputValue.trim())) {
        showAlert(e.target, 'Por favor, ingrese tanto el usuario como la contraseña para iniciar sesión.', 'warning');
        return;
    }

    $('#btnLogin').prop('disabled', true);
    logIn(inputs);
}

function logIn(obj) {
    const element = document.querySelector('.formLogin');
    showLoader();

    fetch(`${controller}/Login`, {
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
                $('#btnLogin').prop('disabled', false);
                return;
            }

            return response.json();
        })
        .then(result => {
            const { IsSuccess, Message = "" } = result._response;

            if (!IsSuccess) {
                Swal.close();
                showAlert(element, Message, "warning");
                $('#btnLogin').prop('disabled', false);
                return;
            }

            Swal.close();
            $('#btnLogin').prop('disabled', false);
            setTimeout(() => {
                window.location.replace(`${controllerHome}/Index`);
                Swal.close();
            }, 200)
        })
        .catch(error => {
            Swal.close();
            showAlert(element, "Ocurrió un error al intentar iniciar sesión. Inténtelo más tarde.", "error");
            $('#btnLogin').prop('disabled', false);
            console.error(`Error: ${error}`);
        });
}