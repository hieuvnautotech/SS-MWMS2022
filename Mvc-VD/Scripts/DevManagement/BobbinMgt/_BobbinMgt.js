//Create Grid
$(function () {

    $grid = $("#BobbinMgtGrid").jqGrid
        ({
            url: "/DevManagement/searchBobbinMgt_TSD",
            datatype: 'json',
            mtype: 'GET',
            colModel: [
                { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
                //{ key: false, label: 'Xả Container', name: 'xa', width: 80, align: 'center', formatter: xa_container  },
                { key: false, label: 'Type', name: 'mc_type', width: 80, align: 'center', sort: true },
                { key: false, label: 'Code', name: 'bb_no', width: 100, align: 'left', sort: true },
                { key: false, label: 'Name', name: 'bb_nm', width: 150, align: 'left' },
                { key: false, label: 'Lot', name: 'mt_cd', width: 300, align: 'left' },
                { key: false, label: 'PO', name: 'at_no', width: 100, align: 'center' },

                { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                { key: false, label: 'Barcode', name: 'barcode', width: 250, align: 'left' },
                { key: false, label: 'Description', name: 're_mark', width: 300, align: 'left' },
                { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
                { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            ],

            gridComplete: function () {
                var rows = $("#BobbinMgtGrid").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var bb_no = $("#BobbinMgtGrid").getCell(rows[i], "bb_no");
                    var giatri = $('#remark_color').val();
                    if (bb_no == giatri) {
                        $("#BobbinMgtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                    }
                }
            },

            onSelectRow: function (rowid, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#BobbinMgtGrid").jqGrid("getGridParam", "selrow"),
                    row_id = $("#BobbinMgtGrid").getRowData(selectedRowId),
                    bno = row_id.bno,
                    bb_no = row_id.bb_no,
                    bb_nm = row_id.bb_nm,
                    purpose = row_id.purpose,
                    re_mark = row_id.re_mark;
                $("#m_bb_no").val(bb_no);
                $("#m_bb_nm").val(bb_nm);
                $("#m_purpose").val(purpose);
                $("#m_mc_type").val(row_id.mc_type);
                $("#m_bno").val(bno);
                $("#m_re_mark").val(re_mark);
                $("#c_bno").val(bno);

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false),

                    $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").removeClass("hidden");
                $("#tab_c1").addClass("hidden");
                $("#tab_c2").addClass("active");
            },
            pager: $('#BobbinMgtPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            height: 450,
            width: $(".box-body").width() - 5,
            caption: 'Bobbin Management',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            pager: "#BobbinMgtPager",
            loadonce: true,
            shrinkToFit: false,
            autowidth: false,
            multiselect: true,
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

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#foMgtGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#BobbinMgtGrid").closest(".ui-jqgrid").parent().width();
        $("#BobbinMgtGrid").jqGrid("setGridWidth", newWidth, false);
    });
    //Button Create
    function fun_check1() {
        if ($("#c_bb_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#c_bb_nm").val("");
            $("#c_bb_nm").focus();
            return false;
        }
        return true;
    }

    $("#c_save_but").click(function () {
        if ($("#form1").valid() == true) {
            var bb_no = $("#c_bb_no").val().toUpperCase(),
                purpose = $("#c_purpose").val(),
                bb_nm = $("#c_bb_nm").val(),
                re_mark = $("#c_re_mark").val();
            {
                $.ajax({
                    url: "/DevManagement/insertBobbinMgt",
                    type: "Post",
                    dataType: "json",
                    data: {
                        bb_no: bb_no,
                        purpose: purpose,
                        bb_nm: bb_nm,
                        re_mark: re_mark,
                        mc_type: $("#c_mc_type").val(),
                    },
                    success: function (data) {
                        if (data.result == true) {
                            var id = data.kq.bno;
                            $("#BobbinMgtGrid").jqGrid('addRowData', id, data.kq, 'first');
                            $("#BobbinMgtGrid").setRowData(id, false, { background: "#d0e9c6" });
                            //jQuery("#BobbinMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            SuccessAlert(data.message);
                        }
                        else {
                            ErrorAlert(data.message);
                        }
                    },
                    error: function (data) {
                        alert('The Code is the same. Please check again.');
                    }
                });
            }
        }
    });

    function fun_check2() {
        if ($("#m_bb_nm").val().trim() == "") {
            alert("Please enter the Name.");
            $("#m_bb_nm").val("");
            $("#m_bb_nm").focus();
            return false;
        }
        return true;
    }

    $("#m_save_but").click(function () {
        if ($("#form2").valid() == true) {
            var bno = $("#m_bno").val(),
                bb_no = $("#m_bb_no").val(),
                purpose = $("#m_purpose").val(),
                bb_nm = $("#m_bb_nm").val(),
                re_mark = $("#m_re_mark").val();
            {
                $.ajax({
                    url: "/DevManagement/updateBobbinMgt",
                    type: "Put",
                    dataType: "json",
                    data: {
                        bno: bno,
                        bb_no: bb_no,
                        purpose: purpose,
                        bb_nm: bb_nm,
                        re_mark: re_mark,
                        mc_type: $("#m_mc_type").val(),
                    },
                    success: function (data) {
                        debugger;
                        if (data.result == true) {
                            var id = data.kq.bno;
                            $("#BobbinMgtGrid").setRowData(id, data.kq, { background: "#d0e9c6" });
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
        var s_bb_no = $("#s_bb_no").val().trim(),
            s_bb_nm = $("#s_bb_nm").val().trim();
        $.ajax({
            url: "/DevManagement/searchBobbinMgt_TSD",
            type: "get",
            dataType: "json",
            data: {
                s_bb_no: s_bb_no,
                s_bb_nm: s_bb_nm,
            },
            success: function (result) {
                $("#BobbinMgtGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });
});

function xa_container(cellValue, options, rowdata, action) {
  
    var html = "";
    if (rowdata.mt_cd) {
        html = `<button class="btn btn-xs btn-primary" data-id="${rowdata.bno}" data-mt_cd="${rowdata.mt_cd}" data-bb_no="${rowdata.bb_no}" onclick="Xa_bb(this);">Xả</button>`;
    }
    return html; 
}
function Xa_bb(e) {
    $("#bobin").val($(e).data("bb_no"));
    $("#mt_cd").val($(e).data("mt_cd"));
    $("#id_xa").val($(e).data("id"));
    $("#sts").val("xa_bb");
    $('#dialogDangerous').dialog('open');
}
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
    $("#del_save_but").attr("disabled", true),
        document.getElementById("form2").reset();
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
        $("#BobbinMgtGrid").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "BobbinMgt.pdf",
                mimetype: "application/pdf"
            }
        );
    });
});

$('#excelBtn').click(function (e) {
    $("#BobbinMgtGrid").jqGrid('exportToExcel',
        options = {
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            fileName: "Bobbin Information.xlsx",
            mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            maxlength: 40,
            onBeforeExport: null,
            replaceStr: null
        }
    );
});

$('#htmlBtn').click(function (e) {
    $("#BobbinMgtGrid").jqGrid('exportToHtml',
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
            fileName: "Bobbin Information  ",
            returnAsString: false
        }
    );
});

$("#form1").validate({
    rules: {
        "bb_no": {
            required: true,
        },
        "bb_nm": {
            required: true,
        },
        "c_mc_type": {
            required: true,
        },

    },
});

$("#form2").validate({
    rules: {
        "bb_no": {
            required: true,
        },
        "bb_nm": {
            required: true,
        },
        "m_mc_type": {
            required: true,
        },
    },
});

$("#c_print").on("click", function () {
    var i, selRowIds = $('#BobbinMgtGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        window.open("/DevManagement/_Bobbin?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        alert("Please select Grid.");
    }
})
