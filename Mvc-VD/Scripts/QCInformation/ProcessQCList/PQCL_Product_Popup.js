$(function () {
    var row_id;
    $(".dialog2").dialog({
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
        open: function (event, ui) {

            $("#popups2").jqGrid
            ({
                url: "/Monitoring/getStyle",
                datatype: 'json',
                mtype: 'Get',
                colModel: [

                    { key: false, label: 'Product No', name: 'style_no', width: 150, align: 'left' },
                    { key: false, label: 'Product Name', name: 'style_nm', width: 250, align: 'left' },
                                                         { key: false, label: 'BOM NO', name: 'bom_no', width: 150, align: 'center', formatter: bom_no },
                    { key: false, label: 'Model', name: 'md_cd', width: 300, align: 'left' },
                                        { key: false, label: 'Project Name', name: 'prj_nm', width: 150, align: 'left' },
                       { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popups2").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popups2").getRowData(selectedRowId);
                    //if (row_id != null) {
                    //    $('#s_bom_no').val(row_id.bom_no);
                    //    //$('.dialog2').dialog('close');
                    //}
                },

                pager: jQuery('#popupspager2'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
                width: 902,
                autowidth: false,
                caption: 'Product Management',
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

    $(".poupdialog2").click(function () {
        $('.dialog2').dialog('open');
    });


    //$('#closestyle2').click(function () {
    //    $('.dialog2').dialog('close');

    //});
    $('#savestyle2').click(function () {
        if (row_id != null) {
            $('#style_no').val(row_id.style_no);
            $('.dialog2').dialog('close');
        }

    });

    $("#searchBtn_popup_s").click(function () {

        var bom_no = $("#s_bom_no_popup_s").val().trim(),
            style_no = $("#s_style_no_popup_s").val().trim(),
            model_no = $("#s_model_no_popup_s").val().trim();
        $.ajax({
            url: "/Monitoring/searchStyle",
            type: "get",
            dataType: "json",
            data: {
                bom_no: bom_no,
                style_no: style_no,
                model_no: model_no,
            },
            success: function (result) {
                $("#popups2").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });

    function bom_no(cellvalue, options, rowObject) {
        var a, b;
        if (cellvalue == null) {
            return " "
        }
        else {
            a = cellvalue.substr(0, 1);
            b = cellvalue.substr(1, 11);
            d = cellvalue.substr(11);
            c = parseInt(b);
            return a + c + d;
        }
    };


    //$("#searchBtn_popup2").click(function () {

    //    var bom_no = $("#s_bom_no_popup2").val().trim(),
    //        style_no = $("#s_style_no_popup2").val().trim(),
    //        model_no = $("#s_model_no_popup2").val().trim();
    //    $.ajax({
    //        url: "/SalesMgt/searchBomNoPopup",
    //        type: "get",
    //        dataType: "json",
    //        data: {
    //            bom_no: bom_no,
    //            style_no: style_no,
    //            model_no: model_no,
    //        },
    //        success: function (result) {
    //            $("#popups").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
    //        }
    //    });

    //});

    //s_model_no_popup


});


