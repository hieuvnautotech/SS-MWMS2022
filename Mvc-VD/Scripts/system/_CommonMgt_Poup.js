$("#dialogDangerous_cm").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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

$("#deletestyle").click(function () {
    $.ajax({
        url: "/system/deleteCommon",
        type: "post",
        dataType: "json",
        data: {
            commonmaincode: $('#m_mt_cd').val(),
        },
        success: function (result) {
            if (result != 0) {
                jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous_cm').dialog('close');
});
    
    

$('#closestyle').click(function () {
    $('#dialogDangerous_cm').dialog('close');
});

$("#dialogDangerous_dt").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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

$("#deletestyle_dt").click(function () {
    $.ajax({
        url: "/system/deleteCommon_dt",
        type: "post",
        dataType: "json",
        data: {
            commondt_manincode: $('#dm_mt_cd').val(),
            commondt_dtcd: $('#dm_dt_cd').val(),
        },
        success: function (result) {
            if (result != 0) {
                jQuery("#commondtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous_dt').dialog('close');
});



$('#closestyle_dt').click(function () {
    $('#dialogDangerous_dt').dialog('close');
});
