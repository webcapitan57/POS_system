import {addGoogleMapsScript, addMapDivs} from "./locationTask";

const taskId = parseInt(document.querySelector('#TaskId').value);
const setUpId = parseInt(document.querySelector('#SetUpId').value);
const teacherId = parseInt(document.querySelector('#TeacherId').value);
const newTask = document.querySelector('#NewTask').value;
let saveTask = false;
let allPhotoQuestionsDiv = document.querySelector('#allPhotoQuestionsDiv');
const submitButton = document.querySelector('#saveTask');

window.addEventListener("load", getTask);
document.querySelector('#AddPhotoQuestion').addEventListener("click", function () {
    getPhotoQuestion();
});
window.onbeforeunload = function (e) {
    if (!saveTask) {
        return "onopgeslagen veranderingen worden misschien niet opgeslagen en lege data wordt verwijderd, weet je zeker dat je de pagina wil verwijderen?"
    }
}


submitButton.addEventListener("click", function () {
    const photoQuestionsList = allPhotoQuestionsDiv.querySelectorAll("#allPhotoQuestionsDiv > div")
    let photoQuestionsSend = []
    photoQuestionsList.forEach(function (currentValue) {
        photoQuestionsSend.push(compilePhotoQuestion(currentValue))
    })
    const title = document.querySelector("#taskTitle").value
    const info = document.querySelector("#taskInfo").value

    updateTask(title, info, photoQuestionsSend)
})

//disableSubmit();

function attachEventHandlers(element) {
    let allButtons = element.querySelectorAll("button")
    for (let button of allButtons) {
        if (button.id.includes("AddSideQuestion_")) {
            let photoQuestionId = button.id.replace(/^\D+/g, '');
            button.addEventListener('click', function () {
                getSideQuestion(photoQuestionId);
            });
            continue;
        }
        if (button.id.includes("AddOption_")) {
            let photoQuestionId = button.closest("div[id^=PhotoQuestion]")
                .id.replace(/^\D+/g, '');
            let sideQuestionId = button.id.replace(/^\D+/g, '');
            button.addEventListener('click', function () {
                let table = button.closest("table")

                let options = table.querySelectorAll("input[name='value']")

                if (options.length === 0) {
                    getOption(photoQuestionId, sideQuestionId)
                }
                getOption(photoQuestionId, sideQuestionId)

            });
            continue;
        }
        if (button.id.includes("RemovePhotoQuestion_")) {
            button.addEventListener('click', function () {
                removePhotoQuestion(button)
            });
            continue;
        }
        if (button.id.includes("RemoveSideQuestion_")) {
            button.addEventListener('click', function () {
                removeSideQuestion(button);
            });
        }
        if (button.id.includes("ClearOptions_")) {
            button.addEventListener('click', function () {
                removeOption(button);
            });
        }
    }
    /*  let allInputs = element.getElementsByTagName("input")
        for(let input in allInputs){
          if(input.name === "question" || input.name === "value" || input.name === "opdrachtOmschrijving"){
              inp.addEventListener('focusout',function() {
                  disableSubmit();
              }); 
          }
      }*/
}

/*Post function section*/
function getPhotoQuestion() {
    fetch('/api/Tasks/' + taskId + '/PhotoQuestions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            return response.json()
        }
    }).then(function (data) {
        showPhotoQuestion(data)
    })
}

function getSideQuestion(photoQuestionId) {
    fetch('/api/Tasks/' + taskId + '/PhotoQuestions/' + photoQuestionId + '/SideQuestions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            return response.json()
        }
    }).then(function (data) {
        showSideQuestion(photoQuestionId, data)
    })
}

function getOption(photoQuestionId, sideQuestionId) {
    fetch('/api/Tasks/' + taskId + '/PhotoQuestions/' + photoQuestionId + '/SideQuestions/' + sideQuestionId + '/options', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            return response.json()
        }
    }).then(function (data) {
        showOption(sideQuestionId, data)
    })
}

/*End Post function section*/

/*Get function section*/
function getTask() {
    fetch('/api/Tasks/' + taskId, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            return response.json()
        }
    }).then(function (data) {
        for (const question of data) {
            showPhotoQuestion(question)
        }
        // add Google Maps API if SetTask or allowLocations true for TeacherTask
        let task = data[0].task
        if (task.setUp != null) {
            addGoogleMapsScript()
            addMapDivs(data)
        } else if (task.teacher.setUp.allowLocations) {
            addGoogleMapsScript()
            addMapDivs(data)
        }
    })
}

/*End get function section*/

/*Delete function section*/
function removePhotoQuestion(element) {
    const photoQuestionId = element.id.replace(/^\D+/g, '');

    fetch('/api/Tasks/' + taskId + "/PhotoQuestions/" + photoQuestionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest("#PhotoQuestion_" + photoQuestionId).remove()
        }
    })
}

function removeSideQuestion(element) {
    const sideQuestionId = element.id.replace(/^\D+/g, '');
    const photoDiv = element.closest("div[id^=PhotoQuestion]")
    const photoQuestionId = photoDiv.id.replace(/^\D+/g, '')

    fetch('/api/Tasks/' + taskId + "/PhotoQuestions/" + photoQuestionId + "/SideQuestions/" + sideQuestionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest("#SideQuestion_" + sideQuestionId).remove()
        }
    })
}

function removeOption(element) {
    const sideQuestionId = element.id.replace(/^\D+/g, '');
    const photoDiv = element.closest("div[id^=PhotoQuestion]")
    const photoQuestionId = photoDiv.id.replace(/^\D+/g, '')

    fetch('/api/Tasks/' + taskId + "/PhotoQuestions/" + photoQuestionId + "/SideQuestions/" + sideQuestionId + "/Options", {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            let table = element.closest('table');
            for (let i = 0; i < table.rows.length - 2; i++) {
                table.deleteRow(i)
                i--
            }
            element.hidden = true;
        }
    })
}

/*End Delete function section*/

/*Put function section*/
function updateTask(title, info, photoQuestions) {
    saveTask = validateInput();
    if (saveTask) {
        let task;
        task = {setUpId: setUpId, teacherId: teacherId, title: title, info: info, questions: photoQuestions};

        fetch('/api/Tasks/Update/' + taskId, {
            mode: 'cors',
            method: 'PUT',
            body: JSON.stringify(task),
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }).then(function (response) {
            if (response.ok) {
                if (setUpId !== -1) {
                    window.location.href = "/SetUps/" + setUpId + "/edit"
                } else {
                    window.location.href = "/group/index"
                }

            } else {
                throw new Error();
            }
        }).catch(() => {
            alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
        })
    } else {
        document.querySelector("#superSpan").innerHTML = "er zijn verplichte velden niet ingevuld"
    }
}

/*End Put function section*/

/*Convert to string function section*/
function compilePhotoQuestion(photoQuestion) {
    const photoQuestionId = photoQuestion.id.replace(/^\D+/g, '')
    const photoQInfo = photoQuestion.querySelector(`[id="PhotoQuestionInfo_${photoQuestionId}"]`)

    let question = photoQInfo.querySelector("input[name='question']").value
    //needs to be removed after validation is implemented
    question = question || ""

    let tips = photoQInfo.querySelector("textarea[name='tips']").value
    tips = tips || ""

    const sideQuestionDiv = photoQuestion.querySelector(`[id="PhotoQuestionSQ_${photoQuestionId}"]`)
    let sideQuestions = []
    const sideQuestionList = sideQuestionDiv.querySelectorAll(`div[id*="SideQuestion_"]`)
    sideQuestionList.forEach(function (currentValue) {
        sideQuestions.push(compileSideQuestion(currentValue))
    })

    return {
        photoQuestionId: photoQuestionId,
        question: question,
        tips: tips,
        sideQuestions: sideQuestions
    };
}

function compileSideQuestion(sideQuestion) {
    const sideQuestionId = sideQuestion.id.replace(/^\D+/g, '')
    let question = sideQuestion.querySelector("input[name='question']").value
    question = question || ""
    let options = []
    const optionsTable = sideQuestion.querySelector("table")
    const optionsList = optionsTable.querySelectorAll("input[name='value']")
    optionsList.forEach(function (currentValue) {
        options.push({sideQuestionOptionId: currentValue.id.replace(/^\D+/g, ''), value: currentValue.value})
    })
    return {sideQuestionId: sideQuestionId, question: question, sideQuestionOptions: options};
}

/*End Convert to string function section*/

/*Showing html function section*/
function showPhotoQuestion(photoQuestion) {
    let photoQuestionElement = document.createElement("div")
    photoQuestionElement.id = `PhotoQuestion_${photoQuestion.photoQuestionId}`
    photoQuestionElement.innerHTML = `
       <div id="PhotoQuestionInfo_${photoQuestion.photoQuestionId}" class="photo-vraag-div">
       <div class="create-task-head">
            <input name="question" placeholder="[Geef hier een fotovraag]" class="input-pw intput-border" value="${photoQuestion.question ? photoQuestion.question : ""}" id="photoQuestionQuestion_${photoQuestion.photoQuestionId}">
            <button id="RemovePhotoQuestion_${photoQuestion.photoQuestionId}" class="btn btn-right btn-x shadow-none"
            data-toggle="tooltip" data-placement="bottom" title="Verwijder fotovraag">
            <img src="/resources/Icons/cross.svg" alt="Verwijder fotovraag">
            </button>
            </div>
            <span id="photoQuestionSpan_${photoQuestion.photoQuestionId}"></span>
           
            <hr>
            <label>Tips bij foto</label>
            <textarea name="tips" placeholder="[Tips]" class="input-fw">${photoQuestion.tips ? photoQuestion.tips : ""}</textarea>
            <br>
            <div id="PhotoQuestionMap_${photoQuestion.photoQuestionId}"></div>
            <br>
            
            <button id="AddSideQuestion_${photoQuestion.photoQuestionId}" class="btn btn-secondary btn-right">
            <img src="/resources/Icons/plus-circle-white.svg" alt="Voeg deelvraag toe">
            Deelvraag toevoegen
            </button>
            <br>
        </div>   
        <div id="PhotoQuestionSQ_${photoQuestion.photoQuestionId}">    
        </div>      
    `

    allPhotoQuestionsDiv.insertBefore(photoQuestionElement, allPhotoQuestionsDiv.childNodes[0])

    attachEventHandlers(photoQuestionElement);
    photoQuestion.sideQuestions = photoQuestion.sideQuestions || []
    for (const sideQuestion of photoQuestion.sideQuestions) {
        showSideQuestion(photoQuestion.photoQuestionId, sideQuestion)
    }
}

function showSideQuestion(photoQuestionId, sideQuestion) {
    const sideQuestionDiv = document.querySelector(`[id="PhotoQuestionSQ_${photoQuestionId}"]`)
    let sideQuestionElements = document.createElement("div")
    sideQuestionElements.id = `SideQuestion_${sideQuestion.sideQuestionId}`
    sideQuestionElements.className = "side-vraag-div"
    sideQuestionElements.innerHTML = `
    <div class="create-task-head">
    <input name="question" placeholder="[Geef hier een deelvraag]" class="input-pw intput-border" value="${sideQuestion.question ? sideQuestion.question : ''}" id="sideQuestionQuestion_${sideQuestion.sideQuestionId}">
    <button id="RemoveSideQuestion_${sideQuestion.sideQuestionId}" class="btn btn-right shadow-none btn-x"
    data-toggle="tooltip" data-placement="bottom" title="Verwijder deelvraag">
    <img src="/resources/Icons/cross.svg" alt="Verwijder deelvraag">
    </button> 
    </div> 
    <span id="sideQuestionSpan_${sideQuestion.sideQuestionId}"></span>
    <hr>
    <table id="SideQuestionOptions_${sideQuestion.sideQuestionId}">
        <tr>
        <td><button id="AddOption_${sideQuestion.sideQuestionId}" class="btn shadow-none">
        <img src="/resources/Icons/plus-square-dotted.svg" alt="Extra optie">
        Extra optie</button>
        </td>
        </tr>
        <tr>
        <td>
        <button id="ClearOptions_${sideQuestion.sideQuestionId}" hidden="true" class="btn shadow-none">
        <img src="/resources/Icons/trash-red.svg" alt="Verwijder extra optie">
        verwijder opties
        </button>
        </td>
        <span id="optionTableSpan_${sideQuestion.sideQuestionId}"></span>
        </tr>
    </table>  
    `

    sideQuestionDiv.insertBefore(sideQuestionElements, sideQuestionDiv.childNodes[0])

    attachEventHandlers(sideQuestionElements);
    sideQuestion.sideQuestionOptions = sideQuestion.sideQuestionOptions || []
    for (const option of sideQuestion.sideQuestionOptions) {
        showOption(sideQuestion.sideQuestionId, option)
    }
}

function showOption(sideQuestionId, option) {
    const sideQuestion = document.querySelector(`div[id="SideQuestion_${sideQuestionId}"]`)
    let optionsTable = sideQuestion.querySelector("table")
    optionsTable.querySelector("button[id^='ClearOptions_']").hidden = false

    let sideQuestionElements = document.createElement("tr")
    sideQuestionElements.innerHTML = `
    <td>
    <input id="option_${option.sideQuestionOptionId}" name="value" placeholder="[Optie]" value="${option.value ? option.value : ''}">
    <span id="optionSpan_${option.sideQuestionOptionId}"></span>
    </td>
    `
    optionsTable.insertBefore(sideQuestionElements, optionsTable.children[optionsTable.rows.length - 2])
    attachEventHandlers(sideQuestionElements);
}

/*End Showing html functions section*/

/*niche function section*/
function validateInput() {
    let allClear = true
    const title = document.querySelector("#taskTitle").value
    let titleSpan = document.querySelector("#titleSpan")
    let photoQuestions = document.querySelectorAll(`[id *= "photoQuestionQuestion_"]`)
    let sideQuestions = document.querySelectorAll(`[id *= "sideQuestionQuestion_"]`)
    /*let sideQuestionTables = document.querySelectorAll(`[id *= "SideQuestionOptions_"]`)*/
    let options = document.querySelectorAll(`[id *= "option_"]`)
    if (title.trim() === "") {
        titleSpan.innerHTML = "Voer aub een titel in."
        allClear = false
    } else if (titleSpan.innerHTML !== "") {
        titleSpan.innerHTML = ""
    }
    for (let photoQuestion of photoQuestions) {
        let tempId = photoQuestion.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#photoQuestionSpan_${tempId}`)

        if (photoQuestion.value.trim() === "") {
            tempSpan.innerHTML = "Geef aub een waarde aan de foto vraag"
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    for (let sideQuestion of sideQuestions) {
        let tempId = sideQuestion.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#sideQuestionSpan_${tempId}`)

        if (sideQuestion.value.trim() === "") {
            tempSpan.innerHTML = "Geef aub een waarde aan de zij vraag"
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    /* for(let sideQuestionTable in sideQuestionTables){
         let tempId = sideQuestionTable.id.replace(/^\D+/g, '');
         let tempSpan = document.querySelector(`#optionTableSpan_${tempId}`)
 
         if(sideQuestionTable.rows.length === 3){
             tempSpan.innerHTML = "Een multiple choice vraag kan niet maar 1 optie hebben"
             allClear = false
         }else if(tempSpan.innerHTML !== ""){
             tempSpan.innerHTML = ""
         }
     }*/
    for (let option of options) {
        let tempId = option.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#optionSpan_${tempId}`)

        if (option.value.trim() === "") {
            tempSpan.innerHTML = "Dit veld heeft een waarde nodig."
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    return allClear
}

/*End niche function section*/

