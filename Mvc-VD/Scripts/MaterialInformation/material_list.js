var getFactory = "/MaterialInformation/Warehouse";
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
    //$('#start').val() - finalDate;
    //-end

    $("#start").datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });

    $("#end").datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    }).datepicker('setDate', 'today');



});

function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location*</option>';
            //html += '<option value=' + data[0].lct_cd + ' selected="selected" >' + data[0].lct_nm + '</option>';
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
            html += '<option value="" selected="selected">*Location Status*</option>';
            //html += '<option value=' + data[0].dt_cd + ' selected="selected" >' + data[0].dt_nm + '</option>';
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
            //html += '<option value=' + data[0].dt_cd + ' selected="selected" >' + data[0].dt_nm + '</option>';
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
            //html += '<option value=' + data[0].dt_cd + ' selected="selected" >' + data[0].dt_nm + '</option>';
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
       datatype: 'json',
       mtype: 'Get',
       colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                { label: 'ML INF', name: '', width: 70, align: 'center', formatter: ML_info },
                { label: 'LOT INF', name: '', width: 70, align: 'center', formatter: Lot_info },
                //{ label: 'USE INF', name: '', width: 70, align: 'center', formatter: Use_info },
                { label: 'ML No', name: 'mt_cd', width: 500, align: 'left' },
                { label: 'Lot NO', name: 'lot_no', width: 150, align: 'left' },
                { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
                { label: 'MT Cd', name: 'mt_cd', width: 250, align: 'left' },
                { label: 'Group Unit', name: 'gr_qty_mt', width: 150, align: 'right', formatter: donvi1 },
                { label: 'Group Qty', name: 'gr_qty', width: 100, align: 'right', formatter: donvi },
                { label: 'Type', name: 'mt_type', width: 150, align: 'left' },
                { label: 'Type', name: 'mt_type', width: 150, align: 'left', hidden: true },
                { label: 'Unit Code', name: 'unit_cd', width: 150, align: 'right', hidden: true },
                { label: 'Status', name: 'status', width: 150, align: 'left' },
                { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
                { label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left', hidden: true },
                { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
                { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
                { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
                { label: 'Description', name: 'remark', width: 150, align: 'left' },
                { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
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
       pager: '#Material_jqgridGridPager',
       viewrecords: true,
       height: 400,
       rowNum: 50,
       rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
       rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
       // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
       loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
       emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
       gridview: true,
       shrinkToFit: false,
       datatype: function (postData) { getData(postData); },
       jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
       ajaxGridOptions: { contentType: "application/json" },
       autowidth: true,
       loadonce: false,

       gridComplete: function () { $('.loading').hide(); },
   });


    function donvi(cellvalue, options, rowobject) {
        var unit_cd = rowobject.unit_cd;
        var html = "";
        if (cellvalue == null) { cellvalue = ""; }
        if (unit_cd == null) { unit_cd = ""; }
        html = cellvalue + " " + unit_cd;
        return html;
    }

    function donvi1(cellvalue, options, rowobject) {
        var unit_cd = rowobject.unit_cd;
        var html = "";
        if (cellvalue == null) { cellvalue = ""; }
        if (unit_cd == null) { unit_cd = ""; }
        html = cellvalue + " " + unit_cd;
        return html;
    }


});



//Mt_cd_popup
function Mt_cd_popup(cellValue, options, rowdata, action) {
    if (rowdata.mt_type == 'CMT') {
        var html = '<button style="color: dodgerblue; border: none; background: none; " data-wmtid="' + rowdata.wmtid + '" onclick="open_Mt_cd_popup(this)">' + cellValue + '</button>';
        return html;
    }
    else {
        var html = rowdata.mt_cd;
        return html;
    }
}
function open_Mt_cd_popup(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);

    var param = {
        wmtid: $(e).data('wmtid')
    };

    window.open("/MaterialInformation/open_Mt_cd_popup_window?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);

}
//Mt_cd_popup
//report Lot infomation
function ML_info(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-primary"  data-wmtid="' + rowdata.wmtid + '" onclick="Btn_ml_info(this);">ML INF</button>';
    return html;
}
function Btn_ml_info(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);

    var param = {
        wmtid: $(e).data('wmtid')
    };

    window.open("/MaterialInformation/open_Btn_ml_info?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1000, height = 450, top=" + top + ", left=" + left);
}

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

    window.open("/MaterialInformation/open_Btn_lot_info?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);
}
//report Lot infomation

//report User infomation
function Use_info(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-primary"  data-wmtid="' + rowdata.wmtid + '" onclick="Btn_User_info(this);">USE INF</button>';
    return html;
}
function Btn_User_info(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);

    var param = {
        wmtid: $(e).data('wmtid')
    };

    window.open("/MaterialInformation/open_Btn_use_info?" +
        "wmtid=" + $(e).data('wmtid'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);
}
//report User infomation


//
$("#searchBtn").click(function () {
    $("#Material_jqgrid").clearGridData();

    $('.loading').show();

    var grid = $("#Material_jqgrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);
});

function getData(pdata) {
    $('.loading').show();
    var mt_no = $("#c_mt_no").val().trim();
    var mt_nm = $("#s_mt_nm").val().trim();
    var mt_type = $("#s_mt_type").val();
    var mt_sts_cd = $("#mt_sts_cd").val();
    var lct_cd = $("#c_lct_cd").val();
    var lct_sts_cd = $("#lct_sts_cd").val();
    var mt_cd = $("#s_mt_cd").val().trim();
    var start = $("#start").val().trim();
    var end = $("#end").val().trim();
    var params = new Object();
    if (jQuery('#Material_jqgrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mt_no = mt_no;
    params.mt_nm = mt_nm;
    params.mt_type = mt_type;
    params.mt_sts_cd = mt_sts_cd;
    params.lct_cd = lct_cd;
    params.lct_sts_cd = lct_sts_cd;
    params.mt_cd = mt_cd;
    params.end = end;
    params.start = start;
    $("#Material_jqgrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/MaterialInformation/searchwmaterial_list",
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
};