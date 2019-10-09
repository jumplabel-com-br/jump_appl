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

    if ($('#Employee_Password').val() != $('#ConfirmPassword').val()) {
        alert('As senhas não conhecidem');
        $('#Employee_Password').focus();
        return false;
    }

    let Document = $('input[name="Document"]').prop('files')[0];

    Document != undefined ? $('#Employee_Document').val(Document.name) : $('#Employee_Document').val('Sem Documento');

    $('.modalSpinner').modal('show');
    $('#toast-container').toggle();
    $('#EmployeeForm').submit();
}

$('#ConfirmPassword').val($('#Employee_Password').val())

if (window.matchMedia("(max-width: 300px)").matches) {
    $('.col-2').addClass('col-8');
    $('.col-2').addClass('mt-3');
    $('.col-8').removeClass('col-2');

    //$('.col-4 div').remove('div')
}