$(document).ready(function () {
    $("#searchDataTable").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});


setTimeout(function () {
    Projects();
    $('#projects').val($('#Projects').val());
}, 1000)

$('#accessLevel').val() == "2" ? $('.card-header-actions').remove() : '';
$('#clients').val($('#Clients').val())
$('#manager').val($('#Manager').val())
$('#manager_project').val($('#Manager_Project').val())