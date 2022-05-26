
$(function () {
    $(".diologMTbobin").dialog({
        width: '50%',
        height: 600,
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
            $("#popupMTbobin").jqGrid
     ({
         datatype: 'json',
         mtype: 'Get',
         colModel: [
             { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
              { key: false, label: 'Bobbin Code', name: 'bb_no', width: 150, align: 'center', sort: true },
              { key: false, label: 'Bobbin Name', name: 'bb_nm', width: 150, align: 'left' },
              { key: false, label: 'Material', name: 'mt_cd', width: 400, align: 'left' },
              { key: false, label: 'Barcode', name: 'barcode', width: 150, align: 'center' },

         ],
         onSelectRow: function (rowid, selected, status, e) {
             $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
             $("#save_mt_bb").removeClass("disabled");
             var selectedRowId = $("#popupMTbobin").jqGrid("getGridParam", 'selrow');
             row_id = $("#popupMTbobin").getRowData(selectedRowId);
             if (row_id != null) {
                 $('#close_mt_bb').click(function () {
                     $('.diologMTbobin').dialog('close');
                 });
                 $("#save_mt_bb").click(function () {
                    
                     //$('#cp_bb_no_pr').val(row_id.bb_no);
                     $('#id_bb_pr').val(row_id.bno);
                     $('#id_bb_cm').val(row_id.bno);
                     $('#cp_bb_no2').val(row_id.bb_no);
                     $('#cp_bb_no3').val(row_id.bb_no);
                     $('#cp_bb_cm').val(row_id.bb_no);
                     $('#s_mt_cd').val(row_id.mt_cd);
                     $('#pp_pr_cd').val(row_id.mt_cd);
                     
                     $('.diologMTbobin').dialog('close');
                 });
             }
         },
         pager: "#PageMTBobbin",
         pager: jQuery('#PageMTBobbin'),
         viewrecords: true,
         rowList: [50, 100, 200, 500, 1000],
         height: 300,
         width: $(".boxB").width(),
         autowidth: false,
         rowNum: 50,
         caption: 'Bobbin List',
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


    $('#close_mt_bb').click(function () {
        $('.diologMTbobin').dialog('close');
    });

});

$(".poupdialogMTbb").click(function () {
    GetType_mt_bom();

    bom = $('#cp_bom_no').val();
    part = $('#cp_process_no').val();
    level=$("#level").val();
    $("#popupMTbobin").setGridParam({ url: "/Lot/getppMTBobbin?bom=" + bom + "&part=" + part + "&level=" + level , datatype: "json" }).trigger("reloadGrid");
    $('.diologMTbobin').dialog('open');
});
$("#searchBtn_popupMtbobbin").click(function () {
    $.ajax({
        url: "/Lot/searchMTbobbinPopup",
        type: "get",
        dataType: "json",
        data: {
            bb_no: $('#s_mt_bb_no').val().trim(),
            bb_nm: $('#s_mt_bb_nm').val().trim(),
            ml_nm: $('#ml_nm_popup1').val().trim(),
            bom : $('#mt_bom_popup').val(),
            part: $('#cp_process_no').val(),
            level:$("#level").val(),
        },
        success: function (result) {
            $("#popupMTbobin").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});



$("#save_bb_map").click(function () {
    if ($("#cp_lot").val() == "") {
        alert("Please enter your Composite Lot");
        $("#cp_lot").val("");
        $("#cp_lot").focus();
        return false;
    }
    if ($("#cp_bb_no").val() == "") {
        alert("Please enter your Bobbin");
        $("#cp_bb_no").val("");
        $("#cp_bb_no").focus();
        return false;
    } else {
        $.ajax({
            url: "/ActualWO/Mapping_bb_lot",
            type: "get",
            dataType: "json",
            data: {
                mt_cd: $("#cp_lot").val(),
                bb_no: $("#cp_bb_no").val(),
                wmtid: $("#id_mt_cm").val(),
                bno: $("#id_bb_cm").val(),
            },
            success: function (result) {
                var id = result.ds2.wmtid;
                $("#tb_mt_lot").setRowData(id, result.ds2, { background: "#d0e9c6" });
            }
        });
    }
});


