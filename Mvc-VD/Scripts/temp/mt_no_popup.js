$(function () {
    $(".dialogMT").dialog({
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

            $("#popupsMT").jqGrid
            ({
                url: "/ShippingMgt/getMT",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { label: 'mtid', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                   { label: 'Material Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                   { label: 'Material No', name: 'mt_no', sortable: true, align: 'left' },
                   { label: 'Material Name', name: 'mt_nm', sortable: true, width: 100, align: 'center' },
                   { label: 'mf_cd', name: 'mf_cd', sortable: true, width: 100, align: 'center' },
                   { label: 'sp_cd', name: 'sp_cd', width: 100, align: 'center' },
                   { label: 'Width', name: 'width', width: 100, hidden: true },
                   { label: 'width_unit', name: 'width_unit', width: 100, align: 'left' },
                   { label: 'spec', name: 'spec', width: 100, align: 'right' },
                   { label: 'spec_unit', name: 'spec_unit', width: 100, align: 'left' },
                   { label: 'price', name: 'price', width: 100, align: 'right' },
                   { label: 'price_unit', name: 'price_unit', width: 100, align: 'left' },
                   { label: 'photo_file', name: 'photo_file', width: 100, align: 'center', formatter: image_logo, },
                   { label: 're_mark', name: 're_mark', width: 100, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsMT").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsMT").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#mt_no').val(row_id.mt_no);
                        $('.dialogMT').dialog('close');
                       
                    }
                },

                pager: jQuery('#popupspagerMT'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                //width: $(".ui-dialog").width() - 5,
                autowidth: false,
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
    $(".poupdialogMT").click(function () {
        $('.dialogMT').dialog('open');
    });
    $('#closeMT').click(function () {
        $('.dialogMT').dialog('close');
    });
    function image_logo(cellValue, options, rowdata, action) {
        if (cellValue != null) {
            var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
            return html;

        } else {
            return "";
        }
    }
});