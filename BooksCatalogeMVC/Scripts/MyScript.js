$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $('#Content').load(this.href, function () {
            $('#action').modal({
                keyboard: true
            }, 'show');
        });
        return false;
    });
});

$('#SortDown').change(function () {
    $(this).parents('form').submit();
    
});



