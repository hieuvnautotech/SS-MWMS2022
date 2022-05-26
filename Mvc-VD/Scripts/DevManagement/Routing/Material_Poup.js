$(".dialog_MATERIAL2").dialog({
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
                datatype: function (postData) { getDataOutBox1(postData); },
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

                        //$("#savestyle_material").click(function () {
                        //    var $grid = $("#popupMaterial"), selIds = $grid.jqGrid("getGridParam", "selarrrow"), i, n,
                        //    cellValues = [];
                        //    cellValues_mt_no_view = [];
                        //    ListMaterial = [];
                        //    for (i = 0, n = selIds.length; i < n; i++) {
                        //        cellValues.push($grid.jqGrid("getCell", selIds[i], "mtid"));
                        //        cellValues_mt_no_view.push($grid.jqGrid("getCell", selIds[i], "mt_no"));
                        //        ListMaterial.push($grid.jqGrid("getCell", selIds[i], "mtid"));
                        //    }
                        //    var stt = $("#stt_mt").val();
                        //    $('#c_mt_no').val(cellValues.join(","));
                        //    $('#c_mt_no_view').val(cellValues_mt_no_view.join(","));
                        //    $('#m_mt_no').val(row_id.mt_no);
                        //    $('#add_mt_tt').val(row_id.mt_no);
                        //    $('.dialog_MATERIAL2').dialog('close');
                        //});
                        $("#savestyle_material").click(function () {
                       
                           
                            var stt = $("#stt_mt").val();
                         
                            $('#m_mt_no').val(row_id.mt_no);
                            $('#c_mt_no').val(row_id.mt_no);
                            $('#add_mt_tt').val(row_id.mt_no);
                            $('.dialog_MATERIAL2').dialog('close');
                        });

                    }
                },

                pager: jQuery('#pagerMaterial'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: false,
                height: 250,
                width: null,
                autowidth: true,
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
            $("#popupMaterial").clearGridData();
            $('.loading').show();
            var grid = $("#popupMaterial");
            grid.jqGrid('setGridParam', { search: true });
            var pdata = grid.jqGrid('getGridParam', 'postData');
            getDataOutBox1(pdata);
        });

    },
});
function getDataOutBox1(pdata) {
    $('.loading').show();
    var type = $("#s_mt_type_popup").val().trim();
    var code = $("#mt_no_popup").val().trim();
    var name = $("#mt_nm_popup").val().trim();
    var start1 = ""
    var end1 = ""
    var params = new Object();
    if (jQuery('#popupMaterial').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.type = type;
    params.code = code;
    params.name = name;
    params.start = start1;
    params.end = end1;
    //params.end1Data = type;
    $("#popupMaterial").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/DevManagement/searchmaterial_2',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#popupMaterial")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

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
    //debugger
    $('.dialog_MATERIAL2').dialog('open');
});

$('#closestyle_material').click(function () {
    $('.dialog_MATERIAL2').dialog('close');
});



