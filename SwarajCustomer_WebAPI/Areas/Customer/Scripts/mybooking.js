function GetMyBookings() {
    $("#divGrid").html(gbl_loader_html);
    $.ajax({
        url: $_GetMyBookingsUrl,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#divGrid").html('');
            $("#divGrid").html(data);
            $("#AUpcoming").trigger("click");

            setTimeout(function () {
                $(".my-rating").starRating({
                    initialRating: parseInt($("#hddReting").val()),
                    strokeColor: '#894A00',
                    strokeWidth: 10,
                    starSize: 25,
                    readOnly: true,
                    useFullStars: true
                });
            }, 1000);
        },
        failure: function (response) {
            displayMessage("failure    " + response.responseText, 'warning');
        },
        error: function (response) {
            displayMessage("error   " + response.responseText, 'error');
        }
    });
}

function CancelBooking(BookingID, OrderNumber) {
    ConfirmationMsg('Are you sure, you want to cancel this booking ?', function () {
        $("#hddbooking_id").val(parseInt(BookingID));
        $("#hddorder_number").val(OrderNumber);
        $("#modalconfirm").modal('hide');
        $("#logoutModal1").modal('show');
    });
}

function _CancelBooking() {
    var isValid = true;
    var booking_id = $("#hddbooking_id").val();
    var order_number = $("#hddorder_number").val();
    var ression = $("#txtRession").val().trim();

    if (ression == "") {
        $('#spnRession').html('Please Enter Ression');
        $('#spnRession').removeClass('hide');
        isValid = false;
    }
    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        $.ajax({
            url: $_BookingCancelUrl,
            type: 'POST',
            data: JSON.stringify({
                'booking_id': booking_id,
                "order_number": order_number,
                "Message": ression
            }),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            processData: false,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Basic " + $_authorization);
                request.setRequestHeader("Role", $_RoleName);
                request.setRequestHeader("Content-Type", "application/json");
            },
            success: function (result) {
                debugger;

                if (result.Data != "") {
                    $("#logoutModal1").modal('hide');
                    displayMessage(result.Data, 'success');
                    setTimeout(function () {
                        GetMyBookings();
                    }, 1000);
                } else {
                    ("#logoutModal1").modal('show');
                    displayMessage(result.Data, 'warning');
                }
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

function ValidateEmail(inputText) {

    var mailformat = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (inputText.match(mailformat)) {
        return true;
    } else {
        return false;
    }
}
function myBooking(_objects, type) {
    var i, tabcontent, tablinks;

    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(type).style.display = "block";
    _objects.target.className += " active"; //currentTarget

}

// Function that validates email address through a regular expression.

function ClearFields() {
    $("#logoutModal1").modal('hide');
    $("#hddbooking_id").val('');
    $("#hddorder_number").val('');
    $("#txtRession").val('');
    $("#spnRession").html('');
}
function _cancelReminder() {

    $("#logoutModal2").modal('hide');

    $("#hddBookingID").val('');
    $("#hddOrderNumber").val('');

    $("#spnPujaName").html('');
    $("#spnPujaAddress").html('');

    $("#txtEmailAddress").val('');
    $("#spnEmailAddress").html('');


    debugger;
    var d = new Date();
    var currMonth = d.getMonth();
    var currYear = d.getFullYear();
    var currDate = d.getDate();
    var endDate = new Date(currYear, currMonth, currDate);
    $('#startdate_id').data("DateTimePicker").setValue(d);

}

function AddReminder(BookingID, OrderNumber, PujaName) {
    _cancelReminder();

    $("#hddBookingID").val(parseInt(BookingID));
    $("#hddOrderNumber").val(OrderNumber);

    $("#spnPujaName").html(PujaName)
    $("#spnPujaAddress").html($("#hddPujaAddress").val());

    $("#logoutModal2").modal('show');
}

function SaveReminder() {
    debugger;
    var isValid = true;
    var pujaName = $("#spnPujaName").html();
    var Date = $("#startdate_id").val();
    var Time = $("#DateOfTime").val();
    var Email = $("#txtEmailAddress").val();
    var BookingID = $("#hddBookingID").val();
    var OrderNumber = $("#hddOrderNumber").val();

    if (Time == "") {
        $('#spnDateOfTime').html('Please Enter Date Of Time');
        $('#spnDateOfTime').removeClass('hide');
        isValid = false;
    }
    else if (Email == "") {
        $('#spnEmailAddress').html('Please Enter Email');
        $('#spnEmailAddress').removeClass('hide');
        $('#spnDateOfTime').addClass('hide');
        isValid = false;
    }
    else if (!ValidateEmail(Email)) {
        $('#spnEmailAddress').html('Please Enter a vaild Email!');
        $('#spnEmailAddress').removeClass('hide');
        $('#spnDateOfTime').addClass('hide');
        isValid = false;
    }
    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        $.ajax({
            url: $_SaveReminderUrl,
            type: 'POST',
            data: JSON.stringify({
                'PujaName': pujaName,      
                'OrderNumber': OrderNumber,
                'Date': Date,
                "Time": Time,
                "Email": Email
            }),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            processData: false,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Basic " + $_authorization);
                request.setRequestHeader("Role", $_RoleName);
                request.setRequestHeader("Content-Type", "application/json");
            },
            success: function (response) {
                debugger;

                if (response !== null) {
                    if (response.indexOf('successfully') >= 0) {
                        displayMessage(response, 'success');            
                        setTimeout(function () { window.location.reload(); }, 2000);
                    } else {
                        displayMessage(response, 'warning');
                    }
                    $("#btn_save").attr('disabled', false);
                } else {
                    displayMessage(response, 'error');
                    $("#btn_save").attr('disabled', false);
                    setTimeout(function () { window.location.reload(); }, 2000);
                }
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