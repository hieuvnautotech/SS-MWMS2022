$(function () {
    $("#list").jqGrid
    ({
        url: "/fgwms/GetProInfo",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'plno', name: 'plno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Product Code', name: 'style_no', sortable: true, width: '200', align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', sortable: true, width: '250', align: 'left' },
            { key: false, label: 'Qty', name: 'gr_qty', sortable: true, width: '100', align: 'right' },
            { key: false, label: 'Status', name: 'sts_cd', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Location', name: 'lct_cd', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Location Status', name: 'lct_sts_cd', editable: true, width: '100px', align: 'center' },
            //{ key: false, label: 'Po No', name: 'po_no', editable: true, width: '100px', align: 'center' },
            //{ key: false, label: 'Fo No', name: 'fo_no', editable: true, width: '100px', align: 'center' },
            //{ key: false, label: 'Line No', name: 'line_no', editable: true, width: '100px', align: 'center' },
            //{ key: false, label: 'Bom No', name: 'bom_no', editable: true, width: '100px', align: 'center' },
        ],

        pager: jQuery('#gridpager'),
        rowNum: 500,
        rowList: [500, 1000, 2000, 5000, 10000],
        rownumbers: true,
        autowidth: true,
        width: '100%',
        shrinkToFit: false,
        viewrecords: true,
        height: '450',
        loadonce: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },

        multiselect: false,
    })
    $("#searchBtn").click(function () {
            style_no = $("#style_no").val().trim(),
            po_no = $("#po_no").val().trim(),
            fo_no = $("#fo_no").val().trim(),
            bom_no = $("#bom_no").val().trim(),
            start = $("#start").val(),
            end = $("#end").val();

        $.ajax({
            url: "/fgwms/searchProInfo",
            type: "get",
            dataType: "json",
            data: {
                style_no: style_no,
                po_no: po_no,
                fo_no: fo_no,
                bom_no: bom_no,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });
});//grid1

//var getDivision = "/Monitoring/getDivision";
//$(document).ready(function () {
//    _getDivision();
//});
//function _getDivision() {

//    $.get(getDivision, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Lot Division*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
//            });
//            $("#lot_div_cd").html(html);
//        }
//    });
//}