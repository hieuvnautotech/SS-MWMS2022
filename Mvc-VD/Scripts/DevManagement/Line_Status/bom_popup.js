$(function () {
    $(".dialog2").dialog({
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

            $("#popupbom").jqGrid
            ({
                url: '/DevManagement/GetBomMgt',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'code', name: 'bom_no', width: 100, align: 'center', },
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
                    var selectedRowId = $("#popupbom").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupbom").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_bom_no').val(row_id.bom_no);
                        $('#m_bom_no').val(row_id.bom_no); 
                        $('#rel_bom').val(row_id.bom_no);
                        $('.dialog2').dialog('close');
                    }
                },

                pager: jQuery('#pagerbom'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'BOM',
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

    $(".poupdialogbom").click(function () {
        $('.dialog2').dialog('open');
    });
    
    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popupbom').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popupbom").closest(".ui-jqgrid").parent().width();
    //    $("#popupbom").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closebom').click(function () {
        $('.dialog2').dialog('close');
    });

});