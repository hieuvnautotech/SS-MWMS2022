$(function () {
    $(".dialog").dialog({

        height: 500,
        width: 800,
        resizable: true,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",

        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
    });


    $('#closestyle').click(function () {
        $('.dialog').dialog('close');
    });
});