$(function () {
    $(".dialog3").dialog({
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

            $("#popupsSD").jqGrid
            ({
                url: "/Shippingmgt/getPopupSD",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'sid', name: 'sid   ', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'SD No', name: 'sd_no', width: 100, align: 'center' },
                    { key: false, label: 'Status', name: 'sd_sts_cd', width: 100, },
                    { key: false, label: 'Location Code', name: 'lct_cd', width: 100, align: 'center' },
                    { key: false, label: 'Worker ID', name: 'worker_id', width: 100, align: 'center' },
                    { key: false, label: 'Manager ID', name: 'manager_id', width: 100, align: 'center' },
                    { key: false, label: 'Woker Date', name: 'work_dt', width: 100, align: 'center' },
                    { key: false, label: 'real_work_dt', name: 'real_work_dt', width: 100, align: 'center' },
                    { key: false, label: 'mt_qty', name: 'mt_qty', width: 100, align: 'center' },
                    { key: false, label: 'mr_no', name: 'mr_no', width: 100, align: 'center' },
                    { key: false, label: 'use_yn', name: 'use_yn', width: 100, align: 'center' },
                    { key: false, label: 'del_yn', name: 'del_yn', width: 100, align: 'center' },
                    { key: false, label: 'reg_id', name: 'reg_id', width: 100, align: 'center' },
                    { key: false, label: 'chg_id', name: 'chg_id', width: 100, align: 'center' },
                    { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsSD").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsSD").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#sd_no').val(row_id.sd_no);
                        $('.dialog3').dialog('close');
                    }
                },

                pager: jQuery('#popupspagerSD'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".dialog3").width() - 30,
                autowidth: false,
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
        $('.dialog3').dialog('open');
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popups').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popups").closest(".ui-jqgrid").parent().width();
    //    $("#popups").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closeSD').click(function () {
        $('.dialog3').dialog('close');
    });

});