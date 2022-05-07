for (const image of document.querySelectorAll('.gallery-image')) {
    image.addEventListener('click', () => {

        document.querySelector('#section-gallery').style.display = 'none'

        for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
            if (image.id.split('_')[1] === sideAnswersDiv.id.split('_')[1]) {
                sideAnswersDiv.style.display = 'inline'
            } else {
                sideAnswersDiv.style.display = 'none'
            }
        }
    })
}

for (const btnBack of document.querySelectorAll('.btn-gallery-back')) {
    btnBack.addEventListener('click', () => {
        
        document.querySelector('#section-gallery').style.display = 'block'
        
        for (const sideAnswersDiv of document.querySelectorAll('.gallery-side-answers')) {
            sideAnswersDiv.style.display = 'none'
        }
    })
}