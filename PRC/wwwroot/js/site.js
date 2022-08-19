// Set the current newsletter.
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];
function setNewsLetter() {
    var newsletter = document.querySelector(".newsletter");
    const azureBlob = "https://pierpontrcassets.blob.core.windows.net/website/Sidelines-";
    const pdf = ".pdf";
    var newsUrl = "";
    var d = new Date();
    var y = d.getFullYear();
    var m = monthNames[d.getMonth()].substring(0, 3);
    newsUrl = azureBlob + m + "-" + y + pdf;
    newsletter.href = newsUrl;
}

setNewsLetter();

var date = new Date();
var month = date.getMonth() + 1;
var thisMonth = date.getMonth() + 1;
var year = date.getFullYear();
var thisYear = date.getFullYear();

var days = daysInMonth(year, month);
var monthName = monthNames[date.getMonth()];

//Setup the modal that displays the event data.
var modal = document.getElementById("eventModal");
var span = document.getElementsByClassName("close")[0];
var eventTitle = document.querySelector('.event-title');
var eventDated = document.querySelector('.event-dated');
var eventContent = document.querySelector('.event-content');

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

function daysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}

function firstDayOfMonth() {
    var date = new Date(year, month - 1, 1);
    var startDay = date.getDay();
    var btn = document.querySelectorAll(".btn-date");
    switch (startDay) {
        case 0:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 1:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 2:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 3:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 4:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 5:
            btn[0].style.gridColumn = startDay + 1;
            break;
        case 6:
            btn[0].style.gridColumn = startDay + 1;
            break;
        default:
    }
} 

function makeCalendar(days, month, monthName) {    
    var indicator = document.querySelector('.month-indicator');
    var grid = document.querySelector('.date-grid');

    var dd = String(date.getDate()).padStart(2, '0');

    for (i = 1; i <= days; i++) {
        //Name of the month on top.
        indicator.innerHTML = monthName;

        var btn = document.createElement("BUTTON");
        var time = document.createElement("TIME");

        //This class allows all of the buttons to be grouped by class.
        //Used to set the first day of the month. (Mon, Tues, etc.)
        btn.classList.add("btn-date");

        //Set the datetime attribute to be used for modal display.
        time.dateTime = monthName + " " + i + ", " + year;

        //Append a 0 to dates lower than 10 because of event data.
        i < 10 ? time.innerHTML = 0 + i.toString() : time.innerHTML = i;

        //This class allows all of the time elements to be grouped by class.
        //Used to loop through calendar and add event data.
        time.classList.add("cal-date");

        //Add style to the current day. Used only for current month.
        if (month == thisMonth && year == thisYear) {
            dd == i ? btn.classList.add("current-day") : null;
        }

        btn.appendChild(time);
        grid.appendChild(btn);        
    }

    firstDayOfMonth(year, month); //Positioning the first day. 
    loadEvents(month, year); //Gets the event data from the sever.
};

function nextMonth() {
    month = month == 12 ? changeYear(1) : month + 1;
    var grid = document.querySelector('.date-grid');
    var days = daysInMonth(year, month);
    var monthName = monthNames[month - 1];

    grid.innerHTML = "";
    makeCalendar(days, month, monthName);
}

function prevMonth() {
    month = month == 1 ? changeYear(-1) : month - 1;
    var grid = document.querySelector('.date-grid');
    var days = daysInMonth(year, month);
    var monthName = monthNames[month - 1];

    grid.innerHTML = "";
    makeCalendar(days, month, monthName);
}

function changeYear(dir) {
    if (dir == 1) {
        year++;
        month = 1;
    } else {
        year--;
        month = 12;
    }
    return month;
}

function loadEvents(month, year) {
    const eventEndPoint = "/Events/List";
    var xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var data = JSON.parse(this.responseText);
            var dates = document.querySelectorAll('.cal-date');

            for (var key in data) {
                //Get the string array items for the date.
                eventDate = data[key].date[8] + data[key].date[9];
                for (i = 0; i < dates.length; i++) {
                    if (dates[i].innerHTML == eventDate) {
                        dates[i].parentNode.classList.add('event-date');

                        //Create the listener to bring up event details in a modal.
                        dates[i].addEventListener("click", showEvent);

                        //Add the event title and content. Keep them hidden until clicked.
                        var title = document.createElement("H6");
                        var desc = document.createElement("P");
                        title.classList.add('hidden');
                        title.classList.add('event-title');
                        title.innerHTML = data[key].title;
                        desc.classList.add('hidden');
                        desc.classList.add('event-desc');
                        desc.innerHTML = data[key].content;
                        dates[i].appendChild(title);
                        dates[i].appendChild(desc);
                    }
                }
            }
        }
    }

    var data = JSON.stringify({
        MonthYear: month.toString() + year.toString()
    });
    
    xhttp.open("POST", eventEndPoint, true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send(data);
}

function showEvent(e) {
    //Clear the modal.
    eventTitle.innerHTML = "";
    eventDated.innerHTML = "";
    eventContent.innerHTML = "";

    //Add event data.
    eventTitle.innerHTML = e.target.children[0].innerHTML;
    eventDated.innerHTML = e.target.dateTime;
    eventContent.innerHTML = e.target.children[1].innerHTML;

    //Show modal.
    modal.style.display = "block";
}

makeCalendar(days, month, monthName);

var slider = document.querySelector('.slider'); 
var slideContainer = document.querySelector('.slide-container');

if (slider.children.length == 0) {
    slideContainer.style.display = "none";
}