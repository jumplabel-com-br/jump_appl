var arrEmail = [];
var emailValid;

function SearchEmailValid() {
    $.ajax({
        url: '/api/EmployeesAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            arrEmail = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function EmployeeSubmit() {
    SearchEmailValid();

    let wlh = window.location.href.split('/')[4]

    $('#employee_salary').hide();
    $('#employee_appointment').hide();

    arrEmail.filter(obj => obj.email == $('#Employee_Email').val()).length > 0 ? emailValid = false : emailValid = true;

    if (emailValid == false && wlh == 'Create') {
        alert('Este email já foi cadastrado');
        $('#Employee_Email').focus();
        return false;
    }

    if ($('#Employee_Email').val() == '') {
        alert('Preencher o campo de email');;
        $('#Employee_Email').focus();
        return false;
    }

    if ($('#Employee_Name').val() == '') {
        alert('Preencher o campo de nome');;
        $('#Employee_Name').focus();
        return false;
    }

    if ($('#Salary').val() == '') {
        $('#Salary').focus();
        $('#employee_salary').show();
        return;
    }

    if ($('#Employee_Appointment').val() == '') {
        $('#Employee_Appointment').focus();
        $('#employee_appointment').show();
        return;
    }

    if ($('#Employee_Password').val() == '' && wlh == 'Create') {
        alert('Preencher o campo de senha');
        $('#Employee_Password').focus();
        return false;
    }

    if ($('#Employee_Access_LevelId').val() == '' || $('#Employee_Access_LevelId').val() == null) {
        alert('Preencher o nivel de acesso');
        $('#Employee_Access_LevelId').focus();
        return false;
    }

    if ($('#Employee_Contract_Mode').val() == '' || $('#Employee_Contract_Mode').val() == null) {
        alert('Preencher o modo de contrato');
        $('#Employee_Contract_Mode').focus();
        return false;
    }
    
    if ($('#Employee_Password').val() != $('#ConfirmPassword').val() && wlh == 'Create') {
        alert('As senhas não conhecidem');
        $('#Employee_Password').focus();
        return false;
    }

    if (wlh == 'Edit' && $('#pswd').val() != $('#ConfirmPassword').val()) {
        alert('As senhas não conhecidem');
        $('#Employee_Password').focus();
        return false;
    }

    if (wlh == 'Edit' && $('#pswd').val() != null && $('#pswd').val() != '') {
        $('#Employee_Password').val($('#pswd').val());
    }
    /*

    if ($('#Document').val().length > 0) {
        let Document = $('#Document').prop('files')[0];

        if (
            document != undefined &&
            Document.name.substr(Document.name.length - 3).toLowerCase() != 'pdf' ||
            Document.name.substr(Document.name.length - 3).toLowerCase() != 'jpg' ||
            Document.name.substr(Document.name.length - 3).toLowerCase() != 'jpeg' ||
            Document.name.substr(Document.name.length - 3).toLowerCase() != 'png') {
            alert('O tipo de documento só pode ser PNG, JPG ou PDF');
            return false;
        }

        if (Document != undefined && $('#Document').prop("files")[0].size > 10000) {
            alert('O tamanho do arquivo deve ser de no máximo 1MB');
            return false;
        }

        value = Document.name.replace(/[ |&|$|#|@|%|*]/g, '');

        Document != undefined ? $('#Employee_Document').val($('#Outlays_File').val(value)) : $('#Employee_Document').val('Sem Documento');
    }
    */
    $('.modalSpinner').modal('show');
    $('#toast-container-saved').toggle();
    $('#EmployeeForm').submit();
}

$('#ConfirmPassword').val($('#Employee_Password').val())

if (window.matchMedia("(max-width: 300px)").matches) {
    $('.col-2').addClass('col-8');
    $('.col-2').addClass('mt-3');
    $('.col-8').removeClass('col-2');

    //$('.col-4 div').remove('div')
}
$('#ConfirmPassword').val('')

