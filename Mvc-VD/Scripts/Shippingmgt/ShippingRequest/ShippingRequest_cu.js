
//LƯỚI view all
$grid = $("#w_mr").jqGrid({
    url: '/Shippingmgt/mr_info',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'mrid', name: 'mrid', width: 50, hidden: true },
        { label: 'MSR No', name: 'mr_no', width: 110, align: 'center' },
        { label: 'Location', name: 'lct_cd', width: 180, align: 'left' },
        { label: 'Requester', name: 'worker_id', width: 110, align: 'left' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left', },
        { label: 'Req Receive Date', name: 'req_rec_dt', width: 110, align: 'center' },
        { label: 'Real Receive Date', name: 'real_rec_dt', width: 110, align: 'center' },
        { label: 'Mt Qty', name: 'mt_qty', width: 110, align: 'right', },
        { label: 'Related Bom', name: 'rel_bom', width: 110, },
        { label: 'Remark', name: 'remark', width: 110, align: 'left', },
        { key: false, label: 'Create User', name: 'reg_id', },
       { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       { key: false, label: 'Change User', name: 'chg_id', },
       { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#w_mrpage',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 600,
    width: $(".boxC").width() - 5,
    caption: 'MR Master',
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
        var selectedRowId = $("#w_mr").jqGrid("getGridParam", 'selrow');
        row_id = $("#w_mr").getRowData(selectedRowId);
        $("#w_mr_detail").setGridParam({ url: "/Shippingmgt/getmr_detail_sp?" + "mr_no=" + row_id.mr_no, datatype: "json" }).trigger("reloadGrid");


    },
});

$('#w_mr').jqGrid('setGridWidth', $(".boxC").width() + 15);
$(window).on("resize", function () {
    var newWidth = $("#w_mr").closest(".ui-jqgrid").parent().width();
    $("#w_mr").jqGrid("setGridWidth", newWidth, false);
});

$grid = $("#w_mr_detail").jqGrid({
    url: '/Shippingmgt/getmr_detail_sp',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'mrdid', name: 'mrdid', width: 50, hidden: true },
        { label: 'MT No', name: 'mt_no', width: 200, align: 'left' },
        { label: 'MT CD', name: 'mt_cd', width: 200, align: 'left' },
        { label: 'MT Name', name: 'mt_nm', width: 200, align: 'left' },
        { label: 'Group Unit', name: 'gr_qty_with', width: 100, align: 'right' },
        { label: 'Req Qty', name: 'req_qty', width: 110, align: 'right' },
          { label: 'Req Bundle Qty', name: 'req_bundle_qty', width: 80, align: 'right' },
        //{ label: 'Unit', name: 'unit_cd', width: 60, align: 'right' },
        { label: 'Width(mm)', name: 'new_with', width: 110, align: 'right' },
        { label: 'Length(mm)', name: 'new_spec', width: 110, align: 'right' },
        { label: 'Area(m²)', name: 'area_all', width: 110, align: 'right' },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#w_mrdetailpage',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 600,
    width: $(".boxC").width() - 5,
    caption: 'MR Detail',
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
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        $("#del_save_but").removeClass("disabled");
        jQuery("#ShipRequest").setGridParam({ datatype: "json" }).trigger('reloadGrid');

    },
});

$('#w_mr_detail').jqGrid('setGridWidth', $(".boxC").width() + 15);
$(window).on("resize", function () {
    var newWidth = $("#w_mr_detail").closest(".ui-jqgrid").parent().width();
    $("#w_mr_detail").jqGrid("setGridWidth", newWidth, false);
});

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});

$("#searchBtn").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchMR_Master",
        type: "get",
        dataType: "json",
        data: {
            mr_no: $('#s_mr_no').val().trim(),
            bom_no: $('#rel_bom').val().trim(),
            start: $('#start').val().trim(),
            end: $('#end').val().trim(),
        },
        success: function (result) {
            $("#w_mr").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});


//popup

$grid = $("#ShipRequest").jqGrid({
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { label: 'Status', name: '', width: 50, align: 'center', formatter: btn_check },
        { label: 'STATUS', name: 'status', width: 150, align: 'left', hidden: true },
        { label: 'MT No', name: 'mt_no_r', width: 150, align: 'left', formatter: xoa_phay },
        { label: 'MT No', name: 'mt_no', width: 150, align: 'left', hidden: true },
        { label: 'Export Actual', name: 'mt_no_r', width: 150, align: 'left' },
        { label: 'Type', name: 'mt_type', width: 80, align: 'center' , formatter: Xuatlieu_thucte },
        { label: 'Width(mm)', name: 'width', width: 80, align: 'right' },
        { label: 'Group', name: 'gr_qty', formatter: 'integer', hidden: true },
        { label: 'Group Unit', name: 'gr_qty', width: 100, align: 'right' },
        //{ label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
         { label: 'Total qty ', name: 'Available', width: 80, align: 'right', formatter: congAvailable },
          { label: 'sum_vlinkho', name: 'sum_av', width: 80, align: 'right', hidden: true },
          { label: 'so_roll_tt', name: 'so_roll_tt', width: 80, align: 'right', hidden: true },
         { label: 'Available', name: 'so_roll', width: 80, align: 'right', formatter: congAvailable1 },
       
        { label: 'Transport Request', name: 'reg_qty_tr', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Debit Request', name: 'debit', width: 100, align: 'right', formatter: 'integer' },
        { label: 'trid', name: 'trid', width: 100, hidden: true },
        { label: 'bdidr', name: 'bdidr', width: 100, hidden: true },
        { label: 'Req Bundle Qty', name: 'bundle_qty', width: 100, align: 'left', formatter: tinhtoan },
        { label: 'Req Qty', name: 'reg', width: 100, align: 'left', formatter: thanhqua },
        {
            label: 'Req Bundle Qty', name: 'bundle_qty', width: 110,  formatter: 'integer' ,align: 'right', editable: true, editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                    $(element).keyup(function (e) {
                        var id = e.target.id.replace("_bundle_qty", "")
                        var row = $("#ShipRequest").jqGrid('getRowData', id);
                        var gtri = this.value;
                        var ketqua = parseInt(gtri) * parseInt(row.gr_qty);
                        var cell = jQuery('#' + id + '_reg');
                        if (gtri == "") {
                        } else {
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
            }, editrules: { integer: true, required: true }, formatter: 'integer',
        },
        //{
        //    label: 'Req Qty', name: 'reg', width: 110, align: 'right', editable: true, editoptions: {
        //        dataInit: function (element) {

        //            $(element).keypress(function (e) {
        //                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //                    return false;
        //                }
        //            });

        //        }
        //    }, editrules: { integer: true, required: true },
        //},

        { label: 'Unit', name: 'unit_cd', width: 80 },
        { label: 'Total Export Qty', name: 'Total_Export', width: 150, formatter: 'integer', align: 'right', },
        //{ label: 'Length(mm)', name: 'spec', width: 80, align: 'right' },
        //{ label: 'Area(m²)', name: 'area', width: 80, align: 'right' },
         { label: 'MT Name', name: 'mt_nm', width: 110, align: 'left', hidden: true },
         { label: 'so_luong_tam', name: 'so_luong_tg', width: 110, align: 'left', hidden: true },
         { label: 'mt_no_tg', name: 'mt_no_tg', width: 110, align: 'left', hidden: true },
         { label: 'buddle_tg', name: 'buddle_tg', width: 110, align: 'left', hidden: true },
         { label: 'MT No', name: 'count_add', width: 200, align: 'left', formatter: count_add, hidden: true },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    gridComplete: function () {
        var rows = $("#ShipRequest").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var debit = $("#ShipRequest").getCell(rows[i], "debit");
            var giatri = ((debit != null && debit != "" && debit != undefined) ? debit : 0)
            if (giatri > 0) {
                $("#ShipRequest").jqGrid('setRowData', rows[i], false, { background: 'red' });
            }
        }
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
    subGrid: true, // set the subGrid property to true to show expand buttons for each row
    subGridRowExpanded: function (subgrid_id, row_id) {
        var subgrid_table_id;
        subgrid_table_id = subgrid_id + "_t";
        var bom_no = ($("#c_bom_no").val() != "" && $("#c_bom_no").val() != null && $("#c_bom_no").val() != undefined ? $("#c_bom_no").val() : $("#bom_fo").val())
        var rowData1 = $('#ShipRequest').jqGrid('getRowData', row_id);
        var gitri = rowData1.count_add;

        jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
        jQuery("#" + subgrid_table_id).jqGrid({
            url: "/ShippingMgt/Get_chuoi_request?chuoi_mt_no=" + gitri + "&bom_no=" + bom_no,
            datatype: 'json',
            colModel: [
                  { label: 'MT No', name: 'mt_no', width: 200, align: 'left', },
                  { label: 'Total Qty(m)', name: 'gr_qty', width: 200, align: 'right', formatter: 'integer' },
                  { label: 'Available MT All(Roll)', name: 'Available', width: 200, align: 'right', formatter: 'integer' },
                  { label: 'Available MT(Roll)', name: 'Available_cha', width: 200, align: 'right', formatter: 'integer' },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            },
            height: '100%',
            sortname: 'num',
            sortorder: "asc",
            jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        });
    },

    viewrecords: true,
    height: 313,
    width: null,
    multiPageSelection: true,
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
});

function btn_check(cellValue, options, rowdata, action) {

    var mt_qty = rowdata.Available;
    var STATUS = rowdata.status;

    if (mt_qty > 0) {
        var html = '<button class="btn btn-xs btn-success" name="finish' + mt_qty + '" data-mt_qty="' + mt_qty + '" ">OK</button>';
    }
    else {
        var html = '<button class="btn btn-xs btn-warning" name="finish' + mt_qty + '" data-mt_qty="' + mt_qty + '" ">NO</button>';
    }
    return html;
}


var lastSelection;

function count_add(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = rowdata.mt_no + "," + rowdata.mt_no_r
        return html;
    }
    return kq;
}
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

function congAvailable(cellValue, options, rowdata, action) {
    var sumav = ((rowdata.sum_av != null && rowdata.sum_av != "" && rowdata.sum_av != undefined) ? rowdata.sum_av : 0);
    var Available = ((rowdata.Available != null && rowdata.Available != "" && rowdata.Available != undefined) ? rowdata.Available : 0);
    var kq = Available + sumav;
    return kq;
}
function congAvailable1(cellValue, options, rowdata, action) {
    var so_roll = ((rowdata.so_roll != null && rowdata.so_roll != "" && rowdata.so_roll != undefined) ? rowdata.so_roll : 0);
    var so_roll_tt = ((rowdata.so_roll_tt != null && rowdata.so_roll_tt != "" && rowdata.so_roll_tt != undefined) ? rowdata.so_roll_tt : 0);
    var kq = so_roll + so_roll_tt;
    return kq;
}
function thanhqua(cellValue, options, rowdata, action) {
    var kq = "<input class='test text-right' type='number' data-id='" + rowdata.wmtid + "' id='reg_text_" + rowdata.wmtid + "'min='0' role='textbox' value='0' onkeyup='ketqacon(this)'  data-name_input='" + rowdata.wmtid + "'>";
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r.split(",")
        var array1 = rowdata.bdidr.split(",")
        array.forEach(function (item, index, array) {
            html += "<input class='test text-right' type='number' id='reg_text_" + array1[index] + "' data-id='" + rowdata.wmtid + "'  min='0' data-name_input='" + array1[index] + "'  value='0'  role='textbox' onkeyup='ketqacon(this)'  >  <br>";
        })
        gitri = "<input class='test text-right' type='number' id='reg_text_" + rowdata.wmtid + "' min='0' data-id='" + rowdata.wmtid + "'  role='textbox' value='0' onkeyup='ketqacon(this)' data-name_input='" + rowdata.wmtid + "' ><br>" + html;
        return gitri;
    }
    return kq;
}
function ketqacon(e) {
    var name_id = $(e).data("name_input")

    var name_input = "text_" + $(e).data("name_input");
    var name2_input = "reg_text_" + $(e).data("name_input");
    var name3_input = "check_" + $(e).data("name_input");
    var ketqua = $("#" + name2_input).val();
    ketqua = (parseInt(ketqua != "" && ketqua != "underfined" && ketqua != null ? ketqua : 0));

    if (ketqua < 0) {
        $("#" + name2_input).val(0);
        return false;
    }

    // add vao giatri
    var id = $(e).data("id")

    var value1 = $("#" + name3_input).val();

    var gtri = ketqua;
    var gtri_buddle = $("#" + name_input).val();

    var rowData = $('#ShipRequest').jqGrid('getRowData', id);
    var kq = "";
    var kq2 = "";
    var kq3 = "";
    var kq4 = 0;
    //tim kiếm vị trí của mt trong mảng

    var mang = rowData.mt_no_tg.split(",").indexOf(value1)
    var mang2 = rowData.mt_no_r.indexOf("<br>")
    if (mang2 == -1) { kq4 = ketqua; }
    if ($("#" + name3_input).prop('checked') == true) {
        if (mang != -1) {
            var array = rowData.mt_no_tg.split(",");
            var array2 = rowData.so_luong_tg.split(",");
            var array3 = rowData.buddle_tg.split(",");

            array.forEach(function (item, index, array) {
                if (index == mang) {
                    kq += gtri + ",";
                    kq3 += gtri_buddle + ",";
                    kq4 += ketqua;
                }
                else {
                    kq += array2[index] + ",";
                    kq3 += array3[index] + ",";
                    kq4 += parseInt(array2[index]);
                }
               
            })
            kq = kq.substr(0, kq.length - 1);
            kq3 = kq3.substr(0, kq3.length - 1);
        }

    }

    $("#ShipRequest").jqGrid("setCell", id, "Total_Export", kq4);
    $("#ShipRequest").jqGrid("setCell", id, "so_luong_tg", kq);
    $("#ShipRequest").jqGrid("setCell", id, "buddle_tg", kq3);
}


function tinhtoan(cellValue, options, rowdata, action) {
    var gr_qty = rowdata.gr_qty;
    var kq = "<input class='test text-right' type='number' min='0' data-id='" + rowdata.wmtid + "' id='text_" + rowdata.wmtid + "' role='textbox' onkeypress='return digitKeyOnly(event)' value='0' onkeyup='tinh_toancon(this)' data-gr_qty='" + gr_qty + "'data-name_input='" + rowdata.wmtid + "'>";
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r.split(",")
        var array1 = rowdata.bdidr.split(",")
        array.forEach(function (item, index, array) {
            html += "<input class='test text-right' type='number' min='0' data-id='" + rowdata.wmtid + "'  id='text_" + array1[index] + "'  onkeypress='return digitKeyOnly(event)' value='0'  data-gr_qty='" + gr_qty + "' data-name_input='" + array1[index] + "' role='textbox' onkeyup='tinh_toancon(this)'>  <br>";
        })
        gitri = "<input class='test text-right' type='number' min='0' data-id='" + rowdata.wmtid + "'  id='text_" + rowdata.wmtid + "'  onkeypress='return digitKeyOnly(event)' role='textbox' value='0' data-gr_qty='" + gr_qty + "'data-name_input='" + rowdata.wmtid + "' onkeyup='tinh_toancon(this)'><br>" + html;
        return gitri;
    }
    return kq;
}
function tinh_toancon(e) {
    var name_id = $(e).data("name_input")

    var name_input = "text_" + $(e).data("name_input");
    var name2_input = "reg_text_" + $(e).data("name_input");
    var name3_input = "check_" + $(e).data("name_input");
    var gr_qty = $(e).data("gr_qty")
    var value = $("#" + name_input).val();
    var ketqua = parseInt(value) * parseInt(gr_qty);
    $("#" + name2_input).val(ketqua);



    // add vao giatri
    var id = $(e).data("id")

    var value1 = $("#" + name3_input).val();

    var gtri = ketqua;
    var gtri_buddle = $("#" + name_input).val();

    var rowData = $('#ShipRequest').jqGrid('getRowData', id);
    var kq = "";
    var kq2 = "";
    var kq3 = "";
    var kq4 = 0;
    //tim kiếm vị trí của mt trong mảng

    var mang = rowData.mt_no_tg.split(",").indexOf(value1)
    var mang2 = rowData.mt_no_r.indexOf("<br>")
    if (mang2 == -1) { kq4 = ketqua; }
    
    if ($("#" + name3_input).prop('checked') == true) {
        if (mang != -1) {
            var array = rowData.mt_no_tg.split(",");
            var array2 = rowData.so_luong_tg.split(",");
            var array3 = rowData.buddle_tg.split(",");

            array.forEach(function (item, index, array) {
                if (index == mang) {
                    kq += gtri + ",";
                    kq3 += gtri_buddle + ",";
                    kq4 += ketqua;
                }
                else {
                    kq += array2[index] + ",";
                    kq3 += array3[index] + ",";
                    kq4 += parseInt(array2[index]);
                }
              
            })
            kq = kq.substr(0, kq.length - 1);
            kq3 = kq3.substr(0, kq3.length - 1);
        }

    }

    $("#ShipRequest").jqGrid("setCell", id, "Total_Export", kq4);
    $("#ShipRequest").jqGrid("setCell", id, "so_luong_tg", kq);
    $("#ShipRequest").jqGrid("setCell", id, "buddle_tg", kq3);
}

function xoa_phay(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r.split(",")
        var array1 = rowdata.bdidr.split(",")
        array.forEach(function (item, index, array) {
            html += "<input type='checkbox' id='check_" + array1[index] + "' value=" + item + "  data-id='" + rowdata.wmtid + "' data-buddle='text_" + array1[index] + "' data-phu='check_" + array1[index] + "' data-name_input='reg_text_" + array1[index] + "' data-value='" + item + "' onclick='chane_data(this)'> " + item + "<br>";
        })
        gitri = "<input type='checkbox'  id='check_" + rowdata.wmtid + "' value=" + rowdata.mt_no + "  data-id='" + rowdata.wmtid + "' data-buddle='text_" + rowdata.wmtid + "' data-phu='check_" + rowdata.wmtid + "' data-name_input='reg_text_" + rowdata.wmtid + "' data-value='" + rowdata.mt_no + "' onclick='chane_data(this)'>" + rowdata.mt_no + "<br>" + html;
        return gitri;
    }
    return kq;
}
function Xuatlieu_thucte(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_r;
    if (gitri != null) {
        var html = "";
        var array = rowdata.mt_no_r.split(",")
        var array1 = rowdata.bdidr.split(",")
        array.forEach(function (item, index, array) {
            html += "<input type='checkbox' id='check_" + array1[index] + "' value=" + item + "  data-id='" + rowdata.wmtid + "' data-buddle='text_" + array1[index] + "' data-phu='check_" + array1[index] + "' data-name_input='reg_text_" + array1[index] + "' data-value='" + item + "' onclick='chane_data(this)'> " + item + "<br>";
        })
        gitri = "<input type='checkbox'  id='check_" + rowdata.wmtid + "' value=" + rowdata.mt_no + "  data-id='" + rowdata.wmtid + "' data-buddle='text_" + rowdata.wmtid + "' data-phu='check_" + rowdata.wmtid + "' data-name_input='reg_text_" + rowdata.wmtid + "' data-value='" + rowdata.mt_no + "' onclick='chane_data(this)'>" + rowdata.mt_no + "<br>" + html;
        return gitri;
    }
    return kq;
}
function chane_data(e) {

    var id = $(e).data("id")
    var name_input = $(e).data("name_input")
    var value = $(e).data("value")
    var phu = $(e).data("phu")
    var buddle = $(e).data("buddle")
    var gtri = $("#" + name_input).val();
    var gtri_buddle = $("#" + buddle).val();

    var rowData = $('#ShipRequest').jqGrid('getRowData', id);
    var kq = "";
    var kq2 = "";
    var kq3 = "";
    var kq4 = "";
    //tim kiếm vị trí của mt trong mảng

    var mang = rowData.mt_no_tg.split(",").indexOf(value)
    var mang2 = rowData.mt_no_r.indexOf("<br>")
    if (mang2 == -1) { kq4 = ketqua; }
    if ($("#" + phu).prop('checked') == true) {
        if (rowData.so_luong_tg == "") { kq = gtri; } else { kq = rowData.so_luong_tg + "," + gtri; }
        if (rowData.mt_no_tg == "") { kq2 = value; } else { kq2 = rowData.mt_no_tg + "," + value; }
        if (rowData.buddle_tg == "") { kq3 = gtri_buddle; } else { kq3 = rowData.buddle_tg + "," + gtri_buddle; }
        kq4 = parseInt(rowData.Total_Export) + parseInt(gtri);

    } else {

        if (mang != -1) {
            var array = rowData.mt_no_tg.split(",");
            var array2 = rowData.so_luong_tg.split(",");
            var array3 = rowData.buddle_tg.split(",");

            array.forEach(function (item, index, array) {
                if (index != mang) {
                    kq += array2[index] + ",";
                    kq2 += item + ",";
                    kq3 += array3[index] + ",";
                    kq4 = parseInt(rowData.Total_Export) + parseInt(gtri);
                }
            })
            kq = kq.substr(0, kq.length - 1);
            kq2 = kq2.substr(0, kq2.length - 1);
            kq3 = kq3.substr(0, kq3.length - 1);
        }
    }

    $("#ShipRequest").jqGrid("setCell", id, "so_luong_tg", kq);
    $("#ShipRequest").jqGrid("setCell", id, "mt_no_tg", kq2);
    $("#ShipRequest").jqGrid("setCell", id, "buddle_tg", kq3);
    $("#ShipRequest").jqGrid("setCell", id, "Total_Export", kq4);
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
        { label: 'MT No', name: 'mt_no', width: 110, align: 'left', hidden: true },
         { label: 'MT No', name: 'view', width: 150, align: 'left', formatter: xoa },
        //{ label: 'MT Cd', name: 'mt_cd', width: 150, align: 'left' },
        { label: 'Type', name: 'mt_type', width: 80, align: 'left' },
        { label: 'Group Unit', name: 'gr_qty', width: 100, align: 'right' },
        { label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
         { label: 'Total qty ', name: 'Available', width: 80, align: 'right'},
         { label: 'Available', name: 'so_roll', width: 80, align: 'right', },
        { label: 'Transport Request', name: 'reg_qty_tr', width: 100, align: 'right', formatter: 'integer' },
          { label: 'trid', name: 'trid', width: 100, hidden: true },
        { label: 'Req Bundle Qty', name: 'bundle_qty', width: 80, align: 'right', formatter: 'integer', hidden: true },
        { label: 'Reg qty', name: 'reg', width: 110, align: 'right', formatter: 'integer', hidden: true },
        { label: 'Reg Bundle qty', name: 'view_budlel', width: 110, align: 'right', formatter: 'integer', formatter: xoa2 },
        { label: 'Reg qty', name: 'view_reg', width: 110, align: 'right', formatter: 'integer', formatter: xoa3 },
        { label: 'Unit', name: 'unit_cd', width: 80 },
          { label: 'Total Export Qty', name: 'Total_Export', width: 150, formatter: 'integer', align: 'right', },
        { label: 'Width(mm)', name: 'width', width: 80, align: 'right', hidden: true },
        { label: 'MT Name', name: 'mt_nm', width: 110, align: 'left', hidden: true },
        { label: 'so_luong_tg', name: 'so_luong_tg', width: 110, align: 'left', hidden: true },
        { label: 'mt_no_tg', name: 'mt_no_tg', width: 110, align: 'left', hidden: true },
          { label: 'buddle_tg', name: 'buddle_tg', width: 110, align: 'left', hidden: true },
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
            url: "/Shippingmgt/Automt_no",
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
            url: "/Shippingmgt/Autofo_no",
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
function xoa(cellValue, options, rowdata, action) {
    var kq = rowdata.mt_no;
    var gitri = rowdata.mt_no_tg;
    if (gitri != null && gitri != "") {
        var html = "";
        var array = rowdata.mt_no_tg.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = html;
        return gitri;
    }
    return kq;
}
function xoa2(cellValue, options, rowdata, action) {
    var kq = rowdata.bundle_qty;
    var gitri = rowdata.buddle_tg;
    if (gitri != null && gitri != "") {
        var html = "";
        var array = rowdata.buddle_tg.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = html;
        return gitri;
    }
    return kq;
}
function xoa3(cellValue, options, rowdata, action) {
    var kq = rowdata.reg;
    var gitri = rowdata.so_luong_tg;
    if (gitri != null && gitri != "") {
        var html = "";
        var array = rowdata.so_luong_tg.split(",")
        array.forEach(function (item, index, array) {
            html += item + "<br>";
        })
        gitri = html;
        return gitri;
    }
    return kq;
}
$("#save_2").click(function () {
    var fruits = [];
    var id_fr = [];
    var bundle = [];
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "", bundle_qty = "";

    if (selRowIds.length > 0) {
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];



            bundle_qty = getCellValue(selRowIds[i], 'bundle_qty');
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            reg_qty = $("#" + "reg_text_" + ret.wmtid).val();
            if (reg_qty != "0") {
                var giatri = reg_qty;
                var giatri1 = ret.wmtid;
                var giatri2 = $("#" + "text_" + ret.wmtid).val();
                if (giatri != "0") {
                    if (parseInt(giatri) <= parseInt(ret.Available) || parseInt(giatri) == parseInt(ret.Available)) {
                        var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
                        rowData.reg = giatri;
                        if (bundle_qty != undefined) {
                            rowData.bundle_qty = bundle_qty;
                        } else {
                            rowData.bundle_qty = giatri2;
                        }
                        $("#State").jqGrid('delRowData', ret.wmtid);
                        $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
                        $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });
                    }
                }
            }
            //else {
            //    if (reg_qty != "0" && reg_qty != "" ) {
            //        var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
            //        rowData.reg = reg_qty;
            //        if (parseInt(rowData.reg) < parseInt(rowData.Available) || parseInt(rowData.reg) == parseInt(rowData.Available)) {
            //            if (bundle_qty != undefined) {
            //                rowData.bundle_qty = bundle_qty;
            //            } else {
            //                rowData.bundle_qty = giatri2;
            //            }
            //            $("#State").jqGrid('delRowData', ret.wmtid);
            //            $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
            //            $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });
            //        }
            //    }
            //}
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
    if ($("#whouse").val().trim() == "") {
        alert("Please enter the Destination.");
        $("#whouse").val("");
        $('#whouse').focus();
        return false;
    }
    if ($("#userid").val().trim() == "") {
        alert("Please enter the Requester.");
        $("#userid").val("");
        $('#userid').focus();
        return false;
    }
    if ($("#uname").val().trim() == "") {
        alert("Please enter the Manager   =>.");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    } else {
        var models = $("#State").jqGrid('getRowData');
        var event_tmp = [];
        var item = {
            whouse: $('#whouse').val(),
            worker: $('#userid').val(),
            manager: $('#userid2').val(),
            req_rec_dt: $('#req_rec_dt').val(),
            rel_bom: $('#rel_bom').val(),
            remark: $('#remark').val(),
            dscc: models,
        };
        event_tmp.push(item);
        $.ajax({
            type: 'POST',
            url: '/Shippingmgt/insert_mr_info',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(event_tmp),
            traditonal: true,
            success: function (response) {
                var id = response.mrid;
                $("#w_mr").jqGrid('addRowData', id, response, 'first');
                $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });
                $("#remark").val("");
                $("#userid").val("");
                $("#userid2").val("");
                $("#whouse").val("");
                $("#State").clearGridData();
                jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            },
            error: function (response) {

            }
        });
    }
});
$("#Pay").click(function () {
    var models = $("#State").jqGrid('getRowData');
    if (models.length == 0) {
        alert("Don't have List on Table Req MT List.");
        return false;
    }
    if ($("#whouse").val().trim() == "") {
        alert("Please enter the Destination.");
        $("#whouse").val("");
        $('#whouse').focus();
        return false;
    }
    if ($("#userid").val().trim() == "") {
        alert("Please enter the Requester.");
        $("#userid").val("");
        $('#userid').focus();
        return false;
    }
    if ($("#uname").val().trim() == "") {
        alert("Please enter the Manager   =>.");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    } else {
        var models = $("#State").jqGrid('getRowData');
        var event_tmp = [];
        var item = {
            whouse: $('#whouse').val(),
            worker: $('#userid').val(),
            manager: $('#userid2').val(),
            req_rec_dt: $('#req_rec_dt').val(),
            rel_bom: $('#rel_bom').val(),
            remark: $('#remark').val(),
            dscc: models,
        };
        event_tmp.push(item);
        $.ajax({
            type: 'POST',
            url: '/Shippingmgt/Pay_mt_replace',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(event_tmp),
            traditonal: true,
            success: function (response) {
                var id = response.mrid;
                $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });
                $("#remark").val("");
                $("#userid").val("");
                $("#userid2").val("");
                $("#whouse").val("");
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
                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
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
                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname2">';
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
    width: '80%',
    height: 800,
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
    var fo_no = $('#bom_fo').val();
    var mt_no = $('#c_mt_no').val();
    var pm_no = $('#c_pm_no').val();
    params.bom_no = bom_no;
    params.fo_no = fo_no;
    params.mt_no = mt_no;
    params.pm_no = pm_no;

    $("#ShipRequest").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/Shippingmgt/Getmt_list",
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
function digitKeyOnly(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var value = Number(e.target.value + e.key) || 0;

    if ((keyCode >= 37 && keyCode <= 40) || (keyCode == 8 || keyCode == 9 || keyCode == 13) || (keyCode >= 48 && keyCode <= 57)) {
        return isValidNumber(value);
    }
    return false;
}

function isValidNumber(number) {
    return (number > 0)
}
