
$(document).ready(function () {
    $('#toast-container').hide();

    document.querySelectorAll('.Activities').forEach(obj => {
        obj.innerHTML.trim().length > 30 ? obj.innerHTML = obj.innerHTML.trim().substr(0, 20) + '...' : ''
    });


    $("#searchDataTable").on("keyup", function () {
        var value = $("#searchDataTable").val();
        var month = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + new Date().getFullYear() : '';
        $("#tbodyHour tr").filter(function () {
            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1 &&
                $(this).text().toLowerCase().indexOf(month) > -1);
            SumTotalHours();
        });
    });


    function AtualizaComboAno() {
        for (i = new Date().getFullYear(); i >= 1990 && i <= i; i--) {
            arrAno.push({ 'ano': i })
        }

        $('#searchYearDataTable').html(
            `<option value="">Todos</option>
            ${arrAno.map(obj => {
                return `
            <option ${obj.ano == $('#Year').val() ? 'selected' : ''} value="${obj.ano}">${obj.ano}</option>
            `
            }).join('')}`)
    }

    AtualizaComboAno();
    /*
    $('#searchMothDataTable, #searchYearDataTable').on('change', function () {
        var month = $("#searchMothDataTable").val() != "" && $("#searchMothDataTable").val() < 10 ? '0' + $("#searchMothDataTable").val() + '/' + $('#searchYearDataTable').val() : $("#searchMothDataTable").val() + '/' + $('#searchYearDataTable').val();

        if ($('#searchMothDataTable').val().length == 0 && $('#searchYearDataTable').val().length == 0) {
            month = '';
        } else if ($('#searchMothDataTable').val().length == 0 && $('#searchYearDataTable').val().length >= 1) {
            month = '/' + $('#searchYearDataTable').val();
        }

        $('table').DataTable().search(month).draw();
        $('table tbody tr td').text() != "Não foram encontrados resultados" ? SumTotalHours() : $('#TotalOfSumHours').val('');
    });

    var month = new Date().getMonth() + 1 + '/' + new Date().getFullYear();
    $("#searchMothDataTable").val(new Date().getMonth() + 1)
    $('table').DataTable().search(month).draw();
    SumTotalHours();
    */
});

function SumTotalHours() {

    if ($('table tbody tr td').text() == "Não foram encontrados resultados" ) {
        $('#TotalOfSumHours').val('');
        return false;
    }

    let hours = 0;
    let minutes = 0;

    document.querySelectorAll('.totalHours').forEach(obj => {
        if (obj.style.display != 'none') {
            hours += new Date('1999-01-01 ' + obj.textContent.trim()).getHours();
            minutes += new Date('1999-01-01 ' + obj.textContent.trim()).getMinutes();
        }
    });

    for (var i = minutes; minutes >= 60; i++) {
        hours += 1
        minutes -= 60
    }

    hours < 10 ? hours = '0' + hours : '';
    minutes < 10 ? minutes = '0' + minutes : '';

    $('#TotalOfSumHours').val(hours+':'+minutes)

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
    $('.btn-btn-cancel').css({marginLeft : "50%"});
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

SumTotalHours();
$('#searchMothDataTable').val($('#Month').val())
$('#searchYearDataTable').val($('#Year').val())
$('#approval').val($('#Approval').val());
$('#description').val($('#Description').val());
$('#clients').val($('#Clients').val());
$('#projects').val($('#Projects').val());