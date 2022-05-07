const reportButtons = document.getElementsByClassName('btn-report');
const teacherId = document.getElementById('teacherId').innerHTML;
const likeButtonDivs = document.getElementsByClassName('gallery-like-buttons')

for (const reportButton of reportButtons) {
    reportButton.addEventListener('click', () => {
        
        let proceed = confirm("Bent u zeker dat u dit antwoord wilt rapporteren? Deze actie kan niet ongedaan gemaakt worden.")
        if (proceed){
            addReportEvent(reportButton.id, teacherId)
                .then(r => {
                    if (r.ok){
                        reportButton.parentElement.classList.add('vset-reported');
                        for (const likeButtonDiv of likeButtonDivs) {
                            if (likeButtonDiv.id === reportButton.id){
                                likeButtonDiv.style.display = 'none'
                            }
                        }
                    }
                });
        }
    })
}

async function addReportEvent(answerId, teacherId) {
    return fetch('/api/qualityScoreEvents/' + teacherId + '/' + answerId + '/' + -1 , {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function (response) {
        if (response.ok) {
            console.log(response)
            return response
        } else {
            throw new Error()
        }
    }).catch((e) => {
        alert("Oops, Event: " + e.name + ": " + e.message)
    })
}