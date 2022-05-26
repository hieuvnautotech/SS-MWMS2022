$(".dialogworkorder").dialog({
    width: '70%',
    height: 900,
    maxWidth: 1000,
    maxHeight: 900,
    minWidth: '70%',
    minHeight: 900,
    zIndex: 1000,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        //"ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        //"ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {
    },
});


$('#closeinedetail').click(function () {
    $('.dialogworkorder').dialog('close');
    jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
});

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".actual").dialog({
    width: '70%',
    height: 1100,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
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


$('#closeactual').click(function () {
    $('.actual').dialog('close');
});

$("#Print_actual").on("click", function () {
    var id = $("#pp_fno").val();

    window.open("/ActualWO/Printdetail_actual?" + "id=" + id, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".plan").dialog({
    width: '70%',
    height: 1100,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
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


$('#closeplan').click(function () {
    $('.plan').dialog('close');
});

$("#Print_plan").on("click", function () {
    var id = $("#pp_fno1").val();
    window.open("/ActualWO/Printdetail_plan?" + "id=" + id, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".dialogStandby").dialog({
    width: '50%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
       // "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {
    },
});


$('#CloseStandby').click(function () {
    $('.dialogStandby').dialog('close');

    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
});

// Khai báo URL stand của bạn ở đây
var GetCheck = "/ActualWO/GetCheckMaterial";

//check material - MMS006
$(document).ready(function () {
    _GetCheck();
    $("#checkMaterial").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});
function _GetCheck() {

    $.get(GetCheck, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#checkMaterial").html(html);
            $("#checkmechin").html(html);
            $("#checksaff").html(html);
        }
    });
}
//$("#SaveStandby").click(function () {
//    var checkMaterial = $('#checkMaterial').val();
//    checkmechin = $("#checkmechin").val(),
//    checksaff = $("#checksaff").val(),

//    olddno =   $("#olddno_day").val();

//    $.ajax({
//        url: "/ActualWO/ModifyLineCheckday",
//        type: "get",
//        dataType: "json",
//        data: {
//            olddno : olddno,
//            checkMaterial: checkMaterial,
//            checkmechin: checkmechin,
//            checksaff: checksaff,
//        },
//        success: function (result) {
//            alert("Save Success!!!");
          
//        },
//        error: function (result) {
//            alert("error");
//        }
//    });
//});