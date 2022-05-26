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
        onCellSelect: function (rowid, iCol, cellcontent, e) {
            debugger;
            if (iCol == 14) {
                $('.deletebtn').click(function () {
                    var menucode = $(this).data("menucode");
                    $.ajax({
                        url: "/system/deleteMenu",
                        type: "post",
                        dataType: "json",
                        data: {
                            menucode: menucode,
                        },
                        success: function (result) {
                            $("#jqGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            document.getElementById("form1").reset();

                        },
                        error: function (result) {
                            alert('Data is not existing. Please check again');
                        }
                    });
                });
            }
        },
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
        onSelectRow: function (rowid, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#jqGrid").jqGrid("getGridParam", 'selrow');
            row_id = $("#jqGrid").getRowData(selectedRowId);
            var mn_cd = row_id.mn_cd;
            var url_link = row_id.url_link;
            var order_no = row_id.order_no;
            var use_yn = row_id.use_yn;
            var mn_full = row_id.mn_full;
            var mn_nm_level = [];
            var i = 1;
            while (i <= 4) {
                var p = mn_full.indexOf(">");
                if (p != -1) {
                    var s = mn_full.slice(0, p);
                    mn_nm_level.push(s);
                    mn_full = mn_full.slice(p + 1);
                    i++;
                } else {
                    var s = mn_full;
                    mn_nm_level.push(s);
                    i = 5;
                }
            }
            $("#m_save_but").removeAttr("disabled");
            $("#del_save_but").removeAttr("disabled");
            document.getElementById("form2").reset();
            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");

            $('#m_mn_cd').val(mn_cd);
            $('#m_url_link').val(url_link);
            $('#m_order_no').val(order_no);
            $("#m_use_yn").val(use_yn);

            $("#m_mn_nm_1").removeAttr("readonly");
            $("#m_mn_nm_2").removeAttr("readonly");
            $("#m_mn_nm_3").removeAttr("readonly");
            $("#m_mn_nm_4").removeAttr("readonly");

            $("#m_mn_nm_1").val(mn_nm_level[0]);
            $("#m_mn_nm_2").val(mn_nm_level[1]);
            $("#m_mn_nm_3").val(mn_nm_level[2]);
            $("#m_mn_nm_4").val(mn_nm_level[3]);

            if ($("#m_mn_nm_1").val() == "") { $("#m_mn_nm_1").attr("readonly", "readonly") }
            if ($("#m_mn_nm_2").val() == "") { $("#m_mn_nm_2").attr("readonly", "readonly") }
            if ($("#m_mn_nm_3").val() == "") { $("#m_mn_nm_3").attr("readonly", "readonly") }
            if ($("#m_mn_nm_4").val() == "") { $("#m_mn_nm_4").attr("readonly", "readonly") }


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


    $("#m_save_but").click(function () {

        if (($('#m_mn_nm_1').val() == "") && ($('#m_mn_nm_2').val() == "") && ($('#m_mn_nm_3').val() == "") && ($('#m_mn_nm_4').val() == "")) {
            alert("Please insert the menu name");
        } else {
            $.ajax({
                url: "/system/updateMenuMgt",
                type: "post",
                dataType: "json",
                data: {
                    m_mn_cd: $('#m_mn_cd').val(),
                    m_mn_nm_1: $('#m_mn_nm_1').val(),
                    m_mn_nm_2: $('#m_mn_nm_2').val(),
                    m_mn_nm_3: $('#m_mn_nm_3').val(),
                    m_mn_nm_4: $('#m_mn_nm_4').val(),
                    m_order_no: $('#m_order_no').val(),
                    m_url_link: $('#m_url_link').val(),
                    m_use_yn: $('#m_use_yn').val(),
                },
                success: function (data) {
                    if (data.result == false) {
                        alert("Menu was existing");
                    }
                    jQuery("#jqGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
            });
        }
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });

    $("#c_save_but").click(function () {
        if (($('#c_mn_nm_1').val() == "") && ($('#c_mn_nm_2').val() == "") && ($('#c_mn_nm_3').val() == "") && ($('#c_mn_nm_4').val() == "")) {
            alert("Please insert the menu name");
        } else {
            $.ajax({
                url: "/system/insertMenu",
                type: "post",
                dataType: "json",
                data: {
                    c_mn_nm_1: $('#c_mn_nm_1').val(),
                    c_mn_nm_2: $('#c_mn_nm_2').val(),
                    c_mn_nm_3: $('#c_mn_nm_3').val(),
                    c_mn_nm_4: $('#c_mn_nm_4').val(),
                    c_order_no: $('#c_order_no').val(),
                    c_url_link: $('#c_url_link').val(),
                    c_use_yn: $('#c_use_yn').val(),
                },
                success: function (data) {
                    if (data.result == false) {
                        alert("Menu was existing");
                    }
                    jQuery("#jqGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
            });
        }
    });

    // Tab
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
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
    });
});