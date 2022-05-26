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
        url: "/DevManagement/deleteDevelopCommonMT",
        type: "Delete",
        dataType: "json",
        data: {
            Developcommonmaincode: $('#m_mt_cd').val(),
        },
        success: function (data) {
            if (data.result == true) {
                jQuery("#DevelopCommonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous_cm').dialog('close');
});   

$('#closestyle').click(function () {
    $('#dialogDangerous_cm').dialog('close');
});

$("#del_save_but").click(function () {
    $('#dialogDangerous_cm').dialog('open');
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
        url: "/DevManagement/deleteDevelopCommondt",
        type: "Delete",
        dataType: "json",
        data: {
            Developcommondt_manincode: $('#dm_mt_cd').val(),
            Developcommondt_dtcd: $('#dm_dt_cd').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#DevCommDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous_dt').dialog('close');
});



$('#closestyle_dt').click(function () {
    $('#dialogDangerous_dt').dialog('close');
});

$("#ddel_save_but").click(function () {
    $('#dialogDangerous_dt').dialog('open');
});
