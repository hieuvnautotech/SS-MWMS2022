$(".dialog_machine").dialog({
    width: '50%',
    height: 500,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        //"ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {

        $("#popupmachine1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'pmid', name: 'pmid', align: 'center', width: 50 },
                { key: false, label: 'Machine', name: 'mc_no', width: 200, align: 'left', },
                { key: false, label: 'Destination', name: 'remark', width: 200, align: 'left', },
                { key: false, label: 'Start Date', name: 'start_dt', width: 200, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'End Date', name: 'end_dt', width: 200, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'Use', name: 'use_yn', editable: true, width: '50', align: 'center', },
            ],

            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupmachine1").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupmachine1").getRowData(selectedRowId);
                $('#m2_mc_type').val(row_id.mc_type);
                $("#pmid").val(row_id.pmid);
                $("#m2_prounit_cd").val(row_id.prounit_cd);
                $("#m_prounit_nm").val(row_id.prounit_nm);
                $("#m_mc_no").val(row_id.mc_no);
                $("#m2_use_yn").val(row_id.use_yn);
                $("#m_start").val(row_id.start_dt);
                $("#m_end").val(row_id.end_dt);
                $("#m2_remark").val(row_id.remark);

                $("#tab_3").removeClass("active");
                $("#tab_4").addClass("active");
                $("#tab_c3").removeClass("active");
                $("#tab_c4").removeClass("hidden");
                $("#tab_c3").addClass("hidden");
                $("#tab_c4").addClass("active");
                $("#m2_save_but").attr("disabled", false);
                $("#del_save_but_2").attr("disabled", false);
            },

            pager: '#PageMTmachine',
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: true,
            height: 250,
            width: $(".boxMaterial").width() - 5,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            caption: ' Process Machine Unit',
            rownumbers: true,
            gridview: true,
            multiselect: true,
            shrinkToFit: false,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },


        });

    },
});

$(".Popup_mc_pr").on("click", function () {
    $('.dialog_machine').dialog('open');
    $.ajax({
        url: "/ActualWO/Getprocess_machineunit",
        type: "GET",
        dataType: "json",
        data: {
            id_actual: ($('#id_actual').val() == "" ? 0 : $('#id_actual').val()),
        },
        success: function (response) {
            $("#popupmachine1").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response }).trigger("reloadGrid");

        },

    });
});




$('#close_mc').click(function () {
    $('.dialog_machine').dialog('close');
});

$("#tab_3").on("click", "a", function (event) {
    //document.getElementById("form3").reset();
    $("#tab_4").removeClass("active");
    $("#tab_3").addClass("active");
    $("#tab_c4").removeClass("active");
    $("#tab_c3").removeClass("hidden");
    $("#tab_c4").addClass("hidden");
    $("#tab_c3").addClass("active");
    $('#start').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        daysOfWeekDisabled: [0, 6],
    });
    $('#end').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        date: new Date('9999-12-31 23:59:59'),
    });

});
$("#tab_4").on("click", "a", function (event) {

    //document.getElementById("form4").reset();
    $("#tab_3").removeClass("active");
    $("#tab_4").addClass("active");
    $("#tab_c3").removeClass("active");
    $("#tab_c4").removeClass("hidden");
    $("#tab_c3").addClass("hidden");
    $("#tab_c4").addClass("active");
    $("#m2_save_but").attr("disabled", true);
    $("#del_save_but_2").attr("disabled", true);
    $("#m_start").datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        daysOfWeekDisabled: [0, 6],
    });
    $("#m_end").datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        date: new Date('9999-12-31 23:59:59'),
    });

});

$('#m_start').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});
$('#m_end').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});

//$("#c2_save_but").click(function () {
    $("#create_machine").click(function () {
    if ($("#form3").valid() == true) {
        $.ajax({
            url: "/ActualWO/CreateProcessMachineUnit",
            type: "POST",
            dataType: "json",
            data: {
                mc_no: $('#c_mc_no').val(),
                id_actual: $('#id_actual').val(),
                use_yn: $('#m2_use_yn').val(),
                remark: $('#c2_remark').val(),
            },
            success: function (data) {
                switch (data.result) {
                     case -1:
                        ErrorAlert(data.message);
                        break;
                    case 0:
                        var id = data.kq.pmid;
                        $("#popupmachine1").jqGrid('addRowData', id, data.kq, 'first');
                        $("#popupmachine1").setRowData(id, false, { background: "#d0e9c6" });
                        $("#so_luong_mchine").val(1);
                        SuccessAlert(data.message);
                        break;
                    case 1:
                        ErrorAlert(data.message);
                        break;
                    case 2:
                        ErrorAlert(data.message);
                        break;
                    case 3:
                        call_create("create_mc", data.update, data.start, data.end);
                        break;
                    case 4:
                        ErrorAlert(data.message);
                        break;
                }
            },
        });
    }
});
$("#m2_save_but").click(function () {
    if ($("#form4").valid() == true) {
        $.ajax({
            type: "PUT",
            dataType: "json",
            url: "/ActualWO/ModifyProcessMachineUnit",
            data: {
                mc_no: $('#m_mc_no').val(),
                pmid: $('#pmid').val(),
                id_actual: $('#id_actual').val(),
                use_yn: $('#m2_use_yn').val(),
                start: $('#m_start').val(),
                end: $('#m_end').val(),
                remark: $('#m2_remark').val(),
            },
            success: function (data) {
                console.log(data);
                switch (data.result) {
                    case -1:
                        ErrorAlert(data.message);
                        break;
                    case 0:
                        var id = data.item.pmid;
                        $("#popupmachine1").setRowData(id, data.item, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                        break;
                    case 1:
                        ErrorAlert(data.message);
                        break;
                    case 2:
                        ErrorAlert(data.message);
                        break;
                    case 3:
                        ErrorAlert(data.message);
                        break;
                    case 4:
                        ErrorAlert(data.message);
                        break;
                    case 5:
                        ErrorAlert(data.message);
                        break;
                }

            }
        });
    }

});
$("#form3").validate({
    rules: {
        "mc_no": {
            required: true,
        },
    },
});
$("#form4").validate({
    rules: {
        "mc_no": {
            required: true,
        },
    },
});
