$(function () {
    $(".dialog").dialog({
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
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#popupStyle").jqGrid
            ({
                url: "/DevManagement/GetPopupStyle",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
                   { key: false, label: 'Style', name: 'style_no', width: 150, align: 'left' },
                   { key: false, label: 'Mode code', name: 'md_cd', sortable: true, width: '250', align: 'center', align: 'left' },
                   { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200' },
                   { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
                   { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px', align: 'center' },
                   { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
                   { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px', align: 'center' },
                   { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180', align: 'center' },
                    { key: false, label: 'Remark', name: 'cav', editable: true, width: '100px' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupStyle").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupStyle").getRowData(selectedRowId);
                    //if (row_id != null) {
                    //    $('#c_style_no').val(row_id.style_no);
                    //    $('#m_style_no').val(row_id.style_no);
                    //    $('.dialog').dialog('close');
                    //}
                },

                pager: jQuery('#pagerStyle'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                //width: $(".box-body").width() - 5,
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



    $("#selected1").click(function () {
        $('#m_style_no').val(row_id.style_no);
        $('.dialog').dialog('close');
    });


    $('#closestyle1').click(function () {
        $('.dialog').dialog('close');
    });

});