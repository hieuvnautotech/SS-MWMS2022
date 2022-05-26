$(function () {
    $(".dialogActual").dialog({
        width: '50%',
        height: 500,
        maxWidth: 1000,
        maxHeight: 500,
        minWidth: '50%',
        minHeight: 500,
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

            $("#popupsActual").jqGrid
            ({
                url: "/WorkOrder/getWoNoPopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                    { key: false, label: 'WO NO', name: 'fo_no', width: 150, align: 'center' },
                    { key: false, label: 'PO NO', name: 'po_no', width: 150, align: 'center' },
                    { key: false, label: 'Bom', name: 'bom_no', width: 150, align: 'center' },
                    { key: false, label: 'Line NO', name: 'process_no', width: 100, align: 'center' },
                    { key: false, label: 'Factory', name: 'lct_cd', width: 150, align: 'left' },
                    { key: false, label: 'Product Date', name: 'product_dt', width: 150, align: 'center' },
                    { key: false, label: 'Product Real Date', name: 'product_real_dt', width: 150, align: 'left' },
                    { key: false, label: 'FO Qty', name: 'fo_qty', width: 150, align: 'right', formatter: 'integer' },
                    { key: false, label: 'Description', name: 're_mark', width: 200, align: 'left' },
                    { key: false, label: 'Create User', name: 'reg_id', width: 100, align: 'center' },
                    { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { key: false, label: 'Chage User', name: 'chg_id', width: 100, align: 'center' },
                    { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupssfono").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupssfono").getRowData(selectedRowId);
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#savestyle_web").removeClass("disabled");
                    var selectedRowId = $("#popupsActual").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsActual").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#savestyle_web").click(function () {
                            $('#s_fo_no').val(row_id.fo_no)
                            $('.dialogActual').dialog('close');
                        });
                    }
                },
                pager: jQuery('#popupspagerActual'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: $(".boxWO").width(),
                autowidth: false,
                caption: '',
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

    $(".ACtualWopopup").click(function () {
        $('.dialogActual').dialog('open');
    });

});
$('#searchBtn_wo_popup').click(function () {
    var wo_no = $('#wo_no_popup').val().toString().trim();
    $.ajax({
        url: "/ActualWO/searchBtn_wo_popup",
        type: "get",
        dataType: "json",
        data: {
            wo_no: wo_no,
        },
        success: function (result) {
            $("#popupsActual").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result.data }).trigger('reloadGrid');
        }
    });
});