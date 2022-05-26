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
        url: `/TIMS/GetInventory?view=1`,
        mtype: `GET`,
        datatype: `json`,
        colModel: [
            { name: 'Length', align: 'center', hidden: true },
            { label: 'Product Code', name: 'product_cd', width: 400, align: 'left' },
            { name: 'Size', align: 'center', hidden: true },

            { name: 'Unit', align: 'center', hidden: true },
            { name: 'Qty', align: 'center', hidden: true },


            { name: 'MaterialNo', label: 'Code', align: 'left' },
            { name: 'MaterialName', label: 'Name', align: 'left' },
            { name: '', label: 'Q`ty(Roll/EA)', align: 'right', formatter: QtyFormatter },
            { name: 'MateriaName', label: 'Length', align: 'right', formatter: LengthFormatter },
            { name: 'VBobbinCd', label: 'Container', align: 'left', hidden: true },

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
        //loadui: 'disable',
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
        gridComplete: function () {
            var rows = grid.getDataIDs();
        },
        loadComplete: function () {
            var rows = grid.getDataIDs();
            //var id;
            for (var id of rows) {
                var rowData = grid.getRowData(id);
                var materialNumber = rowData.MaterialNo;
                if (mtNoTemp && materialNumber.includes(mtNoTemp)) {
                    grid.jqGrid('setSelection', id, true);
                    grid.expandSubGridRow(id);
                }
            }
        },
        onSelectRow: function (rowid, status, e, iRow, iCol) {
            var selectedRowId = grid.jqGrid("getGridParam", 'selrow');
            var rowData = grid.getRowData(selectedRowId);
        }
    });
}

function bntCellValue(cellValue, options, rowdata, action) {

    //var at_no = rowdata.Id;
    var Id = rowdata.Id;
    var html = `<button class="btn btn-xs btn-primary" data-id_actual="${Id}" onclick="destroy(this);">Destroy</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};

function destroy(e) {
    //var id = $(e).data("id_actual");
    //var id2 = $(e).data("id_actual");
    $.ajax({
        url: '/TIMS/destroyLotCode?id=' + $(e).data("id_actual"),
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                console.log(data.data);

                var id = data.data[0].Id;
                console.log(id);
                $("#" + childGridID).setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });
                //console.log(data.data[0]);


                //$("#list").jqGrid('delRowData', id);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
}

function bntCellValue2(cellValue, options, rowdata, action) {

    //var at_no = rowdata.Id;
    var Id = rowdata.Id;
    var html = `<button class="btn btn-xs btn-primary" data-id_actual="${Id}" onclick="redo(this);">Redo</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
};

function redo(e) {
    //var id = $(e).data("id_actual");
    //var id2 = $(e).data("id_actual");
    $.ajax({
        url: '/TIMS/redoLotCode?id=' + $(e).data("id_actual"),
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                console.log(data.data);

                var id = data.data[0].Id;
                console.log(id);
                $("#" + childGridID).setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });
                //console.log(data.data[0]);


                //$("#list").jqGrid('delRowData', id);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
}



var childGridID = ``;
function showChildGrid(parentRowID, parentRowKey) {
    var rdata = grid.getRowData(parentRowKey);
    childGridID = `${parentRowID}_table`;
    var childGridPager = `${parentRowID}_pager`;
    $(`#${parentRowID}`).append(`<table id="${childGridID}"></table><div id="${childGridPager}" class="scroll"></div>`);

    $(`#${childGridID}`).jqGrid({
        url: `/TIMS/GetInventory?view=2&mtNoSpecific=${rdata.MaterialNo}&mtCode=${mtCodeTemp}&recDate=${recDateTemp}`,
        mtype: "GET",
        datatype: `json`,
        async: false,
        page: 1,
        colModel: [
            { name: "", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue },
            { name: "", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue2 },
            { name: 'Id', key: true, hidden: true },
            { name: 'Length', hidden: true },
            { name: 'Qty', hidden: true },
            { name: 'Unit', hidden: true },
            { name: 'StatusCode', hidden: true },

            { name: 'MaterialCode', label: 'Composite Code', align: 'left', width: 500 },
            { name: 'MaterialName', label: 'Material', align: 'left' },
            { name: 'VBobbinCd', label: 'Container', align: 'left' },
            { name: '', label: 'Q`ty(Roll/EA)', align: 'right', formatter: QtyFormatter },
            { name: '', label: 'Length', align: 'right', formatter: LengthFormatter },
            { name: 'Size', label: 'Size', align: 'right' },
            { name: 'StatusName', label: 'Status', align: 'center' },
            { name: 'ReceivedDate', label: 'Received Date', align: 'center' },


        ],
        shrinkToFit: false,
        width: '100%',
        height: '100%',
        pager: "#" + childGridPager,
        rowNum: 50,
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
        onSelectRow: function (id, rowid, status, e) {
        },
    });
}

//SEARCH FUNCTION
var mtNoTemp = ``;
var mtCodeTemp = ``;
var recDateTemp = ``;
$(`#searchBtn`).on(`click`, function () {
    var mtCode = $(`#sMaterialCode`).val() == null ? `` : $(`#sMaterialCode`).val().trim();
    //var mtNo = $(`#sMaterialNo`).val() == null ? `` : $(`#sMaterialNo`).val().trim();
    var sVBobbinCd = $(`#sVBobbinCd`).val() == null ? `` : $(`#sVBobbinCd`).val().trim();

    var recDateStart = $(`#recevice_dt_start`).val() == null ? `` : $(`#recevice_dt_start`).val().trim();
    var recDateEnd = $(`#recevice_dt_end`).val() == null ? `` : $(`#recevice_dt_end`).val().trim();

    var s_prd_cd = $(`#s_prd_cd`).val() == null ? `` : $(`#s_prd_cd`).val().trim();

    $.ajax({
        url: `/TIMS/GetInventory2?mtCode=${mtCode}&sVBobbinCd=${sVBobbinCd}&recDateStart=${recDateStart}&recDateEnd=${recDateEnd}&prd_cd=${s_prd_cd}&view=1`,
    })
        .done(function (response) {
            //mtNoTemp = response.mtNoTemp;
            //if (!mtCode) {
            //    mtCodeTemp = ``;
            //}
            //else {
            //    mtCodeTemp = mtCode;
            //}
            ////if (!recDate) {
            ////    recDateTemp = ``;
            ////}
            ////else {
            ////    recDateTemp = recDate;
            ////}
            console.log(response.listInfo);
            grid.jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response.listInfo }).trigger("reloadGrid");
            return;
        })
        .fail(function () {
            ErrorAlert("System Error.");
            return;
        });
});

$(`#excelBtn`).on(`click`, function () {
    var mtCode = $(`#sMaterialCode`).val() == null ? `` : $(`#sMaterialCode`).val().trim();
    var mtNo = $(`#sMaterialNo`).val() == null ? `` : $(`#sMaterialNo`).val().trim();
    var mtName = $(`#sMaterialName`).val() == null ? `` : $(`#sMaterialName`).val().trim();
    var recDate = $(`#sReceivedDate`).val() == null ? `` : $(`#sReceivedDate`).val().trim();
    //$.ajax({
    //    url: `/TIMS/PrintExcelFile?mtCode=${mtCode}&mtNo=${mtNo}&mtName=${mtName}&recDate=${recDate}`,
    //});

    $('#exportData').attr('action', `/TIMS/PrintExcelFile?mtCode=${mtCode}&mtNo=${mtNo}&mtName=${mtName}&recDate=${recDate}`);
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