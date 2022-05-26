$(document).ready(function () {
    $('#return_date').datepicker({
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
});

$(function () {
    $grid = $("#GeneralGrid").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { label: 'id_actualpr', name: 'id_actualpr', width: 200, align: 'left', key: true, hidden: true },
                {label: 'Model', name: 'model', width: 300, align: 'left' },
                { label: 'Product', name: 'product', width: 300, align: 'left' },
                { label: 'Name', name: 'product_name', width: 300, align: 'left' },
                { label: 'Tồn kho (EA)', name: 'qty', width: 300, align: 'right', formatter: 'integer'},


            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            gridComplete: function () {
                $("tr.jqgrow:odd").css("background", "white");
                $("tr.jqgrow:even").css("background", "#f7f7f7");
                $('.loading').hide();

                //$("#GeneralGrid").jqGrid('hideCol', 'setSelection');
            },
            pager: "#GeneralGridPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 600,
            width: null,
            footerrow: true,
            userDataOnFooter: true,
            rowNum: 50,
            caption: '',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false,
            subGrid: true,
            shrinkToFit: false,
            //multiselect: true,
            //multiPageSelection: true,
            //multiselectPosition: "none",
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
            gridComplete: function () {

                //sum Quantity
                var ids = $("#GeneralGrid").jqGrid('getDataIDs');
                var sum_Quantity = 0;
                var $self = $(this)
                for (var i = 0; i < ids.length; i++) {
                    sum_Quantity += parseInt($("#GeneralGrid").getCell(ids[i], "qty"));
                }

                $self.jqGrid("footerData", "set", { model: "Total" });
                $self.jqGrid("footerData", "set", { qty: sum_Quantity });


                jQuery("#GeneralGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        })
});

var wmtid_DT = '';
var childGridID = '';
function showChildGrid(parentRowID, parentRowKey) {
    var flag = false;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    wmtid_DT = parentRowKey;
    $("#" + childGridID).jqGrid({
        mtype: "GET",
        datatype: function (postData) { getDataDT(postData); },
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },

            { label: 'Lot No.', name: 'end_production_dt', width: 80, align: 'center' },

            { label: 'Buyer', name: 'buyer_qr', width: 230, align: 'center' },
            { label: 'PO', name: 'po', width: 110, align: 'center' },
            { label: 'Composite Code', name: 'mt_cd', width: 280, align: 'left' },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Status', name: 'sts_nm', width: 80, align: 'center' },
            //{ label: 'Recevied Date', name: 'recevice_dt', width: 100, align: 'center', formatter: _Date },
            { label: 'Recevied Date', name: 'recevice_dt', width: 150, align: 'center', formatter: _DateTime } ,
            { label: 'Expiry Date', name: 'expiry_dt', width: 150, align: 'center'} 
        ], 
        shrinkToFit: false,
        width:null,
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 50,
        rownumbers: true,
        loadonce: false,
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
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }
            }
        },

        onSelectAll: function (id, rowid, status, e) {
            if (id) {
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }
            }
        }
    });
}

//--------------thanhnam-----------------
function getDataOutBox(pdata) {
    debugger;
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
    params.buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    params.productCode = $('#productCode').val() == null ? "" : $('#productCode').val().trim();
    params.poCode = $('#poCode').val() == null ? "" : $('#poCode').val().trim();
    //params.return_date = $('#return_date').val() == null ? "" : $('#return_date').val().trim();

    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/fgwms/getFGGeneral",
        //url: `/fgwms/getFGgeneralDetail`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#GeneralGrid")[0];
                showing.addJSONData(data);
                //$('.loading-gif-1').hide();
            }
        },
        error: function () {
            //$('.loading-gif-1').hide();
            return;
        }
    });
};

function getDataDT(pdata) {
  
    //$('.loading-gif-2').show();
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

    params.id = wmtid_DT;
    params.buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    //params.productCode = $(`#productCode`).val() == null ? '' : $(`#productCode`).val();
    params.poCode = $(`#poCode`).val() == null ? '' : $(`#poCode`).val();
    params.recevice_dt_start = $(`#recevice_dt_start`).val() == null ? '' : $(`#recevice_dt_start`).val();
    params.recevice_dt_end = $(`#recevice_dt_end`).val() == null ? '' : $(`#recevice_dt_end`).val();

    $("#" + childGridID).jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/fgwms/getFGgeneralDetail",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#" + childGridID)[0];
                showing.addJSONData(data);
                //$('.loading-gif-2').hide();
            }
            else {
                return;
            }
        },
        error: function () {
            //$('.loading-gif-2').hide();
            return;
        }
    });
};

$('#excelBtn').click(function () {
    //$('#loading').show();
    //--- export to excel on server side
    var productCode = $(`#productCode`).val() == null ? `` : $(`#productCode`).val().trim();
    var poCode = $(`#poCode`).val() == null ? '' : $(`#poCode`).val();
    var recevice_dt_start = $(`#recevice_dt_start`).val() == null ? '' : $(`#recevice_dt_start`).val();
    var recevice_dt_end = $(`#recevice_dt_end`).val() == null ? '' : $(`#recevice_dt_end`).val();

    $(`#exportData`).attr(`action`, `ExportFGgeneralToExcel?productCode=${productCode}&poCode=${poCode}&recevice_dt_start=${recevice_dt_start}&recevice_dt_end=${recevice_dt_end}`);
    //$('#loading').hide();
});

$("#printBtn").click(function () {
    var array = [];
    var array2 = [];
    var i, selRowIds = $("#GeneralGrid").jqGrid("getGridParam", "selarrrow"), n, rowData;
    var model = [];
    var row_subGrid = [];

    if (selRowIds.length > 0) {
        for (i = 0; i < selRowIds.length; i++) {
            var ID_LV1 = selRowIds[i];

            var selRowIds_LV2 = $("#GeneralGrid_" + ID_LV1 + "_table").jqGrid("getGridParam", "selarrrow");
            if (selRowIds_LV2 != null) {
                for (j = 0; j < selRowIds_LV2.length; j++) {
                    var ret = $("#GeneralGrid_" + ID_LV1 + "_table").jqGrid('getRowData', selRowIds_LV2[j]);
                    wmtid = ret.wmtid
                    row_subGrid.push(wmtid);
                } //for(j)
            }
            else {
                continue;
            }
        }//for(i)

        if (row_subGrid.length > 0) {
            var mapForm = document.createElement("form");
            mapForm.target = "Map";
            mapForm.type = "hidden";
            mapForm.method = "POST"; // or "post" if appropriate
            mapForm.action = "/fgwms/PrintFGGeneral";

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
    }
    else {
        ErrorAlert("Please Select Grid");
    }
});

$("#searchBtn").click(function () {
    debugger;
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata)
});
function _DateTime(cellvalue) {
    if (cellvalue == null)
        return "";
    var html = "";
    html += cellvalue.substr(0, 4) + "-" + cellvalue.substr(4, 2) + "-" + cellvalue.substr(6, 2) + " " + cellvalue.substr(8, 2) + ":" + cellvalue.substr(10, 2) + ":" + cellvalue.substr(12, 2);
    return html;

}
function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";

    cellvalue = cellvalue.substr(0, 8);
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
    return checknull(cellvalue) + ' ' + checknull(rowObject.bundle_unit);
}

function checknull(value) {
    if (value == null || value == '' || value == 'undefined' || value.length == 0) {
        return "";
    }
    return value
}

$("#buyer_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var buyer_cd = e.target.value.trim();


        $.ajax({
            url: "/fgwms/Search_BuyergetFGGeneral?buyerCode=" + buyer_cd,
            type: "get",
            dataType: "json",
           
           
            success: function (response) {
                if (response.result) {
                  
                    $(".thongbao").addClass("active");
                    $(".thongbao").removeClass("hidden");
                  
                    document.getElementById("noidung").innerHTML  =(response.message);
                }
                else {
                    $(".thongbao").addClass("active");
                    $(".thongbao").removeClass("hidden");
                    document.getElementById("noidung").innerHTML = (response.message);
                }
            },
            error: function () {
                
                $(".thongbao").addClass("active");
                $(".thongbao").removeClass("hidden");
                document.getElementById("noidung").innerHTML = (response.message);

            }
        });


    }
});

$("#tabClose").on("click", function (event) {
  
    $(".thongbao").addClass("hidden");
    $(".thongbao").removeClass("active");
});