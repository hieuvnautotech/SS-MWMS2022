
//----- RD POPUP------------------//
function rd_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-rd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
    return html;
};
$('#s_lot_date').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#end_lot_date').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
function ViewSDPopup(e) {
    $('.popup-dialog.RD_Info_Popup').dialog('open');
    var rd_no = $(e).data('rd_no');


    $.get("/TIMS/PartialView_RD_Info_Popup?" +
        "rd_no=" + rd_no + ""
        , function (html) {
            $("#PartialView_RD_Info_Popup").html(html);
        });
}

//----- list1------------------//
var prevCellVal = { cellId: undefined, value: undefined };
$(function () {
    $("#list1").jqGrid
        ({
            datatype: function (postData) { getData_rece(postData); },
            mtype: 'post',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'PO', name: 'po', width: 120, align: 'center' },
                { label: 'Product', name: 'product', width: 120, align: 'left' },
                { label: 'Product Name', name: 'product_nm', width: 150, align: 'left' },
                { label: 'Lot Date', name: 'lot_date', width: 100, align: 'left' },
                { label: 'Buyer QR', name: 'buyer_qr', width: 250, align: 'center' },
                { label: 'Quantity', name: 'gr_qty', width: 90, align: 'right', formatter: 'integer' },
                { label: 'Bobbin No', name: 'bb_no', width: 233, align: 'left' },
                { label: 'ML NO', name: 'mt_cd', width: 310, align: 'left', hidden: true  },
                { label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'real_qty', width: 100, align: 'right', formatter: 'integer', hidden: true },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 80, align: 'center', hidden: true  },
            ],
            pager: jQuery('#listPager1'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: false, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: '',
            emptyrecords: "No data.",
            caption: 'List',
            height: 500,
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
                //if ($("#list1").getDataIDs().length > 0) {
                //    $(`#FG-WMS`).append(`<i class="fa fa-bell" id="bellFGWMS" style="margin-left:5px;color:red;" aria-hidden="true" style="color:red" title="` + $("#list1").getDataIDs().length + `"></i>`);
                //}
            }


        });
    $("#list1").jqGrid("navGrid", "#listPager1", {
        add: false, edit: false, del: false, search: false, refresh: true
    });
})
$("#scan_buyer_qr").on("keydown", function (e) {

    if (e.keyCode === 13) {
        //kiem tra ml_no null ko
        var buyer_qr = e.target.value.trim();
        if (buyer_qr == "" || buyer_qr == null || buyer_qr == undefined) {
            $("#scan_buyer_qr").val("");
            document.getElementById("scan_buyer_qr").focus();
            return false;
        }
       else {

            $.ajax({
                url: "/fgwms/InsertMTQR_EXT_List?buyer_qr=" + buyer_qr,
                type: "get",
                dataType: "json",
                success: function (response) {
                    if (response.result) {

                        $("#scan_buyer_qr").val("");
                        document.getElementById("scan_buyer_qr").focus();

                        for (var i = 0; i < response.data.length; i++) {
                            var id = response.data[i].wmtid;

                            $("#list1").setRowData(id, response.data[i], { background: "#28a745", color: "#fff" });

                        }
                        var timer = setTimeout(
                            function () {
                                debugger;
                                for (var i = 0; i < response.data.length; i++) {
                                    var id = response.data[i].wmtid;
                                    $("#list1").jqGrid('delRowData', id);
                                }
                                clearTimeout(timer)
                            }, 0);
                        return false;
                    }
                    else {
                        alert(response.message);
                        $("#scan_buyer_qr").val("");
                        document.getElementById("scan_buyer_qr").focus();
                    }
                }
            });

        }


    }

});


//-------SEARCH------------//
$("#searchBtn").click(function () {
    $('#list1').clearGridData();
    $('#list1').jqGrid('setGridParam', { search: true });
    var pdata = $('#list1').jqGrid('getGridParam', 'postData');
    getData_rece(pdata);
});
//----- help to focus on input------------------//

function getData_rece(pdata) {
    var params = new Object();
    debugger;
    if ($('#list1').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var po_no = $('#s_po_no').val().trim();
    var product = $('#s_product').val().trim();
    var buyer = $('#s_buyer').val().trim();
    var lot_date = $('#s_lot_date').val().trim();
    var lot_date_end = $('#end_lot_date').val().trim();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.po = po_no;
    params.product = product;
    params.lot_date = lot_date;
    params.lot_date_end = lot_date_end;
    params.buyer = buyer;

    $('#list1').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/fgwms/GetReceivingScanMLQR",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            console.log(data);
            if (st == "success") {
                var showing = $('#list1')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

json_object = "";
tempList = [];
$(`#excelupload_input`).change(function (evt) {
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event) {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName) {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            for (var i = 0; i < XL_row_object.length; i++) {
                var data_row_tmp = ``;
                if (XL_row_object[i]["CODE"].toLowerCase() != undefined) {
                    data_row_tmp = XL_row_object[i]["CODE"].toLowerCase();
                    excelData.push(data_row_tmp);
                }

            }
        });

        json_object = excelData;

    };

    reader.onerror = function (event) {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});
$(`#ExcelUpload`).on(`click`, function () {
    tempList = [];

    for (var item of json_object) {
        data = {
            Code: item
        };
        tempList.push(data);
    }
    //$(`#excelupload_input`).val('');
    $.ajax({
        url: `/fgwms/UploadExcelFGRecei`,
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tempList),
        traditonal: true
    })
        .done(function (response) {
            if (response.result) {
                debugger;
                if (response.ss) {
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();

                    for (var i = 0; i < response.dataThoaDk.length; i++) {
                        var id = response.dataThoaDk[i].wmtid;

                        $("#list1").setRowData(id, response.dataThoaDk[i], { background: "#28a745", color: "#fff" });

                    }
                    var timer = setTimeout(
                        function () {
                            debugger;
                            for (var i = 0; i < response.dataThoaDk.length; i++) {
                                var id = response.dataThoaDk[i].wmtid;
                                $("#list1").jqGrid('delRowData', id);
                            }
                            clearTimeout(timer)
                        }, 0);
                    var chuoihT = "Hoàn thành:   " + response.ok + "</BR>";
                    var chuoiDatonTai = "Đã nhập kho:   " + response.dsDatontai + "</BR>";
                    var chuoiLOI = "Không tồn tại:  " + " " + response.ng + "</BR>";
                    SuccessAlert(chuoihT + chuoiLOI + chuoiDatonTai);

                    $(`#excelupload_input`).val(``);
                    tempList = [];
                    json_object = "";
                    return false;
                }
                else {
                    var chuoihT = "Hoàn thành:   " + response.ok + "</BR>";
                    var chuoiDatonTai = "Đã nhập kho:   " + response.dsDatontai + "</BR>";
                    var chuoiLOI = "Không tồn tại:  " + " " + response.ng + "</BR>";

                    SuccessAlert(chuoihT + chuoiLOI + chuoiDatonTai);

                    $(`#scan_buyer_qr`).val(``);
                    $(`#excelupload_input`).val(``);
                    tempList = [];
                    json_object = "";
                    return false;
                }

            }
            else {
                ErrorAlert(response.message);
                $(`#excelupload_input`).val(``);
                tempList = [];
                json_object = "";
                return false;
            }


        })
        .fail(function () {
            ErrorAlert(`Lỗi hệ thống.`);
            $(`#excelupload_input`).val(``);
            tempList = [];
            json_object = "";
            return false;
        });
});




//SCAN BUYER FUNCTION
var ListBuyer = [];




$("#scan_temgoi").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var buyer = $("#scan_temgoi").val().trim();



        var IsExist = (ListBuyer.indexOf(buyer));
        if (IsExist != -1 && ListBuyer.length > 0) {

            $.blockUI({
                message: ("Dữ liệu đã được scan rồi, vui lòng scan mã khác"),
                timeout: 500,
                css: {

                    border: 'none',
                    padding: '15px',
                    backgroundColor: 'red',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 1,
                    color: '#fff',
                    fontSize: '20px'
                }
            });
            $("#scan_temgoi").val("");
            document.getElementById("scan_temgoi").focus();
            return false;
        }
        //var buyer = e.target.value.trim();
        if (buyer == "" || buyer == null || buyer == undefined) {
            $("#scan_temgoi").val("");
            document.getElementById("scan_temgoi").focus();
            return false;
        };
        //lấy product trong bảng 2 ra
        var rows = $('#list2').jqGrid('getRowData');
        if (rows.length > 0) {

            //Mã product là: từ đầu đến DZIH

            //let NumberDZIH = rows[0].BuyerCode;
            //var endProduct = NumberDZIH.search("DZIH")
            //var product = rows[0].BuyerCode.substring(0, endProduct)



            //Mã product là: từ đầu đến DZIH

            let NumberDZIH = rows[0].BuyerCode.toUpperCase();
            var endProduct = NumberDZIH.search("DZIH");
            if (endProduct <= 0) {
                endProduct = NumberDZIH.search("EA8D");
            }
            var product = rows[0].BuyerCode.substring(0, endProduct)




            if (!buyer.includes(product)) {
                ErrorAlert("Không cùng PRODUCT");
                $("#scan_temgoi").val("");
                document.getElementById("scan_temgoi").focus();
                return false;
            }
            //var product = rows[0].ProductNo.replace("-", "");
            //kiểm tra tem vào có cùng loại với product trước đó không,
            //if (!buyer.includes(product)) {
            //    $.blockUI({
            //        message: ("Không cùng PRODUCT"),
            //        timeout: 500,
            //        css: {

            //            border: 'none',
            //            padding: '15px',
            //            backgroundColor: 'red',
            //            '-webkit-border-radius': '10px',
            //            '-moz-border-radius': '10px',
            //            opacity: 1,
            //            color: '#fff',
            //            fontSize: '20px'
            //        }
            //    });
            //    $("#scan_temgoi").val("");
            //    document.getElementById("scan_temgoi").focus();
            //    return false;
            //}
        }
        $.ajax({

            url: "/fgwms/CheckStatusBuyerToRece",
            type: "Get",
            dataType: "json",
            data: {
                buyerCode: buyer
            },

            success: function (response) {
                if (response.result) {
                    ListBuyer.push(buyer);

                    $("#list2").jqGrid('addRowData', response.data.wmtid, response.data, 'first');
                    $("#list2").setRowData(response.data.wmtid, false, { background: "#28a745", color: "#fff" });

                    $("#scan_temgoi").val("");
                    document.getElementById("scan_temgoi").focus();
                    $.unblockUI();
                    SuccessAlert(response.message);
                    return false;
                }
                else {
                    $("#scan_temgoi").val("");
                    document.getElementById("scan_temgoi").focus();

                    $.blockUI({
                        message: response.message,
                        timeout: 2000,
                        css: {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: 'red',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: 1,
                            color: '#fff',
                            fontSize: '20px'
                        }
                    });

                    ErrorAlert(response.message);
                    return false;
                }
            }
        });
    }
});
function Del_Row(e) {
    $(e).parent().remove();
}

$("#Save_completed").on("click", function () {
    var rowIDMes = []
    var rowIDSap = []

    var rows = $("#list2").jqGrid('getRowData');


    for (i = 0; i < rows.length; i++) {
        var TypeSystem = rows[i].TypeSystem;

        if (TypeSystem == "" || TypeSystem == "MES") {

            rowIDMes.push(rows[i].BuyerCode);
        }
        if (TypeSystem == "SAP") {
            rowIDSap.push(rows[i].wmtid);
        }
    }

    if (rows.length < 1) {
        ErrorAlert("Vui lòng scan mã buyer để tiếp tục.");
        return;
    }
    var model1 = rows[0].Model;
    object = {};


    object['WmtidMES'] = rowIDMes;
    object['WmtidsAP'] = rowIDSap;
    object['ModelCode'] = model1;
    if (rows == "" || rows == null || rows == undefined) {

        $.blockUI({
            message: "Dữ liệu rỗng, Vui lòng scan Nguyên Vật Liệu để tiếp tục",
            timeout: 500,
            css: {

                border: 'none',
                padding: '15px',
                backgroundColor: 'red',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: 1,
                color: '#fff',
                fontSize: '20px'
            }
        });
        $("#scan_temgoi").val("");
        document.getElementById("scan_temgoi").focus();
        return false;
    }
    var r = confirm("Bạn có chắc muốn NHẬP KHO FG không?");
    if (r === true) {
        $.ajax({
            url: "/fgwms/UpdateReciFG",
            type: "Put",
            contentType: 'application/json',
            data: JSON.stringify(object),
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    $("#list2").jqGrid('clearGridData');
                    ListBuyer = []
                    return;
                }
                else {
                    ErrorAlert(data.message);
                    return;
                }
            }
        });
    }
    return false;

});
$("#Clear_all_box").on("click", function () {
    var r = confirm("Chọn OK để xóa danh sách scan.");
    if (r === true) {
        $("#list2").jqGrid('clearGridData');
        ListBuyer = [];
        $(`#excelupload_input1`).val(``);
        tempList = [];
        json_object = "";

        $("#result").html("");
        return false;
    }
    return false;
});

//upload excel
JsonDataBuyer = "";
$(`#excelupload_input1`).change(function (evt) {
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event) {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName) {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            for (var i = 0; i < XL_row_object.length; i++) {
                var data_row_tmp = ``;
                if (XL_row_object[i]["CODE"].toLowerCase() != undefined) {
                    data_row_tmp = XL_row_object[i]["CODE"].toLowerCase();
                    excelData.push(data_row_tmp);
                }
            }
        });
        JsonDataBuyer = excelData;
    };
    reader.onerror = function (event) {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});
$(`#ExcelUpload1`).on(`click`, function () {

    tempList = [];

    for (var item of JsonDataBuyer) {
        data = {
            Code: item.toUpperCase(),
        };
        tempList.push(data);
    }
    if (tempList.length == 0) {

        alert("Vui lòng chọn file excel");
        return false;
    }
    $.blockUI({ message: '<h4>Loading...</h4>' });

    $.ajax({
        url: `/fgwms/UploadExcelFGRecei1`,
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tempList),
        traditonal: true
    })
        .done(function (response) {
            $.unblockUI();
            if (response.result) {

                if (response.ss) {


                    $("#scan_temgoi").val("");
                    document.getElementById("scan_temgoi").focus();
                    $("#list2").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response.dataThoaDk, background: "#d0e9c6" }).trigger("reloadGrid");





                    $("#list4").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response.dataKhongCoTrongHeThong, background: "#d0e9c6" }).trigger("reloadGrid");




                    var chuoihT = "Đủ yêu cầu NHẬP KHO:   " + response.ok + "</BR>";
                    var chuoiDatonTai = "Đã nhập kho:   " + response.dsDatontai + "</BR>";

                    var chuoiKhacProduct = "Khác Product:  " + " " + response.khacproduct + "</BR>";
                    var DataKhongTontaitronghethong = "Tem gói không tồn tại:  " + " " + response.dataSoluongKhongCoTrongHeThong + "</BR>";

                    $("#result").html("<label class='text-success'>&nbsp;&nbsp;Đủ yêu cầu NHẬP KHO: " + response.ok + "</label><br/>" + "<label class='text-primary'>&nbsp;&nbsp;Đã nhập kho: " + response.dsDatontai + "</label></br/>" + "<label class='text-danger'>&nbsp;&nbsp;Khác Product: " + response.khacproduct + "</label></br>" + "<label class='text-danger'>&nbsp;&nbspKhông tồn tại trong hệ thống " + response.dataSoluongKhongCoTrongHeThong + "</label>");

                    $.blockUI({
                        message: chuoihT + chuoiDatonTai + chuoiKhacProduct + DataKhongTontaitronghethong,
                        timeout: 2000,
                        css: {
                            theme: true,
                            border: 'none',
                            padding: '15px',
                            backgroundColor: 'red',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: 1,
                            color: '#fff',
                            fontSize: '20px'
                        },

                    });

                    var abnc = response.dataKhacProduct;
                    if (abnc.length > 0) {
                        if (abnc[0].BuyerCode != '' && abnc[0].BuyerCode != null) {
                            response.dataKhacProduct.forEach(function (element) {

                                $("#list4").jqGrid('addRowData', element, element, 'first');
                                $("#list4").setRowData(element.BuyerCode, false, { background: "#e76d6d", color: "#fff", fontSize: "13px" });
                            });
                        }
                    }


                    $(`#excelupload_input1`).val(``);
                    tempList = [];
                    JsonDataBuyer = "";
                    return false;
                }
                else {


                    $(`#scan_buyer_qr`).val(``);
                    $(`#excelupload_input1`).val(``);
                    tempList = [];
                    JsonDataBuyer = "";
                    return false;
                }

            }
            else {
                ErrorAlert(response.message);
                $(`#excelupload_input1`).val(``);
                tempList = [];
                JsonDataBuyer = "";
                return false;
            }


        })
        .fail(function () {
            $.unblockUI();
            ErrorAlert(`Lỗi hệ thống.`);
            $(`#excelupload_input1`).val(``);
            tempList = [];
            JsonDataBuyer = "";
            return false;
        });
});



$('#excelBtn').click(function () {
    //debugger;
    var po_no = $('#s_po_no').val().trim();
    var product = $('#s_product').val().trim();
    var buyer = $('#s_buyer').val().trim();
    var lot_date = $('#s_lot_date').val().trim();
    var lot_date_end = $('#end_lot_date').val().trim();

    $(`#exportData`).attr(`action`, `/fgwms/ExportToExcelReceiFG?po_no=${po_no}&product=${product}&buyer=${buyer}&lot_date=${lot_date}&lot_date_end=${lot_date_end}`);

});

$("#list4").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [

            {
                name: 'BuyerCode', key: false, sortable: true, width: 280, align: 'center', label: 'Buyer Code', title: false
            },

        ],

        pager: jQuery('#listPager4'),
        viewrecords: true,
        rowList: [500, 1000, 2000, 5000, 10000],
        rowNum: 10000,
        height: 400,
        width: null,
        autowidth: false,
        caption: 'Tem gói không đạt yêu cầu',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        loadonce: true,
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
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

    });

$("#list2").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmtid', hidden: true },
            { name: 'TypeSystem', key: false, hidden: true },
            { name: 'ProductNo', sortable: true, width: 100, align: 'left', label: 'Product' },
            {
                name: 'BuyerCode', key: false, sortable: true, width: 280, align: 'center', label: 'Buyer Code', title: false
            },
            {
                name: 'Quantity', sortable: true, align: 'right', label: 'Total',
                formatter: 'integer', formatoptions: { thousandsSeparator: ",", deaultValue: "0" },

                width: 80,
            },

            { name: 'ProductName', sortable: true, width: 100, align: 'left', label: 'Product Name' },
            { name: 'Model', sortable: true, width: 100, align: 'left', label: 'Model' },
            { name: 'bb_no', sortable: true, width: 250, align: 'center', label: 'Bobbin No' },
        ],
        footerrow: true,
        userDataOnFooter: true,
        pager: jQuery('#listPager2'),
        viewrecords: true,
        rowList: [500, 1000, 2000, 5000, 10000],
        rowNum: 10000,
        height: 400,
        width: null,
        autowidth: false,
        caption: `Tem gói thỏa điều kiện nhập kho`,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        loadonce: true,
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
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
        gridComplete: function () {
            prevCellVal = { cellId: undefined, value: undefined };

            var ids = $("#list2").jqGrid('getDataIDs');
            var sum_Quantity = 0;
            var $self = $(this)
            for (var i = 0; i < ids.length; i++) {

                sum_Quantity += parseInt($("#list2").getCell(ids[i], "Quantity"));
            }
            $self.jqGrid("footerData", "set", { ProductNo: "Total" });
            $self.jqGrid("footerData", "set", { Quantity: sum_Quantity });
        },
    });