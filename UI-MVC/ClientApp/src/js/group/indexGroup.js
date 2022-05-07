const setBg = () => {

    for (const element of document.getElementsByClassName("color")) {
        let randomColor = Math.floor(Math.random() * 16777215).toString(16);
        element.style.backgroundColor = "#80" + randomColor
    }

}


window.onload = setBg;


let taskButtons = document.querySelectorAll(`[id *= "editTaskButton_"]`)
for (let taskButton of taskButtons) {
    taskButton.addEventListener("click", () => {
        let tempId = taskButton.id.replace(/^\D+/g, '');
        window.location.href = "/createTasks/EditTask/" + tempId
    })
}

const startSessionButtons = document.querySelectorAll(`#startsession`)
for (let button of startSessionButtons) {
    button.addEventListener('click', (e) => {
        getGroup(e.currentTarget.href.split('=')[1])
    })
}

function getGroup(groupCode) {
    let group

    return fetch('/api/Groups/' + groupCode, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            response.json().then(group => {
            if (group.teacher.setUp.archived) {
                window.stop()
                alert('De setup is gearchiveerd en hierdoor kunt u de groep niet starten, gelieve een admin te contacteren')
            }
        })
            return false
        } else {
            throw new Error();
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
}