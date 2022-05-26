
$(".dialoglinedetail").dialog({

    width: '75%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
    resizable: false,
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

$('#closestyle_linedetail').click(function () {
    $('.dialoglinedetail').dialog('close');
});

