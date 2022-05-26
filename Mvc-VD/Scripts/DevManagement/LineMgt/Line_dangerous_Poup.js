$(function () {
    $("#dialogDangerous_del").dialog({
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

    $("#deletestyle_del").click(function () {
        switch ($("#status_delete").val())
        {
            case "del_save_but":
                $.ajax({
                    url: "/DevManagement/Deleteline",
                    type: "post",
                    dataType: "json",
                    data: {
                        lid: $('#m_pid').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                            $("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            $("#list2").clearGridData();
                            $("#list4").clearGridData();
                            refesh_graph();
                        }
                        else {
                            alert('Line was not existing. Please check again');
                        }

                    },
                    error: function (result) { }
                });
                $('#dialogDangerous_del').dialog('close');
                break;
            case "del_part_save_but":
                $.ajax({
                    url: "/DevManagement/Deleteline_process",
                    type: "post",
                    dataType: "json",
                    data: {
                        dlpid: $("#m_ppid").val(),
                    },
                    success: function (response) {
                        if (response.result) {
                            //var id = data.dlpid;
                            //$("#list2").setRowData(id, data, { background: "#d0e9c6" });
                            $("#list2").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                           
                        }
                        else {
                            alert('Line Process was not existing. Please check again');
                        }
                       
                    },
                    error: function (data) { alert('Line Process was not existing. Please check again'); }
                });
                $('#dialogDangerous_del').dialog('close');
                break;
            case "del_bomdt_save_but":
                $.ajax({
                    url: "/DevManagement/Deleteline_process_seguence",
                    type: "post",
                    dataType: "json",
                    data: {
                        lpid: $('#m_pbid').val(),
                    },
                    success: function (data) {
                        if (data.result == 1) {
                            $("#list4").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                            var id = $('#m_process_no').val();
                            if (id != undefined && id != '') {
                                _GetPartcode(id);
                                _GetBefor(id);
                            }
                            refesh_graph();
                            document.getElementById("form8").reset();
                        
                        }
                        else {
                            alert('Line Process Seguence was not existing. Please check again');
                        }

                    },
                    error: function (result) { }
                });
                $('#dialogDangerous_del').dialog('close');
                break;
            case "del_bd":

                var lpid = $("#id_val_bd").val();
                mydelete_ok(lpid);

            $('#dialogDangerous_del').dialog('close');
                break;
        }      
    });
    
    

    $('#closestyle_del').click(function () {
        $('#dialogDangerous_del').dialog('close');
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
        }
        $('#dialogDangerous_del').dialog('open');
    });
});