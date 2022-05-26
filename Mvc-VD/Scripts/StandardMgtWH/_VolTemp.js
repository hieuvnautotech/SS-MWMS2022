
$(function () {
    $grid = $("#VolTempGrid").jqGrid
   ({
       url: "/StandardMgtWh/getVolTemp",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'ehid', name: 'ehid', index: 'ehid', width: 10, hidden: true },
           { label: 'E NO', name: 'e_no', index: 'mt_cd', width: 100, align: 'center' },  
           { label: 'Date Time', name: 'eh_dt', width: 150, align: 'center', formatter: DateFormatter },         
           { label: 'Voltage', name: 'vol_val', width: 100, align: 'right' },
           { label: 'Temperature', name: 'temp_val', width: 100, align: 'right' },
           { label: 'Motion', name: 'motion_val', width: 100, align: 'right' },
           { label: 'Smoke', name: 'smoke_val', width: 100, align: 'right' },
       ],
       pager: "#VolTempGridPager",
       pager: jQuery('#VolTempGridPager'),
       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: 220,
       width: $(".box-body").width() - 5,
       rowNum: 50,
       caption: 'Voltage & Temperature',
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
function DateFormatter(cellvalue, options, rowObject) {
    return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$4:$5:$6")
};

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#VolTempGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#VolTempGrid").jqGrid("setGridWidth", newWidth, false);
});