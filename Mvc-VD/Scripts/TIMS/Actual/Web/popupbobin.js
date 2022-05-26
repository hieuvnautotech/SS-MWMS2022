$('#dialogCompositeRollNormal_ContainerBtn').on('click', function () {
    $('.popupDialogComposite_RollNormal_Container').dialog('open');
    $("#cp_bb_no").val("");
    $("#dialogCompositeRollNormalContainer_ContainerCode").val("");
    $("#dialogCompositeRollNormalContainer_ContainerName").val("");
    $("#dialogCompositeRollNormalContainer_MaterialCode").val("");
    get_ds_bb();
});
var bobbinNumber = '';
var bobbinMaterialCode = '';

$(".popupDialogComposite_RollNormal_Container").dialog({
    width: '70%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 800,
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
        $('#dialogCompositeRollNormalContainer_Grid').jqGrid({

            datatype: function (postData) { getDataListBobbin(postData); },
            mtype: 'Get',
            colModel: [
                { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
                { key: false, label: 'Container Code', name: 'bb_no', width: 300, },
                { key: false, label: 'Material', name: 'mt_cd', width: 400, align: 'left' },
            ],
            pager: '#dialogCompositeRollNormalContainer_Page',
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 400,
            width: null,
            loadonce: false,
            caption: '',
            emptyrecords: 'No Data',
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            multiselect: false,
            //gridComplete: function () {
            //    var rows = grid.getDataIDs();
            //    var grid_a = $('#dialogCompositeRollNormalContainer_Grid');

            //    $('td[rowspan="1"]', grid_a).each(function () {
            //        var spans = $('td[rowspanid="' + this.id + '"]', grid_a).length + 1;

            //        if (spans > 1) {
            //            $(this).attr('rowspan', spans);
            //        }
            //    });
            //},
            //loadComplete: function () {
            //    var rows = grid.getDataIDs();
            //},
            onSelectRow: function () {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#dialogCompositeRollNormalContainer_Grid").jqGrid("getGridParam", 'selrow');
                var rowData = $("#dialogCompositeRollNormalContainer_Grid").getRowData(selectedRowId);
                bobbinNumber = rowData.bb_no;
                $("#cp_bb_no").val(rowData.bb_no);
                bobbinMaterialCode = rowData.mt_cd;
                if ((bobbinMaterialCode) && $("#type_roll").val() != "200") {
                    $('#dialogCompositeRollNormalContainer_CreateBtn').addClass('disabled');
                } else {
                    $('#dialogCompositeRollNormalContainer_CreateBtn').removeClass('disabled');
                }
            },
        });
    },
    close: function () {
    }
});

function get_ds_bb() {


    //var dialogCompositeRollNormalContainer_ContainerCode = $('#dialogCompositeRollNormalContainer_ContainerCode').val() == null ? "" : $('#dialogCompositeRollNormalContainer_ContainerCode').val().trim();
    //var dialogCompositeRollNormalContainer_ContainerName = $('#dialogCompositeRollNormalContainer_ContainerName').val() == null ? "" : $('#dialogCompositeRollNormalContainer_ContainerName').val().trim();
    //var dialogCompositeRollNormalContainer_MaterialCode = $('#dialogCompositeRollNormalContainer_MaterialCode').val() == null ? "" : $('#dialogCompositeRollNormalContainer_MaterialCode').val().trim();
    //$.ajax({
    //    url: `/TIMS/searchbobbinPopup?bb_no=${dialogCompositeRollNormalContainer_ContainerCode}&bb_nm=${dialogCompositeRollNormalContainer_ContainerName}&mt_cd=${dialogCompositeRollNormalContainer_MaterialCode}&id_actual=${actualId}`,//Hằng code
    //    type: "get",
    //    dataType: "json",
    //    success: function (result) {
    //        $("#dialogCompositeRollNormalContainer_Grid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: row = result }).trigger("reloadGrid");
    //    }
    //});

    $('#dialogCompositeRollNormalContainer_Grid').clearGridData();
    $('#dialogCompositeRollNormalContainer_Grid').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogCompositeRollNormalContainer_Grid').jqGrid('getGridParam', 'postData');
    getDataListBobbin(pdata);
}
//$("#save_bb").click(function () {
//    //insert Roll
//    if ($("#cp_bb_no").val() == "") {
//        alert("Please enter your container");
//        return false;
//    }
//    if ($("#cp_lot").val() == "") {
//        alert("Please enter your Composite Lot");
//        return false;
//    } else {
//        $.ajax({
//            url: "/TIMS/insertw_material_mping",
//            type: "get",
//            dataType: "json",
//            data: {
//                mt_cd: $("#cp_lot").val(),
//                bb_no: $("#cp_bb_no").val(),
//                id_actual: $("#id_actual").val(),
//            },
//            success: function (response) {
//                $('.popupDialogComposite_RollNormal_Container').dialog('close');
//                if (response.result != false) {
//                    SuccessAlert("Success");
//                    for (var i = 0; i < response.list.length; i++) {
//                        var id = response.list[i].wmmid;
//                        $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('addRowData', id, response.list[i], 'first');
//                        $("#dialogCompositeRollNormal_MateritalGrid").setRowData(id, false, { background: "#d0e9c6" });
//                    }
//                    //$("#mt_lot1").val(response.list[0].mt_cd);
//                } else {
//                    ErrorAlert(response.message);
//                }
//            }
//        });
//    }
//});
function getDataListBobbin(pdata) {


    var params = new Object();

    if ($('#dialogCompositeRollNormalContainer_Grid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.bb_no = $("#dialogCompositeRollNormalContainer_ContainerCode").val().trim();
    params.bb_nm = $("#dialogCompositeRollNormalContainer_ContainerName").val().trim();
    params.mt_cd = $("#dialogCompositeRollNormalContainer_MaterialCode").val().trim();
    params.id_actual = actualId;

    $('#dialogCompositeRollNormalContainer_Grid').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/searchbobbinPopup`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
      
            if (st == "success") {
                var showing = $('#dialogCompositeRollNormalContainer_Grid')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#save_bb").click(function () {
    //insert Roll
    if ($("#cp_bb_no").val() == "") {
        alert("Please enter your container");
        return false;
    }
    //alert($("#cp_lot").val());
    if ($("#cp_lot").val() == "") {
        alert("Please enter your Composite Lot");
        return false;
    } else {
        $.ajax({
            url: "/TIMS/InsertMaterialMappingRollTims",
            type: "Post",
            dataType: "json",
            data: {
                mt_cd: $("#cp_lot").val(),
                bb_no: $("#cp_bb_no").val(),
                id_actual: $("#id_actual").val(),
            },
            success: function (response) {
                $('.popupDialogComposite_RollNormal_Container').dialog('close');
                if (response.result != false) {
                    SuccessAlert("Success");
                    for (var i = 0; i < response.list.length; i++) {
                        var id = response.list[i].wmmid;
                        $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('addRowData', id, response.list[i], 'first');
                        $("#dialogCompositeRollNormal_MateritalGrid").setRowData(id, false, { background: "#d0e9c6" });
                    }
                    //$("#mt_lot1").val(response.list[0].mt_cd);
                } else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});
$("#scan_EA").click(function () {
    //insert Roll
    if ($("#cp_bb_no").val() == "") {
        alert("Please enter your container");
        return false;
    }
    $.ajax({
        url: "/TIMS/InsertMaterialMappingEATims",
        type: "POST",
        dataType: "json",
        data: {
            bb_no: $("#cp_bb_no").val(),
            psid: $("#psid").val(),
            id_actual: $("#id_actual").val(),
            staff_id: $("#staff_id").val(),
        },
        success: function (data) {
            $('.popupDialogComposite_RollNormal_Container').dialog('close');
            if (data.result == false) {
                ErrorAlert(data.message);
            } else {
                SuccessAlert("Success"); 
                var id = data.ds1.wmtid;
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('addRowData', id, data.ds1, 'first');
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").setRowData(id, false, { background: "#d0e9c6" });
            }
        }
    });

});