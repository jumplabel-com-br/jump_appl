function SubmitPricing() {

    if (verificationsOnSubmitDetails() == false) {
        return false;
    }

    $('#DetailsPricing_HourConsultant').val().length == 0 ? $('#DetailsPricing_HourConsultant').val(0) : '';
    $('#DetailsPricing_HourSale').val().length == 0 ? $('#DetailsPricing_HourSale').val(0) : '';
    $('#DetailsPricing_ValueCLTType').val().length == 0 ? $('#DetailsPricing_ValueCLTType').val(0) : '';
    $('#DetailsPricing_VT').val().length == 0 ? $('#DetailsPricing_VT').val(0) : '';
    $('#DetailsPricing_Cust').val().length == 0 ? $('#DetailsPricing_Cust').val(0) : '';
    $('#DetailsPricing_AgeYears').val().length == 0 ? $('#DetailsPricing_AgeYears').val(0) : '';

    var form = $('#DetailsPricingsForm');
    $('#toast-container-saved').toggle();
    $('.modalSpinner').modal('show');
    form.submit();
}

function verificationsOnSubmitDetails() {

    $('#typeContract').hide();
    $('#specialtyName').hide();
    $('#hoursMonth').hide();
    $('#hourConsultant').hide();
    $('#hourSale').hide();
    $('#valueCLTType').hide();
    $('#vt').hide();
    $('#cust').hide();
    $('#ageYears').hide();

    if ($('#DetailsPricing_TypeContract').val().length == 0) {
        $('#typeContract').show();
        return false;
    }
    if ($('#DetailsPricing_SpecialtyName').val().length == 0) {
        $('#specialtyName').show();
        return false;
    }
    if ($('#DetailsPricing_HoursMonth').val().length == 0) {
        $('#hoursMonth').show();
        return false;
    }
    if ($('#DetailsPricing_HourConsultant').val().length == 0 && $('#DetailsPricing_TypeContract').val() == 2) {
        $('#hourConsultant').show();
        return false;
    }
    if ($('#DetailsPricing_HourSale').val().length == 0) {
        $('#hourSale').show();
        return false;
    }
    if ($('#DetailsPricing_ValueCLTType').val().length == 0 && ($('#DetailsPricing_TypeContract').val() == 1 || $('#DetailsPricing_TypeContract').val() == 3)) {
        $('#valueCLTType').show();
        return false;
    }
    /*
    if ($('#DetailsPricing_VT').val().length == 0) {
        $('#vt').show();
        return false;
    }
    if ($('#DetailsPricing_Cust').val().length == 0) {
        $('#cust').show();
        return false;
    }
    */

    if ($('#DetailsPricing_AgeYears').val().length == 0 && ($('#DetailsPricing_TypeContract').val() == 1 || $('#DetailsPricing_TypeContract').val() == 3)) {
        $('#ageYears').show();
        return false;
    }

}

function ageYears() {
    var inputYear = $('.clssAgeYears').val().split('-')

    var years = new Date() - new Date(inputYear[1] + '-' + inputYear[2] + '-' +inputYear[0]);

    years = years / 1000; //transformando millisegundos em segundos
    years = years / 60; // transformando segundos em minutos
    years = years / 60; // transformando  minutos em horas
    years = years / 24; // transformando horas em dias
    years = years / 365;

    $('#DetailsPricing_AgeYears').val(years);
}