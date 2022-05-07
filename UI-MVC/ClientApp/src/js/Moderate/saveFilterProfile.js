const saveFilterProfileButton = document.getElementById('save-filter-profile')
const profileSelect = document.getElementById('filter-profiles');
const filters = document.getElementsByClassName('filter-select');


saveFilterProfileButton.addEventListener('click', () => {
    if (!checkEmpty()) {
        createNewProfile()
    }else {
        alert("Gelieve minstens 1 filter open een  geldig waarde te zetten!")
    }
})

//Returns true if all filter-select elements have no value
function checkEmpty() {
    let allEmpty = true;
    for (const filter of filters) {
        if (filter.value !== '') {
            allEmpty = false;
        }
    }
    return allEmpty;
}

function createNewProfile() {
    const teacherId = document.querySelector('#teacherId').innerHTML;

    const filterProfileName = document.querySelector('#filter-profile-name').value;
    if (filterProfileName === ''){
        alert("Profielnaam is niet ingevuld!")
    } else {
        let usedFilters = new Map();
        for (const filterSelect of filters) {
            if (filterSelect.value !== '') {
                const value = filterSelect.value;
                const description = filterSelect.id;
                usedFilters[description] = value;
            }
            
               
        }

        let filterProfile = {name: filterProfileName, userId: teacherId, filters: usedFilters}

        fetch('/api/FilterProfiles/', {
            method: 'PUT',
            body: JSON.stringify(filterProfile),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            if (response.ok) {
                return response.json();
            }
        }).then(function (data) {
            let optionElement = document.createElement('option');
            optionElement.value = data.filterProfileId;
            optionElement.innerHTML = data.profileName;

            profileSelect.appendChild(optionElement);
            profileSelect.value = data.filterProfileId;
        })
    }
    document.querySelector('#filter-profile-name').value=''
}