function SubmitPricing() {
    verificationsOnSubmitDetails();

    var form = $('#DetailsPricingsForm');
    $('.modalSpinner').modal('show');
    form.submit();
}

function verificationsOnSubmitDetails() {
    if ($('#TypeContract').val().length == 0) {
        $('#typeContract').show();
        return false;
    }
    if ($('#SpecialtyName').val().length == 0) {
        $('#specialtyName').show();
        return false;
    }
    if ($('#HoursMonth').val().length == 0) {
        $('#hoursMonth').show();
        return false;
    }
    if ($('#HourConsultant').val().length == 0) {
        $('#hourConsultant').show();
        return false;
    }
    if ($('#HourSale').val().length == 0) {
        $('#hourSale').show();
        return false;
    }
    if ($('#ValueCLTType').val().length == 0) {
        $('#valueCLTType').show();
        return false;
    }
    if ($('#VT').val().length == 0) {
        $('#vt').show();
        return false;
    }
    if ($('#Cust').val().length == 0) {
        $('#cust').show();
        return false;
    }
    if ($('#AgeYears').val().length == 0) {
        $('#ageYears').show();
        return false;
    }
}