function copy() {
    let copyText = document.querySelector('#loginUrl');
    copyText.select();
    document.execCommand("copy");
    let img = document.querySelector("#unClipped")
    if (img !== null) {
        img.setAttribute('src', "/resources/Icons/clipboard-done.svg")
        img.setAttribute('id',"clipped")
    }
}

document.querySelector('#saveUrl').addEventListener("click", copy);
