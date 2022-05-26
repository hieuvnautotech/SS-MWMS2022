$(".dialogpo").dialog({
    width: '50%',
    height: 600,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '50%',
    minHeight: 600,
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
        $("#list_primary").jqGrid
            ({
                datatype: function (postData) { getData_primary(postData); },

                mtype: 'Get',
                colModel: [
                    { key: true, label: 'id_actualpr', name: 'id_actualpr', width: 80, align: 'center', hidden: true },
                    { label: '', name: 'process_count', hidden: true },
                    { label: '', name: 'DTQ', hidden: true },
                    { label: 'PO NO', name: 'at_no', width: 120 },
                    { label: 'Product', name: 'product', width: 150 },
                    { label: 'Description', name: 'remark', width: 170 },
                    { label: 'Target', name: 'target', width: 100, align: 'right', formatter: 'integer' },
                    //{ label: 'Processing', name: '', width: 150, align: 'center', formatter: ProcessCalculating },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $("#list_primary").removeClass("disabled");
                    var selectedRowId = $("#list_primary").jqGrid("getGridParam", 'selrow');
                    row_id = $("#list_primary").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#Selected_po").click(function () {
                            $('#at_no').val(row_id.at_no);
                            $('#box_no').val("");
                            $('#bb_no').val("");
                            $('#mt_lot').val("");
                            $('#buyer_qr').val("");
                            $('.dialogpo').dialog('close');
                        });
                    }
                },
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                gridComplete: function () {

                    $('.loading').hide();
                },
                pager: "#jqGrid_primaryPager",
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 350,
                width: null,
                autowidth: false,
                rowNum: 500,
                caption: '',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                loadonce: false,
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
function PercentagePrimary(cellValue, options, rowdata, action) {
    if (rowdata.target == 0 || rowdata.totalTarget == undefined || rowdata.actual == undefined) {
        return 0.00 + ` %`;
    }
    return ((rowdata.actual / rowdata.totalTarget) * 100).toFixed(2) + ` %`;
}

function ProcessCalculating(cellValue, options, rowdata, action) {
    var po = rowdata.at_no;
    var html = ``;
    $.ajax({
        url: `/ActualWO/ProcessCalculating?po=${po}`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            html += `${response} / ${rowdata.process_count}`;
        },
        error: function () {
        }
    });
    return html;
}

$('#sDate').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
function getData_primary(pdata) {
    var params = new Object();

    if ($('#list_primary').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.product = $("#sProductCode").val().trim();
    params.at_no = $("#at_nos").val().trim();
    params.reg_dt = $("#sDate").val().trim();
    $('#list_primary').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/fgwms/Getdataw_popupactual`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list_primary')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$(".pp_at_no").click(function () {
    $('.dialogpo').dialog('open');
    $("#list_primary").clearGridData();
    $("#list_primary").jqGrid('setGridParam', { search: true });
    var pdata = $("#list_primary").jqGrid('getGridParam', 'postData');
    getData_primary(pdata)
});
$("#searchBtn_po").click(function () {
    $("#list_primary").clearGridData();
    $("#list_primary").jqGrid('setGridParam', { search: true });
    var pdata = $("#list_primary").jqGrid('getGridParam', 'postData');
    getData_primary(pdata)
});
