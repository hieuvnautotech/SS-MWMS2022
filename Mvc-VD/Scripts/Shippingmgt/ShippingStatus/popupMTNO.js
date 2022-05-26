$(function () {
    $(".dialogMT").dialog({
        width: '100%',
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

            $("#popupsMT").jqGrid
            ({
                url: "/Shippingmgt/getPopupMT",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'mtid', name: 'mtid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'MT Type', name: 'mt_type', width: 100, align: 'center' },
                    { key: false, label: 'MT No', name: 'mt_no', width: 150, align: 'left' },
                    { key: false, label: 'MT Name', name: 'mt_nm', width: 150, align: 'left' },
                    { key: false, label: 'MF Code', name: 'mf_cd', width: 150, align: 'left' },
                    { key: false, label: 'SP Code', name: 'sp_cd', width: 100, align: 'left' },
                    { key: false, label: 'width', name: 'width', width: 100, align: 'right', formatter: 'integer' },
                    { key: false, label: 'width Unit', name: 'width_unit', width: 100, align: 'left' },
                    { key: false, label: 'Spec', name: 'spec', width: 100, align: 'right', formatter: 'integer' },
                    { key: false, label: 'Spec Unit', name: 'spec_unit', width: 100, align: 'left' },
                    { key: false, label: 'Price ', name: 'price', width: 100, align: 'right', formatter: 'integer' },
                    { key: false, label: 'Price Unit', name: 'price_unit', width: 100, align: 'left' },
                    { key: false, label: 'Photo File', name: 'photo_file', width: 100, align: 'center' },
                    { key: false, label: 'Description', name: 're_mark', width: 100, align: 'left' },
                    { key: false, label: 'Use N', name: 'use_yn', width: 100, align: 'center' },
                    { key: false, label: 'Del N', name: 'del_yn', width: 100, align: 'center' },
                    { key: false, label: 'Create Name', name: 'reg_id', width: 100, align: 'left' },
                    { key: false, label: 'Change Name', name: 'chg_id', width: 100, align: 'left' },
                    { label: 'Create Date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },
                    { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'y-m-d h:m:s' }, width: '200' },

                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popupsMT").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupsMT").getRowData(selectedRowId);
                    if (row_id != null) {
                        
                    }
                    $("#savestyleMT").click(function () {
                        $('#mt_no').val(row_id.mt_no);
                        $('.dialogMT').dialog('close');
                    });
                },

                pager: jQuery('#popupspagerMT'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 220,
               
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
    });

    $(".poupdialogMT").click(function () {
        $('.dialogMT').dialog('open');
    });
    $('#closeMT').click(function () {
        $('.dialogMT').dialog('close');
    });

});