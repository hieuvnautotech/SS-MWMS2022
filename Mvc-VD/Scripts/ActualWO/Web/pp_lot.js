$(function () {
    $(".lot_pp").dialog({
        width: '55%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popuplot").jqGrid
            ({
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                     { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                    { label: 'Lot Code', name: 'mt_cd', width: 350, },
                    { label: 'QR Code', name: 'mt_qrcode', width: 250, align: 'center' },
                    { label: 'Material', name: 'mt_no', width: 250, align: 'center' },
                    { label: 'Fac', name: 'lct_cd', width: 100, align: 'left' },
                    { label: 'Process', name: 'prounit_cd', width: 100, align: 'left' },
                    { label: 'Date', name: 'date', width: 100, align: 'center' },
                    { label: 'GR QTY', name: 'gr_qty', width: 100, align: 'right' },
                    { label: 'QR Code', name: 'mt_barcode', width: 350,  },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popuplot").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popuplot").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#selected").click(function () {
                            $('#s_mt_cd').val(row_id.mt_barcode);
                            $('.lot_pp').dialog('close');
                        });
                    }
                },

                pager: jQuery('#pagerlot'),
                viewrecords: true,
                height: 400,
                rowNum: 50,
                rownumbers: false,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
                rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
                // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
                loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
                emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
                gridview: true,
                shrinkToFit: false,
                datatype: function (postData) { getDataOutBox(postData); },
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

        },
    });

    $(".poupdialoglot").click(function () {
        $('.lot_pp').dialog('open');
    });
    $('#closelot').click(function () {
        $('.lot_pp').dialog('close');
    });

});



$("#searchBtn_popuplot").click(function () {

    var grid = $("#popuplot");
    grid.jqGrid('setGridParam', { search: true });
    var postData = grid.jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = postData.rows;
    params.sidx = postData.sidx;
    params.sord = postData.sord;
    params._search = postData._search;

    var mt_cd = $("#mt_cd").val();
    params.mt_cd = mt_cd;
   

    $.ajax({
        url: '/ActualWO/getcomLot_pp',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            console.log(data);
            if (st == "success") {
                var grid = $("#popuplot")[0];
                grid.addJSONData(data);
            }
        }
    })
});

function getDataOutBox(pdata) {
    var mt_cd = $("#mt_cd").val();
    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mt_cd = mt_cd;
 

    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/ActualWO/getcomLot_pp',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            console.log(data);
            if (st == "success") {
                var grid = $("#popuplot")[0];
                grid.addJSONData(data);
            }
        }
    })
}