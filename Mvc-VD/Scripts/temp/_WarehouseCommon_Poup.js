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
        url: "/StandardMgtWh/deleteWHSCommon",
        type: "post",
        dataType: "json",
        data: {
            mt_cd: $('#m_mt_cd').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#WHSCommonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                jQuery("#WHSCommonDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('Warehouse was not existing. Please check again');
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
        url: "/StandardMgtWh/deleteWHSCommonDt",
        type: "post",
        dataType: "json",
        data: {
            mt_cd: $('#m_mt_cd').val(),
            dm_dt_cd: $('#dm_dt_cd').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#WHSCommonDtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('Warehouse Detail was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous_dt').dialog('close');
});



$('#closestyle_dt').click(function () {
    $('#dialogDangerous_dt').dialog('close');
});
