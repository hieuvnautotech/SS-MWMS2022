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
                url: "/Lot/getcomLot",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                    { label: 'Lot Code', name: 'mt_cd', width: 350, align: 'left' },
                    { label: 'Material', name: 'mt_no', width: 100, align: 'left' },
                    { label: 'Factory', name: 'lct_cd', width: 100, align: 'left' },
                    { label: 'Process', name: 'prounit_cd', width: 100, align: 'left' },
                    { label: 'Date', name: 'date', width: 100, align: 'center' },
                    { label: 'GR QTY', name: 'gr_qty', width: 100, align: 'right' },
                    { label: 'QR Code', name: 'mt_barcode', width: 350, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popuplot").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popuplot").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected").click(function () {
                            $('#s_mt_cd').val(row_id.mt_cd);
                            $('#mt_no').val(row_id.mt_no);
                            $('.lot_pp').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerlot'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
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



$("#searchBtn_popuplot").click(function () {
    var mt_cd = $('#mt_cd').val().trim();
    $.ajax({
        url: "/Lot/searchlotPopup",
        type: "get",
        dataType: "json",
        data: {
            mt_cd: mt_cd,
        },
        success: function (result) {
            $("#popuplot").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});