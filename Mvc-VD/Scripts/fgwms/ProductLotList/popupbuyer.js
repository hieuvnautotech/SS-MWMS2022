$(function () {
    $(".diologbuyer").dialog({
        width: '50%',
        height: 520,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        zIndex: 1000,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only ui-dialog-titlebar-close": "display: none !important",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {
            $("#popupbuyer").jqGrid
     ({
         url: "/fgwms/GetBuyerInfo_popup",
         datatype: 'json',
         mtype: 'Get',
         colModel: [
                { key: true, label: 'byno', name: 'byno', width: 80, align: 'center', hidden: true },
            { key: false, label: 'Buyer Code', name: 'buyer_cd', sortable: true, width: '80', align: 'center' },
            { key: false, label: 'Buyer Name', name: 'buyer_nm', sortable: true, width: '200' },
            { key: false, label: 'Brand Name', name: 'brd_nm', editable: true, sortable: true, width: '150' },
            { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px' },
            { key: false, label: 'Logo', name: 'logo', editable: true, width: '100px', hidden: true },
            { key: false, label: 'Website', name: 'web_site', editable: true, width: '180' },
            { key: false, label: 'Phone Number', name: 'phone_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Cell Number', name: 'cell_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'E-Mail', name: 'e_mail', editable: true, width: '200px' },
            { key: false, label: 'Fax', name: 'fax_nb', editable: true, width: '100px', align: 'center' },
            { key: false, label: 'Address', name: 'address', editable: true, width: '250' },
            { key: false, label: 'Use YN', name: 'use_yn', editable: true, width: '50', align: 'center' },
            { key: false, label: 'Create Name', name: 'reg_id', index: 'reg_id', width: '100px' },
            { key: false, label: 'Create date', name: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '150' },
            { key: false, label: 'Change Name', name: 'chg_id', editable: true, width: '100px' },
            { key: false, label: 'Change date', name: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }, width: '150' }
         ],
         onSelectRow: function (rowid, selected, status, e) {
             $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
             $("#save_buyer").removeClass("disabled");
             var selectedRowId = $("#popupbuyer").jqGrid("getGridParam", 'selrow');
             row_id = $("#popupbuyer").getRowData(selectedRowId);
             if (row_id != null) {
                 $('#close_buyer').click(function () {
                     $('.popupbuyer').dialog('close');
                 });
                 $("#save_buyer").click(function () {
                     $('#cp_buyer_qr').val(row_id.buyer_cd);
                     $('.diologbuyer').dialog('close');
                 });
             }
         },
         pager: "#Pagebuyer",
         pager: jQuery('#Pagebuyer'),
         viewrecords: true,
         rowList: [50, 100, 200, 500, 1000],
         height: 300,
         width: $(".boxB").width(),
         autowidth: false,
         rowNum: 50,
         caption: 'Buyer Information',
         loadtext: "Loading...",
         emptyrecords: "No data.",
         rownumbers: true,
         gridview: true,
         loadonce: true,
         shrinkToFit: false,
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


    $('#close_buyer').click(function () {
        $('.diologbuyer').dialog('close');
        jQuery("#popupbuyer").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    });

});

$(".poupdialogbuyer").click(function () {
    jQuery("#popupbuyer").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    $('.diologbuyer').dialog('open');
});
$("#searchBtn_popupbuyer").click(function () {
    $.ajax({
        url: "/fgwms/searchBuyer_popup",
        type: "get",
        dataType: "json",
        data: {
            buyer_cd: $('#cp_buyer_cd').val().trim(),
            buyer_nm: $('#cp_buyer_nm').val().trim(),
        },
        success: function (result) {
            $("#popupbuyer").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});

