$("#dialogDangerousfile").dialog({
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

$("#deletestylefile").click(function () {
    $.ajax({
        url: "/ActualWO/deleteFile",
        type: "post",
        dataType: "json",
        data: {
            id: $('#fi_m').val(),
        },
        success: function (data) {
            if (data.result == true) {
                $('#fi_m').val(" ");
                jQuery("#issueGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $('.custom-file-label').each(function () {
                    if ($(this).text() == data.filename) {
                        $(this).text('');
                    }
                });
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerousfile').dialog('close');
});

$('#closestylefile').click(function () {
    $('#dialogDangerousfile').dialog('close');
});

$('.btndel_file_save_but').click(function () {
    $('#dialogDangerousfile').dialog('open');
});