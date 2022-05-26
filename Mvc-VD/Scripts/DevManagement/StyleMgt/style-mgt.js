$("#list").jqGrid
({
    url: "/DevManagement/GetStyleMgt",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
            { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Code', name: 'style_no', width: 80, align: 'center' },
            { key: false, label: 'Mode code', name: 'md_cd', sortable: true, width: '230', align: 'center', align: 'left' },
            { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200', align: 'left' },
            { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180', align: 'left' },
            { key: false, label: 'Description', name: 'cav', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Packing Amount(EA)', name: 'pack_amt', editable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Type', name: 'bom_type', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'TDS no', name: 'tds_no', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'use_yn', name: 'use_yn', editable: true, width: '100px', hidden: true },
            { key: false, label: 'del_yn', name: 'del_yn', editable: true, width: '100px', hidden: true },
            { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', width: '100px', align: 'left' },
            {
                key: false, label: 'Create Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200'
            },
            { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Change Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    gridComplete: function () {
        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var style_no = $("#list").getCell(rows[i], "style_no");
            var giatri = $('#remark_color').val();
            if (style_no == giatri) {
                $("#list").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, selected, status, e) {
        debugger;
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);

        $('#m_sid').val(row_id.sid);
        $('#m_style_no').val(row_id.style_no);
        $('#m_md_cd').val(row_id.md_cd);
        $('#m_prj_nm').val(row_id.prj_nm);
        $('#m_ssver').val(row_id.ssver);
        $('#m_part_nm').val(row_id.part_nm);
        $('#m_standard').val(row_id.standard);
        $('#m_cust_rev').val(row_id.cust_rev);
        $('#m_order_num').val(row_id.order_num);
        $('#m_pack_amt').val(row_id.pack_amt);
        $('#m_cav').val(row_id.cav);
        $('#m_bom_type').val(row_id.bom_type);
        $('#m_tds_no').val(row_id.tds_no);


        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);

        if (row_id != null) {
            $("#dm_save_but").attr("disabled", true);
        }
    },

    pager: jQuery('#gridpager'),
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    height: 300,
    width: $(".box-body").width(),
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    caption: "Style Information",
    sortable: true,
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
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {
    if ($('#form1').valid() == true) {
        $.ajax({
            url: "/DevManagement/CreateStyleMgt",
            type: "get",
            dataType: "json",
            data: {
                style_no: $('#c_style_no').val().toUpperCase(),
                md_cd: $('#c_md_cd').val(),
                prj_nm:$('#c_prj_nm').val(),
                ssver: $('#c_ssver').val(),
                part_nm: $('#c_part_nm').val(),
                standard: $("#c_standard").val(),
                cust_rev: $("#c_cust_rev").val(),
                order_num: $("#c_order_num").val(),
                pack_amt: $("#c_pack_amt").val(),
                cav: $("#c_cav").val(),
                bom_type: $("#c_bom_type").val(),
                tds_no: $("#c_tds_no").val(),
            },
            success: function (data) {
                if (data.result == 0) {
                    var c_style_no = $('#c_style_no').val();
                    $('#remark_color').val(c_style_no.toUpperCase());
                    jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("Style Information was existing. Please checking again !");
                }
            },
            error: function (data) {
                alert('The Code is the same. Please check again.');
            }
        });
    }
});
$("#m_save_but").click(function () {
    if ($('#form2').valid() == true) {
        $.ajax({
            type: "PUT",
            dataType: "json",
            url: "/DevManagement/ModifyStyleMgt",
            data: {
                sid: $('#m_sid').val(),
                style_no: $('#m_style_no').val(),
                md_cd: $('#m_md_cd').val(),
                prj_nm: $('#m_prj_nm').val(),
                ssver: $('#m_ssver').val(),
                part_nm: $('#m_part_nm').val(),
                standard: $("#m_standard").val(),
                cust_rev: $("#m_cust_rev").val(),
                order_num: $("#m_order_num").val(),
                pack_amt: ($("#m_pack_amt").val() == "") ? 0 : $("#m_pack_amt").val(),
                cav: $("#m_cav").val(),
                bom_type: $("#m_bom_type").val(),
                tds_no: $("#m_tds_no").val(),
            },
            success: function (data) {
                if (data.result == true) {
                    var m_style_no = $('#m_style_no').val();
                    $('#remark_color').val(m_style_no.toUpperCase());
                    jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#searchBtn").click(function () {
    var style_no = $('#style_no').val().trim();
    var md_cd = $('#md_cd').val().trim();
    var prj_nm = $('#prj_nm').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    $.ajax({
        url: "/DevManagement/searchStyle",
        type: "get",
        dataType: "json",
        data: {
            style_no: style_no,
            md_cd: md_cd,
            prj_nm: prj_nm,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
        }
    });

});

function image_logo(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}
function bntCellValue(cellValue, options, rowdata, action) {
    id_delete = rowdata.sid;
    var html = '<button class="btn btn-xs btn-danger delete-btn" title="Delete" data-id_delete="' + rowdata.sid + '" >X</button>';
    return html;
};
$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event) {

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
    $('#excelBtn').click(function (e) {
        $("#list").jqGrid('exportToExcel',
                options = {
                    includeLabels: true,
                    includeGroupHeader: true,
                    includeFooter: true,
                    fileName: "StyleMgt.xlsx",
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
                fileName: "StyleMgt",
                returnAsString: false
            }
            );
    });

});
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#list").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "StyleMgt.pdf",
                mimetype: "application/pdf"
            }
            );
    });

});


// Khai báo URL stand của bạn ở đây
var baseService = "/DevManagement";
var GetModelcode = baseService + "/GetModelcode";
var GetSSVer = baseService + "/GetSSVer";
var GetParkNm = baseService + "/GetParkNm";
var GetStandard = baseService + "/GetStandard";
var GetCustRev = baseService + "/GetCustRev";
var GetOrder = baseService + "/GetOrder";
var GetType = baseService + "/GetType";
var GetTdsno = baseService + "/GetTdsno";

var GetModelcodeMD = baseService + "/GetModelcode";
var GetSSVerMD = baseService + "/GetSSVer";
var GetParkNmMD = baseService + "/GetParkNm";
var GetStandardMD = baseService + "/GetStandard";
var GetCustRevMD = baseService + "/GetCustRev";
var GetOrderMD = baseService + "/GetOrder";
var GetTypeMD = baseService + "/GetType";
var GetTdsnoMD = baseService + "/GetTdsno";
//Create
//Model code - DEV001
$(document).ready(function () {
    _GetModelcode();
    $("#c_md_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetModelcode() {

    $.get(GetModelcode, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Model code*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_md_cd").html(html);
        }
    });
}
//End Model code
//SSVer - DEV002
$(document).ready(function () {
    _GetSSVer();
    $("#c_ssver").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetSSVer() {

    $.get(GetSSVer, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*SS Version*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_ssver").html(html);
        }
    });
}
//End SSVer
//Park Name - DEV003
$(document).ready(function () {
    _GetParkNm();
    $("#c_part_nm").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetParkNm() {

    $.get(GetParkNm, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Part Name*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_part_nm").html(html);
        }
    });
}
//End Park Name
//Standard - DEV004
$(document).ready(function () {
    _GetStandard();
    $("#c_standard").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetStandard() {

    $.get(GetStandard, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Standard*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_standard").html(html);
        }
    });
}
//End Standard
//Customer Rev - DEV005
$(document).ready(function () {
    _GetCustRev();
    $("#c_cust_rev").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetCustRev() {

    $.get(GetCustRev, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Customer Rev*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_cust_rev").html(html);
        }
    });
}
//End Customer Rev
//Order Number - DEV006
$(document).ready(function () {
    _GetOrder();
    $("#c_order_num").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetOrder() {

    $.get(GetOrder, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Order Number*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_order_num").html(html);
        }
    });
}
//End Order Number
//Type - DEV007
$(document).ready(function () {
    _GetType();
    $("#c_bom_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetType() {

    $.get(GetType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_bom_type").html(html);
        }
    });
}
//End Type
//Type - DEV008
$(document).ready(function () {
    _GetTdsno();
    $("#c_tds_no").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetTdsno() {

    $.get(GetTdsno, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*TDS No*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#c_tds_no").html(html);
        }
    });
}
//End Type
//.!---------End Create-------.!/

//Modify
//Model code - DEV001
$(document).ready(function () {
    _GetModelcodeMD();
    $("#m_md_cd").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetModelcodeMD() {

    $.get(GetModelcodeMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Model code*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_md_cd").html(html);
        }
    });
}
//End Model code
//SSVer - DEV002
$(document).ready(function () {
    _GetSSVerMD();
    $("#m_ssver").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetSSVerMD() {

    $.get(GetSSVerMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*SS Version*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_ssver").html(html);
        }
    });
}
//End SSVer
//Park Name - DEV003
$(document).ready(function () {
    _GetParkNmMD();
    $("#m_part_nm").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetParkNmMD() {

    $.get(GetParkNmMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Part Name*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_part_nm").html(html);
        }
    });
}
//End Park Name
//Standard - DEV004
$(document).ready(function () {
    _GetStandardMD();
    $("#m_standard").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetStandardMD() {

    $.get(GetStandardMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Standard*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_standard").html(html);
        }
    });
}
//End Standard
//Customer Rev - DEV005
$(document).ready(function () {
    _GetCustRevMD();
    $("#m_cust_rev").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetCustRevMD() {

    $.get(GetCustRevMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Customer Rev*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_cust_rev").html(html);
        }
    });
}
//End Customer Rev
//Order Number - DEV006
$(document).ready(function () {
    _GetOrderMD();
    $("#m_order_num").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetOrderMD() {

    $.get(GetOrderMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Order Number*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_order_num").html(html);
        }
    });
}
//End Order Number
//Type - DEV007
$(document).ready(function () {
    _GetTypeMD();
    $("#m_bom_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetTypeMD() {

    $.get(GetTypeMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_bom_type").html(html);
        }
    });
}
//End Type
//Type - DEV008
$(document).ready(function () {
    _GetTdsnoMD();
    $("#m_tds_no").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetTdsnoMD() {

    $.get(GetTdsnoMD, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*TDS No*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#m_tds_no").html(html);
        }
    });
}
//End Type
//.!---------End Modify-------.!/

function setName_link(cellvalue, options, rowdata, action) {

    if (cellvalue != null) {
        var html = '<a href="/DevManagement/Bom_no?bid=' + rowdata.bid + '" style="color:red" target="_sub">' + cellvalue + '</a>'  ;
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid

$("#form1").validate({
    rules: {
        "style_no": {
            required: true,
        },
        "m_md_cd": {
            required: true,
        },
        "prj_nm": {
            required: true,
        },
        "pack_amt": {
            digits: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "style_no": {
            required: true,
        },
        "m_md_cd": {
            required: true,
        },
        "prj_nm": {
            required: true,
        },
        "pack_amt": {
            digits: true,
        },
    },
});
