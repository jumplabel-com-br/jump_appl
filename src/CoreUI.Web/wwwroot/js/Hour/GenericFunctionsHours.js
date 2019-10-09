var arrDates = [];
var dateValid;

function TextNameProject(id) {

    let select = document.querySelector('#' + id);
    let option = select.children[select.selectedIndex];
    let texto = option.textContent;
    console.log(texto);

    document.querySelector('#Hour_Project').value = texto;
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
            data = data.filter(obj => obj.project_Id == 3 && obj.employee_Id == 1)

            data.forEach(obj => {
                if ($('#Hour_Date').val() < obj.start_Date || $('#Hour_Date').val() > obj.end_Date) {
                    dateValid = false;
                    return false;
                } else {
                    dateValid = true;
                    return true;
                }
            });
        })
        .fail(function () {
            console.log("error");
        });
}

function HourSubmit() {

    JsonChecksDatesStartAndEnd();

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

    if ($('#Hour_Arrival_Time').val() == '' || $('#Hour_Arrival_Time').val().replace(':00.000', '') == '00:00') {
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

    if ($('#Hour_Exit_Time').val() == '' || $('#Hour_Exit_Time').val().replace(':00.000', '') == '00:00') {
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


    if (!dateValid) {
        alert('O dia lançado para este projedo é inválido, pois a data não é compativel com o tempo do projeto');
        return false;
    }

    $('.modalSpinner').modal('show');
    $('#toast-container-saved').toggle();

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
    let wlp = window.location.pathname.split('/')[2];
    wlp == 'Create' || wlp == 'Edit' ? $('.text-danger-span').hide() : '';

    SetValuesHours();

    if (window.matchMedia("(max-width: 300px)").matches) {
        $('.col-2').addClass('col-8');
        $('.col-2').addClass('mt-3');
        $('.col-8').removeClass('col-2');
    }
});