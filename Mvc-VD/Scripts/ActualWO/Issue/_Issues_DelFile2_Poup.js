$("#dialogDangerousfile2").dialog({
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

$("#deletestyle2").click(function () {
    $.ajax({
        url: "/ActualWO/deleteFile",
        type: "post",
        dataType: "json",
        data: {
            id: $('#fi_m2').val(),

        },
        success: function (result) {
            if (result != 0) {
                $('#fi_m2').val(" ");

                document.getElementById("customFile2_m").value = "";
                $('p[id="customFile_n2"]').text('');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerousfile2').dialog('close');
});

$('#closestyle2').click(function () {
    $('#dialogDangerousfile2').dialog('close');
});

$('#btndel_file_save_but2').click(function () {
    $('#dialogDangerousfile2').dialog('open');
});