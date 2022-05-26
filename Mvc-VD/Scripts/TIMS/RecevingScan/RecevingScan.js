$(document).ready(function () {

    $("#s_input_dt").datepicker({
        dateFormat: 'yy-mm-dd',

    });
    $("#SeachDate").datepicker({
        dateFormat: 'yy-mm-dd',

    }).datepicker("setDate", new Date());

    $('#searchBtnPP').click(function ()
    {
        var date = $('#SeachDate').val();
        var product = $('#productCode').val().trim();
        var seachShift = $('#seachShift').val().trim();

        $.ajax({
            url: "/TIMS/getdetail_RecordReceing?date=" + date + "&product=" + product + "&shift=" + seachShift,
            type: "get",
            dataType: "json",

            success: function (result)
            {
                $("#tableDatetimeProcess").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');

            }
        });
    });
});

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


//----- RD POPUP------------------//
function rd_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-rd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
    return html;
};

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
            url: "/TIMS/GetListMaterialTimsReceivingPO",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
                { label: 'PO No', name: 'at_no', width: 200, align: 'left' },
                { label: 'Product', name: 'product', width: 200, align: 'left' },
                { label: 'Container', name: 'bb_no', width: 200, align: 'left' },

                //{ label: 'Material Type', name: 'mt_type_nm', width: 120, align: 'center'},
                //{ label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 100, align: 'center' },

                //{ label: 'Departure', name: 'from_lct_nm', width: 180, align: 'center' },
                { key: false, label: 'Mapping Date', name: 'input_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                //{
                //    name: "", width: 100, align: "center", label: "Action", resizable: false, title: false, hidden: true,
                //    cellattr: function (rowId, val, rawObject, cm, rdata) {
                //        var result;

                //        if (prevCellVal.value == val) {
                //            result = ' style="" rowspanid="' + prevCellVal.cellId + '"';
                //        }
                //        else {
                //            var cellId = this.id + '_row_' + rowId + '_' + cm.name;

                //            result = ` rowspan="${rawObject.rowpan}" id="${cellId}">  <button class="btn btn-xs btn-danger"  data-bb_no="${rawObject.bb_no}"  onclick="Del_Row(this);" > Delete </button`;

                //            prevCellVal = { cellId: cellId, value: val };
                //        }

                //        return result;
                //    }
                //},

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
            caption: 'List',
            height: 500,
            width: null,
            autowidth: false,
            shrinkToFit: false,
            jsonReader:
            {
                root: "data",
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
    $("#list1").jqGrid("navGrid", "#listPager1", {
        add: false, edit: false, del: false, search: false, refresh: true
    });
})

$("#bb_no").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var bb_no = e.target.value.trim();

        $.ajax({
            url: "/TIMS/UpdateMTQRRDList" ,

            type: "POST",
            dataType: "json",
            data: {
                bb_no: bb_no
            },
            success: function (response) {
                if (response.result) {
                    for (i = 0; i < response.Data.length; i++) {
                        $("#list1").jqGrid('setRowData', response.Data[i].wmtid, false, { background: "#28a745", color: "#fff" });
                        $("#list1").jqGrid('delRowData', response.Data[i].wmtid);

                        $("#list1").jqGrid('addRowData', response.Data[i].wmtid, response.Data[i], 'first');
                        $("#list1").jqGrid('setRowData', response.Data[i].wmtid, false, { background: "#28a745", color: "#fff" });
                    }
                    var timer = setTimeout(
                        function () {
                            for (i = 0; i < response.Data.length; i++) {
                                $("#list1").jqGrid('delRowData', response.Data[i].wmtid);
                            }
                            clearTimeout(timer);
                        }, 6000);

                    $("#bb_no").val("");
                    document.getElementById("bb_no").focus();

                } else {
                    alert(response.message);

                    $("#bb_no").val("");
                    document.getElementById("bb_no").focus();
                }
            }
        });
    }
});


//-------SEARCH------------//
$("#searchBtn").click(function () {
    var po_no = $('#s_po_no').val().trim();
    var product = $('#s_product').val().trim();
    var input_dt = $('#s_input_dt').val().trim();
    var bb_no = $('#s_bb_no').val().trim();

    $("#list1").setGridParam({ url: "/TIMS/GetListMaterialTimsReceivingPO?po_no=" + po_no + "&bb_no=" + bb_no + "&product=" + product + "&input_dt=" + input_dt, datatype: "json" }).trigger("reloadGrid");
});
//----- help to focus on input------------------//
setTimeout(function () { $("#bb_no").focus(); }, 1);




///////////////////////////////////// list 2 /////////////////////////////
//-------SEARCH------------//
$("#searchBtn").click(function ()
{
    var po_no = $('#s_po_no').val().trim();
    var product = $('#s_product').val().trim();
    var input_dt = $('#s_input_dt').val().trim();
    var bb_no = $('#s_bb_no').val().trim();

    $("#list1").setGridParam({ url: "/TIMS/Get_List_Material_TimsReceiving_PO?po_no=" + po_no + "&bb_no=" + bb_no + "&product=" + product + "&input_dt=" + input_dt, datatype: "json" }).trigger("reloadGrid");
});
//----- help to focus on input------------------//
setTimeout(function () { $("#bb_no").focus(); }, 1);

$(function ()
{
    var date = $('#SeachDate').val();
    var product = $('#productCode').val();
    var seachShift = $('#seachShift').val();
    $("#tableDatetimeProcess").jqGrid({

        url: "/TIMS/getdetail_RecordReceing?date=" + date + "&product=" + product + "&shift=" + seachShift,
        mtype: "GET",
        datatype: "json",

        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'Product Code', name: 'product', width: 120 },
            { label: 'PO', name: 'at_no', width: 120, },
            { label: 'Bobbin', name: 'bb_no', width: 240, },
            { label: 'Material Code', name: 'mt_cd', width: 300, },
            {
                label: 'Quantity', name: 'real_qty', width: 100, align: 'right',
                formatter: 'integer',
                summaryTpl: "Total: {0}", // set the summary template to show the group summary
                summaryType: "sum",
                align: 'right',
            },
            //{
            //    label: 'Quantity real', name: 'gr_qty', width: 100, align: 'right',
            //    formatter: 'integer',
            //    summaryTpl: "Total: {0}", // set the summary template to show the group summary
            //    summaryType: "sum"
            //},
            //{ label: 'Quantity Real', name: 'gr_qty', width: 80, },
            //{ label: 'Quantity', name: 'real_qty', width: 80, },
            { label: 'Receving Date', name: 'receive_date', width: 90, },
            { label: 'Shift', name: 'shift', width: 90, },
            { label: 'Receving Date Time', name: 'recevice_dt_tims', width: 130 },

        ],
        shrinkToFit: false,
        width: null,
        height: '500',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rownumbers: true,
        loadonce: false,

        multiPageSelection: true,
        rowList: [100, 200, 500, 1000],
        viewrecords: true,
        rowNum: 100,
        pager: "#tableDatetimeProcessPage",
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },

        subGrid: false,
        footerrow: false,
        //gridComplete: function () {

        //    prevCellVal = { cellId: undefined, value: undefined };
        //    var ids = $("#tableDatetimeProcess").jqGrid('getDataIDs');


        //    var sum_Quantity1 = 0;


        //    for (var i = 0; i < ids.length; i++) {
        //        sum_Quantity1 += parseInt($("#tableDatetimeProcess").getCell(ids[i], "real_qty"));

        //    }
        //    alert(sum_Quantity1);
        //      $("#tableDatetimeProcess").jqGrid("footerData", "set", {
        //          mt_cd: "Total:",
        //          real_qty: sum_Quantity1
        //    });

        //},
        grouping: true,
        groupingView:
        {
            groupField: ["receive_date", "product", "shift"],
            groupColumnShow: [true, true, true],
            groupText: ["<b>{0}</b>", "<b>{0}</b>", "<b>{0}</b>"],
            groupOrder: ["asc"],
            groupSummary: [true, true, true], // will use the "summaryTpl" property of the respective column
            groupSummaryPos: ['footer', 'footer', 'footer'],
            groupCollapse: false,
            groupDataSorted: true,
            showSummaryOnHide: true,
            loadonce: false,
        }

    });
});