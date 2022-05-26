$(function () {
    $(".dialog_PM").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 700,
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

            $("#popuppm").jqGrid
            ({
                url: "/Shippingmgt/getpm_popup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                       { key: true, label: 'id', name: 'smid', width: 50, align: 'right', hidden: true },
                        { label: 'PM NO', name: 'pm_no', width: 120, align: 'left' },
                        { label: 'Product Cnt', name: 'prod_cnt', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'Order Date', name: 'order_dt', width: 120, align: 'center', hidden: true },
                        //{ label: 'Order Date', name: 'order_dt1', width: 120, align: 'center', formatter: excel_test },
                        { label: 'Delivery Qty', name: 'delivery_qty', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'Actual Qty', name: 'actual_qty', width: 80, align: 'right', formatter: 'integer' },
                        { label: 'Remain Qty', name: 'pm_rm_qty', width: 80, align: 'right', formatter: 'integer' },
                        { label: 'WR Qty', name: 'fo_qty', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'WR Remain Qty', name: 'fo_rm_qty', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'SO Qty', name: 'so_qty', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'SO Remain Qty', name: 'so_rm_qty', width: 120, align: 'right', formatter: 'integer' },
                        { label: 'Description', name: 're_mark', width: 300, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popuppm").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popuppm").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected_pm").click(function () {
                            $('#c_fo_no').val("");
                            $('#c_mt_no').val("");
                            $('#c_pm_no').val(row_id.pm_no);
                            $('#bom_fo').val("");
                            $('#c_bom_no').val("");
                            var grid = $("#ShipRequest");
                            grid.jqGrid('setGridParam', { search: true });
                            var pdata = grid.jqGrid('getGridParam', 'postData');
                            getData_search(pdata);
                            $('.dialog_PM').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerpm'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 500,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'PM',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                loadonce: true,
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

    $(".poupdialogpm").click(function () {
        $('.dialog_PM').dialog('open');
    });

    $("#searchBtn_pm_no").click(function () {
        var pm_no = $('#p_pm_no').val().trim();
        var po_no = $('#p_po_no').val().trim();
        $.ajax({
            url: "/Shippingmgt/searchPoMgt_popup",
            type: "get",
            dataType: "json",
            data: {
                pm_no: pm_no,
                po_no: po_no,
            },
            success: function (result) {
                console.log(result.rows);
                $("#popuppm").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result.rows }).trigger("reloadGrid");
            }
        });

    });

    $('#closepm').click(function () {
        $('.dialog_PM').dialog('close');
    });

});
