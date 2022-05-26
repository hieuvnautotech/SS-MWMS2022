$(".poupdialogTray").click(function () {
    $('.diologbobin2').dialog('open');
    get_ds_bb_2();
});



$(function () {
    $(".diologbobin2").dialog({
        width: '50%',
        height: 520,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });
    $('#close_bb').click(function () {
        $('.diologbobin2').dialog('close');
     
    });
});
function get_ds_bb_2() {
    //debugger;
    var pdata = $('#popupbobin2').jqGrid('getGridParam', 'postData');
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    $.ajax({
        url: "/TIMS/searchbobbinPopupDV",
        type: "Get",
        dataType: "json",
        data: {
            bb_no: $('#s_bb_no2').val().trim(),
            bb_nm: $('#s_bb_nm2').val().trim(),
        },
        success: function (result) {
            $("#popupbobin2").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
}
$("#searchBtn_popupbobbin2").click(function () {
    get_ds_bb_2();
});

$("#popupbobin2").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
            { key: false, label: 'Container Code', name: 'bb_no', width: 150, align: 'center', sort: true },
            { key: false, label: 'Container Code', name: 'bb_no', width: 150, align: 'center', sort: true },
            { key: false, label: 'Container Name', name: 'bb_nm', width: 150, align: 'left' },
            { key: false, label: 'Create User', name: 'reg_id', align: 'center' },
            { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#popupbobin2").jqGrid("getGridParam", 'selrow');
            row_id = $("#popupbobin2").getRowData(selectedRowId); 
            if (row_id != null) {
                $('#tray_create').val(row_id.bb_no);
                $('.diologbobin2').dialog('close'); 
            }
        },
        pager: jQuery('#PageBobbin2'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width:null,
        autowidth: false,
        rowNum: 50,
        caption: 'Container List',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        loadonce: true,
        shrinkToFit: false,
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