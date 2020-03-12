$(document).ready(function () {
    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    function AtualizaComboAno() {
        for (i = 1990; i <= new Date().getFullYear(); i++) {
            arrAno.push({ 'ano': i })
        }

        $('#searchYearDataTable').html(
            `
            ${arrAno.map(obj => {
                return `
            <option ${obj.ano == $('#Year').val() ? 'selected' : ''} value="${obj.ano}">${obj.ano}</option>
            `
            }).join('')}`)
    }

    AtualizaComboAno();
});

setTimeout(function () {
    Projects();
    $('#projects').val($('#Projects').val());
}, 1000)

$('#clients').val($('#Clients').val())
$('#employees').val($('#Employees').val())
$('#searchMothDataTable').val($('#Month').val())
$('#searchYearDataTable').val($('#Year').val())
