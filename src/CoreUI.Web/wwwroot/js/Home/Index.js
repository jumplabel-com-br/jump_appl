var wlhs = window.location.href.split('/');

if (wlhs[3] == '#true') {
    $('.UserOrPasswordInvalid').show();
}

function newPassoword() {

    if ($('#Employee_Email').val() == "") {
        alert("deve ser preenchido o campo de Email para alterar a senha");
        return;
    }

    var randomPassword = '';
    var arrayToRandomic = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];


    for (i = 0; i < 32; i++) {
        randomPassword += arrayToRandomic[Math.ceil(Math.random() * (arrayToRandomic.length - 1))]
    }

    forgotPassword(randomPassword);

    //alert(`sua nova senha é: ${randomPassword}`)
}


function forgotPassword(senha) {

    let params = {
        email: $('#Employee_Email').val(),
        senha
    }

    $.ajax({
        url: '/SendEmail/forgotPassword',
        type: 'POST',
        data: params,
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        }
    })
        .done(function (data) {
            alert(data);
            $('.modalSpinner').modal('hide');
        })
        .fail(function () {
            console.log("error");
            $('.modalSpinner').modal('hide');
        });
}