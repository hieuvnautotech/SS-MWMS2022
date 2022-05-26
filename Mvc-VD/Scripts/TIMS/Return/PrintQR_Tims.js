

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
    $grid = $("#PrintQrGrid").jqGrid
        ({
            //url: "/MaterialInformation/Getw_mate_list_wip",
            //url: "/MaterialInformation/searchwmaterial_list_wip",
            //datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                //{ label: 'Machine', name: 'mt_no', width: 400, align: 'left' },
                { label: 'CODE', name: 'mt_no', width: 400, align: 'left' },
                //{ label: 'NAME', name: 'mt_nm', width: 250, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right', formatter: qty_unit },
                { label: '', name: 'bundle_unit', width: 150, align: 'right', hidden: true },
                { label: 'Lenght', name: 'lenght', width: 150, align: 'right' },
                { label: 'Size ', name: 'size', width: 150, align: 'right' },
                { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
                { label: 'Process', name: 'name', width: 110, align: 'right' },
                { label: 'Product', name: 'product', width: 110, align: 'right' },

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
            },
            pager: "#PrintQrGridPager",
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
            //multiselect: true,
            //multiPageSelection: true,
            //multiboxonly: true,
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
        //async: false,
        //page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            { label: 'Material', name: 'mt_cd', width: 400, align: 'left' },
            //{ label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
            //{ label: 'Material', name: 'mt_nm', width: 250, align: 'left' },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
            {
                label: 'Lenght', name: 'lenght', width: 150, align: 'right', editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { required: true }, editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                            defect_qty = parseInt(this.value);
                        });

                        $(element).keyup(function (e) {
                            defect_qty = parseInt(this.value);

                        });

                        $(element).keydown(function (e) {
                            var selectedRowId = $("#PrintQrGrid_" + parentRowKey + "_table").jqGrid("getGridParam", 'selrow');
                            var row_id = $("#PrintQrGrid_" + parentRowKey + "_table").getRowData(selectedRowId);
                            var defect_qty = parseInt(this.value);
                            console.log(defect_qty);
                            if (e.which == 13) {


                                if (defect_qty == 0) {
                                    defect_qty = row_id.lenght1;
                                }



                                var chuoi = $(e).data("wmtid") + "_lenght1";
                                var gr = $("#" + chuoi + "").val();
                                var giatri = gr;
                                console.log(giatri);
                                if ((giatri == "") || (giatri == undefined)) {
                                    giatri = defect_qty;
                                }
                                console.log(giatri);

                                $.ajax({
                                    url: '/TIMS/update_lenght_qty?wmtid=' + row_id.wmtid + "&lenght_qty=" + giatri + "&id1=" + parentRowKey,
                                    type: "get",
                                    dataType: "json",
                                    success: function (data) {
                                        if (data.result) {
                                            var id = data.data[0].wmtid;
                                            $("#PrintQrGrid_" + parentRowKey + "_table").setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });
                                            console.log(data.data[0]);
                                            var id1 = data.data1[0].wmtid.toString();


                                            $("#PrintQrGrid").setRowData(id1, data.data1[0], { background: "#28a745", color: "#fff" });
                                            //$("#list").setRowData(id, data.Data[0], { background: "#28a745", color: "#fff" });
                                            SuccessAlert();
                                            //console.log(data.data1[0]);
                                        }
                                        else {

                                            ErrorAlert(data.message)

                                            //alert('Error');
                                        }


                                    },

                                });
                            }
                            else {
                                defect_qty = parseInt(this.value);
                            }
                        });
                    }
                }

            },
            { label: 'Lenght', name: 'lenght1', width: 150, align: 'right', hidden: true },

            { label: 'Size ', name: 'size', width: 150, align: 'right' },
            { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            { label: 'Process', name: 'name', width: 110, align: 'right' },
            { label: 'Product', name: 'product', width: 110, align: 'right' },
            { label: 'RT Date', name: 'return_date', width: 110, align: 'right', formatter: _Date },

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
                //$("#" + childGridID).jqGrid('editRow', id, true);
                var selRowIds_LV2 = $("#PrintQrGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");
                //$("#" + childGridID).jqGrid('editRow', id, true);
                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#PrintQrGrid").jqGrid('setSelection', parentRowKey, true);

                }
                if (selRowIds_LV2.length == 0) {
                    $("#PrintQrGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }


                //var rowIds = $("#" + childGridID).jqGrid('getDataIDs');
                //iterate over each row
                rowData = $("#" + childGridID).jqGrid('getRowData', id);
                var len = rowData['lenght1'];
                //var confirm_qty = rowData['req_qty'];
                $("#" + childGridID).setCell(id, 'lenght', len);// modify a cell (row, col, value, css or sth u wanna do)

                //$("#" + childGridID).jqGrid('editRow', id, true);


                if (id && id !== lastSelection) {
                    jQuery(this).restoreRow(lastSelection);
                    lastSelection = id;
                }
                jQuery(this).editRow(id, true);

            }
        },

        onSelectAll: function (id, rowid, status, e) {
            if (id) {
                //$("#" + childGridID).jqGrid('editRow', id, true);
                var selRowIds_LV2 = $("#PrintQrGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");
                //$("#" + childGridID).jqGrid('editRow', id, true);
                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#PrintQrGrid").jqGrid('setSelection', parentRowKey, true);

                }
                if (selRowIds_LV2.length == 0) {
                    $("#PrintQrGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }
            }
        },
    });

}

var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#" + childGridID);
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
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
    var rows = $("#PrintQrGrid").getDataIDs();
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
    params.return_date = $('#return_date').val() == null ? "" : $('#return_date').val().trim();

    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#PrintQrGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/TIMS/getPrintQR",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#PrintQrGrid")[0];
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


$("#m_confirm").click(function () {
    var array = [];
    var array2 = [];
    var i, selRowIds = $("#PrintQrGrid").jqGrid("getGridParam", "selarrrow"), n, rowData;
    var model = [];
    var row_subGrid = [];
    $('#loading').show();
    var myList = $("#PrintQrGrid").jqGrid('getDataIDs');
    for (var i = 0; i < myList.length; i++) {
        var i, selRowIds = $("#PrintQrGrid_" + myList[i] + "_table").jqGrid("getGridParam", "selarrrow"), n, rowData;
        if (selRowIds != undefined) {
            for (var j = 0; j < selRowIds.length; j++) {
                row_subGrid.push(selRowIds[j]);
            }
        }
    }


    if (row_subGrid.length > 0) {
        $.ajax({
            type: "get",
            dataType: 'json',
            url: "/TIMS/printQRConfirm?id=" + row_subGrid,
            success: function (data) {
                //dat
                if (data.result) {
                    $("#PrintQrGrid").clearGridData();
                    $("#PrintQrGrid").jqGrid('setGridParam', { search: true });
                    var pdata = $("#PrintQrGrid").jqGrid('getGridParam', 'postData');
                    getDataOutBox(pdata);

                    SuccessAlert('');
                    $('#loading').hide();
                }
                else {

                    ErrorAlert(data.message);
                    $('#loading').hide();

                }
            },
            error: function (data) {

                ErrorAlert(data.message);
                $('#loading').hide();
            }
        });
    }
    else {
        ErrorAlert("Please Select Material Grid");
        $('#loading').hide();
    }

    //if (selRowIds.length > 0) {

    //    for (i = 0; i < selRowIds.length; i++) {
    //        var ID_LV1 = selRowIds[i];

    //        var selRowIds_LV2 = $("#PrintQrGrid_" + ID_LV1 + "_table").jqGrid("getGridParam", "selarrrow");
    //        if (selRowIds_LV2 != null) {

    //            for (j = 0; j < selRowIds_LV2.length; j++) {
    //                var ret = $("#PrintQrGrid_" + ID_LV1 + "_table").jqGrid('getRowData', selRowIds_LV2[j]);
    //                wmtid = ret.wmtid
    //                row_subGrid.push(wmtid);
    //            } //for(j)
    //        }
    //        else {
    //            continue;
    //        }
    //    }//for(i)






});

$("#searchBtn").click(function () {
    $("#PrintQrGrid").clearGridData();
    $("#PrintQrGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#PrintQrGrid").jqGrid('getGridParam', 'postData');
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
        url: "/TIMS/getPrintQRDetail",
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

    $('#exportData').attr('action', "/WIP/ExportgeneralToExcel");
});

$("#printBtn").click(function () {
    debugger;
    var array = [];
    var array2 = [];
    var i, selRowIds = $("#PrintQrGrid").jqGrid("getGridParam", "selarrrow"), n, rowData;
    var model = [];
    var row_subGrid = [];
    var myList = $("#PrintQrGrid").jqGrid('getDataIDs');
    for (var i = 0; i < myList.length; i++) {
        var i, selRowIds = $("#PrintQrGrid_" + myList[i] + "_table").jqGrid("getGridParam", "selarrrow"), n, rowData;
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
        mapForm.action = "/TIMS/btnPrintQR";

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


    //if (selRowIds.length > 0) {

    //    for (i = 0; i < selRowIds.length; i++) {
    //        var ID_LV1 = selRowIds[i];

    //        var selRowIds_LV2 = $("#PrintQrGrid_" + ID_LV1 + "_table").jqGrid("getGridParam", "selarrrow");
    //        if (selRowIds_LV2 != null) {

    //            for (j = 0; j < selRowIds_LV2.length; j++) {
    //                var ret = $("#PrintQrGrid_" + ID_LV1 + "_table").jqGrid('getRowData', selRowIds_LV2[j]);
    //                wmtid = ret.wmtid
    //                row_subGrid.push(wmtid);
    //            } //for(j)
    //        }
    //        else {
    //            continue;
    //        }
    //    }//for(i)


    //}




});
