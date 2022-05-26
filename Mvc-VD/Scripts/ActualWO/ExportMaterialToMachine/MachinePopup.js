
$(function () {
    $(".poupdialogMachine").dialog({
        width: '50%',
        height: 500,
        maxWidth: 1000,
        maxHeight: 500,
        minWidth: '50%',
        minHeight: 500,
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
                    datatype: function (postData) { getData_machine(postData); },
                    datatype: 'json',
                    mtype: 'Get',
                    colModel: [
                        { key: true, label: 'mmo', name: "mno", width: 80, align: 'center', hidden: true },
                        { key: false, label: 'Type', name: 'mc_type', width: 90, align: 'center' },
                        { key: false, label: 'Code', name: 'mc_no', width: 100, align: 'center', sort: true },
                        { key: false, label: 'Name', name: 'mc_nm', width: 150, align: 'left' },
                        { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                        { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'left', hidden: true },
                        { key: false, label: 'Use', name: 'su_dung', width: 150, align: 'left' },
                        /*  { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },*/
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
                            $("#savemachine").click(function () {
                                $('#CreateMachineCode').val(row_id.mc_no);
                                $('#ModifyMachineCode').val(row_id.mc_no);
                                $('.poupdialogMachine').dialog('close');
                            });
                        }
                    },

                    pager: jQuery('#pagermachine'),
                    viewrecords: true,
                    rowList: [50, 100, 200, 500, 1000],
                    height: 300,
                    width: null,
                    autowidth: false,
                    rowNum: 50,
                    caption: '',
                    loadtext: "Loading...",
                    emptyrecords: "No data.",
                    rownumbers: true,
                    gridview: true,
                    loadonce: true,
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
    $(".ClickppMachine").click(function () {
        $('.poupdialogMachine').dialog('open');
        $('#popupmachine').clearGridData();
        $('#popupmachine').jqGrid('setGridParam', { search: true });
        var pdata = $('#popupmachine').jqGrid('getGridParam', 'postData');
        getData_machine(pdata);
    });


});

$(document).ready(function () {
    _getmc_type_pp();
});
function _getmc_type_pp() {
    $.get("/ActualWO/get_mc_type", function (data) {
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

function getData_machine(pdata) {
    var params = new Object();

    if ($('#popupmachine').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var mc_type = $('#mc_type_pp').val();
    var mc_no = $('#mc_no_pp').val().trim();
    var mc_nm = $('#mc_nm_pp').val().trim();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.mc_type = mc_type;
    params.mc_no = mc_no;
    params.mc_nm = mc_nm;
    //params.processCode = $('#sMaterialCode').val();

    $('#popupmachine').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/ActualWO/searchpopupmachine",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#popupmachine')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#searchBtn_machine").click(function () {
    $('#popupmachine').clearGridData();
    $('#popupmachine').jqGrid('setGridParam', { search: true });
    var pdata = $('#popupmachine').jqGrid('getGridParam', 'postData');
    getData_machine(pdata);
});
