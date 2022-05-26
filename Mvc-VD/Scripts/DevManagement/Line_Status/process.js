$grid = $("#list").jqGrid
({
    url: "/DevManagement/GetProcess",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
    { key: true, label: 'lid', name: 'lid', width: 100, align: 'center', hidden: true, },
    { key: false, label: 'Factory', name: 'fac_cd', width: 100, align: 'center', },
    { key: false, label: 'Factory Name', name: 'fac_cd_nm', width: 100, align: 'center', },
    { key: false, label: 'Line No', name: 'line_no', width: 100, align: 'center', hidden: true, },
    { key: false, label: 'Line No', name: 'line_no1', width: 100, align: 'center', formatter: process_popup },
    { key: false, label: 'Name', name: 'line_nm', width: 250, align: 'left', formatter: titlebom },
    { key: false, label: 'Name', name: 'line_nm', width: 250, align: 'left', hidden: true },
    { key: false, label: 'sid', name: 'sid', sortable: true, width: '100px', align: 'center', hidden: true },
    { key: false, label: 'Bom', name: 'bom_no', sortable: true, width: '100px', align: 'center' },
    { key: false, label: 'Style', name: 'style_no', width: 100, align: 'center' },
    { key: false, label: 'Mode code', name: 'md_cd', sortable: true, width: '100px', align: 'center' },
    { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200' },
    { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
    { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px' },
    { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
    { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px' },
    { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180' },
    { key: false, label: 'Remark', name: 'cav', editable: true, width: '100px' },
    { key: false, label: 'Need Time', name: 'need', editable: true, width: '100px', align: 'right' },
    {
        label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
        formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
    },
    ],
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
        $('#m_process_no').val(row_id.line_no);
        $('#s_process_no').val(row_id.line_no);
        $('#m_process_nm').val(row_id.line_nm);
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

        $("#tab_8").removeClass("active");
        $("#tab_7").addClass("active");
        $("#tab_c8").removeClass("active");
        $("#tab_c7").removeClass("hidden");
        $("#tab_c8").addClass("hidden");
        $("#tab_c7").addClass("active");

        if (row_id != null) {
            var pid = row_id.lid;
            var id = row_id.line_no;
            if (id != undefined && id != '') {
                _GetPartcode(id);
                _GetBefor(id);
            }
            $.ajax({
                url: "/DevManagement/getbieudo?" + "pid=" + pid + "&style_no=" + row_id.style_no + "&mode=" + row_id.md_cd,
                type: "get",
                dataType: "json",
                success: function (result) {
                    $('#list5').html(result);
                },
                error: function (data) {
                    alert('No previous or subsequent process exists!');
                }
            });

        }
    },

    pager: '#jqGridPager',
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 200,
    width: $(".boxA").width(),
    caption: 'Line Information',
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
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".boxA").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});


//excel
$("#c_save_but").click(function () {

    if ($("#c_process_nm").val().trim() == "") {
        alert("Please enter your Name");
        $("#c_process_nm").val("");
        $("#c_process_nm").focus();
        return false;
    }
    if ($("#c_bom_no").val().trim() == "") {
        alert("Please enter your Bom");
        $("#c_bom_no").val("");
        $("#c_bom_no").focus();
        return false;
    }
    if ($("#c_fac_cd").val().trim() == "") {
        alert("Please enter your Factory");
        $("#c_fac_cd").val("");
        $("#c_fac_cd").focus();
        return false;
    }
    else {
        $.ajax({
            url: "/DevManagement/CreatepartProcess",
            type: "get",
            dataType: "json",
            data: {
                bom_no: $('#c_bom_no').val(),
                fac_cd: $('#c_fac_cd').val(),
            },
            success: function (data) {
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            },
            error: function (data) {
                alert('Erro!');
            }
        });
        $.ajax({
            url: "/DevManagement/Createprocess",
            type: "get",
            dataType: "json",
            data: {
                bom_no: $('#c_bom_no').val(),
                process_nm: $('#c_process_nm').val(),
                fac_cd: $('#c_fac_cd').val(),
            },
            success: function (data) {
                jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                document.getElementById("form1").reset();
            },
            error: function (data) {
                alert('Erro!');
            }
        });
    }
});
$("#m_save_but").click(function () {

    $.ajax({
        type: "get",
        dataType: "json",
        url: "/DevManagement/ModifyProcess",
        data: {
            pid: $('#m_pid').val(),
            bom_no: $('#m_bom_no').val(),
            process_no: $('#m_process_no').val(),
            process_nm: $('#m_process_nm').val(),
        },
        success: function (data) {
            jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (data) {
            alert('Erro!');
        }
    });

});
$("#start").datepicker({
    format: 'mm/dd/yyyy',
});
//$('#end').datepicker().datepicker('setDate', 'today');
// Init Datepicker end
$("#end").datepicker({
    format: 'mm/dd/yyyy',
    "autoclose": true
});
$("#searchBtn").click(function () {
    var fac_cd = $('#s_fac_cd').val().trim();
    var bom_no = $('#bom_no').val().trim();
    var md_cd = $('#md_cd').val().trim();
    var process_nm = $('#process_nm').val().trim();
    var process_no = $('#process_no').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
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
});
$grid = $("#list2").jqGrid
({
    url: "/DevManagement/GetPartProcess",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'dlpid', name: 'dlpid', width: 100, align: 'center', hidden: true },
        { key: false, label: 'Line No', name: 'line_no', width: 100, align: 'center', hidden: true },
        { key: false, label: 'Process Code', name: 'pr_no_pr_type', width: 150, align: 'center' },
        { key: false, name: 'process_no', width: 150, align: 'center', hidden: true },
        { key: false, label: 'Process Name', name: 'process_nm', sortable: true, width: '100px', align: 'left' },
        { key: false, label: 'Process Type', name: 'process_type', sortable: true, width: '100px', align: 'center' },
        { key: false, label: 'Item Code', name: 'item_vcd', sortable: true, width: 100 },
        { key: false, label: 'Step', name: 'man_num', sortable: true, width: '100px', align: 'right' },
        { key: false, label: 'Tack time', name: 'need_time', sortable: true, width: '100px', align: 'right' },
        { key: false, label: 'Color', name: 'color', editable: true, width: '100px', formatter: rgb_color_col, width: 120, },
        { key: false, label: 'Color', name: 'color', editable: true, width: '100px', hidden: true },
        { key: false, label: 'Background_image', name: 'photo_file', editable: true, width: '100px', formatter: downloadLink, align: 'center', sorttype: false, sortable: false, },
        { key: false, label: 'Background_image', name: 'photo_file', editable: true, width: '100px', hidden: true },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
        row_id = $("#list2").getRowData(selectedRowId);

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
        $("#m_mc_type").val(row_id.process_type);
        $("#m_need_time").val(row_id.need_time);
        $("#m_color").val(row_id.color);
        $('.input-group-addon > i').css('background-color', row_id.color);
        if (logo != null) {
            $("#m_logo").html('<img src="../images/' + logo + '" style="height:50px" />');

        } else {
            $("#m_logo").html("");
        }
    },

    pager: jQuery('#partpager'),
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: true,
    shrinkToFit: false,
    viewrecords: true,
    height: 200,
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
}).navGrid('#gridpager',
{
    edit: false, add: false, del: false, search: false,
    searchtext: "Search Student", refresh: true
},
{
    zIndex: 100,
    caption: "Search",
    sopt: ['cn']
});
//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
//$('#list2').jqGrid('setGridWidth', $(".box-body").width());
//$(window).on("resize", function () {
//    var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
//    $("#list2").jqGrid("setGridWidth", newWidth, false);
//});
function downloadLink(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}

function rgb_color_col(cellValue, options, rowdata, action) {
    var html = cellValue + '&nbsp' + '<button class="btn btn-xs" style="float: right; border: 1px solid silver;background-color:' + cellValue + ';color:' + cellValue + '">color</button>';
    return html;
}
$("#m2_save_but").click(function () {
    var selRowId = $('#list2').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top   Part Information on the grid.");
        return false;
    }

    else {
        var formData = new FormData();
        var files = $("#imgupload2").get(0).files;

        formData.append("file", files[0]);
        formData.append("dlpid", $('#m_ppid').val());
        formData.append("man_num", $('#m_man_num').val());
        formData.append("need_time", $('#m_need_time').val());
        formData.append("process_type", $('#m_mc_type').val());
        formData.append("color", $('#m_color').val());
        formData.append("item_vcd", $('#m_item_vcd').val());
        //formData.append("prounit_cd", $('#m_prounit_cd').val());
        formData.append("logo", $('#m_logo1').val());

        $.ajax({
            type: "POST",
            url: "/DevManagement/ModifyPartProcess",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result = true) {
                    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form4").reset();
                }
                else {
                    alert("Something Went Wrong..");
                }
            }

        });
    }
});


//grid2
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
    { label: 'pbid', name: 'pbid', key: true, align: 'center', hidden: true },
    { label: 'Line No', name: 'line_no', sortable: true, align: 'center' },
    { label: 'Process Code', name: 'process_no', width: 150, align: 'center' },
    { label: 'Next Process', name: 'next_process_no', sortable: true, width: '100px', align: 'center' },
    { label: 'level', name: 'level', sortable: true, width: '100px', align: 'center' },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list4").jqGrid("getGridParam", 'selrow');
        row_id = $("#list4").getRowData(selectedRowId);


        $('#m_pbid').val(row_id.pbid);
        $('#gm_part_no').val(row_id.process_no);
        $('#bem_part_no').val(row_id.next_process_no);

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
    },
    pager: jQuery('#seguencepager'),
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: true,
    shrinkToFit: false,
    viewrecords: true,
    height: 200,
    caption: 'Process Seguence',
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
    searchtext: "Search ", refresh: true
},
{
    zIndex: 100,
    caption: "Search ",
    sopt: ['cn']
});




$("#c4_save_but").click(function () {


    if ($("#g_part_no").val() == "") {
        alert("please enter the Part");
        $("#g_part_no").val("");
        $('#g_part_no').focus();
        return false;
    }
    if ($("#be_part_no").val() == "") {
        alert("please enter the Befor");
        $("#be_part_no").val("");
        $('#be_part_no').focus();
        return false;
    }
    else {
        var a = $('#be_part_no').val();
        var b = $('#g_part_no').val();
        if (a != b) {
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
                    alert("successfully bom detail Insert");
                    jQuery("#list4").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
                    var id = $('#m_process_no').val();
                    if (id != undefined && id != '') {
                        _GetPartcode(id);
                        _GetBefor(id);
                    }
                    //$.ajax({
                    //    url: "/DevManagement/getbieudo",
                    //    type: "get",
                    //    dataType: "json",
                    //    data: {
                    //        process_no: $('#m_process_no').val(),
                    //    },
                    //    success: function (result) {
                    //        $('#list5').html(result);
                    //    },
                    //    error: function (data) {
                    //        //alert('No previous or subsequent process exists!');
                    //    }
                    //});
                },
                error: function (data) {
                    alert("Something Went Wrong..");
                }
            });
        }
        else {
            alert("Not coincide..");
        }
    }
});
$("#m4_save_but").click(function () {
    if ($("#gm_part_no").val().trim() == "") {
        alert("please enter the Part");
        $("#gm_part_no").val("");
        $('#gm_part_no').focus();
        return false;
    }
    if ($("#bem_part_no").val().trim() == "") {
        alert("please enter the Befor");
        $("#bem_part_no").val("");
        $('#bem_part_no').focus();
        return false;
    } else {
        var selRowId = $('#list4').jqGrid("getGridParam", "selrow");
        if (selRowId == null) {
            alert("Please select the top bom detail on the grid.");
            return false;
        } else {
            var a = $('#bem_part_no').val();
            var b = $('#gm_part_no').val();
            if (a != b) {
                $.ajax({
                    url: "/DevManagement/updateseguence",
                    type: "post",
                    dataType: "json",
                    data: {
                        line_no: $('#gm_part_no').val(),
                        next_part_no: $('#bem_part_no').val(),
                        pbid: $('#m_pbid').val(),

                    },
                    success: function (data) {
                        alert("successfully bom detail Update");
                        jQuery("#list4").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
                        //$.ajax({
                        //    url: "/DevManagement/getbieudo",
                        //    type: "get",
                        //    dataType: "json",
                        //    data: {
                        //        process_no: $('#m_process_no').val(),
                        //    },
                        //    success: function (result) {
                        //        $('#list5').html(result);
                        //    },
                        //    error: function (data) {
                        //        alert('No previous or subsequent process exists!');
                        //    }
                        //});
                    },
                    error: function (data) {
                        alert("Something Went Wrong..");
                    }
                });

            }
            else {
                alert("Not coincide..");
            }
        }
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
            html += '<option value="" selected="selected">*Process No*</option>';
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
            html += '<option value="" selected="selected">*Next Process*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
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
            html += '<option value="" selected="selected">*Next Process*</option>';
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
            html += '<option value="" selected="selected">*Next Process*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_no + '>' + item.process_no + '</option>';
            });
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
        var html = '<a  data-toggle="modal"   style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="Title(' + lid + ');" class="poupgbom">' + cellvalue + '</a>';
        return html;
    } else {
        return cellvalue;
    }
} //options.rowid
function process_popup(cellvalue, options, rowobject) {
    var pid = rowobject.lid;
    var sid = rowobject.sid;
    if (cellvalue != null) {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        c = parseInt(b);
        var html = '<a  data-toggle="modal"   style="color: dodgerblue;" data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="process_1(' + pid + ',' + sid + ');" class="poupgbom">' + a + c + '</a>';
        return html;
    } else {
        return cellvalue;
    }
}
function process_1(pid, sid) {
    $('.dialog4').dialog('open');
    $.ajax({
        url: "/DevManagement/getbieudo?" + "pid=" + pid + "&sid=" + sid,
        type: "get",
        dataType: "json",
        success: function (result) {
            $('#popuppro').html(result);
        },
        error: function (data) {
            alert('No previous or subsequent process exists!');
        }
    });
}
function Title(lid) {
    $('.dialog3').dialog('open');
    $.ajax({
        url: '/DevManagement/GetBomMgt1?' + "lid=" + lid,
        type: "get",
        dataType: "json",
        success: function (result) {
            $("#popupbom1").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });
    $(document).ready(function () {
        Addhtmlexport(lid);
        _getinfo(lid);
    });
}
var getinfo = "/DevManagement/getinfo";

function _getinfo(lid) {
    $.get(getinfo + "?id=" + lid, function (data) {
        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<table id="info"><tr><td> <h4 class="tieu_de">Line Information</h4></td></tr><tr><td>Code no </td><td>' + item.process_no + '</td><td>SS Version</td>    <td>' + item.ssver + '</td><td>Customer Rev</td><td>' + item.cust_rev + '</td><td>CAV</td><td>' + item.cav + '</td><td>Register date </td><td>' + item.reg_dt + '</td> </tr>'
                html += '<tr><td>Mode code</td><td>' + item.md_cd + '</td><td>Part Name</td>   <td>' + item.part_nm + '</td><td>Ver</td> <td>' + item.Ver + '</td><td>Type</td><td>' + item.bom_type + '</td></tr>';
                html += '<tr><td>Project Name</td><td>' + item.process_nm + '</td><td>Spec</td><td>' + item.spec + '</td><td>Packing Amount(AE)</td><td>' + item.pack_amt + '</td> <td>TDS no</td><td>' + item.tds_no + '</td></tr></table>';
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
        width: '95%',
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
            //cellattr: form
            //cellattr: jsFormatterCell_2
            //cellattr: jsFormatterCell_3
            $("#popupbom1").jqGrid
            ({
                url: '/DevManagement/GetBomMgt1',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                { key: true, label: 'pmid', name: 'pmid', width: 100, align: 'center', hidden: true },
                { label: 'pid', name: 'pid', width: 100, align: 'center', hidden: true },
                { label: 'Line No', name: 'line_no', width: 100, align: 'center', cellattr: form },
                { label: 'Process no', name: 'process_no', width: 100, align: 'center', },
                { label: 'Man Number', name: 'man_num', width: '100px', align: 'center', },
                { label: 'Machine', name: 'mc_no', sortable: true, width: '100px', align: 'center', },
                { label: 'Material Code', name: 'mt_no', key: true, width: 150, align: 'center' },
                { label: 'Material Name', name: 'mt_nm', sortable: true, },
                { label: 'Material Width', name: 'width', sortable: true, width: 50, align: 'right' },
                { label: 'Requirements', name: 'need_qty', width: 60, align: 'right' },
                { label: 'Feeding', name: 'feed_size', width: 100, align: 'right' },
                { label: 'Unit', name: 'feed_unit', width: 100, align: 'left' },
                { label: 'Division', name: 'div_cd', width: 100, },
                { label: 'Change Date', name: 'chg_dt', width: 110, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
                { label: 'Change Number', name: 'chg_ver', width: 60, align: 'right' },
                { label: 'Reason for Change ', name: 'chg_cd', width: 150, align: 'left', hidden: true },
                { label: 'Reason for Change ', name: 'chg_cd1', width: 150, align: 'left' },
                { label: 'Note', name: 'Remark', width: 90, align: 'center' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupbom").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupbom").getRowData(selectedRowId);
                    $('#del_save_but').removeAttr('disabled');

                },
                gridComplete: function () {  // 그리드 생성 후 호출되는 이벤트
                    var grid = this;
                    $('td[rowspan="1"]', grid).each(function () {
                        var spans = $('td[rowspanid="' + this.id + '"]', grid).length + 1;
                        if (spans > 1) {
                            $(this).attr('rowspan', spans);
                        }
                    });
                },
                userDataOnFooter: true, // use the userData parameter of the JSON response to display data on footer
                pager: jQuery('#pagerStyle'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'Detail',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
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

        },
    });


    $('#closebom1').click(function () {
        $('.dialog3').dialog('close');
    });

});
$(function () {
    $(".dialog4").dialog({
        width: '50%',
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


    $('#closepro').click(function () {
        $('.dialog4').dialog('close');
    });

});

//이전셀의 값

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
    $.get("/DevManagement/get_mc_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Process Type*</option>';
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
            html += '<option value="" selected="selected">*Factory*</option>';
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
