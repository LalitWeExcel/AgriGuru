function DeleteCartItem(BookingID, OrderNumber) {

    ConfirmationMsg('Are you sure, you want to delete this record?', function () {
        var isValid = true;
        $("#delete_btn").attr('disabled', true);

        if (!isValid) {
            $(window).scrollTop(0);
            return false;
        } else {

            $.ajax({
                url: $_DeleteCartItemUrl,
                data: JSON.stringify({
                    "booking_id": parseInt(BookingID),
                    "order_number": OrderNumber,
                }),
                type: "POST",
                dataType: "JSON",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function (request) {
                    request.setRequestHeader("Authorization", "Basic " + $_authorization);
                    request.setRequestHeader("Role", $_RoleName);
                    request.setRequestHeader("Content-Type", "application/json");
                },
                success: function (response) {
                    debugger;

                    if (response.Response.indexOf('Successful') >= 0) {
                        displayMessage(response.Data, 'success');
                        setTimeout(function () {
                            window.location.reload();
                            $("#delete_btn").attr('disabled', false);
                        }, 2000);
                    } else {
                        displayMessage(response.Data, 'error');
                        $("#delete_btn").attr('disabled', false);
                        window.location.reload();
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
    });

    return true;
}

function CheckoutMyCartPayment(isValid) {

    ConfirmationMsg('Are you sure, you want to check out this record?', function () {
        $("#btn_save").attr('disabled', true);
        var _req = [];

        $('.mycard').each(function (index, value) {
            var booking_id = $(this).data('bookingid');
            var order_number = $(this).data('ordernumber');
            _req.push({
                "BookingID": parseInt(booking_id),
                "OrderNumber": order_number
            });
        });

        if (!isValid) {
            $(window).scrollTop(0);
            return false;
        } else {

            $.ajax({
                url: $_CheckoutMyCtPaymenturl,
                data: JSON.stringify({
                    'CheckOut': _req,
                    "FullPayment": "Y"
                }),
                type: "POST",
                dataType: "JSON",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function (request) {
                    request.setRequestHeader("Authorization", "Basic " + $_authorization);
                    request.setRequestHeader("Role", $_RoleName);
                    request.setRequestHeader("Content-Type", "application/json");
                },
                success: function (response) {
                    debugger;

                    if (response.Response.indexOf('Successful') >= 0) {
                        $("#btn_save").attr('disabled', false);
                        window.location = response.Data;
                    } else {
                        displayMessage(response.Data, 'error');
                        $("#btn_save").attr('disabled', false);
                        window.location.reload();
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
    });

    return true;
}



