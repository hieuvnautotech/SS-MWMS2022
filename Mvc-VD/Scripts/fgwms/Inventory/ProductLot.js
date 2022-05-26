

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
    //$('#end').datepicker({
    //    dateFormat: 'yy-mm-dd',
    //    "autoclose": true
    //}).datepicker('setDate', 'today');
});
$(function () {
    $grid = $("#ProductLotGrid").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { key: true, label: 'bmno', name: 'bmno', width: 50, align: 'right', hidden: true },
                { label: 'Box No', name: 'bx_no', width: 250, align: 'left', formatter: code_pp },
                { label: 'Buyer Group', name: 'buyer_qr', width: 250, align: 'left' },
                {
                    label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right',/* formatter: qty_unit },
                { label: '', name: 'bundle_unit', width: 150, align: 'right', hidden: true */ },
                { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
                { label: 'Mapping Date', name: 'mapping_dt', width: 110, align: 'right', },

            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            gridComplete: function () {
                
                $('.loading').hide();

                //$("#GeneralGrid").jqGrid('hideCol', 'setSelection');
            },
            pager: "#ProductLotGridPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 200,
            width:null,
            autowidth: false,
            rowNum: 50,
            caption: '',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: false,
            //subGrid: true,
            shrinkToFit: false,
            //multiselect: true,
            //multiPageSelection: true,
            //multiselectPosition: "none",
            multiboxonly: true,
            datatype: function (postData) { getDataOutBox(postData); },
            //subGridRowExpanded: showChildGrid,
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
var wmtid_DT = '';
var childGridID = '';
function code_pp(cellValue) {
    var html = '<a class="css_pp_cilck" data-value="' + cellValue + '" onclick="ViewEXTPopup(this);">' + cellValue + '</a>';
    return html;
};

function showChildGrid(parentRowID, parentRowKey) {
    var flag = false;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    wmtid_DT = parentRowKey;
    $("#" + childGridID).jqGrid({

        mtype: "GET",
        datatype: function (postData) { getDataDT(postData); },
        //async: false,
        //page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            //{ label: 'Lot Info', name: '', width: 70, align: 'center', formatter: Lot_info },
            //{ label: 'Use Info', name: '', width: 70, align: 'center', formatter: Use_info },
            { label: 'Composite Code', name: 'mt_cd', width: 400, align: 'left' },
            //{ label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
            { label: 'Material', name: 'mt_nm', width: 250, align: 'left' },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
            { label: 'Lenght', name: 'lenght', width: 150, align: 'right' },
            { label: 'Size ', name: 'size', width: 150, align: 'right' },
            { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            { label: 'Recevied Date', name: 'recevice_dt', width: 110, align: 'right', formatter: _Date },

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
        //multiPageSelection: true,
        //multiboxonly: true,
        //multiselect: true,
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

        //onSelectRow: function (id, rowid, status, e) {
        //    if (id) {
        //        $("#" + childGridID).jqGrid('editRow', id, true);
        //    }
        //},
    });

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
//--------------thanhnam-----------------
function getDataOutBox(pdata) {
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
    params.mt_no = $('#mt_no').val() == null ? "" : $('#mt_no').val().trim();
    params.buyer_gr = $('#s_buyer').val() == null ? "" : $('#s_buyer').val().trim();

    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#ProductLotGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/fgwms/getFGProductLot",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#ProductLotGrid")[0];
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
$("#searchBtn").click(function () {
    $("#ProductLotGrid").clearGridData();
    $("#ProductLotGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#ProductLotGrid").jqGrid('getGridParam', 'postData');
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

    params.wmtid_DT = wmtid_DT;
    //params.mrd_no_LotCodeOrder = mrd_no_LotCodeOrder;


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
                $('.loading-gif-2').hide();
            }
        },
        error: function () {
            $('.loading-gif-2').hide();
            return;
        }
    });
};
$('#excelBtn').click(function () {

    //--- export to excel on server side
    debugger;
    $('#exportData').attr('action', "/fgwms/ExportFGgeneralToExcel");
});

function ViewEXTPopup(e) {
    $.get("/fgwms/get_bieudo_tonghop6?value=" + $(e).data("value"), function (data) {
        if (data.result) {
            $("#myUL").html(data.kq);

            var toggler = document.getElementsByClassName("caret1");
            var i;

            for (i = 0; i < toggler.length; i++) {
                toggler[i].addEventListener("click", function () {
                    this.parentElement.querySelector(".nested").classList.toggle("active");
                    this.classList.toggle("caret1-down");
                });
            }
        }
        else {
            alert("No Data");
        }
    });
}

 
function PpDetailFGProductLot(e) {
    $('.popup-dialog.FG_Info_Popup').dialog('open');
    var id = $(e).data('id');
    var mt_lot = $(e).data('mt_lot');


    $.get("/fgwms/PartialView_FG_Info_Popup?" +
        "id=" + id + "" + "&mt_lot=" + mt_lot + ""
        , function (html) {
            $("#PartialView_FG_Info_Popup").html(html);
        });
}




function ViewEXTPopup2(e) {

    //$.get("/fgwms/Partial_getTreeDash?" +
    //    "value=" + $(e).data("value") + ""
    //    , function (html) {
    //        console.log(html);
    //        $("Partial_TreeProduct").html(html);
    //    });
}


