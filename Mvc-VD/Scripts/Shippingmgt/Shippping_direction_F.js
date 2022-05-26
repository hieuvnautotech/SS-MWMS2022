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

    $("#list1").jqGrid
    ({
        url: "/ShippingMgt/getshipinfo_Directions_F",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'psid', name: 'psid', width: 30, align: 'center', hidden: true },
             { label: 'PSD No', name: 'psd_no', width: 120, align: 'center' },
             { label: 'Destination', name: 'dest_lct_cd', width: 150, align: 'left' },
             { label: 'Status', name: 'sd_sts_cd', width: 100, align: 'left' },
             { label: ' Work Date', name: 'work_dt', width: 150, align: 'center', formatter: formatdate },
             { label: 'Worker', name: 'worker_id', width: 110, align: 'left' },
             { label: 'Manager', name: 'manager_id', width: 150, align: 'left' },
             { label: 'PSD CNT', name: 'psd_qty', width: 100, align: 'right', formatter: 'integer' },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, selected, status, e) {

            var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
            row_id = $("#list1").getRowData(selectedRowId);

            var rid = row_id.psid;
            var psd_no = row_id.psd_no;

            $("#list2").setGridParam({ url: "/ShippingMgt/getReDetails_Directions_F?" + "psd_no=" + psd_no, datatype: "json" }).trigger("reloadGrid");
            $("#list3").setGridParam({ url: "/ShippingMgt/getship3_Directions_F?" + "psd_no=" + psd_no, datatype: "json" }).trigger("reloadGrid");
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
        url: "/ShippingMgt/getReDetails_Directions_F",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'psdid', name: 'psdid', width: 50, hidden: true },
             { label: 'PSD NO  ', name: 'psd_no', width: 150, align: 'left' },
             { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
             { label: 'Product Name', name: 'style_nm', width: 110, align: 'right' },
             { label: 'Delivery', name: 'req_qty', width: 110, align: 'right' },
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
var wh = "/ShippingMgt/lct_005";
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
            $("#lct_cd").html(html);
        }
    });
}
$(function () {
    $("#list3").jqGrid
    ({
        url: "/ShippingMgt/getship3_Directions_F",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'psdpid', name: 'psdpid', width: 50, hidden: true },
             { label: 'PSD NO', name: 'psd_no', width: 110, align: 'left' },
             { label: 'Product Lot Code', name: 'psdp_no', width: 110, align: 'left' },
             { label: 'Finish Lot', name: 'prd_lcd', width: 400, align: 'left' },
             { label: 'Group Qty', name: 'gr_qty', width: 110, align: 'right', formatter: 'integer' },
           
             { label: 'Product No', name: 'style_no', width: 150, align: 'left' },
             { label: 'Product Name', name: 'style_nm', width: 110 },
             { label: 'Status', name: 'psdp_sts_cd', width: 110 },

             { label: 'Location', name: 'lct_cd', width: 110 },
             { label: 'Location Status', name: 'lct_sts_cd', width: 110, align: 'left', hidden: true },
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
    var psd_no = $('#psd_no').val().trim();
    var lct_cd = $('#lct_cd').val();
    var worker_id = $('#worker_id').val();
    var style_no = $('#style_no').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
   
    $.ajax({
        url: "/ShippingMgt/searchRequest_Directions_F",
        type: "get",
        dataType: "json",
        data: {
            psd_no: psd_no,
            worker_id: worker_id,
            style_no: style_no,
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

