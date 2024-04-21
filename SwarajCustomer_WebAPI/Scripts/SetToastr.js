$(document).ready(function () {
    if ($('#success').val()) {
        displayMessage($('#success').val(), 'success');
    }
    if ($('#error').val()) {
        displayMessage($('#error').val(), 'error');
    }
    if ($('#warning').val()) {
        displayMessage($('#warning').val(), 'warning');
    }
    if ($('#info').val()) {
        displayMessage($('#info').val(), 'info');
    }
    
});
var displayMessage = function (message, msgType) {
 
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-center-custom",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": (msgType == "error" ? "6000" : "400"),
        "hideDuration": (msgType == "error" ? "6000" : "1000"),
        "timeOut": (msgType == "error" ? "6000" : "5000"),
        "extendedTimeOut": (msgType == "error" ? "6000" : "1000"),
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };


    toastr[msgType](message);
};