function FormSubmit() {
    verificationsOnSubmit();

    var form = $('#PricingsForm').serialize();

    $.ajax({
        url: '/Pricings/CreateAsync',
        type: 'POST',
        dataType: 'html',
        data: form,
    })
        .done(function (data) {
            console.log("success");
            $('#hiring_Id').val(data);

            Modal(`/DetailsPricings/Create`, 'GET');
            setTimeout(function () { $('#Hiring_Id').val(data); }, 1000)
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

function verificationsOnSubmit() {

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

    if ($('#Pricing_AccountExecutive').val().length == 0) {
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
}