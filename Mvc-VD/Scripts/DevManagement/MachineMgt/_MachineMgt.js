$(function () {

    $grid = $("#MachineMgtGrid").jqGrid
        ({
            url: "/DevManagement/getMachineMgtData",
            datatype: 'json',
            mtype: 'GET',
            colModel: [
                { key: true, label: 'mmo', name: "mno", width: 80, align: 'center', hidden: true },
                { key: false, label: 'Type', name: 'mc_type', width: 110, align: 'center' },
                //{ key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'center' },
                { key: false, label: 'Code', name: 'mc_no', width: 150, align: 'left', },
                { key: false, label: 'Name', name: 'mc_nm', width: 150, align: 'left' },
                { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                { key: false, label: 'Description', name: 're_mark', width: 300, align: 'left' },
                { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                { label: 'Create Date', name: 'reg_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "d/m/Y H:i:s", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
                { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "d/m/Y H:i:s", newformat: "Y-m-d H:i:s" } },

            ],
            gridComplete: function () {
                var rows = $("#MachineMgtGrid").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var mc_no = $("#MachineMgtGrid").getCell(rows[i], "mc_no");
                    var giatri = $('#remark_color').val();
                    if (mc_no == giatri) {
                        $("#MachineMgtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                    }
                }
            },

            onSelectRow: function (rowid, status, e) {
                var selectedRowId = $("#MachineMgtGrid").jqGrid("getGridParam", 'selrow');
                var row_id = $("#MachineMgtGrid").getRowData(selectedRowId);
                var mno = row_id.mno;
                var mc_type = row_id.mc_type;
                var mc_no = row_id.mc_no;
                var mc_nm = row_id.mc_nm;
                var purpose = row_id.purpose;
                var re_mark = row_id.re_mark;

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false),


                    $("#m_mc_type").val(mc_type);
                $("#m_mc_no").val(mc_no);
                $("#m_mc_nm").val(mc_nm);
                $("#m_purpose").val(purpose);
                $("#m_mno").val(mno);
                $("#m_re_mark").val(re_mark);
                $("#c_mno").val(mno);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").removeClass("hidden");
                $("#tab_c1").addClass("hidden");
                $("#tab_c2").addClass("active");
            },

            pager: "#MachineMgtPager",
            pager: jQuery('#MachineMgtPager'),
            width: $(".box-body").width() - 5,
            rowNum: 50,
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 450,
            caption: 'MachineMgt',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            autowidth: false,
            loadonce: true,
            multiselect: true,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
        })
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#MachineMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#MachineMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#MachineMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });
    function fun_check1() {
        if ($("#c_mc_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#c_mc_nm").val("");
            $("#c_mc_nm").focus();
            return false;
        }
        if ($("#c_mc_type").val().trim() == "") {
            alert("Please enter the Type.");
            $("#c_mc_type").val("");
            $("#c_mc_type").focus();
            return false;
        }
        return true;
    }
    $("#c_save_but").click(function () {

        if ($("#form1").valid()) {
            var mno = $('#c_mno').val();
            var mc_type = $('#c_mc_type').val();
            var mc_no = $('#c_mc_no').val().toUpperCase();
            var purpose = $('#c_purpose').val();
            var mc_nm = $('#c_mc_nm').val();
            var re_mark = $('#c_re_mark').val();

            {
                $.ajax({
                    url: "/DevManagement/insertMachineMgt",
                    type: "get",
                    dataType: "json",
                    data: {
                        mno: mno,
                        mc_type: mc_type,
                        mc_no: mc_no,
                        purpose: purpose,
                        mc_nm: mc_nm,
                        re_mark: re_mark
                    },
                    success: function (data) {
                        debugger;
                        if (data.result == true) {
                            var id = data.kq.mno;
                            $("#MachineMgtGrid").jqGrid('addRowData', id, data.kq, 'first');
                            $("#MachineMgtGrid").setRowData(id, false, { background: "#d0e9c6" });
                            //jQuery("#MachineMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            SuccessAlert(data.message);
                        }
                        else {
                            ErrorAlert(data.message);
                        }
                    },
                });
            }
        }
    });

    function fun_check2() {
        if ($("#m_mc_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#m_mc_nm").val("");
            $("#m_mc_nm").focus();
            return false;
        }
        if ($("#m_mc_type").val().trim() == "") {
            alert("Please enter the Type.");
            $("#m_mc_type").val("");
            $("#m_mc_type").focus();
            return false;
        }
        return true;
    }

    $("#m_save_but").click(function () {
        if ($("#form2").valid()) {
            var mno = $('#m_mno').val();
            var mc_type = $('#m_mc_type').val();
            var mc_no = $('#m_mc_no').val();
            var purpose = $('#m_purpose').val();
            var mc_nm = $('#m_mc_nm').val();
            var re_mark = $('#m_re_mark').val();
            {
                $.ajax({
                    url: "/DevManagement/updatetMachineMgt",
                    type: "PUT",
                    dataType: "json",
                    data: {
                        mno: mno,
                        mc_type: mc_type,
                        mc_no: mc_no,
                        purpose: purpose,
                        mc_nm: mc_nm,
                        re_mark: re_mark
                    },
                    success: function (data) {
                        if (data.result == true) {
                            var m_buyer_cd = mc_no;
                            $('#remark_color').val(mc_no.toUpperCase());
                            jQuery("#MachineMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            SuccessAlert(data.message);
                        }
                        else {
                            ErrorAlert(data.message);
                        }
                    }
                });
            }
        }
    });
    //Search
    $("#searchBtn").click(function () {

        var mc_type = $('#mc_type').val()
        var mc_no = $('#s_mc_no').val().trim();
        var mc_nm = $('#s_mc_nm').val().trim();
        var start = $('#start').val()
        var end = $('#end').val()
        $.ajax({
            url: "/DevManagement/searchMachineMgt",
            type: "Get",
            dataType: "json",
            data: {
                mc_type: mc_type,
                mc_no: mc_no,
                mc_nm: mc_nm,
                start: start,
                end: end
            },
            success: function (result) {
                $("#MachineMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });

});

function bntCellValue(cellValue, options, rowdata, action) {
    id_delete = rowdata.mno;
    var html = '<button class="btn btn-xs btn-danger delete-btn" title="Delete" data-id_delete="' + rowdata.mno + '" >X</button>';
    return html;
};


//Date
$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});
// Init Datepicker end
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});

var getType = "/DevManagement/getType_MachineMgt";
//Create
$(document).ready(function () {
    _getTypeC();
    $("#c_mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeC() {
    $.get(getType, function (data) {
        console.log(data);
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Type All</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '[' + item.dt_cd + ']' + '</option>';
            });
            $(".mc_type").html(html);
        }
    });
}
//update
$(document).ready(function () {
    _getType();
    $("#m_mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getType() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Type All</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '[' + item.dt_cd + ']' + '</option>';
            });
            $("#m_mc_type").html(html);

        }
    });
}
//search

$(document).ready(function () {
    _getTypeS();
    $("#mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeS() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Type All</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '[' + item.dt_cd + ']' + '</option>';
            });
            $("#mc_type").html(html);
        }
    });
}

// Tab Create/Modifly
$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true),
        document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

});
//Export

$('#excelBtn').click(function (e)
{
    $("#MachineMgtGrid").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Machine Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
});

$('#htmlBtn').click(function (e)
{
    $("#MachineMgtGrid").jqGrid('exportToHtml',
        options = {
            title: '',
            onBeforeExport: null,
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            tableClass: 'jqgridprint',
            autoPrint: false,
            topText: '',
            bottomText: '',
            fileName: "Machine Information  ",
            returnAsString: false
        }
    );
});

//$(document).ready(function (e) {
//    $('#pdfBtn').click(function (e) {
//        $("#MachineMgtGrid").jqGrid('exportToPdf',
//            options = {
//                title: null,
//                orientation: 'landscape',
//                pageSize: 'A4',
//                description: null,
//                onBeforeExport: null,
//                download: 'download',
//                includeLabels: true,
//                includeGroupHeader: true,
//                includeFooter: true,
//                fileName: "MachineMgt.pdf",
//                mimetype: "application/pdf"
//            }
//        );
//    });
//});

//$('#excelMaterial').click(function (e) {
//    $("#MachineMgtGrid").jqGrid('exportToExcel',
//        options = {
//            includeLabels: true,
//            includeGroupHeader: true,
//            includeFooter: true,
//            fileName: "Machine  Information.xlsx",
//            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//            maxlength: 40,
//            onBeforeExport: null,
//            replaceStr: null
//        }
//    );
//});

$('#htmlBtn').click(function (e) {
    $("#MachineMgtGrid").jqGrid('exportToHtml',
        options = {
            title: '',
            onBeforeExport: null,
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            tableClass: 'jqgridprint',
            autoPrint: false,
            topText: '',
            bottomText: '',
            fileName: "Machine Information  ",
            returnAsString: false
        }
    );
});

//$(document).ready(function (e) {
//    $('#excelBtn').click(function (e) {
//        $("#MachineMgtGrid").jqGrid('exportToExcel',
//             options = {
//                 includeLabels: true,
//                 includeGroupHeader: true,
//                 includeFooter: true,
//                 fileName: "MachineMgt.xlsx",
//                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                 maxlength: 40,
//                 onBeforeExport: null,
//                 replaceStr: null
//             }
//         );
//    });
//});
//$(document).ready(function (e) {
//    $('#htmlBtn').click(function (e) {
//        $("#MachineMgtGrid").jqGrid('exportToHtml',
//            options = {
//                title: '',
//                onBeforeExport: null,
//                includeLabels: true,
//                includeGroupHeader: true,
//                includeFooter: true,
//                tableClass: 'jqgridprint',
//                autoPrint: false,
//                topText: '',
//                bottomText: '',
//                fileName: "MachineMgt",
//                returnAsString: false
//            }
//         );
//    });
//});

$("#qrPrintBtn").on("click", function () {
    var i, selRowIds = $('#MachineMgtGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        window.open("/DevManagement/_PrintQRMachine?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        alert("Please select Grid.");
    }
})








insertmaterial_tmp = [];
json_object = "";

$("#imgupload").change(function (evt) {
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event) {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName) {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            console.log(XL_row_object);
            for (var i = 0; i < XL_row_object.length; i++) {
                var data_row_tmp = [];

                (XL_row_object[i]["TYPE"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["TYPE"]));
                (XL_row_object[i]["Code"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Code"]));
                (XL_row_object[i]["Name"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Name"]));
                (XL_row_object[i]["Purpose"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Purpose"]));
                //(XL_row_object[i]["Packing Amount(EA)"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Packing Amount(EA)"]));
                //data_row_tmp.push(XL_row_object[i]);
                excelData.push(data_row_tmp);
            }
            json_object = JSON.stringify(excelData);
            ////console.log(JSON.parse(json_object));
        })
    };

    reader.onerror = function (event) {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});

$("#uploadBtn").click(function () {
    $('#loading').show();
    if ($("#imgupload").val() == "") {
        ErrorAlert("Please select file to upload");
        return false
    };

    var data_create = 0;
    var data_update = 0;
    var data_error = 0;
    var data_pagedata = -1;
    insertmaterial_tmp = [];


    if (json_object != "") {

        var obj = JSON.parse(json_object.toString());

        var length = obj.length;
        for (i = 0; i <= length - 1; i++) {
            var tmp = [];
            var b = obj[i];
            var arr = Object.keys(b).map(function (key) { return b[key]; });
            for (j = 0; j <= 4; j++) {
                tmp.push(arr[j]);
            }
            item = {
                mc_type: tmp[0],
                mc_no: tmp[1],
                mc_nm: tmp[2],
                purpose: tmp[3],
            }
            insertmaterial_tmp.push(item);
        }

        $.ajax({
            url: "/DevManagement/insertMachineExcel",
            type: 'POST',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(insertmaterial_tmp),
            traditonal: true,
            success: function (response) {
                if (response.result) {
                    data_create = data_create + response.data_create;
                    data_update = data_update + response.data_update;
                    data_error = data_error + response.data_error;
                    console.log(response.data);
                    data_pagedata = data_pagedata + 1;
                    $("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");

                    $.each(response.data, function (key, item) {

                        var id = item[0].mno;
                        var STATUS = item[0].STATUS;
                        console.log(item[0].reg_dt);
                        if (STATUS == 'INSERT') {

                            $("#MachineMgtGrid").jqGrid('addRowData', id, item[0], 'first');

                            $("#MachineMgtGrid").setRowData(id, false, { background: "#28a745", color: "#fff" });

                            SuccessAlert(response.message);
                        }
                    });
                    $('#loading').hide();
                    document.getElementById("imgupload").value = "";

                }
                else {
                    ErrorAlert(response.message);
                    $(`#loading`).hide();
                    document.getElementById("imgupload").value = "";
                    return;

                }
            },
        });
    }
});




