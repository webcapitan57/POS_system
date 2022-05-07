let filterSelects = document.getElementsByClassName('filter-select');
//let currentFilters = new Map();



for (const filterSelect of filterSelects) {
    filterSelect.addEventListener('change', () => {
        const key = filterSelect.id;
        
        //currentFilters.set(key, filterSelect.value);
        filterAnswers(key,filterSelect.value)
       // filterAnswersByCurrentFilters()
        
    })
}

async function filterAnswers(){


    let currentFilters = new Map();

    for (let filter of filterSelects){
        currentFilters.set(filter.id,filter.value)
    }
    let allTags= document.querySelectorAll(`[id^="tags_"]`)
    for ( let phototags of allTags){
        let id=phototags.id.split('_')[1]
        let image=document.querySelector(`#gallery-image_${id}`)
        for (let photoTag of phototags.children){

            let canShow=true

            for (let [key, value] of currentFilters){
                
                if (photoTag.innerText.split(':')[0].trim().toLowerCase()===key.toLowerCase() &&
                    photoTag.innerText.split(':')[1].trim().toLowerCase()!==value.toLowerCase() && value!==""){
                    canShow=false
                    break
                }


            }
            if (canShow===false){

                image.parentElement.style.display = 'none'

                break;
            }
            if (canShow===true){
                image.parentElement.style.display = 'block'
            }


        }
    }
  
   
    
    
}


async function filterAnswersByCurrentFilters() {

    let photos = document.getElementsByClassName('gallery-image')
    
    for (const photo of photos) {
        let showing = true;

        for (const [key, value] of currentFilters) {
            
               await fetch('/api/tags/' + photo.id + '/' + key, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(function (response) {
                    if (response.ok) {
                        return response.json();
                    }
                }).then(function (data) {
                    if (value !== '' && data.value !== value) {
                        showing = false
                    }
                }).then(() => {
                    if (!showing) {
                        photo.parentElement.style.display = 'none'
                    } else {
                        photo.parentElement.style.display = 'block'
                    }
                }).catch((e) => {
                    console.log(e.message)
                })
        }
    }
}

export function FilterValueExists(tag){
    
    
    for (let selectFilter of document.querySelectorAll('.filter-select')){
        let exists=false;
       if (tag.description.toLowerCase() === selectFilter.id.toString().toLowerCase()){
           
           for (let filterValue of selectFilter){
               
               if (filterValue.getAttribute('value').toString().toLowerCase()===tag.value.toLowerCase()){
                   exists=true
                   break;
               }
           }
           if (exists===false){
               let option=document.createElement('option')
               option.setAttribute('value',tag.value)
               option.innerHTML=tag.value
               
               selectFilter.appendChild(option)
           }
       } 
    }
}

