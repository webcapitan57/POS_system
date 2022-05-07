for (const setupToAddElement of document.getElementsByClassName("btn-add-setup")) {
    setupToAddElement.addEventListener("click", () => addSetup(setupToAddElement))
}

for (const setupToRemoveElement of document.getElementsByClassName("btn-remove-setup")) {
    setupToRemoveElement.addEventListener("click", () => removeSetup(setupToRemoveElement))
}

function addSetup(setup) {
    const setupId = parseInt(setup.parentElement.parentElement.querySelector('th').textContent)
    const userId = document.querySelector('#user_id').textContent

    fetch('/api/Admins/' + userId + '/' + setupId, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }

    }).then(function (response) {
        if (response.ok) {

            const parent = setup.parentElement
            const newSetup = setup.cloneNode(true)
            parent.replaceChild(newSetup, setup)

            parent.parentElement.classList.add('bg-primary')
            newSetup.classList.replace('btn-add-task', 'btn-remove-task')
            newSetup.innerHTML =
                "<svg xmlns='http://www.w3.org/2000/svg' width='40' height='40' fill='currentColor' className='bi bi-dash-circle-fill' viewBox='0 0 16 16'> " +
                "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-7z'/>" +
                "</svg>"

            newSetup.addEventListener("click", () => removeSetup(newSetup))
        }
    }).catch(() => {
        alert('Oeps something went wrong')
    })

}

function removeSetup(setup) {
    const setupId = parseInt(setup.parentElement.parentElement.querySelector('th').textContent)
    const userId = document.querySelector('#user_id').textContent

    fetch('/api/Admins/' + userId + '/' + setupId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {

            const parent = setup.parentElement
            const newSetup = setup.cloneNode(true)
            parent.replaceChild(newSetup, setup)

            parent.parentElement.classList.remove('bg-primary')
            newSetup.classList.replace('btn-remove-task', 'btn-add-task')
            newSetup.innerHTML =
                "<svg xmlns='http://www.w3.org/2000/svg' width='40' height='40' fill='currentColor' " +
                "className='bi bi-plus-circle-fill' viewBox='0 0 16 16'> " +
                "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z'/> " +
                "</svg>"

            newSetup.addEventListener("click", () => addSetup(newSetup))
        }
    }).catch(() => {
        alert('Oeps something went wrong')
    })

}

