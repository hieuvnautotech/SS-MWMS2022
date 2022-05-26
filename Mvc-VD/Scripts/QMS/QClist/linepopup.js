$(function () {
    $(".dialogLine").dialog({
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

            $("#popupsLine").jqGrid
            ({
                url: "/QMS/getPopupLine",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'fqno', name: 'fqno   ', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'FO NO', name: 'line_no', width: 100, align: 'center' },
                     { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsLine").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsLine").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#line_no').val(row_id.line_no);
                        $('.dialogLine').dialog('close');
                    }
                },

                pager: '#popupspagerLine',
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".dialogLine").width() -30,
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

    $(".poupdialogLine").click(function () {
        $('.dialogLine').dialog('open');
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popups').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popups").closest(".ui-jqgrid").parent().width();
    //    $("#popups").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closestyleLine').click(function () {
        $('.dialogLine').dialog('close');
    });

});