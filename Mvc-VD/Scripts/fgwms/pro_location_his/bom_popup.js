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
                { key: false, label: 'Code', name: 'bom_no', width: 150, align: 'center', },
                { key: false, label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
                { key: false, label: 'Product Name', name: 'style_nm', width: 450, align: 'left' },
                { key: false, label: 'Model', name: 'md_cd', sortable: true, width: 450, align: 'left' },
                { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200' },
                { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
                { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px' },
                { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px' },
                { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180' },
                { key: false, label: 'Need Time', name: 'need', editable: true, width: '100px', align: 'right' },
                { key: false, label: 'Description', name: 'cav', editable: true, width: '100px' },
                { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupbom").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupbom").getRowData(selectedRowId);
                    $("#savestyle_BOM").removeClass("disabled");

                    if (row_id != null) {
                        $("#closestyle_BOM").click(function () {
                            $('.dialog2').dialog('close');
                        });
                        $("#savestyle_BOM").click(function () {
                            $('#c_bom_no').val(row_id.bom_no);
                            $('#m_bom_no').val(row_id.bom_no);
                            $('.dialog2').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerbom'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: $(".boxBOM").width(),
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

    $(".poupdialogbom").click(function () {
        $('.dialog2').dialog('open');
    });
    $('#searchBtn_BOM_popup').click(function () {
        var bom_no = $('#s_bom_no_popup').val().trim();
        var md_cd = $('#s_model_no_popup').val().trim();
        var style_no = $('#s_style_no_popup').val().trim();
        $.ajax({
            url: "/DevManagement/searchBom",
            type: "get",
            dataType: "json",
            data: {
                bom_no: bom_no,
                md_cd: md_cd,
                style_no: style_no,
            },
            success: function (result) {
                $("#popupbom").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
            }
        });
    });

    $('#closebom').click(function () {
        $('.dialog2').dialog('close');
    });

});