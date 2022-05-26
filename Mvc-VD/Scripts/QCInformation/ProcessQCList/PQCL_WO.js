$(function () {
    $(".dialogWO").dialog({
        width: '50%',
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
            //"ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupsWO").jqGrid
            ({
                url: "/QCInformation/getWoNoPopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                 { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                    { key: false, label: 'WO NO', name: 'fo_no', width: 120, align: 'center', formatter: fo_no },
                    { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center', formatter: po_no },
                 { key: false, label: 'Routing', name: 'process_no', width: 120, align: 'center' },
                 { key: false, label: 'Bom', name: 'bom_no', width: 120, align: 'center', formatter: bom_no },
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
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsWO").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsWO").getRowData(selectedRowId);
                    if (row_id != null) {
                        //$('#fo_no').val(row_id.fo_no);
                        //$('.dialogWO').dialog('close');
                    }
                },

                pager: jQuery('#popupspagerWO'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: 902,

                autowidth: true,
                caption: '  WO info',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                loadonce: true,
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
        $('.dialogWO').dialog('open');
    });

    $('#savestyle').click(function () {
        if (row_id != null) {
            $('#fo_no').val(row_id.fo_no);
            $('.dialogWO').dialog('close');
        }

    });


    function fo_no(cellvalue, options, rowObject) {
        var a, b;
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        c = parseInt(b);
        return a + c;
    };
    function po_no(cellvalue, options, rowObject) {
        var a, b;
        if (cellvalue == null) {
            return " "
        }
        else {
            a = cellvalue.substr(0, 1);
            b = cellvalue.substr(1, 11);
            c = parseInt(b);
            return a + c;
        }
    };

    function bom_no(cellvalue, options, rowObject) {
        var a, b;
        if (cellvalue == null) {
            return " "
        }
        else {
            a = cellvalue.substr(0, 1);
            b = cellvalue.substr(1, 11);
            d = cellvalue.substr(11);
            c = parseInt(b);
            return a + c + d;
        }
    };

    $('#closesWO').click(function () {
        $('.dialogWO').dialog('close');
    });

});