$(".dialogBom").dialog({
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

        $("#popupsbom").jqGrid
        ({
            url: "/Dashchart/getPopupbom",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'oflno', name: 'oflno   ', width: 80, align: 'center', hidden: true },
                { key: false, label: 'BOM No', name: 'bom_no', width: 100, align: 'center' },
                { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
                  
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#popupsbom").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupsbom").getRowData(selectedRowId);
                if (row_id != null) {
                    $('#bom_no').val(row_id.bom_no);
                    $('.dialogBom').dialog('close');
                }
            },

            pager: jQuery('#popupspagerbom'),
            viewrecords: true,
            rowList: [20, 50, 200, 500],
            height: 220,
            width: $(".dialogBom").width() - 30,
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

$(".poupdialogbom").click(function () {
    $('.dialogBom').dialog('open');
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#popupsbom').jqGrid('setGridWidth', $(".ui-dialog").width());
$(window).on("resize", function () {
    var newWidth = $("#popupsbom").closest(".ui-jqgrid").parent().width();
    $("#popupsbom").jqGrid("setGridWidth", newWidth, false);
});

$('#closestyle4').click(function () {
    $('.dialogBom').dialog('close');
});

