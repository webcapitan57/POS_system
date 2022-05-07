const unmarkPhotoButtons = document.querySelectorAll(`[id ^= "unmark"]`)

unmarkPhotoButtons.forEach(b => b.addEventListener('click', e => unmarkPhoto(e.target)))

function unmarkPhoto(element) {
    const answerId = parseInt(element.id.split('_')[1])
    
    fetch('/api/markedphotos/' + answerId + '/unmark', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                element.parentElement.parentElement.remove()
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

