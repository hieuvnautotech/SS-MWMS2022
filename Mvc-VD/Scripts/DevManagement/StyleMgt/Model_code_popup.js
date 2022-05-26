$(function () {
    $(".dialogModelcode").dialog({
        width: "100%",
        height: 800,
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

            $("#popupModel").jqGrid
            ({
                url: "/DevManagement/GetModelcode",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { key: true, label: 'mdid', name: 'mdid', index: 'mdid', hidden: true },
                      { key: false, label: 'Model Code', name: 'md_cd', width: 420, align: 'left' },
                      { key: false, label: 'Model Name', name: 'md_nm', width: 420, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupModel").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupModel").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#m_md_cd').val(row_id.md_cd);
                        $('#c_md_cd').val(row_id.md_cd);
                        $('.dialogModelcode').dialog('close');
                    }
                },

                pager: jQuery('#popupspagerModel'),
                viewrecords: true,
                rowList: [100, 200, 300, 500, 1000, 2000, 5000, 10000],
                rowNum:100,
                height: 500,
                //width: $(".ui-dialog").width() - 50,
                autowidth: false,
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
    $(".poupdialogModelCode").click(function () {
        $('.dialogModelcode').dialog('open');
    });
    $('#closeModelCode').click(function () {
        $('.dialogModelcode').dialog('close');
    });

    $("#searchBtn1").click(function () {
        var s_md_cd = $('#s_md_cd').val().trim();
        var s_md_nm = $('#s_md_nm').val().trim();
        $.ajax({
            url: "/DevManagement/searchModeCode",
            type: "GET",
            dataType: "json",
            data: {
                md_cd: s_md_cd,
                md_nm: s_md_nm,
            },
            success: function (result) {
                $("#popupModel").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger('reloadGrid');
            }
        });

    });
});