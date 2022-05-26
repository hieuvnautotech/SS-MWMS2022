$(function () {
    $(".dialog6").dialog({
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

            $("#popupitem_mt").jqGrid
            ({
                url: "/QMS/Getqc_item",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'ino', name: 'ino', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'Item Code', name: 'item_cd', width: 100, align: 'center', },
                    { key: false, label: 'Item Type', name: 'item_type', width: 100, align: 'center', },
                    { key: false, label: 'Ver', name: 'ver', width: 100, align: 'center' },
                    { key: false, label: 'Item Name', name: 'item_nm', sortable: true, width: '100px', align: 'center' },
                    { key: false, label: 'Item v Code', name: 'item_vcd', sortable: true, width: '200' },
                    { key: false, label: 'Item Explain', name: 'item_exp', sortable: true, width: '200' },
                    { key: false, label: 'Use Y/N', name: 'use_yn', editable: true, width: '100px' },
                    { key: false, label: 'Create Use', name: 'reg_id', editable: true, width: '100px' },
                    {
                    label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                    },
                    { key: false, label: 'Change Use', name: 'chg_id', editable: true, width: '100px' },
                    {
                    label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                    },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupitem_mt").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupitem_mt").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#m_item_vcd').val(row_id.item_vcd);
                        $('.dialog6').dialog('close');
                    }
                },

                pager: jQuery('#page_item_mt'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".dialog6").width() - 30,
                autowidth: false,
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


    $(".poupdialog6item_mt").click(function () {
        $('.dialog6').dialog('open');
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popupitem_mt').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popupitem_mt").closest(".ui-jqgrid").parent().width();
    //    $("#popupitem_mt").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closeitem_mt').click(function () {
        $('.dialog6').dialog('close');
    });

});
