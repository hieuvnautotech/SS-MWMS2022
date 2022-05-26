$grid = $("#mfInfoGrid").jqGrid
({
    url: "/standard/getManufacInfoData",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'mfno', name: 'mfno', width: 80, align: 'center', hidden: true },
        { key: false, label: 'Code', name: 'mf_cd', width: '100px', align: 'left' },
        { key: false, label: 'Name', name: 'mf_nm', width: '200', align: 'left' },
        { key: false, label: 'Brand Name', name: 'brd_nm', editable: true, width: '100px', align: 'left' },
        { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px', align: 'left' },
        { key: false, label: 'Logo', name: 'logo', editable: true, width: '100px', formatter: image_logo, align: 'center', exportcol: false },
        { key: false, label: 'Logo', name: 'logo', editable: true, width: '100px', hidden: true, exportcol: true },
        { key: false, label: 'Website', name: 'web_site', editable: true, width: '180', align: 'left' },
        { key: false, label: 'Phone Number', name: 'phone_nb', editable: true, width: '100px', align: 'right' },
        { key: false, label: 'Address', name: 'address', editable: true, width: '200', align: 'left' },
        { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px' },
        { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
        { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px' },
        { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },

    ],
    gridComplete: function () {
        var rows = $("#mfInfoGrid").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var mf_cd = $("#mfInfoGrid").getCell(rows[i], "mf_cd");
            var giatri = $('#remark_color').val();
            if (mf_cd == giatri) {
                $("#mfInfoGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#mfInfoGrid").jqGrid("getGridParam", 'selrow');
        row_id = $("#mfInfoGrid").getRowData(selectedRowId);

        var mfno = row_id.mfno;
        var mf_cd = row_id.mf_cd;
        var mf_nm = row_id.mf_nm;
        var brd_nm = row_id.brd_nm;
        var web_site = row_id.web_site;
        var address = row_id.address;
        var phone_nb = row_id.phone_nb;
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

        $('#m_mfno').val(mfno);
        $('#m_logo1').val(logo);
        $('#m_mf_cd').val(mf_cd);
        $('#m_mf_nm').val(mf_nm);
        $('#m_brd_nm').val(brd_nm);
        $("#m_web_site").val(web_site);
        $("#m_address").val(address);
        $("#m_phone_nb").val(phone_nb);
        $("#m_re_mark").val(re_mark);

        if (logo != null) {
            $("#m_logo").html('<img src="../Images/ManufactImg/' + logo + '" style="height:50px" />');

        } else {
            $("#m_logo").html("");
        }
    },
    pager: '#mfInfoGridpager',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 400,
    width: $(".box-body").width() - 5,
    sortable: true,
    loadonce: true,
    caption: 'Manufatory Information',
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
$('#mfInfoGrid').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#mfInfoGrid").closest(".ui-jqgrid").parent().width();
    $("#mfInfoGrid").jqGrid("setGridWidth", newWidth, false);
});


function fun_check1() {
    if ($("#c_mf_cd").val().trim() == "") {
        alert("Please enter your Code");
        $("#c_mf_cd").val("");
        $("#c_mf_cd").focus();
        return false;
    }
    if ($("#c_mf_nm").val().trim() == "") {
        alert("Please enter your Name");
        $("#c_mf_nm").val("");
        $("#c_mf_nm").focus();
        return false;
    }
    return true;
}

$("#c_save_but").click(function () {

    if ($("#form1").valid() == true) {
        var formData = new FormData();
        var files = $("#imgupload").get(0).files;
        formData.append("file", files[0]);
        formData.append("mf_cd", $('#c_mf_cd').val());
        formData.append("mf_nm", $('#c_mf_nm').val());
        formData.append("brd_nm", $('#c_brd_nm').val());
        formData.append("web_site", $('#c_web_site').val());
        formData.append("cell_nb", $('#c_cell_nb').val());
        formData.append("address", $('#c_address').val());
        formData.append("phone_nb", $('#c_phone_nb').val());
        formData.append("re_mark", $('#c_re_mark').val());

        $.ajax({
            type: 'POST',
            url: "/standard/insertManufacInfo",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data)
            {
                if (data.result1 == true)
                {
                    if (data.result == 0)
                    {
                        var c_mf_cd = $('#c_mf_cd').val();
                        $('#remark_color').val(c_mf_cd.toUpperCase());
                        jQuery("#mfInfoGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        SuccessAlert(data.message);
                    }
                    else
                    {
                        alert("Manufactory Infor was existing. Please checking again.");
                    }
                }
                else
                {
                    ErrorAlert(data.message);
                }
            }
        })
    }
});
$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var formData = new FormData();
        var files = $("#imgupload2").get(0).files;

        formData.append("file", files[0]);
        formData.append("mf_cd", $('#m_mf_cd').val());
        formData.append("mfno", $('#m_mfno').val());
        formData.append("mf_nm", $('#m_mf_nm').val());
        formData.append("brd_nm", $('#m_brd_nm').val());
        formData.append("web_site", $('#m_web_site').val());
        formData.append("address", $('#m_address').val());
        formData.append("phone_nb", $('#m_phone_nb').val());
        formData.append("e_mail", $('#m_e_mail').val());
        formData.append("re_mark", $('#m_re_mark').val());
        formData.append("logo", $('#m_logo1').val());

        $.ajax({
            type: "POST",
            url: "/standard/updateManufacInfo",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.result != 0) {
                    var m_mf_cd = $('#m_mf_cd').val();
                    $('#remark_color').val(m_mf_cd.toUpperCase());
                    jQuery("#mfInfoGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    if (data.imgname != "") { $("#m_logo").html('<img src="../Images/ManufactImg/' + data.imgname + '" style="height:50px" />'); };
                }
                else {
                    alert("Manufactory Information was existing. Please checking again !");
                }
            }
        });
    }


});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});


$("#searchBtn").click(function () {
    var mf_cd = $('#mf_cd').val().trim();
    var mf_nm = $('#mf_nm').val().trim();
    var brd_nm = $('#brd_nm').val().trim();
    $.ajax({
        url: "/standard/searchManufacInfo",
        type: "get",
        dataType: "json",
        data: {
            mf_cd: mf_cd,
            mf_nm: mf_nm,
            brd_nm: brd_nm
        },
        success: function (result) {

            $("#mfInfoGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

function image_logo(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../Images/ManufactImg/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}

$("#tab_1").on("click", "a", function (event) {
    $("#mfInfoGrid").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event) {
    $("#mfInfoGrid").trigger('reloadGrid');
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#m_logo").html("");
});


$('#htmlBtn').click(function (e) {
    $("#mfInfoGrid").jqGrid('exportToHtml',
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
            fileName: "Manufatory Information",
            returnAsString: false
        }
       );
});

$('#excelBtn').click(function (e) {
    $("#mfInfoGrid").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Manufactory Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: function _replStrFunc(v) {
                return v.replace(/(<([^>]+)>)/ig, '')
                .replace(/>/g, '&gt;')
                .replace(/[\x00-\x09\x0B\x0C\x0E-\x1F\x7F-\x9F]/g, '');
            },
        }
    );
});

$("#form1").validate({
    rules: {
        "mf_cd": {
            required: true,
        },
        "mf_nm": {
            required: true,
        },
        "brd_nm": {
            required: true,
        },
        "phone_nb": {
            digits: true,
        },

        "web_site": {
            url: false,
        },
    },
});
$("#form2").validate({
    rules: {
        "mf_cd": {
            required: true,
        },
        "mf_nm": {
            required: true,
        },
        "brd_nm": {
            required: true,
        },
        "phone_nb": {
            digits: true,
        },

        "web_site": {
            url: false,
        },
    },
});

