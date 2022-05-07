const flaggedSection = document.querySelector("#flagged")
const notFlaggedSection = document.querySelector("#notFlagged")

const sliders=document.querySelector('#moderate-sliders')

const showFlaggedButton = document.querySelector("#showFlagged")
const removeFlaggedPhotosButton = document.querySelector("#removeFlaggedPhotos")

const flagButtons = document.getElementsByClassName('flag-button')
const unflagButtons = document.getElementsByClassName('unflag-button')

addEventListeners()

function addEventListeners() {
    showFlaggedButton.addEventListener('click', e => checkFlagged(e.target))
    removeFlaggedPhotosButton.addEventListener('click', () => removeFlaggedPhotos())

    for (const flagButton of flagButtons) {
        
        flagButton.addEventListener('click', () => flagPhoto(parseInt(flagButton.id.split('_')[1])))
    }

    for (const unflagButton of unflagButtons) {
        unflagButton.addEventListener('click', () => unflagPhoto(parseInt(unflagButton.id.split('_')[1])))
    }
}

function checkFlagged(element) {
    if (element.checked) {
        showFlagged()
    } else {
        showNotflagged()
    }
}

function showFlagged() {
    flaggedSection.style.display = 'inline'
    notFlaggedSection.style.display = 'none'
}

function showNotflagged() {
    flaggedSection.style.display = 'none'
    notFlaggedSection.style.display = 'inline'
}

function removeFlaggedPhotos() {
    const flaggedAnswers = flaggedSection.querySelectorAll(`[id *= "answer"]`)
    flaggedAnswers.forEach(fa => removeFlaggedPhoto(fa))
}

function removeFlaggedPhoto(element) {
    let photoQuestionId = element.firstElementChild.id.split('_')[1]
    let photoId = element.firstElementChild.id.split('_')[1]

    fetch('/api/markedphotos/' + photoQuestionId + '/' + photoId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            element.parentElement.remove()
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function flagPhoto(answerId) {
    
    fetch('/api/Answers/' + answerId + '/flag', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            let answer = document.querySelector(`#gallery-image_${answerId}`).parentElement
            let clone = answer.cloneNode(true)
            answer.remove()
            flaggedSection.querySelector('ul').appendChild(clone)
            addImageEventListeners()
            changeFlagButtonToUnflag(answerId)
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function unflagPhoto(answerId) {
    answerId=parseInt(answerId)
    fetch('/api/Answers/' + answerId + '/unflag', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(response => {
        if (response.ok) {
            let answer = document.querySelector(`#gallery-image_${answerId}`).parentElement
            let clone = answer.cloneNode(true)
            answer.remove()
            notFlaggedSection.querySelector('ul').appendChild(clone)
            addImageEventListeners()
            changeUnflagButtonToFlag(answerId)
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

function changeFlagButtonToUnflag(answerId) {
    for (const flagButtonDiv of document.getElementsByClassName('moderate-flag-buttons')) {
        if (parseInt(flagButtonDiv.id.split('_')[1]) === answerId){
            flagButtonDiv.classList.add('moderate-flagged')
        }
    }
}

function changeUnflagButtonToFlag(answerId) {
    for (const flagButtonDiv of document.getElementsByClassName('moderate-flag-buttons')) {
        if (parseInt(flagButtonDiv.id.split('_')[1]) === answerId){
            flagButtonDiv.classList.remove('moderate-flagged')
        }
    }
}

function addImageEventListeners() {
    for (const image of document.getElementsByClassName('gallery-image')) {
        image.addEventListener('click', () => {

            document.getElementById('photos').style.display = 'none'
            document.getElementById('filter-bar').style.display = 'none'

            for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
                if (image.id.split('_')[1]=== sideAnswersDiv.id.split('_')[1]) {
                    sideAnswersDiv.style.display = 'inline'
                } else {
                    sideAnswersDiv.style.display = 'none'
                }
            }
        })
    }
}