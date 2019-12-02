var arrDates = [];
var arrProjectTeam;
var arrClients;
var arrHours;
var arrFilterClients;
var wlh = window.location.href.split('/')[4]

function FilterProjectPerClient() {
    $.ajax({
        url: '/api/ProjectsAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        }
    })
        .done(function (data) {
            //console.log(data);
            $('#listClients').val().length > 0 ? arrFilterClients = data.filter(obj => obj.client_Id == $('#listClients').val()) : arrFilterClients = data;
            $('#Project_team_Project_Id').html(listProjects(arrFilterClients))
            $('#Project_team_Project_Id').prop('disabled', false);
            setTimeout(function () {
                $('.modalSpinner').modal('hide');
            },1000);
        })
        .fail(function () {
            console.log("error");
        });
}

function listProjects(model) {
    return `
    <option value="">Selecione...</option>
    ${model.map(obj => {
        return `
            <option value="${obj.id}">${obj.project_Name}</option>
        `
    })}
    `
}

function searchHours() {
    $.ajax({
        url: '/api/HoursAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);

            arrHours = data;
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
                    let Id = obj.id;
                    let employee_Id = obj.employee_Id;
                    let start_Date = obj.start_Date;
                    let end_Date = obj.end_Date;

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

//disabledInput($('#Project_team_Employee_Id').val(), $('#Project_team_Start_Date').val(), $('#Project_team_End_Date').val(), $('#Project_team_Project_Id').val());

function filterClient() {
    $.ajax({
        url: '/api/ProjectsAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);

            arrClients = data;
            arrClients = arrClients.filter(obj => obj.id == $('#Project_team_Project_Id').val());
            arrClients.length > 0 ? $('#listClients').val(arrClients[0].client_Id) : '';

        })
        .fail(function () {
            console.log("error");
        });
}


function projectTeamSubmit(start, end) {
    JsonChecksDatesStartAndEndProjectTeam();
    

    let employeeId = employee;
    let start_Date = $('#Project_team_Start_Date').val();
    let end_Date = $('#Project_team_End_Date').val();
    let count = arrDates.filter(obj => obj.employee_Id == employeeId && obj.start_Date.replace('T00:00:00', '') >= start_Date && obj.end_Date.replace('T00:00:00', '') == end_Date && obj.Id == $('#Project_team_Id').val())

    if (start_Date == '0000-00-00') {
        alert('Formtato da data de inicio é inválido')
        return false;
    }

    if (end_Date == '0000-00-00') {
        alert('Formtato da data de fim é inválido')
        return false;
    }

    if ($('#Project_team_Start_Date').val() > hoursStart) {
        alert('A alteração não pode ser feita, pois há horas deste funcionário com a data maior que a data de início');
        return false;
    }

    if ($('#Project_team_End_Date').val() < hoursEnd) {
        alert('A alteração não pode ser feita, pois há horas deste funcionário com a data maior que a data de fim');
        return false;
    }
    
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

if (wlh == 'Edit') {
    searchHours();
    let hours = arrHours.filter(obj => obj.employee_Id == $('#Project_team_Employee_Id').val() && obj.date.replace('T00:00:00', '') >= $('#Project_team_Start_Date').val() && obj.date.replace('T00:00:00', '') <= $('#Project_team_End_Date').val() && obj.id_Project == $('#Project_team_Project_Id').val())
    var hoursStart = hours.length > 0 ? hours[0].date.replace('T00:00:00', '') : '';
    var hoursEnd = hours.length > 0 ? hours[hours.length - 1].date.replace('T00:00:00', '') : '';
    //filterClient();
}

