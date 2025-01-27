function showAlert(element, msj, type) {
    const existingAlert = document.querySelector('.alertMessage');

    if (existingAlert) {
        existingAlert.remove();
    }

    const newAlert = document.createElement('div');
    newAlert.className = 'alert d-flex justify-content-center align-items-center alertMessage my-3 text-center fw-bold animate__animated animate__fadeIn animate__faster';
    if (type === 'error') {
        newAlert.classList.add('alert-danger');
        newAlert.innerHTML = `
                <svg xmlns="http://www.w3.org/2000/svg" class="bi flex-shrink-0 me-2 mt-1" width="24" height="24" role="img" aria-label="Error:">
                    <path fill="currentColor" d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
                </svg>
            `;
    } else if (type === 'warning') {
        newAlert.classList.add('alert-warning');
        newAlert.innerHTML = `
                <svg xmlns="http://www.w3.org/2000/svg" class="bi flex-shrink-0 me-2 mt-1" width="24" height="24" role="img" aria-label="Warning:">
                    <path fill="currentColor" d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
                </svg>
            `;
    } else {
        newAlert.classList.add('alert-success');
        newAlert.innerHTML = `
                <svg xmlns="http://www.w3.org/2000/svg" class="bi flex-shrink-0 me-2 mt-1" width="24" height="24" role="img" aria-label="Success:">
                    <path fill="currentColor" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                </svg>
            `;
    }

    const message = document.createElement('div');
    message.textContent = msj;
    newAlert.appendChild(message);

    element.appendChild(newAlert);

    setTimeout(() => {
        newAlert.classList.add('animate__fadeOut');
        setTimeout(() => {
            newAlert.remove();
        }, 300)
    }, 5000);
}

function showLoader() {
    Swal.fire({
        title: "Cargando...",
        didOpen: () => {
            Swal.showLoading();
        },
        showClass: {
            popup: `
                animate__animated
                animate__zoomIn
                animate__faster
            `
        },
        hideClass: {
            popup: `
                animate__animated
                animate__zoomOut
                animate__faster
            `
        },
        allowOutsideClick: false
    });
}

function showLoaderCustom(title, msj) {
    Swal.fire({
        title: title,
        html: msj,
        didOpen: () => {
            Swal.showLoading();
        },
        showClass: {
            popup: `
                animate__animated
                animate__zoomIn
                animate__faster
            `
        },
        hideClass: {
            popup: `
                animate__animated
                animate__zoomOut
                animate__faster
            `
        }
    });
}

function validateDomains(email) {
    const domains = [
        "gmail.com",
        "hotmail.com"
    ];

    const domain = email.split("@")[1];

    return domains.some(validDomain => validDomain === domain);
}