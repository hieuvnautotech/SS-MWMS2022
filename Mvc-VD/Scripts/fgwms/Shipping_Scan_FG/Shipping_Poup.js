$(function () {
    var lstData;
    var dataOld;
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
                    url: "/standard/GetBuyerInfo",
                    datatype: 'json',
                    mtype: 'Get',
                    colModel: [
                        { key: true, label: 'byno', name: 'byno', width: 80, align: 'center', hidden: true },
                        { key: false, label: 'Buyer Code', name: 'buyer_cd', sortable: true, width: '80', align: 'center' },
                        { key: false, label: 'Buyer Name', name: 'buyer_nm', sortable: true, width: '200' },
                        { key: false, label: 'Brand Name', name: 'brd_nm', editable: true, sortable: true, width: '150' },
                        { key: false, label: 'Description', name: 're_mark', editable: true, width: '100px' }
                    ],
                    onSelectRow: function (rowid, selected, status, e) {
                        var selectedRowId = $("#popup_qc").jqGrid("getGridParam", 'selrow');
                        row_id = $("#popup_qc").getRowData(selectedRowId);

                        if (row_id != null) {
                            $("#close_qc").click(function () {
                                $('.dialogQC').dialog('close');
                            });
                            $("#selected_qc").click(function () {

                                $('.dl_nm').val(row_id.buyer_nm);
                                $('#m_dl_nm').val(row_id.buyer_nm);
                                $('.dl_nm2').val(row_id.buyer_nm);
                                $('#m_dl_nm2').val(row_id.buyer_nm);
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
                    search: true,
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
                })


        },


    });

    $("#searchBtn1").click(function () {
        var filter;
        var code = $("#sp_cd").val().trim();
        var name = $("#sp_nm").val().trim().toUpperCase();
        
        var data = $("#popup_qc").getRowData();
        if (dataOld == null) {
            dataOld = $("#popup_qc").getRowData();
        }

        if (code != "" || name != "") {
            filter = data.filter(x => (name != "" && x.buyer_nm.toUpperCase().includes(name)) || (code != "" &&  x.buyer_cd == code));
        } 
        else {
            filter = dataOld;
        }
        
        if (filter != null) {
            $("#popup_qc").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: filter }).trigger("reloadGrid");
        }
        else {
            $("#popup_qc").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
            alert('Not found');
        }
   

});

$('#close_qc').click(function () {
    $('.dialogQC').dialog('close');
});
$('.poupdialogFG').click(function () {
    $('.dialogQC').dialog('open');
});
    
});