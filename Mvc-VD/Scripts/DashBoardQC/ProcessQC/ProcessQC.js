$(document).ready(function () {

    $("#container-pie").addClass("hidden");
    $("#container-bar").removeClass("active");
    //gettable();
    //gettable2();

    //$("#end").datepicker({
    //    dateFormat: 'yy-mm-dd',
    //}).datepicker('setDate', new Date().getDay + 7);

    $("#start").datepicker({
        dateFormat: 'yy-mm-dd',
    }).datepicker('setDate', 'today');

    $("#end").datepicker({
        dateFormat: 'yy-mm-dd',
    }).datepicker('setDate', 'today');
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();

    //start chart bar
    $.get("/DashBoardQC/PartialView_PQCDash_Bar?start=" + start + "&end=" + end, function (html) {
        $("#PartialView_PQCDash_Bar").html(html);
    });
    //start chart pie
    $.get("/DashBoardQC/PartialView_PQCDash_Pie?start=" + start + "&end=" + end, function (html) {
        $("#PartialView_PQCDash_Pie").html(html);
    });

});
$("#barchart").change(function () {
    var id = $(this).val();
    if (id == 1) {
        $("#PartialView_PQCDash_Bar").addClass("active");
        $("#PartialView_PQCDash_Bar").removeClass("hidden");

        $("#PartialView_PQCDash_Pie").addClass("hidden");
        $("#PartialView_PQCDash_Pie").removeClass("active");

    }
    else {
        $("#PartialView_PQCDash_Pie").addClass("active");
        $("#PartialView_PQCDash_Pie").removeClass("hidden");

        $("#PartialView_PQCDash_Bar").addClass("hidden");
        $("#PartialView_PQCDash_Bar").removeClass("active");

    }
});
$("#searchBtn").click(function () {
    var ml_no = $('#s_ml_no').val();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();
    var id = $('#barchart').val();

    if (id == 1) {
        //start chart


        $.get("/DashBoardQC/PartialView_PQCDash_Bar?ml_no=" + ml_no + "&start=" + start + "&end=" + end, function (html) {
            $("#PartialView_PQCDash_Bar").html(html);
        });

    }
    else {
        $.get("/DashBoardQC/PartialView_PQCDash_Pie?ml_no=" + ml_no + "&start=" + start + "&end=" + end, function (html) {
            $("#PartialView_PQCDash_Pie").html(html);
        });

    }

});
//function gettable1() {
//    $.get("/DashBoardQC/gettable_proqc", function (data) {
//        if (data != null && data != undefined && data.length) {
//            $.each(data, function (key, item) {
//                var ajax = $.ajax({
//                    url: "/DashBoardQC/getcolum_proqc?fq_no=" + item.fq_no,
//                    type: "get",
//                    success: function (rtnData) {
//                        var list1 = [],
//                         list2 = [];
//                        console.log(rtnData);
//                        $.each(rtnData, function (dataType, data) {
//                            list1.push(data.qc_qty);
//                            list2.push(data.check_value);

//                        });
//                        console.log(list1);
//                        console.log(list2);

//                        var ctx = document.getElementById("bar-chart-grouped-" + key + "").getContext("2d")
//                        config = {
//                            type: "bar",

//                            data: {

//                                labels: list2,
//                                datasets: [{
//                                    label: item.check_subject,
//                                    data: list1,
//                                    backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                    borderWidth: 1,
//                                    fill: false,
//                                },
//                                ],
//                            },
//                            options: {
//                                legend: {
//                                    display: true,
//                                    labels: {
//                                        boxWidth: 0
//                                    }
//                                },
//                                title: {
//                                    display: true,
//                                    width: 200,
//                                    position: 'bottom',
//                                    text: item.process
//                                },
//                                scales: {
//                                    yAxes: [{
//                                        ticks: {
//                                            beginAtZero: true
//                                        }
//                                    }]
//                                },
//                                tooltips: {
//                                    enabled: false,
//                                },
//                                plugins: {
//                                    datalabels: {
//                                        display: true,
//                                        align: 'center',
//                                        anchor: 'center',
//                                        //color: '#36A2EB'
//                                        //align: 'top',
//                                        //fontColor: 'red',
//                                        formatter: function (value, context) {
//                                            console.log(value);
//                                            return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                        },
//                                        font: {
//                                            weight: 'bold',
//                                            //color: 'red',
//                                        },
//                                        color: 'black',
//                                        //labels: {
//                                        //    value: {},
//                                        //    title: {
//                                        //        color: 'blue'
//                                        //    }
//                                        //}
//                                        //formatter: function (value, context) {
//                                        //    return GetValueFormatted(value);
//                                        //}
//                                    }
//                                }
//                            }
//                        };

//                        window.myBar = new Chart(ctx, config);
//                    }

//                });
//            });
//        }
//    });
//}


//function gettable() {
//    $.get("/DashBoardQC/gettable_proqc", function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//            var giu_lieu = data.length;
//            var cars = [];
//            var reg = 0;
//            cars.push(reg);
//            $.each(data, function (key, item) {
//                var reg_moi = 0;

//                if (reg < giu_lieu) {
//                    if (cars == 0) {
//                        var reg_moi = 0;
//                        cars.push(reg_moi);
//                    }
//                    else {
//                        var ds = cars.length - 1;
//                        var reg_moi = cars[ds] + 4;
//                        if (reg_moi < giu_lieu && key == reg_moi) {
//                            cars.push(reg_moi);
//                        }
//                    }

//                    if (reg_moi == key) {
//                        html += '<tr>';
//                    }
//                    html += '<td ><canvas id="bar-chart-grouped-' + key + '" style="width : 350px" height="200"></canvas> </td>';

//                }
//            });
//            html += '</table>';
//            $("#table_prounit").html(html);
//        }
//    });
//    gettable1();
//}
////table view
//function gettable2() {
//    $.get("/DashBoardQC/gettable_proqc", function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//            var giu_lieu = data.length;
//            var cars = [];
//            var reg = 0;
//            cars.push(reg);
//            $.each(data, function (key, item) {
//                var reg_moi = 0;

//                if (reg < giu_lieu) {
//                    if (cars == 0) {
//                        var reg_moi = 0;
//                        cars.push(reg_moi);
//                    }
//                    else {
//                        var ds = cars.length - 1;
//                        var reg_moi = cars[ds] + 4;
//                        if (reg_moi < giu_lieu && key == reg_moi) {
//                            cars.push(reg_moi);
//                        }
//                    }

//                    if (reg_moi == key) {
//                        html += '<tr>';
//                    }


//                    html += '<td ><canvas id="pie-chart-grouped-' + key + '"  style="width : 350px" height="200"></canvas> </td>';
//                }
//            });
//            html += '</table>';
//            $("#table_pie").html(html);
//        }
//    });
//    gettable3();
//}


//function gettable3() {
//    $.get("/DashBoardQC/gettable_proqc", function (data) {
//        if (data != null && data != undefined && data.length) {
//            $.each(data, function (key, item) {
//                var ajax = $.ajax({
//                    url: "/DashBoardQC/getcolum_proqc2?fq_no=" + item.fq_no,
//                    type: "get",
//                    success: function (rtnData) {
//                        var list1 = [],
//                                            list2 = [];
//                        console.log(rtnData);
//                        $.each(rtnData, function (dataType, data) {
//                            list1.push(data.qc_qty);
//                            list2.push(data.check_value);

//                        });
//                        new Chart(document.getElementById("pie-chart-grouped-" + key + ""),
//                        {
//                            type: 'pie',
//                            labels: "TEST",
//                            data: {
//                                datasets: [
//                                {
//                                    backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                    data: list1,
//                                }

//                                ],

//                                labels: list2,
//                            },

//                            options: {
//                                title: {
//                                    position: 'bottom',
//                                    display: true,
//                                    text: item.process
//                                },
//                                legend: {
//                                    display: true
//                                },
//                                animation: {

//                                    animateRotate: false,

//                                },

//                                tooltips: {
//                                    enabled: true,
//                                }
//                            },
//                            plugins: {
//                                datalabels: {
//                                    display: true,
//                                    align: 'center',
//                                    anchor: 'center',
//                                    //color: '#36A2EB'
//                                    //align: 'top',
//                                    //fontColor: 'red',
//                                    formatter: function (value, context) {
//                                        console.log(value);
//                                        return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                    },
//                                    font: {
//                                        weight: 'bold',
//                                        //color: 'red',
//                                    },
//                                    color: 'black',
//                                    //labels: {
//                                    //    value: {},
//                                    //    title: {
//                                    //        color: 'blue'
//                                    //    }
//                                    //}
//                                    //formatter: function (value, context) {
//                                    //    return GetValueFormatted(value);
//                                    //}
//                                }
//                            }
//                        });
//                    }
//                });
//            })

//        }
//    })
//}

//$("#barchart").change(function () {
//    var id = $(this).val();
//    if (id == 1) {
//        $("#container-pie").addClass("hidden");
//        $("#container-pie").removeClass("active");
//        $("#container-bar").addClass("active");
//        $("#container-bar").removeClass("hidden");

//        $("#table_pie").addClass("hidden");
//        $("#table_pie").removeClass("active");
//        $("#table_prounit").addClass("active");
//        $("#table_prounit").removeClass("hidden");
//        gettable();
//    }
//    else {
//        $("#container-bar").addClass("hidden");
//        $("#container-bar").removeClass("active");
//        $("#container-pie").addClass("active");
//        $("#container-pie").removeClass("hidden");

//        $("#table_pie").addClass("active");
//        $("#table_pie").removeClass("hidden");
//        $("#table_prounit").addClass("hidden");
//        $("#table_prounit").removeClass("active");
//        gettable2();

//    }
//});

//$("#searchBtn").click(function () {
//    var end = $('#end').val().trim();
//    var start = $('#start').val().trim();

//    var id = $('#barchart').val().trim();


//    var process = $('#c_prounit_cd').val().trim();


//    if (id == 1 && (start == "" && end == "" && process == "")) {
//        $.ajax({
//            url: "/DashBoardQC/searchProcessQcchart",
//            type: "get",
//            datatype: "json",
//            data: {
//                start: start,
//                process: process,
//                end: end,
//            },
//            success: function (result) {
//                console.log(result);
//                if (result.length == 0) {
//                    $("#table_pie").addClass("hidden");
//                    $("#table_pie").removeClass("active");
//                    $("#table_prounit").addClass("hidden");
//                    $("#table_prounit").removeClass("active");
//                } else {
//                    var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//                    var giu_lieu = result.length;
//                    var cars = [];
//                    var reg = 0;
//                    cars.push(reg);
//                    $.each(result, function (key, item) {
//                        var reg_moi = 0;

//                        if (reg < giu_lieu) {
//                            if (cars == 0) {
//                                var reg_moi = 0;
//                                cars.push(reg_moi);
//                            }
//                            else {
//                                var ds = cars.length - 1;
//                                var reg_moi = cars[ds] + 4;
//                                if (reg_moi < giu_lieu && key == reg_moi) {
//                                    cars.push(reg_moi);
//                                }
//                            }

//                            if (reg_moi == key) {
//                                html += '<tr>';
//                            }
//                            html += '<td ><canvas id="bar-chart-grouped-' + key + '" style="width : 350px" height="200"></canvas> </td>';

//                        }
//                    });
//                    html += '</table>';
//                    $("#table_prounit").html(html);
//                    $("#table_prounit").addClass("active");
//                    $("#table_prounit").removeClass("hidden");



//                    $.each(result, function (key, item) {
//                        var ajax = $.ajax({
//                            url: "/DashBoardQC/getcolum_proqc?fq_no=" + item.fq_no,
//                            type: "get",
//                            success: function (rtnData) {
//                                var list1 = [],
//                                 list2 = [];
//                                console.log(rtnData);
//                                $.each(rtnData, function (dataType, data) {
//                                    list1.push(data.qc_qty);
//                                    list2.push(data.check_value);

//                                });
//                                console.log(list1);
//                                console.log(list2);

//                                var ctx = document.getElementById("bar-chart-grouped-" + key + "").getContext("2d")
//                                config = {
//                                    type: "bar",

//                                    data: {

//                                        labels: list2,
//                                        datasets: [{
//                                            label: item.check_subject,
//                                            data: list1,
//                                            backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                            borderWidth: 1,
//                                            fill: false,
//                                        },
//                                        ],
//                                    },
//                                    options: {
//                                        legend: {
//                                            display: true,
//                                            labels: {
//                                                boxWidth: 0
//                                            }
//                                        },
//                                        title: {
//                                            display: true,
//                                            width: 200,
//                                            position: 'bottom',
//                                            text: item.process
//                                        },
//                                        scales: {
//                                            yAxes: [{
//                                                ticks: {
//                                                    beginAtZero: true
//                                                }
//                                            }]
//                                        },
//                                        tooltips: {
//                                            enabled: false,
//                                        },
//                                        plugins: {
//                                            datalabels: {
//                                                display: true,
//                                                align: 'center',
//                                                anchor: 'center',
//                                                //color: '#36A2EB'
//                                                //align: 'top',
//                                                //fontColor: 'red',
//                                                formatter: function (value, context) {
//                                                    console.log(value);
//                                                    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                                },
//                                                font: {
//                                                    weight: 'bold',
//                                                    //color: 'red',
//                                                },
//                                                color: 'black',
//                                                //labels: {
//                                                //    value: {},
//                                                //    title: {
//                                                //        color: 'blue'
//                                                //    }
//                                                //}
//                                                //formatter: function (value, context) {
//                                                //    return GetValueFormatted(value);
//                                                //}
//                                            }
//                                        }
//                                    }
//                                };

//                                window.myBar = new Chart(ctx, config);
//                            }

//                        });
//                    });
//                }
//            }
//        });
//    }

//    else if (id != 1 && (start == "" && end == "" && process == "")) {
//        $.ajax({
//            url: "/DashBoardQC/searchProcessQcchart",
//            type: "get",
//            datatype: "json",
//            data: {
//                start: start,
//                process: process,
//                end: end,
//            },
//            success: function (result) {
//                console.log(result);
//                if (result.length == 0) {
//                    $("#table_pie").addClass("hidden");
//                    $("#table_pie").removeClass("active");
//                    $("#table_prounit").addClass("hidden");
//                    $("#table_prounit").removeClass("active");
//                } else {
//                    var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//                    var giu_lieu = result.length;
//                    var cars = [];
//                    var reg = 0;
//                    cars.push(reg);
//                    $.each(result, function (key, item) {
//                        var reg_moi = 0;

//                        if (reg < giu_lieu) {
//                            if (cars == 0) {
//                                var reg_moi = 0;
//                                cars.push(reg_moi);
//                            }
//                            else {
//                                var ds = cars.length - 1;
//                                var reg_moi = cars[ds] + 4;
//                                if (reg_moi < giu_lieu && key == reg_moi) {
//                                    cars.push(reg_moi);
//                                }
//                            }

//                            if (reg_moi == key) {
//                                html += '<tr>';
//                            }


//                            html += '<td ><canvas id="pie-chart-grouped-' + key + '"  style="width : 350px" height="200"></canvas> </td>';
//                        }
//                    });
//                    html += '</table>';
//                    $("#table_pie").html(html);


//                    $("#table_pie").addClass("active");
//                    $("#table_pie").removeClass("hidden");
//                    $.each(result, function (key, item) {
//                        var ajax = $.ajax({
//                            url: "/DashBoardQC/getcolum_proqc2?fq_no=" + item.fq_no,
//                            type: "get",
//                            success: function (rtnData) {
//                                var list1 = [],
//                                                    list2 = [];
//                                console.log(rtnData);
//                                $.each(rtnData, function (dataType, data) {
//                                    list1.push(data.qc_qty);
//                                    list2.push(data.check_value);

//                                });
//                                new Chart(document.getElementById("pie-chart-grouped-" + key + ""),
//                                {
//                                    type: 'pie',
//                                    labels: "TEST",
//                                    data: {
//                                        datasets: [
//                                        {
//                                            backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                            data: list1,
//                                        }

//                                        ],

//                                        labels: list2,
//                                    },

//                                    options: {
//                                        title: {
//                                            position: 'bottom',
//                                            display: true,
//                                            text: item.process
//                                        },
//                                        legend: {
//                                            display: true
//                                        },
//                                        animation: {

//                                            animateRotate: false,

//                                        },

//                                        tooltips: {
//                                            enabled: true,
//                                        },
//                                        plugins: {
//                                            datalabels: {
//                                                display: true,
//                                                align: 'center',
//                                                anchor: 'center',
//                                                //color: '#36A2EB'
//                                                //align: 'top',
//                                                //fontColor: 'red',
//                                                formatter: function (value, context) {
//                                                    console.log(value);
//                                                    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                                },
//                                                font: {
//                                                    weight: 'bold',
//                                                    //color: 'red',
//                                                },
//                                                color: 'black',
//                                                //labels: {
//                                                //    value: {},
//                                                //    title: {
//                                                //        color: 'blue'
//                                                //    }
//                                                //}
//                                                //formatter: function (value, context) {
//                                                //    return GetValueFormatted(value);
//                                                //}
//                                            }
//                                        }
//                                    }
//                                });
//                            }
//                        });
//                    })
//                }
//            }
//        });
//    }
//    else if (id != 1 && (start != "" || end != "" || process != "")) {

//        $.ajax({
//            url: "/DashBoardQC/searchProcessQcchart",
//            type: "get",
//            datatype: "json",
//            data: {
//                start: start,
//                process: process,
//                end: end,
//            },
//            success: function (result) {
//                console.log(result);
//                if (result.length == 0) {
//                    $("#table_pie").addClass("hidden");
//                    $("#table_pie").removeClass("active");
//                    $("#table_prounit").addClass("hidden");
//                    $("#table_prounit").removeClass("active");
//                } else {
//                    var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//                    var giu_lieu = result.length;
//                    var cars = [];
//                    var reg = 0;
//                    cars.push(reg);
//                    $.each(result, function (key, item) {
//                        var reg_moi = 0;

//                        if (reg < giu_lieu) {
//                            if (cars == 0) {
//                                var reg_moi = 0;
//                                cars.push(reg_moi);
//                            }
//                            else {
//                                var ds = cars.length - 1;
//                                var reg_moi = cars[ds] + 4;
//                                if (reg_moi < giu_lieu && key == reg_moi) {
//                                    cars.push(reg_moi);
//                                }
//                            }

//                            if (reg_moi == key) {
//                                html += '<tr>';
//                            }


//                            html += '<td ><canvas id="pie-chart-grouped-' + key + '"  style="width : 350px" height="200"></canvas> </td>';
//                        }
//                    });
//                    html += '</table>';
//                    $("#table_pie").html(html);


//                    $("#table_pie").addClass("active");
//                    $("#table_pie").removeClass("hidden");
//                    $.each(result, function (key, item) {
//                        var ajax = $.ajax({
//                            url: "/DashBoardQC/getcolum_proqc2?fq_no=" + item.fq_no,
//                            type: "get",
//                            success: function (rtnData) {
//                                var list1 = [],
//                                                    list2 = [];
//                                console.log(rtnData);
//                                $.each(rtnData, function (dataType, data) {
//                                    list1.push(data.qc_qty);
//                                    list2.push(data.check_value);

//                                });
//                                new Chart(document.getElementById("pie-chart-grouped-" + key + ""),
//                                {
//                                    type: 'pie',
//                                    labels: "TEST",
//                                    data: {
//                                        datasets: [
//                                        {
//                                            backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                            data: list1,
//                                        }

//                                        ],

//                                        labels: list2,
//                                    },

//                                    options: {
//                                        title: {
//                                            position: 'bottom',
//                                            display: true,
//                                            text: item.process
//                                        },
//                                        legend: {
//                                            display: true
//                                        },
//                                        animation: {

//                                            animateRotate: false,

//                                        },

//                                        tooltips: {
//                                            enabled: true,
//                                        },
//                                        plugins: {
//                                            datalabels: {
//                                                display: true,
//                                                align: 'center',
//                                                anchor: 'center',
//                                                //color: '#36A2EB'
//                                                //align: 'top',
//                                                //fontColor: 'red',
//                                                formatter: function (value, context) {
//                                                    console.log(value);
//                                                    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                                },
//                                                font: {
//                                                    weight: 'bold',
//                                                    //color: 'red',
//                                                },
//                                                color: 'black',
//                                                //labels: {
//                                                //    value: {},
//                                                //    title: {
//                                                //        color: 'blue'
//                                                //    }
//                                                //}
//                                                //formatter: function (value, context) {
//                                                //    return GetValueFormatted(value);
//                                                //}
//                                            }
//                                        }
//                                    }
//                                });
//                            }
//                        });
//                    })
//                }
//            }
//        });


//    }
//    else if (id == 1 && (start != "" || end != "" || process != "")) {


//        $.ajax({
//            url: "/DashBoardQC/searchProcessQcchart",
//            type: "get",
//            datatype: "json",
//            data: {
//                start: start,
//                process: process,
//                end: end,
//            },
//            success: function (result) {
//                console.log(result);
//                if (result.length == 0) {
//                    $("#table_pie").addClass("hidden");
//                    $("#table_pie").removeClass("active");
//                    $("#table_prounit").addClass("hidden");
//                    $("#table_prounit").removeClass("active");
//                } else {
//                    var html = '<table width="10%" align = "right" bordercolor = "black" border = "1" cellspacing = "10px">';
//                    var giu_lieu = result.length;
//                    var cars = [];
//                    var reg = 0;
//                    cars.push(reg);
//                    $.each(result, function (key, item) {
//                        var reg_moi = 0;

//                        if (reg < giu_lieu) {
//                            if (cars == 0) {
//                                var reg_moi = 0;
//                                cars.push(reg_moi);
//                            }
//                            else {
//                                var ds = cars.length - 1;
//                                var reg_moi = cars[ds] + 4;
//                                if (reg_moi < giu_lieu && key == reg_moi) {
//                                    cars.push(reg_moi);
//                                }
//                            }

//                            if (reg_moi == key) {
//                                html += '<tr>';
//                            }
//                            html += '<td ><canvas id="bar-chart-grouped-' + key + '" style="width : 350px" height="200"></canvas> </td>';

//                        }
//                    });
//                    html += '</table>';
//                    $("#table_prounit").html(html);
//                    $("#table_prounit").addClass("active");
//                    $("#table_prounit").removeClass("hidden");



//                    $.each(result, function (key, item) {
//                        var ajax = $.ajax({
//                            url: "/DashBoardQC/getcolum_proqc?fq_no=" + item.fq_no,
//                            type: "get",
//                            success: function (rtnData) {
//                                var list1 = [],
//                                 list2 = [];
//                                console.log(rtnData);
//                                $.each(rtnData, function (dataType, data) {
//                                    list1.push(data.qc_qty);
//                                    list2.push(data.check_value);

//                                });
//                                console.log(list1);
//                                console.log(list2);

//                                var ctx = document.getElementById("bar-chart-grouped-" + key + "").getContext("2d")
//                                config = {
//                                    type: "bar",

//                                    data: {

//                                        labels: list2,
//                                        datasets: [{
//                                            label: item.check_subject,
//                                            data: list1,
//                                            backgroundColor: ['aqua', 'blue', 'fuchsia', 'gray', 'green', 'lime', 'maroon', 'navy', 'olive', 'orange', 'purple', 'red', 'silver', 'teal', 'white', 'yellow'],
//                                            borderWidth: 1,
//                                            fill: false,
//                                        },
//                                        ],
//                                    },
//                                    options: {
//                                        legend: {
//                                            display: true,
//                                            labels: {
//                                                boxWidth: 0
//                                            }
//                                        },
//                                        title: {
//                                            display: true,
//                                            width: 200,
//                                            position: 'bottom',
//                                            text: item.process
//                                        },
//                                        scales: {
//                                            yAxes: [{
//                                                ticks: {
//                                                    beginAtZero: true
//                                                }
//                                            }]
//                                        },
//                                        tooltips: {
//                                            enabled: false,
//                                        },
//                                        plugins: {
//                                            datalabels: {
//                                                display: true,
//                                                align: 'center',
//                                                anchor: 'center',
//                                                //color: '#36A2EB'
//                                                //align: 'top',
//                                                //fontColor: 'red',
//                                                formatter: function (value, context) {
//                                                    console.log(value);
//                                                    return value.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
//                                                },
//                                                font: {
//                                                    weight: 'bold',
//                                                    //color: 'red',
//                                                },
//                                                color: 'black',
//                                                //labels: {
//                                                //    value: {},
//                                                //    title: {
//                                                //        color: 'blue'
//                                                //    }
//                                                //}
//                                                //formatter: function (value, context) {
//                                                //    return GetValueFormatted(value);
//                                                //}
//                                            }
//                                        }
//                                    }
//                                };

//                                window.myBar = new Chart(ctx, config);
//                            }

//                        });
//                    });
//                }
//            }
//        });
//    }
//});


var getType = "/DashBoardQC/getProcess";
//Create
$(document).ready(function () {
    _getTypeC();

});
function _getTypeC() {

    $.get(getType, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected"></option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.process_nm + '>' + item.process_nm + '</option>';
            });
            $("#c_prounit_cd").html(html);
        }
    });
}
