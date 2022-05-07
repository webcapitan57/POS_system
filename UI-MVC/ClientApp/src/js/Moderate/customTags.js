//const tagInputs = document.getElementsByClassName('new-tag-input')
const addTagButtons = document.querySelectorAll(`[id*="tagAddBtn_"]`)
const tagDivs = document.getElementsByClassName('moderate-tags')
const customTags = document.querySelectorAll(`[id^="tags_"]`)
const deleteTagButtons = document.getElementsByClassName('tag-delete')

const customFilterSelect = document.querySelector('#custom-filter-select')
const images = document.querySelectorAll(`[id*="-image_"]`)

window.addEventListener('load',()=> {
//Adding and removing tags
    for (const addTagButton of addTagButtons) {
        addTagButton.addEventListener('click', () => {
            addTagToAnswer(parseInt(addTagButton.id.split('_')[1]))
        })
    }
})

    export function addTagToAnswer(answerId) {
        let inputelement = document.querySelector(`#tagInput_${answerId}`)
        let value = inputelement.value


        //Checks if value is empty
        if (value === undefined || value === '') {
            alert("Er is geen waarde opgegeven!")
        } else {

            fetch('/api/Answers/' + parseInt(answerId) + '/customTags/add/' + value, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(() => {


                let tagDiv = document.querySelector(`#tags_${answerId}`)
                //Creates HTML for the custom tag element
                let divElement = document.createElement('div');
                divElement.classList.add('moderate-custom-tag');
                divElement.setAttribute('name', value)
                divElement.innerHTML = value;

                let aElement = document.createElement('a');
                aElement.id = answerId;
                aElement.classList.add('tag-delete');
                aElement.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16"> ' +
                    '<path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/> ' +
                    '</svg> '

                aElement.addEventListener('click', () => {
                    removeTag(aElement.parentElement,)
                })

                divElement.appendChild(aElement)
                tagDiv.appendChild(divElement)


                //Adds the value to the custom tag filter select
                let alreadyAdded = false;
                for (const option of customFilterSelect.children) {
                    if (option.getAttribute('value') === value) {
                        alreadyAdded = true;
                    }
                }

                if (alreadyAdded === false) {
                    const optionElement = document.createElement('option');
                    optionElement.value = value;
                    optionElement.innerHTML = value;
                    customFilterSelect.appendChild(optionElement)
                }


                //Makes the custom tag filter visible
                customFilterSelect.parentElement.style.display = 'inline'

            })

            
        }
        //Empties the input field
        inputelement.value = '';
    }
        
    
    function removeTag(divelement) {

        let value = divelement.getAttribute('name')
        
        let answerId=divelement.parentElement.id.split('_')[1]
        fetch('/api/Tags/'+answerId + "/"+value,{
            method: 'DELETE',
            headers:{
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }).then(function (response){if (response.ok){
            divelement.remove()
            //let customFilterDiv=document.querySelector('#custom-filter')
            let canRemove = true;
            for (let customTag of document.querySelectorAll('.moderate-custom-tag')) {
                if (customTag.getAttribute('name') === value) {
                    canRemove = false
                    break
                }
            }
            if (canRemove === true) {
                let select = document.querySelector('#custom-filter-select')
                for (let option of select.querySelectorAll('option')) {
                    let o = option.getAttribute('value')
                    if (option.getAttribute('value') === value) {
                        option.remove()


                    }
                }
            }  
            
        }
        }).catch(() => {alert('Oeps, er ging iets mis')})
        
    }


//Filtering by custom tags
    customFilterSelect.addEventListener('change', () => {
        filterByCustomTag(customFilterSelect.value)
    })

    function filterByCustomTag(value) {
        if (value === '') {
            for (const image of images) {
                image.style.display = 'inline';
            }
        } else {
            for (const image of images) {
                image.style.display = 'none';
            }

            for (const customTagDiv of customTags) {
                    
                if (customTagDiv.children.length !== 0) {
                
                    for (let customTag of customTagDiv.children) {

                    if (customTag.getAttribute('name') === value) {
                        let answerId = parseInt(customTag.parentElement.getAttribute('id').split('_')[1]);
                        for (const image of images) {
                            if (parseInt(image.id.split('_')[1]) === answerId)
                                image.style.display = 'inline';
                        }
                    }


                }
            }
            }
        }

    }


