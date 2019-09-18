
function TextNameProject() {
    let select = document.querySelector('#Hour_Id_Project');
    let option = select.children[select.selectedIndex];
    let texto = option.textContent;
    console.log(texto)

    document.querySelector('#Hour_Project').value = texto;
}


function calculaHora() {

    /*if ($('#Hour_Start_Time').val() == '') {
        alert('Informar horário de inicio');
        return;

    } else if ($('#Hour_Stop_Time').val() == '') {
        alert('Informar horário de inicio de intervalo');
        return;

    } else if ($('#Hour_Start_Time_2').val() == '') {
        alert('Informar horário de retorno de intervalo');
        return;

    } else if ($('#Hour_Stop_Time_2').val() == '') {
        alert('Informar horário de saída')
        return;

    }*/

    var Hour_Arrival_Time = new Date($('#Hour_Date').val() + ' ' + $('#Hour_Arrival_Time').val().replace(':00.000', ''));
    var Hour_Beginning_Of_The_Break = new Date($('#Hour_Date').val() + ' ' + $('#Hour_Beginning_Of_The_Break').val().replace(':00.000', ''));
    var Hour_End_Of_The_Break = new Date($('#Hour_Date').val() + ' ' + $('#Hour_End_Of_The_Break').val().replace(':00.000', ''));
    var Hour_Exit_Time = new Date($('#Hour_Date').val() + ' ' + $('#Hour_Exit_Time').val().replace(':00.000', ''));

    var diff = Hour_Beginning_Of_The_Break.getTime() - Hour_Arrival_Time.getTime();

    Hour_End_Of_The_Break != 'Invalid Date' ? diff += Hour_Exit_Time.getTime() - Hour_End_Of_The_Break.getTime() : '';

    var hours = Math.floor(diff / (1000 * 60 * 60));
    diff -= hours * (1000 * 60 * 60);

    var mins = Math.floor(diff / (1000 * 60));
    diff -= mins * (1000 * 60);

    /*
    if (Hour_Arrival_Time >= Hour_Beginning_Of_The_Break || Hour_Beginning_Of_The_Break >= Hour_End_Of_The_Break || Hour_End_Of_The_Break >= Hour_Exit_Time) {
        return false;
    }*/

    if (hours < 10) {
        hours = '0' + hours
    }

    if (mins < 10) {
        mins = '0' + mins
    }

    $('#Hour_Total_Hours_In_Activity').val(hours + ':' + mins);


}

function SetValuesHours() {
    $('#Hour_Start_Time').val($('#Hour_Arrival_Time').val())
    $('#Hour_Stop_Time').val($('#Hour_Beginning_Of_The_Break').val())
    $('#Hour_Start_Time_2').val($('#Hour_End_Of_The_Break').val())
    $('#Hour_Stop_Time_2').val($('#Hour_Exit_Time').val())
    $('#Hour_Total_Activies_Hours').val($('#Hour_Total_Hours_In_Activity').val())
}


function HourSubmit() {
    $('#id_project').hide();
    $('#hour_date').hide();
    $('#hour_arrival_time').hide();
    $('#hour_beginning_of_the_break').hide();
    $('#hour_end_of_the_break').hide();
    $('#hour_exit_time').hide();
    $('#hour_total_hours_in_activity').hide();
    $('#hour_activies').hide();
    SetValuesHours();

    $('#Hour_Arrival_Time').val() == '' ? $('#Hour_Arrival_Time').val('00:00') : '';
    $('#Hour_Beginning_Of_The_Break').val() == '' ? $('#Hour_Beginning_Of_The_Break').val('00:00') : '';
    $('#Hour_End_Of_The_Break').val() == '' ? $('#Hour_End_Of_The_Break').val('00:00') : '';
    $('#Hour_Exit_Time').val() == '' ? $('#Hour_Exit_Time').val('00:00') : '';

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
    if (($('#Hour_Arrival_Time').val() > $('#Hour_Beginning_Of_The_Break').val() && $('#Hour_Beginning_Of_The_Break').val().replace(':00.000','') != '00:00') && ($('#Hour_Arrival_Time').val() != '00:00' && $('#Hour_Beginning_Of_The_Break').val() != '00:00') && ($('#Hour_Arrival_Time').val() != '' && $('#Hour_Beginning_Of_The_Break').val() != '')) {
        alert('Hour between arrival and beginning of the break invalid because arrival is bigger than beginning of the break')
        return false;
    }

    if ($('#Hour_End_Of_The_Break').val() > $('#Hour_Exit_Time').val() && ($('#Hour_Exit_Time').val().replace(':00.000', '') != '00:00' && $('#Hour_End_Of_The_Break').val() != '00:00') && ($('#Hour_Exit_Time').val() != '' && $('#Hour_End_Of_The_Break').val() != '')) {
        alert('Hour between end of break and exit invalid because end of break is bigger than exit')
        return false;
    }

    if ($('#Hour_Arrival_Time').val().replace(':00.000', '') == '00:00' || $('#Hour_Exit_Time').val().replace(':00.000', '') == '00:00') {
        alert('Arrival time and departure time cannot be reset');
        return false;
    }

    if ($('#Hour_End_Of_The_Break').val().replace(':00.000', '') == '00:00' && $('#Hour_Beginning_Of_The_Break').val().replace(':00.000', '') != '00:00') {
        alert('End of range cannot be 00: 00 or "" e Start of longest interval 0');
        return false;
    }

    if ($('#Hour_End_Of_The_Break').val().replace(':00.000', '') == '00:00' || $('#Hour_Beginning_Of_The_Break').val().replace(':00.000', '') == '00:00') {
        alert('Range start cannot be 00:00 or "" and end of longest range 0');
        return false;
    }*/
    $('#HoursForm').submit();
}

function ActiveLinck() {
    let wlp = window.location.pathname.split('/')[2]

    if (wlp == 'Create') {
        $('.biHourCreate').addClass('active')
    } else {
        $('.biHome').addClass('active')
    }
}

$(document).ready(function () {
    let wlp = window.location.pathname.split('/')[2]
    wlp == 'Create' || wlp == 'Edit' ? $('.text-danger-span').hide() : '';

    SetValuesHours();
});