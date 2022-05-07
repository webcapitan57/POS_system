for (const image of document.getElementsByClassName('gallery-image')) {
    image.addEventListener('click', () => {

        document.getElementById('gallery').style.display = 'none'
        document.getElementById('filter-bar').style.display = 'none'

        for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
            if (image.id.split('_')[1] === sideAnswersDiv.id.split('_')[1]) {
                sideAnswersDiv.style.display = 'inline'
            } else {
                sideAnswersDiv.style.display = 'none'
            }
        }
    })
}

for (const btnBack of document.getElementsByClassName('btn-gallery-back')) {
    btnBack.addEventListener('click', () => {
        
        document.getElementById('gallery').style.display = 'block'
        document.getElementById('filter-bar').style.display = 'flex'
        
        for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
            sideAnswersDiv.style.display = 'none'
        }
    })
}