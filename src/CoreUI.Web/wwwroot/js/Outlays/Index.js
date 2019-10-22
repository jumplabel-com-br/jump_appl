var arrAno = [];

function AtualizaComboAno() {
    for (i = 1990; i <= new Date().getFullYear(); i++) {
        arrAno.push({ 'ano': i })
    }

    $('#searchYearDataTable').html(
        `<option value="">Todos</option>
            ${arrAno.map(obj => {
            return `
            <option value="${obj.ano}">${obj.ano}</option>
            `
        }).join('')}`)
}

AtualizaComboAno();

$('#searchMothDataTable, #searchYearDataTable').on('change', function () {
    var month = $("#searchMothDataTable").val();
    var year = $('#searchYearDataTable').val();
    console.log(month + '/' + year);

    $("#tbodyHour tr").filter(function () {
        $(this).toggle(
            $(this).text().toLowerCase().indexOf(month + '/' + year) > -1);
    });
});