$("#dialogDangerousphoto").dialog({
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

$("#deletestylephoto").click(function () {
    $.ajax({
        url: "/ActualWO/deletePhoto",
        type: "post",
        dataType: "json",
        data: {
            id: $('#pi_m').val(),

        },
        success: function (data) {
            console.log(data.result);

            if (data.result == true) {
                $('#pi_m').val(" ");
                jQuery("#issueGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $('.custom-file-label').each(function () {
                    if ($(this).text() == data.photoname) {
                        $(this).text('');
                    }
                });
                $('#photo_item').slick('refresh');
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerousphoto').dialog('close');
    
});

$('#closestylephoto').click(function () {
    $('#photo_item').slick('refresh');
    $('#dialogDangerousphoto').dialog('close');
});

$('.btndel_photo_save_but').click(function () {
    $('#dialogDangerousphoto').dialog('open');
});