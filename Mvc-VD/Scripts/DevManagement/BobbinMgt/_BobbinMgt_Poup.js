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
    var sts = $("#sts").val();
    switch (sts) {
        case "xa_bb":
            $.ajax({
                url: "/ActualWO/DestroyBobbinMgt",
                type: "Get",
                dataType: "json",
                data: {
                    bobin: $("#bobin").val(),
                    mt_cd: $("#mt_cd").val(),
                },
                success: function (responsie) {
                    if (responsie.result) {
                        SuccessAlert(responsie.message);
                        var id = $("#id_xa").val();
                        var rowData = $('#BobbinMgtGrid').jqGrid('getRowData', id);
                        rowData.mt_cd = null;
                        $('#BobbinMgtGrid').jqGrid('setRowData', id, rowData);
                        $("#BobbinMgtGrid").setRowData(id_dv, false, { background: "#d0e9c6" });

                    } else {
                        ErrorAlert(responsie.message);
                    }
                },
                error: function (result) { }
            });
            break;
        default:
            $.ajax({
                url: "/DevManagement/deleteBobbinMgt",
                type: "Delete",
                dataType: "json",
                data: {
                    bno: $("#m_bno").val(),
                },
                success: function (data) {
                    if (data.result == true) {
                        jQuery("#BobbinMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                },
                error: function (result) { }
            });
            break;
    }

        $('#dialogDangerous').dialog('close');
    });
    
    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });

    $('#del_save_but').click(function () {
        $('#dialogDangerous').dialog('open');
    });
