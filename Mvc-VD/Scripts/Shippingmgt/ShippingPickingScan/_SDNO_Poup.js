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
            //"ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupSD").jqGrid
            ({
                url: "/ShippingMgt/getPopupBOM",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
                    { label: 'SD NO', name: 'sd_no', width: 100, align: 'center' },
                    { label: 'Status', name: 'sd_sts_cd', width: 80, align: 'center' },
                    { label: 'Destination', name: 'lct_cd', sortable: true, width: 150, align: 'left' },
                    { label: 'Worker', name: 'worker_id', width: 100, align: 'left' },
                    { label: 'Manager', name: 'manager_id', width: 100, align: 'left' },
                    { label: 'Work date', name: 'work_dt', sortable: true, width: 100, align: 'center', align: 'center' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupSD").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupSD").getRowData(selectedRowId);
                    $('#selected').click(function () {
                        if (row_id != null) {
                            $('#sd_no').val(row_id.sd_no);
                            $('.dialog').dialog('close');
                        }
                    });
                },

                pager: jQuery('#pagerpopupSD'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: null,
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
        close: function (event, ui) {
            $('.dialog').dialog('close');
        }
    });

    $(".poupdialog").click(function () {
        $('.dialog').dialog('open');
    });



});