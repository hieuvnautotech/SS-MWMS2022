$(function () {
    $("#list").jqGrid
        ({
            url: "/wipwms/search_list_WIP_History",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wipmtid', name: 'wipmtid', width: 50, align: 'right', hidden: true },
                { label: 'BOM NO', name: 'rel_bom', width: 150, align: 'left', hidden: true },
                { label: 'BOM NO', name: 'rel_bom1', width: 100, align: 'left', formatter: BOMFormatter },
                { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
                { label: 'MT NO', name: 'mt_no', width: 200, align: 'left' },
                { label: 'MT Name', name: 'mt_nm', width: 200, align: 'left' },
                { label: 'Status', name: 'hist_sts_cd', width: 120, align: 'left', formatter: status_format },
                { label: 'Qty ', name: 'gr_qty', width: 80, align: 'right' },
                { label: 'Remark ', name: 'remark', width: 80, align: 'left' },
                { label: 'From Location ', name: 'from_lct_nm', width: 150, align: 'left', hidden: true },
                { label: 'Location Status', name: 'lct_sts_cd', width: 80, align: 'left', hidden:true },
                { label: 'Departure', name: 'from_lct_cd', width: 80, align: 'left', hidden: true },
                { label: 'Out Date', name: 'output_dt', width: 100, align: 'center', formatter: DateFormatter, hidden: true },
                { label: 'Destination', name: 'to_lct_nm', width: 150, align: 'left' },
                { label: 'Input Date', name: 'input_dt', width: 100, align: 'center', formatter: DateFormatter, hidden: true },
                { label: 'Bobbin No', name: 'bb_no', width: 100, align: 'left', hidden: true },
                { label: 'Description', name: 're_mark', width: 100, align: 'left', hidden: true },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                row_id = $("#list").getRowData(selectedRowId);
                $('#wmtid').val(row_id.wmtid);
                $('#pdid').val(row_id.pdid);
                $('#mpo_no').val(row_id.mpo_no);
                $('#mdpo_no').val(row_id.mdpo_no);
                $('#mt_no_sv').val(row_id.mt_no);
                $('#c_item_vcd').val(row_id.item_vcd);
                $('#c_return_ml_no').val(row_id.mt_cd);
                $('#c_edit_qty').val(row_id.gr_qty);
                $('#c_return_qty1').val(row_id.gr_qty);
            },

            pager: "#gridpager",
            pager: jQuery('#gridpager'),
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 600,
            width: $(".box-body").width() - 5,
            autowidth: false,
            rowNum: 50,
            caption: 'Receiving Manual(WIP)',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            loadonce: true,
            viewrecords: true,
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
        })
    $("#searchBtn").click(function () {
        //  var shelf_cd = $('#shelf_cd').val();
        var mt_no = $('#c_mt_no').val();
        var bom_no = $('#bom_no').val();
        var mt_cd = $('#mt_barcode').val();
        var lct_cd = $('#c_lct_cd').val();
        $.ajax({
            url: "/wipwms/search_list_WIP_History",
            type: "get",
            dataType: "json",
            data: {
                //shelf_cd: shelf_cd,
                mt_no: mt_no,
                bom_no: bom_no,
                mt_cd: mt_cd,
                lct_cd: lct_cd,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });


});//grid1


//$(function () {
//    "use strict";
//    var mydata = [
//            { id: "10", invdate: "2017-06-14T1:25:42.00", name: "test", amount: "" },
//            { id: "20", invdate: "2015-09-01", name: "test2", amount: "300.00", tax: "20.00", closed: false, ship_via: "FE", total: "320.00" },
//            { id: "30", invdate: "2015-09-01", name: "test3", amount: "400.00", tax: "30.00", closed: false, ship_via: "FE", total: "430.00" },
//            { id: "40", invdate: "2015-10-04", name: "test4", amount: "200.00", tax: "10.00", closed: true, ship_via: "TN", total: "210.00" },
//            { id: "50", invdate: "2015-10-31", name: "test5", amount: "300.00", tax: "20.00", closed: false, ship_via: "FE", total: "320.00" },
//            { id: "60", invdate: "2015-09-06", name: "test6", amount: "400.00", tax: "30.00", closed: false, ship_via: "FE", total: "430.00" },
//            { id: "70", invdate: "2015-10-04", name: "test7", amount: "200.00", tax: "10.00", closed: true, ship_via: "TN", total: "210.00" },
//            { id: "80", invdate: "2015-10-03", name: "test8", amount: "300.00", tax: "20.00", closed: false, ship_via: "FE", total: "320.00" },
//            { id: "90", invdate: "2015-09-01", name: "test9", amount: "400.00", tax: "30.00", closed: false, ship_via: "TN", total: "430.00" },
//            { id: "100", invdate: "2015-09-08", name: "test10", amount: "500.00", tax: "30.00", closed: true, ship_via: "TN", total: "530.00" },
//            { id: "110", invdate: "2015-09-08", name: "test11", amount: "500.00", tax: "30.00", closed: false, ship_via: "FE", total: "530.00" },
//            { id: "120", invdate: "2015-09-10", name: "test12", amount: "500.00", tax: "30.00", closed: false, ship_via: "FE", total: "530.00" }
//    ];
//    $("#grid1").jqGrid({
//        colModel: [
//            { name: "name", label: "Client", width: 53 },
//            {
//                name: "invdate", label: "Date", width: 75, align: "center", sorttype: "date",
//                formatter: "date", formatoptions: { newformat: "d/m/Y" }
//            },
//            { name: "amount", label: "Amount", width: 65, template: "number" },
//            { name: "tax", label: "Tax", width: 41, template: "number" },
//            { name: "total", label: "Total", width: 51, template: "number" },
//            { name: "closed", label: "Closed", width: 59, template: "booleanCheckboxFa", firstsortorder: "desc" },
//            {
//                name: "ship_via", label: "Shipped via", width: 87, align: "center", formatter: "select",
//                formatoptions: { value: "FE:FedEx;TN:TNT;DH:DHL", defaultValue: "DH" }
//            }
//        ],
//        data: mydata,
//        rowNum: 10,
//        pager: true,
//        iconSet: "fontAwesome",
//        rownumbers: true,
//        sortname: "invdate",
//        sortorder: "desc",
//        subGrid: true,
//        subGridRowExpanded: function (subgridDivId, rowid) {
//            $("#" + $.jgrid.jqID(subgridDivId))
//            	.html("Simple subgrid data for the row with rowid=<strong>" +
//                		rowid + "</strong>");
//        }
//    }).jqGrid("remapColumnsByName", ["name", "invdate", "subgrid"], true);

//});

var getWH = "/wipwms/Warehouse_Return_Wip";
$(document).ready(function () {
    _getWH();
});

$("#warehouse").on('change', function () {
    var id = $(this).val();
    if (id != undefined && id != '') {
        _getLocation(id);
    }
});
function _getWH() {

    $.get(getWH, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * WareHouse *</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.index_cd + '</option>';
            });
            $("#warehouse").html(html);
        }
    });
}


$('#btn_save_qty').click(function () {
    if ($("#form2").valid() != true) {
        return false;
    }
    if ($('#wmtid').val() == "" || $('#wmtid').val() == undefined || $('#wmtid').val() == null) {
        alert("Please select row");
        return false;
    }
    var wmtid = $('#wmtid').val();
    var c_return_qty = $('#c_edit_qty').val();
    var c_remark = $('#c_remark').val();


    $.ajax({
        type: "get",
        dataType: 'json',
        url: "/wipwms/insert_update_split",
        data: {
            wmtid: $('#wmtid').val(),
            gr_qty: $('#c_edit_qty').val(),
            c_remark: $('#c_remark').val(),
            lct_cd: $('#warehouse').val(),
        },
        success: function (result) {
            // alert(result.message);
            $("#list").jqGrid("setCell", result.rowid, "gr_qty", result.data);
            $("#list").jqGrid("setCell", result.rowid, "remark", c_remark);
            $("#list").jqGrid('setRowData', result.rowid, false, { color: 'white', weightfont: 'bold', background: '#AEB6BF ' });

            //for (var i = 0; i < data.length; i++) {
            //    var id = data[i].wmtid;
            //    $("#list").setRowData(id, data[i], { background: "#d0e9c6" });
        }
    });


});


$("#form2").validate({
    rules: {

        "c_edit_qty": {

            number: true,
            required: true,
            min: 0,

        },

    }
});
function BOMFormatter(cellvalue, options, rowObject) {
    if (cellvalue == null || cellvalue == "") {
        return ""
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);
        return a + c + d;
    }
};

function bom_no_subtr2(cellvalue, options, rowObject) {
    var a, b, c, d;
    cellvalue = rowObject.rel_bom;
    if (cellvalue == null || cellvalue =="") {
        return ""
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);


        return a + c + d;
    }
};
function status_format(cellvalue, options, rowObject) {
    
    if (cellvalue == null) {
        return ""
    }
    else {
        if (cellvalue == "001") {
            return "Return to WIP";
        }
        if (cellvalue == "002") {
            return "Return to WMS";
        }
        return cellvalue;       
    }    
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
};


function DateFormatter(cellvalue, options, rowObject) {
    if (cellvalue != null) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
    }
    else { return "" }
};

$(document).ready(function () {

    _getFactory();
    //_GetStatus();
});

var getFactory = "/wipwms/Warehouse_Wip_Hitory";
//var GetStatus = "/wipwms/GetStatus";
function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Destionation</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
}
//function _GetStatus() {
//    $.get(GetStatus, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">Status</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
//            });
//            $("#mt_sts_cd").html(html);
//        }
//    });
//}