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
                url: "/ActualWO/Getqc_item_pp",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                        { key: true, label: 'ino', name: 'ino', width: 100, align: 'center', hidden: true },
                        { key: false, label: 'QC Code', name: 'item_cd', width: 100, align: 'center', hidden: true },
                        { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '80px', align: 'center'},
                        { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '60px', align: 'center', hidden: true },
                        { key: false, label: 'QC Type', name: 'item_type', width: 60, align: 'center', },
                        { key: false, label: 'Ver', name: 'ver', width: 50, align: 'center', hidden: true },
                        { key: false, label: 'QC Name', name: 'item_nm', sortable: true, width: '100px', align: 'left' },
                        { key: false, label: 'QC Explain', name: 'item_exp', sortable: true, width: '200' },
                        { key: false, label: 'Use', name: 'use_yn', editable: true, width: '50px',align:'center' },
                        { key: false, label: 'Create User', name: 'reg_id', editable: true, width: '100px' },
                        {
                        label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                        },
                        { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px' },
                        {
                        label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                        },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#saveitem_mt").removeClass("disabled");
                    var selectedRowId = $("#popupitem_mt").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupitem_mt").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#closeitem_mt').click(function () {
                            $('.dialog6').dialog('close');
                        });
                        $("#saveitem_mt").click(function () {
                            $('#c_item_vcd').val(row_id.item_vcd);
                            $('#m_item_vcd').val(row_id.item_vcd);
                            $('.dialog6').dialog('close');
                        });                   
                    }
                },

                pager: jQuery('#page_item_mt'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 250,
                width: $(".dialog6").width() - 30,
                autowidth: false,
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                loadonce: true,
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
$("#searchBtn_ITEM_popup").click(function () {
    $.ajax({
        url: "/ActualWO/searchqc_item_pp",
        type: "get",
        dataType: "json",
        data: {
            item_type: $('#PP_item_type').val().trim(),
            item_cd: $('#PP_item_cd').val().trim(),
            item_nm: $('#PP_item_nm').val().trim(),
            item_exp: $('#PP_item_exp').val().trim(),
        },
        success: function (result) {
            $("#popupitem_mt").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
$(document).ready(function () {
    _getitem_type();
});
function _getitem_type() {
    $.get("/QMS/get_item_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $.each(data, function (key, item) {
                if (item.dt_cd == "PC") {
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                }
            });
            $("#PP_item_type").html(html);
        }
    });
}