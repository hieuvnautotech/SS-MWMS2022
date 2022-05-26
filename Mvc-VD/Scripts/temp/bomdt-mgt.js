$(function () {
    $("#list").jqGrid
    ({
        url: "/DevManagement/GetBomdtMgt",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'bid', name: 'bid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'code', name: 'bom_no', width: 80, align: 'center' },
            { key: false, label: 'Mode code', name: 'md_cd', sortable: true, width: '100px', align: 'center' },
            { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200', formatter: setName_link, },
            { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
            { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px' },
            { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px' },
            { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180' },
             { key: false, label: 'Remark', name: 'cav', editable: true, width: '100px' },
            { key: false, label: 'Packing Amount(EA)', name: 'pack_amt', editable: true, width: '100px' },
            { key: false, label: 'Type', name: 'bom_type', editable: true, width: '100px' },
            { key: false, label: 'TDS no', name: 'tds_no', editable: true, width: '100px' },
            { key: false, label: 'use_yn', name: 'use_yn', editable: true, width: '100px', hidden: true },
            { key: false, label: 'del_yn', name: 'del_yn', editable: true, width: '100px', hidden: true },
            { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', width: '100px' },
            {
                key: false, label: 'Create User', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "d-m-Y H:i:s" }, width: '200'
            },
            { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px' },
            { key: false, label: 'Change Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "d-m-Y H:i:s" }, width: '200' }

        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $('#m_bid').val(row_id.bid);
            $('#m_bom_no').val(row_id.bom_no);
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

            $("#m_save_but").removeClass("hidden");
            $("#c_save_but").removeClass("active");
            $("#m_save_but").addClass("active");
            $("#c_save_but").addClass("hidden");


        },
        pager: jQuery('#gridpager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: '100%',
        caption: 'Students Records',
        emptyrecords: 'No Students Records are Available to Display',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false,
    }).navGrid('#gridpager',
        {
            edit: false, add: false, del: false, search: false,
            searchtext: "Search Student", refresh: true
        },
        {
            zIndex: 100,
            caption: "Search Students",
            sopt: ['cn']
        });

    $("#c_save_but").click(function () {
        if ($("#c_md_cd").val().trim() == "") {
            alert("Please enter your model code");
            $("#c_md_cd").val("");
            $("#c_md_cd").focus();
            return false;
        }
        if ($("#c_prj_nm").val().trim() == "") {
            alert("Please enter your project name");
            $("#c_prj_nm").val("");
            $("#c_prj_nm").focus();
            return false;
        }
        if ($("#c_ssver").val().trim() == "") {
            alert("Please enter your ss version");
            $("#c_ssver").val("");
            $("#c_ssver").focus();
            return false;
        }
        if ($("#c_part_nm").val().trim() == "") {
            alert("Please enter your Part Name");
            $("#c_part_nm").val("");
            $("#c_part_nm").focus();
            return false;
        }

        if ($("#c_part_nm").val().trim() == "") {
            alert("Please enter your ss sersion");
            $("#c_part_nm").val("");
            $("#c_part_nm").focus();
            return false;
        }
        if ($("#c_tds_no").val().trim() == "") {
            alert("Please enter your TDS.No");
            $("#c_tds_no").val("");
            $("#c_tds_no").focus();
            return false;
        }
        if ($("#c_standard").val().trim() == "") {
            alert("Please enter your standard");
            $("#c_standard").val("");
            $("#c_standard").focus();
            return false;
        }
        if ($("#c_order_num").val().trim() == "") {
            alert("Please enter your order numer");
            $("#c_order_num").val("");
            $("#c_order_num").focus();
            return false;
        }

        if ($("#c_cav").val().trim() == "") {
            alert("Please enter your Remark");
            $("#c_cav").val("");
            $("#c_cav").focus();
            return false;
        }
        if ($("#c_cust_rev").val().trim() == "") {
            alert("Please enter your custumer rev");
            $("#c_cust_rev").val("");
            $("#c_cust_rev").focus();
            return false;
        }
        if ($("#c_pack_amt").val() == "") {
            alert("Please enter your Packing Amount CAV");
            $("#c_pack_amt").val("");
            $("#c_pack_amt").focus();
            return false;
        }
        if ($("#c_bom_type").val().trim() == "") {
            alert("Please enter your type");
            $("#c_bom_type").val("");
            $("#c_bom_type").focus();
            return false;
        }

        else {

            $.ajax({
                url: "/DevManagement/CreateBomMgt",
                type: "get",
                dataType: "json",
                data: {
                    bom_no: $('#c_bom_no').val(),
                    md_cd: $('#c_md_cd').val(),
                    prj_nm: $('#c_prj_nm').val(),
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
                    jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });
    $("#m_save_but").click(function () {
        $.ajax({
            type: "PUT",
            dataType: "json",
            url: "/DevManagement/ModifyBomManagement",
            data: {
                bid: $('#m_bid').val(),
                bom_no: $('#m_bom_no').val(),
                md_cd: $('#m_md_cd').val(),
                prj_nm: $('#m_prj_nm').val(),
                ssver: $('#m_ssver').val(),
                part_nm: $('#m_part_nm').val(),
                standard: $("#m_standard").val(),
                cust_rev: $("#m_cust_rev").val(),
                order_num: $("#m_order_num").val(),
                pack_amt: $("#m_pack_amt").val(),
                cav: $("#m_cav").val(),
                bom_type: $("#m_bom_type").val(),
                tds_no: $("#m_tds_no").val(),
            },
            success: function (data) {
                jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });

    });
    $("#start").datepicker({
        format: 'mm/dd/yyyy',
    });
    $('#end').datepicker().datepicker('setDate', 'today');
    // Init Datepicker end
    $("#end").datepicker({
        format: 'mm/dd/yyyy',
        "autoclose": true
    });
    $("#searchBtn").click(function () {
        var bom_no = $('#bom_no').val().trim();
        var md_cd = $('#md_cd').val().trim();
        var prj_nm = $('#prj_nm').val().trim();
        var start = $('#start').val().trim();
        var end = $('#end').val().trim();
        $.ajax({
            url: "/DevManagement/searchBom",
            type: "get",
            dataType: "json",
            data: {
                bom_no: bom_no,
                md_cd: md_cd,
                prj_nm: prj_nm,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
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

    // Tab Create/Modifly    
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
        $("#m_save_but").attr("disabled", true);
        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
    });

    $(document).ready(function (e) {
        $('#pdfBtn').click(function (e) {
            $('#list').tableExport({
                type: 'pdf',
                pdfFontSize: '12',
                fileName: 'BusinessType_Infor',
                escape: false,
                headings: true,
                footers: true,
            });
        });
    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $('#list').tableExport({
                type: 'xls',
                fileName: 'BusinessType_Infor',
                escape: false,
                headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
                footers: true,
            });
        });
    });
    $(document).ready(function (e) {
        $('#htmlBtn').click(function (e) {
            $('#list').tableExport({
                type: 'doc',
                fileName: 'BusinessType_Infor',
                escape: false,
                headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
                footers: true,
            });
        });
    });


});//grid1

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
        var html = '<a href="/DevManagement/Bom_no?bid=' + rowdata.bid + '" style="color:red" target="_sub">' + cellvalue + '</a>';
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid