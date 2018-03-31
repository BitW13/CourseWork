//id - того блока до которого надо прокрутить 
//<li><a href="javascript://0" onclick="slowScroll('#sk1')">ABOUT</a></li>

function slowScroll(id) {
    var offset = 50;
    $('html, body').animate({
        scrollTop: $(id).offset().top - offset
    }, 1000);
    return false;
}