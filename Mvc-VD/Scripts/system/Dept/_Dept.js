$(function () {
    $("#DeptGrid").jqGrid
    ({
        url: "/System/DeptData",
        datatype: 'json',
        mtype: 'GET',
        colModel: [
            { name: "id", label: "id", hidden: true },
            { name: 'dpno', label: "dpno", key: true, width: 50, align: 'center', hidden: true },
            { name: 'depart_cd', label: "Department Code", width: 130, sort: true },
            { name: 'depart_nm', label: "Department Name", width: 150, },
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
        gridComplete: function () {
            var rows = $("#DeptGrid").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var depart_cd = $("#DeptGrid").getCell(rows[i], "depart_cd");
                var giatri = $('#remark_color').val();
                if (depart_cd == giatri) {
                    $("#DeptGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },
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
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: $(".box-body").width() - 5,
        sortable: true,
        loadonce: true,
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
        if ($("#form1").valid() == true) {
            var dpno = $('#c_dpno').val();
            var depart_cd = $('#c_depart_cd').val().toUpperCase();
            var depart_nm = $('#c_depart_nm').val();
            var use_yn = $('#c_use_yn').val();
            var re_mark = $('#c_re_mark').val();
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
                    if (data.result == 0) {
                        var depart_cd = data.depart_cd;
                        $('#remark_color').val(depart_cd);
                        jQuery("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    } else {
                        alert('The Dept was existing. Please check again.');
                    }
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });

    $("#m_save_but").click(function () {
        if ($("#form2").valid() == true) {
            var depart_cd = $('#m_depart_cd').val().toUpperCase();
            var depart_nm = $('#m_depart_nm').val();
            var use_yn = $('#m_use_yn').val();
            var re_mark = $('#m_re_mark').val();
            var dpno = $('#m_dpno').val();
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
                    if (data.result != 0) {
                        var depart_cd = data.depart_cd;
                        $('#remark_color').val(depart_cd);
                        jQuery("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    } else {
                        alert('The Dept was not existing. Please check again.');
                    }


                }
            });
        }
    });

    $("#searchBtn").click(function () {
        var s_depart_cd = $('#s_depart_cd').val().trim();
        var s_depart_nm = $('#s_depart_nm').val().trim();

        $.ajax({
            url: "/system/searchDept",
            type: "get",
            dataType: "json",
            data: {
                depart_cd: s_depart_cd,
                depart_nm: s_depart_nm,

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

$('#excelBtn').click(function (e) {
    $("#DeptGrid").jqGrid('exportToExcel',
            options = {
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "Department.xlsx",
                mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                maxlength: 40,
                onBeforeExport: null,
                replaceStr: null
            }
        );
});

$('#htmlBtn').click(function (e) {
    $("#DeptGrid").jqGrid('exportToHtml',
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
            fileName: "Department",
            returnAsString: false
        }
        );
});

$("#form1").validate({
    rules: {
        "depart_cd": {
            required: true,
        },
        "depart_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "depart_cd": {
            required: true,
        },
        "depart_nm": {
            required: true,
        },
    },
});