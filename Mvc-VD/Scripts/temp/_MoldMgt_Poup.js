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
            url: "/DevManagement/deleteMold",
            type: "post",
            dataType: "json",
            data: {
                m_md_no: $("#m_md_no").val(),
            },
            success: function (result) {
                    jQuery("#Mold").setGridParam({ datatype: "json" }).trigger('reloadGrid');
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


