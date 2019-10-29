var wlhs = window.location.href.split('/');
var data;
var arrProjectTeam = [];
var arrProjects = [];
var arrEmployees = [];

function ReturnAjax(url) {
    $.ajax({
        url: url,
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (datas) {
            //console.log("success");
            data = datas;
        })
        .fail(function () {
            console.log("error");
        });
}

function Clients() {
    ReturnAjax('/api/ProjectsAPI');
    //console.log(data);
    let value = data.filter(obj => obj.id == $('#Outlays_Project_Id').val())[0].client_Id;
    $('#Outlays_Client_Id').val(value)
}

function Projects() {
    let arr = [];

    ReturnAjax('/api/Project_teamAPI');
    arrProjectTeam = data;

    ReturnAjax('/api/ProjectsAPI');
    arrProjects = data;

    arr = arrProjectTeam.concat(arrProjects)
    arr = arr.filter(obj => obj.client_Id == $('#Outlays_Client_Id').val());
    $('#Outlays_Project_Id').html(SelectProject(arr))
}

function Employees() {
    let arr = [];
    let count = [];
    var employees = []
    arrProjectTeam;

    ReturnAjax('/api/EmployeesAPI');
    arrEmployees = data;

    arr = arrProjectTeam.concat(arrProjects)
    arr.filter(obj => obj.project_Id == $('#Outlays_Project_Id').val())

    for (var i = 0; i < arr.filter(obj => obj.project_Id == $('#Outlays_Project_Id').val()).length; i++) {
        count.push(arr.filter(obj => obj.project_Id == $('#Outlays_Project_Id').val())[i].employee_Id)
    }

    for (var i = 0; i < count.length; i++) {
        if (arrEmployees.filter(obj => obj.id == count[i]).length > 0) {
            employees.push({ 'id': arrEmployees.filter(obj => obj.id == count[i])[0].id, 'name': arrEmployees.filter(obj => obj.id == count[i])[0].name });
        }
        
    }

    $('#Outlays_Employee_Id').html(SelectEmployee(employees))
}



function SelectProject(model) {

    if (model.length == 0) {
        return `<option value="">Sem projeto para este cliente</option>`
    }
    else {
        return `
        <option value="">Selecione o projeto</option>
        ${model.map(obj => {
            return `
            <option value="${obj.id}"> ${obj.project_Name} </option>
        `}).join('')}
        `
    }
}

function SelectEmployee(model) {
    if (model.length == 0) {
        return `<option value="">Sem funcionário para este projeto</option>`
    }
    else {
        return `
        <option value="">Selecione o funcionário</option>
        ${model.map(obj => {
            return `
            <option value="${obj.id}"> ${obj.name} </option>
        `}).join('')}
        `
    }
}

function sizeFile(size) {
    var i = Math.floor(Math.log(size) / Math.log(1024));
    return (size / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
}

function OutlaysSubmit() {
    $('#id_client').hide();
    $('#id_project').hide();
    $('#outlays_date').hide();
    $('#outlays_noteNumber').hide();
    $('#outlays_noteValue').hide();
    $('#outlays_file').hide();
    $('#outlays_description').hide();

    if ($('#Outlays_Client_Id').val().length == 0 && wlhs[3] == 'OutlaysAdmin') {
        $('#id_client').show();
        $('#Outlays_Client_Id').focus();
        return false;
    }

    if ($('#Outlays_Project_Id').val().length == 0) {
        $('#id_project').show();
        $('#Outlays_Project_Id').focus();
        return false;
    }

    if ($('#Outlays_Employee_Id').val().length == 0 && wlhs[3] == 'OutlaysAdmin') {
        $('#id_employee').show();
        $('#Outlays_Employee_Id').focus();
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

    if ($('#Document').val().length == 0 && wlhs[4] != 'Edit') {
        $('#outlays_file').show();
        $('#Document').focus();
        return false;
    }

    if ($('#Outlays_Description').val().length == 0) {
        $('#outlays_description').show();
        $('#Outlays_Description').focus();
        return false;
    }

    $('#Outlays_File').val('Sem Document');

    if ($('#Document').val().length > 0) {
        let document = $('#Document').prop("files")[0]

        if (sizeFile($('#Document').prop("files")[0].size).substr(sizeFile($('#Document').prop("files")[0].size).length - 2) != "kB" && sizeFile($('#Document').prop("files")[0].size).substr(sizeFile($('#Document').prop("files")[0].size).length - 2) != "MB") {
            alert('O arquivo não pode ser maior que 1 MB')
        }

        if (document != undefined && sizeFile($('#Document').prop("files")[0].size) != "1 MB" && sizeFile($('#Document').prop("files")[0].size).substr(sizeFile($('#Document').prop("files")[0].size).length - 2) != "kB" && sizeFile($('#Document').prop("files")[0].size).substr(sizeFile($('#Document').prop("files")[0].size).length - 2) != "MB") {
            alert('O tamanho do arquivo deve ser de no máximo 1MB');
            return false;
        }

        if (
            document != undefined &&
            document.name.substr(document.name.length - 3).toLowerCase() != 'pdf' &&
            document.name.substr(document.name.length - 3).toLowerCase() != 'jpg' &&
            document.name.substr(document.name.length - 3).toLowerCase() != 'jpeg' &&
            document.name.substr(document.name.length - 3).toLowerCase() != 'png') {
            alert('O tipo de documento só pode ser PNG, JPG ou PDF');
            return false;
        }

        value = document.name.replace(/[ |&|$|#|@|%|*]/g, '');
        $('#Outlays_File').val(value);
    }

   

    $('#toast-container-saved').toggle();
    $('#OutlaysForm').submit();
}

$(document).ready(function () {
    $('#Outlays_NoteValue').mask('000.000.000.000.000,00', { reverse: true });
});