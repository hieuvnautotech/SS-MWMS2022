function formatdate(cellValue, options, rowdata, action) {

    if (cellValue != null) {

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

$(function () {
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);
    $grid = $("#list").jqGrid({
        //url: "/fgwms/GetCheckInvenF",
        url: "/fgwms/GetCheckInventoryFG",
        mtype: 'GET',
        datatype: 'json',
        colModel: [
            //{ key: true, label: 'vdno', name: 'vdno', width: 50, hidden: true },
            //{ label: 'Code', name: 'vn_cd', width: 200, align: 'center' },
            //{ label: 'Inventory Name', name: 'vn_nm', width: 200, align: 'left' },
            //{ label: 'Barcode', name: 'barcode', width: 300, align: 'left' },
            //{ label: 'Check Location', name: 'lct_cd', width: 200, align: 'center' },
            //{ label: 'Location', name: 'lct_cd1', width: 200, align: 'center' },
            //{ label: 'Match', name: 'Match_yn', width: 80, align: 'center',
            //    cellattr: function (rowId, value, rowObject, colModel, arrData, rdata) {
            //        if(value == 'UnMatch'){
            //            return 'style=background-color:#FFE08C;';
            //        }else if(value == 'Match'){
            //            return 'style=background-color:#B2EBF4;';
            //        }else{
            //            return 'style=background-color:#6799FF;';
            //        }
            //    },
            //},
            //{ key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px' },
            //{ key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
            //{ key: false, label: 'Update Name', name: 'chg_id', editable: true, width: '100px', hidden: true },
            //{ key: false, label: 'Update date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', hidden: true }
            { name: 'style_no', label: 'Product Code', width: 200, align: 'left', key: true, hidden: false },
            { name: 'style_nm', label: 'Product Name', width: 400, align: 'left' },
            { name: 'available', label: 'Available Quantity', width: 200, align: 'right', formatter: 'integer', },
            //{ name: 'total', label: 'Total Quantity', width: 200, align: 'right', formatter: 'integer', },
            //{ name: 'sent', label: 'Sent Quantity', width: 200, align: 'right', formatter: 'integer', },
        ],
        loadonce: true,
        shrinkToFit: false,
        pager: '#gridpager',
        rowNum: 50,
        width: '100%',
        autowidth: true,
        //width: $(".ui-dialog").width() - 50,
       // width: $(".box-body").width() - 5,
        //autowidth: true,
        rownumbers: true,
        multiselect: false,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        height: 660,
        loadonce: true,
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        //subGridRowExpanded: showChildGrid,
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
});

function showChildGrid(parentRowID, parentRowKey) {
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    
    // send the parent row primary key to the server so that we know which grid to show
    // add a table and pager HTML elements to the parent grid row - we will render the child grid here
    $("#" + parentRowID).html('<table id="' + childGridID + '"></table><div id="' + childGridPagerID + '" class="scroll"></div>');

    $("#" + childGridID).jqGrid({
        url: "/fgwms/ProductInOut?id=" + parentRowKey,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'plno', name: 'plno', width: 50, hidden: true },

            { label: 'Product Code', name: 'prd_cd', width: 360, align: 'left' },
            { label: 'Product Lot Code', name: 'prd_lcd', width: 500, align: 'left' },
            
            { label: 'WO No', name: 'fo_no', width: 150, align: 'center' },
            { label: 'PO No', name: 'po_no', width: 100, align: 'center' },
            { label: 'BOM No', name: 'bom_no', width: 100, align: 'center' },
            { label: 'Input Date', name: 'input_dt', width: 100, align: 'center', formatter: formatdate },
            { label: 'Output Date', name: 'output_dt', width: 100, align: 'center', formatter: formatdate },
            
        ],
        shrinkToFit: false,
        loadonce: true,
        width: '100%',
        height: '100%',
        //subGrid: false, // set the subGrid property to true to show expand buttons for each row
        //subGridRowExpanded: showThirdLevelChildGrid, // javascript function that will take care of showing the child grid
        pager: '#' + childGridPagerID,
        rowNum: 50,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [50, 100, 150, 200],
        viewrecords: true,
        rowNum: 50,
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
    $("#" + childGridID).jqGrid('filterToolbar'/*, {
        stringResult: true, searchOnEnter: false,
        defaultSearch: '', ignoreCase: true
    }*/);
    $("#" + childGridID).jqGrid('navGrid', "#" + childGridPagerID, {
        search: false, // show search button on the toolbar
        add: false,
        edit: false,
        del: false,
        refresh: true
    });
}


$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: 'CheckInventory(F)',
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});

$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#list").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                border: true,
                fileName: 'CheckInventory(F)',
                returnAsString: false
            }
         );
    });
});

//get select
// Khai báo URL stand của bạn ở đây
var wh = "/fgwms/Warehouse";
$(document).ready(function () {
    _getwhouse();
});
function _getwhouse() {
    $.get(wh, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"></option>';
            $.each(data, function (key, item) {

                html += '<option value=' + item.shelf_cd + '>' + item.shelf_cd + '</option>';
            });
            $("#real_lct_cd").html(html);
            $("#lct_cd").html(html);
        }
    });
}
var getM = "/fgwms/getmatch";
$(document).ready(function () {
    _getmatch();
});
function _getmatch() {
    $.get(getM, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_nm + '>' + item.dt_nm + '</option>';
            });
            $("#match").html(html);
        }
    });
}
var getName = "/fgwms/getVTName";
$(document).ready(function () {
    _getName();
});
function _getName() {
    $.get(getName, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.vn_cd + '>' + item.vn_nm + '</option>';
            });
            $("#vn_nm").html(html);
        }
    });
}
$("#searchBtn").click(function () {
    var product = $('#style_no').val();
    if (product == null || product == undefined) {
        product == "";
    }
    $.ajax({
        //url: "/fgwms/searchInventorydetailF",
        url: "/fgwms/GetCheckInventoryFG",
        type: "get",
        dataType: "json",
        data: {
            //vn_nm: $('#vn_nm').val(),
            //real_lct_cd: $('#real_lct_cd').val(),
            //lct_cd: $('#lct_cd').val(),
            //start: $('#start').val(),
            //end: $('#end').val(),
            //match: $('#match').val(),
            //mt_cd: $('#mt_cd').val(),

            product: product
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

function resizeJqGridWidth(grid_id, div_id, width) {
    $(window).bind('resize', function () {
        $('#list').setGridWidth(width, true); //Back to original width
        $('#list').setGridWidth($('#' + div_id).width(), true); //Resized to new width as per window
    }).trigger('resize');
}


