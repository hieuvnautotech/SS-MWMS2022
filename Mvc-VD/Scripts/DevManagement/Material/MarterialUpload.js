insertmaterial_tmp = [];
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
                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName], { range: 5 });
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
   
    for (var i = 0; i < json_object.length; i++) {
        item = {
            Type: json_object[i]["Type"],
            Barcode: json_object[i]["Barcode Management"],
            MT_No: json_object[i]["MT No"],
            Name: json_object[i]["Name"],
            bundle_qty: json_object[i]["Bundle Qty"],
            bundle_unit: json_object[i]["Bundle Unit"],
            Width: json_object[i]["Width (mm)"],
            Length: json_object[i]["Length (M)"],
            sp_cd: json_object[i]["Supplier"],
        }
        insertmaterial_tmp.push(item);
    }
        $.ajax({
            url: "/DevManagement/insertMaterialTemp",
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(insertmaterial_tmp),
            traditonal: true,
            success: function (response) {
                if (response.result) {
                    data_create = data_create + response.data_create;
                    data_update = data_update + response.data_update;
                    data_error = data_error + response.data_error;
                    data_pagedata = data_pagedata + 1;
                    $("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");
                    $("#list").clearGridData();
                    var mtt_no = response.mtt_no;
                    var grid = $("#list");
                    grid.jqGrid('setGridParam', { search: true });
                    var postData = grid.jqGrid('getGridParam', 'postData');
                    var params = new Object();
                    params.page = 1;
                    params.rows = postData.rows;
                    params.sidx = postData.sidx;
                    params.sord = postData.sord;
                    params._search = postData._search;
                    params.s_excel_no = mtt_no;
                    $.ajax({
                        url: '/DevManagement/searchmaterial_2',
                        type: "Get",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        traditional: true,
                        data: params,
                        success: function (data, st) {
                            console.log('materailuploadjs');
                            if (st == "success") {
                                var grid = $("#list")[0];
                                grid.addJSONData(data);
                                console.log("Page current: " + data_pagedata);
                                if (data_pagedata == segment) { $('#loading').hide() };
                            }
                        }
                    });

                }
            },
        });
    $('#loading').hide();
});
