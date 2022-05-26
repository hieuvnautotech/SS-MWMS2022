$grid = $("#list").jqGrid({
    url: "/ShippingMgt/GetSd",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { label: 'MT NO', name: 'mt_no', width: 110, align: 'center' },
        { label: 'Name', name: 'mt_nm', width: 110, align: 'left' },
        { label: 'Total qty', name: 'total_qty', width: 110, align: 'right' },
        {
            label: 'Req qty', name: 'req_qty', width: 110, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
            formatter: 'integer', editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                }
            }
        },
        { label: 'width', name: 'width', width: 110, align: 'right' },
        { label: 'spec', name: 'spec', width: 110, align: 'right' },
        { label: 'Available qty', name: 'ff', width: 110, align: 'right', formatter: 'integer' },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#gridpager',
    height: 250,
    width: $(".boxlist1").width() - 5,
    autowidth: false,
    rownumbers: true,
    multiselect: true,
    multiPageSelection: true,
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 350,
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
    onSelectRow: editRow,
});

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".boxlist1").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
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

                html += '<option value=' + item.index_cd + '>' + item.index_cd + '</option>';
            });
            $("#lct_cd").html(html);
        }
    });
}

$grid = $("#list2").jqGrid({
    url: "/ShippingMgt/GetSdmt",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'sid', name: 'sid', width: 50, hidden: true },
        { label: 'bom_no', name: 'bom_no', width: 110, align: 'center', hidden: true },
        { label: 'rd_no', name: 'rd_no', width: 110, align: 'center', hidden: true },
        { label: 'SD NO', name: 'sd_no', width: 110, align: 'center' },
        { label: 'Location', name: 'lct_nm', width: 200, align: 'left' },
        { label: 'Worker', name: 'worker_id', width: 200, align: 'left' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'left' },
        { label: 'Work date', name: 'work_dt', width: 110, align: 'center' },
        { label: 'Real Work Date', name: 'real_work_dt', width: 110, align: 'center' },
        { label: 'Creat user', name: 'reg_id', width: 110, align: 'center' },
        { label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d h:i A" } },
        { label: 'Change user', name: 'chg_id', width: 110, align: 'center' },
        { label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d h:i A" } },
    ],
    loadonce: true,
    shrinkToFit: false,
    autowidth: false,
    pager: '#list2Page',
    height: 250,
    width: $(".boxlist2").width() - 5,
    rowNum: 2000,
    rownumbers: true,
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 300,
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
    onSelectRow: function (rowid, status, e) {

    },
});

$('#list2').jqGrid('setGridWidth', $(".boxlist2").width());
$(window).on("resize", function () {
    var newWidth = $("#list2").closest(".ui-jqgrid").parent().width();
    $("#list2").jqGrid("setGridWidth", newWidth, false);
});

$("#work_date").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#work_date").datepicker({ dateFormat: 'yy-mm-dd' }).datepicker('setDate', 'today');

//search
$("#searchBtn").click(function () {
    $.ajax({
        url: "/ShippingMgt/searchGeneration",
        type: "get",
        dataType: "json",
        data: {
            mr_no: $('#mr_no').val().trim(),
            bom_no: $('#bom_no').val().trim(),
            mt_no: $('#mt_no').val().trim(),
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
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
    $("#Direction").click(function () {
        if ($("#lct_cd").val().trim() == "") {
            alert("please select the location");
            $("#lct_cd").val("");
            $('#lct_cd').focus();
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
            alert("please enter the Manager");
            $("#userid2").val("");
            $('#userid2').focus();
            return false;
        }
        if ($("#work_date").val().trim() == "") {
            alert("please enter the Work date");
            $("#work_date").val("");
            $('#work_date').focus();
            return false;
        }
        if ($("#mr_no1").val().trim() == "") {
            alert("please enter the MR No");
            $("#mr_no1").val("");
            $('#mr_no1').focus();
            return false;
        }
        else {
            var lct_cd = $('#lct_cd').val();
            var worker = $('#userid').val();
            var manager = $('#userid2').val();
            var work_dt = $('#work_date').val();
            var mr_no = $('#mr_no1').val();


            var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData, v_req_qty = "";
            for (i = 0, n = selRowIds.length; i < n; i++) {
                v_req_qty = getCellValue(selRowIds[i], 'req_qty');
            }
            {
                $.ajax({
                    url: "/ShippingMgt/insertSdinfo?id=" + selRowIds,
                    type: "get",
                    dataType: "json",
                    data: {
                        lct_cd: lct_cd,
                        worker_id: worker,
                        manager_id: manager,
                        work_dt: work_dt,
                        mr_no: mr_no,
                        req_qty: v_req_qty,
                    },
                    success: function (data) {
                        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                        jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                    }
                });

            };
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

