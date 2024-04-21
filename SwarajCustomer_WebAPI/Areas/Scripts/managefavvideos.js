function GetManageFavVideos() {

    var $_NoOfRecords = 10;
    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }

    $("#divGrid").html(gbl_loader_html);

    $.ajax({
        url: $_GetManageFavVideosUrl,
        data: {
            'page': current_page > 0 ? current_page : $("#page").val(),
            'noofrecords': $_NoOfRecords,
            'search': $("#txt_search").val().trim(),
            'languageId': parseInt($("#ddLanguageId option:selected").val())
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
            } else {

                $("#divNoData").addClass("hide");
                $("#divGrid").removeClass("hide");

            }
        }
    });
}

function SaveManageFavVideos() {

    debugger;

    var isValid = true;
    var fileData = new FormData();
    var Ids = $("#hddId").val();
    var Name = $("#txtName").val().trim();
    var Description = $("#txtDescription").val().trim();
    var LanguageId = parseInt($("#LanguageId option:selected").val());
  
    if (Name == "") {
        $('#spnName').html('Please Enter Name !');
        $('#spnName').removeClass('hide');
        isValid = false;
    } else if (Description < 1 || Description > 100) {
        $('#spnDescription').html('Description should be max 100 digit.');
        $('#spnDescription').removeClass('hide');
        $('#spnName').html('');
        isValid = false;
    }
    else if (LanguageId == "0" || LanguageId == NaN) {
        $('#spnLanguage').html('Please Select Language');
        $('#spnLanguage').removeClass('hide');
        $('#spnDescription').html('');
        isValid = false;
    }

    if (window.FormData !== undefined) {
        var vedioUpload = $("#upload-image2").get(0);
        var video = vedioUpload.files;

        if (video.length > 0) {
            fileData.append('fileVideo', video[0]);
        } else {
            if (Ids == 0) {
                $('#spnImageName2').html('Please Select  vedio Thumbnail');
                $('#spnImageName2').removeClass('hide');
                $('#spnDiscountInRupees_Independent').html('');
                isValid = false;
            }
        }
    }


    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {


        fileData.append('AdvertisementId', Ids);
        fileData.append('Name', Name);
        fileData.append('Description', Description);
        fileData.append('LanguageId', LanguageId);

        $.ajax({
            url: $_SaveManageFavVideossUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                $('#logoutModal1').modal('hide');
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {

                        GetLanguages(0);
                        GetManageFavVideos();

                        displayMessage(data, 'success');
                    } else {
                        displayMessage(data, 'warning');
                        $("#loaderContainer").css("display", "none");

                    }
                    $("#btn_save").attr('disabled', false);
                } else {
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

function Delete(id) {
    ConfirmationMsg('Are you sure, you want to delete this Fav Video ?', function () {
        DeleteAdvertising(id);
    });
}

function DeleteAdvertising(id) {
    $("#loaderContainer").css("display", "block");
    $.ajax({
        url: $_Delete,
        type: 'POST',
        data: JSON.stringify({
            'id': id
        }),
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data > 0) {
                $("#modalconfirm").modal('hide');
                displayMessage('Record deleted successfully', 'success');
                GetManageFavVideos();
                setTimeout(function () {
                    $("#loaderContainer").css("display", "none");
                }, 2000);

            }
        }
    });
    return true;
}


function OpenPopup(Ids, type) {
    ClearFields();
    $("#hddId").val(Ids);

}

function ClearFields() {

    GetLanguages(0);

    $("#ModelTitle").html('Add Manage Fav Videos')
    $("#hddId").val('');
    $('#spnActivate').html('');
    $("#spnImageName").html('');
    $("#txtName").val('');
    $("#spnName").html('');
    $("#txtDescription").val('');
    $("#spnDescription").html('');
    $('#ImageThumbnail2').attr('src', $_vedio_icon);

}
////https://www.greennet.org.uk/support/understanding-file-sizes

$('#upload-image2').on('change', function (e) {

    $('#spnImageName2').html('');
    myfile = $(this).val();
    if (e.target.files.length === 0) {
        $(this).val('');
        $('#ImageThumbnail2').attr('src', $_vedio_icon);
        $("#picnamespn").text('');
        return;
    }
    var fname = e.target.files[0].name;
    var fsize = e.target.files[0].size;
    $("#picnamespn").html(fname);
    var ext = myfile.split('.').pop();
    var ext1 = '.' + ext.toLowerCase();

    if (vedio_ext.includes(ext1)) {
        var selected_file = $('#upload-image2').get(0).files[0];
        selected_file = window.URL.createObjectURL(selected_file);
        $('#ImageThumbnail2').attr('src', selected_file);
        // return true;
    } else {

        displayMessage('Only .mp4 file should get uploaded', 'warning');
        $(this).val('');
    }

    var less_then = fsize < vedio_size_minimum;
    var greater_then = fsize > vedio_size_maximum;

    if (less_then) {

        displayMessage('The uploaded vedio should be greater than or equal to 30 Kb', 'warning');
        $(this).val('');
        $('#ImageThumbnail2').attr('src', $_vedio_icon);
        $("#picnamespn").text('');
    } else if (greater_then) {

        displayMessage('The uploaded vedio should be less than or equal to 5 Mb', 'warning');
        $(this).val('');
        $('#ImageThumbnail2').attr('src', $_vedio_icon);
        $("#picnamespn").text('');
    }
});

function ActivateDeactivate(isactive) {
    var tableControl = document.getElementById('WebGrid');
    var ids = [];
    var username = [];

    $('input:checkbox:checked', tableControl).each(function () {
        var newsId = parseInt($(this).val());
        var name = $(this).data('username');

        if (newsId !== null && newsId > 0) {
            ids.push({
                Id: newsId.toString()
            });
        }
    });

    $.ajax({
        url: $_ActivateDeactivate,
        data: JSON.stringify({
            'advertising_ids': ids,
            'IsActive': isactive
        }),
        type: 'POST',
        datatype: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            $("#modalconfirm").modal('hide');
            if (data > 0) {
                GetManageFavVideos();
                if (isactive === 'Y') {
                    displayMessage('advertising ads activated successfully', 'success');
                } else {
                    displayMessage('advertising ads deactivated successfully', 'success');
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
    ConfirmationMsg('Are you sure, do you want to activate this vedio ?', function () {
        ActivateDeactivate(isactive);
    });
}

function Deactivate(dactivate) {
    ConfirmationMsg('Are you sure, do you want to deactivate this vedio ?', function () {
        ActivateDeactivate(dactivate);
    });
}

function GetLanguages(LanguageId) {
    $.ajax({
        url: $_GetLanguagesUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $("#LanguageId").html('<option value="0"> All </option>');
            $("#ddLanguageId").html('<option value="0"> All </option>');

            var Language = '';
            $.each(result, function (i, obj) { Language = Language + "<option value=" + obj.Id + ">" + obj.Name + "</option>"; });

            $(Language).appendTo('#LanguageId');
            $(Language).appendTo('#ddLanguageId');

            $("#LanguageId option").filter(function () { return $(this).text() == LanguageId }).attr('selected', true);
            $("#ddLanguageId option").filter(function () { return $(this).text() == LanguageId }).attr('selected', true);
        }
    });
}

