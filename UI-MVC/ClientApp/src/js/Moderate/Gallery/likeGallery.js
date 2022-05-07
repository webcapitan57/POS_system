export const currentTeacher = document.getElementById('teacherId').innerHTML
const likeButtons = document.getElementsByClassName('gallery-btn-like')
const unlikeButtons = document.getElementsByClassName('gallery-btn-unlike')
const buttonDivs = document.getElementsByClassName('gallery-like-buttons')

for (const likeButton of likeButtons) {
    likeButton.addEventListener('click', () => {
        addLikeEvent(likeButton.parentElement.id.split('_')[1], currentTeacher, 1)
    })
}

for (const unlikeButton of unlikeButtons) {
    unlikeButton.addEventListener('click', () => {
        removeLikeEvent(unlikeButton.parentElement.id.split('_')[1], currentTeacher)
    })
}

export async function addLikeEvent(answerId, teacherId) {
    return fetch('/api/qualityScoreEvents/' + teacherId + '/' + answerId + '/' + 1 , {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            console.log(response)
            toggleButtons(answerId, true)
        } else {
            throw new Error()
        }
    }).catch((e) => {
        alert("Oops, Event: " + e.name + ": " + e.message)
    })
}

export async function removeLikeEvent(answerId, teacherId) {
    return fetch('/api/qualityScoreEvents/' + teacherId + '/' + answerId , {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            console.log(response)
            toggleButtons(answerId, false)
        } else {
            throw new Error()
        }
    }).catch((e) => {
        alert("Oops, Event: " + e.name + ": " + e.message)
    })
}

async function toggleButtons(answerId, liked) {
    if (liked){
        for (const buttonDiv of buttonDivs) {
            if (buttonDiv.id.split('_')[1] === answerId){
                buttonDiv.classList.add('gallery-liked')
            }
        }
    } else {
        for (const buttonDiv of buttonDivs) {
            if (buttonDiv.id.split('_')[1] === answerId){
                buttonDiv.classList.remove('gallery-liked')
            }
        }
    }
}