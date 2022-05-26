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
var testData = [];
$("#searchBtn_list").click(function () {
    $("#list").clearGridData();
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getDataList(pdata)
});

function getCheck(e) {

    //var checkbox = document.getElementById("myCheck");
    //if (checkbox.checked === true) {

    //}
    //else {
    //    return;
    //}
    $("#s_locationCode").val("");
    $("#s_locationNAme").val("");
    return;

}
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
        url: "/ExportToMachine/getGeneral_Material",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
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
$(function () {
    $grid = $("#list").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                { label: 'Product Code', name: 'product_cd', width: 200, align: 'left' },
                { label: 'MT NO', name: 'mt_no', width: 200, align: 'left' },
                { label: 'MT Name', name: 'mt_nm', width: 400, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right' },
                { name: 'DSD', hidden: true },
                { name: 'CSD', hidden: true },
                { name: 'returnMachine', hidden: true },
                { name: 'bundle_unit', hidden: true },
                { label: 'Đang sử dụng Qty Length(M)', name: '', width: 150, align: 'right', formatter: dvi_dsd },
                { label: 'Chưa sử dụng Qty Length(M)', name: '', width: 150, align: 'right', formatter: dvi_csd },
                { label: 'Trả về nhưng chưa xác nhận', name: '', width: 150, align: 'right', formatter: dvi_rtk },
                { label: 'Tồn Kho Qty Length(M)', name: '', width: 150, align: 'right', formatter: total },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            pager: "#listPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 500,
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
            //multiselectPosition: "none",
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
    if (rowdata.bundle_unit == "EA") {
        return rowdata.DSD + rowdata.CSD + "EA";
    }
 
    return rowdata.DSD + rowdata.CSD + rowdata.returnMachine + " M";
}
function dvi_dsd(cellValue, options, rowdata, action) {
    if (rowdata.bundle_unit == "EA") {
        return rowdata.DSD + "EA";
    }
    return rowdata.DSD + " M";
}
function dvi_csd(cellValue, options, rowdata, action) {
    if (rowdata.bundle_unit == "EA") {
        return rowdata.CSD + "EA";
    }
    return rowdata.CSD + " M";
}
function dvi_rtk(cellValue, options, rowdata, action) {
    if (rowdata.bundle_unit == "EA") {
        return rowdata.returnMachine + "EA";
    }
    return rowdata.returnMachine + " M";
}
function qty_unit(cellvalue, options, rowObject) {
    return checkNullNuber(cellvalue) + ' ' + checknullString(rowObject.bundle_unit);
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
//function getDataList(pdata) {
//    var params = new Object();
//    var rows = $("#list").getDataIDs();
//    if (rows.length == 0) {
//        params.page = 1;
//    }
//    else { params.page = pdata.page; }
//    params.rows = pdata.rows;
//    params.sidx = pdata.sidx;
//    params.sord = pdata.sord;
//    params._search = pdata._search;
//    params.mt_no = $('#s_mt_no').val() == null ? "" : $('#s_mt_no').val().trim();
//    params.mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();
//    params.product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
//    params.lct_cd = $('#s_locationCode').val() == null ? "" : $('#s_locationCode').val().trim();
//    var mt_cd = $('#ss_mt_cd').val() == null ? "" : $('#ss_mt_cd').val().trim();

//    params.mt_cd = $('#ss_mt_cd').val() == null ? "" : $('#ss_mt_cd').val().trim();
//    params.recevice_dt_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
//    params.recevice_dt_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
//    params.sts = $('#status').val() == null ? "" : $('#status').val();
//    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
//    $.ajax({
//        url: "/ExportToMachine/getGeneral_Material",

//        type: "GET",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        traditional: true,
//        data: params,
//        success: function (data, st) {
//            debugger;
//            if (st == "success") {
//                var showing = $("#list")[0];
//                showing.addJSONData(data);
//            }
//        },
//        error: function () {
//            return;
//        }
//    });
//};
var mt_no_list = "";
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
    function showChildGrid_List(parentRowID, parentRowKey) {
        var flag = false;
        childGridID = parentRowID + "_table";
        debugger;
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
            url: `/ExportToMachine/GetgeneralDetail_List?mt_no=${encodeURIComponent(mt_no_list)}&mt_nm=${mt_nm}&product_cd=${product_cd}&recevice_dt_start=${recevice_dt_start}&recevice_dt_end=${recevice_dt_end}&sts=${sts}&lct_cd=${lct_cd}&mt_cd=${encodeURIComponent(mt_cd)}`,
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                { label: 'Export Code', name: 'ExportCode', width: 80, align: 'left' },
                { label: 'MT Code', name: 'material_code', width: 350, align: 'left' },
                {
                    label: 'Material', name: 'mt_nm', width: 350, align: 'left', hidden: true
                },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right', formatter: qty_unit },
                { label: '', name: 'bundle_unit', width: 100, align: 'right', hidden: true },
                { label: 'Lenght', name: 'lenght', width: 100, align: 'right' },
                { label: 'Size ', name: 'size', width: 100, align: 'right' },
                //{ label: 'Location', name: 'lct_nm', width: 130, align: 'left' },
                { label: 'Status', name: 'sts_nm', width: 100, align: 'left' },
                { label: 'Recevied Date', name: 'receipt_date', width: 200, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
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
                console.log(ids);
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

$(`#excelBtn`).on(`click`, function () {
    var mt_no = $('#s_mt_no').val() == null ? "" : $('#s_mt_no').val().trim();
    var mt_nm = $('#s_mt_nm').val() == null ? "" : $('#s_mt_nm').val().trim();

    var recevice_dt_start = $('#Rece_start').val() == null ? "" : $('#Rece_start').val().trim();
    var recevice_dt_end = $('#Rece_end').val() == null ? "" : $('#Rece_end').val().trim();
    var sts = $('#status').val() == null ? "" : $('#status').val();
    var product_cd = $('#s_product_cd').val() == null ? "" : $('#s_product_cd').val().trim();
    $('#exportData').attr('action', `/ExportToMachine/PrintExcelFile?mt_no=${mt_no}&mt_nm=${mt_nm}&recevice_dt_start=${recevice_dt_start}&recevice_dt_end=${recevice_dt_end}&sts=${sts}&ProductCode=${product_cd}`);
});
