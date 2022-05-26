
//LƯỚI view all
$grid = $("#w_mr").jqGrid({
    url: '/Shippingmgt/mr_info_main',
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
        url: "/Shippingmgt/searchMR_Master_remain",
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

var prevCellVal = { cellId: undefined, value: undefined, mt_no: undefined };
var prevCellVal2 = { cellId2: undefined, value2: undefined, mt_no2: undefined };
var prevCellVal3 = { cellId3: undefined, value3: undefined, mt_no3: undefined };
var prevCellVal4 = { cellId4: undefined, value4: undefined, mt_no4: undefined };
var prevCellVal5 = { cellId5: undefined, value5: undefined, mt_no5: undefined };
var prevCellVal6 = { cellId6: undefined, value6: undefined, mt_no6: undefined };
var prevCellVal7 = { cellId7: undefined, value7: undefined, mt_no7: undefined };
var prevCellVal8 = { cellId8: undefined, value8: undefined, mt_no8: undefined };

$grid = $("#ShipRequest").jqGrid({
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },

        { key: false, label: 'MR NO', name: 'mr_no', width: 150, align: 'center' },
        { key: false, label: 'BOM', name: 'rel_bom', width: 150, align: 'center' },
        { key: false, label: 'Model', name: 'md_cd', sortable: true, width: '200' },
        { key: false, label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
        { key: false, label: 'Product Name', name: 'style_nm', width: 150, align: 'left' },

        { label: 'Status', name: '', width: 50, align: 'center', formatter: btn_check, },
        { label: 'STATUS', name: 'status', width: 150, align: 'left', hidden: true },
        { label: 'MT NO ORIGIN', name: 'mt_no', width: 150, align: 'left', hidden: true },
        { label: 'MT No', name: 'mt_no_r', width: 150, align: 'left' },
        { label: 'MT CD', name: 'mt_cd', width: 150, hidden: true },
        { label: 'Export Actual MT', name: '', width: 150, formatter: selectbox },
        //{
        //    label: '', name: '', width: 150, formatter: function (cellValue) {
        //        return "<span class='mysubgrid fa fa-fw fa-plus'></span>" +
        //            "&nbsp;";
        //    }
        //},
        {
            label: 'Type', name: 'mt_type_nm', width: 150, align: 'center', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal.value == val && rawObject.mt_no == prevCellVal.mt_no) {
                        result = ' style="display: none" rowspanid="' + prevCellVal.cellId + '"';
                    }
                    else {
                        var cellId = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId + '"';
                        prevCellVal = { cellId: cellId, value: val };
                    }
                    prevCellVal.mt_no = rawObject.mt_no
                    return result;
                }
            }
        },
        {
            label: 'Width(mm)', name: 'width', width: 80, align: 'right', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal2.value2 == val && rawObject.mt_no == prevCellVal2.mt_no2) {
                        result = ' style="display: none" rowspanid="' + prevCellVal2.cellId2 + '"';
                    }
                    else {
                        var cellId2 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId2 + '"';
                        prevCellVal2 = { cellId2: cellId2, value2: val };
                    }
                    prevCellVal2.mt_no2 = rawObject.mt_no
                    return result;
                }
            }
        },
        { label: 'Group', name: 'gr_qty', formatter: 'integer', hidden: true },
        {
            label: 'Group Unit(m)', name: 'gr_qty', width: 100, align: 'right', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal3.value3 == val && rawObject.mt_no == prevCellVal3.mt_no3) {
                        result = ' style="display: none" rowspanid="' + prevCellVal3.cellId3 + '"';
                    }
                    else {
                        var cellId3 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId3 + '"';
                        prevCellVal3 = { cellId3: cellId3, value3: val };
                    }
                    prevCellVal3.mt_no3 = rawObject.mt_no
                    return result;
                }
            }
        },
         { label: '', name: 'rong', width: 50,hidden:true },
        { label: 'Total qty', name: 'available1', width: 80, align: 'right' },
        { label: 'Available', name: 'tong_so_roll_tk1', width: 80, align: 'right' },

        { label: 'so_avai_all', name: 'so_avai_all', width: 80, align: 'right', hidden: true },
        { label: 'so_roll_all', name: 'so_roll_all', width: 80, align: 'right', hidden: true },
        { label: 'Total qty ', name: 'available', width: 80, align: 'right', hidden: true },
        { label: 'Available', name: 'tong_so_roll_tk', width: 80, align: 'right', hidden: true },
        {
            label: 'trid', name: 'trid', width: 100, hidden: true
        },
         { label: 'mt_type', name: 'mt_type', hidden: true },
         {
             label: 'Req Bundle Qty', name: 'bundle_qty', width: 110, formatter: 'integer', align: 'right', editable: true, editoptions: {
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

                             ketqua = (ketqua > row.available ? 0 : ketqua);
                             cell.val(ketqua);
                             var wmtid = tim_kiem_id(row.mt_no, "bundle_qty");
                             var row2 = $("#ShipRequest").jqGrid('getRowData', wmtid[0]);
                             var tongkq = wmtid[1];
                             var Total_Roll = wmtid[2];
                             var debit = row.reg_qty_tr - (wmtid[1] + (row2.reg_qty_tr - row2.debit1));
                             $("#ShipRequest").jqGrid("setCell", wmtid[0], "debit", (debit > 0 ? debit : 0));
                             $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Export", tongkq);
                             $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Roll", Total_Roll);

                         }

                     });
                 }
             }, editrules: { integer: true, required: true }, formatter: 'integer',
         },

                  {
                      label: 'Req Qty', name: 'reg', width: 110, formatter: 'integer', align: 'right', editable: true, editoptions: {
                          dataInit: function (element) {
                              $(element).keypress(function (e) {
                                  if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                      return false;
                                  }
                              });
                              $(element).keyup(function (e) {
                                  var id = e.target.id.replace("_reg", "")
                                  var row = $("#ShipRequest").jqGrid('getRowData', id);
                                  var gtri = this.value;
                                  var ketqua = gtri;
                                  var cell = jQuery('#' + id + '_reg');
                                  if (gtri == "") {
                                  } else {

                                      ketqua = (ketqua > row.available ? 0 : ketqua);
                                      cell.val(ketqua);
                                      var wmtid = tim_kiem_id(row.mt_no, "reg");
                                      var tongkq = wmtid[1];
                                      var Total_Roll = wmtid[2];
                                      var row2 = $("#ShipRequest").jqGrid('getRowData', wmtid[0]);
                                      var debit = row.reg_qty_tr - (wmtid[1] + (row2.reg_qty_tr - row2.debit1));
                                      $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Export", tongkq);
                                      $("#ShipRequest").jqGrid("setCell", wmtid[0], "debit", (debit > 0 ? debit : 0));
                                      $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Roll", Total_Roll);

                                  }

                              });
                          }
                      }
                  },
        {
            label: 'Unit', name: 'unit_cd', width: 80, cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal4.value4== val && rawObject.mt_no == prevCellVal4.mt_no4) {
                        result = ' style="display: none" rowspanid="' + prevCellVal4.cellId4 + '"';
                    }
                    else {
                        var cellId4 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId4 + '"';
                        prevCellVal4 = { cellId4: cellId4, value4: val };
                    }
                    prevCellVal4.mt_no4 = rawObject.mt_no
                    return result;
                }
            }
        },
         {
             label: 'Total Roll Qty', name: 'Total_Roll', width: 100, formatter: 'integer', align: 'right', cellattr: function (rowId, val, rawObject, cm, rdata) {
                 var ktra1_dong = $("#do_dai_bang").val();
                 if (ktra1_dong > 1) {
                     var result;

                     if (prevCellVal5.value5 == val && rawObject.mt_no == prevCellVal5.mt_no5) {
                         result = ' style="display: none" rowspanid="' + prevCellVal5.cellId5 + '"';
                     }
                     else {
                         var cellId5 = this.id + '_row_' + rowId + '_' + cm.name;

                         result = ' rowspan="1" id="' + cellId5 + '"';
                         prevCellVal5 = { cellId5: cellId5, value5: val };
                     }
                     prevCellVal5.mt_no5= rawObject.mt_no
                     return result;
                 }
             }
         },
        {
            label: 'Total Export Qty', name: 'Total_Export', width: 150, formatter: 'integer', align: 'right', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal6.value6 == val && rawObject.mt_no == prevCellVal6.mt_no6) {
                        result = ' style="display: none" rowspanid="' + prevCellVal6.cellId6 + '"';
                    }
                    else {
                        var cellId6 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId6 + '"';
                        prevCellVal6 = { cellId6: cellId6, value6: val };
                    }
                    prevCellVal6.mt_no6 = rawObject.mt_no
                    return result;
                }
            }
        },
        {
            label: 'Transport Request', name: 'reg_qty_tr', width: 100, align: 'right', formatter: 'integer', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal7.value7 == val && rawObject.mt_no == prevCellVal7.mt_no7) {
                        result = ' style="display: none" rowspanid="' + prevCellVal7.cellId7 + '"';
                    }
                    else {
                        var cellId7 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId7 + '"';
                        prevCellVal7 = { cellId7: cellId7, value7: val };
                    }
                    prevCellVal7.mt_no7 = rawObject.mt_no
                    return result;
                }
            }
        },
         { label: 'Debit Request', name: 'debit1', width: 100, align: 'right', formatter: 'integer', formatter: 'integer', hidden: true },
        {
            label: 'Debit Request', name: 'debit', width: 100, align: 'right', formatter: 'integer', formatter: 'integer', cellattr: function (rowId, val, rawObject, cm, rdata) {
                var ktra1_dong = $("#do_dai_bang").val();
                if (ktra1_dong > 1) {
                    var result;

                    if (prevCellVal8.value8 == val && rawObject.mt_no == prevCellVal8.mt_no8) {
                        result = ' style="display: none" rowspanid="' + prevCellVal8.cellId8 + '"';
                    }
                    else {
                        var cellId8 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId8 + '"';
                        prevCellVal8 = { cellId8: cellId8, value8: val };
                    }
                    prevCellVal8.mt_no8 = rawObject.mt_no
                    return result;
                }
            }
        },
         { label: 'Destination', name: 'remark', width: 100, },
         { label: 'Status', name: 'status', width: 100, hidden: true },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    beforeSelectRow: function (rowid, e) {
        var $target = $(e.target), $subgridTd;
        if ($target.hasClass("mysubgrid")) {
            $subgridTd = $target.closest("tr.jqgrow>td")
                .siblings("td")
                .filter(".ui-sgcollapsed");

            // simulate opening/closing of the subgrid
            $target.removeClass("fa-minus fa-plus"); // remove both classes
            $target.addClass(
                $subgridTd.hasClass("sgexpanded") ?
                    "fa-plus" :
                    "fa-minus"
            );
            // simulate click on the original subgrid button
            $subgridTd.find(".ui-sghref").click();
        }
        $("table#ShipRequest  tbody tr.ui-subgrid td:nth-child(1)").attr('colspan',7);
        return true; // allow selection of the row
    },
    gridComplete: function () {
        var grid = this;

        $('td[rowspan="1"]', grid).each(function () {
            var spans = $('td[rowspanid="' + this.id + '"]', grid).length + 1;

            if (spans > 1) {
                $(this).attr('rowspan', spans);
            }
        });
        var rows = $("#ShipRequest").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var status = $("#ShipRequest").getCell(rows[i], "status");
            if (status == "001") {
                $("#ShipRequest").jqGrid('setRowData', rows[i], false, { background: '#FFC0CB' });
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
    loadComplete: function () {
        if (iCheckbox === undefined) {
            iCheckbox = getColumnIndexByName($(this), "cb");
        }
        if (iCheckbox >= 0) {
            // if multiselect:true are used, iCheckbox will be 1 in case of rownumbers:true
            // and iCheckbox=0 in case of rownumbers:false

            // first we need to place checkboxes from the table body on the last place
            moveTableColumn(this.rows, iCheckbox, "jqgrow");
        }
    },
    subGrid: true, // set the subGrid property to true to show expand buttons for each row
    subGridRowExpanded: function (subgrid_id, row_id) {
        var subgrid_table_id;
        subgrid_table_id = subgrid_id + "_t";
        //var bom_no = ($("#c_bom_no").val() != "" && $("#c_bom_no").val() != null && $("#c_bom_no").val() != undefined ? $("#c_bom_no").val() : $("#bom_fo").val())
        var rowData = $('#ShipRequest').jqGrid('getRowData', row_id);
        var mt_cd = rowData.mt_cd;
        var mt_no_r = rowData.mt_no_r;
        var mt_no = rowData.mt_no;
        var reg_qty_tr = rowData.reg_qty_tr;
        var mt_type = rowData.mt_type;
        jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");



        jQuery("#" + subgrid_table_id).jqGrid({
            url: "/ShippingMgt/Get_chuoi_request?mt_cd=" + mt_cd + "&mt_no=" + mt_no_r + "&mt_no_cha=" + mt_no + "&reg_qty_tr=" + reg_qty_tr + "&mt_type=" + mt_type,
            datatype: 'json',
            colModel: [
                { label: 'wmtid', name: 'wmtid', hidden: true },
                { label: 'MT NO ORIGIN', name: 'mt_no_cha', width: 150, align: 'left', hidden: true },
                { label: 'MT NO', name: 'mt_no', width: 150, align: 'left' },
                { label: 'gr_qty', name: 'gr_qty', width: 200, align: 'left', hidden: true },
                 { label: '', name: '', width: 480, align: 'center', },
                { label: 'Total qty', name: 'tong', width: 80, align: 'right', formatter: 'integer' },
                { label: 'Available', name: 'dem', width: 80, align: 'right', formatter: 'integer' },
                { label: 'reg_qty_tr', name: 'reg_qty_tr', width: 80, align: 'right', hidden: true },
                {
                    label: 'Req Bundle Qty', name: 'bundle_qty', width: 110, formatter: 'integer', align: 'right', formatter: tinhtoan
                },
                {
                    label: 'Req Qty', name: 'reg', width: 110, align: 'right', formatter: thanhqua
                },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            },
            multiselect: false,
            height: '100%',
            sortname: 'num',
            sortorder: "asc",
            onCellSelect: function (rowid) {
                var lastSel = "";
                if (rowid && rowid !== lastSel) {

                    jQuery("#" + subgrid_table_id).restoreRow(lastSel);
                    lastSel = rowid;
                }
                jQuery("#" + subgrid_table_id).editRow(rowid, true);
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
}).jqGrid("hideCol", "subgrid");

function tinhtoan(cellValue, options, rowdata, action) {
    var gr_qty = rowdata.gr_qty;
    var kq = "<input class='test text-right' type='number' min='0' data-id='" + rowdata.wmtid + "' data-dem='" + rowdata.dem + "' data-mt_no_cha='" + rowdata.mt_no_cha + "'data-reg_qty_tr='" + rowdata.reg_qty_tr + "' id='" + rowdata.wmtid + "_bundle_qty1' role='textbox' onkeypress='return digitKeyOnly(event)' value='0' onkeyup='tinh_toancon(this)' data-gr_qty='" + rowdata.gr_qty + "'>";
    return kq;
}
function thanhqua(cellValue, options, rowdata, action) {
    var gr_qty = rowdata.gr_qty;
    var kq = "<input class='test text-right' type='number' min='0' data-id='" + rowdata.wmtid + "'  data-tong='" + rowdata.tong + "'  data-mt_no_cha='" + rowdata.mt_no_cha + "' data-reg_qty_tr='" + rowdata.reg_qty_tr + "'id='" + rowdata.wmtid + "_reg1' role='textbox' onkeypress='return digitKeyOnly(event)' value='0' onkeyup='ketqacon(this)' data-gr_qty='" + rowdata.gr_qty + "'>";
    return kq;
}
function tinh_toancon(e) {

    var id = $(e).data("id")
    var gr_qty = $(e).data("gr_qty")
    var mt_no_cha = $(e).data("mt_no_cha")
    var reg_qty_tr = $(e).data("reg_qty_tr")
    var dem = $(e).data("dem")

    var gtri = $("#" + id + "_bundle_qty1").val();
    var ketqua = parseInt(gtri) * parseInt(gr_qty);
    var cell = jQuery('#' + id + '_reg1');

    if (gtri == "") {
    } else {
        ketqua = (gtri > dem ? 0 : ketqua);
        cell.val(ketqua);
        var wmtid = tim_kiem_id(mt_no_cha, "bundle_qty");
        var tongkq = wmtid[1];
        var Total_Roll = wmtid[2];
        var row2 = $("#ShipRequest").jqGrid('getRowData', wmtid[0]);
        var debit = reg_qty_tr - (wmtid[1] + (row2.reg_qty_tr - row2.debit1));

        $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Export", tongkq);
        $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Roll", Total_Roll);
        $("#ShipRequest").jqGrid("setCell", wmtid[0], "debit", (debit > 0 ? debit : 0));
    }
}
function ketqacon(e) {

    var id = $(e).data("id")
    var gr_qty = $(e).data("gr_qty")
    var mt_no_cha = $(e).data("mt_no_cha")
    var reg_qty_tr = $(e).data("reg_qty_tr")
    var tong = $(e).data("tong")
    var cell = jQuery('#' + id + '_reg1');
    if (cell.val() > tong) { cell.val("0"); }


    var wmtid = tim_kiem_id(mt_no_cha, "reg");
    var tongkq = wmtid[1];
    var row2 = $("#ShipRequest").jqGrid('getRowData', wmtid[0]);
    var debit = reg_qty_tr - (wmtid[1] + (row2.reg_qty_tr - row2.debit1));
    var Total_Roll = wmtid[2];
    $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Export", tongkq);
    $("#ShipRequest").jqGrid("setCell", wmtid[0], "Total_Roll", Total_Roll);
    $("#ShipRequest").jqGrid("setCell", wmtid[0], "debit", (debit > 0 ? debit : 0));
}
function tim_kiem_id(mt_no, sts) {
    var rows = $("#ShipRequest").getDataIDs();
    var wmtid = 0;
    var sum = 0;
    var Total_Roll = 0;

    for (var i = 0; i < rows.length; i++) {
        var mt_no_jq = $("#ShipRequest").getCell(rows[i], "mt_no");
        var mt_no_r_jq = $("#ShipRequest").getCell(rows[i], "mt_no_r");

        if (mt_no_jq == mt_no) {
            var id = $("#ShipRequest").getCell(rows[i], "wmtid");

            if (id != null && id != undefined && id != "") {
                var cell = (jQuery('#' + id + '_reg').val() != undefined && jQuery('#' + id + '_reg').val() != "" && jQuery('#' + id + '_reg').val() != null ? jQuery('#' + id + '_reg').val() : "0");
                var cell2 = (jQuery('#' + id + '_bundle_qty').val() != undefined && jQuery('#' + id + '_bundle_qty').val() != "" && jQuery('#' + id + '_bundle_qty').val() != null ? jQuery('#' + id + '_bundle_qty').val() : "0");
                //var ktra_sts = (sts == "reg" ? $("#ShipRequest").getCell(rows[i], "available") : $("#ShipRequest").getCell(rows[i], "tong_so_roll_tk"))

                var availbel_cha = $("#ShipRequest").getCell(rows[i], "available");
                var trthai_check = $("#jqg_ShipRequest_" + id + "").prop('checked');
                if ((cell != "0") && (parseInt(cell) <= parseInt(availbel_cha)) && (trthai_check == true)) {
                    sum += parseInt(cell);
                    Total_Roll += parseInt(cell2);
                }
                var selRowIds2 = $("#ShipRequest_" + id + "_t").getDataIDs();
                for (var j = 0; j < selRowIds2.length; j++) {
                    var so = $("#ShipRequest_" + id + "_t").getCell(selRowIds2[j], "wmtid");
                    if ($("#" + so + "_reg1").val() != undefined && $("#" + so + "_reg1").val() != "" && $("#" + so + "_reg1").val() != 0) {
                        var kq_con = $("#" + so + "_reg1").val();
                        var kq_con1 = $("#" + so + "_bundle_qty1").val();
                        sum += parseInt(kq_con);
                        Total_Roll += parseInt(kq_con1);
                    }
                }
            }

        }
        if (mt_no_jq == mt_no_r_jq && mt_no == mt_no_jq) {
            wmtid = $("#ShipRequest").getCell(rows[i], "wmtid");
        }
    }
    var ds = [wmtid, sum, Total_Roll];
    return (ds)
}

function btn_check(cellValue, options, rowdata, action) {

    var mt_qty = rowdata.available;
    var STATUS = rowdata.status;

    if (mt_qty > 0) {
        var html = '<button class="btn btn-xs btn-success" name="finish' + mt_qty + '" data-mt_qty="' + mt_qty + '" ">OK</button><span class="mysubgrid fa fa-fw fa-plus"></span>';
    }
    else {
        var html = '<button class="btn btn-xs btn-warning" name="finish' + mt_qty + '" data-mt_qty="' + mt_qty + '" ">NO</button><span class="mysubgrid fa fa-fw fa-plus"></span>';
    }
    return html;
}

var lastSelection;

function selectbox(cellValue, options, rowdata, action) {
    var html = '<select id="' + rowdata.wmtid + '" onChange="changeBox2(this);">';
    html += '<option value="' + rowdata.wmtid + '_sizebom" >Size BOM</option>';
    html += '<option value="' + rowdata.wmtid + '_othersize" >Other Size</option>';
    html += '</select>';
    return html;
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

function changeBox2(e) {
    var available1 = 0;
    var tong_so_roll_tk1 = 0;
    var id = "";
    if (e.value.match("_sizebom")) {
        id = e.value.replace("_sizebom", "");
        var row = $("#ShipRequest").jqGrid('getRowData', id);
        var available1 = row.available;
        var tong_so_roll_tk1 = row.tong_so_roll_tk;

    }
    if (e.value.match("_othersize")) {
        id = e.value.replace("_othersize", "");
        var row = $("#ShipRequest").jqGrid('getRowData', id);
        var available1 = row.so_avai_all;
        var tong_so_roll_tk1 = row.so_roll_all;
    }
    $("#ShipRequest").jqGrid("setCell", id, "available1", available1);
    $("#ShipRequest").jqGrid("setCell", id, "tong_so_roll_tk1", tong_so_roll_tk1);

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


$("#req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#c_req_date").datepicker({
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
//$("#save_2").click(function () {
//    var fruits = [];
//    var id_fr = [];
//    var bundle = [];
//    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "", bundle_qty = "";

//    if (selRowIds.length > 0) {
//        for (i = 0, n = selRowIds.length; i < n; i++) {
//            id = selRowIds[i];



//            bundle_qty = getCellValue(selRowIds[i], 'bundle_qty');
//            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
//            reg_qty = $("#" + ret.wmtid + "_reg").val();

//            var giatri = reg_qty;
//            var giatri1 = ret.wmtid;
//            var giatri2 = $("#" + ret.wmtid + "_bundle_qty").val();

//            var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);

//            var selRowIds2 = $("#ShipRequest_" + ret.wmtid + "_t").getDataIDs();
//            for (var j = 0; j < selRowIds2.length; j++) {

//                var so = $("#ShipRequest_" + id + "_t").getCell(selRowIds2[j], "wmtid");
//                if ($("#" + so + "_reg1").val() != undefined && $("#" + so + "_reg1").val() != "" && $("#" + so + "_reg1").val() != 0) {

//                    rowData.mt_no_c2 = rowData.mt_no;
//                    rowData.bundle_qty = $("#" + so + "_bundle_qty1").val();
//                    rowData.reg = $("#" + so + "_reg1").val();
//                    rowData.mt_no_r = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "mt_no");
//                    rowData.available = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "tong");
//                    rowData.tong_so_roll_tk = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "dem");

//                    $("#State").jqGrid('delRowData', ret.wmtid);
//                    $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
//                    $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });
//                }
//            }
//            if (reg_qty != "0") {
//                if (giatri != "0") {
//                    console.log(selRowIds2);
//                    if (parseInt(giatri) <= parseInt(ret.available) || parseInt(giatri) == parseInt(ret.available)) {

//                        rowData.reg = giatri;
//                        if (bundle_qty != undefined) {
//                            rowData.bundle_qty = bundle_qty;
//                        } else {
//                            rowData.bundle_qty = giatri2;
//                        }
//                        $("#State").jqGrid('delRowData', ret.wmtid);
//                        $("#State").jqGrid('addRowData', ret.wmtid, rowData, 'last');
//                        $("#State").setRowData(ret.wmtid, false, { background: "#d0e9c6" });
//                    }
//                }
//            }
//        }
//        var grid = $("#ShipRequest");
//        grid.jqGrid('setGridParam', { search: true });
//        var pdata = grid.jqGrid('getGridParam', 'postData');
//        getData_search(pdata);
//    }
//    else {
//        alert("Please select Grid. or Exits in Req MT List");
//    }

//});

//$("#save_3").click(function () {
//    var i, n, rowData, selRowIds = $("#State").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
//    for (i = 0, n = selRowIds.length; i < n; i++) {
//        id = selRowIds[i];
//        $("#State").jqGrid('delRowData', id);
//    }
//});

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

//$(".save_req").click(function () {
//    var selRowIds = $("#State").jqGrid('getRowData');
//    if (selRowIds.length == 0) {
//        alert("Don't have List on Table Req MT List.");
//        return false;
//    }
//    if ($("#whouse").val().trim() == "") {
//        alert("Please enter the Destination.");
//        $("#whouse").val("");
//        $('#whouse').focus();
//        return false;
//    }
//    if ($("#userid").val().trim() == "") {
//        alert("Please enter the Requester.");
//        $("#userid").val("");
//        $('#userid').focus();
//        return false;
//    }
//    if ($("#uname").val().trim() == "") {
//        alert("Please enter the Manager   =>.");
//        $("#uname").val("");
//        $('#uname').focus();
//        return false;
//    } else {
//        var models = $("#State").jqGrid('getRowData');
//        var event_tmp = [];
//        var item = {
//            whouse: $('#whouse').val(),
//            worker: $('#userid').val(),
//            manager: $('#userid2').val(),
//            req_rec_dt: $('#req_rec_dt').val(),
//            rel_bom: $('#rel_bom').val(),
//            remark: $('#remark').val(),
//            dscc: models,
//        };
//        event_tmp.push(item);
//        $.ajax({
//            type: 'POST',
//            url: '/Shippingmgt/insert_mr_info',
//            async: true,
//            dataType: 'json',
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify(event_tmp),
//            traditonal: true,
//            success: function (response) {
//                if (response == false) { alert("Need Pay") } else {
//                    var id = response.mrid;
//                    $("#w_mr").jqGrid('addRowData', id, response, 'first');
//                    $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });
//                    $("#remark").val("");
//                    $("#userid").val("");
//                    $("#userid2").val("");
//                    $("#whouse").val("");
//                    $("#State").clearGridData();
//                    var grid = $("#ShipRequest");
//                    grid.jqGrid('setGridParam', { search: true });
//                    var pdata = grid.jqGrid('getGridParam', 'postData');
//                    getData_search(pdata);
//                    alert("Success");
//                }
//            },
//            error: function (response) {

//            }
//        });
//    }
//});
$("#Pay").click(function () {
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "", bundle_qty = "";
    var models_new = [];
    if (selRowIds.length > 0) {
        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];



            bundle_qty = getCellValue(selRowIds[i], 'bundle_qty');
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            reg_qty = $("#" + ret.wmtid + "_reg").val();

            var giatri = reg_qty;
            var giatri1 = ret.wmtid;
            var giatri2 = $("#" + ret.wmtid + "_bundle_qty").val();



            var selRowIds2 = $("#ShipRequest_" + ret.wmtid + "_t").getDataIDs();
            for (var j = 0; j < selRowIds2.length; j++) {

                var so = $("#ShipRequest_" + id + "_t").getCell(selRowIds2[j], "wmtid");
                if ($("#" + so + "_reg1").val() != undefined && $("#" + so + "_reg1").val() != "" && $("#" + so + "_reg1").val() != 0) {
                    var rowData = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
                    rowData.mt_no_c2 = rowData.mt_no;
                    rowData.bundle_qty = $("#" + so + "_bundle_qty1").val();
                    rowData.reg = $("#" + so + "_reg1").val();
                    rowData.mt_no_r = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "mt_no");
                    rowData.available = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "tong");
                    rowData.tong_so_roll_tk = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "dem");
                    rowData.wmtid = $("#ShipRequest_" + ret.wmtid + "_t").getCell(selRowIds2[j], "wmtid");

                    models_new.push(rowData);
                }
            }
            if (reg_qty != "0") {
                if (giatri != "0") {
                    console.log(selRowIds2);
                    if (parseInt(giatri) <= parseInt(ret.available) || parseInt(giatri) == parseInt(ret.available)) {
                        var rowData1 = $('#ShipRequest').jqGrid('getRowData', ret.wmtid);
                        rowData1.reg = giatri;
                        if (bundle_qty != undefined) {
                            rowData1.bundle_qty = bundle_qty;
                        } else {
                            rowData1.bundle_qty = giatri2;
                        }
                        models_new.push(rowData1)
                    }
                }
            }
        }

    }

    if (models_new.length == 0) {
        alert("Don't have List on Table Material List.");
        return false;
    } else {
        var event_tmp = [];
        var item = {
            whouse: $('#whouse').val(),
            worker: $('#userid').val(),
            manager: $('#userid2').val(),
            req_rec_dt: $('#req_rec_dt').val(),
            rel_bom: $('#rel_bom').val(),
            remark: $('#remark').val(),
            dscc: models_new,
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
                console.log(response);
                for (var i = 0; i < response.length; i++) {

                    $("#w_mr").setRowData(response[i].mrid, false, { background: "#d0e9c6" });


                }

                $("#State").clearGridData();
                var grid = $("#ShipRequest");
                grid.jqGrid('setGridParam', { search: true });
                var pdata = grid.jqGrid('getGridParam', 'postData');
                getData_search(pdata);
                alert("Success");
            },
            error: function (response) {
                alert("error");
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
//$(document).ready(function () {
//    var wage = document.getElementById("userid");
//    wage.addEventListener("keydown", function (e) {
//        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
//            var id = e.target.value;
//            getchar(id);
//        }
//    });


//});

//function getchar(id) {
//    $.get(getadmin + "?id=" + id, function (data) {
//        if (data != null && data != undefined) {
//            $.each(data, function (key, item) {
//                var html = '';
//                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
//                $("#new").html(html);
//            });
//        }
//    });
//}

//$(document).ready(function () {
//    var wage1 = document.getElementById("userid2");
//    wage1.addEventListener("keydown", function (e) {
//        if (e.keyCode === 13) {  //checks whether the pressed key is "Enter"
//            var id = e.target.value;
//            getchar1(id);
//        }
//    });


//});

//function getchar1(id) {
//    $.get(getadmin + "?id=" + id, function (data) {
//        if (data != null && data != undefined) {
//            $.each(data, function (key, item) {
//                var html = '';
//                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname2">';
//                $("#new1").html(html);
//            });
//        }
//    });
//};

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
    var grid = $("#ShipRequest");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getData_search(pdata);

    $('#c_fo_no').val("");
    $('#c_mt_no').val("");
    $('#c_pm_no').val("");
    $('#c_bom_no').val("");
});
$("#search_pp").click(function () {
    var grid = $("#ShipRequest");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getData_search(pdata)
});
$('#close_create').click(function () {
    $('.PopupCreate').dialog('close');
});





function getData_search(pdata) {
    prevCellVal = { cellId: undefined, value: undefined, mt_no: undefined };
    prevCellVal2 = { cellId2: undefined, value2: undefined, mt_no2: undefined };
    prevCellVal3 = { cellId3: undefined, value3: undefined, mt_no3: undefined };
    prevCellVal4 = { cellId4: undefined, value4: undefined, mt_no4: undefined };
    prevCellVal5 = { cellId5: undefined, value5: undefined, mt_no5: undefined };
    prevCellVal6 = { cellId6: undefined, value6: undefined, mt_no6: undefined };
    prevCellVal7 = { cellId7: undefined, value7: undefined, mt_no7: undefined };
    prevCellVal8 = { cellId8: undefined, value8: undefined, mt_no8: undefined };

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

    var bom_no = $('#c_bom_no').val().trim();
    var mt_no = $('#c_mt_no').val().trim();
    var req_date = $('#c_req_date').val().trim();
    params.bom_no = bom_no;
    params.mt_no = mt_no;
    params.req_date = req_date;

    $("#ShipRequest").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/Shippingmgt/Getmt_list_remain",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {


            if (st == "success") {
                $("#do_dai_bang").val(data.rows.length);
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


