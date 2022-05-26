$(function () {
    $(".diologbobindv").dialog({
        width: '60%',
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
        },
    });
});
function get_ds_bb_dv(pdata) {
    //debugger;
    var pdata = $('#popupbobindv').jqGrid('getGridParam', 'postData');
    var params = new Object();
    if (pdata.page == 0) {
        params.page = 1;
    }
    else {
        params.page = pdata.page;
    }
    params.rows = pdata.rows;
   
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.bb_no = $('#s_bb_nodv').val().trim();
    params.bb_nm = $('#s_bb_nmdv').val().trim();
    $.ajax({
        url: "/TIMS/searchbobbinPopupDV",
        type: "Get",
        dataType: "json",
        data: params,
        success: function (result, st) {
            if (st == "success") {
                var showing = $('#popupbobindv')[0];
                showing.addJSONData(result);
            }
        },
        error: function () {
            return;
        }
    });
}
$("#searchBtn_popupbobbindv").click(function () {
    var pdata = $('#popupbobindv').jqGrid('getGridParam', 'postData');
    get_ds_bb_dv(pdata);
});
var select_bb = "";
$("#popupbobindv").jqGrid
    ({
        //datatype: 'json',
        datatype: function (postData) { get_ds_bb_dv(postData); },
        mtype: 'Get',
        colModel: [
            { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
            { key: false, label: 'Container Code', name: 'bb_no', width: 150, align: 'center', sort: true },
            { key: false, label: 'Container Name', name: 'bb_nm', width: 150, align: 'left' },
            { key: false, label: 'Create User', name: 'reg_id', align: 'center' },
            { label: 'Create Date', name: 'reg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Change Date', name: 'chg_dt', width: 150, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            $("#save_bb").removeClass("disabled");
            var selectedRowId = $("#popupbobindv").jqGrid("getGridParam", 'selrow');
            row_id = $("#popupbobindv").getRowData(selectedRowId);
            select_bb = row_id.bb_no;
        },
        pager: jQuery('#PageBobbindv'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        rowNum: 50,
        sortable: true,
        loadonce: false,
        height: 300,
        multiselect: false,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Container List',
        
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

$("#selected").click(function () {
    if (!select_bb) {
        alert('Select 1 bobbin.');
        return;
    }
    else {
        //debugger;
       
        var sts = $("#sts_change_bb").val();
        if (sts == "EA") {
           
            //var rowDatas = $('#dialogCompositeRollNormal_MateritalGrid').jqGrid('getRowData', $("#id_dv").val());
            //mtcdpar = rowDatas.material_code_parent;
            //mtcdchi = rowDatas.material_code;
            $.get("/TIMS/Changebb_dvEA?bb_no=" + select_bb + "&wmmid=" + $("#id_dv").val(), function (data) {
                $('.diologbobindv').dialog('close');
                if (data.result == true) {
                    SuccessAlert("Success");
                    var rowData = $('#dialogCompositeRollNormal_MateritalGrid').jqGrid('getRowData', $("#id_dv").val());
                    rowData.bb_no = select_bb;
                    $('#dialogCompositeRollNormal_MateritalGrid').jqGrid('setRowData', $("#id_dv").val(), rowData);
                } else {
                    ErrorAlert(data.message);
                }
            });
        } else {
            var rowDatas = $('#tb_mt_cd_sta').jqGrid('getRowData', $("#id_dv").val());
            mtcdpar = rowDatas.material_code_parent;
            mtcdchi = rowDatas.material_code;
            grpqty = rowDatas.gr_qty;
            $.get("/TIMS/ChangeBobinDevideTims?bb_no=" + select_bb + "&material_code=" + mtcdpar + "&material_code_child=" + mtcdchi + "&gr_qty="+grpqty, function (data) {
                $('.diologbobindv').dialog('close');
                if (data.result == true) {
                    SuccessAlert("Success");
                    var rowData = $('#tb_mt_cd_sta').jqGrid('getRowData', $("#id_dv").val());
                    rowData.bb_no = select_bb;
                    rowData.gr_qty = rowData.gr_qty;
                    rowData.sl_tru_ng = rowData.gr_qty;
                    $('#tb_mt_cd_sta').jqGrid('setRowData', $("#id_dv").val(), rowData);
                } else {
                    ErrorAlert(data.message);
                }
            });
        }
    }
});