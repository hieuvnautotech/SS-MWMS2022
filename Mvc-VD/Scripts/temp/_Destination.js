$(function () {
    var row_id, row_id2;
    $grid = $("#destinationGrid").jqGrid
    ({
       url: "/SalesMgt/getDestinationData",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'id', name: 'lctno', width: 60, align: 'right', hidden: true },
           { label: 'Location Code', name: 'lct_cd', width: 150, align: 'center' },
           { label: 'Location Name', name: 'lct_nm', width: 250, align: 'left' },
           { label: 'Full Name', name: 'mn_full', width: 400, align: 'left' },
           { label: 'up_lct_cd', name: 'up_lct_cd', width: 10, align: 'left', hidden: true },
           { label: 'level_cd', name: 'level_cd', width: 10, align: 'left', hidden: true },

           { key: false, label: 'Level', name: 'level_cd', width: 60, align: 'center' },
           { key: false, label: 'Use Y/N', name: 'use_yn', width: 100, align: 'center' },
           { key: false, label: 'Remark', name: 're_mark', width: 200, align: 'left' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       gridComplete: function () {
           var rows = $("#destinationGrid").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_use_yn = $("#destinationGrid").getCell(rows[i], "use_yn");
               if (v_use_yn == "N") {
                   $("#destinationGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
               }
           }
       },

       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#destinationGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#destinationGrid").getRowData(selectedRowId);
           var lctno = row_id.lctno;
           var lct_nm = row_id.lct_nm;
           var use_yn = row_id.use_yn;
           var mn_full = row_id.mn_full;
           var up_lct_cd = row_id.up_lct_cd;
           var level_cd = row_id.level_cd;
           var re_mark = row_id.re_mark;

           document.getElementById("form2").reset();
           $("#m_save_but").attr("disabled", false);
           $("#del_save_but").attr("disabled", false);
           $('#m_lct_nm').val(lct_nm);
           $("#m_use_yn").val(use_yn);
           $('#m_re_mark').val(re_mark);
           $('#m_lctno').val(lctno);
           $('#c_lctno').val(lctno);
           $('#m_mn_full').val(mn_full);
           $('#m_level_cd').val(level_cd);
           $('#m_up_lct_cd').val(up_lct_cd);

           if (row_id.level_cd == "005") {
               $("#c_re_mark").prop("disabled", true);
               $("#c_lct_nm").prop("disabled", true);
               $("#c_save_but").prop("disabled", true);
               $("#c_reset_but").prop("disabled", true);
           } else {
               $("#c_re_mark").prop("disabled", false);
               $("#c_lct_nm").prop("disabled", false);
               $("#c_save_but").prop("disabled", false);
               $("#c_reset_but").prop("disabled", false);
           }

       },

       pager: jQuery('#destinationGridPager'),
       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: 400,
       width: $(".box-body").width() - 5,
       autowidth: false,
       caption: 'Destination',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
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
    $('#destinationGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#destinationGrid").closest(".ui-jqgrid").parent().width();
        $("#destinationGrid").jqGrid("setGridWidth", newWidth, false);
    });

    function fun_check1() {
        if ($("#c_lct_nm").val().trim() == "") {
            alert("Please enter Location Name");
            $("#c_lct_nm").val("");
            $("#c_lct_nm").focus();
            return false;
        }
        var selRowId = $('#destinationGrid').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Location on the grid.");
            return false;
        }
        return true;
    }

    $("#c_save_but").click(function () {

        if (fun_check1() == true) {
            var lct_nm = $('#c_lct_nm').val();
            var use_yn = $('#c_use_yn').val();
            var re_mark = $('#c_re_mark').val();
            var lctno = $('#c_lctno').val();
            {
                $.ajax({
                    url: "/SalesMgt/insertDestination",
                    type: "get",
                    dataType: "json",
                    data: {
                        lct_nm: lct_nm,
                        use_yn: use_yn,
                        re_mark: re_mark,
                        lctno: lctno,
                    },
                    success: function (data) {
                        jQuery("#destinationGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    }
                });
            }
        }
    });
    $("#m_save_but").click(function () {

        var lct_nm = $('#m_lct_nm').val();
        var use_yn = $('#m_use_yn').val();
        var re_mark = $('#m_re_mark').val();
        var lctno = $('#m_lctno').val();
        var mn_full = $('#m_mn_full').val();
        var up_lct_cd = $('#m_up_lct_cd').val();
        var level_cd = $('#m_level_cd ').val();
        $.ajax({
            url: "/SalesMgt/updateDestination",
            type: "get",
            dataType: "json",
            data: {
                lct_nm: lct_nm,
                use_yn: use_yn,
                re_mark: re_mark,
                lctno: lctno,
                up_lct_cd: up_lct_cd,
                mn_full: mn_full,
                level_cd: level_cd,
            },
            success: function (data) {
                jQuery("#destinationGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
        });

    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });

    $("#searchBtn").click(function () {
        var lo_name = $('#lo_name').val();
        var level_name = $('#level_name').val();
        var start = $('#start').val();
        var end = $('#end').val();

        $.ajax({
            url: "/SalesMgt/searchDestination",
            type: "get",
            dataType: "json",
            data: {
                lo_name: lo_name,
                level_name: level_name,
                start: start,
                end: end,
            },
            success: function (data) {
                $("#destinationGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
            }
        });
    });
});

$('#c_lct_nm').on("click", function () {
    document.getElementById("form1").reset();
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
});
$("#m_save_but").attr("disabled", true);
$("#del_save_but").attr("disabled", true);

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'Destination_Infor',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'Destination_Infor',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'Destination_Infor',
            escape: false,
            headers: true,
        });
    });
});