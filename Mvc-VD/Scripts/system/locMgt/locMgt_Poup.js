$(function () {
    $("#dialogDangerous").dialog({
        width: '20%',
        height: 100,
        maxWidth: '20%',
        maxHeight: 100,
        minWidth: '20%',
        minHeight: 100,
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
            url: "/system/deletelocMgt",
            type: "post",
            dataType: "json",
            data: {
                lctno: $('#m_lctno').val(),
            },
            success: function (data) {
                if (data.result == 1) {
                    jQuery("#locMgt").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert('Location was not existing. Please check again');
                }

            },
            error: function (result) { }
        });
        $('#dialogDangerous').dialog('close');
    });
    
    

    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });   
});