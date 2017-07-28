function AjaxResponse(input) {
    var json = input.responseJSON == undefined ? input : input.responseJSON;
    if (json.success) {
        ActivateModal(json.modal);
    } 
}

$(document).on({
    ajaxStart: function () { ToggleAnimationLock(true); },
    ajaxStop: function () { ToggleAnimationLock(false); }
});

function ActivateModal(input) {
    var title = input.title;
    var message = input.message;

    $('#modalTitle').text(title);
    $('#modalMessage').text(message);

    $('#modalId').modal();
}

function ToggleAnimationLock(show) {
    if (show) {
        $('.loader').removeClass('hidden');
        $('.container').addClass('overlay');
    } else {
        $('.loader').addClass('hidden');
        $('.container').removeClass('overlay');
    }
}