
$(".dialog_MATERIAL").dialog({
    width: '50%',
    height: 500,
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
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { label: 'ID', name: 'materialid', key: true, align: 'center', hidden: true },
                { label: 'ML No', name: 'material_code', sortable: true, align: 'left', width: 280, },
                { label: 'Lot No', name: 'lot_no', sortable: true, align: 'left', width: 100, },
                {label: 'Length/EA', name: 'gr_qty', width: 60, align: 'right', formatter: 'integer', editrules: { integer: true, required: true },  },
                { label: 'Expiry Date', name: 'expiry_date', sortable: true, align: 'center', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
                { label: 'Date of Receipt', name: 'date_of_receipt', sortable: true, align: 'center', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
                { label: 'Expore Date', name: 'export_date', sortable: true, align: 'center', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
            ],
            cmTemplate: { title: false },
            gridComplete: function () {
                $("#load_list").hide();
                //var rows = $("#popupMaterial").getDataIDs();
                //for (var i = 0; i < rows.length; i++) {
                //    var remark = $("#popupMaterial").getCell(rows[i], "remark");
                //    if (parseInt(remark)>0) {
                //        $("#popupMaterial").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                //    }
                //}
            },
            onSelectRow: function (rowid, selected, status, e) {
            
                //$('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                $("#savestyle_material").removeClass("disabled");
                var selectedRowId = $("#popupMaterial").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupMaterial").getRowData(selectedRowId);
                if (row_id != null) {
                    $('#close_mt').click(function () {
                        $('.dialog_MATERIAL').dialog('close');
                    });
                    $("#savestyle_material").click(function () {

                        $('#s_mt_cd').val(row_id.material_code);
                        $('#pp_pr_cd').val(row_id.material_code);
                        $('.dialog_MATERIAL').dialog('close');
                    });
                }
            },

           viewrecords: true,
            height: 200,
            rowNum: 50,
            rownumbers: true,      //컬럼 맨 앞에 순번컬럼 붙일지 말지( 1,2,3...)
            rowList: [50, 100, 200, 500, 1000], //한 번에 보여줄 레코드 수를 변동할 때 선택 값
            // reload 여부이면 true로 설정하며 한번만 데이터를 받아오고 그 다음부터는 데이터를 받아오지 않음
            loadtext: "Loading...",			// 서버연동시 loading 중이라는 표시에 문자열 지정
            emptyrecords: "No data.",	// 데이터가 없을 경우 보열줄 문자열 
            gridview: true,
            shrinkToFit: false,
            datatype: function (postData) { getDataOutBox(postData); },
            pager: "#pagerMaterial",
            jsonReader:
             {
                 root: "rows",
                 page: "page",
                 total: "total",
                 records: "records",
                 repeatitems: false,
                 Id: "0"
             },
            ajaxGridOptions: { contentType: "application/json" },
            autowidth: true,
        });
    }
});
function getDataOutBox(pdata) {
    var params = new Object();
    params.page = pdata.page;
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.id_actual = $("#id_actual").val();
    //params.type = $("#s_mt_type_popup").val();
    params.mt_no = $("#mt_no_popup").val();
    params.mt_cd = $("#mt_nm_popup").val().trim();
    $("#popupMaterial").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/ActualWO/Popup_composite_mt',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, status) {
            //console.log(data);
            if (status == "success") {
                console.log(data);
                var grid = $("#popupMaterial")[0];
                grid.addJSONData(data);
            }
        }
    })
}
$("#searchBtn_material_popup").click(function () {
    $("#popupMaterial").clearGridData();
    var grid = $("#popupMaterial");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
 
});
function GetType_Marterial() {
    $.get("/ActualWO/GetType_Marterial", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#s_mt_type_popup").html(html);
        }
    });
};

$(".poupdialogMaterial").click(function () {
    $('.dialog_MATERIAL').dialog('open');
    var grid = $("#popupMaterial");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataOutBox(pdata);
    GetType_Marterial();
    jQuery("#tb_mt_cd").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
});

$('#close_mt').click(function () {
    $('.dialog_MATERIAL').dialog('close');
});



function date(cellValue, options, rowdata, action) {
    var html = "";
    if (cellValue != null && cellValue != "" && cellValue.toString().indexOf('-') == -1) {
        html = cellValue.substr(0, 4) + '-' + cellValue.substr(4, 2) + '-' + cellValue.substr(6, 2);
    }
    return html;
}