$(function () {
    $(".dialogMR").dialog({
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

            $("#popupsMR").jqGrid
            ({
                url: "/Shippingmgt/getPopupMRO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'mrid', name: 'mrid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'MSR NO', name: 'mr_no', width: 100, align: 'center' },
                    { key: false, label: 'Status', name: 'mr_sts_cd', width: 100, align: 'center' },
                    { key: false, label: 'Location', name: 'lct_cd', width: 100, align: 'center' },
                    { key: false, label: 'Work ID', name: 'worker_id', width: 100, align: 'center' },
                    { key: false, label: 'Manager ID', name: 'manager_id', width: 100, align: 'center' },
                    { key: false, label: 'Reg Receive Date', name: 'req_rec_dt', width: 100, align: 'center' },
                    { key: false, label: 'Real Receving Date', name: 'real_rec_dt', width: 100, align: 'center' },
                    { key: false, label: 'Relation bom', name: 'rel_bom', width: 100, align: 'center' },
                    { key: false, label: 'Description', name: 'remark', width: 100, align: 'center' },
                    { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popupsMR").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsMR").getRowData(selectedRowId);
                    if (row_id != null) {

                    }
                    $("#savestyleMR").click(function () {
                        $('#mr_no').val(row_id.mr_no);
                        $('.dialogMR').dialog('close');
                    });
                },

                pager: jQuery('#popupspagerMR'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,

                autowidth: true,
                caption: 'MSR NO',
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
        $('.dialogMR').dialog('open');
    });
    $('#closeMR').click(function () {
        $('.dialogMR').dialog('close');
    });

});