$(function () {
    var row_id, row_id2;
    
    $("#soMgtGrid").jqGrid
   ({
       url: "/SalesMgt/getsoMgtData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { label: 'sno', name: 'sno', index: 'sno', width: 10, hidden: true },
           { label: 'SO NO', name: 'ship_no', width: 100, align: 'center' },
           { label: 'PO NO', name: 'po_no', width: 100, align: 'center' },
           { label: 'Destination', name: 'dest_cd', width: 150, align: 'center' },
           { label: 'Delivery Date', name: 'delivery_dt', width: 300, align: 'center' },
           { label: 'Delivery real Date', name: 'delivery_real_dt', width: 100, align: 'center' },
           { label: 'Delivery Qty', name: 'so_qty', width: 100, align: 'right' },
           { label: 'Delivery real Date', name: 'delivery_real_dt', width: 100, align: 'center' },
           { label: 'Create User', name: 'reg_id', width: 100, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Chage User', name: 'chg_id', width: 100, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
       },
       pager: jQuery('#soMgtGridPager'),
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 400,
       width: $(".box-body").width() - 5,
       autowidth: false,
       caption: 'SO Management',
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

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#soMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#soMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#soMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $("#searchBtn").click(function () {

        var ship_no = $('#s_ship_no').val()
        var po_no = $('#s_po_no').val().trim();
        var start = $('#start').val();
        var end = $('#end').val();
        $.ajax({
            url: "/SalesMgt/searchsoMgtData",
            type: "get",
            dataType: "json",
            data: {
                ship_no: ship_no,
                po_no: po_no,
                start: start,
                end: end
            },
            success: function (result) {
                $("#soMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });
    $("#start").datepicker({
        format: 'mm/dd/yyyy',
    });
    $("#end").datepicker({
        format: 'mm/dd/yyyy',
    });

    //$('#end').datepicker().datepicker('setDate', 'today');
    //// Init Datepicker end
    //$("#end").datepicker({
    //    format: 'mm/dd/yyyy',
    //    "autoclose": true
    //});
});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'SOMgt_Infor',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'SOMgt_Infor',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'SOMgt_Infor',
            escape: false,
            headers: true,
        });
    });
});