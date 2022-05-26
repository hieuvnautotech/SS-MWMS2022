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
                var selectedRowId = $("#popups").jqGrid("getGridParam", 'selrow');
                row_id = $("#popups").getRowData(selectedRowId);
                if (row_id != null) {
                    $("#savestyle").click(function () {
                        $('#c_bom_no').val(row_id.bom_no);
                        $('#m_bom_no').val(row_id.bom_no);
                        $('.dialog').dialog('close');
                    });
                }
            },

            pager: jQuery('#popupspager'),
            viewrecords: true,
            rowList: [20, 50, 200, 500],
            height: 220,
            width: $(".ui-dialog").width() - 50,
            autowidth: false,
            caption: 'BOM Information',
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

$(".poupdialog").click(function () {
    $('.dialog').dialog('open');
});
    
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#popups').jqGrid('setGridWidth', $(".ui-dialog").width());
$(window).on("resize", function () {
    var newWidth = $("#popups").closest(".ui-jqgrid").parent().width();
    $("#popups").jqGrid("setGridWidth", newWidth, false);
});

$('#closestyle').click(function () {
    $('.dialog').dialog('close');
});


// Popup Dangerous
$("#dialogDangerous").dialog({
    width: '20%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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

    },
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});


$("#deletestyle_popup").click(function () {
    $.ajax({
        url: "/SalesMgt/deletePO",
        type: "post",
        dataType: "json",
        data: {
            m_po_no: $('#m_po_no').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#poMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('PO NO was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});



$('#closestyle_popup').click(function () {
    $('#dialogDangerous').dialog('close');
});