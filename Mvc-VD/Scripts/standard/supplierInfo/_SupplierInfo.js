$grid = $("#list").jqGrid({
    url: '/standard/selectSupplierInfo',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'spno', name: 'spno', key: true, width: 80, align: 'center', hidden: true },
        { label: 'Supplier Code', name: 'sp_cd', sortable: true, width: 120 },
        { label: 'Name', name: 'sp_nm', width: 150, sorttype: false, sortable: true, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Supplier Type', name: 'bsn_tp', sortable: true, width: 100, align: 'left' },
        { label: 'Business type', name: 'bsn_tp', sortable: true, width: 100, align: 'left', hidden: true },
        { label: 'Changer', name: 'chg_id', sortable: true, width: 80, align: 'center' },
        { label: 'Tel', name: 'phone_nb', sortable: true, width: 90, align: 'left' },
        { label: 'Cell', name: 'cell_nb', sortable: true, width: 90, align: 'left' },
        { label: 'Fax', name: 'fax_nb', sortable: true, width: 90, align: 'left' },
        { label: 'E-Mail', name: 'e_mail', sortable: true, width: 200, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Website', name: 'web_site', sortable: true, width: 200, align: 'left' },
        { label: 'Address', name: 'address', sortable: true, width: 500, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
        { label: 'Remark', name: 're_mark', sortable: true, width: 200, align: 'left' },
    ],

    height: 400,
    width: $(".box-body").width() - 5,
    pager: '#listPager',
    rownumbers: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    viewrecords: true,
    emptyrecords: "No data.",
    gridview: true,
    rownumbers: true,
    shrinkToFit: false,
    loadonce: true,
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

        //$('#m_business_type :selected').text(bsn_tp);
        $('#m_business_type').val(bsn_tp);
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
               fileName: "Supplier Information  ",
               returnAsString: false
           }
       );
});

$('#excelBtn').click(function (e) {
    $("#list").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Supplier Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
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
    if ($("#form2").valid()) {
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
            success: function (response) {
                if (response.result == true) {
                    //var id = response.spno;
                    //$("#list").setRowData(id, response.value, { background: "#d0e9c6" });
                    jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(response.message);
                }
            },
            error: function (data) {
                alert("Supplier Infor was existing. Please checking again.");
            }
        });
    }
});

$("#c_save_but").click(function () {
    var params = new Object();

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
    params.bsn_tp = $('#c_business_type').val()

    if ($("#form1").valid()) {
        $.ajax({
            url: "/standard/insertSupplierInfo",
            type: "POST",
            dataType: "json",
            data: params,
            success: function (response) {
                if (response.result == true) {
                    var id = response.vaule.spno;
                    $("#list").jqGrid('addRowData', id, response.vaule, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(response.message);
                }
            },
            error: function (data) {
                alert("Supplier Infor was existing. Please checking again.");
            }
        });
    }
});
// Khai báo URL stand của bạn ở đây
//Get data for Select box
$.ajax({
    url: "/standard/GetAllCountries",
    type: "GET",
    datatype: "json",
    success: function (data) {
        var html = "<option value='' disabled selected>Supplier Type</option>";
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $(".bsn").html(html);

        var html = "<option value='' selected>Supplier Type</option>";
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $("#bsn_search").html(html);

    }
});

$("#form1").validate({
    rules: {
        "sp_cd": {
            required: true,
        },
        "sp_nm": {
            required: true,
        },
        "business_type": {
            required: true,
        },
        "phone_nb": {
            digits: true,
        },
        "e_mail": {
            email: true,
        },
        "web_site": {
            url: false,
        },
        "cell_nb": {
            digits: true,
        },
    },
});
$("#form2").validate({
    rules: {
        "sp_cd": {
            required: true,
        },
        "sp_nm": {
            required: true,
        },
        "phone_nb": {
            digits: true,
        },
        "e_mail": {
            email: true,
        },
        "web_site": {
            url: false,
        },
        "cell_nb": {
            digits: true,
        },
    },
});