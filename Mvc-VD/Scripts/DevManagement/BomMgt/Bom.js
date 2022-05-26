var ProductCode = "";
var parentRowKey1 = 0;
var ListMaterial = [];

$("#list").jqGrid({
    datatype: function (postData) { getDataBOM(postData); },
    mtype: 'Get',
    colModel: [
        { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
        { key: false, label: 'Code', name: 'bom_no', width: 300, align: 'center', hidden: true },
        { key: false, label: 'Model', name: 'md_cd', sortable: true, width: 300, align: 'left' },
        { key: false, label: 'Product Code', name: 'style_no', width: 200, align: 'left' },
        { key: false, label: 'Product Name', name: 'style_nm', width: 200, align: 'left' },
        //{
        //    label: 'Apply(Y/N)', name: 'isapply', width: 100, align: 'center', hidden: true, formatter: function (cellvalue, options, rowObject) {

        //        return '<input type="checkbox" name="selectedCall" value="' + cellvalue + '" id = "idMaterialBom' + cellvalue + '" onclick="getCheck(this)"/>'
                  
        //    },
           
        //},
        //{ key: false, label: 'Is Apply', name: 'IsApply', width: 300, align: 'left', hidden: true},
        { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 500, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });

        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        ProductCode = row_id.style_no;
        $("#c_bom_no").val(row_id.bom_no);
        $("#bid").val(row_id.bid);
        $("#c_style_no").val(row_id.style_no);
        $("#c_mt_no").val(row_id.mt_no);
        $("#c_cav").val(row_id.cav);
        $("#c_need_time").val(row_id.need_time);
        $("#c_mt_no_view").val(row_id.mt_no);
        $("#c_buocdap").val(row_id.buocdap);
        //$(".materialpp").hide();
        //var isCheck = row_id.IsActive;
        //if (isCheck == 1) {
        //    document.getElementById("isActive").checked = true;
        //}
        //else {
        //    document.getElementById("isActive").checked = false;
        //}
        var lastSelection = "";

        if (rowid && rowid !== lastSelection) {
            var grid = $("#list");
            grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
            lastSelection = rowid;
        }
    },
    onCellSelect: editRow,
    pager: jQuery('#jqGridPager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    multiselect: true,
    shrinkToFit: false,
    viewrecords: true,
    height: 300,
    width: $(".boxList").width() - 5,
    sortable: true,
    loadonce: false,
    subGrid: true,
    subGridRowExpanded: showChildGridBom,
    caption: 'Bom Information',
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    gridComplete: function () {

        //var rows = $("#list").getDataIDs();
        //for (var i = 0; i < rows.length; i++) {
        //    var ID = $("#list").getCell(rows[i], "bid");
        //    var IsActive = $("#list").getCell(rows[i], "IsApply");
        //    if (IsActive == 'Y') {
        //        $("#idMaterialBom" + ID).attr("checked", true);
              
        //    }
        //    else {
             
        //        $("#idMaterialBom" + ID).attr("checked", false);
        //    }
        //}

    }
});
//$.jgrid.defaults.responsive = true;
//$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".boxList").width());
var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
}

var childGridID = "";
function showChildGrid(parentRowID, parentRowKey) {
    parentRowKey1 = parentRowKey;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    $("#" + childGridID).jqGrid({
        url: "/DevManagement/getBomMaterial?id=" + parentRowKey,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            {
                key: true, label: 'Check', name: 'MaterialBOMID', align: 'center', hidden: true

            },
            {
                label: 'Material No', name: 'MaterialBOMID', width: 100, align: 'center', formatter: function (cellvalue, options, rowObject) {

                    return '<input type="radio" name="selectedCall" value="' + cellvalue + '" id = "idMaterialBom' + cellvalue + '" onclick="getRadio(this)"/>'
                }
            },
            { label: 'Material No', name: 'materialNo', width: 150, },
            { label: 'Material Name', name: 'materialName', width: 400, sortable: true, },
            { label: 'IsActive', name: 'active', width: 400, sortable: true, hidden: true },
      
            { name: '', label: 'Delete', width: 100, align: 'center', formatter: DeleteMaterial, exportcol: false },


        ],
        shrinkToFit: false,
        loadonce: false,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        rowNum: 50,
        pager: "#" + childGridPagerID,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        gridComplete: function () {

            var rows = jQuery("#" + childGridID).getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var MaterialBOMID = jQuery("#" + childGridID).getCell(rows[i], "MaterialBOMID");
                var IsActive = jQuery("#" + childGridID).getCell(rows[i], "IsActive");
                if (IsActive == 'true') {
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    'background-color': 'green',
                    //});
                }
                else {
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    background: 'yellow',
                    //});
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", false);
                }
            }

        }
    });
}

function showChildGridBom(parentRowID, parentRowKey) {
    parentRowKey1 = parentRowKey;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $('#list').jqGrid('getRowData', parentRowKey);
  
    $("#" + childGridID).jqGrid({
        url: "/DevManagement/getBomdsMaterial?style_no=" + rowData.style_no,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
            { key: false, label: 'Code', name: 'bom_no', width: 150, align: 'center', hidden: true },
            { name: '', label: 'Add', width: 50, align: 'center', formatter: AddMaterial, exportcol: false },
            { key: false, label: 'Model', name: 'md_cd', sortable: true, width: 100, align: 'left', hidden: true },
            { key: false, label: 'Product Code', name: 'style_no', width: 150, align: 'left', hidden: true },
            { label: 'MT Code', name: 'mt_no', width: 130 },

            { label: 'MT Name', name: 'mt_nm', sortable: true, width: 150 },
            { label: 'Tính hiệu suất', name: 'IsActive', width: 100 },
            { key: false, label: 'CAV', name: 'cav', sortable: true, width: '50', align: 'right', formatter: 'integer' },
            { key: false, label: 'Số lần sử dụng', name: 'need_time', sortable: true, width: '100', align: 'right', formatter: 'integer' },
            { key: false, label: 'Bước dập/Số đo (mm)', name: 'buocdap', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Số Mét liệu cần để tạo ra 1EA(m)', name: 'need_m', sortable: true, width: '150', align: 'right', formatter: 'integer' },
            { key: false, label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { name: '', label: 'Delete', width: 100, align: 'center', formatter: DeleteBomMaterial, exportcol: false },

        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });

            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
            row_id = $("#" + childGridID).getRowData(selectedRowId);
            ProductCode = row_id.style_no;
            $("#c_bom_no").val(row_id.bom_no);
            $("#bid").val(row_id.bid);
            $("#c_style_no").val(row_id.style_no);
            $("#c_mt_no").val(row_id.mt_no);
            $("#c_cav").val(row_id.cav);
            $("#c_need_time").val(row_id.need_time);
            $("#c_mt_no_view").val(row_id.mt_no);
            $("#c_buocdap").val(row_id.buocdap);
            //$(".materialpp").hide();
            var isCheck = row_id.IsActive;
            if (isCheck == 'true') {
                document.getElementById("isActive").checked = true;
            }
            else {
                document.getElementById("isActive").checked = false;
            }

        },
        shrinkToFit: false,
        loadonce: false,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rownumbers: true,

        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        subGrid: true,
        subGridRowExpanded: showChildGridCap3,
        pager: "#" + childGridPagerID,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        gridComplete: function () {

            var rows = jQuery("#" + childGridID).getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var MaterialBOMID = jQuery("#" + childGridID).getCell(rows[i], "MaterialBOMID");
                var IsActive = jQuery("#" + childGridID).getCell(rows[i], "IsActive");
                if (IsActive == 'true') {
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    'background-color': 'green',
                    //});
                }
                else {
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    background: 'yellow',
                    //});
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", false);
                }
            }

        }
    });
}

function getRadio(e) {
    var r = confirm("Material này sẽ được chọn làm nguyên vật liệu để tính hiệu xuất, bạn có chắc không?");
    if (r == true) {
        $.ajax({
            url: "/DevManagement/UpdateMaterialDeTinhHieuSuat",
            type: "get",
            headers: {
                "Content-Type": "application/json"
            },
            data: {
                productCode: e.value,
                //BOMID: parentRowKey1,
            },
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);

                }
                else {
                    ErrorAlert(data.message);
                }
            },
        });

    } else {

        return false;
    }
}

function getCheck(e) {
    var r = confirm("Bạn có chắc muốn thay đổi không?");
    var checkbox = document.getElementById("idMaterialBom" + e.value);
    var check = "";

    if (checkbox.checked === true) {
        check = "Y";
    }
    else {
        check = "N";
    }
    if (r == true) {
        // Khai báo tham số
        $.ajax({
            url: "/DevManagement/UpdateProductDeApply",
            type: "get",
            headers: {
                "Content-Type": "application/json"
            },
            data: {
                bid: e.value,
                IsApply: check,
            },
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    

                }
                else {
                    ErrorAlert(data.message);                  
                }
            },
        });

    } else {
       
        if (check === "Y") {
            document.getElementById("idMaterialBom" + e.value).checked = false;
        }
        else {
            document.getElementById("idMaterialBom" + e.value).checked = true;
        }
        return false;
    }

}

$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
});

$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
        var materialNo = $("#c_mt_no_view").val();
        var ProductCd = $("#c_style_no").val();
        if (materialNo == "" || ProductCd == "") {
            alert("Vui lòng chọn Product và Liệu");
            return;
        }
        var isActive = 0;
        var y = $('#isActive').is(':checked');
        if (y) {
            isActive = 1;
        }

debugger;
        var c_cav = $("#c_cav").val();
        var c_need_time = $("#c_need_time").val();
        var c_buocdap = $("#c_buocdap").val();

        var settings = {
            "url": "/DevManagement/CreateBomManagement",
            "method": "POST",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify({
                "materialNo": materialNo,
                "ProductCode": ProductCode,
                "cavit": parseInt(c_cav),
                "need_time": parseInt(c_need_time),
                "buocdap": parseFloat(c_buocdap),
                "isActive": isActive,
            }),
        };

        $.ajax(settings).done(function (data) {
            if (data.result == true) {
                var id = data.data.bid;
                $("#" + childGridID).jqGrid('addRowData', id, data.data, 'first');
                $("#" + childGridID).setRowData(id, false, { background: "#d0e9c6" });
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        });
    }
});

$("#m_save_but").click(function () {

debugger;
    var isActive = 0;
        var y = $('#isActive').is(':checked');
        if (y == true) {
            isActive = 1;
    } 

    $.ajax({
        type: "PUT",
        contentType: 'application/json',
        url: "/DevManagement/ModifyBomManagement",
        data: JSON.stringify({

            bid: $('#bid').val(),
            bom_no: $("#c_bom_no").val(),
            style_no: $("#c_style_no").val(),
            mt_no: $("#c_mt_no_view").val(),
            mt_no: $("#c_mt_no_view").val(),
            cav: parseInt($("#c_cav").val()),
            need_time: parseFloat($("#c_need_time").val()),
            buocdap: parseFloat($("#c_buocdap").val()),
            isActive: isActive
        }),
        success: function (data) {
            debugger;
            if (data.result) {
                var id = data.data[0].bid;
                $("#" + childGridID).setRowData(id, data.data[0], { background: "#d0e9c6" });
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
        error: function (data) {
            ErrorAlert("Lỗi!!");
        }
    });
    
});

$("#searchBtn").click(function () {
    $("#list").clearGridData();
    var grid = $("#list");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');
    getDataBOM(pdata);
});

function getDataBOM(pdata) {
    
    var product = $("#s_style_no").val().trim();
    var product_nm = $("#s_style_nm").val().trim();
    var md_cd = $("#s_md_cd").val().trim();
    var mt_no = $("#s_mt_cd").val().trim();
    var mt_nm = $("#s_mt_nm").val().trim();
    var params = new Object();
    if (jQuery('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params.product = product;
    params.product_nm = product_nm;
    params.md_cd = md_cd;
    params.mt_no = mt_no;
    params.mt_nm = mt_nm;

    //params.end1Data = type;
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    params._search = pdata._search;

    $.ajax({
        url: '/DevManagement/GetBom',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
                $('.loading').hide();
            }
        }
    })
};

$("#deletestyle").click(function () {
    debugger;
    var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
    row_id = $("#list").getRowData(selectedRowId);
    
    $.ajax({
        url: `/DevManagement/deletebom`,
        type: "Delete",
        dataType: "json",
        data: {
            bid: row_id.bid,
        },
        success: function (data) {
            if (data.result == true) {
                var id = row_id.bid;
                $("#list").jqGrid('delRowData', id);
                jQuery("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
    $('#dialogDangerous').dialog('close');
});

$("#form1").validate({
    rules: {
        //"bom_no": {
        //    required: true,
        //},
        "style_no": {
            required: true,
        },

        "cav": {
            required: true,
            number: true,
        },
        "need_time": {
            required: true,
        },
        "buocdap": {
            required: true,
        },
    },
});

function AddMaterial(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-success button-srh"  data-style_no="${rowdata.style_no}"  data-bid="${rowdata.bid}" data-mt_no="${rowdata.mt_no}" onclick="AddMaterialOnClick(this)">+</button>`;
}

function AddMaterialOnClick(e) {
    $("#ProductCode").val(e.dataset.style_no);
    $("#c_mt_no").val(e.dataset.mt_no);
    $("#bid").val(e.dataset.bid);
    $('.dialog_addMATERIAL').dialog('open');
}

function DeleteMaterial(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-MaterialBOMID="${rowdata.MaterialBOMID}"  onclick="DeleteMaterialOnClick(this)">X</button>`;
}

function DeleteMaterialOnClick(e) {
    var id = e.dataset.materialbomid;
    var settings = {
        "url": "/DevManagement/DeleteMaterialBomManagement",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "id": id
        }),
    };
    $.ajax(settings).done(function (data) {
        if (data.result) {
            SuccessAlert(data.message);
            $("#" + childGridID).jqGrid('delRowData', id);
        }
        else {
            ErrorAlert(data.message);
        }
    });
}

function DeleteBomMaterial(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-bid="${rowdata.bid}" onclick="DeleteBomMaterialOnClick(this)">X</button>`;
}

function DeleteBomMaterialOnClick(e) {
    var result = confirm("Bạn có chắc muốn xóa không?");
    if (result) {
        var bid = e.dataset.bid;
        var settings = {
            "url": "/DevManagement/DelMaterialBomManagement",
            "method": "Delete",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify({
                "bid": bid
            }),
        };
        $.ajax(settings).done(function (data) {
            if (data.result) {

                $("#" + childGridID).jqGrid('delRowData', data.id);
                SuccessAlert(data.message);
                return false;
            }
            else {
                ErrorAlert(data.message);
                return false;
            }
        });
    }
    {
        return false;
    }
   
}
var parentRowKey2 = 0;
var childGridID2 = "";

function showChildGridCap3(parentRowID, parentRowKey) {
    parentRowKey2 = parentRowKey;
    childGridID2 = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID2 + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $("#" + childGridID).jqGrid('getRowData', parentRowKey);


    $("#" + childGridID2).jqGrid({
        url: "/DevManagement/getBomdsMaterialCap3?style_no=" + rowData.style_no + "&mt_no=" + rowData.mt_no,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'Id', name: 'id', width: 100, align: 'center',hidden:true },
            { label: 'MT Code', name: 'materialno', width: 140 },
            { label: 'MT Name', name: 'materialname', sortable: true, width: 150 },
            { key: false, label: 'Create Date', name: 'CreateDate', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { name: '', label: 'Delete', width: 100, align: 'center', formatter: DeleteBomMaterial2, exportcol: false },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
            row_id = $("#" + childGridID).getRowData(selectedRowId);
            ProductCode = row_id.style_no;
        },
        shrinkToFit: false,
        loadonce: false,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rownumbers: true,

        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,
        subGrid: false,
     
        pager: "#" + childGridPagerID,
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        gridComplete: function () {

            var rows = jQuery("#" + childGridID).getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var MaterialBOMID = jQuery("#" + childGridID).getCell(rows[i], "MaterialBOMID");
                var IsActive = jQuery("#" + childGridID).getCell(rows[i], "IsActive");
                if (IsActive == 'true') {
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //("#idMaterialBom" + MaterialBOMID).attr("checked", true);
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    'background-color': 'green',
                    //});
                }
                else {
                    //jQuery("#" + childGridID).jqGrid('setCell', rows[i], "MaterialBOMID", "", {
                    //    background: 'yellow',
                    //});
                    $("#idMaterialBom" + MaterialBOMID).attr("checked", false);
                }
            }

        }
    });
}

function DeleteBomMaterial2(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-Id="${rowdata.id}" onclick="DeleteBomMaterialOnClick2(this)">X</button>`;
}

function DeleteBomMaterialOnClick2(e) {
    var Id = e.dataset.id;
    var r = confirm("Bạn có chắc muốn xóa không?");
    if (r == true) {
        var settings = {
            "url": "/DevManagement/DelMaterialBom",
            "method": "Delete",
            "timeout": 0,
            "headers": {
                "Content-Type": "application/json"
            },
            "data": JSON.stringify({
                "Id": Id
            }),
        };

        $.ajax(settings).done(function (data) {
            if (data.result == true) {
                SuccessAlert(data.message);
                $("#" + childGridID2).jqGrid('delRowData', Id);
            }
            else {
                ErrorAlert(data.message);
            }
        });
    }
    else {
        return false;
    }
}