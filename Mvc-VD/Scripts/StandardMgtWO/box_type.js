$("#list").jqGrid({
    url: "/StandardMgtWO/GetdsLbox",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'ltno', name: 'ltno', width: 100, align: 'center', hidden: true },
        { key: false, label: 'LBox Code', name: 'ltype_cd', width: 150, align: 'center' },
        { key: false, label: 'Name', name: 'ltype_nm', width: 200, align: 'left' },
        { key: false, label: 'SBox Use', name: 'sbox_use_yn', sortable: true, width: 80, align: 'center' },
        { key: false, label: 'SBox Type', name: 'stype_nm', sortable: true, width: '150' },
        { key: false, label: 'SBox Type', name: 'stype_cd', sortable: true, width: '150', hidden: true },
        { key: false, label: 'SBox Qty', name: 'sbox_qty', editable: true, width: '100px', align: 'right' },
        { key: false, label: 'Product Qty', name: 'prd_qty', editable: true, width: '100px', align: 'right' },
        { key: false, label: 'Use YN', name: 'use_yn', editable: true, width: '50px', align: 'center' },
        { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
        { label: 'Change Name', name: 'chg_id', width: 90, align: 'left', },
        { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);

        $("#list3").clearGridData();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        if (row_id.sbox_use_yn == "N") {
            var html = '<input type="hidden" name="name" value="0" class="phanbiet1"/>';
            html += ' <div class="col-lg-4 py-1"> <label>Product Qty</label></div>';
            html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="m_sbox_qty" value="' + row_id.sbox_qty + '" class="form-control form-control-text text-right"></div>';
            $(".change_kq1").html(html);
        } else {
            var html = '<input type="hidden" name="name" value="1" class="phanbiet1"/>';
            html += ' <div class="col-lg-4 py-1"> <label>SBox Qty</label></div>';
            html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="m_sbox_qty"  value="' + row_id.sbox_qty + '" class="form-control form-control-text text-right"></div>';
            $(".change_kq1").html(html);
        }
        $('#m_ltype_nm').val(row_id.ltype_nm);
        $('#m_sbox_use_yn').val(row_id.sbox_use_yn);
        $('#m_stype_cd').val(row_id.stype_cd);
        $('#m_use_yn').val(row_id.use_yn);
        $('#ltno').val(row_id.ltno);
    },

    pager: jQuery('#listGridPager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 300,
    width:null,
    sortable: true,
    loadonce: true,
    caption: 'LBox Type',
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
$('#list').jqGrid('setGridWidth', $(".boxList").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
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
    $("#list").trigger("reloadGrid");
    $("#list2").clearGridData();
    $("#list3").clearGridData();
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#ver_vf").attr("disabled", true);
});
$("#list2").jqGrid({
    url: "/StandardMgtWO/Getdatasbox",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'stno', name: 'stno', width: 100, align: 'center', hidden: true },
        { key: false, label: 'SBox Code', name: 'stype_cd', width: 150, align: 'left' },
        { key: false, label: 'SBox Name', name: 'stype_nm', width: 150, align: 'left' },
        { key: false, label: 'Product Qty', name: 'prd_qty', sortable: true, width: 50, align: 'right' },
        { key: false, label: 'Use YN ', name: 'use_yn', sortable: true, width: '50', align: 'center' },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
        row_id = $("#list2").getRowData(selectedRowId);
        $("#tab_d1").removeClass("active");
        $("#tab_d2").addClass("active");
        $("#tab_dc1").removeClass("active");
        $("#tab_dc2").removeClass("hidden");
        $("#tab_dc1").addClass("hidden");
        $("#tab_dc2").addClass("active");
        $("#dm_save_but").attr("disabled", false);
        $('#m_stype_nm').val(row_id.stype_nm); 
        $('#m_prd_qty').val(row_id.prd_qty);
        $('#stno').val(row_id.stno);
    },

    pager: jQuery('#list2GridPager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 200,
    width: null,
    sortable: true,
    loadonce: true,
    caption: 'SBox Type',
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
$('#list2').jqGrid('setGridWidth', $(".boxList").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list2").jqGrid("setGridWidth", newWidth, false);
});
$("#tab_d1").on("click", "a", function (event) {
    document.getElementById("form3").reset();
    $("#tab_d2").removeClass("active");
    $("#tab_d1").addClass("active");
    $("#tab_dc2").removeClass("active");
    $("#tab_dc1").removeClass("hidden");
    $("#tab_dc2").addClass("hidden");
    $("#tab_dc1").addClass("active");
});
$("#tab_d2").on("click", "a", function (event) {
    $("#dm_save_but").attr("disabled", true);
    document.getElementById("form4").reset();
    $("#tab_d1").removeClass("active");
    $("#tab_d2").addClass("active");
    $("#tab_dc1").removeClass("active");
    $("#tab_dc2").removeClass("hidden");
    $("#tab_dc1").addClass("hidden");
    $("#tab_dc2").addClass("active");
});
$(document).ready(function () {
    _Getms_box();
});
function _Getms_box() {
    $.get("/StandardMgtWO/Getms_box", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*SBox Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value="' + item.stype_cd + '">' + item.stype_nm + '</option>';
            });
            $("#c_stype_cd").html(html);
            $("#m_stype_cd").html(html);
        }
    });
}
$("#searchBtn").click(function () {
    var ltype_cd = $('#s_ltype_cd').val().trim();
    var ltype_nm = $("#s_ltype_nm").val().trim();

    $.ajax({
        url: "/StandardMgtWO/SearchLboxdata",
        type: "get",
        dataType: "json",
        data: {
            ltype_cd: ltype_cd,
            ltype_nm: ltype_nm,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});
$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
        $.ajax({
            url: "/StandardMgtWO/CreatLbox",
            type: "get",
            dataType: "json",
            data: {
                ltype_nm: $('#c_ltype_nm').val().trim(),
                sbox_use_yn: $('#c_sbox_use_yn').val(),
                stype_cd: $('#c_stype_cd').val(),
                sbox_qty: $('#c_sbox_qty').val(),
                use_yn: $('#c_use_yn').val().trim(),
                phanbiet: $('.phanbiet').val(),
            },
            success: function (data) {
                var id = data.ltno;
                $("#list").jqGrid('addRowData', id, data, 'first');
                $("#list").setRowData(id, false, { background: "#d0e9c6" });
                document.getElementById("form1").reset();
                var html = '<input type="hidden" name="name" value="1" class="phanbiet"/>';
                html += ' <div class="col-lg-4 py-1"> <label>SBox Qty</label></div>';
                html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="c_sbox_qty" class="form-control form-control-text text-right"></div>';
                $(".change_kq").html(html);
            },
            error: function (data) {
                alert('. Please check again.');
            }
        });
    }
});
$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        $.ajax({
            url: "/StandardMgtWO/EditLbox",
            type: "get",
            dataType: "json",
            data: {
                ltype_nm: $('#m_ltype_nm').val().trim(),
                sbox_use_yn: $('#m_sbox_use_yn').val(),
                stype_cd: $('#m_stype_cd').val().trim(),
                sbox_qty: $('#m_sbox_qty').val(),
                use_yn: $('#m_use_yn').val().trim(),
                ltno: $('#ltno').val(),
                phanbiet: $('.phanbiet1').val(),
            },
            success: function (response) {
                console.log(response);
                if (response.list) {
                    var id = response.list.ltno;
                    $("#list").setRowData(id, response.list, { background: "#d0e9c6" });
                    document.getElementById("form2").reset();
                    }
                else {
                    alert(response.message);
                }
            },
            error: function (data) {
                alert('. Please check again.');
            }
        });
    }
});
$("#form1").validate({
    rules: {
        "ltype_nm": {
            required: true,
        },
    },
    rules: {
        "sbox_qty": {
            required: true,
            number: true,
        },
    },
});
$("#form2").validate({
    rules: {
        "ltype_nm": {
            required: true,
        },
    },
    rules: {
        "sbox_qty": {
            required: true,
            number: true,
        },
    },
});


$("#dc_save_but").click(function () {
    if ($("#form3").valid() == true) {
        $.ajax({
            url: "/StandardMgtWO/CreateSbox",
            type: "get",
            dataType: "json",
            data: {
                stype_nm: $('#c_stype_nm').val().trim(),
                prd_qty: $('#c_prd_qty').val(),
            },
            success: function (data) {
                var id = data.stno;
                $("#list2").jqGrid('addRowData', id, data, 'first');
                $("#list2").setRowData(id, false, { background: "#d0e9c6" });
                document.getElementById("form3").reset();
                _Getms_box();
            },
            error: function (data) {
                alert('. Please check again.');
            }
        });
    }
});
$("#dm_save_but").click(function () {
    if ($("#form4").valid() == true) {
        $.ajax({
            url: "/StandardMgtWO/EditSbox",
            type: "get",
            dataType: "json",
            data: {
                stype_nm: $('#m_stype_nm').val().trim(),
                prd_qty: $('#m_prd_qty').val(),
                stno: $('#stno').val(),
            },
            success: function (response) {
                if (response.a) {
                    var id = response.a.stno;
                    $("#list2").setRowData(id, response.a, { background: "#d0e9c6" });
                    document.getElementById("form4").reset();
                }
                else {
                    alert(response.message);
                }
                _Getms_box();
            },
            error: function (data) {
                alert('. Please check again.');
            }
        });
    }
});
$("#form3").validate({
    rules: {
        "stype_nm": {
            required: true,
        },
    },
    rules: {
        "prd_qty": {
            required: true,
            number: true,
        },
    },
});
$("#form4").validate({
    rules: {
        "stype_nm": {
            required: true,
        },
    },
    rules: {
        "prd_qty": {
            required: true,
            number: true,
        },
    },
});

$("select#c_sbox_use_yn").change(function () {
var selValue = $(this).val();
if (selValue == "N") {
    var html = '<input type="hidden" name="name" value="0" class="phanbiet"/>';
        html+=' <div class="col-lg-4 py-1"> <label>Product Qty</label></div>';
        html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="c_sbox_qty" class="form-control form-control-text text-right"></div>';
        $(".change_kq").html(html);
} else {
    var html = '<input type="hidden" name="name" value="1" class="phanbiet"/>';
    html += ' <div class="col-lg-4 py-1"> <label>SBox Qty</label></div>';
    html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="c_sbox_qty" class="form-control form-control-text text-right"></div>';
    $(".change_kq").html(html);
}

});
$("select#m_sbox_use_yn").change(function () {
    var selValue = $(this).val();
    if (selValue == "N") {
        var html = '<input type="hidden" name="name" value="0" class="phanbiet1"/>';
        html += ' <div class="col-lg-4 py-1"> <label>Product Qty</label></div>';
        html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="m_sbox_qty" class="form-control form-control-text text-right"></div>';
        $(".change_kq1").html(html);
    } else {
        var html = '<input type="hidden" name="name" value="1" class="phanbiet1"/>';
        html += ' <div class="col-lg-4 py-1"> <label>SBox Qty</label></div>';
        html += '<div class="col-lg-8 py-1"><input type="text" name="sbox_qty" id="m_sbox_qty" class="form-control form-control-text text-right"></div>';
        $(".change_kq1").html(html);
    }

});