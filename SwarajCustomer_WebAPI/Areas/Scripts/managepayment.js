function GetManagePaymentData() {

    $('#save_excel').prop('disabled', true);
    var $_NoOfRecords = 10;
    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }

    $("#divGrid").html(gbl_loader_html);

    $.ajax({
        url: $_GetManagePaymentUrl,
        data: {
            'page': current_page > 0 ? current_page : $("#page").val(),
            'noofrecords': $_NoOfRecords,
            'startdate': $("#startdate_id").val(),
            'enddate': $("#enddate_id").val(),
            'paymentstatus' : $('#ddPaymentStatus option:selected').val(),
            'paymentmode' : $('#ddPaymentMode option:selected').val(),
            'search': $("#txt_search").val().trim(),
            "state_id": $("#ddState option:selected").val(),
            "district_id": $("#ddDistrict option:selected").val()

        },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

           
            $("#divGrid").html(data);
            if (gbl_row_count > 0) {

                $("#spnTotelSuccessPayment").html($("#hddTotelSuccessPayment").val());
                $("#spnTotelFailedPayment").html($("#hddTotelFailedPayment").val());
                $("#spnTotelRevenue").html($("#hddTotelRevenue").val());

                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', false);
            } else {
                $("#spnTotelSuccessPayment").html(0);
                $("#spnTotelFailedPayment").html(0);
                $("#spnTotelRevenue").html(0.00);


                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', true);
            }
        }
    });
}

function exportInExcel() {

    $.ajax({
        url: $_ExcelDownLoadUrl,
        data: {
            'startdate': $("#startdate_id").val(),
            'enddate': $("#enddate_id").val(),
            'paymentstatus': $('#ddPaymentStatus option:selected').val(),
            'paymentmode': $('#ddPaymentMode option:selected').val(),
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

function ClearFields() {
    $("#divGrid").html('');

    $("#spnTotelSuccessPayment").html('');
    $("#spnTotelFailedPayment").html('');
    $("#spnTotelRevenue").html('');

    $("#hddTotelSuccessPayment").val('');
    $("#hddTotelFailedPayment").val('');
    $("#hddTotelRevenue").val('');

    $('#ddState option:eq(0)').prop('selected', true);
    $('#ddDistrict option:eq(0)').prop('selected', true);
 
}

$(function () {

    //fillter state
    $("#ddState").change(function () {
        GetDistrict($(this).val(), 0)
    });

});
