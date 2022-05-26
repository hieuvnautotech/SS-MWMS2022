$(function () {
    $(".dialog_PO").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 700,
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
            $("#s_start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

            $("#s_end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

            var date = new Date();
            var currentDate = date.toISOString().slice(0, 10);

            document.getElementById('s_end').value = currentDate;

            var future = new Date(); // get today date
            future.setDate(future.getDate() - 7); // - 7 days

            var finalDate = future.getFullYear() + '-' + ((future.getMonth() + 1) < 10 ? '0' : '') + (future.getMonth() + 1) + '-' + ((future.getDate() + 1) < 10 ? '0' : '') + (future.getDate());

            var finalDate = future.getFullYear() + '-' + ((future.getMonth() + 1) < 10 ? '0' : '') + (future.getMonth() + 1) + '-' + future.getDate();

            var finalDate = future.getFullYear() + '-' + ((future.getMonth() + 1) < 10 ? '0' : '') + (future.getMonth() + 1) + '-' + ((future.getDate()) < 10 ? '0' : '') + (future.getDate());

            //console.log(finalDate);

            document.getElementById('s_start').value = finalDate;
            $("#popups_PO").jqGrid
            ({
                url: "/PurchaseMgt/search_po_popup?" + "s_start=" + finalDate + "&s_end=" + currentDate,
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'id', name: 'soid', width: 50, align: 'right', hidden: true },
                    { label: 'PO NO', name: 'po_no', width: 100, align: 'center' },
                    { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
                    { label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
                    { label: 'BOM', name: 'bom_no', width: 110, align: 'center' },
                    { label: 'Order Date', name: 'order_dt', width: 120, align: 'center' },
                    { label: 'Delivery Date', name: 'delivery_dt', width: 120, align: 'center' },
                    { label: 'Destinationcd', name: 'lct_cd', width: 150, align: 'center', hidden: true },
                    { label: 'Destination', name: 'lct_nm', width: 150, align: 'left' },
                    { label: 'Delivery Qty', name: 'delivery_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Qty', name: 'fo_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'WR Remain Qty', name: 'fo_rm_qty', width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Qty', name: 'so_qty', width: 60, width: 120, align: 'right', formatter: 'integer' },
                    { label: 'SO Remain Qty', name: 'so_rm_qty', width: 150, align: 'right', formatter: 'integer' },
                    { label: 'Description', name: 're_mark', width: 300, align: 'left' },

                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popups_PO").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups_PO").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#selectPO").click(function () {
                            switch (status_PO) {
                                case 's':
                                    $('#po_no').val(row_id.po_no);
                                    break;
                                case 'c':
                                    $('#c_po_no').val(row_id.po_no);
                                    $('#c_bom_no').val(row_id.bom_no);
                                    break;
                                case 'm':
                                    $('#m_po_no').val(row_id.po_no);
                                    break;
                            }
                            $('.dialog_PO').dialog('close');
                        });
                    }
                },
                pager: '#popupspager_PO',
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: $(".boxPo").width(),
                autowidth: false,
                loadtext: "Loading...",
                emptyrecords: "No data.",
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

        },
        close: function (event, ui) {
            document.getElementById("form_pp_po").reset();
            jQuery("#popups_PO").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
    });

    $(".popup_po_no").click(function () {
        switch ($(this).attr('id')) {
            case 's_popup_po_no':
                status_PO = "s";
                break;
            case 'c_popup_po_no':
                status_PO = "c";
                break;
            case 'm_popup_po_no':
                status_PO = "m";
                break;
        }
        $('.dialog_PO').dialog('open');
    });

    $('#closePO').click(function () {
        $('.dialog_PO').dialog('close');
    });

});

$("#searchBtn_po_popup").click(function () {
    var s_po_no = $('#s_po_no').val().trim();
    var s_style_no = $('#s_style_no').val().trim();
    var s_start = $('#s_start').val().trim();
    var s_end = $('#s_end').val().trim();
    var s_bom_no = $('#s_bom_no').val().trim();
    var s_destination = $('#s_destination').val().trim();

    $.ajax({
        url: "/PurchaseMgt/search_po_popup",
        type: "get",
        dataType: "json",
        data: {
            s_po_no: s_po_no,
            s_style_no: s_style_no,
            s_start: s_start,
            s_end: s_end,
            s_bom_no: s_bom_no,
            s_destination: s_destination,
        },
        success: function (result) {
            $("#popups_PO").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
//destination
var destination = "/PurchaseMgt/getDestination";
$(document).ready(function () {
    _getdestination();
});
function _getdestination() {

    $.get(destination, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Destination*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.lct_cd + '>' + item.lct_nm + '</option>';
            });
            $("#s_destination").html(html);
        }
    });
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$("#dialogDangerous").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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
});
$("#dialog_deledetail").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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
});
//popup delete info
$("#del_save_but").click(function () {
    $('#dialogDangerous').dialog('open');
});

$('#closesdetele').click(function () {
    $('#dialogDangerous').dialog('close');
});

$("#deletestyle").click(function () {
    $.ajax({
        url: "/PurchaseMgt/deleteRequest",
        type: "post",
        dataType: "json",
        data: {
            pmid: $('#m_pmid').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                $("#list").clearGridData();

                $('.loading').show();

                var grid = $("#list");
                grid.jqGrid('setGridParam', { search: true });
                var pdata = grid.jqGrid('getGridParam', 'postData');

                getDataOutBox(pdata);
                //jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

                $("#list").clearGridData();

                $('.loading').show();

                var grid = $("#list");
                grid.jqGrid('setGridParam', { search: true });
                var pdata = grid.jqGrid('getGridParam', 'postData');

                getDataPopup(pdata);

                $("#list2").jqGrid('clearGridData');
                document.getElementById("form1").reset();
                $("#tab_2").removeClass("active");
                $("#tab_1").addClass("active");
                $("#tab_c2").removeClass("active");
                $("#tab_c1").removeClass("hidden");
                $("#tab_c2").addClass("hidden");
                $("#tab_c1").addClass("active");

            }
            else {
                alert('Ordered, no deletion allowed. Please check again');
            }

        },
        error: function (data) { alert('Ordered, no deletion allowed. Please check again'); }
    });
    $('#dialogDangerous').dialog('close');
});
//popup delete info
//popup delete detail
$("#d_del_save_but").click(function () {
    $('#dialog_deledetail').dialog('open');
});

$('#c_delete_d').click(function () {
    $('#dialog_deledetail').dialog('close');
});

$("#d_delete_d").click(function () {
    var mpo_no = $('#dm_mpo_no').val();
    var bom_no = $('#dm_bom_no').val();
    if (bom_no == "") {
        bom_no = $('#bom_no_to_delete').val();
    }
    var pmid = $('#m_pmid').val();

    $.ajax({
        url: "/PurchaseMgt/deleteRequestSubBom",
        type: "post",
        dataType: "json",
        data: {
            psid: $('#m_psid').val(),
        },
        success: function (response) {

            if (response.result) {
                var ids = jQuery("#list").jqGrid('getDataIDs');
                

                for (i = 0; i <= ids.length; i++) {
                    var rowId = ids[i];
                    var rowData = jQuery('#list').jqGrid('getRowData', rowId);

                    if (rowData['pmid'] == pmid) {
                        $('#list').jqGrid('setSelection', rowId, true);
                        break;
                    } 

                }
                document.getElementById("form4").reset();
                $('#dm_save_but').attr("disabled", true);
                $("#d_del_save_but").attr("disabled", true);
                //$("#list2").setGridParam({ url: "/PurchaseMgt/getRequestBomSub?" + "bom_no=" + bom_no + "&mpo_no=" + mpo_no, datatype: "json" }).trigger("reloadGrid");
            }
            else {
                alert('Ordered, no deletion allowed. Please check again');
            }

        },
        error: function (data) { alert('Ordered, no deletion allowed. Please check again'); }
    });
    $('#dialog_deledetail').dialog('close');
});
//popup delete detail

function getDataPopup(pdata) {
    $('.loading').show();
    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.mpo_no = $("#mpo_no").val();
    params.po_no = $('#po_no').val();
    params._search = pdata._search;
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });


    $.ajax({
        url: "/PurchaseMgt/searchRequestBom",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
            }
        }
    })
};