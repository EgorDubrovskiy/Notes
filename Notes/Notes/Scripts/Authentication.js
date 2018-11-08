let RegValPass = new ValidationPassword();
let RegValPassConf = new ValidationPasswordConf();
let RegValMainEmail = new ValidationForDoubleEmail();
let RegValRestEmail = new ValidationForDoubleEmail();

let AutValMainEmail = new ValidationExistMainEmail();
let AutValPass = new ValidationPassForEmail();

let PassRecValRecEmail = new SimpleValidationRecEmail();

var RecapchaReg; var RecapchaAut; var RecapchaPassRec;

var onloadCallback = function(){
    //recapcha begin
    // Renders the HTML elements reCAPTCHA widgets.
    // The id of the reCAPTCHA widget is assigned to 'widgetId1'.
    RecapchaReg = grecaptcha.render('g-recaptcha-response-reg', {
        'sitekey': '6LeaWFUUAAAAAAa32wacPyJwDIXPpHCp05iJbpJG'
    });
    RecapchaAut = grecaptcha.render(document.getElementById('g-recaptcha-response-aut'), {
        'sitekey': '6LeaWFUUAAAAAAa32wacPyJwDIXPpHCp05iJbpJG'
    });
    RecapchaPassRec = grecaptcha.render('g-recaptcha-response-passRecovery', {
        'sitekey': '6LeaWFUUAAAAAAa32wacPyJwDIXPpHCp05iJbpJG'
    });
    //recapcha end
}

$(document).ready(function () {

    $('.notSpace').keypress(function (key) {
        if (key.charCode == 32) return false;
    });
    $('.notSpace').on('paste', function (e) { return false; });

});

function Registr() {
    if (RegValMainEmail.IsVal && RegValRestEmail.IsVal && RegValPass.IsVal &&
        RegValPassConf.IsVal && IsValidRecapcha(RecapchaReg))
    {
        var MainEmail = $('#form_signup_email').val();
        var RestEmail = $('#form_signup_restoring_email').val();
        var Password = $('#form_signup_pass').val();

        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/Registration?MainEmail=" + MainEmail + "&RestoringEmail=" +
                RestEmail + "&Password=" + Password,
            success: function (data)
            {
                $('#messageBoxText').html('<div class="text-center">Поздравляем, вы успешно зарегистрираны!</div><p>В течении нескольких минут на указанную вами почту придёт письмо! Для завершения регистрациии перейдите по ссылке в письме!</p>');
                $('#signupFormOk').modal('show');
            }
        });
    }
    else $('#errorAreaForReg').removeClass("d-none");
}

function UserLogin() {
    if (AutValMainEmail.IsVal && AutValPass.IsVal && IsValidRecapcha(RecapchaAut)) {
        var MainEmail = $('#form_authorization_email').val();
        window.location.replace("http://localhost/Notes/User/Login?MainEmail=" + MainEmail);
    }
    else $('#errorAreaLogin').removeClass("d-none");
}

function PassRecSendToEmail() {
    if (PassRecValRecEmail.IsVal && IsValidRecapcha(RecapchaPassRec)) {
        var Email = $('#passRecoveryEmail').val();
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/SendMesForPassRec?RestoringEmail=" + Email,
            success: function ()
            {
                $('#passRecoveryModal').modal('hide');
                $('#messageBoxText').html('<p>В течении нескольких минут на указанную вами почту придёт письмо! Для восстановления пароля перейдите по ссылке в письме</p>');
                $('#signupFormOk').modal('show');
            }
        });
    }
    else $('#errorAreaPassRecovery').removeClass("d-none");
}

function IsValidRecapcha(RecapWidget) {
    var IsVal;
    var response = grecaptcha.getResponse(RecapWidget);
    $.ajax({
        type: "POST", url: "http://localhost/Notes/User/IsValidRecapcha?response=" + response, async: false,
        success: function (data) { IsVal = Boolean(data); }
    });
    grecaptcha.reset(RecapWidget);
    return IsVal;
}

//модальное окно для отправки письма на вост. пароля
$('#passRecoveryModal').modal({
    backdrop: "static",//запрежаем закрытие при клике вне формы мышью
    show: false
});

//модальное окно для уведомления пользователя о успешной регистрации
$('#signupFormOk').modal({
    backdrop: "static",
    show: false
});

function PassRecoveryClick() { $('#passRecoveryModal').modal('show'); }