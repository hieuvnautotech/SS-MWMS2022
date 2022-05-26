

        $grid = $("#ShipRequest").jqGrid({
            url: '/Shippingmgt/Getmt_list',
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
                //{ label: 'pdid', name: 'pdid', width: 110, align: 'center' },
                { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
                { label: 'bom_no', name: 'bom_no', width: 110, align: 'center', hidden: true },
                { label: 'fo_no', name: 'fo_no', width: 110, align: 'center', hidden: true },
                { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
                { label: 'Material Name', name: 'mt_nm', width: 110, align: 'center' },
                { label: 'Total qty', name: 'total_qty', width: 110, align: 'center' },
                { label: 'Available', name: 'Available', width: 110, align: 'center'},
                { label: 'width', name: 'width', width: 110, align: 'center' },
                { label: 'spec', name: 'spec', width: 110, align: 'center' },
                { label: 'Reg qty', name: 'reg', width: 110, align: 'center', editable: true, editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });
                    }
                }, editrules: { integer: true, required: true }, formatter: 'integer', },
            ],
            loadonce: true,
            shrinkToFit: false,
            pager: '#ShipRequestPager',
            rowNum: 2000,
            rownumbers: true,
            caption: 'Material List',
            multiselect: true,
            multiPageSelection: true,
            rowList: [60, 80, 100, 120],
            viewrecords: true,
            height: 415,
            onSelectRow: editRow, // the javascript function to call on row click. will ues to to put the row in edit mode

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
var wh = baseService + "/Warehouse";
var getadmin = "/ReceivingMgt/getadmin";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Location *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.index_cd + '>' + item.index_cd + '</option>';
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
        { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
        { label: 'Material Name', name: 'mt_nm', width: 110, align: 'center' },
        { label: 'Total qty', name: 'total_qty', width: 110, align: 'center' },
        { label: 'Available', name: 'Available', width: 110, align: 'center', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, formatter: 'integer', },
        { label: 'width', name: 'width', width: 110, align: 'center' },
        { label: 'spec', name: 'spec', width: 110, align: 'center' },
        { label: 'Reg qty', name: 'reg', width: 110, align: 'center', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, formatter: 'integer', },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#StatePager',
    rowNum: 2000,
    rownumbers: true,
    caption: 'Req MT List',
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 415,
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
    format: 'yyyy-mm-dd',
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
            var i, selRowIds = $('#ShipRequest').jqGrid("getGridParam", "selarrrow"), n, rowData;
            for (i = 0, n = selRowIds.length; i < n; i++) {
                var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
                var id = ret.wmtid;
                var id2 = n;

                var reg = ret.reg
                fruits.push(reg);
            };
            var mt_qty = id2;
            $.ajax({
                url: "/Shippingmgt/Deatil_mt?" + "id=" + selRowIds + "&" + "reg_qty=" + fruits,
                type: "get",
                dataType: "json",
                success: function (result) {
                    $("#State").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                }
            });
        });

//LƯỚI view all
$grid = $("#w_mr").jqGrid({
    url: '/Shippingmgt/mr_info',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'mrid', name: 'mrid', width: 50, hidden: true },
        { label: 'Reg No', name: 'mr_no', width: 110, align: 'center' },
        { label: 'Loaction', name: 'lct_cd', width: 180, align: 'center' },
        { label: 'Requester', name: 'worker_id', width: 110, align: 'left' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left', },
        { label: 'Req Receive Date', name: 'req_rec_dt', width: 110, align: 'center' },
        { label: 'Real Receive Date', name: 'real_rec_dt', width: 110, align: 'center' },
        { label: 'Mt Qty', name: 'mt_qty', width: 110, align: 'right', },
        { label: 'Related Bom', name: 'rel_bom', width: 110, align: 'center', },
        { label: 'Remark', name: 'remark', width: 110, align: 'left', },
        { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
       { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
       { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#w_mrpage',
    rownumbers: true,
    caption: 'Req Info',
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 250,
    jsonReader:
  {
      root: "rows",
      page: "page",
      total: "total",
      records: "records",
      repeatitems: false,
      Id: "0"
  },
    width: $(".box-body").width() - 5,
    autowidth: false,
    onSelectRow: function (rowid, status, e) {

    },
});
        
$("#Request").click(function () {
    if ($("#whouse").val().trim() == "") {
        alert("please select the location");
        $("#whouse").val("");
        $('#whouse').focus();
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
    if ($("#uname2").val().trim() == "") {
        alert("please enter name");
        $("#uname2").val("");
        $('#uname2').focus();
        return false;
    }
    if ($("#userid2").val().trim() == "") {
        alert("please enter  Manager");
        $("#userid2").val("");
        $('#userid2').focus();
        return false;
    }
    if ($("#req_rec_dt").val().trim() == "") {
        alert("please enter  Req Receive Date ");
        $("#req_rec_dt").val("");
        $('#req_rec_dt').focus();
        return false;
    }
    if ($("#remark").val().trim() == "") {
        alert("please enter the remark ");
        $("#remark").val("");
        $('#remark').focus();
        return false;
    }
    if ($("#rel_bom").val().trim() == "") {
        alert("please enter  Related Bom ");
        $("#rel_bom").val("");
        $('#rel_bom').focus();
        return false;
    }
    else {
             
        var fruits = [];
        var i, selRowIds = $('#ShipRequest').jqGrid("getGridParam", "selarrrow"), n, rowData;
        for (i = 0, n = selRowIds.length; i < n; i++) {
            var ret = $("#ShipRequest").jqGrid('getRowData', selRowIds[i]);
            var id = ret.wmtid;
            var id2 = n;
            var reg = ret.reg
            fruits.push(reg);
        };
        //alert(reg);

        //insert w_mr_detail
        $.ajax({
            type: "post",
            dataType: 'text',
            url: "/Shippingmgt/insertw_mr_detail?" + "id=" + selRowIds + "&" + "req_qty=" + fruits,
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            cache: false,
        });


        var mt_qty = $("#State").jqGrid("getGridParam", 'records');
        $.ajax({
            type: "post",
            dataType: 'text',
            url: "/Shippingmgt/insert_mr_info",
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            cache: false,
            data: JSON.stringify({
                whouse: $('#whouse').val(),
                worker: $('#userid').val(),
                mt_qty: mt_qty,
                manager: $('#userid2').val(),
                req_rec_dt: $('#req_rec_dt').val(),
                rel_bom: $('#rel_bom').val(),
                remark: $('#remark').val(),
            }),
            success: function (data) {
                jQuery("#w_mr").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
                jQuery("#ShipRequest").setGridParam({ rowNum: 200, datatype: "json" }).trigger('reloadGrid');
                $("#rel_bom").val("");
                $("#remark").val("");
                $("#userid").val("");
                $("#userid2").val("");
                $("#whouse").val("");
                $("#State").setGridParam({ serach: false, searchdata: {}, page: 1 }).trigger("reloadGrid");

                //document.getElementById("new1").reset();
            }
        });
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
}

$(".poupdialogBom").click(function () {
    $('.dialogBom').dialog('open');
});
$('#closeBom').click(function () {
    $('.dialogBom').dialog('close');
});