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

$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        //url: "/fgwms/GetProLotReceivelist",
        url: "/fgwms/searchProLotReceivelist",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'plno', name: 'plno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Finish Lot', name: 'prd_lcd', width: 300, align: 'left' },
            { key: false, label: 'Product Code', name: 'style_no', sortable: true, width: '200', align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', sortable: true, width: '250', align: 'left' },
            { key: false, label: 'Qty', name: 'gr_qty', width: 50, align: 'right', formatter: 'integer' },
            { key: false, label: 'Product date', name: 'prd_dt', editable: true, width: '100px', align: 'center', formatter: formatdate, hidden: true },
            { key: false, label: 'Status', name: 'sts_cd', editable: true, width: '80', align: 'center' },
            //{ key: false, label: 'Location', name: 'lct_cd', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Location', name: 'lct_nm', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Location Status', name: 'lct_sts_cd', editable: true, width: '100px', align: 'center', hidden: true },
            //{ key: false, label: 'Departure', name: 'from_lct_cd', editable: true, width: '150', align: 'center' },
            //{ key: false, label: 'Out Date', name: 'output_dt', editable: true, width: '100px', align: 'center' },
            //{ key: false, label: 'Destination', name: 'to_lct_cd', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Input Date', name: 'input_dt', editable: true, width: '100px', align: 'center', formatter: formatdate },
            { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'QR Code', name: 'qrcode', editable: true, width: '400', align: 'left', hidden: true },
            { key: false, label: 'PO', name: 'po_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'WO', name: 'fo_no', editable: true, width: '100', align: 'center' },
            { key: false, label: 'BOM', name: 'bom_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Routing', name: 'line_no', editable: true, width: '100', align: 'center', },
            //{ key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px', sortable: true, },
            { key: false, label: 'Put Away', name: 'chg_id', editable: true, width: '100px', sortable: true, align: 'center' },
            { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, },
            { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, }
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);


        },
        gridComplete: function () {
            var rows = $("#list").getDataIDs();
            for (var i = 0; i < rows.length; i++) {


            }
        },
        pager: jQuery('#gridpager'),
        rowNum: 500,
        rowList: [500, 1000, 2000, 5000, 10000],
        rownumbers: true,
        autowidth: true,
        width: '100%',
        shrinkToFit: false,
        viewrecords: true,
        height: '450',
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

        multiselect: false,
    });
    
});//grid1

var getDest = "/fgwms/getDest";
$(document).ready(function () {
    _getDestC();
    $("#to_lct_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getDestC() {

    $.get(getDest, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Dest*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#to_lct_cd").html(html);
        }
    });
}
var getStatus = "/fgwms/getStatus";
$(document).ready(function () {
    _getStatusC();
    $("#sts_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getStatusC() {

    $.get(getStatus, function (data) {
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
$("#searchBtn").click(function () {
    var prd_lcd = $('#prd_lcd').val().trim(),
        to_lct_cd = $("#to_lct_cd").val().trim(),
        style_no = $("#style_no").val().trim(),
        po_no = $("#po_no").val().trim(),
        fo_no = $("#fo_no").val().trim(),
        bom_no = $("#bom_no").val().trim(),
        start = $("#start").val(),
        end = $("#end").val();
    //sts_cd = $("#sts_cd").val();

    $.ajax({
        url: "/fgwms/searchProLotReceivelist",
        type: "get",
        dataType: "json",
        data: {
            prd_lcd: prd_lcd,
            to_lct_cd: to_lct_cd,
            style_no: style_no,
            po_no: po_no,
            fo_no: fo_no,
            bom_no: bom_no,
            start: start,
            end: end,
            //sts_cd: sts_cd,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});