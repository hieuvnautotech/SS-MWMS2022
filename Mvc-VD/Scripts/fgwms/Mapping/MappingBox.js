var gridProduct = $(`#listProduct`);
var gridBox = $(`#listBox`);
var gridProductOverview = $(`#listProductOverview`);
var gridBoxOverview = $(`#listBoxOverView`);
var productCode = "";
$(`#sDate`).datepicker({
    showButtonPanel: true,
    dateFormat: 'yy-mm-dd'
}).datepicker("setDate", new Date());
//DIALOG BUYER
$(`.popupDialogBuyer`).on(`click`, function ()
{
    $(`.dialogBuyer`).dialog(`open`);
});
$(`.dialogBuyer`).dialog({
    width: `50%`,
    height: 500,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm"
    },
    resize: function (event, ui)
    {
        $(`.ui-dialog-content`).addClass(`m-0 p-0`);
    },
    open: function (event, ui)
    {
        $(`#dialogBuyerSelectBtn`).addClass(`disabled`);
        $(`#dialogBuyerCode`).val(``);
        $(`#dialogBuyerMaterialCode`).val(``);
        $(`#dialogBuyerMaterialNo`).val(``);
        ShowDialogBuyerGrid();
        $(`#dialogBuyerGrid`).jqGrid('clearGridData').jqGrid().setGridParam({ url: `/fgwms/GetBuyerCode`, datatype: 'json' }).trigger("reloadGrid");
    },
});
function ShowDialogBuyerGrid()
{
    $(`#dialogBuyerGrid`).jqGrid
        ({
            //url: `/fgwms/GetBuyerCode`,
            datatype: function (postData) { getDataBuyer(postData); },
            //datatype: `json`,
            mtype: `GET`,
            colModel: [
                { name: 'Id', key: true, hidden: true },

                { name: 'BuyerCode', sortable: true, width: 250, align: 'center', label: 'Buyer Code' },
                { name: 'MaterialCode', sortable: true, width: 370, align: 'left', label: 'Material Code' },
                { name: 'MaterialNo', sortable: true, width: 100, align: 'left', label: 'Product' },
                { name: 'Quantity', sortable: true, width: 80, align: 'right', label: 'Quantity', formatter: 'integer' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            pager: `#dialogBuyerPager`,
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000,5000,10000],
            sortable: true,
            loadonce: false,
            height: 250,
            width: null,
            autowidth: true,
            loadtext: `Loading...`,
            emptyrecords: `No data`,
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: false,
            //jsonReader:
            //{
            //    root: `data`,
            //    page: `page`,
            //    total: `total`,
            //    records: `records`,
            //    repeatitems: false,
            //    Id: `0`
            //},
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            onSelectRow: function (rowid, selected, status, e)
            {
                $(`.ui-state-highlight`).css({ 'border': '#AAAAAA' });
                $(`#dialogBuyerSelectBtn`).removeClass(`disabled`);
                var selectedRowId = $(`#dialogBuyerGrid`).jqGrid(`getGridParam`, `selrow`);
                var rowData = $(`#dialogBuyerGrid`).getRowData(selectedRowId);
                if (rowData)
                {
                    $(`#dialogBuyerSelectBtn`).click(function ()
                    {
                        $(`#scBuyerCode`).val(rowData.BuyerCode);
                        productCode = rowData.MaterialNo;
                        $(`.dialogBuyer`).dialog(`close`);
                        document.getElementById("scBuyerCode").focus();
                    });
                }
            },
        });
}
$(`#dialogBuyerSearchBtn`).on(`click`, function ()
{
    //var buyercode = $(`#dialogBuyerCode`).val();
    //var materialcode = $(`#dialogBuyerMaterialCode`).val();
    //var materialno = $(`#dialogBuyerMaterialNo`).val();
/*  $(`#dialogBuyerGrid`).jqGrid('clearGridData').jqGrid().setGridParam({ url: `/fgwms/GetBuyerCode?buyerCode=${buyercode}&materialCode=${materialcode}&materialNo=${materialno}`, datatype: 'json' }).trigger("reloadGrid");*/

    $('#dialogBuyerGrid').clearGridData();
    $('#dialogBuyerGrid').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogBuyerGrid').jqGrid('getGridParam', 'postData');
    getDataBuyer(pdata);
});
function getDataBuyer(pdata) {
  /*debugger*/
    var params = new Object();

    if ($('#list_primary').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;


    params.buyercode = $("#dialogBuyerCode").val().trim();
    params.dialogBuyerMaterialCode = $("#dialogBuyerMaterialCode").val().trim();
    params.productCode = $("#dialogBuyerProductCode").val().trim();

    $("#dialogBuyerGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/fgwms/GetBuyerCode",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#dialogBuyerGrid')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

//GRID BOX
function GridBox()
{
    gridBox.jqGrid({
        mtype: `GET`,
        colModel: [
            { name: `id`, key: true, hidden: true },
            { name: `BoxNo`, sortable: true, width: 300, align: `left`, label: `Tem thùng` },
            { name: `totalQty`, sortable: true, width: 100, align: `center`, label: `Số lượng`, formatter: 'integer', formatoptions: { thousandsSeparator: ",", deaultValue: "0" } },
        ],
        pager: `#listBoxPager`,
        height: 300,
        width: null,
        rowNum: 50,
        rowList: [50, 100, 200, 500,1000],
        loadtext: `Loading...`,
        emptyrecords: `No data`,
        multiboxonly: true,
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: false,
        multiselect: false,
        autoResizing: true,
        viewrecords: true,
        caption: `Box`,
        jsonReader:
        {
            root: `rows`,
            page: `page`,
            total: `total`,
            records: `records`,
            repeatitems: false,
            Id: `0`
        },

        gridComplete: function ()
        {
            var rowIds = gridBox.getDataIDs();
        },
        loadComplete: function ()
        {
            var rowIds = gridBox.getDataIDs();
        },
        onSelectRow: function (rowid, status, e, iRow, iCol)
        {
            var selectedRowId = gridBox.jqGrid(`getGridParam`, `selrow`);
            var rowId = gridBox.getRowData(selectedRowId);
        }
    });
}

//GRID PRODUCT
var prevCellVal = { cellId: undefined, value: undefined };
function GridProduct()
{
    gridProduct.jqGrid({
        mtype: `GET`,
        colModel: [
            { name: 'Id', key: true, hidden: true },
            { name: 'StampType', hidden: true },
            { name: 'TypeSystem', hidden: true},
            { name: 'ProductNo', sortable: true, width: 130, align: 'left', label: 'Product' },
            {
                name: 'Quantity', sortable: true, align: 'right', label: 'Total',
                formatter: 'integer', formatoptions: { thousandsSeparator: ",", deaultValue: "0" },

                width: 80,

            },
            { name: 'BuyerCode', sortable: true, width: 270, align: 'center', label: 'Buyer Code', title: false,  },
            { name: 'lot_date', sortable: true, width: 100, align: 'center', label: 'Lot Date', title: false,  },
            {
                name: "", width: 50, align: "center", label: "Delete", resizable: false, title: false,
                cellattr: function (rowId, val, rawObject, cm, rdata) {

                    var result = "<button class='btn btn-xs btn-delete btn-danger' data-BuyerCode= '" + rawObject.BuyerCode + "' onclick='DeleteRows(this)'><i class='fa fa-trash' aria-hidden='true'>&nbsp;</i>&nbsp;</button";
                    return result;
                }
            },
            //{
            //    name: "", width: 80, align: "center", label: "Delete", resizable: false, title: false,
            //    cellattr: function (rowId, val, rawObject, cm, rdata)
            //    {
            //        var result;

            //        if (prevCellVal.value == val)
            //        {
            //            result = ` rowspanid="${prevCellVal.cellId}" `;
            //        }
            //        else
            //        {
            //            var cellId = `${this.id}_row_${rowId}_${cm.name}`;

            //            result = ` rowspan="${rdata.Count}" id="${cellId}"><button class="btn btn-xs btn-danger" data-buyercode="${rdata.BuyerCode}" onclick="DeleteRows(this);"><i class="fa fa-trash" aria-hidden="true">&nbsp;</i></button `;

            //            prevCellVal = { cellId: cellId, value: val };
            //        }

            //        return result;
            //    }

            //},
        ],
        footerrow: true,
        userDataOnFooter: true,
        pager: `#listProductPager`,
        height: 300,
        width: null,
        rowNum: 400,
        rowList: [400, 500, 800,1000],
        loadtext: `Loading...`,
        emptyrecords: `No data`,
        multiboxonly: true,
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: true,
        multiselect: false,
        multiboxonly: false,
        autoResizing: true,
        viewrecords: true,
        caption: `Product`,
        jsonReader:
        {
            root: `rows`,
            page: `page`,
            total: `total`,
            records: `records`,
            repeatitems: false,
            Id: `0`
        },

        gridComplete: function ()
        {
            prevCellVal = { cellId: undefined, value: undefined };
            var rowIds = gridProduct.getDataIDs();
            rowIds.forEach(GridScanProduct_SetRowColor);

            //sum Quantity
            var ids = gridProduct.jqGrid('getDataIDs');
            var sum_Quantity = 0;
            var $self = $(this)
            for (var i = 0; i < ids.length; i++) {
                sum_Quantity += parseInt($("#listProduct").getCell(ids[i], "Quantity"));
            }

            $self.jqGrid("footerData", "set", { ProductNo: "Total" });
            $self.jqGrid("footerData", "set", { Quantity: sum_Quantity });


            jQuery("#listProduct").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        loadComplete: function ()
        {
            var rowIds = gridProduct.getDataIDs();
        },
        onSelectRow: function (rowid, status, e, iRow, iCol)
        {
            var selectedRowId = gridProduct.jqGrid(`getGridParam`, `selrow`);
            var rowId = gridProduct.getRowData(selectedRowId);
        }
    });
}
function GridScanProduct_SetRowColor(id)
{
    var flag = false;
    for (var item of idList)
    {
        if (item.toString() == id)
        {
            flag = true;
            break;
        }
    }
    if (!flag)
    {
        gridProduct.setRowData(id, false, { background: "#fff", color: 'black' });
    }
    else
    {
        gridProduct.setRowData(id, false, { background: "#28a745", color: 'white', 'font-size': '1.2em' });
    }
}
function DeleteRows(e)
{
    var buyer_cd = e.dataset.buyercode;
    var rowIds = gridProduct.getDataIDs();

    if (rowIds)
    {
        var rows = gridProduct.getRowData();
        for (var item of rows)
        {
            if (item.BuyerCode == buyer_cd)
            {
                gridProduct.delRowData(item.Id);
            }
        }
        SuccessAlert(``);
    }
    return;
}

//GRID BOX OVERVIEW
var boxCodeMapped = ``;
var boxCodeMappedStatus = ``;
function GridBoxOverview()
{
    gridBoxOverview.jqGrid({
        //url: `/fgwms/GetSumMappedBoxes?boxCode=`,
        datatype: function (postData) { getData_TableStampbox(postData); },
        mtype: `GET`,
     /*   datatype: 'json',*/
        colModel: [
            { name: `bmno`, key: true, hidden:true},
            { name: `status`, hidden: true },
            { name: 'ProductNo', sortable: true, width: 90, align: 'center', label: 'Product', title: false, },
            { name: `totalQty`, sortable: true, width: 90, align: `right`, label: `Số lượng`, formatter: 'integer', formatoptions: { thousandsSeparator: ",", deaultValue: "0" } },
            { name: `bx_no`, sortable: true, width: 230, align: `left`, label: `Tem thùng` },


            { name: `statusName`, sortable: true, width: 80, align: `right`, label: `Trạng thái` },
            { name: `unMapBtn`, sortable: false, width: 80, align: `center`, label: " " },
        ],
        pager: `#listBoxOverViewPager`,
        height: 450,
        width: null,
        rowNum: 50,
        rowList: [50, 100, 200, 1000],
        loadtext: `Loading...`,
        emptyrecords: `No data`,
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: false,
        multiselect: true,
        multiboxonly: false,
        autoResizing: true,
        viewrecords: true,
        caption: `Mapped Box`,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        gridComplete: function ()
        {
            var rowIds = gridBoxOverview.getDataIDs();
            for (var id of rowIds)
            {
                var rowData = gridBoxOverview.getRowData(id);
                if (rowData.status == '001')
                {
                    gridBoxOverview.setRowData(id, false, { background: "#f2e5bf", color: "#0f0f0f" });
                    gridBoxOverview.jqGrid('setCell', id, "unMapBtn", "", { 'display': 'none' });
                }
                else
                {
                    var html = '';
                    html += '<button class="btn btn-sm btn-warning button-srh" data-boxcode="' + rowData.bx_no + '" onclick="UnMap(this);"><i class="fa fa-unlink" aria-hidden="true">&nbsp;UN-MAP</i></button>';
                    gridBoxOverview.jqGrid("setCell", id, "unMapBtn", html);
                }
            }
        },
        loadComplete: function ()
        {
            var rowIds = gridBoxOverview.getDataIDs();
            for (var id of rowIds)
            {
                var rowData = gridBoxOverview.getRowData(id);
                if (rowData.bx_no == boxCodeGlobal)
                {
                    gridBoxOverview.setRowData(id, false, { background: "#28a745", color: "#fff" });
                    gridBoxOverview.setSelection(id, true);
                }
            }

        },
        onSelectRow: function (rowid, status, e, iRow, iCol)
        {
            var selrowId = gridBoxOverview.jqGrid(`getGridParam`, `selrow`);
            var rowData = gridBoxOverview.getRowData(selrowId);
            boxCodeMapped = rowData.bx_no;
            boxCodeMappedStatus = rowData.sts;

            var buyerCode = $(`#searchBuyerCode`).val() == null ? "" : $(`#searchBuyerCode`).val().trim();

            if (boxCodeMapped)
            {
                gridProductOverview.setGridParam({ url: `/fgwms/GetMappedProducts?boxCodeMapped=${boxCodeMapped}&buyerCode=${buyerCode}`, datatype: "json" }).trigger("reloadGrid");
            }
        }
    });
}

function getData_TableStampbox(pdata) {
    var params = new Object();

    if (gridBoxOverview.jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.boxCode = $(`#searchBoxCode`).val().trim();
    params.BuyerCode = $(`#searchBuyerCode`).val();
    params.ProductCode = $(`#searchProductCode`).val().trim();
    params.sDate = "";

    gridBoxOverview.jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/fgwms/GetSumMappedBoxes`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = gridBoxOverview[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
function UnMap(e)
{
    var box = e.dataset.boxcode;
    $.ajax({
        url: `/fgwms/UnMappingBox?boxCode=${box}`,
        type: "POST",
    })
        .done(function (response)
        {
            if (response.flag)
            {
                gridBoxOverview.jqGrid('clearGridData').jqGrid().setGridParam({ url: `/fgwms/GetSumMappedBoxes?boxCode=`, datatype: `json` }).trigger(`reloadGrid`);
                SuccessAlert(response.message);
                return;
            }
            ErrorAlert(response.message);
            return;
        })
        .fail(function ()
        {
            ErrorAlert(`System error.`);
            return;
        });
}

//GRID PRODUCT OVERVIEW
function GridProductOverview()
{
    gridProductOverview.jqGrid({
        mtype: `GET`,
        colModel: [
            { name: `Id`, key: true, hidden: true },
            { name: `MaterialCode`, hidden: true },
            { name: 'Count', hidden: true },

            { name: `ProductNo`, sortable: true, width: 100, align: `center`, label: `Product` },
            { name: `Quantity`, sortable: true, width: 100, align: `right`, label: `Quantity`, formatter: 'integer', formatoptions: { thousandsSeparator: ",", deaultValue: "0" } },

            { name: `BuyerCode`, sortable: true, width: 250, align: `center`, label: `Mã tem gói`, title: false,},
            { name: `lot_date`, sortable: true, width: 100, align: `center`, label: `Lot Date`, title: false, hidden: true},
            //{
            //    name: "unMapBtnProduct", width: 100, align: "center", label: " ", resizable: false, title: false,
            //    cellattr: function (rowId, val, rawObject, cm, rdata)
            //    {
            //        var result;

            //        if (prevCellVal.value == val)
            //        {
            //            result = ` rowspanid="${prevCellVal.cellId}" `;
            //        }
            //        else
            //        {
            //            var cellId = `${this.id}_row_product_unmap${rowId}_${cm.name}`;

            //            result = ` rowspan="${rdata.Count}" id="${cellId}" `;

            //            prevCellVal = { cellId: cellId, value: val };
            //        }

            //        return result;
            //    }

            //},
        ],
        pager: `#listProductOverviewPager`,
        height: 450,
        width: null,
        rowNum: 50,
        rowList: [50, 100, 200, 1000],
        loadtext: `Loading...`,
        emptyrecords: `No data`,

        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: true,
        multiselect: false,
        multiboxonly: false,
        autoResizing: true,
        viewrecords: true,
        caption: `Mapped Products`,
        jsonReader:
        {
            root: `data`,
            page: `page`,
            total: `total`,
            records: `records`,
            repeatitems: false,
            Id: `0`
        },
        gridComplete: function ()
        {
            prevCellVal = { cellId: undefined, value: undefined };

        },
        loadComplete: function ()
        {
            var rowIds = gridProductOverview.getDataIDs();

            var rowIds = gridProductOverview.getDataIDs();
            for (var id of rowIds)
            {
                var rowData = gridProductOverview.getRowData(id);
                if (boxCodeMappedStatus == `000`)
                {
                    var html = ``;
                    html += `<button  class="btn btn-sm btn-warning button-srh" data-buyercode="${rowData.BuyerCode}" onclick="UnMapBuyer(this);"><i class="fa fa-unlink" aria-hidden="true">&nbsp;UN-MAP</i></button>`;
                    gridProductOverview.jqGrid("setCell", id, "unMapBtnProduct", html);
                    $(this).jqGrid('showCol', ["unMapBtnProduct"]);
                }
                else
                {
                    $(this).jqGrid('hideCol', ["unMapBtnProduct"]);
                }
            }
        },
        onSelectRow: function (rowid, status, e, iRow, iCol)
        {
            var selrowId = gridProductOverview.jqGrid(`getGridParam`, `selrow`);
            var rowData = gridProductOverview.getRowData(selrowId);
        }
    });
}
function UnMapBuyer(e)
{
    var buyer = e.dataset.buyercode;
    $.ajax({
        url: `/fgwms/UnMappingBuyer?buyerCode=${buyer}`,
        type: "POST",
    })
        .done(function (response)
        {
            if (response.flag)
            {
                var rowIds = gridProductOverview.getDataIDs();
                var tempIds = [];
                for (var id of rowIds)
                {
                    var rowData = gridProductOverview.getRowData(id);
                    if (rowData.BuyerCode === buyer)
                    {
                        tempIds.push(id);
                    }
                }
                while (Array.isArray(tempIds) && tempIds.length)
                {
                    delRowInGridProductOverView(tempIds[0]);
                    tempIds = arrayRemove(tempIds, tempIds[0]);
                }
                gridBoxOverview.jqGrid('clearGridData').jqGrid().setGridParam({ url: `/fgwms/GetSumMappedBoxes?boxCode=`, datatype: `json` }).trigger(`reloadGrid`);
                SuccessAlert(response.message);
                return;
            }
            ErrorAlert(response.message);
            return;
        })
        .fail(function ()
        {
            ErrorAlert(`System error.`);
            return;
        });
}

function arrayRemove(arr, value)
{
    return arr.filter(function (ele)
    {
        return ele != value;
    });
}

function TotalQuantity(cellValue, options, rowdata, action)
{
    var total = 0;
    var buyer = rowdata.BuyerCode;
    var rows = gridProductOverview.getRowData();

    for (var item of rows)
    {
        if (item.BuyerCode === buyer)
        {
            total += item.Quantity;
        }
    }

    return total;
}

//SCAN BUYER FUNCTION
var pressOneTime = 1;
$(`#scBuyerCode`).on(`keyup`, function (e)
{
    if (pressOneTime == 1)
    {
        if (e.keyCode === 13)
        {
            pressOneTime = 2;

            ScanProduct();
        }
    }
    return;
});
var idList = [];
function ScanProduct()
{
    gridBox.jqGrid('clearGridData', true);
    var buyerCode = $(`#scBuyerCode`).val() == null ? "" : $(`#scBuyerCode`).val().trim();
    if (!buyerCode)
    {
        ErrorAlert(`Scan buyer code.`);
        pressOneTime = 1;
        return;
    }
    ////get product code
    //var buyerCode = $(`#scBuyerCode`).val() == null ? "" : $(`#scBuyerCode`).val().trim();
    //$.get(`/fgwms/BuyerCodeGEtProuct?buyerCode=${buyerCode}`, function (response) {
    //    alert(1);
    //    if (response.flag && response.data) {
    //        productCode = response.data[0];
    //        return;
    //    }
    //    return;
    //});
    ////get product code

    var rows = gridProduct.getRowData();
    var existed = false;
    var khacproduct = false;
    var firstRowType = undefined;
    if (rows.length > 0)
    {
        firstRowType = gridProduct.getRowData()[0].StampType;
        for (var item of rows)
        {
            if (item.BuyerCode === buyerCode)
            {
                existed = true;
            }
            //if (item.ProductNo != productCode) {
            //    khacproduct = true;
            //}
        }
    }

    if (existed)
    {
        ErrorAlert(`Mã Buyer đã được Scan.`);
        $(`#scBuyerCode`).val(``);
        pressOneTime = 1;
        return;
    }
    //if (khacproduct) {
    //    ErrorAlert(`Không cùng loại Product`);
    //    pressOneTime = 1;
    //    khacproduct = "";
    //    return;
    //}
    //alert(2);
    $.ajax({
        url: `/fgwms/BuyerCodeScanning?buyerCode=${buyerCode}&type=${firstRowType}`,
        type: `POST`,
        dataType: `json`,

    })
        .done(function (response)
        {

            pressOneTime = 1;
            idList = [];
            if (response.flag && response.data)
            {
                //gridProduct.jqGrid('clearGridData');

                    var rows = gridProduct.getRowData();
                    for (var item2 of rows) {

                        if (response.data.ProductNo != item2.ProductNo) {
                            ErrorAlert(`Không cùng loại Product`);
                            $(`#scBuyerCode`).val(``);
                            return;
                        }
                    }
                var id = response.data.Id;
                    idList.push(id);
                    gridProduct.jqGrid('addRowData', id, response.data, 'first');


                $(`#scBuyerCode`).val(``);
                SuccessAlert(response.message);
                return;
            }

            ErrorAlert(response.message);
            $(`#scBuyerCode`).val(``);
            return;
        })
        .fail(function ()
        {
            pressOneTime = 1;
            ErrorAlert("System error.");
            return;
        });
}

//SCAN BOX FUNCTION
$(`#scBoxCode`).on(`keydown`, function (e)
{
    if (e.keyCode === 13)
    {
        ScanBox();
    }
});
function ScanBox()
{
    var boxCode = $(`#scBoxCode`).val() == null ? "" : $(`#scBoxCode`).val().trim();
    if (!boxCode)
    {
        ErrorAlert(`Scan box code.`);
        return;
    }
    boxCodeGlobal = boxCode;
    $.ajax({
        url: `/fgwms/BoxCodeScanning?boxCode=${boxCode}`,
        type: `POST`,
        dataType: `json`
    })
        .done(function (response)
        {
            boxCodeGlobal = boxCode;
            if (response.flag)
            {
                gridBox.jqGrid('clearGridData');
                gridBox.jqGrid('addRowData', 1, response.data, 'first');
                gridBox.setRowData(1, false, { background: "#28a745", color: 'white', 'font-size': '1.2em' });
                $(`#scBoxCode`).val(``);
                SuccessAlert(response.message);
                return;
            }
            ErrorAlert(response.message);
            $(`#scBuyerCode`).val(``);
            return;
        })
        .fail(function ()
        {
            ErrorAlert("System error.");
            return;
        });
}

//MAPPING FUNCTION
var boxCodeGlobal = ``;
var object = {};
$(`#mapBtn`).on(`click`, function () {
    debugger;

    var rowIds = gridProduct.getDataIDs();

    var rows = gridProduct.jqGrid('getRowData');

    if (rowIds.length < 1) {
        ErrorAlert("Vui lòng scan mã buyer để tiếp tục.");
        return;
    }

    object = {};
    object['Wmtids'] = rowIds;
    object['ProductCode'] = rows[0].ProductNo;

    $.ajax({
        url: `/fgwms/CheckMappingBox`,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(object)
    })
        .done(function (response) {
            if (response.result) {
                if (!response.exist) {
                    MappingBox(object);
                }
                else {
                    $('#dialog_Finish').dialog('open');
                }
            }
            else {
                ErrorAlert(response.message);
                return false;
            }
        })
        .fail(function () {
            ErrorAlert(`Lỗi hệ thống.`);
            return false;
        });

});
//$(`#mapBtn`).on(`click`, function ()
//{
//    var rowIds = gridProduct.getDataIDs();
//    var rowIDMes = []
//    var rowIDSap = []

//    var rows = gridProduct.jqGrid('getRowData');
//    for (i = 0; i < rows.length; i++) {
//        var TypeSystem = rows[i].TypeSystem;

//        if (TypeSystem == "MES") {

//            rowIDMes.push(rows[i].Id);
//        }
//        if (TypeSystem == "SAP") {
//            rowIDSap.push(rows[i].Id);
//        }
//    }

//    if (rowIds.length < 1)
//    {
//        ErrorAlert("Vui lòng scan mã buyer để tiếp tục.");
//        return;
//    }

//    //buyerCodeGlobal = Array.from(new Set(buyerCodeGlobal)); // --- remove duplicated values in the array

//    object = {};
//    object['Wmtids'] = rowIds;
//    object['WmtidMES'] = rowIDMes;
//    object['WmtidsAP'] = rowIDSap;

//    $.ajax({
//        url: `/fgwms/CheckMappingBox`,
//        type: "POST",
//        contentType: 'application/json',
//        data: JSON.stringify(object)
//    })
//        .done(function (response)
//        {
//            if (response.result)
//            {
//                if (!response.exist)
//                {
//                    MappingBox(object);
//                }
//                else
//                {
//                    $('#dialog_Finish').dialog('open');
//                }
//            }
//            else {
//                ErrorAlert(response.message);
//                return false;
//            }
//        })
//        .fail(function ()
//        {
//            ErrorAlert(`Lỗi hệ thống.`);
//            return false;
//        });

//});

function MappingBox(e)
{
    $.blockUI({
        message: "Đang thực hiện vui lòng đợi trong giây lát...",
        css: {

            border: 'none',
            padding: '15px',
            backgroundColor: 'red',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            color: '#fff',
            fontSize: '20px'
        }
    });
    $.ajax({
        url: `/fgwms/MappingBox`,
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(e)
    })
        .done(function (response)
        {
            if (response.result)
            {
                boxCodeGlobal = response.data[0].box_no;
                gridProduct.jqGrid("clearGridData", true).trigger("reloadGrid");
                for (var item of response.data)
                {
                    gridBox.jqGrid('addRowData', item.id, item, 'first');
                    gridBox.setRowData(item.id, false, { background: "#d0e9c6" });
                }

              /*  gridBoxOverview.jqGrid('clearGridData');*/
                //    .jqGrid('setGridParam', { data: response.data, datatype: 'json' })
                //    .trigger('reloadGrid');
                gridBoxOverview.jqGrid('clearGridData').jqGrid().setGridParam({ url: `/fgwms/GetSumMappedBoxes?boxCode=`, datatype: `json` }).trigger(`reloadGrid`);
                SuccessAlert(response.message);
                $.unblockUI();
            }
            else
            {
                ErrorAlert(response.message);
                $.unblockUI();
            }
            return;
        })
        .fail(function ()
        {
            ErrorAlert(`Lỗi hệ thống.`);
            $.unblockUI();
            return;
        });
};

$("#dialog_Finish").dialog({
    width: '30%',
    height: 100,
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
    resize: function (event, ui)
    {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui)
    {
    },
    close: function (event, ui)
    {
    }
});

$("#finish_mapping").click(function ()
{
    MappingBox(object);
    $('#dialog_Finish').dialog('close');
});

$('#closefinish').click(function ()
{
    $('#dialog_Finish').dialog('close');
});

//RESET FUNCTION
$(`#resetBtn`).on(`click`, function ()
{
    //var rowIds = gridProduct.getDataIDs();

    //if (!rowIds)
    //{
    //    ErrorAlert(`Select products to unmap.`);
    //    return;
    //}

    //while (Array.isArray(rowIds) && rowIds.length)
    //{
    //    delRowInGrid(rowIds[0]);
    //}

    gridProduct.clearGridData(true);

    SuccessAlert(`Clear successfully.`);
    return;
});
function delRowInGrid(e)
{
    gridProduct.delRowData(e);
}
function delRowInGridProductOverView(e)
{
    gridProductOverview.delRowData(e);
}

//SEARCH FUNCTION
$(`#searchBtn`).on(`click`, function ()
{
    var searchBoxCode = $(`#searchBoxCode`).val().trim();
    var searchProductCode = $(`#searchProductCode`).val().trim();
    var sDate = $(`#sDate`).val().trim();
    var searchBuyerCode = $(`#searchBuyerCode`).val();
    gridBoxOverview.setGridParam({ url: `/fgwms/GetSumMappedBoxes?boxCode=${searchBoxCode}` + `&BuyerCode=${searchBuyerCode}` + `&ProductCode=${searchProductCode}` + `&sDate=${sDate}`, datatype: "json" }).trigger("reloadGrid");

    gridProductOverview.jqGrid('clearGridData');
});

//DOCUMENT READY
$(function ()
{
    GridBox();
    GridBoxOverview();
    GridProduct();
    GridProductOverview();
});

window.onload = function ()
{
    document.getElementById("scBuyerCode").focus();
};

function arrayRemove(arr, value)
{
    return arr.filter(function (ele)
    {
        return ele != value;
    });
}

json_object = "";
tempList = [];
$(`#excelupload_input`).change(function (evt)
{
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event)
    {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName)
        {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            for (var i = 0; i < XL_row_object.length; i++)
            {
                var data_row_tmp = ``;
                if (XL_row_object[i]["CODE"] != undefined)
                {
                    data_row_tmp = XL_row_object[i]["CODE"];
                    excelData.push(data_row_tmp);
                }

            }
        });

        json_object = excelData;

    };

    reader.onerror = function (event)
    {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});

//EXCEL UPLOAD
$(`#ExcelUpload`).on(`click`, function ()
{
    $("#er").html("");
    gridProduct.jqGrid('clearGridData');
    $.blockUI({
        message: "Đang thực hiện vui lòng đợi trong giây lát...",
       /* timeout: 500,*/
        css: {

            border: 'none',
            padding: '15px',
            backgroundColor: 'red',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            color: '#fff',
            fontSize: '20px'
        }
    });
    debugger;
    tempList = [];

    for (var item of json_object)
    {
        data = {
            Code: item
        };
        tempList.push(data);
    }
    $(`#excelupload_input`).val('');
    $.ajax({
        url: `/fgwms/ShowingScanBuyerTemporary`,
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tempList),
        traditonal: true
    })
        .done(function (response)
        {
            if (response.flag && response.listData)
            {
                $("#er").html("")
                $("#ViewThongTinloi").html("");
                gridProduct.jqGrid('clearGridData');

                idList = [];
                for (var item of response.listData)
                {
                    var id = item.Id;
                    idList.push(id);
                    gridProduct.jqGrid('addRowData', id, item, 'first');
                    gridProduct.setRowData(id, false, { background: "#28a745", color: "#fff" });


                }

                $.unblockUI();
                $(`#scBuyerCode`).val(``);
                SuccessAlert(response.message);
                json_object = "";
                tempList = [];

                return;
            }
            else {
                $("#ViewThongTinloi").html("");
                $.blockUI({
                    message: response.message,
                    timeout: 2000,
                    css: {

                        border: 'none',
                        padding: '15px',
                        backgroundColor: 'red',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: 1,
                        color: '#fff',
                        fontSize: '20px'
                    }
                });
                idList = [];

                $(`#scBuyerCode`).val(``);
                json_object = "";
                tempList = [];

                $("#ViewThongTinloi").html(response.message);
                var html = `Buyer Code lỗi:  ${response.message}`
                $("#er").html(html)
                return;
            }

            //if (response)
            //{
            //    //data_create = data_create + response.data_create;
            //    //data_update = data_update + response.data_update;
            //    //data_error = data_error + response.data_error;
            //    //data_pagedata = data_pagedata + 1;
            //    //$("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");
            //    //console.log(response);

            //    gridProduct.clearGridData();
            //    for (var item of response)
            //    {
            //        gridProduct.jqGrid('addRowData', item.wmtid, item, 'first');
            //        gridProduct.setRowData(id, false, { background: "#28a745", color: "#fff" });
            //    }
            //}
        })
        .fail(function ()
        {
            ErrorAlert(`Lỗi hệ thống.`);
            json_object = "";
            tempList = [];
            $.unblockUI();

        });
});
/*-------------------------------in mã-----------------------*/

$("#printQRBtn").click(function () {
    var i, selRowIds = gridBoxOverview.jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {

        window.open("/fgwms/PrintQR?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        ErrorAlert("Vui lòng chọn trên bảng");
    }

});
$('#excelBtn').click(function () {
 //debugger

    $('#exportData').attr('action', "/fgwms/exportTemToExcel");
});

/*-------------------------------in mã-----------------------*/












