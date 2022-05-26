$("#list").jqGrid
({
    url: "/ShippingMgt/statusdata",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'mrid', name: 'mrid', width: 50, hidden: true },
        { label: 'MR No', name: 'mr_no', width: 110, align: 'center' },
        { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
        { label: 'Material Name', name: 'mt_nm', width: 110, align: 'center' },
        { label: 'State', name: 'mr_sts_cd', width: 110, align: 'center', hidden: true },
        { label: 'State', name: 'dt_name', width: 110, align: 'center' },
        { label: 'Requester', name: 'req_qty', width: 110, align: 'center' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'center' },
        { label: 'Reg receive Date', name: 'req_rec_dt', width: 110, align: 'center' },
        { label: 'Real Receive Date', name: 'real_rec_dt', width: 110, align: 'right' },
        { label: 'MT Qty', name: 'mt_qty', width: 110, align: 'right' },
        { label: 'Relation Bom', name: 'rel_bom', width: 110, align: 'right' },
        { label: 'Remark', name: 'remark', width: 110, align: 'right' },
        { label: 'Creat User', name: 'reg_id', width: 200 },
        { label: 'Create Date', name: 'reg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Change User', name: 'chg_id', width: 200, align: 'center' },
        { label: 'Change Date', name: 'chg_dt', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
    ],
    pager: "#listPager",
    viewrecords: true,
    rowList: [20, 50, 200, 500],
    height: 400,
    width: $(".box-body").width() - 5,
    autowidth: false,
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


});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

//Create
var getType = "/ShippingMgt/getTypeStShipping";
_getTypeC();
function _getTypeC() {
    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $('state').empty();
            html += '<option value="" selected="selected">* State *</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#state").html(html);
        }
    });
}

$("#searchBtn").click(function () {
    var mr_no = $('#mr_no').val();
    var mt_no = $('#mt_no').val();
    var state = $('#state').val();
    $.ajax({
        url: "/ShippingMgt/searchRequest2",
        type: "get",
        dataType: "json",
        data: {

            mr_no: mr_no,
            mt_no: mt_no,
            state: state,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype:'local', data: result.rows }).trigger("reloadGrid");
        }
    });
});

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

