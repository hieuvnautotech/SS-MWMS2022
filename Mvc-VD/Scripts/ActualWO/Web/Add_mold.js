$(".dialog_mold").dialog({
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

        $("#popupmold").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'mdid', name: 'mdid', align: 'center', hidden: true },
                { key: false, label: 'Mold', name: 'md_no', width: 200, align: 'left', },
                { key: false, label: 'Destination', name: 'remark', width: 200, align: 'left', },
                { key: false, label: 'Start Date', name: 'start_dt', width: 200, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'End Date', name: 'end_dt', width: 200, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'Use', name: 'use_yn', editable: true, width: '100px', width: 50, align: 'center', },  
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupmold").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupmold").getRowData(selectedRowId);
                $("#pmid_ml").val(row_id.pmid);
                $("#m_ml_mc_no").val(row_id.mc_no);
                $("#m2_ml_use_yn").val(row_id.use_yn);
                $("#m_start_ml").val(row_id.start_dt);
                $("#m_end_ml").val(row_id.end_dt);
                $("#m2_ml_remark").val(row_id.remark);


                $("#tab_3_ml").removeClass("active");
                $("#tab_4_ml").addClass("active");
                $("#tab_c3_ml").removeClass("active");
                $("#tab_c4_ml").removeClass("hidden");
                $("#tab_c3_ml").addClass("hidden");
                $("#tab_c4_ml").addClass("active");
                $("#m2_ml_save_but").attr("disabled", false);
                $("#de_ml_save_but_2").attr("disabled", false);
            },

            pager: '#Pagemold',
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: true,
            height: 250,
            width: $(".boxmold").width() - 5,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            caption: ' Process Mold Unit',
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: true,
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

$(".Popup_mold_pr").on("click", function () {
    var prounit_cd = $('#cp_process_nm').val();
    $('.dialog_mold').dialog('open');
    $.ajax({
        url: "/ActualWO/Getprocess_moldunit",
        type: "get",
        dataType: "json",
        data: {
            id_actual: $('#id_actual').val(),
        },
        success: function (response) {           
            $("#popupmold").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response }).trigger("reloadGrid");
            
        },

    });
  
});

$("#tab_3_ml").on("click", "a", function (event) {
    //document.getElementById("form3").reset();
    $("#tab_4_ml").removeClass("active");
    $("#tab_3_ml").addClass("active");
    $("#tab_c4_ml").removeClass("active");
    $("#tab_c3_ml").removeClass("hidden");
    $("#tab_c4_ml").addClass("hidden");
    $("#tab_c3_ml").addClass("active");
    $('#start_ml').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        daysOfWeekDisabled: [0, 6],
    });
    $('#end_ml').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        date: new Date('9999-12-31 23:59:59'),
    });

});
$("#tab_4_ml").on("click", "a", function (event) {

    //document.getElementById("form4").reset();
    $("#tab_3_ml").removeClass("active");
    $("#tab_4_ml").addClass("active");
    $("#tab_c3_ml").removeClass("active");
    $("#tab_c4_ml").removeClass("hidden");
    $("#tab_c3_ml").addClass("hidden");
    $("#tab_c4_ml").addClass("active");
    $("#m2_ml_save_but").attr("disabled", true);
    $("#del_ml_save_but_2").attr("disabled", true);
    $('#m_start_ml').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        daysOfWeekDisabled: [0, 6],
    });
    $('#m_end_ml').datetimepicker({
        format: 'YYYY-MM-DD HH:mm:ss',
        date: new Date('9999-12-31 23:59:59'),
    });

});

$("#c2_ml_save_but").click(function () {
    if ($("#form3_ml").valid() == true) {
        $.ajax({
            url: "/ActualWO/CreateProcessMoldUnit",
            type: "Post",
            dataType: "json",
            data: {
                md_no: $('#c_ml_mc_no').val(),
                use_yn: $('#c2_ml_use_yn').val(),
                id_actual: $('#id_actual').val(),
                remark: $('#c2_ml_remark').val().trim(),
            },
            success: function (data) {
                //debugger;
                switch (data.result) {
                    case 0:
                        var id = data.kq.mdid;
                        $("#popupmold").jqGrid('addRowData', id, data.kq, 'first');
                        $("#popupmold").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                        break;
                    case 1:
                        ErrorAlert(data.message);
                        break;
                    case 2:
                        ErrorAlert(data.message);
                        break;
                    case 3:
                        call_create("create_mold", data.update, data.start, data.end);
                        break;
                    case 4:
                        ErrorAlert(data.message);
                        break;

                }
            },
        });
    }
});
$("#m2_ml_save_but").click(function () {
    if ($("#form4_ml").valid() == true) {
        $.ajax({
            type: "get",
            dataType: "json",
            url: "/ActualWO/Modifyprocessmachine_unit",
            data: {
                mc_no: $('#m_ml_mc_no').val(),
                pmid: $('#pmid_ml').val(),
                use_yn: $('#m2_ml_use_yn').val(),
                start: $('#m_start_ml').val(),
                id_actual: $('#id_actual').val(),
                end: $('#m_end_ml').val(),
                remark: $('#m2_ml_remark').val().trim(),
            },
            success: function (data) {
                switch (data.result) {
                    case 0:
                        var id = data.item.pmid;
                        $("#popupmold").setRowData(id, data.item, { background: "#d0e9c6" });
                        break;
                    case 1:
                        alert('The Process Machine Unit was setting duplicate date');
                        break;
                    case 2:
                        alert('Start day was bigger End day. That is wrong');
                        break;
                }

            }
        });
    }

});
$("#form3_ml").validate({
    rules: {
        "mc_no": {
            required: true,
        },
    },
});
$("#form4_ml").validate({
    rules: {
        "mc_no": {
            required: true,
        },
    },
});
