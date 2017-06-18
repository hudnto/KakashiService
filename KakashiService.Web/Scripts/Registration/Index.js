(function () {
    $('#btnClone').click(SubmitForm);
})();

function SubmitForm() {
    var serviceName = $('ServiceName').val();
    var port = $('Port').val();
    var buildPath = $('BuildPath').val();
    var url = $('Url').val();
    $.ajax({
        dataType: 'json',
        method: 'post',
        success: function (response) {
            alert(response);
        },
        url: urlRoot + '/Registration/Register',


    });
}