$(function () {
    $(".so").dialog({
        width: '50%',
        height: 800,
        maxWidth: '100%',
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 1000,
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

            $("#popupso").jqGrid
            ({
                url: "/Shippingmgt/Getpopupso",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { label: 'sno', name: 'sno', key: true, width: 50, align: 'center', hidden: true },
                   { label: 'SO', name: 'ship_no', key: true, width: 200, align: 'left' },
                   { label: 'Product Code', name: 'style_no', sortable: true, width: 200, align: 'left' },
                   { label: 'Po No', name: 'po_no', sortable: true, width: 100, align: 'right' },
                   { label: 'Qty', name: 'so_qty', sortable: true, width: 100, align: 'right' },
                   { label: 'Remark', name: 're_mark', width: 100, align: 'center' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupso").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupso").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#selected_so").click(function () {
                            $('#so').val(row_id.ship_no);
                        $('.so').dialog('close');
                        });

                    }
                },

                pager: jQuery('#pageso'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                rowNum:50,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'SO',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                multiselect: false,
                loadonce: true,
                autowidth:true,
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
    $(".poupdialogso").click(function () {
        $('.so').dialog('open');
    });

    $("#searchBtn_pp_so").click(function () {
        var code = $("#pp_so").val().trim();
        $.ajax({
            url: "/Shippingmgt/searchpp_so",
            type: "get",
            dataType: "json",
            data: {
                code: code,
            },
            success: function (result) {
                $("#popupso").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });

    $('#closeso').click(function () {
        $('.so').dialog('close');
    });

});