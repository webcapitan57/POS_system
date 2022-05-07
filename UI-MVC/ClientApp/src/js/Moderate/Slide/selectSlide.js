const photoDivs = document.getElementsByClassName('photo');
let selected = '1';
const max = photoDivs.length;

showSelected();

const nextButtons = document.getElementsByClassName('btn-next');
const previousButtons = document.getElementsByClassName('btn-previous');

for (const previousButton of previousButtons) {
    previousButton.addEventListener('click', () => {
        let current = Number(selected);
        if (current > 1) {
            selected = String(current - 1);
        }
        showSelected()
    })
}

for (const nextButton of nextButtons) {
    nextButton.addEventListener('click', () => {
        let current = Number(selected);
        if (current < max) {
            selected = String(current + 1);
        }
        showSelected()
    })
}

window.addEventListener("keydown", (event) => {
    if (event.key === 'ArrowRight') {
        let current = Number(selected);
        if (current < max) {
            selected = String(current + 1);
        }
    } else if (event.key === 'ArrowLeft') {
        let current = Number(selected);
        if (current > 1) {
            selected = String(current - 1);
        }

    }
    showSelected()
})

async function showSelected() {
    for (const photoDiv of photoDivs) {
        if (photoDiv.id === selected) {
            photoDiv.parentElement.classList.add('selected')
        } else {
            photoDiv.parentElement.classList.remove('selected')
        }
    }
}