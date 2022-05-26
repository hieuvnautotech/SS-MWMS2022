$(".poupdialogSelectLocation").dialog({
    width: '50%',
    height: 500,
    maxWidth: 1000,
    maxHeight: 500,
    minWidth: '50%',
    minHeight: 450,
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

        $("#ListRack").jqGrid
            ({
                url: '/ExportToMachine/GetFactory_Location',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'ID', name: 'lctno', width: 60, hidden: true },
                    { label: 'Factory Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 250, align: 'center' },
                    { label: 'Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 250, },
                    { label: 'Level', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
                    { label: 'WIP', name: 'wp_yn', sortable: true, width: 60, align: 'center' },
                    { label: 'Factory', name: 'ft_yn', sortable: true, width: 60, align: 'center' },
                    { label: 'Movement', name: 'mv_yn', sortable: true, width: 60, align: 'center' },
                    { label: 'Nothing', name: 'nt_yn', sortable: true, width: 60, align: 'center' },
                    { label: 'use Y/N', name: 'use_yn', sortable: true, width: 60, align: 'center' },
                    { label: 'Remark', name: 're_mark', width: 100, sortable: true, },
                    { label: 'Index', name: 'index_cd', width: 100, sortable: true, },
                    { label: 'QR Code', name: 'lct_bar_cd', width: 100, sortable: true, },
                    { label: 'Create Name', name: 'reg_id', sortable: true, width: 70, },
                    { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                    { label: 'Change Name', name: 'chg_id', sortable: true, width: 80, },
                    { key: false, label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#SelectLocation").removeClass("disabled");
                    var selectedRowId = $("#ListRack").jqGrid("getGridParam", 'selrow');
                    row_id = $("#ListRack").getRowData(selectedRowId);
                    if (row_id != null) {

                        $("#SelectLocation").click(function () {
                            $('#SelectRack').val(row_id.lct_cd);
                            $('#SelectRackName').val(row_id.lct_nm);
                            document.getElementById("scan_mt_cd").focus();
                            $('.poupdialogSelectLocation').dialog('close');
                        });
                    }
                },

                pager: '#pagerListRack',
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: true,
                height: 250,
                width: null,
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

        $("#searchBtn_Location_popup").click(function () {
            $.ajax({
                url: "/StandardMgtWO/SearchFactory_Location",
                type: "Get",
                dataType: "json",
                data: {
                    lct_cd: $('#s_locationCode').val().trim(),
                    lct_nm: $('#s_locationName').val().trim(),
                },
                success: function (result) {
                    $("#ListRack").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

                }
            });

        });



    },
});

$(".SelectLocation").click(function () {
    $('.poupdialogSelectLocation').dialog('open');
});

$('#closestyle_product').click(function () {
    $('.poupdialogSelectLocation').dialog('close');
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


$("#deletestyle_popup").click(function () {
    $.ajax({
        url: "/SalesMgt/deletePO",
        type: "post",
        dataType: "json",
        data: {
            m_po_no: $('#m_po_no').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#poMgtGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');
            }
            else {
                alert('PO NO was not existing. Please check again');
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});

$('#closestyle_popup').click(function () {
    $('#dialogDangerous').dialog('close');
});