var getFactory = "/wipwms/Warehouse_wip";
var GetType = "/wipwms/GetType";
var GetLocationsts = "/wipwms/GetLocationsts";
var GetStatus = "/wipwms/GetStatus";
$(document).ready(function () {
    _getFactory();
    _GetType();
    _GetLocationsts();
    _GetStatus();
});
function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
}
function _GetLocationsts() {
    $.get(GetLocationsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#lct_sts_cd").html(html);
        }
    });
}
function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd").html(html);
        }
    });
}
function _GetType() {
    $.get(GetType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".mt_type").html(html);
        }
    });
}
$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$(function () {
    $grid = $("#Material_jqgrid").jqGrid
   ({
       //url: "/wipwms/Getw_mate_info_wip",
       url: "/wipwms/searchwmaterial_info_wip",         
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },     
       { label: 'BOM NO', name: 'rel_bom', width: 100, align: 'left', hidden: true },
            { label: 'BOM NO', name: 'rel_bom1', width: 100, align: 'left', formatter: bom_no_subtr2 },
           { label: 'MT NO', name: 'mt_no', width: 300, align: 'left'},
           { label: 'MT Name', name: 'mt_nm', width: 300, align: 'left' },
           //{ label: 'Type', name: 'mt_type', width: 80, align: 'left' },
           { label: 'Receiving Qty', name: 'wait_qty', width: 100, align: 'right', formatter: 'integer', hidden: false },
           { label: 'Stock Qty', name: 'stock_qty', width: 80, align: 'right', formatter: 'integer' },
           { label: 'Using Qty', name: 'using_qty', width: 80, align: 'right', formatter: 'integer' },
           { label: 'Used Qty', name: 'used_qty', width: 80, align: 'right', formatter: 'integer' },
           //{ label: 'Status', name: 'mt_sts_cd', width: 80, align: 'left' },
           //{ label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
           //{ label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left' },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       pager: "#Material_jqgridGridPager",
       pager: jQuery('#Material_jqgridGridPager'),
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: 600,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 50,
       caption: '',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       loadonce: true,
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
   })
    
});

function bom_no_subtr2(cellvalue, options, rowObject) {
    var a, b, c, d;
    cellvalue = rowObject.rel_bom;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);


        return a + c + d;
    }
};


$("#searchBtn").click(function () {
    $.ajax({
        url: "/wipwms/searchwmaterial_info_wip",
        type: "get",
        dataType: "json",
        data: {
            bom_no: $("#bom_no").val().trim(),
            mt_no: $("#c_mt_no").val().trim(),
            //prd_lcd: $("#prd_lcd").val().trim(),
           
            //mt_nm: $("#s_mt_nm").val().trim(),
            //mt_type: $("#s_mt_type").val().trim(),
            //mt_sts_cd: $("#mt_sts_cd").val().trim(),
            //lct_cd: $("#c_lct_cd").val(),
            //lct_sts_cd: $("#lct_sts_cd").val().trim(),
            //start: $("#start").val().trim(),
            //end: $("#end").val().trim(),
        },
        success: function (result) {
            $("#Material_jqgrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
