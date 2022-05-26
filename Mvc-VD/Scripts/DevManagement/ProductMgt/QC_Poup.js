$(function () {
    $(".dialogQC").dialog({
        width: '100%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 600,
        minWidth: '50%',
        minHeight: 600,
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
            $("#popup_qc").jqGrid
            ({
                url: "/DevManagement/getQC_gs",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                     { key: true, label: 'ino', name: 'ino', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'QC Code', name: 'item_vcd', width: 150, align: 'center' },
                    { key: false, label: 'QC Type', name: 'item_type', width: 100, align: 'center' },
                    { key: false, label: 'Ver', name: 'ver', width: 70, align: 'center' },
                    { key: false, label: 'QC Name', name: 'item_nm', width: 150, align: 'left' },
                    { key: false, label: 'QC Explain', name: 'item_exp', width: 150, align: 'left' },
                    { key: false, label: 'Use Y/N', name: 'use_yn', width: 70, align: 'center' },
                    { key: false, label: 'Del Y/N', name: 'del_yn', width: 70, align: 'center', hidden: true },
                    { key: false, label: 'Create User', name: 'reg_id', width: 150, align: 'center' },
                    { key: false, label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { newformat: 'Y-m-d h:m:s' } },
                    { key: false, label: 'Change User', name: 'chg_id', width: 150, align: 'center' },
                    { key: false, label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { newformat: 'Y-m-d h:m:s' } },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popup_qc").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popup_qc").getRowData(selectedRowId);
                   
                    if (row_id != null) {
                        $("#close_qc").click(function () {
                            $('.dialogQC').dialog('close');
                        });
                        $("#selected_qc").click(function () {
                            $('#c_item_vcd').val(row_id.item_vcd);
                            $('#m_item_vcd').val(row_id.item_vcd);
                            $('.dialogQC').dialog('close');
                        });
                    }
                },

                pager: jQuery('#page_qc'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                rowNum: 50,
                loadonce: true,
                height: 400,
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

    //$("#deletestyle").click(function () {
    //    $.ajax({
    //        url: "/DevManagement/deleteStyle",
    //        type: "post",
    //        dataType: "json",
    //        data: {
    //            sid: $('#m_sid').val(),
    //        },
    //        success: function (data) {
    //            if (data.result != 0) {
    //                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
    //                document.getElementById("form1").reset();
    //                $("#tab_2").removeClass("active");
    //                $("#tab_1").addClass("active");
    //                $("#tab_c2").removeClass("active");
    //                $("#tab_c1").removeClass("hidden");
    //                $("#tab_c2").addClass("hidden");
    //                $("#tab_c1").addClass("active");

    //            }
    //            else {
    //                alert('Buyer was not existing. Please check again');
    //            }

    //        },
    //        error: function (result) { }
    //    });
    //    $('#dialogStyle').dialog('close');
    //});



    $('#close_qc').click(function () {
        $('.dialogQC').dialog('close');
    });
    $('.poupdialogQC').click(function () {
        $('.dialogQC').dialog('open');
    });
    
});