$("#picMGrid").jqGrid({
    url: "/ShippingMgt/getpicM",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 80, align: 'center', hidden: true },
        { key: false, label: 'Material code', name: 'mt_cd', sortable: true, width: '200', align: 'left' },
        { key: false, label: 'Material Name', name: 'mt_nm', editable: true, width: '100', align: 'center' },
        { key: false, label: 'State', name: 'mt_sts_cd', editable: true, width: '100px', align: 'center' },
        { key: false, label: 'Location', name: 'lct_cd', editable: true, width: '200', align: 'center' },
        { key: false, label: 'Location State', name: 'lct_sts_cd', editable: true, width: '180', align: 'center' },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });    
    },
    pager: "#picMGridPager",
    viewrecords: true,
    rowList: [20, 50, 200, 500],
    height: 450,
    width: $(".box-body").width() - 5,
    autowidth: false,
    rowNum: 20,
    caption: 'Picking (M)',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    shrinkToFit: false,
    loadonce: true,
    viewrecords: true,
    multiselect: true,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
}),

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#picMGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#picMGrid").closest(".ui-jqgrid").parent().width();
    $("#picMGrid").jqGrid("setGridWidth", newWidth, false);
});


function fun_check1() {

    var selRowIds = $("#picMGrid").jqGrid("getGridParam", 'selarrrow');


    if ($("#m_lct_cd").val().trim() == "") {
        alert("Please enter the Location.");
        $("#m_lct_cd").val("");
        $("#m_lct_cd").focus();
        return false;
    }
    if (selRowIds.length == 0) {
        alert("Please select Grid.");
        return false;
    }
    return true;
}


$("#m_save_but").on("click", function () {

    var i, selRowIds = $('#picMGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;

    var lct_cd = $('#m_lct_cd').val();
    var sd_no = $('#c_sd_no').val();
    if (fun_check1() == true) {

        $.ajax({
            type: "post",
            dataType: 'text',
            url: "/ShippingMgt/updatepicM?id=" + selRowIds,
            //url: "/ReceivingMgt/updaterecAppr?id=" 
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            data: JSON.stringify({
                lct_cd: lct_cd,
                sd_no: sd_no
            }),
            success: function (result) {

                //$("#picMGrid").jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                jQuery("#picMGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });
    }
})

//$("#getPutin").click(function () {

//    var shelf_cd = $('#shelf_cd').val();
//    $.ajax({
//        url: "/ReceivingMgt/getPutin",
//        type: "get",
//        dataType: "json",
//        data: {
//            shelf_cd: shelf_cd,
//        },
//        success: function (result) {
//            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });
//});

$("#searchBtn").click(function () {
    var mr_no = $('#mr_no').val().trim();
    var mt_no = $('#mt_no').val().trim();
    var mt_barcode = $('#mt_barcode').val().trim();
    var start = $('#start').val();
    var end = $('#end').val();
    $.ajax({
        url: "/ShippingMgt/searchpicM",
        type: "get",
        dataType: "json",
        data: {
            mr_no: mr_no,
            mt_no: mt_no,
            mt_barcode: mt_barcode,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#picMGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});


$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'Picking (M)',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'Picking (M)',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'Picking (M)',
            escape: false,
            headers: true,
        });
    });
});



$("#start").datepicker({
    format: 'yyyy-mm-dd',
});

$("#end").datepicker({
    dateFormat: 'yyyy-mm-dd'
});

var getType = "/ShippingMgt/getType";

$(document).ready(function () {
    _getTypeS();
    $("#m_lct_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeS() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#m_lct_cd").html(html);
        }
    });
}
