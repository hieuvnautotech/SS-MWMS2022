/// <reference path="../Views/ReceivingMgt/Print.html" />
/// <reference path="../Views/ReceivingMgt/Print.html" />
$(function () {
    $("#wrdinfoGrid").jqGrid
   ({
       url: "/ReceivingMgt/getwrdinfo",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'rid', name: 'rid', width: 50, align: 'right', hidden: true },
           { label: 'RD NO', name: 'rd_no', width: 120, align: 'center' },
           { label: 'Location', name: 'lct_cd', width: 120, align: 'center' },
           { label: 'State', name: 'rd_sts_cd', width: 100, align: 'center' },
           { label: 'Worker Date', name: 'worker_dt', width: 120, align: 'center' },
           { label: 'Worker', name: 'worker_id', width: 120, align: 'center' },
           { label: 'Manager', name: 'manager_id', width: 120, align: 'center' },
           { label: 'MT qty', name: 'mt_qty', width: 120, align: 'center' },
       ],

       onSelectRow: function (rowid, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#wrdinfoGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#wrdinfoGrid").getRowData(selectedRowId);

           var rd_no = row_id.rd_no

           $("#wmaterialinfoGrid").clearGridData();
           $("#wrddetailGrid").clearGridData();


           $("#wrddetailGrid").setGridParam({ url: "/ReceivingMgt/getwrddetail?" + "rd_no=" + rd_no, datatype: "json" }).trigger("reloadGrid");


       },

       pager: "#wrdinfoGridPager",
       pager: jQuery('#wrdinfoGridPager'),
       rowList: [10, 50, 100, 200],
       loadonce: true, //tải lại dữ liệu
       viewrecords: true,
       rownumbers: true,
       hoverrows: false,
       caption: '',
       emptyrecords: "No data.",
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },
       height: 250,
       width: $("#two").width() - 5,
       autowidth: false,
       shrinkToFit: false,


   });
    $('#wrdinfoGrid').jqGrid('setGridWidth', $("#two").width());
    $(window).on("resize", function () {
        var newWidth = $("#wrdinfoGrid").closest(".ui-jqgrid").parent().width();
        $("#wrdinfoGrid").jqGrid("setGridWidth", newWidth, false);
    });


    $('#wrdinfoGrid').jqGrid('setGridWidth', $("#box-body-2").width());
    $(window).on("resize", function () {
        var newWidth = $("#wrdinfoGrid").closest(".ui-jqgrid").parent().width();
        $("#wrdinfoGrid").jqGrid("setGridWidth", newWidth, false);
    });
});


$(function () {
    $("#wrddetailGrid").jqGrid
   ({
       url: "/ReceivingMgt/getwrddetail",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'rdid', name: 'rdid', width: 50, align: 'right', hidden: true },
           { label: 'RD NO', name: 'rd_no', width: 110, align: 'center' },
           { label: 'MT NO', name: 'mt_no', width: 110, align: 'center' },
           { label: 'MT NAME', name: 'mt_nm', width: 110, align: 'center' },
           { label: 'MPO', name: 'mpo_no', width: 120, align: 'center' },
           { label: 'MDPO', name: 'mdpo_no', width: 120, align: 'center' },
       ],



       pager: "#wrddetailGridPager",
       rowList: [1000, 1500, 2000, 2500],
       loadonce: true, //tải lại dữ liệu
       viewrecords: true,
       rownumbers: true,
       caption: '',
       emptyrecords: "No data.",
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },
       height: 250,
       width: $("#three").width() - 5,
       autowidth: false,
       shrinkToFit: false,



       onSelectRow: function (rowid, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#wrddetailGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#wrddetailGrid").getRowData(selectedRowId);

           var mt_no = row_id.mt_no
           $("#wmaterialinfoGrid").clearGridData();

           $("#wmaterialinfoGrid").setGridParam({ url: "/ReceivingMgt/getwmaterialinfo?" + "mt_no=" + mt_no, datatype: "json" }).trigger("reloadGrid");

       },



   });


    $('#wrddetailGrid').jqGrid('setGridWidth', $("#three").width());
    $(window).on("resize", function () {
        var newWidth = $("#wrddetailGrid").closest(".ui-jqgrid").parent().width();
        $("#wrddetailGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $('#wrddetailGrid').jqGrid('setGridWidth', $("#box-body-3").width());
    $(window).on("resize", function () {
        var newWidth = $("#wrddetailGrid").closest(".ui-jqgrid").parent().width();
        $("#wrddetailGrid").jqGrid("setGridWidth", newWidth, false);
    });


    //Start Author and menu connect
    $(function () {
        $("#wmaterialinfoGrid").jqGrid
       ({
           url: "/ReceivingMgt/getwmaterialinfo",
           datatype: 'json',
           mtype: 'Get',
           colModel: [
          { key: true, label: 'Wmtid', name: 'wmtid', width: 50, align: 'center' },
          { label: 'Material Code', name: 'mt_no', width: 200, align: 'center' },
          { label: 'State', name: 'mt_sts_cd', width: 110, align: 'center' },
          { label: 'Location', name: 'lct_cd', width: 180, align: 'center' },
          { label: 'Location State', name: 'lct_sts_cd', width: 120, align: 'center' },
          { label: 'Barcode', name: 'mt_barcode', width: 200, align: 'center' },
           ],
           onSelectRow: function (rowid, selRowIds, selected, status, e) {
               $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           },


           pager: "#wmaterialinfoGridPager",
           viewrecords: true,
           rowList: [20, 50, 200, 500],
           height: 220,
           width: $(".box-body").width() - 5,
           autowidth: false,
           rowNum: 20,
           caption: '',
           loadtext: "Loading...",
           emptyrecords: "No data.",
           rownumbers: true,
           gridview: true,
           loadonce: true,
           shrinkToFit: false,
           multiselect: true,
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



    })

    $("#c_print").on("click", function () {


        var i, selRowIds = $('#wmaterialinfoGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;


        if (selRowIds.length > 0) {

            window.open("/ReceivingMgt/_Print?wmtid=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
        }
        else {
            alert("Please select Grid.");
        }
    })
});

$("#searchBtn").click(function () {

    var s_rd_no = $('#s_rd_no').val().trim();
    var s_lct_cd = $('#s_lct_cd').val().trim();
    var s_worker_id = $('#s_worker_id').val().trim();
    var s_mt_no = $('#s_mt_no').val().trim();
    var start = $('#s_worker_dt_start').val()
    var end = $('#s_worker_dt_end').val()

            $.ajax({
                url: "/ReceivingMgt/searchDirections",
                type: "get",
                dataType: "json",
                data: {
                    s_rd_no: s_rd_no,
                    s_lct_cd: s_lct_cd,
                    s_worker_id: s_worker_id,
                    s_mt_no: s_mt_no,
                    start: start,
                    end: end
                },
                success: function (result) {
                    $("#wrdinfoGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                    //$("#wrddetailGrid").clearGridData();
                    $("#wrddetailGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                    $("#wmaterialinfoGrid").clearGridData();
                }
            });
    //    }
    //}
});

$("#s_worker_dt_start").datepicker({
    format: 'yyyy-mm-dd',
});
//$('#end').datepicker().datepicker('setDate', 'today');
// Init Datepicker end
$("#s_worker_dt_end").datepicker({
    format: 'yyyy-mm-dd',
    //"autoclose": true
});
//function fun_check1() {
//    if (start != "") {
//        if (isValidDate(start) == true) {
//            console.log(isValidDate(start));
//        }
//    }
//}

//function isValidDate(start) {
//    var regEx = /([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))/;
//    if (!start.match(regEx))
//        return false;  // Invalid format
//    var d = new Date(start);
//    var dNum = d.getTime();
//    if (!dNum && dNum !== 0)
//        return false;
//    return d.toISOString().slice(0, 10) === start;
//}



$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'Directions',
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
            fileName: 'Directions',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'Directions',
            escape: false,
        });
    });
});