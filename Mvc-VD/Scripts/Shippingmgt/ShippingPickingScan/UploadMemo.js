insertmaterial_info_memo = [];
json_object = "";
$(document).ready(function () {
    function changeDate(exdate) {
        var e0date = new Date(0);
        var offset = e0date.getTimezoneOffset();
        return new Date(0, 0, exdate - 1, 0, -offset, 0);
    };
    $("#imgupload").change(function (evt) {
        var selectedFile = evt.target.files[0];
        var reader = new FileReader();
        var excelData = [];
        reader.onload = function (event) {
            var data = event.target.result;
            var workbook = XLSX.read(data, { type: 'binary' });
            workbook.SheetNames.forEach(function (sheetName) {
                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName], { range:6 });
                json_object = XL_row_object;
            })
        };

        reader.onerror = function (event) {
            console.error("File could not be read! Code " + event.target.error.code);
        };
        reader.readAsBinaryString(selectedFile);
    });
});
$("#uploadBtn").click(function () {
   
    if ($("#imgupload").val() == "") { return false; }
    $('#loading').show();
    var data_create = 0;
    var data_update = 0;
    var data_error = 0;
    var data_pagedata = -1;
    var model = "";
    var product_nm = "";
    var product_cd = "";
    for (var i = 0; i < json_object.length; i++) {
        if (json_object[i]["__EMPTY_2"] != undefined) {
            model = json_object[i]["__EMPTY_2"];
        }
        if (json_object[i]["__EMPTY_3"] != undefined) {
            product_nm = json_object[i]["__EMPTY_3"];
        }
        if (json_object[i]["__EMPTY_4"] != undefined) {
            product_cd = json_object[i]["__EMPTY_4"];
        }
        console.log(json_object[i]["__EMPTY_5"]);
        if (json_object[i]["__EMPTY_5"] != undefined ) {
            item = {
                sd_no: $("#sd_mm").val(),
                md_cd: model,
                style_nm: product_nm,
                style_no: product_cd,
                mt_cd: json_object[i]["__EMPTY_5"],
                width: json_object[i]["__EMPTY_6"],
                width_unit: json_object[i]["__EMPTY_7"],
                spec: json_object[i]["__EMPTY_9"],
                spec_unit: json_object[i]["__EMPTY_10"],
                month_excel: json_object[i]["__EMPTY_11"],
                lot_no: (json_object[i]["__EMPTY_13"] != undefined ? json_object[i]["__EMPTY_13"] : ""),
                TX: json_object[i]["__EMPTY_14"], 
                total_m: json_object[i]["__EMPTY_15"],
                total_m2: json_object[i]["__EMPTY_16"],
                total_ea: json_object[i]["__EMPTY_17"],
            }
            insertmaterial_info_memo.push(item);
        }
    }
    $.ajax({
        url: "/ShippingMgt/Upload_ExcelMemory",
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(insertmaterial_info_memo),
        traditonal: true,
        success: function (response) {
            $('#imgupload').val('');
            if (response.result) {
                data_create = data_create + response.data_create;
                data_update = data_update + response.data_update;
                data_error = data_error + response.data_error;
                data_pagedata = data_pagedata + 1;
                $("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");

                //var timer = setTimeout(
                //    function () {
                //        $("#result").html("");
                //        clearTimeout(timer)
                //    }, 12000);
                $('#list_create').clearGridData();
                $('#list_create').jqGrid('setGridParam', { search: true });
                var pdata = $('#list_create').jqGrid('getGridParam', 'postData');
                getData_primary(pdata);
              
            }
        },
    });
    $('#loading').hide();
});
