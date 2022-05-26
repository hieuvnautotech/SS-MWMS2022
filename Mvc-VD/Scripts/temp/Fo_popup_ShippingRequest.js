$(function () {
    $(".dialog3").dialog({
        width: '100%',
        height: 450,
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

            $("#popupfo").jqGrid
            ({
                url: "/Shippingmgt/getfoMgtpopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                  { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                   { key: false, label: 'FO NO', name: 'fo_no', width: 120, align: 'center' },
                   { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center' },
                   { key: false, label: 'Factory', name: 'lct_cd', width: 150, align: 'center' },
                   { key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
                   { key: false, label: 'Product real Date', name: 'product_real_dt', width: 150, align: 'left' },
                   { key: false, label: 'FO Qty', name: 'fo_qty', width: 100, align: 'right' },
                   { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
                   { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                   { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                   { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                   { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupfo").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupfo").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_fo_no').val(row_id.fo_no);
                        $('.dialog3').dialog('close');
                    }
                },

                pager: jQuery('#pagerMaterial'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'Material Information',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
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

    $(".poupdialogFono").click(function () {
        $('.dialog3').dialog('open');
    });

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#popupfo').jqGrid('setGridWidth', $(".ui-dialog").width());
    $(window).on("resize", function () {
        var newWidth = $("#popupfo").closest(".ui-jqgrid").parent().width();
        $("#popupfo").jqGrid("setGridWidth", newWidth, false);
    });

    $('#closefo').click(function () {
        $('.dialog3').dialog('close');
    });

});