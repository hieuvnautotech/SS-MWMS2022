$(function () {
    $("#list").jqGrid
    ({
        url: "/wipwms/search_list_WIP_Return_W_WMS",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
          { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
          { label: 'pdid', name: 'pdid', width: 50, align: 'right', hidden: true },
           { label: 'mpo_no', name: 'mpo_no', width: 50, align: 'right', hidden: true },
            { label: 'mdpo_no', name: 'mdpo_no', width: 50, align: 'right', hidden: true },
            { label: 'BOM NO', name: 'rel_bom', width: 150, align: 'left', formatter: BOMFormatter},
            { label: 'BOM NO', name: 'rel_bom1', width: 100, align: 'left', hidden: true  },
           { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
           { label: 'MT NO', name: 'mt_no', width: 150, align: 'left' },
           { label: 'MT Name', name: 'mt_nm', width: 200, align: 'left' },
           { label: 'Status', name: 'mt_sts_cd', width: 80, align: 'left' },
           { label: 'Qty ', name: 'gr_qty', width: 80, align: 'right' },
            { label: 'Remark ', name: 'remark', width: 80, align: 'left' },
           { label: 'Location ', name: 'lct_nm', width: 80, align: 'left', hidden: true },
           { label: 'Location Status', name: 'lct_sts_cd', width: 80, align: 'left', hidden: true },
           { label: 'Departure', name: 'from_lct_cd', width: 80, align: 'left', hidden: true },
           { label: 'Out Date', name: 'output_dt', width: 100, align: 'center', formatter: DateFormatter, hidden: true },
           { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left', hidden: true },
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
    })
    $("#searchBtn").click(function () {
        //  var shelf_cd = $('#shelf_cd').val();
        var mt_no = $('#c_mt_no').val();
        var bom_no = $('#bom_no').val();
        var mt_cd = $('#mt_barcode').val();
        var lct_cd = $('#c_lct_cd').val();
        $.ajax({
            url: "/wipwms/search_list_WIP_Return_W_WMS",
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

                //var mygrid = jQuery("#list")[0];                
                //mygrid.addJSONData(result);
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });


});//grid1




//var getWH = "/ReceivingMgt/getWH";
//$(document).ready(function () {
//    _getWH();
//   // _getLocation(id);
//});

////$("#warehouse").on('change', function () {
////    var id = $(this).val();
////    if (id != undefined && id != '') {
////        _getLocation(id);
////    }
////});
//function _getWH() {

//    $.get(getWH, function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected"> * WareHouse *</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.lct_cd + '>' + item.index_cd + '</option>';
//            });
//            $("#warehouse").html(html);
//        }
//    });
//}



$('#btn_save_qty').click(function () {
    //if ($("#form2").valid() != true) {
    //    return false;
    //}
    if ($('#wmtid').val() == "" || $('#wmtid').val() == undefined || $('#wmtid').val() == null) {
        alert("Please select row");
        return false;
    }
    var wmtid = $('#wmtid').val();
    var c_return_qty = $('#c_edit_qty').val();
    var c_remark = $('#c_remark').val();
    var i, n, rowData, selRowIds = $("#list").jqGrid("getGridParam", "selarrrow");
    //var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;
    //if (selRowIds.length == 0 || selRowIds.length == undefined) {
    //    alert("Please select data ");
    //    return false
    //}
    var to_lct_cd = $('#warehouse').val();

    //to_lct_cd = $("#warehouse  option:selected").text();
    to_lct_cd = "Kho NVL";


    $.ajax({
        type: "get",
        dataType: 'json',
        url: "/wipwms/return_material_m_wms?id=" + selRowIds,
        data: {
            //to_lct_cd: $('#warehouse').val(),
            to_lct_cd: "001W01000000000000",
            //wmtid: $('#wmtid').val(),
            //edit_qty: $('#c_edit_qty').val(),
            //c_remark: $('#c_remark').val()
        },
        success: function (result) {

            // alert(result.message);
            if (result) {
                for (i = 0, n = selRowIds.length; i < n; i++) {
                    $("#list").jqGrid("setCell", selRowIds[i], "to_lct_cd", to_lct_cd);
                    $("#list").setRowData(selRowIds[i], false, { background: "#d0e9c6" });
                    //  $("#list").jqGrid("setCell", result.rowid, "remark", c_remark);
                    $("#list").jqGrid('setRowData', result.rowid, false, { color: 'white', weightfont: 'bold', background: '#AEB6BF ' });

                }
            }
            else {
                alert("Data Wrong");
            }

            //for (var i = 0; i < data.length; i++) {
            //    var id = data[i].wmtid;
            //    $("#list").setRowData(id, data[i], { background: "#d0e9c6" });
        }
    });


});




//$("#form2").validate({
//    rules: {

//        "warehouse": {
//            // number: true,
//            required: true,
//            // min: 0,

//        },

//    }
//});


//function bom_no_subtr2(cellvalue, options, rowobject) {
//    var a, b, c, d;
//    var x = rowObject.rel_bom;
//    if (rowObject.rel_bom == null || rowObject.rel_bom == "" ) {
//        return " "
//    }
//    else {
//        a = rowObject.rel_bom.substr(0, 1);
//        b = rowObject.rel_bom.substr(1, 11);
//        d = rowObject.rel_bom.substr(rowObject.rel_bom.length - 1, 1);
//        c = parseInt(b);


//        return a + c + d;
//    }
//};
//function bom_no_subtr2(cellvalue, options, rowObject) {
//    var a, b, c, d;
//   // cellvalue = rowObject.rel_bom;
//    if (cellvalue == null || cellvalue == "") {
//        return ""
//    }
//    else {
//        a = cellvalue.substr(0, 1);
//        b = cellvalue.substr(1, 11);
//        d = cellvalue.substr(cellvalue.length - 1, 1);
//        c = parseInt(b);


//        return a + c + d;
//    }
//};

function BOMFormatter(cellvalue, options, rowObject) {
    if (cellvalue == null || cellvalue == "") {
        return ""
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);
        return a + c + d; }
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
    _GetStatus();
   // _GetDataGrid();
});

var getFactory = "/wipwms/Warehouse_Return_M_WMS";
var GetStatus = "/wipwms/GetStatus";
function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Factory</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
}
function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">Status</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd").html(html);
        }
    });
}

function _GetDataGrid () {
    //  var shelf_cd = $('#shelf_cd').val();
    //var mt_no = $('#c_mt_no').val();
    //var bom_no = $('#bom_no').val();
    //var mt_cd = $('#mt_barcode').val();
    //var lct_cd = $('#c_lct_cd').val();
    $.ajax({
        url: "/wipwms/search_list_WIP_Return_W_WMS",
        type: "get",
        dataType: "json",
        data: {
            //shelf_cd: shelf_cd,
            //mt_no: mt_no,
            //bom_no: bom_no,
            //mt_cd: mt_cd,
            //lct_cd: lct_cd,
        },
        success: function (result) {

            var mygrid = jQuery("#list")[0];
            mygrid.addJSONData(result);
            //$("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
}