$grid = $("#list").jqGrid({
    url: '/standard/selectSupplierInfo',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'spno', name: 'spno', key: true, width: 80, align: 'center', hidden: true },
        { label: 'Supplier Code', name: 'sp_cd', width: 120, align: 'center' },
        { label: 'Name', name: 'sp_nm', width: 150, sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Busines type', name: 'bsn_tp', width: 100, align: 'center' },
        //{ label: 'BSN', name: 'bsn_tp', width: 80, hidden: true, align: 'center' },
        { label: 'Changer', name: 'chg_id', width: 80, align: 'center' },
        { label: 'Tel', name: 'phone_nb', width: 90, align: 'left' },
        { label: 'Cell', name: 'cell_nb', width: 90, align: 'left' },
        { label: 'Fax', name: 'fax_nb', width: 90, align: 'left' },
        { label: 'E-Mail', name: 'e_mail', width: 200, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Website', name: 'web_site', width: 200, align: 'left' },
        { label: 'Address', name: 'address', width: 200, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Remark', name: 're_mark', width: 200, align: 'left' },
    ],
    height: 400,
    width: $(".box-body").width() - 5,
    pager: '#listPager',
    rownumbers: true,
    rowList: [50, 100, 200],
    viewrecords: true,
    emptyrecords: "No data.",
    gridview: true,
    rownumbers: true,
    shrinkToFit: false,
    caption: 'Supplier Information',
    jsonReader:
     {
         root: "rows",
         page: "page",
         total: "total",
         records: "records",
         repeatitems: false,
         Id: "0"
     },
    autowidth: false,
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        var spno = row_id.spno;
        var sp_cd = row_id.sp_cd;
        var sp_nm = row_id.sp_nm;
        var e_mail = row_id.e_mail;
        var phone_nb = row_id.phone_nb;
        var web_site = row_id.web_site;
        var address = row_id.address;
        var cell_nb = row_id.cell_nb;
        var fax_nb = row_id.fax_nb;
        var changer_id = row_id.changer_id;
        var re_mark = row_id.re_mark;
        var bsn_tp = row_id.bsn_tp;
        var up_mn_cd = row_id.up_mn_cd;
        var mn_cd_full = row_id.mn_cd_full;

        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);

        $('#m_business_type :selected').text(bsn_tp);
        $('#m_spno').val(spno);
        $('#m_sp_cd').val(sp_cd);
        $('#m_sp_nm').val(sp_nm);
        $('#m_e_mail').val(e_mail);
        $('#m_phone_nb').val(phone_nb);
        $('#m_web_site').val(web_site);
        $('#m_address').val(address);
        $('#m_re_mark').val(re_mark);
        $('#m_changer_id').val(changer_id);
        $('#m_cell_nb').val(cell_nb);
        $('#m_fax_nb').val(fax_nb);

        

        
    },

});
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});


$("#searchBtn").click(function () {
    var code = $("#sp_cd").val().trim();
    var name = $("#sp_nm").val().trim();
    var bsn_search = $("#bsn_search").val();

    $.ajax({
        url: "/standard/SearchsupplierInfo",
        type: "GET",
        dataType: "json",
        data: {
            codeData: code,
            nameData: name,
            bsn_searchData: bsn_search,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        },
        error: function (result) {
            alert('Not found');
        }
    });
});




$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            type: 'pdf',
            fileName: "supplierInfo",
            pdfFontSize: '7',
            headings: true,
            bootstrap: true,
            footers: true,
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            type: 'xls',
            fileName: "supplierInfo",
            escape: false,
            headings: true,
            bootstrap: true,
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            headings: true,
            bootstrap: true,
            footers: true,
            fileName: "supplierInfo",
            escape: false,
        });
    });
});

$("#tab_1").on("click", "a", function (event) {
    $("#list").trigger("reloadGrid");
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


$("#m_save_but").click(function () {
    var params = new Object();

        params.spno = $('#m_spno').val();
        params.sp_cd = $('#m_sp_cd').val();
        params.sp_nm = $('#m_sp_nm').val();
        params.e_mail = $('#m_e_mail').val();
        params.phone_nb = $('#m_phone_nb').val();
        params.web_site = $('#m_web_site').val();
        params.address = $('#m_address').val();
        params.re_mark = $('#m_re_mark').val();
        params.fax_nb = $('#m_fax_nb').val();
        params.cell_nb = $('#m_cell_nb').val();
        params.bsn_tp = $('#m_business_type').val();

    $.ajax({
        url: "/standard/updateSupplierInfo",
        type: "PUT",
        dataType: "json",
        data: params,
        success: function (data) {
            if (data.result == true) {
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });

});

$("#c_save_but").click(function () {
    var params = new Object();

    if ($("#c_sp_cd").val().trim() == "") {
        alert("Please enter the code.");
        $("#c_sp_cd").val("");
        $('#c_sp_cd').focus();
        return false;
    }
    if ($("#c_sp_nm").val().trim() == "") {
        alert("Please enter the Name.");
        $("#c_sp_nm").val("");
        $("#c_sp_nm").focus();
        return false;
    }
    if ($("#c_e_mail").val().trim() == "") {
        alert("Please enter the E-mail.");
        $("#c_e_mail").val("");
        $("#c_e_mail").focus();
        return false;
    }
    if ($("#c_phone_nb").val().trim() == "") {
        alert("Please enter the Phone Number.");
        $("#c_phone_nb").val("");
        $("#c_phone_nb").focus();
        return false;
    }
    if ($("#c_web_site").val().trim() == "") {
        alert("Please enter the Website");
        $("#c_web_site").val("");
        $("#c_web_site").focus();
        return false;
    }
    if ($("#c_address").val().trim() == "") {
        alert("Please enter the Address.");
        $("#c_address").val("");
        $("#c_address").focus();
        return false;
    }

        params.spno = $('#c_spno').val().trim(),
        params.sp_cd = $('#c_sp_cd').val().trim(),
        params.sp_nm = $('#c_sp_nm').val().trim(),
        params.e_mail = $('#c_e_mail').val().trim(),
        params.phone_nb = $('#c_phone_nb').val().trim(),
        params.web_site = $('#c_web_site').val().trim(),
        params.address = $('#c_address').val().trim(),
        params.re_mark = $('#c_re_mark').val().trim(),
        params.fax_nb = $('#c_fax_nb').val().trim(),
        params.cell_nb = $('#c_cell_nb').val().trim(),
        params.changer_id = $('#c_changer_id').val().trim(),
        params.bsn_tp = $('#c_business_type').val().trim()

    else {
        $.ajax({
            url: "/standard/insertSupplierInfo",
            type: "POST",
            dataType: "json",
            data: params,
            success: function (response) {
                if (response.result == true) {
                    /*jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');*/
                    var id = response.vaule.spno;
                    $("#list").jqGrid('addRowData', id, response.vaule, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});
// Khai báo URL stand của bạn ở đây
//Get data for Select box
$.ajax({
    url: "/standard/GetAllCountries",
    type: "get",
    datatype: "json",
    success: function (data) {
        var html = "<option value='' disabled selected>Business Type</option>";
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $(".bsn").html(html);

        var html = "<option value='' selected>Business Type All</option>";
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $("#bsn_search").html(html);
        
    } 
});
