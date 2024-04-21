function GetManageOrderData() {
    $('#save_excel').prop('disabled', true);
    var $_NoOfRecords = 10;
    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }

    $("#divGrid").html(gbl_loader_html);
    $.ajax({
        url: $_GetManageOrderUrl,
        data: {
            'page': current_page > 0 ? current_page : $("#page").val(),
            'noofrecords': $_NoOfRecords,
            'startdate': $("#startdate_id").val(),
            'enddate': $("#enddate_id").val(),
            "status": $("#MasterPageType option:selected").val(),
            'search': $("#txt_search").val().trim(),
            "state_id": $("#ddState option:selected").val(),
            "district_id": $("#ddDistrict option:selected").val()
        },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $("#divGrid").html('');
            $("#divGrid").html(data);
            if (gbl_row_count > 0) {
                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', false);
            } else {
                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', true);
            }
        }
    });
}

function Update() {

    var isValid = true;
    var fileData = new FormData();
    var Ids = parseInt($("#hddId").val());
    var OrderNumber = $("#hddOrderNumber").val();
    var BookingType = $("#hddBookingType").val();
    var NewPurohitID = parseInt($("#ddProhit option:selected").val());
    var Ression = $("#txtRession").val().trim();
    var OldProhitID = parseInt($("#hhdOldProhitID").val());

    debugger;
    if (Ression == "" || Ression == null) {
        $('#spnRession').html('Please Enter Ression !');
        $('#spnRession').removeClass('hide');
        isValid = false;
    }
    else if (NewPurohitID == "0") {
        $('#spnprohit').html('Please Select Option!');
        $('#spnprohit').removeClass('hide');
        $('#spnRession').html('');
        isValid = false;
    }



    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        debugger;
        fileData.append('BookingID', Ids);
        fileData.append('OrderNumber', OrderNumber);
        fileData.append('BookingType', BookingType);
        fileData.append('NewPurohitID', NewPurohitID);
        fileData.append('Ression', Ression);
        fileData.append('OldProhitID', OldProhitID);

        $.ajax({
            url: $_UpdateUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                debugger;
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {

                        GetManageOrderData();
                        displayMessage(data, 'success');
                        $('#logoutModal2').modal('hide');
                    }
                    else {
                        displayMessage(data, 'warning');
                        $("#loaderContainer").css("display", "none");
                        $('#logoutModal2').modal('show');

                    }
                    $("#btn_save").attr('disabled', false);
                } else {
                    displayMessage(data, 'error');
                    $("#btn_save").attr('disabled', false);
                    $("#loaderContainer").css("display", "none");
                    $('#logoutModal2').modal('show');
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

function UpdatePackage() {

    var isValid = true;
    var fileData = new FormData();
    var PurohitArray = [];
    var Ids = parseInt($("#hddId").val());
    var OrderNumber = $("#hddOrderNumber").val();
    var NoOfPandit = parseInt($("#hddNoOfPandit").val());
    var OldProhitID = parseInt($("#hhdOldProhitID").val());

    $("#MultipleProhit option:selected").each(function () {
        PurohitArray.push($(this).val());
    });

    if (PurohitArray.length < NoOfPandit) {
        $('#spnProhit').html('Please Select total  "' + NoOfPandit + '"  Prohit!');
        $('#spnProhit').removeClass('hide');
        isValid = false;
    }

    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        fileData.append('BookingID', Ids);
        fileData.append('OrderNumber', OrderNumber);
        fileData.append('NewPurohitID', PurohitArray.join(','));
        fileData.append('OldProhitID', OldProhitID);

        $.ajax({
            url: $_UpdatePackageUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {
                 
                        GetManageOrderData();
                        displayMessage(data, 'success');
                        $('#logoutModal1').modal('hide');
                    }
                    else {
                        displayMessage(data, 'warning');
                        $("#loaderContainer").css("display", "none");
                        $('#logoutModal1').modal('show');
                    }
                    $("#btn_save").attr('disabled', false);
                } else {
                    displayMessage(data, 'error');
                    $("#btn_save").attr('disabled', false);
                    $("#loaderContainer").css("display", "none");
                    $('#logoutModal1').modal('show');
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

function ConfirmProhit(BookingID, OrderNumber, PurohitName, CustCity) {

    ConfirmationMsg('Are you sure you want to confirm ' + PurohitName + '-' +CustCity+ ' for this Booking?', function () {
        $.ajax({
            url: $_ConfirmProhitUrl,
            type: 'POST',
            data: JSON.stringify({
                'BookingID': BookingID,
                'OrderNumber': OrderNumber
            }),
            datatype: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                var n = data.includes("successfully");
                if (data.toLowerCase().indexOf("successfully") >= 0) {
                    $("#modalconfirm").modal('hide');
                    displayMessage(data, 'success');

                    GetManageOrderData();
                }
            }
        });
    });
}

function OpenPopup(BookingID, OrderNumber, OldProhitID, type, NoOfPandit) {

    ClearFields();
    debugger;

    $("#hddId").val(BookingID);
    $("#hddOrderNumber").val(OrderNumber);
    $("#hhdOldProhitID").val(OldProhitID);
    $("#hddNoOfPandit").val(NoOfPandit);
    $("#hddBookingType").val(type);

    GetProhit(OldProhitID, type, parseInt(NoOfPandit));

    if (parseInt(NoOfPandit) > 0) {
        $("#logoutModal1").modal('show');
    } else {
        $("#logoutModal2").modal('show');
    }
    if (type == "Astrologer") {
        $("#Modal2title").html('Assign Astrologer');
        $("#title").html('Available Astrologer');
    } else {
        $("#Modal2title").html('Assign Prohits');
        $("#title").html('Available Prohits');
    }
}

function GetProhit(OldProhitID, type, NoOfPandit) {

    $.ajax({
        url: $_GetProhitUrl,
        data: {
            'BookingType': type
        },
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#ddProhit").html('<option value="0"> -- All -- </option>');

            var prohit = '';
            $.each(result, function (i, obj) {
                // #temp Prohitid not be selected 
                if (OldProhitID != obj.value) {
                    prohit = prohit + "<option value=" + obj.value + ">" + obj.text + "</option>";
                }
            });
            $(prohit).appendTo('#ddProhit');

            var multiselect = $("#MultipleProhit").data("kendoMultiSelect");
            multiselect.value([]);
            multiselect.refresh();
            multiselect.setDataSource(result);
            multiselect.options.maxSelectedItems = NoOfPandit;

        }
    });
}

function ClearFields() {

    $("#Modal2title").html('');
    $("#title").html('')
    $("#hddId").val('');
    $("#hddOrderNumber").val('');
    $("#hhdExitProhitID").val('');
    $("#hddBookingType").val('');
    $("#hddNoOfPandit").val('');
    $('#txtRession').val('');
    $('#spnProhit').html('');
    $('#spnprohit').html('');
    
    $('#spnRession').html('');
    $('#ddProhit option:eq(0)').prop('selected', true);
    $('#ddState option:eq(0)').prop('selected', true);
    $('#ddDistrict option:eq(0)').prop('selected', true);

}

function exportInExcel() {

    $.ajax({
        url: $_ExcelDownLoadUrl,
        data: {
            'startdate': $("#startdate_id").val(),
            'enddate': $("#enddate_id").val(),
            "status": $("#MasterPageType option:selected").val(),
            'search': $("#txt_search").val().trim(),
            "state_id": $("#ddState option:selected").val(),
            "district_id": $("#ddDistrict option:selected").val(),
            "StateName": $("#ddState option:selected").text(),
            "DistrictName": $("#ddDistrict option:selected").text()
        },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (Rdata) {
            var bytes = new Uint8Array(Rdata.FileContents);
            var blob = new Blob([bytes], {
                type: Rdata.ContentType
            });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = Rdata.FileDownloadName;
            link.click();
        }
    });

}

function Details(i, o) {
    location.href = $_Details + "?i=" + i + "&o=" + o;
}

$(function () {
    $("#ddState").change(function () {
        GetDistrict($(this).val(), 0)
    });
});


//function GetMuiltipleProhit(ExitProhitID, type, NoOfPandit) {

//    $.ajax({
//        url: $_GetProhitUrl,
//        data: {
//            'BookingType': type
//        },
//        cache: false,
//        type: 'GET',
//        datatype: 'html',
//        contentType: 'application/json; charset=utf-8',
//        success: function (result) {

//            debugger;
//            var multiselect = $("#MultipleProhit").data("kendoMultiSelect");
//            multiselect.value([]);
//            multiselect.refresh();
//            multiselect.setDataSource(result);
//            multiselect.options.maxSelectedItems = 2; 


//            //var multiselect = $("#MultipleProhit").kendoMultiSelect({
//            //    dataTextField: "text",
//            //    dataValueField: "value",
//            //    placeholder: "Select Prohits",
//            //    autoClose: false,
//            //    maxSelectedItems: NoOfPandit,
//            //    dataSource: {
//            //        data: result
//            //    }
//            //}).data('kendoMultiSelect');

//        }
//    });
//}