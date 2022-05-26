$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        url: "/fgwms/getpicking_manual_f",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pno', name: 'pno', width: 50, align: 'right', hidden: true },
            { label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            { label: 'Group QTY', name: 'gr_qty', width: 100, align: 'right' },
            { label: 'Status', name: 'sts_cd', width: 100, align: 'center', hidden: true },
            { label: 'WO', name: 'fo_no', width: 150, align: 'center' },
            { label: 'Location', name: 'lct_nm', width: 100, align: 'left'},
            { label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left', hidden: true },
            { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
            { label: 'Out Date', name: 'output_dt', width: 150, align: 'center', formatter: formatdate, hidden: true },
            { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            //{ label: 'Input Date', name: 'input_dt', width: 150, align: 'left' },
            { label: 'Description', name: 're_mark', width: 150, align: 'left' },
            { label: 'Create Name', name: 'reg_id', sortable: true, width: 150, align: 'center' },
            //{ key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            //{ key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'QR Code', name: 'qrcode', width: 350, align: 'left', hidden: true },
            { label: 'PO', name: 'po_no', width: 350, align: 'left', hidden: true },
            { label: 'BOM', name: 'bom_no', width: 350, align: 'left', hidden: true },
            { label: 'Line', name: 'line_no', width: 350, align: 'left', hidden: true },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
        },
        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 550,
        //width: $(".box-body").width() - 5,
        autowidth: true,
        rowNum: 50,
        caption: 'Picking Manual (F) ',
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

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

var getLocation = "/fgwms/getLocation";
//var getDestination = "/SalesMgt/getDestinationData";

$(document).ready(function () {
    //_getLocation();
});
function _getLocation() {

    $.get(getDest, function (data)
    {
        if (data != null && data != undefined && data.length)
        {
            var html = '';
            html += '<option value="" selected="selected">*Destination*</option>';
            $.each(data, function (key, item)
            {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#shelf_cd").html(html);
        }
    });
    //$.get(getDest, function (data)
    //{
    //    if (data != null && data != undefined && data.length) {
    //        var html = '';
    //        html += '<option value="" selected="selected">*Location*</option>';
    //        $.each(data, function (key, item) {
    //            html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
    //        });
    //        $("#shelf_cd").html(html);
    //    }
    //});
}

var getDest = "/SalesMgt/getDest";
$(document).ready(function ()
{
    _getLocation();
    _getDestC();
    $("#c_lct_cd").on('change', function () {
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
            $("#c_lct_cd").html(html);
        }
    });
}

$("#getPutin").on("click", function () {
    var shelf_cd = $('#shelf_cd').val();
    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;
  
    if ($("#shelf_cd").val().trim() == "") {
        alert("Please enter your line name");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    if (selRowIds == "") {
        alert("Please select row you want change");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    $('#loading').show();
    $.ajax({
        type: "get",
        dataType: 'json',
        url: "/fgwms/Updatepicking_manual_F?id=" + selRowIds,
        data: {
            lct_cd: shelf_cd,
        },
        success: function (response) {

            $('#loading').hide();
            if (response.result) {
                alert(response.message);
                for (var i = 0; i < response.data.length; i++) {
                    var id = response.data[i].pno;
                    $("#list").setRowData(id, response.data[i], { background: "#d0e9c6" });
                }
            }
            else {
                alert(response.message);
            }

        },
        error: function (request, status, error) {
            alert("error");
            $('#loading').hide();
        }
    });
});

$("#searchBtn").click(function () {
    var prd_lcd = $('#prd_lcd').val().trim();
    var style_no = $('#m_style_no').val().trim();
    var start = $('#start').val().trim();
    var lct_cd = $('#c_lct_cd').val();
    var end = $('#end').val();
    $.ajax({
        //url: "/fgwms/searchPicking_Manual_F",
        url: "/fgwms/getpicking_manual_f",
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
        },
       
    });
});

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