$(function () {
    $(".sm").dialog({
        width: '50%',
        height: 800,
        maxWidth: '100%',
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 1000,
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

            $("#popupsm").jqGrid
            ({
                url: "/Shippingmgt/Getpopupsm",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { label: 'smno', name: 'smno', key: true, width: 50, align: 'center', hidden: true },
                   { label: 'SM', name: 'sm_no', key: true, width: 200, align: 'left' },
                   { label: 'Delivery Date', name: 'delivery_dt', sortable: true, width: 200, align: 'left' },
                   { label: 'Po No', name: 'delivery_real_dt', sortable: true, width: 100, align: 'right' },
                   { label: 'Destination', name: 'dest_cd', sortable: true, width: 100, align: 'right' },
                   { label: 'Remark', name: 're_mark', width: 100, align: 'center' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsm").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsm").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#selected_sm").click(function () {
                            $('#sm').val(row_id.sm_no);
                            $('.sm').dialog('close');
                        });

                    }
                },

                pager: jQuery('#pagesm'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                rowNum:50,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'SM',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                multiselect: false,
                loadonce: true,
                autowidth:true,
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
    $(".poupdialogsm").click(function () {
        $('.sm').dialog('open');
    });

    $("#searchBtn_pp_sm").click(function () {
        var code = $("#pp_sm").val().trim();
        $.ajax({
            url: "/Shippingmgt/searchpp_sm",
            type: "get",
            dataType: "json",
            data: {
                code: code,
            },
            success: function (result) {
                $("#popupso").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });

    $('#closesm').click(function () {
        $('.sm').dialog('close');
    });

});