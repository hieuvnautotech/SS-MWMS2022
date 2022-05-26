$(function () {
    $(".dialogfo_no").dialog({
        width: '70%',
        height: 900,
        maxWidth: 1000,
        maxHeight: 900,
        minWidth: '50%',
        minHeight: 900,
        zIndex: 1000,
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
            
            $("#list").jqGrid
            ({
                url: "/ActualWO/getActual",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                     { key: true, label: 'fno', name: 'fno', width: 50, hidden: true },
                     { label: 'WO', name: 'fo_no', width: 110, align: 'center' },
                     { label: 'Po no', name: 'po_no', width: 110, align: 'center' },
                     { label: 'Line', name: 'line_no', width: 110, align: 'center' },
                     { label: 'Product Code', name: 'style_no', width: 180, align: 'left' },
                     { label: 'Product Name', name: 'style_nm', width: 220, align: 'left' },
                     { label: 'Bom no', name: 'bom_no', width: 110, align: 'center' },
                     { label: 'Factory', name: 'lct_cd', width: 150, align: 'center' },
                     { label: 'Status', name: 'order_sts_cd', width: 110, align: 'center' },
                     { label: 'Qty', name: 'fo_qty', width: 110, align: 'right', formatter: 'integer' },
                     { label: 'Create Date', name: 'reg_dt', align: 'center', width: 180, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
                     { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 180, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } }
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });                   
                    var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                    row_id = $("#list").getRowData(selectedRowId);
                    $("#list1").clearGridData();
                    $("#list1").setGridParam({ url: "/ActualWO/getActualLine_Issue?" + "fo_no=" + row_id.fo_no + "&line_no=" + row_id.line_no, datatype: "json" }).trigger("reloadGrid");
                    $("#savestylefo_no").attr("disabled", true);
                    $("#list2").clearGridData();
                },
                pager: jQuery('#gridpager'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 300,
                width: $(".boxA_New").width(),
                autowidth: false,
                caption: 'WO',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                viewrecords: true,
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

            $("#list1").jqGrid
            ({
                url: "/ActualWO/getActualLine_Issue",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                        //{ key: true, label: 'oldno', name: 'oldno', width: 50, hidden: true },
                        //{ label: 'Level', name: 'level', width: 80, align: 'center' },
                        //{ label: 'old_no', name: 'old_no', width: 80, align: 'center', hidden: true },
                        //{ label: 'Process', name: 'process_nm', width: 150, align: 'center' },
                        //{ label: 'Process no', name: 'process_no', width: 110, align: 'center', hidden: true },
                        //{ label: 'Qty', name: 'prounit_target_qty', width: 80, align: 'right', formatter: 'integer' },
                        //{ label: 'Start date', name: 'start_dt', width: 110, align: 'center' },
                        //{ label: 'End date', name: 'end_dt', width: 110, align: 'center' },
                        //{ label: 'Material', name: 'material_check_yn', width: 100, align: 'center', },
                        //{ label: 'Machine', name: 'machine_check_yn', width: 110, align: 'center', },
                        //{ label: 'Staff', name: 'staff_check_yn', width: 110, align: 'center', },
                        //{ label: 'Description', name: 're_mark', width: 110, align: 'center' },
                        { label: 'dlpid', name: 'dlpid', hidden: true },
                        { label: 'Level', name: 'level', width: 50, align: 'center' },
                        { label: 'Process No', name: 'process_no', width: 50, align: 'center' },
                        { label: 'old_no', name: 'old_no', width: 80, align: 'center', hidden: true },
                        { label: 'Process', name: 'process_nm', width: 150, align: 'center', hidden: true },
                        { label: 'Process Type', name: 'process_type', width: 50, align: 'center' },
                        { label: 'Unit Qty', name: 'prounit_qty', width: 50, align: 'right', formatter: 'integer' },
                        { label: 'Start Date', name: 'start_dt', width: 80, align: 'center' },
                        { label: 'End Date', name: 'end_dt', width: 80, align: 'center' },
                        { label: 'Target qty', name: 'target_qty', width: 100, align: 'right', formatter: 'integer' },
                        { label: 'Schedule qty', name: 'sked_qty', width: 100, align: 'right', formatter: 'integer' },
                        { label: 'Actual', name: 'done_qty', width: 50, align: 'right', formatter: 'integer' },
                        { label: 'Efficiency(%)', name: 'rate', width: 100, align: 'right' },
                        { label: 'Defective', name: 'refer_qty', width: 80, align: 'right', formatter: 'integer' },
                        { label: 'Defect Rate(%)', name: 'refer_rate', width: 100, align: 'right' }
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (id, rowid1, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
                    row_id1 = $("#list1").getRowData(selectedRowId);

                    $("#list2").clearGridData();
                    $("#list2").setGridParam({ url: "/ActualWO/getActualdetail_Issue?" + "fo_no=" + row_id.fo_no + "&line_no=" + row_id.line_no + "&process_no=" + row_id1.process_no + "&old_no=" + row_id1.old_no, datatype: "json" }).trigger("reloadGrid");
                    $("#savestylefo_no").attr("disabled", false);

                },
                gridComplete: function () {
                    var rows = $("#list1").getDataIDs();
                    for (var i = 0; i < rows.length; i++) {
                        var condition_display = true;
                        var process_nm = $("#list1").getCell(rows[i], "process_nm");
                        var start_dt = $("#list1").getCell(rows[i], "start_dt");
                        var rows1 = $("#issueGrid").getDataIDs();
                        for (var j = 0; j < rows1.length; j++) {
                            var condition_process_nm = $("#issueGrid").getCell(rows1[j], "process_nm");
                            var condition_start_dt = $("#issueGrid").getCell(rows1[j], "start_dt");
                            if ((process_nm == condition_process_nm) && (start_dt == condition_start_dt)) {
                                condition_display = false;                              
                            }
                        }
                        if (!condition_display) {
                            $("#list1").jqGrid('setRowData', rows[i], false, { background: '#d0e9c6' });
                        }
                    }
                },
                pager: jQuery('#gridpager1'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: $(".boxB_New").width(),
                autowidth: false,
                caption: 'Process Of Routing',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                loadonce: true,
                viewrecords: true,
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
            $("#list2").jqGrid({
                url: "/ActualWO/getActualdetail_Issue",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    //{ key: true, label: 'olddno', name: 'olddno', width: 80, align: 'center', hidden: true },
                    //{ key: false, label: 'Date', name: 'work_dt', width: 179, align: 'center' },
                    //{ label: 'Target', name: 'prod_qty', width: 152, align: 'right', formatter: 'integer' },
                    //{ label: 'Actual', name: 'done_qty', width: 82, align: 'right', formatter: 'integer' },
                    //{ label: 'Defective', name: 'refer_qty', width: 81, align: 'right', formatter: 'integer' },
                    //{ key: false, label: 'refer_qty', name: 'refer_qty', width: 81, align: 'right', hidden: true },
                    //{ key: false, label: 'Efficiency', name: 'efficiency', width: 190, align: 'right', formatter: numberFormatter },
                    { label: 'oldno', name: 'oldno', width: 50, hidden: true },
                    { label: 'Process No', name: 'process_no', width: 150, align: 'center' },
                    { label: 'Process No', name: 'process_no1', width: 150, align: 'center', hidden: true },
                    { label: 'Target', name: 'line_prod_qty', width: 150, align: 'right', formatter: 'integer' },
                    { label: 'Start Date', name: 'stat_time', width: 150, align: 'center' },
                    { label: 'End Date', name: 'end_time', width: 150, align: 'center' }
                ],
                pager: jQuery('#gridpager2'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: $(".boxC_New").width(),
                autowidth: false,
                caption: 'Process Of Routing Detail',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                shrinkToFit: false,
                loadonce: true,
                viewrecords: true,
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
                //subGrid: true,
                //subGridRowExpanded: function (subgrid_id, row_id) {
                //    var subgrid_table_id;

                //    subgrid_table_id = subgrid_id + "_t";
                //    jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
                //    jQuery("#" + subgrid_table_id).jqGrid({
                //        url: "/Plan/Getdaytime?id=" + row_id,
                //        datatype: 'json',
                //        colModel: [
                //            { key: true, label: 'olddtno', name: 'olddtno', align: 'center', hidden: true },
                //            { label: "olddt_no", name: "olddt_no", width: 110, align: "center", hidden: true },
                //            { label: "oldd_no", name: "oldd_no", width: 110, align: "center", hidden: true },
                //            { label: "Process no", name: "process_no", width: 92, align: "center", },
                //            { label: "Process type", name: "process_type", width: 87, align: "center" },
                //            { label: "Start time", name: "start_time", width: 79, align: "center", },
                //            { label: 'Target', name: 'prod_qty', width: 73, align: 'right' },
                //            { label: 'Actual', name: 'done_qty', width: 82, align: 'right', },
                //                { label: "Defect", name: "refer_qty", width: 81, align: 'right', },
                //            { label: "Efficiency(%)", name: "rate", width: 95, align: 'right', formatter: numberFormatter },
                //            { label: "Defect Rate(%)", name: "refer_rate", width: 95, align: 'right', formatter: numberFormatter },
                //        ],
                //        height: '100%',
                //        rowNum: 20,
                //        sortname: 'num',
                //        sortorder: "asc",
                //        jsonReader:
                //{
                //    root: "rows",
                //    page: "page",
                //    total: "total",
                //    records: "records",
                //    repeatitems: false,
                //    Id: "0"
                //},
                //    });
                //},
                jsonReader:
                {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    Id: "0"
                },
            })
            function numberFormatter(cellvalue, options, rowObject) {
                kg2 = cellvalue.toString().replace(" %", "");
                if (cellvalue == null || cellvalue == "") {
                    kq = 0;
                }
                else if (cellvalue.toString().includes(".")) {
                    kq = parseFloat(cellvalue).toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                    kg2 = kq;
                } else {
                    kq = cellvalue.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                    kg2 = kq;
                }

                return kg2 + " %"
            };
        },
    });




    $(".poupdialogfo_no").click(function () {
        $('.dialogfo_no').dialog('open');
    });



    $('#savestylefo_no').click(function () {
        if (row_id != null) {
            $('#fo_no_c').val(row_id.fo_no);
            $('#style_no_c').val(row_id.style_no);
            $('#style_nm_c').val(row_id.style_nm);
            $('#process_nm_c').val(row_id1.process_nm);
            $('#start_c').val(row_id1.start_dt);

            $('#condition_fo_no').val(row_id.fo_no);
            $('#condition_process_nm').val(row_id1.process_nm);
            $('#condition_start_dt').val(row_id1.start_dt);

            $('#end_c').val(row_id1.end_dt);
            $('#old_no_c').val(row_id1.old_no);

            //$.ajax({
            //    url: "/ActualWO/getDoingTime?old_no=" + row_id1.old_no,
            //    type: "get",
            //    dataType: "json",
            //    success: function (rtnData) {
            //        $.each(rtnData, function (dataType, data) {
            //            {
            //                $("#doing_time_c").val(data);
            //                $("#doing_time_m").val(data);
            //            }
            //        });
            //    }
            //});

            $('#fo_no_m').val(row_id.fo_no);
            $('#style_no_m').val(row_id.style_no);
            $('#style_nm_m').val(row_id.style_nm);
            $('#process_nm_m').val(row_id1.process_nm);
            $('#start_m').val(row_id1.start_dt);
            $('#end_m').val(row_id1.end_dt);
            $('#old_no_m').val(row_id1.old_no);
            $('.dialogfo_no').dialog('close');
        }

    });


    //$('#closestyle').click(function () {
    //    $('.dialogfo_no').dialog('close');
    //});
});