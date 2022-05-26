var pq_no = "";
var item_vcd = "";


$(function () {
    $("#OQCListGrid").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'pqno', name: 'pqno', width: 50, hidden: true },
                { label: 'PQ NO', name: 'pq_no', width: 110, align: 'center' },
                { label: 'Material Lot Code', name: 'ml_no', width: 300, align: 'center' },
                { label: 'item_vcd', name: 'item_vcd', index: 'oflno', width: 10, hidden: true },
                { label: 'Work Date', name: 'work_dt', width: 150, align: 'center', formatter: _Date },
                { label: 'OK Qty', name: 'ok_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Qc Qty', name: 'check_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Defective Qty', name: 'defect_qty', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
                { key: false, label: 'Defect QC Rate (%)', name: 'defect_qty_qc_rate', sortable: true, width: '150', align: 'right' },
                { key: false, label: 'OK Qty Rate (%)', name: 'ok_qty_qc_rate', sortable: true, width: '150', align: 'right' }
            ],
            onSelectRow: function (rowid, selected, status, e) {

                var selectedRowId = $("#OQCListGrid").jqGrid("getGridParam", 'selrow');

                row_id = $("#OQCListGrid").getRowData(selectedRowId);
                pq_no = row_id.pq_no;
                item_vcd = row_id.item_vcd;

                $("#list2").setGridParam({ /*url: "/WMSReceiving/GetMaterialLotCode",*/ datatype: function (postData) { getOQCDTData(postData); } }).trigger("reloadGrid");



                $("#tab_2").removeClass("active");
                $("#tab_1").addClass("active");
                //$("#tab_c2").removeClass("active");
                //$("#tab_c1").removeClass("hidden");
                //$("#tab_c2").addClass("hidden");
                //$("#tab_c1").addClass("active");
                //gettabel();

                $.get("/QCInformation/_PartialView_OQC_Chart?item_vcd=" + item_vcd + "&pq_no=" + pq_no, function (html) {
                    $("#_PartialView_OQC_Chart").html(html);
                });

            },
            pager: "#OQCListpager",
            pager: jQuery('#OQCListpager'),
            rowNum: 50,
            viewrecords: true,
            rowList: [50, 100, 200, 500, 100],
            height: 250,
            width: null,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            datatype: function (postData) { getOQCData(postData); },
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


    //$("#searchBtn").click(function () {

    //    var fo_no = $('#s_fo_no').val().trim();
    //    var style_no = $('#c_style_no').val().trim();
    //    var bom_no = $('#c_bom_no').val().trim();
    //    $.ajax({
    //        url: "/QCInformation/Search_pr_qc_info",
    //        type: "get",
    //        dataType: "json",
    //        data: {
    //            fo_no: fo_no,
    //            style_no: style_no,
    //            bom_no: bom_no,
    //        },
    //        success: function (result) {
    //            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
    //        }
    //    });
    //});
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $("#list").jqGrid('exportToExcel',
                options = {
                    includeLabels: true,
                    includeGroupHeader: true,
                    includeFooter: true,
                    fileName: "OQC Information.xlsx",
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
            $("#list").jqGrid('exportToHtml',
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
                    fileName: "OQC Information",
                    returnAsString: false
                }
            );
        });

    });

});//grid1

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$(function () {
    $grid = $("#list2").jqGrid
        ({
            colModel: [
                { key: true, label: 'pqhno', name: 'pqhno', width: 50, hidden: true },
                { label: 'PQ No ', name: 'pq_no', width: 110, align: 'center' },
                //{ label: 'Process', name: 'process_no', width: 110, align: 'left' },
                { label: 'Subject', name: 'check_subject', width: 200, align: 'left' },
                { label: 'Value', name: 'check_value', width: 110, align: 'left' },
                { label: 'Defective Qty', name: 'check_qty', width: 110, align: 'right', formatter: 'integer' },

            ],
            pager: jQuery('#grid2pager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            loadonce: true,
            height: '300',
            //caption: 'Part',
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            autowidth: true,
            multiselect: false,
        })
    //.navGrid('#partpager',
    //    {
    //        edit: false, add: false, del: false, search: false,
    //        searchtext: "Search Student", refresh: true
    //    },
    //    {
    //        zIndex: 100,
    //        caption: "Search",
    //        sopt: ['cn']
    //    });
});//grid2
$("#tab_1").on("click", "a", function (event) {
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
    gettabel();
});
$("#tab_2").on("click", "a", function (event) {
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    gettabel();
});


function gettabel() {
    var item_vcd = $("#item_vcd").val();
    var mang = [];
    $.get("/QCInformation/Getdetaipr_qc_info?" + "item_vcd=" + item_vcd, function (data) {
        var trc = data[0].check_subject;
        var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
        var html2 = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
        $.each(data, function (key, item) {
            var sau = item.check_subject;
            if (trc != sau || (key) == (data.length - 1)) {
                var gtri = 0;
                if ((key) == (data.length - 1)) {
                    gtri = key;
                } else {
                    gtri = key - 1;
                }
                mang.push(gtri);
                html += '<td ><canvas id="bar-chart-grouped-' + gtri + '" style="width : 350px" height="200"></canvas> </td>';
                html2 += '<td ><canvas id="pie-chart-grouped-' + gtri + '"  style="width : 350px" height="200"></canvas> </td>';

            }
            trc = item.check_subject;
        });
        html += '</table>';
        html2 += '</table>';
        $("#table_prounit").html(html);
        $("#table_pie").html(html2);
        if (html2 != "") {
            getdatabar(data, mang);
        }
    });

}

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
function po_no(cellvalue, options, rowObject) {
    var a, b;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        b = cellvalue.substr(1);
        c = parseInt(b);
        return a + c;
    }
};




function getdatabar(data, mang) {
    //getgiulieulenbang
    var list1 = [];
    var name = [];

    for (var i = 0; i < data.length; i++) {
        list1.push(data[i].qc_qty);
        name.push(data[i].check_value);
        if (mang.indexOf(i) > -1) {
            var ctx = document.getElementById("bar-chart-grouped-" + i + "");
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: name,
                    datasets: [{
                        label: data[i].check_subject,
                        data: list1,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            align: 'center',
                            anchor: 'center',
                            formatter: function (value, context) {
                                console.log(value);
                                return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                            },
                            font: {
                                weight: 'bold',
                            },
                            color: 'black',

                        }
                    },
                    legend: {
                        display: true,
                        labels: {
                            boxWidth: 0
                        }
                    },
                }
            });

            var myChart1 = new Chart(document.getElementById("pie-chart-grouped-" + i + ""),
                {
                    type: 'pie',
                    data: {
                        datasets: [
                            {
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 206, 86, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    'rgba(153, 102, 255, 0.2)',
                                    'rgba(255, 159, 64, 0.2)'
                                ],
                                data: list1,
                            }
                        ],
                        labels: name,
                    },

                    options: {
                        title: {
                            position: 'top',
                            display: true,
                            text: data[i].check_subject,
                        },
                        animation: {
                            animateRotate: false,
                        },
                        legend: {
                            display: true,
                            labels: {
                                boxWidth: 0
                            }
                        },
                        tooltips: {
                            enabled: true,
                        },
                        plugins: {
                            datalabels: {
                                display: true,
                                align: 'center',
                                anchor: 'center',
                                formatter: function (value, context) {
                                    console.log(value);
                                    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                },
                                font: {
                                    weight: 'bold',
                                },
                                color: 'black',

                            }
                        }
                    }
                });
            list1 = [];
            name = [];
        }
    };


}




$("#searchBtn").click(function () {

    $("#OQCListGrid").clearGridData();

    $('.loading').show();

    var grid = $("#OQCListGrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getOQCData(pdata);
    $("#list2").jqGrid('clearGridData');
});


function getOQCData(pdata) {

    $('.loading').show();
    var params = new Object();
    if (jQuery('#OQCListGrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.pq_no = $('#pq_no').val() == null ? "" : $('#pq_no').val().trim();
    params.ml_no = $('#ml_no').val() == null ? "" : $('#ml_no').val().trim();
    params.start = $('#start').val() == null ? "" : $('#start').val().trim();
    params.end = $('#end').val() == null ? "" : $('#end').val().trim();



    $("#OQCListGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: "/QCInformation/getOQCList",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#OQCListGrid")[0];
                grid.addJSONData(data);
                $('.loading').hide();
                $("#OQCDTListGrid").jqGrid('clearGridData');
            }
        }
    })
};

function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";
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
};




function getOQCDTData(pdata) {
    $('.loading-gif-2').show();
    var params = new Object();
    var rows = $("#list2").getDataIDs();
    //if (gridLot.jqGrid('getGridParam', 'reccount') == 0)
    if (rows.length == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.pq_no = pq_no;
    params.item_vcd = item_vcd;

    $("#list2").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/QCInformation/getOQCDTList",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list2")[0];
                showing.addJSONData(data);
                $('.loading-gif-2').hide();
            }
        },
        error: function () {
            $('.loading-gif-2').hide();
            return;
        }
    });
};
