var arrEmployees = [];

function AjaxEmployee() {
    $.ajax({
        url: '/api/EmployeesAPI',
        type: 'GET',
        dataType: 'json',
        data: { },
    })
        .done(function (data) {
            console.log("success");;
            arrEmployees = data;

        })
        .fail(function () {
            console.log("error");
        });

}

function changePassword() {

    if ($('#currentPassword').val().length == 0) {
        alert('Preencha o campo de senha atual');
        $('#currentPassword').focus();
        return false;
    }


    if ($('#password').val().length == 0) {
        alert('Preencha o campo de senha');
        $('#password').focus();
        return false;
    }

    if ($('#repeatPassword').val().length == 0) {
        alert('Preencha o campo de confirmação de senha');
        $('#repeatPassword').focus();
        return false;
    }

    if ($('#password').val() != $('#repeatPassword').val()) {
        alert('As senhas não idênticas');
        $('#password').focus();
        return false;
    }


    if (arrEmployees.filter(obj => obj.email == $('#email').val() && obj.password == $('#currentPassword').val()).length == 0) {
        alert('Sua senha atual está incorreta');
        $('#currentPassword').focus();
        return false;
    }

    alert('Senha alterada com sucesso');
    $('#UpdatePassword').submit();
}

jQuery(document).ready(function ($) {
    AjaxEmployee();
});