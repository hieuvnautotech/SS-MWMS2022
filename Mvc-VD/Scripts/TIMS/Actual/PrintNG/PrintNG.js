var grid = $('#list');

//GET DATA (pdata)
function getData(pdata)
{
    var params = new Object();

    if (grid.jqGrid('getGridParam', 'reccount') == 0)
    {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.code = $('#sMaterialCode').val().trim();
    params.product = $('#sproduct').val().trim();
    params.at_no = $('#sat_no').val().trim();

    grid.jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/GetTIMSMaterialNG`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st)
        {
            if (st == "success")
            {
                var showing = grid[0];
                showing.addJSONData(data);
            }
        },
        error: function ()
        {
            return;
        }
    });
};

//GRID
function Grid()
{
    grid.jqGrid({
        mtype: 'GET',
        datatype: function (postData) { getData(postData); },
        colModel: [
            { name: 'Id', key: true, width: 50, align: 'center', hidden: true },
            { label: 'Detail', align: 'center', formatter: Detail, width: 50, },
            //{ label: 'Check QC', align: 'center', formatter: CheckQC, width: 80, },
            { label: 'Buyer QR', name: 'buyer_qr', width: 300},
            { name: 'Qty', sortable: false, width: 100, align: 'right' ,formatter:'integer'},
            { label: 'Số lượng chưa phân loại', name: 'SLCK', sortable: false, width: 200, align: 'right', formatter: 'integer' },
            { label: 'Composite Code', name: 'WMaterialCode', sortable: true, width: 400, align: 'left' },
            { label: 'PO NO', name: 'at_no', sortable: true, width: 130, align: 'center' },
            { label: 'Product', name: 'product', sortable: true, width: 180, align: 'left' },
        ],
        pager: '#listpage',
        height: 300,
        width: null,
        rowNum: 50,
        rowList: [5, 10, 20, 40, 100],
        loadtext: "Loading...",
        emptyrecords: "No data.",
        multiboxonly: true,
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: true,
        multiselect: true,
        autoResizing: true,
        viewrecords: true,
        caption: 'Material NG',
        //loadui: 'disable',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        multiselect: true,
        //multiboxonly: true,
        gridComplete: function ()
        {
            var rows = grid.getDataIDs();
        },
        loadComplete: function ()
        {
            var rows = grid.getDataIDs();
        },
        onSelectRow: function (rowid, status, e, iRow, iCol)
        {
            var selectedRowId = grid.jqGrid("getGridParam", 'selrow');
            var rowId = grid.getRowData(selectedRowId);
        }
    });
}

$("#OK").jqGrid({
    datatype: function (postData) { getData_OK(postData); },
    mtype: 'Post',
    colModel: [
        { name: 'wmtid', key: true, width: 50, align: 'center', hidden: true },
        { label: 'Composite Code', name: 'material_code', sortable: true, width: 400, align: 'left' },
        { label: 'Qty', name: 'gr_qty', sortable: true, width: 100, align: 'right' },
        { label: 'Cancel', name: 'Cancel', width: 100, align: 'center', formatter: Cancel },
    ],
    pager: '#OKpage',
    height: 300,
    width: null,
    rowNum: 50,
    rowList: [5, 10, 20, 40, 100],
    loadtext: "Loading...",
    emptyrecords: "No data.",
    multiboxonly: true,
    rownumbers: true,
    shrinkToFit: false,
    autowidth: false,
    loadonce: true,
    multiselect: true,
    autoResizing: true,
    viewrecords: true,
    caption: 'Composite NG=>OK',
    //loadui: 'disable',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    multiselect: true,
    //multiboxonly: true,
    gridComplete: function () {
        var rows = grid.getDataIDs();
    },
    loadComplete: function () {
        var rows = grid.getDataIDs();
    },
    onSelectRow: function (rowid, status, e, iRow, iCol) {
        var selectedRowId = grid.jqGrid("getGridParam", 'selrow');
        var rowId = grid.getRowData(selectedRowId);
    }
});

$("#searchBtn").click(function ()
{
    grid.clearGridData();
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getData(pdata);
});

function getData_OK(pdata) {
    var params = new Object();

    if ($("#OK").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    $("#OK").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/GetTIMSMaterialOK`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#OK")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

$("#printBtn").click(function ()
{
    var array = [];

    var selRowIds = grid.jqGrid("getGridParam", "selarrrow");
    var selRowIds2 = $("#OK").jqGrid("getGridParam", "selarrrow");

    if (selRowIds.length > 0 || selRowIds2.length>0)
    {
        for (i = 0; i < selRowIds.length; i++)
        {
            var rowData = grid.jqGrid('getRowData', selRowIds[i]);
            var id = rowData.Id;
            array.push(id);
        };
        for (i = 0; i < selRowIds2.length; i++) {
            var rowData1 = $("#OK").jqGrid('getRowData', selRowIds2[i]);
            var id = rowData1.wmtid;
            array.push(id);
        }; 

        var mapForm = document.createElement("form");
        mapForm.target = "Map";
        mapForm.type = "hidden";
        mapForm.method = "POST";
        mapForm.action = "/TIMS/PrintMaterialNG";

        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = "qrcode";
        mapInput.value = array;

        mapForm.appendChild(mapInput);

        document.body.appendChild(mapForm);

        map = window.open(" ", "Map", "width=700, height=800, left=40%, top=40%, location=no, status=no,");

        if (map)
        {
            mapForm.submit();
        } else
        {
            ErrorAlert("Allow script for this map to work.'");
        }
    }

    else
    {
        ErrorAlert("Please Select Grid");
    }
});

//DOCUMENT READY
$(function ()
{
    Grid();
     /*$('.dialog_qc_code_pp').dialog('open');*/
});

$("#Scan").click(function () {
    var gr_qty = $("#gr_qty").val().trim();
    if (gr_qty == "" || gr_qty=="0") {
        alert("PLease Enter Quantity");
        return;
    }
    var mt_cd = $("#mt_cd").val().trim();
    var buyer_qr = $("#buyer_qr").val().trim();
    if (mt_cd == "" && buyer_qr=="") {
        alert("PLease Enter Composite Code Or Buyer QR");
        return;
    }
    else {
        $.ajax({
            url: "/TIMS/Change_OK_NG",
            type: "Get",
            dataType: "json",
            data: {
                gr_qty: gr_qty,
                mt_cd: mt_cd,
                buyer_qr: buyer_qr,
                reason:  $("#reason").val().trim(),
            },
            success: function (data) {
                console.log()
                if (data.result == true) {
                    SuccessAlert(data.message);
                    grid.clearGridData();
                    grid.jqGrid('setGridParam', { search: true });
                    var pdata = grid.jqGrid('getGridParam', 'postData');
                    getData(pdata);
                    var id = data.kq.wmtid;
                    $("#OK").jqGrid('addRowData', id, data.kq, 'first');
                    $("#OK").setRowData(id, false, { background: "#d0e9c6" });
                } else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
});

function Cancel(cellValue, options, rowdata, action) {
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-danger button-srh"  data-mt_cd="' + rowdata.material_code + '" data-id="' + id + '"onclick="CancelCon(this);">Cancel</button>';
    return html;
}

function CancelCon(e) {
    $.ajax({
        url: '/TIMS/Cancel_NG_OK?mt_cd=' + $(e).data("mt_cd"),
        type: "Get",
        dataType: "json",
        success: function (data) {
            if (data.result == true) {
                grid.clearGridData();
                grid.jqGrid('setGridParam', { search: true });
                var pdata = grid.jqGrid('getGridParam', 'postData');
                getData(pdata);
                var id = $(e).data("id");
                $("#OK").jqGrid('delRowData', id);
                SuccessAlert(data.message);
            } else {
                ErrorAlert(data.message)
            }
        },
    });
}

$(".dialog1").dialog({
    width: '70%',
    height: "auto",
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
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

    },
    close: function (event, ui) {
        $('#popupbom').empty();
    },
});

$('#closebom1').click(function () {
    $('.dialog1').dialog('close');
    $('#popupbom').empty();
});

function Detail(cellValue, options, rowdata, action) {
    html = '<button  class="btn btn-sm btn-info button-srh" data-mt_cd="' + rowdata.WMaterialCode + '"onclick="detail_con(this);">Detail</button>';
    return html;
}
 
function detail_con(e) {
    $('.dialog1').dialog('open');
    var table = $('#popupbom').DataTable({
        "processing": true,
        "ajax": {
            "url": "/TIMS/GetReasonDown?mt_cd=" + $(e).data("mt_cd"),
            "type": "Get",
            "datatype": "json"
        },

        "searching": false,
        "paging": false,
        "bInfo": false,
        "lengthMenu": [50, 100, 200, 500, 1000],
        "columns": [
            {
                'className': 'details-control',
                'orderable': false,
                'data': null,
                'defaultContent': ''
            },
            { "data": "mt_cd" },
            { "data": "gr_qty" },
            { "data": "gr_down" },
            { "data": "reason" },
            { "data": "reg_id" },
            { "data": "reg_dt" },
        ],
        'columnDefs': [
            {
                "searchable": false,
                "orderable": false,
                "targets": 0, // your case first column
                "className": "text-center",
            },
            {
                "targets": 3, // your case first column
                "className": "text-right",
            },
            {
                "targets": 4, // your case first column
                "className": "text-right",
            },
        ],
        "order": [[2, 'asc']],
        //'rowsGroup': [1],
        'rowsGroup': true,
        "bDestroy": true,
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

}

/*-----------------------Check qc---------------------------------------------------------------------*/
function CheckQC(cellValue, options, rowdata, action) {

    if (rowdata.buyer_qr == null || rowdata.buyer_qr == "") {
        html = '<button  class="btn btn-sm btn-warning button-srh"  data-at_no="' + rowdata.at_no + '" data-id="' + rowdata.Id + '"onclick="Click_checkQC(this);">Check QC</button>';
        return html;
    }
    else {
        return "";
    }   
}

function Click_checkQC(e) {

    $('.dialog_qc_code_pp').dialog('open');
    var id = $(e).data("id")
    var rowData = grid.jqGrid('getRowData', id);
    //gan gia tri vao cac input trong popup
    $("#qc_po").val($(e).data("at_no"));

    $("#tong_qc").val(rowData.Qty);
    $("#qc_mt_cd").val(rowData.WMaterialCode);
    $("#qc_gr_qty").val(rowData.SLCK);
    var max_qty_phanloai = rowData.SLCK;
    document.getElementById("qc_gr_qty").max = max_qty_phanloai;

    $("#qc_item_vcd").val("TI000001A");

    var item_vcd = "TI000001A";

    let today = new Date().toISOString().slice(0, 10)
    $("#qc_date").val(today);

    var mt_cd = "";
  
    table_facline_qc(item_vcd,rowData.WMaterialCode);
    table_facline_qc_value();
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
        $('.dialog_qc_code_pp').dialog('close');
        //document.getElementById("qc_ok_qty").value = "0";
        document.getElementById("qc_gr_qty").value = "0";
        //document.getElementById("qc_ng_qty").value = "0";
        $('input:checkbox').removeAttr('checked');
        a = 0;
        $("#list_qc").trigger("reloadGrid");
        $("#list_qc_value").clearGridData();

        jQuery("#list_facline_qc").trigger("reloadGrid");
        $("#list_facline_qc_value").clearGridData();

        $("#p_SaveQcfacline").attr("disabled", true);
    },
});

function getViewItemQC() {
    $.get("/ActualWO/PartialView_Phan_loai_QC?item_vcd=TI000001A", function (html) {
        $("#view_qc").html(html);
    });
}

//start table m_facline_qc

function table_facline_qc(item_vcd, ml_tims) {
    $("#list_facline_qc").setGridParam({ url: "/TIMS/Getfacline_qc_PhanLoai?item_vcd=" + item_vcd + "&mt_lot=" + ml_tims, datatype: "json" }).trigger("reloadGrid");
    $("#list_facline_qc").jqGrid
        ({
            url: "/TIMS/Getfacline_qc_PhanLoai?mt_lot=" + ml_tims + "&item_vcd=" + item_vcd,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqno', name: 'Id', width: 80, align: 'center', hidden: true },
                { key: false, label: 'FQ NO', name: 'fq_no', sortable: true, width: '120', align: 'center' },
                { key: false, label: 'Composite Code', name: 'ml_tims', sortable: true, width: '300', align: 'center' },
                { key: false, label: 'Total Defect Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
                //{ key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
                //{ key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
                row_id = $("#list_facline_qc").getRowData(selectedRowId);
                $("#list_facline_qc_value").clearGridData();
                $("#list_facline_qc_value").setGridParam({ url: "/TIMS/Getfacline_qc_value?" + "fq_no=" + row_id.fq_no, datatype: "json" }).trigger("reloadGrid");
            },
            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: "100%",
            width: null,
            rowNum: 50,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: false,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            viewrecords: true,
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
}
function table_facline_qc_value() {
    $("#list_facline_qc_value").jqGrid
        ({
            url: "/TIMS/Getfacline_qc_value",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqhno', name: 'Id', width: 80, align: 'center', hidden: true },
                { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
                { key: false, label: 'Content', name: 'check_value', sortable: true, width: '110', align: 'left' },
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
                { key: false, label: 'Date', name: 'date_ymd', sortable: true, width: '90', align: 'center' },
                { label: 'Delete', name: 'delete', width: 50, formatter: FormatDelQc_value, align: 'center', },
            ],

            onCellSelect: editRow_facline,
            onSelectRow: function (rowid, selected, status, e) {
                $("#p_SaveQcfacline").attr("disabled", false);
            },

            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: "100%",
            width: null,
            rowNum: 50,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: false,
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
        })
}
var lastSel;
function editRow_facline(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
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
        'ml_tims': $('#qc_mt_cd').val(),
        'date_ymd': $('#qc_date').val(),
        'gr_qty': $('#qc_gr_qty').val(),
        'item_vcd': $('#qc_item_vcd').val(),
        'at_no': $('#qc_po').val(),

    });

    $.ajax({
        type: 'POST',
        url: "/TIMS/InsertQcPhanLoai",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: models,
        cache: false,
        processData: false,
        success: function (data) {
            if (data.result) {

                SuccessAlert("Success");
                document.getElementById("qc_gr_qty").value = "0";


                grid.setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                jQuery("#list_facline_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_facline_qc_value").clearGridData();
                $(".testnumber").val("0");
                var max_check_qty = document.getElementById("qc_gr_qty").max;
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });

    grid.clearGridData();
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getData(pdata);

});

//delete Qc value 
function FormatDelQc_value(cellValue, options, rowdata, action) {
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-danger button-srh"  data-id="' + rowdata.fqhno + '"onclick="Click_DelQC_value(this);">X</button>';
    return html;
}

function Click_DelQC_value(e) {
    var id = $(e).data("id");

    $.ajax({
        type: 'POST',
        url: "/ActualWO/Delete_Qc_valuePhanLoai?id=" + id,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',

        cache: false,
        processData: false,
        success: function (response) {
            if (response.result) {

                SuccessAlert("Success");
                $("#list_facline_qc_value").jqGrid('delRowData', id);

                $('#list_facline_qc').jqGrid('setRowData', response.data.fqno, response.data);
                $("#list_facline_qc").setRowData(response.data.fqno, false, { background: "#d0e9c6" });

                var Qty = $("#tong_qc").val();
                var tongso_da_ktra = response.data.check_qty;
                var soluongcolai = parseInt(Qty) - parseInt(tongso_da_ktra);
                document.getElementById("qc_gr_qty").value = soluongcolai;

                var pdata = grid.jqGrid('getGridParam', 'postData');
                getData(pdata);
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });
}
//delete Qc value
/*-----------------------Check qc---------------------------------------------------------------------*/