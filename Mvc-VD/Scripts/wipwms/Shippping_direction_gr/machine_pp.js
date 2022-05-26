
$(function () {
    $(".dialog_machine").dialog({
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

            $("#popupmachine").jqGrid
            ({
                url: "/Plan/GetPopupmachine",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                  { key: true, label: 'mmo', name: "mno", width: 80, align: 'center', hidden: true },
                   { key: false, label: 'Type', name: 'mc_type', width: 110, align: 'center' },
                   { key: false, label: 'Code', name: 'mc_no', width: 150, align: 'left', sort: true },
                   { key: false, label: 'Name', name: 'mc_nm', width: 150, align: 'left' },
                   { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                   { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'left', hidden: true },
                          { key: false, label: 'Use', name: 'su_dung', width: 150, align: 'left' },
                   { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
                   { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                   { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                   { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                   { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#savemachine").removeClass("disabled");
                    var selectedRowId = $("#popupmachine").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupmachine").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#closemachine').click(function () {
                            $('.dialog_machine').dialog('close');
                        });
                        $("#savemachine").click(function () {
                            $('#c_mc_no').val(row_id.mc_no);
                            $('#c_mc_id').val(row_id.mno);
                            $('#m_mc_no').val(row_id.mc_no);
                            $('.dialog_machine').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagermachine'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 50, 200, 500],
                sortable: true,
                loadonce: true,
                height: 220,
                width: $(".dialog_machine").width() - 30,
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
    $(".poupdialogmachine").click(function () {
        $('.dialog_machine').dialog('open');
    });

    $('#closemachine').click(function () {
        $('.dialog_machine').dialog('close');
    });

});

$(document).ready(function () {
    _getmc_type_pp();
});
function _getmc_type_pp() {
    $.get("/Plan/get_mc_type", function (data) {
        if (data != null && data != undefined && data.length) {
            //console.log(data);
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mc_type_pp").html(html);
        }
    });
}
$("#searchBtn_machine").click(function () {
    var mc_type = $('#mc_type_pp').val();
    var mc_no = $('#mc_no_pp').val().trim();
    var mc_nm = $('#mc_nm_pp').val().trim();

    $.ajax({
        url: "/Plan/searchpopupmachine",
        type: "get",
        dataType: "json",
        data: {
            mc_type: mc_type,
            mc_no: mc_no,
            mc_nm: mc_nm,
        },
        success: function (result) {
            $("#popupmachine").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
