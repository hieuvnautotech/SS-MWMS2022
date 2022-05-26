$(function () {
    $("#list").jqGrid
    ({
        url: "/StandardMgtWO/getw_worktime_ymd",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'wtid', name: 'wtid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'line_cd', name: 'line_cd', width: 100, align: 'center' },
            { key: false, label: 'date_ym', name: 'date_ym', sortable: true, width: '150', align: 'center', align: 'left' },
            { key: false, label: 'date_ymd', name: 'date_ymd', sortable: true, width: '200', align: 'right' },
            { key: false, label: 'date_d', name: 'date_d', editable: true, width: '100px', align: 'right' },
            {
                key: false, label: 'week_n', name: 'week_n',
            },
            { key: false, label: 'week', name: 'week', },
            { key: false, label: 'work_time', name: 'work_time', editable: true, width: '100px', align: 'right' },
            { key: false, label: 'work_sts', name: 'work_sts', editable: true, width: '100px', align: 'right' },
            { key: false, label: 'prod_yn', name: 'prod_yn', editable: true, width: '100px', align: 'right' },

        ],
        onSelectRow: function (rowid, cellValue, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            //$('#m_oflno').val(row_id.oflno);
            //$('#m_fo_no').val(row_id.fo_no);

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

});

