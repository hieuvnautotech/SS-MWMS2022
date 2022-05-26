$("#list").jqGrid({
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
        { label: 'Widthdfdsfdsfds', name: 'new_with', sortable: true, width: 80, align: 'right', cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
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
        { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntCellValue },

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
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);

        var byno = row_id.byno;
        var buyer_cd = row_id.buyer_cd;
        var buyer_nm = row_id.buyer_nm;
        var brd_nm = row_id.brd_nm;
        var web_site = row_id.web_site;
        var cell_nb = row_id.cell_nb;
        var address = row_id.address;
        var phone_nb = row_id.phone_nb;
        var e_mail = row_id.e_mail;
        var fax_nb = row_id.fax_nb;
        var use_yn = row_id.use_yn;
        var re_mark = row_id.re_mark;
        var logo = row_id.logo;

        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#tab_1").removeClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);

        $('#m_byno').val(byno);
        $('#m_logo1').val(logo);
        $('#m_buyer_cd').val(buyer_cd);
        $('#m_buyer_nm').val(buyer_nm);
        $('#m_brd_nm').val(brd_nm);
        $("#m_web_site").val(web_site);
        $("#m_cell_nb").val(cell_nb);
        $("#m_address").val(address);
        $("#m_phone_nb").val(phone_nb);
        $("#m_e_mail").val(e_mail);
        $("#m_fax_nb").val(fax_nb);
        $("#m_use_yn").val(use_yn);
        $("#m_re_mark").val(re_mark);

        if (logo != null) {
            $("#m_logo").html('<img src="../Images/BuyerImg/' + logo + '" style="height:50px" />');

        } else {
            $("#m_logo").html("");
        }
    },
    pager: '#gridpager',
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 400,
    width: $(".box-body").width() - 5,
    caption: 'Buyer Information',
    emptyrecords: 'No Data',
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
    if ($("#c_buyer_cd").val().trim() == "") {
        alert("Please enter your buyer code");
        $("#c_buyer_cd").val("");
        $("#c_buyer_cd").focus();
        return false;
    }
    if ($("#c_buyer_nm").val().trim() == "") {
        alert("Please enter your buyer name");
        $("#c_buyer_nm").val("");
        $("#c_buyer_nm").focus();
        return false;
    }

    var formData = new FormData();
    var files = $("#imgupload").get(0).files;

    formData.append("file", files[0]);
    formData.append("buyer_cd", $('#c_buyer_cd').val());
    formData.append("buyer_nm", $('#c_buyer_nm').val());
    formData.append("brd_nm", $('#c_brd_nm').val());
    formData.append("web_site", $('#c_web_site').val());
    formData.append("cell_nb", $('#c_cell_nb').val());
    formData.append("address", $('#c_address').val());
    formData.append("phone_nb", $('#c_phone_nb').val());
    formData.append("e_mail", $('#c_e_mail').val());
    formData.append("fax_nb", $('#c_fax_nb').val());
    formData.append("use_yn", $('#c_use_yn').val());
    formData.append("re_mark", $('#c_re_mark').val());

    $.ajax({
        type: 'POST',
        url: "/DevManagement/insertmaterial",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result = true) {
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert("Buyer was existing. Please checking again !");
            }
        }
    })
});
$("#m_save_but").click(function () {
    var formData = new FormData();
    var files = $("#imgupload2").get(0).files;
              
    formData.append("file", files[0]);
    formData.append("buyer_cd", $('#m_buyer_cd').val());
    formData.append("byno", $('#m_byno').val());
    formData.append("buyer_nm", $('#m_buyer_nm').val());
    formData.append("brd_nm", $('#m_brd_nm').val());
    formData.append("web_site", $('#m_web_site').val());
    formData.append("cell_nb", $('#m_cell_nb').val());
    formData.append("address", $('#m_address').val());
    formData.append("phone_nb", $('#m_phone_nb').val());
    formData.append("e_mail", $('#m_e_mail').val());
    formData.append("fax_nb", $('#m_fax_nb').val());
    formData.append("use_yn", $('#m_use_yn').val());
    formData.append("re_mark", $('#m_re_mark').val());
    formData.append("logo", $('#m_logo1').val());

    $.ajax({
        type: "POST",
        url: "/standard/Editbuyer",
        data:formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.result != 0) {
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                if (data.imgname != "") { $("#m_logo").html('<img src="../Images/BuyerImg/' + data.imgname + '" style="height:50px" />'); };
            }
            else {
                alert("Buyer Information was existing. Please checking again !");
            }
        }
    });
});
      
$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});

$("#searchBtn").click(function () {
    var sp_cd = $('#sp_cd').val().trim();
    var sp_nm = $('#sp_nm').val().trim();
    $.ajax({
        url: "/DevManagement/searchmaterial",
        type: "Get",
        dataType: "json",
        data: {
            buyer_cd: sp_cd,
            buyer_nm: sp_nm,
        },
        success: function (result) {
            if (result != null) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        }
    });

});
            
    function image_logo(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../Images/BuyerImg/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").addClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
        
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
    $("#m_logo").empty();
});
      
   
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'BusinessType_Infor',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'BusinessType_Infor',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'BusinessType_Infor',
            escape: false,
            headers: true,
        });
    });
});
 
         