
Position_list = [];
$("#form1").validate({
    rules: {
        "c_userid": {
            required: true,
        },
        "c_uname": {
            required: true,
        },
        "position": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "m_userid": {
            required: true,
        },
        "m_uname": {
            required: true,
        },
        "position": {
            required: true,
        },
    },
});

$.ajax({
    async: false,
    url: "/DevManagement/getposition",
    type: "GET",
    dataType: "json",
    data: {},
    success: function (data)
    {
        $.each(data, function (key, item)
        {
            Position_list.push(item);
        });
    },
});

function getDatePicker()
{
    $(`#c_birthday`).datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true,
        "changeYear": true
    });
    $(`#c_joinDate`).datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true,
        "changeYear": true
    });
    $(`#m_birthday`).datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true,
        "changeYear": true
    });
    $(`#m_joinDate`).datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true,
        "changeYear": true
    });
}

$("#list").jqGrid({
    url: '/DevManagement/userInfoData_staff',
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
        { name: "userid", label: "No.", key: true },
        { name: "uname", width: 200, align: "left", label: "User Name", resizable: true },
        { name: "position_cd", width: 200, align: "left", label: "Position", resizable: true, formatter: possitionData },
        { name: "reg_id", width: 100, align: "left", label: "Create User", resizable: true },
        { name: "reg_dt", width: 160, align: "center", label: "Create Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { name: "chg_id", width: 100, align: "left", label: "Change User", resizable: true },
        { name: "chg_dt", width: 160, align: "center", label: "Change Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { name: "barcode", hidden: true },
        { name: "birth_dt", hidden: true },
        { name: "join_dt", hidden: true },
        { name: "depart_cd", hidden: true },
        { name: "gender", hidden: true },
    ],
    sortable: true,
    loadonce: true,
    gridview: true,
    caption: 'Staff information',
    pager: '#pager',
    rowList: [50, 100, 200, 500],
    viewrecords: true,
    rownumbers: true,
    height: 350,
    width: $(".box-body").width() - 5,
    shrinkToFit: false,
    autowidth: false,
    multiselect: true,
    gridComplete: function ()
    {
        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++)
        {
            var userid = $("#list").getCell(rows[i], "userid");
            var giatri = $('#remark_color').val();
            if (userid == giatri)
            {
                $("#list").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
            }
        }
    },
    loadComplete: function ()
    {
        var rows = $("#list").getDataIDs();
        for (var rowID of rows)
        {
            for (var id of idList)
            {
                if (rowID == id)
                {
                    $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    break;
                }
            }
        }

    },
    onSelectRow: function (rowid, status, e)
    {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        var row_id = $("#list").getRowData(selectedRowId);

        var birthDay = row_id.birth_dt;
     //   birthDay = birthDay.replace(/(\d{4})(\d{2})(\d{2})/g, '$1-$2-$3');
        var joinDay = row_id.join_dt;
    //    joinDay = joinDay.replace(/(\d{4})(\d{2})(\d{2})/g, '$1-$2-$3');

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");

        $("#m_userid").val(row_id.userid);
        $("#m_uname").val(row_id.uname);

        $("#m_userCode").val(row_id.barcode);
        $("#m_birthday").val(birthDay);
        $("#m_gender").val(row_id.gender);
        $("#m_department").val(row_id.depart_cd);
        $("#m_joinDate").val(joinDay);

        $.each(Position_list, function (key, item)
        {
            if (row_id.position_cd == item.dt_nm) { $('#m_position').val(item.dt_cd); }
        });
    },
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());

$(window).on("resize", function ()
{
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

function possitionData(cellValue, options, rowdata, action)
{
    if (cellValue != null)
    {
        var html = '';
        $.each(Position_list, function (key, item)
        {
            if (cellValue == item.dt_cd) { html += item.dt_nm; }
        });
        return html;
    }
    else
    {
        return "";
    }
}

$.ajax({
    url: "/DevManagement/getposition",
    type: "GET",
    datatype: "json",
    success: function (data)
    {
        $(".position").empty();
        var html = '';
        html += '<option value="" selected="selected">*Position*</option>';
        $.each(data, function (key, item)
        {
            html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
        });
        $(".position").html(html);
    }
});

//Button Modify
$("#c_save_but").click(function ()
{
    if ($("#form1").valid() == true)
    {
        $.ajax({
            url: "/DevManagement/insertstaff",
            type: "POST",
            dataType: "json",
            data: {
                userid: $("#c_userid").val().trim(),
                uname: $("#c_uname").val().trim(),
                position: $('#c_position').val(),
                userCode: $('#c_userCode').val().trim(),
                birthDay: $('#c_birthday').val(),
                gender: $('#c_gender').val(),
                department: $('#c_department').val(),
                joinDay: $('#c_joinDate').val()
            },
            success: function (data)
            {
                if (data.result == true)
                {
                    var id = data.kq.mbid;
                    $("#list").jqGrid('addRowData', id, data.kq, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                    $('#remark_color').val(data.kq.userid);
                } else
                {
                    ErrorAlert(data.message);
                }
            },
            error: function (result)
            {
                alert('Create user fail. Please check again');
            }
        });
    }

});

$("#m_save_but").click(function ()
{
    if ($("#form2").valid() == true)
    {
        $.ajax({
            url: "/DevManagement/modifyStaff",
            type: "PUT",
            dataType: "json",
            data: {
                userid: $("#m_userid").val(),
                uname: $("#m_uname").val(),
                position: $('#m_position').val(),
                userCode: $('#m_userCode').val(),
                birthDay: $('#m_birthday').val(),
                gender: $('#m_gender').val(),
                department: $('#m_department').val(),
                joinDay: $('#m_joinDate').val()
            },
            success: function (data)
            {
                debugger;
                if (data.result == true)
                {
                    var id = data.kq.userid;
                    $("#list").setRowData(id, data.kq, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                    $('#remark_color').val(data.kq.userid);
                } else{
                    ErrorAlert(data.message);
                }
            },
        });
    }

});

$('#del_save_but').click(function ()
{
    $('#dialogDangerous').dialog('open');
});

$("#tab_1").on("click", "a", function (event)
{
    $("#list").trigger('reloadGrid');
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event)
{
    $("#list").trigger('reloadGrid');
    //$("#m_save_but").attr("disabled", true);
    //$("#del_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

// Search staff
$("#searchBtn").click(function ()
{
    var searchType = $('#searchType').val();
    var keywordInput = $('#keywordInput').val();
    var position = $('#s_position').val();

    $.ajax({
        url: "/DevManagement/SearchStaff",
        type: "GET",
        dataType: "json",
        traditional: true,
        data: {
            searchType: searchType,
            keywordInput: keywordInput,
            position: position,
        },
        success: function (data, st) {
            debugger;
            if (st == "success") {
                var showing = $('#list')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });

});

//---print qr-code
$("#barCodeBtn").on("click", function ()
{
    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0)
    {
        window.open("/DevManagement/_printQRUser?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no");
    }
    else
    {
        alert("Please select Grid.");
    }
});

////////////////////// UPLOAD EXCEL FILE //////////////////////////////////////////
var insertmaterial_tmp = [];
var json_object = "";
var staff_object = "";
var idList = [];
$("#imgupload").change(function (evt)
{
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    var staffData = [];
    reader.onload = function (event)
    {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName)
        {
            if (sheetName = "Trang_tính1")
            {
                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);

                for (var i = 0; i < XL_row_object.length; i++)
                {
                    var data_row_tmp = [];

                    (XL_row_object[i]["Mã code"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Mã code"]);
                    (XL_row_object[i]["Mã"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Mã"]);
                    (XL_row_object[i]["Họ và tên"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Họ và tên"]);
                    (XL_row_object[i]["Ngày sinh"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Ngày sinh"]);
                    (XL_row_object[i]["Giới tính"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Giới tính"]);
                    (XL_row_object[i]["Tên bộ phận hiện tại"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Tên bộ phận hiện tại"]);
                    (XL_row_object[i]["Ngày vào làm"] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Ngày vào làm"]);
                    (XL_row_object[i]["Chức danh "] == undefined) ? data_row_tmp.push("") : data_row_tmp.push(XL_row_object[i]["Chức danh "]);

                    excelData.push(data_row_tmp);
                    staffData.push(XL_row_object[i]);
                }
                json_object = JSON.stringify(excelData);
                staff_object = JSON.stringify(staffData);
            }

        });
    };

    reader.onerror = function (event)
    {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});



$("#uploadBtn").click(function ()
{
    $(`#loading`).show();
    if ($(`#imgupload`).val() == "")
    {
        ErrorAlert("Please select file to upload");
        $(`#loading`).hide();
        return false;
    }
    insertmaterial_tmp = [];

    if (staff_object != "") {

        var obj = JSON.parse(staff_object.toString());
        var length = obj.length;
        for (var i = 0; i <= length - 1; i++) {
            var tmp = [];
            var b = obj[i];
            var arr = Object.keys(b).map(function (key) { return b[key]; });
            for (j = 0; j < 9; j++) {
                tmp.push(arr[j]);
            }
            item = {
                BarCode: tmp[0],
                Id: tmp[1],
                Name: tmp[2],
                BirthDate: reformatDateString(tmp[3]),
                Gender: tmp[4],
                DepartmentName: tmp[5],
                JoinDate: reformatDateString(tmp[6]),
                PositionName: tmp[7],
                LocationCode: "staff",
            }
            insertmaterial_tmp.push(item);
        }

        $.ajax({
            url: `/DevManagement/InsertStaffByExcelFile`,
            type: `POST`,
            async: true,
            dataType: `json`,
            contentType: `application/json; charset=utf-8`,
            data: JSON.stringify(insertmaterial_tmp),
            traditonal: true,
            success: function (response) {
                debugger;
                $(`#list`).jqGrid('clearGridData').jqGrid().setGridParam({ url: `/DevManagement/userInfoData_staff`, datatype: `json` }).trigger(`reloadGrid`);
                if (response.result == true) {
                    idList = [];
                    for (var item of response.data) {
                        var id = item.userid;
                        idList.push(id);
                        var mbid = item.mbid;
                        $("#list").jqGrid('addRowData', mbid, item, 'first');
                        $("#list").setRowData(mbid, false, { background: "#d0e9c6" });
                    }
                    $(`#loading`).hide();
                    SuccessAlert(response.message);
                }
                else {
                    ErrorAlert(esponse.message);
                    $(`#loading`).hide();
                }

            },
        });
    }
});

function reformatDateString(s)
{
    var initial = s.split(/\//);
    if (initial[0] < 10)// month
    {
        initial[0] = `0` + initial[0];
    }

    if (initial[1] < 10)// day
    {
        initial[1] = `0` + initial[1];
    }

    var temp = [initial[0], initial[1], initial[2]].join(`/`);
    var date = new Date(temp);

    var dd = date.getDate();
    var mm = (date.getMonth() + 1);
    var yyyy = date.getFullYear();
    if (dd < 10)
    {
        dd = `0` + dd;
    }
    if (mm < 10)
    {
        mm = `0` + mm;
    }
    var fullDate = yyyy.toString().concat(mm.toString(), dd.toString());
    return fullDate;
}
//// GREAT FUNCTION TO REFORMAT DATETIME TO STRING
//function reformatDateString(s)
//{
//    var b = s.split(/\D/);
//    return b.reverse().join('');
//}

//DOCUMENT READY
$(function ()
{
    getDatePicker();
    GetAllDepartments();
});

//GET MACHINE TYPES
function GetAllDepartments()
{
    $.ajax({
        url: `/DevManagement/GetAllDepartments`,
        type: "GET",
        dataType: "json",
    })
        .done(function (data)
        {
            var html = '';
            if (data)
            {
                $.each(data, function (key, item)
                {
                    html += `<option value="${item.depart_cd}">${item.depart_nm}</option>`;
                });
                $("#c_department").html(html);
                $("#m_department").html(html);
                return;
            }
            else
            {
                ErrorAlert("Cannot find any department.");
                return;
            }
        })
        .fail(function ()
        {
            ErrorAlert("System Error.");
            return;
        });
}
