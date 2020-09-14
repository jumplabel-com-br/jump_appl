var arrDates = [];
var arrHours = [];
var arrProjects = [];
var arrProjectTeam = [];
var arrEmployees = [];

var data;
var dateValid;
var existingDate;
var horasInvalida;
var wlh = window.location.href.split('/')[4];

/*
function FilterProjectPerEmployee() {
    $.ajax({
        url: '/api/ProjectsAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            arrProjects = [];
            arrProjects = data;
            arrProjects = arrProjects.filter((obj, item) => obj.id == arrProjectTeam[item]);


            $('#Hour_Id_Project').html(SelectProject(arrProjects))
        })
        .fail(function () {
            console.log("error");
        });
}

function FilterProjectTeamPerEmployee() {
    $.ajax({
        url: '/api/Project_teamAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            arrProjectTeam = [];

            if (data.length > 0) {
                data.forEach(obj => 
                    arrProjectTeam.push(obj.id)
                    )
            }

            FilterProjectPerEmployee();

        })
        .fail(function () {
            console.log("error");
        });
}*/

function Attachment() {
    $('#hrefAttachment').attr({ href: 'Files/Hour/Attachment' + $("#Hour_Attachment").val() });
    $('#hrefAttachment').html($("#Hour_Attachment").val());
}

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

function Client() {
    ReturnAjax('/api/ProjectsAPI');
    //console.log(data);
    let value = data.filter(obj => obj.id == $('#Hour_Id_Project').val())[0].client_Id;
    $('#Client_Id').val(value)
}

function Project() {
    let arr = [];

    ReturnAjax('/api/Project_teamAPI');
    arrProjectTeam = data;

    ReturnAjax('/api/ProjectsAPI');
    arrProjects = data;

    arr = arrProjectTeam.concat(arrProjects)
    arr = arr.filter(obj => obj.client_Id == $('#Client_Id').val());
    $('#Hour_Id_Project').html(SelectProject(arr))
}

function Employee() {
    let arr = [];
    let count = [];
    var employees = []
    arrProjectTeam;

    ReturnAjax('/api/EmployeesAPI');
    arrEmployees = data;

    arr = arrProjectTeam.concat(arrProjects)
    arr.filter(obj => obj.project_Id == $('#Hour_Id_Project').val())

    for (var i = 0; i < arr.filter(obj => obj.project_Id == $('#Hour_Id_Project').val()).length; i++) {
        count.push(arr.filter(obj => obj.project_Id == $('#Hour_Id_Project').val())[i].employee_Id)
    }

    for (var i = 0; i < count.length; i++) {
        if (arrEmployees.filter(obj => obj.id == count[i]).length > 0) {
            employees.push({ 'id': arrEmployees.filter(obj => obj.id == count[i])[0].id, 'name': arrEmployees.filter(obj => obj.id == count[i])[0].name });
        }

    }

    $('#Hour_Employee_Id').html(SelectEmployee(employees))
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

function TextNameProject(id) {

    let select = document.querySelector('#' + id);
    let option = select.children[select.selectedIndex];
    let texto = option.textContent;
    console.log(texto);

    document.querySelector('#Hour_Project').value = texto;
}

function setValBilling(val) {
    val == '3' ? $('#Hour_Billing').val(0) : $('#Hour_Billing').val(1);
}


function calculaHora() {

    let Hour_Date = $('#Hour_Date').val();
    let Arrival_Time = $('#Hour_Arrival_Time').val().replace(':00.000', '');
    let Beginning_Of_The_Break = $('#Hour_Beginning_Of_The_Break').val().replace(':00.000', '');
    let End_Of_The_Break = $('#Hour_End_Of_The_Break').val().replace(':00.000', '');
    let Exit_Time = $('#Hour_Exit_Time').val().replace(':00.000', '');

    if (Arrival_Time == '') {
        $('#Hour_Arrival_Time').val('00:00:00.000');
    }

    if (Beginning_Of_The_Break == '') {
        $('#Beginning_Of_The_Break').val('00:00:00.000');
    }

    if (End_Of_The_Break == '') {
        $('#End_Of_The_Break').val('00:00:00.000');
    }

    if (Exit_Time == '') {
        $('#Exit_Time').val('00:00:00.000');
    }

    var Hour_Arrival_Time = new Date(Hour_Date + ' ' + Arrival_Time);
    var Hour_Beginning_Of_The_Break = new Date(Hour_Date + ' ' + Beginning_Of_The_Break);
    var Hour_End_Of_The_Break = new Date(Hour_Date + ' ' + End_Of_The_Break);
    var Hour_Exit_Time = new Date(Hour_Date + ' ' + Exit_Time);

    var diff = Hour_Beginning_Of_The_Break.getTime() - Hour_Arrival_Time.getTime();

    Hour_End_Of_The_Break != 'Invalid Date' ? diff += Hour_Exit_Time.getTime() - Hour_End_Of_The_Break.getTime() : '';

    var hours = Math.floor(diff / (1000 * 60 * 60));
    diff -= hours * (1000 * 60 * 60);

    var mins = Math.floor(diff / (1000 * 60));
    diff -= mins * (1000 * 60);

    if (hours < 10) {
        hours = '0' + hours;
    }

    if (mins < 10) {
        mins = '0' + mins;
    }

    $('#Hour_Total_Hours_In_Activity').val(hours + ':' + mins);
}

function SetValuesHours() {
    $('#Hour_Start_Time').val() == '' ? $('#Hour_Start_Time').val('00:00:00.000') : '';
    $('#Hour_Stop_Time').val() == '' ? $('#Hour_Stop_Time').val('00:00:00.000') : '';
    $('#Hour_Start_Time_2').val() == '' ? $('#Hour_Start_Time_2').val('00:00:00.000') : '';
    $('#Hour_Stop_Time_2').val() == '' ? $('#Hour_Stop_Time_2').val('00:00:00.000') : '';

    $('#Hour_Start_Time').val($('#Hour_Arrival_Time').val());
    $('#Hour_Stop_Time').val($('#Hour_Beginning_Of_The_Break').val());
    $('#Hour_Start_Time_2').val($('#Hour_End_Of_The_Break').val());
    $('#Hour_Stop_Time_2').val($('#Hour_Exit_Time').val());
    $('#Hour_Total_Activies_Hours').val($('#Hour_Total_Hours_In_Activity').val());
}

function JsonChecksDatesStartAndEnd() {

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
            console.log(data);

            arrDates = [];
            arrDates = data;
            data = data.filter(obj => obj.project_Id == $('#Hour_Id_Project').val() && obj.employee_Id == employee)

            data.forEach(obj => {
                if ($('#Hour_Date').val() < obj.start_Date.replace('T00:00:00', '') || $('#Hour_Date').val() > obj.end_Date.replace('T00:00:00', '') && obj.employee_Id == employee) {
                    dateValid = false;
                } else {
                    dateValid = true;
                }
            });

            arrDates.filter(obj => obj.employee_Id == $('#Hour_Employee_Id').val() && obj.project_Id == $('#Hour_Id_Project').val()).length == 0 ? horasInvalida = false : horasInvalida = true;
        })
        .fail(function () {
            console.log("error");
        });
}

function searchProjectsPerEmployee() {
    $.ajax({
        url: '../../api/HoursAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: { defaultVerification : "0"},
    })
        .done(function (data) {
            console.log(data);
            arrHours = data;

            if (data.filter(obj =>
                obj.employee_Id == $('#Hour_Employee_Id').val() &&
                $('#Hour_Id').val() != obj.id &&
                (
                    $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                    $('#Hour_Arrival_Time').val().replace('.000', '') >= obj.arrival_Time.split('T')[1] &&
                    $('#Hour_Exit_Time').val().replace('.000', '') <= obj.exit_Time.split('T')[1]
                ) ||
                $('#Hour_Id').val() != obj.id &&
                $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                $('#Hour_Arrival_Time').val().replace('.000', '') <= obj.exit_Time.split('T')[1] &&
                $('#Hour_Arrival_Time').val().replace('.000', '') >= obj.start_Time.split('T')[1] &&
                obj.employee_Id == $('#Hour_Employee_Id').val()).length > 0)
            {
                return existingDate = false;
            } //?  : existingDate = true;

            if (data.filter(obj =>
                obj.id != $('#Hour_Id').val() &&
                obj.employee_Id == $('#Hour_Employee_Id').val() &&
                $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                $('#Hour_Arrival_Time').val().replace('.000', '') > obj.arrival_Time.replace(':00', '').split('T')[1] &&
                $('#Hour_Exit_Time').val().replace('.000', '') < obj.exit_Time.replace(':00', '').split('T')[1]
            ).length > 0

                ||

                data.filter(obj =>
                    obj.id != $('#Hour_Id').val() &&
                    obj.employee_Id == $('#Hour_Employee_Id').val() &&
                    $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                    $('#Hour_Arrival_Time').val().replace('.000', '') < obj.arrival_Time.replace(':00', '').split('T')[1] &&
                    $('#Hour_Exit_Time').val().replace('.000', '') < obj.exit_Time.replace(':00', '').split('T')[1]
                    && $('#Hour_Exit_Time').val().replace('.000', '') > obj.arrival_Time.replace(':00', '').split('T')[1]
                ).length > 0

                ||

                data.filter(obj =>
                    obj.id != $('#Hour_Id').val() &&
                    obj.employee_Id == $('#Hour_Employee_Id').val() &&
                    $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                    $('#Hour_Arrival_Time').val().replace('.000', '') < obj.exit_Time.replace(':00', '').split('T')[1]
                    && $('#Hour_Exit_Time').val().replace('.000', '') > obj.arrival_Time.replace(':00', '').split('T')[1]
                ).length > 0

                ||

                data.filter(obj =>
                    obj.id != $('#Hour_Id').val() &&
                    obj.employee_Id == $('#Hour_Employee_Id').val() &&
                    $('#Hour_Date').val() == obj.date.replace('T00:00:00', '') &&
                    $('#Hour_Arrival_Time').val().replace('.000', '') == obj.start_Time.replace(':00', '') &&
                    $('#Hour_Exit_Time').val().replace('.000', '') == obj.stop_Time_2.replace(':00', '') &&
                    $('#Hour_Description').val() == '3'

                ).length > 0) {

                return existingDate = false;

            } else {
                return existingDate = true;
            }
            

        })
        .fail(function () {
            console.log("error");
        });
}

function CopySubmit() {
    $('#HoursForm').attr("action", "/Hours/Create");
    $('#HoursForm #Hour_Id').remove();

    HourSubmit();
}

function sizeFile(size) {
    var i = Math.floor(Math.log(size) / Math.log(1024));
    return (size / Math.pow(1024, i)).toFixed(2) * 1 + ' ' + ['B', 'kB', 'MB', 'GB', 'TB'][i];
}

$('.btn-btn-nao-modal').on('click', function () {
    $('#toast-container-modal').hide();
    //$('.modalIndex').modal('hide');
    //$('#HoursForm').submit();
});

function HourSubmit() {

    JsonChecksDatesStartAndEnd();
    searchProjectsPerEmployee();

    let Hour_Date = $('#Hour_Date').val();
    let Arrival_Time = $('#Hour_Arrival_Time').val().replace(':00.000', '');
    let Beginning_Of_The_Break = $('#Hour_Beginning_Of_The_Break').val().replace(':00.000', '');
    let End_Of_The_Break = $('#Hour_End_Of_The_Break').val().replace(':00.000', '');
    let Exit_Time = $('#Hour_Exit_Time').val().replace(':00.000', '');

    $('#id_project').hide();
    $('#hour_date').hide();
    $('#hour_arrival_time').hide();
    $('#hour_beginning_of_the_break').hide();
    $('#hour_end_of_the_break').hide();
    $('#hour_exit_time').hide();
    $('#hour_total_hours_in_activity').hide();
    $('#hour_activies').hide();
    SetValuesHours();

    if ($('#Hour_Id_Project').val() == '') {
        $('#id_project').show();
        $('#Hour_Id_Project').focus();
        return false;
    }

    if ($('#Hour_Date').val() == '') {
        $('#hour_date').show();
        $('#Hour_Date').focus();
        return false;
    }

    if ($('#Hour_Arrival_Time').val() == ''/* || $('#Hour_Arrival_Time').val().replace(':00.000', '') == '00:00'*/) {
        $('#hour_arrival_time').show();
        $('#Hour_Arrival_Time').focus();
        return false;
    }

    if ($('#Hour_Beginning_Of_The_Break').val() == '') {
        $('#hour_beginning_of_the_break').show();
        $('#Hour_Beginning_Of_The_Break').focus();
        return false;
    }

    if ($('#Hour_End_Of_The_Break').val() == '') {
        $('#hour_end_of_the_break').show();
        $('#Hour_End_Of_The_Break').focus();
        return false;
    }

    if ($('#Hour_Exit_Time').val() == '' || $('#Hour_Exit_Time').val().replace(':00.000', '') == '00:00' && $('#Hour_Description').val() != '3') {
        $('#hour_exit_time').show();
        $('#Hour_Exit_Time').focus();
        return false;
    }

    if ($('#Hour_Activies').val() == '') {
        $('#hour_activies').show();
        $('#Hour_Activies').focus();
        return false;
    }

    /*
    if (new Date($('#Hour_Date').val()) > new Date()) {
        alert('O campo de data não pode ser maior que o dia de hoje');
        return false;
    }
    */

    if (Arrival_Time > Beginning_Of_The_Break && Beginning_Of_The_Break != '00:00' && Beginning_Of_The_Break != '') {
        alert('Hora entre a chegada e o início do intervalo inválida porque a chegada é maior que o início do intervalo');
        return false;
    }

    if (Exit_Time < End_Of_The_Break && End_Of_The_Break != '00:00' && End_Of_The_Break != '') {
        alert('Hora entre a saída e o final do intervalo inválida porque o final do intervalo é maior que a saída');
        return false;
    }

    if (Beginning_Of_The_Break != '00:00' && Beginning_Of_The_Break != '' && (End_Of_The_Break == '00:00' || End_Of_The_Break == '')) {
        alert('Se você preencheu o início do intervalo, também deve preencher o final do intervalo');
        return false;
    }

    if ((Beginning_Of_The_Break == '00:00' || Beginning_Of_The_Break == '') && End_Of_The_Break != '00:00' && End_Of_The_Break != '') {
        alert('Se você preencheu o final do intervalo, também deve preencher o início do intervalo');
        return false;
    }

    if (Beginning_Of_The_Break > End_Of_The_Break) {
        alert('Se você preencheu o final do intervalo, também deve preencher o início do intervalo');
        return false;
    }

    if (Arrival_Time > Exit_Time) {
        alert('A chegada não pode ser maior que a saída');
        return false;
    }


    if (dateValid == false) {
        alert('O dia lançado para este projedo é inválido, pois a data não é compativel com o tempo do projeto');
        return false;
    }

    if (existingDate == false && $('#TypeReleases').val() == '10') {
        alert('Já existe horas lançadas para este dia entre estes horários');
        return false;
    }

    if (horasInvalida == false && accessLevel != 3) {
        alert('Não há este tipo de projeto para você');
        return false;
    }

    $('#Hour_File').val('Sem Documento');

    if ($('#Attachment').prop("files")[0] != undefined && sizeFile($('#Attachment').prop("files")[0].size).substr(sizeFile($('#Attachment').prop("files")[0].size).length - 2).toUpperCase() != "MB" && sizeFile($('#Attachment').prop("files")[0].size).substr(sizeFile($('#Attachment').prop("files")[0].size).length - 2).toUpperCase() != "KB" && sizeFile($('#Attachment').prop("files")[0].size).substr(sizeFile($('#Attachment').prop("files")[0].size).length - 4).replace(/ kB| mB| MB| KB/g, '') > 2)
    {
        alert("O tamanho do anexo excede 2 MB");
        return false;
    }

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
            document.name.substr(document.name.length - 4).toLowerCase() != 'jpeg' &&
            document.name.substr(document.name.length - 3).toLowerCase() != 'png') {
            alert('O tipo de documento só pode ser PNG, JPG ou PDF');
            return false;
        }

        value = document.name.replace(/[ |&|$|#|@|%|*]/g, '');
        $('#Hour_File').val(value);
    }

    if ($('#Hour_Description').val() == '3' && $('#Document').val().length == 0 && wlh != "Edit") {
        alert('Anexo de documento é obrigatório');
        return false;
    }

    if ($('#Hour_Description').val() == '') {
        alert('A descrição de dia útil/ férias/ licença é obrigatório');
        return false;
    }

    $('.modalSpinner').modal('show');
    $('#toast-container-saved').toggle();
    //$('#toast-container-modal').show();
    $('#HoursForm').submit();
}

function ActiveLinck() {
    let wlp = window.location.pathname.split('/')[2];

    if (wlp == 'Create') {
        $('.biHourCreate').addClass('active');
    } else {
        $('.biHome').addClass('active');
    }

    $('.active').css("color", 'blue');
}

$(document).ready(function () {

    $('#Hour_Description').val() == 3 ? $('.divDocument').show() : $('.divDocument').hide()


    let wlp = window.location.pathname.split('/');
    wlp[2] == 'Create' || wlp[2] == 'Edit' ? $('.text-danger-span').hide() : '';
    (wlp[2] == 'Edit' || wlp[2] == 'Details') && (wlp[1] == 'ModeAdmin') ? Client() : '';

    SetValuesHours();

    if (window.matchMedia("(max-width: 300px)").matches) {
        $('.col-2').addClass('col-8');
        $('.col-2').addClass('mt-3');
        $('.col-8').removeClass('col-2');
    }
});