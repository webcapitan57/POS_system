/* buttons */
const addGroupProfileQuestionButton = document.querySelector('#addGroupProfileQuestion')
const addGroupMcProfileQuestionButton = document.querySelector('#addGroupMcProfileQuestion')
const addStudentProfileQuestionButton = document.querySelector('#addStudentProfileQuestion')
const addStudentMcProfileQuestionButton = document.querySelector('#addStudentMcProfileQuestion')
const createSetupButton = document.querySelector("#saveSetup")
/* end of buttons */

/* addEventHandlers for buttons */
addGroupProfileQuestionButton.addEventListener('click', () => addGroupProfileQuestion(setUpId));
addGroupMcProfileQuestionButton.addEventListener('click', () => addGroupMcProfileQuestion(setUpId))
addStudentProfileQuestionButton.addEventListener('click', () => addStudentProfileQuestion(setUpId));
addStudentMcProfileQuestionButton.addEventListener('click', () => addStudentMcProfileQuestion(setUpId))
/* end of addEventHandlers for buttons */


/* logo */
const LogoInput = document.querySelector('#logo')
/* end of logo */

/* addEventHandlers logo */
LogoInput.addEventListener('change', (event) => {
    ChangeSetUpLogoHTML(event.target)
})
/* end of addEventHandlers logo */


/* Event for createSetupButton */
createSetupButton.addEventListener('click', (e) => {
    return saveData(0)
})

export async function saveData(validate = 0, taskId = 0) {
    if (validate === 0) {
        saved = validateInput()
    } else {
        saved = true
    }
    if (saved) {
        setUpName = document.querySelector("#setUpName").value
        setUpGeneralText = document.querySelector("#setUpGeneralText").value
        createTeacherTask = document.querySelector("#CreateTeacherTask").checked
        allowLocations = document.querySelector('#AllowLocations').checked
        primColor = document.querySelector("#primColor").value
        secColor = document.querySelector("#secColor").value

        updateGroupProfileOptions()
            .then(() => updateGroupProfileQuestions()
                .then(() => updateStudentProfileOptions()
                    .then(() => updateStudentProfileQuestions()
                        .then(() => updateSetUp(setUpName, setUpGeneralText, createTeacherTask, allowLocations, primColor, secColor)
                            .then(() => {
                                if (validate === 0) {
                                    window.location.href = redirectUrl
                                } else if (validate === 1) {
                                    window.location.href = "/createTasks/CreateSetTask/" + setUpId
                                } else {
                                    window.location.href = "/createTasks/EditTask/" + taskId
                                }
                            })))))
    } else {
        document.getElementById("saveSetupSpan").innerHTML = "er zijn verplichte velden niet ingevuld"
    }
}

/* end of Event for createSetupButton */


/* profileQuestionsDivs */
const groupProfileQuestionsDiv = document.querySelector('#groupProfileQuestionsDiv')
const studentProfileQuestionsDiv = document.querySelector('#studentProfileQuestionsDiv')
/* end of profileQuestionsDivs */


/* setup elements */
let setUpDiv = document.querySelector(`[id *= "setUpDiv"]`)
let setUpId = parseInt(setUpDiv.id.split("_")[1])
let setUpName = document.querySelector("#setUpName").value
let setUpGeneralText = document.querySelector("#setUpGeneralText").value
let createTeacherTask = document.querySelector("#CreateTeacherTask").checked
let allowLocations = document.querySelector("#AllowLocations").checked
let primColor = document.querySelector("#primColor").value
let secColor = document.querySelector("#secColor").value
let setUpLogo = document.querySelector('#setUpLogo')
/* end of setup elements */


/* redirect url */
let redirectUrl = ""
/* end of redirect url */


/* on before unload */
let saved = false;

window.onbeforeunload = function (e) {
    if (!saved) {
        return "Onopgeslagen veranderingen worden misschien niet opgeslagen en lege data wordt verwijderd, weet je zeker dat je de pagina wil verwijderen?"
    }
}
/* end of on before unload */


/* addEventHandlers */

/* GroupProfileQuestions */
function addEventHandlersGroupProfileQuestion(id) {
    let removeGroupProfileQuestionButton = document.querySelector(`#removeGroupProfileQuestion_${id}`)
    removeGroupProfileQuestionButton.addEventListener('click', (e) => removeGroupProfileQuestion(e.target))
}

function addEventHandlersGroupMcProfileQuestion(id) {
    let addOptionButton = document.querySelector(`#addGroupProfileOption_${id}`)

    addOptionButton.addEventListener('click', (e) => addGroupProfileOption(e.target))

}

function addEventHandlersGroupProfileOption(optionId) {
    const groupOption = document.querySelector(`#groupOption_${optionId}`)
    const removeButton = groupOption.firstElementChild.querySelector('button')
    removeButton.addEventListener('click', e => removeGroupProfileOption(e.currentTarget))
}

function addEventHandlersGroupProfileOptions(element) {
    const groupOptions = element.querySelectorAll(`[id *= "#groupOption_"]`)
    for (let option of groupOptions) {
        const removeButton = option.firstElementChild.querySelector('button')
        removeButton.addEventListener('click', e => removeGroupProfileOption(e.currentTarget))
    }
}

/* end of GroupProfileQuestions */

/* StudentProfileQuestions */
function addEventHandlersStudentProfileQuestion(id) {
    const removeStudentProfileQuestionButton = document.querySelector(`#removeStudentProfileQuestion_${id}`)
    removeStudentProfileQuestionButton.addEventListener('click', (e) => removeStudentProfileQuestion(e.target))
}

function addEventHandlersStudentMcProfileQuestion(id) {
    let addOptionButton = document.querySelector(`#addStudentProfileOption_${id}`)
    addOptionButton.addEventListener('click', (e) => addStudentProfileOption(e.target))
}

function addEventHandlersStudentProfileOption(optionId) {
    const studentOption = document.querySelector(`#studentOption_${optionId}`)
    const removeButton = studentOption.firstElementChild.querySelector('button')
    removeButton.addEventListener('click', e => removeStudentProfileOption(e.currentTarget))
}

function addEventHandlersStudentProfileOptions(element) {
    const studentOptions = element.querySelectorAll(`[id *= "studentOption_"]`)
    for (let option of studentOptions) {
        const removeButton = option.firstElementChild.querySelector('button')
        removeButton.addEventListener('click', e => removeStudentProfileOption(e.currentTarget))
    }
}

/* end of StudentProfileQuestions */
/* end of addEventHandlers */


/* setup PUT function */
async function updateSetUp(setUpName, setUpGeneralText, createTeacherTask, allowLocations, primColor, secColor) {
    saved = true
    let setUp = {
        name: setUpName,
        generalText: setUpGeneralText,
        createTasks: Boolean(createTeacherTask),
        allowLocations: Boolean(allowLocations),
        primColor: primColor,
        secColor: secColor
    }

    return fetch('/api/setups/' + setUpId, {
        method: 'PUT',
        body: JSON.stringify(setUp),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            redirectUrl = response.url
        } else {
            throw new Error()
        }
    }).catch((e) => {
        alert('Oops, something went wrong! Our slaves are working hard to find a solution.')
    })
}

/* end of setup PUT function */


/* update functions */

/* update functions for groupProfileQuestions */
async function updateGroupProfileQuestions() {
    const groupProfileQuestions = document.querySelectorAll(`[id *="groupProfileQuestion_"]`)
    for (let groupProfileQuestion of groupProfileQuestions) {
        let questionId = parseInt(groupProfileQuestion.id.split('_')[1])
        let question = document.querySelector(`#Question_${questionId}`).value
        let description = document.querySelector(`#Description_${questionId}`).value
        let isRequired = document.querySelector(`#IsRequired_${questionId}`).value
        await updateGroupProfileQuestion(questionId, question, description, isRequired)
    }
}

async function updateGroupProfileQuestion(questionId, question, description, isRequired) {
    let groupProfileQuestion = {question: question, description: description, isrequired: Boolean(isRequired)}

    return fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + questionId, {
        method: 'PUT',
        body: JSON.stringify(groupProfileQuestion),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (!response.ok) {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'))
}

async function updateGroupProfileOptions() {
    let optionsElements = document.querySelectorAll(`[id *= "groupOptions_"]`)
    for (let optionsElement of optionsElements) {
        let profileQuestionId = parseInt(optionsElement.id.split('_')[1])
        for (let optionElement of optionsElement.childNodes) {
            if (optionElement.nodeName === 'DIV') {
                let profileOptionId = parseInt(optionElement.id.split('_')[1])
                let value = document.querySelector(`#Value_${profileOptionId}`).value
                await updateGroupProfileOption(profileQuestionId, profileOptionId, value)
            }
        }
    }
}

async function updateGroupProfileOption(profileQuestionId, profileOptionId, value) {
    let option = {value: value}
    return fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + profileQuestionId + '/groupoptions/' + profileOptionId, {
        method: 'PUT',
        body: JSON.stringify(option),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (!response.ok) {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'))
}

/* end of update functions for groupProfileQuestions */

/* update functions for groupProfileQuestions */
async function updateStudentProfileQuestions() {
    const studentProfileQuestions = document.querySelectorAll(`[id *="studentProfileQuestion_"]`)
    for (let studentProfileQuestion of studentProfileQuestions) {
        let questionId = parseInt(studentProfileQuestion.id.split('_')[1])
        let question = document.querySelector(`#Question_${questionId}`).value
        let description = document.querySelector(`#Description_${questionId}`).value
        let isRequired = document.querySelector(`#IsRequired_${questionId}`).value
        await updateStudentProfileQuestion(questionId, question, description, isRequired)
    }
}

async function updateStudentProfileQuestion(questionId, question, description, isRequired) {
    let studentProfileQuestion = {question: question, description: description, isrequired: Boolean(isRequired)}

    return fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + questionId, {
        method: 'PUT',
        body: JSON.stringify(studentProfileQuestion),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (!response.ok) {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

async function updateStudentProfileOptions() {
    let optionsElements = document.querySelectorAll(`[id *= "studentOptions_"]`)
    for (let optionsElement of optionsElements) {
        let profileQuestionId = parseInt(optionsElement.id.split('_')[1])
        for (let optionElement of optionsElement.childNodes) {
            if (optionElement.nodeName === 'DIV') {
                let profileOptionId = parseInt(optionElement.id.split('_')[1])
                let value = document.querySelector(`#Value_${profileOptionId}`).value
                await updateStudentProfileOption(profileQuestionId, profileOptionId, value)
            }
        }
    }
}

async function updateStudentProfileOption(profileQuestionId, profileOptionId, value) {
    let option = {value: value}
    return fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + profileQuestionId + '/studentoptions/' + profileOptionId, {
        method: 'PUT',
        body: JSON.stringify(option),
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (!response.ok) {
            throw new Error()
        }
    }).catch((e) => {
        alert("Oops, ProfileQuestion: " + e.name + ": " + e.message)
    })

}

/* end of update functions */


/* POST functions */

/* POST functions for groupProfileQuestions*/
function addGroupProfileQuestion() {
    fetch('/api/setups/' + setUpId + '/groupprofilequestions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                loadNewGroupProfileQuestion(data)
            })
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'))
}

function addGroupMcProfileQuestion() {
    fetch('/api/setups/' + setUpId + '/groupprofilequestions/mc', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                loadNewGroupMcProfileQuestion(data)
            })
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function addGroupProfileOption(element) {
    const profileQuestionId = parseInt(element.id.split('_')[1])

    fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + profileQuestionId + '/groupoptions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(option => {
                loadNewGroupOption(option, profileQuestionId)
            })
        } else
            throw new Error
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

/* end of POST functions for groupProfileQuestions*/

/* POST functions for studentProfileQuestions*/
function addStudentProfileQuestion() {
    fetch('/api/setups/' + setUpId + '/studentprofilequestions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                loadNewStudentProfileQuestion(data)
            })
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

function addStudentMcProfileQuestion() {
    fetch('/api/setups/' + setUpId + '/studentprofilequestions/mc', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(data => {
                loadNewStudentMcProfileQuestion(data)
            })
        } else {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function addStudentProfileOption(element) {

    let profileQuestionId = parseInt(element.parentElement.id.split('_')[1])

    fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + profileQuestionId + '/studentoptions', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            response.json().then(option => {
                loadNewStudentOption(option, profileQuestionId)
            })
        } else
            throw new Error
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

/* end of POST functions for studentProfileQuestions*/
/* end of POST functions */


/* Load functions */

/* Load functions for groupProfileQuestions */
function loadNewGroupProfileQuestion(question) {
    let groupProfileQuestionElement = document.createElement('div');
    groupProfileQuestionElement.id = `"groupProfileQuestion_${question.profileQuestionId}"`;

    groupProfileQuestionElement.innerHTML = `
    <br/><h4>Open Profiel Vraag
    <button class="btn" id="removeGroupProfileQuestion_${question.profileQuestionId}">
    <img src="/resources/Icons/cross.svg" alt="verwijder"  >
    </button></h4>

    <div class="form-group">
        <label>Vraag:</label>
        <input id="Question_${question.profileQuestionId}" type="text" placeholder="Geef hier je open vraag" class="form-control"/>
        <span id="QuestionSpan_${question.profileQuestionId}"></span>
    </div>
    
    <div class="form-group">
        <label>Categorie:</label>
        <input id="Description_${question.profileQuestionId}" type="text" placeholder="Kernwoord (max 20 karakters)" class="form-control"/>
        <span id="DescriptionSpan_${question.profileQuestionId}"></span>
    </div>

    <div class="form-group">
        <label>Verplicht</label>
        <input id="IsRequired_${question.profileQuestionId}" type="checkbox"/>
    </div>
`
    groupProfileQuestionsDiv.insertBefore(groupProfileQuestionElement, groupProfileQuestionsDiv.childNodes[0])
    addEventHandlersGroupProfileQuestion(question.profileQuestionId)
}

function loadNewGroupMcProfileQuestion(question) {
    fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + question.profileQuestionId + '/groupoptions', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                response.json().then(options => {
                    let optionElements = document.createElement('div')
                    optionElements.id = `groupOptions_${question.profileQuestionId}`

                    for (let i = 0; i < options.length; i++) {
                        optionElements.innerHTML += `<div id="groupOption_${options[i].profileOptionId}">
                        <div class="setup-edit-options">
                        <label>Optie:  </label>
                        <input id="Value_${options[i].profileOptionId}" type="text" placeholder="Optie" class="form-control"/>
                        
                        <button class="btn" id="removeGroupOption_${question.profileQuestionId}_${options[i].profileOptionId}">
                        <img src="/resources/Icons/cross.svg" alt="verwijder" class="sm-cross">
                        </button><span id="OptionValueSpan_${options[i].profileOptionId}"></span></div></div>`
                    }

                    let groupProfileQuestionElement = document.createElement('div');
                    groupProfileQuestionElement.id = `"groupProfileQuestion_${question.profileQuestionId}"`;

                    groupProfileQuestionElement.innerHTML = `
                    <br/><h4>MC Profiel Vraag
                    <button class="btn" id="removeGroupProfileQuestion_${question.profileQuestionId}">
                    <img src="/resources/Icons/cross.svg" alt="verwijder">
                    </button></h4>

                    <div class="form-group">
                        <label>Vraag:</label>
                        <input id="Question_${question.profileQuestionId}" type="text" placeholder="Geef hier je meerkeuzevraag" class="form-control"/>
                         <span id="QuestionSpan_${question.profileQuestionId}"></span>
                    </div>
                    
                    <div class="form-group">
                        <label>Categorie:</label>
                        <input id="Description_${question.profileQuestionId}" type="text" placeholder="Kernwoord (max 20 karakters)" class="form-control"/>
                        <span id="DescriptionSpan_${question.profileQuestionId}"></span>
                    </div>

                    <div class="form-group">
                        <label>Verplicht</label>
                        <input id="IsRequired_${question.profileQuestionId}" type="checkbox"/>
                    </div>
                    
                    <button class="btn btn-light btn-options" id="addGroupProfileOption_${question.profileQuestionId}">
                    <img src="/resources/Icons/plus-square-dotted.svg" alt="+ extra optie">
                    Voeg extra optie toe
                    </button>
                    `
                    groupProfileQuestionsDiv.insertBefore(groupProfileQuestionElement, groupProfileQuestionsDiv.childNodes[0])

                    groupProfileQuestionElement.appendChild(optionElements)

                    addEventHandlersGroupProfileQuestion(question.profileQuestionId)
                    addEventHandlersGroupMcProfileQuestion(question.profileQuestionId)
                    addEventHandlersGroupProfileOptions(optionElements)
                })
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

function loadNewGroupOption(option, profileQuestionId) {
    const groupOptionsDiv = document.querySelector(`#groupOptions_${profileQuestionId}`)

    let groupOptionElement = document.createElement('div');
    groupOptionElement.id = `groupOption_${option.profileOptionId}`
    groupOptionElement.innerHTML =
        `<div class="setup-edit-options">
             <label>Optie:  </label>
             <input id="Value_${option.profileOptionId}" type="text" placeholder="Optie" class="form-control"/>
             
             <button class="btn" id="removeGroupOption_${profileQuestionId}_${option.profileOptionId}">
             <img src="/resources/Icons/cross.svg" alt="verwijder" class="sm-cross"> 
             </button><span id="OptionValueSpan_${option.profileOptionId}"></span>
        </div>`

    groupOptionsDiv.appendChild(groupOptionElement)
    addEventHandlersGroupProfileOption(option.profileOptionId)
}

/* end of Load functions for groupProfileQuestions */

/* Load functions for studentProfileQuestions */
function loadNewStudentProfileQuestion(question) {
    let studentProfileQuestionElement = document.createElement('div');
    studentProfileQuestionElement.id = `studentProfileQuestion_${question.profileQuestionId}`;

    studentProfileQuestionElement.innerHTML = `<br/><h4>Open Profiel Vraag
    <button class="btn" id="removeStudentProfileQuestion_${question.profileQuestionId}">
        <img src="/resources/Icons/cross.svg" alt="verwijder">
    </button></h4>

    <div class="form-group">
        <label>Vraag:</label>
        <input id="Question_${question.profileQuestionId}" type="text" placeholder="Geef hier je open vraag" class="form-control"/>
        <span id="QuestionSpan_${question.profileQuestionId}"></span>
    </div>
    
    <div class="form-group">
        <label>Categorie:</label>
        <input id="Description_${question.profileQuestionId}" type="text" placeholder="Kernwoord (max 20 karakters)" class="form-control"/>
        <span id="DescriptionSpan_${question.profileQuestionId}"></span>
    </div>

    <div class="form-group">
        <label>Verplicht</label>
        <input id="IsRequired_${question.profileQuestionId}" type="checkbox"/>
    </div>`
    studentProfileQuestionsDiv.insertBefore(studentProfileQuestionElement, studentProfileQuestionsDiv.childNodes[0])
    addEventHandlersStudentProfileQuestion(question.profileQuestionId)
}

function loadNewStudentMcProfileQuestion(question) {
    fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + question.profileQuestionId + '/studentoptions', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                response.json().then(options => {
                    let optionElements = document.createElement('div')
                    optionElements.id = `studentOptions_${question.profileQuestionId}`

                    for (let i = 0; i < options.length; i++) {
                        optionElements.innerHTML += `<div id="studentOption_${options[i].profileOptionId}">
                        <div class="setup-edit-options">
                        <label>Optie:  </label>
                        <input id="Value_${options[i].profileOptionId}" type="text" placeholder="Optie" class="form-control"/>
                      
                        <button class="btn" id="removeStudentOption_${question.profileQuestionId}_${options[i].profileOptionId}">
                        <img src="/resources/Icons/cross.svg" alt="verwijder" class="sm-cross">
                        </button>
                        <span id="OptionValueSpan_${options[i].profileOptionId}"></span></div></div>`
                    }

                    let studentProfileQuestionElement = document.createElement('div');
                    studentProfileQuestionElement.id = `"studentProfileQuestion_${question.profileQuestionId}"`;

                    studentProfileQuestionElement.innerHTML = `
                    <br/><h4>Multiple Choice Profiel Vraag
                    <button class="btn" id="removeStudentProfileQuestion_${question.profileQuestionId}">
                    <img src="/resources/Icons/cross.svg" alt="verwijder">
                    </button></h4>

                    <div class="form-group">
                        <label>Vraag:</label>
                        <input id="Question_${question.profileQuestionId}" type="text" placeholder="Geef hier je meerkeuzevraag" class="form-control"/>
                        <span id="QuestionSpan_${question.profileQuestionId}"></span>
                    </div>
    
                    <div class="form-group">
                        <label>Categorie:</label>
                        <input id="Description_${question.profileQuestionId}" type="text" placeholder="Kernwoord (max 20 karakters)" class="form-control"/>
                        <span id="DescriptionSpan_${question.profileQuestionId}"></span>
                    </div>

                    <div class="form-group">
                        <label>Verplicht</label>
                        <input id="IsRequired_${question.profileQuestionId}" type="checkbox"/>
                    </div>
                    
                    <button class="btn btn-light btn-options" id="addStudentProfileOption_${question.profileQuestionId}">
                    <img src="/resources/Icons/plus-square-dotted.svg" alt="+ extra optie">
                     Voeg extra optie toe
                    </button>`
                    studentProfileQuestionsDiv.insertBefore(studentProfileQuestionElement, studentProfileQuestionsDiv.childNodes[0])

                    studentProfileQuestionElement.appendChild(optionElements)

                    addEventHandlersStudentProfileQuestion(question.profileQuestionId)
                    addEventHandlersStudentMcProfileQuestion(question.profileQuestionId)
                    addEventHandlersStudentProfileOptions(optionElements)
                })
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

function loadNewStudentOption(option, profileQuestionId) {
    const studentOptionsDiv = document.querySelector(`#studentOptions_${profileQuestionId}`)

    let studentOptionElement = document.createElement('div');
    studentOptionElement.id = `studentOption_${option.profileOptionId}`
    studentOptionElement.innerHTML =
        `<div class="setup-edit-options">
         <label>Optie:</label>
         <input id="Value_${option.profileOptionId}" type="text" placeholder="Optie" class="form-control"/>
         <button class="btn" id="removeStudentOption_${profileQuestionId}_${option.profileOptionId}">
            <img src="/resources/Icons/cross.svg" alt="verwijder" class="sm-cross">
         </button> 
         <span id="OptionValueSpan_${option.profileOptionId}"></span>
         </div>`

    studentOptionsDiv.appendChild(studentOptionElement)
    addEventHandlersStudentProfileOption(option.profileOptionId)
}

/* end of Load functions for studentProfileQuestions */
/* end of Load functions */


/* DELETE functions */

/* DELETE functions for groupProfileQuestions*/
export function removeGroupProfileQuestion(element) {
    const profileQuestionId = parseInt(element.parentElement.id.split("_")[1])

    fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + profileQuestionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                element.closest('div').remove()
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function removeGroupProfileOption(element) {
    const id = element.id.split('_')
    const profileQuestionId = parseInt(id[1])
    const profileOptionId = parseInt(id[2])

    let studentprofileOptionsDiv = document.querySelector(`#groupOptions_${profileQuestionId}`)

    //Check if the McQuestion of which the user wants to delete an option has more than 2 options
    //Because a McQuestion with only 1 option wouldn't make any sense
    if (studentprofileOptionsDiv.children.length <= 2) {
        return;
    }

    fetch('/api/setups/' + setUpId + '/groupprofilequestions/' + profileQuestionId + '/groupoptions/' + profileOptionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest('div').parentElement.remove()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

/* end of DELETE functions for groupProfileQuestions*/

/* DELETE functions for studentProfileQuestions*/
export function removeStudentProfileQuestion(element) {
    const profileQuestionId = parseInt(element.parentElement.id.split("_")[1])

    fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + profileQuestionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
            if (response.ok) {
                element.closest('div').remove()
            } else {
                throw new Error()
            }
        }
    ).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

export function removeStudentProfileOption(element) {

    const id = element.id.split('_')
    const profileQuestionId = parseInt(id[1])
    const profileOptionId = parseInt(id[2])

    let studentprofileOptionsDiv = document.querySelector(`#studentOptions_${profileQuestionId}`)

    //Check if the McQuestion of which the user wants to delete an option has more than 2 options
    //Because a McQuestion with only 1 option wouldn't make any sense
    if (studentprofileOptionsDiv.children.length <= 2) {
        return;
    }


    fetch('/api/setups/' + setUpId + '/studentprofilequestions/' + profileQuestionId + '/studentoptions/' + profileOptionId, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then((response) => {
        if (response.ok) {
            element.closest('div').parentElement.remove()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

/* end of DELETE functions for studentProfileQuestions*/
/* end of DELETE functions */


/* setupLogo functions */

/* Upload the chosen ProfilePicture if it has a value */
function UploadSetUpLogo(e) {
    if (e.files.length === 0) {
        return;
    }

    let formdata = new FormData()
    formdata.append('Image', e.files[0])


    fetch('/api/Photos/setuplogo/' + setUpId, {
        method: 'POST',
        body: formdata,
        headers:
            {
                'Accept': 'application/json'
            }

    }).then(function (response) {
        if (!response.ok) {
            throw new Error()
        }
    }).catch(() => alert('Oops, something went wrong! Our slaves are working hard to find a solution.'));
}

function ChangeSetUpLogoHTML(e) {
    let source = URL.createObjectURL(e.files[0])
    setUpLogo.setAttribute('src', source)
    UploadSetUpLogo(e);
}

/* end of setupLogo functions */

/* validation*/
export function validateInput() {
    let allClear = true
    const title = document.querySelector("#setUpName").value
    let titleSpan = document.querySelector("#setUpNameSpan")
    let questions = document.querySelectorAll(`[id ^= "Question_"]`)
    let descriptions = document.querySelectorAll(`[id *= "Description_"]`)
    let options = document.querySelectorAll(`[id *= "Value_"]`)
    if (title.trim() === "") {
        titleSpan.innerHTML = "Voer aub een setup naam in"
        allClear = false
    } else if (titleSpan.innerHTML !== "") {
        titleSpan.innerHTML = ""
    }
    for (let question of questions) {
        let tempId = question.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#QuestionSpan_${tempId}`)

        if (question.value.trim() === "") {
            tempSpan.innerHTML = "Dit veld is verplicht"
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    for (let description of descriptions) {
        let tempId = description.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#DescriptionSpan_${tempId}`)

        if (description.value.trim() === "") {
            tempSpan.innerHTML = "Dit veld is verplicht"
            allClear = false
        } else if (description.value.length > 20) {
            tempSpan.innerHTML = "Gelieve minder dan 20 karakters te gebruiken"
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    
    for (let option of options) {
        let tempId = option.id.replace(/^\D+/g, '');
        let tempSpan = document.querySelector(`#OptionValueSpan_${tempId}`)

        if (option.value.trim() === "") {
            tempSpan.innerHTML = "Dit veld is verplicht"
            allClear = false
        } else if (tempSpan.innerHTML !== "") {
            tempSpan.innerHTML = ""
        }
    }
    return allClear
}

/* end of validation*/