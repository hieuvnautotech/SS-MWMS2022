
$(function () {

    $grid = $("#Directions").jqGrid({
        url: '/wipwms/GetDirections_WIP',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'sdid', name: 'sdid', width: 50, hidden: true },
            { label: 'SD NO', name: 'sd_no', width: 110, align: 'center' },
            { label: 'MT No', name: 'mt_no', width: 180, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', width: 180, align: 'left' },
            { label: 'Status', name: 'md_sts_cd', width: 110, align: 'left' },
            { label: 'Qty', name: 'req_qty', width: 110, align: 'right' },
              { label: 'Width', name: 'width', width: 110, align: 'right' },
             { label: 'Length (M)', name: 'spec', width: 110, align: 'right' },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        loadonce: true,
        shrinkToFit: false,
        pager: '#DirectionsPage',
        rowNum: 2000,
        rownumbers: true,
        caption: 'Directions',
        multiselect: true,
        multiPageSelection: true,
        rowList: [60, 80, 100, 120],
        loadonce: true,
        viewrecords: true,
        height: 280,
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
});

//get select
// Khai báo URL stand của bạn ở đây
var getadmin = "/wipwms/getadmin";


//LƯỚI LƯU
$(function () {
    $grid = $("#State").jqGrid({
        url: '/wipwms/Getw_rd_info_WIP',
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            { key: true, label: 'rid', name: 'rid', width: 50, hidden: true },
            { label: 'RD No', name: 'rd_no', width: 110, align: 'center' },
             {
                 label: 'Status', name: 'rd_sts_cd', width: 100, align: 'center', cellattr: function (rowId, value, rowObject, colModel, arrData) {
                     if (value == 'Indication') {
                         return 'style=background-color:#ffe08c;';
                     }
                     if (value == 'Finish') {
                         return 'style=background-color:#b7f0b1;';
                     }
                     else {
                         return value;
                     }
                 }
             },
            { label: 'Location', name: 'lct_cd', width: 200, align: 'left' },
            { label: 'Worker', name: 'worker_id', width: 110, align: 'left' },
            { label: 'Manager', name: 'manager_id', width: 110, align: 'left' },
            { label: 'Worker Date', name: 'worker_dt', width: 110, align: 'center' },
            { label: 'Mt Qty', name: 'mt_qty', width: 110, align: 'right' },
            { label: 'Creat user', name: 'reg_id', width: 110, align: 'center' },
          {
              label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
              formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
          },
        ],
        pager: jQuery('#StatePage'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        loadonce: true,
        height: '220',
        caption: 'Status',
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
        autowidth: true,
        multiselect: false,
    });
});


$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$("#work_date").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#work_date').datepicker().datepicker('setDate', 'today');
//search
var wh = "/wipwms/lct_destination_WIP";
$(document).ready(function () {
    _getwhouse();
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
            html += '<option value="" selected="selected"> * Detination *</option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#lct_cd").html(html);
            $("#whouse").html(html);
        }
    });
}
$("#searchBtn").click(function () {
    var sd_no = $('#sd_no').val().trim();
    var lct_cd = $('#lct_cd').val();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    $.ajax({
        url: "/wipwms/searchsd_WIP",
        type: "get",
        dataType: "json",
        data: {
            sd_no: sd_no,
            lct_cd: lct_cd,
            start: start,
            end: end,
        },
        success: function (result) {
            $("#Directions").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});


$("#Direction").click(function () {
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
        alert("please enter the User");
        $("#uname").val("");
        $('#uname').focus();
        return false;
    }
    else {
        var i, n, rowData, selRowIds = $("#Directions").jqGrid("getGridParam", "selarrrow");
       
        var mt_qty = selRowIds.length;
        $.ajax({
            url: "/wipwms/insert_rd_info_WIP?" + "id=" + selRowIds,
            type: "post",
            dataType: "json",
            data: {
                lct_cd: $('#whouse').val(),
                worker_id: $('#userid').val(),
                manager_id: $('#userid2').val(),
                worker_dt: $('#work_date').val(),
                mt_qty: mt_qty,
            },
            success: function (data) {
                console.log(data);
                var id = data.rid;
                $("#State").jqGrid('addRowData', id, data, 'first');
                $("#State").setRowData(id, false, { background: "#d0e9c6" });

                jQuery("#ShipRequest").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                $('#whouse').val();
                 $('#userid').val();
                 $('#userid2').val();
                $('#work_date').val();
                },
            error: function (data) {

                alert('The Code is the same. Please check again.');
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
