(function () {
    $('#btnClone').click(SubmitForm);
})();

function SubmitForm() {
    $('#result div').remove();
    var urlData = $('#Url').val();
    var json = { Url: urlData };
    var url = urlRoot + '/ReadService/Read';
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

function PrintResult(success, message) {

}