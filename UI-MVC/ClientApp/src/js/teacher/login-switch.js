document.getElementById('show-group-code').addEventListener('click', () => {
    document.getElementById('email-login').classList.replace('login-wide','login-narrow');
    document.getElementById('group-code-login').classList.replace('login-narrow','login-wide');
})

document.getElementById('show-email').addEventListener('click', () => {
    document.getElementById('group-code-login').classList.replace('login-wide','login-narrow');
    document.getElementById('email-login').classList.replace('login-narrow','login-wide');
})  

//TODO hardcoded setupId
let setUpId = 1
let groupLinkSubmit = document.getElementById('new-group')
groupLinkSubmit.addEventListener('click', ev => {
    ev.preventDefault()
    fetch('/api/teachers/' + setUpId , {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.url) {
            window.location.href = response.url
        } else {
            throw new Error();
        }
    })
    .catch(() => {
        alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
    })
})


let groupLinkLogin = document.getElementById('group-login-submit')
groupLinkLogin.addEventListener('click', ev => {
    let groupCode = document.getElementById("group-code-value").value
    let pass = document.getElementById("group-pass-value").value
    ev.preventDefault()
    fetch('/api/teachers/' + groupCode + "/" + pass , {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    }).then(function (response) {
        if (response.url) {
            window.location.href = response.url
        } else {
            throw new Error();
        }
    })
        .catch(() => {
            alert("Oops, something went wrong! Our slaves are working hard to find a solution.")
        })
})