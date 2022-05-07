addEventHandlersDelete()

function addEventHandlersDelete() {
    let removeButtons = document.querySelectorAll(`[id *= "removeSetUp"]`)
    for (let button of removeButtons) {
        button.addEventListener('click', function (e) {
            deleteSetUp(e.target)
        })
    }
}

function deleteSetUp(element) {
    const setUpId = parseInt(element.parentElement.id.split("_")[1]);

    fetch('/api/setups/' + setUpId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest("tr").remove();
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
}