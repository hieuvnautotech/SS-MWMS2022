
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
       rowList: [20, 50, 200, 500],
       height: 220,
       width: $(".box-body").width() - 5,
       rowNum: 20,
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


//function DateFormatter(cellvalue, options, rowObject) {

//    var date = cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6");  // "2012/12/31 02:33:50";
//    //var year = cellvalue.substring(0, 4),
//    //    month = cellvalue.substring(4, 6),
//    //    day = cellvalue.substring(6, 8),
//    //    hour = cellvalue.substring(8, 10),
//    //    minute = cellvalue.substring(10, 12),
//    //    second = cellvalue.substring(12, 14);
//    //date = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;

//    return date;
//    //var parsedDate = Date.parse(cellvalue, "yyyyMMddHHmmss");

//    //// if parsed date is null, just used the passed cell value; otherwise,
//    //// transform the date to desired format
//    //var formattedDate = parsedDate ? parsedDate.toString("yyyy-MM-dd HH:mm:ss") : cellvalue;

//    //re
//};


function DateFormatter(cellvalue, options, rowObject) {
    return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$4:$5:$6")
};