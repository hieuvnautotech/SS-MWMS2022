
$(function () {
    $("#processQc4Grid").jqGrid
    ({
        url: "/QCInformation/processQc4",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { label: 'oflno', name: 'oflno', index: 'oflno', width: 10, hidden: true, key: true },
            { label: 'WO NO', name: 'fo_no', width: 120, align: 'center', hidden: true },
            { label: 'WO NO', name: 'fo_no2', width: 120, align: 'center', formatter: fo_no },
            { label: 'PO NO', name: 'po_no', width: 120, align: 'center', formatter: po_no },
            { label: 'Routing No', name: 'line_no', width: 120, align: 'center' },
            { label: 'Product Code', name: 'style_no', width: 250, align: 'left' },
            { label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
            { label: 'BOM No', name: 'bom_no', width: 120, align: 'center', formatter: bom_no },
            { label: 'Factory', name: 'lct_nm', width: 120, align: 'left' },
            { label: 'Target', name: 'target_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Qc Rate', name: 'qc_rate', width: 100, align: 'right', formatter: numberFormatter },
            { label: 'Check Qty', name: 'check_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'OK Qty', name: 'ok_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Defective Qty', name: 'defective_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Defect Rate', name: 'defect_rate', width: 100, align: 'right', formatter: numberFormatter }
        ],

        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },

        loadComplete: function () {
            var rows = $("#processQc4Grid").jqGrid('getDataIDs');
            $("#processQc4Grid").setSelection(rows[0]);
            for (var i = 0; i < rows.length; i++) {
                var v_use_yn = $("#processQc4Grid").getCell(rows[i], "use_yn");
                if (v_use_yn == "N") {
                    $("#processQc4Grid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
            $("#processQcSubGrid").clearGridData();
            $("#processQcDtGrid").clearGridData();
        },

        //onSelectRow: function (rowid, selected, status, e) {
        onSelectRow: function () {
            var selectedRowId = $("#processQc4Grid").jqGrid("getGridParam", 'selrow');
            rowid = $("#processQc4Grid").getRowData(selectedRowId);
            $("#processQcSubGrid").clearGridData();
            $("#processQcDtGrid").clearGridData();
            $("#processQcDtGrid").setGridParam({ url: "/QCInformation/processQcDt?fo_no=" + rowid.fo_no + "&line_no=" + rowid.line_no, datatype: "json" }).trigger("reloadGrid");
            
            //var rowData = $(this).getLocalRow(rowid);
            //$("#processQcSubGrid").clearGridData();
            //$("#processQcDtGrid").clearGridData();
            //$("#processQcDtGrid").setGridParam({ url: "/QCInformation/processQcDt?fo_no=" + rowData.fo_no + "&line_no=" + rowData.line_no, datatype: "json" }).trigger("reloadGrid");
        },

        pager: "#processQc4GridPager",
        pager: jQuery('#processQc4GridPager'),
        rowNum: 50,
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 220,
        caption: 'Process QC List ',
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

$(function () {
    $("#processQcDtGrid").jqGrid
    ({
        url: "/QCInformation/processQcDt",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'oldno', name: 'oldno', width: 80, align: 'center', hidden: true },
            { label: 'line_no', name: 'line_no', width: 80, align: 'center', hidden: true },
            { label: 'fq_no', name: 'fq_no', width: 80, align: 'center', hidden: true},
            { label: 'fo_no', name: 'fo_no', width: 80, align: 'center', hidden: true },
             { label: 'Level', name: 'process_no', width: 80, align: 'center', hidden: true },
            { label: 'Level', name: 'prounit_cd', width: 80, align: 'center', hidden: true },
            { label: 'Level', name: 'level', width: 80, align: 'center', hidden: true },
            { label: 'Level', name: 'level2', width: 80, align: 'center', formatter: levelformatter },
            { label: 'Process NO', name: 'process_nm', width: 100, align: 'center' },
            { label: 'Target QTY', name: 'sked_qty', width: 150, align: 'right', formatter: 'integer' },
            { label: 'Qc Code', name: 'item_vcd', align: 'center', width: 120 },
            { label: 'Qc Rate', name: 'qc_rate', align: 'right', width: 120, formatter: numberFormatter },
            { label: 'Check Qty', name: 'check_qty', align: 'right', width: 120, formatter: 'integer' },
            { label: 'OK Qty', name: 'ok_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Defective Qty', name: 'defective_qty', width: 100, align: 'right', formatter: 'integer' },
            { label: 'Defect Rate', name: 'defect_rate', width: 100, align: 'right', formatter: numberFormatter }
        ],
        gridComplete: function () {
            //var rows = $("#processQcDtGrid").getDataIDs();
            var rows = $("#processQcDtGrid").jqGrid('getDataIDs');
            $('#processQcDtGrid').setSelection(rows[0]);
            for (var i = 0; i < rows.length; i++) {
                var v_use_yn = $("#processQcDtGrid").getCell(rows[i], "use_yn");
                if (v_use_yn == "N") {
                    $("#processQcDtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },

        onSelectRow: function (rowid, selected, status, e) {
            var rowData = $(this).getLocalRow(rowid);
            $("#processQcSubGrid").clearGridData();
            $("#processQcSubGrid").setGridParam({ url: "/QCInformation/processQcSub?fo_no=" + rowData.fo_no + "&process_no=" + rowData.process_no + "&prounit_cd=" + rowData.prounit_cd, datatype: "json" }).trigger("reloadGrid");
            $.get("/QCInformation/_PQC_Chart?fo_no=" + rowData.fo_no + "&process_no=" + rowData.process_no + "&prounit_cd=" + rowData.prounit_cd, function (html) {
                $("#_PQC_Chart").html(html);
            });
            //gettabel();
        },

        pager: "#processQcDtGridPager",
        pager: jQuery('#processQcDtGridPager'),
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

$(function () {
    $("#processQcSubGrid").jqGrid
    ({
        url: "/QCInformation/processQcSub",
        datatype: 'json',
        mtype: 'Get',
        colModel: [

           { label: 'Process', name: 'process_no', width: 120, align: 'center', /* cellattr: form */ },
           { label: 'Subject', name: 'check_subject', width: 200, align: 'left' },
           { label: 'Check Value', name: 'check_value', width: 150, align: 'left' },
           { label: 'Check Qty', name: 'check_qty', align: 'right', width: 120 },
        ],
        gridComplete: function () {
            var rows = $("#processQcDtGrid").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var v_use_yn = $("#processQcDtGrid").getCell(rows[i], "use_yn");
                if (v_use_yn == "N") {
                    $("#processQcDtGrid").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                }
            }
        },

        onSelectRow: function (rowid,selected, status, e) {
            var selectedRowId = $("#processQcDtGrid").jqGrid("getGridParam", 'selrow');
            row_id = $("#processQcDtGrid").getRowData(selectedRowId);
            

        },
        pager: "#processQcSubGridPager",
        pager: jQuery('#processQcSubGridPager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500],
        height: 300,
        width: '100%',
        autowidth: true,
        rowNum: 50,
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


//function gettabel() {
//    var item_vcd = $("#item_vcd").val();
//    var fq_no = $("#fq_no").val();
//    var mang = [];
//    $.get("/QCInformation/processQcSub?fq_no=" + fq_no + "&item_vcd=" + item_vcd, function (data) {

//        if (data.length == 0) {
//            $("#container-pie").addClass("hidden");
//            $("#container-pie").removeClass("active");
//            $("#container-bar").addClass("hidden");
//            $("#container-bar").removeClass("active");
            
//        }
//        else if (data.length != 0) {

//            $("#container-pie").addClass("active");
//            $("#container-pie").removeClass("hidden");
//            $("#container-bar").addClass("active");
//            $("#container-bar").removeClass("hidden");

//            $("#table_pie").addClass("active");
//            $("#table_pie").removeClass("hidden");
//            $("#table_prounit").addClass("active");
//            $("#table_prounit").removeClass("hidden");

//            var trc = data[0].check_subject;
//            var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//            var html2 = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//            $.each(data, function (key, item) {
//                var sau = item.check_subject;
//                if (trc != sau || (key) == (data.length - 1)) {
//                    var gtri = 0;
//                    if ((key) == (data.length - 1)) {
//                        gtri = key;
//                    } else {
//                        gtri = key - 1;
//                    }
//                    mang.push(gtri);
//                    html += '<td ><canvas id="bar-chart-grouped-' + gtri + '" style="width : 350px" height="200"></canvas> </td>';
//                    html2 += '<td ><canvas id="pie-chart-grouped-' + gtri + '"  style="width : 350px" height="200"></canvas> </td>';
//                }
//                trc = item.check_subject;
//            });
//            html += '</table>';
//            html2 += '</table>';
//            $("#table_prounit").html(html);
//            $("#table_pie").html(html2);
//            if (html2 != "") {
//                getdatabar(data, mang);
//            }
//        }
//    }
//  );
//}
//function getdatabar(data, mang) {
//    //getgiulieulenbang
//    var list1 = [];
//    var list2 = [];
//    var name = [];

//    for (var i = 0; i < data.length; i++) {
//        list1.push(data[i].check_qty);
//        name.push(data[i].check_value);

//        list2.push(data[i].process_no);

//        if (mang.indexOf(i) > -1) {
//            var ctx = document.getElementById("bar-chart-grouped-" + i + "");
//            var myChart = new Chart(ctx, {
//                type: 'bar',
//                data: {
//                    labels: name,
//                    datasets: [{
//                        label: data[i].check_subject,
//                        data: list1,
//                        backgroundColor: [
//                            'rgba(255, 99, 132, 0.2)',
//                            'rgba(54, 162, 235, 0.2)',
//                            'rgba(255, 206, 86, 0.2)',
//                            'rgba(75, 192, 192, 0.2)',
//                            'rgba(153, 102, 255, 0.2)',
//                            'rgba(255, 159, 64, 0.2)'
//                        ],
//                        borderColor: [
//                            'rgba(255, 99, 132, 1)',
//                            'rgba(54, 162, 235, 1)',
//                            'rgba(255, 206, 86, 1)',
//                            'rgba(75, 192, 192, 1)',
//                            'rgba(153, 102, 255, 1)',
//                            'rgba(255, 159, 64, 1)'
//                        ],
//                        borderWidth: 1
//                    }]
//                },
//                options: {
//                    scales: {
//                        yAxes: [{
//                            ticks: {
//                                beginAtZero: true
//                            }
//                        }]
//                    },
//                    legend: {
//                        display: true,
//                        labels: {
//                            boxWidth: 0
//                        }
//                    },
//                    title: {
//                        display: true,
//                        width: 200,
//                        position: 'bottom',
//                        text: list2[i],
//                    },
//                    plugins: {
//                        datalabels: {
//                            display: true,
//                            align: 'center',
//                            anchor: 'center',
//                            //color: '#36A2EB'
//                            //align: 'top',
//                            //fontColor: 'red',
//                            formatter: function (value, context) {
//                                console.log(value);
//                                return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                            },
//                            font: {
//                                weight: 'bold',
//                                //color: 'red',
//                            },
//                            color: 'black',
//                            //labels: {
//                            //    value: {},
//                            //    title: {
//                            //        color: 'blue'
//                            //    }
//                            //}
//                            //formatter: function (value, context) {
//                            //    return GetValueFormatted(value);
//                            //}
//                        }
//                    }
//                }
//            });

//            var myChart1 = new Chart(document.getElementById("pie-chart-grouped-" + i + ""),
//                      {
//                          type: 'pie',
//                          data: {
//                              datasets: [
//                              {
//                                  backgroundColor: [
//                                   'rgba(255, 99, 132, 0.2)',
//                                   'rgba(54, 162, 235, 0.2)',
//                                   'rgba(255, 206, 86, 0.2)',
//                                   'rgba(75, 192, 192, 0.2)',
//                                   'rgba(153, 102, 255, 0.2)',
//                                   'rgba(255, 159, 64, 0.2)'
//                                  ],
//                                  data: list1,
//                              }
//                              ],
//                              labels: name,
//                          },

//                          options: {
//                              title: {
//                                  position: 'bottom',
//                                  display: true,
//                                  text: list2[i],
//                              },
//                              animation: {
//                                  animateRotate: false,
//                              },
//                              legend: {
//                                  display: true,
//                                  labels: {
//                                      boxWidth: 0
//                                  }
//                              },
//                              tooltips: {
//                                  enabled: true,
//                              },
//                              plugins: {
//                                  datalabels: {
//                                      display: true,
//                                      align: 'center',
//                                      anchor: 'center',
//                                      //color: '#36A2EB'
//                                      //align: 'top',
//                                      //fontColor: 'red',
//                                      formatter: function (value, context) {
//                                          console.log(value);
//                                          return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                      },
//                                      font: {
//                                          weight: 'bold',
//                                          //color: 'red',
//                                      },
//                                      color: 'black',
//                                      //labels: {
//                                      //    value: {},
//                                      //    title: {
//                                      //        color: 'blue'
//                                      //    }
//                                      //}
//                                      //formatter: function (value, context) {
//                                      //    return GetValueFormatted(value);
//                                      //}
//                                  }
//                              }
//                          },
//                      });
//            list1 = [];
//            name = [];
//        }
//    };


//}

$("#searchBtn").click(function () {
    var fo_no = $('#fo_no').val().trim();
    var style_no = $('#style_no').val().trim();
    var bom_no = $('#s_bom_no').val().trim();

    $.ajax({
        url: "/QCInformation/searchProcessQc4t",
        type: "get",
        dataType: "json",
        data: {
            fo_no: fo_no,
            style_no: style_no,
            bom_no: bom_no,
        },
        success: function (result) {

            $("#processQc4Grid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            $("#processQcSubGrid").clearGridData();
            $("#processQcDtGrid").clearGridData();
        }
    });

});
$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

//$("#tab_1").on("click", "a", function (event) {
//    //document.getElementById("form1").reset();
//    $("#tab_2").removeClass("active");
//    $("#tab_1").addClass("active");
//    $("#tab_c2").removeClass("active");
//    $("#tab_c1").removeClass("hidden");
//    $("#tab_c2").addClass("hidden");
//    $("#tab_c1").addClass("active");
//    gettabel();

//});
//$("#tab_2").on("click", "a", function (event) {
//    $("#tab_1").removeClass("active");
//    $("#tab_2").addClass("active");
//    $("#tab_c1").removeClass("active");
//    $("#tab_c2").removeClass("hidden");
//    $("#tab_c1").addClass("hidden");
//    $("#tab_c2").addClass("active");
//    gettabel();
//});

$(document).ready(function (e) {
    $('#pdfBtn').click(function (e) {
        $("#processQc4Grid").jqGrid('exportToPdf',
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
                fileName: "PQCInformation.pdf",
                mimetype: "application/pdf"
            }
         );
    });
});

$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#processQc4Grid").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "PQCInformation.xlsx",
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
        $("#processQc4Grid").jqGrid('exportToHtml',
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
                fileName: "PQCInformation",
                returnAsString: false
            }
         );
    });
});

function fo_no(cellvalue, options, rowObject) {
    var a, b;
    a = cellvalue.substr(0, 1);
    b = cellvalue.substr(1, 11);
    c = parseInt(b);
    return a + c;
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
        return a + c +d;
    }
};

function DateFormatter(cellvalue, options, rowObject) {
    return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5")
};

function numberFormatter(cellvalue, options, rowObject) {
    if (cellvalue == null) return "%"
    return cellvalue + " %"
};

function levelformatter(cellvalue, options, rowObject) {
    var a, b, c;
    if (cellvalue == "Last") {
        return cellvalue
    }
    else {
        c = parseInt(cellvalue);
        return c;
    }
};