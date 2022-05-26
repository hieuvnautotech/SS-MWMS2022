/// <reference path="jquery-3.4.1.min.js" />

$(function() {
    $(".popup-dialog").dialog({
        width: '70%',
        height: "auto",
        maxWidth: 1000,
        minWidth: '50%',
        minHeight: 450,
        position: ['center', 20],
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden"
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

        },
    });

    $('.popup-dialog-close').click(function (e) {
        $(e).closest(".popup-dialog").dialog('close');
    });
});

//$(document).on({
//    ajaxStart: function () { $("body").addClass("dialog-loading"); },
//    ajaxStop: function () { $("body").removeClass("dialog-loading"); }
//});

function numberFormatterPercent(cellvalue) {
    if (cellvalue) {
        var kg = cellvalue.toString().replace(" %", "");
        if (!kg) {
            kg = 0;
        } else if (kg.toString().includes(".")) {
            kg = parseFloat(cellvalue).toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        } else {
            kg = cellvalue.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        }
        return kg + " %";
    } else {
        return "0 %";
    }
}

$(function () {
    function getDatetimeNow() {
        var d = new Date(),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear(),
            hours = d.getHours(),
            minutes = d.getMinutes();

        if (month.length < 2) { month = '0' + month; }
        if (day.length < 2) { day = '0' + day; }
        if (hours < 10) { hours = '0' + hours; }
        if (minutes < 10) minutes = '0' + minutes;
        var time = hours + ":" + minutes;
        var full = year + "-" + month + "-" + day + " " + time;
        $(".datetime-now").html(full);
    }

    getDatetimeNow();
    setInterval(getDatetimeNow, 500);
});

$(function () {
    function getBarcode() {
        var barcode = $(".barcode");

        for (var i = 0; i < barcode.length; i++) {
            var that = $(barcode[i]);
            that.barcode(that.data("barcode"), "code128", { barWidth: 1, barHeight: 35 });
        }
    }
    setInterval(getBarcode, 500);
});


//$(function() {
//    $('.timepicker').timepicker({
//        timeFormat: 'h:mm',
//        interval: 60,
//        minTime: '10',
//        maxTime: '6:00pm',
//        defaultTime: '11',
//        startTime: '10:00',
//        dynamic: false,
//        dropdown: true,
//        scrollbar: true
//    });
//})