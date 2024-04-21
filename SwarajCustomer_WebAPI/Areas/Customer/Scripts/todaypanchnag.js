function $_Getpanchang(date, time) {
    $("#divGrid").html(gbl_loader_html);

    $.ajax({
        url: $_GetpanchangUrl,
        data: { 'date': date, 'time': time },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {      
            $("#divGrid").html('');
            $("#divGrid").html(data);
        },
        failure: function (response) {
            displayMessage("failure    " + response.responseText, 'warning');
        },
        error: function (response) {
            displayMessage("error   " + response.responseText, 'error');
        }
    });
}