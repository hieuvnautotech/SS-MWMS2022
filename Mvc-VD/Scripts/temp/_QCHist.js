    $("#qcHistGrid").jqGrid
    ({
        url: "/ProducePlan/getProductQCHist",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'qcno', name: 'qcno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Line Code', name: 'line_cd', width: 80, align: 'center' },
            { key: false, label: 'Line Name', name: 'line_nm', width: 150, align: 'left' },
            { key: false, label: 'Product Date', name: 'product_dt', width: 110, align: 'center' },
            { key: false, label: 'FO NO', name: 'fo_no', width: 120, align: 'center' },
            { key: false, label: 'Weight', name: 'w_val', sortable: true, width: '150', align: 'right' },
            { key: false, label: 'Length', name: 'l_val', sortable: true, width: '150', align: 'right' },
            { key: false, label: 'QC1', name: 'qc_01', width: '100', align: 'center' },
            { key: false, label: 'QC2', name: 'qc_02', width: '100', align: 'center' },
            { key: false, label: 'QC3', name: 'qc_0', width: '100', align: 'center' },
            { key: false, label: 'QC3', name: 'qc_0', width: '100', align: 'center' },
            { key: false, label: 'Photo', name: 'photo_file', width: 120, align: 'center', formatter: image_logo },
            { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
            { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],

        pager: "#qcHistGridPager",
        pager: jQuery('#qcHistGridPager'),
        viewrecords: true,
        rowList: [50, 200, 500],
        height: 500,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: 'QC History',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        multiselect: false,
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

    })
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#qcHistGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#qcHistGrid").closest(".ui-jqgrid").parent().width();
        $("#qcHistGrid").jqGrid("setGridWidth", newWidth, false);
    });



$("#searchBtn").click(function () {
    var line_cd = $('#s_line_cd').val().trim();
    var line_nm = $('#s_line_nm').val().trim();
    var fo_no = $('#s_fo_no').val();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    $.ajax({
        url: "/ProductPlan/searchQCHist",
        type: "get",
        dataType: "json",
        data: {
            line_cd: line_cd,
            line_nm: line_nm,
            fo_no: fo_no,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#qcHistGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

function image_logo(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
        return html;
    }
    else {
        return "";
    }
}


var getType = "/ProducePlan/getFoNo";

$(document).ready(function () {
    _getTypeS();
    $("#s_fo_no").on('change', function () {
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
                html += '<option value=' + item.fo_no + '>' + item.fo_no + '</option>';
            });
            $("#s_fo_no").html(html);
        }
    });
}
$("#start").datepicker({
    format: 'yyyy-mm-dd',
});

$("#end").datepicker({
    format: 'yyyy-mm-dd',
});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'QCHistory',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            type: 'xls',
            fileName: 'QCHistory',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'QCHistory',
            escape: false,
        });
    });
});