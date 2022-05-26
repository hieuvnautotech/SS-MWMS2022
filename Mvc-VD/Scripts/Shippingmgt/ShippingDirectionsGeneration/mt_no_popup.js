$(function () {
    $(".dialog_MT").dialog({
        width: '50%',
        height: 500,
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

            $("#popups_MT").jqGrid
            ({
                url: "/PurchaseMgt/getPopupMT",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                    { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                    { label: 'Code', name: 'mt_no', key: true, width: 250 },
                    { label: 'Name', name: 'mt_nm', sortable: true, width: 250 },
                    { label: 'Supplier', name: 'sp_cd', sortable: true, width: 100, align: 'center', },
                    { label: 'Manufacturer', name: 'mf_cd', sortable: true, width: 100, align: 'center', },
                    { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', hidden: true },
                    { label: 'width unit', name: 'width_unit', hidden: true, align: 'right' },
                    { label: 'Spec', name: 'spec', sortable: true, width: 100, align: 'right' },
                    { name: 'spec', sortable: true, width: 100, align: 'right', hidden: true },
                    { label: 'Spec Unit', name: 'spec_unit', width: 60, hidden: true, align: 'right' },
                    { label: 'Area', name: 'area', sortable: true, width: 100, align: 'right' },
                    { name: 'area', sortable: true, width: 100, align: 'right', hidden: true },
                    { label: 'Price', name: 'price', width: 90, align: 'right' },
                    { name: 'price', width: 60, align: 'right', hidden: true, },
                    { label: 'price Unit', name: 'price_unit', width: 60, hidden: true, },
                    { label: 'Area Unit', name: 'area_unit', width: 60, hidden: true, align: 'right' },
                    { label: 'Photo', name: 'photo_file', width: 100, formatter: downloadLink, align: 'center' },
                    { label: 'Photo', name: 'photo_file', hidden: true },
                    { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Description', name: 're_mark', width: 130, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                    { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
                    { label: 'Change Name', name: 'chg_id', width: 90, align: 'left', },
                    { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popups_MT").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups_MT").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#selectMT').click(function () {
                            $('#mt_no').val(row_id.mt_no);
                            $('#mtid').val(row_id.mtid);
                            $('.dialog_MT').dialog('close');
                        });
                    }
                },
                pager: jQuery('#popupspager_MT'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000, 5000, 10000],
                rowNum: 50,
                height: 250,
                //width: $(".ui-dialog").width() - 50,
                autowidth: true,
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
    function downloadLink(cellValue, options, rowdata, action) {
        if (cellValue != null) {
            var html = '<a href="#" class="popupimg" data-img="' + cellValue + '"><img src="./../Images/MarterialImg/' + cellValue + '" style="height:20px;" /></a>';
            return html;

        } else {
            return "";
        }
    }
    $(".poupdialog_MT").click(function () {
        $('.dialog_MT').dialog('open');
    });

    $('#closeMT').click(function () {
        $('.dialog_MT').dialog('close');
    });
    $("#searchBtn_material_popup").click(function () {
        var mt_no = $('#material_cd_popup').val().trim();
        var mt_nm = $('#material_nm_popup').val().trim();

        $.ajax({
            url: "/PurchaseMgt/search_material_popup",
            type: "get",
            dataType: "json",
            data: {
                mt_no: mt_no,
                mt_nm: mt_nm,
            },
            success: function (result) {
                $("#popups_MT").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

});

