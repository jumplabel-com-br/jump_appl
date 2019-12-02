$(document).ready(function () {
    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$('#accessLevel').val() == "2" ? $('.card-header-actions').remove() : '';
