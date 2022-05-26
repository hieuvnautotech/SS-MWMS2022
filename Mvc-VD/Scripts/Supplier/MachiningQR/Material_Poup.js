$(".dialog_MATERIAL").dialog({
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

        $("#popupMaterial").jqGrid
        ({
            url: "/Supplier/Getpopupmaterial?mt_cd=" + $("#mt_cd").val() + "&mt_no=" + $("#c_mt_no").val(),
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                { label: 'Code', name: 'mt_no', key: true, width: 250 },
                { label: 'Name', name: 'mt_nm', sortable: true, width: 250 },
                { label: 'Supplier', name: 'sp_cd', sortable: true, width: 100, align: 'center', },
                { label: 'Manufacturer', name: 'mf_cd', sortable: true, width: 100, align: 'center', },
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
                integer: { thousandsSeparator: " ", defaultValue: '0' },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                currency: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, prefix: "", suffix: " VND", defaultValue: '0.00' },
            },
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                $("#savestyle_material").removeClass("disabled");
                var selectedRowId = $("#popupMaterial").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupMaterial").getRowData(selectedRowId);
                if (row_id != null) {
                   
                    $("#savestyle_material").click(function () {
                        var stt = $("#stt_mt").val();
                        $('#c_mt_no').val(row_id.mt_no);
                        $('#m_mt_no').val(row_id.mt_no);
                        $('#add_mt_tt').val(row_id.mt_no);
                        $('.dialog_MATERIAL').dialog('close');
                });

                }
            },

            pager: jQuery('#pagerMaterial'),
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

        $('#searchBtn_material_popup').click(function () {
            var code = $("#mt_no_popup").val().trim();
            var name = $("#mt_nm_popup").val().trim();
            var mt_no = $("#c_mt_no").val().trim();
            var mt_cd=("#mt_cd").val()
            $.ajax({
                url: "/Supplier/searchmaterial",
                type: "get",
                dataType: "json",
                data: {
                    codeData: code,
                    nameData: name,
                    mt_cd: mt_cd,
                    mt_no:mt_no
                },
                success: function (result) {
                    $("#popupMaterial").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
                }
            });
        });

    },
});

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
function downloadLink(cellValue, options, rowdata, action) {
    if (cellValue != null) {
        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
        return html;

    } else {
        return "";
    }
}

GetType_Marterial();
function GetType_Marterial() {
    $.get("/DevManagement/GetType_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".gettype").empty();
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $(".gettype").html(html);
        }
    });
};

$(".poupdialogMaterial").click(function () {
    $('.dialog_MATERIAL').dialog('open');
});

$('#closestyle_material').click(function () {
    $('.dialog_MATERIAL').dialog('close');
});



