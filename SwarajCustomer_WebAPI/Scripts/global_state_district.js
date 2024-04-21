function GetState(state_id) {

    $.ajax({
        url: $_GetStatetUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
   
            $("#ddState").html('<option value="0"> All </option>');
            $("#ddDistrict").html('<option value="0"> All </option>');
            var state = '';

            $.each(result, function (i, obj) {
                state = state + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });
            $(state).appendTo('#ddState');

            $("#ddState option").filter(function () {
                return $(this).val() == state_id;
            }).attr('selected', true);
        }
    });
}

function GetDistrict(Id, districtId) {

    $.ajax({
        url: $_GetDistrictUrl,
        data: {
            'Ids': parseInt(Id)
        },
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
      
            $("#ddDistrict").html('<option value="0"> All </option>');
            var District = '';
            $.each(result, function (i, obj) {
                District = District + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });

            $(District).appendTo('#ddDistrict');
            $("#ddDistrict option").filter(function () {
                return $(this).val() == districtId;
            }).attr('selected', true);

        }
    });
}

function AddState(state_id) {

    $.ajax({
        url: $_GetStatetUrl,
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
        
            $("#AddState").html('<option value="0"> All </option>');
            var state = '';
            $.each(result, function (i, obj) {
                state = state + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });
            $(state).appendTo('#AddState');
            $("#AddState option").filter(function () {
                return $(this).val() == state_id;
            }).attr('selected', true);
        }
    });
}

function GetMultipleDistrict(Id, districtId) {
    debugger;
    $.ajax({
        url: $_GetDistrictUrl,
        data: {
            'Ids': parseInt(Id)
        },
        cache: false,
        type: 'GET',
        datatype: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $("#MultipleDistrict").html('');
            $("#MultipleDistrict option").remove();
            var bootstrap = '';
            debugger;
            $.each(result, function (i, obj) {
                bootstrap = bootstrap + "<option value=" + obj.Id + ">" + obj.Name + "</option>";
            });
            $(bootstrap).appendTo('#MultipleDistrict');

            $('#MultipleDistrict').multiselect('refresh');
            $('#MultipleDistrict').multiselect('rebuild');

            if (districtId.length > 0) {
                $('#MultipleDistrict').multiselect('select', districtId.split(','));
            }
        }
    });
}

//function GetMultipleDistrict(Id, districtId) {

//    $.ajax({
//        url: $_GetDistrictUrl,
//        data: {  'Ids': parseInt(Id)  },
//        cache: false,
//        type: 'GET',
//        datatype: 'html',
//        contentType: 'application/json; charset=utf-8',
//        success: function (result) {
//            debugger;
//            var multiselect = $("#MultipleDistrict").data("kendoMultiSelect");
//            multiselect.dataSource.filter({});
//            multiselect.value([]);
//            multiselect.refresh();
//            multiselect.setDataSource(result);

//            if (districtId.length > 0) {
//                multiselect.value(districtId.split(',')); //[314,316,317]
//            }

//            $("#select").removeClass("hide");
//            $("#deselect").removeClass("hide");
//        }
//    });
//}