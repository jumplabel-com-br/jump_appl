
$(document).ready(function () {
    $('#toast-container').hide();

    document.querySelectorAll('.Activities').forEach(obj => {
        obj.innerHTML.trim().length > 30 ? obj.innerHTML = obj.innerHTML.trim().substr(0, 20) + '...' : ''
    });

    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            SumTotalHours();
        });
    });

    $("#searchMothDataTable").on("change", function () {
        var value = $(this).val() != '' ? $(this).val() + '/' + new Date().getFullYear() : '';
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            SumTotalHours();
        });
    });

    $('#searchMothDataTable').val(new Date().getMonth()+1);

    let value = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + new Date().getFullYear() : '';
    $("#tbodyHour tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        SumTotalHours();
    });

    SumTotalHours();
});

function SumTotalHours() {

    let hours = 0;
    let minutes = 0;


    document.querySelectorAll("#tbodyHour tr").forEach((teste, item) => {
        if (teste.style.display == 'none') {
            document.querySelectorAll('#tbodyHour tr .totalHours')[item].style.display = 'none'
        } else {
            document.querySelectorAll('#tbodyHour tr .totalHours')[item].style.display = ''
        }
    }); 

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