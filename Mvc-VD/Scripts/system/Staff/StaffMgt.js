$("#m_joinday").datepicker({
    dateFormat: 'yy-mm-dd'
});

$("#m_birthday").datepicker({
    dateFormat: 'yy-mm-dd'
});

$("#c_joinday").datepicker({
    dateFormat: 'yy-mm-dd'
});

$("#c_birthday").datepicker({
    dateFormat: 'yy-mm-dd'
});
$(document).ready(function () {
    $.ajax({
        url: "/DevManagement/getgrade",
        type: "get",
        datatype: "json",
        success: function (data) {
            var html = "<option value='' disabled selected>*Grade*</option>";
            for (i = 1; i <= data.length; i++) {
                html = html + "<option value='" + data[i - 1].at_nm + "'>" + data[i - 1].at_nm + "</option>";
                $(".grade").html(html);
            }
        }
    });
});
Gender_list = [];
Department_list = [];
Position_list = [];
$.ajax({
    async: false,
    url: "/DevManagement/getgender",
    type: "get",
    dataType: "json",
    data: {},
    success: function (data) {
        $.each(data, function (key, item) {
            Gender_list.push(item);
        });
    },
});
$.ajax({
    async: false,
    url: "/DevManagement/GetAllDepartments",
    type: "Get",
    dataType: "json",
    data: {},
    success: function (data) {
        $.each(data, function (key, item) {
            Department_list.push(item);
        });
    },
});
$.ajax({
    async: false,
    url: "/DevManagement/getposition",
    type: "get",
    dataType: "json",
    data: {},
    success: function (data) {
        $.each(data, function (key, item) {
            Position_list.push(item);
        });
    },
});



$("#list").jqGrid({
    url: '/DevManagement/userInfoData',
    datatype: 'json',
    mtype: 'GET',
    jsonReader: {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    colModel: [
        { name: "userid", width: 100, align: "left", label: "User Id", resizable: true, key: true },
        { name: "uname", width: 200, align: "left", label: "User Name", resizable: true },
        { name: "nick_name", width: 100, align: "left", label: "Nick Name", resizable: true },
        { name: "grade", width: 100, align: "left", label: "Grade", resizable: true },
        { name: "upw", hidden: true },
        { name: "join_dt", width: 100, align: "left", label: "Join date", resizable: true },
        { name: "birth_dt", width: 100, align: "left", label: "Birthday", resizable: true },
        { name: "gender", width: 100, align: "left", label: "Gender", resizable: true, formatter: genderData },
        { name: "depart_cd", width: 150, align: "left", label: "Department", resizable: true, formatter: departData },
        { name: "position_cd", width: 200, align: "left", label: "Position", resizable: true, formatter: possitionData },
        { name: "cel_nb", width: 100, align: "left", label: "Cell Number", resizable: true },
        { name: "e_mail", width: 200, align: "left", label: "E-Mail", resizable: true },
        { name: "scr_yn", width: 100, align: "left", label: "Screen Alarm", align: 'center', resizable: true },
        { name: "mail_yn", width: 100, align: "left", label: "Notice Mail", align: 'center', resizable: true },
        { name: "memo", width: 200, align: "left", label: "Memo", resizable: true },
        { name: "reg_id", width: 100, align: "left", label: "Create User", resizable: true },
        { name: "reg_dt", width: 160, align: "center", label: "Create Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { name: "chg_id", width: 100, align: "left", label: "Change User", resizable: true },
        { name: "chg_dt", width: 160, align: "center", label: "Change Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },



    ],
    sortable: true,
    loadonce: true,
    gridview: true,
    caption: 'User information',
    pager: '#pager',
    rowList: [50, 100, 200, 500],
    viewrecords: true,
    rownumbers: true,
    height: 350,
    width: $(".box-body").width() - 5,
    shrinkToFit: false,
    autowidth: false,
    multiselect: true,
    gridComplete: function () {
        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var userid = $("#list").getCell(rows[i], "userid");
            var giatri = $('#remark_color').val();
            if (userid == giatri) {
                $("#list").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        var row_id = $("#list").getRowData(selectedRowId);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");

        $("#m_userid").val(row_id.userid);
        $("#m_uname").val(row_id.uname);
        $("#m_upw").val(row_id.upw);
        $("#m_nick_name").val(row_id.nick_name);
        $("#m_cel_nb").val(row_id.cel_nb);
        $("#m_e_mail").val(row_id.e_mail);
        $("#m_memo").val(row_id.memo);
        $('#m_p_grade').val(row_id.grade);
        $("#m_birthday").val(row_id.birth_dt);
        $("#m_joinday").val(row_id.join_dt);
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
        $.each(Gender_list, function (key, item) {
            if (row_id.gender == item.dt_nm) { $('#m_gender').val(item.dt_cd); }
        });
        $.each(Department_list, function (key, item) {
            if (row_id.depart_cd == item.dt_nm) { $('#m_department').val(item.dt_cd); }
        });
        $.each(Position_list, function (key, item) {
            if (row_id.position_cd == item.dt_nm) { $('#m_position').val(item.dt_cd); }
        });


        if (row_id.mail_yn == 'Y') {
            $('#m_mail_y').prop('checked', true);
        } else {
            $('#m_mail_n').prop('checked', true);
        };
        if (row_id.scr_yn == 'Y') {
            $('#m_scr_y').prop('checked', true);
        } else {
            $('#m_scr_n').prop('checked', true);
        };
    },
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());

$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

function genderData(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '';
        $.each(Gender_list, function (key, item) {
            if (cellValue == item.dt_cd) { html += item.dt_nm; }
        });
        return html;
    }
    else {
        return "";
    }
}

function departData(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '';
        $.each(Department_list, function (key, item) {
            if (cellValue == item.depart_cd) { html += item.depart_nm; }
        });
        return html;
    }
    else {
        return "";
    }
}

function possitionData(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '';
        $.each(Position_list, function (key, item) {
            if (cellValue == item.dt_cd) { html += item.dt_nm; }
        });
        return html;
    }
    else {
        return "";
    }
}

//Button Create
function fun_check1() {
    if (($("#c_userid").val().trim() == "") || ($("#c_upw").val().trim() == "") || ($("#c_p_grade").val() == null)) {
        alert("The Name or Password or Grade is blank");
        $("#c_userid").focus();
        return false;
    } else {
        return true;
    };
}

//Get data for Select box

$.ajax({
    url: "/DevManagement/GetAllDepartments",
    type: "Get",
    datatype: "json",
    success: function (data) {
        $(".department").empty();
        var html = '';
        html += '<option value="" selected="selected">*Department*</option>';
        $.each(data, function (key, item) {
            html += '<option value=' + item.depart_cd + '>' + item.depart_nm + '</option>';
        });
        $(".department").html(html);
    }
});
$.ajax({
    url: "/DevManagement/getposition",
    type: "get",
    datatype: "json",
    success: function (data) {
        $(".position").empty();
        var html = '';
        html += '<option value="" selected="selected">*Position*</option>';
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $(".position").html(html);
    }
});
$.ajax({
    url: "/DevManagement/getgender",
    type: "get",
    datatype: "json",
    success: function (data) {
        $(".gender").empty();
        var html = '';
        html += '<option value="" selected="selected">*Gender*</option>';
        $.each(data, function (key, item) {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $(".gender").html(html);
    }
});



//Button Modify
function fun_check2() {
    if (($("#m_userid").val().trim() == '') || ($("#m_upw").val().trim() == "") || ($("#m_p_grade").val() == null)) {
        alert("The Name or Password or Grade is blank");
        $("#m_userid").focus();
        return false;
    } else {
        return true;
    };
}

$("#c_save_but").click(function () {
    if (fun_check1() == true) {
        $.ajax({
            url: "/DevManagement/insertUser",
            type: "Post",
            dataType: "json",
            data: {
                userid: $("#c_userid").val(),
                uname: $("#c_uname").val(),
                upw: $("#c_upw").val(),
                nick_name: $("#c_nick_name").val(),
                cel_nb: $("#c_cel_nb").val(),
                e_mail: $("#c_e_mail").val(),
                memo: $("#c_memo").val(),
                grade: $('#c_p_grade').val(),
                scr_yn: $("input[name='c_scr_yn']:checked").val(),
                mail_yn: $("input[name='c_mail_yn']:checked").val(),
                joinday: $("#c_joinday").val(),
                birthday: $("#c_birthday").val(),
                gender: $('#c_gender').val(),
                department: $('#c_department').val(),
                position: $('#c_position').val(),
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.value.userid;
                    $("#list").jqGrid('addRowData', id, data.value, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                   //jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                } else {
                    ErrorAlert(data.message);
                }

            }
        });
    }

});

$("#m_save_but").click(function () {
    if (fun_check2() == true) {
        $.ajax({
            url: "/DevManagement/modifyUser",
            type: "Put",
            dataType: "json",
            data: {
                userid: $("#m_userid").val(),
                uname: $("#m_uname").val(),
                upw: $("#m_upw").val(),
                nick_name: $("#m_nick_name").val(),
                cel_nb: $("#m_cel_nb").val(),
                e_mail: $("#m_e_mail").val(),
                memo: $("#m_memo").val(),
                grade: $('#m_p_grade').val(),
                scr_yn: $("input[name='m_scr_yn']:checked").val(),
                mail_yn: $("input[name='m_mail_yn']:checked").val(),
                joinday: $("#m_joinday").val(),
                birthday: $("#m_birthday").val(),
                gender: $('#m_gender').val(),
                department: $('#m_department').val(),
                position: $('#m_position').val(),
            },
            success: function (data) {
                if (data.result == true) {
                    var userid = data.userid;
                    $('#remark_color').val(userid);
                    jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                } else {
                    ErrorAlert(data.message);
                }
            },
            
        });
    }

});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});

$("#tab_1").on("click", "a", function (event) {
    $("#list").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");


});
$("#tab_2").on("click", "a", function (event) {
    $("#list").trigger('reloadGrid');
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

});

// Search user
$("#searchBtn").click(function () {
    var searchType = $('#searchType').val().toString().trim();
    var keywordInput = $('#keywordInput').val().toString().trim();
    var department = $('#s_department').val().toString().trim();
    var position = $('#s_position').val().toString().trim();
    $.ajax({
        url: "/DevManagement/searchUser",
        type: "GET",
        dataType: "json",
        data: {
            searchType: searchType,
            keywordInput: keywordInput,
            department: department,
            position: position,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
        }
    });

});

////////////////////////////////////////////////////////////////////////////////////////////////////


//---print qr-code
$("#barCodeBtn").on("click", function () {
    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        window.open("/DevManagement/_printQRUser?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        alert("Please select Grid.");
    }
})

