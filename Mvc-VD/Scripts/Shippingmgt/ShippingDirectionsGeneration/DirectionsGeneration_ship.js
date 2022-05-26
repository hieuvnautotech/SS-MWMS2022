var ds_wmtid = "";
$(function () {
    $("#list").jqGrid({
        url: "/ShippingMgt/Get_list_Generation",
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'mrdid', name: 'mrdid', width: 50, hidden: true },
            { label: 'MT No', name: 'mt_no', width: 180, align: 'left', formatter: pp_mt_no },
            { label: 'MT CD', name: 'mt_cd', width: 180, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 180, align: 'left' },
            { label: 'Type', name: 'mt_type', width: 50, align: 'center' },
            { label: 'Group Unit', name: 'group_unit', width: 70, align: 'right' },
            { label: 'Group Unit', name: 'gr_qty', width: 70, align: 'right', formatter: 'integer', hidden: true },
            { label: 'Available', name: 'available', width: 60, align: 'right', formatter: 'integer' },
            { label: 'Req Bundle Qty', name: 'req_bundle_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Req qty', name: 'req_qty', width: 60, align: 'right', formatter: 'integer' },
               //{
               //    label: 'Send Bundle Qty', name: 'send_bundle_qty', width: 100, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
               //    formatter: 'integer',

               //},
               {
                   label: 'Send Bundle Qty', name: 'send_bundle_qty', width: 110, align: 'right', editable: false, editoptions: {
                       dataInit: function (element) {
                           $(element).keypress(function (e) {
                               if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                   return false;
                               }
                           });

                           $(element).keyup(function (e) {

                               var id = e.target.id.replace("_send_bundle_qty", "")
                               var row = $("#list").jqGrid('getRowData', id);
                               var gtri = this.value;
                               var ketqua = parseInt(gtri) * parseInt(row.gr_qty);
                               var cell = jQuery('#' + id + '_send_qty');
                               if (gtri == "") {
                                   cell.val(0);
                               } else {
                                   //ketqua = ketqua.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                                   cell.val(ketqua);
                               }

                               if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                   return false;
                               }
                           });
                           $(element).keydown(function (e) {

                               var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                               var row_id = $("#list").getRowData(selectedRowId);
                               if (e.which == 13) {
                                   var ketqua = parseInt(this.value) * parseInt(row_id.gr_qty);

                               }
                           });
                       }
                   }, editrules: { integer: true, required: true }, formatter: 'integer',
               },

            {
                label: 'Send Qty', name: 'send_qty', width: 80, align: 'right', editable: false, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                formatter: 'integer',
            },
            { label: 'Real Bundle Qty', name: 'real_bundle_qty', width: 110, align: 'right', formatter: 'integer' },
            { label: 'Real Qty', name: 'real_qty', width: 80, align: 'right', formatter: 'integer' },
            { label: 'Width(mm)', name: 'width', width: 80, align: 'right' },
            { label: 'Length(m)', name: 'spec', width: 80, align: 'right' },
            { label: 'area(m2)', name: 'area', width: 80, align: 'right' },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },

        loadonce: true,
        shrinkToFit: false,
        pager: '#gridpager',
        rowNum: 50,

        rownumbers: true,
        multiselect: true,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        height: 235,
        multiboxonly: true,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        width: null,

        onSelectRow: function (id, rowid, status, e) {
            //$("#Direction").removeAttr("disabled");
            if (id) {
                jQuery('#list').jqGrid('editRow', id, true);
                //var gr_qty = $("#list").getCell(id, "gr_qty");
                //console.log(gr_qty);
            }
            //jQuery(this).editRow(id, {
            //    keys: true,
            //    onEnter: function (rowid, options, event) {
            //        var gr_qty = event.gr_qty.value;
            //        alert(gr_qty);
            //    }
            //});
        },
        subGrid: true,
        subGridRowExpanded: function (subgrid_id, row_id) {
            var subgrid_table_id;

            subgrid_table_id = subgrid_id + "_t";
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table> </div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/ShippingMgt/Getds_W_material222?id=" + row_id,
                datatype: 'json',
                colModel: [
                 { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
                    { label: "ML NO", name: "mt_cd", width: 400, align: "left" },
                    { label: "Group Qty", name: "gr_qty", width: 110, align: "right" },
                    { label: 'Delete', name: '', width: 100, align: 'center', formatter: btn_delete },

                ],
                height: '100%',
                rowNum: 1000,
                sortname: 'num',
                sortorder: "asc",

            });
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
            var rows = $("#list").getDataIDs();
            var $self = $(this);

            //for (var i = 0; i < rows.length; i++) {
            //    var req_bundle_qty = $("#list").getCell(rows[i], "req_bundle_qty");
            //    var req_qty = $("#list").getCell(rows[i], "req_qty");
            //    var real_bundle_qty = $("#list").getCell(rows[i], "real_bundle_qty");
            //    var real_qty = $("#list").getCell(rows[i], "real_qty");
            //    if (parseInt(req_bundle_qty) >= parseInt(real_bundle_qty)) {

            //        var kq = parseInt(req_bundle_qty) - parseInt(real_bundle_qty);
            //        if (kq > 0) {
            //            $("#list").jqGrid("setCell", rows[i], "req_bundle_qty", Math.round(kq));
            //        }
            //        else {
            //            $("#list").jqGrid("setCell", rows[i], "req_bundle_qty", 0);
            //        }
            //    }
            //    if (parseInt(req_qty) >= parseInt(real_qty)) {

            //        var kq = parseInt(req_qty) - parseInt(real_qty);
            //        if (kq > 0) {
            //            $("#list").jqGrid("setCell", rows[i], "req_qty", Math.round(kq));
            //        }
            //        else {
            //            $("#list").jqGrid("setCell", rows[i], "req_qty", 0);
            //        }
            //    }
            //}
    
          
           
            for (var i = 0; i < rows.length; i++) {
                var req_qty = $("#list").getCell(rows[i], "req_qty");
                var real_qty = $("#list").getCell(rows[i], "real_qty");
              
                if (real_qty < req_qty) {
                    $("#list").jqGrid('setCell', rows[i], "real_qty", "", {
                        'background-color': 'orange ',
                    });
                }
                else {
                    $("#list").jqGrid('setCell', rows[i], "real_qty", "", {
                        background: '',
                    });
                }
            }
        },
        loadComplete: function () {
            var grid = $("#list");
            var rows = $("#list").getDataIDs();
            var subGridCells = $("td.sgcollapsed", list[0]);
            $.each(subGridCells, function (i, value) {
                var req_qty = $("#list").getCell(rows[i], "req_qty");
                var real_qty = $("#list").getCell(rows[i], "real_qty");

                if (parseInt(req_qty) <= parseInt(real_qty) || parseInt(req_qty) == 0) {
                    $(value).unbind('click').html('');
                }
            });
        },
    });
  
});
function btn_delete(cellValue, options, rowdata, action) {

    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + id + '"onclick="Xoa_sub_con(this);">Delete</button>';
    return html;
}
function Xoa_sub_con(e) {
    $.ajax({
        url: '/ShippingMgt/Xoa_sub_shipping?wmtid=' + $(e).data("id"),
        type: "get",
        dataType: "json",
        success: function (data) {

            if (data) {

                jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            } else {
                alert(response.message);
            }

        },

    });
}
$("#list").on("keydown", function (event) {
    var target = $("#list").find('input')[0];
    if (target) {
        if (event.keyCode === 113 && this.p.selrow) { // F2 key pressed and row is selected
            $(this).jqGrid("editRow", this.p.selrow, { keys: true });

        }
    }
})
$("#list tr").keyup(function (e) {
    var rowid = parseInt($(this).attr("id"));
    var r = $("#list").getRowData(rowid);
    var col1 = r.column1;
    alert(col1);
    if (e.keyCode == 46) {
        alert(col1);
    }
});
var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list");
        grid.jqGrid('editRow', id, { keys: true, focusField: 4 });
        lastSelection = id;
    }
}
function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
//get select
// Khai báo URL stand của bạn ở đây
var wh = "/ShippingMgt/Warehouse";
var getadmin = "/ShippingMgt/getadmin";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Location *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#lct_cd").html(html);
        }
    });
}
function pp_mt_no(cellValue, options, rowdata, action) {

    var html = "";
    
    if (cellValue != null) {

        if (parseInt(rowdata.req_qty) <= parseInt(rowdata.real_qty) || parseInt(rowdata.req_qty) == 0) {
            var html = rowdata.mt_no;
            return html;
        }
        else {
            var html = '<button style="color: dodgerblue; border: none; background: none; " data-mrdid="' + rowdata.mrdid + '" data-mt_no="' + rowdata.mt_no + '" onclick="open_MT_nO_popup(this)">' + cellValue + '</button>';
            return html;
        }
       
    }
}

function open_MT_nO_popup(e) {

    $('.popup-dialog.ReceivingDirection_MT_NO_Popup').dialog('open');
    var mt_no = $(e).data('mt_no');
    var mrdid = $(e).data('mrdid');


    $.get("/ShippingMgt/ReceivingDirection_MT_NO_Popup?" +
        "mt_no=" + mt_no + "&mrdid=" + mrdid + "  "

        , function (html) {
            $("#ReceivingDirection_MT_NO_Popup").html(html);
        });


}

//list_master
$(function () {
    $grid = $("#list1").jqGrid({
        url: "/ShippingMgt/GetSdmt",
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'sid', name: 'sid', width: 50, hidden: true },
            { label: 'bom_no', name: 'bom_no', width: 110, align: 'center', hidden: true },
            { label: 'rd_no', name: 'rd_no', width: 110, align: 'center', hidden: true },
            { label: 'SD NO', name: 'sd_no', width: 90, align: 'center' },
            { label: 'Destination', name: 'lct_nm', width: 150, align: 'left' },
            { label: 'Worker', name: 'worker_id', width: 60, align: 'left' },
            { label: 'Manager', name: 'manager_id', width: 60, align: 'left' },
            { label: 'Work date', name: 'work_dt', width: 100, align: 'center' },
            { label: 'MR', name: 'mr_no', width: 90, align: 'center' },
        ],
        loadonce: true,
        shrinkToFit: false,
        pager: '#list1Page',
        rowNum: 50,
        rownumbers: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        height: 270,
        caption: "SD Master",
        jsonReader:
      {
          root: "rows",
          page: "page",
          total: "total",
          records: "records",
          repeatitems: false,
          Id: "0"
      },
        width: null,

        onSelectRow: function (rowid, status, e) {
            var rowData = $(this).getLocalRow(rowid);
            $("#list2").clearGridData();
            $("#list2").setGridParam({ url: "/ShippingMgt/GetSdmt_detail?sd_no=" + rowData.sd_no, datatype: "json" }).trigger("reloadGrid");
        },
    });
});
//list_master
//list_detail
$(function () {
    $grid = $("#list2").jqGrid({
        url: "/ShippingMgt/GetSdmt_detail",
        mtype: 'GET',
        datatype: 'json',
        colModel: [

            { key: true, label: 'sdid', name: 'sdid', width: 50, hidden: true },
            { label: 'bom_no', name: 'bom_no', width: 110, align: 'center', hidden: true },
            { label: 'rd_no', name: 'rd_no', width: 1, align: 'center', hidden: true },
            { label: 'SD NO', name: 'sd_no', width: 100, align: 'center' },
            { label: 'MT NO', name: 'mt_no', width: 200, align: 'left' },
            { label: 'MT CD', name: 'mt_cd', width: 200, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 200, align: 'left' },
            { label: 'Group Unit', name: 'gr_qty', width: 80, align: 'right' },
            { label: 'Send Bundle Qty', name: 'send_bundle_qty', width: 80, align: 'right' },
            //{
            //    label: 'Send Bundle Qty', name: 'send_bundle_qty', width: 110, align: 'right', editable: true, editoptions: {
            //        dataInit: function (element) {
            //            $(element).keypress(function (e) {
            //                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //                    return false;
            //                }
            //            });

            //            $(element).keyup(function (e) {

            //                var id = e.target.id.replace("_send_bundle_qty", "")
            //                var row = $("#list2").jqGrid('getRowData', id);
            //                var gtri = this.value;
            //                var ketqua = parseInt(gtri) * parseInt(row.gr_qty);
            //                var cell = jQuery('#' + id + '_send_qty');
            //                if (gtri == "") {
            //                    cell.val(0);
            //                } else {
            //                    //ketqua = ketqua.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
            //                    cell.val(ketqua);
            //                }

            //                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //                    return false;
            //                }
            //            });
            //            $(element).keydown(function (e) {

            //                var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
            //                var row_id = $("#list2").getRowData(selectedRowId);
            //                if (e.which == 13) {
            //                    var ketqua = parseInt(this.value) * parseInt(row_id.gr_qty);

            //                }
            //            });
            //        }
            //    }, editrules: { integer: true, required: true }, formatter: 'integer',
            //},
            //{
            //    label: 'Send Qty', name: 'send_qty', width: 90, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
            //    formatter: 'integer',
            //},
            { label: 'Send Qty', name: 'send_qty', width: 80, align: 'right' },
            { label: 'Width(mm)', name: 'width', width: 80, align: 'right' },
            { label: 'Length(m)', name: 'length', width: 80, align: 'right' },
            { label: 'Area(m2)', name: 'area', width: 80, align: 'right' },
            { label: 'Edit', name: 'edit', width: 80, align: 'center', formatter: bntCellEdit },

        ],
        loadonce: true,
        shrinkToFit: false,
        pager: '#list2Page',
        rowNum: 50,
        rownumbers: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        caption: "SD Detail",
        height: 270,
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
        width: null,

        onSelectRow: function (id, rowid, status, e) {
            //if (id) {
            //    jQuery('#list2').jqGrid('editRow', id, true);

            //}
        },
    });
});
//list_detail
var date = new Date();
var currentDate = date.toISOString().slice(0, 10);

document.getElementById('work_date').value = currentDate;
$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#work_date").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#work_date").datepicker({ dateFormat: 'yy-mm-dd' }).datepicker('setDate', 'today');

//search
//$("#searchBtn").click(function () {
//    $.ajax({
//        url: "/ShippingMgt/searchGeneration",
//        type: "get",
//        dataType: "json",
//        data: {
//            mr_no: $('#mr_no').val().trim(),
//            bom_no: $('#bom_no').val().trim(),
//            start : $('#start').val(),
//            end : $('#end').val(),
//        },
//        success: function (result) {
//            $("#List_mr").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });
//});
function getData(pdata) {
    $('.loading').show();
    var params = new Object();
    if (jQuery('#List_mr').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mr_no = $('#mr_no').val().trim();
    params.bom_no = $('#bom_no').val().trim();
    params.start = $('#start').val();
    params.end = $('#end').val();
    $("#List_mr").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/ShippingMgt/searchGeneration",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#List_mr")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

$("#searchBtn").click(function () {

    $("#List_mr").clearGridData();

    $('.loading').show();

    var grid = $("#List_mr");
    grid.jqGrid('setGridParam', { search: true });

    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);
});
//MR list

$(function () {
    $("#List_mr").jqGrid({
        //url: "/ShippingMgt/getMRNO",
        mtype: 'GET',
        //datatype: 'json',
        datatype: function (postData) { getData(postData); },
        colModel: [
             { key: true, label: 'mrid', name: 'mrid', width: 80, align: 'center', hidden: true },
            { key: false, label: 'req_qty', name: 'req_qty', width: 100, align: 'center', hidden: true },
            { key: false, label: 'real_qty', name: 'real_qty', width: 100, align: 'center', hidden: true },
            { key: false, label: 'MR NO', name: 'mr_no', width: 100, align: 'center' },
            { key: false, label: 'Destination', name: 'lct_nm', width: 150, align: 'left' },
            { key: false, label: 'Destination', name: 'lct_cd', width: 80, align: 'left', hidden: true },
            { key: false, label: 'Requester', name: '', width: 80, align: 'left' },
            { key: false, label: 'Manager', name: 'manager_id', width: 50, align: 'left' },
            { key: false, label: 'Req Receive Date', name: 'req_rec_dt', width: 100, align: 'center' },
            { key: false, label: 'MT CNT', name: 'mt_qty', width: 50, align: 'right' },
            { key: false, label: 'Related Bom', name: 'rel_bom', width: 100, align: 'center' },
            { key: false, label: 'Description', name: 'remark', width: 100, align: 'left' },
            { key: false, label: 'tong_roll', name: 'tong_roll', hidden: true },

        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },

        loadonce: false,
        shrinkToFit: false,
        pager: '#List_mr_page',
        caption: 'MR List',
        rowNum: 50,
        rownumbers: true,
        multiselect: false,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        height: 340,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        width: null,

        onSelectRow: function (rowid, cellValue, selected, status, e) {
            //var rowData = $(this).getLocalRow(rowid);
            //$("#mr-no").val(rowData.mr_no);
            //$("#lct_cd").val(rowData.lct_cd);

            var selectedRowId = $("#List_mr").jqGrid("getGridParam", 'selrow');
            row_id = $("#List_mr").getRowData(selectedRowId);

            $("#mr-no").val(row_id.mr_no);
            $("#lct_cd").val(row_id.lct_cd);

            $("#list").setGridParam({ url: "/ShippingMgt/Get_list_Generation?mr_no=" + row_id.mr_no, datatype: "json" }).trigger("reloadGrid");

        },
        gridComplete: function () {
            var rows = $("#List_mr").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var tong_roll = $("#List_mr").getCell(rows[i], "tong_roll");
                //var real_qty = $("#List_mr").getCell(rows[i], "real_qty");
                if (tong_roll > 0) {
                    $("#List_mr").jqGrid('setCell', rows[i], "mr_no", "", {
                        'background-color': 'orange ',
                    });
                }
                //else {
                //    $("#List_mr").jqGrid('setCell', rows[i], "mr_no", "", {
                //        background: '',
                //    });
                //}
            }
        },
    });

});

//MR list


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
}

//$("#Direction").click(function () {

//    if ($("#form1").valid()) {
//        var lct_cd = $('#lct_cd').val();
//        var worker_id = $('#userid').val();
//        var manager_id = $('#uname2').val();
//        var work_dt = $('#work_date').val();
//        var mr_no = $('#mr-no').val();
//        var real_work_dt = $('#work_date').val();

//        var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData, send_bundle_qty = "", send_qty = "", vailable_qty = "";

//        var send_qty = [];
//        var send_bundle_qty = [];
//        var send_code = [];

//        var id_dk = [];//ds k thỏa dk chứa gtri avalible nhỏ hơn send qty

//        for (i = 0, n = selRowIds.length; i < n; i++) {

//            //console.log(selRowIds[i]);

//            get_send_qty = getCellValue(selRowIds[i], 'send_qty');
//            get_send_bundle_qty = getCellValue(selRowIds[i], 'send_bundle_qty');

//            get_code = $("#list").jqGrid('getRowData', selRowIds[i]).mrdid;
//            get_id_dk = $("#list").jqGrid('getRowData', selRowIds[i]).mt_no;
//            vailable_qty = $("#list").jqGrid('getRowData', selRowIds[i]).Available;

//            if (get_send_qty == undefined) {
//                var ret = $("#list").jqGrid('getRowData', selRowIds[i]);

//                var send_value = ret.send_qty;
//                var send_bundle_value = ret.send_bundle_qty;
//                var send_code_value = ret.mrdid;

//                vailable = ret.Available;

//                if (parseInt(vailable_qty) >= parseInt(send_value)) {
//                    send_qty.push(send_value);
//                    send_bundle_qty.push(send_bundle_value);
//                    send_code.push(send_code_value);

//                }
//                else {
//                    id_dk.push(get_code);
//                }
//            }
//            else {
//                if (parseInt(vailable_qty) >= parseInt(get_send_qty)) {
//                    send_qty.push(get_send_qty);
//                    send_bundle_qty.push(get_send_bundle_qty);
//                    send_code.push(get_code);

//                }
//                else {
//                    id_dk.push(get_code);
//                }
//            }
//        }
//        //console.log(fruits);

//        if (selRowIds.length > 0) {
//            $('#loading').show();
//            $.ajax({
//                url: "/ShippingMgt/insertSdinfo?" + "id=" + send_code + "&" + "req_qty=" + send_qty + "&" + "req_bundle_qty=" + send_bundle_qty,
//                type: "get",
//                dataType: "json",
//                data: {
//                    lct_cd: lct_cd,
//                    worker_id: worker_id,
//                    manager_id: manager_id,
//                    work_dt: work_dt,
//                    real_work_dt: real_work_dt,
//                    mr_no: mr_no,
//                },
//                success: function (response) {
//                    $('#loading').hide();
//                    //debugger
//                    if (response.result) {
//                        var id = response.result.sid;
//                        $("#list1").jqGrid('addRowData', id, response.result, 'first');
//                        $("#list1").setRowData(id, false, { background: "#d0e9c6" });

//                        $.each(response.resutl1, function (key, item) {
//                            var id = item.sdid;
//                            $("#list2").jqGrid('addRowData', id, item, 'first');
//                            $("#list2").setRowData(id, false, { background: "#d0e9c6" });
//                        });
//                        for (var i = 0; i < send_code.length; i++) {
//                            var id = send_code[i];
//                            $("#list").setRowData(id, send_code, { background: "#d0e9c6" });
//                        }
//                        //if (id_dk.length > 0) {
//                        //    alert(id_dk + "  "+ "Cann't Direction, Please Check again!!!")
//                        //}

//                        //window.swalSuccess(response.message).then(x => { });
//                    } else {
//                        $('#loading').hide();
//                        jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                        jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                        $('#loading').hide();
//                        alert("The Available = 0 or less than send qty ");
//                        //window.toastr.error(response.message, 'Error!');
//                    }
//                },
//            });
//        }
//        else {
//            alert("Please select Grid.");
//        }

//    }
//})
$("#Direction").click(function () {

    if ($("#form1").valid()) {
        var lct_cd = $('#lct_cd').val();
        var worker_id = $('#userid').val();
        var manager_id = $('#uname2').val();
        var work_dt = $('#work_date').val();
        var mr_no = $('#mr-no').val();
        var real_work_dt = $('#work_date').val();


        var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData, send_bundle_qty = "", send_qty = "", vailable_qty = "";

        var send_qty = [];
        var send_bundle_qty = [];
        var send_code = [];

        var id_dk = [];//ds k thỏa dk chứa gtri avalible nhỏ hơn send qty

        for (i = 0, n = selRowIds.length; i < n; i++) {

            //console.log(selRowIds[i]);

            get_send_qty = getCellValue(selRowIds[i], 'send_qty');
            get_send_bundle_qty = getCellValue(selRowIds[i], 'send_bundle_qty');

            get_code = $("#list").jqGrid('getRowData', selRowIds[i]).mrdid;
            get_id_dk = $("#list").jqGrid('getRowData', selRowIds[i]).mt_no;
            vailable_qty = $("#list").jqGrid('getRowData', selRowIds[i]).available;
            sendd_qty = $("#list").jqGrid('getRowData', selRowIds[i]).send_qty;



            if (get_send_qty == undefined) {
                var ret = $("#list").jqGrid('getRowData', selRowIds[i]);

                var send_value = ret.send_qty;
                var send_bundle_value = ret.send_bundle_qty;
                var send_code_value = ret.mrdid;

                vailable = ret.available;

                if (parseInt(vailable_qty) >= 0 && parseInt(sendd_qty) > 0) {
                    send_qty.push(send_value);
                    send_bundle_qty.push(send_bundle_value);
                    send_code.push(send_code_value);

                }
                else {
                    id_dk.push(get_code);
                }
            }
            else {
                if (parseInt(vailable_qty) >= parseInt(get_send_qty) && parseInt(sendd_qty) > 0) {
                    send_qty.push(get_send_qty);
                    send_bundle_qty.push(get_send_bundle_qty);
                    send_code.push(get_code);

                }
                else {
                    id_dk.push(get_code);
                }
            }
        }

        if (selRowIds.length > 0) {
            $('#loading').show();
            $.ajax({
                url: "/ShippingMgt/insertSdinfo?" + "id=" + send_code + "&" + "req_qty=" + send_qty + "&" + "req_bundle_qty=" + send_bundle_qty,
                type: "get",
                dataType: "json",
                data: {
                    lct_cd: lct_cd,
                    worker_id: worker_id,
                    manager_id: manager_id,
                    work_dt: work_dt,
                    real_work_dt: real_work_dt,
                    mr_no: mr_no,
                },
                success: function (response) {
                    $('#loading').hide();
                    if (response.result) {
                        var id = response.result.sid;
                        $("#list1").jqGrid('addRowData', id, response.result, 'first');
                        $("#list1").setRowData(id, false, { background: "#d0e9c6" });

                        $.each(response.resutl1, function (key, item) {
                            var id = item.sdid;
                            $("#list2").jqGrid('addRowData', id, item, 'first');
                            $("#list2").setRowData(id, false, { background: "#d0e9c6" });
                        });
                        $.each(response.resutl2, function (key, item) {
                            var id = item.mrdid;
                            $("#list").setRowData(id, item, { background: "#d0e9c6" });
                        });
                    } else {
                        $('#loading').hide();
                        jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                        jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                        $('#loading').hide();
                        alert("The Available = 0 or less than send qty ");
                    }
                },
            });
        }
        else {
            alert("Please select Grid.");
        }

    }
})

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'pdf',
            pdfFontSize: '12',
            fileName: 'DirectionsGeneration',
            escape: false,
        });
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $('table').tableExport({
            headers: true,
            type: 'xls',
            fileName: 'DirectionsGeneration',
            escape: false,
            headers: true,
        });
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $('table').tableExport({
            type: 'doc',
            fileName: 'DirectionsGeneration',
            escape: false,
            headers: true,
        });
    });
});

////Button Edit
function bntCellEdit(cellvalue, options, rowdata, action) {
    var html = '<button' +
        ' data-sdid=' + rowdata.sdid +
        ' data-mt_no=' + rowdata.mt_no +
        ' data-send_bundle_qty=' + rowdata.send_bundle_qty +
        ' data-send_qty=' + rowdata.send_qty +
        ' class="btn btn-xs btn-warning" onclick="ReceivingDirection_Edit_Popup(this); ">Edit</button>';

    return html;
}
function ReceivingDirection_Edit_Popup(e) {
    var that = $(e);

    var chuoi = $(e).data("sdid") + "_send_bundle_qty";
    var gr = $("#" + chuoi + "").val();
    var send_bundle_qty = gr;

    if ((send_bundle_qty == "") || (send_bundle_qty == undefined)) {
        send_bundle_qty = that.data("send_bundle_qty");
        var adfd = send_bundle_qty;
    }

    var chuoi1 = $(e).data("sdid") + "_send_qty";
    var gr1 = $("#" + chuoi1 + "").val();
    var send_qty = gr1;
    if ((send_qty == "") || (send_qty == undefined)) {
        send_qty = that.data("send_qty");
    }



    $.get("/ShippingMgt/PartialView_ReceivingDirection_Edit_Popup?" +
        "sdid=" + that.data("sdid") +
        "&mt_no=" + that.data("mt_no") +
        "&send_qty=" + (send_qty) +
        "&send_bundle_qty=" + (send_bundle_qty),
        function (html) {
            $("#PartialView_ReceivingDirection_Edit_Popup").html(html);
        });

    $('.popup-dialog.ReceivingDirection_Edit_Popup').dialog('open');
}

////Button bntCellSplit
function bntCellSplit(cellvalue, options, rowdata, action) {
    var sdmid = rowdata.sdmid;
    var html = '<button' +
        ' data-sdmid=' + rowdata.sdmid +
        ' class=" del_save_but btn btn-xs btn-success " onclick="btn_Split_Save(' + sdmid + ');" >Split</button>';
    return html;
}
var GetStatus = "/MaterialInformation/GetStatus";
$(document).ready(function () {
    _GetStatus();
});
function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd_pp").html(html);
            $("#mt_sts_cd1").html(html);

        }
    });
}
function btn_Split_Save(sdmid) {
    $('#sdmid222').val(sdmid);
    $.ajax({
        url: "/ShippingMgt/Select_split?" + "sdmid=" + sdmid,
        type: "post",
        dataType: "json",
        success: function (response) {

            //debugger
            if (response.result) {

                $('#wmtid').val(response.w_material_info.wmtid);

                $('#mt_cd').val(response.w_material_info.mt_cd);
                $('#mt_no_origin').val(response.w_material_info.orgin_mt_cd);
                $('#gr_qty_pp').val(response.w_material_info.gr_qty);
                $('#mt_sts_cd_pp').val(response.w_material_info.mt_sts_cd);
                $('#remark_pp').val(response.w_material_info.remark);

            } else {

            }
        },
    });

    $('.dialog_Split').dialog('open');
}
$("#save_split").click(function () {
    $.ajax({
        url: "/ShippingMgt/split_Shipping_Derection",
        type: "get",
        dataType: "json",
        data: {
            wmtid: $('#wmtid1').val(),
            mt_cd: $('#mt_cd1').val(),
            gr_qty: $('#gr_qty1').val(),
            mt_sts_cd: $('#mt_sts_cd1').val(),
            remark: $('#remark1').val().trim(),
        },
        success: function (response) {
            $('.Shipping_Derection_Split').dialog('close');
            if (response.x) {
                alert("Success");
                var id = response.x.wmtid;
                $("#list_mt").jqGrid("setCell", id, "gr_qty", response.x.gr_qty+"m");
                $("#list_mt").jqGrid("setCell", id, "gr_qty1", response.x.gr_qty);
                $("#list_mt").setRowData(id, false, { background: "#d0e9c6" });

                console.log(response.x);
                console.log(response.b);
               

                var rowData = $('#list_mt').jqGrid('getRowData', id);
                rowData.gr_qty = response.b.gr_qty + "m";
                rowData.mt_cd = response.b.mt_cd;
                rowData.gr_qty1 = response.b.gr_qty;
                rowData.remark = response.b.remark;
                rowData.mt_sts_cd = response.b.mt_sts_cd;
                rowData.orgin_mt_cd = response.b.orgin_mt_cd;
                rowData.mt_sts_cd1 = response.b.bb_no; 
                rowData.wmtid = response.b.wmtid;
                rowData.expiry_dt = response.b.expiry_dt;

                $("#list_mt").jqGrid('addRowData', response.b.wmtid, rowData, 'first');
                $("#list_mt").setRowData(response.b.wmtid, false, { background: "#d0e9c6" });

               
            } else {
                alert(response.message);
            }
        }
    });
    //}
});

$(".dialog_Split").dialog({
    width: '70%',
    height: 150,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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
        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    },
});
$(".Shipping_Derection_Split").dialog({
    width: '70%',
    height: 150,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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
    },
});
$("#save").click(function () {
    $.ajax({
        url: "/ShippingMgt/insert_update_split",
        type: "get",
        dataType: "json",
        data: {
            wmtid: $('#wmtid').val(),
            mt_cd: $('#mt_cd').val(),
            gr_qty: $('#gr_qty_pp').val(),
            mt_sts_cd: $('#mt_sts_cd_pp').val(),
            remark: $('#remark_pp').val().trim(),

            sdmid: $('#sdmid222').val(),
        },
        success: function (response) {
            console.log(response);
            if (response.result) {
                jQuery("#list_edit").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $('.dialog_Split').dialog('close');
            } else {
                alert(response.message);
            }
        }
    });
    //}
});


$("#Btn_Add").click(function () {
    var sdid = $('#sdid').val();
    var sd_no = $('#sd_no').val();
    var send_bundle_qty = $('#send_bundle_qty').val();
    var send_qty = $('#send_qty').val();
    var mt_no = $('#mt_no').val();

    var i, selRowIds = $('#list_add').jqGrid("getGridParam", "selarrrow"), n, rowData;

    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
    }

    $.ajax({

        url: "/ShippingMgt/Add_Shipping_derection?" + "id=" + selRowIds + "&sdid=" + sdid + "&sd_no=" + sd_no + "&send_bundle_qty=" + send_bundle_qty + "&req_qty=" + send_qty + "&mt_no=" + mt_no,
        type: "post",
        dataType: "json",
        success: function (response) {

            //debugger
            if (response.result) {
                $('.popup-dialog.ReceivingDirection_Add_Popup').dialog('close');
                jQuery("#list_edit").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list_add").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            } else {
                alert(response.message);
                jQuery("#list_edit").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list_add").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        },
    });
})

$("#delete_shipping_list").click(function () {
    $.ajax({
        url: "/ShippingMgt/Dele_shipping_derection",
        type: "post",
        dataType: "json",
        data: {
            sdmid: $('#sdmid').val(),
        },
        success: function (response) {
            //debugger
            if (response.result) {
                alert(response.message);
                jQuery("#list_add").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list_edit").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

            }
            else {
                alert(response.message);
                jQuery("#list_edit").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        },
        error: function (data) { alert('Ordered, no deletion allowed. Please check again'); }
    });
    $('#dialogDelete').dialog('close');
});
$("#dialogDelete").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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
});


$("#exp_input_dt").datepicker({ dateFormat: 'yy-mm-dd' }).val();