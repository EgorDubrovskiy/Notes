let ValPass = new ValidationPassword();
let ValPassConf = new ValidationPasswordConf();

function PassChange() {
    if (ValPass.IsVal && ValPassConf.IsVal)
    {
        $.ajax({
            type: "POST", url: "http://localhost/Notes/User/SaveNewPass?NewPassword=" + $('#PassInput').val() +
                "&RestoringEmail=" + getUrlVars()["RestoringEmail"],
            success: function () {
                window.location.replace("http://localhost/Notes/Home/Index");
            }
        });
    }
    else $('#errorArea').removeClass("d-none");
}

$('#signupFormOk').modal({
    backdrop: "static",
    show: false
});