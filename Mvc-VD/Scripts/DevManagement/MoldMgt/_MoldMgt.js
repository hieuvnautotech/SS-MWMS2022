$(function () {
    $grid = $("#Mold").jqGrid
       ({
           //url: "/DevManagement/MoldMgtData",
           datatype: 'json',
           mtype: 'GET',

           colModel: [
               { label: 'mdno', name: 'mdno', key: true, width: 40, align: 'center', hidden: true },
               { label: ' Code', name: 'md_no', width: 150, align: 'left' },
               { label: ' Name', name: 'md_nm', width: 300, align: 'left' },
               { label: 'Purpose ', name: 'purpose', width: 150 },
               { label: 'Barcode', name: 'barcode', width: 150, align: 'center', hidden: true },
               { label: 'Description', name: 're_mark', width: 150 },
               { label: 'Create User', name: 'reg_id', width: 100, align: 'center' },
               { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
               { label: 'Change User', name: 'chg_id', width: 100, align: 'center' },
               { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
           ],
           pager: "#MoldPager",
           viewrecords: true,
           rowNum: 50,
           rowList: [50, 100, 200, 500, 1000],
           height: 450,
           width: $(".box-body").width() - 5,
           caption: 'Mold Information',
           loadtext: "Loading...",
           emptyrecords: "No data.",
           //rownumbers: true,
           gridview: true,
           shrinkToFit: false,

           datatype: function (postData) { getDataOutBox(postData); },
           jsonReader:
               {
                   root: "rows",
                   page: "page",
                   total: "total",
                   records: "records",
                   repeatitems: false,
                   Id: "0"
               },
           ajaxGridOptions: { contentType: "application/json" },
           autowidth: true,
           loadonce: false,

           gridComplete: function () {
               var rows = $("#Mold").getDataIDs();
               for (var i = 0; i < rows.length; i++) {
                   var md_no = $("#Mold").getCell(rows[i], "md_no");
                   var giatri = $('#remark_color').val();
                   if (md_no == giatri) {
                       $("#Mold").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                   }
               }
           },

           onSelectRow: function (rowid, status, e) {
               $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
               var selectedRowId = $("#Mold").jqGrid("getGridParam", 'selrow');
               var row_id = $("#Mold").getRowData(selectedRowId);
               var md_no = row_id.md_no;
               var mdno = row_id.mdno;
               var md_nm = row_id.md_nm;
               var purpose = row_id.purpose;
               var re_mark = row_id.re_mark;

               $("#m_save_but").attr("disabled", false);
               $("#del_save_but").attr("disabled", false);
               $("#tab_1").removeClass("active");
               $("#tab_2").addClass("active");
               $("#tab_c1").removeClass("active");
               $("#tab_c2").removeClass("hidden");
               $("#tab_c1").addClass("hidden");
               $("#tab_c2").addClass("active");

               $("#m_md_no").val(md_no);
               $("#m_md_nm").val(md_nm);
               $("#m_purpose").val(purpose);
               $("#m_re_mark").val(re_mark);
               $("#m_mdno").val(mdno);
           },
       });
});

$(window).on("resize", function () {
    var newWidth = $("#Mold").closest(".ui-jqgrid").parent().width();
    $("#Mold").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {

    debugger;
    var md_no = $('#c_md_no').val().toUpperCase();
    var md_nm = $('#c_md_nm').val();
    var purpose = $('#c_purpose').val();
    var re_mark = $('#c_re_mark').val();

    if ($("#form1").valid() == true) {
        $.ajax({
            url: "/ActualWO/InsertMold",
            type: "Post",
            dataType: "json",
            data: {
                md_no: md_no,
                md_nm: md_nm,
                purpose: purpose,
                re_mark: re_mark,

            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.kq.mdno;
                    $("#Mold").jqGrid('addRowData', id, data.kq, 'first');
                    $("#Mold").setRowData(id, false, { background: "#d0e9c6" });
                    var c_md_no = md_no; 
                    $('#remark_color').val(c_md_no.toUpperCase());
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
            }
        },

        });
    }
});

$("#m_save_but").click(function () {
    var mdno = $('#m_mdno').val();
    var md_no = $('#m_md_no').val();
    var md_nm = $('#m_md_nm').val();
    var purpose = $('#m_purpose').val();
    var re_mark = $('#m_re_mark').val();
    if ($("#form2").valid() == true) {

        {
            $.ajax({
                url: "/ActualWO/UpdateMold",
                type: "get",
                dataType: "json",
                data: {
                    mdno: mdno,
                    md_no: md_no,
                    md_nm: md_nm,
                    purpose: purpose,
                    re_mark: re_mark,
                },
                success: function (data) {
                    if (data.result == true) {
                        var m_md_no = md_no;
                        $('#remark_color').val(m_md_no.toUpperCase());
                        var id = data.data.mdno;
                        $("#Mold").setRowData(id, data.data, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    }


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
    document.getElementById("form2").reset();
    $("#Mold").trigger('reloadGrid');
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

});

$('#excelBtn').click(function () {
    var md_no = $("#s_md_no").val().trim();
    var md_nm = $("#s_md_nm").val().trim();
    $('#exportData').attr('action', "/ActualWO/ExportToExcelMold?md_no=" + md_no + "&md_nm=" + md_nm);
});

$('#htmlBtn').click(function (e) {
    $("#Mold").jqGrid('exportToHtml',
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
            fileName: "Mold Information  ",
            returnAsString: false
        }
        );
});

$("#searchBtn").click(function () {
    //var s_md_no = $('#s_md_no').val().trim();
    //var s_md_nm = $('#s_md_nm').val().trim();
    //$.ajax({
    //    url: "/DevManagement/searchMold",
    //    type: "get",
    //    dataType: "json",
    //    data: {
    //        s_md_no: s_md_no,
    //        s_md_nm: s_md_nm,
    //    },
    //    success: function (result) {
    //        $("#Mold").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
    //    }
    //});
    $("#list").clearGridData();

    $('.loading').show();
    var grid = $("#Mold");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getDataOutBox(pdata);
});

//$(document).ready(function (e) {
//    $('#pdfBtn').click(function (e) {
//        $('#Mold').tableExport({
//            type: 'pdf',
//            pdfFontSize: '12',
//            fileName: '',
//            escape: false,
//        });
//    });
//});
//$(document).ready(function (e) {
//    $('#excelBtn').click(function (e) {
//        $('#Mold').tableExport({
//            type: 'xls',
//            fileName: 'Dept_Info',
//            escape: false,
//        });
//    });
//});
//$(document).ready(function (e) {
//    $('#htmlBtn').click(function (e) {
//        $('#Mold').tableExport({
//            type: 'doc',
//            fileName: 'Dept_Info',
//            escape: false,
//        });
//    });
//});

$("#form1").validate({
    rules: {
        "md_no": {
            required: true,
        },
        "md_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "md_no": {
            required: true,
        },
        "md_nm": {
            required: true,
        },
    },
});

function getDataOutBox(pdata) {
    $(".loading").show();
    var md_no = $("#s_md_no").val().trim();
    var md_nm = $("#s_md_nm").val().trim();

    var params = new Object();
    if (jQuery('#Mold').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.md_no = md_no;
    params.md_nm = md_nm;

    $("#Mold").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
       //url: "/DevManagement/MoldMgtData",

        url: "/ActualWO/MoldMgtData", 
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#Mold")[0];
                grid.addJSONData(data);
                $(".loading").hide();
            }
        }
    });
};