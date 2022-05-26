$grid = $("#ShipRequest").jqGrid({
    url: '/wipwms/Getmt_list_WIP',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'bdid', name: 'bdid', width: 50, key: true, hidden: true },
          //{ label: 'wmtid', name: 'wmtid', width: 50, key: true, hidden: true },
        { label: 'Gr_Qty', name: 'gr_qty', width: 50, align: 'left', hidden: true },
         { label: 'BOM No', name: 'bom_no', width: 150, align: 'left' },
        { label: 'MT No', name: 'mt_no', width: 150, align: 'left' },
        { label: 'MT Name', name: 'mt_nm', width: 150, align: 'left' },
        { label: 'Group Unit', name: 'group_unit', width: 80, align: 'right' },
        { label: 'Available', name: 'mt_qty', width: 80, align: 'right', formatter: 'integer' },
        //{ label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
        //{ label: 'Available', name: 'available_qty', width: 80, align: 'right', formatter: 'integer' },

        {
            label: 'Req Bundle Qty', name: 'req_bundle_qty', width: 110, align: 'right', editable: true, editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        //-----characters only
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                    $(element).keyup(function (e) {
                        var id = e.target.id.replace("_req_bundle_qty", "")
                        var row = $("#ShipRequest").jqGrid('getRowData', id);
                        var gtri = this.value;
                        var ketqua = parseInt(gtri) * parseInt(row.gr_qty);
                        var cell = jQuery('#' + id + '_req_qty');
                        if (gtri == "") {
                        } else {
                            //ketqua = ketqua.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                            cell.val(ketqua);
                        }

                    });
                    $(element).keydown(function (e) {
                        var selectedRowId = $("#ShipRequest").jqGrid("getGridParam", 'selrow');
                        var row_id = $("#ShipRequest").getRowData(selectedRowId);
                        if (e.which == 13) {
                            var ketqua = parseInt(this.value) * parseInt(row_id.gr_qty);

                        }
                    });
                }
            }, editrules: { integer: true, required: true }, formatter: 'integer'
        },

        {
            label: 'Req Qty', name: 'req_qty', width: 110, align: 'right', editable: true, editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                }
            }, editrules: { integer: true, required: true }, formatter: 'integer',
        },

        { label: 'Width', name: 'width', width: 110, align: 'right' },
        { label: 'Length (M)', name: 'spec', width: 110, align: 'right' },
        { label: 'Unit', name: 'unit_cd', width: 80, align: 'center' },
        { label: 'Width(mm)', name: 'width', width: 80, align: 'right' },
        { label: 'Length(mm)', name: 'spec', width: 80, align: 'right' },
        { label: 'Area(m²)', name: 'area', width: 80, align: 'right' },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    loadonce: true,
    shrinkToFit: false,
    pager: '#ShipRequestPager',
    rowNum: 2000,
    rownumbers: true,
    caption: 'Material List',
    multiselect: true,
    loadonce: true,
    multiPageSelection: true,
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 320,
    onSelectRow: editRow, // the javascript function to call on row click. will ues to to put the row in edit mode
    onCellSelect: function (rowid) {
        var lastSel = "";
        if (rowid && rowid !== lastSel) {

            jQuery('#ShipRequest').restoreRow(lastSel);
            lastSel = rowid;
        }
        jQuery('#ShipRequest').editRow(rowid, true);
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
    //width: $(".box-body").width() - 5,
    autowidth: true,
    //onSelectRow: function (rowid, status, e) {

    //},
});
//excel
var lastSelection;

function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#ShipRequest");
        grid.jqGrid('restoreRow', lastSelection);
        grid.jqGrid('editRow', id, { keys: true, focusField: 4 });
        lastSelection = id;
    }
}
//get select
// Khai báo URL stand của bạn ở đây
var baseService = "/StandardMgtWh";
var wh = "/wipwms/location_sr_shipp_wip";
var wh1 = "/wipwms/lct_destination";
var getadmin = "/wipwms/getadmin";

$(document).ready(function () {
    _getwhouse();
    _getlct();
    $.ajax({
        url: '/Home/GetSessionMenuData',
        type: 'GET',
        datatype: 'json',
        success: function (SessionData) {
            $("#userid").val(SessionData.userid);
            if (SessionData.userid == "root") {
                $("#uname").val("root");
            }
            else {
                $.get(getadmin + "?id=" + SessionData.userid, function (data) {
                    if (data != null && data != undefined) {
                        $("#uname").val(data[0].uname);
                    }
                });
            }
        },
    });
});

function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Location *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#location").html(html);
        }
    });
}

function _getlct() {
    $.get(wh1, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Detination *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#whouse").html(html);
        }
    });
}
//LƯỚI LƯU

$grid = $("#State").jqGrid({
    url: '/wipwms/Detail_mt_WIP',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
    
        { key: true, label: 'bdid', name: 'bdid', width: 50, hidden: true },
        { label: 'BOM No', name: 'bom_no', width: 150, align: 'left' },
        { label: 'MT No', name: 'mt_no', width: 110, align: 'left' },
        { label: 'MT Name', name: 'mt_nm', width: 110, align: 'left' },
        { label: 'Req Bundle Qty', name: 'req_bundle_qty', width: 110, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, formatter: 'integer', },
        { label: 'Req qty', name: 'req_qty', width: 110, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, formatter: 'integer', },
        { label: 'Width', name: 'width', width: 110, align: 'right' },
        { label: 'Length (M)', name: 'spec', width: 110, align: 'right' },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#StatePager',
    rowNum: 2000,
    rownumbers: true,
    caption: 'Direction List',
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 320,
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
    //width: $(".box-body").width() - 5,
    autowidth: true,
    onSelectRow: function (rowid, status, e) {

    },
});

$("#req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#req_rec_dt').datepicker().datepicker('setDate', 'today');

$("#searchBtn").click(function () {
    $.ajax({
        url: "/wipwms/searchmt_list_WIP",
        type: "get",
        dataType: "json",
        data: {
            //fo_no: $('#c_fo_no').val(),
            bom_no: $('#c_bom_no').val(),
            mt_no: $('#c_mt_no').val(),
           // location: $('#location'),
        },
        success: function (result) {
            $("#ShipRequest").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

$("#c_bom_no").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/DevManagement/Autobom",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.bom_no, value: item.bom_no };
                }))

            },
            error: function (jqXHR, textStatus, errorThrown) {
                var result = [{ label: "no results", value: response.term }];
                response(result);
            },
        })
    },
    messages: {
        noResults: '',
    }
});

$("#c_mt_no").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/DevManagement/Automt_no",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.mt_no, value: item.mt_no };
                }))

            },
            error: function (jqXHR, textStatus, errorThrown) {
                var result = [{ label: "no results", value: response.term }];
                response(result);
            },
        })
    },
    messages: {
        noResults: '',
    }
});

$("#c_fo_no").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/ShippingMgt/Autofo_no",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.fo_no, value: item.fo_no };
                }))

            },
            error: function (jqXHR, textStatus, errorThrown) {
                var result = [{ label: "no results", value: response.term }];
                response(result);
            },
        })
    },
    messages: {
        noResults: '',
    }
});

$("#save_1").click(function () {
    //var fruits = [];
    //var id_fr = [];
    //var i, n, rowData, selRowIds = $("#State").jqGrid("getGridParam", "selarrrow"), id = "", req_qty = "";
   
    //for (i = 0, n = selRowIds.length; i < n; i++) {
    //    id = selRowIds[i];
    //    req_qty = getCellValue(selRowIds[i], 'req_qty');
    //    if (req_qty == undefined) {
    //        var ret = $("#State").jqGrid('getRowData', selRowIds[i]);

    //        var giatri = ret.reg;
    //        var giatri1 = ret.wmtid;
    //        if (giatri != "0") {
    //            fruits.push(giatri);
    //            id_fr.push(giatri1);
    //        }
    //    } else {
    //        if (reg_qty != "0") {
    //            fruits.push(reg_qty);
    //            id_fr.push(id);
    //        }
    //    }
    //}

    //$.ajax({
    //    url: "/wipwms/Detail_mt_WIP?" + "id=" + id_fr + "&" + "req_qty=" + fruits,
    //    type: "get",
    //    dataType: "json",
    //    success: function (data) {
    //        for (var i = 0; i < data.length; i++) {
    //            var id = data[i].wmtid;
    //            $("#State").jqGrid('delRowData', id);
    //        }
    //    }
    //});

    //------------------------
    var i, n, selRowIds = $("#State").jqGrid("getGridParam", "selarrrow"), id;
    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        $("#State").jqGrid('delRowData', id);
    }
});

$("#save_2").click(function () {
    var fruits = [];
    var fruits2 = [];
    var id_fr = [];
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "", req_bundle = "";
    var j, m, rowData, selRowIds2 = $("#State").jqGrid("getGridParam", "selarrrow");


    if (selRowIds.length > 0) {
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];
            req_qty = getCellValue(selRowIds[i], 'req_qty');
            req_bundle_qty = getCellValue(selRowIds[i], 'req_bundle_qty');
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            if (req_qty == undefined && req_qty != "0" && req_qty != "") {

                var giatri = ret.req_qty;
                var giatri1 = ret.wmtid;
                var giatri2 = ret.req_bundle_qty;
                if (giatri != "0") {

                    var rowData = $('#ShipRequest').jqGrid('getRowData', ret.bdid);

                    rowData.reg = giatri;
                    if (req_bundle_qty != undefined) {
                        rowData.req_bundle_qty = req_bundle_qty;
                    } else {                                                          
                        rowData.req_bundle_qty = giatri2;
                    }

                    $("#State").jqGrid('delRowData', ret.bdid);
                                                                                            
                    $("#State").jqGrid('addRowData', ret.bdid, rowData, 'last');
                    $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });

                }
            } else {
                if (req_qty != "0" && req_qty != "") {
                    var rowData = $('#ShipRequest').jqGrid('getRowData', ret.bdid);
                    rowData.req_qty = req_qty;
                    if (req_bundle_qty != undefined) {
                        rowData.req_bundle_qty = req_bundle_qty;
                    } else {
                        rowData.req_bundle_qty = giatri2;
                    }
                    $("#State").jqGrid('delRowData', ret.bdid);
                    $("#State").jqGrid('addRowData', ret.bdid, rowData, 'last');
                    $("#State").setRowData(ret.bdid, false, { background: "#d0e9c6" });
                }
            }
        }
    }
    else {
        alert("Please select Grid.");
    }


    //$("#State").jqGrid('addRowData', id, data[i], 'first');
    //$("#State").setRowData(id, false, { background: "#d0e9c6" });


    //if (selRowIds.length > 0) {


    //    for (i = 0, n = selRowIds.length; i < n; i++) {
    //        id = selRowIds[i];

    //        if (selRowIds2.indexOf(id) == -1) {

    //            req_qty = getCellValue(selRowIds[i], 'req_qty');
    //            req_bundle = getCellValue(selRowIds[i], 'req_bundle_qty');
    //            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);

    //            if (req_qty == undefined) {
    //                var giatri = ret.req_qty;
    //                var giatri1 = ret.wmtid;
    //                var giatri2 = ret.req_bundle_qty;
    //                if (giatri != "0") {
    //                    fruits.push(giatri);
    //                    fruits2.push(giatri2);
    //                    id_fr.push(giatri1);
    //                }
    //            }
    //            else {
    //                if (req_qty != "0") {
    //                    fruits.push(req_qty);
    //                    fruits2.push(req_bundle);
    //                    id_fr.push(id);
    //                }
    //            }
    //        }
    //    }
    //        $.ajax({
    //            url: "/wipwms/Detail_mt_WIP?" + "id=" + id_fr + "&req_qty=" + fruits + "&req_bundle=" + fruits2,
    //            type: "get",
    //            dataType: "json",
    //            success: function (data) {
    //                $("#State").clearGridData();
    //                for (var i = 0; i < data.length; i++) {

    //                    var id = data[i].wmtid;
    //                    $("#State").jqGrid('addRowData', id, data[i], 'first');
    //                    $("#State").setRowData(id, false, { background: "#d0e9c6" });
    //                }
    //            }
    //        });
        
    //}
    //else {
    //    alert("Please select Grid.");
    //}
});


var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#ShipRequest");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
}
function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}

//LƯỚI view all
$grid = $("#w_mr").jqGrid({
    url: '/wipwms/sd_info_WIP',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'sid', name: 'sid', width: 50, hidden: true },
        { label: 'SD No', name: 'sd_no', width: 110, align: 'center' },
        { label: 'Destination', name: 'lct_cd', width: 180, align: 'left' },
        { label: 'Mt Qty', name: 'mt_qty', width: 110, align: 'right', },
        { label: 'Worker', name: 'worker_id', width: 110, align: 'left' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left', },
        { label: 'Work Date', name: 'work_dt', width: 110, align: 'center' },
        { label: 'Real Work Date', name: 'real_work_dt', width: 110, align: 'center' },
        { label: 'Remark', name: 'remark', width: 110, align: 'left', },
        { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
        { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#w_mrpage',
    rowNum: 2000,
    rownumbers: true,
    caption: 'Req Info',
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 220,
    width: $(".box-body").width() - 5,
    jsonReader:
  {
      root: "rows",
      page: "page",
      total: "total",
      records: "records",
      repeatitems: false,
      Id: "0"
  },
    //width: $(".box-body").width() - 5,
    autowidth: false,
    onSelectRow: function (rowid, status, e) {

    },
});

$("#btn_Request").click(function () {
    if ($("#c_mc_id").val().trim() == "") {
        alert("please select the location");
        $("#c_mc_id").val("");
        $('#c_mc_id').focus();
        return false;
    }
    if ($("#userid").val().trim() == "") {
        alert("please enter the Worker");
        $("#userid").val("");
        $('#userid').focus();
        return false;
    }
    if ($("#uname").val().trim() == "") {
        alert("please enter name");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    }
    if ($("#req_rec_dt").val().trim() == "") {
        alert("please enter  Req Receive Date ");
        $("#req_rec_dt").val("");
        $('#req_rec_dt').focus();
        return false;
    }
    //else {
    var fruits = [];
    var fruits_bom = [];
        var ids = jQuery("#State").jqGrid("getDataIDs");
        for (var i = 0; i < ids.length; i++) {
            var giatri = $("#State").getCell(ids[i], "req_qty");
            
            fruits.push(giatri);
            var bom_no = $("#State").getCell(ids[i], "bom_no");
            fruits_bom.push(bom_no);
        }

        var mt_qty = $("#State").jqGrid("getGridParam", 'records');

        $.ajax({
            url: "/wipwms/insert_sd_info_WIP?" + "id=" + ids + "&" + "req_qty=" + fruits,
            type: "post",
            dataType: "json",
            data: {
                lct_cd: $('#c_mc_id').val(),
                worker_id: $('#userid').val(),
                mt_qty: mt_qty,
                manager_id: $('#userid2').val(),
                work_dt: $('#req_rec_dt').val(),
                remark: $('#remark').val(),
            },
            success: function (data) {
                var id = data.sid;
                $("#w_mr").jqGrid('addRowData', id, data, 'first');
                $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });

                $("#State").clearGridData();
                jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                $("#remark").val("");
                $("#userid").val("");
                $("#userid2").val("");
                $("#c_mc_id").val("");
            },
            error: function (data) {
                alert('The Code is the same. Please check again.');
            }
        });
    //}
});

$(document).ready(function () {
    var wage = document.getElementById("userid");
    wage.addEventListener("keydown", function (e) {
        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
            var id = e.target.value;
            getchar(id);
        }
    });
});

function getchar(id) {
    $.get(getadmin + "?id=" + id, function (data) {
        console.log(data);
        if (data != null && data != undefined && data.length != 0) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
                $("#new").html(html);
            });
        } else {
            var html = '';
            html += '<input value="" readonly="readonly"  placeholder="" class="input-text form-control" id="uname">';
            $("#new").html(html);
        }

    });
}

$(document).ready(function () {
    var wage1 = document.getElementById("userid2");
    wage1.addEventListener("keydown", function (e) {
        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
            var id = e.target.value;
            getchar1(id);
        }
    });
});

function getchar1(id) {
    $.get(getadmin + "?id=" + id, function (data) {
        if (data != null && data != undefined && data.length != 0) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname2">';
                $("#new1").html(html);
            });
        } else {
            var html = '';
            html += '<input value="" readonly="readonly"  placeholder="" class="input-text form-control" id="uname2">';
            $("#new1").html(html);
        }
    });
}
