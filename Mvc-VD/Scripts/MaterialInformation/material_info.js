var getFactory = "/MaterialInformation/Warehouse";
var GetType = "/MaterialInformation/GetType";
var GetLocationsts = "/MaterialInformation/GetLocationsts";
var GetStatus = "/MaterialInformation/GetStatus";
$(document).ready(function () {
    _getFactory();
    _GetType();
    _GetLocationsts();
    _GetStatus();
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
};
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
};
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
};
function _GetType() {
    $.get(GetType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_mt_type").html(html);
        }
    });
};
$(function () {
    var grid = $('#Material_jqgrid');
    grid.jqGrid
   ({
       //url: "/MaterialInformation/Getw_mate_info",
       //url: '@Url.Action("searchwmaterial_info", "MaterialInformation")',
       //datatype: 'local',
       contentType: "application/json; charset-utf-8",
       mtype: 'GET',
       colNames: ['wmtid', 'MT NO', 'MT Cd', 'MT Name', 'Type', 'Group Unit', 'Total Qty', 'Group Qty', 'Group Qty', 'Status', 'Location', 'Location Status'],
       colModel: [
                { key: true, name: 'wmtid', width: 50, align: 'right', hidden: true },
                { name: 'mt_no', width: 250, align: 'left' },
                { name: 'mt_cd', width: 450, align: 'left' },
                { name: 'mt_nm', width: 250, align: 'left' },
                { name: 'mt_type', width: 150, align: 'left' },
                { name: 'gr_qty_mt', width: 100, align: 'right' },
                { name: 'total', width: 150, align: 'right', formatter: 'integer' },
                { name: 'gr_qty', width: 150, align: 'right'/*, formatter: donvi*/ },


                //{ name: 'unit_cd', width: 150, align: 'right', hidden: true },
                { name: 'gr_qty', width: 150, align: 'right', hidden: true },

                { name: 'mt_sts_cd', width: 150, align: 'left' },
                { name: 'lct_nm', width: 150, align: 'left' },
                { name: 'lct_sts_cd', width: 150, align: 'left' },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },

       gridComplete: function () {
           $('.loading').hide()
       },

       pager: $('#Material_jqgridGridPager'),
       //pginput: true,
       rowNum: 50,
       rowList: [50, 100, 200, 500, 1000],
       viewrecords: true,
       height: 450,
       //autoheight: true,
       autowidth: true,
       rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
       //한 번에 보여줄 레코드 수를 변동할 때 선택 값
       // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
       loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
       emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
       datatype: function (postData) { getData(postData); },
       gridview: true,
       shrinkToFit: false,
       loadonce: false,
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },

       //ajaxGridOptions: { contentType: "application/json" },

       //recordtext: 'Total Records',
       //pgtext: 'on',

   });
    //$("#Material_jqgrid").jqGrid('navGrid', '#Material_jqgridGridPager', { edit: false, add: false, del: false });




});

function getData(pdata) {
    $('.loading').show();

    var params = new Object();
    if (jQuery('#Material_jqgrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mt_no = $("#c_mt_no").val().trim();
    params.mt_nm = $("#s_mt_nm").val().trim();
    params.mt_type = $("#s_mt_type").val();
    params.mt_sts_cd = $("#mt_sts_cd").val();
    params.lct_cd = $("#c_lct_cd").val();
    params.lct_sts_cd = $("#lct_sts_cd").val();
    $("#Material_jqgrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/MaterialInformation/searchwmaterial_info",
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

function donvi(cellvalue, options, rowobject) {
    var unit_cd = rowobject.unit_cd;
    var html = "";
    if (cellvalue == null) { cellvalue = ""; }
    if (unit_cd == null) { unit_cd = ""; }
    html = cellvalue + " " + unit_cd;
    return html;
};

$("#searchBtn").click(function () {
    $("#Material_jqgrid").clearGridData();

    $('.loading').show();

    var grid = $("#Material_jqgrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);

});
