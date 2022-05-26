var ProductCode = "";
var parentRowKey1 = 0;
var ListMaterial = [];
$("#list").jqGrid({
    datatype: function (postData) { getDataBOM(postData); },
    mtype: 'Get',
    colModel: [
        { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
        { key: false, label: 'Code', name: 'bom_no', width: 300, align: 'center', hidden: true },
        { key: false, label: 'Model', name: 'md_cd', sortable: true, width: 300, align: 'left' },
        { key: false, label: 'Product Code', name: 'style_no', width: 200, align: 'left' },
        { key: false, label: 'Product Name', name: 'style_nm', width: 200, align: 'left' },
        {
            label: 'Apply(Y/N)', name: 'isapply', width: 100, align: 'center', hidden: true, formatter: function (cellvalue, options, rowObject) {

                return '<input type="checkbox" name="selectedCall" value="' + cellvalue + '" id = "idMaterialBom' + cellvalue + '" onclick="getCheck(this)"/>'

            },

        },
        { key: false, label: 'Is Apply', name: 'IsApply', width: 300, align: 'left', hidden: true },
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        ProductCode = row_id.style_no;
        $("#c_bom_no").val(row_id.bom_no);
        $("#bid").val(row_id.bid);
        $("#c_style_no").val(row_id.style_no);
        $("#c_mt_no").val(row_id.mt_no);
        $("#c_cav").val(row_id.cav);
        $("#c_need_time").val(row_id.need_time);
        $("#c_mt_no_view").val(row_id.mt_no);
        $("#c_buocdap").val(row_id.buocdap);
        //$(".materialpp").hide();
        var isCheck = row_id.IsActive;
        if (isCheck == 1) {
            document.getElementById("isActive").checked = true;
        }
        else {
            document.getElementById("isActive").checked = false;
        }
        var lastSelection = "";

        if (rowid && rowid !== lastSelection) {
            var grid = $("#list");
            grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
            lastSelection = rowid;
        }
    },
    onCellSelect: editRow,
    pager: jQuery('#jqGridPager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    multiselect: true,
    shrinkToFit: false,
    viewrecords: true,
    height: 300,
    width: $(".boxList").width() - 5,
    sortable: true,
    loadonce: false,
    subGrid: true,
    subGridRowExpanded: showChildGridBom,
    caption: 'Bom Information',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    gridComplete: function () {

        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var ID = $("#list").getCell(rows[i], "bid");
            var IsActive = $("#list").getCell(rows[i], "IsApply");
            if (IsActive == 'Y') {
                $("#idMaterialBom" + ID).attr("checked", true);

            }
            else {

                $("#idMaterialBom" + ID).attr("checked", false);
            }
        }

    }
});



insertmaterial_tmp = [];
json_object = "";
$(document).ready(function () {
    function changeDate(exdate) {
        var e0date = new Date(0);
        var offset = e0date.getTimezoneOffset();
        return new Date(0, 0, exdate - 1, 0, -offset, 0);
    };
    $("#imgupload").change(function (evt) {
        insertmaterial_tmp = [];
        json_object = "";
        var selectedFile = evt.target.files[0];
        var reader = new FileReader();
        var excelData = [];
        reader.onload = function (event) {
            var data = event.target.result;
            var workbook = XLSX.read(data, { type: 'binary' });
            workbook.SheetNames.forEach(function (sheetName) {
                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
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
        var mt_no = json_object[i]["CODE NVL"];
        if (mt_no != null) {
            item = {
                md_cd: json_object[i]["MODEL"],
                style_no: json_object[i]["CODE PRODUCT"],
                mt_no: json_object[i]["CODE NVL"],
                cav: json_object[i]["CAVIT"],
                need_time: json_object[i]["Số lần sử dụng"],
                buocdap: json_object[i]["Bước dập/Số đo (mm)"],
            }
            insertmaterial_tmp.push(item);
        }
    }
        $.ajax({
            url: "/DevManagement/Upload_BOM",
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(insertmaterial_tmp),
            traditonal: true,
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    for (var i = 0; i < data.danhsach.length; i++) {
                        var id = data.danhsach[i].bid;
                        $("#list").jqGrid('addRowData', id, data.danhsach[i], 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                    }
                }
                else {
                //    ErrorAlert(data.message);
                    var html = "";
                    if (data.style != 0) {
                        html+="Product đã tồn tại! "
                    }
                    if (data.mt_cd != 0) {
                        html += "Material đã tồn tại! "
                    }
                    if (data.exit_style != 0) {
                        html += "Product không tồn tại! "
                    }
                    if (data.cav != 0) {
                        html += "Cavit trống! "
                    }
                    if (data.buocdap != 0) {
                        html += "Bước Dập trống! "
                    }
                    if (data.need_time != 0) {
                        html += "Số lần sử dụng trống! "
                    }
                    ErrorAlert("Vì một số nguyên nhân sau:" + html);
                }
            },
        });
    $("#imgupload").val("");
    $('#loading').hide();
});
