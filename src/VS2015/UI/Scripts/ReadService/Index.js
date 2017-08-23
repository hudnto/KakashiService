/// <reference path="C:\Users\Kabulouzo\Source\Repos\KakashiService\src\VS2015\UI\Content/fineUploader/templates/default.html" />
/// <reference path="C:\Users\Kabulouzo\Source\Repos\KakashiService\src\VS2015\UI\Content/fineUploader/templates/default.html" />
(function () {
    $('#btnForm').click(SubmitForm);
    UploadFile();
})();

function SubmitForm() {
    $('#result div').remove();
    var urlData = $('#Url').val();
    var json = { Url: urlData };
    var url = urlRoot + '/ReadService/Read';
    ClearResult();
    $.ajax({
        data: json,
        method: 'POST',
        url: url,
        success: function (data) {
            PrintResult(data);
        }
    }).fail(function (data) {
        console.log(data);
    });
}

function UploadFile() {
    var galleryUploader = new qq.FineUploader({
        element: document.getElementById("fine-uploader-import"),
        request: {
            endpoint: '/ReadService/ImportFile'
        },
        validation: {
            allowedExtensions: ['wsdl', 'xml', 'xsd']
        }
    });
}


function PrintResult(data) {
    var functions = data.response.functions;
    var total = data.response.totalFunctions;
    var name = data.response.name;
    var totalObject = data.response.totalObject;

    $('#response').append('<div></div>');

    var $response = $('#response div');
    $response.append("<label class='control-label'>Service Name: " + name);

    $response.append("<label class='control-label'>Total functions: " + total);
    functions.forEach(function (item, index) {
        $response.append("<label class='block'>" + (index + 1) + " - " + item);
    });

    $response.append("<label class='control-label'>Total objects: " + totalObject);
}

function ClearResult() {
    $('#response div').remove();
}