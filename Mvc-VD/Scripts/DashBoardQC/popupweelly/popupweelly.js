
$(document).ready(function () {
    setTimeout(function () {
        location.reload();
    }, 20000);
    $.get("/DashBoardQC/Weekly_Work_Order", function (html) {
        $("#noidung").html(html);
        var x_fo_no = $("#c_fo_no").val();
        if (x_fo_no != null && x_fo_no != undefined && x_fo_no != "") {
            $("#fo_no").val(x_fo_no);
        }
    });
    $("#dssdsdsd").click(function () {
        $('#dialog_wl').dialog('open');
        $.get("/DashBoardQC/Weekly_Work_Order", function (html) {
            $("#noi_dung").html(html);
        });
    });
    lOAD_AGAIN_DATA(60);
    var x_fo_no = $("#c_fo_no").val();
    if (x_fo_no != null && x_fo_no != undefined && x_fo_no != "") {
        $("#fo_no").val(x_fo_no);
    }
});

function lOAD_AGAIN_DATA(time_md) {
    if (time_md > 0) {
        time_md--;
        setTimeout("lOAD_AGAIN_DATA(" + time_md + ")", 1000);
    } else {
        _getdata();
    }
}
function _getdata(prounit_cd, id) {
    $.get("/DashBoardQC/Weekly_Work_Order", function (html) {
        $("#noi_dung").html(html);
        $("#noidung").html(html);
    });
    lOAD_AGAIN_DATA(60);
}
$(function () {
    $("#dialog_wl").dialog({
        width: '100%',
        height: 900,
        maxWidth: '100%',
        maxHeight: '100%',
        minWidth: '100%',
        minHeight: '100%',
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
        open: function (event, ui) {

        },
    });

    $('#close').click(function () {
        $('#dialog_wl').dialog('close');
    });

});