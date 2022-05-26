$(function () {
    $(".diologcutting").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 700,
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

            $("#popuptabelpaking").jqGrid
            ({
                url: "/ActualWO/searchcuttingpacking?style_no=" + $('#pp_style_no').val(),
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
                    { label: 'MT Cd', name: 'mt_cd_mt', width: 250, align: 'left' },
                    { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
                    { label: 'Group Qty', name: 'gr_qty', width: 100, align: 'right', formatter: donvi },
                    { name: 'gr_qty1', width: 100, align: 'right', hidden: true },
                    { label: 'unit_cd', name: 'unit_cd', width: 150, align: 'right', hidden: true },
                    { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
                    { label: 'Type', name: 'mt_type', width: 150, align: 'left' },
                    { label: 'Status', name: 'status', width: 150, align: 'left' },
                    { label: 'Status', name: 'mt_sts_cd1', width: 150, align: 'left', hidden: true },
                    { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
                    { label: 'Location Status', name: 'location_status', width: 150, align: 'left' },
                    { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
                    { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
                    { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
                    { label: 'Description', name: 'remark', width: 150, align: 'left' },
                    { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
                    { label: 'Origin ML No', name: 'orgin_mt_cd', width: 250, align: 'left' },
                    { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
                    { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
                    { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                        var selectedRowId = $("#popuptabelpaking").jqGrid("getGridParam", 'selrow');
                        row_id = $("#popuptabelpaking").getRowData(selectedRowId);
                        $("#Selected_Cutting").removeClass("disabled");
                    },

                pager: '#pagerBOM',
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: null,
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
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


            });

            $('#search').click(function () {
                var mt_cd = $('#cp_Paking').val().trim();
                var orgin_mt_cd = $('#orgin_mt_cd').val().trim();
                var style_no = $('#pp_style_no').val().trim();
                $.ajax({
                    url: "/ActualWO/searchcuttingpacking",
                    type: "get",
                    dataType: "json",
                    data: {
                        mt_cd: mt_cd,
                        orgin_mt_cd: orgin_mt_cd,
                        style_no: style_no,
                    },
                    success: function (result) {
                        $("#popuptabelpaking").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
                    }
                });
            });
        },
        //open: function (event, ui) {
        //            $("#popuptabelpaking").jqGrid
        //            ({
        //            datatype: 'json',
        //            mtype: 'Get',
        //            colModel: [
        //                    { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
        //                    { label: 'MT Cd', name: 'mt_cd_mt', width: 250, align: 'left' },
        //                    { label: 'ML NO', name: 'mt_cd', width: 400, align: 'left' },
        //                    { label: 'Group Qty', name: 'gr_qty', width: 100, align: 'right', formatter: donvi },
        //                    { name: 'gr_qty1', width: 100, align: 'right', hidden: true },
        //                    { label: 'unit_cd', name: 'unit_cd', width: 150, align: 'right', hidden: true },
        //                    { label: 'MT NO', name: 'mt_no', width: 250, align: 'left' },
        //                    { label: 'Type', name: 'mt_type', width: 150, align: 'left' },
        //                    { label: 'Status', name: 'status', width: 150, align: 'left' },
        //                    { label: 'Status', name: 'mt_sts_cd1', width: 150, align: 'left', hidden: true },
        //                    { label: 'Location ', name: 'lct_nm', width: 150, align: 'left' },
        //                    { label: 'Location Status', name: 'location_status', width: 150, align: 'left' },
        //                    { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
        //                    { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
        //                    { label: 'Bobbin No', name: 'bb_no', width: 150, align: 'left' },
        //                    { label: 'Description', name: 'remark', width: 150, align: 'left' },
        //                    { label: 'MT Name', name: 'mt_nm', width: 250, align: 'left' },
        //                    { label: 'Origin ML No', name: 'orgin_mt_cd', width: 250, align: 'left' },
        //                    { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
        //                    { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        //                    { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
        //                    { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        //            ],

        //            onSelectRow: function (rowid, selected, status, e) {
        //                var selectedRowId = $("#popuptabelpaking").jqGrid("getGridParam", 'selrow');
        //                row_id = $("#popuptabelpaking").getRowData(selectedRowId);
        //                $("#Selected_Cutting").removeClass("disabled");
        //            },
        //            datatype: function (postData) { getData(postData); },
        //            pager: "#Pagetabelpaking",
        //            viewrecords: true,
        //            rowList: [20, 50, 200, 500],
        //            height: 400,
        //            width: $(".box-body").width() - 5,
        //            autowidth: false,
        //            caption: 'Cutting Paking',
        //            loadtext: "Loading...",
        //            emptyrecords: "No data.",
        //            rownumbers: true,
        //            loadonce: true,
        //            gridview: true,
        //            shrinkToFit: false,
        //            multiselect: true,
        //            jsonReader:
        //            {
        //            root: "rows",
        //            page: "page",
        //            total: "total",
        //            records: "records",
        //            repeatitems: false,
        //            Id: "0"
        //            },
        //            });
        //            },
                    });

    $(".poupdialogpacking").click(function () {
        $('.diologcutting').dialog('open');
        var style_no = $('#pp_style_no').val().trim();
        $("#popuptabelpaking").setGridParam({ url: "/ActualWO/searchcuttingpacking?" + "style_no=" + style_no, datatype: "json" }).trigger("reloadGrid");
    });

    $('#close_paking').click(function () {
        $('.diologcutting').dialog('close');
    });

});

$('#Selected_Cutting').click(function () {
    var i, n, rowData, selRowIds = $("#popuptabelpaking").jqGrid("getGridParam", "selarrrow");

    var models = [];
    for (i = 0, n = selRowIds.length; i < n; i++) {
        var ret = $("#popuptabelpaking").jqGrid('getRowData', selRowIds[i]);
        console.log(ret);
        if (!isEmpty(ret)) {
            models.push(ret);
        }
    }
    if (models.length > 0 && models!=null && ($('#pr_lot').val() != "") && ($('#number_sl').val() > 0)) {
       
        $.ajax({
            type: 'POST',
            url: '/ActualWO/Gopcacmalaichodu?mt_lot=' + $('#pr_lot').val() + "&soluong=" + $('#number_sl').val().trim(),
            async: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(models),
            traditonal: true,
            success: function (response) {
            

                if (response.result == false) { alert(response.message) } else {
                    $('.diologcutting').dialog('close');
                    alert(response.message)

                    update_actual(response.gtri);

                    var rowData = $('#tb_pr_lot').jqGrid('getRowData', response.kq.pno);
                    rowData.gr_qty = response.kq.gr_qty;
                    rowData.count_table2 = rowData.count_table2 + models.length;
                    $('#tb_pr_lot').jqGrid('setRowData', response.kq.pno, rowData);
                    $("#tb_pr_lot").setRowData(response.kq.pno, false, { background: "#d0e9c6" });
      

                }

            },
            error: function (response) {


            }
        });
    }

    else {
        alert("Please,Select ML Code! Or Check Product Lot, Or check Group Qty>0 ")
    }



});
function isEmpty(obj) {
    for (var key in obj) {
        if (obj.hasOwnProperty(key))
            return false;
    }
    return true;
}
function donvi(cellvalue, options, rowobject) {
    var unit_cd = rowobject.unit_cd;
    var html = "";
    if (cellvalue == null) { cellvalue = ""; }
    if (unit_cd == null) { unit_cd = ""; }
    html = cellvalue + " " + unit_cd;
    return html;
};

//function getData(pdata) {
//    //$("#Material_jqgrid").closest(".ui-jqgrid").find('.loading').show();
//    $('.loading').show();
//    var mt_cd = $("#cp_Paking").val().trim();
//    var orgin_mt_cd = $("#orgin_mt_cd").val().trim();
//    var style_no = $("#pp_style_no").val().trim();

//    var params = new Object();
//    if (jQuery('#popuptabelpaking').jqGrid('getGridParam', 'reccount') == 0) {
//        params.page = 1;
//    }
//    else { params.page = pdata.page; }
//    params.rows = pdata.rows;
//    params.sidx = pdata.sidx;
//    params.sord = pdata.sord;

//    params.mt_cd = mt_cd;
//    params.style_no = style_no;
//    params.orgin_mt_cd = orgin_mt_cd;

//    $("#popuptabelpaking").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
//    params._search = pdata._search;

//    $.ajax({
//        url: "/ActualWO/searchcuttingpacking",
//        type: "Get",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        traditional: true,
//        data: params,
//        success: function (data, st) {
//            //console.log(data);

//            if (st == "success") {
//                var grid = $("#popuptabelpaking")[0];
//                grid.addJSONData(data);
//                $('.loading').hide();
//            }
//        }
//    })
//};


