function GetMastersData() {

    var $_NoOfRecords = 10;
    if ($("#NoofRecords").length > 0) {
        $_NoOfRecords = $("#NoofRecords").val();
    }

    $("#divGrid").html(gbl_loader_html);

    $.ajax({
        url: $_GetMastersUrl,
        data: {
            'page': current_page > 0 ? current_page : $("#page").val(),
            'noofrecords': $_NoOfRecords,
            "categoryId": $("#MasterPageType option:selected").val()
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

function OpenPopup(Ids,
    combos_packages_category_id,
    combos_packages_main_product_id,
    CategoryName,
    Name,
    Description,
    MRP,
    Discount,
    Duration,
    IsActive) {

    debugger;
    $("#hddId").val(Ids);
    if (IsActive == "Active") $("#Active").prop("checked", true);
    else $("#Inactive").prop("checked", true);

    if (CategoryName === "Combos-Packages-Service") {
 
        $("#spnCombosPackages").html('Edit Combos-Packages-Service');
        $("#CombosPackagesName").val(Name);
        $("#CombosPackagesDescription").val(Description);
        $("#CombosPackagesMRP").val(MRP);
        $("#CombosPackagesDiscount").val(Discount);
        $("#CombosPackagesDuration").val(Duration);

        $("#CombosPackagesCatagory option").filter(function () {
            if ($(this).val() == combos_packages_category_id) {
                debugger;
                GetMasterCategorys(combos_packages_category_id, combos_packages_main_product_id)
                return $(this).val() == combos_packages_category_id;
            }
        }).attr('selected', true);
        $("#logoutModal3").modal('show');
    }
    else {
        $("#spnCatagoryName").html(CategoryName);
        $("#txtName").val(Name);
        $("#txtDescription").val(Description);
        $("#txtMRP").val(MRP);
        $("#txtDuration").val(Duration);
        $("#logoutModal1").modal('show');
    }
}

function ClearFields() {

    $("#hddId").val('');
    $("#spnCatagoryName").html('');
    $("#Active").prop("checked", false);
    $("#Inactive").prop("checked", false);
    $('#spnActivate').html('');
    $('#spnDiscount').html('');
    $("#txtName").val('');
    $("#txtDescription").val('');
    $("#txtMRP").val('');
    $("#spnName").html('');
    $("#spnDescription").html('');
    $("#spnMRP").html('');


    $('#1').html('');
    $('#2').html('');
    $('#3').html('');
    $('#4').html('');


    $("#AddName").val('');
    $("#AddDescription").val('');
    $("#AddMRP").val('');
    $("#AddDuration").val('');


    $('#MasterPageType option:eq(0)').prop('selected', true);
    $('#AddCatagory option:eq(0)').prop('selected', true);


    // Combos / Packages
    $("#CombosPackagesName").val('');
    $('#spnCombosPackagesName').html('');
    $("#CombosPackagesDescription").val('');
    $('#spnCombosPackagesDescription').html('');
    $("#CombosPackagesMRP").val('');
    $('#spnCombosPackagesMRP').html('');
    $("#CombosPackagesDiscount").val('');
    $('#spnCombosPackagesDiscount').html('');
    $("#CombosPackagesDuration").val('');
    $('#spnCombosPackagesDuration').html('');
    //$('#CombosPackagesCatagory option:eq(0)').prop('selected', true);
    //$('#CombosPackagesSubCatagory option:eq(0)').prop('selected', true);
}

function ManageMasters() {

    $("#hddId").val('');
    $("#spnCatagoryName").html('');
    $('#spnActivate').html('');
    $('#spnDiscount').html('');
    $("#txtName").val('');
    $("#txtDescription").val('');
    $("#txtMRP").val('');
    $("#spnName").html('');
    $("#spnDescription").val('');
    $("#spnMRP").html('');

    var selectValue = $("#MasterPageType option:selected").val();
    $("#AddCatagory option").filter(function () {
        return $(this).val() == selectValue;
    }).prop('selected', true);



    // Combos / Packages
    $("#CombosPackagesName").val('');
    $('#spnCombosPackagesName').html('');
    $("#CombosPackagesDescription").val('');
    $('#spnCombosPackagesDescription').html('');
    $("#CombosPackagesMRP").val('');
    $('#spnCombosPackagesMRP').html('');
    $("#CombosPackagesDiscount").val('');
    $('#spnCombosPackagesDiscount').html('');
    $("#CombosPackagesDuration").val('');
    $('#spnCombosPackagesDuration').html('');
    $('#CombosPackagesCatagory option:eq(0)').prop('selected', true);
    $('#CombosPackagesSubCatagory option:eq(0)').prop('selected', true);
}

function GetCategory() {
    $.ajax({
        url: $_GetCategory,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            $("#MasterPageType").html('<option value="0"> All </option>');
            $("#AddCatagory").html('');

            $("#CombosPackagesCatagory").html('<option value="0"> All </option>');
            $("#CombosPackagesSubCatagory").html('<option value="0"> All </option>');

            var catagory = '';
            var MasterPageType = '';

            $.each(result, function (i, obj) {
                catagory = catagory + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });

            $.each(result, function (i, obj) {
                if (obj.Name !== "Combos-Packages-Service") {
                    MasterPageType = MasterPageType + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
                }
            });

            $(catagory).appendTo('#MasterPageType');
            $(MasterPageType).appendTo('#AddCatagory');
            $(MasterPageType).appendTo('#CombosPackagesCatagory');

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
            $("#CombosPackagesSubCatagory").html('<option value="0"> All </option>');
            var subcatagory = '';

            $.each(result, function (i, obj) {
                subcatagory = subcatagory + "<option value=" + obj.Id + "  data-mrp=" + obj.MRP + " data-duration=" + obj.Duration + ">" + obj.Name + "</option>";
            });
            $(subcatagory).appendTo('#CombosPackagesSubCatagory');

            $("#CombosPackagesSubCatagory option").filter(function () {
                return $(this).val() == MainProductId;
            }).attr('selected', true);

        }
    });
}


function Save() {

    var isValid = true;
    var fileData = new FormData();
    var CategoryId = parseInt($("#AddCatagory option:selected").val());

    var Name = $("#AddName").val().trim();
    var Description = $("#AddDescription").val().trim();
    var MRP = $("#AddMRP").val().trim();
    var Duration = $("#AddDuration").val();

    if (Name === null || Name === '') {
        $('#1').html('Please Enter Name');
        $('#1').removeClass('hide');
        isValid = false;
    } else if (Name.length < 1 || Name.length > 50) {
        $('#1').html('Name should be max 50 characters in length.');
        $('#1').removeClass('hide');
        isValid = false;
    } else if (Description === null || Description === '') {
        $('#2').html('Please Enter Description');
        $('#2').removeClass('hide');
        $('#1').html('');
        isValid = false;
    } else if (Description.length < 1 || Description.length > 250) {
        $('#2').html('Code should be max 250 characters in length.');
        $('#2').removeClass('hide');
        $('#1').html('');
        isValid = false;
    } else if (MRP === null || MRP === '' || MRP < 1) {
        $('#3').html('Please Enter vaild MRP');
        $('#3').removeClass('hide');
        $('#2').html('');
        isValid = false;
    } else if (Duration === null || Duration === '') {
        $('#4').html('Please Enter vaild Duration');
        $('#4').removeClass('hide');
        $('#3').html('');
        isValid = false;
    } else if (Duration < 1 || Duration > 500) {
        $('#4').html('Duration should be max 500 digit.');
        $('#4').removeClass('hide');
        $('#3').html('');
        isValid = false;
    }
    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {

        fileData.append('CategoryId', CategoryId);
        fileData.append('Name', Name);
        fileData.append('Description', Description);
        fileData.append('MRP', MRP);
        fileData.append('Duration', parseInt(Duration));


        $.ajax({
            url: $_GetSaveUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                $('#logoutModal2').modal('hide');
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {
                        $("#MasterPageType option").filter(function () {
                            return $(this).val() == CategoryId;
                        }).prop('selected', true);
                        GetMastersData();
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

function Update() {

    var isValid = true;
    var fileData = new FormData();
    var Ids = $("#hddId").val();
    var Name = $("#txtName").val().trim();
    var Description = $("#txtDescription").val().trim();
    var MRP = $("#txtMRP").val();
    var Duration = $("#txtDuration").val();
    var selectValue = $("input[name='Status']:checked").val();


    if (Name === null || Name === '') {
        $('#spnName').html('Please Enter Name');
        $('#spnName').removeClass('hide');
        isValid = false;
    } else if (Name.length < 1 || Name.length > 50) {
        $('#spnName').html('Name should be max 50 characters in length.');
        $('#spnName').removeClass('hide');
        isValid = false;
    } else if (Description === null || Description === '') {
        $('#spnDescription').html('Please Enter Description');
        $('#spnDescription').removeClass('hide');
        $('#spnName').html('');
        isValid = false;
    } else if (Name === null || Name === '') {
        $('#spnName').html('Please Enter Name');
        $('#spnName').removeClass('hide');
        $('#spnDescription').html('');
        isValid = false;
    } else if (Name.length < 1 || Name.length > 50) {
        $('#spnName').html('Name should be max 50 characters in length.');
        $('#spnName').removeClass('hide');
        $('#spnDescription').html('');
        isValid = false;
    } else if (Description === null || Description === '') {
        $('#spnDescription').html('Please Enter Description');
        $('#spnDescription').removeClass('hide');
        $('#spnName').html('');
        isValid = false;
    } else if (MRP === null || MRP === '' || MRP < 1) {
        $('#spnMRP').html('Please Enter vaild MRP');
        $('#spnMRP').removeClass('hide');
        isValid = false;
    } else if (Duration === null || Duration === '') {
        $('#spnDuration').html('Please Enter vaild Duration');
        $('#spnDuration').removeClass('hide');
        $('#spnMRP').html('');
        isValid = false;
    } else if (Duration < 1 || Duration > 500) {
        $('#spnDuration').html('Duration should be max 500 digit.');
        $('#spnDuration').removeClass('hide');
        $('#spnMRP').html('');
        isValid = false;
    }


    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {
        fileData.append('CategoryId', parseInt(Ids));
        fileData.append('Name', Name);
        fileData.append('Description', Description);
        fileData.append('MRP', MRP);
        fileData.append('Duration', parseInt(Duration));
        fileData.append('IsActive', selectValue);


        $.ajax({
            url: $_GetUpdateUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                if (data > 0) {
                    GetMastersData();
                    $("#logoutModal1").modal('hide');
                    if (selectValue == 'Y') {
                        displayMessage('Record update successfully', 'success');
                    } else {
                        displayMessage('Record update successfully', 'success');
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
    }
    return true;
}

function exportInExcel() {

    var Ids = parseInt($("#MasterPageType option:selected").val());
    var ServiceName = $("#MasterPageType option:selected").text().trim();
    if (Ids == 0) {
        ServiceName = "";
    }

    $.ajax({
        url: $_ExcelDownLoadUrl,
        data: {
            "Ids": Ids,
            "ServiceName": ServiceName
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


function SaveCombosPackages() {

    var isValid = true;
    var Ids = $("#hddId").val();
    var fileData = new FormData();
    var combos_packages_category_id = $("#CombosPackagesCatagory option:selected").val();
    var combos_packages_main_product_id = $("#CombosPackagesSubCatagory option:selected").val();


    var Name = $("#CombosPackagesName").val().trim();
    var Description = $("#CombosPackagesDescription").val().trim();
    var MRP = $("#CombosPackagesMRP").val().trim();
    var Discount = $("#CombosPackagesDiscount").val().trim();
    var Duration = $("#CombosPackagesDuration").val();
    var selectValue = $("input[name='CombosPackagesStatus']:checked").val();


    if (combos_packages_category_id == "0") {
        $('#spnCombosPackagesCatagory').html('Please Select  Category');
        $('#spnCombosPackagesCatagory').removeClass('hide');
        isValid = false;
    }

    else if (combos_packages_main_product_id == "0") {
        $('#spnCombosPackagesSubCatagory').html('Please Select Sub Category');
        $('#spnCombosPackagesSubCatagory').removeClass('hide');
        $('#spnCombosPackagesCatagory').html('');
        isValid = false;
    }

    else if (Name === null || Name === '') {
        $('#spnCombosPackagesName').html('Please Enter Name');
        $('#spnCombosPackagesName').removeClass('hide');
        $('#spnCombosPackagesSubCatagory').html('');
        isValid = false;
    }
    else if (Name.length < 1 || Name.length > 50) {
        $('#spnCombosPackagesName').html('Name should be max 50 characters in length.');
        $('#spnCombosPackagesName').removeClass('hide');
        $('#spnCombosPackagesSubCatagory').html('');
        isValid = false;
    }
    else if (Description === null || Description === '') {
        $('#spnCombosPackagesDescription').html('Please Enter Description.');
        $('#spnCombosPackagesDescription').removeClass('hide');
        $('#spnCombosPackagesName').html('');
        isValid = false;
    }
    else if (Description.length < 1 || Description.length > 250) {
        $('#spnCombosPackagesDescription').html('Code should be max 250 characters in length.');
        $('#spnCombosPackagesDescription').removeClass('hide');
        $('#spnCombosPackagesName').html('');
        isValid = false;
    }
    else if (MRP === null || MRP === '' || MRP < 1) {
        $('#spnCombosPackagesMRP').html('Please Enter vaild MRP Amount');
        $('#spnCombosPackagesMRP').removeClass('hide');
        $('#spnCombosPackagesDescription').html('');
        isValid = false;
    }
    else if (Discount === null || Discount === '' ) {
        $('#spnCombosPackagesDiscount').html('Please Enter vaild Discount');
        $('#spnCombosPackagesDiscount').removeClass('hide');
        $('#spnCombosPackagesMRP').html('');
        isValid = false;
    }
    else if (Discount < 1 || Discount > 90) {
        $('#spnCombosPackagesDuration').html('Duration should be min 1% to  max 90 %.');
        $('#spnCombosPackagesDuration').removeClass('hide');
        $('#spnCombosPackagesDiscount').html('');
        isValid = false;
    }
    else if (Duration < 1 || Duration > 500) {
        $('#spnCombosPackagesDuration').html('Duration should be max 500 digit.');
        $('#spnCombosPackagesDuration').removeClass('hide');
        $('#spnCombosPackagesDiscount').html('');
        isValid = false;
    }
    if (!isValid) {
        $(window).scrollTop(0);
        return false;
    } else {


        fileData.append('Id', Ids);
        fileData.append('combos_packages_category_id', combos_packages_category_id);
        fileData.append('combos_packages_main_product_id', combos_packages_main_product_id);

        fileData.append('Name', Name);
        fileData.append('Description', Description);
        fileData.append('IsActive', selectValue);
        fileData.append('MRP', parseFloat(MRP));
        fileData.append('Discount', parseFloat(Discount));
        fileData.append('Duration', parseInt(Duration));


        $.ajax({
            url: $_GetSaveUpdateCombosPackagesUrl,
            data: fileData,
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data,
            success: function (data) {

                $('#logoutModal3').modal('hide');
                if (data !== null && data.indexOf('html') < 0) {
                    if (data.indexOf('successfully') >= 0) {
                        GetMastersData();
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

$("#CombosPackagesCatagory").change(function () {
    GetMasterCategorys($(this).val(), 0)
});