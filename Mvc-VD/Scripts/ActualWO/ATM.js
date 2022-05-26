$grid = $("#list").jqGrid({
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { label: 'PO NO', name: 'at_no', width: 100, align: 'center', hidden: false },
        { key: true, label: 'id', name: 'id_actual', width: 60, align: 'center', hidden: true },
        { label: 'Product', name: 'product', width: 120 },
        { label: 'Product Name', name: 'product_nm', width: 120 },
        { label: 'Quantity Material(m)', name: 'm_lieu', width: 120, align: 'right' },
        { label: 'Create Date', name: 'reg_dt', width: 100, align: 'center', },
        { name: "", width: 100, align: "center", label: "View", resizable: false, title: false, formatter: bntCellValue },
    ],

    pager: jQuery('#list'),
    rownumbers: true,
    loadonce: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    //subGrid: true,
    //subGridRowExpanded: showChildGrid,
    viewrecords: true,
    gridview: true,
    shrinkToFit: true,
    emptyrecords: "No data.",
    altRows: true,
    multiselect: false,
    height: '400',
    width: '1200',
    multiboxonly: true,
    loadonce: false,
    sortable: true,
    datatype: function (postData) { getDataAT(postData); },
    caption: 'Productivity',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },

    //loadonce: true,
    loadComplete: function () {
        var table = this;
        setTimeout(function () {
            updatePagerIcons(table);
        }, 0);
    }
});



//a = cellvalue.substr(0, 1);
//b = cellvalue.substr(1, 11);
//c = parseInt(b);

function popupView(cellvalue, options, rowobject) {


    if (cellvalue == null && cellvalue == '' && cellvalue == 0 && cellvalue == 'undefined' && cellvalue.length == 0) {
        return '';
    }
    return cellvalue.substr(0, 1) + parseInt(cellvalue.substr(1, 11));


};
$("#list").jqGrid('navGrid', '#list_toppager');
function updatePagerIcons(table) {
    var replacement =
    {
        'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
        'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
        'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
        'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
    };
    $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
        var icon = $(this);
        var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

        if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
    })
}
function bntCellValue(cellValue, options, rowdata, action) {

    var at_no = rowdata.at_no;
    var id_actual = rowdata.id_actual;
    var product = rowdata.product;
    
    var html = `<button class="btn btn-xs btn-primary" data-id_actual="${id_actual}"  
data-product="${product}" data-at_no="${at_no}"  onclick="Product_view(this);">Product View</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
    return html;
}
function Product_view(e) {
    var mapForm = document.createElement("form");
    mapForm.target = "Map";
    mapForm.type = "hidden";
    mapForm.method = "POST"; // or "post" if appropriate
    mapForm.action = "/ActualWO/Daily";

    var mapInput = document.createElement("input");
    mapInput.type = "hidden";
    mapInput.name = "at_no";
    mapInput.value = e.dataset.at_no;

    mapForm.appendChild(mapInput);

    var mapInput2 = document.createElement("input");
    mapInput2.type = "hidden";
    mapInput2.name = "id_actual";
    mapInput2.value = e.dataset.id_actual;
    mapForm.appendChild(mapInput2);

    var mapInput3 = document.createElement("input");
    mapInput3.type = "hidden";
    mapInput3.name = "product";
    mapInput3.value = e.dataset.product;

    mapForm.appendChild(mapInput3);


    var mapInput4 = document.createElement("input");
    mapInput4.type = "hidden";
    mapInput4.name = "reg_dt";
    mapInput4.value = $('#reg_dt').val() == null ? "" : $('#reg_dt').val().trim();

    mapForm.appendChild(mapInput4);


    var mapInput5 = document.createElement("input");
    mapInput5.type = "hidden";
    mapInput5.name = "reg_dt_end";
    mapInput5.value = $('#reg_dt_end').val() == null ? "" : $('#reg_dt_end').val().trim();

    mapForm.appendChild(mapInput5);

    document.body.appendChild(mapForm);

    map = window.open(" ", "Map");

    if (map) {
        mapForm.submit();
    } else {
        ErrorAlert("You must allow popups for this map to work.'");
    }
}

//$("#searchBtn").click(function () {

//});


$("#searchBtn").click(function () {
    $("#list").clearGridData();
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getDataAT(pdata)
});


$(document).ready(function () {

    $('#reg_dt').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#reg_dt_end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });


});



function getDataAT(pdata) {
    $('#loading').show();
    var params = new Object();
    var rows = $("#list").getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    var type = $('#all:checkbox:checked').length > 0;
    if (type == true) {
        $("#list").jqGrid('hideCol', ["at_no"]);
    } else {
        $("#list").jqGrid('showCol', ["at_no"]);
    }

    params.type = type;
    params.product = $('#product_id').val() == null ? "" : $('#product_id').val().trim();
    params.name = $('#name_id').val() == null ? "" : $('#name_id').val().trim();
    params.at_no = $('#at_id').val() == null ? "" : $('#at_id').val().trim();
    params.reg_dt = $('#reg_dt').val() == null ? "" : $('#reg_dt').val().trim();
    params.reg_dt_end = $('#reg_dt_end').val() == null ? "" : $('#reg_dt_end').val().trim();

    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        //url: "/WIP/getGeneral",
        url: "/ActualWO/searchAtm",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list")[0];
                showing.addJSONData(data);
                $('#loading').hide();
            }
        },
        error: function () {
            $('#loading').hide();
            return;
        }
    });
};