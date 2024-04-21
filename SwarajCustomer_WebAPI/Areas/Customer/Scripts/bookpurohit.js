function SavePujaOrder(IsAddToCart) {
    debugger;
    var isValid = true;
    var PujaTime = $("#AddTimeSlot option:selected").val()
    var FirstName = $("#txtFirstName").val();
    var LastName = $("#txtLastName").val();
    var Email = $("#txtEmail").val();
    var Phone = $("#txtContantNumber").val();
    var Address = $("#txtAddress").val();
    var Landmark = $("#txtLandMark").val();
    var CityName = $("#txtCityName").val();
    var StateID = $("#ddState option:selected").val();
    var DistrictID = $("#ddDistrict option:selected").val();
    var Pincode = $("#txtPinCode").val();

    if (PujaTime == "0" || PujaTime == "") {
        $('#spnAddTimeSlot').html('Please Select Puja Time!');
        $('#spnAddTimeSlot').removeClass('hide');
        isValid = false;
    } else if (FirstName == "") {
        $('#spnFirstName').html('Please Enter First Name!');
        $('#spnFirstName').removeClass('hide');
        $('#spnAddTimeSlot').addClass('hide');
        isValid = false;
    } else if (LastName == "") {
        $('#spnLastName').html('Please Enter Last Name!');
        $('#spnLastName').removeClass('hide');
        $('#spnFirstName').addClass('hide');
        isValid = false;
    } else if (Email == "") {
        $('#spnEmail').html('Please Enter Email!');
        $('#spnEmail').removeClass('hide');
        $('#spnLastName').addClass('hide');
        isValid = false;
    } else if (!ValidateEmail(Email)) {
        $('#spnEmail').html('Please Enter a vaild Email!');
        $('#spnEmail').removeClass('hide');
        $('#spnLastName').addClass('hide');
        isValid = false;
    } else if (Phone == "") {
        $('#spnContantNumber').html('Please Enter Mobile Number!');
        $('#spnContantNumber').removeClass('hide');
        $('#spnEmail').addClass('hide');
        isValid = false;
    } else if (Phone.length < 10) {
        $('#spnContantNumber').html('Mobile Number should be 10 digit!!');
        $('#spnContantNumber').removeClass('hide');
        $('#spnEmail').addClass('hide');
        isValid = false;
    } else if (Address == "") {
        $('#spnAddress').html('Please Enter Address!');
        $('#spnAddress').removeClass('hide');
        $('#spnContantNumber').addClass('hide');
        isValid = false;
    } else if (Landmark == "") {
        $('#spnLandMark').html('Please Enter Landmark!');
        $('#spnLandMark').removeClass('hide');
        $('#spnContantNumber').addClass('hide');
        isValid = false;
    } else if (StateID == "0") {
        $('#spnddState').html('Please Select State!');
        $('#spnddState').removeClass('hide');
        $('#spnLandMark').addClass('hide');
        isValid = false;
    } else if (DistrictID == "0") {
        $('#spnddDistrict').html('Please Select District!');
        $('#spnddDistrict').removeClass('hide');
        $('#spnddState').addClass('hide');
        isValid = false;
    } else if (Pincode == "") {
        $('#spnPinCode').html('Please Enter Pincode!');
        $('#spnPinCode').removeClass('hide');
        $('#spnddDistrict').addClass('hide');
        isValid = false;
    } else if (Pincode.length < 6) {
        $('#spnPinCode').html('Pincode should be 6 digit!');
        $('#spnPinCode').removeClass('hide');
        $('#spnddDistrict').addClass('hide');
        isValid = false;
    }


    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        var JsonData = {
            "PujasList": [{
                "PurohitID": parseInt($("#hddPurohitID").val()),
                "PujaID": parseInt($("#hddPujaID").val()),
                "PujaDate": formatDate($("#startdate_id").val()),
                "PujaTime": parseInt(PujaTime),
                "PujaMRP": parseFloat($("#hddPujaMRP").val()),
                "PujaDiscount": parseFloat($("#hddPujaDiscount").val()),
                "PujaDiscountedPrice": parseFloat($("#hddPujaDiscountedPrice").val()),
                "IsSamagri": parseInt($("input[name='Samigri']:checked").val()),
                "SamagriMRP": $("#hddSamagriMRP").val(),
                "SamagriDiscount": $("#hddSamagriDiscount").val(),
                "SamagriDiscountedPrice": $("#hddSamagriDiscountPrice").val(),
                "IsForOther": $("input[name='IsForOther']:checked").val(),
                "FirstName": FirstName,
                "LastName": LastName,
                "Address": Address,
                "Phone": Phone,
                "Landmark": Landmark,
                "CityName": CityName,
                "StateID": parseInt(StateID),
                "districtid": parseInt(DistrictID),
                "Pincode": Pincode,
                "Latitude": $_Latitude,
                "Longitude": $_Longitude,
            }],
            "CustomerDetail": {
                "Cust_Id": $_Customer_UserId
            },
            "IsAddToCart": IsAddToCart,
            "FullPayment": $("input[name='Payment']:checked").val(),
        };

        console.log(JsonData);
        debugger;

        $.ajax({
            url: $_SavePujaOrderUrl,
            data: JSON.stringify(JsonData),
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

                if (response.Data !== null) {
                    if (response.Response.indexOf('Payment') >= 0) {
                        window.location = response.Response;

                    } else if (response.Data.indexOf('cart') >= 0) {
                        displayMessage(response.Data, 'success');
                        // window.location = $_GetDashBoard;
                        window.location.reload();

                    } else if (response.Data.indexOf('already') >= 0) {
                        displayMessage(response.Data, 'warning');
                        window.location.reload();

                    } else {
                        displayMessage(response.Data, 'warning');
                    }
                    $("#btn_save").attr('disabled', false);
                } else {
                    displayMessage(response.Message, 'error');
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

$(function () {

    $("#ddState").change(function () {
        GetDistrict($(this).val(), 0)
    });

    $('input[type=radio][name=IsForOther]').change(function () {

        if (this.value == 'N') {

            GetState($_StateId);
            GetDistrict($_StateId, $_DistrictId);

            $("#txtFirstName").val($_FirstName);
            $("#txtLastName").val($_LastName);
            $("#txtEmail").val($_Email);
            $("#txtContantNumber").val($_MobileNumber);
            $("#txtAddress").val($_Address);
            $("#txtLandMark").val($_Landmark);
            $("#txtPinCode").val($_PinCode);
            $("#txtCityName").val($_CityName);
        } else if (this.value == 'Y') {
            GetState(0);

            $("#txtFirstName").val(' ');
            $("#txtLastName").val(' ');
            $("#txtEmail").val(' ');
            $("#txtContantNumber").val(' ');
            $("#txtAddress").val(' ');
            $("#txtLandMark").val(' ');
            $("#txtPinCode").val(' ');
            $("#txtCityName").val(' ');
        }

    });

    $('input[type=radio][name=Samigri]').change(function () {
        if (this.value == '1') {
            $("#hddSamagriMRP").val($_SamagriMRP);
            $("#hddSamagriDiscount").val($_SamagriDiscount);
            $("#hddSamagriDiscountPrice").val($_SamagriDiscountPrice);
        } else if (this.value == '0') {
            $("#hddSamagriMRP").val('0.00');
            $("#hddSamagriDiscount").val('0.00');
            $("#hddSamagriDiscountPrice").val('0.00');
        }
    });

});

function _selectPuja(_object) {

    $("#hddPujaID").val($(_object).data('pujaid'));
    $("#hddPujaMRP").val($(_object).data('pujamrp'));
    $("#hddPujaDiscount").val($(_object).data('pujadiscount'));
    $("#hddPujaDiscountedPrice").val($(_object).data('pujadiscountedprice'));
}