
//var prevCellVal = { cellId: undefined, value: undefined, mt_no: undefined };

$grid = $("#check_kq").jqGrid
({
    url: "/Shippingmgt/GetTr_Remain",
    datatype: 'json',
    mtype: 'Get',
    colModel: [
            { key: true, label: 'trid', name: 'trid', width: 100, align: 'center', hidden: true, },
            { key: false, label: 'TR No', name: 'tr_no', width: 150, align: 'center', hidden: true, },
            { key: false, label: 'mt_no_mx', name: 'mt_no_mx', width: 150, align: 'center', hidden: true, },
            { key: false, label: 'BOM', name: 'rel_bom', width: 150, align: 'center',hidden:true },
            { key: false, label: 'BOM', name: '', width: 150, align: 'center', formatter: solan_popup },
            { key: false, label: 'Model', name: 'md_cd', sortable: true, width: '200' },
            { key: false, label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', width: 150, align: 'left' },
            { key: false, label: 'MT No', name: 'mt_no', width: 300, formatter: xoa_phay_page2 },
            { key: false, label: 'MT Real', name: 'mt_tt', width: 300, formatter: xoa_phay_page3 },
            { key: false, label: 'Width', name: 'width', width: 100 },
            { key: false, label: 'Length', name: 'Length', width: 100 },
            { key: false, label: 'Req Qty ', name: 'reg_qty_tr', width: 100, align: 'right', },
            { label: 'Issued Qty', name: 'Issued_qty', align: 'right', formatter: xoa_phay_page },
            { label: 'Receive', name: 'receive', formatter: 'integer', align: 'right' },
            { key: false, label: 'Debit ', name: 'debit', width: 100, align: 'right', },
            { key: false, label: 'Status ', name: 'status', width: 100, align: 'right', formatter: selectbox },
            //{ key: false, label: 'Requester', name: 'worker_id', width: 100, },
            //{ key: false, label: 'Manager ', name: 'manager_id', width: 100, },
            { key: false, label: 'Req Date ', name: 'req_rec_dt', width: 100, },
            { key: false, label: 'Receive Date', name: 'date_receive', width: 100, },
            { key: false, label: 'sts_pay', name: 'sts_pay', },
            {
                key: false, label: 'Description ', name: 'remark', width: 200, align: 'right', editable: true, editoptions: {
                    dataInit: function (element) {
                        $(element).keydown(function (e) {
                            if (e.which == 13) {
                                var selectedRowId = $("#check_kq").jqGrid("getGridParam", 'selrow');
                                var row_id = $("#check_kq").getRowData(selectedRowId);
                                var value = this.value;
                                $.ajax({
                                    url: '/Shippingmgt/update_remark_tr?id=' + row_id.trid + "&value=" + value,
                                    type: "get",
                                    dataType: "json",
                                    success: function (data) {
                                        $("#check_kq").jqGrid("setCell", row_id.trid, "Total_Export", value);
                                        $("#check_kq").jqGrid('setRowData', row_id.trid, false, { background: '#FFC0CB' });
                                    },
                                });
                            }
                        });
                    }
                }, 
            },
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    onSelectRow: function (rowid, selected, status, e) {

    },
    onCellSelect: function (rowid) {
        var lastSel = "";
        if (rowid && rowid !== lastSel) {

            jQuery('#check_kq').restoreRow(lastSel);
            lastSel = rowid;
        }
        jQuery('#check_kq').editRow(rowid, true);
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
    height: 500,
    //subGrid: true, // set the subGrid property to true to show expand buttons for each row
    //subGridRowExpanded: function (subgrid_id, row_id) {
    //    var subgrid_table_id;
    //    subgrid_table_id = subgrid_id + "_t";
    //    var rowData = $('#check_kq').jqGrid('getRowData', row_id);
    //    var mt_no_mx = rowData.mt_no_mx;
    //    var rel_bom = rowData.rel_bom;
    //    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");



    //    jQuery("#" + subgrid_table_id).jqGrid({
    //        url: "/ShippingMgt/GetsolannoRequest?mt_no_mx=" + mt_no_mx + "&rel_bom=" + rel_bom,
    //        datatype: 'json',
    //        colModel: [

    //            { label: 'trid_dt', name: 'trid_dt', hidden: true },
    //             { label: '', name: '', width: 650, align: 'center' },
    //            {
    //                label: 'Date Receive', name: 'date_receive_pay', width: 150, align: 'center', cellattr: function (rowId, val, rawObject, cm, rdata) {
    //                        var result;

    //                        if (prevCellVal.value == val ) {
    //                            result = ' style="display: none" rowspanid="' + prevCellVal.cellId + '"';
    //                        }
    //                        else {
    //                            var cellId = this.id + '_row_' + rowId + '_' + cm.name;

    //                            result = ' rowspan="1" id="' + cellId + '"';
    //                            prevCellVal = { cellId: cellId, value: val };
    //                        }
    //                        return result;
    //                }
    //            },
    //             { label: 'mt_no_mx', name: 'mt_no_mx', width: 200, hidden: true },
    //            { label: 'MT NO', name: 'mt_no', width: 150, align: 'left' },
    //            { label: 'MT Real', name: 'mt_tt', width: 200, align: 'left' },
    //            { label: 'Repay Qty', name: 'repay_qty', width: 80, align: 'right', formatter: 'integer' },

    //        ],
    //        onSelectRow: function (rowid, selected, status, e) {
    //            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
    //        },
    //        multiselect: false,
    //        height: '100%',
    //        sortname: 'num',
    //        sortorder: "asc",
    //        gridComplete: function () {
    //            var grid = this;

    //            $('td[rowspan="1"]', grid).each(function () {
    //                var spans = $('td[rowspanid="' + this.id + '"]', grid).length + 1;

    //                if (spans > 1) {
    //                    $(this).attr('rowspan', spans);
    //                }
    //            });
    //        },
    //        onCellSelect: function (rowid) {
    //            var lastSel = "";
    //            if (rowid && rowid !== lastSel) {

    //                jQuery("#" + subgrid_table_id).restoreRow(lastSel);
    //                lastSel = rowid;
    //            }
    //            jQuery("#" + subgrid_table_id).editRow(rowid, true);
    //        },
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
    width: $(".boxA").width(),
    caption: 'Material List',
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
    var newWidth = $("#check_kq").closest(".ui-jqgrid").parent().width();
    $("#check_kq").jqGrid("setGridWidth", newWidth, false);
});
$(".dialog1").dialog({
    width: '50%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
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
        $('#popupbom').empty();
    },
});


$('#closebom1').click(function () {
    $('.dialog1').dialog('close');
    $('#popupbom').empty();
    //$("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
});

function solan_popup(cellValue, options, rowdata, action) {
    var rel_bom = rowdata.rel_bom;
    //var bom_no = rowobject.bom_no;
    var html = "";
    if (rel_bom != null) {
        a = rel_bom.substr(0, 1);
        b = rel_bom.substr(1, 11);
        d = rel_bom.substr(rel_bom.length - 1, 1);
        c = parseInt(b);
        html = '<button style="color: dodgerblue;border: none;background: none; "  data-rel_bom="' + rowdata.rel_bom + '"  data-mt_no_mx="' + rowdata.mt_no_mx + '" onclick="truyenbien_pp(this);">' + a + c + d + '</button>';
        return html;
    }
    return html;
}
function truyenbien_pp(e) {
    var mt_no_mx = $(e).data("mt_no_mx");
    var rel_bom = $(e).data("rel_bom");
    $('.dialog1').dialog('open');
    var table = $('#popupbom').DataTable({
        "processing": true,
        //"bServerSide": true,
        "ajax": {
            "url": "/ShippingMgt/GetsolannoRequest?mt_no_mx=" + mt_no_mx + "&rel_bom=" + rel_bom,
            "type": "Post",
            "datatype": "json"
        },

        "searching": false,
        "paging": false,
        "bInfo": false,
        "lengthMenu": [50, 100, 200, 500, 1000],
        "columns": [
                 {
                     'className': 'details-control',
                     'orderable': false,
                     'data': null,
                     'defaultContent': ''
                 },

            { "data": "rel_bom" },
            { "data": "date_receive_pay" },
            { "data": "mt_no" },
            { "data": "mt_tt" },
            { "data": "repay_qty", render: $.fn.dataTable.render.number(',', '.', 0, '') },
        ],
        'columnDefs': [
              {
                  "searchable": false,
                  "orderable": false,
                  "targets": 0, // your case first column
                  "className": "text-center",
              },
        ],
        "order": [[2, 'asc']],
        //'rowsGroup': [1],
        'rowsGroup': true,
        "bDestroy": true,
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}
   
function xoa_phay_page2(cellValue, options, rowdata, action) {
    var html = "";
    if (cellValue != null && cellValue != "" && cellValue != undefined) {
        var n = cellValue.indexOf(",");
        if (n != -1) {
            var array = cellValue.split(",")
            array.forEach(function (item, index, array) {
                html += item + "<br>";
            })
            return html;
        } else { return cellValue; }
    }
    return html;
}
function xoa_phay_page(cellValue, options, rowdata, action) {
    var html = "";
    if (cellValue != null && cellValue != "" && cellValue != undefined) {
        var n = cellValue.indexOf(",");
        if (n != -1) {
            var array = cellValue.split(",")
            array.forEach(function (item, index, array) {
                html += item + "<br>";
            })
            return html;
        } else { return cellValue; }
    }
    return html;
}
function xoa_phay_page3(cellValue, options, rowdata, action) {
    var html = "";
    if (cellValue != null && cellValue != "" && cellValue != undefined) {
        var n = cellValue.indexOf(",");
        if (n != -1) {
            var array = cellValue.split(",")
            array.forEach(function (item, index, array) {
                html += item + "<br>";
            })
            return html;
        } else { return cellValue; }
    }
    return html;
}
$("#v_req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#Save_Remain").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchTr_Remain",
        type: "get",
        dataType: "json",
        data: {
            bom_no: $('#v_bom_no').val().trim(),
            style_no: $('#v_style_no').val().trim(),
            style_nm: $('#v_style_nm').val().trim(),
            md_cd: $('#v_md_cd').val().trim(),
            mt_nm: $('#v_mt_nm').val().trim(),
            req_rec_dt: $('#v_req_rec_dt').val().trim(),
        },
        success: function (result) {
            $("#check_kq").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result.rows }).trigger("reloadGrid");
        }
    });
});
function selectbox(cellValue, options, rowdata, action) {
    var html = '<select id="status_' + rowdata.trid + '"  data-id="' + rowdata.trid + '" onChange="changeBox2(this);"  >';
    if (cellValue == "002") {
        html += '<option value="002" >Normal</option>';
        html += '<option value="001" >Need Now</option>'
    }
    if (cellValue == "001") {
        html += '<option value="001" >Need Now</option>'
        html += '<option value="002" >Normal</option>';
    }
    html += '</select>';
 
    return html;
   
}

function changeBox2(e) {    
    var id = $(e).data("id")
    $.ajax({
        url: '/Shippingmgt/update_status_tr?id=' + id + "&value=" + e.value,
        type: "get",
        dataType: "json",
        success: function (data) {

            $("#check_kq").jqGrid("setCell", data.trid, "status", data.status);
            $("#check_kq").jqGrid('setRowData', data.trid, false, { background: '#FFC0CB' });
        },
    });;

}