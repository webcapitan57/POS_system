
import {attachEventHandlers} from './uploadedPhotos.js'
import {deliveryId} from './uploadedPhotos.js'

//const sentPhotosDiv=document.querySelector('#sentPhotosDiv')
const assignedPhotosDiv = document.querySelector('#AssignedPhotosDiv')
const photoQuestionRef = document.querySelector('#questionRef')
let photoQuestionId=null
//const deliveryId=parseInt(document.querySelector('#taskDeliveryId').textContent)
if (document.querySelector('#photoQuestionId')!=null){
    photoQuestionId =parseInt(document.querySelector('#photoQuestionId').textContent);
    
}

const currentAnswerDiv = document.querySelector('#CurrentAnswer')
// Putting onchange event listener on the div so we can check if there have been any changes to the sideAnswers
let answersChanged=false
//Check that we are in fact on the ShowAnswers page by seeing if there currentAnswerDiv exists
if (currentAnswerDiv!==null) {

    currentAnswerDiv.addEventListener('change', () => {
        answersChanged = true
    })
    photoQuestionRef.addEventListener('click', async () => {
        
        if (answersChanged===true){ await saveSideAnswers()}
        
    });

}




export function getAssignedPhotos(deliveryId) {
    fetch('/api/Photos/' + deliveryId + '/' +photoQuestionId , {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
            if (response.status === parseInt('204')) {
            }
            if (response.status === parseInt('200')) {
                return response.json().then(function (data) {
                    AddAssignedPhotosOnLoad(data)
                })
            }
        }
    )
}

function AddAssignedPhotosOnLoad(responses) {
    responses.forEach(function (response) {
        AddAssignedPhotoOnLoad(response)
    })
}

function AddAssignedPhotoOnLoad(response) {
    let id = response.photoId
    let source = response.url
    AddAssignedPhotoHTML(id, source)
    let children = assignedPhotosDiv.children
    for (let i = 0; i < children.length; i++) {
        attachEventHandlers(assignedPhotosDiv.children[i])

    }
}

export function AddAssignedPhoto(image) {
    let id = parseInt(image.getAttribute('id').split('_')[1])
    let source = image.getAttribute('src')
    let check = checkIfPhotoIsAlreadyAssigned(id)
    if (check === true) return;
    
    AddAssignedPhotoHTML(id, source)
    let children = assignedPhotosDiv.children
    for (let i = 0; i < children.length; i++) {
        attachEventHandlers(assignedPhotosDiv.children[i])

    }
    ShowAnswerWithSideAnswers(image)
}

function AddAssignedPhotoHTML(id, source) {
    assignedPhotosDiv.style.display = 'flex'
    assignedPhotosDiv.innerHTML += `
    <div class="task-uploaded-photo">
        <img id="assignedPhoto_${id}" src="${source}" alt=""/>
        <button id="deleteAssignedPhoto_${id}" class="delete-photo-button">
            <svg xmlns="http://www.w3.org/2000/svg" class="bi bi-dash-circle-fill" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-7z"/>
            </svg>
        </button>
    </div>`
}

function checkIfPhotoIsAlreadyAssigned(id) {
    let bool = false
    let childs = assignedPhotosDiv.children
    if (childs.length === 0) return false
    for (let i = 0; i < childs.length; i++) {
        let child = childs[i].children[0]
        let imId = parseInt(child.getAttribute('id').split('_')[1])
        if (imId === id) bool = true;

    }
    return bool
}

export function ShowAnswerWithSideAnswers(element) {
    const photoId = element.getAttribute('id').split('_')[1]
    GetAnswerWithSideAnswers(element, photoId)
}

function GetAnswerWithSideAnswers(element, photoId) {
    fetch('/api/Answers/' + deliveryId + '/' + photoQuestionId + '/' + photoId, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
            if (response.status === parseInt('204')) CreateAnswerWithSideAnswers(element, photoId, element.getAttribute('src'))
            if (response.status === parseInt('200')) {
                return response.json().then(function (data) {
                    CreateAnswerHMTL(data, element.getAttribute('src'), photoId)
                })
            }
        }
    )
}

function CreateAnswerWithSideAnswers(element, photoId, src) {
    fetch('/api/Answers/' + deliveryId + '/' + photoQuestionId + '/' + photoId, {
        method: 'POST', headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) return response.json();
    })
        .then(function (data) {
            const d = data
            CreateAnswerHMTL(data, src, photoId)
        })
        .catch(() => {
            alert('Oeps, er ging iets mis')
        })
}

function CreateAnswerHMTL(response, src, photoId) {

    for (const assignedPhoto of assignedPhotosDiv.children) {
        assignedPhoto.classList.remove('selected-photo');
    }
    if (currentAnswerDiv.innerHTML!=="" && answersChanged ===true){
        saveSideAnswers()
        answersChanged=false;
    }
    const x = document.getElementById('assignedPhoto_' + photoId);
    x.parentElement.classList.add('selected-photo');

    currentAnswerDiv.style.display = 'flex';
    currentAnswerDiv.setAttribute('id', photoId)
    currentAnswerDiv.innerHTML = `
    <img src="${src}" alt=""/>
    <div id="sideAnswersDiv"></div>`


    const sideAnswersDiv = document.querySelector('#sideAnswersDiv')
    response.sideAnswers.forEach(function (sideAnswer) {
        createSideAnswerHTML(sideAnswer, sideAnswersDiv)
    })
    
    
}

async function saveSideAnswers() {
    
    let sideAnswerMap = []
    let inputs = currentAnswerDiv.querySelectorAll("input")
    let selects = currentAnswerDiv.querySelectorAll("select")
    inputs.forEach(function (input) {
        //const v=input.value
        let id = parseInt(input.getAttribute('id').split('_')[1])
        
        let sideAnswersToUpdate={
            sideAnswerId:id,
            givenAnswer:input.value
        }
        sideAnswerMap.push(sideAnswersToUpdate)
       

    })
    selects.forEach(function (select) {
        let id = parseInt(select.getAttribute('id').split('_')[1])
        let sideAnswersToUpdate={
            sideAnswerId:id,
            givenAnswer:select.value
        }
        sideAnswerMap.push(sideAnswersToUpdate)
       
    })
    
    saveSideAnswerInputs(sideAnswerMap)
   
}

function createSideAnswerHTML(sideAnswer, div) {
    if (sideAnswer.answeredQuestion.sideQuestionOptions.length === 0) {

        div.innerHTML += `
    <div class="form-group">
        <label for="${sideAnswer.sideAnswerId}">${sideAnswer.answeredQuestion.question}</label>
        <input class="form-control" type="text" id="saveSideAnswer_${sideAnswer.sideAnswerId}" value="${sideAnswer.givenAnswer ? sideAnswer.givenAnswer : ''}">
    </div>`
    }
    if (sideAnswer.answeredQuestion.sideQuestionOptions.length !== 0) {
        div.innerHTML += `
    <div class="form-group">
        <label for="${sideAnswer.sideAnswerId}">${sideAnswer.answeredQuestion.question}</label>
        <select class="form-control" id="saveSideAnswer_${sideAnswer.sideAnswerId}">
            <option value="">Kies een antwoord</option>
        </select>
    </div>`
        let selectedValue = sideAnswer.givenAnswer
        AddSideQuestionOptions(sideAnswer.answeredQuestion.sideQuestionOptions, div.querySelector("select"), selectedValue)
    }

}

function AddSideQuestionOptions(options, selectTag, selectedValue) {
    let t = selectTag;

    for (let i = 0; i < options.length; i++) {
        if (selectedValue === options[i].value) {
            selectTag.innerHTML += `<option value="${options[i].value}" selected>${options[i].value}</option>`
        }
        selectTag.innerHTML += `<option value="${options[i].value}">${options[i].value}</option>`
    }


}

export function saveSideAnswerInputs(sideAnswerArray) {

    
    fetch('/api/SideAnswers/' + deliveryId, {
        method: 'PUT',
        body: JSON.stringify(sideAnswerArray),
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'

        }
    }).then(function (response) {
        if (response.ok) {

        }
    }).catch(() => {
        alert('Oeps, er ging iets mis')
    })
}

export function RemoveAnswer(element) {
    let photoId = parseInt(element.getAttribute('id').split('_')[1])
    fetch('/api/Answers/' + photoQuestionId + '/' + photoId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            if (currentAnswerDiv.getAttribute('id') === photoId.toString()) {
                currentAnswerDiv.innerHTML = ""
                currentAnswerDiv.removeAttribute('id')
                currentAnswerDiv.style.display = 'none'
            }
            RemoveAssignedPhotoHTML(element)
        }


    }).catch(() => {
        alert('Oeps, er ging iets mis')
    })

}

function RemoveAssignedPhotoHTML(element) {
    element.parentElement.remove()
    
    if (assignedPhotosDiv.children.length === 0){
        assignedPhotosDiv.style.display = 'none'
    }
}



