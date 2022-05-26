$(function () {
    $(".dialogMPO").dialog({
        width: '100%',
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

            $("#popupsMPO").jqGrid
            ({
                url: "/DashBoardQC/getPopupMPO",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'mtid', name: 'pdid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'MMPO', name: 'MMPO', width: 100, align: 'center' },
                    { key: false, label: 'MPO', name: 'MPO', width: 100, align: 'center' },
                    { key: false, label: 'MT No', name: 'mt_no', width: 250, align: 'left' },
                    { key: false, label: 'Group Unit', name: 'gr_unit', width: 250, align: 'left' },
                    { key: false, label: 'Received Date', name: 'received_dt', width: 100, align: 'center' },
                    { key: false, label: 'Received Bundle Qty', name: 'rec_bundle_qty', width: 100, align: 'center' },
                    { key: false, label: 'Received Qty', name: 'received_qty', width: 100, align: 'center' },
                    { key: false, label: 'QC Code', name: 'qc_code', width: 100, align: 'center' },
                    { key: false, label: 'QC Rate', name: 'qc_rate', width: 100, align: 'center' },
                    { key: false, label: 'Defect Qty', name: 'defect_qty', width: 100, align: 'center' },

                   

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupsMPO").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsMPO").getRowData(selectedRowId);
                    $("#saveMPO").removeClass("disabled");

                },

                pager: jQuery('#popupspagerMPO'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 240,

                autowidth: true,
                caption: 'MT NO',
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
        close: function (event, ui) {
            $("#s_mpo_no_popup").val("");
            $("#s_mpo_name_popup").val("");
            jQuery("#popupsMPO").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        }
    });



    //-thanhnam--//
    $(".poupdialogl").click(function () {
        $('.dialogMPO').dialog('open');
    });
    //-----//

    $('#saveMPO').click(function () {
        if (row_id != null) {
            $('#s_mpo').val(row_id.MPO);
            $('.dialogMPO').dialog('close');

        }
    });


    $("#searchBtn_popup_s_MPO").click(function () {

        var mpo_no = $("#s_mpo_no_popup").val().trim(),
            mpo_name = $("#s_mpo_name_popup").val().trim();
        $.ajax({
            url: "/DashBoardQC/searchMPO",
            type: "get",
            dataType: "json",
            data: {
                mpo_no: mpo_no,
                mpo_name: mpo_name,
            },
           
            success: function (result) {
                $("#popupsMPO").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
        });

    });

});