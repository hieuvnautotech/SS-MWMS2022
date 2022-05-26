$(function () {
    $("#list").jqGrid
    ({
        url: "/ProducePlan/getwoMgtData",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'oflno', name: 'oflno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Fo no', name: 'fo_no', width: 100, align: 'center' },
            { key: false, label: 'Factory', name: 'lct_cd', sortable: true, width: '150', align: 'center', align: 'center' },
            { key: false, label: 'Daily Qty Per line', name: 'day_qty', sortable: true, width: '200', align: 'right' },
            { key: false, label: 'Line Qty', name: 'line_qty', editable: true, width: '100px', align: 'right' },
            {
                key: false, label: 'Start Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200'
            },
            { key: false, label: 'End Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
            { key: false, label: 'Total days', name: 'total_need_day', editable: true, width: '100px', align: 'right' }

        ],
        onSelectRow: function (rowid, cellValue, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $('#m_mid').val(row_id.mid);
            $('#m_line_cd').val(row_id.line_cd);
            $('#m_line_nm').val(row_id.line_nm);
            $('#m_line_exp').val(row_id.line_exp);
            $('#m_re_mark').val(row_id.re_mark);
            $('#m_use_yn').val(row_id.use_yn);
            $('#m_line_color').val(row_id.line_color);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");

            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
        },
        gridComplete: function () {
            var rows = $("#list").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var v_use_yn = $("#list").getCell(rows[i], "use_yn");
                if (v_use_yn == "N") {
                    $("#list").jqGrid('setRowData', rows[i], false, { background: '#D5D5D5' });
                }
            }
        },

        pager: jQuery('#gridpager'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        shrinkToFit: false,
        autowidth: true,
        viewrecords: true,
        height: '250px',
        width: $(".box-body").width() - 5,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        loadonce: true,
        multiselect: false,
    })
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#list').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
        $("#list").jqGrid("setGridWidth", newWidth, false);
    });
   

    $("#start").datepicker({
        format: 'yyyy-mm-dd',
    });

    $("#end").datepicker({
        format: 'yyyy-mm-dd',
    });
    $("#searchBtn").click(function () {

        var fo_no = $('#s_fo_no').val().trim()
        var lct_cd = $('#m_lct_cd').val();
        var start = $('#start').val()
        var end = $('#end').val()
        $.ajax({
            url: "/ProductPlan/searchwoMgt",
            type: "get",
            dataType: "json",
            data: {
                fo_no: fo_no,
                lct_cd: lct_cd,
                start: start,
                end: end
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
              
            }
        });

    });

    var getType = "/ProducePlan/getType";

    function _getTypeS() {

        $.get(getType, function (data) {
            if (data != null && data != undefined && data.length) {
                var html = '';
                html += '<option value="" selected="selected">*</option>';
                $.each(data, function (key, item) {
                    html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
                });
                $("#m_lct_cd").html(html);
            }
        });
    }

    $(document).ready(function () {
        _getTypeS();
        $("#m_lct_cd").on('change', function () {
            var id = $(this).val();
            if (id != undefined && id != '') {
            }
        });
    });

    $(document).ready(function (e) {
        $('#pdfBtn').click(function (e) {
            $('table').tableExport({
                type: 'pdf',
                bootstrap: true,
                pdfFontSize: '12',
                fileName: 'Style_Mgt',
                escape: false,
                headings: true,
                footers: true,
            });
        });
    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $('table').tableExport({
                type: 'xls',
                bootstrap: true,
                fileName: 'Style_Mgt',
                escape: false,
                headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
                footers: true,
            });
        });
    });
    $(document).ready(function (e) {
        $('#htmlBtn').click(function (e) {
            $('table').tableExport({
                type: 'doc',
                bootstrap: true,
                fileName: 'Style_Mgt',
                escape: false,
                headings: true,                    // (Boolean), display table headings (th/td elements) in the <thead>
                footers: true,
            });
        });
    });


});//grid1


