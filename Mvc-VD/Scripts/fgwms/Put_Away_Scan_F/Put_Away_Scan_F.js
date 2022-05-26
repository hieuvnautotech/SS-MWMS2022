function formatdate(cellValue, options, rowdata, action) {
    if (cellValue != null && cellValue != "") {
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
    var sheet = document.createElement('style');
    var style = document.createElement('style');
    sheet.innerHTML = ".ui-jqgrid .ui-state-hover {border : 1px solid ;background: #32a8a8; color: white; }; ";
    document.body.appendChild(sheet);
    style.innerHTML = ".ui-jqgrid .ui-state-highlight { background: #32a8a8; border-color: rgb(153, 153, 153); color: white; };";
    document.body.appendChild(style);

    $("#list").jqGrid
    ({
        //url: "/fgwms/getput_away_scan",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            //{ key: true, label: 'plno', name: 'plno', width: 50, align: 'right', hidden: true },
            //{ label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            //{ label: 'Group QTY', name: 'gr_qty', width: 100, align: 'right' },
            //{ label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            //{ label: 'Status', name: 'sts_cd', width: 250, align: 'left' },
            //{ label: 'WO', name: 'fo_no', width: 150, align: 'center' },
            //{ label: 'Location', name: 'lct_nm', width: 100, align: 'left' },
            //{ label: 'Location Status', name: 'lct_sts_cd', width: 150, align: 'left' },
            //{ label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left' },
            //{ label: 'Out Date', name: 'output_dt', width: 150, align: 'left' },
            //{ label: 'Destination', name: 'to_lct_cd', width: 150, align: 'left' },
            //{ label: 'Input Date', name: 'input_dt', width: 150, align: 'left' },
            //{ label: 'Description', name: 're_mark', width: 150, align: 'left' },
            //{ label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            //{ key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            //{ key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            //{ label: 'QR Code', name: 'qrcode', width: 350, align: 'left' },
            //{ label: 'PO', name: 'po_no', width: 350, align: 'left' },
            //{ label: 'WO', name: 'fo_no', width: 350, align: 'left' },
            //{ label: 'BOM', name: 'bom_no', width: 350, align: 'left' },
            //{ label: 'Line', name: 'line_no', width: 350, align: 'left' },
            { key: true, label: 'pno', name: 'pno', width: 50, align: 'right', hidden: true },
            { label: 'Finish Lot', name: 'prd_lcd', width: 350, align: 'left' },
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            { label: 'Group Qty', name: 'gr_qty', width: 80, align: 'right' },
            { label: 'Status', name: 'sts_cd', width: 100, align: 'center' },
            { label: 'WO', name: 'fo_no', width: 150, align: 'center' },
            { label: 'Location', name: 'lct_nm', width: 100, align: 'center', hidden: true },
            { label: 'Location Status', name: 'lct_sts_cd', width: 100, align: 'left', hidden: true },
            { label: 'Departure', name: 'from_lct_cd', width: 150, align: 'left', hidden: true },
            { label: 'Out Date', name: 'output_dt', width: 150, align: 'left', hidden: true },
            { label: 'Destination', name: 'to_lct_cd', width: 150, align: 'center' },
            { label: 'Input Date', name: 'input_dt', width: 150, align: 'center', formatter: formatdate },
            { label: 'Description', name: 're_mark', width: 150, align: 'left' },
            { label: 'QR Code', name: 'qrcode', width: 300, align: 'left', hidden: true },
            { label: 'PO', name: 'po_no', width: 100, align: 'left' },
            { label: 'WO', name: 'fo_no', width: 100, align: 'left' },
            { label: 'Product Date', name: 'prd_dt', width: 100, align: 'center', formatter: formatdate, hidden: true },
            { label: 'BOM', name: 'bom_no', width: 100, align: 'left' },
            { label: 'Routing', name: 'line_no', width: 100, align: 'left' },
            { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
            { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $("#c_save_but").attr("disabled", false);
        },
        pager: "#gridpager",
        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 200, 500],
        height: 500,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
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
    });
});//grid1

$("#saveBtn").click(function () {
    if ($("#lct_cd").val().trim() == "") {
        alert("Please enter the Location");
        $("#lct_cd").val("");
        $('#lct_cd').focus();
        return false;
    } else {
        var lct_cd = $('#lct_cd').val().trim();
        var mt_barcode = $('#mt_barcode').val().trim();
        $('#loading').show();
        $.ajax({
            url: "/fgwms/Updatepicking_scan",
            type: "get",
            dataType: "json",
            data: {
                lct_cd: lct_cd,
                mt_barcode: mt_barcode
            },
         
            success: function (response) {
                $('#loading').hide();
                if (response.result) {
                    var id = response.data.pno;
                    $("#list").jqGrid('addRowData', id, response.data, 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    var checkBox = document.getElementById("myCheck");
                    if (checkBox.checked === true) {
                        $("#lct_cd").val(lct_cd);
                        alert(response.message);
                        $("#mt_barcode").val("");
                        $("#mt_barcode").focus();
                        return true;
                    } else {
                        $("#lct_cd").val("");
                        $("#mt_barcode").val("");
                        $("#lct_cd").focus();
                        alert(response.message);
                        return true;
                    }
                }
                else {
                    var checkBox = document.getElementById("myCheck");
                    if (checkBox.checked === true) {
                        $("#lct_cd").val(lct_cd);
                        alert(response.message);
                        $("#mt_barcode").val("");
                        $("#mt_barcode").focus();
                        return false;
                    } else {
                        $("#lct_cd").val("");
                        $("#mt_barcode").val("");
                        $("#lct_cd").focus();
                        alert(response.message);
                        return false;
                    }
                }
            },
            error: function (data) {
                $('#loading').hide();
                alert('QR Code dont have to Forward. Please check again.');
            }
        });
    }
});

$('#mt_barcode').on("keydown", function (e) {
    if (e.keyCode === 13) {
        var lct_cd = $('#lct_cd').val();
        var mt_barcode = $('#mt_barcode').val();
        if ($("#lct_cd").val().trim() == "") {
            alert("Please enter the Location");
            $("#lct_cd").val("");
            $('#lct_cd').focus();
            return false;
        } else {
            $.ajax({
                url: "/fgwms/Updatepicking_scan",
                type: "get",
                dataType: "json",
                data: {
                    lct_cd: lct_cd,
                    mt_barcode: mt_barcode
                },
                //success: function (data) {
                //    if (data == false) {
                //        alert("Location dont have to Finish Warehouse Location or Dont exits QR Code location.Please check again!");
                //    } else {
                //        var id = data.plno;
                //        $("#list").jqGrid('addRowData', id, data, 'first');
                //        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                //        var checkBox = document.getElementById("myCheck");
                //        if (checkBox.checked === true) {
                //            $("#lct_cd").val(lct_cd);
                //        } else {
                //            $("#lct_cd").val("");
                //        }
                //    }
                //},
                success: function (response) {
                    if (response.result) {
                        var id = response.data.pno;
                        $("#list").jqGrid('addRowData', id, response.data, 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                        var checkBox = document.getElementById("myCheck");
                        if (checkBox.checked === true) {
                            $("#lct_cd").val(lct_cd);
                            alert(response.message);
                            $("#mt_barcode").val("");
                            $("#mt_barcode").focus();
                            return true;
                        } else {
                            $("#lct_cd").val("");
                            $("#mt_barcode").val("");
                            $("#lct_cd").focus();
                            alert(response.message);
                            return true;
                        }
                    }
                    else {
                        var checkBox = document.getElementById("myCheck");
                        if (checkBox.checked === true) {
                            $("#lct_cd").val(lct_cd);
                            alert(response.message);
                            $("#mt_barcode").val("");
                            $("#mt_barcode").focus();
                            return false;
                        } else {
                            $("#lct_cd").val("");
                            $("#mt_barcode").val("");
                            $("#lct_cd").focus();
                            alert(response.message);
                            return false;
                        }
                    }
                },
                error: function (data) {
                    alert('QR Code dont have to Forward. Please check again.');
                }
            });
        }
    }

});