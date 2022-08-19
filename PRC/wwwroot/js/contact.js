var form = document.querySelector(".btn-contact");
form.addEventListener("click", sendContact);

function sendContact() {
    const eventEndPoint = "/Contact/Index";
    var xhttp = new XMLHttpRequest();
    var name = document.getElementById("name").value;
    var email = document.getElementById("email").value;
    var message = document.getElementById("message").value;
    var target = document.querySelector(".contact-form").id;

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("Thank you. Your message has been sent.");
            document.getElementById("name").value = "";
            document.getElementById("email").value = "";
            document.getElementById("message").value = "";
        }
    }

    var data = JSON.stringify({
        name: name,
        email: email,
        message: message,
        target: target
    });

    xhttp.open("POST", eventEndPoint, true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send(data);
}