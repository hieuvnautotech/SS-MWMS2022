$(function () {
    $(".dialogMR").dialog({
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

            $("#popupsMR").jqGrid
            ({
                url: "/ShippingMgt/getMRNO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'mrid', name: 'mrid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'MSR NO', name: 'mr_no', width: 100, align: 'center' },
                    //{ key: false, label: 'MSR Status Code', name: 'mr_sts_cd', width: 100, },
                    { key: false, label: 'Location', name: 'lct_nm', width: 180, align: 'left' },
                    { key: false, label: 'Manager', name: 'manager_id', width: 100, align: 'left' },
                    { key: false, label: 'Req Receive Date', name: 'req_rec_dt', width: 100, align: 'center' },
                    { key: false, label: 'Real Receive Date', name: 'real_rec_dt', width: 150, align: 'center' },
                    { key: false, label: 'MT Qty', name: 'mt_qty', width: 50, align: 'right' },
                    { key: false, label: 'Related Bom', name: 'rel_bom', width: 150, align: 'center' },
                    { key: false, label: 'Worker', name: 'worker_id', width: 100, align: 'center' },
                    { key: false, label: 'Description', name: 'remark', width: 100, align: 'left' },
             
                    { key: false, label: 'Create User', name: 'reg_id', width: 100, align: 'center' },
                    { key: false, label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200' },
                    { key: false, label: 'Change User', name: 'chg_id', width: 100, align: 'center' },
                    { key: false, label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200' },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsMR").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsMR").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#selected').click(function () {
                            $('#mr_no').val(row_id.mr_no);
                            $('#mr_no1').val(row_id.mr_no);
                            $('.dialogMR').dialog('close');
                        });
                    }
                },
             

                pager: jQuery('#popupspagerMR'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 250,
                rowNum: 50,
                loadonce: true,
                autowidth: true,
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
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
    $(".poupdialogMR").click(function () {
        $('.dialogMR').dialog('open');
    });
    $('#closeMR').click(function () {
        $('.dialogMR').dialog('close');
    });

});