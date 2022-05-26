
$grid = $("#list").jqGrid
    ({
        url: "/DevManagement/GetProcess",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['lid', 'Line No', 'Routing Code', 'Name', 'Name', 'sid', 'Bom', 'Bom', 'Product Code', 'Product Name',

            'Model', 'Project Name', 'SS Version', 'Part Name', 'Part Name', 'Standard', 'Customer Rev', 'Order Number', 'Cavit', 'Lead Time(EA/h)'],
        colModel: [
            { key: true, name: 'lid', width: 100, align: 'center', hidden: true, },
            { key: false, name: 'line_no', width: 100, align: 'center', hidden: true, },
            { key: false, name: 'line_no1', width: 100, align: 'center', formatter: process_popup },
            { key: false, name: 'line_nm', width: 250, align: 'left', formatter: titlebom },
            { key: false, name: 'line_nm', width: 250, align: 'left', hidden: true },
            { key: false, name: 'sid', sortable: true, width: '100px', align: 'center', hidden: true },
            { key: false, name: 'bom_no', sortable: true, width: '100px', align: 'center', hidden: true },
            { key: false, name: 'bom_no1', sortable: true, width: '100px', formatter: bom_no },
            { key: false, name: 'style_no', width: 150, align: 'left' },
            { key: false, name: 'style_nm', width: 300, align: 'left' },
            { key: false, name: 'md_cd', sortable: true, width: '300', align: 'left' },
            { key: false, name: 'prj_nm', sortable: true, width: '200' },
            { key: false, name: 'ssver', editable: true, width: '100px' },
            { key: false, name: 'part_nm', editable: true, width: '100px' },
            { key: false, name: 'standard', editable: true, width: '100px', align: 'center' },
            { key: false, name: 'cust_rev', editable: true, width: '100px' },
            { key: false, name: 'order_num', editable: true, width: '180' },
            { key: false, name: 'cav', editable: true, width: '100px' },
            { key: false, name: 'need', editable: true, width: '100px', align: 'right', formatter: 'integer' },

            {
                label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
            },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            var a = row_id.bid;
            var line_no = row_id.line_no
            var bom_no = row_id.bom_no
            $('#m_pid').val(row_id.lid);
            $('#m_fac_cd').val(row_id.fac_cd);
            $('#m_bom_no').val(row_id.bom_no);


            var a = row_id.bom_no.substr(0, 1);
            var b = row_id.bom_no.substr(1, 11);
            var d = row_id.bom_no.substr(row_id.bom_no.length - 1, 1);
            var c = parseInt(b);
            var gtri_bom = a + c + d;
            $('#m_bom_no1').val(gtri_bom);
            $('#m_process_no').val(row_id.line_no);
            $('#s_process_no').val(row_id.line_no);
            $('#m_process_nm').val(row_id.line_nm);

            $('#status_pid').val(row_id.lid);
            $('#status_style_no').val(row_id.style_no);
            $('#status_md_cd').val(row_id.md_cd);

            $("#c_prounit_cd").val("");
            document.getElementById("form4").reset();
            $("#list2").setGridParam({ url: "/DevManagement/GetPartProcess?" + "line_no=" + line_no, datatype: "json" }).trigger("reloadGrid");
            $("#list4").setGridParam({ url: "/DevManagement/Getseguen?" + "line_no=" + line_no, datatype: "json" }).trigger("reloadGrid");
            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");
            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
            $("#m2_save_but").addClass("disabled");
            $("#del_part_save_but").addClass("disabled");

            $("#tab_8").removeClass("active");
            $("#tab_7").addClass("active");
            $("#tab_c8").removeClass("active");
            $("#tab_c7").removeClass("hidden");
            $("#tab_c8").addClass("hidden");
            $("#tab_c7").addClass("active");
            //popup
            var style = row_id.style_no
            var style_nm = row_id.style_nm
            var line_no = row_id.line_no
            var html1 = "PD-" + line_no;
            $('#barcode').append(html1);
            $('#barcode').barcode(html1, "code128", { barWidth: 1, barHeight: 30 });

            title_popup(style, style_nm);
            //end_popup
            if (row_id != null) {
                var pid = row_id.lid;
                var id = row_id.line_no;
                if (id != undefined && id != '') {
                    _GetPartcode(id);
                    _GetBefor(id);
                }
                $.ajax({
                    //url: "/DevManagement/getbieudo_songsong?" + "pid=" + pid + "&style_no=" + row_id.style_no + "&mode=" + row_id.md_cd + "&xoa=true",
                    url: "/DevManagement/getbieudo_songsong?" + "pid=" + pid + "&style_no=" + row_id.style_no + "&mode=" + row_id.md_cd + "&xoa=true",

                    type: "get",
                    dataType: "json",
                    success: function (result) {
                        $('#list5').html(result);
                    },
                    error: function (data) {
                        alert('No previous or subsequent process exists!');
                    }
                });
                $("#m_poupdialog_BOM").addClass("hidden");

            }
        },

        pager: '#jqGridPager',
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        rownumbers: true,
        autowidth: false,
        shrinkToFit: false,
        viewrecords: true,
        height: 300,
        width: $(".boxA").width(),
        caption: 'Routing Information',
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
    });

$('#list').jqGrid('setGridWidth', $(".boxA").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});


function bom_no(cellvalue, options, rowObject) {
    var a, b;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        var d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);
        return a + c + d;
    }
    };

$(document).ready(function () {
    _getitem_type();
});
function _getitem_type() {
    $.get("/QMS/get_item_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $.each(data, function (key, item) {
                if (item.dt_cd == "PC" || item.dt_cd == "TI" || item.dt_cd == "GS") {
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                }
            });
            $("#PP_item_type").html(html);
        }
    });
}
$("#c_save_but").click(function () {

    if ($("#form1").valid() == true) {

        $.ajax({
            url: "/DevManagement/Createprocess",
            type: "get",
            dataType: "json",
            data: {
                bom_no: $('#c_bom_no').val(),
                process_nm: $('#c_process_nm').val(),
            },
            success: function (response) {
                if (response.result) {
                    var line_no = response.data.line_no;
                    var bom_no = response.data.bom_no;
                    $('#remark_color_line').val(line_no);
                    var id = response.data.lid;
                    $("#list").jqGrid('addRowData', id, response.data, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    document.getElementById("form1").reset();
                    $.ajax({
                        url: "/DevManagement/CreatepartProcess",
                        type: "get",
                        dataType: "json",
                        data: {
                            bom_no: bom_no,
                            line_no: $('#remark_color_line').val(),
                        },
                        success: function (data) {
                            $("#list2").setGridParam({ url: "/DevManagement/GetPartProcess?" + "line_no=" + line_no, datatype: "json" }).trigger("reloadGrid");
                        },
                        error: function (data) {
                            alert('Erro!');
                        }
                    });
                }
                else {
                    alert('Bom code dont have Part!Please check again');
                }

            },
            //error: function (data) {
            //    alert('Bom code dont have Part!Please check again');
            //}
        });
    }
});

$("#m_save_but").click(function () {
    if ($("#form1").valid() == true) {
        $.ajax({
            type: "get",
            dataType: "json",
            url: "/DevManagement/ModifyProcess",
            data: {
                line_no: $('#m_process_no').val(),
                line_nm: $('#m_process_nm').val(),
                bom_no: $('#m_bom_no').val(),
            },
            success: function (data) {
                var id = $('#m_pid').val();
                $("#list").setRowData(id, data, { background: "#d0e9c6" });
            },
            error: function (data) {
                alert('The Line was not existing. Please check again.');
            }
        });
    }
});

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#searchBtn").click(function () {
    var fac_cd = $('#s_fac_cd').val();
    var bom_no = $('#s_bom_no1').val();
    var md_cd = $('#md_cd').val();
    var process_nm = $('#process_nm').val();
    var process_no = $('#process_no').val();
    var start = $('#start').val();
    var end = $('#end').val();
    $.ajax({
        url: "/DevManagement/searchProcess",
        type: "get",
        dataType: "json",
        data: {
            bom_no: bom_no,
            md_cd: md_cd,
            process_nm: process_nm,
            line_no: process_no,
            start: start,
            end: end,
            fac_cd: fac_cd,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            $("#list_ex").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
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

    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
});

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$grid = $("#list2").jqGrid
    ({
        url: "/DevManagement/GetPartProcess",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'dlpid', name: 'dlpid', width: 100, align: 'center', hidden: true },
            { key: false, label: 'Line No', name: 'line_no', width: 100, align: 'center', hidden: true },
            { key: false, label: 'Part Process Code', name: 'pr_no_pr_type', width: 150 },
            { key: false, name: 'process_no', width: 150, align: 'center', hidden: true },
            { key: false, label: 'Part Process Name', name: 'process_nm', sortable: true, width: '100px', align: 'left' },
            { key: false, label: 'Process Type', name: 'process_type', sortable: true, width: '100px', align: 'center' },
            { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: 100, align: 'center' },
            { key: false, label: 'Staff', name: 'man_num', sortable: true, width: '100px', align: 'right' },
            { key: false, label: 'Lead Time(EA/h)', name: 'need_unit', sortable: true, width: '100px', align: 'right', formatter: unit, hidden: true },
            { key: false, label: 'Lead Time(EA/h)', name: 'need_time', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
            { key: false, label: 'Color', name: 'color', editable: true, width: '100px', formatter: rgb_color_col, width: 120, },
            { key: false, label: 'Color', name: 'color', editable: true, width: '100px', hidden: true },
        ],
        gridComplete: function () {
            var rows = $("#list2").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var process_no = $("#list2").getCell(rows[i], "process_no");
                var giatri = $('#remark_color_process').val();
                if (process_no == giatri) {
                    $("#list2").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
            row_id = $("#list2").getRowData(selectedRowId);
            $("#m_mc_type").val(row_id.process_type);
            var part_nm = row_id.process_nm;
            var bom_no = row_id.bom_no;
            var line_no = row_id.line_no;
            var process_no = row_id.process_no;
            var logo = row_id.photo_file;
            $('#m_logo1').val(logo);
            $("#m2_process_no").val(row_id.line_no);
            $("#m_item_vcd").val(row_id.item_vcd);
            $("#c_part_code").val(row_id.process_no);
            $("#m_part_no").val(row_id.process_no);
            $("#m_part_nm").val(row_id.process_nm);
            $("#m_ppid").val(row_id.dlpid);
            $("#m_man_num").val(row_id.man_num);

            $("#m_need_time").val(row_id.need_time);
            $("#m_color").val(row_id.color);
            $("#m2_save_but").removeClass("disabled");
            $("#del_part_save_but").removeClass("disabled");


            $('.input-group-addon > i').css('background-color', row_id.color);
            //if (logo != null) {
            //    $("#m_logo").html('<img src="../images/' + logo + '" style="height:50px" />');

            //} else {
            //    $("#m_logo").html("");
            //}
        },

        pager: jQuery('#partpager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        height: 200,
        width: $(".boxB").width(),
        rownumbers: true,
        autowidth: false,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Process',
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

$('#list2').jqGrid('setGridWidth', $(".boxB").width());
$(window).on("resize", function () {
    var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
    $("#list2").jqGrid("setGridWidth", newWidth, false);
});

//function downloadLink(cellValue, options, rowdata, action) {
//    if (cellValue != null) {
//        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
//        return html;

//    } else {
//        return "";
//    }
//}

function rgb_color_col(cellValue, options, rowdata, action) {
    var html = ((cellValue == null) ? "" : cellValue) + '&nbsp' + '<button class="btn btn-xs trn" style="float: right; border: 1px solid silver;background-color:' + cellValue + ';color:' + cellValue + '">color</button>';
    return html;
}
function unit(cellValue, options, rowdata, action) {
    var html = "";
    //console.log(rowdata.need_time);
    html = '<button style="border: none;background: none; ">' + rowdata.need_time + ' (EA/h)</button>'
    return html;
}

$("#m2_save_but").click(function () {
    var selRowId = $('#list2').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top   Part Information on the grid.");
        return false;
    }
    else {
        if ($("#form4").valid() == true) {
            var formData = new FormData();
            //var files = $("#imgupload2").get(0).files;

            //formData.append("file", files[0]);
            formData.append("dlpid", $('#m_ppid').val());
            formData.append("man_num", $('#m_man_num').val());
            formData.append("need_time", $('#m_need_time').val());
            formData.append("process_type", $('#m_mc_type').val());
            formData.append("color", $('#m_color').val());
            formData.append("item_vcd", $('#m_item_vcd').val());
            formData.append("logo", $('#m_logo1').val());
            formData.append("process_no", $('#m_part_no').val());

            $.ajax({
                type: "POST",
                url: "/DevManagement/ModifyPartProcess",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (data) {
                    var id = data.dlpid;
                    $("#list2").setRowData(id, data, { background: "#d0e9c6" });
                    document.getElementById("form4").reset();
                }
            });
            refesh_graph();
        }
    }

});

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$("#m3_save_but").click(function () {
    var selRowId = $('#list3').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top  Machine Information on the grid.");
        return false;
    }
    if ($("#c_mc_no").val() == "") {
        alert("please enter the machine no");
        $("#c_mc_no").val("");
        $('#c_mc_no').focus();
        return false;
    }
    if ($("#m_part_code").val() == "") {
        alert("please enter the Part Code Unit");
        $("#m_part_code").val("");
        $('#m_part_code').focus();
        return false;
    }
    else {
        $.ajax({
            url: "/DevManagement/Updatemachineprocess",
            type: "post",
            dataType: "json",
            data: {
                mc_no: $('#c_mc_no').val(),
                lmid: $('#lmid').val(),
            },
            success: function (result) {
                $("#list3").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            },
            error: function (result) {
                alert("Something Went Wrong..");
            }
        });
    }
});

$grid = $("#list4").jqGrid
    ({
        url: "/DevManagement/Getseguen",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { label: 'lpid', name: 'lpid', key: true, align: 'center', hidden: true },
            { label: 'Routing Code', name: 'line_no', sortable: true, align: 'center' },
            { label: 'Part Process Code', name: 'process_no', width: 150 },
            { label: 'Next Process', name: 'next_process_no', sortable: true, width: '150px' },
            { label: 'level', name: 'level', sortable: true, width: '100px', align: 'center' },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list4").jqGrid("getGridParam", 'selrow');
            row_id = $("#list4").getRowData(selectedRowId);
            $('#m_pbid').val(row_id.lpid);
            $('#gm_part_no').val(row_id.process_no);
            $('#bem_part_no').val(row_id.next_process_no);
            $('#level_next').val(row_id.level);
            if (row_id != null) {
                var line_no = row_id.line_no;
                var process_no = row_id.process_no;
                var next_process_no = row_id.next_process_no;
                var level = row_id.level;
                if (line_no != undefined && line_no != '') {
                    _GetMPartcode(line_no, process_no, next_process_no, level);
                    _GetMBefor(line_no, process_no, next_process_no, level)
                }
            }

            $("#tab_7").removeClass("active");
            $("#tab_8").addClass("active");
            $("#tab_c7").removeClass("active");
            $("#tab_c8").removeClass("hidden");
            $("#tab_c7").addClass("hidden");
            $("#tab_c8").addClass("active");
            $("#m4_save_but").attr("disabled", false);
            $("#del_bomdt_save_but").attr("disabled", false);
        },
        pager: jQuery('#seguencepager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        height: 200,
        width: $(".boxC").width(),
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Process Sequence',

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

$('#list4').jqGrid('setGridWidth', $(".boxC").width());
$(window).on("resize", function () {
    var newWidth = $("#list4").closest(".ui-jqgrid").parent().width();
    $("#list4").jqGrid("setGridWidth", newWidth, false);
});

function refesh_graph() {
    var pid = $('#status_pid').val();
    var style_no = $('#status_style_no').val();
    var md_cd = $('#status_md_cd').val();
    $.ajax({
        url: "/DevManagement/getbieudo_songsong?" + "pid=" + pid + "&style_no=" + style_no + "&mode=" + md_cd + "&xoa=true",
        type: "get",
        dataType: "json",
        success: function (result) {
            $('#list5').html(result);
        },
        error: function (data) {

        }
    });
}
function mydelete(lpid) {
    $("#status_delete").val("del_bd");
    $("#id_val_bd").val(lpid);
    $('#dialogDangerous_del').dialog('open');
}

function mydelete_ok(lpid) {
    $.ajax({
        url: "/DevManagement/xoa_inbd?" + "lpid=" + lpid,
        type: "get",
        dataType: "json",
        success: function (result) {
            var id = $('#m_process_no').val();
            if (id != undefined && id != '') {
                _GetPartcode(id);
                _GetBefor(id);
            }
            $('#delete_bd').val("false");
            refesh_graph();
            $("#list4").setGridParam({ datatype: "json" }).trigger('reloadGrid');
        },
        error: function (data) {
        }
    });
}
$("#c4_save_but").click(function () {
    if ($("#form7").valid() == true) {

        var a = $('#be_part_no').val();
        var b = $('#g_part_no').val();


        var count_row_list2 = jQuery("#list2").jqGrid('getGridParam', 'records');
        var count_row_list4 = jQuery("#list4").jqGrid('getGridParam', 'records');

        if (count_row_list2 == 1 && a == b && count_row_list4 == 0 || a != b) {
            $.ajax({
                url: "/DevManagement/insertbefor",
                type: "post",
                dataType: "json",
                data: {
                    line_no: $('#m_process_no').val(),
                    process_no: $('#g_part_no').val(),
                    befor_process_no: $('#be_part_no').val(),

                },
                success: function (data) {
                    var id = data.lpid;
                    $("#list4").jqGrid('addRowData', id, data, 'first');
                    $("#list4").setRowData(id, false, { background: "#d0e9c6" });
                    refesh_graph();
                    var id = $('#m_process_no').val();
                    if (id != undefined && id != '') {
                        _GetPartcode(id);
                        _GetBefor(id);
                    }
                },
                error: function (data) {
                    alert("Something Went Wrong..");
                }
            });
        }
        else {
            alert("Next Process must be different Process No ");
        }

        refesh_graph();
    }
});



$("#cr_save_but").click(function () {
    if ($('#bem_part_no').val() == "") {
        alert("Please select the Grid.");
        return;
    }
    else {
        $.ajax({
            url: "/DevManagement/insertbefor_cr",
            type: "post",
            dataType: "json",
            data: {
                line_no: $('#m_process_no').val(),
                on_process_no: $('#bem1_part_no').val(),//tren Process No
                on_Next_Process: $('#gm1_part_no').val(),//tren  Next Process

                level: $('#level_next').val(),
                down_process_no: $('#bem_part_no').val(),//duoi Process No
                down_Next_Process: $('#gm_part_no').val(),//duoi Next Process

                root_yn: ($('#c_root_yn').prop('checked') == true) ? "1" : "",
            },
            success: function (data) {
                if (data.count == 2) {
                    for (var i = 0; i < data.result.length; i++) {
                        var id = data.result[i].lpid;
                        $("#list4").jqGrid('addRowData', id, data.result[i], 'first');
                        $("#list4").setRowData(id, false, { background: "#d0e9c6" });
                    }
                }

                if (data.count == 1) {
                    var id = data.result.lpid;
                    $("#list4").jqGrid('addRowData', id, data.result, 'first');
                    $("#list4").setRowData(id, false, { background: "#d0e9c6" });

                }
                if (data.count == 0) {
                    alert(data.message);
                }

                refesh_graph();

                var id = $('#m_process_no').val();
                if (id != undefined && id != '') {
                    _GetPartcode(id);
                    _GetBefor(id);
                }
                document.getElementById("c_root_yn").checked = false;
            },
            error: function (data) {
                alert("Something Went Wrong..");
            }
        });
        //var models = [];
        //var selRowIds = $("#list4").jqGrid("getGridParam", "selarrrow");
        //var event_tmp = [];
        //for (i = 0, n = selRowIds.length; i < n; i++) {
        //    id = selRowIds[i];
        //    var ret = $("#list4").jqGrid('getRowData', selRowIds[i]);
        //    var item = {
        //        lpid: ret.lpid,
        //        process_no: ret.process_no,
        //        next_process_no: ret.next_process_no,
        //        level: ret.level,
        //        line_no: ret.line_no,
        //    };
        //    event_tmp.push(item);
        //}

        //var item = {
        //    list: event_tmp,
        //    line_no: $('#m_process_no').val(),
        //    on_process_no: $('#bem1_part_no').val(),//tren Process No
        //    on_Next_Process: $('#gm1_part_no').val(),//tren  Next Process
        //};
        //models.push(item);


        //$.ajax({
        //    type: 'POST',
        //    url: '/DevManagement/insertbefor_cr_kieu2',
        //    async: true,
        //    dataType: 'json',
        //    contentType: "application/json; charset=utf-8",
        //    data: JSON.stringify(models),
        //    traditonal: true,
        //    success: function (response) {
        //        if (data.count == 1) {
        //                        for (var i = 0; i < data.result.length; i++) {
        //                            var id = data.result[i].lpid;
        //                            $("#list4").jqGrid('addRowData', id, data.result[i], 'first');
        //                            $("#list4").setRowData(id, false, { background: "#d0e9c6" });
        //                        }
        //                    }

        //        var id = $('#m_process_no').val();
        //        if (id != undefined && id != '') {
        //            _GetPartcode(id);
        //            _GetBefor(id);
        //        }
        //        refesh_graph();
        //    },
        //    error: function (response) {

        //    }
        //});
        refesh_graph();
    }
});

$("#m4_save_but").click(function () {
    if ($("#form8").valid() == true) {
        var a = $('#bem_part_no').val();
        var b = $('#gm_part_no').val();
        if (a != b) {
            $.ajax({
                url: "/DevManagement/updateseguence",
                type: "get",
                dataType: "json",
                data: {
                    process_no: b,
                    next_process_no: a,
                    pbid: $('#m_pbid').val(),

                },
                success: function (data) {
                    var id = data.lpid;
                    $("#list4").setRowData(id, data, { background: "#d0e9c6" });
                    var id = $('#m_process_no').val();
                    if (id != undefined && id != '') {
                        _GetPartcode(id);
                        _GetBefor(id);
                    }
                },
                error: function (data) {
                    alert("Something Went Wrong..");
                }
            });
        }
        else {
            alert("Next Process must be different Process No ");
        }

        var pid = $('#status_pid').val();
        var style_no = $('#status_style_no').val();
        var md_cd = $('#status_md_cd').val();
        //$.ajax({
        //    url: "/DevManagement/getbieudo?" + "pid=" + pid + "&style_no=" + style_no + "&mode=" + md_cd,
        //    type: "get",
        //    dataType: "json",
        //    success: function (result) {
        //        $('#list5').html(result);
        //    },
        //    error: function (data) {
        //        alert('No previous or subsequent process exists!');
        //    }
        //});
    }
});

$("#tab_7").on("click", "a", function (event) {
    document.getElementById("form7").reset();
    $("#tab_8").removeClass("active");
    $("#tab_7").addClass("active");
    $("#tab_c8").removeClass("active");
    $("#tab_c7").removeClass("hidden");
    $("#tab_c8").addClass("hidden");
    $("#tab_c7").addClass("active");

});
$("#tab_8").on("click", "a", function (event) {

    document.getElementById("form8").reset();
    $("#tab_7").removeClass("active");
    $("#tab_8").addClass("active");
    $("#tab_c7").removeClass("active");
    $("#tab_c8").removeClass("hidden");
    $("#tab_c7").addClass("hidden");
    $("#tab_c8").addClass("active");
    $("#m4_save_but").attr("disabled", true);
    $("#del_bomdt_save_but").attr("disabled", true);
});
// Khai báo URL stand của bạn ở đây


//grid4
$(document).ready(function () {
    $("#c_bom_no").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Autobom",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.bom_no, value: item.bom_no };
                    }))

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });

    $("#m_bom_no").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Autobom",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.bom_no, value: item.bom_no };
                    }))

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });

    $("#c_mc_no").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Automachine",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.mc_no, value: item.mc_no };
                    }))

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });

    $("#m_mc_no").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Automachine",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.mc_no, value: item.mc_no };
                    }))

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });
})



// Khai báo URL stand của bạn ở đây
var baseService = "/DevManagement";
var GetPartcode = baseService + "/GetPartcode";
var GetBefor = baseService + "/GetPartcode";
var GetCodeM = baseService + "/GetCodeM";
var GetCodeNext = baseService + "/GetCodeNext";
function _GetPartcode(id) {
    $.get(GetPartcode + "/" + id, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected" class="trn">Process No</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
            $("#g_part_no").html(html);

        }
    });
}

function _GetMPartcode(line_no, process_no, next_process_no, level) {

    $.get(GetCodeM + "?line_no=" + line_no + "&process_no=" + process_no + "&next_process_no=" + next_process_no + "&level=" + level, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected" class="trn">Next Process</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
            $("#gm1_part_no").html(html);
            $("#gm_part_no").html(html);
            $('#gm_part_no').val(process_no);
            $("#tab_7").removeClass("active");
            $("#tab_8").addClass("active");
            $("#tab_c7").removeClass("active");
            $("#tab_c8").removeClass("hidden");
            $("#tab_c7").addClass("hidden");
            $("#tab_c8").addClass("active");
            $("#m4_save_but").attr("disabled", false);
        }
    });
}
//befor
function _GetBefor(id) {
    $.get(GetBefor + "/" + id, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected" class="trn">Next Process</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
            $("#be_part_no").html(html);

        }
    });
}
//modi befor

function _GetMBefor(line_no, process_no, next_process_no, level) {

    $.get(GetCodeNext + "?line_no=" + line_no + "&process_no=" + process_no + "&next_process_no=" + next_process_no + "&level=" + level, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected" class="trn">Process No</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
            $("#bem1_part_no").html(html);
            $("#bem_part_no").html(html);
            $('#bem_part_no').val(next_process_no);
            $("#tab_7").removeClass("active");
            $("#tab_8").addClass("active");
            $("#tab_c7").removeClass("active");
            $("#tab_c8").removeClass("hidden");
            $("#tab_c7").addClass("hidden");
            $("#tab_c8").addClass("active");
            $("#m4_save_but").attr("disabled", false);
        }
    });
}

function bntCellValue(cellValue, options, rowdata, action) {
    var id = rowdata.pid;
    var html = '<button class="btn btn-xs btn-danger" onclick="delDate(' + id + ');">delete</button>';
    return html;
}
function delDate(id) {
    $.ajax({
        url: "/DevManagement/deleteProcess?id=" + id,
        type: "post",
        dataType: "json",
        success: function (result) {
            $("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            document.getElementById("form1").reset();
            $("#tab_2").removeClass("active");
            $("#tab_1").addClass("active");
            $("#tab_c2").removeClass("active");
            $("#tab_c1").removeClass("hidden");
            $("#tab_c2").addClass("hidden");
            $("#tab_c1").addClass("active");


        },
        error: function (result) {
            alert('User is not existing. Please check again');
        }
    });
}
//tiltle bom

function titlebom(cellvalue, options, rowobject) {
    var lid = rowobject.lid;
    if (cellvalue != null) {
        //var html = '<a href="#" data-toggle="modal"   style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="Title(' + lid + ');" class="poupgbom">' + cellvalue + '</a>';
        var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="Title(' + lid + ');" class="poupgbom">' + cellvalue + '</button>';
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid
function process_popup(cellvalue, options, rowobject) {
    var pid = rowobject.lid;
    var sid = rowobject.sid;
    if (cellvalue != null) {
        //var html = '<a href="#" data-toggle="modal"   style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="process_1(' + pid + ',' + sid + ');" class="poupgbom">' + cellvalue + '</a>';
        var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="process_1(' + pid + ',' + sid + ');" class="poupgbom">' + cellvalue + '</button>';
        return html;
    } else {
        return cellvalue;
    }
}
function process_1(pid, sid) {
    $('.dialog4').dialog('open');
    $("#pp_lid").val(pid);
    $("#pp_sid").val(sid);
    $.ajax({
        url: "/DevManagement/getbieudo_songsong?" + "pid=" + pid + "&sid=" + sid + "&xoa=false",
        type: "post",
        dataType: "json",
        success: function (result) {
            $('#popuppro').html(result);
        },
        error: function (data) {
            alert('No previous or subsequent process exists!');
        }
    });
    $.ajax({
        url: "/DevManagement/GetProcess1?lid=" + pid,
        type: "post",
        dataType: "json",
        success: function (result) {
            //console.log(result);


            var catchuoi = result[0].bom_no;

            catchuoi = catchuoi.substr(0, 1) + parseInt(catchuoi.substr(1, 11)) + catchuoi.substr(11, 1);


            var html = '<td>' + result[0].line_no + '</td>';
            html += '<td>' + result[0].line_nm + '</td>';
            html += '<td>' + catchuoi + '</td>';
            html += '<td>' + result[0].style_no + '</td>';
            html += '<td>' + result[0].style_nm + '</td>';
            html += '<td>' + result[0].md_cd + '</td>';
            html += '<td>' + result[0].prj_nm + '</td>';
            html += '<td>' + result[0].ssver + '</td>';
            $("#popup_line").html(html);
            //$("#popupline").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        },
    });

}
function title_popup(style, style_nm) {
    $("#title1").html("<h1 style='font-size: 43px;'>ROUTING</h1><h2 style='font-size: 20px;'>" + style + " " + '[' + style_nm + " " + ']' + "</h2>");
}
function Title(lid) {
    $("#pp_lid1").val(lid);
    $('.dialog3').dialog('open');
    $('#popupbom1').DataTable({
        "bServerSide": true,
        "ajax": {
            "url": '/DevManagement/GetBomMgt1?lid=' + lid,
            "type": "Post",
            "datatype": "json"
        },
        "searching": false,
        "paging": false,
        "bInfo": false,
        "columns": [
            { "data": "RowNum" },
            { "data": "line_no" },
            { "data": "level" },
            { "data": "process_part" },
            { "data": "process_type" },
            { "data": "need_time" },
            { "data": "man_num" },
            { "data": "feed_size" },
            { "data": "feed_unit" },
            { "data": "div_cd" },
            { "data": "mt_no" },
            { "data": "mt_nm" },
            { "data": "width" },
            { "data": "need_qty" },
        ],
        'columnDefs': [
            {
                "targets": 0, // your case first column
                "className": "text-center",
            },
            {
                "targets": 1, // your case first column
                "className": "text-center",
            },
            {
                "targets": 2, // your case first column
                "className": "text-left",
            },
            {
                "targets": 3, // your case first column
                "className": "text-center",
            },
            {
                "targets": 4, // your case first column
                "className": "text-right",
            },
            {
                "targets": 5, // your case first column
                "className": "text-right",
            },
            {
                "targets": 6, // your case first column
                "className": "text-right",
            },
            {
                "targets": 12, // your case first column
                "className": "text-right",
            },
            {
                "targets": 11, // your case first column
                "className": "text-right",
            },
        ],
        'rowsGroup': [0, 1, 2, 3],
        "bDestroy": true,

    });
    Addhtmlexport(lid);
    _getinfo(lid);
};
var getinfo = "/DevManagement/getinfo";

function _getinfo(lid) {

    $.get(getinfo + "?id=" + lid, function (data) {
        if (data != null && data != undefined) {
            $.each(data, function (key, item) {

                var bom_no_hien = item.bom_no;

                bom_no_hien = bom_no_hien.substr(0, 1) + parseInt(bom_no_hien.substr(1, 11)) + bom_no_hien.substr(11, 1);

                var html = '';
                var html = '<td>' + item.line_no + '</td>';
                html += '<td>' + item.line_nm + '</td>';
                html += '<td>' + bom_no_hien + '</td>';
                html += '<td>' + item.style_no + '</td>';
                html += '<td>' + item.style_nm + '</td>';
                html += '<td>' + item.md_cd + '</td>';
                html += '<td>' + item.prj_nm + '</td>';
                html += '<td>' + item.ssver + '</td>';
                html += '<td>' + item.part_nm + '</td>';
                html += '<td>' + item.standard + '</td>';
                html += '<td>' + item.cust_rev + '</td>';
                html += '<td>' + item.order_num + '</td>';
                html += '<td class="text-right">' + item.need + '</td>';
                //html += '<td>' + item.process_no + '</td>';
                var html1 = "LN-" + item.line_no;
                $('#barcode1').append(html1);
                $('#barcode1').barcode(html1, "code128", { barWidth: 1, barHeight: 30 });

                $("#title").html("<h1 style='font-size: 43px;'>Routing</h1><h2 style='font-size: 20px;'>" + item.style_no + " " + '[' + item.style_nm + " " + ']' + "</h2>");
                $("#info").html(html);
            });
        }
    });
}
function Addhtmlexport(lid) {
    var html = '';
    html += '<form action="/DevManagement/ExportToExcel?lid=' + lid + '" method="post"><button type="submit"  class="btn btn-sm btn-warning button-top"><i class="fa fa-file-excel-o">&nbsp;Excel</i></button></form>';
    $("#export").html(html);
}

$(function () {
    $(".dialog3").dialog({
        width: '70%',
        height: 700,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

        },
    });


    $('#closebom1').click(function () {
        $('.dialog3').dialog('close');
    });

});
$(function () {
    $(".dialog4").dialog({
        width: '60%',
        height: 700,
        //maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,
        resizable: true,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

        },
    });


    $('#closepro').click(function () {
        $('.dialog4').dialog('close');
    });

});

$("#PrintDetail").on("click", function () {
    var lid = $("#pp_lid").val();
    var sid = $("#pp_sid").val();
    window.open("/DevManagement/Print?" + "lid=" + lid + "&sid=" + sid, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});//end click

$("#Print").on("click", function () {
    var lid = $("#pp_lid1").val();
    window.open("/DevManagement/Printdetail_line?" + "lid=" + lid, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});

var prevCellVal = { cellId: undefined, value: undefined };
var chkcell_2 = { cellId2: undefined, chkval2: undefined };
var chkcell_3 = { cellId3: undefined, chkval3: undefined };
var chkcell_4 = { cellId4: undefined, chkval4: undefined };
var form = function (rowId, val, rawObject, cm, rdata) {

    var result;
    if (prevCellVal.value == val) {
        result = ' style="display: none" rowspanid="' + prevCellVal.cellId + '"';
    } else {
        var cellId = this.id + '_row_' + rowId + '_' + cm.name;
        result = ' rowspan="1" id="' + cellId + '"';
        prevCellVal = { cellId: cellId, value: val };
    }

    return result;

};
function jsFormatterCell_2(rowid, val, rowObject, cm, rdata) {
    var result2 = "";
    if (chkcell_2.chkval2 != val) { //check 값이랑 비교값이 다른 경우
        var cellId2 = this.id + '_row_' + rowid + '-' + cm.name;
        result2 = ' rowspan="1" id ="' + cellId2 + '" + name="cellRowspan"';
        chkcell_2 = { cellId2: cellId2, chkval2: val };
    } else {
        result2 = 'style="display:none"  rowspanid="' + chkcell_2.cellId2 + '"'; //같을 경우 display none 처리
        //alert(result);
    }
    return result2;
}
function jsFormatterCell_3(rowid, val, rowObject, cm, rdata) {
    var result3 = "";

    if (chkcell_3.chkval3 != val) { //check 값이랑 비교값이 다른 경우
        var cellId3 = this.id + '_row_' + rowid + '-' + cm.name;
        result3 = ' rowspan="1" id ="' + cellId3 + '" + name="cellRowspan"';
        //alert(result);
        chkcell_3 = { cellId3: cellId3, chkval3: val };
    } else {
        result3 = 'style="display:none"  rowspanid="' + chkcell_3.cellId3 + '"'; //같을 경우 display none 처리
        //alert(result);
    }
    return result3;
}
function jsFormatterCell_4(rowid, val, rowObject, cm, rdata) {
    var result4 = "";
    //if (rowObject.chg)
    if (chkcell_4.chkval4 != val) { //check 값이랑 비교값이 다른 경우
        var cellId4 = this.id + '_row_' + rowid + '-' + cm.name;
        result4 = ' rowspan="1" id ="' + cellId4 + '" + name="cellRowspan"';
        //alert(result);
        chkcell_4 = { cellId4: cellId4, chkval4: val };
    } else {
        result4 = 'style="display:none"  rowspanid="' + chkcell_4.cellId4 + '"'; //같을 경우 display none 처리
        //alert(result);
    }
    return result4;
}
$(document).ready(function () {
    $("#m_prounit_cd").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Autoprounit_cd",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.prounit_cd, value: item.prounit_cd };
                    }))
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });
    $("#c_prounit_cd").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Autoprounit_cd",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.prounit_cd, value: item.prounit_cd };
                    }))
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    var result = [{ label: "no results", value: response.term }];
                    response(result);
                },
            })
        },
        messages: {
            noResults: '',
        }
    });
});
$(document).ready(function () {
    _getmc_type();
    _getfactory();
});
function _getmc_type() {
    //ffff

    //cffdjkkfj
    $.get("/DevManagement/get_mc_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" class="trn">Process Type</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#m_mc_type").html(html);


        }
    });
}
function _getfactory() {
    $.get("/DevManagement/get_factory", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" >*Factory*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.index_cd + '>' + item.index_cd + '</option>';
            });
            $("#c_fac_cd").html(html);
            $("#m_fac_cd").html(html);
            $("#s_fac_cd").html(html);
        }
    });
}
$(".my-colorpicker2").colorpicker();
////////////////////////////////////////////////////////////////////////////////////////////////////////

$("#form1").validate({
    rules: {
        "process_nm": {
            required: true,
        },
        "bom_no": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "process_nm": {
            required: true,
        },
        "bom_no": {
            required: true,
        },
    },
});

$("#form7").validate({
    rules: {
        "g_part_no": {
            required: true,
        },
        "be_part_no": {
            required: true,
        },
    },
});

$("#form8").validate({
    rules: {
        "g_part_no": {
            required: true,
        },
        "be_part_no": {
            required: true,
        },
    },
});
$("#form4").validate({
    rules: {
        "mc_type": {
            required: true,
        },
        "man_num": {
            required: true,
        },
        "m_item_vcd": {
            required: true,
        },

    },
});

//Cắt chuỗi
function catchuoi($chuoi, $gioihan) {
    // nếu độ dài chuỗi nhỏ hơn hay bằng vị trí cắt
    // thì không thay đổi chuỗi ban đầu
    if (strlen($chuoi) <= $gioihan) {
        return $chuoi;
    }
    else {
        /*
        so sánh vị trí cắt
        với kí tự khoảng trắng đầu tiên trong chuỗi ban đầu tính từ vị trí cắt
        nếu vị trí khoảng trắng lớn hơn
        thì cắt chuỗi tại vị trí khoảng trắng đó
        */
        if (strpos($chuoi, " ", $gioihan) > $gioihan) {
            $new_gioihan = strpos($chuoi, " ", $gioihan);
            $new_chuoi = substr($chuoi, 0, $new_gioihan) + ".....";
            return $new_chuoi;
        }
        // trường hợp còn lại không ảnh hưởng tới kết quả
        $new_chuoi = substr($chuoi, 0, $gioihan) + ".....";
        return $new_chuoi;
    }
}
function onchange_language(language) {
    if (language == "vn") {
        //list
        jQuery("#list").jqGrid('setLabel', 'line_no1', 'Chuyền');
        jQuery("#list").jqGrid('setLabel', 'line_nm', 'Tên Chuyền');
        jQuery("#list").jqGrid('setLabel', 'style_no', 'Mã Sản Phẩm');
        jQuery("#list").jqGrid('setLabel', 'style_nm', 'Tên Sản Phẩm');
        jQuery("#list").jqGrid('setLabel', 'prj_nm', 'Tên Dự Án');
        jQuery("#list").jqGrid('setLabel', 'part_nm', 'Tên bộ phận');
        jQuery("#list").jqGrid('setLabel', 'cust_rev', 'Khách hàng Rev');
        jQuery("#list").jqGrid('setLabel', 'order_num', 'Số đơn hàng');
        jQuery("#list").jqGrid('setLabel', 'cav', 'Lỗ');
        jQuery("#list").jqGrid('setLabel', 'need', 'Thời gian dẫn(EA / h)');
        jQuery("#list").jqGrid('setLabel', 'reg_dt', 'Ngày Tạo');
         //list2

        jQuery("#list2").jqGrid('setLabel', 'pr_no_pr_type', 'Mã công đoạn');
        jQuery("#list2").jqGrid('setLabel', 'process_nm', 'Tên công đoạn');
        jQuery("#list2").jqGrid('setLabel', 'process_type', 'Loại công đoạn');
        jQuery("#list2").jqGrid('setLabel', 'item_vcd', 'Mã QC');
        jQuery("#list2").jqGrid('setLabel', 'man_num', 'Nhân Viên');
        jQuery("#list2").jqGrid('setLabel', 'need_time', 'Thời gian dẫn (EA / h)');
        jQuery("#list2").jqGrid('setLabel', 'color', 'Màu Sắc');
        //list4 
        jQuery("#list4").jqGrid('setLabel', 'line_no', 'Mã Chuyền');
        jQuery("#list4").jqGrid('setLabel', 'process_no', 'Công Đoạn trước');
        jQuery("#list4").jqGrid('setLabel', 'next_process_no', 'Công Đoạn Sau');
        jQuery("#list4").jqGrid('setLabel', 'level', 'Cấp độ');
    } else
    {
        //list
        jQuery("#list").jqGrid('setLabel', 'line_no1', 'Routing');
        jQuery("#list").jqGrid('setLabel', 'line_nm', 'Name');
        jQuery("#list").jqGrid('setLabel', 'style_no', 'Product Code');
        jQuery("#list").jqGrid('setLabel', 'style_nm', 'Product Name');
        jQuery("#list").jqGrid('setLabel', 'prj_nm', 'Project Name');
        jQuery("#list").jqGrid('setLabel', 'part_nm', 'Part Name');
        jQuery("#list").jqGrid('setLabel', 'standard', 'Standard');
        jQuery("#list").jqGrid('setLabel', 'cust_rev', 'Customer Rev');
        jQuery("#list").jqGrid('setLabel', 'order_num', 'Order Number');

        //tes
        jQuery("#list").jqGrid('setLabel', 'cav', 'Cav');
        jQuery("#list").jqGrid('setLabel', 'need', 'Lead Time(EA/h)');
        jQuery("#list").jqGrid('setLabel', 'reg_dt', 'Create Date');
        //cccc
         //list2
        jQuery("#list2").jqGrid('setLabel', 'pr_no_pr_type', 'Part Process Code');
        jQuery("#list2").jqGrid('setLabel', 'process_nm', 'Part Process Name');
        jQuery("#list2").jqGrid('setLabel', 'process_type', 'Process Type');
        jQuery("#list2").jqGrid('setLabel', 'item_vcd', 'QC Code');
        jQuery("#list2").jqGrid('setLabel', 'man_num', 'Staff');
        jQuery("#list2").jqGrid('setLabel', 'need_time', 'Lead Time(EA / h)');
        jQuery("#list2").jqGrid('setLabel', 'color', 'Color');
        //list4 
        jQuery("#list4").jqGrid('setLabel', 'line_no', 'Routing Code');
        jQuery("#list4").jqGrid('setLabel', 'process_no', 'Part Process Code');
        jQuery("#list4").jqGrid('setLabel', 'next_process_no', 'Next Process');
        jQuery("#list4").jqGrid('setLabel', 'level', 'Level');
       
    }
}
