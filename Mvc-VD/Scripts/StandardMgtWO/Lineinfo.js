$("#list").jqGrid
({
    url: "/StandardMgtWO/GetLineInfo",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
            { key: true, label: 'mid', name: 'mid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Line', name: 'line_no', width: 80, align: 'center' },
            {
              key: false, label: 'Line Name', name: 'line_nm', sortable: true, width: '100px', align: 'center', align: 'left',
            },
            { key: false, label: 'Line Explantion', name: 'line_exp', sortable: true, width: '200' },
            { key: false, label: 'Re Mark', name: 're_mark', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Line Color', name: 'line_color', editable: true, width: '150', align: 'left', formatter: rgb_color_col },
            { key: false, label: 'Line Color', name: 'line_color', editable: true, width: '150', align: 'left', hidden: true },
            { key: false, label: 'Use Y/n', name: 'use_yn', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Mail Y/n', name: 'mail_yn', editable: true, width: '100px', align: 'center', hidden: true },
            { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', width: '100px' },
            {
                key: false, label: 'Create Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "d-m-Y H:i:s" }, width: '200'
            },
            { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px' },
            { key: false, label: 'Change Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "d-m-Y H:i:s" }, width: '200' }

    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);

        $('#m_mid').val(row_id.mid);
        $('#m_line_no').val(row_id.line_no);
        $('#m_line_nm').val(row_id.line_nm);
        $('#m_line_exp').val(row_id.line_exp);
        $('#m_re_mark').val(row_id.re_mark);
        $('#m_use_yn').val(row_id.use_yn);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
        $('#m_line_color').val(row_id.line_color);
        $('.input-group-addon > i').css('background-color', row_id.line_color);
    },
    pager: '#gridpager',
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    shrinkToFit: false,
    autowidth: false,
    viewrecords: true,
    height: 400,
    width: $(".box-body").width() - 5,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    loadonce: true,
    multiselect: false,
});

function rgb_color_col(cellValue, options, rowdata, action) {
    var html = cellValue + '&nbsp' + '<button class="btn btn-xs" style="float: right; border: 1px solid silver;background-color:' + cellValue + ';color:' + cellValue + '">color</button>';
    return html;
};

$(".my-colorpicker2").colorpicker();

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});
$("#c_save_but").click(function () {
    if ($("#c_line_nm").val().trim() == "") {
        alert("Please enter your line name");
        $("#c_line_nm").val("");
        $("#c_line_nm").focus();
        return false;
    }
    if ($("#c_line_exp").val().trim() == "") {
        alert("Please enter your line Explantion");
        $("#c_line_exp").val("");
        $("#c_line_exp").focus();
        return false;
    }
            
    if ($("#c_use_yn").val().trim() == "") {
        alert("Please enter your use y/n");
        $("#c_use_yn").val("");
        $("#c_use_yn").focus();
        return false;
    }
    if ($("#c_re_mark").val().trim() == "") {
        alert("Please enter your re mark");
        $("#c_re_mark").val("");
        $("#c_re_mark").focus();
        return false;
    }
    else {
        $.ajax({
            url: "/StandardMgtWO/CreateLineinfo",
            type: "get",
            dataType: "json",
            data: {
                line_nm: $('#c_line_nm').val(),
                line_exp: $('#c_line_exp').val(),
                use_yn: $('#c_use_yn').val(),
                re_mark: $('#c_re_mark').val(),
                line_color: $('#c_line_color').val(),
            },
            success: function (data) {
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            },
        });
    }
});
$("#m_save_but").click(function () {
    if ($("#m_line_nm").val().trim() == "") {
        alert("Please enter your line name");
        $("#m_line_nm").val("");
        $("#m_line_nm").focus();
        return false;
    }
    if ($("#m_line_exp").val().trim() == "") {
        alert("Please enter your line Explantion");
        $("#m_line_exp").val("");
        $("#m_line_exp").focus();
        return false;
    }


    if ($("#m_use_yn").val().trim() == "") {
        alert("Please enter your use y/n");
        $("#m_use_yn").val("");
        $("#m_use_yn").focus();
        return false;
    }
    if ($("#m_re_mark").val().trim() == "") {
        alert("Please enter your re mark");
        $("#m_re_mark").val("");
        $("#m_re_mark").focus();
        return false;
    }
    $.ajax({
        type: "get",
        dataType: "json",
        url: "/StandardMgtWO/ModifyLineinfo",
        data: {
            mid: $('#m_mid').val(),
            line_nm: $('#m_line_nm').val(),
            line_exp: $('#m_line_exp').val(),
            use_yn: $('#m_use_yn').val(),
            re_mark: $('#m_re_mark').val(),
            line_color: $('#m_line_color').val(),
        },
        success: function (data) {
            jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        }
    });

});
$("#searchBtn").click(function () {
    //var line_cd = $('#line_cd').val().trim();
    //var line_nm = $('#line_nm').val().trim();

    var line_no = $('#line_no').val().trim();
    var line_nm = $('#line_nm').val().trim();
    $.ajax({
        url: "/StandardMgtWO/searchLine",
        type: "get",
        dataType: "json",
        data: {
            line_no: line_no,
            line_nm: line_nm,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
        }
    });

});

$("#tab_1").on("click", "a", function (event) {
    $("#list").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event) {
    $("#list").trigger('reloadGrid');
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
});


$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            bootstrap: true,
            pdfFontSize: '12',
            fileName: 'Style_Mgt',
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
            bootstrap: true,
            fileName: 'Style_Mgt',
            escape: false,
            headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            bootstrap: true,
            fileName: 'Style_Mgt',
            escape: false,
            headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
            footers: true,
        });
    });
});

