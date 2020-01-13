function SubmitPricing() {

    if (verificationsOnSubmitDetails() == false) {
        return false;
    }

    $('#PricingDetails_HourConsultant').val().length == 0 ? $('#PricingDetails_HourConsultant').val(0) : '';
    $('#PricingDetails_HourSale').val().length == 0 ? $('#PricingDetails_HourSale').val(0) : '';
    $('#PricingDetails_ValueCLTType').val().length == 0 ? $('#PricingDetails_ValueCLTType').val(0) : '';
    $('#PricingDetails_VT').val().length == 0 ? $('#PricingDetails_VT').val(0) : '';
    $('#PricingDetails_Cust').val().length == 0 ? $('#PricingDetails_Cust').val(0) : '';
    $('#PricingDetails_AgeYears').val().length == 0 ? $('#PricingDetails_AgeYears').val(0) : '';

    var form = $('#PricingDetailsForm');

    if ($('#PricingDetails_Id').val() == "") {
        $('#PricingDetailsForm').prop('action', '/PricingDetails/CreateAsync');
        $('#PricingDetails_Id').remove();
    }

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

    if ($('#PricingDetails_TypeContract').val().length == 0) {
        $('#typeContract').show();
        return false;
    }
    if ($('#PricingDetails_SpecialtyName').val().length == 0) {
        $('#specialtyName').show();
        return false;
    }
    if ($('#PricingDetails_HoursMonth').val().length == 0) {
        $('#hoursMonth').show();
        return false;
    }
    if ($('#PricingDetails_HourConsultant').val().length == 0 && $('#PricingDetails_TypeContract').val() == 2) {
        $('#hourConsultant').show();
        return false;
    }
    if ($('#PricingDetails_HourSale').val().length == 0) {
        $('#hourSale').show();
        return false;
    }
    if ($('#PricingDetails_ValueCLTType').val().length == 0 && ($('#PricingDetails_TypeContract').val() == 1 || $('#PricingDetails_TypeContract').val() == 3)) {
        $('#valueCLTType').show();
        return false;
    }
    /*
    if ($('#PricingDetails_VT').val().length == 0) {
        $('#vt').show();
        return false;
    }
    if ($('#PricingDetails_Cust').val().length == 0) {
        $('#cust').show();
        return false;
    }
    */

    if ($('#PricingDetails_DateBirth').val().length == 0 && ($('#PricingDetails_TypeContract').val() == 1 || $('#PricingDetails_TypeContract').val() == 3)) {
        $('#ageYears').show();
        return false;
    }

    if ($('#PricingDetails_TypeContract').val() == 2) {
        if ($('#PricingDetails_ValueCLTType').val() != '' && $('#PricingDetails_ValueCLTType').val() != 0) {
            alert('Valor CLT não deve ser preenchido para PJ');
            return false;
        }
        /*
        if ($('#PricingDetails_VT').val() != '' || $('#PricingDetails_VT').val() != 0) {
            alert('O vale transporte não deve ser peeenchido para PJ');
            return false;
        }
        */
    }

}

function addPricingDetails() {

    if (verificationsOnSubmitDetails() == false) {
        return false;
    }

    if ($('#PricingDetails_HourConsultant').val() == '') {
        $('#PricingDetails_HourConsultant').val(0);
    }

    if ($('#PricingDetails_ValueCLTType').val() == '') {
        $('#PricingDetails_ValueCLTType').val(0);
    }

    if ($('#PricingDetails_VT').val() == '') {
        $('#PricingDetails_VT').val(0);
    }

    if ($('#PricingDetails_Cust').val() == '') {
        $('#PricingDetails_Cust').val(0);
    }

    if ($('#PricingDetails_AgeYears').val() == '') {
        $('#PricingDetails_AgeYears').val(0);
    }


    $('#PricingDetails_Id').remove();
    //let data = $('#PricingDetailsForm').serialize().replace(/R%24%20/g, '')
    let TypeContract = $('#PricingDetails_TypeContract').val();
    let SpecialtyName = $('#PricingDetails_SpecialtyName').val();
    let HoursMonth = $('#PricingDetails_HoursMonth').val();
    let HourConsultant = $('#PricingDetails_HourConsultant').val().replace('R$ ', '').replace(/[.]/g, '');
    let HourSale = $('#PricingDetails_HourSale').val().replace('R$ ', '').replace(/[.]/g, '');
    let ValueCLTType = $('#PricingDetails_ValueCLTType').val().replace('R$ ', '').replace(/[.]/g, '');
    let VT = $('#PricingDetails_VT').val().replace('R$ ', '').replace(/[.]/g, '');
    let Cust = $('#PricingDetails_Cust').val().replace('R$ ', '').replace(/[.]/g, '');
    let DateBirth = $('#PricingDetails_DateBirth').val();
    let AgeYears = $('#PricingDetails_AgeYears').val();
    let Pricing_Id = $('#PricingDetails_Pricing_Id').val();
    //let Id = $('#PricingDetails_Id').val();
    let __RequestVerificationToken = $('#PricingDetailsForm  input[name="__RequestVerificationToken"]').val();

    let infos = {
        TypeContract,
        SpecialtyName,
        HoursMonth,
        HourConsultant,
        HourSale,
        ValueCLTType,
        VT,
        Cust,
        DateBirth,
        AgeYears,
        Pricing_Id,
        //Id,
        __RequestVerificationToken
    }

    $.ajax({
        url: '/PricingDetails/CreateAsync',
        type: 'POST',
        dataType: 'html',
        data: infos,
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function () {
            console.log("success");
            returnPricingDetails();
            $('.success-save-details').hide();
            $('.inputs').val('')
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            setTimeout(function () {
                $('.modalSpinner').modal('hide');
                returnPricingDetails();
            }, 1000)
        });
}

function returnPricingDetails() {
    $.ajax({
        url: `/api/PricingDetailsAPI?pricingId=${returnData}`,
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function (data) {
            console.log("success");
            data.length > 0 ? $('.grid-pricings').html(gridPricingDetails(data)) : $('.grid-pricings').html('');
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            setTimeout(function () {
                $('.modalSpinner').modal('hide');
                maskMoney();
            }, 1000)
        });
}

function typeContract(tc) {
    switch (tc) {
        case 1:
            return 'CLT';
            break;
        case 2:
            return 'PJ';
            break;
        case 3:
            return 'CLT Flex';
            break;
        default:
            return ''
    }
}

function gridPricingDetails(model) {
    return `
    <table id="example" class="table table-responsive-sm table-bordered  table-sm">
                <thead>
                  <tr>
                    <th>
                      Contratação
                    </th>
                    <th>
                      Especialidade/Nome
                    </th>
                    <th>
                      Horas mês
                    </th>
                    <th>
                      Horas  consultor
                    </th>
                    <th>
                      Horas Venda
                    </th>
                    <th>
                      Valor CLT
                    </th>
                    <th>
                      VT
                    </th>
                    <th>
                      Custo/Ajuda
                    </th>
                    <th>
                      Idade
                    </th>
                    ${$('#Pricing_Id').val() == undefined ? `
                    <th></th>` : ''}
                  </tr>
                </thead>
                <tbody>
                    ${model.map(obj => {
        return `
                      <tr>
                        <td>
                            ${typeContract(obj.typeContract) == 0 ? '' : typeContract(obj.typeContract)}
                        </td>
                        <td>
                            ${obj.specialtyName}
                        </td>
                        <td>
                            ${obj.hoursMonth == 0 ? '' : obj.hoursMonth}
                        </td>
                        <td>
                            ${maskSpanMoney(obj.hourConsultant) == 0 ? '' : maskSpanMoney(obj.hourConsultant)}
                        </td>
                        <td>
                            ${maskSpanMoney(obj.hourSale) == 0 ? '' : maskSpanMoney(obj.hourSale)}
                        </td>
                        <td>
                            ${maskSpanMoney(obj.valueCLTType) == 0 ? '' : maskSpanMoney(obj.valueCLTType)}
                        </td>
                        <td>
                            ${maskSpanMoney(obj.vt) == 0 ? '' : maskSpanMoney(obj.vt)}
                        </td>
                        <td>
                            ${maskSpanMoney(obj.cust) == 0 ? '' : maskSpanMoney(obj.cust)}
                        </td>
                        <td>
                            ${obj.ageYears == 0 ? '' : obj.ageYears}
                        </td>
                        ${$('#Pricing_Id').val() == undefined ? `
                        <td>
                            <button type="button" class="btn btn-ghost-primary" onclick="editPricingDetails(${obj.id});$('.success-save-details').show();"><i class="fa fa-edit"></i></button>
                            <button type="button" class="btn btn-ghost-danger" onclick="deletePricingDetails(${obj.id});"><i class="fa fa-trash-o"></i></button>
                        </td>`  : ''}
                    </tr>
                    `}).join('')}
                 </tbody>
           </table>
    `
}

function deletePricingDetails(id) {

    if (confirm('Deseja realmente deletar está informação ?')) {
        $.ajax({
            url: '/PricingDetails/Delete',
            type: 'GET',
            dataType: 'html',
            data: { id },
            beforeSend: function () {
                $('.modalSpinner').modal('show');
            }
        })
            .done(function () {
                console.log("success");
                setTimeout(function () { $('.modalSpinner').modal('hide'); }, 1000)
                returnPricingDetails();
            })
            .fail(function () {
                console.log("error");
                setTimeout(function () {
                    $('.modalSpinner').modal('hide');
                    returnPricingDetails();
                }, 1000)
            });

    }
}

function editPricingDetails(id) {
    $('.PricingDetails_Id').remove();
    $('.PricingDetails_Id').remove();

    $.ajax({
        url: `/api/PricingDetailsAPI/${id}`,
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function (data) {
            console.log("success");

            let infos = data;
            dateBirth = infos.dateBirth.split('T')[0] == '1900-01-01' ? '' : infos.dateBirth.split('T')[0];

            $('#PricingDetails_TypeContract').val(infos.typeContract)
            $('#PricingDetails_Pricing_Id').val(infos.pricing_Id)
            $('#PricingDetails_SpecialtyName').val(infos.specialtyName)
            $('#PricingDetails_HoursMonth').val(infos.hoursMonth)
            $('#PricingDetails_HourConsultant').val(infos.hourConsultant)
            $('#PricingDetails_HourSale').val(infos.hourSale)
            $('#PricingDetails_ValueCLTType').val(infos.valueCLTType)
            $('#PricingDetails_VT').val(infos.vt)
            $('#PricingDetails_Cust').val(infos.cust)
            $('#PricingDetails_DateBirth').val(dateBirth);
            $('#PricingDetails_AgeYears').val(infos.ageYears)

            maskSpanMoney(infos.hourConsultant, 'PricingDetails_HourConsultant')
            maskSpanMoney(infos.hourSale, 'PricingDetails_HourSale')
            maskSpanMoney(infos.valueCLTType, 'PricingDetails_ValueCLTType')
            maskSpanMoney(infos.vt, 'PricingDetails_VT')
            maskSpanMoney(infos.cust, 'PricingDetails_Cust')

            $('#PricingDetailsForm').append(`<input type="hidden" class="PricingDetails_Id" name="PricingDetails.Id" id="PricingDetails_Id" value=${infos.id} />`);

        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            setTimeout(function () {
                $('.modalSpinner').modal('hide');
                maskMoney();
                replace0ToEmpty();
            }, 1000)
        });
}

returnPricingDetails();

function savePricingDetails() {

    if (verificationsOnSubmitDetails() == false) {
        return false;
    }

    if ($('#PricingDetails_HourConsultant').val() == '') {
        $('#PricingDetails_HourConsultant').val(0);
    }

    if ($('#PricingDetails_ValueCLTType').val() == '') {
        $('#PricingDetails_ValueCLTType').val(0);
    }

    if ($('#PricingDetails_VT').val() == '') {
        $('#PricingDetails_VT').val(0);
    }

    if ($('#PricingDetails_Cust').val() == '') {
        $('#PricingDetails_Cust').val(0);
    }

    if ($('#PricingDetails_AgeYears').val() == '' || $('#PricingDetails_TypeContract').val() == 2) {
        $('#PricingDetails_AgeYears').val(0);
    }

    //let data = $('#PricingDetailsForm').serialize()//.replace(/R%24%20/g, '')
    let TypeContract = $('#PricingDetails_TypeContract').val();
    let SpecialtyName = $('#PricingDetails_SpecialtyName').val();
    let HoursMonth = $('#PricingDetails_HoursMonth').val();
    let HourConsultant = $('#PricingDetails_HourConsultant').val().replace('R$ ', '').replace(/[.]/g, '');
    let HourSale = $('#PricingDetails_HourSale').val().replace('R$ ', '').replace(/[.]/g, '');
    let ValueCLTType = $('#PricingDetails_ValueCLTType').val().replace('R$ ', '').replace(/[.]/g, '');
    let VT = $('#PricingDetails_VT').val().replace('R$ ', '').replace(/[.]/g, '');
    let Cust = $('#PricingDetails_Cust').val().replace('R$ ', '').replace(/[.]/g, '');
    let DateBirth = $('#PricingDetails_DateBirth').val();
    let AgeYears = $('#PricingDetails_AgeYears').val();
    let Pricing_Id = $('#PricingDetails_Pricing_Id').val();
    let Id = $('#PricingDetails_Id').val();
    let __RequestVerificationToken = $('#PricingDetailsForm  input[name="__RequestVerificationToken"]').val();

    let infos = {
        TypeContract,
        SpecialtyName,
        HoursMonth,
        HourConsultant,
        HourSale,
        ValueCLTType,
        VT,
        Cust,
        DateBirth,
        AgeYears,
        Pricing_Id,
        Id,
        __RequestVerificationToken
    }

    $.ajax({
        url: '/PricingDetails/EditAsync',
        type: 'POST',
        dataType: 'html',
        data: infos,
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function () {
            console.log("success");
            returnPricingDetails();
            $('.success-save-details').hide();
            $('.PricingDetails_Id').remove();
            $('.inputs').val('')

        })
        .fail(function () {
            console.log("error");
            $('.success-save-details').hide();
            $('.PricingDetails_Id').remove();
            $('.inputs').val('')
        })
        .always(function () {
            setTimeout(function () {
                $('.modalSpinner').modal('hide');
                maskMoney();
                returnPricingDetails();
            }, 1000)
        });
}

function ageYears() {
    var inputYear = $('.clssAgeYears').val().split('-')

    var years = new Date() - new Date(inputYear[1] + '-' + inputYear[2] + '-' + inputYear[0]);

    years = years / 1000; //transformando millisegundos em segundos
    years = years / 60; // transformando segundos em minutos
    years = years / 60; // transformando  minutos em horas
    years = years / 24; // transformando horas em dias
    years = years / 365;
    years = parseInt(years);

    $('#PricingDetails_AgeYears').val(years);
}

function replace0ToEmpty() {
    document.querySelectorAll('form input, form select').forEach((obj, item) =>
        $(obj).val() == 0 ? $(obj).val('') : ''
    )
}

$('.money').mask('000.000.000.000.000,00', { reverse: true });