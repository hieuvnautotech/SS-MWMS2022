var day_now;
var day1;
var day2;
var day3;
var day3;
var day5;
var day6;

var p_stock_day1;
var p_stock_day2;
var p_stock_day3;
var p_stock_day4;
var p_stock_day5;
var p_stock_day6;

day_now = getdate_now();
day1 = adddate(day_now, -1);
day2 = adddate(day_now, -2);
day3 = adddate(day_now, -3);
day4 = adddate(day_now, -4);
day5 = adddate(day_now, -5);
day6 = adddate(day_now, -6);

function getdate_now() {
    var newdate = new Date();
    var dd = newdate.getDate();
    if (dd.toString().length == 1) dd = "0" + dd;
    var mm = newdate.getMonth() + 1;
    if (mm.toString().length == 1) mm = "0" + mm;
    var y = newdate.getFullYear();
    //'yy-mm-dd',
    //var someFormattedDate = mm + '/' + dd + '/' + y;
    var someFormattedDate = y + '-' + mm + '-' + dd;
    return someFormattedDate;
}

function adddate(i_date, i_days) {
    // var tt = document.getElementById('txtDate').value;

    var date = new Date(i_date);
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + i_days);

    var dd = newdate.getDate();
    if (dd.toString().length == 1) dd = "0" + dd;
    var mm = newdate.getMonth() + 1;
    if (mm.toString().length == 1) mm = "0" + mm;
    var y = newdate.getFullYear();
    //'yy-mm-dd',
    //var someFormattedDate = mm + '/' + dd + '/' + y;
    var someFormattedDate = y + '-' + mm + '-' + dd;
    return someFormattedDate;
}

$("#list").jqGrid
    ({
        url: "/QCInformation/GetQMSNG",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'stt', name: 'stt', width: 80, align: 'center', hidden: true },
            { key: false, label: 'ProductCode', name: 'ProductCode', sortable: true, width: '90', align: 'left' },
            { key: false, label: 'PO', name: 'at_no', sortable: true, width: '110', align: 'left' },
            { key: false, label: 'Shift', name: 'Shift', sortable: true, width: '40', align: 'left' },
            { key: false, label: 'Total', name: 'Total', sortable: true, width: '80', align: 'left', formatter: 'integer' },
            { key: false, label: 'OK', name: 'OK', sortable: true, width: '80', align: 'left', formatter: 'integer' },
            { key: false, label: 'NG', name: 'NG', sortable: true, width: '80', align: 'left', formatter: 'integer' },
            { key: false, label: 'UnCheck', name: 'chuaphanloai', sortable: true, width: '70', align: 'left', formatter: 'integer' },

            {
                label: 'Create Date', name: 'CreateOn', align: 'center', width: '80', formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" }
            },
            { label: 'Check QC', align: 'center', formatter: CheckQC, width: 60, },
        ],

        onSelectRow: function (rowid, selected, status, e) {
            ;
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $("#list_facline_qc_value").clearGridData();
            $("#list_facline_qc_value").setGridParam({ url: "/QCinformation/GetfaclineQCValue?ProductCode=" + row_id.ProductCode + "&datetime=" + row_id.CreateOn + "&Shift=" + row_id.Shift, datatype: "json" }).trigger("reloadGrid");
        },
        footerrow: true,
        userDataOnFooter: true,
        pager: "#list_gridpager",
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        autowidth: true,
        width: null,
        rowNum: 50,
        caption: 'TQC ',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: false,
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

            var ids = $("#list").jqGrid('getDataIDs');
            var sum_Quantity_total = 0;
            var sum_Quantity_ok = 0;
            var sum_Quantity_ng = 0;
            var sum_Quantity_uncheck = 0;
            var $self = $(this)
            for (var i = 0; i < ids.length; i++) {

                sum_Quantity_total += parseInt($("#list").getCell(ids[i], "Total"));
                sum_Quantity_ok += parseInt($("#list").getCell(ids[i], "OK"));
                sum_Quantity_ng += parseInt($("#list").getCell(ids[i], "NG"));
                sum_Quantity_uncheck += parseInt($("#list").getCell(ids[i], "chuaphanloai"));
            }
            $self.jqGrid("footerData", "set", { Shift: "Total" });
            $self.jqGrid("footerData", "set", { Total: sum_Quantity_total });
            $self.jqGrid("footerData", "set", { OK: sum_Quantity_ok });
            $self.jqGrid("footerData", "set", { NG: sum_Quantity_ng });
            $self.jqGrid("footerData", "set", { chuaphanloai: sum_Quantity_uncheck });
        },
    })


$('#start_date_ymd').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
$('#end_date_ymd').datepicker({
    dateFormat: 'yy-mm-dd',
    "autoclose": true
});
//-------SEARCH------------//
$("#searchBtn").click(function () {
    var productCode = $('#productCode').val().trim();
    var start_date_ymd = $('#start_date_ymd').val().trim();
    var end_date_ymd = $('#end_date_ymd').val().trim();
    var at_no = $('#at_no').val().trim();
    $.ajax({
        url: "/QCInformation/GetQMSNG?at_no=" + at_no + "&productCode=" + productCode + "&start_date_ymd=" + start_date_ymd + "&end_date_ymd=" + end_date_ymd,
        type: "get",
        dataType: "json",
        success: function (result) {

            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });
});

$("#list_facline_qc_value").jqGrid
    ({
        //url: "/TIMS/Getfacline_qc_value",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Content', name: 'check_value', sortable: true, width: '150', align: 'left' },
            { key: false, label: 'Defect Qty', name: 'check_qty', sortable: true, width: '80', align: 'left' },

        ],


        footerrow: true,
        userDataOnFooter: true,
        pager: "#list_gridpager_value",
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        autowidth: true,
        width: null,
        rowNum: 50,
        caption: 'TQC Value',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: false,
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

            var ids = $("#list_facline_qc_value").jqGrid('getDataIDs');
            var sum_Quantity_total = 0;
            var $self = $(this)
            for (var i = 0; i < ids.length; i++) {

                sum_Quantity_total += parseInt($("#list_facline_qc_value").getCell(ids[i], "check_qty"));

            }
            $self.jqGrid("footerData", "set", { check_value: "Total" });
            $self.jqGrid("footerData", "set", { check_qty: sum_Quantity_total });
        },
    })
/*-----------------------Check qc---------------------------------------------------------------------*/
function CheckQC(cellValue, options, rowdata, action) {

    if (rowdata.buyer_qr == null || rowdata.buyer_qr == "") {
        html = '<button  class="btn btn-sm btn-warning button-srh" data-NG = "' + rowdata.chuaphanloai + '" data-CreateOn = "' + rowdata.CreateOn + '" data-Shift  = "' + rowdata.Shift + '" data-ProductCode = "' + rowdata.ProductCode + '" data-at_no="' + rowdata.at_no + '" data-id="' + rowdata.Id + '"onclick="Click_checkQC(this);">Check</button>';
        return html;
    }
    else {
        return "";
    }
}
var ml_tims = "";
function Click_checkQC(e) {

    $('.dialog_qc_code_pp').dialog('open');

    //gan gia tri vao cac input trong popup
    var at_no = $(e).data("at_no");
    var productcode = $(e).data("productcode");
    var shift = $(e).data("shift");
    var ng = $(e).data("ng");
    var qc_item_vcd = "TI000001A";
    var createon = $(e).data("createon");


    $("#qc_po").val(at_no);
    $("#product_code").val(productcode);
    $("#shift").val(shift);
    var ngQuantity = parseInt(ng);
    $("#qc_gr_qty").val(ngQuantity);
    document.getElementById("qc_gr_qty").max = ngQuantity;

    $("#qc_item_vcd").val(qc_item_vcd);
    $("#qc_date").val(createon);

    ml_tims = at_no + "-" + productcode + "-" + shift + "-TIMS-NG";

    table_facline_qc(at_no, productcode, createon, shift);

}
$(".dialog_qc_code_pp").dialog({
    width: '60%',
    height: 800,
    maxWidth: 800,
    maxHeight: 450,
    minWidth: '60%',
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
    open: function () {
        getViewItemQC();
    },

    close: function (event, ui) {
        $('.dialog_qc_code_pp').dialog('close');
        /*$('.dialog_qc_code_pp').dialog('close');*/
        //document.getElementById("qc_ok_qty").value = "0";
        document.getElementById("qc_gr_qty").value = "0";
        //document.getElementById("qc_ng_qty").value = "0";
        $('input:checkbox').removeAttr('checked');
        a = 0;
        $("#list_qc").trigger("reloadGrid");
        $("#list_qc_value").clearGridData();

        jQuery("#list_facline_qc").trigger("reloadGrid");


        $("#p_SaveQcfacline").attr("disabled", true);
    },
});
function getViewItemQC() {

    $.get("/QCInformation/PartialView_Phan_loai_QC?item_vcd=TI000001A", function (html) {
        $("#view_qc").html(html);
    });
}
$("#save_qc_code").on("click", function () {
    //kiem tra xem số lượng check bằng số lượng check_Qty chưa ,-- nếu chưa return false
    var view_qc = $("#view_qc");
    var bien = 0;
    var inputs = view_qc.find('input[type="number"]');
    for (var i = 0; i < inputs.length; i++) {
        var input = $(inputs[i]);
        if (input.val() != null && input.val() != '') {
            bien += parseInt(input.val());
        }
    }

    // var okqty = $('#qc_ok_qty').val();
    var check_qty = $('#qc_gr_qty').val();
    if (check_qty == 0) {
        $('#qc_gr_qty').focus();
        return false;
    }

    if (bien < check_qty) {
        ErrorAlert("Tổng số lượng kiểm tra thiếu, vui lòng kiểm tra lại!");
        return false;
    }

    var listTable = $("#view_qc > div");
    var model = [];

    for (var i = 0; i < listTable.length; i++) {
        var item = listTable[i];
        var icno = $(item).data("icno");

        var itemObj = {};
        itemObj['icno'] = icno;

        var itemTr = [];

        var tr = $(item).find("tbody tr");
        for (var j = 0; j < tr.length; j++) {
            var trItem = tr[j];
            var icdno = $(trItem).data("icdno");
            var input = $(trItem).find("input").val();

            var objTr = {
                icdno: icdno,
                input: input
            };
            itemTr.push(objTr);
        }

        itemObj['objTr'] = itemTr;

        model.push(itemObj);
    }

    var models = JSON.stringify({
        'model': model,
        'productcode': $('#product_code').val(),
        'shift': $('#shift').val(),
        'date_ymd': $('#qc_date').val(),
        'gr_qty': $('#qc_gr_qty').val(),
        'item_vcd': $('#qc_item_vcd').val(),
        'at_no': $('#qc_po').val(),
        'ml_tims': ml_tims
    });

    var r = confirm("Bấm OK để phân loại mã này");
    if (r) {
        $.ajax({
            type: 'POST',
            url: "/QCInformation/InsertQcPhanLoai",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: models,
            cache: false,
            processData: false,
            success: function (data) {
                if (data.result) {

                    SuccessAlert("Success");
                    document.getElementById("qc_gr_qty").value = "0";


                    $("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                    jQuery("#list_facline_qcvalue").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                    $(".testnumber").val("0");
                    var max_check_qty = document.getElementById("qc_gr_qty").max;
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });

        var productCode = $('#productCode').val().trim();
        var start_date_ymd = $('#start_date_ymd').val().trim();
        var end_date_ymd = $('#end_date_ymd').val().trim();
        var at_no = $('#at_no').val().trim();

        $("#list").setGridParam({ url: "/QCInformation/GetQMSNG?at_no=" + at_no + "&productCode=" + productCode + "&start_date_ymd=" + start_date_ymd + "&end_date_ymd=" + end_date_ymd, datatype: "json" }).trigger("reloadGrid");
        //grid.clearGridData();
        //grid.jqGrid('setGridParam', { search: true });
        //var pdata = grid.jqGrid('getGridParam', 'postData');
        //getData(pdata);
    }
    else {
        return false;
    }

});
//start table m_facline_qc
function table_facline_qc(at_no, productcode, createon, shift) {
    //$("#list_facline_qc").setGridParam({
    //    url: "/QCInformation/Getfacline_qc_PhanLoai?item_vcd=" + item_vcd + "&mt_lot=" + mt_lot
    //    , datatype: "json"
    //}).trigger("reloadGrid");
    //$("#list_facline_qc").jqGrid
    //    ({
    //        url: "/QCInformation/Getfacline_qc_PhanLoai?item_vcd=" + item_vcd + "&mt_lot=" + mt_lot,
    //        datatype: 'json',
    //        mtype: 'Get',
    //        colModel: [
    //            { key: true, label: 'fqno', name: 'fqno', width: 80, align: 'center', hidden: true },
    //            { key: false, label: 'FQ NO', name: 'fq_no', sortable: true, width: '100', align: 'center' },
    //            { key: false, label: 'Composite Code', name: 'ml_tims', sortable: true, width: '280', align: 'center' },
    //            { key: false, label: 'Total Defect Qty', name: 'check_qty', sortable: true, align: 'right', width: '90', formatter: 'integer' },
    //            //{ key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
    //            //{ key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
    //        ],
    //        formatter: {
    //            integer: { thousandsSeparator: ",", defaultValue: '0' },
    //            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
    //            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    //        },
    //        onSelectRow: function (rowid, selected, status, e) {
    //            var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
    //            row_id = $("#list_facline_qc").getRowData(selectedRowId);
    //            $("#list_facline_qcvalue").clearGridData();
    //            $("#list_facline_qcvalue").setGridParam({ url: "/QCInformation/Getfacline_qc_value1?" + "fq_no=" + row_id.fq_no, datatype: "json" }).trigger("reloadGrid");
    //        },
    //        viewrecords: true,
    //        rowList: [50, 100, 200, 500],
    //        height: "100%",
    //        width: null,
    //        rowNum: 50,
    //        loadtext: "Loading...",
    //        emptyrecords: "No data.",
    //        rownumbers: false,
    //        gridview: true,
    //        shrinkToFit: false,
    //        loadonce: false,
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
    $("#list_facline_qcvalue").setGridParam({ url: "/QCInformation/Getfacline_qc_value1?" + "at_no=" + at_no + "&productcode=" + productcode + "&date_ymd=" + createon + "&shift=" + shift, datatype: "json" }).trigger("reloadGrid");

    $("#list_facline_qcvalue").jqGrid
        ({
            url: "/QCInformation/Getfacline_qc_value1?" + "at_no=" + at_no + "&productcode=" + productcode + "&date_ymd=" + createon + "&shift=" + shift,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
                //{ key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '120', align: 'left' },
                { key: false, label: 'Content', name: 'check_value', sortable: true, width: '120', align: 'left' },
                {
                    key: false, label: 'Defect Qty', name: 'check_qty', sortable: true, align: 'right', width: '90', editable: false,
                    editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                    formatter: 'integer', editoptions: {
                        dataInit: function (element) {
                            $(element).keypress(function (e) {
                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                    return false;
                                }
                            });
                        }
                    },
                },
                { key: false, label: 'Date', name: 'date_ymd', sortable: true, width: '80', align: 'center' },
                { label: 'Delete', name: 'delete', width: 50, formatter: FormatDelQc_value, align: 'center', },
            ],
            //footerrow: true,
            //userDataOnFooter: true,

            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: "100%",
            width: null,
            caption: 'TQC Detail',
            rowNum: 50,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            viewrecords: true,
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
            //gridComplete: function () {
            //    prevCellVal = { cellId: undefined, value: undefined };

            //    var ids = $("#list_facline_qc_value").jqGrid('getDataIDs');
            //    var sum_Quantity_total = 0;
            //    var $self = $(this)
            //    for (var i = 0; i < ids.length; i++) {

            //        sum_Quantity_total += parseInt($("#list_facline_qc_value").getCell(ids[i], "check_qty"));

            //    }
            //    $self.jqGrid("footerData", "set", { check_value: "Total" });
            //    $self.jqGrid("footerData", "set", { check_qty: sum_Quantity_total });
            //},
        })

}

//delete Qc value 
function FormatDelQc_value(cellValue, options, rowdata, action) {
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-danger button-srh"  data-id="' + rowdata.fqhno + '"onclick="Click_DelQC_value(this);">X</button>';
    return html;
}
var lastSel;
function editRow_facline(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}

function Click_DelQC_value(e) {
    var id = $(e).data("id");
    var r = confirm("Bấm OK để xác nhận xóa");
    if (r) {
        $.ajax({
            type: 'Delete',
            url: "/QCInformation/Delete_Qc_valuePhanLoai?id=" + id,
            dataType: 'json',
            success: function (response) {
                if (response.result) {

                    SuccessAlert("Success");
                    $("#list_facline_qcvalue").jqGrid('delRowData', id);


                    var Qty = $("#tong_qc").val();
                    var tongso_da_ktra = response.data.check_qty;
                    var soluongcolai = parseInt(Qty) - parseInt(tongso_da_ktra);
                    document.getElementById("qc_gr_qty").value = soluongcolai;

                    var productCode = $('#productCode').val().trim();
                    var start_date_ymd = $('#start_date_ymd').val().trim();
                    var end_date_ymd = $('#end_date_ymd').val().trim();
                    var at_no = $('#at_no').val().trim();
                    $("#list").setGridParam({ url: "/QCInformation/GetQMSNG?at_no=" + at_no + "&productCode=" + productCode + "&start_date_ymd=" + start_date_ymd + "&end_date_ymd=" + end_date_ymd, datatype: "json" }).trigger("reloadGrid");
                }
                else {
                    ErrorAlert(response.message);
                }
            }
        });
    } else {
        return false;
    }

}
//delete Qc value