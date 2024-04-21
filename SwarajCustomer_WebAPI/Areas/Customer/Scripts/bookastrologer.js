function SaveAstroOrder(IsAddToCart) {

    var isValid = true;
    var type = $("#hddConsultionmedium").val();

    var PujaTime = $("#AddTimeSlot option:selected").val();
    var BookingDuration = $("#hddBookingDurationId").val();
    var Address = $("#txtAddress").val();
    var LandMark = $("#txtLandMark").val();
    var StateID = $("#ddState option:selected").val();
    var DistrictID = $("#ddDistrict option:selected").val();
    var Pincode = $("#txtPinCode").val();
    var Phone = $("#txtPhoneNumber").val();
    var Whatsapp = $("#txtWhatsappNumber").val();
    var Skype = $("#txtSkypeId").val();
    var FirstName = $("#txtFirstName").val();
    var LastName = $("#txtLastName").val();
    var TOB = $("#DateOfTime").val();
    var POB = $("#txtPlaceofBirth").val();

    if (PujaTime == "0" || PujaTime == "") {
        $('#spnAddTimeSlot').html('Please Select Puja Time!');
        $('#spnAddTimeSlot').removeClass('hide');
        isValid = false;
    } else if (BookingDuration == "") {
        $('#spnbookingduration').html('Please Select Pick and Slot');
        $('#spnbookingduration').removeClass('hide');
        $('#spnAddTimeSlot').addClass('hide');
        isValid = false;
    } else if (FirstName == "") {
        $('#spnFirstName').html('Please Enter Last Name!');
        $('#spnFirstName').removeClass('hide');
        $('#spnbookingduration').addClass('hide');
        isValid = false;
    } else if (LastName == "") {
        $('#spnLastName').html('Please Enter Last Name!');
        $('#spnLastName').removeClass('hide');
        $('#spnbookingduration').addClass('hide');
        $('#spnFirstName').addClass('hide');
        isValid = false;
    } else if (TOB === "") {
        $('#spnDateOfTime').html('Please Enter Date of Time!');
        $('#spnDateOfTime').removeClass('hide');
        $('#spnbookingduration').addClass('hide');
        $('#spnLastName').addClass('hide');
        isValid = false;
    } else if (POB === "") {
        $('#spnPlaceofBirth').html('Please Enter Place of birth!');
        $('#spnPlaceofBirth').removeClass('hide');
        $('#spnbookingduration').addClass('hide');
        $('#spnDateOfTime').addClass('hide');
        isValid = false;
    } else if (type == "Upcoming") {

        if (Address == "0" || Address == "") {
            $('#spnAddress').html('Please Enter Address!');
            $('#spnAddress').removeClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (LandMark == "") {
            $('#spnLandMark').html('Please Enter Landmark!');
            $('#spnLandMark').removeClass('hide');
            $('#spnAddress').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (StateID == "0") {
            $('#spnddState').html('Please Select State!');
            $('#spnddState').removeClass('hide');
            $('#spnLandMark').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (DistrictID == "0") {
            $('#spnddDistrict').html('Please Select District!');
            $('#spnddDistrict').removeClass('hide');
            $('#spnddState').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (Pincode == "") {
            $('#spnPinCode').html('Please Enter Pincode!');
            $('#spnPinCode').removeClass('hide');
            $('#spnddDistrict').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (Pincode.length < 6) {
            $('#spnPinCode').html('Pincode should be 6 digit!');
            $('#spnPinCode').removeClass('hide');
            $('#spnddDistrict').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        }
    } else if (type == "Completed") {

        if (Phone == "") {
            $('#spnPhoneNumber').html('Please Enter Mobile Number!');
            $('#spnPhoneNumber').removeClass('hide');
            $('#spnPinCode').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        } else if (Phone.length < 10) {
            $('#spnPhoneNumber').html('Mobile Number should be 10 digit!!');
            $('#spnPhoneNumber').removeClass('hide');
            $('#spnPinCode').addClass('hide');
            $('#spnPlaceofBirth').addClass('hide');
            isValid = false;
        }
    } else if (type == "Cancelled") {

        var radioValue = $("input[name='gender']:checked").val();
        if (radioValue.value === "1") {

            if (Whatsapp == "") {
                $('#spnWhatsappNumber').html('Please Enter Whatsapp Number!');
                $('#spnWhatsappNumber').removeClass('hide');
                $('#spnPinCode').addClass('hide');
                $('#spnPlaceofBirth').addClass('hide');
                isValid = false;
            } else if (Whatsapp.length < 10) {
                $('#spnWhatsappNumber').html('Whatsapp Number should be 10 digit!!');
                $('#spnWhatsappNumber').removeClass('hide');
                $('#spnPinCode').addClass('hide');
                $('#spnPlaceofBirth').addClass('hide');
                isValid = false;
            }
        } else if (radioValue.value === "2") {
            if (Skype == "") {
                $('#spnSkype').html('Please Enter Skype id!');
                $('#spnSkype').removeClass('hide');
                $('#spnWhatsappNumber').addClass('hide');
                $('#spnPinCode').addClass('hide');
                $('#spnPlaceofBirth').addClass('hide');
                isValid = false;
            } else if (!ValidateEmail(Skype)) {
                $('#spnSkype').html('Please Enter a vaild Skype id!');
                $('#spnSkype').removeClass('hide');
                $('#spnWhatsappNumber').addClass('hide');
                $('#spnPinCode').addClass('hide');
                $('#spnPlaceofBirth').addClass('hide');
                isValid = false;
            }
        }
    }

    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {
      
        var JsonData = {
            "OrderList": [{
                "ServiceID": parseInt($("#hddServicesId").val()),
                "BookingDate": formatDate($("#startdate_id").val()),
                "BookingTime": parseInt(PujaTime),
                "BookingDuration": parseInt(BookingDuration),
                "AstroMRP": $("#hddAstroMRP").val(),
                "AstroDiscount": $("#hddAstroDiscount").val(),
                "AstroDiscountedPrice": $("#hddAstroDiscountedPrice").val(),
                "AstrologerID": parseInt($("#hddAstrologerId").val()),
                "FirstName": FirstName,
                "LastName": LastName,
                "Phone": Phone,
                "Address": Address,
                "CityName": $_CityName,
                "StateID": parseInt(StateID),
                "DistrictID": parseInt(DistrictID),
                "Latitude": $_Latitude,
                "Longitude": $_Longitude,
                "Landmark": LandMark,
                "Pincode": Pincode,
                "Whatsapp": Whatsapp,
                "Skype": Skype,
                "Gender": $("input[name='MaleFemale']:checked").val(),
                "DOB": formatDate($("#DateOfBirth").val()),
                "TOB": TOB,
                "POB": POB,
                "IsForOther": $("input[name='IsForOther']:checked").val(),
                "IsPaid": "N"
            }],
            "CustomerDetail": {
                "Cust_Id": $_Customer_UserId
            },
            "AddToCart": IsAddToCart,
            "FullPayment": $("input[name='Payment']:checked").val()
        }

        debugger;
        console.log(JsonData);

        $.ajax({
            url: $_SaveAstroOrderUrl,
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
        debugger;
        if (this.value == 'N') {

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
            $("#txtCityName").val(' ');
            $("#txtPinCode").val(' ');
            $("#txtPlaceofBirth").val(' ');

        }

    });

    $('input[type=radio][name=gender]').change(function () {

        if (this.value == '1') {
            $(".Whatsapp").removeClass('hide');
            $(".Skype").addClass('hide');

        } else if (this.value == '2') {
            $(".Whatsapp").addClass('hide');
            $(".Skype").removeClass('hide');
        }
    });
});

function _selectDurationSlots(_object) {
    var bookingduration = $(_object).data('bookingdurationid');
    var astromrp = $(_object).data('astromrp');
    var astrodiscount = $(_object).data('astrodiscount');
    var astrodiscountedprice = $(_object).data('astrodiscountedprice');

    $("#hddBookingDurationId").val(bookingduration);
    $("#hddAstroMRP").val(astromrp);
    $("#hddAstroDiscount").val(astrodiscount);
    $("#hddAstroDiscountedPrice").val(astrodiscountedprice);
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

    $("#hddConsultionmedium").val(type);
    if (type == 'Upcoming') {

        $("#txtPhoneNumber").val('');
        $("#txtWhatsappNumber").val('');
        $("#spnSkypeId").val('');

        $("#txtHomeAddress").val('');
        $("#txtLandMark").val('');
        $("#txtPinCodes").val('');
        GetState(0);
        $("#ddDistrict").html('<option value="0"> All </option>');
    } else if (type == 'Completed') {

        $("#txtPhoneNumber").val('');
        $("#txtWhatsappNumber").val('');
        $("#spnSkypeId").val('');

        $("#txtHomeAddress").val('');
        $("#txtLandMark").val('');
        $("#txtPinCodes").val('');


    } else if (type == 'Cancelled') {
        $("#txtPhoneNumber").val('');
        $("#txtWhatsappNumber").val('');
        $("#spnSkypeId").val('');

        $("#txtHomeAddress").val('');
        $("#txtLandMark").val('');
        $("#txtPinCodes").val('');
    }
}

function RedirectToIndex(idx) {
    location.href = $_RedirectToIndex;
}

function toTimestamp(year, month, day) {
    
    var datum = new Date(Date.UTC(year, month - 1, day));
    if (year === undefined) return new Date();
    else return datum;//.getTime() / 1000;
}