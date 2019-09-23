$(document).ready(function () {
    if (window.matchMedia("(max-width: 300px)").matches) {
        $('.col-2').addClass('col-8');
        $('.col-2').addClass('mt-3');
        $('.col-8').removeClass('col-2');
    }
});