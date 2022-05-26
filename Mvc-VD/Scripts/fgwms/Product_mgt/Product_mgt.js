$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        url: "/fgwms/getProduct_mgt",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pno', name: 'pno', width: 50, align: 'right', hidden: true },
            { label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            { label: 'Group QTY', name: 'gr_qty', width: 100, align: 'right' },
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            { label: 'Status', name: 'sts_cd', width: 100, align: 'left' },
            { label: 'Status', name: 'sts_code', width: 100, align: 'left', hidden: true  },
            { label: 'WO', name: 'fo_no', width: 100, align: 'center' },
            { label: 'Location', name: 'lct_nm', width: 100, align: 'left' },
            { label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'center', hidden: true },
            { label: 'Location Status', name: 'lct_sts_code', width: 150, align: 'center', hidden: true },
            { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left', hidden: true },
            { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            { label: 'Input Date', name: 'input_dt', width: 150, align: 'center', formatter: formatdate },
            { label: 'Output Date', name: 'output_dt', width: 150, align: 'center', formatter: formatdate },
            { label: 'Description', name: 're_mark', width: 150, align: 'left' },
            { label: 'QR Code', name: 'qrcode', width: 350, align: 'left', hidden: true },
            { label: 'PO', name: 'po_no', width: 100, align: 'center' },
            { label: 'WO', name: 'fo_no', width: 100, align: 'center' },
            { label: 'BOM', name: 'bom_no', width: 100, align: 'center' },
            { label: 'Routing', name: 'line_no', width: 100, align: 'center' },
            { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $("#sts_cd").val(row_id.sts_code);
            $("#prd_lcd").val(row_id.prd_lcd);
            $("#re_mark").val(row_id.re_mark);
            $("#plno").val(row_id.pno);
        },

        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500,1000],
        height: 400,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: 'Product Management',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
       
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

});//grid1

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#start_out").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end_out").datepicker({ dateFormat: 'yy-mm-dd' }).val();

var getLocation = "/fgwms/getLocation";

$(document).ready(function () {
    _getLocation();
});
function _getLocation() {

    $.get(getLocation, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
}
var getsts = "/fgwms/sts_change";

$(document).ready(function () {
    _getsts();
});
function _getsts() {

    $.get(getsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#sts_cd").html(html);
        }
    });
}

function formatdate(cellValue, options, rowdata, action) {
    if (cellValue != null && cellValue != "") {
        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);
        var html = a + "-" + b + "-" + c;
        return html;
    }
    else {
        var html = "";
        return html;
    }
};

$("#c_save_but").click(function () {
    var pno = $('#plno').val() == null ? "" : $('#plno').val().trim();
    var re_mark = $('#re_mark').val() == null ? "" : $('#re_mark').val().trim();
    var sts_cd = $('#sts_cd').val() == null ? "" : $('#sts_cd').val().trim();
    $('#loading').show();
    $.ajax({
        url: "/fgwms/updatepr_mgt",
        type: "get",
        dataType: "json",
        data: {
            pno: pno,
            re_mark: re_mark,
            sts_cd: sts_cd
        },
        success: function (response) {
            if (response.result) {
                console.log(response.result);
                var id = response.data.pno;
                $("#list").setRowData(id, response.data, { background: "#d0e9c6",color:"black" });
                document.getElementById("form1").reset();
                $('#loading').hide();
                alert(response.message);
                return true;
            }
            else {
                alert(response.message);
                return false;
            }
        },
        error: function () {
            alert('Fault. Please check again.');
            return false;
        }
    });
});

$("#searchBtn").click(function () {
    var prd_lcd = $('#s_mt_cd').val() == null ? "" : $('#s_mt_cd').val().trim();
    var style_no = $('#m_style_no').val() == null ? "" : $('#m_style_no').val().trim();
    var lct_cd = $('#c_lct_cd').val() == null ? "" : $('#c_lct_cd').val().trim();
    var fo_no = $('#s_fo_no').val() == null ? "" : $('#s_fo_no').val().trim();
    var po_no = $('#s_po_no').val() == null ? "" : $('#s_po_no').val().trim();
    var bom_no = $('#c_bom_no').val() == null ? "" : $('#c_bom_no').val().trim();
    var start = $('#start').val() == null ? "" : $('#start').val().trim();
    var end = $('#end').val() == null ? "" : $('#end').val().trim();
    var start_out = $('#start_out').val() == null ? "" : $('#start_out').val().trim();
    var end_out = $('#end_out').val() == null ? "" : $('#end_out').val().trim();
    
    $.ajax({
        //url: "/fgwms/searchproduct_mgt",
        url: "/fgwms/getProduct_mgt",
        type: "get",
        dataType: "json",
        data: {
            prd_lcd: prd_lcd,
            style_no: style_no,
            lct_cd: lct_cd,
            bom_no: bom_no,
            po_no: po_no,
            fo_no: fo_no,
            start: start,
            end: end,
            start_out: start_out,
            end_out: end_out,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});