const removeMarkedPhotoButtons = document.querySelectorAll(`[id *= "remove"]`)

removeMarkedPhotoButtons.forEach(b => b.addEventListener('click', e => removeMarkedPhoto(e.target)))

function removeMarkedPhoto(element) {

    let answerId = parseInt(element.parentElement.id.split('_')[1])
   
    fetch('/api/markedphotos/' + answerId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                element.parentElement.parentElement.parentElement.remove()
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}