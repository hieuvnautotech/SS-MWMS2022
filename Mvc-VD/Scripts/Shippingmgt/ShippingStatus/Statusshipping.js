$(function () {
    $("#list").jqGrid
    ({
        url: "/Shippingmgt/statusdata",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
           { key: true, label: 'mrid', name: 'mrid', width: 50, hidden: true },
             { label: 'MSR No', name: 'mr_no', width: 110, align: 'center' },
             { label: 'MT No', name: 'mt_no', width: 250, align: 'left' },
                          { label: 'MT CD', name: 'mt_cd', width: 250, align: 'left' },
             { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
             { label: 'Status', name: 'mr_sts_cd', width: 110, align: 'left', hidden: true },
             { label: 'Status', name: 'dt_name', width: 110, align: 'center' },
             { label: 'Requester', name: 'worker_id', width: 110, align: 'left' },
             { label: 'Manager', name: 'manager_id', width: 110, align: 'left' },
             { label: 'Reg Receive Date', name: 'req_rec_dt', width: 110, align: 'center' },
             //{ label: 'Real Receive Date', name: 'real_rec_dt', width: 110, align: 'center' },
             { label: 'MT Qty', name: 'mt_qty', width: 110, align: 'right', formatter: 'integer' },
             { label: 'Relation Bom', name: 'rel_bom', width: 110, align: 'left', formatter: bom_popup  },

            { label: 'Description', name: 'remark', width: 110, align: 'left' },
            { label: 'Creat User', name: 'reg_id', width: 200 },
            { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
              { label: 'Change User', name: 'chg_id', width: 200, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],


     onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    },
       
    pager: "#listPager",
    viewrecords: true,
    rowList: [10, 20, 50, 200, 500],
    height: 300,
    width: $(".box-body").width(),
    autowidth: false,
    caption: '',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: true,
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
       
      

    })

});

 
function bom_popup(cellvalue, options, rowobject) {
    var html = "";
    if (cellvalue != null && cellvalue!="") {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        c = parseInt(b);
        return a+c;
    }
    return html;
}

$("#searchBtn").click(function () {
    var mr_no = $('#mr_no').val().trim();
    var mt_no = $('#mt_no').val().trim();
   
    
    var dt_nm = $('#dt_nm').val().trim();


    $.ajax({
        url: "/Shippingmgt/searchRequest2",
        type: "get",
        dataType: "json",
        data: {

            mr_no: mr_no,
            mt_no: mt_no,
            dt_nm: dt_nm,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});




$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "Purchare Status.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#list").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                fileName: "Purchare Status",
                returnAsString: false
            }
         );
    });

});

var getType = "/Shippingmgt/getType_status";

$(document).ready(function () {
    _getTypeC();

});
function _getTypeC() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#dt_nm").html(html);
        }
    });
}