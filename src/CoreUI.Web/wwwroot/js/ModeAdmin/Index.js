﻿function checkedAll() {

    document.querySelectorAll('.trCount').forEach((obj, item) => {

        if (document.querySelector('#' + obj.id).style.display != 'none') {
            //console.log(document.querySelector('#' + obj.id + ' .checkedItem').checked)
            if (document.querySelector('#' + obj.id + ' .checkedItem').checked) {
                document.querySelector('#' + obj.id + ' .checkedItem').checked = false;
                //console.log(1)
            } else {
                document.querySelector('#' + obj.id + ' .checkedItem').checked = true;
                //console.log(2)
            }
        }
        //document.querySelector('#' + obj.id).style.display != 'none' ? document.querySelector('#' + obj.id + ' .checkedItem').checked = true : document.querySelector('#' + obj.id + ' .checkedItem').checked = false
    });
}

var clientes = [];
var employees = [];
var projects = [];

function FilterClients() {

    $.ajax({
        url: '/api/ClientsAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            console.log("success");
            clientes = data;
            //$('#choose_clients').html(CreateFilterClients(data));
        })
        .fail(function () {
            console.log("error");
        });
}

function FilterEmployees() {

    $.ajax({
        url: '/api/EmployeesAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            console.log("success");
            employees = data;
            //$('#choose_employees').html(CreateFilterEmployees(data));
        })
        .fail(function () {
            console.log("error");
        });
}

function FilterProjects() {

    $.ajax({
        url: '/api/ProjectsAPI',
        type: 'GET',
        dataType: 'json',
        data: {},
    })
        .done(function (data) {
            console.log("success");
            projects = data;
            //$('#choose_projects').html(CreateFilterProjects(data));
        })
        .fail(function () {
            console.log("error");
        });
}

function CreateFilterClients(model) {
    return `
       <option value="">Selecione...</option>
       ${model.map((obj) => {
           return `<option value="${obj.name}"> ${obj.name} </option>`
    }).join('')}`
}


function CreateFilterEmployees(model) {
    return `
       <option value="">Selecione...</option>
       ${model.map((obj) => {
        return `<option value="${obj.email.replace('@jumplabel.com.br', '')}"> ${obj.email.replace('@jumplabel.com.br', '')} </option>`
    }).join('')}`
}

function CreateFilterClients(model) {
    return `
       <option value="">Selecione...</option>
       ${model.map((obj) => {
           return `<option value="${obj.name}"> ${obj.name} </option>`
    }).join('')}`
}

function CreateFilterProjects(model) {
    return `
       <option value="">Selecione...</option>
       ${model.map((obj) => {
           return `<option value="${obj.project_Name}"> ${obj.project_Name} </option>`
    }).join('')}`
}

$(document).ready(function () {
    FilterEmployees();
    FilterProjects();
    FilterClients();

    $('#toast-container').hide();

    document.querySelectorAll('.Activities').forEach(obj => {
        obj.innerHTML.trim().length > 30 ? obj.innerHTML = obj.innerHTML.trim().substr(0, 20) + '...' : ''
    });


    
    $("#searchDataTable").on("keyup", function () {
        var value = $("#searchDataTable").val();
        var month = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + new Date().getFullYear() : '';
        var employee = $("#choose_employees").val().toLowerCase();
        var client = $("#choose_clients").val().toLowerCase();
        var project = $("#choose_projects").val().toLowerCase();
        $("#tbodyHour tr").filter(function () {
            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1 &&
                $(this).text().toLowerCase().indexOf(month) > -1 &&
                $(this).text().toLowerCase().indexOf(employee) > -1 &&
                $(this).text().toLowerCase().indexOf(client) > -1 &&
                $(this).text().toLowerCase().indexOf(project) > -1);
            SumTotalHours();
        });
    });


    function AtualizaComboAno() {
        for (i = 1990; i <= new Date().getFullYear(); i++) {
            arrAno.push({ 'ano': i })
        }

        $('#searchYearDataTable').html(arrAno.map(obj => {
            return `
            <option ${obj.ano == new Date().getFullYear() ? 'selected' : ''} value="${obj.ano}">${obj.ano}</option>
            
        `
        })
        )
    }
    AtualizaComboAno();
    $('#searchMothDataTable, #searchYearDataTable').on('change', function () {
        var month = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + $('#searchYearDataTable').val() : '';
        $('table').DataTable().search(month).draw();
        $('table tbdoy tr').length > 0 ?  SumTotalHours() : '';
    });

    var month = new Date().getMonth() + 1 + '/' + new Date().getFullYear();
    $("#searchMothDataTable").val(new Date().getMonth() + 1 )
    $('table').DataTable().search(month).draw();
    SumTotalHours();

    /*
    $('#choose_employees').on('change', function () {
        var employee = $("#choose_employees").val().toLowerCase();
        $('table').DataTable().search(employee).draw();
    });

    $('#choose_clients').on('change', function () {
        var client = $("#choose_clients").val().toLowerCase();
        $('table').DataTable().search(client).draw();
    });

    $('#choose_projects').on('change', function () {
        var project = $("#choose_projects").val().toLowerCase();
        $('table').DataTable().search(project).draw();
    });

    $("#searchMothDataTable, #choose_employees, #choose_projects, #choose_clients").on("change", function () {
        var value = $("#searchDataTable").val();
        var month = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + new Date().getFullYear() : '';
        var employee = $("#choose_employees").val().toLowerCase();
        var client = $("#choose_clients").val().toLowerCase();
        var project = $("#choose_projects").val().toLowerCase();

        $("#tbodyHour tr").filter(function () {
            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1 &&
                $(this).text().toLowerCase().indexOf(month) > -1 &&
                $(this).text().toLowerCase().indexOf(employee) > -1 &&
                $(this).text().toLowerCase().indexOf(client) > -1 &&
                $(this).text().toLowerCase().indexOf(project) > -1);
            SumTotalHours();
        });
    });

    $('#searchMothDataTable').val(new Date().getMonth() + 1);

    let value = $("#searchMothDataTable").val() != '' ? $("#searchMothDataTable").val() + '/' + new Date().getFullYear() : '';
    $("#tbodyHour tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        SumTotalHours();
    });
        SumTotalHours();
    */

    $('.checkedItem').css({ marginLeft: "40%", marginTop: "30%" })

    $('.paginate_button').on("click", function () {
        $("#choose_employees").val('');
        $("#choose_clients").val('');
        $("#choose_projects").val('');
    });
});

function SumTotalHours() {

    let hours = 0;
    let minutes = 0;


    document.querySelectorAll("#tbodyHour tr").forEach((teste, item) => {
        if (teste.style.display == 'none') {
            document.querySelectorAll('#tbodyHour tr .totalHours')[item].style.display = 'none'
        } else {
            document.querySelectorAll('#tbodyHour tr .totalHours')[item].style.display = ''
        }
    });

    document.querySelectorAll('.totalHours').forEach(obj => {
        if (obj.style.display != 'none') {
            hours += new Date('1999-01-01 ' + obj.textContent.trim()).getHours();
            minutes += new Date('1999-01-01 ' + obj.textContent.trim()).getMinutes();
        }
    });

    for (var i = minutes; minutes >= 60; i++) {
        hours += 1
        minutes -= 60
    }

    hours < 10 ? hours = '0' + hours : '';
    minutes < 10 ? minutes = '0' + minutes : '';

    $('#TotalOfSumHours').val(hours + ':' + minutes)

}

function confirmUpdateStatus() {

    if ($('#status').val().length <= 0) {
        $('.toast-container').show();
        $('.toast-message').hide();
        $('.message-error-delete').html('Operação inválida, selecione um status');
        $('.toast-message-cancel').show();
        $('.btn-btn-cancel').css({ marginLeft: "70%" });
        $('.btn-submit-confirm').hide();
        $('#toast-container').toggle();
        return false;
    }

    $('.toast-container').show();
    $('.toast-message').hide();
    $('.message-error-delete').html('Deseja realmente tomar esta ação ?')
    $('.toast-message-cancel').show();
    $('.btn-btn-cancel').css({ marginLeft: "50%" });
    $('.btn-submit-confirm').show();
    $('#toast-container').toggle();
}

function UpdateStatusSubmit() {
    var string = document.querySelector('#ids').value;
    var string1 = string.substr(0, (string.length - 1));
    document.querySelector('#ids').value = string1;

    if (string1.length <= 0) {
        $('.toast-container').show();
        $('.toast-message').hide();
        $('.message-error-delete').html('Operação inválida, check um item da lista');
        $('.toast-message-cancel').show();
        $('.btn-btn-cancel').css({ marginLeft: "70%" });
        $('.btn-submit-confirm').hide();
        $('#toast-container').show();
        return false;
    }

    $('.modalSpinner').modal('show');
    $('#UpdateStatus').submit();

}