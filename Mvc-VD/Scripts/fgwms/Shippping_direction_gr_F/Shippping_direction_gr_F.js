$(function () {

    // grid 1
    $grid_1 = $("#list").jqGrid({
        url: '/fgwms/Getso_list_gr_F',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'sno', name: 'sno', width: 50, hidden: true },
            { label: 'SO', name: 'ship_no', width: 110, align: 'center' },
            { label: 'SM', name: 'sm_no', width: 110, align: 'center' },
            { label: 'Product Code', name: 'style_no', width: 180, align: 'left' },
            { label: 'Destination Code', name: 'dest_cd', width: 180, align: 'left', hidden: true },
            { label: 'Destination', name: 'dest_nm', width: 180, align: 'left', },
            { label: 'Aavaible Qty', name: 'available', width: 110, align: 'right', hidden: true },
            { label: 'Deliver Qty', name: 'so_qty', width: 110, align: 'right', formatter: 'integer' },
            { label: 'Delivery Date', name: 'delivery_dt', width: 100, align: 'center' },
            { label: 'Remark', name: 're_mark', width: 200 },
            { label: 'Transferred', name: 'transferred_yn', width: 200, hidden: true },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        loadonce: true,
        shrinkToFit: false,
        pager: '#listPager',
        rowNum: 50,
        width: null,
        rownumbers: true,
        caption: 'SO List',
        multiselect: true,
        multiboxonly: true,
        multiPageSelection: true,
        rowList: [50, 80, 100, 120],
        loadonce: true,
        viewrecords: true,
        height: 360,
        jsonReader:
      {
          root: "rows",
          page: "page",
          total: "total",
          records: "records",
          repeatitems: false,
          Id: "0"
      },
      
        autowidth: false,
        onSelectRow: function (rowid, status, e) {
            $("#Request").attr("disabled", false);
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            rowid = $("#list").getRowData(selectedRowId);
            if (rowid.transferred_yn != "N") {
                $("#Request").attr("disabled", true);
            }
        },
        loadComplete: function () {
            var rows = $("#list").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var sno = $("#list").getCell(rows[i], "sno");
                var status = $("#list").getCell(rows[i], "transferred_yn");

                if (status == "Y") {
                    $("#list").jqGrid('setRowData', rows[i], false, { background: '#e3d9bc' });
                    $('#jqg_list_' + sno).attr("disabled", true);//.addClass("hidden");
                    $('#jqg_list_' + sno).addClass("hidden");
                }
            }
        },
    });// grid 1

    // grid 2
    $grid_2 = $("#list2").jqGrid({
        url: '/fgwms/Getsd_info_F',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'psid', name: 'psid', width: 50, hidden: true },
            { label: 'PSD No', name: 'psd_no', width: 110, align: 'center' },
            { label: 'Destination', name: 'lct_nm', width: 200, align: 'left' },
            { label: 'Destination', name: 'dest_lct_cd', width: 200, align: 'left', hidden: true },
            { label: 'PSD CNT', name: 'psd_cnt', width: 110, align: 'right' },
            { label: 'Deliver Qty', name: 'reg_ty', width: 110, align: 'right' },
            { label: 'Worker', name: 'worker_id', width: 110, align: 'left' },
            { label: 'Manager', name: 'manager_id', width: 110, align: 'left' },
            { label: 'Worker Date', name: 'work_dt', width: 110, align: 'center', formatter: formatdate },
        ],
        pager: jQuery('#list2Page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        loadonce: true,
        height: '220',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
     
        multiselect: false,
    });// grid 2


});

//get select
// Khai báo URL stand của bạn ở đây
var getadmin = "/ReceivingMgt/getadmin";
$(document).ready(function () {
    _getwhouse();

});

function _getwhouse() {
    $.get("/fgwms/lct_005", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"> * Detination *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#whouse").html(html);
            $("#lct_cd").html(html);
        }
    });
}

//search
$("#searchBtn").click(function () {
    $.ajax({
        //url: "/fgwms/searchShippping_direction_gr_F",
        url: "/fgwms/Getso_list_gr_F",
        type: "get",
        dataType: "json",
        data: {
            sm: $('#sm').val().trim(),
            so: $('#so').val().trim(),
            lct_cd: $('#lct_cd').val(),
            style_no: $('#style_no').val().trim(),
            //start: $('#start').val().trim(),
            //end: $('#end').val().trim(),
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});


$("#Request").click(function () {
    $('#loading').show();
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
        alert("please enter the Worker");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    }
    else {
        var selRowIds = $("#list").jqGrid("getGridParam", "selarrrow");
        var mt_qty = selRowIds.length;
        var req_qtys = [];
        for (var i = 0; i < mt_qty; i++) {
            var qty = $('#list').jqGrid('getCell', selRowIds[i], 'so_qty');
            if (qty == null || qty == undefined) {
                qty = 0;
            }
            req_qtys.push(qty);
        }
        var dest_lct_cd = $('#whouse').val();
        var worker_id = $('#userid').val();
        var manager_id = $('#userid2').val();
        var work_dt = $('#req_rec_dt').val();
        var re_mark = $('#remark').val();
        work_dt = work_dt.replace("-", "");
        work_dt = work_dt.replace("-", "");
        $.ajax({
            url: "/fgwms/insert_psd_F?" + "id=" + selRowIds + "&req_qtys=" + req_qtys,
            type: "post",
            dataType: "json",
            data: {
                dest_lct_cd: dest_lct_cd,
                worker_id: worker_id,
                manager_id: manager_id,
                work_dt: work_dt,
                mt_qty: mt_qty,
                re_mark: re_mark
            },
            success: function (response) {
                if (response.result) {
                    var id = response.data.psid;
                    $("#list2").jqGrid('addRowData', id, response.data, 'first');
                    $("#list2").setRowData(id, false, { background: "#d0e9c6" });

                    for (var i = 0; i < response.info.length; i++) {
                        var id2 = response.info[i].sno;
                        var rowData = jQuery('#list').jqGrid('getRowData', id2);
                        rowData.transferred_yn = "Y";
                        $("#list").setRowData(id2, rowData, { background: "#d0e9c6" });
                        $('#jqg_list_' + id2).attr("disabled", true);
                        $('#jqg_list_' + id2).addClass("hidden");
                    }

                    $('#whouse').val();
                    $('#userid').val();
                    $('#userid2').val();
                    $('#work_date').val();
                    $('#loading').hide();
                    alert(response.message);
                    return true;
                }
                else {
                    $('#loading').hide();
                    alert(response.message);
                    return false;
                }
            },
            error: function () {
                $('#loading').hide();
                alert('Something went wrong. Please call Ms. Nga for more information !');
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
        if (data != null && data != undefined && data.length != 0) {
            $.each(data, function (key, item) {
                var html = '';
                html += '<input value="' + item.uname + '" readonly="readonly"  placeholder="' + item.uname + '" class="input-text form-control" id="uname">';
                $("#new").html(html);
            });
        }
        else {
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
        }
        else {
            var html = '';
            html += '<input value="" readonly="readonly"  placeholder="" class="input-text form-control" id="uname2">';
            $("#new1").html(html);
        }
    });
}


function formatdate(cellValue, options, rowdata, action) {
    if (cellValue != null && cellValue != "") {
        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);
        var html = a + "-" + b + "-" + c;
        return html;
    }
    else {
        var html = "";
        return html;
    }
};

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});

$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});

$("#req_rec_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});

$('#req_rec_dt').datepicker().datepicker('setDate', 'today');
