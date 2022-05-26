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
            url: "/DevManagement/reset_line_next_process",
            type: "post",
            dataType: "json",
            data: {
                line_no: $('#m_process_no').val(),
            },
            success: function (data) {
                if (data.result == 1) {
                    jQuery("#list4").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                    var id = $('#m_process_no').val();
                    if (id != undefined && id != '') {
                        _GetPartcode(id);
                        _GetBefor(id);
                    }
                    refesh_graph();
                }
            },
            error: function (data) { }
        });
        $('#dialogDangerous').dialog('close');
    });
    
    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });

    $('#c4_reset_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });
