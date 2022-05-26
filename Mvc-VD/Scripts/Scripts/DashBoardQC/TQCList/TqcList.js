var day_now;
var day1;
var day2;
var day3;
var day3;
var day5;
var day6;

var p_stock_day1;
var p_stock_day2;
var p_stock_day3;
var p_stock_day4;
var p_stock_day5;
var p_stock_day6;

day_now = getdate_now();
day1 = adddate(day_now, -1);
day2 = adddate(day_now, -2);
day3 = adddate(day_now, -3);
day4 = adddate(day_now, -4);
day5 = adddate(day_now, -5);
day6 = adddate(day_now, -6);

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
$(function () {
    $("#list").jqGrid
        ({
            url: "/QCInformation/GetQMSNG",
            datatype: 'json',
            mtype: 'Get',
            colModel: model.Data.data,
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                row_id = $("#list").getRowData(selectedRowId);
            },

            pager: "#list_gridpager",
            pager: jQuery('#list_gridpager'),
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 300,
            autowidth: true,
            width: null,
            rowNum: 50,
            caption: 'TQC List',
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
            //loadComplete: function () {
            //    var rows = $("#list_delv").getDataIDs();
            //    for (var i = 0; i < rows.length; i++) {
            //        var stock_day1 = $("#list_delv").getCell(rows[i], "stock_day1");
            //        var stock_day2 = $("#list_delv").getCell(rows[i], "stock_day2");
            //        var stock_day3 = $("#list_delv").getCell(rows[i], "stock_day3");
            //        var stock_day4 = $("#list_delv").getCell(rows[i], "stock_day4");
            //        var stock_day5 = $("#list_delv").getCell(rows[i], "stock_day5");
            //        if (parseInt(stock_day1) < 0) {
            //            $("#list_delv").jqGrid('setCell', rows[i], "stock_day1", "", {
            //                'background-color': 'orange ',
            //            });
            //        }
            //        if (parseInt(stock_day2) < 0) {
            //            $("#list_delv").jqGrid('setCell', rows[i], "stock_day2", "", {
            //                'background-color': 'orange ',
            //            });
            //        }
            //        if (parseInt(stock_day3) < 0) {
            //            $("#list_delv").jqGrid('setCell', rows[i], "stock_day3", "", {
            //                'background-color': 'orange ',
            //            });
            //        }
            //        if (parseInt(stock_day4) < 0) {
            //            $("#list_delv").jqGrid('setCell', rows[i], "stock_day4", "", {
            //                'background-color': 'orange ',
            //            });
            //        }
            //        if (parseInt(stock_day5) < 0) {
            //            $("#list_delv").jqGrid('setCell', rows[i], "stock_day5", "", {
            //                'background-color': 'orange ',
            //            });
            //        }
            //    }
            //},
        });
    $("#list").jqGrid('destroyGroupHeader', true);
    $("#list").jqGrid('setGroupHeaders', {
        useColSpanStyle: true,
        groupHeaders: model.Data.group
    });

});
function formatter_total(cellvalue, options, rowobject) {
    var plan = "";
    if (rowobject.totalDefect == "Tổng NG") {
         plan =
            parseInt(rowobject.day_7_n) + parseInt(rowobject.day_7_d)
            + parseInt(rowobject.day_6_n) + parseInt(rowobject.day_6_d)
            + parseInt(rowobject.day_5_n) + parseInt(rowobject.day_5_d)
            + parseInt(rowobject.day_4_n) + parseInt(rowobject.day_4_d)
            + parseInt(rowobject.day_3_n) + parseInt(rowobject.day_3_d)
            + parseInt(rowobject.day_2_n) + parseInt(rowobject.day_2_d)
            + parseInt(rowobject.day_1_n) + parseInt(rowobject.day_1_d);

        return plan;
    }
    return plan;
};
$('#date_ymd').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
//-------SEARCH------------//
$("#searchBtn").click(function () {
    var productCode = $('#productCode').val().trim();
    var date_ymd = $('#date_ymd').val().trim();
    $.ajax({
        url: "/QCInformation/GetQMSNG?productCode=" + productCode + "&date_ymd=" + date_ymd,
        type: "get",
        dataType: "json",
        success: function (result) {
            var total = [];
            total.push(result.total);
            total.push(result.ok);
            total.push(result.ng);
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result.total }).trigger("reloadGrid");
            $("#list")[0].addJSONData(total);
        }
    });
});
