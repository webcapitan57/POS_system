const select = document.getElementById('filter-task')
const answers = document.getElementsByClassName('vset-answer')
const buttonsDiv = document.getElementsByClassName('vset-nav-buttons')
const currentTaskId = document.getElementById('currentTaskId').innerHTML

select.value = currentTaskId

select.addEventListener('change', () => {
    const value = select.value;

    for (const answer of answers) {
        if (answer.id === value){
            answer.style.display = ''
        } else {
            answer.style.display = 'none'
        }
    }

    for (const buttonDiv of buttonsDiv) {
        if (buttonDiv.id === value){
            buttonDiv.style.display = 'flex'
        } else {
            buttonDiv.style.display = 'none'
        }
    }
})