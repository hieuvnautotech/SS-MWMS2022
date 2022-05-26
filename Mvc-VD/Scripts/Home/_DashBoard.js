
$(function () {
    var row_id, row_id2;
    $grid = $("#dashBoardGrid").jqGrid
   ({
       //url: "/Home/getFactory",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           //{ key: true, label: 'id', name: 'mt_id', index: 'mt_id', width: 10, hidden: true },
           { label: 'WO', name: 'fo_no', width: 150, align: 'center', formatter: fo_no },
           { label: 'Total', name: 'prod_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Actual', name: 'done_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Defective', name: 'refer_qty', width: 150, align: 'right', formatter: 'integer' },
           { label: 'Efficiency', name: 'eff_qty', width: 150, align: 'right', formatter: numberFormatter },
           //{ name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntCellValue },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       onSelectRow: function (rowid, cellValue, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
       },
       pager: "#dashBoardGridPager",
       viewrecords: true,
       rowList: [50, 200, 500, 1000],
       height: 260,
       autowidth: false,
       width: $(".boxdashBoardGrid").width() - 10,
       caption: 'WO Management',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       shrinkToFit: false,
       multiselect: false,
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

//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#dashBoardGrid').jqGrid('setGridWidth', $(".boxdashBoardGrid").width());
$(window).on("resize", function () {
    var newWidth = $("#dashBoardGrid").closest(".ui-jqgrid").parent().width();
    $("#dashBoardGrid").jqGrid("setGridWidth", newWidth, false);
});






function fo_no(cellvalue, options, rowObject) {
    var a, b;
    if (cellvalue == null) {
        return " "
    }
    else {
        a = cellvalue.substr(0, 1);
        c = parseInt(cellvalue.substr(1, 11));
        return a + c;
    }
};


function numberFormatter(cellvalue, options, rowObject) {

    //console.log(rowObject);

    if (cellvalue.toString().includes(".")) {
        kq = cellvalue.toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");

    } else {
        kq = cellvalue.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");

    }
    return kq + " %"
};


$(function () {
    var row_id, row_id2;
    $grid = $("#noticeGrid").jqGrid
   ({
      
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'No', name: 'bno', width: 50, align: 'center' },
           { label: 'Subject', name: 'title', width: 300, align: 'left', formatter: PopupLink },
           { label: 'Writer', name: 'reg_id', width: 80, align: 'left' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 200, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },

       onSelectRow: function (rowid, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selectedRowId = $("#noticeMgtGrid").jqGrid("getGridParam", 'selrow');
           row_id = $("#noticeMgtGrid").getRowData(selectedRowId);
           var bno = row_id.bno;
           $('#s_bno').val(bno);

       },

       pager: "#noticeGridPager",

       viewrecords: true,
       rowList: [50, 100, 200, 500, 1000],
       rowNum: 10,
       width: $(".boxnoticeGrid").width() - 10,
       //autowidth: true,
       height: 255,
       caption: 'Notice Management',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
       gridview: true,
       shrinkToFit: false,
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

   });

    $('#noticeGrid').jqGrid('setGridWidth', $(".boxnoticeGrid").width());
    $(window).on("resize", function () {
        var newWidth = $("#noticeGrid").closest(".ui-jqgrid").parent().width();
        $("#noticeGrid").jqGrid("setGridWidth", newWidth, false);
    });

});


//-------------------------------------------------------------------//
function PopupLink(cellvalue, options, rowobject) {

    var regex = /(<([^>]+)>)/ig;
    var bno = rowobject.bno;

    if (cellvalue != null) {

        var html = '<span class="p-0 m-0" data-toggle="modal"  style="color: dodgerblue; text-align: left;"  data-target="#modifyModal" data-backdrop="static" data-keyboard="false" onclick="OpenPopup(' + bno + ');" class="poupgbom">' + cellvalue.replace(regex, "") + '</span>';
        return html;
    }
    return cellvalue.replace(regex, "");
};

function OpenPopup(bno) {
    $('.dialog').dialog('open');
    $(document).ready(function () {

        _getinfo(bno);
    });
}
var getinfo = "/Home/getNotice";

function _getinfo(bno) {


    $.get("/Home/getNotice?bno=" + bno, function (data) {

        var html = '';
        html += '<div class="to"><h4 class="title_c"> Writer </h4><div class="title_name">' + data.reg_id + '</div></div>'
        html += '<div class="to"><h4 class="title_c"> Subject </h4><div class="title_name">' + data.title + '</div></div>'
        html += '<div class="to"><h4 class="title_c"> Content </h4><textarea id="content">' + data.content + '</textarea></div>'
        $("#info").html(html);

        CKEDITOR.replace("content",
                {
                    height: 600,
                    //allowedContent: 'p h1 h2 strong em; a[!href]; img[!src,width,height];',
                    toolbar: [],
                    //toolbarCanCollapse: true,
                    //toolbarStartupExpanded: false,
                    allowedContent: true,
                });

    });
}
//--------------------------------------------------------------------//

function ActualEfficiency() {
    $.ajax({
        type: 'post',
     
        dataType: 'text',
        success: function (data) {

            if (isNaN(data)) {
                html = "";
                html += " ", html += " %", $("#m_actual_efficiency").html(html)
            } else {
                var html = "";
                html += data, html += " %", $("#m_actual_efficiency").html(html)
            }
        }
    });
};
ActualEfficiency()
//setInterval(ActualEfficiency, 1000);
function ActualAmount() {
    $.ajax({
        type: 'post',
     
        dataType: 'text',
        success: function (data) {
            //console.log(data);
            var html = '';
            html += data.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#m_actual_amount").html(0);
        }
    });
};

ActualAmount()
//setInterval(ActualAmount, 1000);





function ReceivingMaterial() {
    $.ajax({
        type: 'post',
  
        dataType: 'text',
        success: function (data) {
            //console.log(data);
            var html = '';
            html += data.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            $("#m_rec_material").html(0);
        }
    });
};

ReceivingMaterial()
//setInterval(ReceivingMaterial, 1000);


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function lineChart() {
    $.ajax({
   
        type: "get",
        dataType: "json",
        success: function (rtnData) {

            var list1 = "", list2 = "", list3 = "", list4 = "";
            $.each(rtnData, function (dataType, data) {
                if (dataType == "rows") { list1 = data[0] }
                if (dataType == "list2") { list2 = data[0] }
                if (dataType == "list3") { list3 = data[0] }
                if (dataType == "list4") { list4 = data[0] }


            });
            var ctx = document.getElementById("myChart").getContext("2d"),
                config = {
                    type: "line",
                    data: {

                        labels: list4,

                        datasets: [{
                            label: 'Target',
                            //backgroundColor: 'green',
                            //fillColor: "#D2DED6",
                            //strokeColor: "#D2DED6",
                            //pointStrokeColor: "#D2DED6",
                            data: list1,
                            borderColor: 'blue',
                            borderWidth: 1,
                            fill: true,
                            //yAxisID: 'y-axis-1',
                        },
                        {
                            label: 'Actual',
                            data: list2,
                            //fillColor: "#3c8dbc",
                            //strokeColor: "#3c8dbc",
                            //pointStrokeColor: "#3c8dbc",
                            borderColor: 'green',

                            borderWidth: 1,
                            fill: true,
                            //yAxisID: 'y-axis-2',

                        },
                        {
                            label: 'Defective',
                            data: list3,
                            borderColor: 'red',
                            borderWidth: 1,
                            fill: true,
                            //yAxisID: 'y-axis-2',

                        }
                        ]

                    },
                    options: {
                        //datasetFill: true,
                        responsive: true,
                        title: {
                            display: true,
                            text: "",
                        },
                        scales: {
                            xAxes: [{
                                display: true,
                                scaleLabel: {
                                    display: true,
                                    labelString: 'Day'

                                },
                                ticks: {
                                    beginAtZero: true
                                }
                            }]

                        },
                        plugins: {
                            datalabels: {
                                display: false,
                                align: 'center',

                            }
                        }

                    }
                };
            window.myPie = new Chart(ctx, config);

        },
        //error: function (rtnData) {
        //    alert('error' + rtnData);
        //}
    });
}
lineChart();
//setInterval(lineChart, 30000);

function pieChart1() {
    $.ajax({
      
        type: "get",
        dataType: "json",
        success: function (rtnData) {
            var list1 = "";
            list2 = "",
            list3 = "";
            $.each(rtnData, function (dataType, data) {

                if (dataType == "pie") { list1 = data }
                if (dataType == "roundtarget") { list2 = data }
            });
            new Chart(document.getElementById("myChart3"), {
                type: 'doughnut',
                data: {
                    datasets: [
                      {
                          //label: "Smoke",

                          backgroundColor: ["green", "blue"],
                          data: [list1, list2],
                          //borderColor: "#FFCD56",
                      }

                    ],
                    labels: ['Actual %', 'Target'],

                },
                options: {
                    title: {
                        position: 'bottom',
                        display: true,
                        text: 'Actual Efficiency'
                    },
                    animation: {
                        ////animateRotate: true,

                        //duration: 250 * 1.5,
                        //easing: 'linear',
                        animateRotate: true,
                        //rotation: -10 * Math.PI,
                        //circumference: 50 * Math.PI,
                        //cutoutPercentage: 50,
                        //duration: 2000,




                    },
                    //layout: {
                    //    padding: {
                    //        bottom: 5
                    //    }
                    //}
                    //,
                    tooltips: {
                        enabled: true,
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            align: 'center',
                            anchor: 'center',
                            formatter: function (value, context) {
                                //console.log(value);
                                return value + ' %';
                            },
                            font: {
                                weight: 'bold',
                                //color: 'red',
                            },
                            color: 'white',
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
        }

    });
}


pieChart1();
//setInterval(pieChart1, 30000);

function pieChart2() {
    $.ajax({
     
        type: "get",
        dataType: "json",
        success: function (rtnData) {

            var list1 = "";
            list2 = "",
            list3 = "";
            $.each(rtnData, function (dataType, data) {
                if (dataType == "defective") { list1 = data }
                if (dataType == "targetsum") { list3 = data }
            });

            new Chart(document.getElementById("myChart4"), {
                type: 'doughnut',
                data: {
                    datasets: [
                      {
                          //label: "Smoke",

                          backgroundColor: ["red", "blue"],
                          data: [list1, list3],
                          //borderColor: "#FFCD56",
                      }

                    ],
                    labels: ['Defective', 'Target']
                },
                options: {
                    title: {
                        position: 'bottom',
                        display: true,
                        text: 'Defective'
                    },
                    animation: {
                        ////animateRotate: true,

                        //duration: 250 * 1.5,
                        //easing: 'linear',
                        animateRotate: true,
                        //rotation: -10 * Math.PI,
                        //circumference: 50 * Math.PI,
                        //cutoutPercentage: 50,
                        //duration: 2000,




                    },
                    //layout: {
                    //    padding: {
                    //        bottom: 5
                    //    }
                    //}
                    //,
                    tooltips: {
                        enabled: true,
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
                                //console.log(value);
                                return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                            },
                            font: {
                                weight: 'bold',
                                //color: 'red',
                            },
                            color: 'white',
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


        }

    });
}

pieChart2();
//setInterval(pieChart2, 30000);


$(document).ready(function () {
    //$.ajax({
    
    //    type: "get",
    //    dataType: "json",
    //    success: function (data) {
    //        var user_curent = $('#username').html();
    //        user_curent = user_curent.split("@", 2);
    //        if (data != null && data != undefined) {
    //            var html = '<div class="direct-chat-messages">';
    //            $.each(data, function (key, item) {
    //                html += '<div class="direct-chat-info clearfix">';
    //                if (user_curent[1] == item.reg_id) {
    //                    html += '<div class="text-primary direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                } else {
    //                    html += '<div class="direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                }

    //                html += '<div class="direct-chat-timestamp pull-right text-right clearfix">' + item.reg_dt + ' </div></div>';
    //                html += '<div class="direct-chat-msg"><div class="direct-chat-text">' + item.message + '</div></div>';
    //            });
    //            html += '</div>';
    //            $("#info2").html(html);
    //            $('#c_message').val("");
    //        }
    //    },
    //    error: function (data) {
    //        alert('The Code is the same. Please check again.');
    //    }
    //});

    $("#c_save_but").click(function () {
        if (($('#c_message').val() != "")) {
            var message = $('#c_message').val();
            $.ajax({
               
                type: "get",
                dataType: "json",
                data: {
                    message: message,
                },
                success: function (data) {
                    var user_curent = $('#username').html();
                    user_curent = user_curent.split("@", 2);
                    if (data != null && data != undefined) {
                        var html = '<div class="direct-chat-messages">';
                        $.each(data, function (key, item) {
                            html += '<div class="direct-chat-info clearfix">';
                            if (user_curent[1] == item.reg_id) {
                                html += '<div class="text-primary direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
                            } else {
                                html += '<div class="direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
                            }

                            html += '<div class="direct-chat-timestamp pull-right text-right clearfix">' + item.reg_dt + ' </div></div>';
                            html += '<div class="direct-chat-msg"><div class="direct-chat-text">' + item.message + '</div></div>';
                        });
                        html += '</div>';
                        $("#info2").html(html);
                        $('#c_message').val("");
                    }
                },
            });
        }
    });

    //$("#c_message").keypress(function (event) {
    //    if (($('#c_message').val() != "") && (event.keyCode == '13')) {
    //        var message = $('#c_message').val();
    //        $.ajax({
            
    //            type: "get",
    //            dataType: "json",
    //            data: {
    //                message: message,
    //            },
    //            success: function (data) {
    //                var user_curent = $('#username').html();
    //                user_curent = user_curent.split("@", 2);
    //                if (data != null && data != undefined) {
    //                    var html = '<div class="direct-chat-messages">';
    //                    $.each(data, function (key, item) {
    //                        html += '<div class="direct-chat-info clearfix">';
    //                        if (user_curent[1] == item.reg_id) {
    //                            html += '<div class="text-primary direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                        } else {
    //                            html += '<div class="direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                        }

    //                        html += '<div class="direct-chat-timestamp pull-right text-right clearfix">' + item.reg_dt + ' </div></div>';
    //                        html += '<div class="direct-chat-msg"><div class="direct-chat-text">' + item.message + '</div></div>';
    //                    });
    //                    html += '</div>';
    //                    $("#info2").html(html);
    //                    $('#c_message').val("");
    //                }
    //            },
    //            error: function (data) {
    //                alert('The Code is the same. Please check again.');
    //            }
    //        });
    //    }
    //});

    //setInterval(function () {
    //    $.ajax({
    //        url: "/Home/getMessageAll",
    //        type: "get",
    //        dataType: "json",
    //        data: {
    //        },
    //        success: function (data) {
    //            var user_curent = $('#username').html();
    //            user_curent = user_curent.split("@", 2);
    //            if (data != null && data != undefined) {
    //                var html = '<div class="direct-chat-messages">';
    //                $.each(data, function (key, item) {
    //                    html += '<div class="direct-chat-info clearfix">';
    //                    if (user_curent[1] == item.reg_id) {
    //                        html += '<div class="text-primary direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                    } else {
    //                        html += '<div class="direct-chat-timestamp pull-left">' + item.reg_id + ' </div>';
    //                    }

    //                    html += '<div class="direct-chat-timestamp pull-right text-right clearfix">' + item.reg_dt + ' </div></div>';
    //                    html += '<div class="direct-chat-msg"><div class="direct-chat-text">' + item.message + '</div></div>';
    //                });
    //                html += '</div>';
    //                $("#info2").html(html);
    //            }
    //        },
    //        error: function (data) {  },
    //    });
    //}, 1000);
});

$(function () {
    $(".dialog").dialog({
        width: '100%',
        height: 850,
        maxWidth: 1000,
        maxHeight: 450,
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

    });
    $('#close').click(function () {
        $('.dialog').dialog('close');
    });

});