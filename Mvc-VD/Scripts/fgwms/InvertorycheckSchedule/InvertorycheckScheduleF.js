$(function () {
   $("#list").jqGrid
   ({
       url: "/fgwms/GetCheckInvenScheduleF",
       mtype: 'GET',
       datatype: 'json',
       colModel: [
           { key: true, label: 'pvno', name: 'pvno', width: 50, hidden: true },
           { label: 'Code', name: 'pvn_cd', width: 180, align: 'center' },
           { label: 'Inventory Name', name: 'pvn_nm', width: 200, align: 'left' },
           { label: 'Start Date', name: 'start_dt', width: 150, align: 'center' },
           { label: 'End Date', name: 'end_dt', width: 150, align: 'center' },
           { label: 'Description', name: 're_mark', width: 150, align: 'left' },
           { label: 'User Y/N', name: 'use_yn', width: 100, align: 'center' },
           { label: 'Create Name', name: 'reg_id', index: 'reg_id', editable: true, width: '100px' },
           { label: 'Create date', name: 'reg_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Change Name', name: 'chg_id', index: 'chg_id', editable: true, width: '100px' },
           { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],

       onSelectRow: function (rowid, status, e) {

           var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
           row_id = $("#list").getRowData(selectedRowId);

           var pvno = row_id.pvno; 
           var pvn_cd = row_id.pvn_cd;
           var pvn_nm = row_id.pvn_nm;
           var start_dt = row_id.start_dt;
           var end_dt = row_id.end_dt;
           var re_mark = row_id.re_mark;
           var use_yn = row_id.use_yn;


           $('#m_pvno').val(pvno);
           $('#m_pvn_cd').val(pvn_cd);
           $('#m_pvn_nm').val(pvn_nm);
           $('#m_start_dt').val(start_dt);
           $('#m_end_dt').val(end_dt);
           $('#m_re_mark').val(re_mark);
           $('#m_use_yn').val(use_yn);

           $("#tab_1").removeClass("active");
           $("#tab_2").addClass("active");
           $("#tab_c1").removeClass("active");
           $("#tab_c2").removeClass("hidden");
           $("#tab_c1").addClass("hidden");
           $("#tab_c2").addClass("active");
           $("#tab_1").removeClass("active");
           $("#m_save_but").attr("disabled", false);
       },
       gridComplete: function () {
           var rows = $("#list").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_use_yn = $("#list").getCell(rows[i], "use_yn");
               if (v_use_yn == "N") {
                   $("#list").jqGrid('setRowData', rows[i], false, { background: 'rgb(213, 213, 213)' });
               }
           }
       },
       pager: "#gridpager",
       pager: jQuery('#gridpager'),
       viewrecords: true,
       rowList: [50, 100,200, 500,1000],
       height: "250",
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 50,
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       loadonce: true,
       viewrecords: true,
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },
   })
  
    $("#c_save_but").click(function () {
        var isValid = $('#form1').valid();
        if (isValid == false) {
            return false;
        }
           var pvn_nm = $('#c_pvn_nm').val();
            var start_dt = $('#c_start_dt').val();
            var end_dt = $('#c_end_dt').val();
            var use_yn = $('#c_use_yn').val();
            var re_mark = $('#c_re_mark').val();
            {
                $.ajax({
                    url: "/fgwms/insertScheduleF",
                    type: "get",
                    dataType: "json",
                    data: {
                        pvn_nm: pvn_nm,
                        start_dt: start_dt,
                        end_dt: end_dt,
                        use_yn: use_yn,
                        re_mark: re_mark,
                    },
                    success: function (data) {
                        var id = data.pvno;
                        $("#list").jqGrid('addRowData', id, data, 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    }
                });
            }
    });

    $("#m_save_but").click(function () {
        var isValid = $('#form2').valid();
        if (isValid == false) {
            return false;
        }
            var pvno = $('#m_pvno').val();
            var pvn_nm = $('#m_pvn_nm').val();
            var start_dt = $('#m_start_dt').val();
            var end_dt = $('#m_end_dt').val();
            var use_yn = $('#m_use_yn').val();
            var re_mark = $('#m_re_mark').val();
            $.ajax({
                url: "/fgwms/updateScheduleF",
                type: "get",
                dataType: "json",
                data: {
                    pvno: pvno,
                    pvn_nm: pvn_nm,
                    start_dt: start_dt,
                    end_dt: end_dt,
                    use_yn: use_yn,
                    re_mark: re_mark,
                },
                success: function (data) {
                    var id = data.pvno;
                    $("#list").setRowData(id, data, { background: "#d0e9c6" });
                }
            });
    });

    $("#searchBtn").click(function () {
        var pvn_nm = $('#pvn_nm').val().trim();
        var start = $('#start').val().trim();
        var end = $('#end').val().trim();
        $.ajax({
            url: "/fgwms/searchInventoryF",
            type: "get",
            dataType: "json",
            data: {
                pvn_nm: pvn_nm,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
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

    });
    $("#tab_2").on("click", "a", function (event) {
        $("#m_save_but").attr("disabled", true);
        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
    });
});


$("#c_start_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#c_end_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#m_start_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#m_end_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "checkInventorySchedule.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#list").jqGrid('exportToHtml',
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
                fileName: "checkInventorySchedule",
                returnAsString: false
            }
         );
    });

});
