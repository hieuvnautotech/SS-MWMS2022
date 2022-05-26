$(function () {
    $(".diologbobin3").dialog({
        width: '50%',
        height: 550,
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
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });
});
$(function () {
    $(".PopupNG").dialog({
        width: '70%',
        height: 520,
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
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });
});
function getData_bboqc(pdata) {
    var params = new Object();

    if ($('#popupbobin3').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var bb_no = $('#s_bb_no3').val().trim();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.bb_no = bb_no;
    params.id_actual = $('#id_actual').val();
    params.at_no = $("#at_no").val();

    $('#popupbobin3').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/TIMS/GetbobinOqc",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#popupbobin3')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#searchBtn_popupbobbin3").click(function () {
    $('#popupbobin3').clearGridData();
    $('#popupbobin3').jqGrid('setGridParam', { search: true });
    var pdata = $('#popupbobin3').jqGrid('getGridParam', 'postData');
    getData_bboqc(pdata);
});
$("#container_OQC").on("click", function () {
    $('.diologbobin3').dialog('open');
    $("#s_bb_no3").val("");
    $("#s_bb_nmdv").val("");

    $('#popupbobin3').clearGridData();
    $('#popupbobin3').jqGrid('setGridParam', { search: true });
    var pdata = $('#popupbobin3').jqGrid('getGridParam', 'postData');
    getData_bboqc(pdata);
});
var bobbinOqc = "";
var prevCellVal1 = { cellId1: undefined, value1: undefined };
$("#popupbobin3").jqGrid
    ({
        mtype: 'Get',
        colModel: [
            { key: true, label: 'blno', name: "wmtid", width: 80, align: 'center', hidden: true },
            {
                key: true, label: 'Container Code', name: 'bb_no', width: 230, sort: true, cellattr: function (rowId, val, rawObject, cm, rdata) {
                    var result;
                    if (prevCellVal1.value1 == val) {
                        result = ' style="display: none" rowspanid="' + prevCellVal1.cellId1 + '"';
                    }
                    else {
                        var cellId1 = this.id + '_row_' + rowId + '_' + cm.name;

                        result = ' rowspan="1" id="' + cellId1 + '"';
                        prevCellVal1 = { cellId1: cellId1, value1: val };
                    }
                    return result;
                }
            },
           // { key: false, label: 'Container Name', name: 'bb_nm', width: 150, align: 'left' },
            { key: false, label: 'Material', name: 'mt_cd', width: 250, align: 'left' },
            { key: false, label: 'Create User', name: 'reg_id', align: 'center' },
            { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            $("#save_bb3").removeClass("disabled");
            var selectedRowId = $("#popupbobin3").jqGrid("getGridParam", 'selrow');
            row_id = $("#popupbobin3").getRowData(selectedRowId);
            bobbinOqc = row_id.bb_no;
        },
        gridComplete: function () {
            var rows = grid.getDataIDs();
            var grid_a = $('#popupbobin3');

            $('td[rowspan="1"]', grid_a).each(function () {
                var spans = $('td[rowspanid="' + this.id + '"]', grid_a).length + 1;

                if (spans > 1) {
                    $(this).attr('rowspan', spans);
                }
            });
        },
        pager: jQuery('#PageBobbin2'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: null,
        autowidth: false,
        rowNum: 50,
        caption: 'Container List',
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
    });

$("#scan_oqc").click(function () {
    if (!bobbinOqc) {
        alert('Select 1 Container.');
        return;
    } else {

        $.get("/TIMS/GetListQrOqcTims?id_actual=" + $("#id_actual").val() + "&staff_id=" + $("#staff_id").val() + "&bb_no=" + bobbinOqc, function (data) {
            $('.diologbobin3').dialog('close');
            if (data.result == true) {
                var id = data.kq.wmtid;
                $("#tb_mt_cd_oqc").jqGrid('addRowData', id, data.kq, 'first');
                    $("#tb_mt_cd_oqc").setRowData(id, false, { background: "#d0e9c6" });
            } else {
                ErrorAlert(data.message);
            }
        });
    }
});
var mt_cpi_paking = "";
var id_cpi_paking = "";
$("#tb_mt_cd_oqc").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmtid', hidden: true },
            { label: 'ML No', name: 'mt_cd', width: 400, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 200, },
            { label: 'Total', name: 'gr_qty', hidden: true },
            { label: 'Quantity', name: 'gr_qty1', formatter: modify_gr, align: 'right', width: 100,  },
            { label: 'QC', name: 'qc', align: 'right', width: 100, formatter: check_oqc },
            { label: 'Container', name: 'bb_no', width: 150, align: 'center' },
            { label: 'color', name: 'count_ng', hidden: true },
        ],
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#tb_mt_cd_oqc").jqGrid("getGridParam", 'selrow');
            row_id = $("#tb_mt_cd_oqc").getRowData(selectedRowId);
            mt_cpi_paking = row_id.mt_cd;
            id_cpi_paking = row_id.wmtid;
        },
        gridComplete: function () {
            var rows = $("#tb_mt_cd_oqc").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var remark = $("#tb_mt_cd_oqc").getCell(rows[i], "count_ng");
                if (parseInt(remark) > 0) {
                    $("#tb_mt_cd_oqc").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                }
            }
        },
        pager: jQuery('#tb_mt_cd_oqc_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        height: 200,
        multiselect: true,
        rownumbers: true,
      
        width: null,
        autowidth: false,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Material',
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

function modify_gr(cellValue, options, rowdata, action) {
    return rowdata.gr_qty;
}
function check_oqc(cellValue, options, rowdata, action) {
    if (rowdata.gr_qty > 0) { }
    var item_vcd = $("#check_qc").val();
    var html = '<button style="color: dodgerblue;border: none;background: none; " data-total_qty="' + rowdata.gr_qty + '" data-id="' + rowdata.wmtid + '"  onclick="item_oqc_check(this);">' + item_vcd + '</button>';
    return html;
};
function item_oqc_check(e) {
    //ktra xem total có > 0 không, nếu == 0 thì ko view pp check qc
    var total_qty = $(e).data("total_qty");
    if (total_qty == 0) {
        alert("There is no quantity to Check QC");
        return false;
    }
    $('.dialog_oqc_code_pp').dialog('open');
    var id = $(e).data("id");
    var rowData = $("#tb_mt_cd_oqc").jqGrid('getRowData', id);
    $("#oqc_gr_qty").val(rowData.gr_qty);
    $("#oqc_mt_cd").val(rowData.mt_cd);
    var item_vcd = $("#check_qc").val();
    table_facline_oqc(item_vcd, rowData.mt_cd);
    table_facline_oqc_value();
    $("input#oqc_ok_qty").attr({
        "max": rowData.gr_qty,        // substitute your own
    });

    $('#oqc_ng_qty').attr('readonly', true);
    var process_type = $("#type_roll").val();
    var name = $("#dialogCompositeProcess").val();
    if (process_type) {
        if (process_type == "100" || name == "OQC") {
            $('#oqc_gr_qty').attr('readonly', false);
        }
        else {
            $('#oqc_gr_qty').attr('readonly', true);

        }
    }
}
$(".dialog_oqc_code_pp").dialog({
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

        getViewItemOQC();
    },

    close: function (event, ui) {
        $('.dialog_oqc_code_pp').dialog('close');
        document.getElementById("oqc_ok_qty").value = "0";
        document.getElementById("oqc_gr_qty").value = "0";
        document.getElementById("oqc_ng_qty").value = "0";
        $('input:checkbox').removeAttr('checked');
        a = 0;
        $("#list_oqc").trigger("reloadGrid");
        $("#list_oqc_value").clearGridData();

        jQuery("#list_facline_oqc").trigger("reloadGrid");
        $("#list_facline_oqc_value").clearGridData();
        $("#op_SaveQcfacline").attr("disabled", true);
    },
});
function getViewItemOQC() {
    $.get("/TIMS/PartialView_View_OQC_WEB?item_vcd=" + $("#check_qc").val(), function (html) {
        $("#view_oqc").html(html);
    });
}
$('input#oqc_ok_qty').keyup(function () {

    var value = parseInt(this.value.trim());
    if (value == 0) {
        $("#oqc_ok_qty").val(0);
        return false;
    }

    if (value != "") {
        var value_check = $("#oqc_gr_qty").val();
        if (parseInt(value) > parseInt(value_check)) {
            $("#oqc_ok_qty").val(value_check)
        }
        var check_qty = (document.getElementById('oqc_gr_qty').value == "") ? 0 : document.getElementById('oqc_gr_qty').value;
        var ok_qty = (document.getElementById('oqc_ok_qty').value == "") ? 0 : document.getElementById('oqc_ok_qty').value;
        if (document.getElementById('oqc_ok_qty').value == "") {
            $("#oqc_ng_qty").val("0");
        }
        var max = parseInt(check_qty) - parseInt(ok_qty);
        $("#oqc_ng_qty").val(max);
    } else {
        $("#oqc_ng_qty").val("0");
    }
});
$("#save_oqc_code").on("click", function () {

    var sluong_max = 0;


    var okqty = $('#oqc_ok_qty').val();
    var check_qty = $('#oqc_gr_qty').val();
    if (check_qty == 0) {
        $('#oqc_gr_qty').focus();
        return
    }

    var listTable = $("#view_oqc > div");
    var model = [];

    for (var i = 0; i < listTable.length; i++) {
        //debugger
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
            if (input > 0) {
                sluong_max += parseInt(input);
            }
            var objTr = {
                icdno: icdno,
                input: input

            };
            itemTr.push(objTr);
        }

        itemObj['objTr'] = itemTr;

        model.push(itemObj);

    }
    if (okqty == 0) {
        var itemTr = [];
        for (var i = 0; i < model.length; i++) {

            for (var j = 0; j < model[i].objTr.length; j++) {

                if (model[i].objTr[j].input > 0) {
                    itemTr.push(model[i].objTr[j].input);
                }
            }
        }
        if (itemTr.length == 0) {
            $('#oqc_ok_qty').focus();
            return
        }
    }
    // nếu ng qty mà lớn hơn check_qty_error thì return false

    var oqc_ng_qty = $('#oqc_ng_qty').val()
    if (parseInt(sluong_max) < parseInt(oqc_ng_qty)) {
        alert("The number check of errors have to enough number NG Qty")
        return false;
    }
    var models = JSON.stringify({
        'model': model,
        'name': $('#oqc_name').val(),
        'style_no': $('#oqc_style_no').val(),
        'item_vcd': $('#oqc_item_vcd').val(),
        'check_qty': $('#oqc_gr_qty').val(),
        'ok_qty': $('#oqc_ok_qty').val(),
        'mt_cd': $('#oqc_mt_cd').val(),
        'date': $('#oqc_date').val(),
        'id_actual': $('#id_actual').val(),
        'check_ng': oqc_ng_qty,
    });

    $.ajax({
        type: 'POST',
        url: "/TIMS/Insert_w_product_oqc",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: models,
        cache: false,
        processData: false,
        success: function (data) {
            if (data.result) {
                SuccessAlert("Success");
                document.getElementById("oqc_gr_qty").value = "0";
                document.getElementById("oqc_ok_qty").value = "0";
                document.getElementById("oqc_ng_qty").value = "0";
                jQuery("#list_oqc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_oqc_value").clearGridData();
                jQuery("#list_facline_oqc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_facline_oqc_value").clearGridData();
                $(".testnumber1").val("0");
                var max_check_qty = document.getElementById("oqc_gr_qty").max;
                call_tableoqc();
            }
            else {
                ErrorAlert(data.message);
            }
        }
    });
});

function table_facline_oqc(item_vcd, mt_cd) {
    $("#list_facline_oqc").setGridParam({ url: "/TIMS/Getfacline_oqc?" + "mt_cd=" + mt_cd + "&item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
    $("#list_facline_oqc").jqGrid
        ({
            url: "/TIMS/Getfacline_Oqc?mt_cd=" + mt_cd + "&item_vcd=" + item_vcd,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'pqno', name: 'pqno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'PQ NO', name: 'pq_no', sortable: true, width: '120', align: 'center' },
                { key: false, label: 'Work Date', name: 'work_dt', sortable: true, width: '150', align: 'center' },
                { key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
                { key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
                { key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list_facline_oqc").jqGrid("getGridParam", 'selrow');
                row_id = $("#list_facline_oqc").getRowData(selectedRowId);
                $("#list_facline_oqc_value").clearGridData();
                $("#list_facline_oqc_value").setGridParam({ url: "/TIMS/Getfacline_oqc_value?" + "pq_no=" + row_id.pq_no, datatype: "json" }).trigger("reloadGrid");
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
            loadonce: true,
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
function table_facline_oqc_value() {
    $("#list_facline_oqc_value").jqGrid
        ({
            url: "/TIMS/Getfacline_oqc_value",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'pqhno', name: 'pqhno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
                { key: false, label: 'Check Value', name: 'check_value', sortable: true, width: '110', align: 'left' },
                {
                    key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', editable: true,
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
            ],

            onCellSelect: editRow_faclineoqc,
            onSelectRow: function (rowid, selected, status, e) {
                $("#op_SaveQcfacline").attr("disabled", false);
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
            loadonce: true,
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

        })

}
var lastSel;
function editRow_faclineoqc(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_oqc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}
function call_tableoqc() {
    $.get("/TIMS/GetMeterialOqcTims?id_actual=" + $("#id_actual").val() + "&staff_id=" + $("#staff_id").val(), function (data) {
        $("#tb_mt_cd_oqc").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
    });
}
$("#Pass_OQC").click(function () {
    //chuyển trạng thái thành công qua bước tiêp theo
    var i, dem_jqgrid = $("#tb_mt_cd_oqc").jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (dem_jqgrid.length < 1) {
        alert('Select  ML NO.');
        return;
    } else {
        //debugger;
        //var params = new Object();
        //params.Listwmtid = [];
        var Listwmtid = [];
        for (var i = 0; i < dem_jqgrid.length; i++)
        {
            Listwmtid.push(dem_jqgrid[i]);
            //params.Listwmtid.push(dem_jqgrid[i]);
        }
        //Listwmtid = JSON.stringify({ Listwmtid });
        ///params.psid = $("#psid").val();
        //paramss = JSON.stringify({ Listwmtid });
        $.ajax({
            url: `/TIMS/ChangestspackingWeb`,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            traditional: true,
            data: JSON.stringify({ Listwmtid, psid: $("#psid").val()}),
            success: function (data) {
                if (data.result == true) {
                    SuccessAlert("Success");
                    //for (var i = 0; i < dem_jqgrid.length; i++) {
                    //    $("#tb_mt_cd_oqc").jqGrid('delRowData', dem_jqgrid[0]);
                    //}
                    for (var i = dem_jqgrid.length - 1; i >= 0; i--) {

                        $("#tb_mt_cd_oqc").delRowData(dem_jqgrid[i]);

                    }
                } else {
                    ErrorAlert(data.message);
                }
            },
            error: function () {
                return;
            }
        });
        //$.get("/TIMS/ChangestspackingWeb?wmtid=" + dem_jqgrid +"&psid="+$("#psid").val(), function (data) {
        //    if (data.result == true) {
                
        //    } else {
        //        ErrorAlert(data.message);
        //    }
        //    //$("#result_oqc").html("<label class='text-success'>&nbsp;&nbsp;Success: " + data.Sucess + "(Container)</label>" + "<label class='text-primary'>&nbsp;&nbsp;Not Enough Quantity: " + data.Not_Enough + "(Lot)</label>");
        //});
    }
});
$("#Return_OQC").click(function () {


    //chuyển trạng thái thành công qua bước tiêp theo
    var i, dem_jqgrid = $("#tb_mt_cd_oqc").jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (dem_jqgrid.length < 1) {
        alert('Select  ML NO.');
        return;
    } else {
       // debugger;
        var Listwmtid = [];
        for (var i = 0; i < dem_jqgrid.length; i++) {
            Listwmtid.push(dem_jqgrid[i]);
            //params.Listwmtid.push(dem_jqgrid[i]);
        }

        $.ajax({
            url: `/TIMS/ReturnPackingWeb`,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            traditional: true,
            data: JSON.stringify({ Listwmtid }),
            success: function (data) {
                if (data.result == true) {
                    SuccessAlert("Success");
                    for (var i = 0; i < dem_jqgrid.length; i++) {
                        $("#tb_mt_cd_oqc").jqGrid('delRowData', dem_jqgrid[0]);
                    }
                } else {
                    ErrorAlert(data.message);
                }
            },
            error: function () {
                return;
            }
        });

        //$.get("/TIMS/ReturnPacking?wmtid=" + dem_jqgrid, function (data) {
        //    if (data.result == true) {
        //        SuccessAlert("Success");
        //        for (var i = 0; i < dem_jqgrid.length; i++) {
        //            $("#tb_mt_cd_oqc").jqGrid('delRowData', dem_jqgrid[0]);
        //        }
        //    } else {
        //        ErrorAlert(data.message);
        //    }
        //});
    }
});



function OK_NG(cellValue, options, rowdata, action) {
    var html = '<button  class="btn btn-sm btn-secondary button-srh" data-mt_cd="' + rowdata.mt_cd + '" onclick="Add_NG(this);">ADD NG</button>';
    return html;
}
function Add_NG(e) {
    $("#mt_cd_ok").val($(e).data("mt_cd"));
    $('.PopupNG').dialog('open');
    $('#dialogNG').clearGridData();
    $('#dialogNG').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogNG').jqGrid('getGridParam', 'postData');
    getData_NG(pdata);
}
$("#dialogNG").jqGrid
    ({
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmtid', hidden: true },
            { label: 'PO NO', name: 'at_no', width: 100, sortable: true },
            { label: 'ML No', name: 'mt_cd', width: 350, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 200, },
            { label: 'Quantity', name: 'gr_qty', },
        ],
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#dialogNG").jqGrid("getGridParam", 'selrow');
            row_id = $("#dialogNG").getRowData(selectedRowId);
        },
        pager: jQuery('#dialogNGPage'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: false,
        height: 200,
        multiselect: true,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Material',
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
$("#PopupNG_Search").click(function () {
    $('#dialogNG').clearGridData();
    $('#dialogNG').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogNG').jqGrid('getGridParam', 'postData');
    getData_NG(pdata);
});
function getData_NG(pdata) {
    var params = new Object();
    if ($('#dialogNG').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var mt_cd = $('#mt_cd_ng').val().trim();
    var mt_no = $('#mt_no_ng').val().trim();
    var po = $('#po_ng').val().trim();
    var id_actual = $('#id_actual').val().trim();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.mt_cd = mt_cd;
    params.mt_no = mt_no;
    params.PO = po;
    params.id_actual = id_actual;

    $('#dialogNG').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/TIMS/GetNotGoodPoTims",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#dialogNG')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

$("#change_groqc").click(function () {
    var myList = $("#tb_mt_cd_oqc").jqGrid('getDataIDs');
    var id = [];
    var gr_qty = [];
    var value_all = [];
    for (var i = 0; i < myList.length; i++) {
        var rowData = $("#tb_mt_cd_oqc").jqGrid('getRowData', myList[i]);
        var chuoi = "#" + myList[i] + "_gr_qty1";
        var get_input = $(chuoi).val();
        var value = "";
        if (get_input == undefined) {
            value = rowData.gr_qty;
        } else {
            value = get_input;
        }
        value_all.push(value);
        id.push(myList[i]);
    }
    //đưa vào controller xử lí
    $.ajax({
        url: "/TIMS/change_gr_oqc?value_new=" + value_all + "&wmtid=" + id,
        type: "post",
        success: function (data) {
            if (data.result == false) {
                ErrorAlert(data.message);
                call_tableoqc();
            } else {
                SuccessAlert("Success");
                call_tableoqc();
            }
            $("#result_oqc").html("<label class='text-success'>&nbsp;&nbsp;Modify: " + data.success + "(Tray)</label>" + "<label class='text-primary'>&nbsp;&nbsp;Has over the other process: " + data.dem_quacd + "(Lot)</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data.dontexits + "(Lot)</label>");
        },
    });
});
$("#Gop_NG_OK").click(function () {
    var so_luongmm = $("#GR_OK_NG").val();
    var i, check_jqgridng = $("#dialogNG").jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (so_luongmm == "") {
        alert("Please Enter Quantity NG");
    }
    if (check_jqgridng.length < 1) {
        alert("Please choose ML LOT");
    }
    else {
        var mt_cd_ok = $("#mt_cd_ok").val();
        var psid = $("#psid").val();
        $.get("/TIMS/Gop_NG?wmtid=" + check_jqgridng + "&soluong=" + so_luongmm + "&mt_cd=" + mt_cd_ok +"&psid=" + psid, function (data) {
            if (data.result == true) {
                SuccessAlert("Success");
            } else {
                ErrorAlert(data.message);
            }
            $('.PopupNG').dialog('close');
            var value = $("#psid").val();
            call_table1(value);
        });
    }
});
