let password = document.getElementById("password");
let passwordStrength = document.getElementById("password-strength");
let lowCase = document.querySelector(".low-case i");
let upperCase = document.querySelector(".upper-case i");
let number = document.querySelector(".one-number i");
let specialChar = document.querySelector(".one-special-char i");
let eightChar = document.querySelector(".eight-character i");

password.addEventListener("keyup", function () {
    let pass = document.getElementById("password").value;
    checkStrength(pass);
});

function checkStrength(password) {
    let strength = 0;

    //If password contains lowercase characters
    if (password.match(/(.*[a-z].*)/)) {
        strength += 1;
        lowCase.classList.remove('fa-circle');
        lowCase.classList.add('fa-check');
    } else {
        lowCase.classList.add('fa-circle');
        lowCase.classList.remove('fa-check');
    }

    //If password contains uppercase characters
    if (password.match(/(.*[A-Z].*)/)) {
        strength += 1;
        upperCase.classList.remove('fa-circle');
        upperCase.classList.add('fa-check');
    } else {
        upperCase.classList.add('fa-circle');
        upperCase.classList.remove('fa-check');
    }

    //If it has numbers and characters
    if (password.match(/([0-9])/)) {
        strength += 2;
        number.classList.remove('fa-circle');
        number.classList.add('fa-check');
    } else {
        number.classList.add('fa-circle');
        number.classList.remove('fa-check');
    }
    //If it has one special character
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) {
        strength += 3;
        specialChar.classList.remove('fa-circle');
        specialChar.classList.add('fa-check');
    } else {
        specialChar.classList.add('fa-circle');
        specialChar.classList.remove('fa-check');
    }
    //If password is greater than 7
    if (password.length > 7) {
        strength += 3;
        eightChar.classList.remove('fa-circle');
        eightChar.classList.add('fa-check');
    } else {
        eightChar.classList.add('fa-circle');
        eightChar.classList.remove('fa-check');
    }

    // If value is less than 2
    if (strength == 1) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-danger');
        passwordStrength.style = 'width: 10%';
    }
    else if (strength == 2) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-danger');
        passwordStrength.style = 'width: 20%';
    }
    else if (strength == 3) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-danger');
        passwordStrength.style = 'width: 30%';
    }
    else if (strength == 4) {
        passwordStrength.classList.remove('progress-bar-danger');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 40%';
    }
    else if (strength == 5) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 50%';
    }
    else if (strength == 6) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 60%';
    }
    else if (strength == 7) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 70%';
    }
    else if (strength == 8) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 80%';
    }
    else if (strength == 9) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.add('progress-bar-warning');
        passwordStrength.style = 'width: 90%';
    }
    else if (strength == 10) {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-danger');
        passwordStrength.classList.add('progress-bar-success');
        passwordStrength.style = 'width: 100%';
    }
    else {
        passwordStrength.classList.remove('progress-bar-warning');
        passwordStrength.classList.remove('progress-bar-success');
        passwordStrength.classList.remove('progress-bar-danger');
        passwordStrength.style = 'width: 0%';
    }
}