$(".dialogbuyer").dialog({
    width: '80%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 800,
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
        $("#GeneralGrid").jqGrid
            ({
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                    { label: 'Product', name: 'product', width: 100, align: 'center' },
                    { label: 'Product Name', name: 'style_nm', width: 150, },
                    { label: 'Model', name: 'md_cd', width: 150, },
                    { label: 'Container Before', name: 'bb_no', width: 200, },
                    { label: 'Location', name: 'lct_nm', width: 100, },
                    { label: 'Lot No', name: 'end_production_dt', width: 80, align: 'center' },
                    { label: 'Buyer', name: 'buyer_qr', width: 300, align: 'left' },
                    { label: 'PO', name: 'po', width: 100, align: 'center' },
                    { label: 'Composite Code', name: 'mt_cd', width: 300, align: 'left' },
                    { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right' },
                    { label: 'Status', name: 'sts_nm', width: 80, align: 'center' },
                    { label: 'Recevied Date', name: 'recevice_dt', width: 100, align: 'center', formatter: _Date },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $("#GeneralGrid").removeClass("disabled");
                    var selectedRowId = $("#GeneralGrid").jqGrid("getGridParam", 'selrow');
                    row_id = $("#GeneralGrid").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#Selected").click(function () {
                            $('#buyer_qr').val(row_id.buyer_qr);
                            $('#box_no').val("");
                            $('#bb_no').val("");
                            $('#mt_lot').val("");
                            $('#at_no').val("");
                            $('.dialogbuyer').dialog('close');
                        });
                    }
                },
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                gridComplete: function () {
                    $("tr.jqgrow:odd").css("background", "white");
                    $("tr.jqgrow:even").css("background", "#f7f7f7");
                    $('.loading').hide();
                },
                pager: "#GeneralGridPager",
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 400,
                width: null,
                rowNum: 50,
                caption: '',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                loadonce: false,
                datatype: function (postData) { getDataOutBox(postData); },
                subGrid: false,
                shrinkToFit: false,
                multiboxonly: true,
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

$('#recevice_dt_start').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#recevice_dt_end').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
function getDataOutBox(pdata) {
    var params = new Object();
    var rows = $("#GeneralGrid").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.productCode = $('#productCode').val() == null ? "" : $('#productCode').val().trim();
    params.poCode = $('#poCode').val() == null ? "" : $('#poCode').val().trim();
    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/fgwms/getbuyer_popup",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#GeneralGrid")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$(".pp_buyer").click(function () {
    $('.dialogbuyer').dialog('open');
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata)
});
$("#searchBtn_buyer").click(function () {
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
});

//////////////////////////////////////////////////////////////////////////////////////////////////
function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";

    cellvalue = cellvalue.substr(0, 8);
    var reg = /(\d{4})(\d{2})(\d{2})/;
    if (reg.test(cellvalue))
        return cellvalue.replace(reg, "$1-$2-$3");
    else {
        reg = /(\d{4})(\d{2})\-(\d{2})/;
        if (reg.test(cellvalue))
            return cellvalue.replace(reg, "$1-$2-$3");
        else {
            reg = /(\d{4})\-(\d{2})(\d{2})/;
            if (reg.test(cellvalue))
                return cellvalue.replace(reg, "$1-$2-$3");
            else {
                reg = /(\d{4})\-(\d{2})\-(\d{2})/;
                if (reg.test(cellvalue))
                    return cellvalue.replace(reg, "$1-$2-$3");
                else {
                    reg = /(\d{4})(\d{2}).(\d{2})/;
                    if (reg.test(cellvalue))
                        return cellvalue.replace(reg, "$1-$2-$3");
                    else {
                        reg = /(\d{4}).(\d{2}).(\d{2})/;
                        if (reg.test(cellvalue))
                            return cellvalue.replace(reg, "$1-$2-$3");

                        else {
                            reg = /(\d{4})(\d{2})\\(\d{2})/;
                            if (reg.test(cellvalue))
                                return cellvalue.replace(reg, "$1-$2-$3");
                            else {
                                reg = /(\d{4})\\(\d{2})\\(\d{2})/;
                                if (reg.test(cellvalue))
                                    return cellvalue.replace(reg, "$1-$2-$3");
                                else {
                                    reg = /(\d{4})\\(\d{2})\.(\d{2})/;
                                    if (reg.test(cellvalue))
                                        return cellvalue.replace(reg, "$1-$2-$3");
                                    else {
                                        reg = /(\d{4})\.(\d{2})\\(\d{2})/;
                                        if (reg.test(cellvalue))
                                            return cellvalue.replace(reg, "$1-$2-$3");
                                        else
                                            return cellvalue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}