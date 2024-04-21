function GetUsersData() {
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
            "role_id": $("#MasterPageType").val()
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
            }
            else {
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
        data:
        {
            'search': $("#txt_search").val().trim(),
            "role_id": $("#MasterPageType option:selected").val(),
            "role_name": $("#MasterPageType option:selected").text()
        },
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (Rdata) {

            debugger;
            var bytes = new Uint8Array(Rdata.FileContents);
            var blob = new Blob([bytes], { type: Rdata.ContentType});
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = Rdata.FileDownloadName;
            link.click();
        }
    });

}

function Details(user_id) {
    location.href = $_Details + "?user_id=" + user_id;
}

function Edit(user_id) {
    location.href = $_Edit + "?user_id=" + parseInt(user_id);
}

function OpenPopup(Ids,status) {
    ClearFields();
    $("#hddId").val(Ids);
    if (status == "Active") $("#active").prop("checked", true);
    else $("#Inactive").prop("checked", true);
}

function ClearFields() {
    $("#hddId").val('');
    $('#spnActivate').html('');
}


function ActivateDeactivate(isactive) {
    var tableControl = document.getElementById('WebGrid');
    var user_ids = [];
    var username = [];

    $('input:checkbox:checked', tableControl).each(function () {
        var newsId = parseInt($(this).val());
        var name = $(this).data('username');

        if (newsId !== null && newsId > 0) {
            user_ids.push({ Id: newsId.toString() });
        }
    });



    $.ajax({
        url: $_ActivateDeactivate,
        data: JSON.stringify({ 'user_ids': user_ids, 'IsActive': isactive }),
        type: 'POST',
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $("#modalconfirm").modal('hide');
            if (data > 0) {
                GetUsersData();
                if (isactive === 'Y') {
                    debugger;
                    displayMessage( 'Record(s) activated successfully', 'success');
                }
                else {
                    displayMessage('Record(s) deactivated successfully', 'success');
                }

            }
        },
        failure: function (response) {
            displayMessage("failure    " + response.responseText, 'warning');
        },
        error: function (response) {
            displayMessage("error   " + response.responseText, 'error');
        }


    });
    return true;
}

function Activate(isactive) {
    ConfirmationMsg('Are you sure, do you want to activate this user ?', function () {
        ActivateDeactivate(isactive);
    });
}


function Deactivate(dactivate) {
    ConfirmationMsg('Are you sure, do you want to deactivate this user ?', function () {
        ActivateDeactivate(dactivate);
    });
}

