$(function () {
    $("#list").jqGrid
    ({
        url: "/wipwms/searchmanual_wip",
        datatype: 'json', //function (postData) { getData(postData); },
        mtype: 'Get',
        colModel: [
          { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },

           { label: 'Bom NO', name: 'rel_bom', width: 150, align: 'left' , hidden: true},
            { label: 'BOM NO', name: 'rel_bom1', width: 100, align: 'left', formatter: BOMFormatter },
           { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
           { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
           { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
           { label: 'Group QTY', name: 'gr_qty', width: 80, align: 'left' },
           { label: 'Status', name: 'mt_sts_cd', width: 80, align: 'left' },
           { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
           { label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left', hidden: true },
           { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left', hidden: true },
           { label: 'Out Date', name: 'output_dt', width: 150, align: 'center', formatter: DateFormatter , hidden: true},
           { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            { label: 'Input Date', name: 'input_dt', width: 150, align: 'center', formatter: DateFormatter1 },
           { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
           { label: 'Description', name: 're_mark', width: 150, align: 'left' },
           { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            //$('#c_pdid').val(row_id.pdid);
            //$('#c_mt_no').val(row_id.mt_no);
            //$('#c_mt_nm').val(row_id.mt_nm);
            //$('#c_rec_input_dt').val(row_id.rec_input_dt);
            //$('#c_rec_qty').val(row_id.rec_qty);
            //$('#c_bundle_qty').val(row_id.bundle_qty);

            //$('#c_mdpo_no').val(row_id.mdpo_no);
            //$('#c_md_sts_cd').val(row_id.md_sts_cd);
            //$('#c_lct_sts_cd').val(row_id.lct_sts_cd);
            //$('#c_md_sts_cd').val(row_id.md_sts_cd);
            //$("#c_save_but").attr("disabled", false);
            var sheet = document.createElement('style');
            sheet.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #b7f0b1; border-color: rgb(153, 153, 153); }";
            document.body.appendChild(sheet);
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
        loadonce: false,
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

    $("#getPutin").on("click", function () {

        var shelf_cd = $('#shelf_cd').val();
        var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;

        if ($("#shelf_cd").val().trim() == "") {
            alert("Please enter your line name");
            $("#c_line_nm").val("");
            $("#c_line_nm").focus();
            return false;
        }
        if (selRowIds == "") {
            alert("Please select row you want change");
            $("#c_line_nm").val("");
            $("#c_line_nm").focus();
            return false;
        }
        $('#loading').show();
        $.ajax({
            type: "get",
            dataType: 'json',
            url: "/wipwms/Updatemanual_wip?id=" + selRowIds,
            data: {
                lct_cd: shelf_cd,
            },
            success: function (data) {
                $('#loading').hide();
                for (var i = 0; i < data.length; i++) {
                    var id = data[i].wmtid;
                    $("#list").setRowData(id, data[i], { background: "#d0e9c6" });
                }
            },
            error: function (error) {
                $('#loading').hide();
                alert("Error: " + eval(error));
            }
        });
       
       // $('#loading').hide();

    });
    


});//grid1

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

var getLocation = "/wipwms/getLocation";
var getFactory = "/wipwms/Warehouse";
var GetStatus = "/wipwms/GetStatus";
$(document).ready(function () {
    _getLocation();
    _getFactory();
    _GetStatus();
});
function _getLocation() {

    $.get(getLocation, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#shelf_cd").html(html);
        }
    });
}
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



function DateFormatter(cellvalue, options, rowObject) {
    if (cellvalue != null) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
    }
    else { return "" }
};
function DateFormatter1(cellvalue, options, rowObject) {
    if (cellvalue != null) {
        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})$/, "$1-$2-$3")
    }
    else { return "" }
};

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

//$("#searchBtn").click(function () {
//    var shelf_cd = $('#shelf_cd').val();
//    var mt_no = $('#c_mt_no').val().trim();
//    var mt_barcode = $('#mt_barcode').val().trim();
//    var mt_sts_cd = $('#mt_sts_cd').val().trim();
//    var lct_cd = $('#c_lct_cd').val();
//    var bom_no = $('#bom_no').val();
//    $.ajax({
//        url: "/wipwms/searchmanual_wip",
//        type: "get",
//        dataType: "json",
//        data: {
//            shelf_cd: shelf_cd,
//            mt_no: mt_no,
//            mt_barcode: mt_barcode,
//            mt_sts_cd: mt_sts_cd,
//            lct_cd: lct_cd,
//            bom_no: bom_no,
//        },
//        success: function (result) {
//            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });
//});

//function getData(pdata) {
//    $('.loading').show();
//    var params = new Object();
//    if (jQuery('#list').jqGrid('getGridParam', 'reccount') == 0) {
//        params.page = 1;
//    }
//    else { params.page = pdata.page; }
//    params.rows = pdata.rows;
//    params.sidx = pdata.sidx;
//    params.sord = pdata.sord;
//    params.shelf_cd = $('#shelf_cd').val();
//    params.mt_no = $('#c_mt_no').val();
//    params.mt_barcode = $('#mt_barcode').val();
//    params.mt_sts_cd = $('#mt_sts_cd').val();
//    params.lct_cd = $('#c_lct_cd').val();
//    params.bom_no = $('#bom_no').val();
//    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
//    params._search = pdata._search;

//    $.ajax({
//        url: "/wipwms/searchmanual_wip",
//        type: "Get",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        traditional: true,
//        data: params,
//        success: function (data, st) {
//            if (st == "success") {
//                var grid = $("#list")[0];
//                grid.addJSONData(data);
//                $('.loading').hide();
//            }
//        }
//    })
//};

$("#searchBtn").click(function () {

    var shelf_cd = $('#shelf_cd').val();
    if (shelf_cd != "" && shelf_cd != null && shelf_cd != undefined)
        shelf_cd = shelf_cd.trim();
    var mt_no = $('#c_mt_no').val().trim();
    if (mt_no != "" && mt_no != null && mt_no != undefined)
        mt_no = mt_no.trim();
    var mt_barcode = $('#mt_barcode').val().trim();
    if (mt_barcode != "" && mt_barcode != null && mt_barcode != undefined)
        mt_barcode = mt_barcode.trim();
        //var mt_sts_cd = $('#mt_sts_cd').val().trim();
    var lct_cd = $('#c_lct_cd').val();
    if (lct_cd != "" && lct_cd != null && lct_cd != undefined)
        lct_cd = lct_cd.trim();
    var bom_no = $('#bom_no').val();
    if (bom_no != "" && bom_no != null && bom_no != undefined)
        bom_no = bom_no.trim();
    $.ajax({
        url: "/wipwms/searchmanual_wip",
        type: "get",
        dataType: "json",
        data: {
            mt_barcode: mt_barcode,
            mt_no: mt_no,
            bom_no: bom_no,
            lct_cd: lct_cd,
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});


function bom_no_subtr2(cellvalue, options, rowObject) {
    var a, b, c, d;
    cellvalue = rowObject.rel_bom;
    if (cellvalue == null) {
        return "sdf"
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(cellvalue.length - 1, 1);
        c = parseInt(b);


        return a + c + d;
    }
};
