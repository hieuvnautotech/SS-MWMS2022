$(function () {
    $("#picScanGrid").jqGrid
   ({
       url: "/ShippingMgt/getpicScan",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
          { key: true, label: 'Wmtid', name: 'wmtid', width: 50, align: 'center' },
          { label: 'Material Code', name: 'mt_no', width: 200, align: 'center' },
          { label: 'Name', name: 'mt_nm', width: 200, align: 'center' },
          { label: 'State', name: 'mt_sts_cd', width: 110, align: 'center' },
          { label: 'Location', name: 'lct_cd', width: 180, align: 'center' },
          { label: 'Location State', name: 'lct_sts_cd', width: 120, align: 'center' }
       ],

       pager: "#picScanGridPager",
       rowList: [10, 50, 100, 200],
       loadonce: true, //tải lại dữ liệu
       viewrecords: true,
       rownumbers: true,
       hoverrows: false,
       caption: 'Picking Scan',
       emptyrecords: "No data.",
       height: 450,
       width: $(".box-body").width() - 5,
       autowidth: false,
       shrinkToFit: false,
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
    $('#picScanGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#picScanGrid").closest(".ui-jqgrid").parent().width();
        $("#picScanGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $('#m_mt_barcode').keypress(function (event) {
        //var lct_cd = $('#lct_cd').val();
        var mt_barcode = $('#m_mt_barcode').val();
        $.ajax({
            url: "/ShippingMgt/getpicScan",
            type: "get",
            dataType: "json",
            data: {
                //lct_cd: lct_cd,
                mt_barcode: mt_barcode
            },
            success: function (result) {
                $("#picScanGrid").jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }

        });

    });

    function fun_check1() {

        var selectedRowId = $("#picScanGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#picScanGrid").getRowData(selectedRowId);
        if (row_id.length==null) {
            alert("Please select Grid.");
            return false;
        }

        if ($("#m_mt_barcode").val().trim() == "") {
            alert("Please enter the Barcode.");
            $("#m_mt_barcode").val("");
            $("#m_mt_barcode").focus();
            return false;
        }
        if ($("#m_lct_cd").val().trim() == "") {
            alert("Please enter the Location.");
            $("#m_lct_cd").val("");
            $("#m_lct_cd").focus();
            return false;
        }
        return true;
    }

    $("#m_save_but").click(function () {
        if (fun_check1() == true) {
            var selectedRowId = $("#picScanGrid").jqGrid("getGridParam", 'selrow');
            row_id = $("#picScanGrid").getRowData(selectedRowId);
            var wmtid = row_id.wmtid;
            var lct_cd = $('#m_lct_cd').val();

            $.ajax({
                url: "/ShippingMgt/updatepicScan",
                type: "get",
                dataType: "json",
                data: {
                    lct_cd: lct_cd,
                    wmtid: wmtid
                },
                success: function (data) {

                    $("#picScanGrid").clearGridData();
                    //jQuery("#picScanGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                }
            });
        }
    });


    //$("#searchBtn").click(function () {

    //    var s_rd_no = $('#s_rd_no').val().trim();
    //    var s_lct_cd = $('#s_lct_cd').val().trim();
    //    var s_worker_id = $('#s_worker_id').val().trim();
    //    var s_mt_no = $('#s_mt_no').val().trim();


    //    var start = $('#s_worker_dt_start').val()
    //    var end = $('#s_worker_dt_end').val()


    //    $.ajax({
    //        url: "/ReceivingMgt/searchDirections",
    //        type: "get",
    //        dataType: "json",
    //        data: {
    //            s_rd_no: s_rd_no,
    //            s_lct_cd: s_lct_cd,
    //            s_worker_id: s_worker_id,
    //            s_mt_no: s_mt_no,
    //            start: start,
    //            end: end
    //        },
    //        success: function (result) {
    //            $("#picScanGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
    //        }
    //    });

});

var getType = "/ShippingMgt/getType";

$(document).ready(function () {
    _getTypeS();
    $("#m_lct_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeS() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#m_lct_cd").html(html);
        }
    });
}




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