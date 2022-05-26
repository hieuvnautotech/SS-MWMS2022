
var row_id, row_id2;

$grid = $('#ModelMgtGrid').jqGrid({
    colModel: [
        { key: true, label: 'id', name: 'mdid', width: 10, hidden: true },
        { label: 'Model Code', name: 'md_cd', width: 350, align: 'left' },
        { label: 'Name', name: 'md_nm', width: 500, align: 'left' },
        { label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' },
        { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
        { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
        { label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],

    gridComplete: function () {
        var rows = $("#ModelMgtGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var md_cd = $("#ModelMgtGrid").getCell(rows[i], "md_cd");
            var giatri = $('#remark_color').val();
            if (md_cd == giatri) {
                $("#ModelMgtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        };
        $('.loading').hide();
    },

    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#ModelMgtGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#ModelMgtGrid").getRowData(selectedRowId);


        $("#m_mdid").val(row_id.mdid);
        $("#m_md_cd").val(row_id.md_cd);
        $("#m_md_nm").val(row_id.md_nm);
        $("#m_use_yn").val(row_id.use_yn);


        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
    },

    pager: "#ModelMgtGridPager",
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000, 5000],
    height: 400,
    width: $(".box-body").width() - 5,
    caption: 'Model Management',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    rowNum: 50,
    gridview: true,
    shrinkToFit: false,
    multiselect: false,
    autowidth: false,
    loadonce: false,
    datatype: function (postData) { getDataOutBox(postData); },
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    ajaxGridOptions: { contentType: "application/json" },
});

function getDataOutBox(pdata) {
    $('.loading').show();
    var md_cd = $('#s_md_cd').val().trim();
    var md_nm = $('#s_md_nm').val().trim();

    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.md_cd = md_cd;
    params.md_nm = md_nm;

    $('#ModelMgtGrid').jqGrid('setGridParam', { search: true, postData: { searchString: $('#auto_complete_search').val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/DevManagement/searchModelMgt',
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $('#ModelMgtGrid')[0];
                grid.addJSONData(data);
            }
        }
    })
}

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#ModelMgtGrid').jqGrid('setGridWidth', $('.box-body').width());

$(window).on("resize", function () {
    var newWidth = $("#ModelMgtGrid").closest(".ui-jqgrid").parent().width();
    $("#ModelMgtGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
        var md_cd = $('#c_md_cd').val().toUpperCase().trim(),
            md_nm = $('#c_md_nm').val().trim(),
            use_yn = $('#c_use_yn').val().trim();

        $.ajax({
            url: "/DevManagement/insertModelMgt",
            type: "POST",
            dataType: "json",
            data: {
                md_cd: md_cd,
                md_nm: md_nm,
                use_yn: use_yn,
            },
            success: function (response) {
                if (response.result == true) {
                    debugger;
                    $('#loading').hide();
                    var id = response.data.mdid;
                    $("#ModelMgtGrid").jqGrid('addRowData', id, response.data, 'first');
                    $("#ModelMgtGrid").setRowData(id, false, { background: "#d0e9c6" });
                    //$("#ModelMgtGrid").trigger("reloadGrid");
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(response.message);
                }
            }
        });
    }

});

$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var mdid = $("#m_mdid").val().trim(),
            md_cd = $("#m_md_cd").val().trim(),
            md_nm = $("#m_md_nm").val().trim(),
            use_yn = $("#m_use_yn").val().trim();
        $.ajax({
            url: "/DevManagement/updateModelMgt",
            type: "PUT",
            dataType: "json",
            data: {
                mdid: mdid,
                md_cd: md_cd,
                md_nm: md_nm,
                use_yn: use_yn,
            },
            success: function (response) {
                if (response.result == true) {
                    var id = response.data.mdid;
                    $("#ModelMgtGrid").setRowData(id,response.data, { background: "#d0e9c6" });
                    SuccessAlert(response.message);
                }
                else {
                    ErorrAlert(response.message);
                }
            }
        });
    }
});

$("#searchBtn").click(function () {
    var grid = $("#ModelMgtGrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
});

$("#del_save_but").click(function () {
    $('#dialogDangerous').dialog('open');
});


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
    $("#del_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});


//$('#excelModelBtn').click(function (e) {
//    $('#ModelMgtGrid').jqGrid('exportToExcel',
//        options = {
//            includeLabels: true,
//            includeGroupHeader: true,
//            includeFooter: true,
//            fileName: "Model  Information.xlsx",
//            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//            maxlength: 40,
//            onBeforeExport: null,
//            replaceStr: null
//        }
//    );
//});
$('#excelModelBtn').click(function ()
{
    var md_cd = $("#m_md_cd").val().trim();
    var md_nm = $("#m_md_nm").val().trim();
    //debugger;
    //var po_no = $('#s_po_no').val().trim();
    //var product = $('#s_product').val().trim();
    //var buyer = $('#s_buyer').val().trim();
    //var lot_date = $('#s_lot_date').val().trim();
    //var lot_date_end = $('#end_lot_date').val().trim();

    $(`#exportData`).attr(`action`, `/DevManagement/ExportToExcelModel?md_nm=${md_nm}&md_cd=${md_cd}`);

});

//$('#excelBtn').click(function () {
//    $('#exportData').attr('action', "/DevManagement/ExportToExcelModel");
//});



//$('#htmlBtn').click(function (e) {
//    $("#ModelMgtGrid").jqGrid('exportToHtml',
//        options = {
//            title: '',
//            onBeforeExport: null,
//            includeLabels: true,
//            includeGroupHeader: true,
//            includeFooter: true,
//            tableClass: 'jqgridprint',
//            autoPrint: false,
//            topText: '',
//            bottomText: '',
//            fileName: "Model Information  ",
//            returnAsString: false
//        }
//        );
//});


//$('#pdfBtn').click(function (e) {
//    $('table').tableExport({
//        type: 'pdf',
//        fileName: "MateriaMgt",
//        pdfFontSize: '7',
//        escape: false,
//        headings: true,
//        bootstrap: true,
//        footers: true,
//    });
//});
//$('#excelBtn').click(function (e) {
//    $('table').tableExport({
//        type: 'xls',
//        fileName: "MateriaMgt",
//        escape: false,
//        headings: true,
//        bootstrap: true,
//        footers: true,
//    });
//});
//$('#htmlBtn').click(function (e) {
//    $('table').tableExport({
//        type: 'doc',
//        fileName: "MateriaMgt",
//        headings: true,
//        bootstrap: true,
//        footers: true,
//        escape: false,
//    });
//});

$("#form1").validate({
    rules: {
        "md_cd": {
            required: true,
        },
        "md_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "md_cd": {
            required: true,
        },
        "md_nm": {
            required: true,
        },
    },
});
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
            for (var i = 0; i < XL_row_object.length; i++) {
                var data_row_tmp = [];
                (XL_row_object[i]["Code"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Code"]));
                (XL_row_object[i]["Name"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Name"]));

                excelData.push(data_row_tmp);
            }
            json_object = JSON.stringify(excelData);
            console.log(JSON.parse(json_object));
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
        $('#loading').hide();
        return false
    }
    //$('#loading').show();
    var data_create = 0;
    var data_update = 0;
    var data_error = 0;
    var data_pagedata = -1;
    insertmaterial_tmp = [];
    var obj = JSON.parse(json_object.toString());
    var length = obj.length;


    if (json_object != "") {
        var obj = JSON.parse(json_object.toString());
        var length = obj.length;
        for (var i = 0; i <= length - 1; i++) {
            var tmp = [];
            var b = obj[i];
            var arr = Object.keys(b).map(function (key) { return b[key]; });
            for (j = 0; j <= 2; j++) {
                tmp.push(arr[j]);
            }
            item = {
                code: tmp[0],
                name: tmp[1],
            }
            insertmaterial_tmp.push(item);
        }
        $.ajax({
            url: "/DevManagement/insertModelExcel",
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

                    $.each(response.data, function (key, item) {
                        var id = item[0].mdid;

                        var STATUS = item[0].STATUS;

                        if (STATUS == 'INSERT') {

                            $("#ModelMgtGrid").jqGrid('addRowData', id, item[0], 'first');

                            //$("#list1").jqGrid('addRowData', ld, response.Data[i], 'first');
                            $("#ModelMgtGrid").setRowData(id, false, { background: "#28a745", color: "#fff" });
                            //$("#list").setRowData(, , { background: "#28a745", color: 'white', 'font-size': '1.2em' });
                            //$('#jqg_picMGrid_' + id).attr("disabled", true).addClass("hidden");
                        }

                    })
                    document.getElementById("imgupload").value = "";
                    $('#loading').hide();
                    SuccessAlert(response.message);
                }
                else {

                    $(`#loading`).hide();
                    document.getElementById("imgupload").value = "";
                    ErrorAlert(response.message);
                }
            },
            error: function () {
                ErrorAlert(`System error`);
                $(`#loading`).hide();
                return;
            }
        });

    }

});