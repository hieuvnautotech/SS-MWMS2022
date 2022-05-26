$(function () {
    $(".pp_wo").dialog({
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

            $("#popupwo").jqGrid
            ({
                url: "/Lot/getWoNoPopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                     { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                    { key: false, label: 'WO NO', name: 'fo_no', width: 120, align: 'center' },
                    { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center' },
                    { key: false, label: 'Line NO', name: 'process_no', width: 120, align: 'center' },
                    { key: false, label: 'Bom', name: 'bom_no', width: 120, align: 'center' },
                    { key: false, label: 'Factory', name: 'lct_cd', width: 120, align: 'left' },
                    { key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
                    { key: false, label: 'Product Real Date', name: 'product_real_dt', width: 150, align: 'left' },
                    { key: false, label: 'FO Qty', name: 'fo_qty', width: 100, align: 'right', formatter: 'integer' },
                    { key: false, label: 'Remark', name: 're_mark', width: 150, align: 'left' },
                    { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                    { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                    { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupwo").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupwo").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected2").click(function () {
                            $('#s_fo_no').val(row_id.fo_no);
                            $('.pp_wo').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerwo'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'WO',
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

    $(".poupdialogWO").click(function () {
        $('.pp_wo').dialog('open');
    });
    $('#closewo').click(function () {
        $('.pp_wo').dialog('close');
    });

});
$("#searchBtn_popupfo_no").click(function () {
    $.ajax({
        url: "/Lot/searchfo_noPopup",
        type: "get",
        dataType: "json",
        data: {
            fo_no: $('#fo_no').val(),
        },
        success: function (result) {
            $("#popupwo").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
