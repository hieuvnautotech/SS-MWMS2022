$(".dialogModelcode").dialog({
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
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                $("#savestyle_style").removeClass("disabled");
                var selectedRowId = $("#popupModel").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupModel").getRowData(selectedRowId);
                if (row_id != null) {
                  
                    $("#savestyle_style").click(function () {
                        $('#m_md_cd').val(row_id.md_cd);
                        $('#c_md_cd').val(row_id.md_cd);
                        $('.dialogModelcode').dialog('close');
                    });
                }
            },

            pager: '#popupspagerModel',
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: true,
            height: 250,
            width: $(".boxModel").width(),
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


    },
});

$(".poupdialogModelCode").click(function () {
    $('.dialogModelcode').dialog('open');
});



//////////////////////////////////////////////////////////////////////////////////////////////////

// Popup Dangerous
$("#dialogDangerous").dialog({
    width: '20%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});


$("#deletestyle").click(function () {
    $.ajax({
        url: "/DevManagement/deleteStyle",
        type: "post",
        dataType: "json",
        data: {
            sid: $('#m_sid').val(),
        },
        success: function (data) {
            if (data.result == true) {
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});

$('#closesdetele').click(function () {
    $('#dialogDangerous').dialog('close');
});

