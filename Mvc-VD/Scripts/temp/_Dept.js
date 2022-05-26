$(function () {
    

    $("#DeptGrid").jqGrid
    ({
        url: "/System/DeptData",
        datatype: 'json',
        mtype: 'GET',
        colModel: [
            { name: "id", label: "id", hidden: true },
            { name: 'dpno', label: "dpno", key: true, width: 50, align: 'center', hidden: true },
            { name: 'depart_cd', label: "Department Code", width: 130, align: 'center', sort: true },
            { name: 'depart_nm', label: "Department Name", width: 150, align: 'center' },
            { name: 'up_depart_cd', label: "up_depart_cd", hidden: true },
            { name: 'level_cd', label: "level_cd", hidden: true },
            { name: 'use_yn', label: "Use Y/N", width: 80, align: 'center' },
            { name: 're_mark', label: "Description", width: 300, align: 'left' },
            { name: 'order_no', label: "order_no", hidden: true },
            { name: 'del_yn', label: "del_yn", hidden: true },
            { name: 'mn_full', label: "mn_full", hidden: true },
            { name: 'reg_nm', label: "Create Name", width: 90, align: 'center' },
            { name: 'reg_dt', label: "Create Date", align: 'center', width: 150, formatter: 'date', formatoptions: { newformat: "Y-m-d H:i:s" } },
            { name: 'chg_nm', label: "Update Name", width: 90, align: 'center' },
            { name: 'chg_dt', label: "Update Date", align: 'center', width: 150, formatter: 'date', formatoptions: { newformat: "Y-m-d H:i:s" } },
        ],
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            if (iCol == 16) {
                $('.delete-btn').click(function () {
                    var dataDept = rowid;
                    $.ajax({
                        url: "/system/deleteDept",
                        type: "post",
                        dataType: "json",
                        data: {
                            dataDept: dataDept,
                        },
                        success: function (result) {
                            $("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            document.getElementById("form1").reset();
                            $("#tab_2").removeClass("active");
                            $("#tab_1").addClass("active");
                            $("#tab_c2").removeClass("active");
                            $("#tab_c1").removeClass("hidden");
                            $("#tab_c2").addClass("hidden");
                            $("#tab_c1").addClass("active");

                        },
                        error: function (result) {
                            alert('User is not existing. Please check again');
                        }
                    });
                });
            }
        },
        onSelectRow: function (rowid, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#DeptGrid").jqGrid("getGridParam", 'selrow');
            var row_id = $("#DeptGrid").getRowData(selectedRowId);
            var depart_cd = row_id.depart_cd;
            var depart_nm = row_id.depart_nm;
            var use_yn = row_id.use_yn;
            var re_mark = row_id.re_mark;
            var dpno = row_id.dpno;

            $("#m_depart_cd").val(depart_cd);
            $("#m_depart_nm").val(depart_nm);
            $("#m_use_yn").val(use_yn);
            $("#m_re_mark").val(re_mark);
            $("#m_dpno").val(dpno);
            $("#c_dpno").val(dpno);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");

            $("#m_save_but").removeAttr("disabled", false);
            $("#del_save_but").removeAttr("disabled", false);
        },
        pager: '#DeptGridPager',
        viewrecords: true,
        rowList: [10, 20, 30, 40, 50, 100],
        height: 300,
        width: $(".box-body").width() - 5,
        caption: 'Department Information',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        autowidth: false,
        multiselect: false,
    });

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#DeptGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#DeptGrid").closest(".ui-jqgrid").parent().width();
        $("#DeptGrid").jqGrid("setGridWidth", newWidth, false);
    });

    // Init Datepicker start
    $("#start1").datepicker({
        format: 'mm/dd/yyyy',
    });
    $('#end1').datepicker().datepicker('setDate', 'today');

    // Init Datepicker end
    $("#end1").datepicker({
        format: 'mm/dd/yyyy',
        "autoclose": true

    });

    $("#c_save_but").click(function () {
        var dpno = $('#c_dpno').val();
        var depart_cd = $('#c_depart_cd').val();
        var depart_nm = $('#c_depart_nm').val();
        var use_yn = $('#c_use_yn').val();
        var re_mark = $('#c_re_mark').val();


        if ($("#c_depart_cd").val().trim() == "") {
            alert("Please enter the Depart Code.");
            $("#c_depart_cd").val("");
            $('#c_depart_cd').focus();
            return false;
        }
        if ($("#c_depart_nm").val().trim() == "") {
            alert("Please enter the Depart Name.");
            $("#c_depart_nm").val("");
            $("#c_depart_nm").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/system/insertDept",
                type: "get",
                dataType: "json",
                data: {
                    depart_cd: depart_cd,
                    depart_nm: depart_nm,
                    use_yn: use_yn,
                    re_mark: re_mark,
                    dpno: dpno,
                },
                success: function (data) {
                    if (data.result) {
                        jQuery("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    } else {
                        alert('The Code is the same. Please check again.');
                    }
                    
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });

    $("#m_save_but").click(function () {

        var depart_cd = $('#m_depart_cd').val();
        var depart_nm = $('#m_depart_nm').val();
        var use_yn = $('#m_use_yn').val();
        var re_mark = $('#m_re_mark').val();
        var dpno = $('#m_dpno').val();

        if ($("#m_dpno").val().trim() == "") {
            alert("Please select Grid.");
            return false;
        }
        if ($("#m_depart_nm").val().trim() == "") {
            alert("Please enter the Depart Name");
            $("#m_depart_nm").val("");
            $("#m_depart_nm").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/system/updateDept",
                type: "get",
                dataType: "json",
                data: {
                    depart_cd: depart_cd,
                    depart_nm: depart_nm,
                    use_yn: use_yn,
                    re_mark: re_mark,
                    dpno: dpno,
                },
                success: function (data) {
                    jQuery("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
            });
        }
    });

    $("#searchBtn").click(function () {
        var s_depart_cd = $('#s_depart_cd').val().trim();
        var s_depart_nm = $('#s_depart_nm').val().trim();
        var start1 = $("#start1").val();
        var end1 = $("#end1").val();
        $.ajax({
            url: "/system/searchDept",
            type: "get",
            dataType: "json",
            data: {
                depart_cd: s_depart_cd,
                depart_nm: s_depart_nm,
                start1Data: start1,
                end1Data: end1,
            },
            success: function (result) {

                $("#DeptGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

});

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#DeptGrid").trigger('reloadGrid');
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event) {
    document.getElementById("form2").reset();
    $("#DeptGrid").trigger('reloadGrid');
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);

});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'pdf',
            pdfFontSize: '7',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'xls',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'doc',
            fileName: 'Dept_Info',
            escape: false,
        });
    });
});
