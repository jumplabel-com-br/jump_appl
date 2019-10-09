var arrDates = [];
var arrProjectTeam;

function searchProjectAndEmployee() {
    $.ajax({
        url: '/api/HoursAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);

            arrProjectTeam = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function JsonChecksDatesStartAndEndProjectTeam() {

    let project = $('#Project_team_Project_Id').val();
    let url = '../api/Project_teamAPI/';
    let wlh = window.location.href.split('/')[4]

    switch (wlh) {

        case 'Create':
            url = url;
            break;

        case 'Edit':
            url = '../../api/Project_teamAPI/';
            break;

        default:
            break;
    }

    $.ajax({
        url: url,
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {

            arrDates = [];

            if (data.length > 0) {
                data.forEach(obj => {
                    Id = obj.id;
                    employee_Id = obj.employee_Id;
                    start_Date = obj.start_Date;
                    end_Date = obj.end_Date;

                    arrDates.push({
                        Id,
                        start_Date,
                        end_Date,
                        employee_Id
                    });
                })
            }


        })
        .fail(function () {
            console.log("error");
        });
}



function disabledInput(employee, start, end, projectId) {
    JsonChecksDatesStartAndEndProjectTeam();

    let wlh = window.location.href.split('/');

    if (wlh[3] == 'Project_team') {

        searchProjectAndEmployee();

        let count = arrProjectTeam.filter(obj => obj.employee_Id == employee && obj.date.replace('T00:00:00', '') >= start && obj.date.replace('T00:00:00', '') <= end && obj.id_Project == projectId)

        if (count.length > 0) {
            alert('Este projeto não pode ser editado por ter horas lançadas');
            $('input[name], select').prop('disabled', true)
            return false;
        }

    }
}

disabledInput($('#Project_team_Employee_Id').val(), $('#Project_team_Start_Date').val(), $('#Project_team_End_Date').val(), $('#Project_team_Project_Id').val());

function projectTeamSubmit(start, end) {

    let start_Date = $('#Project_team_Start_Date').val();
    let end_Date = $('#Project_team_End_Date').val();
    let employeeId = arrDates[0].employee_Id;
    let count = arrDates.filter(obj => obj.employee_Id == employeeId && obj.start_Date.replace('T00:00:00', '') >= start_Date && obj.end_Date.replace('T00:00:00', '') == end_Date)

    console.log(count)
    if (count.length > 0) {
        alert('O funcionário ja tem um projeto deste tipo entre estas datas');
        return false;
    }

    if ($('#Project_team_Project_Id').val() == '' || $('#Project_team_Project_Id').val() == null) {
        alert('Preencher o campo de projeto');
        return false;
    }

    if ($('#Project_team_Employee_Id').val() == '' || $('#Project_team_Employee_Id').val() == null) {
        alert('Preencher o campo de funcionário');
        return false;
    }

    if ($('#Project_team_Start_Date').val() > $('#Project_team_End_Date').val()) {
        alert('Data de finalização não pode ser maior que a de início');
        return false;
    }
    $('#toast-container-saved').toggle();
    $('#projectTeamForm').submit();

}