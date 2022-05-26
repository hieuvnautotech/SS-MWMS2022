$(function () {
    $(".dialog5").dialog({
        width: '50%',
        height: 450,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
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

            $("#popupproes_unit").jqGrid
            ({
                url: '/DevManagement/Getprocess_unint',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'puid', name: 'puid', align: 'center', hidden: true },
                    { key: false, label: 'Process Unit Code', name: 'prounit_cd', width: 200, align: 'center', },
                    { key: false, label: 'Process Unit  Name', name: 'prounit_nm', sortable: true, width: '200', },
                    { key: false, label: 'Color', name: 'color', editable: true, width: '100px', formatter: rgb_color_col, width: 200, },
                    { key: false, label: 'Color', name: 'color', editable: true, width: '100px', hidden: true },
                    { key: false, label: 'Use', name: 'use_yn', editable: true, width: '100px', align: 'center', width: 200, },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupproes_unit").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupproes_unit").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_prounit_cd').val(row_id.prounit_cd);
                        $('#m_prounit_cd').val(row_id.prounit_cd);
                        $('.dialog5').dialog('close');
                    }
                },

                pager: jQuery('#proes_unitpager'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'Process Unit',
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

    $(".poupdialoginunit").click(function () {
        $('.dialog5').dialog('open');
    });
    
    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popupproes_unit').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popupproes_unit").closest(".ui-jqgrid").parent().width();
    //    $("#popupproes_unit").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closeproes_unit').click(function () {
        $('.dialog5').dialog('close');
    });

});
function rgb_color_col(cellValue, options, rowdata, action) {
    var html = cellValue + '&nbsp' + '<button class="btn btn-xs" style="border: 1px solid silver;background-color:' + cellValue + ';color:' + cellValue + '">color</button>';
    return html;
}