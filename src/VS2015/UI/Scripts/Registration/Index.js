(function () {
    $('#btnClone').click(SubmitForm);
    var galleryUploader = new qq.FineUploader({
        element: document.getElementById("fine-uploader-import"),
        request: {
            endpoint: '/Registration/ImportFile'
        },
        validation: {
            allowedExtensions: ['wsdl', 'xml', 'xsd']
        },
        callbacks: {
            onComplete: function (id, fileName, response, xhr) {
                if (response.success) {
                    $('[name="Url"]').prop('disabled', 'disabled');
                }
            }
        }
    });
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
            PrintResult(data.success, data.modal.message);
        }
    }).fail(function (data) {
        console.log(data.modal.message);
    });
}

function PrintResult(success, message) {
    $('#result').removeClass("hidden");
    $('#result').append('<div><p>'+message+'<p/></div>');
    if (success) {
        $('#result div').addClass("alert alert-success");
    } else {
        $('#result div').addClass("alert alert-danger");
    }
}