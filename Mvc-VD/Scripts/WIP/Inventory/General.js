$("#tab_1").on("click", "a", function (event) {
    $("#tab_1").addClass("active");
    $("#tab_2").removeClass("active");
    $("#tab_3").removeClass("active");

    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").removeClass("active");
    $("#tab_c3").removeClass("active");

    $("#tab_c1").addClass("active");
    $("#tab_c2").addClass("hidden");
    $("#tab_c3").addClass("hidden");
});

var testData = [];
$("#tab_2").on("click", "a", function (event) {
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata)

    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_3").removeClass("active");

    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c3").removeClass("active");

    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#tab_c3").addClass("hidden");
});
$("#tab_3").on("click", "a", function (event) {


    $("#tab_1").removeClass("active");
    $("#tab_2").removeClass("active");
    $("#tab_3").addClass("active");

    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c3").removeClass("hidden");

    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c3").addClass("active");
    GridMemo();
});

$(document).ready(function () {
    $('#return_date').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });

    $('#Rece_start').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#Rece_end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#recevice_dt_start').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#recevice_dt_end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#create_dt_start').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#create_dt_end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
});
//#region tab1
$(function () {
    $("#list").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                { label: 'Product', name: 'product_cd', width: 100, align: 'left' },
                { label: 'MT NO', name: 'mt_no', width: 200, align: 'left' },
                { label: 'MT Name', name: 'mt_nm', width: 400, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right' },
                { name: 'DSD', hidden: true },
                { name: 'CSD', hidden: true },
                { label: 'Đang sử dụng Qty Length(M)', name: '', width: 150, align: 'right', formatter: dvi_dsd },
                { label: 'Chưa sử dụng Qty Length(M)', name: '', width: 150, align: 'right', formatter: dvi_csd },
                { label: 'Tồn Kho Qty Length(M)', name: 'total', width: 150, align: 'right', formatter: total },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            pager: jQuery("#listPager"),
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 500,
            width: $(".box-body").width() - 5,
            autowidth: false,
            rowNum: 50,
            caption: 'Material',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false,
            subGrid: true,
            shrinkToFit: false,
            multiselect: false,
            multiPageSelection: true,
            multiboxonly: true,
            datatype: function (postData) { getDataList(postData); },
            subGridRowExpanded: showChildGrid_List,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
        })
});
function total(cellValue, options, rowdata, action) {
    return (rowdata.DSD + rowdata.CSD) + "m";
}
function dvi_dsd(cellValue, options, rowdata, action) {
    return rowdata.DSD + "m";
}
function dvi_csd(cellValue, options, rowdata, action) {
    return rowdata.CSD + "m";
}
$("#searchBtn_list").click(function () {
    $("#list").clearGridData();
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getDataList(pdata)
});
function getDataList(pdata) {
    var params = new Object();
    var rows = $("#list").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.mt_no = $('#s_mt_no').val() == null ? "" : $('#s_mt_no').val().trim();
    params.mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();
    params.product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
    params.lct_cd = $('#s_locationCode').val() == null ? "" : $('#s_locationCode').val().trim();
    var mt_cd = $('#ss_mt_cd').val() == null ? "" : $('#ss_mt_cd').val().trim();

    params.mt_cd = $('#ss_mt_cd').val() == null ? "" : $('#ss_mt_cd').val().trim();
    params.recevice_dt_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
    params.recevice_dt_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
    params.sts = $('#status').val() == null ? "" : $('#status').val();
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/WIP/getGeneral_Material",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            //debugger;
            if (st == "success") {
                var showing = $("#list")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
var mt_no_list = "";

function showChildGrid_List(parentRowID, parentRowKey) {
    var flag = false;
    childGridID = parentRowID + "_table";
//debugger;
    var childGridPagerID = parentRowID + "_pager";

    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    var mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();
    var product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
    var lct_cd = $('#s_locationCode').val() == null ? "" : $('#s_locationCode').val().trim();
    var recevice_dt_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
    var recevice_dt_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
     var sts = $('#status').val() == null ? "" : $('#status').val();
    var mt_cd = $('#ss_mt_cd').val() == null ? "" : $('#ss_mt_cd').val().trim();
    var rowData = $('#list').jqGrid('getRowData', parentRowKey);
    mt_no_list = rowData.mt_no;

    $("#" + childGridID).jqGrid({
        mtype: "GET",
        datatype: `json`,
        url: `/WIP/GetgeneralDetail_List?mt_no=${encodeURIComponent(mt_no_list)}&mt_nm=${mt_nm}&product_cd=${product_cd}&recevice_dt_start=${recevice_dt_start}&recevice_dt_end=${recevice_dt_end}&sts=${sts}&lct_cd=${lct_cd}&mt_cd=${encodeURIComponent(mt_cd)}`,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            { label: 'SD NO', name: 'sd_no', width: 50, align: 'left' },
            { label: 'MT Code', name: 'material_code', width: 350, align: 'left' },
            {
                label: 'Material', name: 'mt_nm', width: 350, align: 'left', hidden: true
            },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right' },
            { label: '', name: 'bundle_unit', width: 100, align: 'right', hidden: true },
            { label: 'Lenght', name: 'lenght', width: 100, align: 'right' },
            { label: 'Size ', name: 'size', width: 100, align: 'right' },
            { label: 'Location', name: 'lct_nm', width: 130, align: 'left' },
            { label: 'Status', name: 'sts_nm', width: 100, align: 'right' },
            { label: 'Recevied Date', name: 'receipt_date', width: 110, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" }},
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        shrinkToFit: false,
        width: null,
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 50,
        loadonce: true,
        rownumbers: true,
        multiselect: true,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        footerrow: true,
        userDataOnFooter: true,
        viewrecords: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        gridComplete: function () {
            prevCellVal = { cellId: undefined, value: undefined };
            var rowIds = $("#" + childGridID).getDataIDs();
            //rowIds.forEach(GridScanProduct_SetRowColor);

            //sum Quantity
            var ids = $("#" + childGridID).jqGrid('getDataIDs');
            var sum_Quantity = 0;
            var sum_lieu = 0;
            var $self = $(this)
            //console.log(ids);
            for (var i = 0; i < ids.length; i++) {
                let item = {
                    "id": ids[i],


                };
                testData.push(item);
                sum_Quantity += parseFloat($("#" + childGridID).getCell(ids[i], "qty"));
                sum_lieu += parseFloat($("#" + childGridID).getCell(ids[i], "lenght"));
            }

            $self.jqGrid("footerData", "set", { sd_no: "Total" });
            $self.jqGrid("footerData", "set", { qty: sum_Quantity });
            $self.jqGrid("footerData", "set", { lenght: sum_lieu });


            jQuery("#listProduct").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
    });
}
function getDataDT_List(pdata) {
    var params = new Object();
    var rows = $("#" + childGridID).getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.mt_no = mt_no_list;
    params.mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();
    params.product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
    params.recevice_dt_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
    params.recevice_dt_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
    params.sts = $('#status').val() == null ? "" : $('#status').val();

    $("#" + childGridID).jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/WIP/GetgeneralDetail_List",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#" + childGridID)[0];
                showing.addJSONData(data);
                $('.loading-gif-2').hide();
            }
        },
        error: function () {
            $('.loading-gif-2').hide();
            return;
        }
    });
};
//#endregion

//#region tab2
$(function () {
    $grid = $("#GeneralGrid").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                { label: 'Model', name: 'model', width: 200, align: 'left' },
                { label: 'Product Code', name: 'product_cd', width: 200, align: 'left' },
                { label: 'Product Name', name: 'product_nm', width: 200, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
                { label: 'Bán Thành Phẩm(EA)', name: '', width: 150, align: 'right', formatter: dvi_btp },
                { name: 'BTP', width: 150, align: 'right', hidden: true },
                { label: 'Thành Phẩm(EA)', name: '', width: 150, align: 'right', formatter: dvi_tp },
                { name: 'TP', width: 150, align: 'right', hidden: true },
                { label: 'Tồn Kho', name: '', width: 150, align: 'right', formatter: total_cp },

            ],

            pager: "#GeneralGridPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 600,
            width: null,
            autowidth: false,
            rowNum: 50,
            caption: 'Composite',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false,
            subGrid: true,
            shrinkToFit: false,
            multiselect: false,
            multiPageSelection: true,
            multiboxonly: true,
            datatype: function (postData) { getDataOutBox(postData); },
            subGridRowExpanded: showChildGrid,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
        })
});
function total_cp(cellValue, options, rowdata, action) {
    return (rowdata.BTP + rowdata.TP) + "EA";
}
function dvi_tp(cellValue, options, rowdata, action) {
    return rowdata.TP + "EA";
}
function dvi_btp(cellValue, options, rowdata, action) {
    return rowdata.BTP + "EA";
}
var product_jqgrid = '';
var childGridID = '';
function showChildGrid(parentRowID, parentRowKey1) {
    var flag = false;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

  var rowData = $('#GeneralGrid').jqGrid('getRowData', parentRowKey1);
    product_jqgrid = rowData.product_cd;
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    $("#" + childGridID).jqGrid({
        mtype: "GET",
        datatype: function (postData) { getDataDT(postData); },
        //async: false,
        //page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right' },
            { name: "DT", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntDestroy },
            { name: "RD", width: 40, align: "center", label: "", resizable: false, title: false, formatter: bntRedo },
            { label: 'PO', name: 'at_no', width: 150, align: 'center' },
            { label: 'Container', name: 'bb_no', width: 250, align: 'left' },
            { label: 'Composite Code', name: 'mt_cd', width: 300, align: 'left' },
            { label: 'Material', name: 'mt_nm', width: 100, align: 'left' },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right', formatter: qty_unit },
            { label: '', name: 'bundle_unit', width: 150, align: 'right', hidden: true },
            //{ label: 'Lenght', name: 'lenght', width: 150, align: 'right' },
            //{ label: 'Size ', name: 'size', width: 150, align: 'right' },
            //{ label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            { label: 'Recevied Date', name: 'reg_date', width: 110, align: 'right', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" } },

        ],

        shrinkToFit: false,
        width: '100%',
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 50,
        width: null,
        rownumbers: true,
        //loadonce: true,
        //multiselect: true,
        multiPageSelection: true,
        multiboxonly: true,
        multiselect: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },

        onSelectRow: function (id, rowid, status, e) {
            if (id) {
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey1 + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey1, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey1, true);
                    flag = false;
                }
            }
        },

        onSelectAll: function (id, rowid, status, e) {
            if (id) {
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey1 + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey1, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey1, true);
                    flag = false;
                }
            }
        }
    });
}
function bntDestroy(cellValue, options, rowdata, action) {
    var mt_cd = rowdata.mt_cd;
    var Id = rowdata.wmtid;
    var html = `<button class="btn btn-xs btn-primary"  data-mt_cd="${mt_cd}" data-wmtid="${Id}" onclick="FuntionDestroy(this);">Destroy</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};
function FuntionDestroy(e) {
    var r = confirm("Bạn có chắc muốn HỦY mã này không???");
    if (r == true) {
        var wmtid = $(e).data("wmtid");
        var mt_cd = $(e).data("mt_cd");
        $.ajax({
            url: '/TIMS/destroyLotCode?id=' + wmtid + '&MaterialCode=' + mt_cd,
            type: "get",
            dataType: "json",
            success: function (data) {
                if (data.result == true) {
                    //console.log(data);
                    //var id = data.data[0].id;
                    //$("#" + childGridID).setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });
                    //return;
                    SuccessAlert(data.message);
                    $("#" + wmtid).css({ background: "#28a745", color: "#fff" });
                }
                else {
                    ErrorAlert(data.message);
                    return;
                }
            },
        });
    }
    else {
        return;
    }

}
function bntRedo(cellValue, options, rowdata, action) {
    var mt_cd = rowdata.mt_cd;
    var Id = rowdata.wmtid;
    var html = `<button class="btn btn-xs btn-primary" data-mt_cd="${mt_cd}" data-wmtid="${Id}" onclick="FuntionRedo(this);">Redo</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};
function FuntionRedo(e) {
    var r = confirm("Bạn có chắc muốn Redo mã này không???");
    if (r == true) {
        var wmtid = $(e).data("wmtid");
        var mt_cd = $(e).data("mt_cd");
        $.ajax({
            url: '/TIMS/redoLotCode?id=' + wmtid + '&MaterialCode=' + mt_cd,
            type: "get",
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    $("#" + wmtid).css({ background: "#28a745", color: "#fff" });
                }
                else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
    else {
        return false;
    }
}
function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";
    var reg = /(\d{4})(\d{2})(\d{2})/;
    if (reg.test(cellvalue))
        return cellvalue.replace(reg, "$1-$2-$3");
    else {
        reg = /(\d{4})(\d{2})\-(\d{2})/;
        if (reg.test(cellvalue))
            return cellvalue.replace(reg, "$1-$2-$3");
        else {
            reg = /(\d{4})\-(\d{2})(\d{2})/;
            if (reg.test(cellvalue))
                return cellvalue.replace(reg, "$1-$2-$3");
            else {
                reg = /(\d{4})\-(\d{2})\-(\d{2})/;
                if (reg.test(cellvalue))
                    return cellvalue.replace(reg, "$1-$2-$3");
                else {
                    reg = /(\d{4})(\d{2}).(\d{2})/;
                    if (reg.test(cellvalue))
                        return cellvalue.replace(reg, "$1-$2-$3");
                    else {
                        reg = /(\d{4}).(\d{2}).(\d{2})/;
                        if (reg.test(cellvalue))
                            return cellvalue.replace(reg, "$1-$2-$3");

                        else {
                            reg = /(\d{4})(\d{2})\\(\d{2})/;
                            if (reg.test(cellvalue))
                                return cellvalue.replace(reg, "$1-$2-$3");
                            else {
                                reg = /(\d{4})\\(\d{2})\\(\d{2})/;
                                if (reg.test(cellvalue))
                                    return cellvalue.replace(reg, "$1-$2-$3");
                                else {
                                    reg = /(\d{4})\\(\d{2})\.(\d{2})/;
                                    if (reg.test(cellvalue))
                                        return cellvalue.replace(reg, "$1-$2-$3");
                                    else {
                                        reg = /(\d{4})\.(\d{2})\\(\d{2})/;
                                        if (reg.test(cellvalue))
                                            return cellvalue.replace(reg, "$1-$2-$3");
                                        else
                                            return cellvalue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
function qty_unit(cellvalue, options, rowObject) {
    return checkNullNuber(cellvalue) + ' ' + checknullString(rowObject.bundle_unit);
}
function checkNullNuber(value) {
    if (value == null || value == '' || value == 'undefined' || value.length == 0) {
        return 0;
    }
    return value;
}
function qty_unit2(cellvalue, options, rowObject) {
    return checknullZero(cellvalue) + ' ' + checknullString(rowObject.bundle_unit);
}
function checknullZero(value) {
    if (value == null || value == '' || value == 'undefined' || value.length == 0) {
        return 0;
    }
    return value;
}
function checknullString(value) {
    if (value == null || value == '' || value == 'undefined' || value.length == 0) {
        return "";
    }
    return value;
}




//--------------thanhnam-----------------
function getDataOutBox(pdata) {
    //debugger;
    //$('.loading-gif-1').show();
    var params = new Object();
    var rows = $("#GeneralGrid").getDataIDs();
    //if (gridLot.jqGrid('getGridParam', 'reccount') == 0)
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.at_no = $('#s_po').val() == null ? "" : $('#s_po').val().trim();
    params.model = $('#s_model').val() == null ? "" : $('#s_model').val().trim();
    params.product = $('#s_prd_cd').val() == null ? "" : $('#s_prd_cd').val().trim();
    params.product_name = $('#s_prd_nm').val() == null ? "" : $('#s_prd_nm').val().trim();
    params.reg_dt_start = $('#create_dt_start').val() == null ? "" : $('#create_dt_start').val().trim();
    params.reg_dt_end = $('#create_dt_end').val() == null ? "" : $('#create_dt_end').val().trim();
    params.mt_cd = $('#s_mt_cd').val() == null ? "" : $('#s_mt_cd').val().trim();
    params.bb_no = $('#s_bb_no').val() == null ? "" : $('#s_bb_no').val().trim();
    params.sts = $('#status_tab2').val() == null ? "" : $('#status_tab2').val().trim();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/WIP/getGeneral",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#GeneralGrid")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};


$("#searchBtn").click(function () {
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata)
});


function getDataDT(pdata) {
    $('.loading-gif-2').show();
    var params = new Object();

    var rows = $("#" + childGridID).getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.product = product_jqgrid;
    params.bb_no = $('#s_bb_no').val() == null ? "" : $('#s_bb_no').val().trim();
    params.sts = $('#status_tab2').val() == null ? "" : $('#status_tab2').val().trim();

    $("#" + childGridID).jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/WIP/GetgeneralDetail",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#" + childGridID)[0];
                showing.addJSONData(data);
                $('.loading-gif-2').hide();
            }
        },
        error: function () {
            $('.loading-gif-2').hide();
            return;
        }
    });
};
//$('#excelBtn').click(function () {
//    //--- export to excel on server side
//    debugger;
//    $('#exportData').attr('action', "/WIP/ExportgeneralToExcel");
//});
$("#printBtn").click(function () {
    var array = [];
    var array2 = [];

    var myList = $("#list").jqGrid('getDataIDs');
    var model = [];
    var row_subGrid = [];
    //hằng code
    for (var i = 0; i < myList.length; i++) {
        var i, selRowIds = $("#list_" + myList[i] + "_table").jqGrid("getGridParam", "selarrrow"), n, rowData;
        if (selRowIds != undefined) {
            for (var j = 0; j < selRowIds.length; j++) {
                row_subGrid.push(selRowIds[j]);
            }
        }
    }

    if (row_subGrid.length > 0) {
        var mapForm = document.createElement("form");
        mapForm.target = "Map";
        mapForm.type = "hidden";
        mapForm.method = "POST"; // or "post" if appropriate
        mapForm.action = "/WIP/PrintGeneral";

        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = "qrcode";
        mapInput.value = row_subGrid;

        mapForm.appendChild(mapInput);
        document.body.appendChild(mapForm);

        map = window.open(" ", "Map", "width=600, height=800, left=0, top=100, location=no, status=no,");

        if (map) {
            mapForm.submit();
        } else {
            ErrorAlert("You must allow popups for this map to work.'");
        }
    }

    else {
        ErrorAlert("Please Select Grid");
    }
});
$("#printBtn_tab2").click(function () {
    var array = [];
    var array2 = [];

    var myList = $("#GeneralGrid").jqGrid('getDataIDs');
    var model = [];
    var row_subGrid = [];
    //hằng code
    for (var i = 0; i < myList.length; i++) {
        var i, selRowIds = $("#GeneralGrid_" + myList[i] + "_table").jqGrid("getGridParam", "selarrrow"), n, rowData;
        if (selRowIds != undefined) {
            for (var j = 0; j < selRowIds.length; j++) {
                row_subGrid.push(selRowIds[j]);
            }
        }
    }

    if (row_subGrid.length > 0) {
        var mapForm = document.createElement("form");
        mapForm.target = "Map";
        mapForm.type = "hidden";
        mapForm.method = "POST"; // or "post" if appropriate
        mapForm.action = "/WIP/PrintGeneral";

        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = "qrcode";
        mapInput.value = row_subGrid;

        mapForm.appendChild(mapInput);
        document.body.appendChild(mapForm);

        map = window.open(" ", "Map", "width=600, height=800, left=0, top=100, location=no, status=no,");

        if (map) {
            mapForm.submit();
        } else {
            ErrorAlert("You must allow popups for this map to work.'");
        }
    }

    else {
        ErrorAlert("Please Select Grid");
    }
});
//#endregion

//#region tab3
var gridMemo = $(`#MemoGrid`);
var childGridMemo = ``;
var mtCodeMemo = ``;
var memoWidth = ``;
var memoSpec = ``;
function GridMemo() {
    gridMemo.jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { key: false, label: 'Material', name: 'mt_cd', width: 200, align: 'left', hidden: false },
                { label: 'Width', name: 'width', width: 150, align: 'right' },
                { label: 'Spec', name: 'spec', width: 150, align: 'right' },
                { label: 'Tổng Roll', name: 'total_roll', width: 150, align: 'right' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            pager: "#MemoGridPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 600,
            width: $(".box-body").width() - 5,
            autowidth: false,
            rowNum: 50,
            caption: '',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false,
            subGrid: true,
            shrinkToFit: false,
            multiselect: false,
            multiPageSelection: true,
            datatype: function (postData) { getDataMemo(postData); },
            subGridRowExpanded: showChildGridMemo,
            loadComplete: function () {
                var rowIds = gridMemo.jqGrid('getDataIDs');
                for (var item of rowIds) {
                    gridMemo.expandSubGridRow(item);
                }
            },
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
}

function showChildGridMemo(parentRowID, parentRowKey) {
    childGridMemo = `memo_${parentRowID}_table`;
    var childGridMemoPagerID = `memo_${parentRowID}_pager`;
    var rowData = gridMemo.jqGrid('getRowData', parentRowKey);
    mtCodeMemo = rowData.mt_cd;
    memoWidth = rowData.width;
    memoSpec = rowData.spec;
    $('#' + parentRowID).append(`<table id="${childGridMemo}"></table><div id="${childGridMemoPagerID}" class=scroll></div>`);
    $("#" + childGridMemo).jqGrid({
        mtype: "GET",
        datatype: function (postData) { getDataDTMemo(postData); },
        colModel: [
            { key: true, name: 'id', width: 50, align: 'right', hidden: true },
            { label: 'Model', name: 'md_cd', width: 100, align: 'left' },
            { label: 'Product', name: 'style_no', width: 100, align: 'left' },
            { label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
            { label: 'Width', name: 'width', width: 50, align: 'right' },
            { label: 'Spec', name: 'spec', width: 50, align: 'right' },
            { label: 'Số cuộn', name: 'TX', width: 50, align: 'right' },
            { label: 'Ngày nhận ', name: 'receiving_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" } },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        shrinkToFit: false,

        height: '100%',
        pager: "#" + childGridMemoPagerID,
        rowNum: 50,
        width: '100%',
        rownumbers: true,
        multiPageSelection: true,
        multiboxonly: false,
        multiselect: false,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        loadonce: false,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },

        onSelectRow: function (id, rowid, status, e) {
            //if (id) {
            //    var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");
            //    if (selRowIds_LV2 != null && flag == false) {
            //        flag = true;
            //        $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
            //    }
            //    if (selRowIds_LV2.length == 0) {
            //        $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
            //        flag = false;
            //    }
            //}
        },
        onSelectAll: function (id, rowid, status, e) {
            //if (id) {
            //    var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");
            //    if (selRowIds_LV2 != null && flag == false) {
            //        flag = true;
            //        $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
            //    }
            //    if (selRowIds_LV2.length == 0) {
            //        $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
            //        flag = false;
            //    }
            //}
        }
    });
}

function getDataMemo(pdata) {
    var params = new Object();
    var rows = $("#MemoGrid").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.MTCode = $(`#memoMTCode`).val() == null ? `` : $(`#memoMTCode`).val().trim();
    params.memoProductCode = $(`#memoProductCode`).val() == null ? `` : $(`#memoProductCode`).val().trim();
    $.ajax({
        url: "/WIP/GetMemo",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = gridMemo[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
}

function getDataDTMemo(pdata) {
    var params = new Object();
    var rows = $("#" + childGridMemo).getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.mtCodeMemo = mtCodeMemo.trim();
    params.productCodeMemo = $(`#memoProductCode`).val() == null ? `` : $(`#memoProductCode`).val().trim();
    params.memoWidth = memoWidth == null ? `0` : memoWidth.trim();
    params.memoSpec = memoSpec == null ? `0` : memoSpec.trim();
    $("#" + childGridMemo).jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/WIP/GetMemoDetail",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#" + childGridMemo)[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            alert(`return error`);
            return;
        }
    });
}

function doThis(pdata, callback) {
    getDataMemo(pdata);
    callback();
}

function andThenThis() {
    var rowIds = gridMemo.jqGrid('getDataIDs');
    for (var item of rowIds) {
        gridMemo.expandSubGridRow(item);
    }
}

$(`#searchBtnMemo`).on(`click`, function () {
    gridMemo.clearGridData();
    gridMemo.jqGrid('setGridParam', { search: true });
    var pdata = gridMemo.jqGrid('getGridParam', 'postData');
    doThis(pdata, andThenThis);
});


//#endregion;
$(`#excelBtn`).on(`click`, function () {
    var mt_no = $('#s_mt_no').val() == null ? "" : $('#s_mt_no').val().trim();
    var mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();
    var s_product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
    var s_locationNAme = $('#s_locationNAme').val() == null ? "" : $('#s_locationNAme').val().trim();

    var Rece_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
    var Rece_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
    /*var sts = $('#status').val() == null ? "" : $('#status').val();*/
 //   $(`#exportData`).attr(`action`, `/DevManagement/ExportToExcelModel?md_nm=${md_nm}&md_cd=${md_cd}`);
    $('#exportData').attr('action', `/WIP/PrintExcelFile?mt_no=${mt_no}&mt_nm=${mt_nm}&s_product_cd=${s_product_cd}&s_locationNAme=${s_locationNAme}&Rece_start=${Rece_start}&Rece_end=${Rece_end}`);

});

$("#ss_mt_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var mdCode = e.target.value.trim();

        $.ajax({
            url: "/WIP/Search_mdCodeGeneral?mt_cd=" + mdCode,
            type: "get",
            dataType: "json",


            success: function (response) {
                if (response.result) {
                    var id = response.data.wmtid;
                    var idNew = testData.find(x => x.id == id).id;
                    if (idNew == null) {
                        alert("Not Found");
                        return;
                    }
                    $('#ss_mt_cd').val("");
                    document.getElementById("ss_mt_cd").focus();
                    //$("#" + childGridID).setRowData(id, response.data, { background: "#28a745", color: "#fff" });
                    $("#" + idNew).css({ background: "#28a745", color: "#fff" });

                }
                else {
                    ErrorAlert(response.message);
                }
            },

        });
    }
});
$(`#excelBtnTab2`).on(`click`, function ()
{
    var at_no = $('#s_po').val() == null ? "" : $('#s_po').val().trim();
    var model = $('#s_model').val() == null ? "" : $('#s_model').val().trim();
    var product = $('#s_prd_cd').val() == null ? "" : $('#s_prd_cd').val().trim();
    var product_name = $('#s_prd_nm').val() == null ? "" : $('#s_prd_nm').val().trim();
    var reg_dt_start = $('#create_dt_start').val() == null ? "" : $('#create_dt_start').val().trim();
    var reg_dt_end = $('#create_dt_end').val() == null ? "" : $('#create_dt_end').val().trim();
    var mt_cd = $('#s_mt_cd').val() == null ? "" : $('#s_mt_cd').val().trim();
    var bb_no = $('#s_bb_no').val() == null ? "" : $('#s_bb_no').val().trim();

    $('#exportDatatab2').attr('action', `/WIP/PrintExcelTab2?at_no=${at_no}&model=${model}&product=${encodeURIComponent(product)}&product_name=${product_name}&reg_dt_start=${reg_dt_start}&reg_dt_end=${reg_dt_end}&mt_cd=${mt_cd}&bb_no=${bb_no}`);

});
