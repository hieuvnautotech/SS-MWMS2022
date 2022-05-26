//$(function () {
//    $(".dialog").dialog({
//        width: '100%',
//        height: 450,
//        maxWidth: 1000,
//        maxHeight: 450,
//        minWidth: '50%',
//        minHeight: 450,
//        resizable: false,
//        fluid: true,
//        modal: true,
//        autoOpen: false,
//        classes: {
//            "ui-dialog": "ui-dialog",
//            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
//            "ui-dialog-titlebar-close": "visibility: hidden",
//        },
//        resize: function (event, ui) {
//            $('.ui-dialog-content').addClass('m-0 p-0');
//        },
//        open: function (event, ui) {

//            $("#popupMaterial").jqGrid
//            ({
//                url: "/Shippingmgt/Getpopupmaterial",
//                datatype: 'json',
//                mtype: 'Get',
//                colModel: [
//                   { label: 'ID', name: 'wmtid', key: true, width: 50, align: 'center', hidden: true },
//                   { label: 'pdid', name: 'pdid', sortable: true, width: 60, align: 'center', hidden: true },
//                   { label: 'Material Code', name: 'mt_cd', key: true, width: 180, align: 'center' },
//                   { label: 'Material No', name: 'mt_no', sortable: true, align: 'center' },
//                   { label: 'MPO No', name: 'mpo_no', sortable: true, width: 100, align: 'center' },
//                   { label: 'MDPO No', name: 'mdpo_no', sortable: true, width: 100, align: 'center' },
//                   { label: 'Barcode', name: 'mt_barcode', width: 100, align: 'center' },
//                   { label: 'State', name: 'mt_sts_cd', width: 100, hidden: true },
//                   { label: 'Location', name: 'lct_cd', width: 100, align: 'right' },
//                   { label: 'lct_sts_cd', name: 'lct_sts_cd', width: 100, align: 'right' },
//                   { label: 'from_lct_cd', name: 'from_lct_cd', width: 100, align: 'right' },
//                   { label: 'to_lct_cd', name: 'to_lct_cd', width: 100, align: 'right' },
//                   { label: 'output_dt', name: 'output_dt', width: 100, align: 'right' },
//                   { label: 'input_dt', name: 'input_dt', width: 100, align: 'right' },
//                   { label: 'worker_id', name: 'worker_id', width: 100, align: 'right' },
//                ],
//                onSelectRow: function (rowid, selected, status, e) {
//                    var selectedRowId = $("#popupMaterial").jqGrid("getGridParam", 'selrow');
//                    row_id = $("#popupMaterial").getRowData(selectedRowId);
//                    if (row_id != null) {
//                        $('#c_mt_no').val(row_id.mt_no);
//                        $('.dialog').dialog('close');
//                    }
//                },

//                pager: jQuery('#pagerMaterial'),
//                viewrecords: true,
//                rowList: [20, 50, 200, 500],
//                height: 220,
//                width: $(".box-body").width() - 5,
//                autowidth: false,
//                caption: 'Material Information',
//                loadtext: "Loading...",
//                emptyrecords: "No data.",
//                rownumbers: true,
//                gridview: true,
//                shrinkToFit: false,
//                multiselect: false,
//                jsonReader:
//                {
//                    root: "rows",
//                    page: "page",
//                    total: "total",
//                    records: "records",
//                    repeatitems: false,
//                    Id: "0"
//                },


//            });

//        },
//    });


//    $(".poupdialogMaterial").click(function () {
//        $('.dialog').dialog('open');
//    });

//    $.jgrid.defaults.responsive = true;
//    $.jgrid.defaults.styleUI = 'Bootstrap';
//    $('#popupMaterial').jqGrid('setGridWidth', $(".ui-dialog").width());
//    $(window).on("resize", function () {
//        var newWidth = $("#popupMaterial").closest(".ui-jqgrid").parent().width();
//        $("#popupMaterial").jqGrid("setGridWidth", newWidth, false);
//    });

//    $('#closestyle').click(function () {
//        $('.dialog').dialog('close');
//    });

//});
//public ActionResult Getpopupmaterial()
//{
//    var sql = new StringBuilder();
//    sql.Append("select *")
//        .Append(" from w_material_info as a ")
//        .Append("GROUP BY a.mt_no order by a.mt_no ");
//    var list = db.w_material_info.SqlQuery(sql.ToString()).ToList<w_material_info>();
//    return Json(new { rows = list }, JsonRequestBehavior.AllowGet);

//}
$(function () {
    $(".dialog").dialog({
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

            $("#popupMaterial").jqGrid
            ({
                url: "/DevManagement/Getpopupmaterial",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { label: 'ID', name: 'mtid', key: true, width: 50, align: 'center', hidden: true },
                   { label: 'Type', name: 'mt_type', sortable: true, width: 60, align: 'center' },
                   { label: 'Code', name: 'mt_no', key: true, width: 80, align: 'center' },
                   { label: 'Name', name: 'mt_nm', sortable: true, },
                   { label: 'Width', name: 'width', sortable: true, width: 50, align: 'right' },
                   { label: 'Spec', name: 'spec', sortable: true, width: 100, align: 'right' },
                   { label: 'Photo', name: 'photo_file', width: 100, formatter: downloadLink, align: 'center' },
                   { label: 'Photo', name: 'photo_file', width: 100, hidden: true },
                   { label: 'Price', name: 'price', width: 60, align: 'right' },
                   { label: 'Re Mark', name: 're_mark', width: 130, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupMaterial").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupMaterial").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_mt_no').val(row_id.mt_no);
                        $('#m_mt_no').val(row_id.mt_no);
                        $('.dialog').dialog('close');
                    }
                },

                pager: jQuery('#pagerMaterial'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".box-body").width() - 5,
                autowidth: false,
                caption: 'Material Information',
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
    function downloadLink(cellValue, options, rowdata, action) {
        if (cellValue != null) {
            var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
            return html;

        } else {
            return "";
        }
    }

    $(".poupdialogMaterial").click(function () {
                $('.dialog').dialog('open');
            });

            $.jgrid.defaults.responsive = true;
            $.jgrid.defaults.styleUI = 'Bootstrap';
            $('#popupMaterial').jqGrid('setGridWidth', $(".ui-dialog").width());
            $(window).on("resize", function () {
                var newWidth = $("#popupMaterial").closest(".ui-jqgrid").parent().width();
                $("#popupMaterial").jqGrid("setGridWidth", newWidth, false);
            });

            $('#closestyle').click(function () {
                $('.dialog').dialog('close');
            });
});