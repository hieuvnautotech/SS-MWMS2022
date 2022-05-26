var row_id, row_id2;
$("#commonGrid").jqGrid
({
    url: "/System/GetJqGridData",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "mt_id", width: 100, align: "left", label: "id", resizable: true, hidden: true },
        { name: "mt_cd", width: 100, align: "center", label: "Main Code", resizable: true },
        { name: "mt_nm", width: 250, label: "Main Name", resizable: true },
        { name: "mt_exp", width: 250, label: "Description", resizable: true },
        { name: "use_yn", width: 100, align: "center", label: "Use Y/N", resizable: true },
    ],
    gridComplete: function () {
        var rows = $("#commonGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var mt_cd = $("#commonGrid").getCell(rows[i], "mt_cd");
            var giatri = $('#mt_remark_color').val();
            if (mt_cd == giatri) {
                $("#commonGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },

    onSelectRow: function (rowid, status, e) {

        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#commonGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#commonGrid").getRowData(selectedRowId);
        var mt_cd = row_id.mt_cd;
        var mt_nm = row_id.mt_nm;
        var mt_exp = row_id.mt_exp;
        var use_yn = row_id.use_yn;
        var mt_id = row_id.mt_id;

        document.getElementById("form2").reset();
        $("#m_save_but").attr("disabled", false);
        if (Session_authName == "root") {
            $("#del_save_but").attr("disabled", false);
        }
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#commondtGridPager").clearGridData();
        $("#commondtGrid").setGridParam({ url: "/system/GetJqGridDataCommDt?" + "mt_cd=" + mt_cd, datatype: "json" }).trigger("reloadGrid");
        
        $("#m_mt_cd").val(mt_cd);
        $("#m_mt_nm").val(mt_nm);
        $("#m_mt_exp").val(mt_exp);
        $("#m_use_yn").val(use_yn);
        $("#m_mt_id").val(mt_id);
        $("#dc_mt_cd").val(mt_cd);
        $("#dc_mt_nm").val(mt_nm);

        document.getElementById("form3").reset();
        $("#tab_d2").removeClass("active");
        $("#tab_d1").addClass("active");
        $("#tab_dc2").removeClass("active");
        $("#tab_dc1").removeClass("hidden");
        $("#tab_dc2").addClass("hidden");
        $("#tab_dc1").addClass("active");
        $("#dm_mt_cd").val(dm_mt_cd);
        $("#dc_mt_nm").val(mt_nm);
        $("#dc_mt_cd").val(mt_cd);
        $("#dc_cdid_cd").val(1);
        $("#dc_dt_order").val(1);
        $("#dc_mt_cd").val(mt_cd);
        $("#dc_mt_nm").val(mt_nm);

    },
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#commonGridPager',
    viewrecords: true,
    height: 250,
    width: $(".box-body").width() - 5,
    caption: 'Common Main',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    shrinkToFit: false,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
                   
});
//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#commonGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#commonGrid").closest(".ui-jqgrid").parent().width();
    $("#commonGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
        var mt_cd = $('#c_mt_cd').val().toUpperCase();
        var mt_nm = $('#c_mt_nm').val();
        var mt_exp = $('#c_mt_exp').val();
        var use_yn = $('#c_use_yn').val();
        $.ajax({
            url: "/system/insertCommon",
            type: "Post",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_exp: mt_exp,
                use_yn: use_yn,
            },
            success: function (data) {
                if (data.result == true) {
                    var Mt_Cd = data.mt_cd;
                    var id = data.value.mt_id;
                    $("#commonGrid").jqGrid('addRowData', id, data.value, 'first');
                    $("#commonGrid").setRowData(id, false, { background: "#d0e9c6" });
                    //jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    $("#commondtGrid").setGridParam({ url: "/system/GetJqGridDataCommDt?" + "mt_cd=" + Mt_Cd, datatype: "json" }).trigger("reloadGrid");
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }            
            }
        });
    }
});

               
//Modifly Common main
$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var mt_cd = $('#m_mt_cd').val();
        var mt_nm = $('#m_mt_nm').val();
        var mt_exp = $('#m_mt_exp').val();
        var use_yn = $('#m_use_yn').val();
        var mt_id = $('#m_mt_id').val();
        $.ajax({
            url: "/system/updateCommon",
            type: "Put",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_exp: mt_exp,
                use_yn: use_yn,
                mt_id: mt_id,
            },
            success: function (data) {
                debugger;
                if (data.result == true) {
                    jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }

});

$("#del_save_but").click(function ()
{
    $('#dialogDangerous_cm').dialog('open');
});
$("#ddel_save_but").click(function () {
    $('#dialogDangerous_dt').dialog('open');
});


// Tab
$("#tab_1").on("click", "a", function (event) {
$("#commonGrid").trigger('reloadGrid');
document.getElementById("form1").reset();
$("#tab_2").removeClass("active");
$("#tab_1").addClass("active");
$("#tab_c2").removeClass("active");
$("#tab_c1").removeClass("hidden");
$("#tab_c2").addClass("hidden");
$("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
$("#commonGrid").trigger('reloadGrid');
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

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       


$("#commondtGrid").jqGrid
({
    url: "/System/GetJqGridDataCommDt",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "cdid", width: 50, align: "left", label: "cdid", resizable: true, hidden: true },
        { name: "mt_cd", width: 100, align: "center", label: "Main Code", resizable: true },
        { name: "mt_nm", width: 250, align: "center", label: "Main Name", resizable: true },
        { name: "dt_cd", width: 100, align: "left", label: "Detail Code", resizable: true },
        { name: "dt_nm", width: 250, align: "left", label: "Detail Name", resizable: true },
        { name: "dt_exp", width: 250, align: "left", label: "Description", resizable: true },
        { name: "dt_order", width: 100, align: "right", label: "Order", resizable: true },
        { name: "use_yn", width: 100, align: "center", label: "Use Y/N", resizable: true },

    ],
    gridComplete: function () {
        var rows = $("#commondtGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var mt_cd = $("#commondtGrid").getCell(rows[i], "mt_cd");
            var dt_cd = $("#commondtGrid").getCell(rows[i], "dt_cd");
            var giatri_mt_cd = $('#mt_remark_color').val();
            var giatri_dt_cd = $('#dt_remark_color').val();
            if ((mt_cd == giatri_mt_cd) && (dt_cd == giatri_dt_cd)) {
                $("#commondtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, status, e) {

        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#commondtGrid").jqGrid("getGridParam", 'selrow');
        row_id2 = $("#commondtGrid").getRowData(selectedRowId);

        var dm_cdid = row_id2.cdid;
        var dm_mt_cd = row_id2.mt_cd;
        var dm_mt_nm = row_id2.mt_nm;
        var dm_dt_cd = row_id2.dt_cd;
        var dm_dt_nm = row_id2.dt_nm;
        var dm_dt_exp = row_id2.dt_exp;
        var dm_dt_order = row_id2.dt_order;
        var dm_use_yn = row_id2.use_yn;

        $("#dm_cdid_cd").val(dm_cdid);
        $("#dm_mt_cd").val(dm_mt_cd);
        $("#dm_mt_nm").val(dm_mt_nm);
        $("#dm_dt_cd").val(dm_dt_cd);
        $("#dm_dt_nm").val(dm_dt_nm);
        $("#dm_dt_order").val(dm_dt_order);
        $("#dm_dt_exp").val(dm_dt_exp);
        $("#dm_use_yn").val(dm_use_yn);
        

        $("#tab_d1").removeClass("active");
        $("#tab_d2").addClass("active");
        $("#tab_dc1").removeClass("active");
        $("#tab_dc2").removeClass("hidden");
        $("#tab_dc1").addClass("hidden");
        $("#tab_dc2").addClass("active");

        
        $("#dm_save_but").attr("disabled", false);
        $("#ddel_save_but").attr("disabled", false);
    },
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#commondtGridPager',
    viewrecords: true,
    height: 150,
    width: $(".box-body").width() - 5,
    caption: 'Common Detail',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    shrinkToFit: false,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
});

$('#commondtGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#commondtGrid").closest(".ui-jqgrid").parent().width();
    $("#commondtGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#dc_save_but").click(function () {
    if ($("#form3").valid() == true) {
        var dc_mt_cd = $('#dc_mt_cd').val();
        var dc_dt_cd = $('#dc_dt_cd').val();
        var dc_dt_nm = $('#dc_dt_nm').val();
        var dc_dt_exp = $('#dc_dt_exp').val();
        var dc_dt_order = $('#dc_dt_order').val();
        var dc_use_yn = $('#dc_use_yn').val();
        $.ajax({
            url: "/system/insertCommonDetail",
            type: "Post",
            dataType: "json",
            data: {
                dc_mt_cd: dc_mt_cd,
                dc_dt_cd: dc_dt_cd,
                dc_dt_nm: dc_dt_nm,
                dc_dt_exp: dc_dt_exp,
                dc_dt_order: (dc_dt_order == "") ? 0 : dc_dt_order,
                dc_use_yn: dc_use_yn,
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.value.cdid;
                    $("#commondtGrid").jqGrid('addRowData', id, data.value, 'first');
                    $("#commondtGrid").setRowData(id, false, { background: "#d0e9c6" });
                    //jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }             
            }
        });
    }
});

//Modifly Common main
$("#dm_save_but").click(function () {
    if ($("#form4").valid() == true) {
        var dm_cdid = $("#dm_cdid_cd").val();
        var dm_mt_cd = $('#dm_mt_cd').val(); 
        var dm_dt_cd = $('#dm_dt_cd').val();
        var dm_dt_nm = $('#dm_dt_nm').val();
        var dm_dt_exp = $('#dm_dt_exp').val();
        var dm_dt_order = $('#dm_dt_order').val();
        var dm_use_yn = $('#dm_use_yn').val();
        $.ajax({
            url: "/system/updateCommonDetail",
            type: "Put",
            dataType: "json",
            data: {
                dm_cdid: dm_cdid,
                dm_mt_cd: dm_mt_cd,
                dm_dt_cd: dm_dt_cd,
                dm_dt_nm: dm_dt_nm,
                dm_dt_exp: dm_dt_exp,
                dm_dt_order: dm_dt_order,
                dm_use_yn: dm_use_yn,
            },
            success: function (data) {
                if (data.result == true) {
                    jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});

$("#tab_d1").on("click", "a", function (event) {
document.getElementById("form3").reset();
$("#tab_d2").removeClass("active");
$("#tab_d1").addClass("active");
$("#tab_dc2").removeClass("active");
$("#tab_dc1").removeClass("hidden");
$("#tab_dc2").addClass("hidden");
$("#tab_dc1").addClass("active");
var selectedRowId = $("#commonGrid").getGridParam('selrow');
//var mt_cd = $("#commonGrid").getCell(selectedRowId, "mt_cd");

//var dm_mt_cd = row_id2.mt_cd;
//$("#dm_mt_cd").val(dm_mt_cd);
//$("#dc_mt_nm").val(mt_nm);
//$("#dc_mt_cd").val(mt_cd);
//$("#dc_cdid_cd").val(1);
//$("#dc_dt_order").val(1);

$("#dc_mt_cd").val($("#m_mt_cd").val());
$("#dc_mt_nm").val($("#m_mt_nm").val());

});
$("#tab_d2").on("click", "a", function (event) {

document.getElementById("form4").reset();
$("#tab_d1").removeClass("active");
$("#tab_d2").addClass("active");
$("#tab_dc1").removeClass("active");
$("#tab_dc2").removeClass("hidden");
$("#tab_dc1").addClass("hidden");
$("#tab_dc2").addClass("active");
$("#dm_save_but").attr("disabled", true);
$("#ddel_save_but").attr("disabled", true);
});

function bntCellValue(cellValue, options, rowdata, action) {
var html = '<button class="btn btn-xs btn-danger deletebtn" title="Delete" data-commonmaincode="' + rowdata.mt_cd + '" >X</button>';
return html;
};

function bntCellValueDetail(cellValue, options, rowdata, action) {
var html = '<button class="btn btn-xs btn-danger detaildeletebtn" title="Delete" data-commondt_manincode="' + rowdata.mt_cd + '" data-commondt_dtcd="' + rowdata.dt_cd + '" >X</button>';
return html;
};

$("#form1").validate({
    rules: {
        "mt_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "mt_nm": {
            required: true,
        },
    },
});

$("#form3").validate({
    rules: {
        "dt_cd": {
            required: true,
        },
        "dt_nm": {
            required: true,
        },
    },
});

$("#form4").validate({
    rules: {
        "dt_cd": {
            required: true,
        },
        "dt_nm": {
            required: true,
        },
    },
});
