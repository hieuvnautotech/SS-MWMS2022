
$(function () {

    $grid = $("#Cofirm").jqGrid({
        url: '/Shippingmgt/w_sd_info',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'sid', name: 'sid', width: 50, hidden: true },
            { label: 'SD No', name: 'sd_no', width: 110, align: 'center' },
            { label: 'lct_cd', name: 'lct_cd', width: 150, align: 'center', hidden: true },
             { label: 'Destination', name: 'lct_nm', width: 150, },
            { label: 'Mt Qty', name: 'mt_qty', width: 150, align: 'right', },
            { label: 'Pick Qty', name: 'pick_qty', width: 150, align: 'right', },
            { label: 'Worker', name: 'worker_id', width: 110, align: 'left', },
            { label: 'Manager', name: 'manager_id', width: 110, align: 'left' }, 
            { label: 'Status', name: 'sd_sts_cd', width: 110, align: 'left' },
            { label: ' Real Work Date', name: 'real_work_dt', width: 110, align: 'center' },
            { label: 'Work Date', name: 'work_dt', width: 110, align: 'center' },
            { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
        ],
        loadonce: true,
        shrinkToFit: false,
        pager: '#Cofirmpage',
        rowNum: 2000,
        rownumbers: true,
        caption: 'Material List',
        multiselect: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [60, 80, 100, 120],
        viewrecords: true,
        height: 415,
        loadonce: true,
        width: 780,
        height: 600,
        rowNum: 10,
        subGrid: true, // set the subGrid property to true to show expand buttons for each row
        subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid
        jsonReader:
      {
          root: "rows",
          page: "page",
          total: "total",
          records: "records",
          repeatitems: false,
          Id: "0"

      },
        //width: $(".box-body").width() - 5,
        autowidth: true,
        //onSelectRow: function (rowid, status, e) {

        //},
    });
});
//
function showChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    $("#" + childGridID).jqGrid({
        url: "/Shippingmgt/DetailCofirm?id=" + parentRowKey,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'sdid', name: 'sdid', width: 50, hidden: true },
            { key: false, label: 'SD NO', name: 'sd_no', width: 110, align: 'center' },
            { label: 'MT NO  ', name: 'mt_no', width: 150, align: 'left' },
            { label: 'MT CD  ', name: 'mt_cd', width: 150, align: 'left' },
            { label: 'MT Name ', name: 'mt_nm', width: 150, align: 'left' },
            { label: 'Group Qty', name: 'gr_qty_with', width: 100, align: 'right' },
            { label: 'Group Unit', name: 'gr_qty', width: 100, align: 'right' },
            { label: 'Send Bundle Qty', name: 'req_bundle_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Send Qty', name: 'req_qty', width: 80, align: 'right', formatter: 'integer' },
            { label: 'ML Qty', name: 'ml_qty', width: 110, align: 'right' },
            { label: 'Width(mm)', name: 'with_new', width: 80, align: 'right' },
            { label: 'Length(mm)', name: 'spec_new', width: 80, align: 'right' },
            { label: 'Area(m²)', name: 'area_new', width: 80, align: 'right' },
        ],
        shrinkToFit: false,
        loadonce: true,
        width: '100%',
        height: '100%',
        subGrid: true, // set the subGrid property to true to show expand buttons for each row
        subGridRowExpanded: showThirdLevelChildGrid, // javascript function that will take care of showing the child grid
        pager: "#" + childGridPagerID,
        rowNum: 2000,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [60, 80, 100, 120],
        viewrecords: true,
        rowNum: 10,
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

}

// the event handler on expanding parent row receives two parameters
// the ID of the grid tow  and the primary key of the row
function showThirdLevelChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    // send the parent row primary key to the server so that we know which grid to show
    var childGridURL = "child3.json";

    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    $("#" + childGridID).jqGrid({
        url: "/Shippingmgt/w_sdmt_list_confim?id=" + parentRowKey,
        mtype: "GET",
        datatype: "json",
        colModel: [
             { key: true, label: 'sdmid', name: 'sdmid', width: 50, hidden: true },
              { key: false, label: 'SD NO', name: 'sd_no', width: 110, align: 'center' },
              { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
              { label: 'Group Qty', name: 'gr_qty', width: 110, align: 'right' },
              { label: 'Location', name: 'lct_cd', width: 200, align: 'left' },
              { label: 'MT No', name: 'mt_no', width: 200, align: 'left' },
              { label: 'MT Name', name: 'mt_nm', width: 200, align: 'left' },
              { label: 'Status', name: 'mt_sts_cd', width: 110, align: 'left' },
              { label: 'Location Status', name: 'lct_sts_cd', width: 110, align: 'left' },
              { label: 'QR Code', name: 'mt_barcode', width: 250, align: 'left' },
        ],
        shrinkToFit: false,
        loadonce: true,
        width: '100%',
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 2000,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [60, 80, 100, 120],
        viewrecords: true,
        rowNum: 10,
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

}
$("#Confirm").click(function () {
    var i, selRowIds = $('#Cofirm').jqGrid("getGridParam", "selarrrow"), n, rowData;
    $('#loading').show();
    //change  w_mr_detail
    $.ajax({

        url: "/Shippingmgt/upadate4_table?" + "id=" + selRowIds,
        type: "post",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                $('#loading').hide();
                jQuery("#Cofirm").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
            }
            else {
                $('#loading').hide();
                alert("Do not have rows ");
            }
        },
        error: function (data) {
            $('#loading').hide();
            alert("Error");
            jQuery("#Cofirm").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
        }
    });
});

//get select
// Khai báo URL stand của bạn ở đây va get select
var wh = "/Shippingmgt/Warehouse";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Destination *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#whouse").html(html);
        }
    });
}

//LƯỚI LƯU

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
//search
$("#searchBtn").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchConfirm",
        type: "get",
        dataType: "json",
        data: {
            sd_no: $('#c_sd_no').val().toString().trim(),
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
