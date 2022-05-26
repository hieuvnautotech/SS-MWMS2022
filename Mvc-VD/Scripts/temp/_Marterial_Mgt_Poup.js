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
                url: "/DevManagement/GetPopupStyle",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'code', name: 'bom_no', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'Style', name: 'style_no', width: 100, align: 'center' },
                    { key: false, label: 'Mode code', name: 'md_cd', sortable: true, width: '100px', align: 'center' },
                    { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200' },
                    { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
                    { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px' },
                    { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
                    { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px' },
                    { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180' },
                     { key: false, label: 'Remark', name: 'cav', editable: true, width: '100px' },
                   {
                       label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                       formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
                   },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popups").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#savestyle").click(function () {
                            $('#c_style_no').val(row_id.style_no);
                            $('#m_style_no').val(row_id.style_no);
                            $('.dialog').dialog('close');
                        });
                    }
                },

                pager: jQuery('#m_popupspager'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".ui-dialog").width() - 50,
                autowidth: false,
                caption: 'Information',
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

});