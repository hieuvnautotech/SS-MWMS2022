$(function () {
    var row_id;
    $(".dialog").dialog({
        width: '100%',
        height: 800,
        maxWidth: 1000,
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
            //"ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popups").jqGrid
            ({
                url: "/Lot/getMaterial2",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                    { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                    { label: 'Code', name: 'mt_no', key: true, width: 200, align: 'left' },
                    { label: 'Name', name: 'mt_nm', sortable: true, width: 200, },
                    { label: 'Supplier Information', name: 'sp_cd', sortable: true, width: 150, align: 'right', },
                    { label: 'Manufacturer', name: 'mf_cd', sortable: true, width: 150, align: 'right', },
                    { label: 'Width', name: 'new_with', sortable: true, width: 80, align: 'right' },
                    { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', hidden: true },
                    { label: 'width unit', name: 'width_unit', hidden: true, align: 'right' },
                    { label: 'Spec', name: 'new_spec', sortable: true, width: 100, align: 'right' },
                    { name: 'spec', sortable: true, width: 100, align: 'right', hidden: true },
                    { label: 'Spec Unit', name: 'spec_unit', width: 60, hidden: true, align: 'right' },
                    { label: 'Area', name: 'area_all', width: 150, align: 'right' },
                    { label: 'Area', name: 'area', width: 60, hidden: true, align: 'right' },
                    { label: 'area_unit', name: 'area_unit', width: 60, hidden: true, align: 'right' },
                    { label: 'Photo', name: 'photo_file', hidden: true },
                    { label: 'Price', name: 'new_price', width: 90, align: 'right' },
                    { name: 'price', width: 60, align: 'right', hidden: true },
                    { label: 'price Unit', name: 'price_unit', width: 60, hidden: true, },
                    { label: 'Description', name: 're_mark', width: 130, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                    { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
                    { label: 'Change Name', name: 'chg_id', width: 90, align: 'left', },
                    { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popups").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups").getRowData(selectedRowId);
                },

                pager: jQuery('#popupspager'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                rowNum: 50,
                autowidth: false,
                caption: 'Material Information',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                multiselect: false,
                loadonce: true,
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

    $(".poupdialog").click(function () {
        $('.dialog').dialog('open');
    });

    function downloadLink(cellValue, options, rowdata, action) {
        var mtid = rowdata.mtid;
        if (cellValue != null) {
            var html = '<img src="../images/' + cellValue + '" style="height:20px;"  onclick="Images(' + mtid + ');"/>';
            return html;

        } else {
            return "";
        }
    }

    $('#savestyle').click(function () {
        if (row_id != null) {
            $('#c_mt_cd').val(row_id.mt_no);
            $('#s_mt_no').val(row_id.mt_no);
           
            $('.dialog').dialog('close');
        }
    });

    $("#searchBtn_popup2").click(function () {
        var mt_no = $('#mt_no').val().trim();
        var mt_nm = $('#mt_nm').val().trim();
        $.ajax({
            url: "/Lot/searchmtPopup",
            type: "get",
            dataType: "json",
            data: {
                mt_no: mt_no,
                mt_nm: mt_nm,
            },
            success: function (result) {
                $("#popups").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

    $('#closemate').click(function () {
        $('.dialog').dialog('close');
    });

});