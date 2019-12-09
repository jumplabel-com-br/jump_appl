var sum = 0;

$('.table tbody tr').each((item) => {
    sum += parseFloat($(`table tbody tr:eq(${item}) td:eq(7)`).text().trim().replace('R$ ', ''))
});


function checkedAll() {

    document.querySelectorAll('.trCount').forEach((obj, item) => {
        if (document.querySelector('#' + obj.id + ' .checkedItem') != null) {
            document.querySelector('#ids').value += obj.id.replace('tr_', '') + ',';
            document.querySelector('#' + obj.id + ' .checkedItem').checked = true;
        }
    });
}

function notCheckedAll() {

    document.querySelectorAll('.trCount').forEach((obj, item) => {
        id = obj.id.replace('tr_', '');
        document.querySelector('#ids').value = document.querySelector('#ids').value.replace(id + ',', '')
        document.querySelector('#' + obj.id + ' .checkedItem') != null ? document.querySelector('#' + obj.id + ' .checkedItem').checked = false : '';
    });
}

function confirmUpdateStatus() {

    if ($('#statusUpdate').val().length <= 0) {
        $('.toast-container').show();
        $('.toast-message').hide();
        $('.message-error-delete').html('Operação inválida, selecione um status');
        $('.toast-message-cancel').show();
        $('.btn-btn-cancel').css({ marginLeft: "70%" });
        $('.btn-submit-confirm').hide();
        $('#toast-container').toggle();
        return false;
    }

    $('.toast-container').show();
    $('.toast-message').hide();
    $('.message-error-delete').html('Deseja realmente tomar esta ação ?')
    $('.toast-message-cancel').show();
    $('.btn-btn-cancel').css({ marginLeft: "50%" });
    $('.btn-submit-confirm').show();
    $('#toast-container').toggle();
}

function UpdateStatusSubmit() {
    var string = document.querySelector('#ids').value;
    var string1 = string.substr(0, (string.length - 1));
    document.querySelector('#ids').value = string1;

    if (string1.length <= 0) {
        $('.toast-container').show();
        $('.toast-message').hide();
        $('.message-error-delete').html('Operação inválida, check um item da lista');
        $('.toast-message-cancel').show();
        $('.btn-btn-cancel').css({ marginLeft: "70%" });
        $('.btn-submit-confirm').hide();
        $('#toast-container').show();
        return false;
    }

    $('.modalSpinner').modal('show');
    $('#UpdateStatus').submit();

}

$('.notesValues').each(function () {
    this.textContent.length > 2 ? $('.notesValues').mask('000.000.000.000.000,00', { reverse: true }) : '';
    this.textContent.substr(0, 1) == ',' || this.textContent.substr(0, 1) == '.'? this.textContent = this.textContent.replace(/,|./, '') : '';
})

$('#searchMothDataTable').val($('#Month').val());
$('#searchYearDataTable').val($('#Year').val());
$('#status').val($('#Status').val());
$('#clients').val($('#Clients').val());
$('#projects').val($('#Projects').val());
$('#employees').val($('#Employees').val());