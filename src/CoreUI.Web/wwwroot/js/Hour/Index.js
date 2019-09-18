
$(document).ready(function () {
    $('#Activities').html().trim().length > 30 ? $('#Activities').html($('#Activities').html().trim().substr(0, 20) + ' ...') : '';

    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});