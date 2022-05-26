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
            case "del_save_but":
                $.ajax({
                    url: "/DevManagement/Deletebom",
                    type: "post",
                    dataType: "json",
                    data: {
                        bid: $('#m_bid').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                            $("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            $("#list2").clearGridData();
                            $("#list3").clearGridData();
                        }
                        else {
                            alert('Bom part was not existing. Please check again');
                        }

                    },
                    error: function (result) { }
                });
                $('#dialogDangerous').dialog('close');
                break;
            case "del_part_save_but":
                $.ajax({
                    url: "/DevManagement/Deletebom_part",
                    type: "post",
                    dataType: "json",
                    data: {
                        bpid: $('#m_bpid').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                            $("#list2").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            $("#list3").clearGridData();
                        }
                        else {
                            alert('Bom part was not existing. Please check again');
                        }

                    },
                    error: function (result) { }
                });
                $('#dialogDangerous').dialog('close');
                break;
            case "del_bomdt_save_but":
                $.ajax({
                    url: "/DevManagement/Deletebom_detail",
                    type: "post",
                    dataType: "json",
                    data: {
                        bdid: $('#m_bdid').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                            $("#list3").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        }
                        else {
                            alert('Bom detail was not existing. Please check again');
                        }

                    },
                    error: function (result) { }
                });
                $('#dialogDangerous').dialog('close');
                break;
            case "del_vltt":
                $.ajax({
                    url: "/DevManagement/Deletevltt",
                    type: "post",
                    dataType: "json",
                    data: {
                        bdidr: $('#id_vltt').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                           var  bdidr=$('#id_vltt').val();
                           $("#popupMaterial_tt").jqGrid('delRowData', bdidr);
                           $("#list3").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                        }
                        else {
                            alert('The Mt No Exits in Bom. Please check again');
                        }

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
        switch ($(this).attr("id"))
        {
            case "del_save_but":
                $("#status_delete").val("del_save_but");
                break;
            case "del_part_save_but":
                $("#status_delete").val("del_part_save_but");
                break;
            case "del_bomdt_save_but":
                $("#status_delete").val("del_bomdt_save_but");
                break;
            case "del_vltt":
                $("#status_delete").val("del_vltt");
                break;
        }
        $('#dialogDangerous').dialog('open');
    });
});