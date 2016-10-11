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




