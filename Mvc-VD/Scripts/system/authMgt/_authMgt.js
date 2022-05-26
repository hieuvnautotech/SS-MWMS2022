// start gridAuthor
$(function () {
    $(document).ready(function () {
        $('.nav-link > p').addClass('text-white');
        $('.AuthorityManagement').addClass('text-warning');
        $('.System-menu').addClass('menu-open');

        _Getrole();
    });

    $("#list").jqGrid
        ({
            url: "/System/GetAuthor",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'Authority Name', name: 'atno', index: 'atno', align: 'center', hidden: true },
                { key: false, label: 'Authority Code', name: 'at_cd', index: 'at_cd', editable: true, sortable: true, align: 'center', width: '100' },
                { key: false, label: 'Authority Name', name: 'at_nm', index: 'at_nm', editable: true, sortable: true, width: '150' },
                { key: false, label: 'Role', name: 'role', index: 'role', hidden: true },
                { key: false, label: 'Role', name: 'role_nm', editable: true, sortable: true, width: '150' },
                { key: false, label: 'Description', name: 're_mark', index: 're_mark', editable: true, sortable: true, width: '200' },
                { key: false, label: 'Ues Y/N', name: 'use_yn', index: 'use_yn', editable: true, width: '50px', align: 'center', },
                { key: false, label: 'Reg Name', name: 'reg_id', index: 'reg_id', width: '100px', align: 'center' },
                { key: false, label: 'Reg Date', name: 'reg_dt', index: 'reg_dt', align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" }, width: '200' },
                { key: false, label: 'Change Name', name: 'Change Name', index: 'chg_id', width: '100px', align: 'center' },
                { key: false, label: 'Change Date', name: 'chg_dt', index: 'reg_dt', align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" }, width: '200' },
            ],

            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                row_id = $("#list").getRowData(selectedRowId);

                var at_cd = row_id.at_cd;
                var at_nm = row_id.at_nm;
                var re_mark = row_id.re_mark;
                var atno = row_id.atno;
                var use_yn = row_id.use_yn;

                $('#m_at_cd').val(at_cd);
                $('#m_at_nm').val(at_nm);
                $('#m_re_mark').val(re_mark);
                $('#m_use_yn').val(use_yn);
                $("#m_atno").val(atno);
                $("#m_role").val(row_id.role);

                $('#m_save_but').removeAttr('disabled');
                $('#del_save_but').removeAttr('disabled');
                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").removeClass("hidden");
                $("#tab_c1").addClass("hidden");
                $("#tab_c2").addClass("active");

                $.ajax({
                    url: '/system/getAuthorMbData',
                    type: 'Get',
                    datatype: 'json',
                    data: { at_cd: row_id.at_cd },
                    success: function (result) {
                        var rows = $("#list2").getDataIDs();
                        for (var i = 0; i < rows.length; i++) {
                            $('#list2').jqGrid('resetSelection');
                        }
                        for (var i = 0; i <= result.length - 1; i++) {
                            for (var j = 0; j <= rows.length; j++) {
                                var v_selected = $("#list2").getCell(rows[j], "userid");
                                if (v_selected == result[i]) {
                                    $('#list2').jqGrid('setSelection', rows[j], true);

                                }
                            }
                        }
                    }
                });

                var rows = $("#list2").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    $('#list2').jqGrid('resetSelection');
                }
                for (var i = 0; i < rows.length; i++) {
                    var v_selected = $("#list2").getCell(rows[i], "grade");
                    if (v_selected == row_id.at_nm) {
                        $('#list2').jqGrid('setSelection', rows[i], true);
                    }
                }

                $.ajax({
                    //url: '/system/getAuthorMenuData',
                    url: '/system/GetAuthorMenu',
                    type: 'Get',
                    datatype: 'json',
                    data: {
                        at_cd: at_cd
                    },
                    success: function (result) {

                        //debugger;
                        $("#list3").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                        //jQuery("#list3").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                        var rows = $("#list3").getDataIDs();
                        //for (var i = 0; i < rows.length; i++) {
                        //    $('#list3').jqGrid('resetSelection');
                        //    var rowData = $('#list3').jqGrid('getRowData', rows[i]);
                        //    rowData.role_selected = "";
                        //    $("#list3").setRowData(rows[i], rowData);
                        //}
                        for (var i = 0; i <= result.length - 1; i++) {
                            for (var j = 0; j <= rows.length; j++) {
                                if (result[i].role != null) {
                                    var v_selected = $("#list3").getCell(rows[j], "mn_cd");
                                    if (v_selected == result[i].mn_cd) {
                                        $('#list3').jqGrid('setSelection', rows[j], true);

                                        //var id = result.data[i].mn_cd;
                                        //$("#list3").setRowData(id, result.data[i]);
                                        //var rowData = $('#list3').jqGrid('getRowData', rows[j]);
                                        //rowData.role = result.data[i].dt_nm;
                                        //$("#list3").setRowData(result.data[i].mn_cd, rowData);
                                    }
                                }

                            }
                        }
                    }
                });


            },
            autowidth: false,
            multiselect: false,
            sortable: true,
            loadonce: true,
            pager: '#gridpagerAu',
            viewrecords: true,
            rowNum: 100,
            rowList: [50, 100, 200, 500, 1000],
            height: 250,
            width: $("#one").width() - 5,
            caption: 'Authority',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
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
});

//Add Data Function
$("#c_save_but").click(function () {
    if ($("#c_at_nm").val().trim() == "") {
        alert("Please enter your authority name");
        $("#c_at_nm").val("");
        $("#c_at_nm").focus();
        return false;
    }
    if ($("#c_re_mark").val().trim() == "") {
        alert("Please enter your re mark");
        $("#c_re_mark").val("");
        $("#c_re_mark").focus();
        return false;
    }
    //if ($("#c_role").val().trim() == "") {
    //    alert("Please enter your Role");
    //    $("#c_role").val("");
    //    $("#c_role").focus();
    //    return false;
    //}
    else {
        $.ajax({
            url: "/system/CreateAuthor",
            type: "Post",
            dataType: "json",
            data: {
                at_cd: $('#c_at_cd').val(),
                at_nm: $('#c_at_nm').val(),
                re_mark: $('#c_re_mark').val(),
                use_yn: $('#c_use_yn').val(),
                //role: $('#c_role').val(),
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.value.atno;
                    $("#list").jqGrid('addRowData', id, data.value, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    $("#list2").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    $("#list3").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });
    }
});

$("#m_save_but").click(function () {
    $.ajax({
        url: "/system/EditAuthor",
        type: "Put",
        dataType: "json",
        data: {
            at_cd: $('#m_at_cd').val(),
            at_nm: $('#m_at_nm').val(),
            re_mark: $('#m_re_mark').val(),
            use_yn: $('#m_use_yn').val(),
            atno: $("#m_atno").val(),
            //role: $("#m_role").val()
        },
        success: function (data) {
            if (data.result == true) {

                var id = data.value.atno;
                $("#list").setRowData(id, data.value, { background: "#d0e9c6" });
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });
});


$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
//End gridAuthor

//start Author and number connect
$(function () {
    $("#list2").jqGrid
        ({
            url: "/System/GetAuMember",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'User Id', name: 'userid', index: 'userid', editable: true, sortable: true, align: 'left', width: '150', frozen: true },
                { label: 'User Name', name: 'uname', index: 'uname', editable: true, sortable: true, width: '150' },
                { label: 'Nick Name', name: 'nick_name', index: 'nick_name', editable: true, sortable: true, width: '150' },

                { label: 'grade', name: 'grade', editable: true, width: '100px', hidden: true }
            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var rows = $("#list3").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var v_selected = $("#list3").getCell(rows[i], "selected");
                    if (v_selected == 'true') {
                        $('#list3').jqGrid('setSelection', rows[i], true);
                    }
                }
            },

            pager: '#PagegridAu_Mem',
            rowList: [10, 50, 100, 200],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Member Connection',
            emptyrecords: 'No Students Records are Available to Display',
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            height: 250,
            width: $("#two").width() - 5,
            autowidth: false,
            shrinkToFit: false,
            multiselect: true,
        });

    $('#list2').jqGrid('setGridWidth', $("#two").width());
    $(window).on("resize", function () {
        var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
        $("#list2").jqGrid("setGridWidth", newWidth, false);
    });

    $("#mbSave").on("click", function () {
        var selRowAuth = $('#list').jqGrid("getGridParam", "selrow");
        var selRowIds = $('#list2').jqGrid("getGridParam", "selarrrow");
        if (selRowAuth == null) {
            alert("Please select data from 'Authority & Member Connection' grid & 'Authority Name'");
        } else {

            var rowId = $('#list').jqGrid("getGridParam", "selrow");
            var v_at_cd = $("#list").jqGrid('getCell', rowId, 'at_cd');
            var v_at_nm = $("#list").jqGrid('getCell', rowId, 'at_nm');
            var v_re_mark = $("#list").jqGrid('getCell', rowId, 're_mark');
            var v_use_yn = $("#list").jqGrid('getCell', rowId, 'use_yn');
            var v_selRowIdUserArr = $('#list2').jqGrid("getGridParam", "selarrrow");

            var data = {
                at_cd: v_at_cd,
                at_nm: v_at_nm,
                re_mark: v_re_mark,
                use_yn: v_use_yn,
                selRowIdUserArr: v_selRowIdUserArr
            };
            $.ajax({
                url: '/system/saveAuthMbInfo',
                type: 'Post',
                dataType: 'json',
                data: { selRowIdMbJSON: JSON.stringify(data) },
                success: function (data) {
                    if (data.result == true) {
                        SuccessAlert("Authority Name " + v_at_nm + " have " + data.value + " User be authorization");
                    }
                    else {
                        ErrorAlert("Something is wrong !!");
                    }
                },
            });
        }

    });
});

$("#menuSave").on("click", function () {

    var selRowAuth = $('#list3').jqGrid("getGridParam", "selrow");
    var i, n, rowData, selRowIds = $("#list3").jqGrid("getGridParam", "selarrrow"), id = "", v_order_sts_cd = "";

    if (selRowAuth == null) {
        alert("Please select data from 'Authority & Menu Connection' grid & 'Authority Name'");
    }
    else {

        var v_selRowIdUserArr = $('#list3').jqGrid("getGridParam", "selarrrow");

        var at_cd = $("#m_at_cd").val();
        if (at_cd == "005") {
            alert("Không được phân quyền cho Admin");
            return false;
        }
        var list_role = [];
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];
            var v_role = getCellValue(selRowIds[i], 'role');

            if (v_role == undefined) {
                var giatri = "RL001";
                list_role.push(giatri);
            }
            else {
                list_role.push(v_role);
            }
        }

        $.ajax({
            url: '/system/saveAuthMenuInfo_addHome?mn_id=' + v_selRowIdUserArr + "&at_cd=" + at_cd + "&list_role=" + list_role,

            type: "post",
            dataType: 'json',

            success: function (response) {
                SuccessAlert(response.message);
            },
            error: function (response) {
                ErrorAlert(response.message);
            }
        });
    }
});

function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
//$("#menuSave").on("click", function () {

//    debugger;
//    var selRowAuth = $('#list').jqGrid("getGridParam", "selrow");

//    if (selRowAuth == null)
//    {
//        alert("Please select data from 'Authority & Menu Connection' grid & 'Authority Name'");
//    }
//    else {
//        var bo_check = "";
//        var rows = $("#list3").getDataIDs();
//        var v_selRowIdUserArr = $('#list3').jqGrid("getGridParam", "selarrrow");
//        for (var i = 0; i < rows.length; i++) {
//            if (v_selRowIdUserArr.indexOf(rows[i]) == -1) { bo_check += rows[i] + ",";}
//        }
//        if (bo_check.indexOf(",") != -1) { bo_check = bo_check.substr(0, bo_check.length - 1) }
//        var at_cd = $("#m_at_cd").val();
//        $.ajax({
//            url: '/system/saveAuthMenuInfo_addHome?mn_id=' + v_selRowIdUserArr + "&at_cd=" + at_cd + "&bo_check=" + bo_check,
//            type: "post",
//            dataType: "json",
//            success: function (response) {
//                SuccessAlert(response.message);
//            }, error: function (response    ) {
//                ErrorAlert(response.message);
//            }
//        });
//    }
//});
//$("#menuSave").on("click", function () {
//    var selRowAuth = $('#list').jqGrid("getGridParam", "selrow");
//    var selRowIds = $('#list3').jqGrid("getGridParam", "selarrrow");

//    if (selRowAuth == null) {
//        alert("Please select data from 'Authority & Menu Connection' grid & 'Authority Name'");
//    } else {
//        var bo_check = "";
//        var rows = $("#list3").getDataIDs();
//        var v_selRowIdUserArr = $('#list3').jqGrid("getGridParam", "selarrrow");
//        for (var i = 0; i < rows.length; i++) {
//            if (v_selRowIdUserArr.indexOf(rows[i]) == -1) { bo_check += rows[i] + ",";}
//        }
//        if (bo_check.indexOf(",") != -1) { bo_check = bo_check.substr(0, bo_check.length - 1) }
//        var at_cd = $("#m_at_cd").val();
//        $.ajax({
//            url: '/system/saveAuthMenuInfo_addHome?mn_id=' + v_selRowIdUserArr + "&at_cd=" + at_cd + "&bo_check=" + bo_check,
//            type: "post",
//            dataType: "json",
//            success: function (response) {
//                alert(response.message);
//            }, error: function (response    ) {
//                alert(response.message);
//            }
//        });
//    }
//});


$("#searchBtn_menu").click(function () {
    var mn_cd = $('#mn_cd').val().toString().trim();
    var mn_full = $('#mn_full').val().toString().trim();
    var at_cd = $("#m_at_cd").val();

    $.ajax({
        url: "/System/GetAuthorMenu",
        type: "Get",
        dataType: "json",
        data: {
            mn_cd: mn_cd,
            mn_full: mn_full,
            at_cd: at_cd,
        },
        success: function (result) {
            $("#list3").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            $.ajax({ 
                url: '/system/getAuthorMenuData',
                type: 'Get',
                datatype: 'json',
                data: { at_cd: $("#m_at_cd").val() },
                success: function (result) {
                    var rows = $("#list3").getDataIDs();
                    for (var i = 0; i < rows.length; i++) {
                        $('#list3').jqGrid('resetSelection');
                    }
                    for (var i = 0; i <= result.data.length - 1; i++) {
                        for (var j = 0; j <= rows.length; j++) {
                            var v_selected = $("#list3").getCell(rows[j], "mn_cd");
                            if (v_selected == result.data[i].mn_cd) {
                                $('#list3').jqGrid('setSelection', rows[j], true);

                            }
                        }
                    }
                }
            });
        }
    });

});

function _Getrole() {
    $.get("/system/getRole", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Role*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".role").html(html);
        }
    });
}

$("#searchBtn").click(function () {
    var code = $('#at_cd').val().toString().trim();
    var name = $('#at_nm').val().toString().trim();

    $.ajax({
        url: "/system/GetAuthor",
        type: "Get",
        dataType: "json",
        data: {
            code: code,
            name: name,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

$("#searchBtn_staff").click(function () {
    var userid = $('#userid').val().toString().trim();
    var uname = $('#uname').val().toString().trim();

    $.ajax({
        url: "/system/GetAuMember",
        type: "Get",
        dataType: "json",
        data: {
            userid: userid,
            uname: uname,
        },
        success: function (result) {
            $("#list2").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

//End Author and number connect

//Start Author and menu connect
$(function () {
    var at_cd = $("#m_at_cd").val();
    $("#list3").jqGrid
        ({
            url: "/System/GetAuthorMenu?at_cd=" + at_cd,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                //{ label: 'id', name: 'mnno', editable: false, sortable: true, align: 'center', width: '90', key: true },
                { label: 'Menu Code', name: 'mn_cd', index: 'mn_cd', editable: false, sortable: true, align: 'center', width: '90', key: true },
                { label: 'Menu Name', name: 'mn_nm', index: 'mn_nm', editable: false, sortable: true, width: '200' },

                {
                    key: false, label: 'Role', name: 'role', width: 80, align: 'center', editable: true, edittype: "select",
                    editoptions: {
                        value: { RL001: 'Read only', RL002: 'Edit' }
                    },
                    cellattr: function (rowId, value, rowObject, colModel, arrData) {
                        //debugger;
                        //if (rowObject.role != null) {
                        //    alert(rowObject.role);
                        //}
                        return rowObject.role;

                    },
                },


                { label: 'Full Name', name: 'mn_full', index: 'mn_full', editable: false, sortable: true, width: '350px' },
                { label: 'URL', name: 'url_link', index: 'url_link', editable: false, sortable: true, width: '250px' },
                { label: '', name: 'at_cd', label: 'Author code', hidden: true },

            ],
            onCellSelect: editRow,
            onSelectRow: function (rowid, selected, status, e) {
                var lastSelection = "";

                if (rowid && rowid !== lastSelection) {
                    var grid = $("#list3");
                    grid.jqGrid('editRow', rowid, { keys: true, focusField:3 });
                    lastSelection = rowid;


                }
            },
            editing: true,
            pager: '#PageGetJqAuMn',
            rowNum: 100,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            caption: 'Menu Connection',

            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            height: 250,
            width: null,
            autowidth: true,
            shrinkToFit: false,
            multiselect: true,
            gridComplete: function () {
                var at_cd = $("#m_at_cd").val();
                //if (at_cd != "") {
                //    $.ajax({
                //        url: '/system/getAuthorMenuData',
                //        type: 'Get',
                //        datatype: 'json',
                //        data: { at_cd: at_cd },
                //        success: function (result) {
                //            var rows = $("#list3").getDataIDs();
                //            for (var i = 0; i < rows.length; i++) {
                //                $('#list3').jqGrid('resetSelection');
                //            }
                //            for (var i = 0; i <= result.length - 1; i++) {
                //                for (var j = 0; j <= rows.length; j++) {
                //                    var v_selected = $("#list3").getCell(rows[j], "mn_cd");
                //                    if (v_selected == result[i]) {
                //                        $('#list3').jqGrid('setSelection', rows[j], true);

                //                    }
                //                }
                //            }
                //        }
                //    });
                //}
                //var rows = $("#list3").getDataIDs();
                //for (var i = 0; i < rows.length; i++) {
                //    var v_selected = $("#list3").getCell(rows[i], "selected");
                //    if (v_selected == 'true') {
                //        $('#list3').jqGrid('setSelection', rows[i], true);
                //    }
                //}

            },
        });

    var lastSel;
    function editRow(id) {
        if (id && id !== lastSel) {
            var grid = $("#list3");
            grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
            lastSel = id;
        }
    }
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#list').jqGrid('setGridWidth', $("#one").width());
    $(window).on("resize", function () {
        var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
        $("#list").jqGrid("setGridWidth", newWidth, false);
    });

    $('#list2').jqGrid('setGridWidth', $("#two").width());
    $(window).on("resize", function () {
        var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
        $("#list2").jqGrid("setGridWidth", newWidth, false);
    });

    $('#list3').jqGrid('setGridWidth', $("#three").width());
    $(window).on("resize", function () {
        var newWidth = $("#list3").closest(".ui-jqgrid").parent().width();
        $("#list3").jqGrid("setGridWidth", newWidth, false);
    });

    $("#tab_1").on("click", "a", function (event) {
        $("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        $("#list2").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        $("#list3").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        document.getElementById("form1").reset();
        $("#tab_2").removeClass("active");
        $("#tab_1").addClass("active");
        $("#tab_c2").removeClass("active");
        $("#tab_c1").removeClass("hidden");
        $("#tab_c2").addClass("hidden");
        $("#tab_c1").addClass("active");

    });
    $("#tab_2").on("click", "a", function (event) {

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
    $('#list3').jqGrid('setGridWidth', $("#box-body-3").width());
    $(window).on("resize", function () {
        var newWidth = $("#list3").closest(".ui-jqgrid").parent().width();
        $("#list3").jqGrid("setGridWidth", newWidth, false);
    });
});



//Don't use
function saveAuthMemberInfo() {
    var rowId = $('#list').jqGrid("getGridParam", "selrow");
    var v_at_cd = $("#list").jqGrid('getCell', rowId, 'at_cd');
    var v_re_mark = $("#list").jqGrid('getCell', rowId, 're_mark');
    var v_use_yn = $("#list").jqGrid('getCell', rowId, 'use_yn');
    console.log("rowId : " + rowId + ", v_at_cd : " + v_at_cd);

    var i, selRowIds = $('#list2').jqGrid("getGridParam", "selarrrow"), n, rowData;
    //console.log("selRowIds : "+selRowIds+", i : "+i);
    var ret = "";
    var v_rnum = "";
    var v_userid = "";

    var last_rnum = $("#list2").getCell(selRowIds[selRowIds.length - 1], "rnum");
    //console.log("last_rnum : "+last_rnum);

    for (i = 0, n = selRowIds.length; i < n; i++) {
        v_rnum = selRowIds[i];
        ret = $("#list2").jqGrid('getRowData', selRowIds[i]);
        v_rnum = ret.rnum;
        v_userid = ret.userid;
        $.ajax({
            type: 'post',
            url: '/System/saveAuthMemberInfo',
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            dataType: 'text',
            cache: false,
            data: JSON.stringify({
                userid: v_userid,
                at_cd: v_at_cd,
                re_mark: v_re_mark,
                use_yn: v_use_yn
            }),
            async: false,
            complete: function (data) {
                $("#list2").setGridParam({ url: "/system/GetAuMember?" + "at_cd=" + obj.at_cd, datatype: "json" }).trigger("reloadGrid");
            },
            error: function (data) {
                alert("The ID is already exists");
            }
        });
    }

}
