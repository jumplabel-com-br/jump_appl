function OutlaysSubmit() {
    $('#id_client').hide();
    $('#id_project').hide();
    $('#outlays_date').hide();
    $('#outlays_noteNumber').hide();
    $('#outlays_noteValue').hide();
    $('#outlays_file').hide();
    $('#outlays_description').hide();

    if ($('#Outlays_Client_Id').val().length == 0) {
        $('#id_client').show();
        $('#Outlays_Client_Id').focus();
        return false;
    }

    if ($('#Outlays_Project_Id').val().length == 0) {
        $('#id_project').show();
        $('#Outlays_Project_Id').focus();
        return false;
    }

    if ($('#Outlays_Date').val().length == 0) {
        $('#outlays_date').show();
        $('#Outlays_Date').focus();
        return false;
    }

    if ($('#Outlays_NoteNumber').val().length == 0) {
        $('#outlays_noteNumber').show();
        $('#Outlays_NoteNumber').focus();
        return false;
    }

    if ($('#Outlays_NoteValue').val().length == 0) {
        $('#outlays_noteValue').show();
        $('#Outlays_NoteValue').focus();
        return false;
    }

    if ($('#Document').val().length == 0) {
        $('#outlays_file').show();
        $('#Document').focus();
        return false;
    }

    if ($('#Outlays_Description').val().length == 0) {
        $('#outlays_description').show();
        $('#Outlays_Description').focus();
        return false;
    }

    $('#Outlays_File').val($('#Document').val().split('\\')[2]);

    $('#toast-container-saved').toggle();
    $('#OutlaysForm').submit();
}

$(document).ready(function () {
    $('#Outlays_NoteValue').mask('000.000.000.000.000,00', { reverse: true });
});