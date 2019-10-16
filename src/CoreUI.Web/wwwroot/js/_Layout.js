var arrProjectTeam;
var arrClient;
var arrProject;
var wlhs = window.location.href.split('/')

ActiveLinck();

$('.modalSpinner').modal('hide');
$('.modalSpinner').hide();

//verifica de pode se está em revisão
if (wlhs[4] != 'ChangePassword') {
    if (approval != null) {
        if (approval > 1 && accessLevel == 3) {
            $('.readonly').prop('disabled', true);
        }
    }

    //ocuta coisas a quem não tem acesso de administrador ou gerencia
    accessLevel != 1 && accessLevel != 2 ? $('.acessAdminGerencia').hide() : '';

    if (accessLevel == 1 || accessLevel == 2) {

        $('#Approver').val(approver);
        $('.AccessItem').show();

    } else {

        $('#Approver').val('');
        $('.AccessItem').hide();

    }
}


function JsonMessagesBell() {

    let contURL = window.location.href.split('/')
    let url = "api/AlertsHours"

    switch (contURL.length) {
        case 6:
            url = "../../api/AlertsHours";
            break;
        case 5:
            url = "../api/AlertsHours";
            break;
        default:
            url = url;
            break;
    }

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {


            if (data.length == 0) {
                $('.badge-danger-cont-alerts-bell').hide();
            };

            $('#dropdownMenuAlerts').html(templateMessagesBellForAdmin(data));
            $('.badge-danger-cont-alerts-bell').html(data.length)
        })
        .fail(function () {
            console.log("ococrreu um erro");
        });
}

function templateMessagesBellForAdmin(model) {
    return `
	<div class="dropdown-header bg-light">
	    <strong>${model.length <= 1 ? `Você tem ${model.length} notificação` : `Você tem ${model.length} notificações`}</strong>
	</div>
	
	<div class="form-control">
	    ${model.map((obj) => {
        let fullDate = obj.date.replace('T00:00:00', '')
        let day = new Date(fullDate).getDate() + 1
        let month = new Date(fullDate).getMonth() + 1
        let year = new Date(fullDate).getFullYear()

        return `
		        <span class="cursor-pointer" onclick="wlhAlertBell(${obj.id})" title="Data: ${day + '/' + month + '/' + year}"><i class="fa fa-user"></i> ${MessageReturn(accessLevel, obj.consultant.replace('@jumplabel.com.br', ''), obj.approval)}</span>
		    <br/>`
    }).join('')}
	</div>
	`
}

function MessageReturn(accessLevel, consultant, approval) {
    if (accessLevel == 3 && approval == 2) {
        return 'Revisão negada';
    } else if (accessLevel == 3 && approval == 3) {
        return 'Revisão Aprovada'
    } else {
        return consultant + ` enviou uma revisão`
    }
}

function wlhAlertBell(id) {

    let paramGereric = window.location.href.split('/');

    if (paramGereric[5] != null && paramGereric[5] != undefined && paramGereric[3] == 'Hours') {
        window.location.href = `${id}`
    } else {
        window.location.href = `/Hours/Edit/${id}`
    }
}

function searchProjectAndEmployee() {
    $.ajax({
        url: '/api/HoursAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);
            arrProjectTeam = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function searchClientPerProject() {
    $.ajax({
        url: '/api/ProjectsAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);

            arrClient = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function searchProjects() {
    $.ajax({
        url: '/api/HoursAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);

            arrProject = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function searchProjectTeamPerProject() {
    $.ajax({
        url: '/api/Project_teamAPI',
        type: 'GET',
        async: false,
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            //console.log(data);
            arrProjectTeam = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function fn_showMessageDelete(id, param1, param2, pram3, param4) {

    let wlh = window.location.href.split('/')
    if (wlh[3] == 'Project_team') {

        searchProjectAndEmployee();

        param1 = param1.split('/')[2] + '-' + param1.split('/')[1] + '-' + param1.split('/')[0]
        param2 = param2.split('/')[2] + '-' + param2.split('/')[1] + '-' + param2.split('/')[0]

        //console.log('param1: ', param1, ' param2: ', param2)
        let count = arrProjectTeam.filter(obj => obj.employee_Id == param4 && obj.date.replace('T00:00:00', '') >= param1 && obj.date.replace('T00:00:00', '') <= param2 && obj.id_Project == pram3)
        console.log(count)
        //console.log(count.length)

        if (count.length > 0) {
            $('.toast-message').hide();
            $('.message-error-delete').html('Não é possível deletar, pois há horas deste funcionário para este projeto')
            $('.toast-message-cancel').show();
            $('#toast-container').toggle();
            return false;
        }


    }

    if (wlh[3] == 'Clients') {
        searchClientPerProject();

        let count = arrClient.filter(obj => obj.client_Id == id && obj.active == 1)

        if (count.length > 0) {
            $('.toast-message').hide();
            $('.message-error-delete').html('Não é possível deletar, pois há projetos deste cliente para um funcionário')
            $('.toast-message-cancel').show();
            $('#toast-container').toggle();
            return false;
        }
    }


    if (wlh[3] == 'Projects') {
        searchProjects();
        searchProjectTeamPerProject();

        let count = arrProject.filter(obj => obj.id_Project == id)
        let countProjectTeam = arrProjectTeam.filter(obj => obj.project_Id == id)


        if (count.length > 0) {
            $('.toast-message').hide();
            $('.message-error-delete').html('Não é possível deletar, pois há horas lançadas com este projeto')
            $('.toast-message-cancel').show();
            $('#toast-container').toggle();
            return false;
        }

        if (countProjectTeam.length > 0) {
            $('.toast-message').hide();
            $('.message-error-delete').html('Não é possível deletar, pois há equipes lançadas com este projeto')
            $('.toast-message-cancel').show();
            $('#toast-container').toggle();
            return false;
        }
    }

    $('.toast-message').show();
    $('.toast-message-cancel').hide();
    $('#IdDelete').val(id);
    $('#toast-container').toggle();
}


if ($('table').length > 0) {


    $('table').DataTable({
        "bJQueryUI": true,
        "bPaginate": false,
        "bFilter": false,
        "info": false,
        "oLanguage": {
            "sProcessing": "Processando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "Não foram encontrados resultados",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando de 0 até 0 de 0 registros",
            "sInfoFiltered": "",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Primeiro",
                "sPrevious": "Anterior",
                "sNext": "Seguinte",
                "sLast": "Último"
            }
        }
    });

    //$('#example_info').html('')
    //$('#DataTables_Table_0_info').html('')

    /*
    $('.sorting').on("click", function () {
        removeSorting();
    });

    function removeSorting() {
        document.querySelectorAll('table thead th')[document.querySelectorAll('table thead th').length - 1].classList.remove('sorting')
        if (document.querySelectorAll('table thead th')[0].classList[0] == "custom-checkbox") {
            document.querySelectorAll('table thead th')[0].classList.remove('sorting')
            document.querySelectorAll('table thead th')[0].classList.remove('sorting_asc')
            document.querySelectorAll('table thead th')[0].setAttribute('aria-disabled', true)
        }
    }

    removeSorting
    */

}

if (wlhs[4] != 'ChangePassword') {
    JsonMessagesBell();
}