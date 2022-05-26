var pq_no = "";
var item_vcd = "";
$(function () {
    $("#OQCListGrid").jqGrid
        ({
            //url: "/QCInformation/Getproduct_qc_list",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'pqno', name: 'pqno', width: 50, hidden: true },
                { label: 'PQ NO', name: 'pq_no', width: 110, align: 'center' },
                { label: 'Material Lot Code', name: 'ml_no', width: 300, align: 'center' },
                { label: 'item_vcd', name: 'item_vcd', index: 'oflno', width: 10, hidden: true },
                { label: 'Work Date', name: 'work_dt', width: 150, align: 'center', formatter: DateFormatter },
                { label: 'OK Qty', name: 'ok_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Qc Qty', name: 'check_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Defective Qty', name: 'defect_qty', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
                { key: false, label: 'Defect QC Rate (%)', name: 'defect_qty_qc_rate', sortable: true, width: '150', align: 'right' },
                { key: false, label: 'OK Qty Rate (%)', name: 'ok_qty_qc_rate', sortable: true, width: '150', align: 'right' }
            ],
            onSelectRow: function (rowid, selected, status, e) {


                var selectedRowId = $("#OQCListGrid").jqGrid("getGridParam", 'selrow');
                row_id = $("#OQCListGrid").getRowData(selectedRowId);
                pq_no = row_id.pq_no;
                item_vcd = row_id.item_vcd;

                //$("#OQCDTListGrid").setGridParam({ url: "/QCInformation/Getdetaipr_qc_list?" + "item_vcd=" + row_id.item_vcd + "&pq_no=" + row_id.pq_no, datatype: "json" }).trigger("reloadGrid");

                $("#OQCDTListGrid").setGridParam({ /*url: "/WMSReceiving/GetMaterialLotCode",*/ datatype: function (postData) { getOQCDTData(postData); } }).trigger("reloadGrid");

            },
            pager: "#OQCListpager",
            pager: jQuery('#OQCListpager'),
            rowNum: 50,
            viewrecords: true,
            rowList: [50, 100, 200, 500, 100],
            height: 250,
            width: null,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            datatype: function (postData) { getOQCData(postData); },
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


    //function bom_no(cellvalue, options, rowObject) {
    //    var a, b;
    //    if (cellvalue == null) {
    //        return " "
    //    }
    //    else {
    //        a = cellvalue.substr(0, 1);
    //        b = cellvalue.substr(1, 11);
    //        d = cellvalue.substr(11);
    //        c = parseInt(b);
    //        return a + c + d;
    //    }
    //};
    //function po_no(cellvalue, options, rowObject) {
    //    var a, b;
    //    if (cellvalue == null) {
    //        return " "
    //    }
    //    else {
    //        a = cellvalue.substr(0, 1);
    //        b = cellvalue.substr(1);
    //        c = parseInt(b);
    //        return a + c;
    //    }
    //};






});//grid1


$("#searchBtn").click(function () {

    $("#OQCListGrid").clearGridData();

    $('.loading').show();

    var grid = $("#OQCListGrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getOQCData(pdata);
});


function getOQCData(pdata) {

    $('.loading').show();
    var params = new Object();
    if (jQuery('#OQCListGrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.pq_no = $('#pq_no').val() == null ? "" : $('#pq_no').val().trim();
    params.ml_no = $('#ml_no').val() == null ? "" : $('#ml_no').val().trim();
    params.start = $('#start').val() == null ? "" : $('#start').val().trim();
    params.end = $('#end').val() == null ? "" : $('#end').val().trim();



    $("#OQCListGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/QCInformation/getOQCList",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#OQCListGrid")[0];
                grid.addJSONData(data);
                $('.loading').hide();
                $("#OQCDTListGrid").jqGrid('clearGridData');
            }
        }
    })
};

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$(function () {
    $grid = $("#OQCDTListGrid").jqGrid
        ({
            //url: "/QCInformation/Getdetaipr_qc_list",
            datatype: 'json',
            mtype: 'post',
            colModel: [
                { key: true, label: 'pqhno', name: 'pqhno', width: 50, hidden: true },
                { label: 'PQ No ', name: 'pq_no', width: 110, align: 'center' },
                //{ label: 'Process', name: 'process_no', width: 110, align: 'left' },
                { label: 'Subject', name: 'check_subject', width: 200, align: 'left' },
                { label: 'Value', name: 'check_value', width: 110, align: 'left' },
                { label: 'Defective Qty', name: 'check_qty', width: 110, align: 'right', formatter: 'integer' },

            ],
            pager: jQuery('#OQCDTListGridpager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            width: null,
            shrinkToFit: false,
            viewrecords: true,
            loadonce: false,
            height: '300',

            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },

            multiselect: false,
        });

});//grid2


function getOQCDTData(pdata) {
    $('.loading-gif-2').show();
    var params = new Object();
    var rows = $("#OQCDTListGrid").getDataIDs();
    //if (gridLot.jqGrid('getGridParam', 'reccount') == 0)
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.pq_no = pq_no;
    params.item_vcd = item_vcd;

    $("#OQCDTListGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/QCInformation/getOQCDTList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#OQCDTListGrid")[0];
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
function DateFormatter(cellvalue, options, rowObject) {
    if (cellvalue == null || cellvalue == '' || cellvalue == 'undefined' || cellvalue.length == 0) {
        return "";
    }
    return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
};

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