function ClientFormSubmit() {

    if ($('#Name').val() == '') {
        alert('Nome Inválido');
        return false;
    }

    $('#toast-container-saved').toggle();
    $('.modalSpinner').modal('show');
    $('#ClientForm').submit();

}