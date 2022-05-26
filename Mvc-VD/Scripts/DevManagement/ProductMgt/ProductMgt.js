$("#list").jqGrid
    ({
        url: "/DevManagement/GetStyleMgt",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Stamp Code', name: 'stamp_code', width: 80, align: 'center', hidden:true },

            { key: false, label: 'Product Code', name: 'style_no', width: 130, align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', width: 200, align: 'left' },
            { key: false, label: 'Model', name: 'md_cd', sortable: true, width: 130, align: 'left' },
            { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: 150, align: 'left' },
            { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '200', align: 'left' },
            { key: false, label: 'Type(Roll/Sheet)', name: 'bom_type', editable: true, width: '80', align: 'left' },
            { key: false, label: '%Loss', name: 'loss', editable: true, width: '100', align: 'left' },
            { key: false, label: 'Vendor', name: 'cust_rev', editable: true, width: '80', align: 'left' },
            { key: false, label: 'Packing Amount(EA)', name: 'pack_amt', editable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Expiry Month', name: 'expiry_month', width: 80, align: 'center' },
            { key: false, label: 'Expiry', name: 'expiry', width: 150, align: 'center' },
            { key: false, label: 'Nhiệt độ bảo quản', name: 'drawingname', editable: true, width: '120' },
            { key: false, label: 'Description', name: 'Description', editable: true, width: '120' },
            { key: false, label: 'Tem', name: 'stamp_name', editable: true, width: '100', align: 'left' },
            { key: false, label: 'productType', name: 'productType', editable: true, width: 50, align: 'left' },
            { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', width: '100px', align: 'left' },
            {
                key: false, label: 'Create Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '130'
            },
            { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px', align: 'left' },
            { key: false, label: 'Change Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '130' },

        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        gridComplete: function () {
            //var rows = $("#list").getDataIDs();
            //for (var i = 0; i < rows.length; i++) {
            //    var style_no = $("#list").getCell(rows[i], "style_no");
            //    var giatri = $('#remark_color').val();
            //    if (style_no == giatri) {
            //        $("#list").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            //    }
            //}
        },
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $('#m_sid').val(row_id.sid);
            $('#m_style_no').val(row_id.style_no);
            $('#m_style_nm').val(row_id.style_nm);
            $('#m_md_cd').val(row_id.md_cd);
            $('#m_prj_nm').val(row_id.prj_nm);
            $('#m_ssver').val(row_id.ssver);
            $('#m_part_nm').val(row_id.part_nm);
            //$('#m_standard').val(row_id.standard);
            $('#m_vendor').val(row_id.cust_rev);
            //$('#m_order_num').val(row_id.order_num);
            $('#m_pack_amt').val(row_id.pack_amt);
            $('#m_cav').val(row_id.cav);
            $('#m_bom_type').val(row_id.bom_type);
            //$('#m_tds_no').val(row_id.tds_no);
            //$('#m_item_vcd').val(row_id.item_vcd);
            $('#m_qc_range').val(row_id.qc_range_cd);
            $('#m_stamp_code').val(row_id.stamp_code);
            $('#m_x_m').val(row_id.expiry_month);
            $('#m_x').val(row_id.expiry);
            $('#m_nhietdobaoquan').val(row_id.drawingname);
            $('#m_loss').val(row_id.loss);
            $('#m_Description').val(row_id.Description);
            $('#m_type_product').val(row_id.productType);

            if (row_id.expiry_month != 0) {
                $('#m_select_month').val(1);

                $("#m_x").removeClass("active");
                $("#m_x").addClass("hidden");

                $("#m_x_m").addClass("active");
                $("#m_x_m").removeClass("hidden");



            }
            else {
                $('#m_select_month').val(2);
                $("#m_x").removeClass("hidden");
                $("#m_x").addClass("active");

                $("#m_x_m").addClass("hidden");
                $("#m_x_m").removeClass("active");

            }

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
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        height: 300,
        width: null,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        caption: "Product Information",
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

//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
//$('#list').jqGrid('setGridWidth', $(".box-body").width());
//$(window).on("resize", function () {
//    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
//    $("#list").jqGrid("setGridWidth", newWidth, false);
//});

$("#c_save_but").click(function () {
    if ($('#form1').valid() == true) {
        $.ajax({
            url: "/DevManagement/CreateStyleMgt",
            type: "Post",
            dataType: "json",
            data: {
                style_no: $('#c_style_no').val().toUpperCase().trim(),
                style_nm: $('#c_style_nm').val().trim(),
                md_cd: $('#c_md_cd').val(),
                prj_nm: $('#c_prj_nm').val(),
                ssver: $('#c_ssver').val(),
                part_nm: $('#c_style_nm').val().trim(),
                pack_amt: $("#c_pack_amt").val().trim(),
                stamp_code: $("#c_stamp_code").val(),
                expiry_month: $("#c_x_m").val(),
                expiry: $("#c_x").val(),
                drawingname: $("#c_nhietdobaoquan").val().trim(),
                loss: $("#c_loss").val().trim(),
                Description: $("#c_Description").val().trim(),
                productType: $("#c_type_product").val().trim(),
                bom_type: $("#c_bom_type").val().trim(),
                cust_rev: $("#c_vendor").val().trim(),

            },
            success: function (data) {
                if (data.result) {
                    var id = data.kq.sid;
                    $("#list").jqGrid('addRowData', id, data.kq, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                    return;
                }
                else
                {
                    ErrorAlert(data.message);
                    return;
                }
            },
            error: function (data) {
                ErrorAlert('Mã đã tồn tại, vui lòng tạo mã khác');
                return;
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
                style_nm: $('#m_style_nm').val().trim(),
                md_cd: $('#m_md_cd').val(),
                prj_nm: $('#m_prj_nm').val().trim(),
                ssver: $('#m_ssver').val().trim(),
                part_nm: $('#m_style_nm').val().trim(),
                pack_amt: ($("#m_pack_amt").val() == "") ? 0 : $("#m_pack_amt").val(),
                stamp_code: $("#m_stamp_code").val(),
                expiry_month: $("#m_x_m").val(),
                expiry: $("#m_x").val(),
                drawingname: $("#m_nhietdobaoquan").val().trim(),
                loss: $("#m_loss").val().trim(),
                Description: $("#m_Description").val().trim(),
                productType: $("#m_type_product").val().trim(),
                bom_type: $("#m_bom_type").val().trim(),
                cust_rev: $("#m_vendor").val().trim(),
            },
            success: function (data) {
                if (data.result == false) {
                    ErrorAlert(data.message);
                }
                else {
                    var id = data.kq.sid;
                    $("#list").setRowData(id, data.kq, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                }

            }
        });
    }
});

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#searchBtn").click(function () {
    var style_no = $('#style_no').val().trim();
    var style_nm = $('#style_nm').val().trim();
    var md_cd = $('#md_cd').val().trim();
    var prj_nm = $('#prj_nm').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    $.ajax({
        url: "/DevManagement/searchStyle",
        type: "GET",
        dataType: "json",
        data: {
            style_no: style_no,
            style_nm: style_nm,
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

$('#excelBtn').click(function (e) {
    $("#list").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Product  Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
});

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
            fileName: "Product  Information  ",
            returnAsString: false
        }
    );
});

//$(document).ready(function (e) {
//    $('#excelBtn').click(function (e) {
//        $("#list").jqGrid('exportToExcel',
//                options = {
//                    includeLabels: true,
//                    includeGroupHeader: true,
//                    includeFooter: true,
//                    fileName: "StyleMgt.xlsx",
//                    mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                    maxlength: 40,
//                    onBeforeExport: null,
//                    replaceStr: null
//                }
//            );
//    });
//});
//$(document).ready(function (e) {
//    $('#htmlBtn').click(function (e) {
//        $("#list").jqGrid('exportToHtml',
//            options = {
//                title: '',
//                onBeforeExport: null,
//                includeLabels: true,
//                includeGroupHeader: true,
//                includeFooter: true,
//                tableClass: 'jqgridprint',
//                autoPrint: false,
//                topText: '',
//                bottomText: '',
//                fileName: "StyleMgt",
//                returnAsString: false
//            }
//            );
//    });

//});
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
    GetAllStamp();
});


// Khai báo URL stand của bạn ở đây
var baseService = "/DevManagement";
var GetModelcode = baseService + "/GetModelcode";
var GetSSVer = baseService + "/GetSSVer";
var GetParkNm = baseService + "/GetParkNm";
var GetStandard = baseService + "/GetStandard";
var GetCustRev = baseService + "/GetCustRev";
var GetOrder = baseService + "/GetOrder";
var GetbomType = baseService + "/GetBomType";
var GetType = baseService + "/GetType";
var GetTdsno = baseService + "/GetTdsno";
var GetQCRange = baseService + "/GetQCRange";

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
//$(document).ready(function () {
//    _GetModelcode();
//    $("#c_md_cd").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetModelcode() {

//    $.get(GetModelcode, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Model code*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_md_cd").html(html);
//        }
//    });
//}
//End Model code
//SSVer - DEV002
//$(document).ready(function () {
//    _GetSSVer();
//    $("#c_ssver").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetSSVer() {

//    $.get(GetSSVer, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*SS Version*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_ssver").html(html);
//        }
//    });
//}
//End SSVer
//Park Name - DEV003
//$(document).ready(function () {
//    _GetParkNm();
//    $("#c_part_nm").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetParkNm() {

//    $.get(GetParkNm, function (data) {
//        if (data != null && data != undefined && data.length) {

//            var html = '';
//            html += '<option value="" selected="selected">*Part Name*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value="' + item.dt_cd + '">' + item.dt_nm + '</option>';
//            });
//            $("#c_part_nm").html(html);
//        }
//    });
//}
//End Park Name
//Standard - DEV004
//$(document).ready(function () {
//    _GetStandard();
//    $("#c_standard").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetStandard() {

//    $.get(GetStandard, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Standard*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_standard").html(html);
//        }
//    });
//}
//End Standard
//Customer Rev - DEV005
//$(document).ready(function () {
//    _GetCustRev();
//    $("#c_cust_rev").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetCustRev() {

//    $.get(GetCustRev, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Customer Rev*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_cust_rev").html(html);
//        }
//    });
//}
//End Customer Rev
//Order Number - DEV006
//$(document).ready(function () {
//    _GetOrder();
//    $("#c_order_num").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetOrder() {

//    $.get(GetOrder, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Order Number*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_order_num").html(html);
//        }
//    });
//}
//End Order Number
//Type - DEV007
$(document).ready(function () {
    _GetType();
});
function _GetType() {
    $.get(GetbomType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#c_bom_type").html(html);
            $("#m_bom_type").html(html);
        }
    });
}
//End Type
//Type - DEV008
//$(document).ready(function () {
//    _GetTdsno();
//    $("#c_tds_no").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetTdsno() {

//    $.get(GetTdsno, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*TDS No*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_tds_no").html(html);
//        }
//    });
//}
//End Type
//.!---------End Create-------.!/

//Modify
//Model code - DEV001
//$(document).ready(function () {
//    _GetModelcodeMD();
//    $("#m_md_cd").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetModelcodeMD() {

//    $.get(GetModelcodeMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Model code*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_md_cd").html(html);
//        }
//    });
//}
//End Model code
//SSVer - DEV002
//$(document).ready(function () {
//    _GetSSVerMD();
//    $("#m_ssver").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetSSVerMD() {

//    $.get(GetSSVerMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*SS Version*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_ssver").html(html);
//        }
//    });
//}
//End SSVer
//Park Name - DEV003
//$(document).ready(function () {
//    _GetParkNmMD();
//    $("#m_part_nm").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetParkNmMD() {

//    $.get(GetParkNmMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Part Name*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value="' + item.dt_cd + '">' + item.dt_nm + '</option>';
//            });
//            $("#m_part_nm").html(html);
//        }
//    });
//}
//End Park Name
//Standard - DEV004
//$(document).ready(function () {
//    _GetStandardMD();
//    $("#m_standard").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetStandardMD() {

//    $.get(GetStandardMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Standard*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_standard").html(html);
//        }
//    });
//}
//End Standard
//Customer Rev - DEV005
//$(document).ready(function () {
//    _GetCustRevMD();
//    $("#m_cust_rev").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetCustRevMD() {

//    $.get(GetCustRevMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Customer Rev*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_cust_rev").html(html);
//        }
//    });
//}
//End Customer Rev
//Order Number - DEV006
//$(document).ready(function () {
//    _GetOrderMD();
//    $("#m_order_num").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetOrderMD() {

//    $.get(GetOrderMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Order Number*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_order_num").html(html);
//        }
//    });
//}
//End Order Number
//Type - DEV007
//$(document).ready(function () {
//    _GetTypeMD();
//    $("#m_bom_type").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });

//});
//function _GetTypeMD() {

//    $.get(GetBomType, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Type*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_bom_type").html(html);
//        }
//    });
//}
//End Type
//Type - DEV008
//$(document).ready(function () {
//    _GetTdsnoMD();
//    $("#m_tds_no").on('change', function () {
//        var id = $(this).val();
//        if (id != undefined && id != '') {
//        }
//    });
//});
//function _GetTdsnoMD() {

//    $.get(GetTdsnoMD, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*TDS No*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
//            });
//            $("#m_tds_no").html(html);
//        }
//    });
//}
//End Type
//$(document).ready(function () {
//    _GetQCRange();
//});
//function _GetQCRange() {

//    $.get(GetQCRange, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*QC Range*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
//            });
//            $("#c_qc_range").html(html);
//            $("#m_qc_range").html(html);
//        }
//    });
//}
//.!---------End Modify-------.!/


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
        "stamp_code": {
            required: true,
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
        "stamp_code": {
            required: true,
        },
    },
});



insertmaterial_tmp = [];
json_object = "";

$("#imgupload").change(function (evt) {
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event) {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName) {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            console.log(XL_row_object);
            for (var i = 0; i < XL_row_object.length; i++) {
                var data_row_tmp = [];

                (XL_row_object[i]["Product Code"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Product Code"]));
                (XL_row_object[i]["Product Name"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Product Name"]));
                (XL_row_object[i]["Model Code"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Model Code"]));
                (XL_row_object[i]["Project Name"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Project Name"]));
                (XL_row_object[i]["Packing Amount(EA)"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Packing Amount(EA)"]));
                (XL_row_object[i]["Tem"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Tem"]));
                (XL_row_object[i]["Expiry"] == undefined) ? data_row_tmp.push("") : (data_row_tmp.push(XL_row_object[i]["Expiry"]));
                //data_row_tmp.push(XL_row_object[i]);
                excelData.push(data_row_tmp);
            }
            json_object = JSON.stringify(excelData);
            console.log(JSON.parse(json_object));
        })
    };

    reader.onerror = function (event) {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});

$("#uploadBtn").click(function () {
    debugger;
    if ($("#imgupload").val() == "") {
        ErrorAlert("Please select file to upload");
        return false
    }
    $('#loading').show();
    var data_create = 0;
    var data_update = 0;
    var data_error = 0;
    var data_pagedata = -1;
    insertmaterial_tmp = [];


    if (json_object != "") {

        var obj = JSON.parse(json_object.toString());
        var length = obj.length;
        for (i = 0; i <= length - 1; i++) {
            var tmp = [];
            var b = obj[i];
            var arr = Object.keys(b).map(function (key) { return b[key]; });
            for (j = 0; j <= 6; j++) {
                tmp.push(arr[j]);
            }
            item = {
                style_no: tmp[0],
                style_nm: tmp[1],
                md_cd: tmp[2],
                prj_nm: tmp[3],
                pack_amt: tmp[4],
                stamp_code: tmp[5],
                expiry: tmp[6],
            }
            insertmaterial_tmp.push(item);
        }

        $.ajax({
            url: "/DevManagement/insertProductExcel",
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(insertmaterial_tmp),
            traditonal: true,
            success: function (response) {
                if (response.result) {
                    debugger;
                    data_create = data_create + response.data_create;
                    data_update = data_update + response.data_update;
                    data_error = data_error + response.data_error;
                    data_pagedata = data_pagedata + 1;
                    $("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");

                    $.each(response.data, function (key, item) {
                        var id = item[0].sid;

                        var STATUS = item[0].STATUS;

                        if (STATUS == 'INSERT') {

                            $("#list").jqGrid('addRowData', id, item[0], 'first');

                            $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });
                        }
                    });
                    document.getElementById("imgupload").value = "";
                    $('#loading').hide();
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(response.message);
                    $(`#loading`).hide();
                    document.getElementById("imgupload").value = "";
                    return;

                }
            },
        });


        //var segment = Math.floor(length / 1000);
        //for (pagedata = 0; pagedata <= segment; pagedata++) {
        //    insertmaterial_tmp = [];
        //    for (line = 0; line <= 999; line++) {
        //        line_curr = pagedata * 1000 + line;
        //        var tmp = [];
        //        var b = obj[line_curr];
        //        if (b != null || b != undefined) {
        //            var arr = Object.keys(b).map(function (key) { return b[key]; });
        //            for (j = 0; j <= 5; j++) {
        //                tmp.push(arr[j]);
        //            }
        //            console.log(tmp);
        //            item = {
        //                code: tmp[0],
        //                name: tmp[1],
        //            }
        //            insertmaterial_tmp.push(item);
        //        }
        //    }

        $('#loading').hide();
    }
});

function GetAllStamp() {
    $.get(`/CreateBuyerQR/GetAllStamp`, function (response) {
        if (response) {
            var html = undefined;
            var html = `<option value="">* Chọn loại tem *</option>`
            $.each(response, function (key, item) {
                html += `<option value="${item.stamp_code}">${item.stamp_name}</option>`
            });
            $(`#c_stamp_code`).html(html);
            $(`#m_stamp_code`).html(html);
        }
    });
};




function changeFunc(e) {
    if (e.value == 1) {
        $("#c_x").val("");
        $("#c_x_m").val("");

        $("#c_x").removeClass("active");
        $("#c_x").addClass("hidden");

        $("#c_x_m").removeClass("hidden");
        $("#c_x_m").addClass("active");
    }
    else {
        $("#c_x").val("");
        $("#c_x_m").val("");

        $("#c_x").removeClass("hidden");
        $("#c_x").addClass("active");

        $("#c_x_m").addClass("hidden");
        $("#c_x_m").removeClass("active");
    }



}
function changeFuncM(e) {
    if (e.value == 1) {
        $("#m_x").val("");
        $("#m_x_m").val("");

        $("#m_x").removeClass("active");
        $("#m_x").addClass("hidden");

        $("#m_x_m").removeClass("hidden");
        $("#m_x_m").addClass("active");
    }
    else {
        $("#m_x").val("");
        $("#m_x_m").val("");

        $("#m_x").removeClass("hidden");
        $("#m_x").addClass("active");

        $("#m_x_m").addClass("hidden");
        $("#m_x_m").removeClass("active");
    }



}