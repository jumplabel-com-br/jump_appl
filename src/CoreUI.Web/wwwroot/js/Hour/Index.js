
$(document).ready(function () {
    $('#toast-container').hide();

    $('#Activities').html().trim().length > 30 ? $('#Activities').html($('#Activities').html().trim().substr(0, 20) + ' ...') : '';

    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

function fn_showMessageDelete(id) {
    $('#IdDelete').val(id);
    $('#toast-container').toggle();
}