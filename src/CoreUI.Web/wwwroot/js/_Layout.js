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
            console.log(data);

            $('#dropdownMenuAlerts').html(templateMessagesBell(data));
            $('.badge-danger-cont-alerts-bell').html(data.length)
        })
        .fail(function () {
            console.log("ococrreu um erro");
        });
}

function templateMessagesBell(model) {
    return `
        <div class="dropdown-header bg-light">
            <strong>${model.length <= 1 ? `Você tem ${ model.length } notificação` :  `Você tem ${ model.length } notificações`}</strong>
          </div>
        
        <div class="form-control">
        ${model.map((obj) => {
            let fullDate = obj.date.replace('T00:00:00', '')
            let day = new Date(fullDate).getDate() + 1
            let month = new Date(fullDate).getMonth() + 1
            let year = new Date(fullDate).getFullYear()

            return `
              <span class="cursor-pointer" onclick="wlhAlertBell(${obj.id})" title="Data: ${day + '/' + month + '/' + year}"><i class="fa fa-user"></i> ${obj.consultant.replace('@jumplabel.com.br', '')} enviou uma revisão</span>
              <br/>`
    }).join('')}
      </div>
      `
}

function wlhAlertBell(id) {

    let paramGereric = window.location.href.split('/');

    if (paramGereric[5] != null && paramGereric[5] != undefined && paramGereric[3] == 'Hours') {
        window.location.href = `${id}`
    } else {
        window.location.href = `/Hours/Edit/${id}`
    }
}

JsonMessagesBell();