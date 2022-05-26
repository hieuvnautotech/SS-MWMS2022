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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupsWO").jqGrid
            ({
                url: "/QMS/getPopupwO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'oflno', name: 'oflno   ', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'FO NO', name: 'fo_no', width: 100, align: 'center' },
                     { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
                  
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsWO").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsWO").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#fo_no').val(row_id.fo_no);
                        $('.dialogWO').dialog('close');
                    }
                },

                pager: '#popupspagerWO',
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".dialogWO").width() -30,
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

    $(".poupdialogWO").click(function () {
        $('.dialogWO').dialog('open');
    });

    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popups').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popups").closest(".ui-jqgrid").parent().width();
    //    $("#popups").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closesWO').click(function () {
        $('.dialogWO').dialog('close');
    });

});