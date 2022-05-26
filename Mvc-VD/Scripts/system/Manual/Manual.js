$grid = $("#noticeMgt").jqGrid
({
    url: '/system/Getnotice_menu',
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { name: "bno", width: 50, align: "left", label: "ID", sortable: true, hidden: true },
        { label: 'Menu Code', name: 'mn_cd', sortable: true, align: 'center' },
        { label: 'Language', name: 'lng_cd', sortable: true, align: 'center' },
        { name: "title", width: 200, align: 'left', label: "Menu Name", formatter: notice_popup, },
        { label: 'Create Date', name: 'reg_dt', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { name: "chg_id", width: 150, label: "Change Name", align: 'left' },
        { label: 'Change Date', name: 'chg_dt', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { name: "", width: 70, align: "center", label: "Detail", resizable: false, title: false, formatter: bntCellValue },
    ],
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#noticeMgt").jqGrid("getGridParam", 'selrow');
        row_id = $("#noticeMgt").getRowData(selectedRowId);
        var bno = row_id.bno;
        $('#s_bno').val(bno);
        $('#del_save_but').removeClass("disabled");
        //$(".manual_detail").click(function () {
        //    bno = $(this).data("detail");
        //    notice1(bno);
        //});
        //$(".manual_modify").click(function () {
        //    bno = $(this).data("modify");
        //    notice(bno);
        //});
    },
    pager: jQuery('#jqGridPager2'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    sortable: true,
    loadonce: true,
    height: 500,
    width: $(".boxA").width(),
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    caption: 'Manual Information',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    multiselect: false,
});

//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#poMgtGrid').jqGrid('setGridWidth', $(".boxA").width());
$(window).on("resize", function () {
    var newWidth = $("#poMgtGrid").closest(".ui-jqgrid").parent().width();
    $("#poMgtGrid").jqGrid("setGridWidth", newWidth, false);
});


function bntCellValue(cellvalue, options, rowobject) {
    var bno = rowobject.bno;
    var html = '<a href="#" class="manual_detail" onclick="notice1(' + bno + ');"><i class="fa fa-file fa-lg text-success"></i></a>';
    return html;
}
//Init Datepicker start
$("#start1").datepicker({
    format: 'mm/dd/yyyy',
});
//$('#end1').datepicker().datepicker('setDate', 'today');

// Init Datepicker end
$("#end1").datepicker({
    format: 'mm/dd/yyyy',
    "autoclose": true

});

$("#searchBtn1").click(function () {
    console.log($("#lng_cd").val());
    $.ajax({
        url: "/system/Searchmanu",
        type: "Get",
        dataType: "json",
        data: {
            title: $("#title").val(),
            mn_cd: $("#s_mn_cd").val(),
            lng_cd: $("#lng_cd").val(),
        },
        success: function (result) {
            $("#noticeMgt").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
function notice_popup(cellvalue, options, rowobject) {
    var bno = rowobject.bno;
    var html = "";
    if (cellvalue != null) {
        var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="notice(' + bno + ');">' + cellvalue + '</button>';
        //var html = '<a href="#" data-toggle="modal" style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" data-modify=' + bno + ' class="manual_modify"><p>' + cellvalue + '</p></a>';            
    }
    return html;
}

function notice(bno) {
    $('.dialog_MODIFY').dialog('open');
    //$('.dialog').dialog('open');
    viewdetailnotice(bno);
}
function notice1(bno) {
    $('.dialog3').dialog('open');
    viewdetail2(bno);
}
function viewdetail2(bno) {
    $("#bno").val(bno);
    $.get("/system/viewdetaillangue?bno=" + bno + "&lng_cd=EN", function (data) {
        $.each(data, function (key, item) {
            $(".title_w").html('Writer');
            $(".title_s").html('Subject');
            $(".title_c").html('Content');
            $('#reg1').val(item.reg_id);
            $('#tieu_de1').val(item.title);
            $('#content1').html(item.content);
        });
    });

}
function viewdetailnotice(bno) {
    $.get("/system/viewdetailnotice?bno=" + bno, function (data) {
        console.log(data.lng_cd);
        $("#m_lng_cd").val(data.lng_cd);
        $('#m_title').val(data.title);
        $('#m_mn_cd').val(data.mn_cd);
        $('#m_content').summernote('code', data.content);
        $('#m_content').summernote({ focus: true });
    });

}
$("#tab_1").on("click", "a", function (event) {
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {

    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

//function TreeFilterChanged2() {
//    $('#form2').submit();
//}

