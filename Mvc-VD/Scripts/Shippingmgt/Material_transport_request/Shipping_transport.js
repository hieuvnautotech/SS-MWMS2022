$grid = $("#ShipRequest").jqGrid({

    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { key: false, label: 'Product Code', name: 'style_no', width: 100, align: 'left' },
        { key: false, label: 'Product Name', name: 'style_nm', width: 100, align: 'left' },
        { key: false, label: 'Model', name: 'md_cd', sortable: true, width: '100' },
        { label: 'MT No', name: 'mt_no', width: 150, align: 'left', hidden: true },
        { label: 'MT No', name: 'mt_no_r', width: 200, align: 'left', formatter: xoa_phay },
        { label: 'MT Cd', name: 'mt_cd', width: 150, align: 'left', hidden: true },
        { label: 'MT Cd', name: 'mt_no_r_cd', width: 150, align: 'left', formatter: xoa_phay2 },
        { label: 'Type', name: 'mt_type_nm', width: 100 },
         { label: 'Width(mm)', name: 'width', width: 80, align: 'right' },
             //{ label: 'Unit', name: 'unit_cd', width: 80 },
        { label: 'Need Qty', name: 'need_qty', align: 'right', width: 60, formatter: 'integer' },
         { label: 'Cav', name: 'cav', formatter: 'integer', width: 50, align: 'right' },
        { label: 'Feed Qty', name: 'feed_size', formatter: 'integer', width: 80, align: 'right' },
        { label: '% Loss', name: 'pt_lot', width: 50, align: 'right' },
        //{ label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
        {
            label: 'Amount Plan(EA)', name: 'amount', width: 110, formatter: 'integer', align: 'right', editable: false, editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                    $(element).keyup(function (e) {
                        var id = e.target.id.replace("_amount", "")
                        var row = $("#ShipRequest").jqGrid('getRowData', id);
                        var gtri = this.value;
                        var feed_size = row.feed_size;
                        var cav = row.cav;
                        var pt_lot = row.pt_lot;
                        var need_qty = row.need_qty;
                        var Available = row.Available;

                        var ketqua = (((parseFloat(feed_size) * parseFloat(gtri)) / parseFloat(cav)) / 1000) * (1 + parseFloat(pt_lot / 100)) * parseFloat(need_qty) - parseFloat(Available);


                        //var ketqua = ((parseInt(gtri) * (feed_size * 0.001)) / cav) * (1 + (pt_lot/100)) * need_qty - (Available);
                        var cell = jQuery('#' + id + '_reg');
                        if (gtri == "") {
                        } else {
                            kq = Math.round(ketqua);
                            if (kq > 0) {
                                cell.val(kq);
                            }
                            else (cell.val(0))
                        }

                    });
                    $(element).keydown(function (e) {
                        var selectedRowId = $("#ShipRequest").jqGrid("getGridParam", 'selrow');
                        var row_id = $("#ShipRequest").getRowData(selectedRowId);
                        if (e.which == 13) {
                            var feed_size = row_id.feed_size;
                            var cav = row_id.cav;
                            var pt_lot = row_id.pt_lot;
                            var need_qty = row_id.need_qty;
                            var Available = row_id.Available;

                            var ketqua = (((parseFloat(feed_size) * parseFloat(gtri)) / parseFloat(cav)) / 1000) * (1 + parseFloat(pt_lot / 100)) * parseFloat(need_qty) - parseFloat(Available);

                            var cell = jQuery('#' + id + '_reg');
                            if (this.value == "") {
                            } else {
                                kq = Math.round(ketqua);
                                if (kq > 0) {
                                    cell.val(kq);
                                }
                                else (cell.val(0))
                            }
                        }
                    });
                }
            }, editrules: { integer: true, required: true }, formatter: 'integer',
        },
        { label: 'Inventory-Detail', name: 'availbel_mt', width: 100, align: 'right', formatter: sum_inventory_dt },
        { label: 'Inventory-Wip-WMS(m)', name: 'Available_new', width: 100, align: 'right', formatter: sum_inventory },
         { label: 'Available', name: 'Available', width: 80, align: 'right', hidden:true },
         {
             label: 'Req Qty', name: 'reg', width: 110, align: 'right', editable: false, formatter: 'integer', editoptions: {
                 dataInit: function (element) {

                     $(element).keypress(function (e) {
                         if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                             return false;
                         }
                     });

                 }
             }, editrules: { integer: true, required: true },
         },
       { label: 'sum_con', name: 'sum_con', formatter: 'integer', hidden: true },
         { label: 'MT No', name: 'count_add', width: 200, align: 'left', formatter: count_add, hidden: true },
         { label: 'MT cd', name: 'count_add2', width: 200, align: 'left', formatter: count_add2, hidden: true },
         { label: 'MT Name', name: 'mt_nm', width: 110, align: 'left', hidden: true },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    shrinkToFit: false,
    pager: '#ShipRequestPager',
    rownumbers: true,
    caption: 'Material List',
    multiselect: true,
    loadonce: false,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    //datatype: function (postData) { getData(postData); },
    shrinkToFit: false,
    viewrecords: true,
    height: 313,
    width: null,
    multiPageSelection: true,
    subGrid: false, // set the subGrid property to true to show expand buttons for each row
    //subGridRowExpanded: function (subgrid_id, row_id) {
    //    var subgrid_table_id;
    //    subgrid_table_id = subgrid_id + "_t";
    //    var bom_no = ($("#c_bom_no").val() != "" && $("#c_bom_no").val() != null && $("#c_bom_no").val() != undefined ? $("#c_bom_no").val() : $("#bom_fo").val())
    //    var rowData1 = $('#ShipRequest').jqGrid('getRowData', row_id);
    //    var gitri = rowData1.count_add;

    //    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
    //    jQuery("#" + subgrid_table_id).jqGrid({
    //        url: "/ShippingMgt/Get_chuoi_sum_receive?chuoi_mt_no=" + gitri + "&bom_no=" + bom_no,
    //        datatype: 'json',
    //        colModel: [
    //              { label: 'MT No', name: 'mt_no', width: 200, align: 'left', },
    //              { label: 'Inventory-Wip-WMS(m)', name: 'gr_qty', width: 200, align: 'right', formatter: 'integer' },
    //        ],
    //        onSelectRow: function (rowid, selected, status, e) {
    //            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    //        },
    //        height: '100%',
    //        sortname: 'num',
    //        sortorder: "asc",
    //        jsonReader:
    //    {
    //        root: "rows",
    //        page: "page",
    //        total: "total",
    //        records: "records",
    //        repeatitems: false,
    //        Id: "0"
    //    },
    //    });
    //},


    ondblClickRow: function (rowid, iRow, iCol, e) {
        $("#ShipRequest").jqGrid('editRow', rowid, true, function () {
            $("input, select", e.target).focus();
        });
        return;
    },
    onSelectRow: function (id, rowid, status, e) {

        var selectedRowId = $("#ShipRequest").jqGrid("getGridParam", 'selrow');
        row_id = $("#ShipRequest").getRowData(selectedRowId);


        jQuery("#ShipRequest").setColProp('amount', { editable: true });
        jQuery("#ShipRequest").setColProp('reg', { editable: true });


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
        var Amout_qty = $('#Amout_qty').val().trim();

        var rows = $("#ShipRequest").getDataIDs();
        for (var i = 0; i < rows.length; i++) {


            var feed_qty = $("#ShipRequest").getCell(rows[i], "feed_size");
            var cav = $("#ShipRequest").getCell(rows[i], "cav");
            var lot = $("#ShipRequest").getCell(rows[i], "pt_lot");
            var need_qty = $("#ShipRequest").getCell(rows[i], "need_qty");
            var Available = $("#ShipRequest").getCell(rows[i], "Available_new");



            if (parseInt(Amout_qty) > 0) {

                var kq = ((((parseFloat(feed_qty) * parseFloat(Amout_qty)) / parseFloat(cav)) / 1000) * (1 + parseFloat(lot / 100)) * parseFloat(need_qty)) - parseFloat(Available);

                $("#ShipRequest").jqGrid("setCell", rows[i], "amount", Math.round(Amout_qty));
                if (kq > 0) {
                    $("#ShipRequest").jqGrid("setCell", rows[i], "reg", Math.round(kq));
                }
                else {
                    $("#ShipRequest").jqGrid("setCell", rows[i], "reg", 0);
                }
            }
            else {
                $("#ShipRequest").jqGrid("setCell", rows[i], "amount", 0);
                $("#ShipRequest").jqGrid("setCell", rows[i], "reg", 0);
            }

        }
    },
});
$('#Save_amout_qty').click(function () {
    var bom_no = $('#c_bom_no').val();
    if ($("#form1").valid() && bom_no != "") {
        var grid = $("#ShipRequest");
        grid.jqGrid('setGridParam', { search: true });
        var pdata = grid.jqGrid('getGridParam', 'postData');
        getData_search(pdata);
    }
    else {
        if (bom_no == "") {
            alert("Please enter the Bom.");
            $('#c_bom_no').focus();
        }

    }
});



function count_add2(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_cd;
    var gitri = rowdata.mt_no_r_cd;
    if (gitri != null) {
        var html = rowdata.mt_cd + "," + rowdata.mt_no_r_cd
        return html;
    }
    return kq;
}

function count_add(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = rowdata.mt_no + "," + rowdata.mt_no_r
        return html;
    }
    return kq;
}
function xoa_phay_page2(cellValue, options, rowdata, action) {
    var html = "";
    var n = cellValue.indexOf(",");
    if (n != -1) {
        var array = cellValue.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        return html;
    } else { return cellValue; }

    return html;
}

function xoa_phay(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = rowdata.mt_no + "<br>" + html;
        return gitri;
    }
    return kq;
}
function xoa_phay2(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_cd;
    var gitri = rowdata.mt_no_r_cd;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r_cd.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = rowdata.mt_cd + "<br>" + html;
        return gitri;
    }
    return kq;
}

function sum_inventory(cellValue, options, rowdata, action) {
    var a = (rowdata.sum_con == "" || rowdata.sum_con == null || rowdata.sum_con == undefined) ? 0 : rowdata.sum_con;
    var b = (rowdata.Available == "" || rowdata.Available == null || rowdata.Available == undefined) ? 0 : rowdata.Available;
    var kq = a + b;
    return kq;
}
function sum_inventory_dt(cellValue, options, rowdata, action) {
    var kq = (rowdata.Available != null ? rowdata.Available : 0);
    var gitri = cellValue;
    if (gitri != null) {
        var html = "";
        var array = cellValue.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = kq + "<br>" + html;
        return gitri;
    }
    return kq;
}

var lastSelection;
function btn_status(cellValue, options, rowdata, action) {

    var pno = rowdata.pno;
    var insp_yn = rowdata.insp_yn;

    if (insp_yn == "N") {
        var html = '<button class="btn btn-xs btn-info" name="finish' + pno + '" data-pno="' + pno + '" data-insp_yn="' + insp_yn + '" onclick="btn_update_status(this);">Finish</button>';
    }
    else {
        var html = '<button class="btn btn-xs btn-info" data-pno="' + pno + '" data-insp_yn="' + insp_yn + '" disabled="disabled">Finish</button>';
    }
    return html;
}

function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#ShipRequest");
        grid.jqGrid('restoreRow', lastSelection);
        grid.jqGrid('editRow', id, { keys: true, focusField: 4 });
        lastSelection = id;
    }
    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    $("#del_save_but").addClass("disabled");
    jQuery("#w_mr").setGridParam({ datatype: "json" }).trigger('reloadGrid');
}

//get select
// Khai báo URL stand của bạn ở đây
var wh = "/Shippingmgt/Warehouse";
var getadmin = "/ReceivingMgt/getadmin";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Destination *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#whouse").html(html);
        }
    });
}

//LƯỚI LƯU
$grid = $("#State").jqGrid({
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { label: 'MT NO', name: 'mt_no_r', width: 150, align: 'left' },
        { label: 'tonghop', name: 'mt_no', width: 150, align: 'left', hidden: true },
        { label: 'cha', name: 'mt_no_mx', width: 150, align: 'left', hidden: true },
        { label: 'MT Cd', name: 'mt_cd', width: 150, align: 'left', hidden: true },
        { label: 'MT Cd', name: 'mt_no_r_cd', width: 150, align: 'left' },
        { label: 'Type', name: 'mt_type_nm', width: 80, align: 'center' },
        { label: 'Feed Qty', name: 'feed_size', formatter: 'integer', width: 80, align: 'right' },
        { label: 'Cav', name: 'cav', formatter: 'integer', hidden: true },
        { label: '% Loss', name: 'pt_lot', width: 100, align: 'right' },
        { label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Inventory-Wip-WMS(m)', name: 'Available', width: 80, align: 'right', formatter: 'integer' },
       { label: 'Need Qty', name: 'need_qty', align: 'right', formatter: 'integer' },
        { label: 'Amount Plan(EA)', name: 'amount', width: 110, formatter: 'integer', align: 'right', },
        { label: 'Req Qty', name: 'reg', width: 110, align: 'right', },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#StatePager',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    shrinkToFit: false,
    viewrecords: true,
    height: 313,
    width: null,
    caption: 'Req MT List',
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
    onSelectRow: function (rowid, status, e) {

    },
});
$("#req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#req_rec_dt').datepicker().datepicker('setDate', 'today');



$("#save_2").click(function () {
    var fruits = [];
    var id_fr = [];
    var bundle = [];
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "", amount = "";

    if (selRowIds.length > 0) {
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];
            reg_qty = getCellValue(selRowIds[i], 'reg');
            amount = getCellValue(selRowIds[i], 'amount');
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            if (reg_qty == undefined && reg_qty != "") {
                var giatri = ret.reg;
                var giatri1 = ret.wmtid;
                var giatri2 = ret.amount;
                var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
                rowData.reg = giatri;
                if (amount != undefined) {
                    rowData.amount = amount;
                } else {
                    rowData.amount = giatri2;
                }
                rowData.mt_no = ret.count_add;
                rowData.mt_no_mx = ret.mt_no;
                rowData.mt_cd = ret.count_add2;
                rowData.mt_no_r = ret.mt_no_r;
                rowData.mt_no_r_cd = ret.mt_no_r_cd;
                $("#State").jqGrid('delRowData', ret.wmtid);
                $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
                $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" })

            } else {
                if (reg_qty != "") {
                    var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
                    rowData.reg = reg_qty;
                    if (amount != undefined) {
                        rowData.amount = amount;
                    } else {
                        rowData.amount = giatri2;
                    }
                    rowData.mt_no_r = ret.mt_no_r;
                    rowData.mt_no_r_cd = ret.mt_no_r_cd;
                    rowData.mt_no = ret.count_add;
                    rowData.mt_no_mx = ret.mt_no;
                    rowData.mt_cd = ret.count_add2;
                    $("#State").jqGrid('delRowData', ret.wmtid);
                    $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
                    $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });

                }
            }
        }
        var grid = $("#ShipRequest");
        grid.jqGrid('setGridParam', { search: true });
        var pdata = grid.jqGrid('getGridParam', 'postData');
        getData_search(pdata);
    }
    else {
        alert("Please select Grid. or Exits in Req MT List");
    }

});

$("#save_3").click(function () {
    var i, n, rowData, selRowIds = $("#State").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        $("#State").jqGrid('delRowData', id);
    }
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
$("#clear_Material_List").click(function () { $("#ShipRequest").clearGridData(); });
$("#clear_Req").click(function () { $("#State").clearGridData(); });
$(".save_req").click(function () {
    var selRowIds = $("#State").jqGrid('getRowData');
    if (selRowIds.length == 0) {
        alert("Don't have List on Table Req MT List.");
        return false;
    }
    if ($("#userid").val().trim() == "") {
        alert("Please enter the Requester.");
        $("#userid").val("");
        $('#userid').focus();
        return false;
    }
    if ($("#uname").val().trim() == "") {
        alert("Please enter the Requester =>.");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    }
    if ($("#uname2").val().trim() == "") {
        alert("Please enter the Manager   =>.");
        $("#uname2").val("");
        $('#uname2').focus();
        return false;
    } else {

        var event_tmp = [];
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];

            var rel_bom = $("#c_bom_no").val();
            if (rel_bom == "") {
                rel_bom = $("#bom_fo").val();
            }
            var ret = selRowIds[i];
            var item = {
                mt_no: ret.mt_no,
                mt_no_mx: ret.mt_no_mx,
                worker_id: $('#uname').val(),
                manager_id: $('#userid2').val(),
                req_rec_dt: $('#req_rec_dt').val(),
                rel_bom: rel_bom,
                feed_size: ret.feed_size,
                need_qty: ret.need_qty,
                cav: ret.cav,
                pt_lot: ret.pt_lot,
                use_yn: "Y",
                reg_qty_tr: ret.reg,
                del_yn: "N",
            };
            event_tmp.push(item);
        }
        $.ajax({
            type: 'POST',
            url: '/Shippingmgt/insertw_transport_sp',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(event_tmp),
            traditonal: true,
            success: function (response) {
                alert("Success");
                $("#State").clearGridData();
                jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            },
            error: function (response) {

            }
        });
    }
});
$(document).ready(function () {
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
$("#userid2").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/ShippingMgt/getuserall",
            type: "POST",
            dataType: "json",
            data: { Prefix: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.userid, value: item.userid };
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
        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<label style="width:100%">&nbsp;&nbsp;</label><input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
                $("#new").html(html);
            });
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
        if (data != null && data != undefined) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<label style="width:100%">&nbsp;&nbsp;</label><input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname2">';
                $("#new1").html(html);
            });
        }
    });
};

$("#form1").validate({
    rules: {
        "whouse": {
            required: true,
        },
        "userid": {
            required: true,
        },
        "uname": {
            required: true,
        },
    },
});
$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
$(".PopupCreate").dialog({
    width: '70%',
    height: 600,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 700,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        "ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {



    },
    close: function (event, ui) {
        $('.PopupCreate').dialog('close');
        $("#State").clearGridData();

    },
});
$("#Create").click(function () {
    $('.PopupCreate').dialog('open');
    $("#ShipRequest").clearGridData();
    $('#c_fo_no').val("");
    $('#c_mt_no').val("");
    $('#c_pm_no').val("");
    $('#c_bom_no').val("");
});
$('#close_create').click(function () {
    $('.PopupCreate').dialog('close');
});
function getData_search(pdata) {


    jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

    var params = new Object();
    if (jQuery('#ShipRequest').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    //params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;

    var bom_no = $('#c_bom_no').val();
    var style_no = $('#style_no').val();
    var style_nm = $('#style_nm').val();
    var md_cd = $('#md_cd').val();
    var fo_no = $('#bom_fo').val();

    params.bom_no = bom_no;
    params.fo_no = fo_no;
    params.style_no = style_no;
    params.style_nm = style_nm;
    params.model = md_cd;


    $("#ShipRequest").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/Shippingmgt/Getmt_list_transport",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            //console.log(data);

            if (st == "success") {
                var grid = $("#ShipRequest")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

$grid = $("#check_kq").jqGrid
({
    datatype: 'json',
    mtype: 'Get',
    colModel: [
        { key: true, label: 'trid', name: 'trid', width: 100, align: 'center', hidden: true, },
         { key: false, label: 'TR No', name: 'tr_no', width: 150, align: 'center' },
        { key: false, label: 'BOM', name: 'rel_bom', width: 150, align: 'center' },
        { key: false, label: 'Product Code', name: 'style_no', width: 100, align: 'left' },
        { key: false, label: 'Product Name', name: 'style_nm', width: 100, align: 'left' },
        { key: false, label: 'Model', name: 'md_cd', sortable: true, width: '100' },
        { key: false, label: 'MT No', name: 'mt_no', width: 300, formatter: xoa_phay_page2 },
        { key: false, label: 'Requester', name: 'worker_id', width: 100, },
        { key: false, label: 'Manager ', name: 'manager_id', width: 100, },
        { key: false, label: 'Req Date ', name: 'req_rec_dt', width: 100, },
        { key: false, label: 'Req Qty ', name: 'reg_qty_tr', width: 100, align: 'right', },
        { label: 'Receive', name: 'receive', formatter: 'integer', align: 'right' },
        { key: false, label: 'Debit ', name: 'debit', width: 100, align: 'right', },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    onSelectRow: function (rowid, selected, status, e) {

    },
    pager: '#check_kqPager',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    sortable: true,
    loadonce: true,
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 300,
    width: $(".boxA").width(),
    caption: 'Material Transport Request',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    autowidth: true,
    multiselect: false,
});

$('#check_kq').jqGrid('setGridWidth', $(".boxA").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#check_kq").jqGrid("setGridWidth", newWidth, false);
});