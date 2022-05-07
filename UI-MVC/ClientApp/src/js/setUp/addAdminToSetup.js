for (const adminToAddElement of document.getElementsByClassName("btn-add-setup")) {
    adminToAddElement.addEventListener("click", () => addSetup(adminToAddElement))
}

function addSetup(admin) {
    const userId = parseInt(admin.parentElement.parentElement.querySelector('th').textContent)
    const setupId = document.querySelector('#setup_id').textContent

    fetch('/api/Admins/' + userId + '/' + setupId, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }

    }).then(function (response) {
        if (response.ok) {

            const parent = admin.parentElement
            const newAdmin = admin.cloneNode(true)
            parent.replaceChild(newAdmin, admin)

            parent.parentElement.classList.add('bg-primary')
            newAdmin.classList.replace('btn-add-task', 'btn-remove-task')
            newAdmin.innerHTML =
                '<svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" fill="currentColor" className="bi bi-check2" viewBox="0 0 16 16">' +
                ' <path d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z"/>' +
                '</svg>'

        }
    }).catch(() => {
        alert('Oeps something went wrong')
    })

}
