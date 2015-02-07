var Modal = {
    callbackFunc: null,
    alert: function (title, body, callBack) {
        this.callbackFunc = callBack;
        $("#alertModal .modal-title").html(title);
        $("#alertModal .modal-body").html(body);
        $("#alertModal").modal({ keyboard: false });
    },
    confirm: function (title, body, callbackOk, callbackFail) {
        if (typeof (callbackOk) == "function")
            Modal.callbackOk = callbackOk;

        if (typeof (callbackFail) == "function")
            Modal.callbackFail = callbackFail;

        $("#confirmModal .modal-title").html(title);
        $("#confirmModal .modal-body").html(body);
        $("#confirmModal").modal({ keyboard: false });
    },
    callbackOk: null,
    callbackFail: null,
    hide: function () {
        $('#alertModal').modal('hide');
        if (typeof (this.callbackFunc) == "function")
            this.callbackFunc();
    }
}

$(document).ready(function () {
    $("#alertModal").click(function () {
        Modal.hide();
    });

    $("#confirmModal .btn-ok").click(function () {
        if (typeof (Modal.callbackOk) == "function")
            Modal.callbackOk();
        $('#confirmModal').modal('hide');
    });
    $("#confirmModal .btn-cancelar").click(function () {
        if (typeof (Modal.callbackFail) == "function")
            Modal.callbackFail();
        $('#confirmModal').modal('hide');
    });
});