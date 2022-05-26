$(function () {

    $("#DeptGrid").jqGrid
   ({
       url: "/System/DeptData",
       datatype: 'json',
       mtype: 'GET',
       colNames: ['id', 'dpno', 'Department Code', 'Department Name', 'up_depart_cd', 'level_cd', 'Use Y/N', 'Description', 'order_no', 'del_yn', 'mn_full', 'Create Name',
           'Create Date', 'Update Name', 'Update Date'],
       colModel: [
           { name: "id", hidden: true },
           { name: 'dpno', key: true, width: 50, align: 'center', hidden: true },
           { name: 'depart_cd', width: 130, align: 'center', sort: true },
           { name: 'depart_nm', width: 150, align: 'center' },
           { name: 'up_depart_cd', hidden: true },
           { name: 'level_cd', hidden: true },
           { name: 'use_yn', width: 80, align: 'center' },
           { name: 're_mark', width: 300, align: 'left' },
           { name: 'order_no', hidden: true },
           { name: 'del_yn', hidden: true },
           { name: 'mn_full', hidden: true },
           { name: 'reg_nm', width: 90, align: 'center' },
           { name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { name: 'chg_nm', width: 90, align: 'center' },
           { name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       gridComplete: function () {
           var rows = $("#DeptGrid").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_use_yn = $("#DeptGrid").getCell(rows[i], "use_yn");
               if (v_use_yn == "N") {
                   $("#DeptGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
               }
           }
       },

       onSelectRow: function (rowid, status, e) {
           var selectedRowId = $("#DeptGrid").jqGrid("getGridParam", 'selrow');
           var row_id = $("#DeptGrid").getRowData(selectedRowId);
           var depart_cd = row_id.depart_cd;
           var depart_nm = row_id.depart_nm;
           var use_yn = row_id.use_yn;
           var re_mark = row_id.re_mark;
           var dpno = row_id.dpno;

           $("#m_save_but").removeClass("hidden");
           $("#c_save_but").removeClass("active");
           $("#m_save_but").addClass("active");
           $("#c_save_but").addClass("hidden");

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

           $("#m_save_but").removeClass("hidden");
           $("#c_save_but").removeClass("active");
           $("#m_save_but").addClass("active");
           $("#c_save_but").addClass("hidden");


       },
       pager: $('#DeptGridPager'),
       rowNum: 50,
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: '100%',
       caption: 'Department Information',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       pager: "#DeptGridPager",
       shrinkToFit: false,
       autowidth: true,
       multiselect: false,
   })

    $("#c_save_but").click(function () {
        var dpno = $('#c_dpno').val();
        var depart_cd = $('#c_depart_cd').val().trim();
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
                    jQuery("#DeptGrid").setGridParam({ rowNum: 20, datatype: "json" }).trigger('reloadGrid');
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
                    jQuery("#DeptGrid").setGridParam({ rowNum: 20, datatype: "json" }).trigger('reloadGrid');
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

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#DeptGrid').tableExport({
            type: 'pdf',
            pdfFontSize: '12',
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