function PercentagePrimary(cellValue, options, rowdata, action) {
    //debugger;
    if (rowdata.target == 0 || rowdata.process_count == 0 || rowdata.totalTarget == undefined || rowdata.actual == undefined) {
        return 0.00 + ` %`;
    }
    return (((rowdata.actual / rowdata.target)) / rowdata.process_count * 100).toFixed(2) + ` %`;
}


function Percentage(cellValue, options, rowdata, action) {
    if (rowdata.target == 0 || rowdata.target == undefined || rowdata.actual == undefined || rowdata.actual == 0) {
        return 0.00 + ` %`;
    }
    return ((rowdata.actual / rowdata.target) * 100).toFixed(2) + ` %`;

}
function ProcessCalculating(cellValue, options, rowdata, action) {

 //debugger;
    var html = "";
    html = rowdata.process_count + "/" + rowdata.count_pr_w;
    return html;
}
function FinishPO(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-po="${rowdata.at_no}" onclick="FinishPOOnClick(this)">Finish</button>`;
}
//function hieusuat(cellValue, options, rowdata, action) {

//    if (rowdata.Actual == 0 || rowdata.Actual == undefined || rowdata.Actual == undefined) {
//        return 0.00 + ` %`;
//    }
//    if (rowdata.Actulalythuyet == 0 || rowdata.Actulalythuyet == undefined || rowdata.Actulalythuyet == undefined) {
//        return 0.00 + ` %`;
//    }
//    var  abc = ((rowdata.Actual / rowdata.Actulalythuyet) * 100).toFixed(2) + ` %`;
//    return abc;
//}


function hieusuat(cellValue, options, rowdata, action) {
    //need_m
    var need_m = rowdata.need_m;
    var SoMetSd = rowdata.lieuthaythe + rowdata.mLieu;
    var Actulalythuyet = 1 / need_m * SoMetSd;

    // ifnull((1/TABLE1.need_m)*(sum(w_m.gr_qty)),0) AS Actulalythuyet,

    if (rowdata.Actual == 0 || rowdata.Actual == undefined || rowdata.Actual == undefined) {
        return 0.00 + ` %`;
    }
    if (Actulalythuyet == 0 || Actulalythuyet == undefined || Actulalythuyet == undefined) {
        return 0.00 + ` %`;
    }
    var abc = ((rowdata.Actual / Actulalythuyet) * 100).toFixed(2) + ` %`;
    return abc;
}


function MetLieuSuDung(cellValue, options, rowdata, action) {
    return rowdata.lieuthaythe + rowdata.mLieu;
}


var poToFinish = undefined;
function FinishPOOnClick(e) {
    poToFinish = undefined;
    poToFinish = e.dataset.po;
    $('#dialog_Finsh').dialog('open');
}

function getData_primary(pdata) {
    //debugger;
    var params = new Object();

    if ($('#list_primary').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.at_no = $("#sPO").val().trim();
    params.bom_type = $("#s_bom_type").val().trim();
    params.product = $("#sProductCode").val().trim();
    params.product_name = $("#sProductName").val().trim();
    params.model = $("#sModel").val().trim();
    params.regstart = $("#sDate").val().trim();
    params.regend = $("#eDate").val().trim();
    params.type = "Web";

    $('#list_primary').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    //debugger;
    $.ajax({
        url: `/ActualWO/GetDataWActualPrimary`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list_primary')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });

     //getData_Finished(pdata);
};



$("#list_primary").jqGrid({
    datatype: function (postData) { getData_primary(postData); },
    mtype: 'Get',
    colModel: [
        { key: true, label: 'id_actualpr', name: 'id_actualpr', width: 80, align: 'center', hidden: true },
        { key: false, label: '', name: 'IsApply', width: 300, align: 'left', hidden: true },
        { label: '', name: 'actual', hidden: true },
        { label: '', name: 'process_count', hidden: true },
        { label: '', name: 'count_pr_w', hidden: true },
        { label: '', name: 'totalTarget', hidden: true },
        { label: '', name: 'poRun', width: 80, align: 'center', hidden: true },
        { label: '', name: 'CountProcess', width: 80, align: 'center', hidden: true },
        {
            label: 'Apply(Y/N)', name: 'id_actualpr1', width: 80, align: 'center', formatter: function (cellvalue, options, rowObject) {

                return '<input type="checkbox" name="selectedCall" value="' + cellvalue + '" id = "id' + cellvalue + '" onclick="getCheck(this)"/>'

            },
        },
        { label: 'PO NO', name: 'at_no', width: 110, align: 'center'},
        { label: 'Create', name: 'create', width: 80, formatter: create_actualcon, align: 'center', },
        { label: 'View', name: 'View', width: 80, formatter: ViewDetailMaterial, align: 'center', },
        { label: 'Product', name: 'product', width: 100 },
        { label: 'Product Name', name: 'style_nm', width: 130 },
        { label: 'Model', name: 'md_cd', width: 100 },
        { label: 'Type', name: 'bom_type', width: 100 },
        { label: 'process_code', name: 'process_code', width: 50 },
        { label: 'Description', name: 'remark', width: 180 },
        { label: 'Target', name: 'target', width: 90, align: 'right', formatter: 'integer' },
        { label: 'Complete (%)', name: '', width: 90, align: 'center', formatter: PercentagePrimary },
        { label: 'Processing', name: '', width: 90, align: 'center', formatter: ProcessCalculating },
        { label: '', name: '', width: 70, align: 'center', formatter: FinishPO },


    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list_primary").jqGrid("getGridParam", 'selrow');
        row_id = $("#list_primary").getRowData(selectedRowId);

        $("#m_style_no").val(row_id.product);
        $("#product").val(row_id.product);
        //alert(row_id.product);
        $("#m_target").val(row_id.target);
        $("#m_remark").val(row_id.remark);
        $("#at_no").val(row_id.at_no);
        $("#mp_style_no").val(row_id.product);
        $("#qc_name").val(row_id.product);
        $("#id_actualpr").val(row_id.id_actualpr);
        $.ajax({
            url: `/ActualWO/GetProductProcesses?productCode=` + encodeURIComponent(row_id.product)
        })
            .done(function (response) {
               // debugger;
                var html = '';
                if (response.length > 0) {

                    html += '<option value=' + response[0].process_code + '>' + response[0].process_name + '</option>';
                    for (var i = 1; i < response.length; i++) {
                        html += '<option value=' + response[i].process_code + '>' + response[i].process_name + '</option>';
                    }

                }
                $("#c_Process").html(html);
                $("#m_Process").html(html);
                $("#m_Process").val(row_id.process_code);
                return;
            })
            .fail(function () {
                return;
            });
        $('#list').clearGridData();
        $('#list').jqGrid('setGridParam', { search: true });
        var pdata = $('#list').jqGrid('getGridParam', 'postData');
        getData(pdata);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", false);
        $("#del_save_but").attr("disabled", false);
    },
    pager: jQuery('#jqGrid_primaryPager'),
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000],
    height: 300,
    width: $(".box-body").width() - 5,
    autowidth: false,
    rowNum: 50,
    caption: '',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: false,
    shrinkToFit: false,
    multiselect: false,
    subGrid: true,
    subGridRowExpanded: showChildGridMaterial,
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
        //debugger;
        var rows = $("#list_primary").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var poRun = $("#list_primary").getCell(rows[i], "poRun");
            var CountProcess = $("#list_primary").getCell(rows[i], "CountProcess");

            if (poRun == CountProcess && CountProcess > 0) {
                $("#list_primary").jqGrid('setCell', rows[i], "at_no", "", {
                    'background-color': '#05d405',
                    color: '#fff',
                    'font-size': '1.025em'
                });
            }
            else if (CountProcess > poRun && poRun > 0) {
                $("#list_primary").jqGrid('setCell', rows[i], "at_no", "", {
                    'background-color': 'yellow',
                    color: '#000',
                    'font-size': '1.025em'
                });
            }
            //if (poRun != null && poRun != undefined && poRun != "") {
            //    $("#list_primary").jqGrid('setCell', rows[i], "at_no", "", {
            //        'background-color': '#05d405',
            //        color: '#fff',
            //        'font-size': '1.025em'
            //    });
            //}
            var id_actualpr = $("#list_primary").getCell(rows[i], "id_actualpr");
            var IsActive = $("#list_primary").getCell(rows[i], "IsApply");
            if (IsActive == 'Y') {
                $("#id" + id_actualpr).attr("checked", true);
            }
            else {
                $("#id" + id_actualpr).attr("checked", false);
            }
                //$("#list_primary").jqGrid('setRowData', rows[i], false, { background: '#05d405', color: '#fff', 'font-size': '1.025em' });
        }
    },
});
function getCheck(e) {
    //debugger;
    var r = confirm("Bạn có chắc muốn thay đổi không?");
    var checkbox = document.getElementById("id" + e.value);
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
            url: "/ActualWO/UpdateProductDeApply",
            type: "Put",
            //headers: {
            //    "Content-Type": "application/json"
            //},
            data: {
                id_actualpr: e.value,
                IsApply: check,
            },
            success: function (data) {
                //debugger;
                if (data.result == true) {
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            },
        });
    } else {
        if (check === "Y") {
            document.getElementById("id" + e.value).checked = false;
        }
        else {
            document.getElementById("id" + e.value).checked = true;
        }
        return false;
    }

}
var childGridID = "";

function showChildGridMaterial(parentRowID, parentRowKey) {
    parentRowKey1 = parentRowKey;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $('#list_primary').jqGrid('getRowData', parentRowKey);


    $("#" + childGridID).jqGrid({
        url: "/ActualWO/getBomdsMaterial?style_no=" + rowData.product + "&at_no=" + rowData.at_no + "&process_code=" + rowData.process_code,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
           { key: true, label: 'bid', name: 'bid', width: 100, align: 'center', hidden: true },
            { key: false, label: 'Code', name: 'at_no', width: 150, align: 'center', hidden: true },
            { key: false, label: 'Code', name: 'ProductCode', width: 150, align: 'center', hidden: true },
            { key: false, label: 'process_code', name: 'process_code', width: 150, align: 'center', hidden: true },
            { label: 'MT Code', name: 'MaterialNo', width: 130 },

            { label: 'MT Name', name: 'MaterialName', sortable: true, width: 400 },
            { label: 'need_m', name: 'need_m', sortable: true, width: 100, hidden: true },
            { label: 'lieuthaythe', name: 'lieuthaythe', sortable: true, width: 100, hidden: true },
            { label: 'mLieu', name: 'mLieu', sortable: true, width: 100, hidden: true },
            { label: 'Tổng số mét đã sử dụng', name: 'mLieu', width: 150, align: 'right', formatter: MetLieuSuDung},
            { label: 'Actulalythuyet', name: 'Actulalythuyet', width: 150, align: 'right', hidden: true, formatter: 'integer' },
            { label: 'actual', name: 'Actual', width: 150, align: 'right', hidden: true },
            { label: 'Hiệu suất(%)', name: 'hieusuat', sortable: true, width: '100', align: 'right', formatter: hieusuat },
            { label: 'NVL đã scan', name: 'SoCuonNVL', sortable: true, width: '100', align: 'right' }

        ],
        onSelectRow: function (rowid, selected, status, e) {

/*debugger;*/
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
            row_id = $("#" + childGridID).getRowData(selectedRowId);


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

         pager: "#" + childGridPagerID,
        subGrid: true,
        subGridRowExpanded: showChildGridMaterialReplace,

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

            }

        }
    });
}
function getData_Finished(pdata) {
    var params = new Object();

    if (jQuery('#list_finish').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.at_no = $("#sPO").val().trim();
    params.product = $("#sProductCode").val().trim();
    params.product_name = $("#sProductName").val().trim();
    params.model = $("#sModel").val().trim();
    params.regstart = $("#sDate").val().trim();
    params.regend = $("#eDate").val().trim();

    //params.product = $("#sProductCode").val().trim();
    //params.at_no = $("#at_nos").val().trim();
    //params.reg_dt = $("#sDate").val().trim();
    $('#list_finish').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/ActualWO/GetFinishPO`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list_finish')[0];
                if (!data.rows || !data.rows.length)
                {
                    return;
                }
                else
                {
                    showing.addJSONData(data);
                }
            }
        },
        error: function () {
            return;
        }
    });
};
function ShowFinishedPOList() {

    $("#list_finish").jqGrid({
        datatype: function (postData) { getData_Finished(postData); },
        mtype: 'Get',
        colModel: [
            { key: true, label: 'id_actualpr', name: 'id_actualpr', width: 80, align: 'center', hidden: true },
            { label: '', name: 'actual', hidden: true },
            { label: '', name: 'process_count', hidden: true },
            { label: '', name: 'totalTarget', hidden: true },

            { label: 'PO NO', name: 'at_no', width: 120 },
            //{ label: 'Create', name: 'create', width: 120, formatter: create_actualcon, align: 'center', },
            { label: 'Product', name: 'product', width: 120 },
            { label: 'Product Name', name: 'style_nm', width: 130 },
            { label: 'Model', name: 'md_cd', width: 130 },
            { label: 'Description', name: 'remark', width: 130 },
            { label: 'Target', name: 'target', width: 100, align: 'right', formatter: 'integer' },

            { label: 'Complete (%)', name: '', width: 100, align: 'center', formatter: PercentagePrimary },
            { label: 'Processing', name: '', width: 80, align: 'center', formatter: ProcessCalculating },
            { label: 'Redo', name: 'redo', width: 80, align: 'center', formatter: Redo },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list_finish").jqGrid("getGridParam", 'selrow');
            var row_id = $("#list_finish").getRowData(selectedRowId);
            $("#product").val(row_id.product);
            $("#list").setGridParam({ url: "/ActualWO/GetDataWActual?at_no=" + row_id.at_no, datatype: "json" }).trigger("reloadGrid");
        },
        pager: jQuery('#jqGrid_finishPager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: '',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        loadonce: false,
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
}

$("#c_save_but").click(function () {

    if ($("#form1").valid() == true) {
        var XacNhan = confirm("Bạn có chắc muốn Tạo PO mới?");
        if (XacNhan) {
            $.ajax({
                url: "/ActualWO/Add_w_actual_primary",
                type: "Post",
                dataType: "json",
                data: {
                    product: $('#c_style_no').val(),
                    process_code: $('#c_Process').val(),
                    target: $('#c_target').val().toString().replace(",", "").replace(",", "").replace(",", ""),
                    remark: $('#c_remark').val(),
                },
                success: function (data) {
                    //  console.log(data);
                    if (data.result) {
                        var id = data.kq.id_actualpr;
                        $("#list_primary").jqGrid('addRowData', id, data.kq, 'first');
                        $("#list_primary").setRowData(id, false, { background: "#d0e9c6" });

                        $("#id" + id).prop("checked", true);

                        SuccessAlert("Tạo PO Thành Công");


                        $('#list').clearGridData();
                        $('#list').jqGrid('setGridParam', { search: true });
                        var pdata = $('#list').jqGrid('getGridParam', 'postData');



                        var params = new Object();

                        if ($('#list').jqGrid('getGridParam', 'reccount') == 0) {
                            params.page = 1;
                        }
                        else { params.page = pdata.page; }

                        params.rows = pdata.rows;
                        params.sidx = pdata.sidx;
                        params.sord = pdata.sord;
                        params._search = pdata._search;

                        params.at_no = data.kq.at_no;

                        $('#list').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

                        $.ajax({
                            url: "/ActualWO/GetDataWActual",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            traditional: true,
                            data: params,
                            success: function (data, st) {
                                if (st == "success") {
                                    var showing = $('#list')[0];
                                    showing.addJSONData(data);
                                }
                            },
                            error: function () {
                                return;
                            }
                        });


                    }
                    else {
                        ErrorAlert(data.message);
                    }
                },
            });
        }
    }
});

$("#m_save_but").click(function () {
    if ($("#form1").valid() == true)
    {
        var TargerNumber = $('#m_target').val().toString().replaceAll(",", "");
        $.ajax({
            url: "/ActualWO/Modify_w_actualprimary",
            type: "PUT",
            dataType: "json",
            data: {
                id_actualpr: $('#id_actualpr').val(),
                product: $('#m_style_no').val(),
                process_code: $('#m_Process').val(),
                target: TargerNumber,
                remark: $('#m_remark').val(),
            },
            success: function (data) {
                //debugger;
                if (data.result) {
                    var id = data.kq.id_actualpr;
                    $("#list_primary").setRowData(id, data.kq, { background: "#d0e9c6" });
                    SuccessAlert("");

                    $('#list').clearGridData();
                    $('#list').jqGrid('setGridParam', { search: true });
                    var pdata = $('#list').jqGrid('getGridParam', 'postData');
                    getData(pdata);
                    //getData_primary(pdata);

                } else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
});

$("#searchBtn").click(function () {
    $('#list_primary').clearGridData();
    $('#list_primary').jqGrid('setGridParam', { search: true });
    var pdata = $('#list_primary').jqGrid('getGridParam', 'postData');
    getData_primary(pdata);
    getData_Finished(pdata);
});

$("#saverecreate_actual").click(function () {
    var name = $("#c_name").val();
    if (name == "") {
        ErrorAlert("PLease choose name");
        return;
    } else {
        var style_no = $("#m_style_no").val();
        var at_no = $("#at_no").val();
        var name = $("#c_name").val();
        $.ajax({
            url: "/ActualWO/Add_w_actual",
            type: "Post",
            dataType: "json",
            data: {
                style_no: style_no,
                at_no: at_no,
                name: name,
            },
            success: function (data) {
                $('#dialogCreate_actual').dialog('close');
                if (data.result == true) {
                    var id = data.kq.id_actual;
                    $("#list").jqGrid('addRowData', id, data.kq, 'last');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    jQuery('#list').trigger('reloadGrid');
                    SuccessAlert(data.message);
                } else {
                    //alert(data.message);
                    ErrorAlert(data.message);
                }
            },
        });
    }
});

function create_actualcon(cellValue, options, rowdata, action) {
    var html = "";
    html = '<button class="btn btn-xs btn-primary"  onclick="create_actual_con(this);">Create</button>';
    return html;
}
function Redo(cellValue, options, rowdata, action) {
    var html = "";
    html = `<button class="btn btn-xs btn-primary"  data-id_actualpr="${rowdata.id_actualpr}"  onclick="redo_con(this);">REDO</button>`;
    return html;
}
function create_actual_con(e) {
    $("#c_name").val($("#target option:first").val());
    $('#dialogCreate_actual').dialog('open');
}

$(document).ready(function (e) {
    $("#sDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeYear: true
    });
    $("#eDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeYear: true
    });
});

$("#dialogCreate_actual").dialog({
    width: '30%',
    height: 200,
    maxWidth: '20%',
    maxHeight: 500,
    minWidth: '20%',
    minHeight: 100,
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
    },
});

//LIST
$("#list").jqGrid({
    url: `/ActualWO/GetDataWActual`,
    mtype: 'Get',
    colModel: [
        { key: true, label: 'id_actual', name: 'id_actual', width: 80, align: 'center', hidden: true },
        { label: '', name: 'target', width: 80, align: 'center', hidden: true },
        { label: 'View', name: '', width: 80, formatter: ViewDetailDatetime, align: 'center', },
        { label: 'Name', name: 'name', width: 80 },

        {
            key: false, label: 'Description', name: 'description', width: 230, align: 'left', editable: true, editrules: { required: true }, editoptions: {
                dataInit: function (element) {
                    $(element).keydown(function (e) {
                        if (e.which == 13) {
                            var XacNhan = confirm("Bạn có chắc muốn thay đổi?");
                            if (XacNhan) {
                                var Noidung = this.value;

                                if ((Noidung == "") || (Noidung == undefined)) {
                                    ErrorAlert("Vui lòng điền nội dung muốn thay đổi.");
                                    return false;
                                }

                                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                                row_id = $("#list").getRowData(selectedRowId);
                                $.ajax({
                                    url: '/ActualWO/updateDescriptionWActual',
                                    type: "PUT",
                                    dataType: "json",
                                    data: {
                                        id_actual: row_id.id_actual,
                                        description: Noidung
                                    },
                                    success: function (data) {
                                        if (data.result) {
                                            SuccessAlert(data.message);

                                        }
                                        else {
                                            ErrorAlert(data.message);
                                            jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                        }

                                    },
                                    error: function () {
                                        ErrorAlert(data.message);
                                        jQuery("#list").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                    }
                                });
                            }
                            else {
                                return false;
                            }
                        }

                    });
                }
            }
        },
        { label: 'Machine', name: 'mc_no', width: 100 },
        { label: 'Process (%)', name: '', width: 120, align: 'center', formatter: Percentage },
        { label: 'QC', name: 'item_vcd', width: 120, hidden: true },
        { label: 'Actual', name: 'actual', width: 80, align: 'right', formatter: 'integer' },
        { label: 'Actual night shift', name: 'actual_cd', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Actual day shift', name: 'actual_cn', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Defective', name: 'defect', width: 80, align: 'right', formatter: 'integer' },
        //{ label: 'Description', name: 'remark', width: 80, align: 'right' },
        { key: false, label: 'Composite', name: '', width: 80, align: 'right', formatter: composite_pp },

        { label: 'Create User', name: 'reg_id', width: 90, align: 'left' },
        //{ label: 'Change Name', name: 'chg_id', width: 90, align: 'left', },
        //{ label: 'Change Date', name: 'chg_dt', width: 100, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
        //{ label: 'Date', name: 'date', width: 80, align: 'center' },
        { label: 'Date', name: 'date', align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
        { label: 'Delete', name: 'delete', width: 100, align: 'center', formatter: xoa_wactual },
        { label: 'ProcessRun', name: 'ProcessRun', width: 100, align: 'center', hidden: true },
        { label: 'Level', name: 'level', width: 80 }
    ],
    gridComplete: function () {
        var rows = $("#list").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var ProcessRun = $("#list").getCell(rows[i], "ProcessRun");


            if (ProcessRun == 2) {//nếu processRun = 2 thì công đoạn này đang chạy , đổi màu xanh, tức là vừa có công nhân vừa có bobbin trong 1 tiếng đổ lại
                $("#list").jqGrid('setCell', rows[i], "name", "", {
                    'background-color': '#05d405',
                    color: '#fff',
                    'font-size': '1.025em'
                });
            }
            if (ProcessRun == 1) {//nếu processRun = 1 nghĩa là mới scan công nhân chưa scan bobbin
                $("#list").jqGrid('setCell', rows[i], "name", "", {
                    'background-color': 'red',
                    color: '#fff',
                    'font-size': '1.025em'
                });
            }
        }
    },
    onCellSelect: editRow,
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);

        $("#actual").val(row_id.actual);
        $("#c_name").val(row_id.name);

        $("#id_actual").val(row_id.id_actual);
        $("#check_qc").val(row_id.item_vcd);
        $("#m_item_vcd").val(row_id.item_vcd);

        $("#qc_item_vcd").val(row_id.item_vcd);
        $("#qc_date").val(row_id.date);
        var lastSelection = "";

        if (rowid && rowid !== lastSelection) {
            var grid = $("#list");
            grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
            lastSelection = rowid;


        }
    },

    pager: jQuery('#jqGridPager'),
    rowNum: 50,
    rowList: [50, 100, 200, 500, 1000],
    rownumbers: true,
    autowidth: false,
    subGrid: false, // set the subGrid property to true to show expand buttons for each row
    subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid
    shrinkToFit: false,
    viewrecords: true,
    height: 300,
    width: $(".boxList").width() - 5,
    sortable: true,
    loadonce: false,
    caption: 'Manufacturing',
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

});
var lastSelection;
function editRow(id) {

    if (id && id !== lastSelection) {
        var grid = $("#list");
        grid.jqGrid('editRow', id, { keys: true, focusField: 3 });
        lastSelection = id;
    }

    //var ret = "/SalesMgt/getLine2?bom_no=" + jQuery("#popups5").getRowData(id).bom_no;
    //jQuery("#popups5").setColProp("line_no", { editoptions: { dataUrl: ret } });
}
function xoa_wactual(cellValue, options, rowdata, action) {
    html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + rowdata.id_actual + '"onclick="xoa_wactual_con(this);">Delete</button>';
    return html;
}

function xoa_wactual_con(e) {
    $.ajax({
        url: '/ActualWO/xoa_wactual_con?id=' + $(e).data("id"),
        type: "Delete",
        dataType: "json",
        success: function (data) {
            if (data.result == true) {
                id = $('#id_actual').val();
                $("#list").jqGrid('delRowData', id);
                jQuery('#list').trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
}

function getData(pdata) {

    var params = new Object();

    if ($('#list').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.at_no = $('#at_no').val();

    $('#list').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/ActualWO/GetDataWActual`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};

function showChildGrid(parentRowID, parentRowKey) {
    //debugger;
    var childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    $("#" + childGridID).jqGrid({
        url: "/ActualWO/GetDetailActual?id=" + parentRowKey,
        type: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'Reg Date', name: 'reg_date', width: 150 },

            { label: 'Container', name: 'bb_no', width: 280, },
            { label: 'ML No', name: 'mt_cd', width: 350, sortable: true, },
            {
                label: 'Quantity', name: 'sl_tru_ng', width: 100, align: 'right', formatter: sl_tru_ng,
                formatter: 'integer',
                summaryTpl: "Total: {0}", // set the summary template to show the group summary
                summaryType: "sum",
                align: 'right',
            },
            {
                label: 'Quantity reality', name: 'gr_qty', width: 100, align: 'right',
                formatter: 'integer',
                summaryTpl: "Total: {0}", // set the summary template to show the group summary
                summaryType: "sum"
            },
            { label: 'Shift', name: 'ca', width: 81, align: 'right' },
            { label: 'Reg Date', name: 'reg_date', width: 100, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" } },

            { name: 'reg_dt', width: 100, hidden: true, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:m:s" }},
        ],
        shrinkToFit: false,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rowNum: 50,
        rownumbers: true,
        loadonce: false,
        multiPageSelection: true,
        rowList: [50, 100, 200, 500,1000],
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
        grouping: true,
        groupingView:
        {
            groupField: ["reg_dt", "ca"],
            groupColumnShow: [true, true],
            groupText: ["<b>{0}</b>", "<b>{0}</b>"],
            groupOrder: ["asc"],
            groupSummary: [true, true], // will use the "summaryTpl" property of the respective column
            groupSummaryPos: ['footer', 'footer'],
            groupCollapse: false,
            groupDataSorted: true,
            showSummaryOnHide: true,
            loadonce: false,
        }
    });
}

$("#tab_primary1").on("click", "a", function (event) {
    //document.getElementById("form1").reset();
    $("#tab_primary2").removeClass("active");
    $("#tab_primary1").addClass("active");
    $("#tab_c_primary2").removeClass("active");
    $("#tab_c_primary1").removeClass("hidden");
    $("#tab_c_primary2").addClass("hidden");
    $("#tab_c_primary1").addClass("active");
});

$("#tab_primary2").on("click", "a", function (event) {
    //document.getElementById("form2").reset();
    $("#tab_primary1").removeClass("active");
    $("#tab_primary2").addClass("active");
    $("#tab_c_primary1").removeClass("active");
    $("#tab_c_primary2").removeClass("hidden");
    $("#tab_c_primary1").addClass("hidden");
    $("#tab_c_primary2").addClass("active");
    ShowFinishedPOList();
});

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
    document.getElementById("c_Process").value = "";
    document.getElementById("m_Process").value = "";
});

$("#tab_2").on("click", "a", function (event) {
    $("#list").trigger("reloadGrid");
    $("#list2").clearGridData();
    $("#list3").clearGridData();
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);
    $("#ver_vf").attr("disabled", true);
    document.getElementById("c_Process").value = "";
    document.getElementById("m_Process").value = "";
});

$("#form1").validate({
    rules: {
        "name": {
            required: true,
        },
        "item_vcd": {
            required: true,
        },
        "style_no": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "name": {
            required: true,
        },
        "item_vcd": {
            required: true,
        },
        "style_no": {
            required: true,
        },
    },
});

function composite_pp(cellValue, options, rowdata, action) {
    ////var date = rowdata.date;
    ////var html = "";

    ////var today = new Date();
    ////var dd = String(today.getDate()).padStart(2, '0');
    ////var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    ////var yyyy = today.getFullYear();
    ////today = yyyy + '-' + mm + '-' + dd;
    ////if (date >= today) {
    html = '<button class="btn btn-xs btn-primary" data-name="' + rowdata.name + '"  data-id_actual="' + rowdata.id_actual + '" onclick="composite_dt(this);">Composite</button>';
    //}
    return html;
}

//document.getElementById("c_target").onblur = function () {
//    this.value = parseFloat(this.value.replace(/,/g, ""))
//        .toString()
//        .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

//document.getElementById("m_target").onblur = function () {
//    this.value = parseFloat(this.value.replace(/,/g, ""))
//        .toString()
//        .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

function check_tontai() {
    if ($("#actual").val() == 0) {
        $("#check").val("ok")
    }
    else {
        alert("Already started production");
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//POPUP_COMPOSITE
$("#close_composite").click(function () {

    var id = $("#id_actual").val();
    //kiểm tra có add công nhân với máy hiện tại chưa
    $.get("/ActualWO/CheckNewWorkerAndMachine?id_actual=" + id, function (data) {

        //debugger;

        if (data.result == true) {
            $('.dialog_composite').dialog('close');
            $('.dialog_composite2').dialog('open');
            var process = $("#process_nm").val();
            if (process.indexOf("STA") != -1) {
                $("#pp_sta").removeClass("hidden");
            }
            else {
                $("#pp_sta").addClass("hidden");
            }

            $('#tableNVLProcess').clearGridData();
            $('#tableNVLProcess').jqGrid('setGridParam', { search: true });
            var pdata = $('#tableNVLProcess').jqGrid('getGridParam', 'postData');
            getData_ProductCode(pdata);
        }
        else {
            ErrorAlert(data.message);
        }
    });
});
$("#copy").click(function () {
    $.get("/ActualWO/Copy_Process?at_no=" + at_no, function (data) {
        if (data.result == true) {
            //var rowData = $('#tb_mt_cd').jqGrid('getRowData', id);
            //rowData.use_yn = data.use_yn;
            //$('#tb_mt_cd').jqGrid('setRowData', id, rowData);
            //$("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
        }
        alert(data.message);
    });
});
$("#back_step1").click(function () {
    $('.dialog_composite2').dialog('close');
    $('.dialog_composite').dialog('open');
});

function composite_dt(e) {
    $('#cp_lot').val("");
    jQuery("#tb_mt_cd").jqGrid("clearGridData");
    $("#process_nm").val($(e).data("name"));
    $("#id_actual").val($(e).data("id_actual"));
    var id_actual = $(e).data("id_actual");
    var grid = $("#tb_mt_lot");
    var find_id_last = $("#list").find(">tbody>tr.jqgrow").filter(":last");
    if (find_id_last[0].id != $(e).data("id_actual")) {
        grid.jqGrid('hideCol', ["QC"]);
    }
    else {
        grid.jqGrid('showCol', ["QC"]);
    }
    grid.jqGrid('setGridParam', { search: true });
    var postData = grid.jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = postData.rows;
    params.sidx = postData.sidx;
    params.sord = postData.sord;
    params._search = postData._search;
    params.id_actual = (id_actual == null ?"0": id_actual);
    $.ajax({
        url: '/ActualWO/GetMtDateWeb',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#tb_mt_lot")[0];
                grid.addJSONData(data);
            }
        }
    })
    $('.dialog_composite').dialog('open');
}

$(".dialog_composite2").dialog({
    width: '80%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '80%',
    minHeight: 800,
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
    open: function () {
    },
    close: function (event, ui) {
        $('.dialog_composite').dialog('close');
    },
});
$(".dialog_composite").dialog({
    width: '30%',
    height: 200,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '30%',
    minHeight: 200,
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
    open: function () {
    },
    close: function (event, ui) {
        $('.dialog_composite').dialog('close');
    },
});
$(".dialog_composite3").dialog({
    width: '70%',
    height: 600,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '60%',
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
    open: function () {
    },
    close: function (event, ui) {
        $('.dialog_composite').dialog('close');
    },
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//dELETE
$("#dialogDangerous").dialog({
    width: '20%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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
    },
});
$('.deletebtn').click(function () {
    switch ($(this).attr("id")) {
        case "del_mold":
            $("#sts_del").val("del_mold");
            break;
        case "del_mc":
            $("#sts_del").val("del_mc");
            break;
        case "del_wk":
            $("#sts_del").val("del_wk");
            break;
        case "del_save_but":
            $("#sts_del").val("del_wactual");
            break;
    }
    $('#dialogDangerous').dialog('open');
});


function refreshMachine() {
    $.ajax({
        url: "/ActualWO/Getprocess_machineunit",
        type: "get",
        dataType: "json",
        data: {
            id_actual: $('#id_actual').val(),
        },
        success: function (response) {
            $("#popupmachine1").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response }).trigger("reloadGrid");

        },

    });
}


function refreshWorker() {
    $.ajax({
        url: "/ActualWO/Getprocess_staff",
        type: "GET",
        dataType: "json",
        data: {
            id_actual: ($('#id_actual').val() == "" ? 0 : $('#id_actual').val()),
        },
        success: function (response) {
            //debugger;
            $("#popupworker").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: response }).trigger("reloadGrid");
        },

    });
}


//DeleteMold_mc_wk_actual
$("#deletestyle").click(function () {

    switch ($("#sts_del").val()) {
        case "del_mold":
            var i, selRowIds = $('#popupmold').jqGrid("getGridParam", "selarrrow"), n, rowData;
            if (selRowIds == "") {
                alert("Please select row you want change");
                return false;
            }
            $.ajax({
                url: "/ActualWO/DeleteMoldMcWkActual?id=" + selRowIds + "&type=mold",
                type: "Delete",
                dataType: "json",
                success: function (data) {
                    if (data.result == true) {
                        SuccessAlert("Delete Success");
                        for (i = 0, n = selRowIds.length; i < n; i++) {
                            id = selRowIds[0];
                            $("#popupmold").jqGrid('delRowData', id);
                        }

                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                },
                error: function (result) {
                    ErrorAlert("System Error");
                }
            });
            $('#dialogDangerous').dialog('close');
            break;
        case "del_mc":
            var i, selRowIds = $('#popupmachine1').jqGrid("getGridParam", "selarrrow"), n, rowData;
            if (selRowIds == "") {
                alert("Please select row you want change");
                return false;
            }
            $.ajax({
                url: "/ActualWO/DeleteMoldMcWkActual?id=" + selRowIds + "&type=mc",
                type: "Delete",
                dataType: "json",
                data: {
                    bpid: $('#m_bpid').val(),
                },
                success: function (data) {
                    if (data.result == true) {
                        for (i = 0, n = selRowIds.length; i < n; i++) {
                            id = selRowIds[0];
                            $("#popupmachine1").jqGrid('delRowData', id);
                        }
                        refreshMachine();
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                },
                error: function (result) { }
            });
            $('#dialogDangerous').dialog('close');
            break;
        case "del_wk":
            var i, selRowIds = $('#popupworker').jqGrid("getGridParam", "selarrrow"), n, rowData;
            if (selRowIds == "") {
                alert("Please select row you want change");
                return false;
            }
            //
            $.ajax({
                url: "/ActualWO/DeleteMoldMcWkActual?id=" + selRowIds + "&type=wk",
                type: "Delete",
                dataType: "json",
                success: function (data) {
                    if (data.result == true) {
                        /*_get_staff();*/
                        for (i = 0, n = selRowIds.length; i < n; i++) {
                            id = selRowIds[0];
                            $("#popupworker").jqGrid('delRowData', id);
                        }
                        refreshWorker();
                        SuccessAlert(data.message);
                    }
                    else {
                        ErrorAlert(data.message);
                    }
                },
                error: function (result) { }
            });
            $('#dialogDangerous').dialog('close');
            break;
        case "del_wactual": $.ajax({
            url: "/ActualWO/Deletew_actual_primary",
            type: "Delete",
            dataType: "json",
            data: {
                id: $('#id_actualpr').val(),
            },
            success: function (data) {

                if (data.result == true) {
                    //debugger;
                    id = $('#id_actualpr').val();
                    $("#list_primary").jqGrid('delRowData', id);
                    //jQuery('#list_primary').trigger('reloadGrid');
                    SuccessAlert(data.message);
                    $("#m_style_no").val(null);
                    $("#m_target").val(0);
                    $("#m_remark").val(null);
                }
                else {
                    ErrorAlert(data.message);
                }
            },
            error: function (result) { }
        });
            $('#dialogDangerous').dialog('close');
            break;
    }
});

$('#closestyle').click(function () {
    $('#dialogDangerous').dialog('close');
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ADD MOLD MCHINE STAFF
$("#dialogAccept").dialog({
    width: '30%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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
    },
});
$('#close_create').click(function () {
    $('#dialogAccept').dialog('close');
});
function call_create(sts, id_update, start, end) {
    if (sts == "create_mold") { $("#thong_bao").html("This mold has already selected. If you confirm it, this mold will finish the task at the previous stage") }
    if (sts == "create_mc") { $("#thong_bao").html("Máy này đã được chọn. Nếu bạn xác nhận nó, máy này sẽ hoàn thành công việc ở giai đoạn trước") }
    if (sts == "create_wk") { $("#thong_bao").html("Công nhân này đã được chọn. Nếu bạn xác nhận nó, công nhân này sẽ hoàn thành công việc ở giai đoạn trước") }
    $("#sts_creat").val(sts);
    $("#id_update").val(id_update);
    $("#start_accept").val(start);
    $("#end_accept").val(end);

    $('#dialogAccept').dialog('open');
}
$("#create_accept").click(function () {

    //debugger;
    switch ($("#sts_creat").val()) {
        case "create_mold":
            $.ajax({
                url: "/ActualWO/Createprocessmold_duplicate",
                type: "Post",
                dataType: "json",
                data: {
                    mc_no: $('#c_ml_mc_no').val(),
                    id_actual: $('#id_actual').val(),
                    use_yn: $('#c2_ml_use_yn').val(),
                    id_update: $('#id_update').val(),
                    start: $('#start_accept').val(),
                    end: $('#end_accept').val(),
                    remark: $('#c2_ml_remark').val().trim(),
                },
                success: function (data) {
                    var id = data.pmid;

                    $("#popupmold").jqGrid('addRowData', id, data, 'first');
                    $("#popupmold").setRowData(id, false, { background: "#d0e9c6" });
                },
            });
            $('#dialogAccept').dialog('close');
            break;
        case "create_mc":
            $.ajax({
                url: "/ActualWO/Createprocessmachine_duplicate",
                type: "Post",
                dataType: "json",
                data: {
                    mc_no: $('#c_mc_no').val(),
                    mc_type: $('#c2_mc_type').val(),
                    use_yn: $('#c2_use_yn').val(),
                    id_actual: $('#id_actual').val(),
                    id_update: $('#id_update').val(),
                    start: $('#start_accept').val(),
                    end: $('#end_accept').val(),
                    remark: $('#c2_remark').val().trim(),
                },
                success: function (response) {

                    if(response.result == true){

                         var id = response.data.pmid;
                        $("#popupmachine1").jqGrid('addRowData', id, response.data, 'first');
                        $("#popupmachine1").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert(data.message);
                   }
                   else{

                        ErrorAlert(data.message);
                    }
                },
            });
            $('#dialogAccept').dialog('close');
            break;

        case "create_wk":
            $.ajax({
                url: "/ActualWO/Createprocessstaff_duplicate",
                type: "get",
                dataType: "json",
                data: {
                    staff_id: $('#c_staff_id').val(),
                    id_actual: $('#id_actual').val(),
                    use_yn: $('#c2_use_yn').val(),
                    id_update: $('#id_update').val(),
                    start: $('#start_accept').val(),
                    end: $('#end_accept').val(),
                },
                success: function (data) {
                    $("#popupworker").jqGrid('addRowData', data.psid, data, 'first');
                    $("#popupworker").setRowData(data.psid, false, { background: "#d0e9c6" });

                },
            });
            $('#dialogAccept').dialog('close');
            break;
        case "create_composite":
            create_composite();
            $('#dialogAccept').dialog('close');
            break;

        case "create_product":
            create_product();
            $('#dialogAccept').dialog('close');
            break;

        //
        case "Finish_Back":
            var id = $("#wmmid").val();
            $.get("/ActualWO/Finish_back?wmmid=" + id, function (data) {
                if (data.result == true) {
                    var rowData = $('#tb_mt_cd').jqGrid('getRowData', id);
                    rowData.use_yn = data.use_yn;
                    $('#tb_mt_cd').jqGrid('setRowData', id, rowData);
                    $("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
                }
                alert(data.message);
            });
            $('#dialogAccept').dialog('close');
            break;
    }
});

//view tổng hợp mold staff machine
$("#dialog_view_th").dialog({
    width: '40%',
    height: 300,
    maxWidth: '100%',
    maxHeight: 100,
    minWidth: '30%',
    minHeight: 300,
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
        //debugger;
        var table = $('#info_mc_wk_mold').DataTable({
            "processing": true,
            "ajax": {
                "url": '/ActualWO/GetInfoMcWkMold?idActual=' + ($('#id_actual').val() == "" ? 0 : $('#id_actual').val()),
                "type": "GET",
                "datatype": "json"
            },
            "searching": false,
            "paging": false,
            "bInfo": false,
            "columns": [
                {
                    'className': 'details-control',
                    'orderable': false,
                    'data': null,
                    'defaultContent': ''
                },
                { "data": "type" },
                { "data": "code" },
                { "data": "name" },
                { "data": "new_start_dt"},
                { "data": "new_end_dt" },
            ],

            'columnDefs': [
                {
                    "searchable": false,
                    "orderable": false,
                    "targets": 0, // your case first column
                    "className": "text-center",
                },
                {
                    "targets": 1, // your case first column
                    "className": "text-left",
                },
                {
                    "targets": 2, // your case first column
                    "className": "text-left",
                },
                {
                    "targets": 3, // your case first column
                    "className": "text-center",
                },
                {
                    "targets": 4, // your case first column
                    "className": "text-center",
                },
            ],
            'rowsGroup': [0],
            "bDestroy": true,
            "order": [[1, 'asc'], [3, 'dsc']],
        });
        table.on('order.dt search.dt', function () {
            table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
        $('#btn-show-all-children').on('click', function () {
            table.rows(':not(.parent)').nodes().to$().find('td:first-child').trigger('click');
        });
    },
});

$("#view_th").click(function () {
    $('#dialog_view_th').dialog('open');
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////\
var idFirstCheckToSaveMTCD = "";
var idCurrentSelected = "";


//mapping
$grid = $("#tb_mt_lot").jqGrid
    ({
        datatype: function (postData) { getData_Composite(postData);  },
        mtype: 'Get',
        colModel: [
            { key: true, label: 'wmmid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'Container', name: 'bb_no', width: 230, },

            { label: 'Quantity', name: 'gr_qty', width: 81, formatter: 'integer', align: 'right', hidden: true },

            { label: 'Quantity', name: 'sl_tru_ng', width: 81, formatter: 'integer', align: 'right', },

            {
                label: 'Quantity reality', name: 'gr_qty1', width: 81, align: 'right', formatter: 'integer'
                , editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                formatter: 'integer', editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });

                        $(element).keydown(function (e) {
                            if (e.which == 13) {
                                var value = this.value;
                                var id = e.target.id.replace("_gr_qty1", "");
                                var row = $("#tb_mt_lot").jqGrid('getRowData', id);
                                check_update_grty(row.mt_cd, value, id);
                            }
                        });
                    }
                },
            },
            { label: 'Check PQC', name: 'QC', width: 100, align: 'center', formatter: qc_code_pp },
            {
                label: 'TGian Tạo', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
            },
            { label: 'MT CNT', name: 'count_table2', width: 50, align: 'right', formatter: 'integer' },
            { name: 'het_ca',hidden:true},
            { label: 'Delete', name: 'delete', width: 100, align: 'center', formatter: xoa_qr_cha },
            { label: 'ML No', name: 'mt_cd', width: 280, sortable: true, },
            {
                key: false, label: 'description', name: 'description', width: 230, align: 'left', editable: true, editrules: { required: true }, editoptions: {
                    dataInit: function (element)
                    {
                        $(element).keydown(function (e)
                        {
                            if (e.which == 13)
                            {
                                var XacNhan = confirm("Bạn có chắc muốn thay đổi?");
                                if (XacNhan)
                                {

                                    var Noidung = this.value;

                                    if ((Noidung == "") || (Noidung == undefined))
                                    {
                                        ErrorAlert("Vui lòng điền nội dung muốn thay đổi.");
                                        return false;
                                    }

                                    var selectedRowId = $("#tb_mt_lot").jqGrid("getGridParam", 'selrow');
                                    row_id = $("#tb_mt_lot").getRowData(selectedRowId);
                                    $.ajax({


                                        url: '/ActualWO/updateDescriptionWMaterialInfo',
                                        type: "get",
                                        dataType: "json",
                                        data: {
                                            wmtid: row_id.wmtid,
                                            description: Noidung
                                        },
                                        success: function (data)
                                        {
                                            if (data.result)
                                            {
                                                SuccessAlert(data.message);
                                            }
                                            else
                                            {
                                                ErrorAlert(data.message);
                                                jQuery("#tb_mt_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                            }
                                        },
                                        error: function ()
                                        {
                                            ErrorAlert(data.message);
                                            jQuery("#tb_mt_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                        }
                                    });
                                }
                                else
                                {
                                    return false;
                                }
                            }

                        });
                    }
                }
            },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#tb_mt_lot").jqGrid("getGridParam", 'selrow');
            row_id = $("#tb_mt_lot").getRowData(selectedRowId);
            $("#tb_mt_cd").setGridParam({ url: "/ActualWO/ds_mapping_w?" + "mt_cd=" + row_id.mt_cd, datatype: "json" }).trigger("reloadGrid");
            $('#id_mt_cm').val(row_id.wmtid);
            $('#cp_lot').val(row_id.mt_cd);

            $('#id_print').val(row_id.wmtid);
            idCurrentSelected = selectedRowId;
            var ids = $("#tb_mt_lot").jqGrid('getDataIDs');
             idFirstCheckToSaveMTCD = ids[0];
            //var rowData = $(this).getRowData(idFirstCheckToSaveMTCD);

        },
        gridComplete: function () {
            var rows = $("#tb_mt_lot").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var gr_qty = $("#tb_mt_lot").getCell(rows[i], "sl_tru_ng");
                var het_ca = $("#tb_mt_lot").getCell(rows[i], "het_ca");
                if (het_ca == 0) {
                    //$("#tb_mt_lot").jqGrid('setRowData', rows[i], false, { background: '#ccc' });
                    $("#tb_mt_lot").jqGrid('setCell', rows[i], "bb_no", "", {
                        'background-color': '#ccc',
                        'color': '#000',
                        'font-size': '1.025em'
                    });
                }
              else if (gr_qty > 0) {
                    //$("#tb_mt_lot").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                    $("#tb_mt_lot").jqGrid('setCell', rows[i], "bb_no", "", {
                        'background-color': '#05d405',
                        'color': '#fff',
                        'font-size': '1.025em'
                    });
                }
            }
        },
        pager: jQuery('#tb_mt_lot_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],

        loadonce: false,
        height: 200,
        width: null,

        onCellSelect: function (rowid) {
            var lastSel = "";
            if (rowid && rowid !== lastSel) {
                jQuery('#tb_mt_lot').restoreRow(lastSel);
                lastSel = rowid;
            }
            var het_ca = $("#tb_mt_lot").getCell(rowid, "het_ca");

            if (het_ca == 0) {
                jQuery('#tb_mt_lot').editRow(rowid, false);
            } else {
                jQuery('#tb_mt_lot').editRow(rowid, true);
            }
        },
        rownumbers: true,
        shrinkToFit: false,
        viewrecords: true,
        multiboxonly: true,
        caption: 'Composite',
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
    });

function sl_tru_ng(cellValue, options, rowdata, action) {
    var html = parseInt(cellValue == null ? 0 : cellValue) + parseInt(rowdata.gr_qty);
    return html;
};
function check_update_grty(mt_cd, value, id) {

    $.get("/ActualWO/check_update_grty?mt_cd=" + mt_cd + "&value=" + value + "&wmtid=" + id, function (data) {
        //debugger;
        if (data.result == true) {
            var id = data.kq[0].wmtid;
            $("#tb_mt_lot").setRowData(id, data.kq[0], { background: "#d0e9c6" });
            SuccessAlert(data.message);
        } else {
            var rowData = $('#tb_mt_lot').jqGrid('getRowData', data.wmtid);
            rowData.gr_qty1 = rowData.gr_qty;
            rowData.sl_tru_ng = parseInt(rowData.sl_tru_ng) - parseInt(rowData.gr_qty);
            $('#tb_mt_lot').jqGrid('setRowData', data.wmtid, rowData);
            ErrorAlert(data.message);
            call_table1();
        }
    });
}

$("#save_bb").click(function () {
    //debugger;
    if ($("#cp_bb_no2").val() == "") {
        alert("Vui lòng chọn mã container !!!");
        return false;
    }
    if ($("#cp_lot").val().trim() == "") {
        alert("Vui lòng chọn mã đầu ra để mapping với đầu vào!!!");
        return false;
    } else {
        $.ajax({
            url: "/ActualWO/insertw_material_mping",
            type: "Get",
            dataType: "json",
            data: {
                mt_cd: $("#cp_lot").val(),
                mt_mapping: $("#s_mt_cd").val(),
                id_actual: $("#id_actual").val(),
                bb_no: $("#cp_bb_no2").val(),
            },
            success: function (response) {
                $('.diologbobin').dialog('close');
                if (response.result == true) {
                    //$("#mt_lot1").val(response.data[0].mt_cd);
                    var id = response.data[0].wmmid;
                    $("#tb_mt_cd").jqGrid('addRowData', id, response.data[0], 'first');
                    $("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(response.message);
                } else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});
$("#save_composite").click(function () {
    //debugger;
    if ($("#s_mt_cd").val() == "") {
        alert("Please enter your Material");
        $("#s_mt_cd").val("");
        $("#s_mt_cd").focus();
        return false;
    }
    var abc = $("#cp_lot").val().trim();
    if ($("#cp_lot").val().trim() == "") {
        alert("Please enter your Composite Lot");
        return false;
    } else {
        //var bb_no = $('#cp_bb_no2').val();
        $.ajax({
            url: "/ActualWO/insertw_material_mping",
            type: "Get",
            dataType: "json",
            data: {
                mt_cd: $("#cp_lot").val(),
                mt_mapping: $("#s_mt_cd").val().trim(),
                id_actual: $("#id_actual").val(),
                bb_no: $("#cp_bb_no2").val(),
            },
            success: function (response) {
                if (response.result == true) {
                    //$("#mt_lot1").val(response.data.mt_cd);
                    var id = response.data[0].wmmid;
                    $("#tb_mt_cd").jqGrid('addRowData', id, response.data[0], 'first');
                    $("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(response.message);
                } else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});
$("#container_bb").click(function () {
    $('.diologbobin').dialog('open');
    $("#s_mt_cd").val("");
    get_ds_bb();
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Check Qc Code
function qc_code_pp(cellValue, options, rowdata, action) {
    if (rowdata.gr_qty == 0) { }
    var item_vcd = $("#check_qc").val();
    var html = '<button style="color: dodgerblue;border: none;background: none; " data-id="' + rowdata.wmtid + '"  onclick="item_qc_check(this);">' + item_vcd + '</button>';
    return html;
};
$(".dialog_qc_code_pp").dialog({
    width: '60%',
    height: 400,
    maxWidth: 800,
    maxHeight: 450,
    minWidth: '60%',
    minHeight: 100,
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
    open: function () {
        //getViewItemQC();
    },

    close: function (event, ui) {
        $('.dialog_qc_code_pp').dialog('close');
        $('.dialog_qc_code_pp').dialog('close');
        document.getElementById("qc_ok_qty").value = "0";
        document.getElementById("qc_gr_qty").value = "0";
        document.getElementById("qc_ng_qty").value = "0";
        $('input:checkbox').removeAttr('checked');
        a = 0;
        $("#list_qc").trigger("reloadGrid");
        $("#list_qc_value").clearGridData();

        jQuery("#list_facline_qc").trigger("reloadGrid");
        //$("#list_facline_qc_value").clearGridData();

        $("#p_SaveQcfacline").attr("disabled", true);
    },
});
//function getViewItemQC() {
//    $.get("/ActualWO/PartialView_View_QC_WEB?item_vcd=" + $("#check_qc").val(), function (html) {
//        $("#view_qc").html(html);
//    });
//}
function item_qc_check(e) {
    $('.dialog_qc_code_pp').dialog('open');
    var id = $(e).data("id")
    var rowData = $('#tb_mt_lot').jqGrid('getRowData', id);
    $("#qc_gr_qty").val(rowData.gr_qty);
    $("#qc_mt_cd").val(rowData.mt_cd);

    $("input#qc_ok_qty").attr({
        "max": rowData.gr_qty,        // substitute your own
    });
    var item_vcd = $("#check_qc").val();
    table_facline_qc(item_vcd, rowData.mt_cd);
    //table_facline_qc_value();
}
$('input#qc_ok_qty').keyup(function () {
    var value = this.value;
    if (value.trim() != "") {
        var value_check = $("#qc_gr_qty").val();
        if (parseInt(value) > parseInt(value_check)) {
            $("#qc_ok_qty").val(value_check)
        }
        var check_qty = (document.getElementById('qc_gr_qty').value == "") ? 0 : document.getElementById('qc_gr_qty').value;
        var ok_qty = (document.getElementById('qc_ok_qty').value == "") ? 0 : document.getElementById('qc_ok_qty').value;
        if (document.getElementById('qc_ok_qty').value == "") {
            $("#qc_ng_qty").val("0");
        }
        var max = parseInt(check_qty) - parseInt(ok_qty);
        $("#qc_ng_qty").val(max);
    } else {
        $("#qc_ng_qty").val("0");
    }
});
//end table w_product_qc
//start table m_facline_qc
function table_facline_qc(item_vcd, mt_cd) {
    $("#list_facline_qc").setGridParam({ url: "/ActualWO/Getfacline_qc?" + "mt_cd=" + mt_cd + "&item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
    $("#list_facline_qc").jqGrid
        ({
            url: "/ActualWO/Getfacline_qc?mt_cd=" + mt_cd + "&item_vcd=" + item_vcd,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqno', name: 'fqno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'FQ NO', name: 'fq_no', sortable: true, width: '120', align: 'center' },
                { key: false, label: 'Work Date', name: 'work_dt', sortable: true, width: '150', align: 'center', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
                /*{ label: 'Date', name: 'date', align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },*/
                { key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
                { key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
                { key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
            ],
            formatter: {
                integer: { thousandsSeparator: ",", defaultValue: '0' },
                currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
            },
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
                row_id = $("#list_facline_qc").getRowData(selectedRowId);
                //$("#list_facline_qc_value").clearGridData();
                //$("#list_facline_qc_value").setGridParam({ url: "/ActualWO/Getfacline_qc_value?" + "fq_no=" + row_id.fq_no, datatype: "json" }).trigger("reloadGrid");
            },

            viewrecords: true,
            rowList: [50, 100, 200, 500],
            height: "100%",
            width: null,
            rowNum: 50,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: false,
            gridview: true,
            shrinkToFit: false,
            loadonce: false,
            viewrecords: true,
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
        })
}
//function table_facline_qc_value() {
//    $("#list_facline_qc_value").jqGrid
//        ({
//            url: "/ActualWO/Getfacline_qc_value",
//            datatype: 'json',
//            mtype: 'Get',
//            colModel: [
//                { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
//                { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
//                { key: false, label: 'Check Value', name: 'check_value', sortable: true, width: '110', align: 'left' },
//                {
//                    key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', editable: true,
//                    editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
//                    formatter: 'integer', editoptions: {
//                        dataInit: function (element) {
//                            $(element).keypress(function (e) {
//                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
//                                    return false;
//                                }
//                            });
//                        }
//                    },
//                },
//            ],

//            onCellSelect: editRow_facline,
//            onSelectRow: function (rowid, selected, status, e) {
//                $("#p_SaveQcfacline").attr("disabled", false);
//            },

//            viewrecords: true,
//            rowList: [50, 100, 200, 500],
//            height: "100%",
//            width: null,
//            rowNum: 50,
//            loadtext: "Loading...",
//            emptyrecords: "No data.",
//            rownumbers: false,
//            gridview: true,
//            shrinkToFit: false,
//            loadonce: false,
//            viewrecords: true,
//            multiselect: true,
//            jsonReader:
//            {
//                root: "rows",
//                page: "page",
//                total: "total",
//                records: "records",
//                repeatitems: false,
//                Id: "0"
//            },
//        })
//}
//var lastSel;
//function editRow_facline(id) {
//    if (id && id !== lastSel) {
//        var grid = $("#list_facline_qc_value");
//        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
//        lastSel = id;
//    }
//}
//$("#p_SaveQcfacline").on("click", function () {
//    //var i, selRowIds = $('#list_facline_qc_value').jqGrid("getGridParam", "selarrrow"), n, rowData, check_qty = "";

//    var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
//    row_id = $("#list_facline_qc").getRowData(selectedRowId);

//    var fqno = row_id.fqno;
//    var fruits = [];
//    for (i = 0, n = selRowIds.length; i < n; i++) {
//        id = selRowIds[i];
//        check_qty = getCellValue(selRowIds[i], 'check_qty');
//        if (check_qty == undefined) {
//            var ret = $("#list_facline_qc_value").jqGrid('getRowData', selRowIds[i]);
//            var giatri = ret.check_qty;
//            fruits.push(giatri);
//        } else {
//            fruits.push(check_qty);
//        }
//    }

//    $.ajax({
//        type: "get",
//        dataType: 'json',
//        url: '/ActualWO/ModifyFaclineQcDetail?' + "id=" + selRowIds + "&" + "check_qty=" + fruits + "&" + "fqno=" + fqno,

//        success: function (data) {
//            if (data.result) {
//                console.log(data);
//                $.each(data.list, function (key, item) {
//                    var id = item.fqhno;
//                    $("#list_facline_qc_value").setRowData(id, item, { background: "#d0e9c6" });
//                    var id = item.fqno;
//                    $("#list_facline_qc").setRowData(id, item, { background: "#d0e9c6" });
//                });
//            }
//            else {
//                alert(data.message);
//            }
//        },
//        error: function (data) {
//            alert("QR Barcode no exist!!!");
//        },
//    });
//});
function getCellValue(rowId, cellId) {
    var cell = jQuery('#' + rowId + '_' + cellId);
    var val = cell.val();
    return val;
}
$("#save_qc_code").on("click", function () {
    //debugger;
    //số 1
    var okqty = $('#qc_ok_qty').val();
    var check_qty = $('#qc_gr_qty').val();
    if (check_qty == 0) {
        $('#qc_gr_qty').focus();
        return
    }
    $.ajax({
        url: "/ActualWO/Insert_w_product_qc",
        type: "Get",
        dataType: "json",
        data: {
            'name': $('#qc_name').val(),
            'style_no': $('#qc_style_no').val(),
            'item_vcd': $('#qc_item_vcd').val(),
            'check_qty': $('#qc_gr_qty').val(),
            'ok_qty': $('#qc_ok_qty').val(),
            'mt_cd': $('#qc_mt_cd').val(),
            'date': $('#qc_date').val(),
            'id_actual': $('#id_actual').val(),
            'ng_qty': $('#qc_ng_qty').val(),
        }
    })
        .done(function (data) {
            if (data.result == true) {
                call_table1();

                document.getElementById("qc_gr_qty").value = "0";
                document.getElementById("qc_ok_qty").value = "0";
                document.getElementById("qc_ng_qty").value = "0";

                jQuery("#tb_mt_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                jQuery("#list_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                $("#list_qc_value").clearGridData();
                jQuery("#list_facline_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                //$("#list_facline_qc_value").clearGridData();
                $(".testnumber").val("0");
                var max_check_qty = document.getElementById("qc_gr_qty").max;
                SuccessAlert(data.message);
            }
            else {
               ErrorAlert(data.message);
            }

            //return;
        })
        .fail(function () {
            //return;
        });

    //$.ajax({
    //    type: 'POST',
    //    url: "/ActualWO/Insert_w_product_qc",
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    data: models,
    //    cache: false,
    //    processData: false,
    //    success: function (data) {
    //        if (data.result) {
    //            call_table1();
    //            alert("Success");
    //            document.getElementById("qc_gr_qty").value = "0";
    //            document.getElementById("qc_ok_qty").value = "0";
    //            document.getElementById("qc_ng_qty").value = "0";

    //            jQuery("#tb_mt_lot").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    //            jQuery("#list_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    //            $("#list_qc_value").clearGridData();
    //            jQuery("#list_facline_qc").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    //            $("#list_facline_qc_value").clearGridData();
    //            $(".testnumber").val("0");
    //            var max_check_qty = document.getElementById("qc_gr_qty").max;
    //        }
    //        else {
    //            alert(data.message);
    //        }
    //    }
    //});
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function getData_Composite(pdata) {
    var params = new Object();

    if ($("#tb_mt_lot").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    var id_actual = ($("#id_actual").val() == null || $("#id_actual").val() == "" ? "0": $("#id_actual").val());
    params.id_actual = id_actual;
    $("#tb_mt_lot").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/ActualWO/GetMtDateWeb',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#tb_mt_lot")[0];
                grid.addJSONData(data);
            }
        }
    })
};

function call_table1() {
    var postData = $("#tb_mt_lot").jqGrid('getGridParam', 'postData');
    var params = new Object();
    params.page = 1;
    params.rows = postData.rows;
    params.sidx = postData.sidx;
    params.sord = postData.sord;
    params._search = postData._search;
    var id_actual = ($("#id_actual").val() == null || $("#id_actual").val() == "" ?"0" : $("#id_actual").val() );

    params.id_actual = id_actual;
    $.ajax({
        url: '/ActualWO/GetMtDateWeb',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            //debugger;
            if (st == "success") {
                var grid = $("#tb_mt_lot")[0];
                grid.addJSONData(data);
            }
        }
    })
}
$("#tb_mt_cd").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmmid', hidden: true },
            { label: '', name: 'Finish', width: 40, formatter: finish_vl, align: 'center' },
            { label: '', name: 'Return', width: 40, formatter: Return_vl, align: 'center' },
            { label: '', name: 'Cancel', width: 40, formatter: cancel, align: 'center' },
            {
                label: 'Tgian Mapping', name: 'mapping_dt', width: 150, align: 'center', formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
            },
            { label: 'mt_lot', name: 'mt_lot', hidden: true },
            { label: 'ML No', name: 'mt_cd', width: 280, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 200, },
            { name: 'gr_qty', hidden: true, formatter: 'integer' },
            { label: 'Total', name: 'total', formatter: total_cp, align: 'right', width: 60 },
            { label: 'Using', name: 'use_yn', width: 50, align: 'center' },
            { label: 'Used', name: 'Used', width: 50, align: 'center' },
            { label: 'Remain', name: 'Remain', width: 50, align: 'center' },
            { label: 'Container', name: 'bb_no', width: 150, align: 'center' },
            { label: 'description', name: 'description', width: 150, align: 'center' },
        ],
        pager: jQuery('#tb_mt_cd_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: false,
        height: 200,
        multiselect: false,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Material',
        gridComplete: function () {
            var rows = $("#tb_mt_cd").getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var use_yn = $("#tb_mt_cd").getCell(rows[i], "use_yn");
                if (use_yn == "N") {
                    $("#tb_mt_cd").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                }
            }
        },
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
function total_cp(cellValue, options, rowdata, action) {
    var gr_qty = (rowdata.gr_qty != "" && rowdata.gr_qty != undefined && rowdata.gr_qty != null ? rowdata.gr_qty : 0);
    var Remain = (rowdata.Remain != "" && rowdata.Remain != undefined && rowdata.Remain != null ? rowdata.Remain : 0);
    var tong = parseInt(gr_qty) + parseInt(Remain);
    return tong;
}
function xoa_qr_cha(cellValue, options, rowdata, action) {
    if (rowdata.sl_tru_ng == 0 || rowdata.sl_tru_ng == undefined) {
        var id = rowdata.wmtid;
        html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + id + '"onclick="Xoa_qr_con(this);">Delete</button>';
        return html;
    }
    return "";
}

function Xoa_qr_con(e) {
    $.ajax({
        url: '/ActualWO/Xoa_mt_pp_composite?id=' + $(e).data("id"),
        type: "Delete",
        dataType: "json",
        success: function (data) {
            if (data.result != false) {
                var id = $(e).data("id");
                $("#tb_mt_lot").jqGrid('delRowData', id);
                $("#tb_mt_cd").clearGridData();
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
}


function Return_vl(cellValue, options, rowdata, action) {

    var id = rowdata.wmmid;
    var mt_cd = rowdata.mt_cd;
    var mt_lot = rowdata.mt_lot;
    var html = '<button class="btn btn-sm btn-success button-srh" id="hideReturn"  data-id="' + id + '"  data-mt_cd="' + mt_cd + '" data-mt_lot="' + mt_lot + '" onclick="Return_vlcon(this);">R</button>';
    return html;
}
function Return_vlcon(e) {
    //debugger;
    $('#dialogReturn').dialog('open');
    $("#mt_cd_rt").val($(e).data("mt_cd"));
    $("#mt_lot_rt").val($(e).data("mt_lot"));
}
$("#savereturn").click(function () {
    //debugger;
    var soluong = $("#return_length").val();
    if (soluong == 0) {
        alert("Length is not valid");
    } else {
        var mt_cd = $("#mt_cd_rt").val();
        var mt_lot = $("#mt_lot_rt").val();
        $.get("/ActualWO/savereturn_lot?soluong=" + soluong + "&mt_cd=" + encodeURIComponent(mt_cd)  + "&mt_lot=" + mt_lot, function (data) {
            $('#dialogReturn').dialog('close');
            $("#return_length").val("0");
            if (data.result == true) {
                var id = data.data.wmmid;
                //var rowData = $('#tb_mt_cd').jqGrid('getRowData', id);
                //rowData.use_yn = "Y";
                $("#tb_mt_cd").setRowData(id, data.data, { background: "#d0e9c6" });
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }
        });
    }
});


function finish_vl(cellValue, options, rowdata, action) {

    var id = rowdata.wmmid;
    var use_yn = "Y";
    var html = '<button  class="btn btn-sm btn-secondary button-srh" data-id="' + id + '" data-use_yn="' + use_yn + '"onclick="Finish_vlcon(this);">F</button>';
    return html;
}
function Finish_vlcon(e) {
    //debugger;
    var id = $(e).data("id");
    var use_yn = $(e).data("use_yn");
    if (use_yn == "N") {
        $("#thong_bao").html("Is A sure you want to return to the used state");
        $("#sts_creat").val("Finish_Back");
        $('#dialogAccept').dialog('open');
        $("#wmmid").val(id)
    }
    else {
        $.get("/ActualWO/Finish_back?wmmid=" + id, function (data) {
            if (data.result == true) {
                var rowData = $('#tb_mt_cd').jqGrid('getRowData', id);
                rowData.use_yn = rowData.use_yn;
                $('#tb_mt_cd').jqGrid('setRowData', id, rowData);
                $("#tb_mt_cd").setRowData(id, false, { background: "#d0e9c6" });
            }
            alert(data.message);
        });
    }
}

$("#dialogReturn").dialog({
    width: '30%',
    height: 200,
    maxWidth: '20%',
    maxHeight: 500,
    minWidth: '20%',
    minHeight: 100,
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
    },
});
function Cancel_vl(e) {
    $.get("/ActualWO/Cancel_mapping?wmmid=" + $(e).data("id"), function (data) {
        if (data.result == true) {
            var wmmid = parseInt($(e).data("id"));
            $("#tb_mt_cd").jqGrid('delRowData', wmmid);
            alert(data.message);
        }
        else {
            alert(data.message);
        }
    });
}
function cancel(cellValue, options, rowdata, action) {
    var html = "";
    if (rowdata.use_yn == "Y") {
        var id = rowdata.wmmid;
        html = '<i class="fa fa-times-circle" aria-hidden="true" data-id="' + id + '"onclick = "Cancel_vl(this);"></i>';
    }
    return html;
}

//GET PROCESS
GetTIMSProcesses();
function GetTIMSProcesses() {
    $.ajax({
        url: `/ActualWO/GetActual_Processes`
    })
        .done(function (response) {
            var html = `<option value="">*Select Process*</option>`;
            $.each(response, function (key, item) {
                    html += `<option value="${item.ProcessCode}">${item.ProcessName}</option>`;
            });
            $("#c_name").html(html);
            return;
        })
        .fail(function () {
            return;
        });
}

$("#create_composite").click(function () {
    var params = new Object();
    params.style_no = $('#mp_style_no').val().trim();
    params.id_actual = $('#id_actual').val().trim();
    params.name = $('#c_name').val().trim();
    params.bb_no = $('#cp_bb_no2').val().trim();

    if ($("#cp_bb_no2").val() == "") {
        alert("Please enter your container");
        return false;
    }
    else {
        $.ajax({
            url: "/ActualWO/AddTrayAndBobbin",
            type: "POST",
            dataType: "json",
            data:params,
            success: function (data) {
                $('.diologbobin').dialog('close');
                if (data.result == false) {
                    alert(data.message);
                } else {
                    var id = data.ds1.wmtid;
                    $("#tb_mt_lot").jqGrid('addRowData', id, data.ds1, 'first');
                    $("#tb_mt_lot").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert(data.message);
                }
            },
        });
    }
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//devide
$("#pp_sta").click(function () {
    //debugger;
    $('.dialog_composite2').dialog('close');
    $('.dialog_composite3').dialog('open');

    $.get("/ActualWO/getmt_date_web_auto?id_actual=" + $("#id_actual").val(), function (data) {
        $("#tb_mt_lot_sta").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
    });
});
$("#back_sta").click(function () {
    $('.dialog_composite3').dialog('close');
    $('.dialog_composite2').dialog('open');
});
$("#container").click(function () {
    $('.diologbobin2').dialog('open');
    get_ds_bb_2();
});
function bntCellValue(cellValue, options, rowdata, action) {
    //debugger;
    if (rowdata.gr_qty == 0) {
        var mt_cd = rowdata.mt_cd;
        var html = `<button class="btn btn-xs btn-primary" data-mt_cd="${mt_cd}" onclick="destroy(this);">Redo</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
        return html;
    }
    return "";
};

function destroy(e) {
    //debugger;
    $.ajax({
        url: '/ActualWO/destroyDevide?mt_cd=' + $(e).data("mt_cd"),
        type: "Get",
        dataType: "json",
        success: function (data) {
            if (data.result == true) {
                var id = data.wmtid;
                var rowData = $('#tb_mt_lot_sta').jqGrid('getRowData', id);
                rowData.gr_qty = data.gr_qty;
                $("#tb_mt_lot_sta").setRowData(id, rowData, { background: "#d0e9c6" });

                SuccessAlert(data.message);
            } else {
                //alert(data.kq);
                ErrorAlert(data.message);
            }
        },
    });
}

$grid = $("#tb_mt_lot_sta").jqGrid
    ({
        datatype: function (postData) { getData(postData); },
        mtype: 'Get',
        colModel: [
            //bao add
            { name: "RD", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue },
            { label: 'Container', name: 'bb_no', width: 240, },

            { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'ML No', name: 'mt_cd', width: 400, sortable: true, },
            {
                label: 'Quantity reality', name: 'gr_qty', width: 81, formatter: 'integer', align: 'right'
            },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
        },
        onSelectRow: function (id, rowid, status, e) {
            //debugger;
            var selectedRowId = $("#tb_mt_lot_sta").jqGrid("getGridParam", 'selrow');
            row_id = $("#tb_mt_lot_sta").getRowData(selectedRowId);
            $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaMMS?" + "mt_cd=" + row_id.mt_cd, datatype: "json" }).trigger("reloadGrid");
            $('#mt_code_bb2').val(row_id.mt_cd);
            $('#so_lgr').val(row_id.gr_qty != null || row_id.gr_qty != "undefined" ? row_id.gr_qty : 0);
            $("#id_dv").val(row_id.wmtid);
        },
        pager: jQuery('#tb_mt_lot_sta_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],

        loadonce: false,
        height: 200,
        width: null,

        onCellSelect: function (rowid) {
            var lastSel = "";
            if (rowid && rowid !== lastSel) {
                jQuery('#tb_mt_lot_sta').restoreRow(lastSel);
                lastSel = rowid;
            }
            jQuery('#tb_mt_lot_sta').editRow(rowid, true);
        },
        rownumbers: true,
        shrinkToFit: false,
        viewrecords: true,
        multiboxonly: true,
        caption: 'Composite',
        jsonReader:
        {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        multiselect: true,
    });

$("#tb_mt_cd_sta").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', hidden: true },
            { label: 'Material Code Parent', name: 'material_code_parent', hidden: true },
            { label: 'Status', name: 'status', hidden: true },
            { label: 'Reg_Date', name: 'reg_date', hidden: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Reg_Id', name: 'reg_id', hidden: true },
            { label: 'Chg_Date', name: 'chg_date', hidden: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Chg_Id', name: 'chg_id', hidden: true },
            { label: 'Actual Code', name: 'id_actual', hidden: true },

            { label: 'ML No', name: 'material_code', width: 400, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 200 },
            { label: 'Quantity Real', name: 'sl_tru_ng', formatter: 'integer', editable: true, edittype: "number", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, hidden: true },
            { label: 'Quantity', name: 'gr_qty', formatter: 'integer', },
            { label: 'Container', name: 'bb_no', width: 150, align: 'center', formatter: change_bobin },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            //debugger;
            var selectedRowId = $("#tb_mt_cd_sta").jqGrid("getGridParam", 'selrow');
            row_id = $("#tb_mt_cd_sta").getRowData(selectedRowId);
            row_id.bb_no = "";
            var myData = encodeURIComponent(JSON.stringify(row_id));
            $("#id_dv").attr("data-hero", myData);
        },
        onCellSelect: function (rowid) {
            var lastSel = "";
            if (rowid && rowid !== lastSel) {
                jQuery('#tb_mt_cd_sta').restoreRow(lastSel);
                lastSel = rowid;
            }
            jQuery('#tb_mt_cd_sta').editRow(rowid, true);
        },
        pager: jQuery('#tb_mt_cd_sta_page'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: true,
        height: 200,
        multiselect: false,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Material',
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
function change_bobin(cellValue, options, rowdata, action) {
    //debugger;
    var id = rowdata.wmtid;
    var html = "";
    var name = "Add Container";
    if (cellValue != null && cellValue != "") { name = cellValue }
    html = '<button style="color: dodgerblue;border: none;background: none; " onclick="change_container(' + id + ');">' + name + '</button>';
    return html;
}
function change_container(id) {
    //debugger;
    $('.diologbobindv').dialog('open');
    get_ds_bb_dv();
    $("#id_dv").val(id);
}
$("#change_gr").click(function () {
    var myList = $("#tb_mt_cd_sta").jqGrid('getDataIDs');
    var id = [];
    var gr_qty = [];
    var value_all = [];
    for (var i = 0; i < myList.length; i++) {
        var rowData = $('#tb_mt_cd_sta').jqGrid('getRowData', myList[i]);
        var get_input = $("#" + myList[i] + "_sl_tru_ng").val();
        var value = "";
        if (get_input == undefined) {
            value = rowData.sl_tru_ng;
        } else {
            value = get_input;
        }
        value_all.push(value);
        gr_qty.push(rowData.gr_qty);
        id.push(myList[i]);
    }
    //đưa vào controller xử lí
    $.ajax({
        url: "/ActualWO/change_gr_dv?value_new=" + value_all + "&value_old=" + gr_qty + "&wmtid=" + id,
        type: "post",
        success: function (data) {
            if (data.result == false) {
                ErrorAlert(data.message);
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaMMS?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
            } else {
                SuccessAlert("Success");
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaMMS?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
                SuccessAlert(data.message);
            }
        },
    });
});

$("#device").click(function () {

    if (parseInt($('#so_lgr').val()) < 1) {
        alert("Quantity must > 0 ");
        return false;
    }
    if ($('#mt_code_bb2').val() == "") {
        alert("Please choose Mt Code need devide");
        return false;
    }
    if ($('#number_dv').val() == "") {
        alert("please enter the Quantity devide");
        $("#number_dv").val("");
        $('#number_dv').focus();
        return false;
    }
    if ($('#number_dv').val() != "" && parseInt($('#number_dv').val(), 10) < 50) {
        alert("Vui lòng không nhập nhỏ hơn 50");
        $("#number_dv").val("");
        $('#number_dv').focus();
        return false;
    }
    var sobichia = $('#so_lgr').val() != null ? parseInt($('#so_lgr').val()) : 0;
    var sochia = $('#number_dv').val() != null ? parseInt($('#number_dv').val()) : 0;
    if ((sobichia / sochia) < 1) {
        alert("Vui lòng kiểm tra lại sản lượng có trong đồ đựng, nó phải lớn hơn hoặc bằng 50 ! ");
        return;
    }
    if ((sobichia / sochia) > 100) {
        alert("Số lượng hộp nhỏ được chia ra không được quá 100 !");
        return false;
    }
    else {
        $.get("/ActualWO/Decevice_sta?mt_cd=" + $('#mt_code_bb2').val() + "&number_dv=" + $("#number_dv").val(), function (data) {
            $('.diologbobin2').dialog('close');

            if (data.result == true) {
                var id_dv = $("#id_dv").val();

                var rowData = $('#tb_mt_lot_sta').jqGrid('getRowData', id_dv);
                rowData.gr_qty = 0;
                $('#tb_mt_lot_sta').jqGrid('setRowData', id_dv, rowData);
                $("#tb_mt_lot_sta").setRowData(id_dv, false, { background: "#d0e9c6" });
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaMMS?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
            } else {
                alert(data.message);
            }
        });
    }
});

$("#dialog_Finsh").dialog({
    width: '20%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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
    },
});
$("#finish_po").click(function () {
    $.ajax({
        url: `/ActualWO/FinishPO?po=${poToFinish}`,
        type: "post",
        dataType: "json",
        success: function (response) {
            if (response.result == true) {
                $('#list').clearGridData();
                $("#list_primary").jqGrid('delRowData', response.id);
                SuccessAlert(response.message);
                document.getElementById("form1").reset();
                document.getElementById("form2").reset();
                $('#list_finish').jqGrid('setGridParam', { search: true });
                var pdata1 = $('#list_finish').jqGrid('getGridParam', 'postData');
                getData_Finished(pdata1);
                return;
            }
            else {
                ErrorAlert(response.message);
                return;
            }
        },
        error: function (response) {
            ErrorAlert(`System error.`);
            return;
        }
    });
    $('#dialog_Finsh').dialog('close');
});

$('#closefinish').click(function () {
    $('#dialog_Finsh').dialog('close');
});
function redo_con(e) {
    $.get("/ActualWO/Redo_Finish?id_actualpr=" + $(e).data("id_actualpr"), function (data) {
        if (data.result == true) {
            var id_actualpr = parseInt($(e).data("id_actualpr"));
            $("#list_finish").jqGrid('delRowData', id_actualpr);
            SuccessAlert(data.message);
            $('#list_primary').jqGrid('setGridParam', { search: true });
            var pdata = $('#list_primary').jqGrid('getGridParam', 'postData');
            getData_primary(pdata);
        }
        else {
            ErrorAlert(data.message);
        }
    });
}

$("#tableNVLProcess").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'id', name: 'id', width: 100, align: 'center', hidden: true },
            { label: 'style_no', name: 'style_no', width: 150, align: 'left', hidden: true },
            { label: 'level', name: 'level', width: 150, align: 'left', hidden: true },
            { label: 'process_code', name: 'process_code', width: 150, align: 'left', hidden: true },
            { label: 'name', name: 'name', width: 150, align: 'left', hidden: true },
            { label: 'Material No', name: 'mt_no', width: 150, align: 'left' },
            { label: 'MT Name', name: 'mt_nm', sortable: true, width: 150 },
        ],
        pager: jQuery('#tableNVLProcesspage'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: false,
        height: 200,
        multiselect: false,
        rownumbers: true,
        width: null,
        shrinkToFit: false,
        viewrecords: true,
        caption: 'Material Process',
        subGrid: true,
        subGridRowExpanded: showChildGridMaterialdetail,
        gridComplete: function () {

        },
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



function getData_ProductCode(pdata) {
    var params = new Object();

    if ($('#tableNVLProcess').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    //params.level = $("#level").val().trim();
    params.product = $("#product").val().trim();
    params.name = $("#c_name").val().trim();
    params.process_code = $("#m_Process").val();
    $('#tableNVLProcess').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/DevManagement/Get_ProductMaterial`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#tableNVLProcess')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};



var parentRowKey2 = 0;
var childGridID2 = "";
function showChildGridMaterialdetail(parentRowID, parentRowKey) {
    parentRowKey2 = parentRowKey;
    childGridID2 = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID2 + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $("#tableNVLProcess").jqGrid('getRowData', parentRowKey);


    $("#" + childGridID2).jqGrid({
        url: "/DevManagement/getMaterialChild?ProductCode=" + rowData.style_no + "&name=" + rowData.name  + "&MaterialPrarent=" + encodeURIComponent(rowData.mt_no) + "&process_code=" + rowData.process_code,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'id', name: 'id', width: 100, align: 'center', hidden: true },
            { label: 'MT Code', name: 'MaterialNo', width: 140 },
            { label: 'MT Name', name: 'MaterialName', sortable: true, width: 250 },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#tableNVLProcess").jqGrid("getGridParam", 'selrow');
            row_id = $("#tableNVLProcess").getRowData(selectedRowId);


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

        }
    });
}


//view danh sách liệu thay thế và hiệu suất của nó ở bảng 1
var parentRowKey3 = 0;
var childGridID3 = "";
function showChildGridMaterialReplace(parentRowID, parentRowKey) {
    //debugger;
    parentRowKey3 = parentRowKey;
    childGridID3 = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID3 + '></table><div id=' + childGridPagerID + ' class=scroll></div>');
    var rowData = $("#" + childGridID).jqGrid('getRowData', parentRowKey);


    $("#" + childGridID3).jqGrid({
        url: "/ActualWO/LayDanhsachLieuThaythe?at_no=" + encodeURIComponent(rowData.at_no) + "&mt_no=" + encodeURIComponent(rowData.MaterialNo) + "&ProductCode=" + encodeURIComponent(rowData.ProductCode) + "&process_code=" + rowData.process_code,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'id', name: 'id', width: 100, align: 'center', hidden: true },
            { label: 'MT Code', name: 'MaterialNo', width: 140 },
            { label: 'MT Name', name: 'MaterialName', sortable: true, width: 250 },
            { label: 'Số mét đã sử dụng', name: 'mLieu', sortable: true, width: 100 },
            { label: 'NVL đã scan', name: 'SoCuonNVL', sortable: true, width: '100', align: 'right' },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#tableNVLProcess").jqGrid("getGridParam", 'selrow');
            row_id = $("#tableNVLProcess").getRowData(selectedRowId);


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

        }
    });
}



//view ViewDetailDatetime
function ViewDetailDatetime(cellValue, options, rowdata, action) {
    var html = "";
    html = '<button class="btn btn-xs btn-primary"  data-it_actual="' + rowdata.id_actual + '"  onclick="ClickViewDetailDatetime(this);">Detail</button>';
    return html;
}


function ClickViewDetailDatetime(e) {
    var id_actual = $(e).data("it_actual")
    $('.popup-dialog.dialog_Viewdetaildatetime').dialog('open');


    $.get("/ActualWO/PartialView_dialog_Viewdetaildatetime?" + "id_actual=" + id_actual + ""
        , function (html) {
            $("#PartialView_dialog_Viewdetaildatetime").html(html);
        });

}
function GetProductProcesses(e) {
    async: false,
        $.ajax({
            url: `/ActualWO/GetProductProcesses?productCode=` + encodeURIComponent(e)
        })
            .done(function (response) {
                var html = '';
                if (response.length > 0) {


                    html += '<option value=' + response[0].process_code + '>' + response[0].process_name + '</option>';
                    for (var i = 1; i < response.length; i++) {
                        html += '<option value=' + response[i].process_code + '>' + response[i].process_name + '</option>';
                    }
                }
                $("#c_Process").html(html);
                // $("#m_Process").html(html);
                return;



            })
            .fail(function () {
                return;
            });



}

//view ClickViewDetailMaterial
function ViewDetailMaterial(cellValue, options, rowdata, action) {
    var html = "";
    html = '<button class="btn btn-xs btn-primary"  data-product="' + rowdata.product + '"   data-at_no="' + rowdata.at_no + '"  onclick="ClickViewDetailMaterial(this);">Detail</button>';
    return html;
}


function ClickViewDetailMaterial(e) {

    var product = $(e).data("product")
    var at_no = $(e).data("at_no")



    $('.popup-dialog.dialog_ViewDetailMaterial').dialog('open');


    $.get("/ActualWO/PartialView_dialog_ViewDetailMaterial?product=" + product +
        "&at_no=" + at_no + ""
        , function (html) {
            $("#PartialView_dialog_ViewDetailMaterial").html(html);
        });

};