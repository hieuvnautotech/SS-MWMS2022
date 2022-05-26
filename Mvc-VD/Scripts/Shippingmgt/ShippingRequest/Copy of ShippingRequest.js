
$grid = $("#ShipRequest").jqGrid({
    url: '/Shippingmgt/Getmt_list',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { label: 'MT No', name: 'mt_no', width: 150, align: 'left' },
        { label: 'MT Name', name: 'mt_nm', width: 150, align: 'left' },
        { label: 'Total qty', name: 'mt_qty', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Available', name: 'Available', width: 80, align: 'right', formatter: 'integer' },
        {
            label: 'Req Qty', name: 'reg', width: 110, align: 'right', editable: true, editoptions: {
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
    loadonce: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 313,
    width: $(".boxA").width() - 7,
    multiPageSelection: true,
    onSelectRow: editRow,       
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

$('#ShipRequest').jqGrid('setGridWidth', $(".boxA").width());
$(window).on("resize", function () {
    var newWidth = $("#ShipRequest").closest(".ui-jqgrid").parent().width();
    $("#ShipRequest").jqGrid("setGridWidth", newWidth, false);
});


var lastSelection;

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
    url: '/Shippingmgt/Deatil_mt',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        //{ label: 'pdid', name: 'pdid', width: 110, align: 'center' },
        { label: 'MT No', name: 'mt_no', width: 110, align: 'left' },
        { label: 'MT Name', name: 'mt_nm', width: 110, align: 'left' },
        { label: 'Reg qty', name: 'reg', width: 110, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, formatter: 'integer', },
        { label: 'Width', name: 'width', width: 110, align: 'right' },
        { label: 'Length (M)', name: 'spec', width: 110, align: 'right' },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#StatePager',
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    shrinkToFit: false,
    viewrecords: true,
    height: 313,
    width: $(".boxB").width() - 7,
    caption: 'Req MT List',
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
    onSelectRow: function (rowid, status, e) {

    },
});

$('#State').jqGrid('setGridWidth', $(".boxB").width());
$(window).on("resize", function () {
    var newWidth = $("#State").closest(".ui-jqgrid").parent().width();
    $("#State").jqGrid("setGridWidth", newWidth, false);
});


$("#req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#req_rec_dt').datepicker().datepicker('setDate', 'today');

$("#searchBtn").click(function () {
    $.ajax({
        url: "/Shippingmgt/searchmt_list",
        type: "get",
        dataType: "json",
        data: {
            fo_no: $('#c_fo_no').val().trim(),
            bom_no: $('#c_bom_no').val().trim(),
            mt_no: $('#c_mt_no').val().trim(),
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

$("#save_2").click(function () {
    var fruits = [];
    var id_fr = [];
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";

    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        reg_qty = getCellValue(selRowIds[i], 'reg');
        var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
        if (reg_qty == undefined) {


            var giatri = ret.reg;
            var giatri1 = ret.wmtid;
            if (giatri != "0") {
                fruits.push(giatri);
                id_fr.push(giatri1);
            }
        } else {
            if (reg_qty != "0") {

                fruits.push(reg_qty);
                id_fr.push(id);
            }
        }
    }

    $.ajax({
        url: "/Shippingmgt/Deatil_mt?" + "id=" + id_fr + "&" + "reg_qty=" + fruits,
        type: "get",
        dataType: "json",
        success: function (data) {
            $("#State").clearGridData();
            for (var i = 0; i < data.length; i++) {
                var id = data[i].wmtid;
                $("#State").jqGrid('addRowData', id, data[i], 'first');
                $("#State").setRowData(id, false, { background: "#d0e9c6" });
            }
        }
    });
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
//LƯỚI view all
$grid = $("#w_mr").jqGrid({
    url: '/Shippingmgt/mr_info',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'mrid', name: 'mrid', width: 50, hidden: true },
        { label: 'MSR No', name: 'mr_no', width: 110, align: 'center' },
        { label: 'Loaction', name: 'lct_cd', width: 180, align: 'left' },
        { label: 'Requester', name: 'worker_id', width: 110, align: 'left' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left', },
        { label: 'Req Receive Date', name: 'req_rec_dt', width: 110, align: 'center' },
        { label: 'Real Receive Date', name: 'real_rec_dt', width: 110, align: 'center' },
        { label: 'Mt Qty', name: 'mt_qty', width: 110, align: 'right', },
        { label: 'Related Bom', name: 'rel_bom', width: 110,  },
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
    height: 300,
    width: $(".boxC").width()-5,
    caption: 'MSR Info',
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

$('#w_mr').jqGrid('setGridWidth', $(".boxC").width()+15);
$(window).on("resize", function () {
    var newWidth = $("#w_mr").closest(".ui-jqgrid").parent().width();
    $("#w_mr").jqGrid("setGridWidth", newWidth, false);
});

$(".save_req").click(function () {
    if ($("#form1").valid() == true)
    {      
        var fruits = [];
        var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";

        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];
            reg_qty = getCellValue(selRowIds[i], 'reg');
            if (reg_qty == undefined) {
                var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
                var giatri = ret.reg;
                fruits.push(giatri);
            } else {

                fruits.push(reg_qty);
            }
        }
        //alert(reg);


        var mt_qty = $("#State").jqGrid("getGridParam", 'records');

        $.ajax({
            url: "/Shippingmgt/insert_mr_info",
            type: "post",
            dataType: "json",
            data: {
                whouse: $('#whouse').val(),
                worker: $('#userid').val(),
                mt_qty: mt_qty,
                manager: $('#userid2').val(),
                req_rec_dt: $('#req_rec_dt').val(),
                rel_bom: $('#rel_bom').val(),
                remark: $('#remark').val(),
            },
            success: function (data) {
                var id = data.mrid;
                $("#w_mr").jqGrid('addRowData', id, data, 'first');
                $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });

                $("#State").clearGridData();
                jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                $("#rel_bom").val("");
                $("#remark").val("");
                $("#userid").val("");
                $("#userid2").val("");
                $("#whouse").val("");



                //insert w_mr_detail
                $.ajax({
                    type: "post",
                    dataType: 'text',
                    url: "/Shippingmgt/insertw_mr_detail?" + "id=" + selRowIds + "&" + "req_qty=" + fruits + "&mr_no=" + data.mr_no,
                    headers: {
                        "Content-Type": "application/json",
                        "X-HTTP-Method-Override": "POST"
                    },
                    cache: false,
                    success: function (data) {
                        $("#Deatil_mt").clearGridData();

                    }
                });
            },
            error: function (data) {

                alert('The Code is the same. Please check again.');
            }
        });       
    }
});

$(".save_req1").click(function () {
    var fruits = [];
    var i, n, rowData, selRowIds = $("#ShipRequest").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";

    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        reg_qty = getCellValue(selRowIds[i], 'reg');
        if (reg_qty == undefined) {
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            var giatri = ret.reg;
            fruits.push(giatri);
        } else {

            fruits.push(reg_qty);
        }
    }
    //alert(reg);


    var mt_qty = $("#State").jqGrid("getGridParam", 'records');

    $.ajax({
        url: "/Shippingmgt/insert_mr_info",
        type: "post",
        dataType: "json",
        data: {
            whouse: $('#whouse').val(),
            worker: $('#userid').val(),
            mt_qty: mt_qty,
            manager: $('#userid2').val(),
            req_rec_dt: $('#req_rec_dt').val(),
            rel_bom: $('#rel_bom').val(),
            remark: $('#remark').val(),
        },
        success: function (data) {
            var id = data.mrid;
            $("#w_mr").jqGrid('addRowData', id, data, 'first');
            $("#w_mr").setRowData(id, false, { background: "#d0e9c6" });

            $("#State").clearGridData();
            jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

            $("#rel_bom").val("");
            $("#remark").val("");
            $("#userid").val("");
            $("#userid2").val("");
            $("#whouse").val("");



            //insert w_mr_detail
            $.ajax({
                type: "post",
                dataType: 'text',
                url: "/Shippingmgt/insertw_mr_detail?" + "id=" + selRowIds + "&" + "req_qty=" + fruits + "&mr_no=" + data.mr_no,
                headers: {
                    "Content-Type": "application/json",
                    "X-HTTP-Method-Override": "POST"
                },
                cache: false,
                success: function (data) {
                    $("#Deatil_mt").clearGridData();

                }
            });
        },
        error: function (data) {

            alert('The Code is the same. Please check again.');
        }
    });
});

$(document).ready(function () {
    $.ajax({
        url: '/Home/GetSessionMenuData',
        type: 'GET',
        datatype: 'json',
        success: function (SessionData) {
            $("#userid").val(SessionData.userid);
            if (SessionData.userid == "root")
            {
                $("#uname").val("root");
            }
            else
            {
                $.get(getadmin + "?id=" + SessionData.userid, function (data) {
                    if (data != null && data != undefined) {
                        $("#uname").val(data[0].uname);
                    }                  
                });
            }         
        },
    });
    $.get('/ShippingMgt/getuserall', function (data) {
            var html = '';
            html += '<option value="" selected="selected" disabled> * Name *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.uname + '>' + item.userid + '</option>';
            });
            $("#userid2").html(html);
    });  
});
//function Changed() {
//    $("#uname2").val($("#userid2").val());
//};
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
