$(function () {
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

            $("#popupBom").jqGrid
            ({
                url: "/ShippingMgt/getBom",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'bid', name: 'bid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'Bom NO', name: 'bom_no', width: 100, align: 'center' },
                    { key: false, label: 'Style No', name: 'style_no', width: 100, align: 'center' },
                    { key: false, label: 'User Y/N', name: 'use_yn', width: 150, align: 'center' },
                    { key: false, label: 'Del Y/N', name: 'del_yn', width: 150, align: 'center' },
                    { key: false, label: 'Create User', name: 'reg_id', width: 150, align: 'center' },
                    { key: false, label: 'Create Date', name: 'reg_dt', width: 150, align: 'center' ,formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200' },
                    { key: false, label: 'Change User', name: 'chg_id', width: 150, align: 'center' },
                    { key: false, label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200'},
                  
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupBom").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupBom").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#bom_no').val(row_id.bom_no);
                        $('.dialogBom').dialog('close');
                    }
                },

                pager: jQuery('#popupspagerBom'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                //width: $(".ui-dialog").width() - 5,
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
    $(".poupdialogBom").click(function () {
        $('.dialogBom').dialog('open');
    });
    $('#closeBom').click(function () {
        $('.dialogBom').dialog('close');
    });

});