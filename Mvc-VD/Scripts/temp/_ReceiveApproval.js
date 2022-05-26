$(function () {
    var row_id, row_id2;
    $grid = $("#recApprGrid").jqGrid
   ({
       url: "/ReceivingMgt/getrecAppr",
       datatype: 'json',
       mtype: 'Get',
       colModel: [
           { key: true, label: 'pdid', name: 'pdid', width: 50, align: 'right', hidden: true },
           { label: 'MPO', name: 'mpo_no', width: 110, align: 'center' },
           { label: 'MDPO', name: 'mdpo_no', width: 110, align: 'center' },
           { label: 'Material No', name: 'mt_no', width: 110, align: 'center' },
           { label: 'Material Name', name: 'mt_nm', width: 120, align: 'center' },
           { label: 'State', name: 'md_sts_cd', width: 120, align: 'center' },
           { label: 'Location State', name: 'lct_sts_cd', width: 120, align: 'center' },
           { label: 'Expect date', name: 'exp_input_dt', width: 120, align: 'center' },
           { label: 'Receive date', name: 'rec_input_dt', width: 150, align: 'center' },
           { label: 'Purchase qty', name: 'need_qty', width: 150, align: 'center' },
           { label: 'Pass qty', name: 'pass_qty', width: 60, width: 80, align: 'right' },
           { label: 'Bundle qty', name: 'bundle_qty', width: 80, align: 'right' },
           { label: 'Bundle Unit qty', name: 'bundle_unit_qty', width: 80, align: 'right' },
           { label: 'Feed size', name: 'feed_size', width: 80, align: 'right' },
           { label: 'Feed unit', name: 'feed_unit', width: 60, width: 100, align: 'right' },
           { label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
           { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
           { label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
           { label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
       ],
       onSelectRow: function (rowid, selRowIds, selected, status, e) {
           $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
           var selRowIds = $('#recApprGrid').jqGrid("getGridParam", "selarrrow");
           console.log("c_approval - selRowIds : " + selRowIds);
       },


       pager: "#recApprGridPager",
       viewrecords: true,
       rowList: [20, 50, 200, 500],
       height: 220,
       width: $(".box-body").width() - 5,
       autowidth: false,
       rowNum: 20,
       caption: 'PO Management',
       loadtext: "Loading...",
       emptyrecords: "No data.",
       rownumbers: true,
       gridview: true,
       loadonce: true,
       shrinkToFit: false,
       multiselect: true,
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

    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';
    $('#recApprGrid').jqGrid('setGridWidth', $(".box-body").width());
    $(window).on("resize", function () {
        var newWidth = $("#recApprGrid").closest(".ui-jqgrid").parent().width();
        $("#recApprGrid").jqGrid("setGridWidth", newWidth, false);
    });

    $("#c_approval").on("click", function () {


        var rowId = $('#recApprGrid').jqGrid("getGridParam", "selrow");

        var pdid = $("#recApprGrid").jqGrid('getCell', rowId, 'pdid');

        var i, selRowIds = $('#recApprGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;;

        for (i = 0, n = selRowIds.length; i < n; i++) {

            var ret = $("#recApprGrid").jqGrid('getRowData', selRowIds[i]);

    
            var pdid2 = ret.pdid;
            $.ajax({
                type: "post",
                dataType: 'text',
                url: "/ReceivingMgt/updaterecAppr",
                headers: {
                    "Content-Type": "application/json",
                    "X-HTTP-Method-Override": "POST"
                },
                cache: false,
                data: JSON.stringify({
                    pdid: pdid2
                }),
                success: function (data) {
                    jQuery("#recApprGrid").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                }

            });



        };


    })
})