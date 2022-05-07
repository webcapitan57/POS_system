
import {addTagToAnswer} from "./customTags";
import {addLikeEvent,removeLikeEvent,currentTeacher} from "./Gallery/likeGallery";
import {flagPhoto, unflagPhoto} from "./moderateFlaggedPhotos"
import * as signalR from "@microsoft/signalr"
import {FilterValueExists} from "./Gallery/filterGallery";
import {removeAnswer} from "./moderateDeleteAnswer"


const acceptDeliveriesButton = document.querySelector("#acceptDeliveries")
const groupCode = document.querySelector(`[id *= "group_"]`).id.split('_')[1]

const bucketName = document.querySelector("#bucket").textContent
const bucketURL = `https://storage.googleapis.com/${bucketName}/Images/`

const flaggedSection = document.querySelector("#flagged")
const notFlaggedSection = document.querySelector("#notFlagged")
const detailDiv = document.querySelector('#detailDiv')

// Create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/moderateDeliveries")
    .build();

// Start connection
async function start() {
    bucketName
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
}

connection.onclose(start);


start().then(() => {
    acceptDeliveriesButton.addEventListener('click', e => {
        checkAcceptDeliveries(e.target)
    })
    //Join SignalR group
    connection.invoke("JoinGroup", groupCode).then(() => {
        console.log("Joined group")
    })

    // Hub methods
    connection.on("SendDeliveries", (deliveryIds) => {
        fetchPhotos(deliveryIds)
    })
})

function checkAcceptDeliveries(target) {
    if (target.checked) {
        joinGroup()
    } else {
        leaveGroup()
    }
}

function joinGroup() {
    connection.invoke("JoinGroup", groupCode).then(() => console.log("Joined group"))
}

function leaveGroup() {
    connection.invoke("LeaveGroup", groupCode).then(() => console.log("Left group"))
}

function fetchPhotos(deliveryIds) {
    deliveryIds.forEach((x) => getDelivery(x));
}

function getDelivery(deliveryId) {
    fetch('/api/Answers/' + deliveryId, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
            if (response.ok) {
                response.json()
                    .then(data => {
                        console.log(data)
                        AddPhotos(data.answers,data.tags)
                    })
            }
        }
    )
}

function AddPhotos(answers,tags) {
    answers.forEach((answer) => AddPhoto(answer,tags))
}

function AddPhoto(answer,tags) {
    let answerId = answer.answerId
    let source = bucketURL + answer.assignedPhoto.picture

    let liElement = document.createElement('li');

    let img=CreateElement('img',["id;gallery-image_"+answerId,"src;"+source,"class;gallery-image"])
    
    let photoDetails = CreateElement('div', ["id;answerDetails_" + answerId, "style;display: none", "class;gallery-side-answers"])

    let btn = CreateElement('button', ["class;btn btn-secondary btn-gallery-back"])
    btn.innerHTML = "Terug";
    let question = document.createElement('h5')
    question.innerHTML = answer.answeredQuestion.question;

    photoDetails.appendChild(btn)
    photoDetails.appendChild(question)

    let div2 = document.createElement('div')
    let image = CreateElement('img', ["src;" + source, "class;side-answers-img"])

    div2.appendChild(image)

    let moderateAnswersDiv = CreateElement('div', ["id;moderate-answer-buttons_" + answerId])
    moderateAnswersDiv.classList.add('moderate-answer-buttons')

    let galleryLikeDiv = CreateElement('div', ["id;like-buttons_" + answerId, "class;gallery-like-buttons"])

    let galleryBtnLikeDiv = CreateElement('div', ["class;gallery-btn-like"])

    let svgLike = CreateSVGElement('m8 2.748-.717-.737C5.6.281 2.514.878 1.4 3.053c-.523 1.023-' +
        '.641 2.5.314 4.385.92 1.815 2.834 3.989 6.286 6.357 3.452-2.368 5.365-4.542 6.286-6.357.955-1.886.838-' +
        '3.362.314-4.385C13.486.878 10.4.28 8.717 2.01L8 2.748zM8 15C-7.333 4.868 3.279-3.04 7.824 1.143c.06.055.119.' +
        '112.176.171a3.12 3.12 0 0 1 .176-.17C12.72-3.042 23.333 4.867 8 15z', ["class;bi bi-heart"])

    galleryBtnLikeDiv.appendChild(svgLike)
    galleryLikeDiv.appendChild(galleryBtnLikeDiv)


    let galleryBtnUnLikeDiv = CreateElement('div', ["class;gallery-btn-unlike"])


    let svgUnLike = CreateSVGElement('M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z',
        ["class;bi bi-heart-fill"])

    galleryBtnUnLikeDiv.appendChild(svgUnLike)
    galleryLikeDiv.appendChild(galleryBtnUnLikeDiv)

    moderateAnswersDiv.appendChild(galleryLikeDiv)
    let moderateFlagDiv;
    if (answer.flagged){
        moderateFlagDiv = CreateElement('div', ["id;flag-buttons_" + answerId, "class;moderate-flag-buttons moderate-flagged"])
    }
    else
    {
         moderateFlagDiv = CreateElement('div', ["id;flag-buttons_" + answerId, "class;moderate-flag-buttons"])
    }

    let moderateflag = CreateElement('div', ["class;moderate-btn-flag"])

    let flagbutton = CreateElement('div', ["id;flag-button_" + answerId, "class;flag-button"])

    let svgFlag = CreateSVGElement('M14.778.085A.5.5 0 0 1 15 .5V8a.5.5 0 0 1-.314.464L14.5 8l.186.464-.003.001-.006.003-.023.009a12.435 12.435 0 0 1-.397.15c-.264.095-.631.223-1.047.35-.816.252-1.879.523-2.71.523-.847 0-1.548-.28-2.158-.525l-.028-.01C7.68 8.71 7.14 8.5 6.5 8.5c-.7 0-1.638.23-2.437.477A19.626 19.626 0 0 0 3 9.342V15.5a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 1 0v.282c.226-.079.496-.17.79-.26C4.606.272 5.67 0 6.5 0c.84 0 1.524.277 2.121.519l.043.018C9.286.788 9.828 1 10.5 1c.7 0 1.638-.23 2.437-.477a19.587 19.587 0 0 0 1.349-.476l.019-.007.004-.002h.001M14 1.221c-.22.078-.48.167-.766.255-.81.252-1.872.523-2.734.523-.886 0-1.592-.286-2.203-.534l-.008-.003C7.662 1.21 7.139 1 6.5 1c-.669 0-1.606.229-2.415.478A21.294 21.294 0 0 0 3 1.845v6.433c.22-.078.48-.167.766-.255C4.576 7.77 5.638 7.5 6.5 7.5c.847 0 1.548.28 2.158.525l.028.01C9.32 8.29 9.86 8.5 10.5 8.5c.668 0 1.606-.229 2.415-.478A21.317 21.317 0 0 0 14 7.655V1.222z',
        ["class;bi bi-flag"],'evenodd')
    flagbutton.appendChild(svgFlag)
    moderateflag.appendChild(flagbutton)
    
    let moderateUnflag = CreateElement('div', ["class;moderate-btn-unflag"])
    let unflagbutton = CreateElement('div', ["id;unflag-button_" + answerId, "class;unflag-button"])
    
    let svgUnFlag=CreateSVGElement('M14.778.085A.5.5 0 0 1 15 .5V8a.5.5 0 0 1-.314.464L14.5 8l.186.464-.003.001-.006.003-.023.009a12.435 12.435 0 0 1-.397.15c-.264.095-.631.223-1.047.35-.816.252-1.879.523-2.71.523-.847 0-1.548-.28-2.158-.525l-.028-.01C7.68 8.71 7.14 8.5 6.5 8.5c-.7 0-1.638.23-2.437.477A19.626 19.626 0 0 0 3 9.342V15.5a.5.5 0 0 1-1 0V.5a.5.5 0 0 1 1 0v.282c.226-.079.496-.17.79-.26C4.606.272 5.67 0 6.5 0c.84 0 1.524.277 2.121.519l.043.018C9.286.788 9.828 1 10.5 1c.7 0 1.638-.23 2.437-.477a19.587 19.587 0 0 0 1.349-.476l.019-.007.004-.002h.001',
        ["class;bi bi-flag-fill"])
    
    let removeAnswerDiv=CreateElement('div', ["id;deleteAnswer_"+answerId,"class;moderate-delete-button"])
    let removeAnswerIcon=CreateSVGElement('M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z',
        ["class;bi bi-thrash"])
    let trashpath=document.createElementNS('http://www.w3.org/2000/svg','path')
    trashpath.setAttribute('d','M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z')
    trashpath.setAttribute('fill-rule',"evenodd")
    removeAnswerIcon.appendChild(trashpath)
    
    removeAnswerDiv.appendChild(removeAnswerIcon)
    moderateAnswersDiv.appendChild(removeAnswerDiv)
    
    

    //Tags
    let moderateAnswerTags = CreateElement('div', ["id;moderate-answer-tags_" + answerId])
    moderateAnswerTags.classList.add('moderate-answer-tags')
    let tagsh6 = document.createElement('h6')
    tagsh6.innerHTML = 'Tags'
    moderateAnswerTags.appendChild(tagsh6)
    let moderateTags = CreateElement('div', ["id;tags_" + answerId, "class;moderate-tags"])
    if (tags!==null) {
        for (let i=0;i<tags.length;i++) {
            let tagDiv = CreateElement('div', ["class;moderate-profile-tag"])
            tagDiv.innerHTML = `${tags[i].description} : ${tags[i].value}`
            moderateTags.appendChild(tagDiv)

            //Checking if the values of our tags are already in our filters
            FilterValueExists(tags[i])
        }
    }
    
    //New Tags
    let newTags=CreateElement('div',["id;new-tag"])
    let newTagInput=CreateElement('input',["id;tagInput_"+answerId,"class;new-tag-input form-control","type;text","placeholder;Nieuwe tag..."])
    let newTagbtn=CreateElement('btn',["id;tagInputBtn_"+answerId,"class;btn-add-tag btn btn-primary"])
    newTagbtn.innerHTML="Toevoegen"
    
    newTags.appendChild(newTagInput)
    newTags.appendChild(newTagbtn)
    
    moderateAnswerTags.appendChild(moderateTags)
    moderateAnswerTags.appendChild(newTags)



    let moderateSideAnswers = CreateElement('div', ["id;side-answers_" + answerId, "class;moderate-side-answers"])
    //SideAnswers
    if (answer.sideAnswers!==null) {
       
        let sideanswersh6 = document.createElement('h6')
        sideanswersh6.innerHTML = "Deelvragen"
        moderateSideAnswers.appendChild(sideanswersh6)
        
        for (let i=0;i<answer.sideAnswers.length;i++){
            let div=CreateElement('div',["class;form-group"])
            let l=document.createElement('label')
            l.innerHTML=`${answer.sideAnswers[i].answeredQuestion.question}`
            let inp=CreateElement('input',["class;form-control","value;"+answer.sideAnswers[i].givenAnswer])
            inp.disabled=true;
            
            div.appendChild(l)
            div.appendChild(inp)
            moderateSideAnswers.appendChild(div)
        }
        
        //Adding the evenListeners to our created Items
        addEventListeners(btn,newTagbtn,galleryBtnLikeDiv,galleryBtnUnLikeDiv,
            flagbutton,unflagbutton,removeAnswerDiv)
        
       
        
        
        

    }
    
    unflagbutton.appendChild(svgUnFlag)
    moderateUnflag.appendChild(unflagbutton)


    moderateFlagDiv.appendChild(moderateflag)
    moderateFlagDiv.appendChild(moderateUnflag)
    
    moderateAnswersDiv.appendChild(moderateFlagDiv)

    
    div2.appendChild(moderateAnswersDiv)
    div2.appendChild(moderateAnswerTags)
    photoDetails.appendChild(div2)
    if (answer.sideAnswers!==null){photoDetails.appendChild(moderateSideAnswers)}
    detailDiv.appendChild(photoDetails)
    img.addEventListener('click', (event) => {

        document.getElementById('photos').style.display = 'none'
        document.getElementById('filter-bar').style.display = 'none'

        detailDiv.querySelector(`#answerDetails_${event.target.id.split('_')[1]}`).style.display = 'inline'
    })
    

    liElement.appendChild(img)

    if (answer.flagged) {
        document.getElementById('flaggedList').appendChild(liElement)

    } else {
        document.getElementById('notFlaggedList').appendChild(liElement)
    }

    console.log(answer)
}


function CreateElement(tagName, attributes) {
    let e = document.createElement(tagName)
    for (let attribute of attributes) {
        let qName = attribute.split(';')[0];
        let value = attribute.split(';')[1];
        e.setAttribute(qName, value);

    }
    return e;
}

function CreateSVGElement(pathvalue, attributes,fillrule="") {

    let svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg')
    svg.setAttribute('viewBox', '0 0 16 16')
    for (let attribute of attributes) {
        let qName = attribute.split(';')[0]
        let value = attribute.split(';')[1]
    }
    let pathelement = document.createElementNS('http://www.w3.org/2000/svg', 'path')
    pathelement.setAttribute('d', pathvalue)
    if (fillrule!==""){ pathelement.setAttribute('fill-rule',fillrule)}
    svg.appendChild(pathelement)
    return svg;


}

function addEventListeners(backButton,newTagButton,like,unlike,flag,unflag,removebutton){
    
    backButton.addEventListener('click',()=>{
        document.getElementById('photos').style.display = 'flex'
        document.getElementById('filter-bar').style.display = 'flex'
        document.getElementById('moderate-sliders').style.display = 'flex'

        for (const sideAnswersDiv of document.getElementsByClassName('gallery-side-answers')) {
            sideAnswersDiv.style.display = 'none'
        }
    })
    newTagButton.addEventListener('click',()=>{
        addTagToAnswer(newTagButton.id.split('_')[1])
    })
    like.addEventListener('click',()=>{
        addLikeEvent(like.parentElement.id.split('_')[1], currentTeacher, 1)
    })

    unlike.addEventListener('click', () => {
        removeLikeEvent(unlike.parentElement.id.split('_')[1], currentTeacher)
    }) 
    flag.addEventListener('click',()=>{
        flagPhoto(parseInt(flag.id.split('_')[1]))
    })
    unflag.addEventListener('click',()=>{
        unflagPhoto(parseInt(unflag.id.split('_')[1]))
    })
    removebutton.addEventListener('click',()=>{
        removeAnswer(parseInt(removebutton.id.split('_')[1]))
    })
}
