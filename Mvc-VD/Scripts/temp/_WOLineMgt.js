$(function () {
    var row_id, row_id2;
    $grid = $("#woMgtGrid").jqGrid
   ({
       url: "/ProducePlan/getwoMgtData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'oflno', name: 'oflno', width: 10, hidden: true },
           { key: false, label: 'FO NO', name: 'fo_no', width: 120, align: 'center' },
           { key: false, label: 'Factory', name: 'lct_cd', width: 120, align: 'center' },
           { key: false, label: 'Total Qty', name: 'fo_qty', width: 80, align: 'right' },
           { key: false, label: 'Daily Qty', name: 'day_qty', width: 80, align: 'right' },
           { key: false, label: 'Line Qty', name: 'line_qty', width: 80, align: 'right' },
           { key: false, label: 'Toltal Daily Qty', name: 'qty_day_line', width: 110, align: 'right' },
           { key: false, label: 'Necessary Days', name: 'need_day', width: 100, align: 'right' },
           { key: false, label: 'Toltal Necessary Days', name: 'total_need_day', width: 130, align: 'right' },
           { key: false, label: 'Reg Product Date', name: 'product_dt', width: 110, align: 'center' },
           { key: false, label: 'Real Product Date', name: 'product_real_dt', width: 110, align: 'center' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, status, e) {

           var selectedRowId = $("#woMgtGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#woMgtGrid").getRowData(selectedRowId);

           $("#m_oflno").val(row_id.oflno);
           $("#m_fo_no").val(row_id.fo_no);
           $("#m_fo_qty").val(row_id.fo_qty);
           $("#m_day_qty").val(row_id.day_qty);
           $("#m_line_qty").val(row_id.line_qty);
           $("#m_qty_day_line").val(row_id.qty_day_line);
           $("#m_need_day").val(row_id.need_day);
           $("#m_total_need_day").val(row_id.total_need_day);
           $("#dc_save_but").attr("disabled", false);
       },


       pager: "#woMgtGridPager",
       pager: jQuery('#woMgtGridPager'),
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 220,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 20,
       caption: 'WO(Line) Management',
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
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#woMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#woMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#woMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });


    function fun_check1() {

        if ($("#m_day_qty").val() == "" || $("#m_day_qty").val() == 0) {
            alert("Please enter the Daily");
            $("#m_day_qty").val("");
            $("#m_day_qty").focus();
            return false;
        }
        if ($("#m_line_qty").val() == "" || $("#m_line_qty").val() == 0) {
            alert("Please enter the Line");
            $("#m_line_qty").val("");
            $("#m_line_qty").focus();
            return false;
        }
        if (isNaN($("#m_day_qty").val()) == true) {
            $("#m_day_qty").val("");
            $("#m_day_qty").focus();
            return false;
        }
        if (isNaN($("#m_line_qty").val()) == true) {
            $("#m_line_qty").val("");
            $("#m_line_qty").focus();
            return false;
        }
        return true;
    }



    $("#dc_save_but").click(function () {
        if (fun_check1() == true) {
            var oflno = $('#m_oflno').val();
            var fo_no = $('#m_fo_no').val();
            var fo_qty = $('#m_fo_qty').val();
            var day_qty = $('#m_day_qty').val();
            var line_qty = $('#m_line_qty').val();
            var qty_day_line = $('#m_qty_day_line').val();
            var need_day = $('#m_need_day').val();
            var total_need_day = $('#m_total_need_day').val();
            {
                console.log(oflno);
                if (oflno == "" || oflno == null || oflno == 0) {
                    $.ajax({
                        url: "/ProductPlan/insertwoMgt",
                        type: "get",
                        dataType: "json",
                        data: {
                            fo_no: fo_no,
                            day_qty: day_qty,
                            line_qty: line_qty,
                            qty_day_line: qty_day_line,
                            need_day: need_day,
                            total_need_day: total_need_day,
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
                }
                else {

                    $.ajax({
                        url: "/ProductPlan/updatewoMgt",
                        type: "get",
                        dataType: "json",
                        data: {
                            oflno: oflno,
                            day_qty: day_qty,
                            line_qty: line_qty,
                            qty_day_line: qty_day_line,
                            need_day: need_day,
                            total_need_day: total_need_day,
                        },
                        success: function (data) {
                            jQuery("#woMgtGrid").setGridParam({ rowNum: 20, datatype: "json" }).trigger('reloadGrid');
                            document.getElementById("form1").reset();
                            $("#dc_save_but").attr("disabled", true);
                        }

                    });
                }
            }
        }

    });

});


function onlyNumber(event) {
    event = event || window.event;
    var keyID = (event.which) ? event.which : event.keyCode;
    if ((keyID >= 48 && keyID <= 57) || (keyID >= 96 && keyID <= 105) || keyID == 8 || keyID == 46 || keyID == 37 || keyID == 39)
        return;
    else
        return false;
}



var getType = "/ProducePlan/getType";

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


$("#start").datepicker({
    format: 'yyyy-mm-dd',
});

$("#end").datepicker({
    format: 'yyyy-mm-dd',
});


$("#searchBtn").click(function () {

    var fo_no = $('#s_fo_no').val().trim()
    var lct_cd = $('#m_lct_cd').val();
    var start = $('#start').val()
    var end = $('#end').val()
    $.ajax({
        url: "/ProductPlan/searchwoMgt",
        type: "get",
        dataType: "json",
        data: {
            fo_no: fo_no,
            lct_cd: lct_cd,
            start: start,
            end: end
        },
        success: function (result) {
            $("#woMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            document.getElementById("form1").reset();
            $("#dc_save_but").attr("disabled", true);
        }
    });

});



$("#start").datepicker({
    format: 'yyyy-mm-dd',
});

$("#end").datepicker({
    format: 'yyyy-mm-dd',
});



