$(function () {
    $(".dialogActual").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 600,
        minWidth: '50%',
        minHeight: 600,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            //"ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupsActual").jqGrid
            ({
                url: "/Plan/getWONO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                     { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                     { key: false, label: 'WO NO', name: 'fo_no', width: 120, align: 'center' },
                     { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center' },
                     { key: false, label: 'Bom', name: 'bom_no', width: 120, align: 'center' },
                     { key: false, label: 'Factory', name: 'lct_nm', width: 120, align: 'left' },
                     //{ key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
                     //{ key: false, label: 'Product Real Date', name: 'product_real_dt', width: 150, align: 'left' },
                     { key: false, label: 'FO Qty', name: 'target_qty', width: 100, align: 'right', formatter: 'integer' },
                     { key: false, label: 'Remark', name: 're_mark', width: 150, align: 'left' },
                     { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                     { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                     { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                     { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    $("#SelectWO").removeClass("disabled");
                    var selectedRowId = $("#popupsActual").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsActual").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#SelectWO').click(function () {
                            $('#fo_no').val(row_id.fo_no)
                            $('.dialogActual').dialog('close');
                        });
                    }
                },

                pager: jQuery('#popupspagerActual'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                rowNum: 50,
                loadonce: true,
                height: 400,
                //width: $(".ui-dialog").width() - 5,
                autowidth: true,
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
    $(".ACtualWopopup").click(function () {
        $('.dialogActual').dialog('open');
    });
    $("#SelectWO").click(function () {
      
    });
    $("#closeWO").click(function () {
        $('.dialogActual').dialog('close');
    });
});