
$(function () {
    var row_id, row_id2;
    $grid = $("#dashBoardGrid").jqGrid
   ({
       url: "/DashBoard/getFactory",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           //{ key: true, label: 'id', name: 'mt_id', index: 'mt_id', width: 10, hidden: true },
           { label: 'WO', name: 'fo_no', width: 150, align: 'center' },
           { label: 'Taget', name: 'prod_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Actual', name: 'done_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Defective', name: 'refer_qty', width: 150, align: 'right', formatter: 'integer' },
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
       rowList: [50, 100, 200, 500, 1000],
       rowNum: 50,
       height: 260,
       autowidth: true,
       //width: $(".box-body").width() - 5,
       caption: 'WO Management',
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


});

$(document).ready(function () {

    $('#dataTable').DataTable({
        "bServerSide": true,
        "ajax": {
            url: "/DashBoard/getFactory",
            "type": "GET",
            "datatype": "json"
        },
        "searching": false,
        "paging": false,
        "bInfo": false,



        "columns": [

            { "data": "fo_no" },
            { "data": "prod_qty" },
            { "data": "done_qty" },
            { "data": "refer_qty" },
            { "data": "eff_qty" },

        ],
        'columnDefs': [{
            "targets": 0, // your case first column
            "className": "text-center",

        }, {
            "targets": 1, // your case first column
            "className": "text-right",

        }, {
            "targets": 2, // your case first column
            "className": "text-right",

        }, {
            "targets": 3, // your case first column
            "className": "text-right",

        }, {
            "targets": 4, // your case first column
            "className": "text-right",

        }
        ],
        "bDestroy": true,


    });

});


function numberFormatter(cellvalue, options, rowObject) {

    console.log(rowObject);

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
       url: "/DashBoard/getnoticeMgt",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'No', name: 'mno', width: 50, align: 'center' },
           { label: 'Subject', name: 'title', width: 350, align: 'left', formatter: PopupLink },
           { label: 'Writer', name: 'reg_id', width: 80, align: 'left' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 120, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
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
       viewrecords: false,
       rowList: [50, 100, 200, 500, 1000],
       rowNum: 50,
       width: $(".boxnoticeGrid").width() - 10,
       //autowidth: true,
       height: 255,
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

    $.jgrid.defaults.width = '100%';
    $.jgrid.defaults.autowidth = true;
    $.jgrid.defaults.autoheight = true;
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';


    //$(box-body).on('resize', function () {
    //    $("#noticeGridPager").setGridWidth($(box-body).width());
    //}).trigger('resize');

});



//function setlink(cellvalue, options, rowdata, action) {

//    if (cellvalue != null) {
//        var html = '<a  href="/StandardMgt/NoticeView?mno=' + rowdata.mno + '" target="_sub">' + cellvalue + '</a>';
//        return html;
//    } else {
//        return cellvalue;
//    }
//}



function PopupLink(cellvalue, options, rowobject) {

    var id = rowobject.mno;
    console.log(id);
    if (cellvalue != null) {
        var html = '<a  data-toggle="modal"  style="color: dodgerblue; text-align: left;"  data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="OpenPopup(' + id + ');" class="poupgbom">' + cellvalue + '</a>';
        return html;
    }
    return cellvalue;

};

function OpenPopup(id) {
    $('.dialog').dialog('open');
    $(document).ready(function () {
        _getinfo(id);
    });
}
var getinfo = "/DashBoard/getNotice";
function _getinfo(id) {
    $.get(getinfo + "?id=" + id, function (data) {

        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                console.log(item.reg_dt);
                console.log(item.reg_id);
                var html = '';
                html += '<div class="to"><h4 class="title_c"> Writer </h4><div class="title_name">' + item.reg_id + " " + item.reg_dt + '</div></div>'
                html += '<div class="to"><h4 class="title_c"> Subject </h4><div class="title_name">' + item.title + '</div></div>'
                html += '<div class="to"><h4 class="title_c"> Content </h4><div class="title_name">' + item.content + '</div></div>'
                //html += '<tr><th class="col-md-1 border border-secondary"  scope="col" >Subject</th><td class="col-md-11 border border-secondary" scope="col" >' + item.title + '</td></tr>';
                //html += '<tr><th class="col-md-1 border border-secondary" scope="col" >Content</th><td class="col-md-11 border border-secondary" scope="col" >' + item.content + '</td></tr></table>';
                $("#info").html(html);
            });
        }
    });
}


//$("#c_create ").on("click", function () {

//    location.href = "/StandardMgt/NoticeCreate";

//});

function ActualEfficiency() {
    $.ajax({
        type: 'post',
        url: '/DashBoard/getActualEfficiency',
        dataType: 'text',
        success: function (data) {

            if (isNaN(data)) {
                html = "";
                html += " ", html += " %", $("#m_actual_efficiency").html(html)
            } else {
                var html = "";
                html += data, html += " %", $("#m_actual_efficiency").html(html)
            }

        }
    });
};
ActualEfficiency()
setInterval(ActualEfficiency, 30000);
function ActualAmount() {
    $.ajax({
        type: 'post',
        url: '/DashBoard/getActualAmount',
        dataType: 'text',
        success: function (data) {
            console.log(data);
            var html = '';
            html += data.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#m_actual_amount").html(html);
        }
    });
};

ActualAmount()
setInterval(ActualAmount, 30000);

