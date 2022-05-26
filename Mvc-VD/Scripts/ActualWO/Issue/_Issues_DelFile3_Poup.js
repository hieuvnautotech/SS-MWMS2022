$("#dialogDangerousfile3").dialog({
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

$("#deletestyle3").click(function () {
    $.ajax({
        url: "/ActualWO/deleteFile",
        type: "post",
        dataType: "json",
        data: {
            id: $('#fi_m3').val(),

        },
        success: function (result) {
            if (result != 0) {
                $('#fi_m3').val(" ");

                document.getElementById("customFile3_m").value = "";
                $('p[id="customFile_n3"]').text('');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerousfile3').dialog('close');
});

$('#closestyle3').click(function () {
    $('#dialogDangerousfile3').dialog('close');
});

$('#btndel_file_save_but3').click(function () {
    $('#dialogDangerousfile3').dialog('open');
});