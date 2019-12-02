function ProjectFormSubmit() {

    if ($('#Project_Project_Name').val() == '' || $('#Project_Project_Name').val() == null) {
        alert('Dê um nome ao projeto');
        return false;
    }

    if ($('#Project_Client_Id').val() == '' || $('#Project_Client_Id').val() == null) {
        alert('Selecione um cliente');
        return false;
    }

    if ($('#Project_Active').val() == '' || $('#Project_Active').val() == null) {
        alert('Selecione um cliente');
        return false;
    }

    $('#toast-container-saved').toggle();
    $('#projectForm').submit();
}

$('#accessLevel').val() == "2" ? $('#Project_Project_Manager_Id, #Project_Manager_Id').attr('disabled', true) : '';
$('#accessLevel').val() == "2" ? $('.card-header-actions').remove() : '';