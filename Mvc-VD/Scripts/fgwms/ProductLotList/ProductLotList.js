$(function () {
    $("#list").jqGrid
    ({
        //url: "/fgwms/GetproLotList",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'pno', name: 'pno', width: 80, align: 'center', hidden: true },
            //{ label: 'Lot Info', name: '', width: 70, align: 'center', formatter: Lot_info },
            //{ label: 'Use Info', name: '', width: 70, align: 'center', formatter: Use_info },
            { key: false, label: 'Finish Lot', name: 'style_no', width: 350, align: 'left' },
            { key: false, label: 'Finish Code', name: 'prd_cd', width: 300, align: 'left', hidden: true },
            { key: false, label: 'Buyer QR', name: 'buyer_qr', width: 100, align: 'left', hidden: true },
            { key: false, label: 'Buyer Name', name: 'buyer_nm', width: 200, align: 'left', hidden: true },
            { key: false, label: 'Product Code', name: 'style_no', sortable: true, width: '120', align: 'left' },
            { key: false, label: 'Product Name', name: 'style_nm', sortable: true, width: '200', align: 'left' },
            { key: false, label: 'Lot division', name: 'lot_division', editable: true, width: '200px', align: 'center', hidden: true },
            { key: false, label: 'Product date', name: 'prd_dt', editable: true, width: '100px', align: 'center', hidden: true },
            { key: false, label: 'Status', name: 'sts_cd', editable: true, width: '100', align: 'center' },
            { key: false, label: 'Location', name: 'lct_cd', editable: true, width: '100px', align: 'left' , hidden: true},
            { key: false, label: 'Location Status', name: 'lct_sts_cd', editable: true, width: '100px', align: 'center', hidden: true },
            { key: false, label: 'Departure', name: 'from_lct_cd', editable: true, width: '100px', align: 'center', hidden: true },
            { key: false, label: 'Out Date', name: 'output_dt', editable: true, width: '100px', align: 'center', formatter: fomatdate, hidden: true },
            { key: false, label: 'Destination', name: 'to_lct_cd', editable: true, width: '200', align: 'center' },
            { key: false, label: 'Input Date', name: 'input_dt', editable: true, width: '100px', align: 'center', formatter: fomatdate, hidden: true },
            { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'QR Code', name: 'qrcode', editable: true, width: '400', align: 'left', hidden: true },
            { key: false, label: 'PO', name: 'po_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'WO', name: 'fo_no', editable: true, width: '100', align: 'center' },
            { key: false, label: 'BOM', name: 'bom_no', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Routing', name: 'line_no', editable: true, width: '100', align: 'center', },
            { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px', sortable: true, },
            { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, },
            { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px', sortable: true, },
            { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, }


        ],

        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
        },

        gridComplete: function () {
            //var rows = $("#list").getDataIDs();
            //for (var i = 0; i < rows.length; i++) {
            //}
            $('.loading').hide();
        },
        pager: '#gridpager',
        viewrecords: true,
        height: 450,
        rowNum: 50,
        rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
        rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
        // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
        loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
        emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
        datatype: function (postData) { getDataOutBox(postData); },
        gridview: true,
        shrinkToFit: false,
        loadonce: false,
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
                url: "/fgwms/searchproLotList_sub?id=" + row_id,
             
                datatype: 'json',
                colModel: [
                 { key: true, label: 'pno', name: 'pno', width: 80, align: 'center', hidden: true },
                { label: 'Lot Info', name: '', width: 70, align: 'center', formatter: Lot_info },
                //{ label: 'Use Info', name: '', width: 70, align: 'center', formatter: Use_info },
                { key: false, label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
                { key: false, label: 'Finish Code', name: 'prd_cd', width: 300, align: 'left', hidden: true },
                { key: false, label: 'Buyer Code', name: 'buyer_qr', width: 100, align: 'left' },
                { key: false, label: 'Buyer Name', name: 'buyer_nm', width: 350, align: 'left' },
                { key: false, label: 'Product Code', name: 'style_no', sortable: true, width: '120', align: 'left', hidden: true },
                { key: false, label: 'Product Name', name: 'style_nm', sortable: true, width: '200', align: 'left', hidden: true },
                { key: false, label: 'Lot division', name: 'lot_division', editable: true, width: '200px', align: 'center', hidden: true },
                { key: false, label: 'Product date', name: 'prd_dt', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'Status', name: 'sts_cd', editable: true, width: '200px', align: 'center' },
                { key: false, label: 'Location', name: 'lct_cd', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Location Status', name: 'lct_sts_cd', editable: true, width: '100px', align: 'center', hidden: true },
                { key: false, label: 'Departure', name: 'from_lct_cd', editable: true, width: '100px', align: 'center', hidden: true },
                { key: false, label: 'Output Date', name: 'output_dt', editable: true, width: '100px', align: 'center', formatter: fomatdate },
                { key: false, label: 'Destination', name: 'destination', editable: true, width: '200', align: 'center' },
                { key: false, label: 'Input Date', name: 'input_dt', editable: true, width: '100px', align: 'center', formatter: fomatdate },
                { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'QR Code', name: 'qrcode', editable: true, width: '400', align: 'left', hidden: true },
                { key: false, label: 'PO', name: 'po_no', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'WO', name: 'fo_no', editable: true, width: '100', align: 'center' },
                { key: false, label: 'BOM', name: 'bom_no', editable: true, width: '100px', align: 'center' },
                { key: false, label: 'Routing', name: 'line_no', editable: true, width: '100', align: 'center', },
                { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px', sortable: true, },
                { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, },
                { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px', sortable: true, },
                { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200', sortable: true, }
                ],
                height: '100%',
                rowNum: 50,
                sortname: 'num',
                sortorder: "asc",
            });

        },
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
    });

});//grid1
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
function getDataOutBox(pdata) {
    $('.loading').show();
    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.prd_lcd = $("#prd_lcd").val().trim();
    params.style_no = $("#style_no").val().trim();
    params.po_no = $("#po_no").val().trim();
    params.fo_no = $("#fo_no").val().trim();
    params.bom_no = $("#bom_no").val().trim();
    params.start = $("#start").val().trim();
    params.end = $("#end").val().trim();
    params.start_out = $("#start_out").val().trim();
    params.end_out = $("#end_out").val().trim();
    params.buyer = $("#cp_buyer_qr").val().trim();

    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    //params._search = pdata._search;

    $.ajax({
        url: "/fgwms/searchproLotList",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
            }
        }
    })
};

$("#searchBtn").click(function () {
    $("#list").clearGridData();
    var prd_lcd = $('#prd_lcd').val().trim();
    var style_no = $("#style_no").val().trim();
    var po_no = $("#po_no").val().trim();
    var fo_no = $("#fo_no").val().trim();
    var bom_no = $("#bom_no").val().trim();
    var start = $("#start").val().trim();
    var end = $("#end").val().trim();
    var start_out = $("#start_out").val().trim();
    var end_out = $("#end_out").val().trim();
    var grid = $("#list");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.prd_lcd = prd_lcd;
    params.style_no = style_no;
    params.po_no = po_no;
    params.fo_no = fo_no;
    params.bom_no = bom_no;
    params.start = start;
    params.end = end;
    params.start_out = start_out;
    params.end_out = end_out;
    params.buyer = $("#cp_buyer_qr").val().trim();
    $.ajax({
        url: "/fgwms/searchproLotList",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
            }
        }
    })

});
function getDataOutBox(pdata) {
    $('.loading').show();
    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.prd_lcd = $("#prd_lcd").val().trim();
    params.style_no = $("#style_no").val().trim();
    params.po_no = $("#po_no").val().trim();
    params.fo_no = $("#fo_no").val().trim();
    params.bom_no = $("#bom_no").val().trim();
    params.end = $("#start").val().trim();
    params.start = $("#end").val().trim();
    params.start_out = $("#start_out").val().trim();
    params.end_out = $("#end_out").val().trim();

    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    //params._search = pdata._search;

    $.ajax({
        url: "/fgwms/searchproLotList",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
            }
        }
    })
};

//report Lot infomation
function Lot_info(cellValue, options, rowdata, action) {
    //var pno = rowdata.pno;
    var html = '<button class="btn btn-xs btn-primary"  data-pno="' + rowdata.pno + '" onclick="Btn_lot_info(this);">LOT INF</button>';
    return html;
}
function Btn_lot_info(e) {
    var left = ($(window).width() / 2) - (1200 / 2),
      top = ($(window).height() / 2) - (600 / 2)
    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);

    var param = {
        pno: $(e).data('pno')
    };

    window.open("/fgwms/open_Btn_lot_info?" +
        "pno=" + $(e).data('pno'),
        "_blank",
        "width = 1300, height = 800, top=" + top + ", left=" + left);

}
//report Lot infomation

//report User infomation
//function Use_info(cellValue, options, rowdata, action) {
//    var wmtid = rowdata.wmtid;
//    var html = '<button class="btn btn-xs btn-primary"  data-wmtid="' + rowdata.wmtid + '" onclick="Btn_User_info(this);">USE INF</button>';
//    return html;
//}
//function Btn_User_info(e) {
//    var left = ($(window).width() / 2) - (1200 / 2),
//      top = ($(window).height() / 2) - (600 / 2)
//    //popup = window.open("", "popup", "width=900, height=600, top=" + top + ", left=" + left);

//    var param = {
//        wmtid: $(e).data('wmtid')
//    };

//    window.open("/fgwms/open_Btn_use_info?" +
//        "wmtid=" + $(e).data('wmtid'),
//        "_blank",
//        "width = 1300, height = 800, top=" + top + ", left=" + left);

//}
//report User infomation

