var grid = $('#list');
function QtyFormatter(cellValue, options, rowdata, action) {
    return `${rowdata.Qty} ${rowdata.Unit}`;
}
function LengthFormatter(cellValue, options, rowdata, action) {
    if (rowdata.Unit == `EA`) {
        return ``;
    }
    return `${rowdata.Length}m`;
}


/////
//GRID
function Grid() {
    grid.jqGrid({
        url: `/TIMS/getInventoryInfo`,
        mtype: `GET`,
        datatype: `json`,
        colModel: [
            { name: 'Length', align: 'center', hidden: true },
            { label: 'Model', name: 'md_cd', width: 180, align: 'left' },
            { label: 'Product Code', name: 'product_cd', width: 180, align: 'left' },
            { label: 'Type(Roll/Sheet)', name: 'bom_type', width: 180, align: 'left' },
            { label: 'Name', name: 'product_nm', width: 180, align: 'left' },
            { label: 'Hàng chờ kiểm', name: 'HCK', width: 180,align: 'right', formatter: 'integer'},
            { label: 'Hàng đang kiểm tra', name: 'DKT', width: 180, align: 'right', formatter: 'integer' },
            { label: 'Hàng Chờ vào OQC', name: 'CKT', width: 180, align: 'right', formatter: 'integer'},
            { label: 'Hàng đóng gói', name: 'HDG', width: 180, align: 'right', formatter: 'integer' },
            { label: 'Hàng Mapping buyer', name: 'MAPPINGBUYER', width: 150, align: 'right', formatter: 'integer' },
            { label: 'Total', name: 'Qty', width: 180, align: 'right', formatter: total, },
            { label: 'Sorting', name: 'SORTING', width: 150, align: 'right', formatter: 'integer' },

        ],
        pager: '#listPager',
        height: 700,
        width: null,
        rowNum: 50,
        rowList: [5, 10, 20, 40, 100],
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: true,
        multiselect: false,
        multiboxonly: false,
        autoResizing: true,
        viewrecords: true,
        caption: 'Inventory Info',
        jsonReader:
        {
            root: "listInfo",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        subGrid: true,
        subGridRowExpanded: showChildGrid,
        //gridComplete: function () {
        //    $("tr.jqgrow:odd").css("background", "white");
        //    $("tr.jqgrow:even").css("background", "#f7f7f7");
        //    $('.loading').hide();

        //    //$("#GeneralGrid").jqGrid('hideCol', 'setSelection');
        //},


    });
}

function bntCellValue(cellValue, options, rowdata, action) {

    var MaterialCode = rowdata.MaterialCode;
    var Id = rowdata.Id;
    var html = `<button class="btn btn-xs btn-primary" data-materialcode="${MaterialCode}" data-id_actual="${Id}" onclick="destroy(this);">Destroy</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};

function destroy(e) {
    var r = confirm("Bạn có chắc muốn Hủy mã này không???");
    if (r) {
        var materialcode = $(e).data("materialcode");
        var id = $(e).data("id_actual");
        $.ajax({
            url: '/TIMS/destroyLotCode?id=' + id + '&MaterialCode=' + materialcode,
            type: "get",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data.result == true) {
                    SuccessAlert(data.message);
                    var id = data.data[0].id;
                    $("#" + childGridID).setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });
                }
                else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
    else {
        return false;
    }
}

function bntCellValue2(cellValue, options, rowdata, action) {

    var MaterialCode = rowdata.MaterialCode;
    var Id = rowdata.Id;
    var html = `<button class="btn btn-xs btn-primary" data-materialcode="${MaterialCode}" data-id_actual="${Id}" onclick="redo(this);">Redo</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};

function redo(e) {
    var r = confirm("Bạn có chắc muốn Redo mã này không???");
    if (r == true) {
        var materialcode = $(e).data("materialcode");
        var id = $(e).data("id_actual");
        $.ajax({
            url: '/TIMS/redoLotCode?id=' + id + '&MaterialCode='+materialcode,
            type: "get",
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    $("#" + wmtid).css({ background: "#28a745", color: "#fff" });
                }
                else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
    else {
        return false;
    }
}


function total(cellValue, options, rowdata, action) {
    var total = rowdata.HCK + rowdata.DKT + rowdata.CKT + rowdata.HDG;
    return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

var childGridID = ``;
function showChildGrid(parentRowID, parentRowKey) {


    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    //wmtid_DT = parentRowKey;

    var status = $(`#status`).val();
    var at_no = $(`#s_at_no`).val() == null ? `` : $(`#s_at_no`).val().trim();
    var rdata = grid.getRowData(parentRowKey);

    $(`#${childGridID}`).jqGrid({
        url: `/TIMS/GetInventoryDetail?ProSpecific=${rdata.product_cd}&prd_cd=${s_prd_cdTemp}&mtCode=${mtCodeTemp}&sVBobbinCd=${sVBobbinCdTemp}&recDateStart=${recDateStartTemp}&recDateEnd=${recDateEndTemp}&status=${status}&po=${at_no}`,
        //url: `/TIMS/GetInventory?view=2&mtNoSpecific=${rdata.MaterialNo}&mtCode=${mtCodeTemp}&recDate=${recDateTemp}`,
        mtype: `GET`,
        //page: 1,
        datatype: `json`,
        colModel: [
            { name: "DT", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue },
            { name: "RD", width:40, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue2 },
            { name: 'Id', key: true, hidden: true },
            { name: 'Length', hidden: true },
            { name: 'Qty', hidden: true },
            { name: 'Unit', hidden: true },
            { name: 'StatusCode', hidden: true },
            { name: 'VBobbinCd', label: 'Container', align: 'center', width: 230  },
            { label: 'PO', name: 'at_no', align: 'center', width: 110 },
            { label: 'Buyer QR', name: 'buyer_qr', align: 'center', width: 220 },
            { name: 'MaterialCode', label: 'Composite Code', align: 'center', width: 300 },
            { name: '', label: 'Q`ty(Roll/EA)', align: 'right', width: 95, formatter: QtyFormatter },
            { name: 'StatusName', label: 'Status', width: 110, align: 'center' },
            { name: 'ReceivedDate', label: 'Received Date', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }   },
        ],
        loadComplete: function () {
            var check_author = $("#authoryty_readonly").val();
            if (check_author == "read") {
                $(`#${childGridID}`).jqGrid('hideCol', ["DT"]);
                $(`#${childGridID}`).jqGrid('hideCol', ["RD"]);
            }
        },
        shrinkToFit: false,
        width: null,
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 50,
        loadonce: false,
        rownumbers: true,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
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
}

//SEARCH FUNCTION
var mtNoTemp = ``;
var mtCodeTemp = ``;
var recDateTemp = ``;

var sVBobbinCdTemp = '';
var recDateStartTemp = '';
var recDateEndTemp = '';
var s_prd_cdTemp = '';

$(`#searchBtn`).on(`click`, function () {
    var mtCode = $(`#sMaterialCode`).val() == null ? `` : $(`#sMaterialCode`).val().trim();
    var model = $(`#s_model`).val() == null ? `` : $(`#s_model`).val().trim();
    var sVBobbinCd = $(`#sVBobbinCd`).val() == null ? `` : $(`#sVBobbinCd`).val().trim();
    var recDateStart = $(`#recevice_dt_start`).val() == null ? `` : $(`#recevice_dt_start`).val().trim();
    var recDateEnd = $(`#recevice_dt_end`).val() == null ? `` : $(`#recevice_dt_end`).val().trim();
    var s_prd_cd = $(`#s_prd_cd`).val() == null ? `` : $(`#s_prd_cd`).val().trim();
    var bom_type = $(`#s_bom_type`).val() == null ? `` : $(`#s_bom_type`).val().trim();
    var at_no = $(`#s_at_no`).val() == null ? `` : $(`#s_at_no`).val().trim();
    var status = $(`#status`).val();
    $.ajax({
        url: `/TIMS/getInventoryInfo?mtCode=${mtCode}&sVBobbinCd=${sVBobbinCd}&recDateStart=${recDateStart}&recDateEnd=${recDateEnd}&prd_cd=${s_prd_cd}&po=${at_no}&status=${status}&bom_type=${bom_type}&model=${model}&view=1`,
    })
        .done(function (response) {
            s_prd_cdTemp = s_prd_cd;
            mtCodeTemp = mtCode;

            sVBobbinCdTemp = sVBobbinCd;

            recDateStartTemp = recDateStart;

            recDateEndTemp = recDateEnd;
            grid.jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response.listInfo }).trigger("reloadGrid");
            return;
        })
        .fail(function () {
            ErrorAlert("System Error.");
            return;
        });
});

$(`#excelBtn`).on(`click`, function () {
    var mt_cd = $(`#sMaterialCode`).val() == null ? `` : $(`#sMaterialCode`).val().trim();
    var model = $(`#s_model`).val() == null ? `` : $(`#s_model`).val().trim();
    var VBobbinCd = $(`#sVBobbinCd`).val() == null ? `` : $(`#sVBobbinCd`).val().trim();
    var startDate = $(`#recevice_dt_start`).val() == null ? `` : $(`#recevice_dt_start`).val().trim();
    var endDate = $(`#recevice_dt_end`).val() == null ? `` : $(`#recevice_dt_end`).val().trim();
    var prd_cd = $(`#s_prd_cd`).val() == null ? `` : $(`#s_prd_cd`).val().trim();
    var bom_type = $(`#s_bom_type`).val() == null ? `` : $(`#s_bom_type`).val().trim();
    var po = $(`#s_at_no`).val() == null ? `` : $(`#s_at_no`).val().trim();
    var status = $(`#status`).val();
    //$.ajax({
    //    url: `/TIMS/PrintExcelFile?mtCode=${mtCode}&mtNo=${mtNo}&mtName=${mtName}&recDate=${recDate}`,
    //});

    $('#exportData').attr('action', `/TIMS/PrintExcelFile?status=${status}&po=${po}&model=${model}&prd_cd=${prd_cd}&bom_type=${bom_type}&mt_cd=${mt_cd}&VBobbinCd=${VBobbinCd}&endDate=${endDate}&startDate=${startDate}`);
    //.done(function (response)
    //{
    //    mtNoTemp = response.mtNoTemp;
    //    if (!mtCode)
    //    {
    //        mtCodeTemp = ``;
    //    }
    //    else
    //    {
    //        mtCodeTemp = mtCode;
    //    }
    //    if (!recDate)
    //    {
    //        recDateTemp = ``;
    //    }
    //    else
    //    {
    //        recDateTemp = recDate;
    //    }
    //    grid.jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response.listInfo }).trigger("reloadGrid");
    //    return;
    //})
    //.fail(function ()
    //{
    //    ErrorAlert("System Error.");
    //    return;
    //});
});

function setDatePicker() {
    $("#recevice_dt_start").empty();
    $("#recevice_dt_start").datepicker({
        dateFormat: 'yy-mm-dd'
    });

    $("#recevice_dt_end").empty();
    $("#recevice_dt_end").datepicker({
        dateFormat: 'yy-mm-dd'
    });
}

//DOCUMENT READY
$(function () {
    setDatePicker();
    Grid();
});

$("#sVBobbinCd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var BobbinCd = e.target.value.trim();


        $.ajax({
            url: "/TIMS/Search_bbTimsGeneral?bb_no=" + BobbinCd,
            type: "get",
            dataType: "json",


            success: function (response) {
                if (response.result) {




                    var id = response.data.wmtid;

                    $("#" + childGridID).setRowData(id, response.data, { background: "#28a745", color: "#fff" });

                }
                else {
                    ErrorAlert(response.message);
                }
            },
            error: function () {

                $(".thongbao").addClass("active");
                $(".thongbao").removeClass("hidden");
                document.getElementById("noidung").innerHTML = (response.message);

            }
        });


    }
});
