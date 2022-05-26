$(function () {
    $(".lot_pp").dialog({
        width: '55%',
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

            $("#popuplot").jqGrid
            ({
                url: "/Lot/getprd_lot",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { key: true, label: 'pno', name: 'pno', width: 50, align: 'right', hidden: true },
                   { label: 'Lot Code', name: 'prd_lcd', width: 350, align: 'left' },
                   { label: 'Product', name: 'style_no', width: 250, align: 'left' },
                   { label: 'Group QTY', name: 'gr_qty', width: 100, align: 'right' },
                   { label: 'WO', name: 'fo_no', width: 150, align: 'center' },
                   { label: 'Factory', name: 'lct_cd', width: 100, align: 'left' },
                   { label: 'Date', name: 'prd_dt', width: 100, align: 'center' },
                   { label: 'QR Code', name: 'qrcode', width: 100, align: 'left' },
                   { label: 'Barcode', name: 'barcode', width: 100, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popuplot").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popuplot").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected").click(function () {
                            $('#s_mt_cd').val(row_id.prd_lcd);
                            $('#mt_no').val(row_id.style_no);
                            $('.lot_pp').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerbobbin'),
                viewrecords: true,
                //rowList: [20, 50, 200, 500],
                height: 600,
                //width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'Lot',
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

    $(".poupdialoglot").click(function () {
        $('.lot_pp').dialog('open');
    });
    $('#closelot').click(function () {
        $('.lot_pp').dialog('close');
    });

});


$("#selected").click(function () {
    //$('#s_mt_cd').val(row_id.mt_cd);
    $('#prd_lcd').val(row_id.prd_lcd);
    $('.lot_pp').dialog('close');
});

$("#searchBtn_popuplot").click(function () {
    var mt_cd = $('#pp_lot_code').val().trim();
    $.ajax({
        url: "/fgwms/searchfinishPopup",
        type: "get",
        dataType: "json",
        data: {
            pp_lot_code: mt_cd,
        },
        success: function (result) {
            $("#popuplot").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});