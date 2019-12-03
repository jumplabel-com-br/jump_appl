/*function fn_responsive() {

    width = window.innerWidth;
    if (width <= 300) {
        $('.col-2').addClass('col-8');
        $('.col-2').addClass('mt-3');
        $('.col-8').removeClass('col-2');

    } else if (width > 300 && width <= 400) {
        $('.col-2').addClass('col-6');
        $('.col-2').addClass('mt-3');
        $('.col-6').removeClass('col-2');

    } else if (width > 400 && width <= 500) {
        $('.col-2').addClass('col-4');
        $('.col-2').addClass('mt-3');
        $('.col-4').removeClass('col-2');
    }
}

$(document).ready(function () {
    fn_responsive();
    $('#toast-container').hide();

    let wlh = window.location.href.split('/')[4]

    if (wlh == "ModeAdmin") {
        $('#backToList').attr('formaction', '/Hours/ModeAdmin')
    }
});*/

$('.btn-edit').on('click', function () {
    $('.btn-submit-sim-modal').on('click', function () {
        Update('Hours/EditAsync', $('#HoursForm'))
    });  
});

$('.btn-copy').on('click', function () {
    $('.btn-submit-sim-modal').on('click', function () {
        Create('Hours/CreateAsync', $('#HoursForm'))
    }); 
});