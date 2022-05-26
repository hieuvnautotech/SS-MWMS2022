$grid = $("#list").jqGrid({
    url: "/ShippingMgt/GetSd",
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', width: 50, hidden: true },
        { label: 'MT NO', name: 'mt_no', width: 110, align: 'center' },
        { label: 'Name', name: 'mt_nm', width: 110, align: 'center' },
        { label: 'Total qty', name: 'total_qty', width: 110, align: 'center' },
        {
            label: 'Req qty', name: 'req_qty', width: 110, align: 'center', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
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
        { label: 'width', name: 'width', width: 110, align: 'center' },
        { label: 'spec', name: 'spec', width: 110, align: 'center' },
        { label: 'Available qty', name: 'ff', width: 110, align: 'center', formatter: 'integer' },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#listPager',
    autowidth: false,
    rownumbers: true,
    multiselect: true,
    multiPageSelection: true,
    rowList: [60, 80, 100, 120],
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
    width: $(".box-body-1").width() - 5,
    autowidth: false,
    onSelectRow: editRow,
});

var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list");
        grid.jqGrid('restoreRow', lastSelection);
        grid.jqGrid('editRow', id, { keys: true });
        lastSelection = id;
    }
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
        { label: 'Location', name: 'lct_cd', width: 200, align: 'center' },
        { label: 'Worker', name: 'worker_id', width: 200, align: 'center' },
        { label: 'Manager', name: 'manager_id', width: 110, align: 'center' },
        { label: 'Worker date', name: 'worker_dt', width: 110, align: 'center' },
        { label: 'Worker Date', name: 'worker_dt', width: 110, align: 'center' },
        { label: 'Creat user', name: 'reg_id', width: 110, align: 'center' },
        {
            label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
            formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
        },
        { label: 'Change user', name: 'chg_id', width: 110, align: 'center' },
        {
            label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
            formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }
        },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#list2Pager',
    rownumbers: true,
    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 300,
    width: $(".box-body-2").width() - 5,
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

    },
});

$("#work_date").datepicker({
    format: 'yyyy-mm-dd',
});
$('#work_date').datepicker().datepicker('setDate', 'today');
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
 
    var lct_cd = $('#lct_cd').val();
    var worker = $('#userid').val();
    var manager = $('#userid2').val();
    var work_dt = $('#work_date').val();
    var mr_no = $('#mr_no1').val();

    var i, selRowIds = $('#list').jqGrid("getGridParam", "selarrrow"), n, rowData;

    {
        $.ajax({
            url: "/ShippingMgt/insertSdinfo?id=" + selRowIds,
            type: "get",
            dataType: "json",
            data: {
                lct_cd: lct_cd,
                worker_id: worker,
                manager_id: manager,
                worker_dt: work_dt,
                mr_no: mr_no,
            },
            success: function (data) {
                jQuery("#list2").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');

            }
        });

    };
})