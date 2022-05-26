$("#scheduleGrid").jqGrid
  ({
      url: "/PurchaseMgt/getSchedule",
      datatype: 'json',
      mtype: 'Get',
      colModel: [
           { key: true, label: 'pmid', name: 'pmid', width: 50, hidden: true },
           { label: 'Mpo no', name: 'mpo_no', width: 110, align: 'center' },
           { label: 'Po no', name: 'po_no', width: 110, align: 'center' },
           { label: 'Bom no', name: 'bom_no', width: 110, align: 'center' },
           { label: 'User YN', name: 'use_yn', width: 110, align: 'center' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
      ],

      onSelectRow: function (rowid, status, e) {
          $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
          var selectedRowId = $("#scheduleGrid").jqGrid("getGridParam", 'selrow');
          row_id = $("#scheduleGrid").getRowData(selectedRowId);
          if (row_id != "") {
              $("#dc_save_but").attr('disabled', true);
              document.getElementById("form3").reset();
          }

          var mpo_no = row_id.mpo_no;
          $('#dc_mpo_no').val(mpo_no);
          $("#scheduledtGrid").setGridParam({ url: "/PurchaseMgt/getScheduleDetail?" + "mpo_no=" + mpo_no, datatype: "json" }).trigger("reloadGrid");
      },

      pager: "#scheduleGridPager",
      viewrecords: true,
      rowList: [50, 100, 200],
      height: 250,
      width: $(".box-body").width() - 5,
      autowidth: false,
      caption: 'Purchase Schedule',
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

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#scheduleGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#scheduleGrid").closest(".ui-jqgrid").parent().width();
    $("#scheduleGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#searchBtn").click(function () {
    var po_no = $('#po_no').val().trim();
    var mpo_no = $('#mpo_no').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    $.ajax({
        url: "/PurchaseMgt/searchSchedule",
        type: "get",
        dataType: "json",
        data: {
            po_no: po_no,
            mpo_no: mpo_no,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#scheduleGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

//----------Detail Purchase--------//
$("#scheduledtGrid").jqGrid
({
    url: "/PurchaseMgt/getScheduleDetail",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
            { key: true, label: 'pdid', name: 'pdid', width: 50, hidden: true },
            { label: 'Mpo no', name: 'mpo_no', width: 110, align: 'center' },
            { label: 'Mdpo no', name: 'mdpo_no', width: 110, align: 'center' },
            { label: 'Mt no', name: 'mt_no', width: 110, align: 'center' },
            { label: 'Expected input date', name: 'exp_input_dt', width: 110, align: 'center' },
            { label: 'Need qty', name: 'need_qty', width: 110, align: 'center' },
            { label: 'Feed size', name: 'feed_size', width: 110, align: 'center' },
            { label: 'Feed unit', name: 'feed_unit', width: 110, align: 'center' },
            { label: 'Purchase qty ', name: 'act_pur_qty', width: 110, align: 'center' },
            { label: 'Last unit price', name: 'last_unit_amt', width: 110, align: 'center' },
            { label: 'Total last price', name: 'tot_last_amt', width: 110, align: 'center' },
            { label: 'VAT (%)', name: 'vat_cd', width: 80, align: 'center' },
            { label: 'VAT Amount', name: 'vat_amt', width: 80, align: 'center' },
            { label: 'Payment Amount', name: 'pay_amt', width: 110, align: 'center' },
            { label: 'Actual Payment Amount', name: 'act_pay_amt', width: 150, align: 'center' },
    ],

    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#scheduledtGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#scheduledtGrid").getRowData(selectedRowId);
        var pdid = row_id.pdid;
        var mdpo_no = row_id.mdpo_no;
        var mt_no = row_id.mt_no;
        var exp_input_dt = row_id.exp_input_dt;
        $("#dc_save_but").attr('disabled', false);
        $("#exp_input_dt").removeAttr("readonly");
        $('#c_pdid').val(pdid);
        $('#dc_mdpo_no').val(mdpo_no);
        $('#dc_mt_no').val(mt_no);
        $('#exp_input_dt').val(exp_input_dt);
    },

    pager: "#scheduledtGridPager",
    rowList: [50, 100, 200, 500, 1000],
    width: $(".box-body").width() - 5,
    caption: 'Purchase Schedule Detail',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 200,
    loadonce: true,
    gridview: true,
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

$('#scheduledtGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#scheduledtGrid").closest(".ui-jqgrid").parent().width();
    $("#scheduledtGrid").jqGrid("setGridWidth", newWidth, false);
});

$("#dc_save_but").click(function () {

    var pdid = $('#c_pdid').val();
    var exp_input_dt = $('#exp_input_dt').val();

    $.ajax({
        url: "/PurchaseMgt/updateScheduleDetail",
        type: "get",
        dataType: "json",
        data: {
            pdid: pdid,
            exp_input_dt: exp_input_dt
        },
        success: function (data) {
            jQuery("#scheduledtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        }
    });

});

$("#start").datepicker({
    format: 'yyyy-mm-dd',
});


$("#end").datepicker({
    format: 'yyyy-mm-dd',
});

$("#exp_input_dt").datepicker({
    format: 'yyyy-mm-dd',
});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'PurchaseSchedule',
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
            fileName: 'PurchaseSchedule',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'PurchaseSchedule',
            escape: false,
        });
    });
});