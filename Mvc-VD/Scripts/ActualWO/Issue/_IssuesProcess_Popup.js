$(function () {
    var row_id;
    $(".dialogsprocessnm").dialog({
        width: '50%',
        height: 500,
        maxWidth: 1000,
        maxHeight: 500,
        minWidth: '50%',
        minHeight: 500,
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
        //close: function (event, ui) {
        //    $("#popupssprocessnm").trigger('reloadGrid');
        //},
        open: function (event, ui) {
            //$("#popupssprocessnm").trigger('reloadGrid');
            $("#popupssprocessnm").jqGrid
            ({
                url: "/ActualWO/getPopupProcess",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'oldno', name: 'oldno', width: 50, hidden: true },
                         { label: 'Level', name: 'level', width: 80, align: 'center' },
                         { label: 'old_no', name: 'old_no', width: 80, align: 'center', hidden: true },
                         { label: 'Process', name: 'process_nm', width: 150, align: 'center' },
                         { label: 'Process no', name: 'process_no', width: 110, align: 'center', hidden: true },
                         { label: 'Qty', name: 'prounit_target_qty', width: 80, align: 'right', formatter: 'integer' },
                         { label: 'Start date', name: 'start_dt', width: 110, align: 'center' },
                         { label: 'End date', name: 'end_dt', width: 110, align: 'center' },

                       { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#savestylesprocessnm").removeClass("disabled");
                    var selectedRowId = $("#popupssprocessnm").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupssprocessnm").getRowData(selectedRowId);
                    //$("#popupssprocessnm").jqGrid('clearGridData');
                },

                pager: jQuery('#popupssprocessnmpager'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: $(".boxProcess").width(),
                autowidth: false,
                caption: '',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                loadonce: true,
                shrinkToFit: false,
                multiselect: false,
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

    $(".poupdialog3").click(function () {
        $('.dialogsprocessnm').dialog('open');
    });


    $('#searchBtn_process_popup').click(function () {
        var process_no = $('#process_no_popup').val().toString().trim();
        var start = $('#process_no_start').val().toString().trim();
        var end = $('#process_no_end').val().toString().trim();
        $.ajax({
            url: "/ActualWO/searchBtn_product_popup",
            type: "get",
            dataType: "json",
            data: {
                process_no: process_no,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#popupssprocessnm").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
            }
        });

    });

    $('#savestylesprocessnm').click(function () {
        if (row_id != null) {
            $('#s_process_nm').val(row_id.process_nm);

            $('.dialogsprocessnm').dialog('close');
            $("#popupssprocessnm").trigger('reloadGrid');
        }

        //$('#process_no_popup').val("");
        //$('#process_no_start').val("");
        //$('#process_no_end').val("");

        $("#popupssprocessnm").trigger('reloadGrid');
    });



});

