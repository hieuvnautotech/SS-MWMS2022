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
            url: "/DevManagement/deleteMachineMgt",
            type: "Delete",
            dataType: "json",
            data: {
                mno: $("#m_mno").val(),
            },
            success: function (data) {
                if (data.result == true) {
                    jQuery("#MachineMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            },
            error: function (result) { }
        });
        $('#dialogDangerous').dialog('close');
    });

    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });


