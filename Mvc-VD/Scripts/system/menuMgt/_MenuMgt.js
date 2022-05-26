$(document).ready(function () {
    $('.nav-link > p').addClass('text-white');
    $('.MenuManagement').addClass('text-warning');

    $('.System-menu').addClass('menu-open');

    $grid = $("#jqGrid").jqGrid({
        url: '/system/GetDataMenu',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { label: 'ID', name: 'mnno', width: 60, align: 'right', hidden: true, key: true },
            { label: 'Menu Code', name: 'mn_cd', width: 100, align: 'center', sortable: true },
            { name: 'up_mn_cd', hidden: true },
            { name: 'level_cd', hidden: true },
            { name: 'sub_yn', hidden: true },
            { name: 'mn_cd_full', hidden: true },
            { label: 'Menu Name', name: 'mn_nm', sortable: true, editable: true, editrules: { required: true, number: true }, width: 200, align: 'left' },
            { label: 'Full Name', name: 'mn_full', sortable: true, width: 350, editrules: { required: true, number: true }, align: 'left' },
            { label: 'URL', name: 'url_link', width: 350, align: 'left', sortable: true, editable: true, editrules: { required: true, number: true } },
            { label: 'USE YN', name: 'use_yn', width: 55, align: 'center', sortable: true, editrules: { required: true, number: true } },
            { label: 'Order No', name: 'order_no', width: 70, align: 'center', sortable: true, editable: true },
            { label: 'Reg Name', name: 'reg_id', width: 90, sortable: true },
            { label: 'Reg Date', name: 'reg_dt', align: 'center', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
            { label: 'Change Name', name: 'chg_id', width: 90, sortable: true },
            { label: 'Change Date', name: 'chg_dt', align: 'center', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
        ],
        loadonce: true,
        userDataOnFooter: true,
        pager: '#jqGridPager',
        rownumbers: true,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        caption: 'Menu information',
        viewrecords: true,
        emptyrecords: "No data.",
        gridview: true,
        rownumbers: true,
        shrinkToFit: false,
        height: 360,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: false,
        onSelectRow: function (rowid, iCol, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#jqGrid").jqGrid("getGridParam", 'selrow');
            row_id = $("#jqGrid").getRowData(selectedRowId);
            $('#m_mn_nm').val(row_id.mn_nm);
            $('#c_mnno').val(row_id.mnno);
            $('#m_url_link').val(row_id.url_link); 
        },

    });

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#jqGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#jqGrid").closest(".ui-jqgrid").parent().width();
        $("#jqGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $(document).ready(function (e) {
        $('#pdfBtn').click(function (e) {
            $('#jqGrid').tableExport({
                type: 'pdf',
                fileName: "menuMgt",
                pdfFontSize: '7',
                escape: false,
            });
        });
    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $('#jqGrid').tableExport({
                type: 'xls',
                fileName: "menuMgt",
                escape: false,
            });
        });
    });
    $(document).ready(function (e) {
        $('#htmlBtn').click(function (e) {
            $('#jqGrid').tableExport({
                type: 'doc',
                fileName: "menuMgt",
                escape: false,
            });
        });
    });

    $("#searchBtn").click(function () {
        var mn_cd = $("#mn_cd").val().toString().trim();
        var mn_nm = $("#mn_nm").val().toString().trim();
        var full_nm = $("#full_nm").val().toString().trim();

        $.ajax({
            url: '/system/GetDataMenu',
            type: "GET",
            dataType: "json",
            data: {
                mn_cd: mn_cd,
                mn_nm: mn_nm,
                full_nm: full_nm,
            },
            success: function (result) {
                $("#jqGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            },
            error: function (data) {
                alert('Not Found');
            }

        });

    });
    $("#m_save_but").click(function () {
        if ($('#m_mn_nm').val() == "") {
            alert("Please check Name");
            return false;
        }
        if ($("#form1").valid() == true) {

            $.ajax({
                url: "/system/updateMenuMgt",
                type: "PUT",
                dataType: "json",
                data: {
                    mn_nm: $('#m_mn_nm').val(),
                    mnno: $('#c_mnno').val(),
                    url_link: $('#m_url_link').val(),
                    use_yn: $('#m_use_yn').val(),
                    re_mark: $('#m_re_mark').val(),
                },
                success: function (data) {
                    if (data.result == true) {
                        jQuery("#jqGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });
    $("#c_save_but").click(function () {
        if ($('#c_root_yn').prop('checked') != true) {
            if ($('#c_mnno').val() == "") {
                alert("Please choice a Menu information in gird");
                return false;
            }
        }
        if ($('#c_mn_nm').val() == "") {
            alert("Please check Name");
            return false;
        }
        if ($("#form2").valid() == true) {
            $.ajax({
                url: "/system/insertMenu",
                type: "POST",
                dataType: "json",
                data: {
                    root_yn: ($('#c_root_yn').prop('checked') == true) ? "1" : "",
                    mn_nm: $('#c_mn_nm').val(),
                    mnno: ($('#c_root_yn').prop('checked') == true) ? 0 : $('#c_mnno').val(),
                    url_link: $('#c_url_link').val(),
                    use_yn: $('#c_use_yn').val(),
                    re_mark: $('#c_re_mark').val(),
                },
                success: function (data) {
                    if (data.result == true) {
                        var id = data.value.mnno;
                        $("#jqGrid").jqGrid('addRowData', id, data.value, 'first');
                        $("#jqGrid").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                    } else {
                        ErrorAlert(data.message);
                    }
                    
                }

            });
        }
    });
});