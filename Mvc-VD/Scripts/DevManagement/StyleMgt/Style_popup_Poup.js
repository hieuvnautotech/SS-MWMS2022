$("#dialogStyle").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 200,
    minWidth: 350,
    minHeight: 200,
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

$("#del_save_but").click(function () {
    $('#dialogStyle').dialog('open');
});

$('#closesdetele').click(function () {
    $('#dialogStyle').dialog('close');
});

$("#del_save_but").click(function () {
    $.ajax({
        url: "/DevManagement/deleteStyle",
        type: "Delete",
        dataType: "json",
        data: {
            sid: $('#m_sid').val(),
        },
        success: function (data) {
            if (data.result == true) {
                $("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                document.getElementById("form1").reset();
                $("#tab_2").removeClass("active");
                $("#tab_1").addClass("active");
                $("#tab_c2").removeClass("active");
                $("#tab_c1").removeClass("hidden");
                $("#tab_c2").addClass("hidden");
                $("#tab_c1").addClass("active");
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});