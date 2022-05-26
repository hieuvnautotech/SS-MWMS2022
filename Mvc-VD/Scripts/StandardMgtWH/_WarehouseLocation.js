

$(function () {
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
            { label: 'Real use Y/N', name: 'real_use_yn', sortable: true, width: 60, align: 'center' },
            { label: 'use Y/N', name: 'use_yn', sortable: true, width: 60, },
            { label: 'Remark', name: 're_mark', width: 100, sortable: true, },
            { label: 'lct_bar_cd', name: 'lct_bar_cd', width: 100, sortable: true, },
            { label: 'Creat User', name: 'reg_id', sortable: true, width: 70, },
            { label: 'Creat Date', width: 110, name: 'reg_dt', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
            { label: 'Change User', name: 'chg_id', sortable: true, width: 80, },
            { label: 'Change Date', name: 'chg_dt', width: 110, sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
        ],
        loadonce: true,
        shrinkToFit: false,
        pager: '#jqGridPager1',
        rownumbers: true,
        caption: 'Warehouse Location Information',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rowList: [60, 80, 100, 120],
        viewrecords: true,
        height: 360,
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
        onSelectRow: function (rowid, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#WHlocMgt").jqGrid("getGridParam", 'selrow');
            row_id = $("#WHlocMgt").getRowData(selectedRowId);
            document.getElementById("form2").reset();
            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
            $('#m_lct_nm').val(row_id.lct_nm);
            $("#m_use_yn").val(row_id.use_yn);
            $('#m_re_mark').val(row_id.re_mark);
            $('#m_lctno').val(row_id.lctno);
            $('#c_lctno').val(row_id.lctno);


        },
    });

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#WHlocMgt').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#WHlocMgt").closest(".ui-jqgrid").parent().width();
        $("#WHlocMgt").jqGrid("setGridWidth", newWidth, false);
    });

    $("#locMgt").jqGrid('navGrid', '#jqGridPager1', { edit: false, add: false, del: false })

    $("#c_save_but").click(function () {
        var selRowId = $('#WHlocMgt').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top WareHouse on the grid.");
            return false;
        }
        if ($("#c_lct_nm").val().trim() == "") {
            alert("Please enter  Name");
            $("#c_lct_nm").val("");
            $("#c_lct_nm").focus();
            return false;
        }

        if ($("#c_re_mark").val() == "") {
            alert("Please enter Remark");
            $("#c_re_mark").val("");
            $("#c_re_mark").focus();
            return false;
        } else {
            $.ajax({
                url: "/StandardMgtWh/insertlocMgt",
                type: "post",
                dataType: "json",
                data: {
                    lct_nm: $('#c_lct_nm').val(),
                    use_yn: $('#c_use_yn').val(),
                    re_mark: $('#c_re_mark').val(),
                    real_use_yn: $('#c_real_use_yn').val(),
                    lctno: $('#c_lctno').val(),
                },
                success: function (data) {
                    jQuery("#WHlocMgt").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form2").reset();
                }

            });
        }
    });
    $("#m_save_but").click(function () {
        $.ajax({
            url: "/StandardMgtWh/updatewhlocMgt",
            type: "get",
            dataType: "json",
            data: {
                lct_nm: $('#m_lct_nm').val(),
                use_yn: $('#m_use_yn').val(),
                re_mark: $('#m_re_mark').val(),
                real_use_yn: $('#m_real_use_yn').val(),
                lctno: $('#m_lctno').val(),
            },
            success: function (data) {
                jQuery("#WHlocMgt").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
                document.getElementById("form1").reset();
            }
        });
    });
    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });
});
$("#searchBtn").click(function () {
    $.ajax({
        url: "/StandardMgtWh/SearchWhLocation",
        type: "get",
        dataType: "json",
        data: {
            whouse: $('#whouse').val().trim(),
            aisle: $('#aisle').val().trim(),
            bay: $('#bay').val().trim(),
        },
        success: function (result) {
            $("#WHlocMgt").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

$('#c_lct_nm').on("click", function () {
    document.getElementById("form2").reset();
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
});

//get select
// Khai báo URL stand của bạn ở đây
var baseService = "/StandardMgtWh";
var wh = baseService + "/Warehouse";
var aisle = baseService + "/Aisle";
var bay = baseService + "/Bay";
$(document).ready(function () {
    _getwhouse();
    _getaisle();
    _getbay();
});
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

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'pdf',
            pdfFontSize: '7',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'xls',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'doc',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
