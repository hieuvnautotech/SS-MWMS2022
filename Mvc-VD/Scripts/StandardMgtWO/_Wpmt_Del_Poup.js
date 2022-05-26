$("#dialogDangerous2").dialog({
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

$("#deletestyle_pm").click(function () {
    $.ajax({
        url: "/StandardMgtWO/deletetWorkPolicyInfo",
        type: "post",
        dataType: "json",
        data: {
            id: $('#m_wid').val(),

        },
        success: function (result) {
            if (result != 0) {
                $("#wpmtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                $("#tab_1").addClass("active");
                $("#tab_2").removeClass("active");
                $("#tab_c1").addClass("active");
                $("#tab_c1").removeClass("hidden");
                $("#tab_c2").addClass("hidden");
                $("#tab_c2").removeClass("active");
                document.getElementById("form1").reset();
                document.getElementById("form2").reset();
            }
            else {
                alert('User was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous2').dialog('close');
});

$('#closestyle_pm').click(function () {
    $('#dialogDangerous2').dialog('close');
});

$('#del_save_but').click(function () {
    $('#dialogDangerous2').dialog('open');
});