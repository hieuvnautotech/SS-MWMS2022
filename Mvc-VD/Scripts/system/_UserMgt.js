//Create Grid
//$(document).ready(function (e) {
    
        $("#list").jqGrid({
            url: '/system/userInfoData',
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
                { name: "userid", width: 100, align: "left", label: "User Id", resizable: true },
                { name: "uname", width: 120, align: "left", label: "User Name", resizable: true },
                { name: "nick_name", width: 100, align: "left", label: "Nick Name", resizable: true },
                { name: "grade", width: 150, align: "left", label: "Grade", resizable: true },
                { name: "cel_nb", width: 100, align: "left", label: "Cell Number", resizable: true },
                { name: "e_mail", width: 200, align: "left", label: "E-Mail", resizable: true },
                { name: "memo", width: 200, align: "left", label: "Memo", resizable: true },
                { name: "reg_id", width: 100, align: "left", label: "Reg Name", resizable: true },
                { name: "reg_dt", width: 160, align: "center", label: "Reg Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { name: "chg_id", width: 100, align: "left", label: "Chage Name", resizable: true },
                { name: "chg_dt", width: 160, align: "center", label: "Chage Date", resizable: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { name: "upw", width: 100, align: "left", label: "upw", resizable: true, hidden: true },
                { name: "mail_yn", width: 100, align: "left", label: "mail_yn", resizable: true, hidden: true },
                { name: "scr_yn", width: 100, align: "left", label: "scr_yn", resizable: true, hidden: true },



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
                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);

                $('#m_p_grade').val(row_id.grade);
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
        $("#c_save_but").click(function () {
            if (fun_check1() == true) {
                $.ajax({
                    url: "/system/insertUser",
                    type: "post",
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
                    },
                    success: function (result) {
                        if (result == 0) {
                            jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            //alert('Create user success');
                        } else {
                            alert('User was existing. Please check again');
                        }
                        
                    },
                    error: function (result) {
                        alert('Create user fail. Please check again');
                    }
                });
            }
            
        });
        //Get data for Select box
        $.ajax({
            url: "/system/getgrade",
            type: "get",
            datatype: "json",
            success: function (result) {
                var html = "<option value='' disabled selected>*Grade*</option>";
                for (i = 1; i <= result.length; i++) {
                    html = html + "<option value='" + result[i - 1].at_nm + "'>" + result[i - 1].at_nm + "</option>";
                    $(".grade").html(html);
                }
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
        $("#m_save_but").click(function () {
            if (fun_check2() == true) {
                $.ajax({
                    url: "/system/modifyUser",
                    type: "post",
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


                    },
                    success: function (result) {
                        jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    },
                    error: function (result) {
                        alert('User is not existing. Please check again');
                    }
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


        // Init Datepicker start
        $("#start").datepicker({
            format: 'dd/mm/yyyy'
        });

        // Init Datepicker end
        $("#end").datepicker({
            format: 'dd/mm/yyyy'
        });

        // Search user
        $("#searchBtn").click(function () {
            var search = searchUser();
        });
    
        
