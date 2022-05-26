
function convertDate(cellValue, options, rowdata, action) {

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
    $("#list").jqGrid
        ({
            datatype: function (postData) { getData(postData); },
            mtype: 'Get',
            colModel: [
                { label: 'wmtid', name: 'wmtid', hidden: true },
                { label: 'Buyer', name: 'buyer_qr', width: 200, align: 'left' },
                { label: 'PO', name: 'po', width: 200, align: 'left' },
                { label: 'Product', name: 'product', width: 200, align: 'left' },
                { label: 'Composite Code', name: 'mt_cd', width: 300, align: 'left' },
                { label: 'Material', name: 'mt_nm', width: 250, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
                { label: 'Lenght', name: 'lenght', width: 150, align: 'right', formatter: 'integer' },
                { label: 'Size ', name: 'size', width: 150, align: 'right' },
                { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);

                $("#m_dlid").val(rowData.dlid);
                $("#m_dl_no").val(rowData.dl_no);
                $("#m_dl_nm").val(rowData.dl_nm);


            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Delivery Infomation',
            emptyrecords: "No data.",
            height: 200,
            width: null,
            autowidth: false,
            shrinkToFit: false,
            multiselect: true,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            gridComplete: function () {



            }
        });
});

function getData(pdata) {
    var params = new Object();

    if ($("#list").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.dl_no = $('#s_dl_no').val().trim();
    params.dl_nm = $('#s_dl_nm').val().trim();
    params.buyer = $('#s_buyer').val().trim();
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/fgwms/GetDilevery_Return',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list")[0];
                console.log(data);
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
//-------SEARCH------------//
$("#searchBtn").click(function () {

    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getData(pdata);
});

//----- Delivery POPUP------------------//


//----- list1------------------//
var prevCellVal = { cellId: undefined, value: undefined };
$(function () {
    $("#list1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'Buyer QR', name: 'buyer_qr', width: 120, align: 'left' },
                { label: 'Container', name: 'bb_no', width: 120, align: 'left' },
                { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
                { label: 'Material Type', name: 'mt_type_nm', width: 130, align: 'center' },
                { label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'gr_qty', width: 80, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 80, align: 'center' },

                { label: 'Departure', name: 'from_lct_nm', width: 100, align: 'center' },
                { label: 'Location', name: 'to_lct_nm', width: 100, align: 'center' },
                { label: 'Destination', name: 'to_lct_nm', width: 100, align: 'center' },
                { label: 'Location Status', name: 'lct_sts_cd', width: 100, align: 'center' },

                { label: 'Receiving Date', name: 'recevice_dt_tims', width: 130, align: 'center', formatter: convertDate },
                {
                    name: "", width: 100, align: "center", label: "Action", resizable: false, title: false,
                    cellattr: function (rowId, val, rawObject, cm, rdata) {
                        var result;

                        if (prevCellVal.value == val) {
                            result = ' style="" rowspanid="' + prevCellVal.cellId + '"';
                        }
                        else {
                            var cellId = this.id + '_row_' + rowId + '_' + cm.name;

                            result = ` rowspan="${rawObject.rowpan}" id="${cellId}">  <button class="btn btn-xs btn-danger"  data-bx_no="${rawObject.bx_no}"  onclick="Del_Row(this);" > Delete </button`;

                            prevCellVal = { cellId: cellId, value: val };
                        }

                        return result;
                    }
                },
            ],


            pager: jQuery('#listPager1'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: '',
            emptyrecords: "No data.",
            height: 250,
            width: null,
            autowidth: false,
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
            gridComplete: function () {
                prevCellVal = { cellId: undefined, value: undefined };
            }


        });
})
$("#scan_bx_no").on("keydown", function (e) {
    if (e.keyCode === 13) {
        //kiem tra bx_no null ko
        var buyer_no = e.target.value.trim();
        if (buyer_no == "" || buyer_no == null || buyer_no == undefined) {
            $("#scan_bx_no").val("");
            document.getElementById("scan_bx_no").focus();
            return false;
        }
        //ktra tren luoi co ton tai khong
        var rows = $('#list1').jqGrid('getRowData');
        for (i = 0; i < rows.length; i++) {
            var bx_no_view = rows[i].buyer;
            if (buyer_no == bx_no_view) {
                alert("Data Duplicated !!!");
                $("#scan_bx_no").val("");
                document.getElementById("scan_bx_no").focus();
                return false;
            }
        }
        $.ajax({
            url: "/fgwms/GetSortingOK_NG",
            type: "get",
            dataType: "json",
            data: {
                buyer_no: buyer_no
            },
            success: function (response) {
                if (response.result) {

                    for (var i = 0; i < response.data.length; i++) {
                        var id = response.data[i].wmtid;

                        $("#list1").jqGrid('addRowData', id, response.data[i], 'first');
                        $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });

                    }

                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();

                } else {
                    alert(response.message);

                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();
                }

            }
        });

    }

});


function Del_Row(e) {

    var bx_no = e.dataset.bx_no;
    var rows = $('#list1').jqGrid('getRowData');

    for (var item of rows) {
        var wmtid = item.wmtid;
        var bx_no_view = item.bx_no;

        if (bx_no == bx_no_view) {

            $("#list1").jqGrid('delRowData', wmtid);
        }

    }
}
$("#Clear_all").on("click", function () {
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {

        $("#list1").jqGrid('clearGridData');

    }

});

$("#Save_completed").on("click", function () {
    //lay danh sach scan 
    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        alert("Please Scan!!!")
        return false;
    }
    var mang = [];
    for (var i = 0; i < rows.length; i++) {
        var rowData = $("#list1").jqGrid('getRowData', rows[i]);
        mang.push(rowData.buyer_qr);
    }
    $.ajax({
        url: "/fgwms/change_sts_fg_tims?buyer_code=" + mang,
        type: "get",
        dataType: "json",

        success: function (data) {

            if (data.result) {
                alert(data.message);
                $("#list1").jqGrid('clearGridData');

                var timer = setTimeout(
                    
                    function () {
                        debugger;
                        $("#list1").jqGrid('clearGridData');
                        clearTimeout(timer)
                    }, 0);
                return false;
            }
            else {
                alert(data.message);
            }
        }
    });

});

//----- help to focus on input------------------//
//setTimeout(function () { $("#scan_bx_no").focus(); }, 1);
//stock
$(function () {
    $grid = $("#GeneralGrid").jqGrid
        ({
            mtype: 'Get',
            colModel: [
                { label: 'wmtid', name: 'wmtid', hidden: true, key: true },
                { label: 'Buyer', name: 'buyer_qr', width: 200, align: 'left' },
                { label: 'PO', name: 'po', width: 200, align: 'left' },
                { label: 'Product', name: 'product', width: 200, align: 'left' },
                { label: 'Composite Code', name: 'mt_cd', width: 300, align: 'left' },
                { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
                { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            gridComplete: function () {
                $("tr.jqgrow:odd").css("background", "white");
                $("tr.jqgrow:even").css("background", "#f7f7f7");
                $('.loading').hide();

                //$("#GeneralGrid").jqGrid('hideCol', 'setSelection');
            },
            pager: "#GeneralGridPager",
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: 200,
            width: null,

            rowNum: 50,
            caption: '',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            caption: 'Stock Infomation',
            loadonce: false,
            subGrid: false,
            shrinkToFit: false,
            multiselect: true,
            multiboxonly: false,
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
        })
});

function getDataOutBox(pdata) {
    //$('.loading-gif-1').show();
    var params = new Object();
    var rows = $("#GeneralGrid").getDataIDs();
    //if (gridLot.jqGrid('getGridParam', 'reccount') == 0)
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    params.productCode = $('#productCode').val() == null ? "" : $('#productCode').val().trim();
    params.poCode = $('#poCode').val() == null ? "" : $('#poCode').val().trim();
    params.return_date = $('#return_date').val() == null ? "" : $('#return_date').val().trim();

    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        //url: "/fgwms/getFGGeneral",
        url: `/fgwms/getFGgeneralDetail`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#GeneralGrid")[0];
                showing.addJSONData(data);
                //$('.loading-gif-1').hide();
            }
        },
        error: function () {
            //$('.loading-gif-1').hide();
            return;
        }
    });
};
$("#searchBtn_stock").click(function () {
    $("#GeneralGrid").clearGridData();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true });
    var pdata = $("#GeneralGrid").jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata)
});


$("#printBtn").click(function () {
    var model = [];
    var row_subGrid = [];
    //hằng code
    var i, selRowIds = $("#list").jqGrid("getGridParam", "selarrrow"), n, rowData;
    var i, selRowIds1 = $("#GeneralGrid").jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds != undefined) {
        for (var j = 0; j < selRowIds.length; j++) {
            var row = $("#list").jqGrid('getRowData', selRowIds[j]);
            row_subGrid.push(row.buyer_qr);
        }
    }
    if (selRowIds1 != undefined) {
        for (var j = 0; j < selRowIds1.length; j++) {
            var row1 = $("#GeneralGrid").jqGrid('getRowData', selRowIds1[j]);
            row_subGrid.push(row1.buyer_qr);
        }
    }
    console.log(row_subGrid);
    if (row_subGrid.length > 0) {
        var mapForm = document.createElement("form");
        mapForm.target = "Map";
        mapForm.type = "hidden";
        mapForm.method = "POST"; // or "post" if appropriate
        mapForm.action = "/fgwms/PrintReturnTims";

        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = "qrcode";
        mapInput.value = row_subGrid;

        mapForm.appendChild(mapInput);
        document.body.appendChild(mapForm);

        map = window.open(" ", "Map", "width=600, height=800, left=0, top=100, location=no, status=no,");

        if (map) {
            mapForm.submit();
        } else {
            ErrorAlert("You must allow popups for this map to work.'");
        }
    }

    else {
        ErrorAlert("Please Select Grid");
    }
});