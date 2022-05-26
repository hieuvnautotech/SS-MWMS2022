var row_id, row_id2;
$("#commonGrid").jqGrid
({
    url: "/System/GetJqGridData",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "mt_id", width: 50, align: "left", label: "id", resizable: true, hidden: true },
        { name: "mt_cd", width: 150, align: "center", label: "Main Code", resizable: true },
        { name: "mt_nm", width: 150, align: "center", label: "Main Name", resizable: true },
        { name: "mt_exp", width: 150, align: "center", label: "Main Explain", resizable: true },
        { name: "use_yn", width: 100, align: "center", label: "Use Y/N", resizable: true },
    ],

    onCellSelect: function (rowid, iCol, cellcontent, e) {
                       
        if (iCol == 6) {
                           
            $('.deletebtn').click(function () {
                var datacommonMaincode = $(this).data("commonmaincode");
                $.ajax({
                    url: "/system/deleteCommon",
                    type: "post",
                    dataType: "json",
                    data: {
                        commonMaincode: datacommonMaincode,
                    },
                    success: function (result) {
                        $("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        $("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        document.getElementById("form1").reset();
                        $("#tab_2").removeClass("active");
                        $("#tab_1").addClass("active");
                        $("#tab_c2").removeClass("active");
                        $("#tab_c1").removeClass("hidden");
                        $("#tab_c2").addClass("hidden");
                        $("#tab_c1").addClass("active");

                    },
                    error: function (result) {
                        alert('Data is not existing. Please check again');
                    }
                });
            });
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
        $("#del_save_but").attr("disabled", false);
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
    },
    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#commonGridPager',
    viewrecords: true,
    rowList: [10, 50, 100, 200],
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
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#commonGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#commonGrid").closest(".ui-jqgrid").parent().width();
    $("#commonGrid").jqGrid("setGridWidth", newWidth, false);
});

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
            url: "/system/insertCommon",
            type: "get",
            dataType: "json",
            data: {
                mt_cd: mt_cd,
                mt_nm: mt_nm,
                mt_exp: mt_exp,
                use_yn: use_yn,
            },
            success: function (data) {
                jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
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
        url: "/system/updateCommon",
        type: "get",
        dataType: "json",
        data: {
            mt_cd: mt_cd,
            mt_nm: mt_nm,
            mt_exp: mt_exp,
            use_yn: use_yn,
            mt_id: mt_id,
        },
        success: function (data) {
            jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        }
    });

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

       


$("#commondtGrid").jqGrid
({
    url: "/System/GetJqGridDataCommDt",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "cdid", width: 80, align: "left", label: "cdid", resizable: true, hidden: true },
        { name: "mt_cd", width: 80, align: "center", label: "Main Code", resizable: true },
        { name: "mt_nm", width: 120, align: "center", label: "Main Name", resizable: true },
        { name: "dt_cd", width: 100, align: "left", label: "Detail Code", resizable: true },
        { name: "dt_nm", width: 150, align: "left", label: "Detail Name", resizable: true },
        { name: "dt_exp", width: 250, align: "left", label: "Detail Explain", resizable: true },
        { name: "dt_order", width: 80, align: "right", label: "Order", resizable: true },
        { name: "use_yn", width: 80, align: "center", label: "Use Y/N", resizable: true },

    ],
    onCellSelect: function (rowid, iCol, cellcontent, e) {

        if (iCol == 9) {
            $('.detaildeletebtn').click(function () {
                var commondt_manincode = $(this).data("commondt_manincode");
                var commondt_dtcd = $(this).data("commondt_dtcd");
                $.ajax({
                    url: "/system/deleteCommon_dt",
                    type: "post",
                    dataType: "json",
                    data: {
                        commondt_manincode: commondt_manincode,
                        commondt_dtcd: commondt_dtcd
                    },
                    success: function (result) {
                        $("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        document.getElementById("form3").reset();

                        $("#tab_d2").removeClass("active");
                        $("#tab_d1").addClass("active");
                        $("#tab_dc2").removeClass("active");
                        $("#tab_dc1").removeClass("hidden");
                        $("#tab_dc2").addClass("hidden");
                        $("#tab_dc1").addClass("active");

                    },
                    error: function (result) {
                        alert('Data is not existing. Please check again');
                    }
                });
            });
        }
    },
    onSelectRow: function (rowid, status, e) {

        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#commondtGrid").jqGrid("getGridParam", 'selrow');
        row_id2 = $("#commondtGrid").getRowData(selectedRowId);

        var dm_cdid = row_id2.cdid;
        var dm_mt_cd = row_id2.mt_cd;
        var dm_dt_cd = row_id2.dt_cd;
        var dm_dt_nm = row_id2.dt_nm;
        var dm_dt_exp = row_id2.dt_exp;
        var dm_dt_order = row_id2.dt_order;
        var dm_use_yn = row_id2.dm_use_yn;

        $("#dm_cdid_cd").val(dm_cdid);
        $("#dm_mt_cd").val(dm_mt_cd);
        $("#dm_dt_cd").val(dm_dt_cd);
        $("#dm_dt_nm").val(dm_dt_nm);
        $("#dc_mt_cd").val(dm_mt_cd);


        $("#dm_dt_exp").val(dm_dt_exp);
        $("#dm_dt_order").val(dm_dt_order);
        $("#dm_use_yn").val(dm_use_yn);

        $("#tab_d1").removeClass("active");
        $("#tab_d2").addClass("active");
        $("#tab_dc1").removeClass("active");
        $("#tab_dc2").removeClass("hidden");
        $("#tab_dc1").addClass("hidden");
        $("#tab_dc2").addClass("active");
        $("#dm_save_but").attr("disabled", false);
    },

    autowidth: false,
    multiselect: false,
    sortable: true,
    loadonce: true,
    pager: '#commondtGridPager',
    viewrecords: true,
    rowList: [10, 50, 100, 200],
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
    var dc_mt_cd = $('#dc_mt_cd').val(); 
    var dc_dt_cd = $('#dc_dt_cd').val(); 
    var dc_dt_nm = $('#dc_dt_nm').val();
    var dc_dt_exp = $('#dc_dt_exp').val();
    var dc_dt_order = $('#dc_dt_order').val();
    var dc_use_yn = $('#dc_use_yn').val();

    if ($("#dc_dt_cd").val() == "") {
        alert("Please enter the code");
        return false;
    }
    if ($("#dc_dt_nm").val() == "") {
        alert("Please enter the name");
        return false;
    }
    if ($("#dc_dt_exp").val() == "") {
        alert("Please enter the explain");
        return false;
    }
    if ($("#dc_dt_order").val() == "") {
        alert("Please enter the detail order");
    }

    else {

        $.ajax({
            url: "/system/insertCommonDetail",
            type: "get",
            dataType: "json",
            data: {
                dc_mt_cd: dc_mt_cd,
                dc_dt_cd: dc_dt_cd,
                dc_dt_nm: dc_dt_nm,
                dc_dt_exp: dc_dt_exp,
                dc_dt_order: dc_dt_order,
                dc_use_yn: dc_use_yn,
            },
            success: function (data) {
                if (data.result == 0) {
                    jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else
                {
                    alert("This Common Detail was existing. Please checking again");
                }
                
            }
        });
    }
});

//Modifly Common main
$("#dm_save_but").click(function () {

    var dm_mt_cd = $('#dm_mt_cd').val(); 
    var dm_dt_cd = $('#dm_dt_cd').val(); 
    var dm_dt_nm = $('#dm_dt_nm').val();
    var dm_dt_exp = $('#dm_dt_exp').val();
    var dm_dt_order = $('#dm_dt_order').val();
    var dm_use_yn = $('#dm_use_yn').val();


    if ($("#dm_dt_order").val() == "") {
        var selectedRowId = $("#commondtGrid").getGridParam('selrow');
        var row = $("#commondtGrid").getRowData(selectedRowId);
        var dt_order = row.dt_order;
        $("#dm_dt_order").val(dt_order);
    }
    else {
        $.ajax({
            url: "/system/updateCommonDetail",
            type: "get",
            dataType: "json",
            data: {
                dm_mt_cd: dm_mt_cd,
                dm_dt_cd: dm_dt_cd,
                dm_dt_nm: dm_dt_nm,
                dm_dt_exp: dm_dt_exp,
                dm_dt_order: dm_dt_order,
                dm_use_yn: dm_use_yn,
            },
            success: function (data) {
                if (data.result != 0) {
                    jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("This Common Detail was existing. Please checking again");
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
var mt_cd = $("#commonGrid").getCell(selectedRowId, "mt_cd");

var dm_mt_cd = row_id2.mt_cd;
$("#dm_mt_cd").val(dm_mt_cd);

$("#dc_mt_cd").val(mt_cd);
$("#dc_cdid_cd").val(1);
$("#dc_dt_order").val(1);


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

function bntCellValue(cellValue, options, rowdata, action) {
var html = '<button class="btn btn-xs btn-danger deletebtn" title="Delete" data-commonmaincode="' + rowdata.mt_cd + '" >X</button>';
return html;
};

function bntCellValueDetail(cellValue, options, rowdata, action) {
var html = '<button class="btn btn-xs btn-danger detaildeletebtn" title="Delete" data-commondt_manincode="' + rowdata.mt_cd + '" data-commondt_dtcd="' + rowdata.dt_cd + '" >X</button>';
return html;
};