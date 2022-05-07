let customSelect = document.getElementById('custom-filter-select')
const photos = document.querySelectorAll(`[id^="grid-image_"]`)
const customTags = document.getElementsByClassName('moderate-custom-tag')

customSelect.addEventListener('change', () => {
    filterGridByCustomTag(customSelect.value)
})

function filterGridByCustomTag(value) {
    
    if (value === ''){
        for (const photo of photos) {
            photo.classList.remove('custom-filter-away')
        }
    } else {
        for (const photo of photos) {
            photo.classList.add('custom-filter-away')
        }
        
        for (const tag of customTags) {
            let showing = true;
            if (tag.getAttribute('name') !== value){
                showing = false;
            }

            for (const photo of photos) {
                if (photo.id.split('_')[1] === tag.parentElement.id.split('_')[1] && showing){
                        photo.classList.remove('custom-filter-away')
                }
            }
        }
    }

    for (const photo of photos) {
        if (photo.classList.contains('custom-filter-away')){
            for (const sideAnswersDiv of document.querySelectorAll(`[id^="sideAnswer_"]`)) {
                if (photo.id.split('_')[1] === sideAnswersDiv.id.split('_')[1]) {
                    sideAnswersDiv.classList.remove('selected-side-answers')
                }
            }
        }
    }
}