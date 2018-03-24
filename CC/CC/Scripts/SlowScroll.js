//id - того блока до которого надо прокрутить 

function slowScroll(id) {
    var offset = 40;
    $('html, body').animate({
        scrollTop: $(id).offset().top - offset
    }, 1000);
    return false;
}