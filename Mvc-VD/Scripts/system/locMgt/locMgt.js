$grid = $("#locMgt").jqGrid({
    url: '/System/Getlocation',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        {
            key: true, label: 'ID', name: 'lctno', width: 60, align: 'right', hidden: true
        },
        { label: 'Location Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 200, align: 'center' },
        { label: 'Location Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 300, },
        {
            label: 'Full Name', name: 'mn_full', sortable: true, editable: true, width: 250, editrules: { required: true, number: true },
            cellattr: function (rowId, cellValue, rowObject) {
                return ' title="' + cellValue + '"';
            }
        },
        { label: 'Level NM', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
        { label: 'Movement', name: 'mv_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Use YN', name: 'use_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Description', name: 're_mark', width: 100, sortable: true, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"';} },
        { label: 'Create Name', name: 'reg_id', sortable: true, width: 100, },
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Change Name', name: 'chg_id', sortable: true, width: 100, },
        { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#jqGridPager1',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    loadonce: true,
    viewrecords: true,
    height: 360,
    width: $(".box-body").width(),
    autowidth: false,
    caption: "Location Information",
    jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
            
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#locMgt").jqGrid("getGridParam", 'selrow');
        row_id = $("#locMgt").getRowData(selectedRowId);

        var lctno = row_id.lctno;
        var lct_nm = row_id.lct_nm;
        var use_yn = row_id.use_yn;
        var mv_yn = row_id.mv_yn;
        var re_mark = row_id.re_mark;
        
        $('#m_lct_nm').val(lct_nm);
        $("#m_use_yn").val(use_yn);      
        $('#m_re_mark').val(re_mark);
        $('#m_lctno').val(lctno);
        $('#c_lctno').val(lctno);
        (mv_yn == 'Y') ? $('#m_mv_yn').prop('checked', true) : $('#m_mv_yn').prop('checked', false);
        $("#del_save_but").removeClass("disabled", false);
        document.getElementById("form2").reset();
        if (row_id.level_cd == "004") {
            $("#c_re_mark").prop("disabled", true);
            $("#c_lct_nm").prop("disabled", true);
            $("#c_save_but").prop("disabled", true);
            $("#c_reset_but").prop("disabled", true);
            $("#c_mt_yn").prop("disabled", true);
            $("#c_mv_yn").prop("disabled", true);
            $("#c_ds_yn").prop("disabled", true);

        } else {
            $("#c_re_mark").prop("disabled", false);
            $("#c_lct_nm").prop("disabled", false);
            $("#c_save_but").prop("disabled", false);
            $("#c_reset_but").prop("disabled", false);
            $("#c_mt_yn").prop("disabled", false);
            $("#c_mv_yn").prop("disabled", false);
            $("#c_ds_yn").prop("disabled", false);
        }
                  
    },
});
            
$("#locMgt").jqGrid('navGrid', '#jqGridPager1', { edit: false, add: false, del: false })
//xuất file pdf,excel,doc
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#locMgt").jqGrid('exportToExcel',
                options = {
                    includeLabels: true,
                    includeGroupHeader: true,
                    includeFooter: true,
                    fileName: "Location Information.xlsx",
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
        $("#locMgt").jqGrid('exportToHtml',
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
                fileName: "Location Information  ",
                returnAsString: false
            }
            );
    });

});
$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#locMgt").jqGrid('exportToPdf',
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
                fileName: "Location.pdf",
                mimetype: "application/pdf"
            }
            );
    });

});


$("#c_save_but").click(function () {
        if ($('input[name=root_yn]').is(":checked") == false) {
        if ($('#c_lctno').val() == "") {
            alert("Please select the a Location on the grid");
        }
        else {
            if ($("#form2").valid() == true) {
                $.ajax({
                    url: "/system/insertlocMgt",
                    type: "get",
                    dataType: "json",
                    data: {
                        lctno: ($('#c_lctno').val() == "") ? 1 : $('#c_lctno').val(),
                        mv_yn: ($('#c_mv_yn').prop('checked') == true) ? "Y" : "N",
                        use_yn: $('#c_use_yn').val(),
                        re_mark: $('#c_re_mark').val(),
                        lct_nm: $('#c_lct_nm').val(),
                        root_yn: ($('#c_root_yn').prop('checked') == true) ? "1" : "",
                    },
                    success: function (data) {
                        var id = data.lctno;
                        $("#locMgt").jqGrid('addRowData', id, data, 'first');
                        $("#locMgt").setRowData(id, false, { background: "#d0e9c6" });
                        document.getElementById("form2").reset();
                    }

                });
            }
        }
    }
    else
    {
        if ($("#form2").valid() == true) {
            $.ajax({
                url: "/system/insertlocMgt",
                type: "get",
                dataType: "json",
                data: {
                    lctno: ($('#c_lctno').val() == "") ? 1 : $('#c_lctno').val(),
                    mv_yn: ($('#c_mv_yn').prop('checked') == true) ? "Y" : "N",
                    use_yn: $('#c_use_yn').val(),
                    re_mark: $('#c_re_mark').val(),
                    lct_nm: $('#c_lct_nm').val(),
                    root_yn: ($('#c_root_yn').prop('checked') == true) ? "1" : "",
                },
                success: function (data) {
                    var id = data.lctno;
                    $("#locMgt").jqGrid('addRowData', id, data, 'first');
                    $("#locMgt").setRowData(id, false, { background: "#d0e9c6" });
                    document.getElementById("form2").reset();
                }
,
                error: function (data) {
                    alert("Something Went Wrong..");
                }
            });
        }
    }
});
$("#m_save_but").click(function () {
    if ($("#form1").valid() == true) {
        $.ajax({
            url: "/system/updatelocMgt",
            type: "get",
            dataType: "json",
            data: {
                lctno: $('#m_lctno').val(),
                mv_yn: ($('#m_mv_yn').prop('checked') == true) ? "Y" : "N",
                use_yn: $('#m_use_yn').val(),
                re_mark: $('#m_re_mark').val(),
                lct_nm: $('#m_lct_nm').val(),
            },
            success: function (data) {
                    var id = data.lctno;
                    $("#locMgt").setRowData(id, data, { background: "#d0e9c6" });
                document.getElementById("form1").reset();

            },
            error: function (data) {
                alert("Something Went Wrong..");
            }
        });
    }   
});
$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
function fun_reset() {
    document.getElementById("form2").reset();
}

$("#searchBtn").click(function () {
$.ajax({
    url: "/system/Searchloc_mgt",
    type: "get",
    dataType: "json",
    data: {
        lct_cd: $('#lct_cd').val().trim(),
        lct_nm: $('#lct_nm').val().trim(),
    },
    success: function (result) {
        $("#locMgt").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
    }
});

});
////////////////////////////////////////////////////////////////////////////////////////////
$("#form1").validate({
    rules: {
        "lct_nm": {
            required: true,

        },
    },
});
$("#form2").validate({
    rules: {
        "lct_nm": {
            required: true,

        },
    },
});