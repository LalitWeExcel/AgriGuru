
function SaveUpdate() {
    var isValid = true;
    var fileData = new FormData();
    var _multipleDistrictArray = [];

    var Ids = $("#hddId").val();
    var MRP = parseFloat($("#hddMRP").val());

    var CategoryId = parseInt($("#AddCatagory option:selected").val());
    var MainProductId = parseInt($("#SubCatagory option:selected").val());

    var Discount = $("#txtDiscount").val().trim();
    var DiscountInRupees = parseInt($("#txtDiscountInRupees").val().trim());
    var DiscountType = $("#DiscountType option:selected").val();

    var Description = $("#txtDescription").val().trim();
    var Type = $("input[name='Status']:checked").val();
    var IsGlobal = $("input[name='IsGlobal']:checked").val();

    var Stateid = $("#AddState option:selected").val();
    var StateName = $("#AddState option:selected").text().trim();

    $("#MultipleDistrict option:selected").each(function () {
        _multipleDistrictArray.push({
            'DistrictId': parseInt($(this).val()),
            'DistrictName': $(this).text()
        });
    });

    var BookingDate = $("#startdate_id").val().trim();
    var TimeSlotid = $("#AddTimeSlot option:selected").val();
    var Url = $("#txturl").val();
    debugger;

    if (CategoryId == "0") {
        $('#spnActivate').html('Please Select Catagory!');
        $('#spnActivate').removeClass('hide');
        isValid = false;
    }
    else if (MainProductId == "0") {
        $('#spnSubCatagory').html('Please Select sub Catagory!');
        $('#spnSubCatagory').removeClass('hide');
        $('#spnActivate').addClass('hide');
        isValid = false;
    }
    else if (DiscountType === "percentage" && Discount < 1 || Discount > 100) {
        $('#spnDiscount').html('Discount should be max 100 %.');
        $('#spnDiscount').removeClass('hide');
        $('#spnSubCatagory').html('');
        $('#spnActivate').html('');
        DiscountInRupees = 0;
        isValid = false;
    }
    else if (DiscountType === "amount" && DiscountInRupees > MRP) {
        $('#spnDiscountInRupees').html('Discount In Rupees should be less then MRP amount ' + MRP);
        $('#spnDiscountInRupees').removeClass('hide');
        $('#spnSubCatagory').html('');
        $('#spnActivate').html('');
        Discount = 0;
        isValid = false;
    }
    else if (Description == "" || Description < 1 || Description > 100) {
        $('#spnDescription').html('Description should be max 100 digit.');
        $('#spnDescription').removeClass('hide');
        $('#spnDiscount').html('');
        $('#spnDiscountInRupees').html('');
        $('#spnActivate').html('');
        isValid = false;
    }
    else if (IsGlobal === "Y" && Type === "E") {
        if (!checkUrl(Url)) {
            $('#spnUrl').html('Please Enter vaild E Puja Url!');
            $('#spnUrl').removeClass('hide');
            $('#spnTimeSlot').html('');
            $('#spnDescription').html('');
            $('#spnActivate').html('');
            isValid = false;
        }
    }
    else if (IsGlobal === "N" && Type === "L") {
        if (Stateid == "0") {
            $('#spnState').html('Please Select State !!');
            $('#spnState').removeClass('hide');
            $('#spnActivate').html('');
            $('#spnTimeSlot').html('');
            $('#spnDescription').html('');
            isValid = false;
        }
        else if (_multipleDistrictArray.length < 1) {
            $('#spnDistrict').html('Please Select District !!');
            $('#spnDistrict').removeClass('hide');
            $('#spnActivate').html('');
            $('#spnTimeSlot').html('');
            $('#spnDescription').html('');
            $('#spnState').html('');
            isValid = false;
        }
    }
    if (window.FormData !== undefined) {
        var fileUpload = $("#upload-image").get(0);
        var files = fileUpload.files;
        if (files.length > 0) {
            fileData.append(files[0].name, files[0]);
        }
        else {
            if (Ids == 0) {
                $('#spnImageName').html('Please Select  Image Thumbnail');
                $('#spnImageName').removeClass('hide');
                $('#spnDescription').html('');
                isValid = false;
            }
        }
    }

    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        if (Type === "L") { BookingDate = ""; TimeSlotid = ""; Url = ""; }
        if (Type === "E") { Stateid = 0; StateName = ""; _multipleDistrictArray = []; }
        if (IsGlobal === "N" && Type === "E") { IsGlobal = "Y" }

        fileData.append('AdvertisementId', Ids);
        fileData.append('CategoryId', CategoryId);
        fileData.append('MainProductId', MainProductId);
        fileData.append('Discount', parseFloat(Discount));
        fileData.append('DiscountInRupees', parseInt(DiscountInRupees));
        fileData.append('Description', Description);
        fileData.append('Type', Type);
        fileData.append('IsGlobal', IsGlobal);
        fileData.append('StateName', StateName);
        fileData.append('StateId', parseInt(Stateid));
        fileData.append('BookingDate', BookingDate);
        fileData.append('TimeSlotid', parseInt(TimeSlotid));
        fileData.append('Url', Url);
        fileData.append('MuiltipleDistrictList', JSON.stringify(_multipleDistrictArray));


        $.ajax({
            url: $_GetSaveUpdateUrl,
            data: fileData,
            type: "POST",
            dataType: "JSON",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                $('#logoutModal2').modal('hide');
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {
                        displayMessage(data, 'success');
                        setTimeout(function () {
                            debugger;
                            ReturnToIndex();
                        }, 2000);
                    }
                    else {
                        displayMessage(data, 'warning');
                        $("#loaderContainer").css("display", "none");

                    }
                    $("#btn_save").attr('disabled', false);
                }
                else {
                    displayMessage(data, 'error');
                    $("#btn_save").attr('disabled', false);
                    $("#loaderContainer").css("display", "none");
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

function ReturnToIndex() {
    location.href = $_IndexUrl;
}

function Get(Id) {

    $.ajax({
        url: $_GetUrl,
        data: { 'Ids': parseInt(Id) },
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            GetMasterCategorys(result.CategoryId, result.MainProductId);
            GetCategorys(result.CategoryId);

            $('#AddCatagory').val(result.CategoryId);
            $('#SubCatagory').val(result.MainProductId);



            AddState(result.StateId);
            GetMultipleDistrict(result.StateId, result.DistrictId);
            $('#AddState').val(result.StateId);


            $('#ImageThumbnail').attr('src', result.ImageName);
            $('#spnMRP').html(result.MRP);
            $('#hddMRP').val(result.MRP);

            $('#spnduration').html(result.Duration);
            $('#txtDiscount').val(result.Discount);
            $('#txtDiscountInRupees').val(result.DiscountInRupees);
            $('#txtDescription').val(result.Description);

            var Discount = $('#txtDiscount').val();
            var DiscountInRupees = $('#txtDiscountInRupees').val();

            if (Discount != 0) {
                $('.percentage').removeClass('hide')
                $('.amount').addClass('hide')
                $('#DiscountType option:eq(0)').prop('selected', true);
            }
            else if (DiscountInRupees != 0) {
                $('.percentage').addClass('hide')
                $('.amount').removeClass('hide')
                $('#DiscountType option:eq(1)').prop('selected', true);
            }

            // local && e puja ads
            if (result.Type == "L") {
                $('input[type=radio][name=Status][id=Yes]').prop("checked", true);

                $(".divEPuja").addClass("hide");
                $(".divStateCity").removeClass("hide");
                $(".divIsGlobal").removeClass("hide");
            }
            else {
                $('input[type=radio][name=Status][id=No]').prop("checked", true);
                $(".divEPuja").removeClass("hide");
                $(".divStateCity").addClass("hide");
                $(".divIsGlobal").addClass("hide");
            }

            // IsGlobal ads

            if (result.IsGlobal == "Y") {
                $('input[type=radio][name=IsGlobal][id=Yes]').prop("checked", true);
                $(".divStateCity").addClass("hide");

                if (result.Type == "E") {

                    var date = new Date(result.BookingDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
                    $('#startdate_id').data("DateTimePicker").setDate('');
                    $('#startdate_id').data("DateTimePicker").setDate(date);
                    $('#AddTimeSlot').val(result.TimeSlotid)
                    $('#txturl').val(result.Url);
                }
            }
            else {
                $('input[type=radio][name=IsGlobal][id=No]').prop("checked", true);
                $(".divStateCity").removeClass("hide");
            }

        }
    });
}

function GetCategorys(Id) {
    $.ajax({
        url: $_GetCategorysUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $("#AddCatagory").html('<option value="0"> All </option>');
            $("#SubCatagory").html('<option value="0"> All </option>');
            var catagory = '';
            $.each(result, function (i, obj) {
                catagory = catagory + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });

            $(catagory).appendTo('#AddCatagory');
            $("#AddCatagory option").filter(function () {
                return $(this).val() == Id;
            }).attr('selected', true);
        }
    });
}

function GetMasterCategorys(Id, MainProductId) {

    $.ajax({
        url: $_GetMasterCategorys,
        data: { 'Ids': parseInt(Id) },
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#SubCatagory").html('<option value="0"> All </option>');
            var subcatagory = '';

            $.each(result, function (i, obj) {
                subcatagory = subcatagory + "<option value=" + obj.Id + "  data-mrp=" + obj.MRP + " data-duration=" + obj.Duration + ">" + obj.Name + "</option>";
            });
            $(subcatagory).appendTo('#SubCatagory');
            $("#SubCatagory option").filter(function () {
                return $(this).val() == MainProductId.toString();
            }).attr('selected', true)




        }
    });
}

function GetTimeSlot() {
    debugger;
    $.ajax({
        url: $_GetTimeSlotUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $("#AddTimeSlot").html('');
            var catagory = '';
            $.each(result, function (i, obj) {
                catagory = catagory + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });
            $(catagory).appendTo('#AddTimeSlot');
        }
    });
}

$(function () {

    //Add Edit popop state
    $("#AddState").change(function () {
        GetMultipleDistrict($(this).val(), 0);
    });


    $("#AddCatagory").change(function () {
        GetMasterCategorys($(this).val(), 0)
    });

    $("#DiscountType").change(function () {
        var value = $(this).val();
        $('#txtDiscount').val('');
        $('#txtDiscountInRupees').val('');

        if (value === "percentage") {
            $('.percentage').removeClass('hide')
            $('.amount').addClass('hide')
        }
        else if (value === "amount") {
            $('.percentage').addClass('hide')
            $('.amount').removeClass('hide')
        }
    });


    $("#SubCatagory").change(function () {

        $('#spnMRP').html("0.00");
        $('#spnduration').html("0");
        $('#hddMRP').val('0.00');
        var mrp = $(this).find('option:selected').attr("data-mrp");

        $('#spnMRP').html(mrp);
        $('#hddMRP').val(mrp);
        $('#spnduration').html($(this).find('option:selected').attr("data-duration"));
    });

    $('input[type=radio][name=Status]').change(function () {
        //  for Is Global false always 
        $('input[type=radio][name=IsGlobal][id=No]').prop("checked", true);

        if (this.id == 'Yes') {
            $(".divEPuja").addClass("hide");
            $(".divStateCity").removeClass("hide");
            $(".divIsGlobal").removeClass("hide");
        }
        else if (this.id == 'No') {

            $(".divEPuja").removeClass("hide");
            $(".divStateCity").addClass("hide");
            $(".divIsGlobal").addClass("hide");
        }

    });

    $('input[type=radio][name=IsGlobal]').change(function () {
        if (this.id == 'Yes') {
            $(".divStateCity").addClass("hide");
        }
        else if (this.id == 'No') {
            $(".divStateCity").removeClass("hide");
        }
    });

});

function checkUrl(url) {
    var pattern = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;
    if (pattern.test(url)) {
        return true;
    } else {
        return false;
    }
}

////https://www.greennet.org.uk/support/understanding-file-sizes
$('#upload-image').on('change', function (e) {

    $('#spnImageName').html('');
    myfile = $(this).val();
    if (e.target.files.length === 0) {
        $(this).val('');
        $('#ImageThumbnail').attr('src', $_Image);
        $("#picnamespn").text('');
        return;
    }
    var fname = e.target.files[0].name;
    var fsize = e.target.files[0].size;
    $("#picnamespn").html(fname);
    var ext = myfile.split('.').pop();
    var ext1 = '.' + ext.toLowerCase();
    if (img_ext.includes(ext1)) {
        var selected_file = $('#upload-image').get(0).files[0];
        selected_file = window.URL.createObjectURL(selected_file);
        $('#ImageThumbnail').attr('src', selected_file);
        // return true;
    } else {

        displayMessage('Only jpeg,jpg,png file should get uploaded', 'warning');
        $(this).val('');
    }


    var less_then = fsize < img_size_minimum;
    var greater_then = fsize > img_size_maximum;

    if (less_then) {

        displayMessage('The uploaded image should be greater than or equal to 30 Kb', 'warning');
        $(this).val('');
        $('#ImageThumbnail').attr('src', $_Image);
        $("#picnamespn").text('');
    }
    else if (greater_then) {

        displayMessage('The uploaded image should be less than or equal to 5 Mb', 'warning');
        $(this).val('');
        $('#ImageThumbnail').attr('src', $_Image);
        $("#picnamespn").text('');
    }
});


function ClearFields() {
    debugger;
    $("#title").html('Add')
    $("#hddId").val('');
    $('#spnDiscount').html('');
    $('#spnActivate').html('');
    $('#spnSubCatagory').html('');
    $("#spnDescription").html('');
    $("#spnImageName").html('');
    $("#spnDiscountInRupees").html('');

    $('#hddMRP').val('');
    $('#spnMRP').html("0.00");
    $('#spnduration').html("0");
    $('#spnUrl').html('');
    $("#txturl").val('');
    $("#txtDiscount").val('');
    $("#txtDescription").val('');
    $("#txtDiscountInRupees").val('');


    $('#ImageThumbnail').attr('src', $_Image);
    $('#DiscountType option:eq(0)').prop('selected', true);
    $('.amount').addClass('hide');
    $("#MultipleDistrict option").remove();
}