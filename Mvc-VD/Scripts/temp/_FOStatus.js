$(function () {
    var row_id, row_id2;
    $grid = $("#foSttGrid").jqGrid
   ({
       url: "/ProducePlan/getfoSttData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'fno', name: 'fno', index: 'fno', width: 10, hidden: true },
           { key: false, label: 'FO NO', name: 'fo_no', width: 120, align: 'center' },
           { key: false, label: 'PO NO', name: 'po_no', width: 120, align: 'center' },
           { key: false, label: 'Factory', name: 'lct_cd', width: 150, align: 'center' },
           { key: false, label: 'Product Date', name: 'product_dt', width: 100, align: 'center' },
           { key: false, label: 'Product real Date', name: 'product_real_dt', width: 150, align: 'left' },
           { key: false, label: 'FO Qty', name: 'fo_qty', width: 100, align: 'right' },
           { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
       },

       pager: "#foSttGridPager",
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 220,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 20,
       caption: 'FO Status',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       multiselect: false,
       loadonce: true,
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
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#foSttGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#foSttGrid").closest(".ui-jqgrid").parent().width();
        $("#foSttGrid").jqGrid("setGridWidth", newWidth, false);
    });
});
$("#searchBtn").click(function () {

    var fo_no = $('#s_fo_no').val()
    var po_no = $('#s_po_no').val().trim();
    var start = $('#start').val()
    var end = $('#end').val()
    $.ajax({
        url: "/ProducePlan/searchfoSttData",
        type: "get",
        dataType: "json",
        data: {
            fo_no: fo_no,
            po_no: po_no,
            start: start,
            end: end
        },
        success: function (result) {
            $("#foSttGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

$("#start").datepicker({
    format: 'yyyy-mm-dd',
});
$("#end").datepicker({
    format: 'yyyy-mm-dd',
});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'FOStatus',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            type: 'xls',
            fileName: 'FOStatus',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'FOStatus',
            escape: false,
        });
    });
});


