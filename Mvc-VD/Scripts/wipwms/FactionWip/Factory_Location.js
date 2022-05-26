$grid = $("#list").jqGrid({
    url: '/ExportToMachine/GetLocation',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        { key: true, label: 'ID', name: 'lctno', width: 60, hidden: true },
        { label: 'Factory Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 250, align: 'center' },
        { label: 'Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 250, },
        { label: 'Level', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
        { name: "", width: 100, align: "center", label: "View", resizable: false, title: false, formatter: bntCellValue },
    ],
    loadonce: false,
    shrinkToFit: false,
    pager: '#jqGridPager1',
    rownumbers: true,
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    viewrecords: true,
    height: 360,
    width:null,
    multiselect: false,
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
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
       
                  
    },
});
            
function bntCellValue(cellValue, options, rowdata, action) {
    var lct_cd = rowdata.lct_cd;
    var html = '<button class="btn btn-xs btn-primary btnView" data-lct_cd="' + lct_cd +'"  onclick="ViewDetail(this);" "> Detail</button>';
    return html;
}
function ViewDetail(e) {

    var mapForm = document.createElement("form");
    mapForm.target = "Map";
    mapForm.type = "hidden";
    mapForm.method = "POST"; // or "post" if appropriate
    mapForm.action = "/ExportToMachine/ViewLoactionDetail";

    var mapInput = document.createElement("input");
    mapInput.type = "hidden";
    mapInput.name = "lct_cd";
    mapInput.value = e.dataset.lct_cd;

    mapForm.appendChild(mapInput);
    document.body.appendChild(mapForm);

    map = window.open(" ", "Map");

    if (map) {
        mapForm.submit();
    } else {
        ErrorAlert("You must allow popups for this map to work.'");
    }
   
}
$("#searchBtn").click(function () {
    $.ajax({
        url: "/ExportToMachine/GetLocation",
        type: "get",
        dataType: "json",
        data: {
            lct_cd: $('#lct_cd').val().trim(),
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });

});

