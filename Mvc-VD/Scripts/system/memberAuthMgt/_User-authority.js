$(function () {

    $("#list").jqGrid
    ({
       url: "/System/GetMemberInfo",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'userid', name: 'userid', index: 'userid', sortable: true, align: 'left', width: '100px' },
           { key: false, label: 'User name', name: 'uname', index: 'uname', sortable: true, width: '100px', align: 'left' },
           { key: false, label: 'Grade', name: 'grade', index: 'grade', sortable: true, width: '100px', align: 'right' },
           { key: false, label: 'Tell Phone', name: 'tel_nb', index: 'tel_nb', editable: true, width: '100px' },
           { key: false, label: 'Email', name: 'e_mail', index: 'e_mail', editable: true, width: '150', align: 'left' },
           { key: false, label: 'lctno', name: 'lct_cd', index: 'lct_cd', editable: true, width: '120', align: 'center' ,hidden:true},
           { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', editable: true, width: '100px' },
           { key: false, label: 'Create Date', name: 'reg_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
           { key: false, label: 'Change User', name: 'chg_id', index: 'chg_id', editable: true, width: '100px' },
           { key: false, label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
       ],
       pager: '#gridpager',
       rowNum: 50,
       rowList: [50, 100, 200, 500, 1000],
       height: 200,
       width: $(".box-body").width() - 5,
       rownumbers: true,
       autowidth: false,
       shrinkToFit: false,
       viewrecords: true,
       caption: 'User Authority',
       onSelectRow: function (rowid, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
           row_id = $("#list").getRowData(selectedRowId);

           var userid = row_id.userid;
           $("#list2").clearGridData();
           $("#list2").setGridParam({ url: "/System/GetMemberLct?" + "userid=" + userid, datatype: "json" }).trigger("reloadGrid");
       },
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

    $("#list2").jqGrid
   ({
       url: "/System/GetMemberLct",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { name: 'rnum', label: 'RNUM', sortable: true, align: 'center', width: '35px', hidden: true },
           { key: true, name: 'lctno', label: 'lctno', sortable: true, align: 'right', width: '35px', hidden: true },
           { label: 'check_yn', name: 'check_yn', width: 60, align: 'center', hidden: true },
           { label: 'userid', name: 'userid', width: 100, hidden: true },
           { name: 'lct_cd', label: 'Location Code', sortable: true, width: '150', align: 'center' },
           { name: 'lct_nm', label: 'Location Name', sortable: true, width: '100px' },
           { label: 'UP Location Code', name: 'up_lct_cd', width: 100, align: 'center', hidden: true },
           { name: 'mn_full', label: 'Full Name', editable: true, width: '200' },
           { label: 'Location RFID', name: 'lct_rfid', width: 100, align: 'right' },
           { label: 'Loc BarCode', name: 'lct_bar_cd', width: 100, align: 'right' },
           { label: 'Levle NM', name: 'level_cd', width: 60, align: 'right', hidden: true },
           { label: 'ORDER NO', name: 'order_no', width: 60, align: 'right', hidden: true },
           { label: 'Shipping', name: 'sp_yn', width: 55, align: 'center' },
           { label: 'Movement', name: 'mv_yn', width: 55, align: 'center' },
           { label: 'Destroy', name: 'ds_yn', width: 55, align: 'center' },
           { label: 'Rental', name: 'rt_yn', width: 55, align: 'center' },
           { name: 'use_yn', label: 'Use Yn', editable: true, width: '50px', align: 'center' },
           { label: 'Remark', name: 're_mark', width: 140, hidden: true },
           { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', editable: true, width: '100px' },
           { key: false, label: 'Create Date', name: 'reg_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
           { key: false, label: 'Change User', name: 'chg_id', index: 'chg_id', editable: true, width: '100px' },
           { key: false, label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
           { name: 'selected', label: 'selected', editable: true, width: '100px', hidden: true }
       ],
       onSelectRow: function (rowid, selRowIds, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selRowIds = $('#list2').jqGrid("getGridParam", "selarrrow");
           console.log("excepSave - selRowIds : " + selRowIds);
       },
       rownumbers: true,
       pager: '#gridpager2',
       rowNum: 50,
       rowList: [50, 100, 200, 500, 1000],
       width: $("#box-body").width() - 5,
       height: 300,
       multiselect: true,
       gridComplete: function () {
           var rows = $("#list2").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_selected = $("#list2").getCell(rows[i], "selected");
               if (v_selected == 'true') {
                   $('#list2').jqGrid('setSelection', rows[i], true);
               }
           }
       },
       viewrecords: true,
       caption: 'Location Authority',
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
       autowidth: true,
   });

    $("#excepSave").on("click", function () {
        var selRowIds = $('#list2').jqGrid("getGridParam", "selarrrow");
        var rowId = $('#list').jqGrid("getGridParam", "selrow");
        var v_userid = $("#list").jqGrid('getCell', rowId, 'userid');
        $.ajax({
            type: 'post',
            url: '/system/delMemberLocationInfo',
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            dataType: 'text',
            data: JSON.stringify({ userid: v_userid }),
            async: false,
            cache: false,
            success: function (at_cd) {
                if (selRowIds.length > 0) {
                    saveMemberLocationInfo();
                    alert("The operation completed successfully.");
                }
            },
            complete: function () {
                if (selRowIds.length == 0) {
                    alert("The operation completed successfully.");
                }
            }
        });
    });



$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});
$('#list2').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
    $("#list2").jqGrid("setGridWidth", newWidth, false);
});

function saveMemberLocationInfo() {
    var rowId = $('#list').jqGrid("getGridParam", "selrow");
    var v_userid = $("#list").jqGrid('getCell', rowId, 'userid');
    console.log("rowId : " + rowId + ", v_userid : " + v_userid);

    var i, selRowIds = $('#list2').jqGrid("getGridParam", "selarrrow"), n, rowData;
    var ret = "";
    var v_rnum = "";
    var v_lctno = "";
    var v_lct_cd = "";
    var v_mn_cd = "";
    var v_re_mark = "";
    var v_use_yn = "";

    var last_rnum = $("#list2").getCell(selRowIds[selRowIds.length - 1], "rnum");

    for (i = 0, n = selRowIds.length; i < n; i++) {
        v_rnum = selRowIds[i];
        ret = $("#list2").jqGrid('getRowData', selRowIds[i]);

        v_rnum = ret.rnum;
        v_lctno = ret.lctno;
        v_lct_cd = ret.lct_cd
        v_re_mark = ret.re_mark;
        v_use_yn = ret.use_yn;

        $.ajax({
            type: 'post',
            url: '/System/saveMemberLocationInfo',
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            dataType: 'text',
            cache: false,
            data: JSON.stringify({
                lctno: v_lctno,
                rnum: v_rnum,
                userid: v_userid,
                lct_cd: v_lct_cd,
                re_mark: v_re_mark,
                use_yn: v_use_yn
            }),
            async: false,
            success: function (rnum) {
                if (rnum != null && rnum != "") {
                    $("#list2").setRowData(rnum, false, { background: "#FAED7D" });

                }
                if (last_rnum == rnum) {
                    alert("The operation completed successfully.");
                }
            },
        });
    }
}

});

