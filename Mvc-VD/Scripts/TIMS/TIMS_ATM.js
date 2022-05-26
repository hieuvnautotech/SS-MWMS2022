$grid = $("#list").jqGrid({
    url: '/TIMS/GetData_atm',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'id_actual', name: 'id_actual', width: 80, align: 'center', hidden: true },
        { label: 'Product', name: 'product', width: 120 },
        { label: 'Name', name: 'name', width: 120 },
        { label: 'Date', name: 'date', width: 120, align: 'center' },
        { name: "", width: 200, align: "center", label: "View", resizable: false, title: false, formatter: bntCellValue },
    ],
    


    rownumbers: true,
    loadonce: true,
    rowNum: 10000,
    //subGrid: true,
    //subGridRowExpanded: showChildGrid,
    viewrecords: true,
    altRows: true,
    multiselect: false,
    height: '400',
    multiboxonly: true,
    sortable: true,
    caption: 'Productivity',
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
    var id = rowdata.id_actual;
    var html = '<button class="btn btn-xs btn-primary" onclick="Product_view(' + id + ');">Product View</button>&nbsp;&nbsp;&nbsp;&nbsp;';
    return html;
}
function Product_view(id) {
    var ret = jQuery("#list").jqGrid('getRowData', id);
    window.open("/TIMS/Daily?id=" + id);
}




$("#searchBtn").click(function () {
    var product = $('#product_id').val().trim();
    var name = $('#name_id').val().trim();
    

    $.ajax({
        url: "/TIMS/searchAtm",
        type: "get",
        dataType: "json",
        data: {
            product: product,            
            name: name,
            
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });

});