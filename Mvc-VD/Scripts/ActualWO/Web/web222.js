$(function () {
    $("#list").jqGrid
    ({
        url: "/ActualWO/getActual",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'fno', name: 'fno', width: 50, hidden: true },
             { label: 'WO', name: 'fo_no', width: 60, align: 'center', formatter: Workorder_popup },
             { label: 'WO', name: 'fo_no', width: 110, align: 'center', hidden: true },
             { label: 'PO', name: 'po_no', width: 110, align: 'center' },
             { label: 'Routing', name: 'line_no', width: 80, align: 'center' },
             { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
             { label: 'Product Name', name: 'style_nm', width: 300, align: 'left' },
             { label: 'Bom no', name: 'bom_no', width: 110, align: 'center' },
             { label: 'Factory', name: 'lct_cd', width: 150, align: 'left' },
             { label: 'Factory', name: 'index_cd', width: 150, align: 'left', hidden: true },
             { label: 'Status', name: 'order_sts_cd', width: 80, align: 'center' },
             { label: 'Qty', name: 'fo_qty', width: 90, align: 'right', formatter: 'integer' },
             { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Actual', name: '', width: 70, align: 'center', formatter: actual_pp },
            { label: 'Plan', name: '', width: 70, align: 'center', formatter: plan_pp }
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, status, e) {

            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $("#list1").clearGridData();
            $("#list1").setGridParam({ url: "/ActualWO/getActualLine?" + "fo_no=" + row_id.fo_no + "&line_no=" + row_id.line_no, datatype: "json" }).trigger("reloadGrid");
            $("#list2").clearGridData();


            var fno = row_id.fno;
            var fo_no = row_id.fo_no;
            var style_no = row_id.style_no;
            var style_nm = row_id.style_nm;
            var line = row_id.line_no;


            $('#qc_style_no').val(row_id.style_no);
            $('#qc_po_no').val(row_id.po_no);
            $('#qc_bom_no').val(row_id.bom_no);

            //list2
            $('#fo_no_h').val(row_id.fo_no);
            $('#line_no_h').val(row_id.line_no);


            line_no(fno, fo_no, style_no, style_nm, line); cp_lct_cd

            //poup composite 
            $('#cp_fo_no').val(row_id.fo_no);
            $('#cp_style_no').val(row_id.style_no);
            $('#cp_bom_no').val(row_id.bom_no);
            $('#mt_bom_popup').val(row_id.bom_no);
            $('#cp_factory1').val(row_id.lct_cd);
            $('#cp_lct_cd').val(row_id.index_cd);

            //poup product 

            $('#pp_fo_no').val(row_id.fo_no);
            $('#pp_style_no').val(row_id.style_no);
            $('#pp_factory1').val(row_id.lct_cd);
            $('#pp_lct_cd').val(row_id.index_cd);
            $('#pp_bom_no').val(row_id.bom_no);
        },

        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: "300",
        width: null,
        caption: 'WO',
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
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
    $("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    $("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();
});
$(function () {
    $("#list_excel").jqGrid
    ({
        url: "/ActualWO/getActual",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'fno', name: 'fno', width: 50, hidden: true },
             { label: 'WO', name: 'fo_no', width: 60, align: 'center', formatter: Workorder_excel },
             { label: 'WO', name: 'fo_no', width: 110, align: 'center', hidden: true },
             { label: 'PO', name: 'po_no', width: 110, align: 'center' },
             { label: 'Routing', name: 'line_no', width: 80, align: 'center' },
             { label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
             { label: 'Product Name', name: 'style_nm', width: 300, align: 'left' },
             { label: 'Bom no', name: 'bom_no', width: 110, align: 'center' },
             { label: 'Factory', name: 'lct_cd', width: 150, align: 'left' },
             { label: 'Factory', name: 'index_cd', width: 150, align: 'left', hidden: true },
             { label: 'Status', name: 'order_sts_cd', width: 80, align: 'center' },
             { label: 'Qty', name: 'fo_qty', width: 90, align: 'right', formatter: 'integer' },
             { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
             { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        //pager: jQuery('#gridpager'),
        //viewrecords: true,
        //rowList: [50, 100, 200, 500, 1000],
        //height: "300",
        //width: null,
        caption: 'WO',
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
        viewrecords: true,
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
  
});
function Title() {
    $('.dialogworkorder').dialog('open');
}
function line_no(fno, fo_no, style_no, style_nm, line) {

    var html = "WS-" + fo_no + "-" + line;
    var title_product = style_no + " " + "[" + style_nm + "]";
    $("#title_work").html(html);
    $("#title_product").html(title_product);

    $('#fno').val(fno);

    $('#barcode').append(html);
    $('#barcode').barcode(html, "code128", { barWidth: 1, barHeight: 30 });

    $(document).ready(function () {

        $('#dataTable').DataTable({
            "bServerSide": true,

            "ajax": {
                "url": "/ActualWO/getActualWorkorder?" + "fno=" + fno,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [
                { "data": "fo_no" },
                { "data": "po_no" },
                { "data": "bom_no" },
                { "data": "lct_cd" },
                { "data": "line_no" },
                { "data": "fo_qty", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                { "data": "delivery_dt" },
                { "data": "re_mark" },
            ],

            'columnDefs': [
              {
                  "targets": 0, // your case first column
                  "className": "text-center",
                  "width": "15%"
              },
               {
                   "targets": 1, // your case first column
                   "className": "text-center",
                   "width": "15%"
               },
               {
                   "targets": 2, // your case first column
                   "className": "text-center",
                   "width": "15%"
               },
               {
                   "targets": 3, // your case first column
                   "className": "text-left",
                   "width": "15%"
               },
                {
                    "targets": 4, // your case first column
                    "className": "text-center",
                    "width": "15%"
                },
                {
                    "targets": 5, // your case first column
                    "className": "text-right",
                    "width": "15%"
                },
                 {
                     "targets": 6, // your case first column
                     "className": "text-center",
                     "width": "15%"
                 },
            ],
            "bDestroy": true,
        });
        $('#dataTable1').DataTable({
            "bServerSide": true,
            "ajax": {
                "url": "/ActualWO/GetActual_material?" + "fno=" + fno,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [

                { "data": "process_type" },
                { "data": "mt_nm" },
                { "data": "width" },
                { "data": "need_qty" },
                { "data": "feed_size" },
                { "data": "feed_unit" },
                { "data": "div_cd" },

            ],
            'rowsGroup': [0],
            'columnDefs': [
              {
                  "targets": 0, // your case first column
                  "className": "text-center",
                  "width": "15%"
              },
               {
                   "targets": 2, // your case first column
                   "className": "text-right",
                   "width": "15%"
               },
               {
                   "targets": 3, // your case first column
                   "className": "text-right",
                   "width": "15%"
               },
               {
                   "targets": 4, // your case first column
                   "className": "text-right",
                   "width": "15%"
               },
            ],

            "bDestroy": true,
        });
        $('#dataTable2').DataTable({
            "bServerSide": true,
            "ajax": {
                "url": "/ActualWO/getActualMachin?" + "fno=" + fno,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [

                { "data": "prounit_cd" },
                { "data": "mc_type" },
                { "data": "mc_no" },
                { "data": "start_dt" },
                { "data": "end_dt" },

            ],
            'rowsGroup': [0, 1],
            'columnDefs': [
             {
                 "targets": 0, // your case first column
                 "className": "text-center",
                 "width": "10%"
             },
              {
                  "targets": 1, // your case first column
                  "className": "text-center",
                  "width": "15%"
              },
              {
                  "targets": 2, // your case first column
                  "className": "text-center",
                  "width": "20%"
              },
              {
                  "targets": 3, // your case first column
                  "className": "text-center",
                  "width": "15%"
              },
               {
                   "targets": 4, // your case first column
                   "className": "text-center",
                   "width": "15%"
               },
            ],
            "bDestroy": true,
        });
        $('#dataTable3').DataTable({
            "bServerSide": true,
            "ajax": {
                "url": "/ActualWO/GetActual_staff?" + "fno=" + fno,
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [
                { "data": "prounit_cd" },
                { "data": "staff_id" },
                { "data": "uname" },
                { "data": "dt_nm" },
                { "data": "start_dt" },
                { "data": "end_dt" },
            ],
            'rowsGroup': [0],
            'columnDefs': [
             {
                 "targets": 0, // your case first column
                 "className": "text-center",
                 "width": "10%"
             },
              {
                  "targets": 1, // your case first column
                  "className": "text-left",
                  "width": "15%"
              },
              {
                  "targets": 2, // your case first column
                  "className": "text-left",
                  "width": "15%"
              },
              {
                  "targets": 3, // your case first column
                  "className": "text-left",
                  "width": "15%"
              },
               {
                   "targets": 4, // your case first column
                   "className": "text-center",
                   "width": "15%"
               },
                {
                    "targets": 5, // your case first column
                    "className": "text-center",
                    "width": "15%"
                },
            ],
            "bDestroy": true,
        });
    });
    $("#PrintDetail1").on("click", function () {
        var fno = $("#fno").val();
        window.open("/ActualWO/PrintWorkorder?" + "fno=" + fno, "PRINT", "width=860, height=800, left=0, top=100, location=no, status=no,")
    })
}
function Workorder_popup(cellValue, options, rowdata, action) {
    var fo_no = rowdata.fo_no;
    var a, b;
    a = fo_no.substr(0, 1);
    b = fo_no.substr(1, 11);
    c = parseInt(b);
    var html = '<button style="color: dodgerblue;border: none;background: none; " onclick="Title();">' + a + c + '</button>';
    return html;


};
function Workorder_excel(cellValue, options, rowdata, action) {
    var fo_no = rowdata.fo_no;
    var a, b;
    a = fo_no.substr(0, 1);
    b = fo_no.substr(1, 11);
    c = parseInt(b);
    var html =  a + c ;
    return html;
};
$(function () {
    $(".dialogworkorder").dialog({
        width: '70%',
        height: 900,
        maxWidth: 1000,
        maxHeight: 900,
        minWidth: '70%',
        minHeight: 900,
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });


    $('#closeinedetail').click(function () {
        $('.dialogworkorder').dialog('close');
        jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    });

});

//line_popup
/////--------------line------------
var lastSel;
$(function () {
    $("#list1").jqGrid({
        url: "/ActualWO/getActualLine",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'oldno', name: 'oldno', width: 50, hidden: true },
             { label: 'Level', name: 'level', width: 40, align: 'center' },
             { label: 'old_no', name: 'old_no', width: 80, align: 'center', hidden: true },
             { label: 'Process', name: 'process_nm', width: 100, align: 'center' },
             { label: 'Process no', name: 'process_no', width: 110, align: 'center', hidden: true },
             { label: 'Process no', name: 'process_type', width: 110, align: 'center', hidden: true },
             { label: 'Qty', name: 'sked_qty', width: 50, align: 'right', formatter: 'integer' },
             { label: 'Start date', name: 'start_dt', width: 90, align: 'center' },
             { label: 'End date', name: 'end_dt', width: 90, align: 'center' },
             {
                 label: 'MT', name: 'material_check_yn', width: 80, align: 'center', editable: true, edittype: "select",
                 editoptions: {
                     dataUrl: "/ActualWO/getcheck",
                     buildSelect: function (data) {
                         //console.log(data);
                         //console.log(data.length);
                         var m, k, l, n, data = data.split(',"'), s = "<select>";
                         for (i = 0, j = data.length / 2; i < data.length / 2, j < data.length; i++, j++) {
                             m = data[i].replace('["', "");
                             l = m.replace('"', "");

                             k = data[j].replace('"', "");
                             n = k.replace(']', "");

                             s += '<option value="' + l + '">' + n + '</option>';
                             //console.log(s);
                         }
                         return s += '</select>';
                     },
                 },
                 cellattr: function (rowId, value, rowObject, colModel, arrData) {
                     if (value == 'OK') {
                         return 'style=background-color:green;';
                     }
                     if (value == 'Yet') {
                         return 'style=background-color:yellow;';
                     }
                     else {
                         return value;
                     }
                 }
             },
             {
                 label: 'Machine', name: 'machine_check_yn', width: 80, align: 'center', editable: true, edittype: "select",
                 editoptions: {
                     dataUrl: "/ActualWO/getcheck",
                     buildSelect: function (data) {
                         //console.log(data);
                         //console.log(data.length);
                         var m, k, l, n, data = data.split(',"'), s = "<select>";
                         for (i = 0, j = data.length / 2; i < data.length / 2, j < data.length; i++, j++) {
                             m = data[i].replace('["', "");
                             l = m.replace('"', "");

                             k = data[j].replace('"', "");
                             n = k.replace(']', "");

                             s += '<option value="' + l + '">' + n + '</option>';
                             //console.log(s);
                         }
                         return s += '</select>';
                     },
                 },
                 cellattr: function (rowId, value, rowObject, colModel, arrData) {
                     if (value == 'OK') {
                         return 'style=background-color:green;';
                     }
                     if (value == 'Yet') {
                         return 'style=background-color:yellow;';
                     }
                     else {
                         return value;
                     }
                 }

             },
             {
                 label: 'Staff', name: 'staff_check_yn', width: 80, align: 'center', editable: true, edittype: "select",
                 editoptions: {
                     dataUrl: "/ActualWO/getcheck",
                     buildSelect: function (data) {
                         //console.log(data);
                         //console.log(data.length);
                         var m, k, l, n, data = data.split(',"'), s = "<select>";
                         for (i = 0, j = data.length / 2; i < data.length / 2, j < data.length; i++, j++) {
                             m = data[i].replace('["', "");
                             l = m.replace('"', "");

                             k = data[j].replace('"', "");
                             n = k.replace(']', "");

                             s += '<option value="' + l + '">' + n + '</option>';
                             //console.log(s);
                         }
                         return s += '</select>';
                     },
                 },
                 cellattr: function (rowId, value, rowObject, colModel, arrData) {
                     if (value == 'OK') {
                         return 'style=background-color:green;';
                     }
                     if (value == 'Yet') {
                         return 'style=background-color:yellow;';
                     }
                     else {
                         return value;
                     }
                 }
             },
             { label: 'Display', name: 'setting', width: 110, align: 'center', formatter: bntCellValue },
             { label: 'Description', name: 're_mark', width: 110, align: 'center' },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (id, rowid1, status, e) {

            var lastSelection = "";

            if (id && id !== lastSelection) {
                var grid = $("#list1");
                grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
                lastSelection = id;

            }


            var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
            row_id1 = $("#list1").getRowData(selectedRowId);

            //poup composite 
            $('#cp_process_nm').val(row_id1.process_nm);
            $('#cp_process_no').val(row_id1.process_no);
            $('#pp_process_no').val(row_id1.process_no);
            $('#cp_process_type').val(row_id1.process_type);

           



            $('#old_no').val(row_id1.old_no);

            var fo_no = $('#fo_no_h').val();
            var line_no = $('#line_no_h').val();
          

            $("#list2").clearGridData();
            $("#list2").setGridParam({ url: "/ActualWO/getActualdetail?" + "fo_no=" + fo_no + "&line_no=" + line_no + "&process_no=" + row_id1.process_no + "&old_no=" + row_id1.old_no + "&level=" + row_id1.level, datatype: "json" }).trigger("reloadGrid");

            $('#qc_old_no').val(row_id1.old_no);
            $('#qc_process_type').val(row_id1.process_type);

            $('#qc_level').val(row_id1.level);

            $("#SaveDisplay").attr("data-old_no", row_id1.old_no);
            $("#SaveDisplay").attr("data-oldno", row_id1.oldno);

            var level = row_id1.level;
            if (level == "Last") {

                $("#tab1").show();
                $("#tab2").hide();
                $("#list_qc").trigger("reloadGrid");
                $("#list_qc_value").clearGridData();
            }
            else {
                $("#tab1").hide();
                $("#tab2").show();

                //jQuery("#list_facline_qc").setGridParam({ url: "/ActualWO/Getfacline_qc?item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
                jQuery("#list_facline_qc").trigger("reloadGrid");
                $("#list_facline_qc_value").clearGridData();
            }

        },
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        onCellSelect: editRow,
        pager: jQuery('#gridpager1'),
        rowList: [50, 100, 200, 500, 1000],
        height: "250",
        width: null,
        rowNum: 50,
        caption: 'Routing',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        shrinkToFit: false,
        loadonce: true,
        multiselect: true,
        autoResizing: true,
        viewrecords: true,
    })
});

var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list1");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
}

function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
$("#mbSaveLine").click(function () {
    var i, n, rowData, selRowIds = $("#list1").jqGrid("getGridParam", "selarrrow"), id = "", v_material = "", v_machine = "", v_staff = "";

    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];

        v_material = getCellValue(selRowIds[i], 'material_check_yn');
        v_machine = getCellValue(selRowIds[i], 'machine_check_yn');
        v_staff = getCellValue(selRowIds[i], 'staff_check_yn');

        if (id == "" || v_material == "" || v_machine == "" || v_staff == "") {
            continue;
        }
        $.ajax({
            type: 'post',
            url: '/ActualWO/ModifyLineCheck',
            headers: {
                "Content-Type": "application/json",
                "X-HTTP-Method-Override": "POST"
            },
            dataType: 'text',
            data: JSON.stringify({
                oldno: id,
                material: v_material,
                machine: v_machine,
                staff: v_staff
            }),
            //async: false,
            success: function (data) {
                jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });

    }
});
//desplay
function bntCellValue(cellValue, options, rowdata, action) {
    var old_no = rowdata.old_no;
    var oldno = rowdata.oldno;

    var html = '<button class="btn btn-xs btn-danger" id="setting" onclick="setting(' + oldno + '); ">Setting</button>';
    return html;
}
function setting(oldno) {
    var lastSel;
    $('.dialogDisplay').dialog('open');
    $("#popupsDisplay").setGridParam({ url: "/ActualWO/getDisplay?oldno=" + oldno, datatype: "json" }).trigger("reloadGrid");
    $("#popupsDisplay").jqGrid
     ({
         url: "/ActualWO/getDisplay?oldno=" + oldno,
         datatype: 'json',
         mtype: 'Get',
         colModel: [
              { key: true, label: 'mpid', name: 'mpid', width: 50, hidden: true },
              { label: 'mdid', name: 'mdid', width: 50, hidden: true },
              { label: 'old_no', name: 'old_no', width: 10, hidden: true },
              { label: 'Policy code', name: 'policy_code', width: 80, align: 'center' },
              { label: 'Policy name', name: 'policy_name', width: 80, align: 'left' },
              { label: 'Policy type', name: 'policy_type', width: 80, align: 'center' },
              {
                  label: 'Doing time', name: 'doing_time', width: 80, formatter: 'integer', align: 'right', editable: true,
                  editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                  formatter: 'integer', editoptions: {
                      dataInit: function (element) {
                          $(element).keypress(function (e) {
                              if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                  return false;
                              }
                          });
                      }
                  },
              },
              {
                  key: false, label: 'Use Y/N', name: 'use_yn', width: 80, align: 'center', editable: true, edittype: "select",
                  editoptions: {
                      value: { Y: 'Y', N: 'N' }
                  },
              },
              { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
              { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
              { label: 'Change User', name: 'chg_id', width: 90, align: 'center' },
              { label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }

         ],
         formatter: {
             integer: { thousandsSeparator: ",", defaultValue: '0' },
             currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
             number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
         },


         onCellSelect: editRow,
         gridComplete: function () {
             var rows = $("#popupsDisplay").getDataIDs();
             for (var i = 0; i < rows.length; i++) {
                 var v_use_yn = $("#popupsDisplay").getCell(rows[i], "use_yn");
                 if (v_use_yn == "N") {
                     $("#popupsDisplay").jqGrid('setRowData', rows[i], false, { background: '#D5D5D5' });
                 }
             }

         },
         onSelectRow: function (rowid, selected, status, e) {
             var lastSelection = "";

             if (rowid && rowid !== lastSelection) {
                 var grid = $("#popupsDisplay");
                 grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
                 lastSelection = rowid;


             }
         },
         pager: jQuery('#popupspagerDisplay'),
         viewrecords: true,
         rowList: [50, 100, 200, 500, 1000],
         rowNum: 50,
         loadonce: true,
         height: 350,
         //width: $(".ui-dialog").width() - 5,
         autowidth: true,
         loadtext: "Loading...",
         emptyrecords: "No data.",
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

    function editRow(id) {
        if (id && id !== lastSel) {
            var grid = $("#popupsDisplay");
            grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
            lastSel = id;
        }
    }
}

$(function () {
    $(".dialogDisplay").dialog({
        width: '50%',
        height: 700,
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
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
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
/////--------------detail information of process------------
$(function () {
    $("#list2").jqGrid
    ({
        url: "/ActualWO/getActualdetail",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'olddno', name: 'olddno', width: 80, align: 'center', hidden: true },
            { label: 'fff', name: 'oldd_no', width: 80, align: 'center', hidden: true },
            { label: 'Routing', name: 'line_no', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Date', name: 'work_dt', width: 100, align: 'center' },
            { label: 'Target', name: 'prod_qty', width: 80, align: 'right', formatter: 'integer' },
            {
                label: 'Actual', name: 'done_qty', width: 82, align: 'right', editable: true, formatter: 'integer', editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                formatter: 'integer', editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });
                    }
                },
            },
            {
                label: 'Defective', name: 'refer_qty', width: 81, align: 'right', formatter: 'integer', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                formatter: 'integer', editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });
                    }
                },
            },
            { key: false, label: 'refer_qty', name: 'refer_qty', width: 81, align: 'right', hidden: true },
            { key: false, label: 'Efficiency', name: 'efficiency', width: 80, align: 'right', formatter: numberFormatter },
            { key: false, label: 'Composite', name: 'level_part', width: 80, align: 'right', formatter: composite_pp },

            { label: 'MT', name: 'material_check_yn', width: 100, align: 'center' },
            { label: 'Machine', name: 'machine_check_yn', width: 110, align: 'center' },
            { label: 'Staff', name: 'staff_check_yn', width: 110, align: 'center' },

            { label: 'Staff', name: 'material_cd', width: 110, align: 'center', hidden: true },
            { label: 'Staff', name: 'machine_cd', width: 110, align: 'center', hidden: true },
            { label: 'Staff', name: 'staff_cd', width: 110, align: 'center', hidden: true },

            { label: 'Stand by', name: 'standby', width: 110, align: 'center', formatter: standby },
            { label: 'QC Code', name: 'item_vcd', width: 110, align: 'center', formatter: qc_code_pp },
            { label: 'QC Code', name: 'item_vcd', width: 110, align: 'center', hidden: true },
            { label: 'QC Qty', name: 'qc_qty', width: 110, align: 'right', formatter: 'integer' },
            { label: 'QC Rate', name: 'qc_rate', width: 110, align: 'right', formatter: 'integer' },

        ],
        pager: jQuery('#gridpager2'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: "250",
        width: null,
        rowNum: 50,
        caption: 'Detail information of process',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
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
        onSelectRow: function (id, rowid2, status, e) {

            if (id && id !== lastSel) {
                jQuery(this).restoreRow(lastSel);
                lastSel = id;
            }
            jQuery(this).editRow(id, true);

            //$(".CheckStandby").click(function () {
            //    var olddno = $(this).data("olddno");
            //    var material_cd = $(this).data("material_cd");
            //    var machine_cd = $(this).data("machine_cd");
            //    var staff_cd = $(this).data("staff_cd");

            //    $("#checkMaterial").val(material_cd);
            //    $("#checkmechin").val(machine_cd);
            //    $("#checksaff").val(staff_cd);
            //    $("#olddno_day").val(olddno);

            //    var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            //    row_id = $("#list").getRowData(selectedRowId);


            //    var fno = row_id.fno;

            //    //console.log(olddno, fno, material_cd);
            //    Check(olddno, fno);
            //});

            var selectedRowId2 = $("#list2").jqGrid("getGridParam", "selrow");
            row_id2 = $("#list2").getRowData(selectedRowId2);
            //getViewItemQC(row_id2.olddno, row_id2.item_vcd);
            $('#qc_item_vcd').val(row_id2.item_vcd);

        },
        subGrid: true,
        subGridRowExpanded: function (subgrid_id, row_id) {
            var subgrid_table_id;

            subgrid_table_id = subgrid_id + "_t";
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/Plan/Getdaytime?id=" + row_id,
                datatype: 'json',
                colModel: [
                 { key: true, label: 'olddtno', name: 'olddtno', align: 'center', hidden: true },
                    { label: "olddt_no", name: "olddt_no", width: 110, align: "center", hidden: true },
                    { label: "oldd_no", name: "oldd_no", width: 110, align: "center", hidden: true },
                    { label: "Process no", name: "process_no", width: 92, align: "center", },
                    { label: "Process type", name: "process_type", width: 87, align: "center" },
                    { label: "Start time", name: "start_time", width: 79, align: "center", },
                    { label: 'Target', name: 'prod_qty', width: 73, align: 'right' },
                    { label: 'Actual', name: 'done_qty', width: 82, align: 'right', },
                    { label: "Defect", name: "refer_qty", width: 81, align: 'right', },
                    { label: "Efficiency(%)", name: "rate", width: 95, align: 'right', formatter: numberFormatter },
                    { label: "Defect Rate(%)", name: "refer_rate", width: 95, align: 'right', formatter: numberFormatter },
                ],
                height: '100%',
                rowNum: 20,
                sortname: 'num',
                sortorder: "asc",
            });
        },
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
        if (cellvalue == null || cellvalue == "" || isNaN(cellvalue)) {
            kg2 = 0;
        }
        else if (cellvalue.toString().includes(".")) {
            kq = parseFloat(cellvalue).toFixed(2).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            kg2 = kq;
        } else if (cellvalue == "0 %") {
            kq = cellvalue.toString().replace(" %", "");

        }

        else {
            kq = cellvalue.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
            kg2 = kq;
        }


        return kg2 + " %"
    };


    $("#mbSave").click(function () {
        var i, n, rowData, selRowIds = $("#list2").jqGrid("getGridParam", "selarrrow"), id = "", v_done_qty = "", v_refer_qty = "", v_prod_qty = "";

        var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
        var row_id2 = $("#list2").getRowData(selectedRowId);

        var selectedRowId = $("#list1").jqGrid("getGridParam", 'selrow');
        var row_lisst1 = $("#list1").getRowData(selectedRowId);
        var level = row_lisst1.level;

        for (i = 0, n = selRowIds.length; i < n; i++) {
            id = selRowIds[i];

            v_done_qty = getCellValue(selRowIds[i], 'done_qty');
            v_refer_qty = getCellValue(selRowIds[i], 'refer_qty');
            v_prod_qty = getCellValue(selRowIds[i], 'prod_qty');

            if (id == "" || v_done_qty == "" || v_refer_qty == "") {
                continue;
            }

            $.ajax({
                type: 'post',
                url: '/ActualWO/ActualDetalEdit',
                headers: {
                    "Content-Type": "application/json",
                    "X-HTTP-Method-Override": "POST"
                },
                dataType: 'text',
                data: JSON.stringify({
                    olddno: id,
                    done_qty: v_done_qty,
                    refer_qty: v_refer_qty,
                    prod_qty: v_prod_qty
                }),
                //async: false,
                success: function () {
                    var refer_new = row_id2.refer_qty;
                    if (v_refer_qty != 0) {
                        $('.dialog').dialog('open');
                    }
                    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                },
                error: function (data) {
                },
            });
        }
    });
   
    $("#searchBtn").click(function () {
        var fo_no = $('#s_fo_no').val().trim();
            bom_no = $("#s_bom_no").val().trim(),
            style_no = $("#s_style_no").val().trim(),
            start = $("#start").val(),
            end = $("#end").val();

        $.ajax({
            url: "/ActualWO/searchActualWO",
            type: "get",
            dataType: "json",
            data: {
                fo_no: fo_no,
                bom_no: bom_no,
                style_no: style_no,
                start: start,
                end: end,
            },
            success: function (result) {
                $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                $("#list_excel").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
                $("#list1").clearGridData();
                $("#list2").clearGridData();
            }
        });
    });

});
//Stand by

function standby(cellValue, options, rowdata, action) {
    var olddno = rowdata.olddno;
    var material_cd = rowdata.material_cd;
    var machine_cd = rowdata.machine_cd;
    var staff_cd = rowdata.staff_cd;

    var html = '<button class="btn btn-xs btn-primary CheckStandby" onclick="Check(this);" data-olddno="' + olddno + '" data-material_cd="' + material_cd + '" data-machine_cd="' + machine_cd + '" data-staff_cd="' + staff_cd + '">Check</button>';
    return html;
}

function Check(e) {

    var olddno = $(e).data("olddno");
    var material_cd = $(e).data("material_cd");
    var machine_cd = $(e).data("machine_cd");
    var staff_cd = $(e).data("staff_cd");

    $("#checkMaterial").val(material_cd);
    $("#checkmechin").val(machine_cd);
    $("#checksaff").val(staff_cd);
    $("#olddno_day").val(olddno);

    var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
    row_id = $("#list").getRowData(selectedRowId);


    var fno = row_id.fno;

    $('.dialogStandby').dialog('open');
    $('#standMaterial').DataTable({
        "bServerSide": true,
        "ajax": {
            "url": "/ActualWO/GetActual_material?" + "fno=" + fno,
            "type": "GET",
            "datatype": "json"
        },
        "searching": false,
        "paging": false,
        "bInfo": false,
        "columns": [

            { "data": "process_type" },
            { "data": "mt_nm" },
            { "data": "width" },
            { "data": "need_qty" },
            { "data": "feed_size" },
            { "data": "feed_unit" },
            { "data": "div_cd" },

        ],
        'rowsGroup': [0],
        'columnDefs': [
           {
               "targets": 0, // your case first column
               "className": "text-center",
               "width": "10%"
           },
            {
                "targets": 2, // your case first column
                "className": "text-right",
                "width": "15%"
            },
            {
                "targets": 3, // your case first column
                "className": "text-right",
                "width": "15%"
            },
            {
                "targets": 4, // your case first column
                "className": "text-right",
                "width": "15%"
            },
        ],
        "bDestroy": true,
    });

    $('#standmechin').DataTable({
        "bServerSide": true,
        "ajax": {
            "url": "/ActualWO/getActualMachin?" + "fno=" + fno,
            "type": "GET",
            "datatype": "json"
        },
        "searching": false,
        "paging": false,
        "bInfo": false,
        "columns": [

            { "data": "prounit_cd" },
            { "data": "mc_type" },
            { "data": "mc_no" },
            { "data": "start_dt" },
            { "data": "end_dt" },

        ],
        'rowsGroup': [0, 1],
        'columnDefs': [
         {
             "targets": 0, // your case first column
             "className": "text-center",
             "width": "10%"
         },
          {
              "targets": 1, // your case first column
              "className": "text-center",
              "width": "15%"
          },
          {
              "targets": 2, // your case first column
              "className": "text-center",
              "width": "20%"
          },
          {
              "targets": 3, // your case first column
              "className": "text-center",
              "width": "15%"
          },
           {
               "targets": 4, // your case first column
               "className": "text-center",
               "width": "15%"
           },
        ],
        "bDestroy": true,
    });
    $('#standStaff').DataTable({
        "bServerSide": true,
        "ajax": {
            "url": "/ActualWO/GetActual_staff?" + "fno=" + fno,
            "type": "GET",
            "datatype": "json"
        },
        "searching": false,
        "paging": false,
        "bInfo": false,
        "columns": [
            { "data": "prounit_cd" },
            { "data": "staff_id" },
            { "data": "uname" },
            { "data": "dt_nm" },
            { "data": "start_dt" },
            { "data": "end_dt" },
        ],
        'rowsGroup': [0],
        'columnDefs': [
         {
             "targets": 0, // your case first column
             "className": "text-center",
             "width": "10%"
         },
          {
              "targets": 1, // your case first column
              "className": "text-left",
              "width": "15%"
          },
          {
              "targets": 2, // your case first column
              "className": "text-left",
              "width": "15%"
          },
          {
              "targets": 3, // your case first column
              "className": "text-left",
              "width": "15%"
          },
           {
               "targets": 4, // your case first column
               "className": "text-center",
               "width": "15%"
           },
            {
                "targets": 5, // your case first column
                "className": "text-center",
                "width": "15%"
            },
        ],
        "bDestroy": true,
    });
};
$(function () {
    $(".dialogStandby").dialog({
        width: '50%',
        height: 700,
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
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
        },
    });


    $('#CloseStandby').click(function () {
        $('.dialogStandby').dialog('close');

        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    });

});
// Khai báo URL stand của bạn ở đây
var GetCheck = "/ActualWO/GetCheckMaterial";

//check material - MMS006
$(document).ready(function () {
    _GetCheck();
    GetType_Marterial();
    GetType_mt_bom();
    $("#checkMaterial").on('change', function () {
        var id = $(this).val();
        if (id != undefined && id != '') {
        }
    });
});



function GetType_mt_bom() {
    var bom = $('#mt_bom_popup').val();
    var part = $('#cp_process_no').val();
    $.get("/ActualWO/GetType_mt_bom?bom=" + bom + "&part=" + part, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $(".gettype").empty();
            html += '<option value="" selected="selected">*MT*</option>';
            $.each(data, function (key, item) {
                html += '<option value="' + item.mt_no + '">' + item.mt_no + '</option>';
            });
            $("#mt_no_popup").html(html);
            $("#mt_no_popup1").html(html);
            
        }
    });
};
function _GetCheck() {

    $.get(GetCheck, function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#checkMaterial").html(html);
            $("#checkmechin").html(html);
            $("#checksaff").html(html);
        }
    });
}
$("#SaveStandby").click(function () {
    var checkMaterial = $('#checkMaterial').val();
    checkmechin = $("#checkmechin").val(),
    checksaff = $("#checksaff").val(),

    olddno = $("#olddno_day").val();

    $.ajax({
        url: "/ActualWO/ModifyLineCheckday",
        type: "get",
        dataType: "json",
        data: {
            olddno: olddno,
            checkMaterial: checkMaterial,
            checkmechin: checkmechin,
            checksaff: checksaff,
        },
        success: function (result) {
            alert("Save Success!!!");

        },
        error: function (result) {
            alert("error");
        }
    });
});

//End check material

//pp_actual

function actual_pp(cellValue, options, rowdata, action) {
    var fno = rowdata.fno;
    var html = '<button class="btn btn-xs btn-primary" onclick="actual(' + fno + ');">Actual</button>';
    return html;
}
function actual(fno) {
    $('.actual').dialog('open');

    $("#pp_fno").val(fno);

    $.get("/Plan/PartialView_Actual_Popup?fno=" + fno, function (html) {
        $("#PartialView_Actual_Popup").html(html);
    });


    $.get("/ActualWO/pp_actual1_wo" + "?id=" + fno, function (data) {

        var html1 = "WA-" + data.fo_no;
        $('#barcode_actual').append(html1);
        $('#barcode_actual').barcode(html1, "code128", { barWidth: 1, barHeight: 30 });

        $("#title_actual").html("<h4 class='red'>" + data.style_no + " " + '[' + data.style_nm + " " + ']' + "</h4>");

    });

}

$(function () {
    $(".actual").dialog({
        width: '70%',
        height: 1100,
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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

        },
    });


    $('#closeactual').click(function () {
        $('.actual').dialog('close');
    });

});
//plan_pp
function plan_pp(cellValue, options, rowdata, action) {
    var fno = rowdata.fno;
    var html = '<button class="btn btn-xs btn-primary" onclick="plan(' + fno + ');">Plan</button>';
    return html;
}
function plan(fno) {
    $('.plan').dialog('open');
    $("#pp_fno1").val(fno);

    $.get("/Plan/PartialView_Plan_Popup?fno=" + fno, function (html) {
        $("#PartialView_Plan_Popup").html(html);
    });
    $.get("/ActualWO/pp_actual1_wo" + "?id=" + fno, function (data) {

        var html1 = "WP-" + data.fo_no;
        $('#barcode_plan').append(html1);
        $('#barcode_plan').barcode(html1, "code128", { barWidth: 1, barHeight: 30 });

        $("#title_plan").html("<h4 class='red'>" + data.style_no + " " + '[' + data.style_nm + " " + ']' + "</h4>");

    });

}

$(function () {
    $(".plan").dialog({
        width: '70%',
        height: 1100,
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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

        },
    });


    $('#closeplan').click(function () {
        $('.plan').dialog('close');
    });

});

$("#Print_plan").on("click", function () {
    var id = $("#pp_fno1").val();
    window.open("/ActualWO/Printdetail_plan?" + "id=" + id, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});//end click
$("#Print_actual").on("click", function () {
    var id = $("#pp_fno").val();

    window.open("/ActualWO/Printdetail_actual?" + "id=" + id, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});//end click

//Qc Code
function qc_code_pp(cellValue, options, rowdata, action) {
    var item_vcd = rowdata.item_vcd;
    var olddno = rowdata.olddno;

    var html = '<button style="color: dodgerblue;border: none;background: none; " data-olddno="' + olddno + '" data-item_vcd="' + item_vcd + '" onclick="item_qc_check(this);">' + cellValue + '</button>';

    if (cellValue != null) {
        return html;
    }
    else {
        html = "";
        return html;
    }

};
function item_qc_check(e) {
    olddno = $(e).data("olddno");
    item_vcd = $(e).data("item_vcd");

    $('.dialog_qc_code_pp').dialog('open');
}
function getViewItemQC(olddno, item_vcd) {

    $.get("/ActualWO/popupm_day?olddno=" + olddno, function (data) {
        if (data != null && data != undefined && data.length) {
            $.each(data, function (key, item) {
                var date = item.work_ymd.substring(0, 4) + "-" + item.work_ymd.substring(4, 6) + "-" + item.work_ymd.substring(6, 8);

                $('#qc_fo_no').val(item.fo_no);
                $('#qc_line_no').val(item.line_no);
                $('#qc_process_no').val(item.process_no);
                $('#qc_prounit_cd').val(item.prounit_cd);
                $('#qc_date').val(date);

                $('#qc_olddno').val(item.olddno);
                $('#qc_oldd_no').val(item.oldd_no);
                $('#qc_qc_no').val(item_vcd);

            });
        }
    });
    $.get("/ActualWO/PartialView_View_QC_WEB?item_vcd=" + item_vcd + "&olddno=" + olddno, function (html) {

        $("#view_qc").html(html);
    });
    //$.get("/ActualWO/select_item_cd?item_vcd=" + item_vcd, function (data) {
    //    if (data != null && data != undefined && data.length) {
    //        var name1 = "";
    //        var name2 = "";
    //        var name3 = "";
    //        var name4 = "";
    //        var name5 = "";
    //        var html1 = "";
    //        var html2 = "";
    //        var html3 = "";
    //        var html4 = "";
    //        var html5 = "";
    //        var item_vcd = '';
    //        var check_id = '';

    //        var input = 1;
    //        var l = 0;
    //        var stt = 1;
    //        var id = "";
    //        var bien = 1;
    //        $.each(data, function (key, item) {
    //            item_vcd = item.item_vcd;

    //            check_id = item.check_id;
    //            if (item.check_type == "text") {
    //                if (id == "" || id != item.icno) {
    //                    id = item.icno;
    //                    no = item.icno;
    //                    html1 += '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
    //                    stt++;
    //                }
    //                html1 += '<h4>' + item.check_name + '</h4>';
    //                html1 += '<input type="text" name="input_text" id="text_' + input++ + '"  class="input-text form-control" value="">';
    //            }
    //            if (item.check_type == "select") {
    //                if (l == 0) {
    //                    html2 += '<select id="select_type" class="form-control">';
    //                    html2 += '&nbsp;&nbsp;&nbsp;<option value="" selected="selected">**</option>';
    //                    l++;
    //                }
    //                if (id == "" || id != item.icno) {
    //                    stt++;
    //                }
    //                name2 = '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
    //                html2 += '<option value="' + item.check_name + '">&nbsp;&nbsp;' + item.check_name + '</option>';
    //            }
    //            if (item.check_type == "radio") {
    //                if (id == "" || id != item.icno) {
    //                    stt++;
    //                }
    //                name3 = '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
    //                html3 += '<div id="radio-group" class="order_qty"> ';
    //                html3 += ' &nbsp;&nbsp;&nbsp;<input type="radio" name="radio" value="' + item.check_name + '"  />&nbsp;&nbsp;' + item.check_name + '</div>';
    //            }
    //            if (item.check_type == "check" || item.check_type == "range") {

    //                if (id == "" || id != item.icno) {

    //                    id = item.icno;
    //                    no = item.icno;
    //                    html4 += '<h4><th class="day_css">' + stt + ".&nbsp;&nbsp;&nbsp;" + item.check_subject + '</th><h4>';
    //                    html4 += ' <table style="width:100%" class="table_con"><tr>';
    //                    html4 += '<td class="title_qty-' + item.icno + ' title_qty" width="5%">' + '<input type="checkbox" name="checkbox1" id="checkAll-' + item.icno + '" value="" onclick="myFunctionAll(' + item.icno + ')">' + '<td>';
    //                    html4 += '<td class="title_qty"  width="35%">' + 'Qty' + '<td>';
    //                    html4 += '<td class="title_qty" width="60%">' + 'Content' + '<td>';
    //                    html4 += ' <tr> </table>';
    //                    stt++;
    //                }

    //                html4 += ' <table style="width:100%" class="table_con">';
    //                html4 += ' <tr>';
    //                html4 += '<td class="day_css" width="5%">' + ' <input type="checkbox" onclick="myFunction()" name="checkbox" id="dau-' + bien + '" class="abc-' + item.icno + '" value="0"   data-valuetwo="' + item.icdno + '"  />' + '</td>';
    //                html4 += '<td class="day_css" width="35%">' + ' <input type="text" name="checkbox_qty" id="abc-' + bien + '" onkeyup="TinhToan(' + bien + ')" value="0"  class=order-qty />' + '</td>';
    //                html4 += '<td class="day_css" width="60%">' + item.check_name + '</td>';
    //                html4 += ' </tr>';
    //                html4 += ' </table>';
    //            }
    //            if (item.check_type == "photo") {
    //                if (id == "" || id != item.icno) {
    //                    stt++;
    //                }
    //                name5 = '<h4>' + stt + '.&nbsp;&nbsp;&nbsp;' + item.check_subject + '</h4>';
    //                html5 += '<h4>' + item.check_name + '</h4>';
    //                html5 += ' &nbsp;&nbsp;&nbsp;   <input type="file" name="photo" id="photo" accept="image/gif, image/jpeg, image/png">';
    //            }

    //            bien++;
    //        });

    //        var tang = 0;
    //        $('#qc_item_vcd').val(item_vcd);
    //        $('#qc_qc_no').val(item_vcd);
    //        $('#qc_check_cd').val(check_cd);
    //        $('#input_text').val(input - 1);
    //        $('#qc_check_id').val(check_id);
    //        if (name1 != "") {
    //            tang = tang + 1;
    //            name1 = name1.replace("<h4>1.", "<h4>" + tang + ".");
    //        }
    //        if (name2 != "") {
    //            tang = tang + 1;
    //            name2 = name2.replace("<h4>2.", "<h4>" + tang + ".");
    //        }
    //        html2 += "</select>";
    //        if (name3 != "") {
    //            tang = tang + 1;
    //            name3 = name3.replace("<h4>3.", "<h4>" + tang + ".");
    //        }

    //        if (name4 != "") {
    //            tang = tang + 1;
    //            name4 = name4.replace("<h4>4.", "<h4>" + tang + ".");
    //        }
    //        if (name5 != "") {
    //            tang = tang + 1;
    //            name5 = name5.replace("<h4>5.", "<h4>" + tang + ".");
    //        }
    //        var html = name1 + html1 + name2 + html2 + name3 + html3 + name4 + html4 + name5 + html5;
    //        $("#view_qc").html(html);

    //        $(".list_qc").show();
    //    } else {
    //        var html = "";
    //        $(".list_qc").hide();
    //        $("#view_qc").html(html);
    //    }
    //});


}
$(".dialog_qc_code_pp").dialog({
    width: '60%',
    height: 800,
    maxWidth: 800,
    maxHeight: 450,
    minWidth: '60%',
    minHeight: 450,
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
    open: function () {
        functionGetmpoQc(olddno, item_vcd);
        functionGetmpoQc_Value();
        table_facline_qc(olddno, item_vcd);
        table_facline_qc_value();
        getViewItemQC(olddno, item_vcd);
    },
    close: function (event, ui) {
        $('.dialog_qc_code_pp').dialog('close');

        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    },
});
$(".dialog_composite").dialog({
    width: '60%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '60%',
    minHeight: 450,
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
    open: function () {

    },
    close: function (event, ui) {
        $('.dialog_composite').dialog('close');
    },
});

$(".dialog_product").dialog({
    width: '60%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '60%',
    minHeight: 450,
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
    open: function () {

    },
  
});
$('#close_qc').click(function () {
    $('.dialog_qc_code_pp').dialog('close');
    document.getElementById("c_qc_qty1").value = "0";
    document.getElementById("p_ok_qty").value = "0";
    $('input:checkbox').removeAttr('checked');
    a = 0;


    $("#list_qc").trigger("reloadGrid");
    $("#list_qc_value").clearGridData();

    jQuery("#list_facline_qc").trigger("reloadGrid");
    $("#list_facline_qc_value").clearGridData();

    $("#p_SaveQcfacline").attr("disabled", true);

});

$("#save_qc_code").on("click", function () {
    debugger;
    //số 2
    var listTable = $("#view_qc > div");
    var model = [];

    for (var i = 0; i < listTable.length; i++) {
        var item = listTable[i];
        var icno = $(item).data("icno");

        var itemObj = {};
        itemObj['icno'] = icno;

        var itemTr = [];

        var tr = $(item).find("tbody tr");
        for (var j = 0; j < tr.length; j++) {
            var trItem = tr[j];
            var icdno = $(trItem).data("icdno");
            var input = $(trItem).find("input").val();

            var objTr = {
                icdno: icdno,
                input: input
            };
            itemTr.push(objTr);
        }

        itemObj['objTr'] = itemTr;

        model.push(itemObj);

    }

    var models = JSON.stringify({
        'model': model,
        'oldhno': $('#qc_olddno').val(),
        'olddno': $('#qc_olddno').val(),
        'oldd_no': $('#qc_oldd_no').val(),
        'old_no': $('#qc_old_no').val(),
        'fo_no': $('#qc_fo_no').val(),
        'po_no': $('#qc_po_no').val(),
        'bom_no': $('#qc_bom_no').val(),
        'line_no': $('#qc_line_no').val(),
        'process_no': $('#qc_process_no').val(),
        'prounit_cd': $('#qc_prounit_cd').val(),
        'style_no': $('#qc_style_no').val(),
        'process_type': $('#qc_process_type').val(),
        'work_dt': $('#qc_date').val(),
        'item_vcd': $('#qc_item_vcd').val(),
        'check_qty': $('#c_qc_qty1').val(),
        'input_text': $('#qc_input_text').val(),
        'check_id': $('#qc_check_id').val(),
        'check_cd': $('#qc_check_cd').val(),
        'level': $('#qc_level').val(),
        'ok_qty': $('#p_ok_qty').val(),


 
    });

    $.ajax({
        type: 'Get',
        url: "/ActualWO/Insert_w_product_qc",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: models,
        cache: false,
        processData: false,
        success: function (result) {
            if (data.result == true) {
                document.getElementById("c_qc_qty1").value = "0";
                document.getElementById("p_ok_qty").value = "0";
                //$('input:checkbox').removeAttr('checked');
                //a = 0;
                //var id1 = data.pqno;
                //var id2 = data.fqno;
                jQuery("#list_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_qc_value").clearGridData();
                jQuery("#list_facline_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_facline_qc_value").clearGridData();
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
           
        }
    });
});


//$("#save_qc_code").click(function () {
//    var giatri = $('#input_text').val();
//    var m = parseInt(giatri);
//    var mang = [];
//    var order_qty = [];
//    for (var i = 0; i < m; i++) {
//        var number = i + 1
//        var id = '#text_' + number + '';
//        var x = $(id).val();
//        mang.push(x);
//    }
//    var checkbox = document.getElementsByName('checkbox');
//    for (var i = 0; i < checkbox.length; i++) {
//        if (checkbox[i].checked === true) {
//            order_qty.push(checkbox[i].value);
//            mang.push(checkbox[i].getAttribute("data-valuetwo"));
//        }
//    }
//    var radio = document.getElementsByName('radio');
//    for (var i = 0; i < radio.length; i++) {
//        if (radio[i].checked === true) {
//            mang.push(radio[i].value);
//        }
//    }
//    mang.push($('#select_type').val());
//    var formData = new FormData();
//    var photo = $("#photo").get(0);
//    if (photo != null) {
//        var files = $("#photo").get(0).files;
//        formData.append("file", files[0]);
//    }

//    formData.append("oldhno", $('#qc_olddno').val());
//    formData.append("olddno", $('#qc_olddno').val());
//    formData.append("oldd_no", $('#qc_oldd_no').val());
//    formData.append("old_no", $('#qc_old_no').val());
//    formData.append("fo_no", $('#qc_fo_no').val());
//    formData.append("po_no", $('#qc_po_no').val());
//    formData.append("bom_no", $('#qc_bom_no').val());
//    formData.append("line_no", $('#qc_line_no').val());
//    formData.append("process_no", $('#qc_process_no').val());
//    formData.append("prounit_cd", $('#qc_prounit_cd').val());
//    formData.append("style_no", $('#qc_style_no').val());
//    formData.append("process_type", $('#qc_process_type').val());
//    formData.append("work_dt", $('#qc_date').val());
//    formData.append("item_vcd", $('#qc_item_vcd').val());
//    formData.append("check_qty", $('#c_qc_qty1').val());
//    formData.append("input_text", $('#qc_input_text').val());
//    formData.append("check_id", $('#qc_check_id').val());
//    formData.append("check_cd", $('#qc_check_cd').val());
//    formData.append("level", $('#qc_level').val());
//    formData.append("ok_qty", $('#p_ok_qty').val());

//    debugger

//    $.ajax({
//        type: 'POST',
//        url: "/ActualWO/Insert_w_product_qc?mang=" + mang + "&order_qty=" + order_qty,
//        data: formData,
//        cache: false,
//        contentType: false,
//        processData: false,
//        success: function (data) {
//            document.getElementById("c_qc_qty1").value = "";
//            $('input:checkbox').removeAttr('checked');
//            a = 0;
//            var id1 = data.pqno;
//            var id2 = data.fqno;
//            jQuery("#list_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//            $("#list_qc_value").clearGridData();
//            jQuery("#list_facline_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//            $("#list_facline_qc_value").clearGridData();
//        }
//    });
//});
function myFunction() {
    if ($("#checkAll").prop('checked', true)) {
        // Get the checkbox
        document.getElementById("c_qc_qty1").value = "";
        var checkbox = document.getElementsByName('checkbox');
        var valuetwo = 0;
        var a = 0;
        for (var i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked === true) {
                valuetwo = checkbox[i].value;
                //valuetwo = checkbox[i].getAttribute("data-valuetwo");

                a += parseInt(valuetwo);
                document.getElementById("c_qc_qty1").value = a;
            }
        }
    }
}
function myFunctionAll(icno) {

    $('#checkAll-' + icno + '').change(function () {
        toggleCheckAll(this.checked)
        totalCount = calculateAll()
        //alert(totalCount);
        //$('input:checkbox[class=abc-' + icno + ']').not(this).prop('checked', this.checked);
    });
    $('input:checkbox[class=abc-' + icno + ']').change(function () {
        totalCount = calculateAll()
    });

    function toggleCheckAll(checked) {
        $('input:checkbox[class=abc-' + icno + ']').prop('checked', checked)
    }

    function calculateAll() {
        count = 0;
        $('input:checkbox[class=abc-' + icno + ']').each(function (index, checkbox) {
            if (checkbox.checked)
                count += parseInt(checkbox.value)
        })
        tong(count)
        return count;
    }
};
function tong(count) {
    var cont_1 = 0;
    cont_1 += count
    document.getElementById("c_qc_qty1").value = cont_1;
    return cont_1;
}
function TinhToan(bien) {
    var vat_cd = 0;
    var a = 0;

    vat_cd = Number(document.getElementById('abc-' + bien).value);
    document.getElementById('dau-' + bien).value = vat_cd;

    myFunction();
};

function functionGetmpoQc(olddno, item_vcd) {
    $("#list_qc").setGridParam({ url: "/ActualWO/GetWproductQc?olddno=" + olddno + "&item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
    $("#list_qc").jqGrid
   ({
       url: "/ActualWO/GetWproductQc?olddno=" + olddno + "&item_vcd=" + item_vcd,
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'pqno', name: 'pqno', width: 80, align: 'center', hidden: true },
           { key: false, label: 'PQ NO', name: 'pq_no', sortable: true, width: '100', align: 'center' },
           { key: false, label: 'Work Date', name: 'work_dt', sortable: true, width: '150', align: 'center' },
           { key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
           { key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
           { key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       gridComplete: function () {
           var rows = $("#list_qc").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_lct_nm = $("#list_qc").getCell(rows[i], "pq_no");
               var v_lct_nm1 = $("#list_qc").getCell(rows[i], "check_qty");
               if (v_lct_nm == "TOTAL") {
                   $("#list_qc").jqGrid('setRowData', rows[i], false, { background: '#FFE400' });
               }
           }
       },
       onSelectRow: function (rowid, selected, status, e) {
           var selectedRowId = $("#list_qc").jqGrid("getGridParam", 'selrow');
           row_id = $("#list_qc").getRowData(selectedRowId);
           $("#list_qc_value").clearGridData();
           $("#list_qc_value").setGridParam({ url: "/ActualWO/GetWproductQc_Value?" + "pq_no=" + row_id.pq_no, datatype: "json" }).trigger("reloadGrid");
       },

       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: "100%",

       width: null,
       rowNum: 50,
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
       gridview: true,
       shrinkToFit: false,
       loadonce: true,
       viewrecords: true,
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
}
function functionGetmpoQc_Value(olddno, item_vcd) {
    $("#list_qc_value").jqGrid
   ({
       url: "/ActualWO/GetWproductQc_Value",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'pqhno', name: 'pqhno', width: 80, align: 'center', hidden: true },
           { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
           { key: false, label: 'Check Value', name: 'check_value', sortable: true, width: '110', align: 'left' },
           {
               key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '65', editable: true,
               editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
               formatter: 'integer', editoptions: {
                   dataInit: function (element) {
                       $(element).keypress(function (e) {
                           if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                               return false;
                           }
                       });
                   }
               },
           },
       ],

       onCellSelect: editRow_qc,
       onSelectRow: function (rowid, selected, status, e) {
           $("#p_SaveQcDetail").attr("disabled", false);
       },

       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: '100%',
       width: null,
      
       rowNum: 50,
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
       gridview: true,
       shrinkToFit: false,
       loadonce: true,
       viewrecords: true,
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

   })

}

var lastSel;
function editRow_qc(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}

function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
$("#p_SaveQcDetail").on("click", function () {

    var i, selRowIds = $('#list_qc_value').jqGrid("getGridParam", "selarrrow"), n, rowData, check_qty = "";

    var selectedRowId = $("#list_qc").jqGrid("getGridParam", 'selrow');
    row_id = $("#list_qc").getRowData(selectedRowId);
    var pqno = row_id.pqno;
    var fruits = [];
    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        check_qty = getCellValue(selRowIds[i], 'check_qty');
        if (check_qty == undefined) {
            var ret = $("#list_qc_value").jqGrid('getRowData', selRowIds[i]);
            var giatri = ret.check_qty;
            fruits.push(giatri);
        } else {

            fruits.push(check_qty);
        }
    }

    $.ajax({
        type: "get",
        dataType: 'json',
        url: '/ActualWO/ModifyQcDetail?' + "id=" + selRowIds + "&" + "check_qty=" + fruits + "&" + "pqno=" + pqno,

        success: function (data) {
            $.each(data, function (key, item) {
                var id = item.pqhno;
                $("#list_qc_value").setRowData(id, item, { background: "#d0e9c6" });
                var id = item.pqno
                $("#list_qc").setRowData(id, item, { background: "#d0e9c6" });
            });

        },
        error: function (data) {
            alert("QR Barcode no exist!!!");
        },
    });
});
//end table w_product_qc
//start table m_facline_qc
function table_facline_qc(olddno, item_vcd) {
    $("#list_facline_qc").setGridParam({ url: "/ActualWO/Getfacline_qc?" + "olddno=" + olddno + "&item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
    $("#list_facline_qc").jqGrid
   ({
       url: "/ActualWO/Getfacline_qc?olddno=" + olddno + "&item_vcd=" + item_vcd,
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'fqno', name: 'fqno', width: 80, align: 'center', hidden: true },
           { key: false, label: 'FQ NO', name: 'fq_no', sortable: true, width: '120', align: 'center' },
           { key: false, label: 'Work Date', name: 'work_dt', sortable: true, width: '150', align: 'center' },
           { key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
           { key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
           { key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
       ],
       formatter: {
           integer: { thousandsSeparator: ",", defaultValue: '0' },
           currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
           number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
       },
       gridComplete: function () {
           var rows = $("#list_facline_qc").getDataIDs();
           for (var i = 0; i < rows.length; i++) {
               var v_lct_nm = $("#list_facline_qc").getCell(rows[i], "fq_no");
               var v_lct_nm1 = $("#list_facline_qc").getCell(rows[i], "check_qty");
               if (v_lct_nm == "TOTAL") {
                   $("#list_facline_qc").jqGrid('setRowData', rows[i], false, { background: '#FFE400' });
               }
           }
       },
       onSelectRow: function (rowid, selected, status, e) {
           var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
           row_id = $("#list_facline_qc").getRowData(selectedRowId);
           $("#list_facline_qc_value").clearGridData();
           $("#list_facline_qc_value").setGridParam({ url: "/ActualWO/Getfacline_qc_value?" + "fq_no=" + row_id.fq_no, datatype: "json" }).trigger("reloadGrid");

       },


       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: "100%",
       width: null,
       rowNum: 50,
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
       gridview: true,
       shrinkToFit: false,
       loadonce: true,
       viewrecords: true,
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
}
function table_facline_qc_value() {
    $("#list_facline_qc_value").jqGrid
   ({
       url: "/ActualWO/Getfacline_qc_value",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
           { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
           { key: false, label: 'Check Value', name: 'check_value', sortable: true, width: '110', align: 'left' },
           {
               key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', editable: true,
               editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
               formatter: 'integer', editoptions: {
                   dataInit: function (element) {
                       $(element).keypress(function (e) {
                           if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                               return false;
                           }
                       });
                   }
               },
           },
       ],

       onCellSelect: editRow_facline,
       onSelectRow: function (rowid, selected, status, e) {
           $("#p_SaveQcfacline").attr("disabled", false);
       },

       viewrecords: true,
       rowList: [50, 100, 200, 500],
       height: "100%",
       width: null,
       rowNum: 50,
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: false,
       gridview: true,
       shrinkToFit: false,
       loadonce: true,
       viewrecords: true,
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

   })

}
var lastSel;
function editRow_facline(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}

function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
$("#p_SaveQcfacline").on("click", function () {

    var i, selRowIds = $('#list_facline_qc_value').jqGrid("getGridParam", "selarrrow"), n, rowData, check_qty = "";

    var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
    row_id = $("#list_facline_qc").getRowData(selectedRowId);

    var fqno = row_id.fqno;
    var fruits = [];
    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        check_qty = getCellValue(selRowIds[i], 'check_qty');
        if (check_qty == undefined) {
            var ret = $("#list_facline_qc_value").jqGrid('getRowData', selRowIds[i]);
            var giatri = ret.check_qty;
            fruits.push(giatri);
        } else {

            fruits.push(check_qty);
        }
    }

    $.ajax({
        type: "get",
        dataType: 'json',
        url: '/ActualWO/ModifyFaclineQcDetail?' + "id=" + selRowIds + "&" + "check_qty=" + fruits + "&" + "fqno=" + fqno,

        success: function (data) {
            $.each(data, function (key, item) {
                var id = item.fqhno;
                $("#list_facline_qc_value").setRowData(id, item, { background: "#d0e9c6" });
                var id = item.fqno;
                $("#list_facline_qc").setRowData(id, item, { background: "#d0e9c6" });
            });
        },
        error: function (data) {
            alert("QR Barcode no exist!!!");
        },
    });
});
$(document).ready(function (e) {
    $('#excelBtn').click(function (e) {
        $("#list_excel").jqGrid('exportToExcel',
             options = {
                 includeLabels: true,
                 includeGroupHeader: true,
                 includeFooter: true,
                 fileName: "Web.xlsx",
                 mimetype: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 maxlength: 40,
                 onBeforeExport: null,
                 replaceStr: null
             }
         );
    });
});
$(document).ready(function (e) {
    $('#htmlBtn').click(function (e) {
        $("#list_excel").jqGrid('exportToHtml',
            options = {
                title: '',
                onBeforeExport: null,
                includeLabels: true,
                includeGroupHeader: true,
                includeFooter: true,
                tableClass: 'jqgridprint',
                autoPrint: false,
                topText: '',
                bottomText: '',
                fileName: "Web",
                returnAsString: false
            }
         );
    });

});



//composite
function composite_pp(cellValue, options, rowdata, action) {
    var date = rowdata.work_dt;
    var html = "";
    if (cellValue == "1") { html = '<button class="btn btn-xs btn-primary" data-olddno="' + rowdata.olddno + '"  data-refer_qty="' + rowdata.refer_qty + '"  data-prod_qty="' + rowdata.prod_qty + '" data-date="' + date + '" onclick="product(this);">Product</button>'; }
    if (cellValue == "0") { html = '<button class="btn btn-xs btn-primary" data-olddno="' + rowdata.olddno + '"  data-refer_qty="' + rowdata.refer_qty + '"  data-prod_qty="' + rowdata.prod_qty + '" data-date="' + date + '" onclick="composite_dt(this);">Composite</button>'; }
    return html;
}

function composite_dt(e) {
    date = $(e).data("date");
    $('#cp_date').val(date);
    var fo_no = $('#cp_fo_no').val();
    var part_no = $('#cp_process_no').val();
    var prounit_cd = $('#cp_process_nm').val();
    var lct = $('#cp_lct_cd').val();
    var style_no = $('#cp_style_no').val();
    var bom_no = $('#cp_bom_no').val();
    getData_mt(date, fo_no, part_no, prounit_cd, lct, style_no, bom_no)
    $('.dialog_composite').dialog('open');

    //update_actual
    $('#ud_id').val($(e).data("olddno"));
    $('#ud_refer_qty').val($(e).data("refer_qty"));
    $('#ud_prod_qty').val($(e).data("prod_qty"));


}
function product(e) {
    date = $(e).data("date");
    $('#pp_date').val(date);
    var fo_no = $('#pp_fo_no').val();
    var lct = $('#pp_lct_cd').val();
    var style_no = $('#pp_style_no').val();
    var bom_no = $('#pp_bom_no').val();
    var part_no = $('#pp_process_no').val();
    var prounit_cd = $('#cp_process_nm').val();
    getData_Pr(date, fo_no, lct, style_no, bom_no, part_no, prounit_cd);
    $('.dialog_product').dialog('open');

    //update_actual
    $('#ud_id').val($(e).data("olddno"));
    $('#ud_refer_qty').val($(e).data("refer_qty"));
    $('#ud_prod_qty').val($(e).data("prod_qty"));

}
$('#close_product').click(function () {
    $('.dialog_product').dialog('close');
    $('#pp_lot_pr').val('');
    $('#pp_pr_cd').val('');
    $('#cp_bb_no_pr').val('');
    $('#pp_gr_qty').val('');
    $('#cp_bb_no3').val('');
    //$('#tb_pr_cd').clearGridData();

    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    jQuery("#tb_pr_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
   
});
$('#close_composite').click(function () {
    $('.dialog_composite').dialog('close');
    $('#cp_lot').val('');
    $('#s_mt_cd').val('');
    $('#cp_bb_no').val('');
    $('#cp_gr_qty').val('');
    $('#cp_bb_no2').val('');
    //$('#tb_mt_cd').clearGridData();

    jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    jQuery("#tb_mt_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
});

$grid = $("#tb_mt_lot").jqGrid
({
    datatype: 'json',
    mtype: 'Get',
    colModel: [
         { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
        { label: 'ML No', name: 'mt_cd', width: 400, sortable: true, },
         {
             label: 'GR QTY', name: 'gr_qty', width: 81, align: 'right', formatter: 'integer', editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
             formatter: 'integer', editoptions: {
                 dataInit: function (element) {
                     $(element).keypress(function (e) {
                         if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                             return false;
                         }
                     });

                     $(element).keydown(function (e) {
                         if (e.which == 13) {
                             $("#gr_qty_update").val(this.value);
                         }
                     });
                 }
             },
         },
           {
               label: 'Status', name: 'bbmp_sts_cd', width: 100, align: 'left', cellattr: function (rowId, value, rowObject, colModel, arrData) {
                   if (value == 'Mapping') {
                       return 'style=background-color:#B7F0B1;';
                   }
                   if (value == 'UnMapping') {
                       return 'style=background-color:#B2CCFF;';
                   }
                   else {
                       return value;
                   }
               }
           },
            { label: 'Bobbin ', name: 'bb_no', width: 150, align: 'center' },
        { label: 'MT CNT', name: 'count_table2', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Action', name: '', width: 100, align: 'center', formatter: Save_gr },
         
    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    onSelectRow: function (id, rowid, status, e) {
        //var lastSelection = "";
        //if (id && id !== lastSelection) {

        //    var grid = $("#tb_mt_lot");
        //    grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        //    lastSelection = id;

        //}
        var selectedRowId = $("#tb_mt_lot").jqGrid("getGridParam", 'selrow');
        row_id = $("#tb_mt_lot").getRowData(selectedRowId);

        $('#id_mt_cm').val(row_id.wmtid);
        $('#cp_lot').val(row_id.mt_cd);

        $('#id_print').val(row_id.wmtid);
        var bom_no = $('#cp_bom_no').val();
        var part = $('#cp_process_no').val();
        $("#tb_mt_cd").setGridParam({ url: "/ActualWO/ds_mapping_w?" + "mt_lot=" + row_id.mt_cd + "&bom_no=" + bom_no + "&part=" + part, datatype: "json" }).trigger("reloadGrid");

    },
    pager: jQuery('#tb_mt_lot_page'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
  
    loadonce: true,
    height: 200,
    width: null,
   
    onCellSelect: function (rowid) {
        var lastSel = "";
        if (rowid && rowid !== lastSel) {
            //var grid = $("#tb_mt_lot");
            //grid.jqGrid('editRow', rowid, { keys: true, focusField: 4 });
            //lastSel = rowid;


            jQuery('#tb_mt_lot').restoreRow(lastSel);
            lastSel = rowid;
        }
        jQuery('#tb_mt_lot').editRow(rowid, true);
    },



    //onSelectRow: function (id) {
    //    if (id && id !== lastSel) {
    //        jQuery('#gridid').restoreRow(lastSel);
    //        lastSel = id;
    //    }
    //    jQuery('#gridid').editRow(id, true);
    //},


    rownumbers: true,
    shrinkToFit: false,
    viewrecords: true,
    caption: 'Composite',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    multiselect: true,
});

//var lastSel;
//function editRow_h(id) {
//    if (id && id !== lastSel) {
//        var grid = $("#tb_mt_lot");
//        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
//        lastSel = id;
//    }
//}

function getData_mt(date, fo_no, part_no, prounit_cd, lct, style_no, bom_no) {
    var grid = $("#tb_mt_lot");
    grid.jqGrid('setGridParam', { search: true });
    var postData = grid.jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = postData.rows;
    params.sidx = postData.sidx;
    params.sord = postData.sord;
    params._search = postData._search;
    params.date = date;
    params.fo_no = fo_no;
    params.part_no = part_no;
    params.prounit_cd = prounit_cd;
    params.lct = lct;
    params.style_no = style_no;
    params.bom_no = bom_no;

    $.ajax({
        url: '/ActualWO/getmt_date_web',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#tb_mt_lot")[0];
                grid.addJSONData(data);
            }
        }
    })
}


$grid = $("#tb_mt_cd").jqGrid
({
    datatype: 'json',
    mtype: 'Get',
    colModel: [
         { key: true, label: 'wmmid', name: 'wmmid',hidden:true},
        { label: 'mt_lot', name: 'mt_lot', width: 400, hidden: true },
        { label: 'ML No', name: 'mt_cd', width: 400, sortable: true },
        { label: 'MT NO', name: 'mt_no', width: 200, },
        { label: 'Use', name: 'use_yn', width: 50, align: 'center' },
        { label: 'Status', name: 'bbmp_sts_cd', width: 100, },
        { label: 'Location', name: 'lct_cd', width: 100, },
        { label: 'Location Status', name: 'lct_sts_cd', width: 100, },
    ],
    pager: jQuery('#tb_mt_cd_page'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    sortable: true,
    loadonce: true,
    height: 200,
    multiselect: true,
    rownumbers: true,
    width: null,
    shrinkToFit: false,
    viewrecords: true,
    caption: 'Material',
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

$("#create_composite").click(function () {
    if ($("#cp_gr_qty").val().trim() == "") {
        alert("Please enter your Group Qty");
        $("#cp_gr_qty").val("");
        $("#cp_gr_qty").focus();
        return false;
    } else {
        $.ajax({
            url: "/ActualWO/insertw_material",
            type: "get",
            dataType: "json",
            data: {
                mt_no: $("#cp_style_no").val(),
                fo_no: $("#cp_fo_no").val(),
                index: $("#cp_lct_cd").val(),
                prounit_cd: $("#cp_process_nm").val(),
                date: $("#cp_date").val(),
                gr_qty: $("#cp_gr_qty").val(),
                bom_no: $("#cp_bom_no").val(),
            },
            success: function (data) {
                var id = data.ds1.wmtid;
                $("#tb_mt_lot").jqGrid('addRowData', id, data.ds1, 'first');
                $("#tb_mt_lot").setRowData(id, false, { background: "#d0e9c6" });
                update_actual(data.gtri);

            },
            error: function (result) {
                alert('Bom Code dont have Part . Please check again');
            }
        });
    }
});

//$("#save_composite").click(function () {

//    if ($("#cp_bb_no2").val().trim() == "" && ($("#cp_lot").val().trim() == "")) {
//        alert("Please enter your Bobin");
//        $("#cp_bb_no2").val("");
//        $("#cp_bb_no2").focus();
//        return false;
//    }
//    if ($("#cp_lot").val().trim() == "") {
//        alert("Please enter your Composite Lot");
//        $("#cp_lot").val("");
//        $("#cp_lot").focus();
//        return false;
//    } else {
//        var bom_no = $('#cp_bom_no').val();
//        var part = $('#cp_process_no').val();
//        var bb_no = $('#cp_bb_no2').val();
//        $.ajax({
//            url: "/ActualWO/insertw_material_mping",
//            type: "get",
//            dataType: "json",
//            data: {
//                mt_cd: $("#cp_lot").val().trim(),
//                qr_code: $("#s_mt_cd").val().trim(),
//                bom_no: bom_no,
//                part: part,
//                bb_no :bb_no,
//                prounit_cd: $("#cp_process_nm").val().toString().replace(part+"-", ""),
//            },
//            success: function (response) {
//                if (response.list) {
//                    var allRowsInGrid = $('#tb_mt_cd').jqGrid('getGridParam', 'data');
//                    for (var i = 0; i < allRowsInGrid.length; i++) {
//                        if ((allRowsInGrid[i].use_yn == null || allRowsInGrid[i].use_yn == "Y") && allRowsInGrid[i].mt_no == response.list.mt_no) {
//                            jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

//                            var id = allRowsInGrid[i].wmmid;
//                            $("#tb_mt_cd").jqGrid('delRowData', id);
//                            $("#tb_mt_cd").jqGrid('addRowData', response.list.wmmid, response.list, 'first');
//                            $("#tb_mt_cd").setRowData(response.list.wmmid, false, { background: "#d0e9c6" });
                          
//                        }
//                    }
//                    if (!response.kq == "") {
//                        jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                        var id2 = response.kq.wmmid;
//                        $("#tb_mt_cd").jqGrid('addRowData', id2, response.kq, 'last');
//                        $("#tb_mt_cd").setRowData(id2, false, { background: "#d0e9c6" });
//                        jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                    }
//                } else {
//                    alert(response.message);
                    
//                }
//            }
//        });
//    }
//});

$("#Delete").click(function () {
    var i, n, rowData, selRowIds = $("#tb_mt_cd").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
    $.ajax({
        url: "/ActualWO/Delete_mapping?id=" + selRowIds,
        type: "get",
        dataType: "json",
        success: function (data) {
            jQuery("#tb_mt_cd").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (result) {
            alert('Does not exits');
        }
    });
});
$("#Finish").click(function () {
    var bb_no = $('#cp_bb_no2').val();
    var i, n, rowData, selRowIds = $("#tb_mt_cd").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
    $.ajax({
        url: "/ActualWO/chane_mapping?id=" + selRowIds + "&bb_no=" + bb_no,
        type: "get",
      
        dataType: "json",
        success: function (result) {
            console.log(result);
            for (i = 0, n = selRowIds.length; i < n; i++) {
                var id = result.data[i].wmmid;
                $("#tb_mt_cd").jqGrid('delRowData', id);
                $("#tb_mt_cd").jqGrid('addRowData', id, result.ds[i], 'first');
                $("#tb_mt_cd").jqGrid('addRowData', id, result.data1[i], 'last');
                $("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
            }
        },
        error: function (result) {
            alert('Does not exits');
        }
    });
});
function Save_gr(cellValue, options, rowdata, action) {

    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-primary button-srh" data-id="' + id + '"onclick="update_gr(this);">Save</button>';
    return html;
}
function update_gr(e) {
    var chuoi = $(e).data("id") + "_gr_qty";
    var gr = $("#" + chuoi + "").val();
    var giatri = gr;
    var bien = $("#gr_qty_update").val();
    if ((giatri == "") || (giatri == undefined)) {
        giatri = bien;
    }
    $.ajax({
        url: '/ActualWO/update_w_material_gr?id=' + $(e).data("id") + "&gr_qty=" + giatri,
        type: "get",
        dataType: "json",
        success: function (data) {
            var id = data.ds1.wmtid;
            $("#tb_mt_lot").setRowData(id, data.ds1, { background: "#d0e9c6" });
            
            update_actual(data.ds);
        },
        error: function (data) {
            alert('erro');
        }
    });
}
$("#Print_qr").click(function () {
    var i, selRowIds = $('#tb_mt_lot').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        window.open("/Lot/Printcomposite?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        alert("Please select Grid.");
    }
    //var id = $('#id_print').val();
    //window.open("/Lot/Printcomposite?" + "id=" + id, "PRINT", "width=500, height=800, left=0, top=100, location=no, status=no,")
});
//popup product
$("#Print_qr_pr").click(function () {
    var i, selRowIds = $('#tb_pr_lot').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        window.open("/Lot/Printprd_lot?id=" + selRowIds, "PRINT", "width=600, height=800, left=200, top=100, location=no, status=no")
    }
    else {
        alert("Please select Grid.");
    }

    //var id = $('#id_print_pr').val();
    //window.open("/Lot/Printprd_lot?" + "id=" + id, "PRINT", "width=500, height=800, left=0, top=100, location=no, status=no,")
});

$grid = $("#tb_pr_lot").jqGrid
({
    datatype: 'json',
    mtype: 'Get',
    colModel: [
         { key: true, label: 'pno', name: 'pno', align: 'center' ,hidden:true},
        { label: 'PL NO', name: 'prd_lcd', width: 400, },
         {
             label: 'GR QTY', name: 'gr_qty', width: 81, align: 'right', formatter: 'integer', editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
             formatter: 'integer', editoptions: {
                 dataInit: function (element) {
                     $(element).keypress(function (e) {
                         if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                             return false;
                         }
                     });

                     $(element).keydown(function (e) {
                         if (e.which == 13) {
                             $("#gr_qty_update_pr").val(this.value);
                         }
                     });
                 }
             },
         },
            { label: 'Bobbin ', name: 'bb_no', width: 150, align: 'center' },
        { label: 'MT CNT', name: 'count_table2', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Action', name: '', width: 100, align: 'center', formatter: Save_pr },

    ],
    formatter: {
        integer: { thousandsSeparator: ",", defaultValue: '0' },
        currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
        number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
    },
    onSelectRow: function (id, rowid, status, e) {
        var selectedRowId = $("#tb_pr_lot").jqGrid("getGridParam", 'selrow');
        row_id = $("#tb_pr_lot").getRowData(selectedRowId);
        $('#id_mt_pr').val(row_id.pno);

        $('#pp_lot_pr').val(row_id.prd_lcd);
        $('#id_print_pr').val(row_id.pno);
        var bom_no = $('#pp_bom_no').val();
        var part = $('#pp_process_no').val();
        $("#tb_pr_cd").setGridParam({ url: "/ActualWO/ds_mapping_w_pr?" + "prd_lcd=" + row_id.prd_lcd + "&bom_no=" + bom_no + "&part=" + part, datatype: "json" }).trigger("reloadGrid");

        $("#packing_qr").attr("data-pno", row_id.pno);

    },
    pager: jQuery('#tb_pr_lot_page'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    sortable: true,
    loadonce: true,
    height: 200,
    width: null,
    onCellSelect: function (rowid) {
        var lastSel = "";
        if (rowid && rowid !== lastSel) {
            jQuery('#tb_pr_lot').restoreRow(lastSel);
            lastSel = rowid;
        }
        jQuery('#tb_pr_lot').editRow(rowid, true);
    },
    rownumbers: true,
    shrinkToFit: false,
    viewrecords: true,
    caption: 'Product',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    multiselect: true,
});

function getData_Pr(date, fo_no, lct, style_no, bom_no, part_no, prounit_cd) {
    var grid = $("#tb_pr_lot");
    grid.jqGrid('setGridParam', { search: true });
    var postData = grid.jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = postData.rows;
    params.sidx = postData.sidx;
    params.sord = postData.sord;
    params._search = postData._search;
    params.date = date;
    params.fo_no = fo_no;
    params.lct = lct;
    params.style_no = style_no;
    params.part_no = part_no;
    params.bom_no = bom_no;
    params.prounit_cd = prounit_cd;

    $.ajax({
        url: '/ActualWO/getpp_pr_lot',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#tb_pr_lot")[0];
                grid.addJSONData(data);
            }
        }
    })
}

function Save_pr(cellValue, options, rowdata, action) {

    var id = rowdata.pno;
    html = '<button  class="btn btn-sm btn-primary button-srh" data-id="' + id + '"onclick="update_pr(this);">Save</button>';
    return html;
}
function update_pr(e) {
    debugger;
    var chuoi = $(e).data("id") + "_gr_qty";
    var gr = $("#" + chuoi + "").val();
    var giatri = gr;
    var bien = $("#gr_qty_update_pr").val();
    if ((giatri == "") || (giatri == undefined)) {
        giatri = bien;
    }
    $.ajax({
        url: '/ActualWO/update_pr_gr?id=' + $(e).data("id") + "&gr_qty=" + giatri,
        type: "get",
        dataType: "json",
        success: function (data) {
            var id = data.data1.pno;
            $("#tb_pr_lot").setRowData(id, data.data1, { background: "#d0e9c6" });
            update_actual(data.gtri);

        },
        error: function (data) {
            alert('erro');
        }
    });
}
$grid = $("#tb_pr_cd").jqGrid
({
    datatype: 'json',
    mtype: 'Get',
    colModel: [
         { key: true, label: 'pmno', name: 'pmno',hidden:true},
        { label: 'ML No', name: 'mt_cd', width: 400, },
        { label: 'MT NO', name: 'mt_no', width: 200, },
        { label: 'Use', name: 'use_yn', width: 50, align: 'center' },
    ],
    pager: jQuery('#tb_pr_cd_page'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    sortable: true,
    loadonce: true,
    height: 200,
    multiselect: true,
    rownumbers: true,
    width: null,
    shrinkToFit: false,
    viewrecords: true,
    caption: 'Material',
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

$("#create_product").click(function () {
    if ($("#pp_gr_qty").val().trim() == "") {
        alert("Please enter your Group Qty");
        $("#pp_gr_qty").val("");
        $("#pp_gr_qty").focus();
        return false;
    } else {
        $.ajax({
            url: "/ActualWO/insertw_product",
            type: "get",
            dataType: "json",
            data: {
                mt_no: $("#pp_style_no").val(),
                fo_no: $("#pp_fo_no").val(),
                lct: $("#pp_lct_cd").val(),
                prd_dt: $("#pp_date").val(),
                gr_qty: $("#pp_gr_qty").val(),
                bom_no: $("#pp_bom_no").val(),
                    part_no : $('#pp_process_no').val(),
                    process_cd: $('#cp_process_nm').val(),
            },
            success: function (data) {
                var id = data.data1.pno;
                $("#tb_pr_lot").jqGrid('addRowData', id, data.data1, 'first');
                $("#tb_pr_lot").setRowData(id, false, { background: "#d0e9c6" });
                update_actual(data.gtri);

            },
            error: function (result) {
                alert('Bom Code dont have Part. Please check again');
            }
        });
    }
});

//$("#save_composite").click(function () {

//    if ($("#cp_bb_no2").val().trim() == "" && ($("#cp_lot").val().trim() == "")) {
//        alert("Please enter your Bobin");
//        $("#cp_bb_no2").val("");
//        $("#cp_bb_no2").focus();
//        return false;
//    }
//    if ($("#cp_lot").val().trim() == "") {
//        alert("Please enter your Composite Lot");
//        $("#cp_lot").val("");
//        $("#cp_lot").focus();
//        return false;
//    } else {
//        var bom_no = $('#cp_bom_no').val();
//        var part = $('#cp_process_no').val();
//        var bb_no = $('#cp_bb_no2').val();
//        $.ajax({
//            url: "/ActualWO/insertw_material_mping",
//            type: "Get",
//            dataType: "json",
//            data: {
//                mt_cd: $("#cp_lot").val(),
//                qr_code: $("#s_mt_cd").val(),
//                bom_no: bom_no,
//                part: part,
//                bb_no: bb_no,
//                prounit_cd: $("#cp_process_nm").val().toString().replace(part + "-", ""),
//            },
//            success: function (response) {
//                if (response.list) {
//                    var allRowsInGrid = $('#tb_mt_cd').jqGrid('getGridParam', 'data');
//                    for (var i = 0; i < allRowsInGrid.length; i++) {
//                        if ((allRowsInGrid[i].use_yn == null || allRowsInGrid[i].use_yn == "Y") && allRowsInGrid[i].mt_no == response.list.mt_no) {
//                            jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

//                            var id = allRowsInGrid[i].wmmid;
//                            $("#tb_mt_cd").jqGrid('delRowData', id);
//                            $("#tb_mt_cd").jqGrid('addRowData', response.list.wmmid, response.list, 'first');
//                            $("#tb_mt_cd").setRowData(response.list.wmmid, false, { background: "#d0e9c6" });

//                        }
//                    }
//                    if (!response.kq == "") {
//                        jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                        var id2 = response.kq.wmmid;
//                        $("#tb_mt_cd").jqGrid('addRowData', id2, response.kq, 'last');
//                        $("#tb_mt_cd").setRowData(id2, false, { background: "#d0e9c6" });
//                        jQuery("#popupMaterial").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
//                    }
//                } else {
//                    alert(response.message);

//                }
//            }
//        });
//    }
//});

$("#save_product").click(function () {
    if ($("#cp_bb_no2").val().trim() == "" && ($("#pp_lot_pr").val().trim() == "")) {
        alert("Please enter your Bobin");
        $("#cp_bb_no2").val("");
        $("#cp_bb_no2").focus();
        return false;
    }
   
    if ($("#pp_lot_pr").val().trim() == "") {
        alert("Please enter your Product Lot");
        $("#pp_lot_pr").val("");
        $("#pp_lot_pr").focus();
        return false;
    } else {
        var bom_no = $('#pp_bom_no').val();
        var part = $('#pp_process_no').val();
        var bb_no = $('#cp_bb_no3').val();
        $.ajax({
            url: "/ActualWO/insertpr_mt_mping",
            type: "get",
            dataType: "json",
            data: {
                mt_cd: $("#pp_lot_pr").val().trim(),
                qr_code: $("#pp_pr_cd").val().trim(),
                bom_no: bom_no,
                part: part,
                process_cd: $('#cp_process_nm').val(),
                bb_no: bb_no,
            },
            success: function (response) {
               
                if (response.a) {
                    var allRowsInGrid = $('#tb_pr_cd').jqGrid('getGridParam', 'data');
                    for (var i = 0; i < allRowsInGrid.length; i++) {
                        if ((allRowsInGrid[i].use_yn == null || allRowsInGrid[i].use_yn == "Y") && allRowsInGrid[i].mt_no == response.a.mt_no) {
                            var id = allRowsInGrid[i].pmno;
                            $("#tb_pr_cd").jqGrid('delRowData', id);
                            $("#tb_pr_cd").jqGrid('addRowData', response.a.pmno, response.a, 'first');
                            $("#tb_pr_cd").setRowData(response.a.pmno, false, { background: "#d0e9c6" });
                        }
                    }
                    if (!response.kq == "") {
                        var id2 = response.kq.pmno;
                        $("#tb_pr_cd").jqGrid('addRowData', id2, response.kq, 'last');
                        $("#tb_pr_cd").setRowData(id2, false, { background: "#d0e9c6" });
                    }
                } else {
                    alert(response.message);
                }
                
            }
        });
    }
});
$("#Delete_pr").click(function () {
    var i, n, rowData, selRowIds = $("#tb_pr_cd").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
    $.ajax({
        url: "/ActualWO/Delete_mapping_pr?id=" + selRowIds,
        type: "get",
        dataType: "json",
        success: function (data) {
            jQuery("#tb_pr_cd").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        },
        error: function (result) {
            alert('Can not Delete');
        }
    });
});
$("#Finish_pr").click(function () {
    var bb_no = $('#cp_bb_no2').val();
    var i, n, rowData, selRowIds = $("#tb_pr_cd").jqGrid("getGridParam", "selarrrow"), id = "", reg_qty = "";
    $.ajax({
        url: "/ActualWO/chane_mapping_pr?id=" + selRowIds + "&bb_no=" + bb_no,
        type: "get",
        dataType: "json",

        success: function (result) {
            for (i = 0, n = selRowIds.length; i < n; i++) {
                var id = result.data[i].pmno;
                $("#tb_pr_cd").jqGrid('delRowData', id);
                $("#tb_pr_cd").jqGrid('addRowData', id, result.ds[i], 'first');
                $("#tb_pr_cd").jqGrid('addRowData', id, result.data[i], 'last');
                $("#tb_pr_cd").setRowData(id, false, { background: "#d0e9c6" });
            }
        },
        error: function (result) {
            alert('Can not Finish');
        }
    });
});

//ham_update_actual
function update_actual(sum_kq) {
        var id = $('#ud_id').val();
        var refer_qty = $('#ud_refer_qty').val();
        var prod_qty = $('#ud_prod_qty').val();
    $.ajax({
        type: 'post',
        url: '/ActualWO/ActualDetalEdit',
        headers: {
            "Content-Type": "application/json",
            "X-HTTP-Method-Override": "POST"
        },
        dataType: 'text',
        data: JSON.stringify({
            olddno: id,
            done_qty: sum_kq,
            refer_qty: refer_qty,
            prod_qty: prod_qty
        }),
        //async: false,
        success: function () {
            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            
        },
        error: function (data) {
        },
    });

}

//pp_packing qr

function btn_packingQR(e) {
    var that = $(e);
 
    var i, selRowIds = $('#tb_pr_lot').jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (selRowIds.length > 0) {
        $.get("/ActualWO/PartialView_Packing_qc?" +
     "pno=" + that.data("pno") ,
     function (html) {
         
         $("#PartialView_Packing_qc").html(html);
     });

        $('.popup-dialog.dialog_pp_packing').dialog('open');

    }
    else {
        alert("Please select Grid.");
    }

   
    
}
