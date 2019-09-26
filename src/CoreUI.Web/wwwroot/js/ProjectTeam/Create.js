function TextName(id, input) {

    let select = document.querySelector('#' + id);
    let option = select.children[select.selectedIndex];
    let texto = option.textContent;
    console.log(texto);

    document.querySelector('#'+input).value = texto;
}