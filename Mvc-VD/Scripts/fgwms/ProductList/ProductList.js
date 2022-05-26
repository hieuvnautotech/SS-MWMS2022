$(function () {
    $("#list").jqGrid
    ({
        url: "/fgwms/GetproList",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pno', name: 'pno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Finish Lot', name: 'prd_lcd', width: 300, align: 'left' },
            { key: false, label: 'Finish Code', name: 'prd_cd', width: 300, align: 'left' },
            { key: false, label: 'Product Code', name: 'style_no', sortable: true, width: '200', align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', sortable: true, width: '250', align: 'left' },
            { key: false, label: 'Lot division', name: 'lot_div_cd', editable: true, width: '150', align: 'center' },
            { key: false, label: 'Product date', name: 'prd_dt', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Status', name: 'sts_cd', editable: true, width: '80', align: 'center' },
            { key: false, label: 'Location', name: 'lct_cd', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Location Status', name: 'lct_sts_cd', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Departure', name: 'from_lct_cd', editable: true, width: '130', align: 'center' },
            { key: false, label: 'Out Date', name: 'output_dt', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Destination', name: 'to_lct_cd', editable: true, width: '130', align: 'center' },
            { key: false, label: 'Input Date', name: 'input_dt', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'QR Code', name: 'qrcode', editable: true, width: '300', align: 'left' },
            { key: false, label: 'PO', name: 'po_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'WO', name: 'fo_no', editable: true, width: '100', align: 'center' },
            { key: false, label: 'BOM', name: 'bom_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Routing', name: 'line_no', editable: true, width: '100', align: 'center', },
            { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px', sortable: true, },
            { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, },
            { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px', sortable: true, },
            { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, }
        ],

        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
        },
        gridComplete: function () {
            var rows = $("#list").getDataIDs();
            //for (var i = 0; i < rows.length; i++) {
            //}
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
    })

    
});//grid1

$("#searchBtn").click(function () {
    var prd_lcd = $('#prd_lcd').val().trim();
    prd_cd = $("#prd_cd").val().trim(),
    lot_div_cd = $("#lot_div_cd").val().trim(),
    style_no = $("#style_no").val().trim(),
    po_no = $("#po_no").val().trim(),
    fo_no = $("#fo_no").val().trim(),
    bom_no = $("#bom_no").val().trim(),
    start = $("#start").val(),
    end = $("#end").val();

    $.ajax({
        url: "/fgwms/searchproList",
        type: "get",
        dataType: "json",
        data: {
            prd_lcd: prd_lcd,
            prd_cd: prd_cd,
            lot_div_cd: lot_div_cd,
            style_no: style_no,
            po_no: po_no,
            fo_no: fo_no,
            bom_no: bom_no,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});


var getDivision = "/fgwms/getDivision";
$(document).ready(function () {
    _getDivision();
});
function _getDivision() {

    $.get(getDivision, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Lot Division*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#lot_div_cd").html(html);
        }
    });
}