$(function () {
    $(".dialog").dialog({
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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popups").jqGrid
            ({
                url: "/ShippingMgt/getPopupBOM",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
                    { label: 'SD NO', name: 'sd_no', width: 100, align: 'center' },
                    { label: 'sd_sts_cd', name: 'sd_sts_cd', width: 80, align: 'center' },
                    { label: 'lct_cd', name: 'lct_cd', sortable: true, width: 150, align: 'left' },
                    { label: 'worker_id', name: 'worker_id', width: 100, align: 'left' },
                    { label: 'manager_id', name: 'manager_id', width: 100, align: 'left' },
                    { label: 'work_dt', name: 'work_dt', sortable: true, width: 100, align: 'center', align: 'center' },
                    { label: 'real_work_dt', name: 'real_work_dt', width: 150, align: 'center' },
                    { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                    { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                    { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popups").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_sd_no').val(row_id.sd_no);
                        $('.dialog').dialog('close');
                    }
                },

                pager: jQuery('#popupspager'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".ui-dialog").width() - 50,
                autowidth: false,
                caption: 'SD INFO',
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

    $(".poupdialog").click(function () {
        $('.dialog').dialog('open');
    });

    $('#closestyle').click(function () {
        $('.dialog').dialog('close');
    });

});