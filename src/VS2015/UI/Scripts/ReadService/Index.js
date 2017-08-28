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
        },
        callbacks: {
            onComplete: function (id, fileName, response, xhr) {
                PrintResult(response);
            }
        }
    });
}


function PrintResult(data) {
    var functions = data.response.functions;
    var total = data.response.totalFunctions;
    var name = data.response.name;
    var totalObject = data.response.totalObject;
    var objects = data.response.objects;

    var $nameService = $('#nameService');
    var $functions = $('#functions');
    var $objects = $('#objects');


    $nameService.append("<label class='block'>Service Name: " + name);

    $functions.append("<label class='block'>Total functions: " + total);
    functions.forEach(function (item, index) {
        $functions.append("<label class='block'>" + (index + 1) + " - " + item);
    });

    $objects.append("<label class='control-label'>Total objects: " + totalObject);
    objects.forEach(function (item, index) {
        $objects.append("<label class='block'>" + (index + 1) + " - " + item);
    });
}

function ClearResult() {
    $('#response div').remove();
}