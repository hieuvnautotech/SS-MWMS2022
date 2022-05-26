$(".dialog_menu").dialog({
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
        "ui-dialog2": "ui-dialog2",
        "ui-dialog2-titlebar": "ui-dialog2 ui-dialog2-titlebar-sm",
        "ui-dialog2-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog2-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {

        $("#popupmenu").jqGrid
        ({
            url: '/System/GetDataMenu',
            mtype: 'GET',
            datatype: 'json',
            colModel: [
            { label: 'ID', name: 'mnno', key: true, width: 60, align: 'right', hidden: true, key: true },
            { name: 'mn_cd', hidden: true },
            { name: 'up_mn_cd', hidden: true },
            { name: 'level_cd', hidden: true },
            { name: 'sub_yn', hidden: true },
            { name: 'mn_cd_full', hidden: true },
            { label: 'Menu Name', name: 'mn_nm', sortable: true, editable: true, editrules: { required: true, number: true }, width: 150, align: 'left' },
            { label: 'Full Name', name: 'mn_full', sortable: true, width: 350, editrules: { required: true, number: true }, align: 'left' },
            { label: 'URL', name: 'url_link', width: 220, align: 'left', sortable: true, editable: true, editrules: { required: true, number: true } },
            { label: 'USE YN', name: 'use_yn', width: 55, align: 'center', sortable: true, editrules: { required: true, number: true } },
            { label: 'Order No', name: 'order_no', width: 70, align: 'center', sortable: true, editable: true },
            { label: 'Reg Name', name: 'reg_id', sortable: true, },
            {
            label: 'Reg Date', name: 'reg_dt', sortable: true, formatter: "date",
            formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
            },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupmenu").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupmenu").getRowData(selectedRowId);
                if (row_id != null) {
                    $('#selectmenu').click(function () {
                        $('#mn_cd').val(row_id.mn_cd);
                        $('#mn_nm').val(row_id.mn_nm);
                        $('.dialog_menu').dialog('close');
                    });
                }
            },

            pager: jQuery('#pagermenu'),
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: true,
            height: 300,
            width: $(".boxMenu").width(),
            autowidth: false,
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
$(".poupdialogmenu").click(function () {
    $('.dialog_menu').dialog('open');
});

$("#saveBtn").click(function () {
    if ($("#notice").valid() == true) {
        $.ajax({
            url: "/system/insertnotice_menucheck",
            type: "get",
            dataType: "json",
            data: {
                mn_cd: $("#mn_cd").val(),
            },
            success: function (data) {
                if (data.result != 0) {
                    alert('Menu manual infor was existing. Please check again');
                } else {
                    $("#notice").submit();
                }
            },
            error: function (result) { }
        });
    }
});



$("#notice").validate({
    rules: {
        "title": {
            required: true,
        },
    },

});

///////////////////////////////////////////////////////////////////////////////////////////////////////

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

$("#deletestyle").click(function () {
    $.ajax({
        url: "/system/deleteManual",
        type: "post",
        dataType: "json",
        data: {
            bno: $('#s_bno').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#noticeMgt").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('Manual Information was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});

$('#closestyle').click(function () {
    $('#dialogDangerous').dialog('close');
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
