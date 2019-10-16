function changePassword() {
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

    $('#UpdatePassword').submit();
}