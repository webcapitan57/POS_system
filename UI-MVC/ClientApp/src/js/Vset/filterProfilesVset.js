const profileSelect = document.getElementById('filter-profiles');
const filters = document.getElementsByClassName('filter-select');

profileSelect.addEventListener('change', () => {
    const value = profileSelect.value;

    for (const tagFilter of filters) {
        tagFilter.value = '';
    }

    if (value !== '') {
        fetch('/api/filterProfiles/' + value + '/filters', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            if (response.ok) {
                return response.json();
            }
        }).then(function (data) {
            for (const filter of data) {

                for (const tagFilter of filters) {
                    if (tagFilter.id === filter.description) {
                        tagFilter.value = filter.value;
                    }
                }
            }

            reloadFilters()
        })
    } else {
        reloadFilters()
    }
})

function reloadFilters() {
    for (const tagFilterElement of filters) {
        const evt = document.createEvent("HTMLEvents");
        evt.initEvent("change", false, true);
        tagFilterElement.dispatchEvent(evt);
    }
}