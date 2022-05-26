$("#dialogDangerousfoto1").dialog({
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

$("#deletestyle4").click(function () {
    $.ajax({
        url: "/ActualWO/deletePhoto",
        type: "post",
        dataType: "json",
        data: {
            id: $('#pi_m').val(),

        },
        success: function (result) {
            if (result != 0) {
                $('#pi_m').val(" ");

                document.getElementById("customPhoto_m").value = "";
                $('p[id="customPhoto_n"]').text('');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerousfoto1').dialog('close');
    $('#photo_item').slick('refresh');
});

$('#closestyle4').click(function () {
    $('#photo_item').slick('refresh');
    $('#dialogDangerousfoto1').dialog('close');
});

$('#btndel_photo_save_but').click(function () {
    $('#dialogDangerousfoto1').dialog('open');
});