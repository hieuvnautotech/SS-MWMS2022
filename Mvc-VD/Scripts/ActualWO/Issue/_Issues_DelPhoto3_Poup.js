$("#dialogDangerousfoto3").dialog({
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

$("#deletestyle6").click(function () {
    $.ajax({
        url: "/ActualWO/deletePhoto",
        type: "post",
        dataType: "json",
        data: {
            id: $('#pi_m3').val(),

        },
        success: function (result) {
            if (result != 0) {
                $('#pi_m3').val(" ");

                document.getElementById("customPhoto3_m").value = "";
                $('p[id="customPhoto_n3"]').text('');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });

    $('#dialogDangerousfoto3').dialog('close');

    //$(".info2").empty();
    $('#photo_item').slick('refresh');
    //$('.info2').slick('unslick');

});

$('#closestyle6').click(function () {
    $('#dialogDangerousfoto3').dialog('close');
    $('#photo_item').slick('refresh');
});

$('#btndel_photo_save_but3').click(function () {
    $('#dialogDangerousfoto3').dialog('open');
});