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
            url: "/system/deleteDept",
            type: "post",
            dataType: "json",
            data: {
                m_depart_cd: $('#m_depart_cd').val(),
            },
            success: function (result) {
                if (result != 0) {
                    jQuery("#DeptGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                }
                else {
                    alert('User was not existing. Please check again');
                }

            },
            error: function (result) { }
        });
        $('#dialogDangerous').dialog('close');
    });
    
    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });


    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });

});