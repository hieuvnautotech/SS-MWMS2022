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
                url: "/TIMS/getPopupMT",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                { label: 'Code', name: 'mt_no', key: true, width: 250 },
                { label: 'Name', name: 'mt_nm', sortable: true, width: 250 },
                { label: 'Group Qty', name: 'gr_qty', sortable: true, width: 100, align: 'right', },
                { label: 'Unit', name: 'unit_cd', sortable: true, width: 60, align: 'left', },
                { label: 'Bundle Unit', name: 'bundle_unit', sortable: true, width: 60, align: 'left' },
                { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', formatter: widthformat },
                { label: 'Width', name: 'width', sortable: true, width: 80, align: 'right', hidden: true },
                { label: 'width unit', name: 'width_unit', hidden: true, align: 'right' },
                { label: 'Spec', name: 'spec', sortable: true, width: 100, align: 'right', formatter: specformat },
                { name: 'spec', sortable: true, width: 100, align: 'right', hidden: true },
                { label: 'Spec Unit', name: 'spec_unit', width: 60, hidden: true, align: 'right' },
                { label: 'Area', name: 'area', sortable: true, width: 100, align: 'right', formatter: areaformat },
                { name: 'area', sortable: true, width: 100, align: 'right', hidden: true },
                { label: 'Price', name: 'price', width: 90, align: 'right', formatter: priceformat },
                { name: 'price', width: 60, align: 'right', hidden: true, },
                { label: 'Supplier', name: 'sp_cd', sortable: true, width: 100, align: 'left', },
                { label: 'Manufacturer', name: 'mf_cd', sortable: true, width: 100, align: 'left', },
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
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popups_MT").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups_MT").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#selectMT').click(function () {
                            $('#c_mt_no').val(row_id.mt_no);
                         
                            $('.dialog_MT').dialog('close');
                        });
                    }
                },
                pager: jQuery('#popupspager_MT'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: $(".boxMaterial").width(),
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
    function downloadLink(cellValue, options, rowdata, action) {
        if (cellValue != null) {
            var html = '<a href="#" class="popupimg" data-img="' + cellValue + '"><img src="./../Images/MarterialImg/' + cellValue + '" style="height:20px;" /></a>';
            return html;

        } else {
            return "";
        }
    }
    function formatNumber(nStr, decSeperate, groupSeperate) {
        nStr += '';
        x = nStr.split(decSeperate);
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + groupSeperate + '$2');
        }
        return x1 + x2;
    }


    function widthformat(cellValue, options, rowdata, action) {
        if (cellValue != null) {

            var html = formatNumber(cellValue, '.', ',') + " " + (rowdata.width_unit);
            return html;

        } else {
            return "";
        }
    }

    function specformat(cellValue, options, rowdata, action) {
        if (cellValue != null) {

            var html = formatNumber(cellValue, '.', ',') + " " + rowdata.spec_unit;
            return html;

        } else {
            return "";
        }
    }

    function areaformat(cellValue, options, rowdata, action) {
        if (cellValue != null) {

            var html = formatNumber(cellValue, '.', ',') + " " + rowdata.area_unit;
            return html;

        } else {
            return "";
        }
    }

    function priceformat(cellValue, options, rowdata, action) {
        if (cellValue != null) {

            var html = formatNumber(cellValue, '.', ',') + " " + rowdata.price_unit;
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
        var mt_type = $('#material_type_popup').val().trim();
        var mt_no = $('#material_cd_popup').val().trim();
        var mt_nm = $('#material_nm_popup').val().trim();

        $.ajax({
            url: "/TIMS/getPopupMT",
            type: "get",
            dataType: "json",
            data: {
                mt_type: mt_type,
                mt_no: mt_no,
                mt_nm: mt_nm,
            },
            success: function (result) {
                $("#popups_MT").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
         
            }
        });

    });
   
});

