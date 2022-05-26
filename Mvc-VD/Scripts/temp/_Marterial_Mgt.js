$grid = $("#list").jqGrid({
    url: '/DevManagement/GetMaterial',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
     { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
     { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
     { label: 'Code', name: 'mt_no', key: true, width: 150, align: 'center' },
     { label: 'Name', name: 'mt_nm', sortable: true, },
     { label: 'Supplier Information', name: 'sp_cd', sortable: true, width: 150, align: 'right', },
     { label: 'Manufacturer', name: 'mf_cd', sortable: true, width: 150, align: 'right', },
     { label: 'Width', name: 'new_with', sortable: true, width: 80, align: 'right' },
     { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', hidden: true },
     { label: 'width unit', name: 'width_unit', hidden: true, align: 'right' },
     { label: 'Spec', name: 'new_spec', sortable: true, width: 100, align: 'right' },
     { name: 'spec', sortable: true, width: 100, align: 'right', hidden: true },
     { label: 'Spec Unit', name: 'spec_unit', width: 60, hidden: true, align: 'right' },
     { label: 'Photo', name: 'photo_file', width: 100, formatter: downloadLink, align: 'center' },
     { label: 'Photo', name: 'photo_file', hidden: true },
     { label: 'Create Date', name: 'reg_dt', width: 110, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
     { label: 'Price', name: 'new_price', width: 90, align: 'right' },
     { name: 'price', width: 60, align: 'right', hidden: true, },
     { label: 'price Unit', name: 'price_unit', width: 60, hidden: true, },
     { label: 'Re Mark', name: 're_mark', width: 130, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
     { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
     { label: 'Change Name', name: 'chg_id', width: 90, align: 'left', },
     { label: 'Change Date', name: 'chg_dt', width: 110, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' }
    },
    cmTemplate: { title: false },
    gridComplete: function () {

        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var v_use_yn = $("#list").getCell(rows[i], "use_yn");
            if (v_use_yn == "N") {
                $("#list").jqGrid('setRowData', rows[i], false, { background: '#D5D5D5' });
            }
        }
    },
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        var mtid = row_id.mtid;
        var mt_type = row_id.mt_type;
        var mt_no = row_id.mt_no;
        var mt_nm = row_id.mt_nm;
        var width = row_id.width;
        var width_unit = row_id.width_unit;
        var spec = row_id.spec;
        var spec_unit = row_id.spec_unit;
        var price = row_id.price;
        var price_unit = row_id.price_unit;
        var re_mark = row_id.re_mark;
        var logo = row_id.photo_file;

        $('#m_logo1').val(logo);
        $('#m_manufac').val(row_id.mf_cd);
        $('#m_supllier').val(row_id.sp_cd);
        $('#m_mtid').val(mtid);
        $('#m_mt_type').val(mt_type);
        $('#m_mt_no').val(mt_no);
        $('#m_mt_nm').val(mt_nm);
        $('#m_width').val(width);
        $('#m_width_unit').val(width_unit);
        $('#m_spec').val(spec);
        $('#m_spec_unit').val(spec_unit);
        $('#m_price').val(price);
        $('#m_price_unit').val(price_unit);
        $('#m_re_mark').val(re_mark);

        if (logo != null) {
            $("#m_logo").html('<img src="../Images/MarterialImg/' + logo + '" style="height:25px" />');

        } else {
            $("#m_photo_file").html("");
        }
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#tab_1").removeClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
    },
    pager: '#jqGridPager',
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 400,
    width: $(".box-body").width() - 5,
    caption: 'Marterial Information',
    emptyrecords: 'No Data',
    multiselect: false,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

$("#m_save_but").click(function () {

    var selRowId = $('#list').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top mterial on the grid.");
        return false;
    }
    if ($("#m_mt_type").val().trim() == "") {
        alert("Please enter Type");
        $("#m_mt_type").val("");
        $("#m_mt_type").focus();
        return false;
    }
    if ($("#m_mt_nm").val().trim() == "") {
        alert("Please enter name");
        $("#m_mt_nm").val("");
        $("#m_mt_nm").focus();
        return false;
    }
    if ($("#m_mt_nm").val().trim() == "") {
        alert("Please enter name");
        $("#m_mt_nm").val("");
        $("#m_mt_nm").focus();
        return false;
    }
    if ($("#m_width").val().trim() == "") {
        alert("Please enter width");
        $("#m_width").val("");
        $("#m_width").focus();
        return false;
    }
    if ($("#m_width_unit").val().trim() == "") {
        alert("Please enter width unit");
        $("#m_width_unit").val("");
        $("#m_width_unit").focus();
        return false;
    }
    if ($("#m_spec").val().trim() == "") {
        alert("Please enter spec");
        $("#m_spec").val("");
        $("#m_spec").focus();
        return false;
    }
    if ($("#m_spec_unit").val().trim() == "") {
        alert("Please enter spec unit");
        $("#m_spec_unit").val("");
        $("#m_spec_unit").focus();
        return false;
    }
    if ($("#m_price").val().trim() == "") {
        alert("Please enter price");
        $("#m_price").val("");
        $("#m_price").focus();
        return false;
    }
    if ($("#m_price_unit").val().trim() == "") {
        alert("Please enter price unit");
        $("#m_price_unit").val("");
        $("#m_price_unit").focus();
        return false;
    } if ($("#m_supllier").val().trim() == "") {
        alert("Please enter supplier Info");
        $("#m_supllier").val("");
        $("#m_supllier").focus();
        return false;
    }
    if ($("#m_manufac").val().trim() == "") {
        alert("Please enter ManuFact Info");
        $("#m_manufac").val("");
        $("#m_manufac").focus();
        return false;
    }
    if ($("#m_re_mark").val().trim() == "") {
        alert("Please enter re mark");
        $("#m_re_mark").val("");
        $("#m_re_mark").focus();
        return false;
    } else {
        var formData = new FormData();
        var files = $("#m_photo_file").get(0).files;
        formData.append("file", files[0]);
        formData.append("mtid", $('#m_mtid').val());
        formData.append("mt_type", $('#m_mt_type').val());
        formData.append("mt_no", $('#m_mt_no').val());
        formData.append("mt_nm", $('#m_mt_nm').val());
        formData.append("width", $('#m_width').val());
        formData.append("width_unit", $('#m_width_unit').val());
        formData.append("spec", $('#m_spec').val());
        formData.append("spec_unit", $('#m_spec_unit').val());
        formData.append("price", $('#m_price').val());
        formData.append("price_unit", $('#m_price_unit').val());
        formData.append("re_mark", $('#m_re_mark').val());
        formData.append("photo_file", $('#m_logo1').val());
        formData.append("sp_cd", $('#m_supllier').val());
        formData.append("mf_cd", $('#m_manufac').val());
        $.ajax({
            type: "Put",
            url: "/DevManagement/updatematerial",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                debugger;
                if (data.result = true) {
                    SuccessAlert("successfully material Update");
                    jQuery("#list").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    ErrorAlert("Something Went Wrong..");
                }
            }
        });
    }
});

function downloadLink(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            fileName: "MateriaMgt",
            pdfFontSize: '7',
            escape: false,
            headings: true,
            bootstrap: true,
            footers: true,
        });
    });
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            type: 'xls',
            fileName: "MateriaMgt",
            escape: false,
            headings: true,
            bootstrap: true,
            footers: true,
        });
    });
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: "MateriaMgt",
            headings: true,
            bootstrap: true,
            footers: true,
            escape: false,
        });
    });

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#list").trigger('reloadGrid');
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

    $("#m_save_but").removeClass("active");
    $("#c_save_but").removeClass("hidden");
    $("#m_save_but").addClass("hidden");
    $("#c_save_but").addClass("active");
    var selectedRowId = $("#commonGrid").getGridParam('selrow');

});
$("#tab_2").on("click", "a", function (event) {

    document.getElementById("form2").reset();
    $("#list").trigger('reloadGrid');
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
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);

});

$("#c_save_but").click(function () {
    if ($("#c_mt_type").val().trim() == "") {
        alert("Please enter Type");
        $("#c_mt_type").val("");
        $("#c_mt_type").focus();
        return false;
    }
    if ($("#c_mt_nm").val().trim() == "") {
        alert("Please enter name");
        $("#c_mt_nm").val("");
        $("#c_mt_nm").focus();
        return false;
    }
    if ($("#c_mt_nm").val().trim() == "") {
        alert("Please enter name");
        $("#c_mt_nm").val("");
        $("#c_mt_nm").focus();
        return false;
    }
    if ($("#c_width").val().trim() == "") {
        alert("Please enter width");
        $("#c_width").val("");
        $("#c_width").focus();
        return false;
    }
    if ($("#c_width_unit").val().trim() == "") {
        alert("Please enter width unit");
        $("#c_width_unit").val("");
        $("#c_width_unit").focus();
        return false;
    }
    if ($("#c_spec").val().trim() == "") {
        alert("Please enter spec");
        $("#c_spec").val("");
        $("#c_spec").focus();
        return false;
    }
    if ($("#c_spec_unit").val().trim() == "") {
        alert("Please enter spec unit");
        $("#c_spec_unit").val("");
        $("#c_spec_unit").focus();
        return false;
    }
    if ($("#c_price").val().trim() == "") {
        alert("Please enter price");
        $("#c_price").val("");
        $("#c_price").focus();
        return false;
    }
    if ($("#c_price_unit").val().trim() == "") {
        alert("Please enter price unit");
        $("#c_price_unit").val("");
        $("#c_price_unit").focus();
        return false;
    }
    if ($("#c_re_mark").val().trim() == "") {
        alert("Please enter re mark");
        $("#c_re_mark").val("");
        $("#c_re_mark").focus();
        return false;
    }
    if ($("#c_supllier").val().trim() == "") {
        alert("Please enter supplier Info");
        $("#c_supllier").val("");
        $("#c_supllier").focus();
        return false;
    }
    if ($("#c_manufac").val().trim() == "") {
        alert("Please enter ManuFact Info");
        $("#c_manufac").val("");
        $("#c_manufac").focus();
        return false;
    }

    else {
        var formData = new FormData();
        var files = $("#c_photo_file").get(0).files;

        formData.append("file", files[0]);
        formData.append("mtid", $('#c_mtid').val());
        formData.append("mt_type", $('#c_mt_type').val());
        formData.append("mt_no", $('#c_mt_no').val());
        formData.append("mt_nm", $('#c_mt_nm').val());
        formData.append("width", $('#c_width').val());
        formData.append("width_unit", $('#c_width_unit').val());
        formData.append("spec", $('#c_spec').val());
        formData.append("spec_unit", $('#c_spec_unit').val());
        formData.append("price", $('#c_price').val());
        formData.append("price_unit", $('#c_price_unit').val());
        formData.append("re_mark", $('#c_re_mark').val());
        formData.append("sp_cd", $('#c_supllier').val());
        formData.append("mf_cd", $('#c_manufac').val());
        $.ajax({
            type: 'POST',
            url: "/DevManagement/insertmaterial",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result = true) {
                    alert("successfully MATERIAL Insert");
                    jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert("Something Went Wrong..");
                }
            }
        })
    }
});

//select option
$("#start1").datepicker({
    format: 'mm/dd/yyyy',
});
//$('#end1').datepicker().datepicker('setDate', 'today');

// Init Datepicker end
$("#end1").datepicker({
    format: 'mm/dd/yyyy',
});
$("#searchBtn").click(function () {
    var type = $("#s_mt_type").val();
    var code = $("#mt_no").val();
    var name = $("#mt_nm").val();
    var start1 = $("#start1").val();
    var end1 = $("#end1").val();

    $.ajax({
        url: "/DevManagement/searchmaterial",
        type: "Get",
        dataType: "json",
        data: {
            typeData: type,
            codeData: code,
            nameData: name,
            start1Data: start1,
            end1Data: end1,
        },
        success: function (result) {
            $("#list")
            .jqGrid('clearGridData')
            .jqGrid('setGridParam', {
                datatype: 'local',
                data: result
            })
    .trigger("reloadGrid");

        },
        error: function (result) {
            alert('Not found');
        }
    });
});
// Khai báo URL stand của bạn ở đây
var baseService = "/DevManagement";
var GetType = baseService + "/GetType";
var GetWidth = baseService + "/GetWidth";
var GetSpec = baseService + "/GetSpec";
var GetPrice = baseService + "/GetPrice";

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});

// TYPE
GetType_Marterial();
function GetType_Marterial() {
    $.get("/DevManagement/GetType_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".gettype").empty();
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".gettype").html(html);
        }
    });
};

GetWidth_Marterial();
function GetWidth_Marterial() {
    $.get("/DevManagement/GetWidth_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".getwidth").empty();
            html += '<option value="" selected="selected">*Width unit*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".getwidth").html(html);
        }
    });
};

GetSpec_Marterial();
function GetSpec_Marterial() {
    $.get("/DevManagement/GetSpec_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".getspec").empty();
            html += '<option value="" selected="selected">*Spec unit*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".getspec").html(html);
        }
    });
};

GetPrice_Marterial();
function GetPrice_Marterial() {
    $.get("/DevManagement/GetPrice_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".getprice").empty();
            html += '<option value="" selected="selected">*Price unit*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".getprice").html(html);
        }
    });
};
