$(function () {
    $(".dialog").dialog({
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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupStyle").jqGrid
            ({
                url: "/DevManagement/GetStyleMgt",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
                { key: false, label: 'Product Code', name: 'style_no', width: 200, align: 'left' },
                { key: false, label: 'Product Name', name: 'style_nm', width: 300, align: 'left' },
                { key: false, label: 'Model Code', name: 'md_cd', sortable: true, width: '300', align: 'left' },
                { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200', align: 'left' },
                { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180', align: 'left' },
                { key: false, label: 'Description', name: 'cav', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Packing Amount(EA)', name: 'pack_amt', editable: true, width: '150', align: 'right', formatter: 'integer' },
                { key: false, label: 'Type', name: 'bom_type', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'TDS no', name: 'tds_no', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'use_yn', name: 'use_yn', editable: true, width: '100px', hidden: true },
                { key: false, label: 'del_yn', name: 'del_yn', editable: true, width: '100px', hidden: true },
                { key: false, label: 'Create User', name: 'reg_id', index: 'reg_id', width: '100px', align: 'left' },
                { key: false, label: 'Create Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
                { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px', align: 'left' },
                { key: false, label: 'Change Date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '200' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupStyle").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupStyle").getRowData(selectedRowId);
                    //if (row_id != null) {
            
                    //}
                },

                pager: jQuery('#pagerStyle'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 250,
                width: $(".boxProduct").width(),
                loadonce: true,
                autowidth: false,
                caption: 'Style',
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

    $(".poupdialogStyle").click(function () {
        $('.dialog').dialog('open');
    });
    //$('#c_style_no').val(row_id.style_no);
  


    $("#selected1").click(function () {
        $('#m_style_no').val(row_id.style_no);
        $('.dialog').dialog('close');
    });

    $('#closestyle1').click(function () {
        $('.dialog').dialog('close');
    });

});