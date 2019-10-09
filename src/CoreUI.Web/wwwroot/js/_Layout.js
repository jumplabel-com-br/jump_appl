var arrProjectTeam;


ActiveLinck();

$('.modalSpinner').modal('hide');
$('.modalSpinner').hide();

//verifica de pode se está em revisão
if (approval != null) {
    if (approval == 1 && accessLevel == 3) {
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

function fn_showMessageDelete(id, start, end, projectId) {

    let wlh = window.location.href.split('/')
    if (wlh[3] == 'Project_team') {

        searchProjectAndEmployee();

        start = start.split('/')[2] + '-' + start.split('/')[1] + '-' + start.split('/')[0]
        end = end.split('/')[2] + '-' + end.split('/')[1] + '-' + end.split('/')[0]

        //console.log('start: ', start, ' end: ', end)
        let count = arrProjectTeam.filter(obj => obj.employee_Id == employee && obj.date.replace('T00:00:00', '') >= start && obj.date.replace('T00:00:00', '') <= end && obj.id_Project == projectId)

        //console.log(count.length)

        if (count.length > 0) {
            $('.toast-message').hide();
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

JsonMessagesBell();