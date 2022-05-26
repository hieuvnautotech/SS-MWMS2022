//$(function () {
//    $("#list").jqGrid
//    ({
//        url: "/ActualWO/getList_box_info_status",
//        datatype: 'json',
//        mtype: 'Get',
//        colModel: [
//             { key: true, label: 'bxno', name: 'bxno', width: 50, hidden: true },
//             { label: 'Tray Box Code', name: 'bx_no', width: 110, align: 'center' },
//             { label: 'Tray Box Name', name: 'bx_nm', width: 110, align: 'center' },
//             { label: 'Product Unit Qty', name: 'prd_qty', width: 100, align: 'right' },
//             { label: 'Product Real Qty', name: 'prd_mapping_qty', width: 100, align: 'right' },
//             { label: 'Create User', name: 'reg_id', width: 110, align: 'left' },
//             { label: 'Change User', name: 'chg_id', width: 180, align: 'left' },

//             { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
//             { label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
         
//        ],
//        formatter: {
//            integer: { thousandsSeparator: ",", defaultValue: '0' },
//            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
//            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
//        },
//        onSelectRow: function (rowid, status, e) {

//            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
//            row_id = $("#list").getRowData(selectedRowId);

//            $("#list1").setGridParam({ url: "/ActualWO/getList_box_status_mapping?" + "bx_no=" + row_id.bx_no, datatype: "json" }).trigger("reloadGrid");
//        },

//        pager: jQuery('#gridpager'),
//        viewrecords: true,
//        rowList: [50, 100, 200, 500, 1000],
//        height: "300",
//        width: null,
//        caption: 'Tray Box',
//        rowNum: 50,
//        loadtext: "Loading...",
//        emptyrecords: "No data.",
//        rownumbers: true,
//        gridview: true,
//        shrinkToFit: false,
//        loadonce: true,
//        viewrecords: true,
//        jsonReader:
//        {
//            root: "rows",
//            page: "page",
//            total: "total",
//            records: "records",
//            repeatitems: false,
//            Id: "0"
//        },
//    })
//    $("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

//    $("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
//});

//$(function () {
//    $("#list1").jqGrid
//    ({
//        url: "/ActualWO/getList_box_status_mapping",
//        datatype: 'json',
//        mtype: 'Get',
//        colModel: [
//             { key: true, label: 'bxno', name: 'bxno', width: 50, hidden: true },
//             { label: 'Tray Box Code', name: 'bx_no', width: 110, align: 'center' },
//             { label: 'Product Code', name: 'style_no', width: 180, align: 'left' },
//             { label: 'Product Name', name: 'style_nm', width: 180, align: 'left' },
//             { label: 'Group Qty', name: 'gr_qty', width: 100, align: 'right' },
//             { label: 'Status', name: 'sts_cd', width: 110, align: 'center' },
//             { label: 'Location', name: 'lct_cd', width: 180, align: 'left' },
//             { label: 'Location Status', name: 'lct_sts_cd', width: 180, align: 'center' },

//             //{ label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
//             //{ label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

//        ],
//        formatter: {
//            integer: { thousandsSeparator: ",", defaultValue: '0' },
//            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
//            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
//        },
//        onSelectRow: function (rowid, status, e) {

//            var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
//            row_id = $("#list1").getRowData(selectedRowId);

//        },

//        pager: jQuery('#gridpager1'),
//        viewrecords: true,
//        rowList: [50, 100, 200, 500, 1000],
//        height: "300",
//        width: null,
//        caption: 'Product List',
//        rowNum: 50,
//        loadtext: "Loading...",
//        emptyrecords: "No data.",
//        rownumbers: true,
//        gridview: true,
//        shrinkToFit: false,
//        loadonce: true,
//        viewrecords: true,
//        jsonReader:
//        {
//            root: "rows",
//            page: "page",
//            total: "total",
//            records: "records",
//            repeatitems: false,
//            Id: "0"
//        },
//    })
 
//});


//$("#searchBtn").click(function () {
//    var bx_no = $('#bx_no').val().trim();
//    var bx_nm = $('#bx_nm').val().trim();
//    var style_no = $('#style_no').val().trim();
//    var prd_lcd = $('#prd_lcd').val().trim();
//    $.ajax({
//        url: "/ActualWO/searchTrayBoxStatus",
//        type: "get",
//        dataType: "json",
//        data: {
//            bx_no: bx_no,
//            bx_nm: bx_nm,
//            style_no: style_no,
//            prd_lcd: prd_lcd,
//        },
//        success: function (result) {
//            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
//        }
//    });

//});