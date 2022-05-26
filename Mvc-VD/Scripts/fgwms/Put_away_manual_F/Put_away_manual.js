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

$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        //url: "/fgwms/getput_away",
        url: "/fgwms/searchput_away",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pno', name: 'pno', width: 50, align: 'right', hidden: true },
            { label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            { label: 'Buyer QR', name: 'buyer_qr', width: 150, align: 'left' },
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' }, 
            { label: 'Group Qty', name: 'gr_qty', width: 80, align: 'right' },
            { label: 'Status', name: 'sts_cd', width: 100, align: 'center' },
            { label: 'WO', name: 'fo_no', width: 150, align: 'center' },
            { label: 'Location', name: 'lct_nm', width: 100, align: 'center', hidden: true },
            { label: 'Location Status', name: 'lct_sts_cd', width: 100, align: 'left', hidden: true },
            //{ label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
            { label: 'Out Date', name: 'output_dt', width: 150, align: 'left', hidden: true },
            { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'center' },
            { label: 'Input Date', name: 'input_dt', width: 150, align: 'center', formatter: formatdate },
            //{ label: 'Description', name: 're_mark', width: 150, align: 'left' },
            { label: 'QR Code', name: 'qrcode', width: 300, align: 'left', hidden: true },
            { label: 'PO', name: 'po_no', width: 100, align: 'left' },
            { label: 'Product Date', name: 'prd_dt', width: 100, align: 'center', formatter: formatdate, hidden: true },
            { label: 'BOM', name: 'bom_no', width: 100, align: 'left' },
            { label: 'Routing', name: 'line_no', width: 100, align: 'center' },
            //{ label: 'Create Name', name: 'reg_id', sortable: true, width: 70, align: 'center' },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'Change Name', name: 'chg_id', sortable: true, width: 80, align: 'center' },
            //{ key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $('#c_pdid').val(row_id.pdid);
            $('#c_mt_no').val(row_id.mt_no);
            $('#c_mt_nm').val(row_id.mt_nm);
            $('#c_rec_input_dt').val(row_id.rec_input_dt);
            $('#c_rec_qty').val(row_id.rec_qty);
            $('#c_bundle_qty').val(row_id.bundle_qty);
            $('#c_mdpo_no').val(row_id.mdpo_no);
            $('#c_md_sts_cd').val(row_id.md_sts_cd);
            $('#c_lct_sts_cd').val(row_id.lct_sts_cd);
            $('#c_md_sts_cd').val(row_id.md_sts_cd);
            $("#c_save_but").attr("disabled", false);
        },

        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: 250,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 20,
        caption: 'Receiving Manual(FG)',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
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
});//grid1

$("#searchBtn").click(function () {
    var prd_lcd = $('#prd_lcd').val();
    var style_no = $('#m_style_no').val().trim();
    var lct_cd = $('#c_lct_cd').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val();
    $.ajax({
        url: "/fgwms/searchput_away",
        type: "get",
        dataType: "json",
        data: {
            prd_lcd: prd_lcd,
            style_no: style_no,
            lct_cd: lct_cd,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});

$("#getPutin").on("click", function () {
    var shelf_cd = $('#shelf_cd').val();
    var selRowIds = $('#list').jqGrid("getGridParam", "selarrrow");

    if ($("#shelf_cd").val().trim() == "") {
        alert("Please select destination !");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    if (selRowIds == "") {
        alert("Please select product to put away !");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    $('#loading').show();
    $.ajax({
        type: "get",
        dataType: 'json',
        url: "/fgwms/Updateput_away?id=" + selRowIds,
        data: {
            lct_cd: shelf_cd,
        },
        success: function (response) {
            $('#loading').hide();
            if (response.result) {
                for (var i = 0; i < response.data.length; i++) {
                    var id = response.data[i].pno;
                    $("#list").setRowData(id, response.data[i], { background: "#d0e9c6" });  
                };   
                alert(response.message);
            }
            else {
                alert(response.message);
            }
        },
        error: function () {
            $('#loading').hide();
            alert("Cannot put away products. Please check again !");
        }
    });
});

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

var getLocation = "/fgwms/getLocation";
$(document).ready(function () {
    _getLocation();

});

function _getLocation() {

    $.get(getLocation, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Destination*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#shelf_cd").html(html);
            $("#c_lct_cd").html(html);
        }
    });
}
