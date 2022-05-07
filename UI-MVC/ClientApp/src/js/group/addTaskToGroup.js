for (const tasksToAddElement of document.getElementsByClassName("btn-add-task")) {
    tasksToAddElement.addEventListener("click", () => addTask(tasksToAddElement))
}

for (const tasksToRemoveElement of document.getElementsByClassName("btn-remove-task")) {
    tasksToRemoveElement.addEventListener("click", () => removeTask(tasksToRemoveElement))
}

function addTask(task) {
    const taskId = parseInt(task.parentElement.parentElement.querySelector('th').textContent)
    const groupCode = document.querySelector('#groupId').textContent

    fetch('/api/GroupTasks/' + groupCode + '/' + taskId, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }

    }).then(function (response) {
        if (response.ok) {

            const parent = task.parentElement
            const newTask = task.cloneNode(true)
            parent.replaceChild(newTask, task)

            parent.parentElement.classList.add('bg-primary')
            newTask.classList.replace('btn-add-task', 'btn-remove-task')
            newTask.innerHTML =
                "<svg xmlns='http://www.w3.org/2000/svg' width='40' height='40' fill='currentColor' className='bi bi-dash-circle-fill' viewBox='0 0 16 16'> " +
                "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM4.5 7.5a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-7z'/>" +
                "</svg>"

            newTask.addEventListener("click", () => removeTask(newTask))
        }
    }).catch(() => {
        alert('Oeps something went wrong')
    })

}

function removeTask(task) {
    const taskId = parseInt(task.parentElement.parentElement.querySelector('th').textContent)
    const groupCode = document.querySelector('#groupId').textContent

    fetch('/api/GroupTasks/' + groupCode + '/' + taskId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {

            const parent = task.parentElement
            const newTask = task.cloneNode(true)
            parent.replaceChild(newTask, task)

            parent.parentElement.classList.remove('bg-primary')
            newTask.classList.replace('btn-remove-task', 'btn-add-task')
            newTask.innerHTML =
                "<svg xmlns='http://www.w3.org/2000/svg' width='40' height='40' fill='currentColor' " +
                "className='bi bi-plus-circle-fill' viewBox='0 0 16 16'> " +
                "<path d='M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z'/> " +
                "</svg>"

            newTask.addEventListener("click", () => addTask(newTask))
        }
    }).catch(() => {
        alert('Oeps something went wrong')
    })

}

