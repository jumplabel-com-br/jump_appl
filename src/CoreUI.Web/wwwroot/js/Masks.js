const maskMoney = function () {
    var element = document.querySelectorAll('.money');

    element.forEach(obj => {
        var valor = obj.value;

        if (valor != '') {
            valor = valor + '';
            valor = parseInt(valor.replace(/[\D]+/g, ''));
            valor = valor + '';
            valor = valor.replace(/([0-9]{2})$/g, ",$1");

            if (valor.length > 6) {
                valor = valor.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");
            }

            obj.value = valor
        }
    });
}

const maskSpanMoney = function (element, id) {
    element = element + '';
    numeros = element.replace(/\D/g, "");
    numeros = numeros.replace(/(\d+)(\d{2})/, "R\$ $1,$2");
    numeros = numeros.replace(/(R\$\s)(\d+)(\d{3})(\,\d{2})/, "$1$2.$3$4");
    numeros = numeros.replace(/(R\$\s)(\d+)(\d{3})(\.\d{3}\,\d{2})/, "$1$2.$3$4");
    numeros = numeros.replace(/(R\$\s)(\d+)(\d{3})(\.\d{3}\.\d{3}\,\d{2})/, "$1$2.$3$4");

    //console.log('id:  ',id);

    if (id != null && id != undefined ) {
        $('#' + id).val(numeros)
        $('#' + id).attr('maxlength', 21)
    }
    return numeros;  

    /*var str = parseInt(element).toLocaleString('pt-br', {
        style: 'currency',
        currency: 'BRL',

    });

    return maskSpanMoney2(str);*/
}

var maskSpanMoney2 = function (element) {
    element = element.replace(',00', '').split('.');
    count = element.length -1
    var str = ''

    for (var i = 0; i < count; i++) {
        str += element[i]+'.'
    }

    str = str.substring(0, (str.length - 1))

    str = str + ',' + element[element.length - 1]


    if (str.substring(0,1) == ',') {
        str = str.substring(1)
    }

    return str;
}