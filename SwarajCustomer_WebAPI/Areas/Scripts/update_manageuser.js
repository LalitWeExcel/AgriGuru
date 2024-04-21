function GetProfile() {

    ClearFields();
    debugger;
    var IsActive = $("#hddIsActive").val().trim();
    if (IsActive === "Active") { $('input[type=radio][name=is_active][id=active]').prop("checked", true); }
    else if (IsActive === "Inactive") { $('input[type=radio][name=is_active][id=Inactive]').prop("checked", true); }

    var export_in_pooja = $("#hddExpotInPooja").val().trim();
    if (export_in_pooja === "Y") { $('input[type=radio][name=export_in_pooja][id=Yes]').prop("checked", true); }
    else if (export_in_pooja === "N") { $('input[type=radio][name=export_in_pooja][id=No]').prop("checked", true); }


    var ready_for_travel = $("#hddReadyForTravel").val().trim();
    if (ready_for_travel === "Y") { $('input[type=radio][name=ready_for_travel][id=Yes]').prop("checked", true); }
    else if (ready_for_travel === "N") { $('input[type=radio][name=ready_for_travel][id=No]').prop("checked", true); }


    var marital_status = $("#hddMaritalStatus").val().trim();
    if (marital_status === "Married") { $('input[type=radio][name=marital_status][id=Yes]').prop("checked", true); }
    else { $('input[type=radio][name=marital_status][id=No]').prop("checked", true); }


    var health_status = $("#hddHealthStatus").val().trim();
    if (health_status == 1) { $('input[type=radio][name=health_status][id=Yes]').prop("checked", true); }
    else if (health_status == 0) { $('input[type=radio][name=health_status][id=No]').prop("checked", true); }

    var gender = $("#hddGender").val().trim();
    if (gender == "M") { $('input[type=radio][name=gender][id=Yes]').prop("checked", true); }
    else { $('input[type=radio][name=gender][id=No]').prop("checked", true); }

    var dob = $("#hddDOB").val();
    var tob = $("#hddTOB").val(); if (tob == "") { tob = "12:00:AM" }

    var _d = dob.split("-");
    var defaultDate = toTimestamp(_d[2], _d[1], _d[0]);

    var d = new Date();
    var currMonth = d.getMonth();
    var currYear = d.getFullYear();
    var currDate = d.getDate();
    var startDate = new Date(currYear, currMonth, currDate);

    $("#DateOfBirth").datetimepicker({
        format: "DD-MM-YYYY",
        pickTime: false,
        autoclose: true,
        defaultDate: defaultDate,
        maxDate: moment()
    });

    $("#BirthTime").datetimepicker({
        format: 'HH:mm',
        pickTime: true,
        pickDate: false,
        defaultDate: tob,
    });


    debugger;
    GetState($_mst_state_id);
    GetDistrict($_mst_state_id, $_mst_district_id);

    $("#ddState").val($_mst_state_id);
    $("#ddDistrict").val($_mst_district_id);
    $('#ImageThumbnail').attr('src', $_Image);

}


function toTimestamp(year, month, day) {
    var datum = new Date(Date.UTC(year, month - 1, day));
    if (year === undefined) return new Date();
    else return datum;//.getTime() / 1000;
}


$(function () {

    $("#ddState").change(function () {
        GetDistrict($(this).val(), 0)
    });

});

function ClearFields() {
    $('#spnState').html('');
    $('#spnDistrict').html('');
}

$('#upload-image').on('change', function (e) {
    debugger;
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