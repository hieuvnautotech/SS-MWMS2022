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
$("#tab_3").on("click", "a", function (event) {
    document.getElementById("form3").reset();
    $("#tab_4").removeClass("active");
    $("#tab_3").addClass("active");
    $("#tab_c4").removeClass("active");
    $("#tab_c3").removeClass("hidden");
    $("#tab_c4").addClass("hidden");
    $("#tab_c3").addClass("active");

});
$("#tab_4").on("click", "a", function (event) {

    document.getElementById("form4").reset();
    $("#tab_3").removeClass("active");
    $("#tab_4").addClass("active");
    $("#tab_c3").removeClass("active");
    $("#tab_c4").removeClass("hidden");
    $("#tab_c3").addClass("hidden");
    $("#tab_c4").addClass("active");
    $("#m3_save_but").attr("disabled", true);
});
$(".my-colorpicker2").colorpicker();
$(function () {
    $("#list").jqGrid
    ({
        url: "/DevManagement/Getprocess_unint",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'puid', name: 'puid',  align: 'center', hidden: true },
            { key: false, label: 'Process Type', name: 'mc_type', width: 120, align: 'center', },
            { key: false, label: 'Process Unit Code', name: 'prounit_cd', width: 120, align: 'center', },
            { key: false, label: 'Process Unit  Name', name: 'prounit_nm', sortable: true, width: '120', },
            { key: false, label: 'Color', name: 'color', editable: true, width: '100px', formatter: rgb_color_col, width: 120, },
            { key: false, label: 'Color', name: 'color', editable: true, width: '100px' ,hidden:true},
            { key: false, label: 'Use', name: 'use_yn', editable: true, width: '100px', align: 'center', width: 120, },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow'); 
            row_id = $("#list").getRowData(selectedRowId);
            $("#puid").val(row_id.puid);
            $("#m_prounit_nm").val(row_id.prounit_nm);
            $("#m_prounit_cd").val(row_id.prounit_cd);
            $("#c2_prounit_cd").val(row_id.prounit_cd);
            $("#m_color").val(row_id.color);
            $("#m_use_yn").val(row_id.use_yn);
            $("#m_mc_type").val(row_id.mc_type);
            $('.input-group-addon > i').css('background-color', row_id.color);
            $("#list2").setGridParam({ url: "/DevManagement/Getprocess_machineunit?" + "prounit_cd=" + row_id.prounit_cd, datatype: "json" }).trigger("reloadGrid");
            $("#c_mc_no").val("");
            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");

            $("#tab_4").removeClass("active");
            $("#tab_3").addClass("active");
            $("#tab_c4").removeClass("active");
            $("#tab_c3").removeClass("hidden");
            $("#tab_c4").addClass("hidden");
            $("#tab_c3").addClass("active");
            $("#m_save_but").attr("disabled", false);
        },

        pager: jQuery('#listPager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: 250,
        caption: ' Process Unit',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        //width: $(".box-body").width() - 5,
        autowidth: true,
        multiselect: false,
    }).navGrid('#listPager',
        {
            edit: false, add: false, del: false, search: false,
            searchtext: "Search Student", refresh: true
        },
        {
            zIndex: 100,
            caption: "Search Students",
            sopt: ['cn']
        });
    function rgb_color_col(cellValue, options, rowdata, action) {
        var html = cellValue + '&nbsp' + '<button class="btn btn-xs" style="float: right; border: 1px solid silver;background-color:' + cellValue + ';color:' + cellValue + '">color</button>';
        return html;
    }
    $("#c_save_but").click(function () {

        if ($("#c_prounit_nm").val().trim() == "") {
            alert("Please enter your process unit name");
            $("#c_prounit_nm").val("");
            $("#c_prounit_nm").focus();
            return false;
        }
        if ($("#c_color").val().trim() == "") {
            alert("Please enter your process unit Color");
            $("#c_color").val("");
            $("#c_color").focus();
            return false;
        }
        if ($("#c_mc_type").val().trim() == "") {
            alert("Please enter your Process Type");
            $("#c_mc_type").val("");
            $("#c_mc_type").focus();
            return false;
        }
        else {

            $.ajax({
                url: "/DevManagement/Createprocess_unit",
                type: "get",
                dataType: "json",
                data: {
                    mc_type: $('#c_mc_type').val(),
                    prounit_nm: $('#c_prounit_nm').val(),
                    color: $('#c_color').val(),
                    use_yn: $('#c_use_yn').val(),
                },
                success: function (data) {
                    jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form1").reset();

                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });
    $("#m_save_but").click(function () {
        if ($("#m_prounit_nm").val().trim() == "") {
            alert("Please enter your process unit name");
            $("#m_prounit_nm").val("");
            $("#m_prounit_nm").focus();
            return false;
        }
        if ($("#m_color").val().trim() == "") {
            alert("Please enter your process unit Color");
            $("#m_color").val("");
            $("#m_color").focus();
            return false;
        }
        if ($("#m_mc_type").val().trim() == "") {
            alert("Please enter your Process Type");
            $("#m_mc_type").val("");
            $("#m_mc_type").focus();
            return false;
        }
        else {
            $.ajax({
                type: "get",
                dataType: "json",
                url: "/DevManagement/Modifyprocess_unit",
                data: {
                    mc_type: $('#m_mc_type').val(),
                    prounit_nm: $('#m_prounit_nm').val(),
                    color: $('#m_color').val(),
                    use_yn: $('#m_use_yn').val(),
                    puid: $('#puid').val(),
                },
                success: function (data) {
                    jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form2").reset();

                }
            });
        }
    });
   
});//grid1
$(function () {
    $("#list2").jqGrid
    ({
        url: "/DevManagement/Getprocess_machineunit",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pmid', name: 'pmid', align: 'center', hidden: true },
            { key: false, label: 'Process Unit Code', name: 'prounit_cd', width: 150, align: 'center', },
            { key: false, label: 'Process Unit  Name', name: 'prounit_nm', sortable: true, width: 150, },
            { key: false, label: 'Machine Or Bobin Code', name: 'mc_no', width: 250 },
            { key: false, label: 'Machine Name', name: 'mc_nm',  width: 150 },
            { key: false, label: 'Use', name: 'use_yn', editable: true, width: '100px', align: 'center', width: 50, },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
            row_id = $("#list2").getRowData(selectedRowId);

            $("#pmid").val(row_id.pmid);
            $("#m2_prounit_cd").val(row_id.prounit_cd);
            $("#m_prounit_nm").val(row_id.prounit_nm);
            $("#m_mc_no").val(row_id.mc_no);
            $("#m_color").val(row_id.color);
            $("#m2_use_yn").val(row_id.use_yn);

            $("#tab_3").removeClass("active");
            $("#tab_4").addClass("active");
            $("#tab_c3").removeClass("active");
            $("#tab_c4").removeClass("hidden");
            $("#tab_c3").addClass("hidden");
            $("#tab_c4").addClass("active");
            $("#m2_save_but").attr("disabled", false);
        },

        pager: jQuery('#list2Pager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: 250,
        caption: ' Process Machine Unit',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        //width: $(".box-body").width() - 5,
        autowidth: true,
        multiselect: false,
    }).navGrid('#list2Pager',
        {
            edit: false, add: false, del: false, search: false,
            searchtext: "Search Student", refresh: true
        },
        {
            zIndex: 100,
            caption: "Search Students",
            sopt: ['cn']
        });

    $("#c2_save_but").click(function () {
        debugger;
        if ($("#c_mc_no").val().trim() == "") {
            alert("Please enter your process machine");
            $("#c_mc_no").val("");
            $("#c_mc_no").focus();
            return false;
        }
        else {

            $.ajax({
                url: "/DevManagement/Createprocessmachine_unit",
                type: "get",
                dataType: "json",
                data: {
                    mc_no: $('#c_mc_no').val(),
                    puid: $('#puid').val(),
                    use_yn: $('#c2_use_yn').val(),
                },
                success: function (data) {
                    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    document.getElementById("form3").reset();

                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });
    $("#m2_save_but").click(function () {

        $.ajax({
            type: "get",
            dataType: "json",
            url: "/DevManagement/Modifyprocessmachine_unit",
            data: {
                mc_no: $('#m_mc_no').val(),
                use_yn: $('#m2_use_yn').val(),
                pmid: $('#pmid').val(),
            },
            success: function (data) {
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                document.getElementById("form4").reset();
            }
        });

    });

});//grid2
$(document).ready(function () {
    $("#s_prounit_cd").autocomplete({
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
    $("#s_mc_no").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/DevManagement/Automcinunit",
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


$("#searchBtn").click(function () {
    var code = $("#s_prounit_cd").val().trim();
    var name = $("#s_prounit_nm").val().trim();
    var machine = $("#s_mc_no").val();

    $.ajax({
        url: "/DevManagement/searchall_unit",
        type: "get",
        dataType: "json",
        data: {
            mc_type: $('#s_mc_type').val(),
            code: code,
            name: name,
            machine: machine,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        },
        error: function (data) {
            alert('Not Found');
        }

    });
    $.ajax({
        url: "/DevManagement/searchall_unit1",
        type: "get",
        dataType: "json",
        data: {
            mc_type: $('#s_mc_type').val(),
            code: code,
            name: name,
            machine: machine,
        },
        success: function (result) {
            $("#list2").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        },
        error: function (data) {
            alert('Not Found');
        }

    });
});

$(document).ready(function () {
    _getmc_type();
});
function _getmc_type() {
    $.get("/DevManagement/get_mc_type", function (data) {
        if (data != null && data != undefined && data.length) {
            console.log(data);
            var html = '';
            html += '<option value="" selected="selected">*Process No*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });

            $("#c_mc_type").html(html);
            $("#s_mc_type").html(html);
            $("#m_mc_type").html(html);

        }
    });
}