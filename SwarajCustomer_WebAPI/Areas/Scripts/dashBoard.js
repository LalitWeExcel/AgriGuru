function GetDashboardData() {
    $_NoOfRecords = 10;

    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }


    var start_date = $("#startdate_id").val();
    var end_date = $("#enddate_id").val();

    $.ajax({
        url: $_urlmethod,
        data: { 'start_date': start_date, 'end_date': end_date, },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#dvDashboardDetails").html(data);
        }
    });
}

function Details(user_id) {
    location.href = $_Details + "?user_id=" + user_id;
}

function OpenPopup(mobile_numeber) {
    ClearFields();

    $("#spnmobile_numeber").html(mobile_numeber);
    $('#logoutModal1').modal('show');
}

function GetComeingBirthday() {
    ClearFields();
    $.ajax({
        url: $_GetComeingBirthdayUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            

            if (data !== null && data.length > 0) {
                var tableHTML = "<table class='table table-bordered table-responsive table-hover'>" +
                    "<thead><tr> <td>Image</td> <td>Customer Name</td> <td>DOB</td><td>Contact</td> </tr></thead> ";
       $.each(data, function (i, obj) {
        tableHTML = tableHTML + "<tr><td><img src=" + obj.ImageUrl + " height=" + "50px" + " data-action='zoom' /></td>"+
        "<td title='Details' style='cursor: pointer' onclick = 'Details(" + obj.adm_user_id + ")'> " + obj.CustomerName + "</td>" +
        "<td title='Details' style='cursor: pointer' onclick = 'Details(" + obj.adm_user_id + ")'> " + obj.DOB + "</td>" +
        "<td title='Birthday Message' style='cursor: pointer' onclick = 'OpenPopup(" + obj.Contact + ")'> " + obj.Contact + "</td>" +
        "</tr>";
                });

                tableHTML = tableHTML + "</table>";
                $("#divExcelError").html(tableHTML)
                $('#logoutModal2').modal('show');
            }
            else { displayMessage("No data found !!", 'warning'); }

        },
        failure: function (response) {
            displayMessage("failure    " + response.responseText, 'warning');
        },
        error: function (response) {
            displayMessage("error   " + response.responseText, 'error');
        }
    });
}


function SendSmsMessage() {


    var isValid = true;
    var mobile_numeber = $("#spnmobile_numeber").html();
    var message = $("#txtmessage").val().trim();

    if (message == "") {
        $('#spnMessage').html('Please Enter Message !!');
        $('#spnMessage').removeClass('hide');
        isValid = false;
    }

    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        $.ajax({
            url: $_SendSmsMessageUrl,
            data: { 'mobileNumber': mobile_numeber, 'message': message },
            cache: false,
            type: 'GET',
            datatype: 'html',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                $('#logoutModal1').modal('hide');
                displayMessage("Happy birthday message sent successfully", 'success');
            },
            failure: function (response) {
                displayMessage("failure    " + response.responseText, 'warning');
            },
            error: function (response) {
                displayMessage("error   " + response.responseText, 'error');
            }
        });
    }
    return true;
}

function ClearFields() {
    $("#spnmobile_numeber").html('');
    $("#txtmessage").val('');
    $("#spnMessage").html('');

    $("#divExcelError").html('');
    $('#logoutModal1').modal('hide');
    $('#logoutModal2').modal('hide');
}