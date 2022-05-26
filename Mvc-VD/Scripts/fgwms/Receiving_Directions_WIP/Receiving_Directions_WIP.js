
$(function () {
    $("#list1").jqGrid
    ({
        url: "/fgwms/getshipinfo_Directions_WIP",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'rid', name: 'rid', width: 30, align: 'center', hidden: true },
             { label: 'RD No', name: 'rd_no', width: 120, align: 'center' },
             { label: 'Destination', name: 'lct_cd', width: 150, align: 'left' },
             { label: 'States', name: 'rd_sts_cd', width: 100, align: 'left' },
             { label: ' Work Date', name: 'worker_dt', width: 150, align: 'center' },
             { label: 'Worker', name: 'worker_id', width: 110, align: 'left' },
             { label: 'Manager', name: 'manager_id', width: 150, align: 'left' },
             { label: 'MT Qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, selected, status, e) {

            var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
            row_id = $("#list1").getRowData(selectedRowId);

            var rid = row_id.rid;
            var rd_no = row_id.rd_no;

            $("#list2").setGridParam({ url: "/fgwms/getReDetails_Directions_WIP?" + "rd_no=" + rd_no, datatype: "json" }).trigger("reloadGrid");
            $("#list3").setGridParam({ url: "/fgwms/getship3_Directions_WIP?" + "rd_no=" + rd_no, datatype: "json" }).trigger("reloadGrid");
        },
        pager: $('#PagegridAu_Mem'),
        pager: "#PagegridAu_Mem",
        rowNum: 10,
        viewrecords: true,
        rowList: [10, 20, 30, 40, 50, 100],
        height: '245',
        caption: '',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
       
        shrinkToFit: false,
        autowidth: true,
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

    })

});

$(function () {
    $("#list2").jqGrid
    ({
        url: "/fgwms/getReDetails_Directions_WIP",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'rdid', name: 'rdid', width: 50, hidden: true },
             { key: false, label: 'RD NO', name: 'rd_no', width: 110, align: 'center' },
             { label: 'MT NO  ', name: 'mt_no', width: 150, align: 'left' },
             { label: 'MT Name ', name: 'mt_nm', width: 150, align: 'left' },
             { label: 'Qty', name: 'd_mt_qty', width: 110, align: 'right' },
             { label: 'Width', name: 'width', width: 110, align: 'right' },
             { label: 'Length (M)', name: 'spec', width: 110, align: 'right' },
        ],
        pager: jQuery('#PageGetJqAuMn'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        loadonce: true,
        height: '220',
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
var wh = "/fgwms/lct_destination";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Location *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#lct_cd").html(html);
        }
    });
}
$(function () {
    $("#list3").jqGrid
    ({
        url: "/fgwms/getship3_Directions_WIP",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'rdmid', name: 'rdmid', width: 50, hidden: true },
             { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
             { label: 'Location', name: 'lct_cd', width: 200, align: 'left' },
             { label: 'Status', name: 'rdm_sts_cd', width: 110, align: 'left' },
             { label: 'Location Status', name: 'lct_sts_cd', width: 110, align: 'left' },
        ],
        pager: "#list3Pager",
        loadonce: true,
        userDataOnFooter: true,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        viewrecords: true,
        emptyrecords: "No data.",
        gridview: true,
        caption: 'Material List',
        loadonce: true,
        rownumbers: true,
        autowidth: true,    
        shrinkToFit: false,
        height: 300,
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

$("#searchBtn").click(function () {
    var rd_no = $('#rd_no').val().trim();
    var lct_cd = $('#lct_cd').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
   
    $.ajax({
        url: "/fgwms/searchRequest_Directions_WIP",
        type: "get",
        dataType: "json",
        data: {

            rd_no: rd_no,
            lct_cd: lct_cd,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#list1").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

