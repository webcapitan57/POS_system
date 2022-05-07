let primColor = document.getElementById("primColor").innerText;
let secColor = document.getElementById("secColor").innerText;


document.documentElement.style.setProperty('--primary-color', primColor);
document.documentElement.style.setProperty('--secondary-color', secColor);

document.getElementById("secColor").style.setProperty('color', 'transparent')
document.getElementById("primColor").style.setProperty('color', 'transparent')


