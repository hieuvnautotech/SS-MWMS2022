
$(function () {
    $(".dialog_pp_mold").dialog({
        width: '50%',
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

            $("#popupmold1").jqGrid
                 ({
                     datatype: 'json',
                     mtype: 'GET',
                     colModel: [
                        { label: 'mdno', name: 'mdno', key: true, width: 40, align: 'center', hidden: true },
                        { label: ' Code', name: 'md_no', width: 150, align: 'left' },
                        { label: ' Name', name: 'md_nm', width: 300, align: 'left' },
                        { label: 'Purpose ', name: 'purpose', width: 150 },
                        { label: 'Barcode', name: 'barcode', width: 150, align: 'left', hidden: true },
                        { key: false, label: 'Use', name: 'su_dung', width: 150, align: 'left' },
                        { label: 'Description', name: 're_mark', width: 150 },
                        { label: 'Create User', name: 'reg_id', width: 100, align: 'center' },
                        { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                        { label: 'Change User', name: 'chg_id', width: 100, align: 'center' },
                        { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                     ],
                     onSelectRow: function (rowid, selected, status, e) {
                         $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                         $("#savemold").removeClass("disabled");
                         var selectedRowId = $("#popupmold1").jqGrid("getGridParam", 'selrow');
                         row_id = $("#popupmold1").getRowData(selectedRowId);
                         if (row_id != null) {
                             $('#closemold').click(function () {
                                 $('.dialog_pp_mold').dialog('close');
                             });
                             $("#savemold").click(function () {
                                 $('#c_ml_mc_no').val(row_id.md_no);
                                 $('#m_ml_mc_no').val(row_id.md_no);
                                 $('.dialog_pp_mold').dialog('close');
                             });
                         }
                     },

                     pager: jQuery('#pagermold'),
                     rownumbers: true,
                     rowNum: 50,
                     rowList: [50, 100, 200, 500, 1000],
                     sortable: true,
                     loadonce: true,
                     height: 220,
                     width: $(".dialog_pp_mold").width() - 30,
                     autowidth: false,
                     shrinkToFit: false,
                     loadonce: false,
                     viewrecords: true,
                     multiselect: false,
                     datatype: function (postData) { getDataOutBox(postData); },
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

            function getDataOutBox(pdata) {
                var md_no = $("#md_no_pp").val().trim();
                var md_nm = $("#md_nm_pp").val().trim();

                var params = new Object();
                params.page = pdata.page;
                params.rows = pdata.rows;
                params.sidx = pdata.sidx;
                params.sord = pdata.sord;
                params.md_no = md_no;
                params.md_nm = md_nm;

                $("#popupmold1").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
                params._search = pdata._search;

                $.ajax({
                    url: "/ActualWO/MoldMgtData",
                    type: "Get",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    traditional: true,
                    data: params,
                    success: function (data, st) {
                        if (st == "success") {
                            var grid = $("#popupmold1")[0];
                            grid.addJSONData(data);
                        }
                    }
                })
            }
            $("#searchBtn_mold").click(function () {
                $("#popupmold1").clearGridData();
                var grid = $("#popupmold1");
                grid.jqGrid('setGridParam', { search: true });
                var pdata = grid.jqGrid('getGridParam', 'postData');
                getDataOutBox(pdata);
            });
        },
    });
    $(".poupdialogmold").click(function () {
        $('.dialog_pp_mold').dialog('open');
    });

    $('#closemold').click(function () {
        $('.dialog_pp_mold').dialog('close');
    });

});

