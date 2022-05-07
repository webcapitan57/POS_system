const removeFlaggedPhotosButton= document.querySelector('#removeFlaggedPhotos');
removeFlaggedPhotosButton.addEventListener('click',()=>DeleteAllFlaggedAnswers()); 




function DeleteFlaggedAnswer(element){

    
    fetch('/api/groups/answer/'+answerId,{
        method: 'DELETE',
        headers:{
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }

    }).then(function (response){if (response.ok){
        element.remove()
    }
    }).catch((e) => {
        alert("Oops, Event: " + e.name + ": " + e.message)
    })
}