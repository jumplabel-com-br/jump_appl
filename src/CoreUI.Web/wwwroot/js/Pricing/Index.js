$('.closeModalIndex').on('click', function () {

    if ($('#Pricing_Id').val() == undefined && $('#DetailsPricingsForm').length  == 1) {
        SubmitPricing();
    } else if ($('#Pricing_Id').val() == undefined && $('#Pricing_TypePricing').val() !=  undefined ) {
        alert('Preencha as duas etapas do formulário ou clique em voltar!');
        return false;
    }
});