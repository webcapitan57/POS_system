
let deleteButtons=document.querySelectorAll(`[id*="removeTask_"]`)

deleteButtons.forEach((element)=>{
    
    
    element.addEventListener('click',()=>{DeleteTask(element)})
})

function DeleteTask(element){
    let taskId=parseInt(element.id.split('_')[1])

    fetch('/api/Tasks/'+taskId,{
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response){if (response.ok){
        DeleteTaskHTML(element)
    }
    }).catch(() => {alert('Oeps, er ging iets mis')})
    
}

function DeleteTaskHTML(element){
    
    element.parentElement.parentElement.remove()
}