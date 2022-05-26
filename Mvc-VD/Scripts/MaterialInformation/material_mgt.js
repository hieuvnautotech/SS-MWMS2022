var getFactory = "/MaterialInformation/Warehouse";
var GetType = "/MaterialInformation/GetType";
var GetLocationsts = "/MaterialInformation/GetLocationsts";
var GetStatus = "/MaterialInformation/GetStatus";

$(document).ready(function () {
    _getFactory();
    _GetType();
    _GetLocationsts();
    _GetStatus();

    //---getDay - 7
    var future = new Date(); // get today date
    future.setDate(future.getDate() - 7); // - 7 days
    var finalDate = future.getFullYear() + '-' + ((future.getMonth() + 1) < 10 ? '0' : '') + (future.getMonth() + 1) + '-' + ((future.getDate() + 1) < 10 ? '0' : '') + (future.getDate());
    document.getElementById('start').value = finalDate;
    //$('#start').val() = finalDate;
    //-end

    $("#start").datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });

    $("#end").datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    }).datepicker('setDate', 'today');

});

function _getFactory() {
    $.get(getFactory, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#c_lct_cd").html(html);
        }
    });
};

function _GetLocationsts() {
    $.get(GetLocationsts, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Location Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#lct_sts_cd").html(html);
        }
    });
};

function _GetStatus() {
    $.get(GetStatus, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Status*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#mt_sts_cd").html(html);
            $("#m_mt_sts_cd").html(html);
            $("#mt_sts_cd_pp").html(html);
            $("#mt_sts_cd_cg").html(html);
        }
    });
};

function _GetType() {
    $.get(GetType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_mt_type").html(html);
            $("#mt_type").html(html);
        }
    });
};

$(function () {
    $grid = $("#Material_jqgrid").jqGrid
   ({
       datatype: 'json',
       mtype: 'GET',
       colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },

            { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
            { label: 'Type', name: 'mt_type', width: 150, align: 'left' },
            { label: 'MT Cd', name: 'mt_cd_mt', width: 250, align: 'left' },
            { label: 'Lot NO', name: 'lot_no', width: 150, align: 'left' },
            { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
            { label: 'Group Unit', name: 'gr_qty_mt', width: 150, align: 'right', formatter: donvi1 },
            { label: 'Group Qty', name: 'gr_qty', width: 100, align: 'right', formatter: donvi },
            { name: 'gr_qty1', width: 100, align: 'right', hidden: true },
            { label: 'unit_cd', name: 'unit_cd', width: 150, align: 'right', hidden: true },
            { label: 'Status', name: 'status', width: 150, align: 'left' },
            { label: 'Status', name: 'mt_sts_cd1', width: 150, align: 'left', hidden: true },
            { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
            { label: 'Location Status', name: 'location_status', width: 150, align: 'left' },
            { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
            { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
            { label: 'Description', name: 'remark', width: 150, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
            { label: 'Origin ML No', name: 'orgin_mt_cd', width: 250, align: 'left' },
            { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       gridComplete: function () {
           //$("#Material_jqgrid").closest(".ui-jqgrid").find('.loading').hide();
           $('.loading').hide();
           $("tr.jqgrow:odd").css("background", "white");
           $("tr.jqgrow:even").css("background", "#f7f7f7");
       },

       //pager: jQuery('#Material_jqgridGridPager'),
       viewrecords: true,
       height: 400,
       rowNum: 50,
       rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
       rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
       // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
       loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
       emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
       gridview: true,
       shrinkToFit: false,
       multiselect: true,
       loadonce: false,
       datatype: function (postData) { getData(postData); },
       pager: "#Material_jqgridGridPager",
       jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
       ajaxGridOptions: { contentType: "application/json" },
       autowidth: true,

       onSelectRow: function (rowid, selected, status, e) {
           var selectedRowId = $("#Material_jqgrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#Material_jqgrid").getRowData(selectedRowId);

           var wmtid = row_id.wmtid;

           $('#wmtid').val(wmtid);
           $('#m_mt_cd').val(row_id.mt_cd);
           $('#mt_cd_cg').val(row_id.mt_cd);
           $('#mt_cd').val(row_id.mt_cd);
           $('#m_gr_qty').val(row_id.gr_qty1);
           $('#gr_qty_pp').val(row_id.gr_qty1);
           $('#gr_qty_cg').val(row_id.gr_qty1);
           $('#m_mt_sts_cd').val(row_id.mt_sts_cd1);
           $('#mt_sts_cd_pp').val(row_id.mt_sts_cd1);
           $('#mt_sts_cd_cg').val(row_id.mt_sts_cd1);
           $('#m_remark').val(row_id.remark);
           $('#remark_pp').val(row_id.remark);
           $('#remark_cg').val(row_id.remark);
           $('#mt_no_origin').val(row_id.mt_no_origin);
           $('#mt_no_origin_cg').val(row_id.mt_no_origin);

           $.get("/MaterialInformation/get_mt_no_gr?mt_no=" + row_id.mt_no, function (data) {
               if (data != null && data != undefined && data.length) {
                   var html = '';
                   html += '<option value="" selected="selected">*MT NO*</option>';
                   $.each(data, function (key, item) {
                       html += '<option value="' + item.mt_no + '">' + item.mt_no + '</option>';
                   });
                   $('#mt_no_cg').html(html);
               }
           });

       },

   });
});

function getData(pdata) {
    //$("#Material_jqgrid").closest(".ui-jqgrid").find('.loading').show();
    $('.loading').show();
    var mt_no = $("#c_mt_no").val().trim();
    var mt_nm = $("#s_mt_nm").val().trim();
    var mt_type = $("#s_mt_type").val();
    var mt_sts_cd = $("#mt_sts_cd").val();
    var lct_cd = $("#c_lct_cd").val();
    var lct_sts_cd = $("#lct_sts_cd").val();
    var mt_cd = $("#s_mt_cd").val().trim();
    var start = $("#start").val().trim();
    var end = $("#end").val().trim();
    var params = new Object();
    if (jQuery('#Material_jqgrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    //params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mt_no = mt_no;
    params.mt_nm = mt_nm;
    params.mt_type = mt_type;
    params.mt_sts_cd = mt_sts_cd;
    params.lct_cd = lct_cd;
    params.lct_sts_cd = lct_sts_cd;
    params.mt_cd = mt_cd;
    params.end = end;
    params.start = start;
    $("#Material_jqgrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/MaterialInformation/searchwmaterial_mgt",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            //console.log(data);

            if (st == "success") {
                var grid = $("#Material_jqgrid")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

$("#searchBtn").click(function () {
    $("#Material_jqgrid").clearGridData();

    $('.loading').show();

    var grid = $("#Material_jqgrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);

});

function donvi(cellvalue, options, rowobject) {
    var unit_cd = rowobject.unit_cd;
    var html = "";
    if (cellvalue == null) { cellvalue = ""; }
    if (unit_cd == null) { unit_cd = ""; }
    html = cellvalue + " " + unit_cd;
    return html;
};

function donvi1(cellvalue, options, rowobject) {
    var unit_cd = rowobject.unit_cd;
    var html = "";
    if (cellvalue == null) { cellvalue = ""; }
    if (unit_cd == null) { unit_cd = ""; }
    if (cellvalue.toString().indexOf(unit_cd) !== -1) { html = cellvalue; } else {
        html = cellvalue + " " + unit_cd;
    }

    return html;
};

$("#m_save_but").click(function () {
    var selRowId = $('#Material_jqgrid').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top Material  on the grid.");
        return false;
    } else {
        $.ajax({
            url: "/MaterialInformation/updateww_materi_mgt",
            type: "get",
            dataType: "json",
            data: {
                wmtid: $('#wmtid').val(),
                mt_cd: $('#m_mt_cd').val(),
                gr_qty: $('#m_gr_qty').val(),
                mt_sts_cd: $('#m_mt_sts_cd').val(),
                remark: $('#m_remark').val().trim(),
            },
            success: function (data) {
                var id2 = data.wmtid;
                var rowData = $('#Material_jqgrid').jqGrid('getRowData', id2);
                rowData.gr_qty = data.gr_qty;
                rowData.gr_qty1 = data.gr_qty;
                rowData.remark = data.remark;
                rowData.mt_sts_cd = data.mt_sts_cd;
                rowData.mt_sts_cd1 = data.orgin_mt_cd;
                $('#Material_jqgrid').jqGrid('setRowData', id2, rowData);
                $("#Material_jqgrid").setRowData(id2, false, { background: "#d0e9c6" });

            }
        });
    }
});

$("#save").click(function () {
    var selRowId = $('#Material_jqgrid').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top Material on the grid.");
        return false;
    } else {
        $.ajax({
            url: "/MaterialInformation/insert_update_split",
            type: "get",
            dataType: "json",
            data: {
                wmtid: $('#wmtid').val(),
                mt_cd: $('#m_mt_cd').val(),
                gr_qty: $('#gr_qty_pp').val(),
                mt_sts_cd: $('#mt_sts_cd_pp').val(),
                remark: $('#remark_pp').val().trim(),
            },
            success: function (response) {
                console.log(response);
                if (response.x) {
                    var id2 = response.x.wmtid;
                    var rowData1 = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    rowData1.gr_qty = response.x.gr_qty;
                    rowData1.gr_qty1 = response.x.gr_qty;
                    $('#Material_jqgrid').jqGrid('setRowData', id2, rowData1);
                    $("#Material_jqgrid").setRowData(id2, false, { background: "#d0e9c6" });

                    var rowData = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    rowData.gr_qty = response.b.gr_qty;
                    rowData.mt_cd = response.b.mt_cd;
                    rowData.gr_qty1 = response.b.gr_qty;
                    rowData.remark = response.b.remark;
                    rowData.mt_sts_cd = response.b.mt_sts_cd;
                    rowData.orgin_mt_cd = response.b.orgin_mt_cd;
                    rowData.mt_sts_cd1 = response.b.bb_no;
                    rowData.wmtid = response.b.wmtid;
                    $("#Material_jqgrid").jqGrid('addRowData', response.b.wmtid, rowData, 'first');
                    $("#Material_jqgrid").setRowData(response.b.wmtid, false, { background: "#d0e9c6" });

                    var rowData1 = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    $('.dialog_Split').dialog('close');
                } else {
                    alert(response.message);
                }
            }
        });
    }
});

$('#save_but').click(function () {
    $('.dialog_Split').dialog('open');
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
});

$('#save_change').click(function () {
    $('.dialog_change').dialog('open');
});

$(".dialog_change").dialog({
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
});

$("#save_cg").click(function () {

    var selRowId = $('#Material_jqgrid').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top Material on the grid.");
        return false;
    } else {
        $.ajax({
            url: "/MaterialInformation/insert_cutting_change",
            type: "get",
            dataType: "json",
            data: {
                wmtid: $('#wmtid').val(),
                mt_cd: $('#mt_cd_cg').val(),
                gr_qty: $('#gr_qty_cg').val(),
                mt_sts_cd: $('#mt_sts_cd_cg').val(),
                remark: $('#remark_cg').val().trim(),
                mt_no: $('#mt_no_cg').val(),
            },
            success: function (response) {
                console.log(response);
                if (response.x) {
                    console.log(response);
                    var id2 = response.x.wmtid;
                    var rowData1 = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    rowData1.gr_qty = response.x.gr_qty;
                    rowData1.gr_qty1 = response.x.gr_qty;
                    rowData1.mt_no = response.x.mt_no;
                    rowData1.mt_sts_cd = response.x.mt_sts_cd;
                    rowData1.mt_sts_cd1 = response.x.bb_no;
                    rowData1.mt_sts_cd1 = response.x.bb_no;
                    rowData1.reg_id = response.x.reg_id;
                    rowData1.reg_dt = response.x.reg_dt;
                    rowData1.chg_dt = response.x.chg_dt;
                    rowData1.chg_id = response.x.chg_id;
                    $('#Material_jqgrid').jqGrid('setRowData', id2, rowData1);
                    $("#Material_jqgrid").setRowData(id2, false, { background: "#d0e9c6" });

                    var rowData = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    rowData.mt_no = response.b.mt_no;
                    rowData.mt_cd_mt = response.b.mt_no;
                    rowData.gr_qty = response.b.gr_qty;
                    rowData.mt_cd = response.b.mt_cd;
                    rowData.gr_qty1 = response.b.gr_qty;
                    rowData.remark = response.b.remark;
                    rowData.mt_sts_cd = response.b.mt_sts_cd;
                    rowData.orgin_mt_cd = response.b.orgin_mt_cd;
                    rowData.mt_sts_cd1 = response.b.bb_no;
                    rowData.reg_id = response.b.reg_id;
                    rowData.reg_dt = response.b.reg_dt;
                    rowData.chg_dt = response.b.chg_dt;
                    rowData.chg_id = response.b.chg_id;
                    $("#Material_jqgrid").jqGrid('addRowData', response.b.wmtid, rowData, 'first');
                    $("#Material_jqgrid").setRowData(response.b.wmtid, false, { background: "#d0e9c6" });

                    var rowData1 = $('#Material_jqgrid').jqGrid('getRowData', id2);
                    $('.dialog_change').dialog('close');
                } else {
                    alert(response.message);
                }
            }
        });
    }
});

$("#PrintBtn").click(function () {
    var fruits = [];
    var i, selRowIds = $('#Material_jqgrid').jqGrid("getGridParam", "selarrrow"), n, rowData;
    for (i = 0, n = selRowIds.length; i < n; i++) {
        var ret = $("#Material_jqgrid").jqGrid('getRowData', selRowIds[i]);
        var wmtid = ret.wmtid
        fruits.push(wmtid);
    };
    window.open("/Lot/Printcomposite?" + "id=" + fruits, "PRINT", "width=500, height=800, left=0, top=100, location=no, status=no,")
});
