
var mt_buyer = "";
var buyer_qr = "";
var bobin_buyer = ""; 
$("#searchBtn").click(function () {
    $('#table_mapping_oqc').clearGridData();
    $('#table_mapping_oqc').jqGrid('setGridParam', { search: true });
    var pdata = $('#table_mapping_oqc').jqGrid('getGridParam', 'postData');
    getData_primary(pdata);
});
$("#table_mapping_oqc").jqGrid
    ({
        datatype: function (postData) { getData_primary(postData); },
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmtid', hidden: true },
            { label: 'PO NO', name: 'at_no', width: 120, sortable: true, align: 'center'},
            { label: 'ML No', name: 'mt_cd', width: 320, sortable: true },
            { label: 'Product', name: 'product', width: 100, },
            { label: 'Total', name: 'gr_qty', align: 'right', width: 100, formatter:'integer' },
            { label: 'Container', name: 'bb_no', width: 230 },
            { label: 'Buyer QR', name: 'buyer_qr', width: 230, align: 'center' },
            { label: 'Date Time', name: 'input_dt', width: 230, align: 'center' },
        ],
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#table_mapping_oqc").jqGrid("getGridParam", 'selrow');
            row_id = $("#table_mapping_oqc").getRowData(selectedRowId);
            mt_buyer = row_id.mt_cd;
            buyer_qr = row_id.buyer_qr; 
            bobin_buyer = row_id.bb_no;
        },
        gridComplete: function () {
            var rows = $("#table_mapping_oqc").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var buyer_qr = $("#table_mapping_oqc").getCell(rows[i], "buyer_qr");
                if (buyer_qr != "") {
                    $("#table_mapping_oqc").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                }
            }
        },
        pager: jQuery('#table_mapping_oqc_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: false,
        height: 500,
        multiselect: true,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Composite',
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


$(document).ready(function () {
    $("#bb_no").on("keydown", function (e) {
        if (e.keyCode === 13) {
            var that = $(this);
            $("#buyer_qr").focus();
        }
    })
    $("#buyer_qr").on("keydown", function (e) {
        if (e.keyCode === 13) {
            if ($("#bb_no").val() == "") {
                ErrorAlert("Vui lòng Scan mã Container");
                $("#bb_no").val("");
                $('#bb_no').focus();
                return;
            }
            if ((!$("#buyer_qr").val()) && (buyer_qr == "")) {
                ErrorAlert("Vui lòng Scan mã buyer Qr");
                $("#buyer_qr").val("");
                $('#buyer_qr').focus();
                return;
            }

            var that = $(this);
            $.get("/TIMS/MappingBuyerTims?bb_no=" + $("#bb_no").val().trim() + "&buyer_qr=" + $("#buyer_qr").val().trim(), function (data) {


                if (data.result) {
                    debugger;
                    $("#bb_no").val("");
                    $('#bb_no').focus();
                    $("#buyer_qr").val("");
                    SuccessAlert("Success");
                    $("#table_mapping_oqc").setRowData(data.dataWeb[0].wmtid, data.dataWeb[0], { background: "#d0e9c6" });
                    //$.each(data.kq, function (key, item) {
                    //    var myList = $("#table_mapping_oqc").jqGrid('getDataIDs');
                    //    for (var i = 0; i < myList.length; i++) {
                    //        var rowData = $("#table_mapping_oqc").getRowData(myList[i]);
                    //        if (rowData.bb_no == bobin_buyer) {
                    //            rowData.buyer_qr = $("#buyer_qr").val();
                    //            $("#table_mapping_oqc").jqGrid('setRowData', myList[i], rowData);
                    //            $("#table_mapping_oqc").setRowData(myList[i], false, { background: "#d0e9c6" });
                    //            $("#bb_no").val("");
                    //            $('#bb_no').focus();
                    //            $("#buyer_qr").val("");
                    //        }
                    //    }
                    //});
                } else {
                    ErrorAlert(data.message);
                    $("#bb_no").val("");
                    $('#bb_no').focus();
                    $("#buyer_qr").val("");
                }
            });
        }
    });
});

$("#Mapping_Buyer").click(function () {
    if ($("#bb_no").val() =="") {
        alert("Vui lòng Scan Đồ đựng của bạn");
        $("#bb_no").val("");
        $('#bb_no').focus();
        return;
    }
    if ((!$("#buyer_qr").val()) && (buyer_qr == "")) {
        alert("Vui lòng Scan mã buyer Qr");
        $("#buyer_qr").val("");
        $('#buyer_qr').focus();
        return;
    }
    else {
        $.get("/TIMS/MappingBuyerTims?bb_no=" + $("#bb_no").val().trim() + "&buyer_qr=" + $("#buyer_qr").val().trim(), function (data) {
            if (data.result) {
                debugger;
                $("#bb_no").val("");
                $('#bb_no').focus();
                $("#buyer_qr").val("");
                SuccessAlert("Success");
                $("#table_mapping_oqc").setRowData(data.dataWeb[0].wmtid, data.dataWeb[0], { background: "#d0e9c6" });
                //$.each(data.kq, function (key, item) {
                //    var myList = $("#table_mapping_oqc").jqGrid('getDataIDs');
                //    for (var i = 0; i < myList.length; i++) {
                //        var rowData = $("#table_mapping_oqc").getRowData(myList[i]);
                //        if (rowData.bb_no == bobin_buyer) {
                //            rowData.buyer_qr = $("#buyer_qr").val();
                //            $("#table_mapping_oqc").jqGrid('setRowData', myList[i], rowData);
                //            $("#table_mapping_oqc").setRowData(myList[i], false, { background: "#d0e9c6" });
                //            $("#bb_no").val("");
                //            $('#bb_no').focus();
                //            $("#buyer_qr").val("");
                //        }
                //    }
                //});
            } else {
                ErrorAlert(data.message);
                $("#bb_no").val("");
                $('#bb_no').focus();
                $("#buyer_qr").val("");
            }
        });
    }
});

function getData_primary(pdata) {
    var params = new Object();
    if ($('#table_mapping_oqc').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.product = $("#Product").val().trim();
    params.bb_bo = $("#container").val().trim();
    params.at_no = $("#at_no").val().trim();
    $('#table_mapping_oqc').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/Getmt_mappingOQC`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#table_mapping_oqc')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

$("#ChangeTemgoi").on("click", function () {
    $('.popup-dialog.ViewStatusTemGoi').dialog('open');
    $.get("/TIMS/PartialView_ViewStatusTemGoiPopup" 
        , function (html) {
            $("#PartialView_ViewStatusTemGoiPopup").html(html);
        });
});
$("#Shipping").on("click", function () {

    var r = confirm("Bạn có chắc muốn xuất khỏi kho TIMS không?");
    if (r === true) {

        var i, check_jqgridng = $("#table_mapping_oqc").jqGrid("getGridParam", "selarrrow"), n, rowData;

        if (check_jqgridng.length < 1) {
            alert("Vui lòng chọn 1 container để tiếp tục");
            return;
        }
        $.get("/TIMS/ShippingContainer?wmtid=" + check_jqgridng, function (data) {
            if (data.result == true) {
                SuccessAlert(data.message);
                $('#table_mapping_oqc').clearGridData();
                $('#table_mapping_oqc').jqGrid('setGridParam', { search: true });
                var pdata = $('#table_mapping_oqc').jqGrid('getGridParam', 'postData');
                getData_primary(pdata);
                return;

            } else {
                ErrorAlert(data.message);
                return;
            }

        });
    }
});

//----- help to focus on input------------------//
setTimeout(function () { $("#bb_no").focus(); }, 1);