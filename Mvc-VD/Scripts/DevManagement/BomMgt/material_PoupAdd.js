$(".dialog_addMATERIAL").dialog({
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

        $("#popupMaterial1").jqGrid
            ({
                datatype: function (postData) { getDataOutBox(postData); },
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
                    $("#savestyle_material1").removeClass("disabled");
                    var selectedRowId = $("#popupMaterial1").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupMaterial1").getRowData(selectedRowId);
                    if (row_id != null) {

                

                        
                    }
                },

                pager: jQuery('#pagerMaterial1'),
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
                multiselect: true,
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

        $('#searchBtn_material_popup1').click(function () {
            $("#popupMaterial1").clearGridData();
            $('.loading').show();
            var grid = $("#popupMaterial1");
            grid.jqGrid('setGridParam', { search: true });
            var pdata = grid.jqGrid('getGridParam', 'postData');
            getDataOutBox(pdata);
        });

    },
});
$("#savestyle_material1").click(function () {
    debugger;
    var r = confirm("Bạn có chắc muốn thêm những liệu này vào Product không?");
    if (r == true) {
       
        var $grid = $("#popupMaterial1"), selIds = $grid.jqGrid("getGridParam", "selarrrow"), i, n,
            
        ListMaterial = [];
        for (i = 0, n = selIds.length; i < n; i++) {
            ListMaterial.push($grid.jqGrid("getCell", selIds[i], "mt_no"));
        }

        var settings = {
            "url": "/DevManagement/ADdBomManagement",
            "method": "POST",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify({
                "ListMaterial": ListMaterial,
                "ProductCode": $("#ProductCode").val(),
                "MaterialPrarent": $("#c_mt_no").val(),
               
            }),
        };
        $.ajax(settings).done(function (data) {
            if (data.result) {
                SuccessAlert(data.message);
            
                $("#" + childGridID2).setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                //$("#" + childGridID2).jqGrid('addRowData', data.data[0].MaterialBOMID, data.data[0], 'first');
                //$("#" + childGridID2).setRowData(data.data[0].MaterialBOMID, false, { background: "#d0e9c6" });
               
            }
            else {
                ErrorAlert(data.message);
            }
        });
        $('.dialog_addMATERIAL').dialog('close');
    }
    else {
        return;

    }

});

function getDataOutBox(pdata) {
    $('.loading').show();
    var type = $("#s_mt_type_popup1").val().trim();
    var code = $("#mt_no_popup1").val().trim();
    var name = $("#mt_nm_popup1").val().trim();
    var start1 = ""
    var end1 = ""
    var params = new Object();
    if (jQuery('#popupMaterial1').jqGrid('getGridParam', 'reccount') == 0) {
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
    $("#popupMaterial1").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/DevManagement/searchmaterial_2',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            console.log('material_poupadd.js');
            if (st == "success") {
                var grid = $("#popupMaterial1")[0];
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






