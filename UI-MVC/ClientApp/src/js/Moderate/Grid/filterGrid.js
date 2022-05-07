let filterSelects = document.getElementsByClassName('filter-select');
let currentFilters = new Map();
const photos = document.getElementsByClassName('preview-photo')


for (const filterSelect of filterSelects) {
    filterSelect.addEventListener('change', () => {
        //const key = filterSelect.id;
        //currentFilters.set(key, filterSelect.value);

        filterAnswers(filterSelect.id,filterSelect.value)
        //filterAnswersByCurrentFilters()
    })
}

async function filterAnswers(){
    let currentFilters = new Map();

    for (let filter of filterSelects){
        currentFilters.set(filter.id,filter.value)
    }
    let allTags= document.querySelectorAll(`[id^="tags_"]`)
    for ( let phototags of allTags) {
        let id = phototags.id.split('_')[1]
        let image = document.querySelector(`#grid-image_${id}`)
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

                image.classList.add('grid-filter-away')
                
                

                break;
            }
            if (canShow===true){
                image.classList.remove('grid-filter-away')
            }


        }
    }
    
    
    
}

async function filterAnswersByCurrentFilters() {
    for (const photo of photos) {
        let showing = true;

        for (const [key, value] of currentFilters) {
            
                fetch('/api/tags/' + photo.id + '/' + key, {
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
                        photo.classList.add('grid-filter-away')
                        for (const sideAnswersDiv of document.getElementsByClassName('side-answers')) {
                            if (photo.id === sideAnswersDiv.id) {
                                sideAnswersDiv.classList.remove('selected-side-answers')
                            }
                        }
                    } else {
                        photo.classList.remove('grid-filter-away')
                    }
                }).catch((e) => {

                })
        }
    }
}
