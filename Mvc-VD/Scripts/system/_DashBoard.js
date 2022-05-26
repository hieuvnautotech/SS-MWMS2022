
$(function () {
    var row_id, row_id2;
    $grid = $("#dashBoardGrid").jqGrid
   ({
       url: "/system/getFactory",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           //{ key: true, label: 'id', name: 'mt_id', index: 'mt_id', width: 10, hidden: true },
           { label: 'Factory', name: 'fo_no', width: 150, align: 'center' },
           { label: 'Total', name: 'prod_qty', width: 150, align: 'right' },
           { label: 'Actual', name: 'done_qty', width: 150, align: 'right' },
           { label: 'Defective', name: 'refer_qty', width: 150, align: 'right' },
           { label: 'Efficiency', name: 'eff_qty', width: 150, align: 'right', formatter: numberFormatter },
           //{ name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntCellValue },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       pager: "#dashBoardGridPager",
       pager: jQuery('#dashBoardGridPager'),
       viewrecords: true,
       rowList: [50, 200, 500, 1000],
       height: 260,
       width: 800,
       rowNum: 50,
       caption: 'Factory',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       multiselect: false,
       autowidth: false,
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


});


function numberFormatter(cellvalue, options, rowObject) {

    if (cellvalue.toString().includes(".")) {
        kq = cellvalue.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");

    } else {
        kq = cellvalue.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");

    }
    return kq + " %"
};


$(function () {
    var row_id, row_id2;
    $grid = $("#noticeGrid").jqGrid
   ({
       url: "/system/getnoticeMgt",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'No', name: 'mno', width: 50, align: 'center' },
           { label: 'Subject', name: 'title', width: 450, align: 'left', formatter: setlink },
           { label: 'Writer', name: 'reg_id', width: 100, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },

       onSelectRow: function (rowid, status, e) {
           var selectedRowId = $("#noticeMgtGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#noticeMgtGrid").getRowData(selectedRowId);
           var bno = row_id.mno;

       },

       pager: "#noticeGridPager",
       pager: jQuery('#noticeGridPager'),
       viewrecords: true,
       rowNum: 50,
       rowList: [50, 100, 200, 500],
       width: 800,
       autowidth: false,
       height: 540,
       caption: 'Notice Management',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
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
    $('#noticeMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    //$('#foSttGrid').jqGrid('setGridHeight', $(window).innerHeight() - 400);
    $(window).on("resize", function () {
        var newWidth = $("#noticeMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#noticeMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });
});



function setlink(cellvalue, options, rowdata, action) {

    if (cellvalue != null) {
        var html = '<a  href="/StandardMgt/NoticeView?mno=' + rowdata.mno + '" target="_sub">' + cellvalue + '</a>';
        return html;
    } else {
        return cellvalue;
    }
}



$("#c_create ").on("click", function () {

    location.href = "/system/NoticeCreate";

});

$(document).ready(function () {
    $.ajax({
        type: 'post',
        url: '/system/getActualEfficiency',
        dataType: 'text',
        success: function (data) {
            var html = '';
            html += data;
            html += ' %';
            $("#m_actual_efficiency").html(html);
        }
    });
});



$(document).ready(function () {
    $.ajax({
        type: 'post',
        url: '/system/getActualAmount',
        dataType: 'text',
        success: function (data) {
            var html = '';
            html += data;
            $("#m_actual_amount").html(html);
        }
    });
});





$("#c_save_but").click(function () {

    var message = $('#c_message').val();


    $.ajax({
        url: "/system/insertmessage",
        type: "get",
        dataType: "json",
        data: {
            message: message,
        },
        success: function (data) {
            jQuery("#woMgtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            document.getElementById("form1").reset();
            $("#dc_save_but").attr("disabled", true);
        },
        error: function (data) {
            alert('The Code is the same. Please check again.');
        }
    });

});