

$("#m_save_but").attr("disabled", true);
$("#del_save_but").attr("disabled", true);

$grid = $("#WHlocMgt").jqGrid({
    url: "/StandardMgtWh/GetWHlocMgt",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'ID', name: 'lctno', width: 60, hidden: true },
        { label: 'WareHouse', name: 'index_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 120, align: 'center' },
        { label: 'Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 250, align: 'center' },
        { label: 'Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 250, },
        { label: 'Level', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
        { label: 'shelf Code', name: 'shelf_cd', sortable: true, editable: true, align: 'left' },
        { label: 'Shelf', name: 'sf_yn', width: 70, sortable: true, align: 'center' },
        { label: 'Movement', name: 'mv_yn', width: 70, sortable: true, align: 'center' },
        { label: 'Nothing', name: 'nt_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Real Use', name: 'real_use_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Use Y/N', name: 'use_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Remark', name: 're_mark', width: 200, sortable: true, },
        { label: 'QR Code', name: 'lct_bar_cd', width: 100, sortable: true, },
        { label: 'Create User', name: 'reg_id', sortable: true, width: 70, },
        { label: 'Create Date', width: 150, name: 'reg_dt', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, align: 'center' },
        { label: 'Change User', name: 'chg_id', sortable: true, width: 80, },
        { label: 'Change Date', name: 'chg_dt', width: 150, sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, align: 'center' },
    ],
    gridComplete: function () {
        var rows = $("#WHlocMgt").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var lct_cd = $("#WHlocMgt").getCell(rows[i], "lct_cd");
            var remark = $('#remark_color').val();
            if (lct_cd == remark) {
                $("#WHlocMgt").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#WHlocMgt").jqGrid("getGridParam", 'selrow');
        row_id = $("#WHlocMgt").getRowData(selectedRowId);
        document.getElementById("form1").reset();
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
        $('#m_lct_nm').val(row_id.lct_nm);
        $('#m_lct_cd').val(row_id.lct_cd);
        $("#m_use_yn").val(row_id.use_yn);
        $("#m_real_use_yn").val(row_id.real_use_yn);
        $('#m_re_mark').val(row_id.re_mark);
        $('#m_lctno').val(row_id.lctno);
        $('#c_lctno').val(row_id.lctno);

        if (row_id.is_yn == 'Y') {
            $('#m_is_yn').prop('checked', true);
        }
        if (row_id.is_yn == 'N') {
            $('#m_is_yn').prop('checked', false);
        }

        if (row_id.sf_yn == 'Y') {
            $('#m_sf_yn').prop('checked', true);

        }
        if (row_id.sf_yn == 'N') {
            $('#m_sf_yn').prop('checked', false);

        }
        if (row_id.mv_yn == 'Y') {
            $('#m_mv_yn').prop('checked', true);

        }
        if (row_id.mv_yn == 'N' || row_id.mv_yn == '') {
            $('#m_mv_yn').prop('checked', false);
        }
        if (row_id.mv_yn == 'Y') {
            $('#m_mv_yn').prop('checked', true);

        }
        if (row_id.mv_yn == 'N' || row_id.mv_yn == '') {
            $('#m_mv_yn').prop('checked', false);
        }
        if (row_id.nt_yn == 'Y') {
            $('#m_nt_yn').prop('checked', true);

        }
        if (row_id.nt_yn == 'N' || row_id.nt_yn == '') {
            $('#m_nt_yn').prop('checked', false);
        }
    },
    loadonce: true,
    shrinkToFit: false,
    pager: '#jqGridPager1',
    rownumbers: true,
    caption: 'Warehouse Location Information',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    viewrecords: true,
    height: 360,
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
    width: $(".box-body").width() - 5,
    autowidth: false,   
});

$('#WHlocMgt').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#WHlocMgt").closest(".ui-jqgrid").parent().width();
    $("#WHlocMgt").jqGrid("setGridWidth", newWidth, false);
});

$(document).ready(function () {
    $('input[name=root_yn]').click(function () {
        if ($(this).prop("checked") == true) {
            $("#c_root_yn").val("1");
        }
        else if ($(this).prop("checked") == false) {
            $("#c_root_yn").val("");
        }
    });
});
$("#c_save_but").click(function () {
    if ($('input[name=root_yn]').is(":checked")) {
        if ($("#c_lctno").val() == "") {
            $("#c_lctno").val("1");
        }
    } else {
        var selRowId = $('#WHlocMgt').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top WareHouse on the grid.");
            return false;
        }
    }
    if ($("#c_lct_nm").val().trim() == "") {
        alert("Please enter  Name");
        $("#c_lct_nm").val("");
        $("#c_lct_nm").focus();
        return false;
    }

    else {
        var nt_yn = ($('#c_nt_yn').prop('checked') == true) ? "Y" : "N";
        var mv_yn = ($('#c_mv_yn').prop('checked') == true) ? "Y" : "N";
        var sf_yn = ($('#c_sf_yn').prop('checked') == true) ? "Y" : "N";
        $.ajax({
            url: "/StandardMgtWh/insertlocMgt",
            type: "get",
            dataType: "json",
            data: {
                lct_nm: $('#c_lct_nm').val().trim(),
                use_yn: $('#c_use_yn').val(),
                re_mark: $('#c_re_mark').val().trim(),
                real_use_yn: $('#c_real_use_yn').val(),
                lctno: $('#c_lctno').val(),
                c_root_yn: $('#c_root_yn').val(),
                sf_yn: sf_yn,
                mv_yn: mv_yn,
                nt_yn: nt_yn,
            },
            success: function (data) {
                var id = data.lctno;
                $("#WHlocMgt").jqGrid('addRowData', id, data, 'first');
                $("#WHlocMgt").setRowData(id, false, { background: "#d0e9c6" });
                document.getElementById("form2").reset();
                $("#c_root_yn").val("");
            }

        });
    }
});

$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var nt_yn = ($('#m_nt_yn').prop('checked') == true) ? "Y" : "N";
        var mv_yn = ($('#m_mv_yn').prop('checked') == true) ? "Y" : "N";
        var sf_yn = ($('#m_sf_yn').prop('checked') == true) ? "Y" : "N";
        $.ajax({

            url: "/StandardMgtWh/updatewhlocMgt",
            type: "post",
            dataType: "json",
            data: {
                lct_nm: $('#m_lct_nm').val(),
                use_yn: $('#m_use_yn').val(),
                re_mark: $('#m_re_mark').val(),
                real_use_yn: $('#m_real_use_yn').val(),
                mv_yn: $('#m_mv_yn:checked').val(),
                lctno: $('#m_lctno').val(),
                sf_yn: sf_yn,
                mv_yn: mv_yn,
                nt_yn: nt_yn,
            },
            success: function (data) {
                if (data.result == 1) {
                    $('#remark_color').val(data.lct_cd);
                    jQuery("#WHlocMgt").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                } else {
                    alert('Sorry. The Location was not existing');
                }
            }

        });
    }
});
$("#searchBtn").click(function () {
    $.ajax({
        url: "/StandardMgtWh/SearchWhLocation",
        type: "get",
        dataType: "json",
        data: {
            whouse: $('#whouse').val().toString().trim(),
            aisle: $('#aisle').val().toString().trim(),
            bay: $('#bay').val().toString().trim(),
        },
        success: function (result) {
            $("#WHlocMgt").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});




//$('#c_lct_nm').on("click", function () {
//    document.getElementById("form2").reset();
//    $("#m_save_but").attr("disabled", true);
//    $("#del_save_but").attr("disabled", true);
//});

//get select
// Khai báo URL stand của bạn ở đây
var baseService = "/StandardMgtWH";
var wh = baseService + "/Warehouse";
var aisle = baseService + "/Aisle";
var bay = baseService + "/Bay";

function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Ware house *</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.index_cd + '>' + item.index_cd + '</option>';
            });
            $("#whouse").html(html);
        }
    });
}
function _getaisle() {
    $.get(aisle, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> *Aisle*</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_cd + '</option>';
            });
            $("#aisle").html(html);
        }
    });
}
function _getbay() {
    $.get(bay, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> *Bay*</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_cd + '</option>';
            });
            $("#bay").html(html);
        }
    });
}
_getwhouse();
_getaisle();
_getbay();

$("#PrintBtn").click(function () {
    var fruits = [];
    var i, selRowIds = $('#WHlocMgt').jqGrid("getGridParam", "selarrrow"), n, rowData;
    for (i = 0, n = selRowIds.length; i < n; i++) {
        var ret = $("#WHlocMgt").jqGrid('getRowData', selRowIds[i]);
        var lctno = ret.lctno
        fruits.push(lctno);
    };
    window.open("/StandardMgtWh/PrintWahouse?" + "id=" + fruits, "PRINT", "width=500, height=800, left=0, top=100, location=no, status=no,")
});

$('#htmlBtn').click(function (e) {
    $("#WHlocMgt").jqGrid('exportToHtml',
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
            fileName: "Warehouse Location Information  ",
            returnAsString: false
        }
    );
});

$('#excelBtn').click(function (e) {
    $("#WHlocMgt").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Warehouse Location Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
});

$("#form1").validate({
    rules: {
        "lct_nm": {
            required: true,

        },
    },
});
$("#form2").validate({
    rules: {
        "lct_nm": {
            required: true,

        },
    },
});

