(function () {
    $('#btnForm').click(SubmitForm);
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

function PrintResult(data) {
    var functions = data.response.functions;
    var total = data.response.totalFunctions;
    var name = data.response.name;
    var totalObject = data.response.totalObject;

    $('#response').append('<div></div>');

    var $response = $('#response div');
    $response.append("<label class='control-label'>Service Name: " + name);

    $response.append("<label class='control-label'>Total functions: " + total);
    functions.forEach(function(item, index) {
        $response.append("<label class='block'>" + (index + 1) + " - " + item);
    });

    $response.append("<label class='control-label'>Total objects: " + totalObject);
}

function ClearResult() {
    $('#response div').remove();
}