$("#tab_1").on("click", "a", function (event) {
    $("#tab_1").addClass("active");
    $("#tab_2").removeClass("active");

    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").removeClass("active");

    $("#tab_c1").addClass("active");
    $("#tab_c2").addClass("hidden");
});
$("#tab_2").on("click", "a", function (event) {

    $("#listMeno").clearGridData();
    $("#listMeno").jqGrid('setGridParam', { search: true });
   var pdata = $("#listMeno").jqGrid('getGridParam', 'postData');
    getDatalistMeno(pdata)

    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");

    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");

    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});
//DOCUMENT READY
$(document).ready(function (e) {
    $(`#s_month`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm'
    }).datepicker("setDate", new Date());
    $(`#s_date`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());
});
$(function () {
    $("#list").jqGrid
        ({
            datatype: function (postData) { getDataList(postData); },

            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                { label: 'Shipping Date', name: 'reg_date_convert', width: 100, align: 'center' },
                { label: 'Product', name: 'product', width: 150, align: 'left' },
                { label: 'Material', name: 'MaterialNo', width: 150, align: 'left' },
                { label: 'Unit', name: 'unit_cd', width: 80, align: 'center' },

                //{ label: 'Lot No', name: 'lot_no', width: 150, align: 'left' },
                { label: 'Tổng số cuộn ', name: 'countSocuon', width: 200, align: 'left' },
                { label: 'Tổng số mét', name: 'TongSoMet', width: 200, align: 'left' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);
            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Show Material Shipping',
            emptyrecords: "No data.",
            height: 400,
            width: null,
            autowidth: false,
            shrinkToFit: false,
            subGrid: true,
            subGridRowExpanded: showChildGridMaterial,
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
});



//-------SEARCH------------//
$("#searchBtn").click(function () {
    $('#list').clearGridData();
    $('#list').jqGrid('setGridParam', { search: true });
    var pdata = $('#list').jqGrid('getGridParam', 'postData');
    getDataList(pdata);

    $("#listMeno").clearGridData();
    $("#listMeno").jqGrid('setGridParam', { search: true });
    var pdata = $("#listMeno").jqGrid('getGridParam', 'postData');
    getDatalistMeno(pdata)


});


function getDataList(pdata) {
    var params = new Object();

    if ($('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.datemonth = $("#s_month").val().trim();
    params.date = $("#s_date").val().trim();
    params.product = $("#s_style_no").val().trim();
    params.material = $("#s_mt_no").val().trim();

//    $('#list').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/ShippingMgt/searchShowMaterialShipping`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

var childGridID = "";

function showChildGridMaterial(parentRowID, parentRowKey) {

    parentRowKey1 = parentRowKey;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $('#list').jqGrid('getRowData', parentRowKey);
  var datemonth = $("#s_month").val().trim();

    $("#" + childGridID).jqGrid({
        url: "/ShippingMgt/getShowMaterialShippingDetail?style_no=" + rowData.product + "&recei_dt=" + rowData.reg_date_convert + "&mt_no=" + encodeURIComponent(rowData.MaterialNo) + "&datemonth=" + datemonth,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 100, align: 'center', hidden: true },

            { label: 'SD No', name: 'sd_no', width: 100 },
            { label: 'MT Code', name: 'mt_cd', width: 300 },
            { label: 'Real Quantity', name: 'countSocuon', width: 100 },
            { label: 'Quantity', name: 'TongSoMet', width: 100 },
            { label: 'Receving Date', name: 'receving_date_convert',  align: 'center',width: 150 },
            { label: 'Lot No', name: 'lot_no', width: 100 },


        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
            row_id = $("#" + childGridID).getRowData(selectedRowId);


        },
        shrinkToFit: false,
        loadonce: false,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rownumbers: true,

        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,

        pager: "#" + childGridPagerID,


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

            var rows = jQuery("#" + childGridID).getDataIDs();
            for (var i = 0; i < rows.length; i++) {

            }

        }
    });
}
//--------------list memo-----------------
$(function () {
    $("#listMeno").jqGrid
        ({
            datatype: function (postData) { getDatalistMeno(postData); },

            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                { label: 'Shipping Date', name: 'reg_date_convert', width: 100, align: 'center' },
                { label: 'Product', name: 'product', width: 150, align: 'left' },
                { label: 'Material', name: 'mt_no', width: 150, align: 'left' },
                { label: 'Lot No', name: 'lot_no', width: 150, align: 'left' },
                { label: 'Width', name: 'width', width: 80, align: 'center' },
                { label: 'Spec', name: 'spec', width: 80, align: 'center' },
                { label: 'Total Roll', name: 'total_roll', width: 100, align: 'left' },
                { label: 'Total M', name: 'total_m', width: 100, align: 'left' },
                { label: 'Total M2', name: 'total_m2', width: 100, align: 'left' },
                { label: 'Total EA', name: 'total_ea', width: 100, align: 'left' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#listMeno").jqGrid("getGridParam", 'selrow');
                var rowData = $("#listMeno").getRowData(selectedRowId);
            },

            pager: jQuery('#listPagerMemo'),
            rowNum: 200,
            rowList: [200, 500, 1000, 5000, 10000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Show Material Memo',
            emptyrecords: "No data.",
            height: 500,
            width: null,
            autowidth: false,
            shrinkToFit: false,
            //subGrid: true,
            //subGridRowExpanded: showChildGridMaterial,
            loadComplete: function () {
                var rowIds = gridMemo.jqGrid('getDataIDs');
                for (var item of rowIds) {
                    gridMemo.expandSubGridRow(item);
                }
            },
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
});
function getDatalistMeno(pdata) {
    var params = new Object();
    var rows = $("#listMeno").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.datemonth = $("#s_month").val().trim();
    params.date = $("#s_date").val().trim();
    params.product = $("#s_style_no").val().trim();
    params.material = $("#s_mt_no").val().trim();

 //   $("#listMeno").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/ShippingMgt/GetMemo",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#listMeno")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};