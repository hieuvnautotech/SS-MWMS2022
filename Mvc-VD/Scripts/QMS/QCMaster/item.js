$(function () {
    $("#list").jqGrid
        ({
            url: "/QMS/Getqc_item",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'ino', name: 'ino', width: 100, align: 'center', hidden: true },
                { key: false, label: 'QC Code', name: 'item_cd', width: 100, align: 'center', hidden: true },
                { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '80px', align: 'center', formatter: item_popup },
                { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '60px', align: 'center', hidden: true },
                { key: false, label: 'QC Type', name: 'item_type', width: 60, align: 'center', },
                { key: false, label: 'Ver', name: 'ver', width: 50, align: 'center', hidden: true },
                { key: false, label: 'QC Name', name: 'item_nm', sortable: true, width: '100px', align: 'left' },
                { key: false, label: 'QC Explain', name: 'item_exp', sortable: true, width: '200' },
                { key: false, label: 'Use', name: 'use_yn', editable: true, width: '50px', align: 'center' },
                { key: false, label: 'Create Use', name: 'reg_id', editable: true, width: '100px' },
                {
                    label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { key: false, label: 'Change Use', name: 'chg_id', editable: true, width: '100px' },
                {
                    label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                row_id = $("#list").getRowData(selectedRowId);
                $('#ino').val(row_id.ino);
                $('#m_item_type').val(row_id.item_type);
                $('#m_item_exp').val(row_id.item_exp);
                $('#m_item_nm').val(row_id.item_nm);
                $('#m_use_yn').val(row_id.use_yn);
                $('#m_item_cd').val(row_id.item_vcd);
                $('#m_ver').val(row_id.ver);

                //get cho luoi 2
                $('#c2_item_cd').val(row_id.item_vcd);
                $('#c2_ver').val(row_id.ver);


                $("#list2").setGridParam({ url: "/QMS/Getqc_check_mt?" + "item_vcd=" + row_id.item_vcd, datatype: "json" }).trigger("reloadGrid");
                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").removeClass("hidden");
                $("#tab_c1").addClass("hidden");
                $("#tab_c2").addClass("active");
                $("#ver_vf").removeClass("hidden");
                $("#m_save_but").removeClass("hidden");
                $("#c_save_but").removeClass("active");
                $("#m_save_but").addClass("active");
                $("#ver_vf").addClass("active");
                $("#c_save_but").addClass("hidden");


                $("#tab_4").removeClass("active");
                $("#tab_3").addClass("active");
                $("#tab_c4").removeClass("active");
                $("#tab_c3").removeClass("hidden");
                $("#tab_c4").addClass("hidden");
                $("#tab_c3").addClass("active");
                $("#c2_save_but").removeClass("hidden");
                $("#m2_save_but").removeClass("active");
                $("#c2_save_but").addClass("active");
                $("#m2_save_but").addClass("hidden");



                $('#c_check_subject').val("");
                $('#c_order_no').val("1");
                $('#c_re_mark').val("");
                $('#c_min_value').val("0");
                $('#c_max_value').val("0");
                $('#c_check_type').val("");
                $("#list3").clearGridData();
            },

            pager: jQuery('#jqGridPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            loadonce: true,
            height: 595,
            width:null,
            caption: 'QC Information',
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
        }).navGrid('#gridpager',
            {
                edit: false, add: false, del: false, search: false,
                searchtext: "Search Student", refresh: true
            },
            {
                zIndex: 100,
                caption: "Search Students",
                sopt: ['cn']
            });

    $("#c_save_but").click(function () {

        if ($("#c_item_type").val().trim() == "") {
            alert("Please enter your Item Type");
            $("#c_item_type").val("");
            $("#c_item_type").focus();
            return false;
        }
        if ($("#c_item_nm").val().trim() == "") {
            alert("Please enter your Item Name");
            $("#c_item_nm").val("");
            $("#c_item_nm").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/QMS/Createqc_item",
                type: "get",
                dataType: "json",
                data: {
                    item_type: $('#c_item_type').val(),
                    item_exp: $('#c_item_exp').val(),
                    item_nm: $('#c_item_nm').val(),
                    use_yn: $('#c_use_yn').val(),
                },
                success: function (data) {
                    var id = data.ino;
                    $("#list").jqGrid('addRowData', id, data, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    document.getElementById("form1").reset();
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });

    function item_popup(cellvalue, options, rowobject) {
        var id = rowobject.ino;
        if (cellvalue != null) {
            var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="popupm_day(' + id + ');" class="poupgbom">' + cellvalue + '</button>';
            return html;
        } else {
            return cellvalue;
        }
    }
    $("#m_save_but").click(function () {
        if ($("#m_item_type").val().trim() == "") {
            alert("Please enter your Item Type");
            $("#m_item_type").val("");
            $("#m_item_type").focus();
            return false;
        }
        if ($("#m_item_nm").val().trim() == "") {
            alert("Please enter your Item Name");
            $("#m_item_nm").val("");
            $("#m_item_nm").focus();
            return false;
        }
        else {
            $.ajax({
                type: "get",
                dataType: "json",
                url: "/QMS/Modifyqc_item",
                data: {
                    item_exp: $('#m_item_exp').val(),
                    item_nm: $('#m_item_nm').val(),
                    use_yn: $('#m_use_yn').val(),
                    ino: $('#ino').val(),
                },
                success: function (data) {
                    var id = data.ino;
                    $("#list").setRowData(id, data, { background: "#d0e9c6" });
                    document.getElementById("form2").reset();
                }
            });
        }
    });

    $("#searchBtn").click(function () {
        $.ajax({
            url: "/QMS/searchqc_item",
            type: "Get",
            dataType: "json",
            data: {
                item_type: $('#s_item_type').val().trim(),
                item_cd: $('#s_item_cd').val().trim(),
                item_nm: $('#s_item_nm').val().trim(),
                item_exp: $('#s_item_exp').val().trim(),
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                $("#list2").clearGridData();
                $("#list3").clearGridData();
            }
        });

    });
    $("#ver_vf").click(function () {
        var selRowId = $('#list').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Item Information on the grid.");
            return false;
        }
        else {
            $.ajax({
                url: "/QMS/insertver_vf",
                type: "get",
                dataType: "json",
                data: {
                    item_cd: $('#m_item_cd').val(),
                    item_type: $('#m_item_type').val(),
                    item_exp: $('#m_item_exp').val(),
                    item_nm: $('#m_item_nm').val(),
                    use_yn: $('#m_use_yn').val(),
                    ver1: $('#m_ver').val(),
                },
                success: function (data) {
                    var id = data.ino;
                    $("#list").jqGrid('addRowData', id, data, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    $("#list2").clearGridData();
                    $("#list3").clearGridData();
                }
            });
        }
    });
    $("#tab_1").on("click", "a", function (event) {
        $("#tab_2").removeClass("active");
        $("#tab_1").addClass("active");
        $("#tab_c2").removeClass("active");
        $("#tab_c1").removeClass("hidden");
        $("#tab_c2").addClass("hidden");
        $("#tab_c1").addClass("active");
        $("#c_save_but").removeClass("hidden");
        $("#ver_vf").removeClass("active");
        $("#m_save_but").removeClass("active");
        $("#c_save_but").addClass("active");
        $("#m_save_but").addClass("hidden");
        $("#ver_vf").addClass("hidden");
    });
    $("#tab_2").on("click", "a", function (event) {
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").removeClass("hidden");
        $("#ver_vf").removeClass("hidden");
        $("#c_save_but").removeClass("active");
        $("#m_save_but").addClass("active");
        $("#ver_vf").addClass("active");
        $("#c_save_but").addClass("hidden");
    });
});//grid1
$(document).ready(function () {
    _getitem_type();
    _getcheck_type();
    _getrange_type();
    _get_defect();
});
function _getitem_type() {
    $.get("/QMS/get_item_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*QC Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_item_type").html(html);
            $("#c_item_type").html(html);
            $("#m_item_type").html(html);
        }
    });
}
function _get_defect() {
    $.get("/QMS/get_defect", function (data) {

        if (data != null && data != undefined && data.length) {

            var html = '';
            html += '';
            var QC_type = $("#m_item_type").val();
            var Check_type = $("#m_check_type").val();


            $.each(data, function (key, item) {
                //if ((((QC_type == "PC") && (Check_type == "check") || Check_type == "CHECK") && item.dt_cd == "N") || (((QC_type == "GS") && (Check_type == "check") || Check_type == "CHECK") && item.dt_cd == "N")) {

                //}  else {
                html = '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                //}

            });
            $("#c_defect_yn").html(html);
            $("#m_defect_yn").html(html);
        }
    });
}
function _getcheck_type() {
    $.get("/QMS/get_check_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Check Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#c_check_type").html(html);
            $("#m_check_type").html(html);
        }
    });
}
function _getrange_type() {
    $.get("/QMS/get_range_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Range Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#c_range_type").html(html);
            $("#m_range_type").html(html);
        }
    });
}
$(function () {
    $grid = $("#list2").jqGrid
        ({
            url: "/QMS/Getqc_check_mt",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'icno', name: 'icno', width: 100, align: 'center', hidden: true },
                { key: false, label: 'QC Code', name: 'item_cd', width: 100, align: 'center', hidden: true },
                { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '130', align: 'center' },
                { key: false, label: 'Ver', name: 'ver', width: 50, align: 'center', hidden: true },
                { key: false, label: 'Check Id', name: 'check_id', sortable: true, width: '100px', align: 'center' },
                { key: false, label: 'Check Type', name: 'check_type', sortable: true, width: '150' },
                { key: false, label: 'Check Subject', name: 'check_subject', sortable: true, width: '200' },
                { key: false, label: 'Min Value', name: 'min_value', sortable: true, width: '100', align: 'right' },
                { key: false, label: 'Max Value', name: 'max_value', sortable: true, width: '100', align: 'right' },
                { key: false, label: 'Range Type', name: 'range_type_nm', sortable: true, width: '100', },
                { key: false, label: 'Range Type', name: 'range_type', sortable: true, width: '100', hidden: true },
                { key: false, label: 'Order', name: 'order_no', sortable: true, width: '50', align: 'right' },
                { key: false, label: 'Remark', name: 're_mark', sortable: true, width: '120' },
                { key: false, label: 'Use Y/N', name: 'use_yn', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'Create Use', name: 'reg_id', editable: true, width: '100px' },
                {
                    label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { key: false, label: 'Change Use', name: 'chg_id', editable: true, width: '100px' },
                {
                    label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { name: "", width: 50, align: "center", label: "Delete", resizable: false, title: false, formatter: delete_del },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
                row_id = $("#list2").getRowData(selectedRowId);
                $('#m2_item_cd').val(row_id.item_vcd);
                $('#m2_ver').val(row_id.ver);
                $('#m_check_id').val(row_id.check_id);
                $('#c2_check_id').val(row_id.check_id);
                $('#m_check_type').val(row_id.check_type);

                if (row_id.check_type == "range" || row_id.check_type == "RANGE") {
                    $("#m_check_type").attr("disabled", true);
                } else { $("#m_check_type").attr("disabled", false); }
                $('#m_check_subject').val(row_id.check_subject);
                $('#m_order_no').val(row_id.order_no);
                $('#m2_use_yn').val(row_id.use_yn);
                $('#m_re_mark').val(row_id.re_mark);
                $('#icno').val(row_id.icno);
                $('#m_range_type').val(row_id.range_type);
                $('#m_min_value').val(row_id.min_value);
                $('#m_max_value').val(row_id.max_value);

                $("#list3").setGridParam({ url: "/QMS/Getqc_check_dt?" + "item_vcd=" + row_id.item_vcd + "&check_id=" + row_id.check_id, datatype: "json" }).trigger("reloadGrid");
                _get_defect();

                $("#tab_3").removeClass("active");
                $("#tab_4").addClass("active");
                $("#tab_c3").removeClass("active");
                $("#tab_c4").removeClass("hidden");
                $("#tab_c3").addClass("hidden");
                $("#tab_c4").addClass("active");
                $("#m2_save_but").removeClass("hidden");
                $("#c2_save_but").removeClass("active");
                $("#m2_save_but").addClass("active");
                $("#c2_save_but").addClass("hidden");

                if (row_id.check_type == "range") {
                    $("#c3_save_but").attr("disabled", true);
                    $("#m3_save_but").attr("disabled", true);
                } else {
                    $("#c3_save_but").attr("disabled", false);
                    $("#m3_save_but").attr("disabled", false);
                }

            },

            pager: jQuery('#jqGrid2Pager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 175,
            loadonce: true,
            caption: 'QC Check Master',
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
            multiselect: false,
        }).navGrid('#jqGrid2Pager',
            {
                edit: false, add: false, del: false, search: false,
                searchtext: "Search Student", refresh: true
            },
            {
                zIndex: 100,
                caption: "Search",
                sopt: ['cn']
            });
    $("#c2_save_but").click(function () {
        var selRowId = $('#list').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Item Information on the grid.");
            return false;
        }
        if ($("#c2_item_cd").val().trim() == "") {
            alert("Please enter your Item Code");
            $("#c2_item_cd").val("");
            $("#c2_item_cd").focus();
            return false;
        }
        if ($("#c_check_type").val().trim() == "") {
            alert("Please enter your Check Type");
            $("#c_check_type").val("");
            $("#c_check_type").focus();
            return false;
        }
        if ($("#c_check_subject").val().trim() == "") {
            alert("Please enter your Check Subject");
            $("#c_check_subject").val("");
            $("#c_check_subject").focus();
            return false;
        }
        if ($("#c_order_no").val().trim() == "") {
            alert("Please enter your Order");
            $("#c_order_no").val("");
            $("#c_order_no").focus();
            return false;
        }
        var min = $("#c_min_value").val();
        var max = $("#c_max_value").val();
        if (parseInt(min) > parseInt(max)) {
            alert("Min Value > Max Value ,Please check again!");
            $("#c_min_value").val("");
            $("#c_min_value").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/QMS/Createcheck_mt",
                type: "Post",
                dataType: "json",
                data: {
                    item_cd: $('#c2_item_cd').val(),
                    use_yn: $('#c2_use_yn').val(),
                    check_type: $('#c_check_type').val(),
                    check_subject: $('#c_check_subject').val(),
                    order_no: $('#c_order_no').val(),
                    re_mark: $('#c_re_mark').val(),
                    ver: $('#c2_ver').val(),
                    min_value: $('#c_min_value').val(),
                    max_value: $('#c_max_value').val(),
                    range_type: $('#c_range_type').val(),
                },
                success: function (data) {
                    if (data.result) {
                        var id = data.data.icno;
                        $("#list2").jqGrid('addRowData', id, data.data, 'first');
                        $("#list2").setRowData(id, false, { background: "#d0e9c6" });
                        $('#c_check_subject').val("");
                        $('#c_order_no').val("1");
                        $('#c_re_mark').val("");
                        $('#c_min_value').val("0");
                        $('#c_max_value').val("0");
                        $('#c_check_type').val("");
                        $("#list3").clearGridData();
                        SuccessAlert(data.data.message);
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
    });
    $("#m2_save_but").click(function () {
        var selRowId = $('#list2').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Item Check Master on the grid.");
            return false;
        }
        if ($("#m2_item_cd").val().trim() == "") {
            alert("Please enter your Item Code");
            $("#m2_item_cd").val("");
            $("#m2_item_cd").focus();
            return false;
        }
        if ($("#m_check_type").val().trim() == "") {
            alert("Please enter your Check Type");
            $("#m_check_type").val("");
            $("#m_check_type").focus();
            return false;
        }
        if ($("#m_check_subject").val().trim() == "") {
            alert("Please enter your Check Subject");
            $("#m_check_subject").val("");
            $("#m_check_subject").focus();
            return false;
        }
        if ($("#m_order_no").val().trim() == "") {
            alert("Please enter your Order");
            $("#m_order_no").val("");
            $("#m_order_no").focus();
            return false;
        }
        var min = $("#m_min_value").val();
        var max = $("#m_max_value").val();
        if (parseInt(min) > parseInt(max)) {
            alert("Min Value > Max Value ,Please check again!");
            $("#m_min_value").val("");
            $("#m_min_value").focus();
            return false;
        }
        else {
            $.ajax({
                type: "Put",
                dataType: "json",
                url: "/QMS/Modifycheck_mt",
                data: {
                    icno: $('#icno').val(),
                    use_yn: $('#m2_use_yn').val(),
                    check_type: $('#m_check_type').val(),
                    check_subject: $('#m_check_subject').val(),
                    order_no: $('#m_order_no').val(),
                    re_mark: $('#m_re_mark').val(),
                    min_value: $('#m_min_value').val(),
                    max_value: $('#m_max_value').val(),
                    range_type: $('#m_range_type').val(),
                },
                success: function (data)
                {
                    if (data.result == true)
                    {
                        var id = data.icno;
                        $("#list2").setRowData(data.data.icno, data.data, { background: "#d0e9c6" });
                        document.getElementById("form4").reset();
                        SuccessAlert(data.message);
                    }
                    else
                    {
                        ErrorAlert(data.message);
                    }
                }
            });
        }
    });

    $("#tab_3").on("click", "a", function (event) {
        $("#tab_4").removeClass("active");
        $("#tab_3").addClass("active");
        $("#tab_c4").removeClass("active");
        $("#tab_c3").removeClass("hidden");
        $("#tab_c4").addClass("hidden");
        $("#tab_c3").addClass("active");
        $("#c2_save_but").removeClass("hidden");
        $("#m2_save_but").removeClass("active");
        $("#c2_save_but").addClass("active");
        $("#m2_save_but").addClass("hidden");
    });
    $("#tab_4").on("click", "a", function (event) {
        $("#tab_3").removeClass("active");
        $("#tab_4").addClass("active");
        $("#tab_c3").removeClass("active");
        $("#tab_c4").removeClass("hidden");
        $("#tab_c3").addClass("hidden");
        $("#tab_c4").addClass("active");
        $("#m2_save_but").removeClass("hidden");
        $("#c2_save_but").removeClass("active");
        $("#m2_save_but").addClass("active");
        $("#c2_save_but").addClass("hidden");
    });
});//grid2

function delete_del(cellValue, options, rowdata, action) {
    var id = rowdata.icno;
    var html = '<button class="btn btn-xs btn-danger" onclick="delDate1(' + id + ');">delete</button>';
    return html;
}
function delDate1(id) {
    $.ajax({
        url: "/QMS/deleteitem_2table?id=" + id,
        type: "Delete",
        dataType: "json",
        success: function (data) {
            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            jQuery("#list3").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (data) {
            alert('Error');
        }
    });
}
$(function () {
    $grid = $("#list3").jqGrid
        ({
            url: "/QMS/Getqc_check_dt",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'icdno', name: 'icdno', width: 100, align: 'center', hidden: true },
                { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '130', align: 'center', hidden: true },
                { key: false, label: 'Check Id', name: 'check_id', sortable: true, width: '100px', align: 'center' },
                { key: false, label: 'Check Code', name: 'check_cd', sortable: true, width: '100px', align: 'center' },
                { key: false, label: 'Check Name', name: 'check_name', sortable: true, width: '100px', align: 'left' },
                { key: false, label: 'Defective', name: 'defect_yn', sortable: true, width: '100px', align: 'center    ' },
                { key: false, label: 'Order', name: 'order_no', sortable: true, width: '50', align: 'right' },
                { key: false, label: 'Remark', name: 're_mark', sortable: true, width: '120' },
                { key: false, label: 'Use Y/N', name: 'use_yn', editable: true, width: '50px', align: 'center' },
                { key: false, label: 'Create Use', name: 'reg_id', editable: true, width: '100px' },
                {
                    label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { key: false, label: 'Change Use', name: 'chg_id', editable: true, width: '100px' },
                {
                    label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { key: false, label: 'Delete', name: 'del_yn', sortable: true, width: '150px', align: 'center', formatter: delete_del2 },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list3").jqGrid("getGridParam", 'selrow');
                row_id = $("#list3").getRowData(selectedRowId);
                $('#m2_check_id').val(row_id.check_id);
                $('#m_check_cd').val(row_id.check_cd);
                $('#m_check_name').val(row_id.check_name);
                $('#m2_order_no').val(row_id.order_no);
                $('#m3_use_yn').val(row_id.use_yn);
                $('#m_order_no').val(row_id.order_no);
                $('#m2_re_mark').val(row_id.re_mark);
                $('#icdno').val(row_id.icdno);
                $('#m_defect_yn').val(row_id.defect_yn);


                $("#tab_5").removeClass("active");
                $("#tab_6").addClass("active");
                $("#tab_c5").removeClass("active");
                $("#tab_c6").removeClass("hidden");
                $("#tab_c5").addClass("hidden");
                $("#tab_c6").addClass("active");
                $("#m3_save_but").removeClass("hidden");
                $("#c3_save_but").removeClass("active");
                $("#m3_save_but").addClass("active");
                $("#c3_save_but").addClass("hidden");
            },

            pager: jQuery('#jqGrid3Pager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 175,
            loadonce: true,
            caption: 'QC Check Detail',
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
            multiselect: false,
        }).navGrid('#jqGrid3Pager',
            {
                edit: false, add: false, del: false, search: false,
                searchtext: "Search Student", refresh: true
            },
            {
                zIndex: 100,
                caption: "Search",
                sopt: ['cn']
            });
    $("#c3_save_but").click(function () {
        var selRowId = $('#list2').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Item Check Master on the grid.");
            return false;
        }
        if ($("#c2_check_id").val().trim() == "") {
            alert("Please enter your Check Id");
            $("#c2_check_id").val("");
            $("#c2_check_id").focus();
            return false;
        }
        if ($("#c_check_name").val().trim() == "") {
            alert("Please enter your Check Name");
            $("#c_check_name").val("");
            $("#c_check_name").focus();
            return false;
        }
        if ($("#c2_order_no").val().trim() == "") {
            alert("Please enter your Order");
            $("#c2_order_no").val("");
            $("#c2_order_no").focus();
            return false;
        }
        else {
            $.ajax({
                url: "/QMS/Createcheck_dt",
                type: "Post",
                dataType: "json",
                data: {
                    check_id: $('#c2_check_id').val(),
                    check_name: $('#c_check_name').val(),
                    use_yn: $('#c3_use_yn').val(),
                    order_no: $('#c2_order_no').val(),
                    re_mark: $('#c2_re_mark').val(),
                    item_vcd: $('#m2_item_cd').val(),
                    defect_yn: $('#c_defect_yn').val(),
                    ver: $('#m2_ver').val(),
                },
                success: function (data)
                {
                    if (data.result == true)
                    {
                        var id = data.icdno;
                        $("#list3").jqGrid('addRowData', data.data.icdno, data.data, 'first');
                        $("#list3").setRowData(id, false, { background: "#d0e9c6" });
                        $('#c_check_name').val("");
                        $('#c2_order_no').val("1");
                        $('#c2_re_mark').val("");
                        SuccessAlert(data.message)
                    }
                    else
                    {
                        ErrorAlert(data.message)
                    }
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });
    $("#m3_save_but").click(function () {
        var selRowId = $('#list3').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top Item Check Detail on the grid.");
            return false;
        }
        if ($("#m2_check_id").val().trim() == "") {
            alert("Please enter your Check Id");
            $("#m2_check_id").val("");
            $("#m2_check_id").focus();
            return false;
        }
        if ($("#m_check_cd").val().trim() == "") {
            alert("Please enter your Check Code");
            $("#m_check_cd").val("");
            $("#m_check_cd").focus();
            return false;
        }
        if ($("#m_check_name").val().trim() == "") {
            alert("Please enter your Check Name");
            $("#m_check_name").val("");
            $("#m_check_name").focus();
            return false;
        }
        if ($("#m2_order_no").val().trim() == "") {
            alert("Please enter your Order");
            $("#m2_order_no").val("");
            $("#m2_order_no").focus();
            return false;
        }
        else {
            $.ajax({
                type: "Put",
                dataType: "json",
                url: "/QMS/Modifycheck_dt",
                data: {
                    icdno: $('#icdno').val(),
                    check_cd: $('#m_check_cd').val(),
                    check_name: $('#m_check_name').val(),
                    use_yn: $('#m3_use_yn').val(),
                    order_no: $('#m2_order_no').val(),
                    re_mark: $('#m2_re_mark').val(),
                    defect_yn: $('#m_defect_yn').val(),

                },
                success: function (data)
                {
                    if (data.result == true)
                    {
                        $("#list3").setRowData(data.data.icdno, data.data, { background: "#d0e9c6" });
                        document.getElementById("form6").reset();
                        SuccessAlert(data.message);
                    }
                    else
                    {
                        ErrorAlert(data.message);
                    }
                },
                error: function (data)
                {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });

    $("#tab_5").on("click", "a", function (event) {
        $("#tab_6").removeClass("active");
        $("#tab_5").addClass("active");
        $("#tab_c6").removeClass("active");
        $("#tab_c5").removeClass("hidden");
        $("#tab_c6").addClass("hidden");
        $("#tab_c5").addClass("active");
        $("#c3_save_but").removeClass("hidden");
        $("#m3_save_but").removeClass("active");
        $("#c3_save_but").addClass("active");
        $("#m3_save_but").addClass("hidden");
    });
    $("#tab_6").on("click", "a", function (event) {
        $("#tab_5").removeClass("active");
        $("#tab_6").addClass("active");
        $("#tab_c5").removeClass("active");
        $("#tab_c6").removeClass("hidden");
        $("#tab_c5").addClass("hidden");
        $("#tab_c6").addClass("active");
        $("#m3_save_but").removeClass("hidden");
        $("#c3_save_but").removeClass("active");
        $("#m3_save_but").addClass("active");
        $("#c3_save_but").addClass("hidden");
    });
});//grid2
function delete_del2(cellValue, options, rowdata, action) {

    var id = rowdata.icdno;
    var html = "";
    if (rowdata.check_name == "Defective" || rowdata.check_name == "Pass") { }
    else {
        html = '<button class="btn btn-xs btn-danger" onclick="delDate2(' + id + ');">delete</button>';
    }
    return html;
}
function delDate2(id) {
    $.ajax({
        url: "/QMS/Delete_qc_itemcheck_dt?id=" + id,
        type: "Delete",
        dataType: "json",
        success: function (data) {
            jQuery("#list3").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (data) {
            alert('Error');
        }
    });
}
