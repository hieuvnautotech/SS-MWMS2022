$(document).ready(function () {
$("#tab_pp1").on("click", "a", function (event) {
document.getElementById("form1").reset();
$("#tab_pp2").removeClass("active");
$("#tab_pp1").addClass("active");
$("#tab_c_pp2").removeClass("active");
$("#tab_c_pp1").removeClass("hidden");
$("#tab_c_pp2").addClass("hidden");
$("#tab_c_pp1").addClass("active");
});
$("#tab_pp2").on("click", "a", function (event) {

    document.getElementById("form2").reset();
    $("#tab_pp1").removeClass("active");
    $("#tab_pp2").addClass("active");
    $("#tab_c_pp1").removeClass("active");
    $("#tab_c_pp2").removeClass("hidden");
    $("#tab_c_pp1").addClass("hidden");
    $("#tab_c_pp2").addClass("active");
});
});

$(function () {
    $(".dialog").dialog({
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

            $("#popupmachine").jqGrid
            ({
                url: "/DevManagement/GetPopupmachine",
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                   { key: true, label: 'mmo', name: "mno", width: 80, align: 'center', hidden: true },
                   { key: false, label: 'Type', name: 'mc_type', width: 110, align: 'center' },
                   { key: false, label: 'Code', name: 'mc_no', width: 150, align: 'center', sort: true },
                   { key: false, label: 'Name', name: 'mc_nm', width: 150, align: 'left' },
                   { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                   { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'center' },
                   { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
                   { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                   { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                   { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                   { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } }
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    var selectedRowId = $("#popupmachine").jqGrid("getGridParam", 'selrow');
                    row_id = $("#popupmachine").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#c_mc_no').val(row_id.mc_no);
                        $('#m_mc_no').val(row_id.mc_no);
                        $('.dialog').dialog('close');
                    }
                },

                pager: jQuery('#pagermachine'),
                viewrecords: true,
                rowList: [20, 50, 200, 500],
                height: 220,
                width: $(".dialog").width() - 30,
                autowidth: false,
                caption: 'Machine',
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
            $("#popupbobin").jqGrid
                 ({
                     url: "/DevManagement/Getpopupbobin",
                     datatype: 'json',
                     mtype: 'Get',
                     colModel: [
                       { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
                       { key: false, label: 'Code', name: 'bb_no', width: 150, align: 'center', sort: true },
                       { key: false, label: 'Name', name: 'bb_nm', width: 150, align: 'left' },
                       { key: false, label: 'Purpose', name: 'purpose', width: 200, align: 'left' },
                       { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'center' },
                       { key: false, label: 'Remark', name: 're_mark', width: 300, align: 'left' },
                       { key: false, label: 'Create User', name: 'reg_id', width: 90, align: 'center' },
                       { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                       { key: false, label: 'Chage User', name: 'chg_id', width: 90, align: 'center' },
                       { key: false, label: 'Chage Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                     ],
                     onSelectRow: function (rowid, selected, status, e) {
                         $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                         var selectedRowId = $("#popupbobin").jqGrid("getGridParam", 'selrow');
                         row_id = $("#popupbobin").getRowData(selectedRowId);
                         if (row_id != null) {
                             $('#c_mc_no').val(row_id.bb_no);
                             $('#m_mc_no').val(row_id.bb_no);
                             $('.dialog').dialog('close');
                         }
                     },

                     pager: jQuery('#pagerbobin'),
                     viewrecords: true,
                     rowList: [20, 50, 200, 500],
                     height: 220,
                     width: $(".dialog").width() - 30,
                     autowidth: false,
                     caption: 'BoBin',
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

    $(".poupdialogmachine").click(function () {
        $('.dialog').dialog('open');
    });
    
    //$.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = 'Bootstrap';
    //$('#popupmachine').jqGrid('setGridWidth', $(".ui-dialog").width());
    //$(window).on("resize", function () {
    //    var newWidth = $("#popupmachine").closest(".ui-jqgrid").parent().width();
    //    $("#popupmachine").jqGrid("setGridWidth", newWidth, false);
    //});

    $('#closemachine').click(function () {
        $('.dialog').dialog('close');
    });

});
