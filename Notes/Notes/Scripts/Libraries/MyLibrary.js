//Класс для проверки на наличие данных в бд
class IsData {

    static IsRestoringEmail(idInput) {
        var email = $('#' + idInput).val();
        email = encodeURIComponent(email);
        var isEmail;
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/IsRestoringEmail?Email=" + email,
            async: false, success: function (data) { isEmail = Boolean(data); }
        });
        return isEmail;
    }

    static IsMainEmail(idInput) {
        var email = $('#' + idInput).val();
        email = encodeURIComponent(email);
        var isEmail;
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/IsMainEmail?Email=" + email,
            async: false, success: function (data) { isEmail = Boolean(data); }
        });
        return isEmail;
    }

    static IsSecretKeyByMainEmail(idInputEmail) {
        var email = $('#' + idInputEmail).val();
        email = encodeURIComponent(email);
        var isSecretKey;
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/IsSecretKeyByMainEmail?Email=" + email,
            async: false, success: function (data) { isSecretKey = Boolean(data); }
        });
        return isSecretKey;
    }

    static IsPassByMainEmail(idPassInput, idEmailInput) {
        var pass = $('#' + idPassInput).val();
        var email = $('#' + idEmailInput).val();
        pass = encodeURIComponent(pass);
        email = encodeURIComponent(email);
        var isPass;
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/IsPassByMainEmail?Pass=" + pass + "&Email=" + email,
            async: false, success: function (data) { isPass = Boolean(data); }
        });
        return isPass;
    }
}

//Класс для уведомления об ошибках при вводе данных в поля
class CoutMessage {
    static InvalTxtArea(idInp, idBlockForMes, errText) {
        document.getElementById(idInp).className = "form-control is-invalid";
        document.getElementById(idBlockForMes).className = "invalid-feedback";
        $('#' + idBlockForMes).html(errText);
    }

    static ValTxtArea(idInp, idBlockForMes) {
        document.getElementById(idInp).className = "form-control is-valid";
        document.getElementById(idBlockForMes).className = "valid-feedback";
        $('#' + idBlockForMes).html("Данные введены правильно!");
    }
}

//Классы для проверки валидации полей

//валидация пароля без проверки из бд
class ValidationPassword {
    constructor() {
        this.IsVal = false;
    }

    PassChange(idInput, idMistakesArea) {
        var value = $('#' + idInput).val();
        var mistakes = "";
        if (value.length == 0) mistakes += "Введите пароль!<br>";
        else {
            if (!(value.length > 8 && value.length < 21)) mistakes += "Пароль должен содержать от 8 до 20 символов!<br>";
            if (!/[0-9]/.test(value)) mistakes += "Пароль должен содержать хотя бы одну цифру!<br>";
            if (/[а-я,А-Я]/.test(value)) mistakes += "Пароль не должен содержать кирилицу!<br>";
            if (!/[a-z]/.test(value)) mistakes += "Пароль должен содержать хотя бы одну строчную букву!<br>";
            if (!/[A-Z]/.test(value)) mistakes += "Пароль должен содержать хотя бы одну прописную букву!<br>";
            if (!/[_, #, *]/.test(value)) mistakes += "Пароль должен содержать хотя бы один специальный символ (_, #, *)!<br>";
        }

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idInput, idMistakesArea, mistakes); }
    }
}

//проверка на соответствие пароля и повторно введённого пароля
class ValidationPasswordConf {
    constructor() {
        this.IsVal = false;
    }

    PassConfChange(idInput, idInput2, idMistakesArea) {
        var value = $('#' + idInput).val();
        var mistakes = "";
        if (value.length == 0) mistakes += "Подтвердите пароль!<br>";
        else if (value != $('#' + idInput2).val()) mistakes += "Повторный пароль введён не верно!<br>";

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idInput, idMistakesArea, mistakes); }
    }

}

class ValidationExistMainEmail {

    constructor() {
        this.IsVal = false;
    }

    EmailChange(idInput, idMistakesArea) {
        var value = $('#' + idInput).val();
        var mistakes = "";
        if (value.length == 0) mistakes += "Введите Email!<br>";
        else if (!/.{1,}@.{1,}\..{1,}/.test(value)) mistakes += "Неверный формат адреса!<br>Пример верного формата - name@mail.ru";
        else if (!IsData.IsMainEmail(idInput)) mistakes += "Пользователь с таким Email не существует!";
        else if (IsData.IsSecretKeyByMainEmail(idInput)) mistakes += "Подтвердите регистрацию на почте!";

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idInput, idMistakesArea, mistakes); }
    }
}

//валидацияпароля для главного email
class ValidationPassForEmail {

    constructor() {
        this.IsVal = false;
    }

    PassChange(idPassInput, idEmailInput, idMistakesArea) {
        var pass = $('#' + idPassInput).val();
        var mistakes = "";
        if (!IsData.IsMainEmail(idEmailInput)) mistakes += "Введите email от зарегистрированного аккаунта!";
        else if (pass.length == 0) mistakes += "Введите пароль!";
        else if (!IsData.IsPassByMainEmail(idPassInput, idEmailInput)) mistakes += "Неверно введённый пароль!";

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idPassInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idPassInput, idMistakesArea, mistakes); }
    }

}

//валидация одного из двух email (главного и для восстановления пароля)
class ValidationForDoubleEmail {

    constructor() {
        this.IsVal = false;
    }

    EmailChange(idInput, idInput2, idMistakesArea) {
        var value = $('#' + idInput).val();
        var value2 = $('#' + idInput2).val();
        var mistakes = "";
        if (value.length == 0) mistakes += "Введите Email!<br>";
        else if (!/.{1,}@.{1,}\..{1,}/.test(value)) mistakes += "Неверный формат адреса!<br>Пример верного формата - name@mail.ru";
        else if (value == value2) mistakes += "Введите разные Email адреса!<br>";
        else if (IsData.IsMainEmail(idInput) || IsData.IsRestoringEmail(idInput)) mistakes += "Пользователь с таким Email уже существует!";

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idInput, idMistakesArea, mistakes); }
    }
}

//проверка на наличие email для вост. пароля в бд
class SimpleValidationRecEmail {

    constructor() {
        this.IsVal = false;
    }

    EmailChange(idInput, idMistakesArea) {
        var value = $('#' + idInput).val();
        var mistakes = "";
        if (value.length == 0) mistakes += "Введите Email!<br>";
        else if (!IsData.IsRestoringEmail(idInput)) mistakes += "Пользователь с таким Email не существует!";

        if (mistakes == "") { this.IsVal = true; CoutMessage.ValTxtArea(idInput, idMistakesArea); }
        else { this.IsVal = false; CoutMessage.InvalTxtArea(idInput, idMistakesArea, mistakes); }
    }
}

function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}


//аналог var_dump из php дял js  
function objDump(object) {
    var out = "";
    if (object && typeof (object) == "object") {
        for (var i in object) {
            out += i + ": " + object[i] + "\n";
        }
    } else {
        out = object;
    }
    alert(out);
}

