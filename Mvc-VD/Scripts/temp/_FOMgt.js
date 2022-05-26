$(function () {
    var row_id, row_id2;
    $("#foMgtGrid").jqGrid
   ({
       url: "/SalesMgt/getfoMgtData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
           { key: false, label: 'FO NO', name: 'fo_no', width: 100, align: 'center' },
           { key: false, label: 'PO NO', name: 'po_no', width: 150, align: 'center' },
           { key: false, label: 'Factory', name: 'lct_cd', width: 300, align: 'center' },
           { key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
           { key: false, label: 'Product Real Date', name: 'product_real_dt', width: 300, align: 'center' },
           { key: false, label: 'FO Qty', name: 'fo_qty', width: 100, align: 'right' },
           { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
       },
       pager: jQuery('#foMgtGridPager'),
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 400,
       width: $(".box-body").width() - 5,
       autowidth: false,
       caption: 'FO Management',
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
    $('#foMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#foMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#foMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $("#searchBtn").click(function () {

        var fo_no = $('#s_fo_no').val().trim();
        var po_no = $('#s_po_no').val().trim();
        var start = $('#start').val();
        var end = $('#end').val();
        $.ajax({
            url: "/SalesMgt/searchfoMgtData",
            type: "get",
            dataType: "json",
            data: {
                fo_no: fo_no,
                po_no: po_no,
                start: start,
                end: end
            },
            success: function (result) {
                $("#foMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
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
            fileName: 'FOMgt_Infor',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'FOMgt_Infor',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'FOMgt_Infor',
            escape: false,
            headers: true,
        });
    });
});