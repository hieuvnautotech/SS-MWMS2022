//$(function () {
//    $("#list").jqGrid
//    ({
//        url: "/TIMS/GetWorkRequest",
//        datatype: 'json',
//        mtype: 'Get',
//        colModel: [
//            { key: true, label: 'sno', name: 'sno', width: 80, align: 'center', hidden: true },
//            { label: 'SO NO', name: 'ship_no', width: '100', align: 'center' },
//            { label: 'PO NO', name: 'po_no', width: '100', align: 'center' },
//            { label: 'BOM NO', name: 'bom_no', width: '100', align: 'center' },
//            { label: 'SO Qty', name: 'so_qty', width: '100', align: 'right', formatter: 'integer' },
//            { label: 'Delivery Date', name: 'delivery_dt', width: '100', align: 'center' },
//            { label: 'Delivery Real Date', name: 'delivery_real_dt', width: '120', align: 'center' },
//            { label: 'Checked', name: 'total_009', width: '100', align: 'right', formatter: 'integer' },
//            { label: 'UnCheck', name: 'total_008', width: '100', align: 'right', formatter: 'integer' },
//        ],
//        formatter: {
//            integer: { thousandsSeparator: ",", defaultValue: '0' },
//            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
//            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
//        },
//        pager: "#listPage",

//        viewrecords: true,
//        rowList: [50, 100, 200, 500, 1000],
//        height: 600,
//        width: null,
//        autowidth: false,
//        rowNum: 50,
//        caption: 'Work Request',
//        loadtext: "Loading...",
//        emptyrecords: "No data.",
//        rownumbers: true,
//        gridview: true,
//        loadonce: true,
//        shrinkToFit: false,
//        jsonReader:
//        {
//            root: "rows",
//            page: "page",
//            total: "total",
//            records: "records",
//            repeatitems: false,
//            Id: "0"
//        },

//        gridComplete: function () {
//            var rows = $("#list").getDataIDs();
//            for (var i = 0; i < rows.length; i++) {
//                var so_qty = $("#list").getCell(rows[i], "so_qty");
//                var total_009 = $("#list").getCell(rows[i], "total_009");
//                if (parseInt(so_qty) > parseInt(total_009)) {
//                    $("#list").jqGrid('setCell', rows[i], "ship_no", "", {
//                        'background-color': 'orange ',
//                    });
//                }
//                else {
//                    $("#list").jqGrid('setCell', rows[i], "ship_no", "", {
//                        background: '',
//                    });
//                }
//            }
//        },
//    })




//});//grid1





//$("#input_dt").datepicker({
//    dateFormat: 'yy-mm-dd',
//    "autoclose": true
//});
//$("#searchBtn").click(function () {
//    $.ajax({
//        url: "/TIMS/GetWorkRequest",
//        type: "get",
//        dataType: "json",
//        data: {
//            po_no: $("#po_no").val().trim(),
//            input_dt: $("#input_dt").val(),

//        },
//        success: function (result) {
//            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });

//});
//function DateFormatter(cellvalue, options, rowObject) {
//    if (cellvalue != null) {
//        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
//    }
//    else { return "" }
//};





$(function () {
    $("#list_delv").jqGrid
    ({
        url: "/SalesMgt/search_delivery?start=" + getdate_now(),
        datatype: 'json',
        mtype: 'Get',
        colModel: [
        { key: true, label: 'sid', name: 'sid', width: 50, align: 'right', hidden: true },
           { label: 'Model', name: 'md_cd', width: 300, align: 'left' },
           { label: 'Code', name: 'style_no', width: 150, align: 'left' },
           { label: 'Name', name: 'style_nm', width: 200, align: 'left' },
           { label: 'Warehouse Stock', name: 'pro_stock', width: 80, align: 'right', exportcol: true },
           { label: 'Product Stock ', name: 'pro_wait', width: 80, align: 'right' },
            { label: 'DV Plan ', name: 'day_1', width: 60, align: 'right' },
             { label: 'Shortage ', name: 'stock_day1', width: 60, align: 'right', formatter: formatter1 },
            { label: 'DV Plan ', name: 'day_2', width: 60, align: 'right' },
             { label: 'Shortage ', name: 'stock_day2', width: 60, align: 'right', formatter: formatter2 },
            { label: 'DV Plan ', name: 'day_3', width: 60, align: 'right' },
             { label: 'Shortage ', name: 'stock_day3', width: 60, align: 'right', formatter: formatter3 },
            { label: 'DV Plan ', name: 'day_4', width: 60, align: 'right' },
             { label: 'Shortage ', name: 'stock_day4', width: 60, align: 'right', formatter: formatter4 },
            { label: 'DV Plan ', name: 'day_5', width: 60, align: 'right' },
             { label: 'Shortage ', name: 'stock_day5', width: 60, align: 'right', formatter: formatter5 },
              { label: 'Total ', name: 'total_plan', width: 60, align: 'right', formatter: formatter_total },
               { label: 'Wait Check ', name: 'pro_wait_check', width: 80, align: 'right' },
           //{ label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
           //{ label: 'Location Status', name: 'lct_sts_cd', width: 80, align: 'left' },
           //{ label: 'Departure', name: 'from_lct_cd', width: 80, align: 'left' },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list_delv").jqGrid("getGridParam", 'selrow');
            row_id = $("#list_delv").getRowData(selectedRowId);
            p_style_no_delv = row_id.style_no;
            $("#list_delv_detail").setGridParam({ url: "/TIMS/search_delivery_detail?style_no=" + row_id.style_no, datatype: "json" }).trigger("reloadGrid");

        },
        loadComplete: function () {
            var rows = $("#list_delv").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var stock_day1 = $("#list_delv").getCell(rows[i], "stock_day1");
                var stock_day2 = $("#list_delv").getCell(rows[i], "stock_day2");
                var stock_day3 = $("#list_delv").getCell(rows[i], "stock_day3");
                var stock_day4 = $("#list_delv").getCell(rows[i], "stock_day4");
                var stock_day5 = $("#list_delv").getCell(rows[i], "stock_day5");
                if (parseInt(stock_day1) < 0) {
                    $("#list_delv").jqGrid('setCell', rows[i], "stock_day1", "", {
                        'background-color': 'orange ',
                    });
                }
                if (parseInt(stock_day2) < 0) {
                    $("#list_delv").jqGrid('setCell', rows[i], "stock_day2", "", {
                        'background-color': 'orange ',
                    });
                }
                if (parseInt(stock_day3) < 0) {
                    $("#list_delv").jqGrid('setCell', rows[i], "stock_day3", "", {
                        'background-color': 'orange ',
                    });
                }
                if (parseInt(stock_day4) < 0) {
                    $("#list_delv").jqGrid('setCell', rows[i], "stock_day4", "", {
                        'background-color': 'orange ',
                    });
                }
                if (parseInt(stock_day5) < 0) {
                    $("#list_delv").jqGrid('setCell', rows[i], "stock_day5", "", {
                        'background-color': 'orange ',
                    });
                }
               }
           },

        pager: "#list_delivery_gridpager",
        pager: jQuery('#list_delivery_gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 800],
        height: 300,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: 'DELIVERY PLAN',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
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
    $("#list_delv").jqGrid('destroyGroupHeader', true);
    $("#list_delv").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: [
          //{ "startColumnName": 'day_1', "numberOfColumns": 2, "titleText":  "dsffsfsadf" },
          //{ "startColumnName": 'day_2', "numberOfColumns": 2, "titleText":  day1  },
          //{ "startColumnName": 'day_3', "numberOfColumns": 2, "titleText":  day2  },
          //{ "startColumnName": 'day_4', "numberOfColumns": 2, "titleText":  day3 },
          //{ "startColumnName": 'day_5', "numberOfColumns": 2, "titleText":  day4 },
          { "startColumnName": 'day_1', "numberOfColumns": 2, "titleText": '<div align="center" >' + day_now + '</div>' },
          { "startColumnName": 'day_2', "numberOfColumns": 2, "titleText": '<div align="center" >' + day1 + '</div>' },
          { "startColumnName": 'day_3', "numberOfColumns": 2, "titleText": '<div align="center" >' + day2 + '</div>' },
          { "startColumnName": 'day_4', "numberOfColumns": 2, "titleText": '<div align="center" >' + day3 + '</div>' },
          { "startColumnName": 'day_5', "numberOfColumns": 2, "titleText": '<div align="center" >' + day4 + '</div>' },

        ]
    });

});


$(function () {
    $("#list_delv_detail").jqGrid
    ({
       // url: "/TIMS/search_delivery_detail?style_no=" + style_no,
        datatype: 'json',
        mtype: 'Get',
        colModel: [
        { key: true, label: 'soid', name: 'soid', width: 50, align: 'right', hidden: true },
           { label: 'Model', name: 'md_cd', width: 250, align: 'left' },
           { label: 'Code', name: 'style_no', width: 250, align: 'left' },
           { label: 'Name', name: 'style_nm', width: 200, align: 'left' },
           { label: 'WM', name: 'fm_no', width: 120, align: 'left', exportcol: true },
           { label: 'WR No', name: 'fo_no', width: 120, align: 'left', exportcol: true },
           { label: 'PO No ', name: 'po_no', width: 120, align: 'left' },
           { label: 'Start Date', name: 'start_dt', width: 100, align: 'left', exportcol: true },
           { label: 'End Date', name: 'end_dt', width: 100, align: 'left' },
           { label: 'Routing', name: 'line_no', width: 100, align: 'left' },

          //  { label: '', name: 'total_qty', width: 80, align: 'right' },
             //{ label: 'Shortage ', name: 'stock_day1', width: 60, align: 'right', formatter: formatter1 },  
        ],

        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list_delv_detail").jqGrid("getGridParam", 'selrow');
            row_id = $("#list_delv_detail").getRowData(selectedRowId);
                                    
        },

        pager: "#list_delivery_detail_gridpager",
        pager: jQuery('#list_delivery_detail_gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 250,
        caption: 'DELIVERY PLAN DETAIL',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
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

});


var day_now;
var day1;
var day2;
var day3;
var day4;

var p_stock_day1;
var p_stock_day2;
var p_stock_day3;
var p_stock_day4;

day_now = getdate_now();
day1 = adddate(day_now, 1);
day2 = adddate(day_now, 2);
day3 = adddate(day_now, 3);
day4 = adddate(day_now, 4);



function formatter1(cellvalue, options, rowobject) {
    var stock_w = rowobject.pro_stock;
    var stock_pw = rowobject.pro_wait;
    var plan = parseInt(rowobject.day_1);
    var stock = parseInt(stock_w) + parseInt(stock_pw);
    p_stock_day1 = stock - plan;
    return (stock - plan);
};



function formatter2(cellvalue, options, rowobject) {
    var stock_w = p_stock_day1;
    var plan = parseInt(rowobject.day_2);
    var stock = parseInt(stock_w);
    p_stock_day2 = stock - plan;
    return (stock - plan);
};

function formatter3(cellvalue, options, rowobject) {
    var stock_w = p_stock_day2;;
    var plan = parseInt(rowobject.day_3);
    var stock = parseInt(stock_w);
    p_stock_day3 = stock - plan;
    return (stock - plan);
};
function formatter4(cellvalue, options, rowobject) {
    var stock_w = p_stock_day3;
    //   var stock_pw = rowobject.pro_wait;
    var plan = parseInt(rowobject.day_4);
    var stock = parseInt(stock_w);
    p_stock_day4 = stock - plan;
    return (stock - plan);
};
function formatter5(cellvalue, options, rowobject) {
    var stock_w = p_stock_day4;
    //   var stock_pw = rowobject.pro_wait;
    var plan = parseInt(rowobject.day_5);
    var stock = parseInt(stock_w);
    return (stock - plan);
};
function formatter_total(cellvalue, options, rowobject) {

    var plan = parseInt(rowobject.day_1) + parseInt(rowobject.day_2) + parseInt(rowobject.day_3) + parseInt(rowobject.day_4) + parseInt(rowobject.day_5);
    // var stock = parseInt(stock_w);
    return plan;
};


$(document).ready(function (e) {
    $('#excelBtn_deli').click(function (e) {
        $("#list_delv").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "SMMgt_Delivery_Plan.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 500,
                 onBeforeExport: null,
                 replaceStr: function _replStrFunc(v) {
                     return v.replace(/<div align="center" >/g, '')
                         .replace(/[</div>]/g, '');
                 }
             }
         );
    });
});

var p_style_no_delv;

$("#searchBtn_delivery_detail").click(function () {


    var start = $("#start_dt").val();
    var end = $('#end_dt').val();
   
    $.ajax({
        url: "/TIMS/search_delivery_detail",
        type: "get",
        dataType: "json",
        data: {
            style_no: p_style_no_delv,
            st_date: start,
            end_date: end,
        },
        success: function (result) {
          
            $("#list_delv_detail").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            //$("#list_delv").clearGridData();
        }
    });

});

$("#searchBtn_delivery").click(function () {

    //var isValid = $('#form3').valid();
    //if (isValid == false) {
    //    return false;
    //}

    if ($('#form3').valid() == false) return false;
    var start = $("#start_delivery").val();
    //  var newdate = new Date(start);

    day_now = start;
    if (day_now == "" || day_now == undefined || day_now == null)
        day_now = getdate_now();
    if (start == "" || start == undefined || start == null)
        start = getdate_now();
    day1 = adddate(start, 1);
    day2 = adddate(start, 2);
    day3 = adddate(start, 3);
    day4 = adddate(start, 4);
    var md_cd = $('#s_model_deli').val();
    var style_no = $('#s_code_deli').val();

    $.ajax({
        url: "/SalesMgt/search_delivery",
        type: "get",
        dataType: "json",
        data: {
            md_cd: md_cd,
            start: start,
            style_no: style_no
        },
        success: function (result) {
            $("#list_delv").jqGrid('destroyGroupHeader', true);
            $("#list_delv").jqGrid('setGroupHeaders', {
                useColSpanStyle: true,
                groupHeaders: [
                  //{ "startColumnName": 'day_1', "numberOfColumns": 2, "titleText":  "dsffsfsadf" },
                  //{ "startColumnName": 'day_2', "numberOfColumns": 2, "titleText":  day1  },
                  //{ "startColumnName": 'day_3', "numberOfColumns": 2, "titleText":  day2  },
                  //{ "startColumnName": 'day_4', "numberOfColumns": 2, "titleText":  day3 },
                  //{ "startColumnName": 'day_5', "numberOfColumns": 2, "titleText":  day4 },
                  { "startColumnName": 'day_1', "numberOfColumns": 2, "titleText": '<div align="center" >' + start + '</div>' },
                  { "startColumnName": 'day_2', "numberOfColumns": 2, "titleText": '<div align="center" >' + day1 + '</div>' },
                  { "startColumnName": 'day_3', "numberOfColumns": 2, "titleText": '<div align="center" >' + day2 + '</div>' },
                  { "startColumnName": 'day_4', "numberOfColumns": 2, "titleText": '<div align="center" >' + day3 + '</div>' },
                  { "startColumnName": 'day_5', "numberOfColumns": 2, "titleText": '<div align="center" >' + day4 + '</div>' },

                ]
            });
            $("#list_delv").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            //$("#list_delv").clearGridData();
        }
    });

});

$("#start_delivery").datepicker
({
    dateFormat: 'yy-mm-dd',
    //"autoclose": true
});
$("#start_dt").datepicker
({
    dateFormat: 'yy-mm-dd',
    //"autoclose": true
});
$("#end_dt").datepicker
({
    dateFormat: 'yy-mm-dd',
    //"autoclose": true
});
function adddate(i_date, i_days) {
    // var tt = document.getElementById('txtDate').value;

    var date = new Date(i_date);
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + i_days);

    var dd = newdate.getDate();
    if (dd.toString().length == 1) dd = "0" + dd;
    var mm = newdate.getMonth() + 1;
    if (mm.toString().length == 1) mm = "0" + mm;
    var y = newdate.getFullYear();
    //'yy-mm-dd',
    //var someFormattedDate = mm + '/' + dd + '/' + y;
    var someFormattedDate = y + '-' + mm + '-' + dd;
    return someFormattedDate;
}

function getdate_now() {
    var newdate = new Date();
    var dd = newdate.getDate();
    if (dd.toString().length == 1) dd = "0" + dd;
    var mm = newdate.getMonth() + 1;
    if (mm.toString().length == 1) mm = "0" + mm;
    var y = newdate.getFullYear();
    //'yy-mm-dd',
    //var someFormattedDate = mm + '/' + dd + '/' + y;
    var someFormattedDate = y + '-' + mm + '-' + dd;
    return someFormattedDate;
}
