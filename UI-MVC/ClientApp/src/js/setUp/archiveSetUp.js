addEventHandlersArchive()

function addEventHandlersArchive() {
    const archiveButtons = document.querySelectorAll(`[id *= "archiveSetUp"]`)
    const archiveFields = document.querySelectorAll(`[id *= "archived"]`)

    archiveButtons.forEach(b => b.addEventListener('click', e => archiveSetUp(e.target)))

    archiveFields.forEach(field => {
        const setUpId = field.parentElement.firstElementChild.textContent
        const removeSetUpButton = document.querySelector(`#removeSetUp_${setUpId}`)

        if (field.textContent === "Actief") {
            removeSetUpButton.style.visibility = 'hidden'
        } else {
            removeSetUpButton.style.visibility = 'visible'
        }
    })
}

function archiveSetUp(element) {
    const setUpId = parseInt(element.parentElement.id.split("_")[1]);

    fetch('/api/setups/' + setUpId + '/archive', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            response.json().then(data => {

                if (data === true) {
                    document.querySelector(`#archived_${setUpId}`).textContent = "Gearchiveerd"

                    // show remove setup after archiving setup
                    const removeSetUpButton = document.querySelector(`#removeSetUp_${setUpId}`)
                    removeSetUpButton.style.visibility = 'visible'
                } else {
                    document.querySelector(`#archived_${setUpId}`).textContent = "Actief"

                    // hide remove setup after unarchiving setup
                    const removeSetUpButton = document.querySelector(`#removeSetUp_${setUpId}`)
                    removeSetUpButton.style.visibility = 'hidden'
                }
            })
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'))
}