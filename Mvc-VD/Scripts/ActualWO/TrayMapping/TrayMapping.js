$(function () {
    $("#list").jqGrid
    ({
        //url: "/ActualWO/getList_tray_info",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'trmno', name: 'trmno', width: 50, hidden: true },
             { label: 'Tray', name: 'tr_no', width: 50, hidden: true },
             { label: 'Product Lot', name: 'prd_lcd', width: 400, align: 'left' },
             { label: 'Product Code', name: 'style_no', width: 110, align: 'left' },
             { label: 'Product Name', name: 'style_nm', width: 180, align: 'left' },

             { label: 'Create User', name: 'reg_id', width: 180, align: 'left' },
             { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
         
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, status, e) {

            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

        },

        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: "300",
        width: null,
        caption: 'WO',
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
    })
    $("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    $("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});
$("#SaveBtn").click(function () {
    
    var tr_no = $('#tr_no').val().trim();
    var prd_lcd = $('#prd_lcd').val().trim();

    $.ajax({
        url: "/ActualWO/Save_Tray_Mapping",
        type: "get",
        dataType: "json",
        data: {
            tr_no: tr_no,
            prd_lcd: prd_lcd,

        },
        success: function (response) {
            if (response.result) {

                var id = response.result.trmno;
                $("#list").jqGrid('addRowData', id, response.result, 'first');
                $("#list").setRowData(id, false, { background: "#d0e9c6" });
                $("#prd_qty").val(response.qty);
            } else {
                if ($("#tr_no").val() == "") {
                    $("#tr_no").val("").focus();
                }
                else {
                    $("#prd_lcd").val("").focus();
                }
            }
        }
    });
   
});
$("#ResetBtn").click(function () {
    $("#tr_no").val("").focus();
    $("#prd_lcd").val("");
    $("#prd_qty").val("");
});

function prd_qty(e) {
    var tr_no = document.getElementById('tr_no').value;
    $(document).ready(function () {
        _getprd_qty();
    });

}

function _getprd_qty() {
    var getprd_qty = "/ActualWO/getprd_qty_tray?" + "tr_no=" + tr_no.value
    $.get(getprd_qty, function (data) {
        if (data) {
            $("#prd_qty").val(data.Qty);
        }
    });
}