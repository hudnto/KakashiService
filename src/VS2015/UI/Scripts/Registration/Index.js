(function () {
    $('#btnClone').click(SubmitForm);
})();

function SubmitForm() {
    $('#result div').remove();

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
            PrintResult(data.modal.status, data.modal.message);
        }
    }).fail(function (data) {
        alert(data.modal.message);
        console.log(data.modal.message);
    });
}

function PrintResult(status, message) {
    $('#result').removeClass("hidden");
    $('#result').append('<div><p>'+message+'<p/></div>');
    if (status === "success") {
        $('#result div').addClass("alert-success");
    } else {
        $('#result div').addClass("alert-danger");
    }
}