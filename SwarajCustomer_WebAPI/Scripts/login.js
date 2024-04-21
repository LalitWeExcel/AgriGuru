


$(function () {

    $("#Email").val('');
    $("#password").val('');

    String.prototype.hexEncode = function () {
        var hex, i;

        var result = "";
        for (i = 0; i < this.length; i++) {
            hex = this.charCodeAt(i).toString(16);
            result += ("000" + hex).slice(-4);
        }

        return result;
    };

    function generate_random_string(string_length) {
        let random_string = '';
        let random_ascii;
        for (let i = 0; i < string_length; i++) {
            random_ascii = Math.floor((Math.random() * 25) + 97);
            random_string += String.fromCharCode(random_ascii)
        }
        return random_string;
    };

    $("#btnSubmit").bind("click", function () {
        $("#failedErrorId").text('');


        var ranNum = Math.floor(100000 + Math.random() * 900000);

        $("#password").val(ranNum + "" + $("#password").val().hexEncode());

        debugger;

        console.log($("#Email").val());
        console.log($("#password").val());

    });

});

$(document).ready(function () {


    $("html").on("contextmenu", function () {
        return false;
    });


});

$(document).keydown(function (event) {
    if (event.keyCode == 123) { // Prevent F12
        return false;
    } else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) { // Prevent Ctrl+Shift+I        
        return false;
    }
});