$('.search').on('change', function () {
    $('.selectlist option:selected').html();
    $('#myForm').submit();
})