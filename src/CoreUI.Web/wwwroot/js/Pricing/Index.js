/*$('.closeModalIndex').on('click', function () {

    if ($('#Pricing_Id').val() == undefined && $('#DetailsPricingsForm').length  == 1) {
        SubmitPricing();
    } else if ($('#Pricing_Id').val() == undefined && $('#Pricing_TypePricing').val() !=  undefined ) {
        alert('Preencha as duas etapas do formulário ou clique em voltar!');
        return false;
    }
});*/

var dtInicialInput = $('#dtInicialInput').val().split('/');
var dtFinalInput = $('#dtFinalInput').val().split('/');
var dtInicialInputIndices = '';
var dtFinalInputIndices = '';

dtInicialInputIndices = dtInicialInputIndices.concat(dtInicialInput[2], '-', dtInicialInput[0] < 10 ? '0' + dtInicialInput[0] : dtInicialInput[0], '-', dtInicialInput[1] < 10 ? '0' + dtInicialInput[1] : dtInicialInput[1]);
dtFinalInputIndices = dtFinalInputIndices.concat(dtFinalInput[2],'-',dtFinalInput[0] < 10 ? '0' + dtFinalInput[0] : dtFinalInput[0],'-',dtFinalInput[1] < 10 ? '0' + dtFinalInput[1] : dtFinalInput[1]);

$('#tipoPrecificacaoInput').val() != '' ? $('#tipoPrecificacao').val($('#tipoPrecificacaoInput').val()) : '';
$('#clientsInput').val() != '' ? $('#clients').val($('#clientsInput').val()) : '';
$('#executivoContaInput').val() != '' ? $('#executivoConta').val($('#executivoContaInput').val()) : '';
$('#alocacaoGerenteInput').val() != '' ? $('#alocacaoGerente').val($('#alocacaoGerenteInput').val()) : '';
$('#responsavelInput').val() != '' ? $('#responsavel').val($('#responsavelInput').val()) : '';
$('#dtInicialInput').val() != '' ? $('#dtInicial').val(dtInicialInputIndices) : '';
$('#dtFinalInput').val() != '' ? $('#dtFinal').val(dtFinalInputIndices) : '';

$('.closeModalIndex').on('click', function () {
    window.location.reload();
    $('.modalSpinner').modal('show');
});