document.querySelectorAll(`[id^="grid-image_"]`)

for (const previewPhoto of document.getElementsByClassName('preview-photo')) {
    previewPhoto.addEventListener('click', () => {
        if (!previewPhoto.classList.contains('grid-filter-away') && !previewPhoto.classList.contains('custom-filter-away')) {
            for (const sideAnswersDiv of document.querySelectorAll(`[id^="sideAnswer_"]`)) {
                if (previewPhoto.id.split('_')[1] === sideAnswersDiv.id.split('_')[1] && !sideAnswersDiv.classList.contains('selected-side-answers')) {
                    sideAnswersDiv.classList.add('selected-side-answers')
                } else {
                    sideAnswersDiv.classList.remove('selected-side-answers')
                }
            }
        }
    })
}