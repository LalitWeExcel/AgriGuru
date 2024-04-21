function ReturnToIndex() {
	location.href = $_IndexUrl;
}

function GetProfile() {

	ClearFields();

	var IsActive = $("#hddIsActive").val().trim();
	if (IsActive === "Active") {
		$('input[type=radio][name=is_active][id=active]').prop("checked", true);
	} else if (IsActive === "Inactive") {
		$('input[type=radio][name=is_active][id=Inactive]').prop("checked", true);
	}

	var export_in_pooja = $("#hddExpotInPooja").val().trim();
	if (export_in_pooja === "Y") {
		$('input[type=radio][name=export_in_pooja][id=Yes]').prop("checked", true);
	} else if (export_in_pooja === "N") {
		$('input[type=radio][name=export_in_pooja][id=No]').prop("checked", true);
	}


	var ready_for_travel = $("#hddReadyForTravel").val().trim();
	if (ready_for_travel === "Y") {
		$('input[type=radio][name=ready_for_travel][id=Yes]').prop("checked", true);
	} else if (ready_for_travel === "N") {
		$('input[type=radio][name=ready_for_travel][id=No]').prop("checked", true);
	}


	var marital_status = $("#hddMaritalStatus").val().trim();
	if (marital_status === "Married") {
		$('input[type=radio][name=marital_status][id=Yes]').prop("checked", true);
	} else {
		$('input[type=radio][name=marital_status][id=No]').prop("checked", true);
	}


	var health_status = $("#hddHealthStatus").val().trim();
	if (health_status == 1) {
		$('input[type=radio][name=health_status][id=Yes]').prop("checked", true);
	} else if (health_status == 0) {
		$('input[type=radio][name=health_status][id=No]').prop("checked", true);
	}

	var gender = $("#hddGender").val().trim();
	if (gender == "M") {
		$('input[type=radio][name=gender][id=Yes]').prop("checked", true);
	} else {
		$('input[type=radio][name=gender][id=No]').prop("checked", true);
	}

	var dob = $("#hddDOB").val();
	var tob = $("#hddTOB").val();
	if (tob == "") {
		tob = "12:00:AM"
	}

	var _d = dob.split("-");
	var defaultDate = toTimestamp(_d[2], _d[1], _d[0]);

	var d = new Date();
	var currMonth = d.getMonth();
	var currYear = d.getFullYear();
	var currDate = d.getDate();
	var startDate = new Date(currYear-10, currMonth, currDate);

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

	$('#ImageThumbnail').attr('src', $_ImageName);


	GetState($_mst_state_id);
	setTimeout(function () {
		GetDistrict($_mst_state_id, $_mst_district_id);
	}, 2000);

	$("#ddState").val($_mst_state_id);
	$("#ddDistrict").val($_mst_district_id);

}

function toTimestamp(year, month, day) {
	var datum = new Date(Date.UTC(year, month - 1, day));
	if (year === undefined) return new Date();
	else return datum; //.getTime() / 1000;
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

function Update() {
	var isValid = true;
	var fileData = new FormData();
	var adm_user_id = $("#hddadm_user_id").val();
	var username = $("#txtusername").val();
	var description = $("#txtdescription").val();
	var Status = $("input[name='is_active']:checked").val();
	var first_name = $("#txtfirst_name").val();
	var last_name = $("#txtlast_name").val();
	var whatsapp_no = $("#txtwhatsapp_no").val();
	var address = $("#txtaddress").val();
	var city = $("#txtcity").val();
	var State_id = parseInt($("#ddState option:selected").val());
	var District_id = parseInt($("#ddDistrict option:selected").val());
	var pincode = $("#txtpincode").val();
	var landmark = $("#txtlandmark").val();
	var gender = $("input[name='gender']:checked").val();
	var marital_status = $("input[name='marital_status']:checked").val();
	var DateOfBirth = $("#DateOfBirth").val();
	var BirthTime = $("#BirthTime").val();

	var PlaceOfBirth = $("#txtPlaceOfBirth").val();
	var aadhar_number_name = $("#txtaadhar_number_name").val();
	var profession = $("#txtprofession").val();
	var health_status = $("input[name='health_status']:checked").val();
	debugger;
	if (window.FormData !== undefined) {
		var fileUpload = $("#upload-image").get(0);
		var files = fileUpload.files;
		if (files.length > 0) {
			fileData.append(files[0].name, files[0]);
		} else {
			if (adm_user_id == 0) {
				$('#spnImageName').html('Please Select  Image Thumbnail');
				$('#spnImageName').removeClass('hide');
				isValid = false;
			}
		}
	} else if (username == "") {
		$('#spnusername').html('User Name Required');
		$('#spnusername').removeClass('hide');
		$('#spnImageName').addClass('hide');
		isValid = false;
	} else if (username > 50) {
		$('#spnusername').html('User Name Must be under 50 characters');
		$('#spnusername').removeClass('hide');
		$('#spnImageName').addClass('hide');
		isValid = false;
	} else if (description == "") {
		$('#spndescription').html('Description Required"');
		$('#spndescription').removeClass('hide');
		$('#spnusername').addClass('hide');
		isValid = false;
	} else if (description > 50) {
		$('#spndescription').html('Description Must be under 50 characters');
		$('#spndescription').removeClass('hide');
		$('#spnusername').addClass('hide');
		isValid = false;
	} else if (Status == undefined) {
		$('#spmis_active').html('Please Select Status');
		$('#spmis_active').removeClass('hide');
		$('#spnlast_name').addClass('hide');
		isValid = false;
	} else if (first_name == "") {
		$('#spnfirst_name').html('First Name Required');
		$('#spnfirst_name').removeClass('hide');
		$('#spnusername').addClass('hide');
		isValid = false;
	} else if (first_name > 50) {
		$('#spnfirst_name').html('First Name Must be under 50 characters');
		$('#spnfirst_name').removeClass('hide');
		$('#spnusername').addClass('hide');
		isValid = false;
	} else if (last_name == "") {
		$('#spnlast_name').html('Last Name Required');
		$('#spnlast_name').removeClass('hide');
		$('#spnfirst_name').addClass('hide');
		isValid = false;
	} else if (last_name > 50) {
		$('#spnlast_name').html('Last Name Must be under 50 characters');
		$('#spnlast_name').removeClass('hide');
		$('#spnfirst_name').addClass('hide');
		isValid = false;
	} else if (whatsapp_no == "") {
		$('#spnwhatsapp_no').html('Last Name Required');
		$('#spnwhatsapp_no').removeClass('hide');
		$('#spnlast_name').addClass('hide');
		isValid = false;
	} else if (whatsapp_no > 10) {
		$('#spnwhatsapp_no').html('Whats - App Number Must be under 10 characters');
		$('#spnwhatsapp_no').removeClass('hide');
		$('#spnlast_name').addClass('hide');
		isValid = false;
	} else if (address == "") {
		$('#spnaddress').html('Address Required');
		$('#spnaddress').removeClass('hide');
		$('#spnwhatsapp_no').addClass('hide');
		isValid = false;
	} else if (address > 50) {
		$('#spnaddress').html('Address Must be under 50 characters');
		$('#spnaddress').removeClass('hide');
		$('#spnwhatsapp_no').addClass('hide');
		isValid = false;
	} else if (city == "") {
		$('#spncity').html('Address city');
		$('#spncity').removeClass('hide');
		$('#spnaddress').addClass('hide');
		isValid = false;
	} else if (city > 50) {
		$('#spncity').html('city Must be under 50 characters');
		$('#spncity').removeClass('hide');
		$('#spnaddress').addClass('hide');
		isValid = false;
	} else if (State_id === 0 || State_id === NaN || State_id === undefined) {
		debugger;
		$('#spnState').html('State Name Required');
		$('#spnState').removeClass('hide');
		$('#spnaddress').addClass('hide');
		isValid = false;
	} else if (District_id === 0 || District_id === NaN || District_id === undefined) {
		debugger;
		$('#spnDistrict').html('District Name Required');
		$('#spnDistrict').removeClass('hide');
		$('#spnState').addClass('hide');
		isValid = false;
	} else if (pincode == "") {
		$('#spnpincode').html('Pin Code Required');
		$('#spnpincode').removeClass('hide');
		$('#spnDistrict').addClass('hide');
		isValid = false;
	} else if (pincode > 8) {
		$('#spnpincode').html('Pin Code Must be under 6-8 characters');
		$('#spnpincode').removeClass('hide');
		$('#spnDistrict').addClass('hide');
		isValid = false;
	} else if (landmark == "") {
		$('#spnlandmark').html('Landmark Required');
		$('#spnlandmark').removeClass('hide');
		$('#spnDistrict').addClass('hide');
		isValid = false;
	} else if (landmark > 50) {
		$('#spnlandmark').html('Landmark Must be under 50 characters');
		$('#spnlandmark').removeClass('hide');
		$('#spnDistrict').addClass('hide');
		isValid = false;
	} else if (gender == undefined) {
		$('#spngender').html('Please Select Gender');
		$('#spngender').removeClass('hide');
		$('#spnlandmark').addClass('hide');
		isValid = false;
	} else if (marital_status == undefined) {
		$('#spnmarital_status').html('Please Select Marital Status');
		$('#spnmarital_status').removeClass('hide');
		$('#spngender').addClass('hide');
		isValid = false;
	} else if (DateOfBirth == "") {
		$('#spnDateOfBirth').html('Date of Birth Required');
		$('#spnDateOfBirth').removeClass('hide');
		$('#spnmarital_status').addClass('hide');
		isValid = false;
	} else if (BirthTime == "") {
		$('#spnBirthTime').html('Time of Birth Required');
		$('#spnBirthTime').removeClass('hide');
		$('#spnDateOfBirth').addClass('hide');
		isValid = false;
	} else if (BirthTime == "") {
		$('#spnBirthTime').html('Time of Birth Required');
		$('#spnBirthTime').removeClass('hide');
		$('#spnDateOfBirth').addClass('hide');
		isValid = false;
	} else if (PlaceOfBirth == "") {
		$('#spnPlaceOfBirth').html('Place of Birth Required');
		$('#spnPlaceOfBirth').removeClass('hide');
		$('#spnDateOfBirth').addClass('hide');
		isValid = false;
	} else if (PlaceOfBirth > 50) {
		$('#spnPlaceOfBirth').html('Landmark Must be under 50 characters');
		$('#spnPlaceOfBirth').removeClass('hide');
		$('#spnDateOfBirth').addClass('hide');
		isValid = false;
	} else if (aadhar_number_name == "") {
		$('#spnaadhar_number_name').html('Adhar Card Number is required');
		$('#spnaadhar_number_name').removeClass('hide');
		$('#spnPlaceOfBirth').addClass('hide');
		isValid = false;
	} else if (aadhar_number_name > 16) {
		$('#spnaadhar_number_name').html('Adhar Card Number Must be under 16 characters');
		$('#spnaadhar_number_name').removeClass('hide');
		$('#spnPlaceOfBirth').addClass('hide');
		isValid = false;
	} else if (profession == "") {
		$('#spnprofession').html('Profession is required');
		$('#spnprofession').removeClass('hide');
		$('#spnaadhar_number_name').addClass('hide');
		isValid = false;
	} else if (profession > 50) {
		$('#spnprofession').html('Profession Must be under 50 characters');
		$('#spnprofession').removeClass('hide');
		$('#spnaadhar_number_name').addClass('hide');
		isValid = false;
	} else if (health_status == "") {
		$('#spnhealth_status').html('Please Select health Status');
		$('#spnhealth_status').removeClass('hide');
		$('#spnprofession').addClass('hide');
		isValid = false;
	}

	if (!isValid) {
		$(window).scrollTop(0);
		return false;
	} else {

		debugger;
		fileData.append('adm_user_id', adm_user_id);
		fileData.append('username', username);
		fileData.append('description', description);
		fileData.append('is_active', Status);
		fileData.append('first_name', first_name);
		fileData.append('last_name', last_name);
		fileData.append('whatsapp_no', whatsapp_no);
		fileData.append('address', address);
		fileData.append('city', city);
		fileData.append('mst_state_id', State_id);
		fileData.append('mst_district_id', District_id);
		fileData.append('pincode', pincode);
		fileData.append('landmark', landmark);
		fileData.append('gender', gender);
		fileData.append('marital_status', marital_status);
		fileData.append('DOB', DateOfBirth);
		fileData.append('TOB', BirthTime);
		fileData.append('PlaceOfBirth', PlaceOfBirth);
		fileData.append('aadhar_number_name', aadhar_number_name);
		fileData.append('profession', profession);
		fileData.append('health_status', health_status);

		$.ajax({
			url: $_UpdateUrl,
			data: fileData,
			type: "POST",
			dataType: "JSON",
			contentType: false, // Not to set any content header
			processData: false, // Not to process data,
			success: function (data) {
				debugger;
				if (data !== null && data.indexOf('html') < 0) {
					if (data.indexOf('successfully') >= 0) {

						displayMessage(data, 'success');
						setTimeout(function () {
							debugger;
							ReturnToIndex();
						}, 2000);

					} else {
						displayMessage(data, 'warning');
						$("#loaderContainer").css("display", "none");

					}
					$("#btn_save").attr('disabled', false);
				} else {
					displayMessage(data, 'error');
					$("#btn_save").attr('disabled', false);
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
	} else if (greater_then) {

		displayMessage('The uploaded image should be less than or equal to 5 Mb', 'warning');
		$(this).val('');
		$('#ImageThumbnail').attr('src', $_Image);

		$("#picnamespn").text('');
	}
});