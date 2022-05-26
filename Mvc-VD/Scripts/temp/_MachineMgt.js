//Create Grid
$(function () {

    $("#MachineMgtGrid").jqGrid
   ({
       url: "/DevManagement/getMachineMgtData",
       datatype: 'json',
       mtype: 'GET',
       colNames: ['mmo', 'Type', 'Code', 'Name', 'Purpose', 'Barcode', 'Remark',
           'Create User', 'Create Date', 'Chage User', 'Chage Date'],
       colModel: [
           { key: true, label: 'mmo', name: "mno", width: 80, align: 'center', hidden: true },
           { key: false, label: 'Type', name: 'mc_type', width: 110 },
           { key: false, label: 'Code', name: 'mc_no', width: 150, align: 'center', sort: true },
           { key: false, label: 'Name', name: 'mc_nm', width: 150 },
           { key: false, label: 'Purpose', name: 'purpose', width: 200 },
           { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'center' },
           { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
           { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#MachineMgtGrid").jqGrid("getGridParam", 'selrow');
           var row_id = $("#MachineMgtGrid").getRowData(selectedRowId);
           var mno = row_id.mno;
           var mc_type = row_id.mc_type;
           var mc_no = row_id.mc_no;
           var mc_nm = row_id.mc_nm;
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

           $("#m_mc_type").val(mc_type);
           $("#m_mc_no").val(mc_no);
           $("#m_mc_nm").val(mc_nm);
           $("#m_purpose").val(purpose);
           $("#m_mno").val(mno);
           $("#m_re_mark").val(re_mark);
           $("#c_mno").val(mno);

           


       },
       pager: '#MachineMgtPager',
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: 450,
       width: $(".box-body").width() - 5,
       caption: 'Machine Information',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       autowidth: false,
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
    $('#MachineMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#MachineMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#MachineMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });
    function fun_check1() {
        if ($("#c_mc_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#c_mc_nm").val("");
            $("#c_mc_nm").focus();
            return false;
        }
        if ($("#c_mc_type").val().trim() == "") {
            alert("Please enter the Type.");
            $("#c_mc_type").val("");
            $("#c_mc_type").focus();
            return false;
        }
        return true;
    }
    $("#c_save_but").click(function () {
        if (fun_check1() == true) {

            var mno = $('#c_mno').val();
            var mc_type = $('#c_mc_type').val();
            var mc_no = $('#c_mc_no').val();
            var purpose = $('#c_purpose').val();
            var mc_nm = $('#c_mc_nm').val();
            var re_mark = $('#c_re_mark').val();

            {
                $.ajax({
                    url: "/DevManagement/insertMachineMgt",
                    type: "get",
                    dataType: "json",
                    data: {
                        mno: mno,
                        mc_type: mc_type,
                        mc_no: mc_no,
                        purpose: purpose,
                        mc_nm: mc_nm,
                        re_mark: re_mark
                    },
                    success: function (data) {
                        debugger;
                        if (data.result == true) {
                            var id = data.kq.mno;
                            $("#MachineMgtGrid").jqGrid('addRowData', id, data.kq, 'first');
                            $("#MachineMgtGrid").setRowData(id, false, { background: "#d0e9c6" });
                            //jQuery("#MachineMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
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
    function fun_check2() {
        if ($("#m_mc_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#m_mc_nm").val("");
            $("#m_mc_nm").focus();
            return false;
        }
        if ($("#m_mc_type").val() == "") {
            alert("Please enter the Type.");
            $("#m_mc_type").val("");
            $("#m_mc_type").focus();
            return false;
        }
        return true;
    }
    $("#m_save_but").click(function () {
        if (fun_check2() == true) {
            var mno = $('#m_mno').val();
            var mc_type = $('#m_mc_type').val();
            var mc_no = $('#m_mc_no').val();
            var purpose = $('#m_purpose').val();
            var mc_nm = $('#m_mc_nm').val();
            var re_mark = $('#m_re_mark').val();
            {
                $.ajax({
                    url: "/DevManagement/updatetMachineMgt",
                    type: "PUT",
                    dataType: "json",
                    data: {
                        mno: mno,
                        mc_type: mc_type,
                        mc_no: mc_no,
                        purpose: purpose,
                        mc_nm: mc_nm,
                        re_mark: re_mark
                    },
                    success: function (data) {
                        if (data.result == true) {
                            jQuery("#MachineMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
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
    //Search
    $("#searchBtn").click(function () {

        var mc_type = $('#mc_type').val()
        var mc_no = $('#s_mc_no').val().trim();
        var mc_nm = $('#s_mc_nm').val().trim();
        var start = $('#start').val()
        var end = $('#end').val()
        $.ajax({
            url: "/DevManagement/searchMachineMgt",
            type: "Get",
            dataType: "json",
            data: {
                mc_type: mc_type,
                mc_no: mc_no,
                mc_nm: mc_nm,
                start: start,
                end: end
            },
            success: function (result) {
                $("#MachineMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

});
//Date
$("#start").datepicker({
    format: 'mm/dd/yyyy',
});

$("#end").datepicker({
    format: 'mm/dd/yyyy',
});

var getType = "/DevManagement/getTypeMachine";
//Create
$(document).ready(function () {
    _getTypeC();
    $("#c_mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeC() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#c_mc_type").html(html);
        }
    });
}
//update
$(document).ready(function () {
    _getType();
    $("#m_mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getType() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#m_mc_type").html(html);

        }
    });
}
//search
$(document).ready(function () {
    _getTypeS();
    $("#mc_type").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _getTypeS() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*All Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mc_type").html(html);
        }
    });
}
// Tab Create/Modifly
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
    $("#MachineMgtGrid").trigger('reloadGrid');
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

});
//Export
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('#MachineMgtGrid').tableExport({
            type: 'pdf',
            fileName: 'MachineMgtGrid',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('#MachineMgtGrid').tableExport({
            type: 'xls',
            fileName: 'MachineMgtGrid',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('#MachineMgtGrid').tableExport({
            type: 'doc',
            fileName: 'MachineMgtGrid',
            escape: false,
            headings: true,
            footers: true,
        });
    });
});
