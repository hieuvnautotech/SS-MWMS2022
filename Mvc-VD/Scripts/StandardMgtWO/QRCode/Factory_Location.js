$grid = $("#list").jqGrid({
    url: '/StandardMgtWO/GetWHfactory_wo',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'ID', name: 'lctno', width: 60, hidden: true },
        { label: 'Factory Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 250, align: 'center' },
        { label: 'Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 250, },
        { label: 'Level', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
        { label: 'WIP', name: 'wp_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Factory', name: 'ft_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Movement', name: 'mv_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Nothing', name: 'nt_yn', sortable: true, width: 60, align: 'center' },
        { label: 'use Y/N', name: 'use_yn', sortable: true, width: 60, align: 'center' },
        { label: 'Remark', name: 're_mark', width: 100, sortable: true, },
        { label: 'Index', name: 'index_cd', width: 100, sortable: true, },
        { label: 'QR Code', name: 'lct_bar_cd', width: 100, sortable: true, },
        { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
        { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#jqGridPager1',
    rownumbers: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    viewrecords: true,
    height: 360,
    width: $(".box-body").width(),
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
    autowidth: false,
    onSelectRow: function (rowid, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        $('#m_lct_nm').val(row_id.lct_nm);
        $("#m_use_yn").val(row_id.use_yn);
        $('#m_re_mark').val(row_id.re_mark);
        $('#m_lctno').val(row_id.lctno);
        $('#c_lctno').val(row_id.lctno);
        $("#del_save_but").removeClass("disabled", false);
        document.getElementById("form2").reset();
        if (row_id.wp_yn == 'Y') {
            $('#m_wp_yn').prop('checked', true);

        }
        if (row_id.wp_yn == 'N' || row_id.wp_yn == '') {
            $('#m_wp_yn').prop('checked', false);
        }
        if (row_id.ft_yn == 'Y') {
            $('#m_ft_yn').prop('checked', true);

        }
        if (row_id.ft_yn == 'N' || row_id.ft_yn == '') {
            $('#m_ft_yn').prop('checked', false);
        }

        if (row_id.mv_yn == 'Y') {
            $('#m_mv_yn').prop('checked', true);

        }
        if (row_id.mv_yn == 'N' || row_id.mv_yn == '') {
            $('#m_mv_yn').prop('checked', false);
        }
        if (row_id.nt_yn == 'N' || row_id.nt_yn == '') {
            $('#m_nt_yn').prop('checked', false);
        }
        if (row_id.level_cd == '005') {
            //$('#m_mv_yn').prop('checked', true);
            $('#c_lct_nm').prop('readonly', true);
            $('#c_re_mark').prop('readonly', true);
            $('#c_use_yn').prop('disabled', true);
            $("#c_save_but").attr("disabled", true);
        } else {
            //$('#m_mv_yn').prop('checked', false);
            $('#c_lct_nm').prop('readonly', false);
            $('#c_re_mark').prop('readonly', false);
            $('#c_use_yn').prop('disabled', false);
            $("#c_save_but").attr("disabled", false);
        }
        if (row_id.level_cd == '001') {
            $.get("/StandardMgtWO/get_lct_001?lct_cd=" + row_id.lct_cd, function (data) {
                if (data != null && data != undefined && data.length) {
                    var html = '<div class="col-md-6"><input type="text" name="lct_nm" id="c_lct_nm" class="form-control form-control-text" placeholder="Location Name"></div><div class="col-md-6">';
                    html += ' <select  required error name="mn_x" id="mn_x" class="form-control ">';
                    html += '<option value="">Select Range</option>';
                    $.each(data, function (key, item) {
                        html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
                    });
                    html += " </select></div>"
                    $("#all_mn").html(html);
                }
            });
        } else {
            var html = '<input type="text" name="lct_nm" id="c_lct_nm" class="form-control form-control-text" placeholder="Location Name" >';
            $("#all_mn").html(html);

        }

                  
    },
});
            

$("#c_save_but").click(function () {
    if ($('#c_root_yn').prop('checked') != true) {
        if ($('#c_lctno').val() == "") {
            alert("Please choice a factory in gird");
            return false;
        }
    }
        var wp_yn = ($('#c_wp_yn').prop('checked') == true) ? "Y" : "N";
        var ft_yn = ($('#c_ft_yn').prop('checked') == true) ? "Y" : "N";
        var mv_yn = ($('#c_mv_yn').prop('checked') == true) ? "Y" : "N";
    var nt_yn = ($('#c_nt_yn').prop('checked') == true) ? "Y" : "N";
    var mnx = $('#mn_x').val();
    if (mnx == "") {
        alert("Please press Range");
        return false;
    }
        $.ajax({
            url: "/StandardMgtWO/insertfactory_lct",
            type: "Post",
            dataType: "json",
            data: {
                root_yn: ($('#c_root_yn').prop('checked') == true) ? "1" : "",
                lct_nm: $('#c_lct_nm').val(),
                mv_yn: ($('#c_mv_yn').prop('checked') == true) ? "Y" : "N",
                use_yn: $('#c_use_yn').val(),
                re_mark: $('#c_re_mark').val(),
                mn_x: mnx,
                lctno: ($('#c_root_yn').prop('checked') == true) ? 0 : $('#c_lctno').val(), 
                mv_yn: mv_yn,
                wp_yn: wp_yn,
                ft_yn: ft_yn,
                nt_yn: nt_yn,
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.value.lctno;
                    $("#list").jqGrid('addRowData', id, data.value, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
               

            }, error: function (data) {
                    alert('Lỗi hệ thống !');
            }

        });        
    
});

$("#m_save_but").click(function () {
    debugger;
    //if ($("#form1").valid() == true) {
        var wp_yn = ($('#m_wp_yn').prop('checked') == true) ? "Y" : "N";
        var ft_yn = ($('#m_ft_yn').prop('checked') == true) ? "Y" : "N";
        var mv_yn = ($('#m_mv_yn').prop('checked') == true) ? "Y" : "N";
        var nt_yn = ($('#m_nt_yn').prop('checked') == true) ? "Y" : "N";
        $.ajax({
            url: "/StandardMgtWO/updatefactory_lct",
            type: "Put",
            dataType: "json",
            data: {
                lct_nm: $('#m_lct_nm').val(),
                use_yn: $('#m_use_yn').val(),            
                re_mark: $('#m_re_mark').val(),
                lctno: $('#m_lctno').val(),
                wp_yn: wp_yn,
                ft_yn: ft_yn,
                mv_yn: mv_yn,
                nt_yn: nt_yn,
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.value.lctno;
                    $("#list").setRowData(id, data.value, { background: "#d0e9c6" });
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            },
            error: function (data) {
                alert("Lỗi hệ thống");
            }

        });
   // }
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});

$("#searchBtn").click(function () {
    $.ajax({
        url: "/StandardMgtWO/SearchFactory_Location",
        type: "Get",
        dataType: "json",
        data: {
            lct_cd: $('#lct_cd').val().trim(),
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });

});

$(document).ready(function () {
    $("#lct_cd").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StandardMgtWO/Autolct_cd",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.lct_cd, value: item.lct_cd };
                    }))

                }
            })
        },
        messages: {
            noResults: '',
        }
    });
});

$("#c_reset_but").click(function () {
    document.getElementById("form2").reset();
});
   
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

$("#PrintBtn").click(function () {
    var fruits = [];
    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds == "") {
        alert("Please choice a factory in gird");
        return false;
    }
    for (i = 0, n = selRowIds.length; i < n; i++) {
        var ret = $("#list").jqGrid('getRowData', selRowIds[i]);
        var lctno = ret.lctno
        fruits.push(lctno);
    };
    window.open("/StandardMgtWO/PrintFactory?" + "id=" + fruits, "PRINT", "width=500, height=800, left=0, top=100, location=no, status=no,")
});

$('#excelBtn').click(function (e) {
    $("#list").jqGrid('exportToExcel',
            options = {
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "Factory Location(WO).xlsx",
                mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                maxlength: 40,
                onBeforeExport: null,
                replaceStr: null
            }
        );
});

$('#htmlBtn').click(function (e) {
    $("#list").jqGrid('exportToHtml',
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
            fileName: "Factory Location(WO)",
            returnAsString: false
        }
        );
});