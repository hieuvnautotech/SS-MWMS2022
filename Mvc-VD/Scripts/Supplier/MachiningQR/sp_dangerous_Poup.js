$(function () {
    $("#dialogDangerous").dialog({
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

    $("#deletestyle").click(function () {
        switch ($("#status_delete").val())
        {
            case "del_spdetail":
                $.ajax({
                    url: "/Supplier/DeleteSP",
                    type: "post",
                    dataType: "json",
                    data: {
                        pdid: $('#pdid').val(),
                    },
                    success: function (data) {
                        alert("Success");
                        $("#list2").jqGrid('delRowData', $('#pdid').val());
                        $("#list3").clearGridData();
                    },
                    error: function (result) { }
                });
                $('#dialogDangerous').dialog('close');
                break;
        }      
    });
    
    

    $('#closestyle').click(function () {
        $('#dialogDangerous').dialog('close');
    });

    $('.deletebtn').click(function () {
        $("#status_delete").val("del_spdetail");
        $('#dialogDangerous').dialog('open');
    });
});