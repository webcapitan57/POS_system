import {getAssignedPhotos} from './showPhotos.js'
import {AddAssignedPhoto} from './showPhotos.js'
import {RemoveAnswer} from './showPhotos.js'
import {ShowAnswerWithSideAnswers} from './showPhotos'
import {saveSideAnswerInput} from './showPhotos.js'
export const deliveryId = parseInt(document.querySelector('#taskDeliveryId').textContent)
const sentPhotosDiv = document.querySelector('#sentPhotosDiv')
 const studentId=parseInt(document.querySelector('#studentId').textContent)
 let check = document.querySelector('#CurrentAnswer')
let uploadPhotoButton=document.querySelector('#file')
uploadPhotoButton.addEventListener('change',(event)=>UploadPhotos(event.target))
window.addEventListener('load', function () {
    getPhotos()

    
    if (check !==null) {
        getAssignedPhotos(deliveryId)
    }
})

export function attachEventHandlers(element) {

    let allImages = element.querySelectorAll("img")
    let allButtons = element.querySelectorAll("button")
    let allInputs = element.querySelectorAll("input")
    let allSelects = element.querySelectorAll("select")
    for (let image of allImages) {
        if (image.id.includes("choosePhoto_") && check !== null) {
            image.addEventListener('click', () => {
                    AddAssignedPhoto(image)
                }
            )
        }
        if (image.id.includes("assignedPhoto_")) {
            image.addEventListener('click', () => {
                ShowAnswerWithSideAnswers(image)
            })
        }


        for (let button of allButtons) {
            if (button.id.includes("deletePhoto")) {
                button.addEventListener('click', () => {
                    RemoveUploadedPhoto(button)
                })
            }
            if (button.id.includes("deleteAssignedPhoto")) {
                button.addEventListener('click', () => {
                    RemoveAnswer(button)

                })
            }
        }

        for (let input of allInputs) {
            if (input.id.includes("saveSideAnswer_")) {
                input.addEventListener('focusout', () => {
                    saveSideAnswerInput(element)
                })
            }
        }
        for (let select of allSelects) {
            if (select.id.includes("saveSideAnswer_")) {
                saveSideAnswerInput(element)
            }
        }

    }
}

function getPhotos() {
    fetch('/api/Photos/' + deliveryId , {
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
                    showUploadedPhotos(data)
                })
            }
        }
    ).catch(() => {
        alert('Oeps, er ging iets mis')
    })


}
function UploadPhotos(e){

    for (let i=0;i<e.files.length;i++){
        UploadPhoto(e.files[i])
    }
}
function UploadPhoto(photo){

    let formdata= new FormData()
    formdata.append('Image',photo)
    let source=URL.createObjectURL(photo)



    fetch('/api/Photos/studentphoto/' + studentId, {
        method: 'POST',
        body: formdata,
        headers:
            {
                'Accept': 'application/json'
            }

    }).then(function (response) {

        if (response.ok) {
            return response.json().then(function (data) {
                let d=data;
                showUploadedPhotos(data,source)
            })
        }
    }).catch(() => {
        alert('Oeps, er ging iets mis')
    })
}




function showUploadedPhotos(responses,source) {
    responses.forEach(function (response) {
        showUploadedPhoto(response,source)
    })
    const children = sentPhotosDiv.children
    //  sentPhotosDiv.childNodes.forEach(function (child){ownaddEventListener(child)})
    for (let i = 0; i < children.length; i++) {
        attachEventHandlers(sentPhotosDiv.children[i])
        attachEventHandlers(sentPhotosDiv.children[i].children[1])
    }
}

function showUploadedPhoto(response,source) {
    let id =response;
    if (source === undefined){
        source=response.url
        id= response.photoId
    }
    sentPhotosDiv.innerHTML += `
    <div class="task-uploaded-photo">
        <img id="choosePhoto_${id}" src="${source}" alt=""/>
        <button id="deletePhoto_${id}" class="delete-photo-button">
            <svg xmlns="http://www.w3.org/2000/svg" class="bi bi-dash-circle-fill" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-7z"/>
            </svg>
        </button>
    </div>`
}

function RemoveUploadedPhoto(element) {
    const photoId = parseInt(element.getAttribute('id').split('_')[1])
    fetch('/api/Photos/' + photoId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            return response.json()
        }
    }).then(function (data) {
        let d = data;
        let t = 't';
        if (data === false) {
            window.alert("Foto kan niet verwijderd worden omdat er nog antwoorden zijn " +
                "voor deze foto. Gelieve deze antwoorden eerst te verwijderen.")
        }
        if (data === true) {
            element.parentElement.remove()
        }
    })
}