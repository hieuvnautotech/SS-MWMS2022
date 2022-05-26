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
            url: "/SalesMgt/getPopupBOM",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'bid', name: 'bid', width: 80, align: 'center', hidden: true },
                { key: false, label: 'BOM', name: 'bom_no', width: 150, align: 'center' },
                { key: false, label: 'Style', name: 'style_no', width: 100, align: 'center' },
                { key: false, label: 'Mode', name: 'md_cd', sortable: true, width: 100, align: 'center', align: 'center' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupsMR").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupsMR").getRowData(selectedRowId);
                if (row_id != null) {
                    $("#savestyleMR").click(function () {
                        $('#mro_no').val(row_id.bom_no);
                        $('.dialogMR').dialog('close');
                    });
                }
            },

            pager: '#popupspagerMR',
            viewrecords: true,
            rowList: [20, 50, 200, 500],
            height: 220,
            width: $(".dialogMR").width() - 30,
            autowidth: false,
            caption: '...........',
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

$(".poupdialogMR").click(function () {
    $('.dialogMR').dialog('open');
});
    
//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#popupsMR').jqGrid('setGridWidth', $(".ui-dialog").width());
$(window).on("resize", function () {
    var newWidth = $("#popupsMR").closest(".ui-jqgrid").parent().width();
    $("#popupsMR").jqGrid("setGridWidth", newWidth, false);
});

$('#closestyleMR').click(function () {
    $('.dialogMR').dialog('close');
});

//..........................................................................

$(".dialogMT").dialog({
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

        $("#popupsMT").jqGrid
        ({
            url: "/SalesMgt/getPopupBOM",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'bid', name: 'bid', width: 80, align: 'center', hidden: true },
                { key: false, label: 'BOM', name: 'bom_no', width: 150, align: 'center' },
                { key: false, label: 'Style', name: 'style_no', width: 100, align: 'center' },
                { key: false, label: 'Mode', name: 'md_cd', sortable: true, width: 100, align: 'center', align: 'center' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupsMT").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupsMT").getRowData(selectedRowId);
                if (row_id != null) {
                    $("#savestyleMT").click(function () {
                        $('#mt_no').val(row_id.bom_no);
                        $('.dialogMT').dialog('close');
                    });
                }
            },

            pager: '#popupspagerMT',
            viewrecords: true,
            rowList: [20, 50, 200, 500],
            height: 220,
            width: $(".dialogMT").width()-30,
            autowidth: false,
            caption: '...........',
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

$(".poupdialogMT").click(function () {
    $('.dialogMT').dialog('open');
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#popupsMT').jqGrid('setGridWidth', $(".ui-dialog").width());
$(window).on("resize", function () {
    var newWidth = $("#popupsMT").closest(".ui-jqgrid").parent().width();
    $("#popupsMT").jqGrid("setGridWidth", newWidth, false);
});

$('#closestyleMT').click(function () {
    $('.dialogMT').dialog('close');
});

