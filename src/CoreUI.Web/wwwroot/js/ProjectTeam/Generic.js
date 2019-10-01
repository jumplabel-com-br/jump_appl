function projectTeamSubmit() {

    if ($('#Project_team_Start_Date').val() > $('#Project_team_End_Date').val()) {
        alert('Data de finalização não pode ser maior que a de início');
        return false;
    }

    $('#toast-container-saved').toggle();
    $('#projectTeamForm').submit();
}