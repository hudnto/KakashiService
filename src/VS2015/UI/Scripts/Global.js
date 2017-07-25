function AjaxResponse(input) {
    var json = input.responseJSON == undefined ? input : input.responseJSON;
    if (json.success) {
        ActivateModal(json.modal);
    } 
}

function ActivateModal(input) {
    var title = input.title;
    var message = input.message;

    $('#modalTitle').text(title);
    $('#modalMessage').text(message);

    $('#modalId').modal();
}
