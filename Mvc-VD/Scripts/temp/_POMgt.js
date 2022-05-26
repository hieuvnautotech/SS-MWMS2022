    var row_id, row_id2;
    $grid = $("#poMgtGrid").jqGrid
   ({
       url: "/SalesMgt/getpoData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'id', name: 'soid', width: 50, align: 'right', hidden: true },
           { label: 'PO NO', name: 'po_no', width: 110, align: 'center' },
           { label: 'BOM', name: 'bom_no', width: 110, align: 'center' },
           { label: 'Style', name: 'style_no', width: 110, align: 'center' },
           { label: 'Model', name: 'md_cd', width: 120, align: 'center' },
           { label: 'Project Name', name: 'prj_nm', width: 200, align: 'left' },
           { label: 'Order Date', name: 'order_dt', width: 120, align: 'center' },
           { label: 'Product Date', name: 'product_dt', width: 120, align: 'center' },
           { label: 'Delivery Date', name: 'delivery_dt', width: 120, align: 'center' },
           { label: 'Destinationcd', name: 'dest_cd', width: 150, align: 'center', hidden: true },
           { label: 'Destination', name: 'dest_nm', width: 150, align: 'center' },
           { label: 'Delivery Qty', name: 'delivery_qty', width: 80, align: 'right' },
           { label: 'FO Qty', name: 'fo_qty', width: 60, width: 80, align: 'right' },
           { label: 'Fo Remain Qty', name: 'fo_rm_qty', width: 80, align: 'right' },
           { label: 'SO Qty', name: 'so_qty', width: 60, width: 100, align: 'right' },
           { label: 'SO Remain Qty', name: 'so_rm_qty', width: 80, align: 'right' },
           { label: 'Remark', name: 're_mark', width: 200, align: 'left' },
       ],
       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#poMgtGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#poMgtGrid").getRowData(selectedRowId);
           var soid = row_id.soid;
           var po_no = row_id.po_no;
           var bom_no = row_id.bom_no;
           var order_dt = row_id.order_dt;
           var delivery_dt = row_id.delivery_dt;
           var dest_cd = row_id.dest_cd;
           var delivery_qty = row_id.delivery_qty;
           var re_mark = row_id.re_mark;
           var product_dt = row_id.product_dt;

           $('#m_soid').val(soid);


           $('#m_po_no').val(po_no);
           $('#m_dest_cd').val(dest_cd);
           $('#m_bom_no').val(bom_no);
           $('#m_delivery_qty').val(delivery_qty);
           $('#m_order_dt').val(order_dt);
           $('#m_re_marks').val(re_mark);


           $('#m_delivery_dt').val(delivery_dt);
           $('#m_product_dt').val(product_dt);

           $("#tab_1").removeClass("active");
           $("#tab_2").addClass("active");
           $("#tab_c1").removeClass("active");
           $("#tab_c2").removeClass("hidden");
           $("#tab_c1").addClass("hidden");
           $("#tab_c2").addClass("active");
           $("#m_save_but").attr("disabled", false);
           $("#del_save_but").attr("disabled", false);
       },

       pager: jQuery('#poMgtGridPager'),
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 400,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 20,
       caption: 'PO Management ',
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


   });

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#poMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#poMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#poMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });
    function fun_check1() {
        if ($("#c_lct_nm").val().trim() == "") {
            alert("Please enter Location Name");
            $("#c_lct_nm").val("");
            $("#c_lct_nm").focus();
            return false;
        }
        var selRowId = $('#destinationGrid').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Location on the grid.");
            return false;
        }
        return true;
    }

    $("#c_save_but").click(function () {

        var po_no = $('#c_po_no').val();
        var dest_cd = $('#c_dest_cd').val();
        var bom_no = $('#c_bom_no').val();
        var delivery_qty = $('#c_delivery_qty').val();
        var order_dt = $('#c_order_dt').val();
        var re_marks = $('#c_re_marks').val();
        var delivery_dt = $('#c_delivery_dt').val();
        var product_dt = $('#c_product_dt').val();
        {
            $.ajax({
                url: "/SalesMgt/insertPoMgt",
                type: "get",
                dataType: "json",
                data: {
                    po_no: po_no,
                    dest_cd: dest_cd,
                    bom_no: bom_no,
                    delivery_qty: delivery_qty,
                    order_dt: order_dt,
                    re_marks: re_marks,
                    delivery_dt: delivery_dt,
                    product_dt: product_dt,
                },
                success: function (data) {
                    jQuery("#poMgtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                }
            });
        }
    });

    $("#m_save_but").click(function () {

        var po_no = $('#m_po_no').val();
        var dest_cd = $('#m_dest_cd').val();
        var bom_no = $('#m_bom_no').val();
        var delivery_qty = $('#m_delivery_qty').val();
        var order_dt = $('#m_order_dt').val();
        var re_marks = $('#m_re_marks').val();
        var delivery_dt = $('#m_delivery_dt').val();
        var product_dt = $('#m_product_dt').val();
        var soid = $('#m_soid').val();

        $.ajax({
            url: "/SalesMgt/updatePoMgt",
            type: "get",
            dataType: "json",
            data: {
                po_no: po_no,
                dest_cd: dest_cd,
                bom_no: bom_no,
                delivery_qty: delivery_qty,
                order_dt: order_dt,
                re_marks: re_marks,
                delivery_dt: delivery_dt,
                product_dt: product_dt,
                soid: soid,
            },
            success: function (data) {
                jQuery("#poMgtGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });

    });

    $("#searchBtn").click(function () {
        var po_no = $('#s_po_no').val().trim();
        var bom_no = $('#s_bom_no').val().trim();
        var style_no = $('#s_style_no').val().trim();
        $.ajax({
            url: "/SalesMgt/searchPoMgt",
            type: "get",
            dataType: "json",
            data: {
                po_no: po_no,
                bom_no: bom_no,
                style_no: style_no
            },
            success: function (result) {
                $("#poMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

    $(document).ready(function (e) {
        $('#pdfBtn').click(function (e) {
            $('table').tableExport({
                headers: true,
                type: 'pdf',
                pdfFontSize: '12',
                fileName: 'POMgt_Infor',
                escape: false,
            });
        });
    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $('table').tableExport({
                headers: true,
                type: 'xls',
                fileName: 'POMgt_Infor',
                escape: false,
                headers: true,
            });
        });
    });
    $(document).ready(function (e) {
        $('#htmlBtn').click(function (e) {
            $('table').tableExport({
                type: 'doc',
                fileName: 'POMgt_Infor',
                escape: false,
                headers: true,
            });
        });
    });

$("#tab_1").on("click", "a", function (event) {
    $("#poMgtGrid").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
    $("#poMgtGrid").trigger('reloadGrid');
    document.getElementById("form2").reset();
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});



var getDest = "/SalesMgt/getDest";
//Create
$(document).ready(function () {
    _getDestC();
    $("#c_dest_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getDestC() {

    $.get(getDest, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Dest*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_dest_cd").html(html);
        }
    });
}
//update
$(document).ready(function () {
    _getDestM();
    $("#m_dest_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getDestM() {

    $.get(getDest, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Dest*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#m_dest_cd").html(html);

        }
    });
}


$("#m_order_dt").datepicker({
    format: 'dd/mm/yyyy',
});

$("#m_delivery_dt").datepicker({
    format: 'dd/mm/yyyy',
});

$("#m_product_dt").datepicker({
    format: 'dd/mm/yyyy',
});

$("#c_order_dt").datepicker({
    format: 'dd/mm/yyyy',
});

$("#c_delivery_dt").datepicker({
    format: 'dd/mm/yyyy',
});

$("#c_product_dt").datepicker({
    format: 'dd/mm/yyyy',
});