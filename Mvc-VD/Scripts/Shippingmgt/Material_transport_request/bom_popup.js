$(function () {
    $(".dialog").dialog({
        width: '50%',
        height: 600,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 700,
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

            $("#popupbom").jqGrid
            ({
                url: '/Shippingmgt/GetBomMgt',
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                      { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'Bom no', name: 'bom_no', width: 100, align: 'center', },
                    { key: false, label: 'Product Code', name: 'style_no', width: 150, align: 'left' },
                    { key: false, label: 'Product Name', name: 'style_nm', width: 150, align: 'left' },
                    { key: false, label: 'Model', name: 'md_cd', sortable: true, width: '250'},
                    { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200' },
                    { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px' },
                    { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px' },
                    { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'center' },
                    { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px' },
                    { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180' },
                     { key: false, label: 'Remark', name: 'cav', editable: true, width: '100px' },
                   {
                       label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                       formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                   },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupbom").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupbom").getRowData(selectedRowId);
                    $("#selected1").removeClass("disabled");
                    if (row_id != null) {
                        
                        $("#selected1").click(function () {
                          
                            $('#c_fo_no').val("");
                            $('#c_mt_no').val("");
                            $('#c_pm_no').val("");
                            $('#bom_fo').val("");
                            $('#c_bom_no').val(row_id.bom_no);
                            $('#bom_no').val(row_id.bom_no);

                            $('#style_no').val(row_id.style_no);
                            $('#style_nm').val(row_id.style_nm);
                            $('#md_cd').val(row_id.md_cd);

                            var grid = $("#ShipRequest");
                            grid.jqGrid('setGridParam', { search: true });
                            var pdata = grid.jqGrid('getGridParam', 'postData');
                            getData_search(pdata);

                           
                        $('#m_bom_no').val(row_id.bom_no); 
                        $('#rel_bom').val(row_id.bom_no);
                        $('.dialog').dialog('close');

                        $("#check_kq").setGridParam({ url: "/Shippingmgt/getdataTranport?" + "bom_no=" + row_id.bom_no, datatype: "json" }).trigger("reloadGrid");
                        });
                    }
                },

                pager: jQuery('#pagerbom'),
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                height: 350,
               
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

        },
    });

    $(".poupdialogbom").click(function () {
        $('.dialog').dialog('open');
    });
    
    $("#searchBtn_ppb").click(function () {
        var bom_no = $('#s_bom_no').val().trim();
        var md_cd = $('#s_md_cd').val().trim();
        //var style_nm = $('#s_style_nm').val().trim();
        var style_no = $('#s_style_no').val().trim();
        $.ajax({
            url: "/Shippingmgt/searchBom",
            type: "get",
            dataType: "json",
            data: {
                bom_no: bom_no,
                md_cd: md_cd,
                //style_nm: style_nm,
                style_no: style_no,
            },
            success: function (result) {
                $("#popupbom").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });

    });



    $('#closebom').click(function () {
        $('.dialog').dialog('close');
    });

});
