$(function () {
    $(".dialogDisplay").dialog({
        width: '50%',
        height: 520,
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
            //"ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });


    $('#CloseDisplay').click(function () {
        $('.dialogDisplay').dialog('close');
        jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    });

});

$("#SaveDisplay").on("click", function () {
    var selRowIdss = $('#list1').jqGrid("getGridParam", "selarrrow");
    var old_no = $("#SaveDisplay").attr("data-old_no");
    var oldno = $("#SaveDisplay").attr("data-oldno");
    var i, n, rowData, selRowIds = $("#popupsDisplay").jqGrid("getGridParam", "selarrrow");
    for (i = 0, n = selRowIds.length; i < n; i++) {

        var v_doing_time = getCellValue(selRowIds[i], "doing_time");
        var v_polyci_code = $("#popupsDisplay").jqGrid('getCell', selRowIds[i], 'policy_code');
        var v_useyn = getCellValue(selRowIds[i], 'use_yn');

        $.ajax({
            type: 'post',
            url: '/ActualWO/ModifyDisplay',
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            dataType: 'text',
            data: JSON.stringify({
                policy_code: v_polyci_code,
                doing_time: v_doing_time,
                use_yn: v_useyn,
                old_no: old_no,
            }),
            async: false,
            cache: false,

        });
    };
    if (oldno > 0) {
        $("#popupsDisplay").setGridParam({ url: "/ActualWO/getDisplay?oldno=" + oldno, datatype: "json" }).trigger("reloadGrid");
    }
    else {
        alert("The old is not to be empty");
        jQuery("#popupsDisplay").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    }
});