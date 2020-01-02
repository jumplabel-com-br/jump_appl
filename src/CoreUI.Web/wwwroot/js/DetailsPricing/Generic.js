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

    if ($('#DetailsPricing_Id').val() ==  "") {
        $('#DetailsPricingsForm').prop('action', '/DetailsPricings/CreateAsync');
        $('#DetailsPricing_Id').remove();
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

function addDetailsPricings() {
    let data = $('#DetailsPricingsForm').serialize()


    $.ajax({
        url: '/DetailsPricings/CreateAsync',
        type: 'POST',
        dataType: 'html',
        data: data,
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function () {
            console.log("success");
            returnDetailsPricings()
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

function returnDetailsPricings() {
    $.ajax({
        url: `/api/DetailsPricingsAPI?pricingId=${returnData}`,
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function (data) {
            console.log("success");
            console.log('data: ', data)
            data.length > 0 ? $('.grid-pricings').html(gridDetailsPricings(data)) : $('.grid-pricings').html('');
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

function gridDetailsPricings(model) {
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
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                    ${model.map(obj => {
                        return `
                      <tr>
                        <td>
                            ${obj.typeContract}
                        </td>
                        <td>
                            ${obj.specialtyName}
                        </td>
                        <td>
                            ${obj.hoursMonth}
                        </td>
                        <td>
                            ${obj.hourConsultant}
                        </td>
                        <td>
                            ${obj.hourSale}
                        </td>
                        <td>
                            ${obj.valueCLTType}
                        </td>
                        <td>
                            ${obj.vt}
                        </td>
                        <td>
                            ${obj.cust}
                        </td>
                        <td>
                            ${obj.ageYears}
                        </td>
                        <td>
                            <button type="button" class="btn btn-ghost-primary" onclick="editDetailsPricings(${obj.id});$('.success-save-details').show();"><i class="fa fa-edit"></i></button>
                            <button type="button" class="btn btn-ghost-danger" onclick="deleteDetailsPricings(${obj.id});"><i class="fa fa-trash-o"></i></button>
                        </td>
                    </tr>
                    `}).join('')}
                 </tbody>
           </table>
    `
}

function deleteDetailsPricings(id) {

    if (confirm('Deseja realmente deletar está informação ?')) {
        $.ajax({
            url: '/DetailsPricings/Delete',
            type: 'GET',
            dataType: 'html',
            data: { id },
            beforeSend: function() {
                $('.modalSpinner').modal('show');
            }
        })
            .done(function () {
                console.log("success");
                $('.modalSpinner').modal('hide');
                returnDetailsPricings();
            })
            .fail(function () {
                console.log("error");
                $('.modalSpinner').modal('hide');
            });

    }
}

function editDetailsPricings(id) {
    $.ajax({
        url: `/api/DetailsPricingsAPI/${id}`,
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function (data) {
            console.log("success");

            console.log('data: ', data)

                let infos = data;

                $('#DetailsPricing_TypeContract').val(infos.typeContract)
                $('#DetailsPricing_Pricing_Id').val(infos.pricing_Id)
                $('#DetailsPricing_SpecialtyName').val(infos.specialtyName)
                $('#DetailsPricing_HoursMonth').val(infos.hoursMonth)
                $('#DetailsPricing_HourConsultant').val(infos.hourConsultant)
                $('#DetailsPricing_HourSale').val(infos.hourSale)
                $('#DetailsPricing_ValueCLTType').val(infos.valueCLTType)
                $('#DetailsPricing_VT').val(infos.vt)
                $('#DetailsPricing_Cust').val(infos.cust)
                $('#DetailsPricing_AgeYears').val(infos.ageYears)

                $('#DetailsPricingsForm').append(`<input type="hidden" name="DetailsPricing.Id" id="DetailsPricing_Id" value=${infos.id} />`);

        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

returnDetailsPricings();

function saveDetailsPricings() {

    let data = $('#DetailsPricingsForm').serialize()

    $.ajax({
        url: '/DetailsPricings/EditAsync',
        type: 'POST',
        dataType: 'html',
        data: data,
        beforeSend: function () {
            $('.modalSpinner').modal('show');
        },
    })
        .done(function () {
            console.log("success");
            returnDetailsPricings();
            $('.success-save-details').hide();
            $('#DetailsPricing_Id').remove();
        })
        .fail(function () {
            console.log("error");
        })
        .always(function () {
            $('.modalSpinner').modal('hide');
        });
}

function ageYears() {
    var inputYear = $('.clssAgeYears').val().split('-')

    var years = new Date() - new Date(inputYear[1] + '-' + inputYear[2] + '-' +inputYear[0]);

    years = years / 1000; //transformando millisegundos em segundos
    years = years / 60; // transformando segundos em minutos
    years = years / 60; // transformando  minutos em horas
    years = years / 24; // transformando horas em dias
    years = years / 365;
    years = parseInt(years);

    $('#DetailsPricing_AgeYears').val(years);
}