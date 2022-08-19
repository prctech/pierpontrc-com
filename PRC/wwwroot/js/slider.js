var count = 0;
var time = 10000;

var slider = document.querySelector('.slider');
var slides = document.querySelector('.slides');
var slideContainer = document.querySelector('.slide-container');

slider.innerHTML = slides.innerHTML;
slideContainer.style.display = "flex";

function changeImg() {
    var slider = document.querySelector('.slider');
    for (var i = 0; i < slider.children.length; i++) {
        slider.children[i].style.opacity = 0;
        if (count == i) {
            slider.children[count].style.opacity = 100;
        }
    }
    
    if (count < slider.children.length - 1) {
        count++;
    } else {
        count = 0;
    }

    setTimeout("changeImg()", time);
}

changeImg();