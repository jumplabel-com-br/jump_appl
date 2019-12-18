function FormSubmit(url, modal) {

    if (verificationsOnSubmit() == false) {
        return false;
    }

    var form = $('#PricingsForm').serialize();

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        data: form,
    })
        .done(function (data) {
            console.log("success");
            $('#hiring_Id') != undefined && $('#hiring_Id') != null ?
                $('#hiring_Id').val(data)
            : '';

            Modal(modal, 'GET');
            setTimeout(function () {
                $('#DetailsPricing_Hiring_Id') != undefined && $('#DetailsPricing_Hiring_Id') != null ?
                    $('#DetailsPricing_Hiring_Id').val(data)
                : '';
            }, 1000)
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

function verificationsOnSubmit() {

    $('#pricing_typePricing').hide();
    $('#pricing_client_Id').hide();
    $('#pricing_allocation').hide();
    $('#pricing_accountExecutive').hide();
    $('#pricing_numberProposal').hide();
    $('#pricing_allocationManager_Id').hide();
    $('#pricing_administrator_Id').hide();
    $('#pricing_initialDate').hide();
    $('#pricing_endDate').hide();
    $('#pricing_TimeBetweenInitialAndEndDate').hide();
    $('#pricing_risk').hide();

    if ($('#Pricing_TypePricing').val().length == 0) {
        $('#pricing_typePricing').show();
        return false;
    }

    if ($('#Pricing_Client_Id').val().length == 0) {
        $('#pricing_client_Id').show();
        return false;
    }

    /*
    if ($('#Pricing_Allocation').val().length == 0) {
        $('#pricing_allocation').show();
        return false;
    }
    */

    if ($('#Pricing_AccountExecutive_Id').val().length == 0) {
        $('#pricing_accountExecutive').show();
        return false;
    }

    /*
    if ($('#Pricing_NumberProposal').val().length == 0) {
        $('#pricing_numberProposal').show();
        return false;
    }
    

    if ($('#Pricing_AllocationManager_Id').val().length == 0) {
        $('#pricing_allocationManager_Id').show();
        return false;
    }

    if ($('#Pricing_Administrator_Id').val().length == 0) {
        $('#pricing_administrator_Id').show();
        return false;
    }
    */

    if ($('#Pricing_InitialDate').val().length == 0) {
        $('#pricing_initialDate').show();
        return false;
    }

    if ($('#Pricing_EndDate').val().length == 0) {
        $('#pricing_endDate').show();
        return false;
    }

    /*
    if ($('#Pricing_TimeBetweenInitialAndEndDate').val().length == 0) {
        $('#pricing_TimeBetweenInitialAndEndDate').show();
        return false;
    }

    if ($('#Pricing_Risk').val().length == 0) {
        $('#pricing_risk').show();
        return false;
    }
    */

    if (parseInt($('#Pricing_TimeBetweenInitialAndEndDate').val()) < 0) {
        alert('Data inicial não pode ser mais recente que a data final');
        return false;
    }
}

function betweenDates() {
    var betweenDate = new Date($('#Pricing_EndDate').val()) - new Date($('#Pricing_InitialDate').val());

    betweenDate = betweenDate / 1000; //transformando millisegundos em segundos
    betweenDate = betweenDate / 60; // transformando segundos em minutos
    betweenDate = betweenDate / 60; // transformando  minutos em horas
    betweenDate = betweenDate / 24; // transformando horas em dias

    $('#Pricing_TimeBetweenInitialAndEndDate').val(betweenDate);
}