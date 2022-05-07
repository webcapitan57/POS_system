const teacherTable = document.querySelector('#teacherTable')
window.addEventListener('load', getTeachers)

function getTeachers() {
    fetch('/api/teachers', {
        method: "GET",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                for (const teacher of data) {
                    showTeacher(teacher)
                }
            })
        } else {
            throw new Error()
        }
    }).catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
}

function showTeacher(teacher) {
    let teacherElement =
        document.createElement("tr");
    teacherElement.innerHTML = `
         <th class="align-items-center" scope="row">${teacher.userId}</th>
         <td class="align-items-center">${teacher.userAccount.userName.split("#")[0]}</td>
         <td><a id="removeTeacher_${teacher.userId}" class="removebtn">
                <img src="/resources/Icons/trash-red.svg" alt="Trash"></a>
         </td>`
    teacherTable.appendChild(teacherElement);
    addEventHandlers()
}

function addEventHandlers() {
    let removeTeacherButtons = document.querySelectorAll(`[id*="removeTeacher_"]`)
    for (let button of removeTeacherButtons) {
        button.addEventListener('click', () => {
            deleteTeacher(button);
        })
    }
}

function deleteTeacher(element) {
    const teacherId = parseInt(element.id.split("_")[1]);

    fetch('/api/teachers/' + teacherId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest("tr").remove();
        }
    })
}
