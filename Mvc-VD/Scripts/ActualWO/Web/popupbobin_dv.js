$(function () {
    $(".diologbobindv").dialog({
        width: '60%',
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
});
function get_ds_bb_dv() {

    $.ajax({
        url: "/ActualWO/searchbobbinPopupDV",
        type: "get",
        dataType: "json",
        data: {
            bb_no: $('#s_bb_nodv').val().trim(),
            bb_nm: $('#s_bb_nmdv').val().trim(),
        },
        success: function (result) {
            $("#popupbobindv").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
}
$("#searchBtn_popupbobbindv").click(function () {
    get_ds_bb_dv();
});
var select_bb = "";
$("#popupbobindv").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
            { key: false, label: 'Container Code', name: 'bb_no', width: 150, align: 'center', sort: true },
            { key: false, label: 'Container Name', name: 'bb_nm', width: 150, align: 'left' },
            { key: false, label: 'Create User', name: 'reg_id', align: 'center' },
            { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            $("#save_bb").removeClass("disabled");
            var selectedRowId = $("#popupbobindv").jqGrid("getGridParam", 'selrow');
            row_id = $("#popupbobindv").getRowData(selectedRowId);
            select_bb = row_id.bb_no;
        },
        pager: jQuery('#PageBobbindv'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: null,
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


$("#selected_dv").click(function () {
    //debugger;
    if (!select_bb) {
        alert('Select 1 bobbin.');
        return;
    }
    else {
        var data_str = $("#id_dv").attr("data-hero");
        var my_object = JSON.parse(decodeURIComponent(data_str));
            my_object.bb_no = select_bb;

        $('.diologbobindv').dialog('close');
        $.ajax({
                url: "/ActualWO/Changebb_dv",
                type: "POST",
                dataType: "json",
                data: {
                     //bb_no: select_bb,
                     item: my_object

                },
                success: function (data) {
                    if (data.result) {
                        var rowData = $('#tb_mt_cd_sta').jqGrid('getRowData', $("#id_dv").val());
                        rowData.bb_no = select_bb;
                        rowData.gr_qty = rowData.gr_qty;
                        rowData.sl_tru_ng = rowData.gr_qty;
                        $('#tb_mt_cd_sta').jqGrid('setRowData', $("#id_dv").val(), rowData);
                        SuccessAlert(data.message); 
                    }
                    else {
                        ErrorAlert(data.message);
                    }

                },
            });
    }
});