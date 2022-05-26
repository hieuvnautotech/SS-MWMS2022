//var getFactory = "/MaterialInformation/Warehouse_wip";
var getFactory = "/wipwms/getLocationPR";
var GetType = "/MaterialInformation/GetType";
var GetLocationsts = "/MaterialInformation/GetLocationsts";
var GetStatus = "/MaterialInformation/GetStatus";

$(document).ready(function () {
    _getFactory();
    _GetType();
    _GetLocationsts();
    _GetStatus();

    //---getDay - 7
    var future = new Date(); // get today date
    future.setDate(future.getDate() - 7); // - 7 days
    var finalDate = future.getFullYear() + '-' + ((future.getMonth() + 1) < 10 ? '0' : '') + (future.getMonth() + 1) + '-' + ((future.getDate() + 1) < 10 ? '0' : '') + (future.getDate());
    document.getElementById('start').value = finalDate;
    //$('#start').val() = finalDate;
    //-end

    $('#start').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });

    $('#end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    }).datepicker('setDate', 'today');
});

function _getFactory() {
    $.get(getFactory, function (data) {
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
function _GetLocationsts() {
    $.get(GetLocationsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> *Location Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#lct_sts_cd").html(html);
        }
    });
}
function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd").html(html);
        }
    });
}
function _GetType() {
    $.get(GetType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_mt_type").html(html);
            $("#mt_type").html(html);
        }
    });
}

$(function () {
    $grid = $("#Material_jqgrid").jqGrid
   ({
       //url: "/MaterialInformation/Getw_mate_list_wip",
       //url: "/MaterialInformation/searchwmaterial_list_wip",
       //datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
           { label: 'Lot Info', name: '', width: 70, align: 'center', formatter: Lot_info },
           { label: 'Use Info', name: '', width: 70, align: 'center', formatter: Use_info },
           { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
           { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
           { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
           { label: 'Type', name: 'mt_type', width: 150, align: 'left' },
           { label: 'Group Qty', name: 'gr_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Status', name: 'status', width: 150, align: 'left' },
           { label: 'Location ', name: 'location', width: 150, align: 'left' },
           { label: 'Location Status', name: 'location_status', width: 150, align: 'left' },
           { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
           { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
           { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
           { label: 'Description', name: 'remark', width: 150, align: 'left' },
           { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       gridComplete: function () {
           $("tr.jqgrow:odd").css("background", "white");
           $("tr.jqgrow:even").css("background", "#f7f7f7");
           $('.loading').hide();
       },
       pager: "#Material_jqgridGridPager",
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: 600,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 50,
       caption: '',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       loadonce: false,
       shrinkToFit: false,
       datatype: function (postData) { getDataOutBox(postData); },
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

//--------------thanhnam-----------------
function getDataOutBox(pdata) {
    $('.loading').show();
    var params = new Object();
    if (jQuery('#Material_jqgrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mt_cd = $("#s_mt_cd").val().trim();
    params.mt_no = $("#c_mt_no").val().trim();
    params.mt_nm = $("#s_mt_nm").val().trim();
    params.mt_type = $("#s_mt_type").val();
    params.mt_sts_cd = $("#mt_sts_cd").val();
    params.lct_cd = $("#c_lct_cd").val();
    params.lct_sts_cd = $("#lct_sts_cd").val();
    params.style_no = $("#style_no").val().trim();
    params.bom_no = $("#bom_no").val().trim();
    params.end = $("#end").val().trim();
    params.start = $("#start").val().trim();
    $("#Material_jqgrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/wipwms/searchwmaterial_list_wip",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#Material_jqgrid")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
}

//report Lot infomation - start
function Lot_info(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-primary"  data-wmtid="' + rowdata.wmtid + '" onclick="Btn_lot_info(this);">LOT INF</button>';
    return html;
}
function Btn_lot_info(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);
    var param = {
        wmtid: $(e).data('wmtid')
    };

    window.open("/wipwms/open_Btn_lot_info?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);
}
//report Lot infomation - end

//report User infomation - start
function Use_info(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-primary"  data-wmtid="' + rowdata.wmtid + '" onclick="Btn_User_info(this);">USE INF</button>';
    return html;
}

function Btn_User_info(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    var param = {
        wmtid: $(e).data('wmtid')
    };
    window.open("/wipwms/open_Btn_use_info?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);
}
//report User infomation - end

$("#searchBtn").click(function () {
    $("#Material_jqgrid").clearGridData();
    $('.loading').show();
    var grid = $("#Material_jqgrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
});


