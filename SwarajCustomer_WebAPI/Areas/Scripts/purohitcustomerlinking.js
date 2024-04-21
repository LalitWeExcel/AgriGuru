function GetPurohitCustomerLinking() {
    $('#save_excel').prop('disabled', true);
    var $_NoOfRecords = 10;
    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }

    $("#divGrid").html(gbl_loader_html);
    $.ajax({
        url: $_GetUsersUrl,
        data:
        {
            'page': current_page > 0 ? current_page : $("#page").val(),
            'noofrecords': $_NoOfRecords,
            'search': $("#txt_search").val().trim(),
            "user_id": $("#MasterPageType").val()
        },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $("#divGrid").html('');
            $("#divGrid").html(data);
            if (gbl_row_count > 0) {

                debugger;
                $("#spnTotalProhits").html($("#hddTotalProhits").val());
                $("#spnTotalAstrologers").html($("#hddTotalAstrologers").val());
                $("#spnTotalCustomers").html($("#hddTotalCustomers").val());


                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', false);
            }
            else {

                $("#spnTotalProhits").html(0);
                $("#spnTotalAstrologers").html(0);
                $("#spnTotalCustomers").html(0);

                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");
                $('#save_excel').prop('disabled', true);
            }
        }
    });
}



function Details(user_id) {
    location.href = $_Details + "?user_id=" + user_id;
}



