$(function () {
    $("#list").jqGrid
    ({
        url: "/ReceivingMgt/getreceiving",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'pmid', name: 'pmid', width: 50, hidden: true },
             { label: 'Mpo No', name: 'mpo_no', width: 110, align: 'center' },
             { label: 'MDPO', name: 'mdpo_no', width: 110, align: 'center' },
             { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
             { label: 'Material Name', name: 'mt_nm', width: 110, align: 'center' },
             { label: 'State', name: 'md_sts_cd', width: 110, align: 'center' },
             { label: 'Expect Date', name: 'exp_input_dt', width: 110, align: 'center' },
             { label: 'Receive Date', name: 'rec_input_dt', width: 110, align: 'center' },
             { label: 'Purchase qty', name: 'act_pur_qty', width: 110, align: 'right' },
             { label: 'Receive qty', name: 'rec_qty', width: 110, align: 'right' },
             { label: 'Pass qty', name: 'pass_qty', width: 110, align: 'right' },
             { label: 'Defect qty', name: 'defect_qty', width: 110, align: 'right' },
             { label: 'Return qty', name: 'return_qty', width: 110, align: 'right' },

             { label: 'Feed size', name: 'feed_size', width: 110, align: 'right' },
             
             { label: 'Feed Unit', name: 'feed_unit', width: 110, align: 'center' },

              //{ name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntCellValue },
        ],
        //// onCellSelect: function (rowid, iCol, cellcontent, e) {
        //     if (iCol == 10) {
        //         $('.delete-btn').click(function () {
        //             var mt_id = $(this).data("id_delete");

        //             $.ajax({
        //                 url: "/PurchaseMgt/delCommmt",
        //                 type: "post",
        //                 dataType: "json",
        //                 data: {
        //                     mt_id: mt_id,
        //                 },
        //                 success: function (result) {
        //                     $("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

        //                     $("#commondtGrid").clearGridData();



        //                     document.getElementById("form1").reset();
        //                     $("#tab_2").removeClass("active");
        //                     $("#tab_1").addClass("active");
        //                     $("#tab_c2").removeClass("active");
        //                     $("#tab_c1").removeClass("hidden");
        //                     $("#tab_c2").addClass("hidden");
        //                     $("#tab_c1").addClass("active");


        //                     document.getElementById("form3").reset();
        //                     $("#tab_d2").removeClass("active");
        //                     $("#tab_d1").addClass("active");
        //                     $("#tab_dc2").removeClass("active");
        //                     $("#tab_dc1").removeClass("hidden");
        //                     $("#tab_dc2").addClass("hidden");
        //                     $("#tab_dc1").addClass("active");

        //                 },
        //                 error: function (result) {
        //                     alert('User is not existing. Please check again');
        //                 } 
        //             });
        //         });
        //     }
        // },


        

        pager: "#listPager",
        pager: jQuery('#listPager'),
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: 220,
        width: '100%',
        autowidth: false,
        rowNum: 20,
        caption: 'Status Information',
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




$("#searchBtn").click(function () {
    var mpo_no = $('#mpo_no').val().trim();
    var mt_no = $('#mt_no').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    var start1 = $('#start1').val().trim();
    var end2 = $('#end2').val().trim();

    $.ajax({
        url: "/ReceivingMgt/searchRequest",
        type: "get",
        dataType: "json",
        data: {
           
            mpo_no: mpo_no,
            mt_no:mt_no,
            start: start,
            end: end,
            start1: start1,
            end2: end2,
          
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

//function bntCellValue(cellValue, options, rowdata, action) {
//    id_delete = rowdata.mt_id;
//    var html = '<button class="btn btn-xs btn-danger delete-btn" title="Delete" data-id_delete="' + rowdata.mt_id + '" >X</button>';
//    return html;
//};

$("#start").datepicker({
    format: 'mm/dd/yyyy',
});
$('#end').datepicker().datepicker('setDate', 'today');
//Init Datepicker end
$("#end").datepicker({
    format: 'mm/dd/yyyy',
    "autoclose": true
});
$("#start1").datepicker({
    format: 'mm/dd/yyyy',
});
$("#end2").datepicker({
    format: 'mm/dd/yyyy',
    "autoclose": true
});



$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#list').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'POMgt',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('#list').tableExport({
            type: 'xls',
            fileName: 'POMgt',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('#list').tableExport({
            type: 'doc',
            fileName: 'POMgt',
            escape: false,
        });
    });
});