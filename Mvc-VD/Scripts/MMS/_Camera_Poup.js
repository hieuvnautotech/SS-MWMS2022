$(".dialog_main").dialog({
    width: 750,
    height: 450,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    resizable: true,
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

        var webrtcPlayer_camera_01 = null;
        webrtcPlayer_camera_01 = new UnrealWebRTCPlayer("camera_01", "camera_01", "", "180.93.223.114", "5119", false, true, "tcp");
        webrtcPlayer_camera_01.Play();
        //$('#main_cam').html('MAIN');

    },
});

$('#button_3').click(function () {
    $('.dialog_main').dialog('open');
});

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------

$(".dialog_top").dialog({
    width: 750,
    height: 450,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    resizable: true,
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

        var webrtcPlayer_camera_02 = null;
        webrtcPlayer_camera_02 = new UnrealWebRTCPlayer("camera_02", "camera_02", "", "180.93.223.114", "5119", false, true, "tcp");
        webrtcPlayer_camera_02.Play();

    },
});

$('#button_2').click(function () {
    $('.dialog_top').dialog('open');
});

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------

$(".dialog_front").dialog({
    width: 750,
    height: 450,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    resizable: true,
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

        var webrtcPlayer_camera_02 = null;
        webrtcPlayer_camera_02 = new UnrealWebRTCPlayer("camera_03", "camera_03", "", "180.93.223.114", "5119", false, true, "tcp");
        webrtcPlayer_camera_02.Play();

    },
});

$('#button_1').click(function () {
    $('.dialog_front').dialog('open');
});

//---------------------------------------------------------------------------------------------------------------------------------------------------------------------

$(document).ready(function () {
    function checkTime(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }
    setInterval(function () {
        var d = new Date();
        var s = d.getSeconds();
        var m = d.getMinutes();
        var h = d.getHours();
        var year = d.getFullYear();
        var month = d.getMonth();
        var date = d.getDate();

        s = checkTime(s);
        m = checkTime(m);
        h = checkTime(h);
        year = checkTime(year);
        month = checkTime(month + 1);
        date = checkTime(date);
        haftday = (h > 12) ? "PM" : "AM";
        $('#daystatus').html(year + '/' + month + '/' + date);
        $('#timestatus').html(haftday + ' : ' + h + ' : ' + m + ' : ' + s);
    }, 1000);
});