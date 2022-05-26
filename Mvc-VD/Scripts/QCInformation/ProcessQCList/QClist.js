
$(function () {
    $("#list1").jqGrid
        ({
            //url: "/QCInformation/getqclist",
            datatype: 'json',
            mtype: 'Get',
            //colNames: ['fqno', 'FQ No', ,, 'Work Date', 'OK Qty', 'Qc Qty'],
            colModel: [
                { label: 'fqno', key: true, name: 'fqno', width: 30, align: 'center', hidden: true },
                { label: 'FQ No', name: 'fq_no', width: 120, align: 'center' },
                { label: 'Material Lot Code', name: 'ml_no', width: 400, align: 'left' },
                { label: 'item_vcd', name: 'item_vcd', index: 'oflno', width: 10, hidden: true },
                //{ label: 'Work Date', name: 'work_dt', width: 150, align: 'center', formatter: DateFormatter },
                { label: 'Work Date', name: 'work_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { label: 'OK Qty', name: 'ok_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Qc Qty', name: 'check_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Defective Qty', name: 'defect_qty', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
                { key: false, label: 'Defect QC Rate (%)', name: 'defect_qty_qc_rate', sortable: true, width: '150', align: 'right' },
                { key: false, label: 'OK Qty Rate (%)', name: 'ok_qty_qc_rate', sortable: true, width: '150', align: 'right' }

            ],

            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },

            gridComplete: function () {
                var rows = $("#list1").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var v_use_yn = $("#list1").getCell(rows[i], "use_yn");
                    if (v_use_yn == "N") {
                        $("#list1").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                    }
                }
                $('.loading').hide();
            },
            onSelectRow: function (rowid, selected, status, e) {

                var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
                row_id = $("#list1").getRowData(selectedRowId);

                //var fqno = row_id.fqno;
                var fq_no = row_id.fq_no;

                //$('#m_fq_no').val(fq_no);
                $("#list3").clearGridData();
                $("#list3").setGridParam({ url: "/QCInformation/getdetail?" + "fq_no=" + fq_no + "&item_vcd=" + row_id.item_vcd, datatype: "json" }).trigger("reloadGrid");
            },

            pager: "#list1Pager",
            //pager: jQuery('#list1Pager'),
            rowNum: 50,
            viewrecords: true,
            rowList: [50, 100, 200, 500, 1000],
            height: '350',
            caption: 'Process QC List ',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: false,
            autowidth: true,
            loadonce: false,
            datatype: function (postData) { getData(postData); },
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

});
function DateFormatter(cellvalue, options, rowObject) {
    return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6")
};



function fo_no(cellvalue, options, rowObject) {
    var a, b;

    if (cellvalue == null) {
        return " "
    }
    else {

        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        c = parseInt(b);
        return a + c;
    }
};
function po_no(cellvalue, options, rowObject) {
    var a, b;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        c = parseInt(b);
        return a + c;
    }
};


function bom_no(cellvalue, options, rowObject) {
    var a, b;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1, 11);
        d = cellvalue.substr(11);
        c = parseInt(b);
        return a + c + d;
    }
};


$(function () {
    $("#list3").jqGrid
        ({
            url: "/QCInformation/getdetail",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqhno', name: 'fqhno', width: 50, hidden: true },
                { label: 'FQ No ', name: 'fq_no', width: 110, align: 'center' },
                //{ label: 'Process', name: 'process_no', width: 110, align: 'left' },
                { label: 'Subject', name: 'check_subject', width: 200, align: 'left' },
                { label: 'Value', name: 'check_value', width: 110, align: 'left' },
                { label: 'Defective Qty', name: 'check_qty', width: 110, align: 'right', formatter: 'integer' },

                //{ name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntCellValue },
            ],
            gridComplete: function () {
                var rows = $("#list3").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var v_use_yn = $("#list3").getCell(rows[i], "use_yn");
                    if (v_use_yn == "N") {
                        $("#list3").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                    }
                }
            },

            onSelectRow: function (rowid, status, e) {

                var selectedRowId = $("#list3").jqGrid("getGridParam", 'selrow');
                row_id = $("#list3").getRowData(selectedRowId);

            },



            pager: "#list3Pager",
            pager: jQuery('#list3Pager'),
            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: 220,
            width: '100%',
            autowidth: true,
            rowNum: 50,
            caption: 'QC Details',
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: false,
            autowidth: true,
            loadonce: true,
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


});

$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
    //"autoclose": true
});


$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#list1").jqGrid('exportToPdf',
            options = {
                title: null,
                orientation: 'landscape',
                pageSize: 'A4',
                description: null,
                onBeforeExport: null,
                download: 'download',
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "PQCList.pdf",
                mimetype: "application/pdf"
            }
        );
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list1").jqGrid('exportToExcel',
            options = {
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                fileName: "PQCList.xlsx",
                mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                maxlength: 40,
                onBeforeExport: null,
                replaceStr: null
            }
        );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#list1").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                fileName: "PQCList",
                returnAsString: false
            }
        );
    });
});

function getData(pdata) {

    $('.loading').show();
    var params = new Object();
    if (jQuery('#list1').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.fq_no = $('#fq_no').val() == null ? "" : $('#fq_no').val().trim();
    params.ml_no = $('#ml_no').val() == null ? "" : $('#ml_no').val().trim();
    params.start = $('#start').val() == null ? "" : $('#start').val().trim();
    params.end = $('#end').val() == null ? "" : $('#end').val().trim();



    $("#list1").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/QCInformation/searchQClistProcess",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list1")[0];
                grid.addJSONData(data);
                $('.loading').hide();
                $("#list3").jqGrid('clearGridData');
            }
        }
    })
};
//-----------------------------------------------------------------------------------

$("#searchBtn").click(function () {

    $("#list1").clearGridData();

    $('.loading').show();

    var grid = $("#list1");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);
});
