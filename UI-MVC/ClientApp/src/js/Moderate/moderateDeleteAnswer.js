
const removeFlaggedPhotosButton= document.querySelector('#removeFlaggedPhotos');
removeFlaggedPhotosButton.addEventListener('click',()=>DeleteAllFlaggedAnswers());
const groupCode =document.querySelectorAll(`[id^="group_"]`)[0]
    .getAttribute('id').split("_")[1]
const flaggedList=document.querySelector('#flaggedList')
const deleteButtons = document.querySelectorAll(`[id^="deleteAnswer_"]`)
for (const deleteButton of deleteButtons) {
    deleteButton.addEventListener('click', () => {
        removeAnswer(parseInt(deleteButton.id.split('_')[1]))
    })
}

export function removeAnswer(answerId) {
    return fetch('/api/Groups/answer/' + answerId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then((response) => { if (response.ok){
        removeSideAnswerHTML(answerId)
        removeGalleryImageHTML(answerId)
        showAnswers()}
    })
}

function removeSideAnswerHTML(answerId) {
    
    let sideAnswerDiv=document.querySelector(`#answerDetails_${answerId}`)
    sideAnswerDiv.remove();
    
}

function removeGalleryImageHTML(answerId) {
   
   let galleryImage=document.querySelector(`#gallery-image_${answerId}`)
    galleryImage.remove()
    
}

function showAnswers() {
    document.getElementById('photos').style.display = 'flex'
    document.getElementById('filter-bar').style.display = 'flex'
    document.getElementById('moderate-sliders').style.display = 'flex'

    for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
        sideAnswersDiv.style.display = 'none'
    }
}
function DeleteAllFlaggedAnswers(){

    fetch('/api/groups/group/'+groupCode,{
        method: 'DELETE',
        headers:{
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }

    }).then(function (response){if (response.ok){
        flaggedList.innerHTML="";
    }
    }).catch((e) => {
        alert("Oops, Event: " + e.name + ": " + e.message)
    })
}