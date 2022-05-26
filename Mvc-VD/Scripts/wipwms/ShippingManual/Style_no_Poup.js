$(function () {
    $(".dialog3").dialog({
        width: '100%',
        height: 800,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 1000,
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

            $("#popupStyle_no").jqGrid
            ({
                url: "/wipwms/GetpopupStyle_no",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { label: 'ID', name: 'sid', key: true, width: 50, align: 'center', hidden: true },
                   { label: 'Product Code', name: 'style_no', sortable: true, width: 150, align: 'center' },
                   { label: 'Product Name', name: 'style_nm', key: true, width: 300, align: 'left' },
                   { label: 'Model', name: 'md_cd', sortable: true, width: 400, align: 'left' },
                   //{ label: 'Width', name: 'width', sortable: true, width: 100, align: 'right' },
                   //{ label: 'Spec', name: 'spec', sortable: true, width: 100, align: 'right' },
                   //{ label: 'Photo', name: 'photo_file', width: 100, formatter: downloadLink, align: 'center' },
                   //{ label: 'Photo', name: 'photo_file', width: 100, hidden: true },
                   //{ label: 'Price', name: 'price', width: 150, align: 'right' },
                   //{ label: 'Re Mark', name: 're_mark', width: 130, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    var selectedRowId = $("#popupStyle_no").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupStyle_no").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#selected_style_no").click(function () {
                        //$('#c_mt_no').val(row_id.mt_no);
                            $('#style_no').val(row_id.style_no);
                            $('.dialog3').dialog('close');
                        });

                    }
                },

                pager: jQuery('#pagerStyle_no'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 600,
                rowNum:50,
                autowidth: false,
                caption: 'Product Information',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                width: '100%',
                gridview: true,
                shrinkToFit: false,
                multiselect: false,
                loadonce: true,
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
    //function downloadLink(cellValue, options, rowdata, action) {
    //    if (cellValue != null) {
    //        var html = '<img src="../images/' + cellValue + '" style="height:20px;" />';
    //        return html;

    //    } else {
    //        return "";
    //    }
    //}



    $("#searchBtn_pp_style_no").click(function () {
        var style_no = $("#s_style_no_pp").val().trim();
        var md_cd = $("#s_md_cd_pp").val().trim();
       // var name = $("#mt_nm").val();
        $.ajax({
            url: "/wipwms/GetpopupStyle_no",
            type: "get",
            dataType: "json",
            data: {
                style_no: style_no,
                md_cd: md_cd,
               // nameData: name,
            },
            success: function (result) {
                $("#popupStyle_no").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
            }
        });
    });

    $('#closestyle_style_no').click(function () {
        $('.dialog3').dialog('close');
    });
    $(".poupdialogStyle_no").click(function () {
        $('.dialog3').dialog('open');
    });

});
//$(document).ready(function () {
//    _GetType();
//});
//function _GetType() {
//    $.get("/Shippingmgt/GetType_M", function (data) {
//        if (data != null && data != undefined && data.length) {
//            var html = '';
//            html += '<option value="" selected="selected">*Type*</option>';
//            $.each(data, function (key, item) {
//                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
//            });
//            $("#mt_type").html(html);
//        }
//    });
//}