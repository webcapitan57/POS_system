for (const image of document.querySelectorAll('.gallery-image')) {
    image.addEventListener('click', () => {

        document.querySelector('#photos').style.display = 'none'
        document.querySelector('#filter-bar').style.display = 'none'
        document.querySelector('#moderate-sliders').style.display = 'none'

        for (const sideAnswersDiv of document.querySelectorAll('.gallery-side-answers')) {
          
            
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
        
        document.querySelector('#photos').style.display = 'flex'
        document.querySelector('#filter-bar').style.display = 'flex'
        document.querySelector('#moderate-sliders').style.display = 'flex'
        
        for (const sideAnswersDiv of document.querySelectorAll('.gallery-side-answers')) {
            sideAnswersDiv.style.display = 'none'
        }
    })
}