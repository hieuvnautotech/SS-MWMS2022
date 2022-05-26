$grid = $("#Cofirm").jqGrid({
    url: '/Shippingmgt/w_sd_info',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'sid', name: 'sid', width: 50, hidden: true },
        { label: 'SD No', name: 'sd_no', width: 150, align: 'center' },
        { label: 'Loaction', name: 'lct_cd', width: 200, align: 'center', },
        { label: 'Worker', name: 'worker_id', width: 110, align: 'left', },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left' },
        { label: 'Status', name: 'sd_sts_cd', width: 110, align: 'center' },
        { label: ' Real Work Date', name: 'real_work_dt', width: 110, align: 'center' },
        { label: 'Work Date', name: 'work_dt', width: 110, align: 'center' },
        { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
        { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#Cofirmpage',
    rownumbers: true,
    caption: 'Material List',
    multiselect: true,
    multiPageSelection: true,
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 300,
    width: $(".box-body").width() - 5,
    autowidth: false,
    loadonce: true,
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    },
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"

    },
    subGrid: true,
    subGridRowExpanded: function(subgrid_id, row_id) {
        var subgrid_table_id;
        subgrid_table_id = subgrid_id+"_t";
        jQuery("#"+subgrid_id).html("<table id='"+subgrid_table_id+"' class='scroll'></table>");
        jQuery("#"+subgrid_table_id).jqGrid({
            url: "/Shippingmgt/DetailCofirm?id=" + row_id,
            datatype: 'json',
            colModel: [
                { label: "sdid", name: "sdid", width: 110, align: "center", },
                { label: "sd_no", name: "sd_no", width: 110, align: "center", },
                { label: "mt_no", name: "mt_no", width: 110, align: "center", },
                { label: "sdm_sts_cd", name: "sdm_sts_cd", 110: 80, align: "center", },
                { label: "d_mt_qty", name: "d_mt_qty", width: 110, align: "center", },
                { label: "MT NO", name: "mt_no", width: 110, align: "center", },
                { label: "MT Name", name: "mt_nm", width: 110, align: "center", },
                { label: "Pick Qty", name: "pick_qty", width: 110, align: "center", },
                { label: "Width", name: "width", width: 110, align: "center", },
                { label: "Spec", name: "spec", width: 110, align: "center", },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            },
            height: '100%',
            sortname: 'num',
            sortorder: "asc",
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
    },
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#Cofirm').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#Cofirm").closest(".ui-jqgrid").parent().width();
    $("#Cofirm").jqGrid("setGridWidth", newWidth, false);
});

$("#Confirm").click(function () {
    var i, selRowIds = $('#Cofirm').jqGrid("getGridParam", "selarrrow"), n, rowData;
    //change  w_mr_detail
    $.ajax({
        url: "/Shippingmgt/upadatew_sd_info?" + "id=" + selRowIds,
        type: "post",
        dataType: "json",
        success: function (data) {
            jQuery("#Cofirm").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (data) {
            jQuery("#Cofirm").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
        }
    });
});

//get select
// Khai báo URL stand của bạn ở đây va get select
var wh = "/StandardMgtWh/Warehouse";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Location *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.index_cd + '>' + item.index_cd + '</option>';
            });
            $("#whouse").html(html);
        }
    });
}

//LƯỚI LƯU

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});
//$('#end').datepicker().datepicker('setDate', 'today');
//search
$("#searchBtn").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchConfirm",
        type: "get",
        dataType: "json",
        data: {
            sd_no: $('#c_sd_no').val().trim(),
            lct_cd: $('#whouse').val().trim(),
            work: $('#work').val().trim(),
            manager: $('#manager').val().trim(),
            start: $('#start').val().trim(),
            end: $('#end').val().trim(),
        },
        success: function (result) {
            $("#Cofirm").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
           
//key_work
$("#c_sd_no").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/Shippingmgt/Autosd_no",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.sd_no, value: item.sd_no };
                }))

            },
            error: function (jqXHR, textStatus, errorThrown) {
                var result = [{ label: "no results", value: response.term }];
                response(result);
            },
        })
    },
    messages: {
        noResults: '',
    }
});