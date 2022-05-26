$(function () {
    var row_id, row_id2;
    $grid = $("#noticeMgtGrid").jqGrid
   ({
       url: "/StandardMgtWO/getnoticeMgt",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'No', name: 'mno', width: 100, align: 'center'},
           { label: 'Subject', name: 'title', width: 120, align: 'left', formatter: setlink },
           { label: 'Writer', name: 'reg_id', width: 120, align: 'center' },
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
           //$('#s_bno').val(bno);
       },

       pager: "#noticeMgtGridPager",
       pager: jQuery('#noticeMgtGridPager'),
       viewrecords: true,
       rowNum: 50,
       rowList: [50, 100, 200, 500],

       width: $(".box-body").width() - 5,
       autowidth: false,
       height: 500,
       caption: 'Notice Management',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
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
        var html = '<a  href="/StandardMgtWO/NoticeView?mno=' + rowdata.mno + '" target="_sub">' + cellvalue + '</a>';
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid


//$("#listBtn").on("click", function (n) {

//});



$("#c_create ").on("click", function () {

    location.href = "/StandardMgtWO/NoticeCreate";

});