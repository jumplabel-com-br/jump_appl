﻿function ProjectFormSubmit() {

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