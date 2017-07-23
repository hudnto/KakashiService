(function () {
    $('#btnClone').click(SubmitForm);
})();

function SubmitForm() {
    var serviceName = $('#ServiceName').val();
    var port = $('#Port').val();
    var buildPath = $('#BuildPath').val();
    var urlData = $('#Url').val();
    var json = { ServiceName: serviceName, Port: port, BuildPath: buildPath, Url: urlData };
    var url = urlRoot + '/Registration/Register';
    $.ajax({
        data: json,     
        method: 'POST',
        url: url,
        success: function (data) {
            alert(data.modal.message);
            console.log(data.modal.message);
        }
    }).done(function (data) {
        console.log('done ' + data);
    });
}