function checkedAll() {

    document.querySelectorAll('.trCount').forEach((obj, item) => {

        if (document.querySelector('#' + obj.id).style.display != 'none') {
            //console.log(document.querySelector('#' + obj.id + ' .checkedItem').checked)
            if (document.querySelector('#' + obj.id + ' .checkedItem').checked) {
                document.querySelector('#' + obj.id + ' .checkedItem').checked = false;
                //console.log(1)
            } else {
                document.querySelector('#ids').value += obj.id.replace('tr_', '') + ','
                document.querySelector('#' + obj.id + ' .checkedItem').checked = true;
                //console.log(document.querySelector('#ids').value += obj.id.replace('tr_', '')+',')
            }
        }
        //document.querySelector('#' + obj.id).style.display != 'none' ? document.querySelector('#' + obj.id + ' .checkedItem').checked = true : document.querySelector('#' + obj.id + ' .checkedItem').checked = false
    });
}

function confirmUpdateStatus() {

    if ($('#status').val().length <= 0) {
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