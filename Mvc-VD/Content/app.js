



$("#searchBtn").click(function () {
    var code = $("#sp_cd").val().trim();
    var name = $("#sp_nm").val().trim();
    var bsn_search = $("#bsn").val();

    $.ajax({
        url: "/standard/SearchsupplierInfo",
        type: "get",
        dataType: "json",
        data: {
            codeData: code,
            nameData: name,
            bsn_searchData: bsn_search,
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



function delDate(id) {
    $("#mpno").val(id);
    document.form3.submit();
}
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#jqGrid').tableExport({
            type: 'pdf',
            fileName: "supplierInfo",
            pdfFontSize: '7',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('#jqGrid').tableExport({
            type: 'xls',
            fileName: "supplierInfo",
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('#jqGrid').tableExport({
            type: 'doc',
            fileName: "supplierInfo",
            escape: false,
        });
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

    $("#m_save_but").removeClass("active");
    $("#c_save_but").removeClass("hidden");
    $("#m_save_but").addClass("hidden");
    $("#c_save_but").addClass("active");
    var selectedRowId = $("#commonGrid").getGridParam('selrow');

});
$("#tab_2").on("click", "a", function (event) {

    document.getElementById("form2").reset();
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
});


$("#m_save_but").click(function () {
    var selRowId = $('#list').jqGrid("getGridParam", "selrow");
    //console.log("selRowId : " + selRowId);
    if (selRowId == null) {
        alert("Please select the top menu on the grid.");
        return false;
    }


    $.ajax({
        url: "/standard/updateSupplierInfo",
        type: "get",
        dataType: "json",
        data: {
            spno: $('#m_spno').val(),
            sp_cd: $('#m_sp_cd').val(),
            sp_nm: $('#m_sp_nm').val(),
            e_mail: $('#m_e_mail').val(),
            phone_nb: $('#m_phone_nb').val(),
            web_site: $('#m_web_site').val(),
            address: $('#m_address').val(),
            fax_nb: $('#m_fax_nb').val(),
            cell_nb: $('#m_cell_nb').val(),
            changer_id: $('#m_changer_id').val(),
            re_mark: $('#m_re_mark').val(),
            bsn_tp: $('#m_business_type').val(),
        },
        success: function (data) {
            jQuery("#list").setGridParam({ rowNum: 20000, datatype: "json" }).trigger('reloadGrid');
        }


    });

});

$("#c_save_but").click(function () {

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

    else {

        $.ajax({
            url: "/standard/insertSupplierInfo",
            type: "get",
            dataType: "json",
            data: {
                spno: $('#c_spno').val(),
                sp_cd: $('#c_sp_cd').val(),
                sp_nm: $('#c_sp_nm').val(),
                e_mail: $('#c_e_mail').val(),
                phone_nb: $('#c_phone_nb').val(),
                web_site: $('#c_web_site').val(),
                address: $('#c_address').val(),
                re_mark: $('#c_re_mark').val(),
                fax_nb: $('#c_fax_nb').val(),
                cell_nb: $('#c_cell_nb').val(),
                changer_id: $('#c_changer_id').val(),
                bsn_tp: $('#c_business_type').val(),

            },
            success: function (data) {
                jQuery("#list").setGridParam({ rowNum: 20000, datatype: "json" }).trigger('reloadGrid');
            },
            error: function (data) {
                alert('The Code is the same. Please check again.');
            }

        });
    }
});
// Khai báo URL stand của bạn ở đây
var baseService = "/standard";
var countryUrl = baseService + "/GetAllCountries";
$(document).ready(function () {
    // load danh sách country
    _getCountries();
    $("#bsn").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getCountries() {
    $.get(countryUrl, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value=""></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#bsn").html(html);
        }
    });
}



$(document).ready(function () {
    // load danh sách country
    _getselectbsn();
    $("#m_business_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});



function _getselectbsn() {
    $.get(countryUrl, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value=""></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#m_business_type").html(html);
        }
    });
}

$(document).ready(function () {
    // load danh sách country
    _getcreatbsn();
    $("#c_business_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});



function _getcreatbsn() {
    $.get(countryUrl, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value=""></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#c_business_type").html(html);
        }
    });
}