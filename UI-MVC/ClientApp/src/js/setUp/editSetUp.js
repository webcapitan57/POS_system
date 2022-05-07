import {
    removeGroupProfileQuestion,
    addGroupProfileOption,
    removeStudentProfileQuestion,
    addStudentProfileOption,
    removeStudentProfileOption, 
    removeGroupProfileOption,
    saveData
} from "./createSetUp";

/* Call addEventHandlers for GroupProfileQuestions */
addEventHandlersGroupProfileQuestions()
addEventHandlersGroupMcProfileQuestions()
addEventHandlersGroupProfileOptions()
/* End of Call addEventHandlers for GroupProfileQuestions */

/* Call addEventHandlers for StudentProfileQuestions */
addEventHandlersStudentProfileQuestions()
addEventHandlersStudentMcProfileQuestions()
addEventHandlersStudentProfileOptions()
/* End of Call addEventHandlers for StudentProfileQuestions */

/* addEventHandlers functions for GroupProfileQuestions */
function addEventHandlersGroupProfileQuestions() {
    let removeGroupProfileQuestionButtons = document.querySelectorAll(`[id *= "removeGroupProfileQuestion_"]`)
    for (let button of removeGroupProfileQuestionButtons) {
        button.addEventListener('click', (e) => removeGroupProfileQuestion(e.target))
    }
}

function addEventHandlersGroupMcProfileQuestions() {
    let addGroupOptionButtons = document.querySelectorAll(`[id *= "addGroupProfileOption_"]`)
    for (let addOptionButton of addGroupOptionButtons) {
        addOptionButton.addEventListener('click', (e) => addGroupProfileOption(e.target))
    }
}

function addEventHandlersGroupProfileOptions() {
    const groupOptionButtons = document.querySelectorAll(`[id *= "removeGroupOption_"]`)
    for (let optionButton of groupOptionButtons) {
        optionButton.addEventListener('click', (e) => removeGroupProfileOption(e.currentTarget))
    }
}
/* end of addEventHandlers functions for GroupProfileQuestions */

/* addEventHandlers functions for StudentProfileQuestions */
function addEventHandlersStudentProfileQuestions() {
    let removeStudentProfileQuestionButtons = document.querySelectorAll(`[id *= "removeStudentProfileQuestion_"]`)
    for (let button of removeStudentProfileQuestionButtons) {
        button.addEventListener('click', (e) => removeStudentProfileQuestion(e.target))
    }
}

function addEventHandlersStudentMcProfileQuestions() {
    const addStudentOptionButtons = document.querySelectorAll(`[id *= "addStudentProfileOption_"]`)
    for (let addOptionButton of addStudentOptionButtons) {
        addOptionButton.addEventListener('click', (e) => addStudentProfileOption(e.target))
    }
}

function addEventHandlersStudentProfileOptions() {
    const studentOptionButtons = document.querySelectorAll(`[id *= "removeStudentOption_"]`)
    for (let optionButton of studentOptionButtons) {
        optionButton.addEventListener('click', (e) => removeStudentProfileOption(e.currentTarget))
    }
}
/* end of addEventHandlers functions for StudentProfileQuestions */


document.getElementById("createTask").addEventListener("click", (e) => {
   return saveData(1)
})


let tasksTable = document.querySelectorAll(`[id *= "editTask_"]`)

for (let i = 0; i < tasksTable.length; i++) {
    tasksTable[i].addEventListener("click", () => {
        let tempId = tasksTable[i].id.replace(/^\D+/g, '');
        return  saveData(2,tempId)
    })
}



