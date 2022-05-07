const LogoInput=document.querySelector('#logo')
LogoInput.addEventListener('change',(event)=>{UploadSetUpLogo(event.target)})
const setUpLogo=document.querySelector('#setUpLogo')
let setUpId=document.querySelector('#setUpId')
function UploadSetUpLogo(e) {
    let formdata = new FormData()
    formdata.append('Image', e.files[0])
    let source = URL.createObjectURL(e.files[0])
    setUpLogo.setAttribute('src',source)


    fetch('/api/Photos/setuplogo/' + setUpId, {
        method: 'POST',
        body: formdata,
        headers:
            {
                'Accept': 'application/json'
            }

    }).then(function (response) {
        if (response.ok){}
            })

}