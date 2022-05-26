$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        url: "/fgwms/getw_product_lct_hist",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            //{ key: true, label: 'plno', name: 'plno', width: 50, align: 'right', hidden: true },
            //{ label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            //{ label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            //{ label: 'Status', name: 'sts_cd', width: 100, align: 'center' },
            //{ label: 'Status', name: 'sts_cd1', width: 100, align: 'left', hidden: true  },
            //{ label: 'WO', name: 'fo_no', width: 100, align: 'center' },
            //{ label: 'Location', name: 'lct_nm', width: 100, align: 'left' },
            //{ label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left' },
            //{ label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
            //{ label: 'Out Date', name: 'output_dt', width: 150, align: 'center' },
            //{ label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            //{ label: 'Input Date', name: 'input_dt', width: 150, align: 'left' },
            //{ label: 'QR Code', name: 'qrcode', width: 350, align: 'left' },
            //{ label: 'PO', name: 'po_no', width: 100, align: 'center' },
            //{ label: 'WO', name: 'fo_no', width: 100, align: 'center' },
            //{ label: 'BOM', name: 'bom_no', width: 100, align: 'center' },
            //{ label: 'Routing', name: 'line_no', width: 100, align: 'center' },
            //{ label: 'Description', name: 're_mark', width: 150, align: 'left' },
            //{ label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            //{ key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            //{ key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { name: 'style_no', label: 'Product Code', width: 200, align: 'left', key: true, hidden: false },
            { name: 'style_nm', label: 'Product Name', width: 400, align: 'left' },
            { name: 'total', label: 'Total Quantity', width: 200, align: 'right', formatter: 'integer', },
            { name: 'available', label: 'Available Quantity', width: 200, align: 'right', formatter: 'integer', },
            { name: 'pending', label: 'Pending Quantity', width: 200, align: 'right', formatter: 'integer', },
            { name: 'sent', label: 'Sent Quantity', width: 200, align: 'right', formatter: 'integer', },
        ],
        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: 600,
        //width: $(".box-body").width() - 5,
        //autowidth: false,
        rowNum: 20,
        caption: 'Product Location History',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
        width: '100%',
        autowidth: true,
        subGrid: true, // set the subGrid property to true to show expand buttons for each row
        subGridRowExpanded: showChildGrid,
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

    //$("#searchBtn").click(function () {
    //    var prd_lcd = $('#s_mt_cd').val().trim();
    //    var style_no = $('#m_style_no').val().trim();
    //    var start = $('#start').val().trim();
    //    var start1 = $('#start1').val().trim();
    //    var lct_cd = $('#c_lct_cd').val();
    //    var end = $('#end').val();
    //    var end1 = $('#end1').val();
    //    var fo_no = $('#s_fo_no').val().trim();
    //    var po_no = $('#s_po_no').val().trim();
    //    var bom_no = $('#c_bom_no').val().trim();
    //    var lct_sts_cd = $('#lct_sts_cd').val().trim();
    //    $.ajax({
    //        url: "/fgwms/searchw_product_lct_hist",
    //        type: "get",
    //        dataType: "json",
    //        data: {
    //            prd_lcd: prd_lcd,
    //            style_no: style_no,
    //            lct_cd: lct_cd,
    //            start: start,
    //            start1: start1,
    //            end1: start1,
    //            end: end,
    //            bom_no: bom_no,
    //            po_no: po_no,
    //            fo_no: fo_no,
    //            lct_sts_cd: lct_sts_cd,
    //        },
    //        success: function (result) {
    //            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
    //        }
    //    });
    //});

    
});//grid1

function showChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    var prd_lct = $('#s_mt_cd').val() == null ? "" : $('#s_mt_cd').val().trim();
    var start_in = $('#start').val() == null ? "" : $('#start').val().trim();
    var end_in = $('#end').val() == null ? "" : $('#end').val().trim();
    var start_out = $('#start1').val() == null ? "" : $('#start1').val().trim();
    var end_out = $('#end1').val() == null ? "" : $('#end1').val().trim();
    // send the parent row primary key to the server so that we know which grid to show
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $("#" + parentRowID).html('<table id="' + childGridID + '"></table><div id="' + childGridPagerID + '" class="scroll"></div>');

    $("#" + childGridID).jqGrid({
        url: "/fgwms/ProductInOut?id=" + parentRowKey + "&prd_lot=" + prd_lct + "&start_in=" + start_in + "&end_in=" + end_in + "&start_out=" + start_out + "&end_out=" + end_out,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'plno', name: 'plno', width: 50, hidden: true },

            { label: 'Product Code', name: 'prd_cd', width: 360, align: 'left' },
            { label: 'Product Lot Code', name: 'prd_lcd', width: 500, align: 'left' },

            { label: 'WO No', name: 'fo_no', width: 150, align: 'center' },
            { label: 'PO No', name: 'po_no', width: 100, align: 'center' },
            { label: 'BOM No', name: 'bom_no', width: 100, align: 'center' },
            { label: 'Input Date', name: 'input_dt', width: 100, align: 'center', formatter: formatdate },
            { label: 'Output Date', name: 'output_dt', width: 100, align: 'center', formatter: formatdate },

        ],
        shrinkToFit: false,
        loadonce: true,
        width: '100%',
        height: '100%',
        //subGrid: false, // set the subGrid property to true to show expand buttons for each row
        //subGridRowExpanded: showThirdLevelChildGrid, // javascript function that will take care of showing the child grid
        pager: '#' + childGridPagerID,
        rowNum: 20,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [20, 50, 80],
        viewrecords: true,
        rowNum: 20,
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
    //$("#" + childGridID).jqGrid('filterToolbar'/*, {
    //    stringResult: true, searchOnEnter: false,
    //    defaultSearch: '', ignoreCase: true
    //}*/);
    //$("#" + childGridID).jqGrid('navGrid', "#" + childGridPagerID, {
    //    search: false, // show search button on the toolbar
    //    add: false,
    //    edit: false,
    //    del: false,
    //    refresh: true
    //});
}

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#start1").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end1").datepicker({ dateFormat: 'yy-mm-dd' }).val();
var getLocation = "/fgwms/getLocation";

$(document).ready(function () {
    _getLocation();
});
function _getLocation() {

    $.get(getLocation, function (data) {
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
var getsts = "/fgwms/getsts_product";
var GetLocationsts = "/fgwms/GetLocationsts";

$(document).ready(function () {
    _getsts();
    _GetLocationsts();
});
function _getsts() {

    $.get(getsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#sts_cd").html(html);
        }
    });
}
function _GetLocationsts() {
    $.get(GetLocationsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> Location Status</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#lct_sts_cd").html(html);
        }
    });
}

$("#searchBtn").click(function () {
    $('#loading').show();
    var prd_lcd = $('#s_mt_cd').val().trim();
    var style_no = $('#m_style_no').val().trim();
    var bom_no = $('#c_bom_no').val().trim();
    var start_in = $('#start').val() == null ? "" : $('#start').val().trim();
    var end_in = $('#end').val() == null ? "" : $('#end').val().trim();
    var start_out = $('#start1').val() == null ? "" : $('#start1').val().trim();
    var end_out = $('#end1').val() == null ? "" : $('#end1').val().trim();
    //var lct_sts_cd = $('#lct_sts_cd').val().trim();
    //var lct_cd = $('#c_lct_cd').val();
    //var fo_no = $('#s_fo_no').val().trim();
    //var po_no = $('#s_po_no').val().trim();
    
    $.ajax({
        //url: "/fgwms/searchw_product_lct_hist",
        url: "/fgwms/getw_product_lct_hist",
        type: "get",
        dataType: "json",
        data: {
            prd_lcd: prd_lcd,
            style_no: style_no,
            bom_no: bom_no,
            start_in: start_in,
            end_in: end_in,
            start_out: start_out,
            end_out: end_out,

            //lct_sts_cd: lct_sts_cd,
            //lct_cd: lct_cd,
            //fo_no: fo_no,
            //po_no: po_no,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            var rowIds = $("#list").getDataIDs();
            var flag = $('#s_mt_cd').val();
            var check = $('#m_style_no').val();


            if (flag || check) {
                $.each(rowIds, function (index, rowId) {
                    for (var i = 0; i < result.length; i++) {
                        if (rowId == result[i].style_no) {
                            $("#list").expandSubGridRow(rowId);
                            break;
                        }
                    }
                    
                    
                });
            }
            $('#loading').hide();
        }
    });
});

$("#c_save_but").click(function () {
    $.ajax({
        url: "/fgwms/updatepr_mgt",
        type: "get",
        dataType: "json",
        data: {
            plno: $('#plno').val(),
            re_mark: $('#re_mark').val(),
            sts_cd: $('#sts_cd').val(),

        },
        success: function (data) {
            var id = data.plno;
            $("#list").setRowData(id, data, { background: "#d0e9c6" });
            document.getElementById("form1").reset();
        }
    });
});

function formatdate(cellValue, options, rowdata, action) {

    if (cellValue != null && cellValue != "") {

        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);

        var html = a + "-" + b + "-" + c;
        return html;
    }
    else {
        var html = "";
        return html;
    }
};