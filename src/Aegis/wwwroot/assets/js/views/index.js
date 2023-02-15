function send2FACode() {
    ajaxGet("/sendcode")
}

function ajaxGet(url, onSuccess, onError) {
    $.ajax({
        type: "GET",
        url: url,
    }).done(function (response) {
        if (!onSuccess) {
            onSuccess = onSuccessDefault;
        }

        onSuccess(response);
    }).fail(function (error) {
        if (!onError) {
            onError = onErrorDefault;
        }

        onError(error);
    });
}


function ajaxPost(url, formData, onSuccess, onError) {
    $.ajax({
        type: "POST",
        url: url,
        data: formData,
    }).done(function (response) {
        if (!onSuccess) {
            onSuccess = onSuccessDefault;
        }

        onSuccess(response);
    }).fail(function (error) {
        if (!onError) {
            onError = onErrorDefault;
        }

        onError(error);
    });
}

function onSuccessDefault() {
    toastr["success"]("Request Recieved!");
}

function onErrorDefault() {
    toastr["error"]("Something went wrong. Please try again later!");
}

$.fn.serializeAll = function () {
    const obj = {};

    $('input', this).each(function () {
        obj[this.name] = $(this).val();
    });

    return $.param(obj);
}