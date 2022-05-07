const UploadedPhotos=document.querySelector('#UploadedPhotos')
const uploadPhoto=document.querySelector('#uploadPhoto')
uploadPhoto.addEventListener('change',function (){uploadNewPhoto();})
const deliveryId=parseInt(document.querySelector('#taskDeliveryId').textContent)

function uploadNewPhoto(){
        const image=uploadPhoto.files[0]
        const photo= {
                Image:image
        }
        fetch('/api/Photos/' + deliveryId, {
                method: 'POST',
                body: JSON.stringify(photo)
                ,
                headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                }
        }).then(function (response){
                if (response.ok){
                        
                        
                }
        }).catch(()=> {alert('Oeps, er ging iets mis')})
}