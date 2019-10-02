function EmployeeSubmit() {

    $('#employee_salary').hide();
    $('#employee_appointment').hide();

    if ($('#Salary').val() == '') {
        $('#Salary').focus();
        $('#employee_salary').show();
        return;
    }

    if ($('#Appointment').val() == '') {
        $('#Appointment').focus();
        $('#employee_appointment').show();
        return;
    }

    let Document = $('input[name="Document"]').prop('files')[0];

    Document != undefined ? $('#Employee_Document').val(Document.name) : $('#Employee_Document').val('Sem Documento');

    $('.modalSpinner').modal('show');
    $('#toast-container').toggle();
    $('#EmployeeForm').submit();
}