$(document).ready(function () {
    $('#return_date').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });

    $('#recevice_dt_start').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#recevice_dt_end').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
    $('#change_lotno').datepicker({
        dateFormat: 'yy-mm-dd',
        "autoclose": true
    });
});

$("#GeneralGrid").jqGrid
    ({
        datatype: function (postData) { getDataOutBox(postData); },
        mtype: 'get',
        colModel: [
            { label: 'id', name: 'id', width: 200, align: 'left', key: true, hidden: true },
            { label: 'Model', name: 'md_cd', width: 300, align: 'left' },
            { label: 'Product', name: 'product_code', width: 300, align: 'left' },
            { label: 'Product Name', name: 'product_name', width: 300, align: 'left' },
            { label: 'Type', name: 'bom_type', width: 300, align: 'left' },
            { label: 'Tồn kho (EA)', name: 'qty', width: 300, align: 'right', formatter: 'integer' },

        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },

        pager: jQuery('#GeneralGridPager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 600,
        width: null,
        autowidth: false,
        rowNum: 50,
        caption: '',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        loadonce: false,
        shrinkToFit: false,
        multiselect: false,
        subGrid: true,
        subGridRowExpanded: showChildGrid,
        footerrow: true,
        userDataOnFooter: true,


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

            //sum Quantity
            var ids = $("#GeneralGrid").jqGrid('getDataIDs');
            var sum_Quantity = 0;
            var $self = $(this)
            for (var i = 0; i < ids.length; i++) {
                sum_Quantity += parseInt($("#GeneralGrid").getCell(ids[i], "qty"));
            }

            $self.jqGrid("footerData", "set", { model: "Total" });
            $self.jqGrid("footerData", "set", { qty: sum_Quantity });


            /*jQuery("#GeneralGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');*/
        }
    });



var wmtid_DT = '';
var childGridID = '';
function showChildGrid(parentRowID, parentRowKey) {
    var flag = false;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";

    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    wmtid_DT = parentRowKey;
    $("#" + childGridID).jqGrid({
        mtype: "GET",
        datatype: function (postData) { getDataDT(postData); },
        colModel: [
            { key: true, label: 'id', name: 'id', width: 50, align: 'right', hidden: true },

            { label: 'Lot No.', name: 'lot_no', width: 100, align: 'center' },

            { label: 'Buyer', name: 'buyer_qr', width: 250, align: 'center' },
            { label: 'PO NO', name: 'at_no', width: 110, align: 'center' },

            {
                key: false, label: 'QTy (Roll/EA)', name: 'qty', width: 100, align: 'right', editable: true, editoptions: {
                    dataInit: function (element) {
                        $(element).keydown(function (e) {

                            if (e.which == 13) {
                                var value = this.value;
                                var id = e.target.id.replace("_qty", "");
                                var row = $("#" + childGridID).jqGrid('getRowData', id);
                                update_pr(value, id);
                            }
                        });
                    }
                },
            },
            { label: 'Status', name: 'statusName', width: 80, align: 'center' },

            { label: 'Receiving Data', name: 'reg_dt', editable: false, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
        ],
        shrinkToFit: false,
        width: null,
        height: '100%',
        pager: "#" + childGridPagerID,
        rowNum: 50,
        rownumbers: true,
        loadonce: false,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
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
        //onCellSelect: function (rowid) {
        //    var lastSel = "";
        //    if (rowid && rowid !== lastSel) {
        //        $("#" + childGridID).restoreRow(lastSel);
        //        lastSel = rowid;
        //    }
        //    $("#" + childGridID).editRow(rowid, true);
        //},
        onCellSelect: editRow,
        onSelectRow: function (id, rowid, status, e) {
            if (id) {
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }

                var lastSelection = "";

                if (rowid && rowid !== lastSelection) {
                    var grid = $("#" + childGridID);
                    grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
                    lastSelection = rowid;


                }



            }
        },

        onSelectAll: function (id, rowid, status, e) {
            if (id) {
                var selRowIds_LV2 = $("#GeneralGrid_" + parentRowKey + "_table").jqGrid("getGridParam", "selarrrow");

                if (selRowIds_LV2 != null && flag == false) {
                    flag = true;
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                }
                if (selRowIds_LV2.length == 0) {
                    $("#GeneralGrid").jqGrid('setSelection', parentRowKey, true);
                    flag = false;
                }
            }
        }
    });
}
var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#" + childGridID);
        grid.jqGrid('editRow', id, { keys: true, focusField: 3 });
        lastSelection = id;
    }


}
function update_pr(value, id) {
    var xacnhan = confirm("Bạn có muốn sửa lại số lượng của mã tem này không?");
    if (xacnhan) {
        var giatri = value;
        $.ajax({
            url: '/FGWmsGeneral/updateQuantity?id=' + id + "&buyerQtuantity=" + giatri,
            type: "get",
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    var rowData = $("#" + childGridID).jqGrid('getRowData', id);
                    rowData.qty = giatri;
                    $("#" + childGridID).setRowData(id, rowData, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                    return;
                } else {

                    ErrorAlert(data.message);
                    return;

                }
            },
            error: function (data) {
                ErrorAlert('erro');
            }
        });
    }
    else {
        return;
    }
}
//--------------thanhnam-----------------
function getDataOutBox(pdata) {
    var params = new Object();

    if ($('#GeneralGrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    params.productCode = $('#productCode').val() == null ? "" : $('#productCode').val().trim();
    params.productName = $('#productName').val() == null ? "" : $('#productName').val().trim();
    params.bom_type = $('#s_bom_type').val() == null ? "" : $('#s_bom_type').val().trim();
    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();



    $('#GeneralGrid').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/FGWmsGeneral/getFGGeneral",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#GeneralGrid')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};


function getDataOutBox1(pdata) {

    var params = new Object();
    var rows = $("#GeneralGrid").getDataIDs();

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
    params.productName = $('#productName').val() == null ? "" : $('#productName').val().trim();
    params.recevice_dt_start = $('#recevice_dt_start').val() == null ? "" : $('#recevice_dt_start').val().trim();
    params.recevice_dt_end = $('#recevice_dt_end').val() == null ? "" : $('#recevice_dt_end').val().trim();
    $("#GeneralGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/FGWmsGeneral/getFGGeneral",

        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#GeneralGrid")[0];
                showing.addJSONData(data);

            }
        },
        error: function () {

            return;
        }
    });
};

function getDataDT(pdata) {

    //$('.loading-gif-2').show();
    var params = new Object();

    var rows = $("#" + childGridID).getDataIDs();
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.id = wmtid_DT;
    params.buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    //params.productCode = $(`#productCode`).val() == null ? '' : $(`#productCode`).val();
    params.poCode = $(`#poCode`).val() == null ? '' : $(`#poCode`).val();
    params.recevice_dt_start = $(`#recevice_dt_start`).val() == null ? '' : $(`#recevice_dt_start`).val();
    params.recevice_dt_end = $(`#recevice_dt_end`).val() == null ? '' : $(`#recevice_dt_end`).val();

    $("#" + childGridID).jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/FGWmsGeneral/getFGgeneralDetail",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#" + childGridID)[0];
                showing.addJSONData(data);
                //$('.loading-gif-2').hide();
            }
            else {
                return;
            }
        },
        error: function () {
            //$('.loading-gif-2').hide();
            return;
        }
    });
};

$('#excelBtn').click(function () {
    //$('#loading').show();
    //--- export to excel on server side
    debugger
    var productCode = $(`#productCode`).val() == null ? `` : $(`#productCode`).val().trim();
    var poCode = $(`#poCode`).val() == null ? '' : $(`#poCode`).val();
    var recevice_dt_start = $(`#recevice_dt_start`).val() == null ? '' : $(`#recevice_dt_start`).val();
    var recevice_dt_end = $(`#recevice_dt_end`).val() == null ? '' : $(`#recevice_dt_end`).val();

    $(`#exportData`).attr(`action`, `ExportFGgeneralToExcel?productCode=${productCode}&poCode=${poCode}&recevice_dt_start=${recevice_dt_start}&recevice_dt_end=${recevice_dt_end}`);
    //$('#loading').hide();
});

$("#printBtn").click(function () {
    var array = [];
    var array2 = [];
    var i, selRowIds = $("#GeneralGrid").jqGrid("getGridParam", "selarrrow"), n, rowData;
    var model = [];
    var row_subGrid = [];

    if (selRowIds.length > 0) {
        for (i = 0; i < selRowIds.length; i++) {
            var ID_LV1 = selRowIds[i];

            var selRowIds_LV2 = $("#GeneralGrid_" + ID_LV1 + "_table").jqGrid("getGridParam", "selarrrow");
            if (selRowIds_LV2 != null) {
                for (j = 0; j < selRowIds_LV2.length; j++) {
                    var ret = $("#GeneralGrid_" + ID_LV1 + "_table").jqGrid('getRowData', selRowIds_LV2[j]);
                    wmtid = ret.wmtid
                    row_subGrid.push(wmtid);
                } //for(j)
            }
            else {
                continue;
            }
        }//for(i)

        if (row_subGrid.length > 0) {
            var mapForm = document.createElement("form");
            mapForm.target = "Map";
            mapForm.type = "hidden";
            mapForm.method = "POST"; // or "post" if appropriate
            mapForm.action = "/fgwms/PrintFGGeneral";

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
    }
    else {
        ErrorAlert("Please Select Grid");
    }
});

$("#searchBtn").click(function () {
    $('#GeneralGrid').clearGridData();
    $('#GeneralGrid').jqGrid('setGridParam', { search: true });
    var pdata = $('#GeneralGrid').jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);


});

$('#exportDataExcel').click(function () {
    debugger;
    var productCode = $(`#productCode`).val() == null ? `` : $(`#productCode`).val().trim();
    var buyerCode = $('#buyer_cd').val() == null ? "" : $('#buyer_cd').val().trim();
    var recevice_dt_start = $(`#recevice_dt_start`).val() == null ? '' : $(`#recevice_dt_start`).val();
    var recevice_dt_end = $(`#recevice_dt_end`).val() == null ? '' : $(`#recevice_dt_end`).val();
    $('#exportData').attr('action', "/FGWmsGeneral/ExportFGgeneralToExcel?productCode=" + productCode + "&buyerCode=" + buyerCode + "&recevice_dt_start=" + recevice_dt_start + "&recevice_dt_end=" + recevice_dt_end);
});




function _DateTime(cellvalue) {
    if (cellvalue == null)
        return "";
    var html = "";
    html += cellvalue.substr(0, 4) + "-" + cellvalue.substr(4, 2) + "-" + cellvalue.substr(6, 2) + " " + cellvalue.substr(8, 2) + ":" + cellvalue.substr(10, 2) + ":" + cellvalue.substr(12, 2);
    return html;

}
function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";

    cellvalue = cellvalue.substr(0, 8);
    var reg = /(\d{4})(\d{2})(\d{2})/;
    if (reg.test(cellvalue))
        return cellvalue.replace(reg, "$1-$2-$3");
    else {
        reg = /(\d{4})(\d{2})\-(\d{2})/;
        if (reg.test(cellvalue))
            return cellvalue.replace(reg, "$1-$2-$3");
        else {
            reg = /(\d{4})\-(\d{2})(\d{2})/;
            if (reg.test(cellvalue))
                return cellvalue.replace(reg, "$1-$2-$3");
            else {
                reg = /(\d{4})\-(\d{2})\-(\d{2})/;
                if (reg.test(cellvalue))
                    return cellvalue.replace(reg, "$1-$2-$3");
                else {
                    reg = /(\d{4})(\d{2}).(\d{2})/;
                    if (reg.test(cellvalue))
                        return cellvalue.replace(reg, "$1-$2-$3");
                    else {
                        reg = /(\d{4}).(\d{2}).(\d{2})/;
                        if (reg.test(cellvalue))
                            return cellvalue.replace(reg, "$1-$2-$3");

                        else {
                            reg = /(\d{4})(\d{2})\\(\d{2})/;
                            if (reg.test(cellvalue))
                                return cellvalue.replace(reg, "$1-$2-$3");
                            else {
                                reg = /(\d{4})\\(\d{2})\\(\d{2})/;
                                if (reg.test(cellvalue))
                                    return cellvalue.replace(reg, "$1-$2-$3");
                                else {
                                    reg = /(\d{4})\\(\d{2})\.(\d{2})/;
                                    if (reg.test(cellvalue))
                                        return cellvalue.replace(reg, "$1-$2-$3");
                                    else {
                                        reg = /(\d{4})\.(\d{2})\\(\d{2})/;
                                        if (reg.test(cellvalue))
                                            return cellvalue.replace(reg, "$1-$2-$3");
                                        else
                                            return cellvalue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

function qty_unit(cellvalue, options, rowObject) {
    return checknull(cellvalue) + ' ' + checknull(rowObject.bundle_unit);
}

function checknull(value) {
    if (value == null || value == '' || value == 'undefined' || value.length == 0) {
        return "";
    }
    return value
}

$("#buyer_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var buyer_cd = e.target.value.trim();


        $.ajax({
            url: "/fgwms/Search_BuyergetFGGeneral?buyerCode=" + buyer_cd,
            type: "get",
            dataType: "json",


            success: function (response) {
                if (response.result) {

                    $(".thongbao").addClass("active");
                    $(".thongbao").removeClass("hidden");

                    document.getElementById("noidung").innerHTML = (response.message);
                }
                else {
                    $(".thongbao").addClass("active");
                    $(".thongbao").removeClass("hidden");
                    document.getElementById("noidung").innerHTML = (response.message);
                }
            },
            error: function () {

                $(".thongbao").addClass("active");
                $(".thongbao").removeClass("hidden");
                document.getElementById("noidung").innerHTML = (response.message);

            }
        });


    }
});

$("#tabClose").on("click", function (event) {

    $(".thongbao").addClass("hidden");
    $(".thongbao").removeClass("active");
});


InsertTemGeneral = [];
json_object = "";
$(document).ready(function () {
    function changeDate(exdate) {
        var e0date = new Date(0);
        var offset = e0date.getTimezoneOffset();
        return new Date(0, 0, exdate - 1, 0, -offset, 0);
    };
    $("#imgupload").change(function (evt) {
        var selectedFile = evt.target.files[0];
        var reader = new FileReader();
        var excelData = [];
        reader.onload = function (event) {
            var data = event.target.result;
            var workbook = XLSX.read(data, { type: 'binary' });
            workbook.SheetNames.forEach(function (sheetName) {
                var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
                json_object = XL_row_object;
            })
        };

        reader.onerror = function (event) {
            console.error("File could not be read! Code " + event.target.error.code);
        };
        reader.readAsBinaryString(selectedFile);
    });
});

$("#uploadBtn").click(function () {
    if ($("#imgupload").val() == "") { return false; }
    $('#loading').show();
    var data_create = 0;
    var data_update = 0;
    var data_error = 0;
    var data_pagedata = -1;

    for (var i = 0; i < json_object.length; i++) {
        item = {
            //Code: json_object[i]["Item No."].trim(),
            //Model: json_object[i]["Item Description"].trim(),
            BuyerCode: json_object[i]["CODE"].trim().toUpperCase(),
            Quantity: json_object[i]["Quantity"],
            //LotNo: json_object[i]["Lot Number"].trim(),

        }
        InsertTemGeneral.push(item);
    }
    $.ajax({
        url: "/FGWmsGeneral/InsertTemGeneral",
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(InsertTemGeneral),
        traditonal: true,
        success: function (response) {
            if (response.result) {
                data_create = data_create + response.data_create;
                data_update = data_update + response.data_update;
                data_error = data_error + response.data_error;
                data_pagedata = data_pagedata + 1;
                $("#result").html("<label class='text-success'>&nbsp;&nbsp;Create: " + data_create + "</label>" + "<label class='text-primary'>&nbsp;&nbsp;Duplicate: " + data_update + "</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data_error + "</label>");
                $("#GeneralGrid").clearGridData();

                var grid = $("#GeneralGrid");
                grid.jqGrid('setGridParam', { search: true });
                var postData = grid.jqGrid('getGridParam', 'postData');
                var params = new Object();
                params.page = 1;
                params.rows = postData.rows;
                params.sidx = postData.sidx;
                params.sord = postData.sord;
                params._search = postData._search;

                $.ajax({
                    url: "/FGWmsGeneral/getFGGeneral",
                    type: "Get",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    traditional: true,
                    data: params,
                    success: function (data, st) {
                        if (st == "success") {
                            var grid = $("#GeneralGrid")[0];
                            grid.addJSONData(data);

                            $('#loading').hide();
                            $(`#imgupload`).val(``);
                            InsertTemGeneral = [];
                            json_object = "";
                            return false;
                        }
                    }
                });

            }
            else {
                $('#loading').hide();
                ErrorAlert(response.message);
                $(`#imgupload`).val(``);
                InsertTemGeneral = [];
                json_object = "";
                return false;
            }
        },
        Error: function () {
            $('#loading').hide();
        }
    });
});


//SCAN BUYER FUNCTION
function removeAscent(str) {
    if (str === null || str === undefined) return str;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
}


function isValid(string) {
    var re = /^[a-zA-Z!@#\$%\^\&*\)\(+=._-]{2,}$/g // regex here
    return re.test(removeAscent(string))
}

$("#scBuyerCode").on("keydown", function (e) {
    var buyer = $("#scBuyerCode").val().trim();

    if (e.keyCode === 13) {
        $.blockUI({ message: '' });
        //kiem tra tem gói có rỗng ko

        if (buyer == "" || buyer == null || buyer == undefined) {
            $("#scBuyerCode").val("");
            document.getElementById("scBuyerCode").focus();
            $.unblockUI();
            return false;
        };
        $.ajax({
            url: "/FGWmsGeneral/CheckStatusBuyerToSAP",
            type: "get",
            dataType: "json",
            data: {
                buyerCode: buyer
            },

            success: function (response) {
                if (response.result) {

                    SuccessAlert(response.message);
                    $("#" + childGridID).jqGrid('addRowData', response.data.id, response.data, 'first');
                    $("#" + childGridID).setRowData(response.data.id, false, { background: "#28a745", color: "#fff" });

                    $("#scBuyerCode").val("");
                    document.getElementById("scBuyerCode").focus();

                    $.unblockUI();
                    return false;
                }
                else {
                    $("#scBuyerCode").val("");
                    document.getElementById("scBuyerCode").focus();

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
                    return false;
                }
            }
        });
    }
});

$("#change_quantity").click(function () {
    var xacnhan = confirm("Bạn có muốn sửa lại số lượng của mã tem này không?");
    if (xacnhan) {

        $.blockUI({ message: '<h3>Loading...</h3>' });
        var i, rowIds = $("#" + childGridID).jqGrid("getGridParam", "selarrrow"), n, rowData;

        if (rowIds == undefined|| rowIds.length == 0) {
            alert("Vui lòng chọn cuộn bất kì để tiếp tục");
            $.unblockUI();
            return false;
        }
        var grQty = $("#quantity").val().trim();
        if (grQty == undefined || grQty == "") {
            alert("Vui lòng nhập số lượng");
            $.unblockUI();
            return false;
        }

        object = {};
        object['listId'] = rowIds;
        object['gr_qty'] = grQty;
        $.ajax({
            url: `/FGWmsGeneral/updateQuantityBuyer`,
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(object),
            traditonal: true,
            success:
                function (data) {
                    $.unblockUI();
                    if (data.result) {
                        SuccessAlert(data.message);
                        $.each(rowIds, function (key, item) {

                            var rowData = $("#" + childGridID).jqGrid('getRowData', item);
                            rowData.qty = grQty;
                            $("#" + childGridID).setRowData(item, rowData, { background: "#d0e9c6" });

                        });
                    }
                    else
                    {
                        $.unblockUI();
                        alert(data.message);
                     }
                },
            error: function () {
                $.unblockUI();
                alert('Error');
            }
        });
    };
});

$("#change_LotNo").click(function () {
    var xacnhan = confirm("Bạn có muốn sửa lại lot no của mã tem này không?");
    if (xacnhan) {

        $.blockUI({ message: '<h3>Loading...</h3>' });
        var i, rowIds = $("#" + childGridID).jqGrid("getGridParam", "selarrrow"), n, rowData;

        if (rowIds === undefined || rowIds.length == 0 )
        {
            alert("Vui lòng chọn cuộn bất kì để tiếp tục");
            $.unblockUI();
            return false;
        }
        var lot_no = $("#change_lotno").val().trim();
        if (lot_no == undefined || lot_no == "") {
            alert("Vui lòng nhập lot no");
            $.unblockUI();
            return false;
        }

        object = {};
        object['listId'] = rowIds;
        object['lot_no'] = lot_no;
        $.ajax({
            url: `/FGWmsGeneral/updateLotNoBuyer`,
            type: 'POST',
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(object),
            traditonal: true,
            success:
                function (data) {
                    $.unblockUI();
                    if (data.result) {
                        SuccessAlert(data.message);
                        $.each(rowIds, function (key, item)
                        {
                            $.each(data.data1, function (key, item1)
                            {
                                if (item1.id == item)
                                {
                                    var rowData = $("#" + childGridID).jqGrid('getRowData', item);
                                    rowData.lot_no = lot_no;
                                    rowData.qty = item1.qty;
                                    $("#" + childGridID).setRowData(item, rowData, { background: "#d0e9c6" });
                                }
                                else
                                {
                                  //  continue;
                                }
                            });
                            //var rowData = $("#" + childGridID).jqGrid('getRowData', item);
                            //rowData.lot_no = lot_no;
                            //$("#" + childGridID).setRowData(item, rowData, { background: "#d0e9c6" });
                        });
                    }
                    else {
                        $.unblockUI();
                        alert(data.message);
                    }
                },
            error: function () {
                $.unblockUI();
                alert('Error');
            }
        });
    };
})