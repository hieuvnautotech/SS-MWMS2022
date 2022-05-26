
var row_id, row_id2;
$grid = $("#WHSCommonGrid").jqGrid
({
    url: "/StandardMgtWO/getWHSCommon",
    datatype: 'json',
    mtype: 'GET',
    colModel: [
        { key: true, label: 'id', name: 'mt_id', index: 'mt_id', width: 10, hidden: true },
        { label: 'Main Code', name: 'mt_cd', index: 'mt_cd', width: 100, align: 'center' },
        { label: 'Main Name', name: 'mt_nm', width: 150, },
        { label: 'Main Explain', name: 'mt_exp', width: 300, align: 'left' },
        { label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' },

    ],

    gridComplete: function () {
        var rows = $("#WHSCommonGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var v_use_yn = $("#WHSCommonGrid").getCell(rows[i], "use_yn");
            if (v_use_yn == "N") {
                $("#WHSCommonGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
            var po_no = $("#WHSCommonGrid").getCell(rows[i], "mt_id");
            if (po_no == $('#color_bg_line').val()) {
                $("#WHSCommonGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }

        }
    },

    onSelectRow: function (rowid, status, e) {

        var selectedRowId = $("#WHSCommonGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#WHSCommonGrid").getRowData(selectedRowId);

        if (row_id != "") {

            $("#tab_d1").addClass("active");
            $("#tab_d2").removeClass("active");
            $("#tab_dc1").addClass("active");
            $("#tab_dc2").addClass("hidden");
            $("#tab_dc1").removeClass("hidden");
            $("#tab_dc2").removeClass("active");
            document.getElementById("form3").reset();

        }


        var mt_cd = row_id.mt_cd;
        var mt_nm = row_id.mt_nm;
        var mt_exp = row_id.mt_exp;
        var use_yn = row_id.use_yn;
        var mt_id = row_id.mt_id;

        $("#m_mt_cd").val(mt_cd);
        $("#m_mt_nm").val(mt_nm);
        $("#m_mt_exp").val(mt_exp);
        $("#m_use_yn").val(use_yn);
        $("#m_mt_id").val(mt_id);

        $("#dc_mt_cd").val(mt_cd);
        $("#dc_mt_nm").val(mt_nm);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");

        $("#m_save_but").attr("disabled", false);

        $("#WHSCommonDtGrid").clearGridData();
        $("#WHSCommonDtGrid").setGridParam({ url: "/StandardMgtWO/getWHSCommonDtData?" + "mt_cd=" + mt_cd, datatype: "json" }).trigger("reloadGrid");
    },

    pager: "#WHSCommonGridPager",
    pager: jQuery('#WHSCommonGridPager'),
    viewrecords: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    height: 200,
    width: $(".box-body").width() - 5,
    caption: 'WO Common(M)',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    shrinkToFit: false,
    multiselect: false,
    autowidth: false,
    loadonce: true,
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


$("#c_save_but").click(function () {
    var mt_cd = $('#c_mt_cd').val();
    var mt_nm = $('#c_mt_nm').val();
    var mt_exp = $('#c_mt_exp').val();
    var use_yn = $('#c_use_yn').val();


    if ($("#c_mt_nm").val().trim() == "") {
        alert("Please enter Main Name");
        $("#c_mt_nm").val("");
        $("#c_mt_nm").focus();
        return false;
    }
    if ($("#c_mt_exp").val().trim() == "") {
        alert("Please enter Main Explain");
        $("#c_mt_exp").val("");
        $("#c_mt_exp").focus();
        return false;
    }
    else {
        $.ajax({
            url: "/StandardMgtWO/insertWHSCommon",
            type: "POST",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_exp: mt_exp,
                use_yn: use_yn,
            },
            success: function (data) {
                debugger;
                if (data.result == true) {
                    var id = data.mt_id;
                    $("#WHSCommonGrid").jqGrid('addRowData', id, data.data, 'first');
                    $("#WHSCommonGrid").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                } else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});

//Modifly Common main
$("#m_save_but").click(function () {

    var mt_cd = $('#m_mt_cd').val();
    var mt_nm = $('#m_mt_nm').val();
    var mt_exp = $('#m_mt_exp').val();
    var use_yn = $('#m_use_yn').val();
    var mt_id = $('#m_mt_id').val();
    $.ajax({
        url: "/StandardMgtWO/updatetWHSCommon",
        type: "PUT",
        dataType: "json",
        data: {
            mt_cd: mt_cd,
            mt_nm: mt_nm,
            mt_exp: mt_exp,
            use_yn: use_yn,
            mt_id: mt_id,
        },
        success: function (data) {
            if (data.result == true) {
                var id = data.mt_id;
                $("#WHSCommonGrid").setRowData(id, data, { background: "#d0e9c6" });
                jQuery("#WHSCommonDtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });

});

$("#searchBtn").click(function () {
    var mt_cd = $('#mt_cd').val().trim();
    var mt_nm = $('#mt_nm').val().trim();
    var mt_exp = $('#mt_exp').val().trim();
    $.ajax({
        url: "/StandardMgtWO/searchWHSCommon",
        type: "GET",
        dataType: "json",
        data: {
            mt_cd: mt_cd,
            mt_nm: mt_nm,
            mt_exp: mt_exp
        },
        success: function (result) {

            $("#WHSCommonGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            $("#WHSCommonDtGrid").clearGridData();

            document.getElementById("form1").reset();
            $("#tab_2").removeClass("active");
            $("#tab_1").addClass("active");
            $("#tab_c2").removeClass("active");
            $("#tab_c1").removeClass("hidden");
            $("#tab_c2").addClass("hidden");
            $("#tab_c1").addClass("active");

            document.getElementById("form3").reset();
            $("#tab_d2").removeClass("active");
            $("#tab_d1").addClass("active");
            $("#tab_dc2").removeClass("active");
            $("#tab_dc1").removeClass("hidden");
            $("#tab_dc2").addClass("hidden");
            $("#tab_dc1").addClass("active");

        }
    });

});

function bntCellValue(cellValue, options, rowdata, action) {
    id_delete = rowdata.mt_id;
    var html = '<button class="btn btn-xs btn-danger delete-btn" title="Delete" data-id_delete="' + rowdata.mt_id + '" >X</button>';
    return html;
};
// Tab
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
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

$("#WHSCommonDtGrid").jqGrid
({
    url: "/StandardMgtWO/getWHSCommonDtData",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'cdid', name: 'cdid', width: 80, align: 'center', hidden: true },
        { label: 'mt_cd', name: 'mt_cd', width: 80, align: 'center', hidden: true, },
        { label: 'Main Name', name: 'mt_nm', width: 150, align: 'center' },
        { label: 'Detail Code', name: 'dt_cd', width: 100, align: 'center' },
        { label: 'Detail Name', name: 'dt_nm', index: 'dt_nm', width: 150, align: 'left' },
        { label: 'Detail Explain', name: 'dt_exp', width: 250, align: 'left' },
        { label: 'Use Y/N', name: 'use_yn', align: 'center', width: 80 },
        { label: 'Order', name: 'dt_order', align: 'center', width: 80 },

    ],

    gridComplete: function () {
        var rows = $("#WHSCommonDtGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var v_use_yn = $("#WHSCommonDtGrid").getCell(rows[i], "use_yn");
            if (v_use_yn == "N") {
                $("#WHSCommonDtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
            var po_no = $("#WHSCommonDtGrid").getCell(rows[i], "cdid");
            if (po_no == $('#color_bg_line2').val()) {
                $("#WHSCommonDtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },



    onSelectRow: function (rowid, status, e) {

        var selectedRowId = $("#WHSCommonDtGrid").jqGrid("getGridParam", 'selrow');
        row_id2 = $("#WHSCommonDtGrid").getRowData(selectedRowId);

        var selectedRowId = $("#WHSCommonGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#WHSCommonGrid").getRowData(selectedRowId);


        var dm_mt_nm = row_id.mt_nm;
        var dm_mt_cd = row_id2.mt_cd;
        var dm_dt_cd = row_id2.dt_cd;
        var dm_dt_nm = row_id2.dt_nm;
        var dm_dt_exp = row_id2.dt_exp;
        var dm_dt_order = row_id2.dt_order;
        var dm_use_yn = row_id2.use_yn;
        var dm_cdid = row_id2.cdid

        $("#dm_mt_nm").val(dm_mt_nm);
        $("#dm_mt_cd").val(dm_mt_cd);
        $("#dm_dt_cd").val(dm_dt_cd);
        $("#dm_dt_nm").val(dm_dt_nm);
        $("#dc_mt_cd").val(dm_mt_cd);
        $("#dm_dt_exp").val(dm_dt_exp);
        $("#dm_dt_order").val(dm_dt_order);
        $("#dm_use_yn").val(dm_use_yn);

        $("#dm_cdid").val(dm_cdid);

        $("#tab_d1").removeClass("active");
        $("#tab_d2").addClass("active");
        $("#tab_dc1").removeClass("active");
        $("#tab_dc2").removeClass("hidden");
        $("#tab_dc1").addClass("hidden");
        $("#tab_dc2").addClass("active");
        $("#dm_save_but").attr("disabled", false);
    },
    pager: jQuery('#WHSCommonDtGridPager'),
    viewrecords: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    height: 200,
    width: $(".box-body").width() - 5,
    shrinkToFit: false,
    autowidth: false,
    rownumbers: true,
    gridview: true,
    loadonce: true,
    caption: 'WO Common(M) Detail',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    pager: "#WHSCommonDtGridPager",
    multiselect: false,
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

function fun_check1() {

    if ($("#dc_mt_cd").val() == "") {
        alert("Please select Common Main Grid.");
        return false;
    }
    if ($("#dc_mt_nm").val() == "") {
        alert("Please select Common Main Grid.");
        return false;
    }
    if ($("#dc_dt_nm").val() == "") {
        alert("Please enter the name");
        $("#dc_dt_nm").val("");
        $("#dc_dt_nm").focus();
        return false;
    }
    if ($("#dc_dt_exp").val() == "") {
        alert("Please enter the explain");
        $("#dc_dt_exp").val("");
        $("#dc_dt_exp").focus();
        return false;
    }

    if (isNaN($("#dc_dt_order").val()) == true) {
        $("#dc_dt_order").val("");
        $("#dc_dt_order").focus();
        return false;
    }

    if ($("#dc_dt_order").val() == "") {
        $("#dc_dt_order").val(1);
        return true;

    }
    return true;
}

$("#dc_save_but").click(function () {

    if (fun_check1() == true) {

        var dt_cd = $('#dc_dt_cd').val().trim();
        var dt_nm = $('#dc_dt_nm').val();
        var dt_exp = $('#dc_dt_exp').val();
        var use_yn = $('#dc_use_yn').val();
        var mt_cd = $('#dc_mt_cd').val();
        var dt_order = $('#dc_dt_order').val();

        {
            $.ajax({
                url: "/StandardMgtWO/insertWHSCommonDt",
                type: "POST",
                dataType: "json",
                data: {
                    dt_cd: dt_cd,
                    dt_nm: dt_nm,
                    dt_exp: dt_exp,
                    use_yn: use_yn,
                    mt_cd: mt_cd,
                    dt_order: dt_order,
                },
                success: function (data) {
                    if (data.result == true) {
                        $('#color_bg_line2').val(data.cdid);
                        jQuery("#WHSCommonDtGrid").setGridParam({ rowNum: 20, datatype: "json" }).trigger('reloadGrid');
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
    if (isNaN($("#dm_dt_order").val()) == true) {
        var selectedRowId = $("#WHSCommonDtGrid").getGridParam('selrow');
        var row = $("#WHSCommonDtGrid").getRowData(selectedRowId);
        var dt_order = row.dt_order;
        $("#dm_dt_order").val(dt_order);
        return false
    }

    if ($("#dm_dt_order").val() == "") {
        var selectedRowId = $("#WHSCommonDtGrid").getGridParam('selrow');
        var row = $("#WHSCommonDtGrid").getRowData(selectedRowId);
        var dt_order = row.dt_order;
        $("#dm_dt_order").val(dt_order);
    }
    return true;
}

$("#dm_save_but").click(function () {

    if (fun_check2() == true) {
        var dt_nm = $('#dm_dt_nm').val();
        var dt_exp = $('#dm_dt_exp').val();
        var use_yn = $('#dm_use_yn').val();
        var cdid = $('#dm_cdid').val();
        var dt_order = $('#dm_dt_order').val();

        $.ajax({

            url: "/StandardMgtWO/updateWHSCommonDt",
            type: "PUT",
            dataType: "json",
            data: {
                dt_nm: dt_nm,
                dt_exp: dt_exp,
                use_yn: use_yn,
                cdid: cdid,
                dt_order: dt_order,
            },
            success: function (data) {
                if (data.result == true) {
                    $('#color_bg_line2').val($('#dm_cdid').val());
                    jQuery("#WHSCommonDtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});


$("#dm_remove_but").click(function () {
    debugger;
    var cdid = $('#dm_cdid').val();
    {
        $.ajax({
            url: "/StandardMgtWO/deleteWHSCommonDt",
            type: "DELETE",
            dataType: "json",
            data: {
                cdid: cdid,
            },
            success: function (data) {
                if (data.result == true) {
                    $('#color_bg_line2').val($('#dm_cdid').val());
                    jQuery("#WHSCommonDtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});

function onlyNumber(event) {
    event = event || window.event;
    var keyID = (event.which) ? event.which : event.keyCode;
    if ((keyID >= 48 && keyID <= 57) || (keyID >= 96 && keyID <= 105) || keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
        return;
    else
        return false;
}

function bntCellValue2(cellValue, options, rowdata, action) {
    id_delete = rowdata.cdid;
    var html = '<button class="btn btn-xs btn-danger delete-btn" title="Delete" data-id_delete="' + rowdata.cdid + '" >X</button>';
    return html;
};

$("#tab_d1").on("click", "a", function (event) {
    document.getElementById("form3").reset();
    $("#tab_d2").removeClass("active");
    $("#tab_d1").addClass("active");
    $("#tab_dc2").removeClass("active");
    $("#tab_dc1").removeClass("hidden");
    $("#tab_dc2").addClass("hidden");
    $("#tab_dc1").addClass("active");
    $("#dc_dt_order").val(1);

    var selectedRowId = $("#WHSCommonGrid").getGridParam('selrow');
    if (selectedRowId == null) {

        $("#dc_mt_cd").val("");
        $("#dc_mt_nm").val("");
    }
    else {

        var mt_cd1 = $("#WHSCommonGrid").getCell(selectedRowId, "mt_cd");
        var mt_nm2 = $("#WHSCommonGrid").getCell(selectedRowId, "mt_nm");
        $("#dc_mt_cd").val(mt_cd1);
        $("#dc_mt_nm").val(mt_nm2);
    }




});
$("#tab_d2").on("click", "a", function (event) {
    $("#dm_save_but").attr("disabled", true);
    document.getElementById("form4").reset();
    $("#tab_d1").removeClass("active");
    $("#tab_d2").addClass("active");
    $("#tab_dc1").removeClass("active");
    $("#tab_dc2").removeClass("hidden");
    $("#tab_dc1").addClass("hidden");
    $("#tab_dc2").addClass("active");
});
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#WHSCommonGrid").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "WOCommon.pdf",
                mimetype: "application/pdf"
            }
         );

        //       $("#WHSCommonDtGrid").jqGrid('exportToPdf',
        //   options = {
        //       title: null,
        //       orientation: 'landscape',
        //       pageSize: 'A4',
        //       description: null,
        //       onBeforeExport: null,
        //       download: 'download',
        //       includeLabels: true,
        //       includeGroupHeader: true,
        //       includeFooter: true,
        //       fileName: "WHSCommonDtGrid.pdf",
        //       mimetype: "application/pdf"
        //   }
        //);
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#WHSCommonGrid").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "WOCommon.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
        //       $("#WHSCommonDtGrid").jqGrid('exportToExcel',
        //    options = {
        //        includeLabels: true,
        //        includeGroupHeader: true,
        //        includeFooter: true,
        //        fileName: "WHSCommonDtGrid.xlsx",
        //        mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //        maxlength: 40,
        //        onBeforeExport: null,
        //        replaceStr: null
        //    }
        //);
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#WHSCommonGrid").jqGrid('exportToHtml',
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
                fileName: "WOCommon",
                returnAsString: false
            }
         );
    });
});
