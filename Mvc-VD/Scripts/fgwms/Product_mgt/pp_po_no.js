$(function () {
    $(".pp_po_no").dialog({
        width: '50%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popuppo_no").jqGrid
            ({
                url: "/Lot/getPoNoPopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { key: true, label: 'id', name: 'soid', width: 50, align: 'right', hidden: true },
                    { label: 'PO NO', name: 'po_no', width: 100, align: 'center' },
                    { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
                    { label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
                    { label: 'BOM', name: 'bom_no', width: 110, align: 'center' },
                    { label: 'Order Date', name: 'order_dt', width: 120, align: 'center' },
                    { label: 'Delivery Date', name: 'delivery_dt', width: 120, align: 'center' },
                    { label: 'Destinationcd', name: 'dest_cd', width: 150, align: 'center', hidden: true },
                    { label: 'Destination', name: 'dest_nm', width: 150, align: 'left' },
                    { label: 'Delivery Qty', name: 'delivery_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Qty', name: 'fo_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Remain Qty', name: 'fo_rm_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Qty', name: 'so_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Remain Qty', name: 'so_rm_qty', width: 150, align: 'right', formatter: 'integer' },
                    { label: 'Description', name: 're_mark', width: 300, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popuppo_no").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popuppo_no").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected3").click(function () {
                            $('#s_po_no').val(row_id.po_no);
                            $('.pp_po_no').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerpo_no'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'PO',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                loadonce: true,
                gridview: true,
                shrinkToFit: false,
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


            });

        },
    });

    $(".poupdialogpo").click(function () {
        $('.pp_po_no').dialog('open');
    });
    $('#closepo_no').click(function () {
        $('.pp_po_no').dialog('close');
    });

});
$("#searchBtn_popuppo_no").click(function () {
    var po_no = $('#po_no').val().trim();
    $.ajax({
        url: "/Lot/searchPO_NOPopup",
        type: "get",
        dataType: "json",
        data: {
            po_no: po_no,
        },
        success: function (result) {
            $("#popuppo_no").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});