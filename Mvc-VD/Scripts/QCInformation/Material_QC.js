$(function () {
    $("#list").jqGrid
    ({
        url: "/QCInformation/GetQCInformation",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
           { key: true, label: 'mqno', name: 'mqno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'MMPO', name: 'mpo_no', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'MPO', name: 'mdpo_no', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'MWR No', name: 'mwro_no', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'MT No', name: 'mt_no', sortable: true, width: '180' },
            { key: false, label: 'MT Name', name: 'mt_nm', sortable: true, width: '180' },
            { key: false, label: 'Receive date', name: 'exp_input_dt', sortable: true, width: '100px', align: 'center', formatter: fomatdate },
            { key: false, label: 'Purchase qty', name: 'act_pur_qty', sortable: true, width: '100px', align: 'right', formatter: 'integer' },
            { key: false, label: 'Receive qty', name: 'rec_bundle_qty', sortable: true, width: '100', align: 'right', formatter: 'integer' },
            { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '100', align: 'center' },
            { key: false, label: 'QC Qty', name: 'qc_qty', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'QC Rate', name: 'qc_rate', sortable: true, width: '150', align: 'right' },
            //{ key: false, label: 'Check Qty', name: 'check_qty', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'OK Qty', name: 'ok_qty', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Defective Qty', name: 'defective_qty', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Defect Rate', name: 'defect_rate', sortable: true, width: '150', align: 'right' }
            
        ],
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $("#item_vcd").val(row_id.item_vcd);
            $("#mdpo_no").val(row_id.mdpo_no);
            //$("#list2").setGridParam({ url: "/QCInformation/Getdetai_mt_qc?" + "item_vcd=" + row_id.item_vcd + "&mdpo_no=" + row_id.mdpo_no, datatype: "json" }).trigger("reloadGrid");

            //start lưới 2
         
            $.get("/QCInformation/PartialView_Getdetai_mt_qc?" +
                    "item_vcd=" + row_id.item_vcd +
                "&mwro_no=" + row_id.mwro_no
                  , function (html) {
                      $("#PartialView_Getdetai_mt_qc").html(html);
                    });
            //end lưới 2

            $("#tab_2").removeClass("active");
            $("#tab_1").addClass("active");
            $("#tab_c2").removeClass("active");
            $("#tab_c1").removeClass("hidden");
            $("#tab_c2").addClass("hidden");
            $("#tab_c1").addClass("active");

            
            //gettabel(row_id.item_vcd, row_id.mdpo_no);
            //gettabelinfo(row_id.item_vcd, row_id.mdpo_no);
            //start chart
            $.get("/QCInformation/PartialView_IQC_Info?item_vcd=" + row_id.item_vcd + "&mwro_no1=" + row_id.mwro_no, function (html) {
                $("#PartialView_IQC_Info").html(html);
            });
        },
        pager: "#gridpager",
        //pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: 250,
        width:null,
       
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
    })
    function fomatdate(cellValue, options, rowdata, action) {

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

    $("#searchBtn").click(function () {

        var mwro_no = $('#mwro_no').val().trim();
        var mt_no = $('#s_mt_no').val().trim();
        var start = $('#start').val();
        var end = $('#end').val();
        $.ajax({
          
            //url: "/QCInformation/searchQCInformation",
            url: "/QCInformation/GetQCInformation",
            type: "get",
            dataType: "json",
            data: {
                mwro_no: mwro_no,
                mt_no: mt_no,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });
    $(document).ready(function (e) {
        $('#excelBtn').click(function (e) {
            $("#list").jqGrid('exportToExcel',
                 options = {
                     includeLabels: true,
                     includeGroupHeader: true,
                     includeFooter: true,
                     fileName: "Material_QC.xlsx",
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
                    fileName: "Material QC Information",
                    returnAsString: false
                }
             );
        });

    });
    $(document).ready(function (e) {
        $('#pdfBtn').click(function (e) {
            $("#list").jqGrid('exportToPdf',
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
                    fileName: "Material_QC.pdf",
                    mimetype: "application/pdf"
                }
             );
        });

    });

});//grid1

$("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

$("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

//tải list2 null
$(function () {
    $grid = $("#list2").jqGrid
    ({
   
        datatype: 'json',
        mtype: 'post',
        colModel: [
                { key: true, label: 'mqhno', name: 'mqhno', width: 200, align: 'center', hidden: true },
                { key: false, label: 'MWR NO', name: 'mdpo_no', width: 100, align: 'center', cellattr: form },
                { key: false, label: 'Subject', name: 'check_subject', width: 100, align: 'center', cellattr: jsFormatterCell_2 },
                { key: false, label: 'Value', name: 'check_value', sortable: true, width: '100px', align: 'left' },
                { key: false, label: 'Qty', name: 'qc_qty', sortable: true, width: '80', align: 'right' },
        ],
        gridComplete: function () {  // 그리드 생성 후 호출되는 이벤트
            var grid = this;
            $('td[rowspan="1"]', grid).each(function () {
                var spans = $('td[rowspanid="' + this.id + '"]', grid).length + 1;
                if (spans > 1) {
                    $(this).attr('rowspan', spans);
                }
            });
        },
        pager: jQuery('#gridpager2'),
        rownumbers: true,
        shrinkToFit: false,
        autoResizing: true,
        viewrecords: true,
        height: '320',
        loadonce: true,
        multiselect: false,
        width: null,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000]
    })
    var prevCellVal = { cellId: undefined, value: undefined };
    var chkcell_2 = { cellId2: undefined, chkval2: undefined };
    var form = function (rowId, val, rawObject, cm, rdata) {

        var result;
        if (prevCellVal.value == val) {
            result = ' style="display: none" rowspanid="' + prevCellVal.cellId + '"';
        } else {
            var cellId = this.id + '_row_' + rowId + '_' + cm.name;
            result = ' rowspan="1" id="' + cellId + '"';
            prevCellVal = { cellId: cellId, value: val };
        }

        return result;

    };

    function jsFormatterCell_2(rowid, val, rowObject, cm, rdata) {
        var result2 = "";
        //if (chkcell_2.chkval2 != val) { //check 값이랑 비교값이 다른 경우
        var cellId2 = this.id + '_row_' + rowid + '-' + cm.name;
        result2 = ' rowspan="1" id ="' + cellId2 + '" + name="cellRowspan"';
        chkcell_2 = { cellId2: cellId2, chkval2: val };
        //} else {
        //    result2 = 'style="display:none"  rowspanid="' + chkcell_2.cellId2 + '"'; //같을 경우 display none 처리
        //    //alert(result);
        //}
        return result2;
    }
});//grid2

$("#tab_1").on("click", "a", function (event) {
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
    gettabel();
    gettabelinfo();
});
$("#tab_2").on("click", "a", function (event) {
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

    gettabel();
    gettabelinfo();
});


function gettabel() {
    var mdpo_no = $("#mdpo_no").val();
    var item_vcd = $("#item_vcd").val();
    var mang = [];
    $.get("/QCInformation/Getbddetai_mt_qc?" + "item_vcd=" + item_vcd + "&mdpo_no=" + mdpo_no, function (data) {
        console.log(data);
        if (data.length > 0) {
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
            html_overview += '</table>';
            html += '</table>';
            html2 += '</table>';
            $("#table_prounit").html(html);
            $("#table_pie").html(html2);
            if (html2 != "") {
                getdatabar(data, mang);
            }
        }
        else {
            $("#table_prounit").html("");
            $("#table_pie").html("");
        }
    });

}

function getdatabar(data, mang) {
    //getgiulieulenbang
    var list1 = [];
    var name = [];

    for (var i = 0; i < data.length; i++) {
        list1.push(data[i].qc_qty);
        var ten = catchuoi(data[i].check_value, 20)
        name.push(ten);
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
                    legend: {
                        display: true,
                        labels: {
                            boxWidth: 0
                        }
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
                              labels: catchuoi(name, 20),
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
                          },
                      });
            list1 = [];
            name = [];
        }
    };


}
//table_info
function gettabelinfo() {
    var mdpo_no = $("#mdpo_no").val();
    var item_vcd = $("#item_vcd").val();

    $.get("/QCInformation/Get_table_info?" + "item_vcd=" + item_vcd + "&mdpo_no=" + mdpo_no, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
            var html_pie = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
            var giu_lieu = data.length;
            var cars = [];
            var reg = 0;
            cars.push(reg);
            $.each(data, function (key, item) {
                var reg_moi = 0;

                if (reg < giu_lieu) {
                    if (cars == 0) {
                        var reg_moi = 0;
                        cars.push(reg_moi);
                    }
                    else {
                        var ds = cars.length - 1;
                        var reg_moi = cars[ds] + 4;
                        if (reg_moi < giu_lieu && key == reg_moi) {
                            cars.push(reg_moi);
                        }
                    }

                    if (reg_moi == key) {
                        html += '<tr>';
                        html_pie += '<tr>';
                    }
                    html += '<td ><canvas id="bar-chart-grouped-' + key + '" style="width : 400px" height="200"></canvas> </td>';
                    html_pie += '<td ><canvas id="pie-chart-grouped-' + key + '" style="width : 400px" height="200"></canvas> </td>';

                }
            });
            html += '</table>';
            html_pie += '</table>';
            $("#table_info").html(html);
            $("#table_pie_info").html(html_pie);

            if (html_pie != "") {
                gettable_info_dt();
            }
        }
        else {
            $("#table_info").html("");
            $("#table_pie_info").html("");
        }
    });
    gettable_info_dt();
}
function gettable_info_dt() {
    var mdpo_no = $("#mdpo_no").val();
    var item_vcd = $("#item_vcd").val();
    $.get("/QCInformation/Get_table_info?" + "item_vcd=" + item_vcd + "&mdpo_no=" + mdpo_no, function (data) {
        //console.log(data);
        if (data != null && data != undefined && data.length) {
            $.each(data, function (key, item) {
                //console.log(data)
                var ajax = $.ajax({
                    url: "/QCInformation/Get_table_info?" + "item_vcd=" + item_vcd + "&mdpo_no=" + mdpo_no,
                    type: "get",
                    success: function (rtnData) {
                        //console.log(rtnData);
                        var list1 = [],
                            list2 = [];
                        list3 = [];
                        mdpo_no = [];
                        mt_no = [];
                        rec_qty = [];
                        var name = ["Check", 'Ok', 'Defective']
                        //console.log(rtnData);
                        $.each(rtnData, function (dataType, data) {
                            list1.push(data.qc_qty);
                            list2.push(data.ok_qty);
                            list3.push(data.def_qty);
                            mdpo_no.push(data.mdpo_no);
                            mt_no.push(data.mt_no);
                            rec_qty.push(data.rec_qty);
                            console.log(rec_qty);
                           
                        });
                        console.log(rec_qty);
                   
                        var ctx = document.getElementById("bar-chart-grouped-" + key + "").getContext("2d")
                        var myChart = new Chart(ctx, {
                            type: "bar",

                            data: {

                                labels: name,
                                datasets: [{

                                    label: 'Check',
                                    data: list1,
                                    backgroundColor: 'green',
                                    borderWidth: 1,
                                    fill: false,

                                },
                                {
                                    label: 'Ok',
                                    data: list2,
                                    backgroundColor: 'yellow',
                                    borderWidth: 1,
                                    fill: false,

                                },
                                 {
                                     label: 'Defective',
                                     data: list3,
                                     backgroundColor: 'red',
                                     borderWidth: 1,
                                     fill: false,

                                 },
                                ],
                            },
                            options: {
                                scales: {
                                    xAxes: [{
                                        ticks: {
                                            min: 'March'
                                        }
                                    }]
                                },
                                title: {
                                    display: true,
                                    label: true,
                                    position: 'bottom',
                                    text: item.mdpo_no + "[" + item.mt_no + "]",
                                },
                                plugins: {
                                    datalabels: {
                                        display: true,
                                        align: 'center',
                                        anchor: 'center',
                                        //color: '#36A2EB'
                                        //align: 'top',
                                        //fontColor: 'red',
                                        formatter: function (value, context) {
                                            console.log(value);
                                            return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                        },
                                        font: {
                                            weight: 'bold',
                                            //color: 'red',
                                        },
                                        color: 'black',
                                        //labels: {
                                        //    value: {},
                                        //    title: {
                                        //        color: 'blue'
                                        //    }
                                        //}
                                        //formatter: function (value, context) {
                                        //    return GetValueFormatted(value);
                                        //}
                                    }
                                }

                            }
                        });
                        //window.myBar = new Chart(ctx, config);

         
                        var myChart1 = new Chart(document.getElementById("pie-chart-grouped-" + key + ""),
                      {
                                type: 'pie',
                                data: {
                                    datasets: [
                                    {
                                        backgroundColor: ["green", "yellow","red"],
                                        data: [list1, list2,list3],
                                    }

                                    ],
                                    labels: ['Pass', 'Defective','ok']
                                },

                                options: {
                                    title: {
                                        position: 'bottom',
                                        display: true,
                                        text: item.mt_no,
                                    },
                                    animation: {

                                        animateRotate: false,

                                    },

                                    tooltips: {
                                        enabled: true,
                                    }
                                    ,
                                    plugins: {
                                        datalabels: {
                                            display: true,
                                            align: 'center',
                                            anchor: 'center',
                                            //color: '#36A2EB'
                                            //align: 'top',
                                            //fontColor: 'red',
                                            //formatter: Math.ceil,
                                            font: {
                                                weight: 'bold',
                                                //color: 'red',
                                            },
                                            color: 'black',
                                            //labels: {
                                            //    value: {},
                                            //    title: {
                                            //        color: 'blue'
                                            //    }
                                            //}
                                            formatter: function (value, context) {
                                                console.log(value);
                                                return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                            }
                                        }
                                    }
                                }
                       });
                    }

                });
            });
        }
        else {
            $("#table_info").html("");
            $("#table_pie_info").html("");
        }
    });
}
//table_info


//Cắt chuỗi
function catchuoi(chuoi, gioihan) {
    // nếu độ dài chuỗi nhỏ hơn hay bằng vị trí cắt
    // thì không thay đổi chuỗi ban đầu
    if (chuoi.length <= gioihan) {
        return chuoi;
    }
    else {
        /*
        so sánh vị trí cắt
        với kí tự khoảng trắng đầu tiên trong chuỗi ban đầu tính từ vị trí cắt
        nếu vị trí khoảng trắng lớn hơn
        thì cắt chuỗi tại vị trí khoảng trắng đó
        */
        if (chuoi.indexOf(" ", gioihan) > gioihan) {
            var new_gioihan = chuoi.indexOf(" ", gioihan);
            var new_chuoi = chuoi.substr(0, new_gioihan) + "...";
            return new_chuoi;
        }
        // trường hợp còn lại không ảnh hưởng tới kết quả
        var new_chuoi1 = chuoi.substr(0, gioihan) + "...";
        return new_chuoi1;
    }
}