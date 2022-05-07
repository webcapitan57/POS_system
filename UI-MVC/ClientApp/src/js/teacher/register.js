let registerButton = document.getElementById("register_form")
window.addEventListener("load",ev=> {
    let usernameInput = document.getElementById("username_input").value
    if(usernameInput!==""){
        document.getElementById("username_span").innerText = "deze gebruikersnaam is al in gebruik in deze setup."
    }
 })