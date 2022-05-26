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
    debugger;
    $.ajax({
        url: "/StandardMgtWh/deleteWHSCommon",
        type: "DELETE",
        dataType: "json",
        data: {
            mt_id: $('#m_mt_id').val(),
        },
        success: function (data) {
            if (data.result == true) {
                jQuery("#WHSCommonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                jQuery("#WHSCommonDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
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
        url: "/StandardMgtWh/deleteWHSCommonDt",
        type: "DELETE",
        dataType: "json",
        data: {
            mt_cd: $('#m_mt_cd').val(),
            cdid: $('#dm_cdid').val(),
        },
        success: function (data) {
            if (data.result == true) {
                jQuery("#WHSCommonDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
    $('#dialogDangerous_dt').dialog('close');
});



$('#closestyle_dt').click(function () {
    $('#dialogDangerous_dt').dialog('close');
});
