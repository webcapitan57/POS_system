const currentTeacher = document.getElementById('teacherId').innerHTML
const likeButtons = document.getElementsByClassName('btn-like')
const unlikeButtons = document.getElementsByClassName('btn-unlike')
const buttonDivs = document.getElementsByClassName('like-buttons')

for (const likeButton of likeButtons) {
    likeButton.addEventListener('click', () => {
        addLikeEvent(parseInt(likeButton.parentElement.id.split('_')[1]), currentTeacher, 1)
    })
}

for (const unlikeButton of unlikeButtons) {
    unlikeButton.addEventListener('click', () => {
        removeLikeEvent(parseInt(unlikeButton.parentElement.id.split('_')[1]), currentTeacher)
    })
}

async function addLikeEvent(answerId, teacherId) {
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

async function removeLikeEvent(answerId, teacherId) {
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
            if (parseInt(buttonDiv.id.split('_')[1]) === parseInt(answerId)){
                buttonDiv.classList.add('liked')
            }
        }
    } else {
        for (const buttonDiv of buttonDivs) {
            if (parseInt(buttonDiv.id.split('_')[1]) === parseInt(answerId)){
                buttonDiv.classList.remove('liked')
            }
        }
    }
}