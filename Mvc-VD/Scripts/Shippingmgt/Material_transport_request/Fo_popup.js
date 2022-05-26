$(function () {
    $(".dialog3").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 700,
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

            $("#popupfo").jqGrid
            ({
                url: "/Shippingmgt/getfoMgtpopup",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
                   { key: false, label: 'FO NO', name: 'fo_no', width: 120, align: 'center' },
                   { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center' },
                   { key: false, label: 'Factory', name: 'lct_cd', width: 150, align: 'center' },
                   { key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
                   { key: false, label: 'Product real Date', name: 'product_real_dt', width: 150, align: 'left' },
                   { key: false, label: 'FO Qty', name: 'fo_qty', width: 100, align: 'right' },
                   { key: false, label: 'bom_no', name: 'bom_no', width: 100, align: 'center', },
                   { key: false, label: 'style_no', name: 'style_no', width: 100, align: 'center', hidden: true },
                   { key: false, label: 'style_nm', name: 'style_nm', width: 100, align: 'center', hidden: true },
                   { key: false, label: 'model', name: 'md_cd', width: 100, align: 'center', hidden: true },
                   { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
                   { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                   { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                   { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                   { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupfo").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupfo").getRowData(selectedRowId); selected2
                    if (row_id != null) {
                        $("#selected2").click(function () {
                            $('#c_fo_no').val(row_id.fo_no);

                            $('#style_no').val(row_id.style_no);
                            $('#style_nm').val(row_id.style_nm);
                            $('#md_cd').val(row_id.md_cd);
                            $('#c_mt_no').val("");
                            $('#c_pm_no').val("");
                            $('#c_bom_no').val("");
                            $('#bom_fo').val(row_id.bom_no);
                            var grid = $("#ShipRequest");
                            grid.jqGrid('setGridParam', { search: true });
                            var pdata = grid.jqGrid('getGridParam', 'postData');
                            getData_search(pdata);
                            $("#check_kq").setGridParam({ url: "/Shippingmgt/getdataTranport?" + "bom_no=" + row_id.bom_no, datatype: "json" }).trigger("reloadGrid");

                        $('.dialog3').dialog('close');

                        });
                    }
                },

                pager: jQuery('#pagerbom'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 400,
                width: null,
                autowidth: false,
                caption: 'WO',
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

    $(".poupdialogFono").click(function () {
        $('.dialog3').dialog('open');
    });

    $('#closefo').click(function () {
        $('.dialog3').dialog('close');
    });

});

$("#searchBtn_pp_fo_no").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchpp_fo_no",
        type: "get",
        dataType: "json",
        data: {
            fo_no: $("#fo_no").val().trim(),
        },
        success: function (result) {
            $("#popupfo").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});