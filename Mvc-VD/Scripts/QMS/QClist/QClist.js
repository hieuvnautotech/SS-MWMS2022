
$(function () {
    $("#list1").jqGrid
    ({
        url: "/QMS/getqclist",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'fqno', name: 'fqno', width: 30, align: 'center', hidden: true },
             { label: 'FQ No', name: 'fq_no', width: 120, align: 'center'},
             { label: 'oldhno', name: 'oldhno', width: 120, align: 'center', hidden: true },
             { label: 'WO  ', name: 'fo_no', width: 150, align: 'center' },
             { label: 'PO No ', name: 'po_no', width: 100, align: 'center' },
             { label: ' Line ', name: 'line_no', width: 150, align: 'center' },
             { label: ' Process ', name: 'process_no', width: 150, align: 'center' },
             { label: 'Work Date', name: 'work_dt', width: 150, align: 'center' },
             { label: 'item Code', name: 'item_vcd', width: 110, align: 'center' },
             { label: 'item Name', name: 'item_nm', width: 100, align: 'center' },
             { label: 'item Explain', name: 'item_exp', width: 100, align: 'center' },
             { label: 'Check qty', name: 'check_qty', width: 100, align: 'right' },
             { label: 'Creat User', name: 'reg_id', width: 200, align: 'center' },
             { label: 'Create Date', name: 'reg_dt', width: 200, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
             { label: 'Change User', name: 'chg_id', width: 200, align: 'center' },
             { label: 'Change Date', name: 'chg_dt', width: 200, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
            row_id = $("#list1").getRowData(selectedRowId);

            //var fqno = row_id.fqno;
            var fq_no = row_id.fq_no;

            //$('#m_fq_no').val(fq_no);
            $("#list3").clearGridData();
            $("#list3").setGridParam({ url: "/QMS/getdetail?" + "fq_no=" + fq_no, datatype: "json" }).trigger("reloadGrid");
        },
        pager: "#list3Pager",
        rowNum: 10,
        viewrecords: true,
        rowList: [10, 20, 30, 40, 50, 100],
        height: 250,
        width: $(".box-body").width() -5,
        caption: 'Process QC List ',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        autowidth: false,
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

    })

});

//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list1').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list1").closest(".ui-jqgrid").parent().width();
    $("#list1").jqGrid("setGridWidth", newWidth, false);
});

$(function () {
    $("#list3").jqGrid
    ({
        url: "/QMS/getdetail",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'fqhno', name: 'fqhno', width: 50, hidden: true },
             { label: 'FQ No ', name: 'fq_no', width: 110, align: 'center' },
             { label: 'item', name: 'item_vcd', width: 110, align: 'center' },
             { label: 'Check Name', name: 'check_id', width: 200, align: 'left' },
             { label: 'Check Code', name: 'check_cd', width: 110, align: 'center' },
             { label: 'Check Value', name: 'check_value', width: 110, align: 'right' },
             { label: 'Check qty', name: 'check_qty', width: 100, align: 'right' },
        ],

        onSelectRow: function (rowid, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list3").jqGrid("getGridParam", 'selrow');
            row_id = $("#list3").getRowData(selectedRowId);

        },

        pager: "#list3Pager",
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: 250,
        width: $(".box-body").width(),
        autowidth: false,
        caption: 'QC Details',
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
    })
});

$('#list3').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list3").closest(".ui-jqgrid").parent().width();
    $("#list3").jqGrid("setGridWidth", newWidth, false);
});

$("#searchBtn").click(function () {
    var fo_no = $('#fo_no').val().trim();
    var line_no = $('#line_no').val().trim();
    var process_no = $('#process_no').val().trim();
    var end = $('#end').val().trim();
  


    $.ajax({
        url: "/QMS/searchQClist",
        type: "get",
        dataType: "json",
        data: {

            fo_no: fo_no,
            line_no: line_no,
            process_no: process_no,
            end: end,
            

        },
        success: function (result) {
            $("#list1").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    //"autoclose": true
});


$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#list1").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "RecAppr.pdf",
                mimetype: "application/pdf"
            }
         );
    });
});

$('#htmlBtn').click(function (e) {
    $("#list1").jqGrid('exportToHtml',
        options = {
            title: '',
            onBeforeExport: null,
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            tableClass: 'jqgridprint',
            autoPrint: false,
            topText: '',
            bottomText: '',
            fileName: "QC List Information  ",
            returnAsString: false
        }
    );
});

$('#excelBtn').click(function (e) {
    $("#list1").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "QC List Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
});
