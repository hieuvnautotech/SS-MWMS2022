$("#dialogDangerousfoto2").dialog({
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

$("#deletestyle5").click(function () {
    $.ajax({
        url: "/ActualWO/deletePhoto",
        type: "post",
        dataType: "json",
        data: {
            id: $('#pi_m2').val(),

        },
        success: function (result) {
            if (result != 0) {
                $('#pi_m2').val(" ");

                document.getElementById("customPhoto2_m").value = "";
                $('p[id="customPhoto_n2"]').text('');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });

    $('#dialogDangerousfoto2').dialog('close');
    //$('.info2').slick('refresh');
    //$(".info2").empty();
    $('#photo_item').slick('refresh');
    //$('.info2').slick('unslick');

});

$('#closestyle5').click(function () {
    $('#dialogDangerousfoto2').dialog('close');
    $('#photo_item').slick('refresh');
});

$('#btndel_photo_save_but2').click(function () {
    $('#dialogDangerousfoto2').dialog('open');
});