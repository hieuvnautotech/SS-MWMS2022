$(function () {
    var row_id;
    $(".dialogsprocessnm").dialog({
        width: '50%',
        height: 450,
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

        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

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
                    var selectedRowId = $("#popupssprocessnm").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupssprocessnm").getRowData(selectedRowId);

                },

                pager: jQuery('#popupssprocessnmpager'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: 902,
                autowidth: false,
                caption: 'Process',
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


    //$('#closestyle2').click(function () {
    //    $('.dialog2').dialog('close');

    //});
    $('#savestylesprocessnm').click(function () {
        if (row_id != null) {
            $('#s_process_nm').val(row_id.process_nm);
            $('.dialogsprocessnm').dialog('close');
        }

    });



});

