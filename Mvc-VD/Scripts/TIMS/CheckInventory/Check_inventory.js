
$(function () {
    $grid = $("#list").jqGrid
   ({
       url: "/TIMS/GetCheck_Inventory",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
            { key: true, label: 'id', name: 'id', width: 50, align: 'right', hidden: true },
            { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left', hidden: true },
            { label: 'MT NO', name: 'mt_no', width: 250, align: 'left', hidden: true },
            { label: 'Product Code', name: 'mt_nm', width: 250, align: 'left' },
            { label: 'Model', name: 'model', width: 250, align: 'left' },
            { label: 'Type', name: 'mt_type', width: 150, align: 'left', hidden: true },
        
            { label: 'Width (mm)', name: 'new_with', sortable: true, width: 80, align: 'right', hidden: true },
            { label: 'Length (M)', name: 'new_spec', sortable: true, width: 100, align: 'right', hidden: true },
            { label: 'Group Qty', name: 'gr_qty1', width: 150, align: 'right', formatter: 'integer' },
            { label: 'UnCheck', name: 'uncheck', width: 150, align: 'right', formatter: 'integer' },
            { label: 'Checked', name: 'checked', width: 150, align: 'right', formatter: 'integer' },
            { label: 'Packing', name: 'packing', width: 150, align: 'right', formatter: 'integer' },
      
             //{ label: 'Lot division ', name: 'lot_div_cd', width: 120, align: 'left' },
            { label: 'Status', name: 'mt_sts_cd', width: 80, align: 'left', hidden: true },
            { label: 'Receive Date', name: 'recevice_dt_tims', width: 100, align: 'center', formatter: fomatdate },
            { label: 'WO NO', name: 'fo_no', width: 100, align: 'center', hidden: true },
            { label: 'BOM NO', name: 'bom_no', width: 100, align: 'center', hidden: true },
          
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       pager: "#listPage",
   
       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       height: 600,
       width: null,
       autowidth: true,
       rowNum: 50,
       caption: 'Check Inventory',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       loadonce: true,
       shrinkToFit: false,
       jsonReader:
       {
           root: "rows",
           page: "page",
           total: "total",
           records: "records",
           repeatitems: false,
           Id: "0"
       },
       subGrid: true,
       subGridRowExpanded: function (subgrid_id, row_id) {
           var subgrid_table_id;

           subgrid_table_id = subgrid_id + "_t";
           jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
           jQuery("#" + subgrid_table_id).jqGrid({
               url: "/TIMS/GetCheck_Inventory_sub?id=" + row_id + "&ml_no=" + $("#s_ml_no").val().trim(),
       
               datatype: 'json',
               colModel: [
                  { key: true, label: 'id', name: 'id', width: 50, align: 'right', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left', hidden: true },
                { label: 'MT NO', name: 'mt_no', width: 250, align: 'left', hidden: true },
                { label: 'Product Code', name: 'mt_cd', width: 500, align: 'left' },
                { label: 'Type', name: 'mt_type', width: 150, align: 'left', hidden: true },

                { label: 'Width (mm)', name: 'new_with', sortable: true, width: 80, align: 'right', hidden: true },
                { label: 'Length (M)', name: 'new_spec', sortable: true, width: 100, align: 'right', hidden: true },
                { label: 'Group Qty', name: 'gr_qty', width: 150, align: 'right', formatter: 'integer' },

                 { label: 'Lot division ', name: 'lot_div_cd', width: 120, align: 'left' },
                { label: 'Status', name: 'mt_sts_cd', width: 100, align: 'left' },
                { label: 'Receive Date', name: 'recevice_dt_tims', width: 100, align: 'center', formatter: fomatdate },
                { label: 'WO NO', name: 'fo_no', width: 100, align: 'center' },
                { label: 'BOM NO', name: 'bom_no', width: 100, align: 'center' },
               ],
               height: '100%',
               rowNum: 50,
               sortname: 'num',
               sortorder: "asc",
           });
       },
   })
    $("#searchBtn").click(function () {
        $.ajax({
            url: "/TIMS/GetCheck_Inventory",
            type: "get",
            dataType: "json",
            data: {
                style_no: $("#s_style_no").val().trim(),
                ml_no: $("#s_ml_no").val().trim(),
                 input_dt: $("#input_dt").val(),
             
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
              
            }
        });

    });
});

$("#input_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
function donvi(cellvalue, options, rowobject) {

    var html = "";
    if (cellvalue == null) { html = ""; }
    if (cellvalue != null) { html = cellvalue+" Roll" }
    return html;
}
$("#input_dt").datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
function fomatdate(cellValue, options, rowdata, action) {

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