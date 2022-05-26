$(function () {
    $(".dialog_PO").dialog({
        width: '50%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 800,
        minWidth: '50%',
        minHeight: 800,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
           
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popups_PO").jqGrid
            ({
                url: "/PurchaseMgt/getPopupPO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'id', name: 'soid', width: 50, align: 'right', hidden: true },
                    { label: 'PO NO', name: 'po_no', width: 100, align: 'center' },
                    { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
                    { label: 'Product Name', name: 'style_nm', width: 250, align: 'left'  },
                    { label: 'BOM', name: 'bom_no', width: 110, align: 'center' },
                    { label: 'Order Date', name: 'order_dt', width: 120, align: 'center' },
                    { label: 'Delivery Date', name: 'delivery_dt', width: 120, align: 'center' },
                    { label: 'Destinationcd', name: 'lct_cd', width: 150, align: 'center', hidden: true },
                    { label: 'Destination', name: 'lct_nm', width: 150, align: 'left' },
                    { label: 'Delivery Qty', name: 'delivery_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Qty', name: 'fo_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Remain Qty', name: 'fo_rm_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Qty', name: 'so_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Remain Qty', name: 'so_rm_qty', width: 150, align: 'right', formatter: 'integer' },
                    { label: 'Description', name: 're_mark', width: 300, align: 'left' },
                  
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popups_PO").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups_PO").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#selectPO').click(function () {
                            $('#po_no').val(row_id.po_no);
                            $('#c_po_no').val(row_id.po_no);
                            $('#c_bom_no').val(row_id.bom_no);
                            $('#m_po_no').val(row_id.po_no);
                            $('.dialog_PO').dialog('close');
                        });
                    }
                },
                pager: jQuery('#popupspager_PO'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000,5000,10000],
                rowNum:50,
                height: 600,
                //width: $(".ui-dialog").width() - 50,
                autowidth: true,
                caption: 'PO',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                multiselect: false,
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
            });

        },
    });

    $(".popup_po_no").click(function () {
        $('.dialog_PO').dialog('open');
    });
    
    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popups').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popups").closest(".ui-jqgrid").parent().width();
    //    $("#popups").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closePO').click(function () {
        $('.dialog_PO').dialog('close');
    });

});

$("#searchBtn_po_popup").click(function () {
    var s_po_no = $('#s_po_no').val().trim();
    var s_style_no = $('#s_style_no').val().trim(); 
    var s_start = $('#s_start').val().trim();
    var s_end = $('#s_end').val().trim();
    var s_bom_no = $('#s_bom_no').val().trim();
    var s_destination = $('#s_destination').val().trim();

    $.ajax({
        url: "/PurchaseMgt/search_po_popup",
        type: "get",
        dataType: "json",
        data: {
            s_po_no: s_po_no,
            s_style_no: s_style_no,
            s_start: s_start,
            s_end: s_end,
            s_bom_no: s_bom_no,
            s_destination: s_destination,
        },
        success: function (result) {
            $("#popups_PO").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
//destination
var destination = "/PurchaseMgt/getDestination";
$(document).ready(function () {
    _getdestination();
});
function _getdestination() {

    $.get(destination, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Destination*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#s_destination").html(html);
        }
    });
}
// end destination
$("#s_start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#s_end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
