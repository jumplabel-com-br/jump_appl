var arrAno = [];
var arr;
var wlhs = window.location.href.split('/')

ActiveLinck();

$('.modalSpinner').modal('hide');
$('.modalSpinner').hide();

//verifica se está em revisão
if (wlhs[4] != 'ChangePassword') {
    if (approval != null) {
        if (approval > 1 && accessLevel == 3) {
            $('.readonly').prop('disabled', true);
        }
    }

    //ocuta coisas a quem não tem acesso de administrador ou gerencia
    accessLevel != 1 && accessLevel != 2 ? $('.acessAdminGerencia').remove() : '';

    if (accessLevel == 1 || accessLevel == 2) {

        $('#Approver').val(approver);
        $('.AccessItem').show();

    } else {

        $('#Approver').val('');
        $('.AccessItem').hide();

    }
}


function JsonMessagesBell() {

    let url = "/api/AlertsHours"

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
    
    <div class="form-control" ${model.length > 0 ? `style="overflow-y: scroll; height: 200px` : ''}">
        ${model.map((obj) => {
        let fullDate = obj.date.replace('T00:00:00', '')
        let day = new Date(fullDate).getDate() + 1
        let month = new Date(fullDate).getMonth() + 1
        let year = new Date(fullDate).getFullYear()

        return `
                <span class="cursor-pointer" onclick="FilterHoursPerEmployee('${obj.consultant.replace('@jumplabel.com.br', '')}', ${obj.id})" ${accessLevel == 3 ? `title="${day + '/' + month + '/' + year}"` : ''}><i class="fa fa-user"></i> ${MessageReturn(accessLevel, obj.consultant.replace('@jumplabel.com.br', ''), obj.approval)}</span>
            <br/>`
    }).join('')}
    </div>
    `
}

function FilterHoursPerEmployee(consultant, id) {
    if (wlhs[3] != 'ModeAdmin' && accessLevel != 3) {
        alert('Acesse Aprovação de horas para verificar as horas do usuário')
        return false;
    }

    if (accessLevel == 3) {
        wlhAlertBell(id)
    }

    $('#choose_employees').val(consultant);
    $('table').DataTable().search(consultant).draw()
    $('table').DataTable().search('')
    $('input[type="search"]').val('')
}

function MessageReturn(accessLevel, consultant, approval) {
    if (accessLevel == 3 && approval == 2) {
        return 'Revisar';
    } else if (accessLevel == 3 && approval == 3) {
        return 'Revisão reprovada'
    } else if (accessLevel == 3 && approval == 4) {
        return 'Revisão aprovada'
    }
    else {
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

function ReturnFunction(url, type = 'GET', async = false, dataType = 'json', data = {}) {
    $.ajax({
        url: url,
        type: type,
        async: async,
        dataType: dataType,
        data: data,
    })
        .done(function (data) {
            //console.log(data);
            arr = data;
        })
        .fail(function () {
            console.log("error");
        });
}

function fn_showMessageDelete(id, param1, param2, pram3, param4) {

    if (wlhs[3] == 'Project_team') {

        ReturnFunction('/api/HoursAPI');

        param1 = param1.split('/')[2] + '-' + param1.split('/')[1] + '-' + param1.split('/')[0]
        param2 = param2.split('/')[2] + '-' + param2.split('/')[1] + '-' + param2.split('/')[0]

        //console.log('param1: ', param1, ' param2: ', param2)
        let count = arr.filter(obj => obj.employee_Id == param4 && obj.date.replace('T00:00:00', '') >= param1 && obj.date.replace('T00:00:00', '') <= param2 && obj.id_Project == pram3)
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

    if (wlhs[3] == 'Clients') {
        ReturnFunction('/api/ProjectsAPI');

        let count = arr.filter(obj => obj.client_Id == id && obj.active == 1)

        if (count.length > 0) {
            $('.toast-message').hide();
            $('.message-error-delete').html('Não é possível deletar, pois há projetos deste cliente para um funcionário')
            $('.toast-message-cancel').show();
            $('#toast-container').toggle();
            return false;
        }
    }


    if (wlhs[3] == 'Projects') {
        ReturnFunction('/api/HoursAPI');
        ReturnFunction('/api/Project_teamAPI');

        let count = arr.filter(obj => obj.id_Project == id)
        let countProjectTeam = arr.filter(obj => obj.project_Id == id)


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

function NameSelect(id) {
    switch (id) {
        case 2:
            return 'Status';
            break;

        case 4:
            return 'Cliente';
            break;

        case 5:
            return 'Projeto';
            break;

        case 6:
            return 'Funcionário';
            break;
        default:
            return 'Selecione';
    }
}

function nameClass(id) {
    switch (id) {
        case 4:
            return 'choose_clients';
            break;

        case 5:
            return 'choose_projects';
            break;

        case 6:
            return 'choose_employees';
            break;

        default:
            return '';
    }
}

if ($('table').length > 0) {
    var initComplete = function () {
        if (wlhs[3] == "ModeAdmin" || wlhs[3] == "OutlaysAdmin") {
            this.api().columns().every(function (id, j) {
                var column = this;
                //console.log(id)
                if ((wlhs[3] == "ModeAdmin" && (id == 2 || id == 4 || id == 5 || id == 6)) || (wlhs[3] == "OutlaysAdmin" && (id == 1 || id == 2 || id == 3 || id == 4))) {
                    var select = $(`<select class="form-control ${wlhs[3] == "ModeAdmin" ? nameClass(id) : ''}" id="${wlhs[3] == "ModeAdmin" ? nameClass(id) : ''}"><option value=""> ${wlhs[3] == "ModeAdmin" ? NameSelect(id) : 'Selecione'}</option></select>`)
                        .appendTo($(column.header()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                            wlhs[3] == "ModeAdmin" ? SumTotalHours() : '';
                        });


                    column.data().unique().sort().each(function (value, j) {
                        //console.log(j);
                        select.append('<option value="' + value + '">' + value + '</option>')
                    });
                }
            });
        }
    };

    columnsModeAdmin = [3, 4, 5, 6, 7, 8, 9, 10, 11];
    columnsOutlaysAdmin = [2, 3, 4, 5, 6, 7, 8]

    columnDefsModeAdmin = [
        { "orderable": false, "targets": 0 },
        { "sorting_asc": false, "targets": 0 },
        { "orderable": false, "targets": 11 },
    ];

    columnDefsOutlaysAdmin = [
        { "orderable": false, "targets": 0 },
        { "sorting_asc": false, "targets": 0 },
        { "orderable": false, "targets": 10 },
    ]

    var columns;
    var columnDefs;

    if (wlhs[3] == "ModeAdmin") {
        columns = columnsModeAdmin;
        columnDefs = columnDefsModeAdmin;

    } else if (wlhs[3] == "OutlaysAdmin") {
        columns = columnsOutlaysAdmin;
        columnDefs = columnDefsOutlaysAdmin;
    }


    function arrFor() {
        if (wlhs[3] == "ModeAdmin") {
            for (var i = 0; i <= 12; i++) {
                if (i == 1 || i == 3 || i >= 7) {
                    $('#example thead tr:eq(1) th')[i].innerHTML = '';
                }

                $('#example thead tr:eq(1) th:eq(' + i + ')').removeClass('sorting_asc');
                $('#example thead tr:eq(1) th:eq(' + i + ')').removeClass('sorting');
            }
        } else if (wlhs[3] == "OutlaysAdmin") {
            for (var i = 0; i <= 10; i++) {
                if (i >= 4) {
                    $('table thead tr:eq(1) th')[i].innerHTML = '';
                }

                $('table thead tr:eq(1) th:eq(' + i + ')').removeClass('sorting_asc');
                $('table thead tr:eq(1) th:eq(' + i + ')').removeClass('sorting');
            }
        }
    }

    var buttons = wlhs[3] == "ModeAdmin" || wlhs[3] == "OutlaysAdmin" ?
        [
            {
                extend: 'excelHtml5',
                footer: true,
                text: '<i class="fa fa-file-excel-o"></i>',
                exportOptions: {
                    // Aqui você inclui o índice da coluna
                    // Sabendo que os índices começam com 0
                    columns
                },

            },
            {
                extend: 'pdfHtml5',
                footer: true,
                text: '<i class="fa fa-file-pdf-o"></i>',
                title: ' ',
                filename: 'RelatórioPDF',
                customize: function (doc) {
                    doc.content[1].table.body[0].forEach(function (h) {
                        h.fillColor = '#EF8223';
                        console.log(h);
                    });

                    doc.content.splice(0, 0, {
                        margin: [0, 0, 170, 0],
                        alignment: 'center',
                        width: 200,
                        heigth: 100,
                        image: 'data:image/png;base64,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'
                    });

                    function docContentSplice(param1, param2, m0, m1, m2, m3, al, width, heigth, text) {
                        return doc.content.splice(param1, param2, {
                            margin: [m0, m1, m2, m3],
                            alignment: al,
                            width: width,
                            heigth: heigth,
                            text: text
                        });
                    }

                    docContentSplice(3, 0, 0, 30, 0, 0, 'left', 200, 100, '______________________________');
                    docContentSplice(4, 0, 0, 3, 0, 0, 'left', 200, 100, 'Assinatura Gestor Jump Label');
                    docContentSplice(5, 0, 0, 3, 0, 0, 'left', 200, 100, 'Nome:');
                    docContentSplice(6, 0, 0, 3, 0, 0, 'left', 200, 100, 'CPF:');

                    docContentSplice(7, 0, 0, 30, 0, 0, 'left', 200, 100, '______________________________');
                    docContentSplice(8, 0, 0, 3, 0, 0, 'left', 200, 100, 'Assinatura Consultor');
                    docContentSplice(9, 0, 0, 3, 0, 0, 'left', 200, 100, 'Nome:');
                    docContentSplice(10, 0, 0, 3, 0, 0, 'left', 200, 100, 'CPF:');
                },

                exportOptions: {
                    // Exporta as colunas
                    columns
                }
            },
            {
                extend: 'print',
                footer: true,
                text: '<i class="fa fa-print"></i>',
                title: '',
                exportOptions: {
                    // Exporta as colunas
                    columns
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('compact').css({ "background-color" : "#EF8223", "color" : "#fff"});
                }
            }
        ] : [];

    $('table').DataTable({
        initComplete: initComplete,
        "bJQueryUI": true,
        "bPaginate": false,
        "bFilter": true,
        "info": false,
        dom: 'Bfrtip',
        colReorder: true,
        buttons: buttons,
        "columnDefs": columnDefs,
        "oLanguage": {
            "sProcessing": "Processando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "Não foram encontrados resultados",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando de 0 até 0 de 0 registros",
            "sInfoFiltered": "",
            "sInfoPostFix": "",
            "sSearch": "Pesquisar:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Primeiro",
                "sPrevious": "Anterior",
                "sNext": "Seguinte",
                "sLast": "Último"
            }
        }
    });

    if (wlhs[3] == "ModeAdmin" || wlhs[3] == "OutlaysAdmin") {

        $('table thead tr').clone(true).appendTo('table thead');

        if (wlhs[3] == "ModeAdmin") {
            var names = ['Aprovação', 'Cobrança', 'Status', 'Data', 'Cliente', 'Projeto', 'Funcionário']

            for (var i = 0; i < 7; i++) {
                $(`table thead tr:eq(0) th`)[i].innerHTML = names[i];
            }
        }

        if (wlhs[3] == "OutlaysAdmin") {
            var names = ['#', 'Status', 'Cliente', 'Projeto', 'Funcionário']

            for (var i = 0; i < 5; i++) {
                $(`table thead tr:eq(0) th`)[i].innerHTML = names[i];
            }
        }

        arrFor();


        $('.buttons-excel, .buttons-pdf, .buttons-print').addClass('btn btn-lg');
        $('.buttons-excel').addClass('btn-success');
        $('.buttons-pdf').addClass('btn-danger');
        $('.buttons-print').addClass('btn-primary');

        $('.buttons-excel').attr('title', 'Baixar excel');
        $('.buttons-pdf').attr('title', 'Baixar PDF');
        $('.buttons-print').attr('title', 'Imprimir');
    }

    $('input[type="search"]').addClass('form-control');


    $('input[type="search"]').on('keyup', function () {
        wlhs[3] == 'Hours' || wlhs[3] == "ModeAdmin" ? SumTotalHours() : '';
    });

    $('table thead tr:eq(0)').css({ "background-color": "#EF8223", "color": "#fff" })
    $('table thead tr:eq(0) th').css({ "border-color": "#010101", "width": "60px" })

}

$('.imgLogo').attr('src', $('#ImgLogo').val())

/*
if (wlhs[4] != 'ChangePassword') {
    JsonMessagesBell();
}
*/