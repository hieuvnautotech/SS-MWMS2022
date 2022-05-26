$(function () {
    $(".dialog6").dialog({
        width: '100%',
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

            $("#popupitem_mt").jqGrid
            ({
                url: "/QMS/searchqc_pp",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'ino', name: 'ino', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'QC Code', name: 'item_cd', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '150px', align: 'center' },
                    { key: false, label: 'QC Type', name: 'item_type', width: 100, align: 'center', },
                    { key: false, label: 'Ver', name: 'ver', width: 50, align: 'center' },
                    { key: false, label: 'QC Name', name: 'item_nm', sortable: true, width: '100px', align: 'left' },
                    { key: false, label: 'QC Explain', name: 'item_exp', sortable: true, width: '200' },
                    { key: false, label: 'Use Y/N', name: 'use_yn', editable: true, width: '50px', align: 'center' },
                    { key: false, label: 'Create Use', name: 'reg_id', editable: true, width: '100px' },
                    {
                    label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                    },
                    { key: false, label: 'Change Use', name: 'chg_id', editable: true, width: '100px' },
                    {
                    label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                    },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupitem_mt").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupitem_mt").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#selected_qc").click(function () {
                            $('#m_item_vcd').val(row_id.item_vcd);
                            $('#c_item_vcd').val(row_id.item_vcd);
                            $('#pp_item_vcd').val(row_id.item_vcd);
                            $('.dialog6').dialog('close'); 
                        });
                    }
                },

                pager: jQuery('#page_item_mt'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".boxQC").width(),
                autowidth: false,
                caption: ' QC Information',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                loadonce: true,
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


    $(".poupdialog6item_mt").click(function () {
        $('.dialog6').dialog('open');
    });


    $('#closeitem_mt').click(function () {
        $('.dialog6').dialog('close');
    });

});
$("#searchBtn_popupitem_no").click(function () {
    $.ajax({
        url: "/QMS/searchqc_pp",
        type: "Get",
        dataType: "json",
        data: {
            item_cd: $('#s_item_cd').val().trim(),
            item_nm: $('#s_item_nm').val().trim(),
            item_exp: $('#s_item_exp').val().trim(),
        },
        success: function (result) {
            $("#popupitem_mt").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});