//#region PRODUCT DIALOG
$(".dialogProduct").click(function () {
    $('.popupDialogProduct').dialog('open');
});
$(".popupDialogProduct").dialog({
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
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm"
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {
        $('#dialogProductNo').val('');
        $('#dialogProductName').val('');
        $('#dialogProductModel').val('');
        $("#dialogProductGrid").jqGrid
            ({
                url: `/TIMS/GetProducts`,
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'sid', name: 'sid', width: 80, align: 'center', hidden: true },
                    { key: false, label: 'Product Code', name: 'style_no', width: 200, align: 'left' },
                    { key: false, label: 'Product Name', name: 'style_nm', width: 300, align: 'left' },
                    { key: false, label: 'Model Code', name: 'md_cd', sortable: true, width: '300', align: 'left' },
                    { key: false, label: 'Project Name', name: 'prj_nm', sortable: true, width: '200', align: 'left' },
                    { key: false, label: 'SS Version', name: 'ssver', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'Part Name', name: 'part_nm', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'Standard', name: 'standard', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'Customer Rev', name: 'cust_rev', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'Order Number', name: 'order_num', editable: true, width: '180', align: 'left' },
                    { key: false, label: 'Description', name: 'cav', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'Packing Amount(EA)', name: 'pack_amt', editable: true, width: '150', align: 'right', formatter: 'integer' },
                    { key: false, label: 'Type', name: 'bom_type', editable: true, width: '100px', align: 'left' },
                    { key: false, label: 'TDS no', name: 'tds_no', editable: true, width: '100px', align: 'left' }
                ],
                formatter: {
                    integer: { thousandsSeparator: ",", defaultValue: '0' },
                    currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
                    number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
                },
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#dialogProductSelectBtn").removeClass("disabled");
                    var selectedRowId = $("#dialogProductGrid").jqGrid("getGridParam", 'selrow');
                    var row_id = $("#dialogProductGrid").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#dialogProductSelectBtn").click(function () {
                            $('#cProduct').val(row_id.style_no);
                            $('#mProduct').val(row_id.style_no);
                            $('.popupDialogProduct').dialog('close');
                        });
                    }
                },
                pager: '#dialogProductPage',
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: false,
                height: 250,
                width: $(".boxProduct").width(),
                autowidth: false,
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
//DIALOG PRODUCT SEARCH FUNCTION
$('#dialogProductSearchBtn').click(function () {
    var productNo = $('#dialogProductNo').val();
    var productName = $('#dialogProductName').val();
    var modelCode = $('#dialogProductModel').val();
    $.ajax({
        url: `/TIMS/GetProducts?productNo=${productNo}&productName=${productName}&modelCode=${modelCode}`
    })
        .done(function (response) {
            $("#dialogProductGrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: response }).trigger("reloadGrid");
            return;
        })
        .fail(function () {
            return;
        });
});
_getitem_type();
//OPEN QC DIALOG

function _getitem_type() {
    $.get("/QMS/get_item_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            $.each(data, function (key, item) {
                if (item.dt_cd != "PC") {
                    html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
                }
            });
            $("#dialogQCType").html(html);
        }
    });
}

//#endregion

//#region QC add VÕ
$(".dialogQC").click(function () {
    $('.popupDialogQC').dialog('open');
});
$(".popupDialogQC").dialog({
    width: '50%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '50%',
    minHeight: 500,
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
        $('#dialogQCCode').val('');
        $('#dialogQCName').val('');
        $('#dialogQCExp').val('');

        $("#dialogQCGrid").jqGrid
            ({
                url: `/TIMS/GetQCs`,
                datatype: 'json',
                mtype: 'Get',
                colModel: [
                    { key: true, label: 'ino', name: 'ino', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'QC Code', name: 'item_cd', width: 100, align: 'center', hidden: true },
                    { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '80px', align: 'center' },
                    { key: false, label: 'QC Code', name: 'item_vcd', sortable: true, width: '60px', align: 'center', hidden: true },
                    { key: false, label: 'QC Type', name: 'item_type', width: 60, align: 'center', },
                    { key: false, label: 'Ver', name: 'ver', width: 50, align: 'center', hidden: true },
                    { key: false, label: 'QC Name', name: 'item_nm', sortable: true, width: '100px', align: 'left' },
                    { key: false, label: 'QC Explain', name: 'item_exp', sortable: true, width: '200' },
                    { key: false, label: 'Use', name: 'use_yn', editable: true, width: '50px', align: 'center' },
                    { key: false, label: 'Create User', name: 'reg_id', editable: true, width: '100px' },
                    {
                        label: 'Create date', name: 'reg_dt', width: 150, align: 'center', formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                    },
                    { key: false, label: 'Change User', name: 'chg_id', editable: true, width: '100px' },
                    {
                        label: 'Change date', name: 'chg_dt', width: 150, align: 'center', formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                    },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#dialogQCSelectBtn").removeClass("disabled");
                    var selectedRowId = $("#dialogQCGrid").jqGrid("getGridParam", 'selrow');
                    var row_id = $("#dialogQCGrid").getRowData(selectedRowId);
                    if (row_id != null) {
                        $("#dialogQCSelectBtn").click(function () {
                            $('#cQc').val(row_id.item_vcd);
                            $('#mQc').val(row_id.item_vcd);
                            $('.popupDialogQC').dialog('close');
                        });
                    }
                },
                pager: '#dialogQCPage',
                viewrecords: true,
                rowNum: 50,
                rowList: [50, 100, 200, 500, 1000],
                sortable: true,
                loadonce: false,
                height: 250,
                width: $(".boxQC").width(),
                autowidth: false,
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
//DIALOG QC SEARCH FUNCTION
$('#dialogQCSearchBtn').click(function () {
    var qcType = $('#dialogQCType').val();
    var qcCode = $('#dialogQCCode').val();
    var qcName = $('#dialogQCName').val();
    var qcExp = $('#dialogQCExp').val();
    $.ajax({
        url: `/TIMS/GetQCs?qcType=${qcType}&qcCode=${qcCode}&qcName=${qcName}&qcExp=${qcExp}`
    })
        .done(function (response) {
            $("#dialogQCGrid").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: response }).trigger("reloadGrid");
            return;
        })
        .fail(function () {
            return;
        });
});

//#endregion

//#region add worker
//OPEN WORKER MAPPING DIALOG
var psid = '';
$(".popupDialogWorkerMapping").dialog({
    width: '50%',
    height: 500,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '50%',
    minHeight: 500,
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
        //grid
        $('#dialogWorkerMappingGrid').jqGrid({
            url: `/TIMS/GetProcessStaffTims?id_actual=${actualId}}`,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'psid', name: 'psid', align: 'center', hidden: true },
                { key: false, label: 'Staff Id', name: 'staff_id', width: 100, align: 'left', },
                { key: false, label: 'Name', name: 'uname', sortable: true, width: '200', align: 'left', },
                { key: false, label: ' Type ', name: 'staff_tp_nm', sortable: true, width: '120', },
                { key: false, label: ' Type ', name: 'staff_tp', sortable: true, width: '120', hidden: false },
                { key: false, label: 'Start Date', name: 'start_dt', width: '200', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'End Date', name: 'end_dt', width: '200', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
                { key: false, label: 'Use', name: 'use_yn', editable: true, width: '100px', align: 'center', width: 200, },
            ],
            pager: '#dialogWorkerMappingPage',
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: false,
            height: 250,
            width: $(".boxStaff").width(),
            autowidth: false,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
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
            onSelectRow: function () {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                //$("#dialogWorkerList_SelectBtn").removeClass("disabled");
                var selectedRowId = $("#dialogWorkerMappingGrid").jqGrid("getGridParam", 'selrow');
                var rowData = $("#dialogWorkerMappingGrid").getRowData(selectedRowId);
                dialogWorkerList_WorkerId = rowData.UserId;
                dialogWorkerList_WorkerName = rowData.UserName;
                psid = rowData.psid;
                $("#psid").val(rowData.psid);
                $("#dialogWorkerMapping_mWorkerId").val(rowData.staff_id);
                $("#dialogWorkerMapping_mWorkerName").val(rowData.uname);
                $("#dialogWorkerMapping_mWorkerType").val(rowData.staff_tp);
                $("#dialogWorkerMapping_mStartDate").val(rowData.start_dt);
                $("#dialogWorkerMapping_mEndDate").val(rowData.end_dt);
                $("#dialogWorkerMapping_mIsUsed").val(rowData.use_yn);

                $("#dialogWorkerMapping_create_li").removeClass("active");
                $("#dialogWorkerMapping_modify_li").addClass("active");
                $("#dialogWorkerMapping_create_tab").removeClass("active");
                $("#dialogWorkerMapping_modify_tab").removeClass("hidden");
                $("#dialogWorkerMapping_create_tab").addClass("hidden");
                $("#dialogWorkerMapping_modify_tab").addClass("active");
                $("#dialogWorkerMapping_modifyBtn").attr("disabled", false);
                $("#dialogWorkerMapping_deleteBtn").attr("disabled", false);
            }
        });
    },
});

function call_create(sts, id_update, start, end) {
    if (sts == "create_mold") { $("#thong_bao").html("This mold has already selected. If you confirm it, this mold will finish the task at the previous stage") }
    if (sts == "create_mc") { $("#thong_bao").html("This Machine has already selected. If you confirm it, this machine will finish the task at the previous stage") }
    if (sts == "create_wk") { $("#thong_bao").html("This Worker has already selected. If you confirm it, this worker will finish the task at the previous stage") }
    $("#sts_creat").val(sts);
    $("#id_update").val(id_update);
    $("#start_accept").val(start);
    $("#end_accept").val(end);

    $('#dialogAccept').dialog('open');
}
$('#dialogWorkerMapping_createBtn').on('click', function () {
    if ($("#dialogWorkerMapping_createForm").valid() == true) {
        $.ajax({
            url: "/TIMS/Createprocess_unitstaff",
            type: "get",
            dataType: "json",
            data: {
                staff_id: $('#dialogWorkerMapping_cWorkerId').val(),
                id_actual: $("#id_actual").val(),
                use_yn: $('#dialogWorkerMapping_cIsUsed').val()
            }
        })
            .done(function (data) {

                switch (data.result) {
                    case 0:
                        SuccessAlert("Success");
                        $("#list_staff").jqGrid('addRowData', data.kq.psid, data.kq, 'first');
                        $("#list_staff").setRowData(data.kq.psid, false, { background: "#d0e9c6" });
                        break;
                    case 1:
                        ErrorAlert('The Staff Unit was setting duplicate time.');
                        break;
                    case 2:
                        ErrorAlert('Start day was bigger End day. That is wrong');
                        break;
                    case 3:
                        call_create("create_wk", data.update, data.start, data.end);
                    case 4:
                        ErrorAlert("Xin vui lòng đăng nhập");
                        break;
                }

                //return;
            })
            .fail(function () {
                //return;
            });
    }
});
$("#dialogWorkerMapping_createForm").validate({
    rules: {
        "worker": {
            required: true,
        },
        "staff": {
            required: true,
        },
    },
});
$("#dialogWorkerMapping_modifyBtn").click(function () {

    $.ajax({
        type: "get",
        dataType: "json",
        url: "/TIMS/ModifyProcessUnitStaffTims",
        data: {
            psid: $('#psid').val(),
            staff_id: $('#dialogWorkerMapping_mWorkerId').val(),
            use_yn: $('#dialogWorkerMapping_mIsUsed').val(),
            start: $('#dialogWorkerMapping_mStartDate').val(),
            end: $('#dialogWorkerMapping_mEndDate').val()
        },
        success: function (data) {
            switch (data.result) {
                case 0:
                    SuccessAlert("Success");
                    var id = data.kq.psid;
                    $("#list_staff").setRowData(id, data.kq, { background: "#d0e9c6" });
                    $("#list_staff").setRowData(id, false, { background: "#d0e9c6" });
                    break;
                case 1:
                    ErrorAlert('The Staff Unit was setting duplicate time.');
                    break;
                case 2:
                    ErrorAlert('Start day was bigger End day. That is wrong');
                    break;
                case 4:
                    alert(data.message);
                    break;
            }
        }
    });
});
$('#dialogWorkerMapping_deleteBtn').on('click', function () {
    deleteType = 'del_wk';
    $('#dialogDangerous').dialog('open');
});
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

$("#deletestyle").click(function () {
    switch (deleteType) {
        case "del_wk":

            $.ajax({
                url: "/Tims/GetProcessStaffTims",
                type: "get",
                dataType: "json",
                data: {
                    id_actual: $("#id_actual").val(),
                    psid: $("#psid").val(),
                },
                success: function (data) {
                    if (data.result == true) {
                        SuccessAlert("Delete Success");
                        var id = data.psid;
                        $("#list_staff").jqGrid('delRowData', id);
                    } else {
                        ErrorAlert(data.message);
                    }
                },
            });
            $('#dialogDangerous').dialog('close');

            break;
        case "del_wactual": $.ajax({
            url: "/TIMS/Deletew_actual",
            type: "post",
            dataType: "json",
            data: {
                id: $('#id_actual').val(),
            },
            success: function (data) {
                if (data.result == true) {
                    SuccessAlert("Delete Success");
                    id = $('#id_actual').val();
                    $("#list").jqGrid('delRowData', id);
                }
                else {
                    ErrorAlert(data.kq);
                }
            },
            error: function (result) { }
        });
            $('#dialogDangerous').dialog('close');
            break;
        default:
            break;
    }
});
$('.deletebtn').click(function () {
    switch ($(this).attr("id")) {
        case "del_mold":
            deleteType = "del_mold";
            break;
        case "del_mc":
            deleteType = "del_mc";
            break;
        case "del_wk":
            deleteType = "del_wk";
            break;
        case "deleteBtn":
            deleteType = "del_wactual";
            break;
    }
    $('#dialogDangerous').dialog('open');
});
//OPEN WORKER LIST DIALOG
var dialogWorkerList_WorkerId = '';
var dialogWorkerList_WorkerName = '';
$('.dialogWorkerList').click(function () {
    $('.popupDialogWorkerList').dialog('open');
});
$(".popupDialogWorkerList").dialog({
    width: '50%',
    height: 500,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '50%',
    minHeight: 500,
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
        $('#dialogWorkerList_SelectBtn').addClass("disabled");
        $.ajax({
            url: `/TIMS/GetAllWorkerPositions`
        })
            .done(function (response) {
                var html = `<option value="">*Select Position*</option>`;
                $.each(response, function (key, item) {
                    html += `<option value="${item.Code}">${item.Name}</option>`;
                });
                $("#dialogWorkerList_PositionCode").html(html);
                return;
            })
            .fail(function () {
                return;
            });

        $('#dialogWorkerListGrid').jqGrid({
            datatype: function (postData) { getData_staff(postData); },
            mtype: 'Get',
            colModel: [
                { key: true, label: 'Id', name: 'UserId', sortable: true, width: 100, hidden: false },
                { key: false, label: 'PositionCode', name: 'PositionCode', width: 100, hidden: true },
                { key: false, label: 'Name', name: 'UserName', sortable: true, width: '300px' },
                { key: false, label: 'Position', name: 'PositionName', sortable: true, width: 300, hidden: false },
            ],
            pager: '#dialogWorkerPage',
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
            onSelectRow: function (rowid, selected, status, e) {
                //debugger;
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                $("#dialogWorkerList_SelectBtn").removeClass("disabled");
                var selectedRowId = $("#dialogWorkerListGrid").jqGrid("getGridParam", 'selrow');
                var rowData = $("#dialogWorkerListGrid").getRowData(selectedRowId);
                dialogWorkerList_WorkerId = rowData.UserId;
                dialogWorkerList_WorkerName = rowData.UserName;
            }
        });
    },
    close: function () {
        return;
    }
});
$('#dialogWorkerList_SelectBtn').on('click', function () {
    //debugger;
    $('#dialogWorkerMapping_cWorkerId').val(dialogWorkerList_WorkerId);
    $('#dialogWorkerMapping_cWorkerName').val(dialogWorkerList_WorkerName);
    $('#dialogWorkerMapping_mWorkerId').val(dialogWorkerList_WorkerId);
    $('#dialogWorkerMapping_mWorkerName').val(dialogWorkerList_WorkerName);
    $('.popupDialogWorkerList').dialog('close');
});
function getData_staff(pdata) {
    var params = new Object();

    if ($('#dialogWorkerListGrid').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var dialogWorkerList_WorkerId = $('#dialogWorkerList_WorkerId').val().trim();
    var dialogWorkerList_WorkerName = $('#dialogWorkerList_WorkerName').val().trim();
    var dialogWorkerList_PositionCode = $('#dialogWorkerList_PositionCode').val();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.UserId = dialogWorkerList_WorkerId;
    params.UserName = dialogWorkerList_WorkerName;
    params.PositionCode = dialogWorkerList_PositionCode;
    //params.processCode = $('#sMaterialCode').val();

    $('#dialogWorkerListGrid').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/TIMS/GetWorkers",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#dialogWorkerListGrid')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#dialogWorkerList_SearchBtn").click(function () {
    $('#dialogWorkerListGrid').clearGridData();
    $('#dialogWorkerListGrid').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogWorkerListGrid').jqGrid('getGridParam', 'postData');
    getData_staff(pdata);
});
//#endregion

//#region lưới võ
$(".popupDialogMaterial").dialog({
    width: '50%',
    height: 800,
    maxWidth: 1000,
    maxHeight: 800,
    minWidth: '50%',
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
    open: function (event, ui) {
        var dialogCompositeRollNormalContainer_ContainerCode = $('#dialogCompositeRollNormalContainer_ContainerCode').val() == null ? "" : $('#dialogCompositeRollNormalContainer_ContainerCode').val().trim();
        var dialogCompositeRollNormalContainer_ContainerName = $('#dialogCompositeRollNormalContainer_ContainerName').val() == null ? "" : $('#dialogCompositeRollNormalContainer_ContainerName').val().trim();
        var dialogCompositeRollNormalContainer_MaterialCode = $('#dialogCompositeRollNormalContainer_MaterialCode').val() == null ? "" : $('#dialogCompositeRollNormalContainer_MaterialCode').val().trim();
        $('#dialogCompositeRollNormalContainer_Grid').jqGrid({
            //url: `/ActualWO/searchbobbinPopup?bb_no=${dialogCompositeRollNormalContainer_ContainerCode}&bb_nm=${dialogCompositeRollNormalContainer_ContainerName}&mt_cd=${dialogCompositeRollNormalContainer_MaterialCode}&id_actual=${actualId}`,//Hằng code
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { key: true, label: 'bno', name: "bno", width: 80, align: 'center', hidden: true },
                { key: false, label: 'Container Code', name: 'bb_no', width: 150, align: 'center', sort: true },
                { key: false, label: 'Container Name', name: 'bb_nm', width: 150, align: 'left' },
                { key: false, label: 'Material', name: 'mt_cd', width: 400, align: 'left' },
            ],
            pager: '#dialogCompositeRollNormalContainer_Page',
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 300,
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
            gridComplete: function () {
                var rows = grid.getDataIDs();
            },
            loadComplete: function () {
                var rows = grid.getDataIDs();
            },
            onSelectRow: function () {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            },
        });
    },
    close: function () {
    }
});
//GET PROCESS
function GetTIMSProcesses() {
    $.ajax({
        url: `/TIMS/GetTIMSProcesses`
    })
        .done(function (response) {
            var html = `<option value="">*Select Process*</option>`;
            $.each(response, function (key, item) {
                html += `<option value="${item.ProcessCode}">${item.ProcessName}</option>`;
            });
            $("#cProcess").html(html);
            $("#mProcess").html(html);
            return;
        })
        .fail(function () {
            return;
        });
}
//GET ROLL
function GetTIMSRolls() {
    var html = `<option value="">*Select Roll*</option>`;
    $.ajax({
        url: `/TIMS/GetTIMSRolls`
    })
        .done(function (response) {
            if (response) {
                $.each(response, function (key, item) {
                    html += `<option value="${item.Code}">${item.Name}</option>`;
                });
            }
            $("#cRoll").html(html);
            $("#mRoll").html(html);
            return;
        })
        .fail(function () {
            $("#cRoll").html(html);
            $("#mRoll").html(html);
            return;
        });
}
$("#searchBtn").click(function () {
    $('#list_primary').clearGridData();
    $('#list_primary').jqGrid('setGridParam', { search: true });
    var pdata = $('#list_primary').jqGrid('getGridParam', 'postData');
    getData_primary(pdata);
    getData_Finished(pdata);
});
$("#list_primary").jqGrid({
    //url: "/ActualWO/Getdataw_actual",
    datatype: function (postData) { getData_primary(postData); },
    mtype: 'Get',
    colModel: [
        { key: true, label: 'id_actualpr', name: 'id_actualpr', width: 80, align: 'center', hidden: true },
        { label: 'PO NO', name: 'at_no', width: 110, align: 'center', },
        { label: 'Create', name: 'create', width: 70, formatter: create_actualcon, align: 'center', },
        { label: 'Product', name: 'product', width: 100 },
        { label: 'Product Name', name: 'style_nm', width: 100 },
        { key: false, label: 'Type(Roll/Sheet)', name: 'bom_type', sortable: true, width: '70', align: 'left' },
        { label: 'Model', name: 'md_cd', width: 100 },
        { label: 'Target', name: 'target', width: 100, align: 'right', formatter: 'integer' },
        { label: 'Description', name: 'remark', width: 120 },
        { label: 'Complete (%)', name: '', width: 80, align: 'center', formatter: PercentagePrimary },
        { label: 'Processing', name: '', width: 80, align: 'center', formatter: ProcessCalculating },
        { label: '', name: 'poRun', width: 80, align: 'center', hidden: true },
        { label: '', name: 'process_count', width: 80, align: 'center', hidden: true },
        { label: '', name: 'finish', width: 80, align: 'center', formatter: FinishPO },

    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('#list_staff').clearGridData();

        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list_primary").jqGrid("getGridParam", 'selrow');
        row_id = $("#list_primary").getRowData(selectedRowId);
        actualProductCode = row_id.product;
        $("#at_no").val(row_id.at_no);

        $("#id_actualpr").val(row_id.id_actualpr);
        $("#oqc_name").val(row_id.product);
        $("#dialogCompositeProduct").val(row_id.product);
        $("#mTarget").val(row_id.target);
        $("#mRemark").val(row_id.remark);
        $("#mProduct").val(row_id.product);
        $("#qc_name").val(row_id.product);

        $('#list').clearGridData();
        $('#list').jqGrid('setGridParam', { search: true });
        var pdata = $('#list').jqGrid('getGridParam', 'postData');
        getData(pdata);

        $('#list_SX').clearGridData();
        $('#list_SX').jqGrid('setGridParam', { search: true });
        var pdata = $('#list_SX').jqGrid('getGridParam', 'postData');
        getData_SX(pdata);

        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#modifyBtn").attr("disabled", false);
        $("#deleteBtn").attr("disabled", false);
    },
    pager: jQuery('#jqGrid_primaryPager'),
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000],
    height: 300,
    width: $(".box-primary").width() - 5,
    autowidth: false,
    rowNum: 50,
    caption: '',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: false,
    subGrid: false,
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
    gridComplete: function () {

        var rows = $("#list_primary").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var poRun = $("#list_primary").getCell(rows[i], "poRun");
            var CountProcess = $("#list_primary").getCell(rows[i], "process_count");

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
        }
    },
});
function create_actualcon(cellValue, options, rowdata, action) {
    var html = "";
    html = '<button class="btn btn-xs btn-primary"  onclick="create_actual_con(this);">Create</button>';
    return html;
}
function create_actual_con(e) {
    $('#dialogCreate_actual').dialog('open');
}
$(document).ready(function (e) {
    $("#sDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeYear: true
    });
});
$("#list_SX").jqGrid({
    datatype: function (postData) { getData_SX(postData); },
    mtype: 'Get',
    colModel: [
        { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
        { label: 'Composite Code', name: 'mt_cd', width: 290, sortable: true, },
        { label: 'Container', name: 'bb_no', width: 215, },
        { label: 'Quantity', name: 'real_qty', width: 81, formatter: 'integer', align: 'right' },
        { label: 'Quantity Real', name: 'gr_qty', width: 81, formatter: 'integer', align: 'right' },
        { label: 'Status', name: 'mt_sts_cd', width: 90 },
        { label: 'Recevice Date', name: 'recevice_dt_tims', width: 140, align: 'center', formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }  },
    ],
    onSelectRow: function (rowid, selected, status, e) {
    },
    pager: jQuery('#jqGrid_SXPager'),
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000],
    height: 300,
    width: $(".box-sx").width() - 5,
    autowidth: false,
    rowNum: 50,
    caption: '',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: false,
    subGrid: false,
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
function getData_SX(pdata) {
    var params = new Object();
    if ($('#list_SX').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.at_no = $("#at_no").val().trim();
    $('#list_SX').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/Getdataw_actual_SX`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (data.result) {
                var showing = $('#list_SX')[0];
                showing.addJSONData(data);
            }
            else {
                ErrorAlert(data.message);
            }
        },
        error: function () {
            return;
        }
    });
};
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
function getData_primary(pdata) {
    var params = new Object();
    var searchFiler = $("#list_primary").val();
    if ($('#list_primary').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.at_no = $("#sPO").val().trim();
    params.product = $("#sProductCode").val().trim();
    params.product_name = $("#sProductName").val().trim();
    params.bom_type = $("#s_bom_type").val().trim();
    params.model = $("#sModel").val().trim();
    params.regstart = $("#sDate").val().trim();
    params.regend = $("#eDate").val().trim();


    params.type = "Web";
    $('#list_primary').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/GetDataWActualPrimary`,
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
};
//GRID
function Grid() {
    grid.jqGrid({
        mtype: 'GET',
       datatype: function (postData) { getData(postData); },
        colModel: [
            { name: 'Id', key: true, hidden: true },
            { name: 'RollCode', hidden: true },
            { label: '', name: 'target', width: 80, align: 'center', hidden: true },
            { label: 'View', name: '', width: 50, formatter: ViewDetailDatetime, align: 'center', },
            { label: 'Name', name: 'Name_View', width: 120 },
            { label: 'Name', name: 'Name', hidden: true },
            { label: 'Actual', name: 'ActualQty', width: 80, align: 'right', formatter: 'integer' },
            { label: 'Process (%)', name: '', width: 120, align: 'center', formatter: Percentage },
            { label: 'Type', name: 'RollName', sortable: true, width: 50, align: 'left' },
            { name: 'QCCode', sortable: true, width: 80, align: 'center' },
            { label: 'Date', name: 'Date', width: 80, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            { label: 'Process', name: 'Name', sortable: true, width: 100, align: 'left', hidden: true },
            {
                label: 'IsFinish', name: 'Id', width: 60, align: 'center', formatter: function (cellvalue, options, rowObject) {

                    return '<input type="checkbox" name="selectedCall" value="' + cellvalue + '" id = "id_actual' + cellvalue + '" onclick="getCheck(this)"/>'

                },

            },
            { name: 'Id', key: false, width: 50, hidden: true },


            { label: 'IsFinish', name: 'IsFinish', width: 50, align: 'center', hidden: true },
            { label: 'Delete', name: 'delete', width: 60, align: 'center', formatter: xoa_wactual },
            { label: 'ProcessRun', name: 'ProcessRun', width: 100, align: 'center', hidden: true },
        ],
        pager: '#listpage',
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: 400,
        caption: 'Process',
        width: null,
        loadonce: false,
        emptyrecords: 'No Data',
        subGrid: false,
        subGridRowExpanded: showChildGrid,
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
        gridComplete: function () {
            var rows = grid.getDataIDs();
            for (var i = 0; i < rows.length; i++) {
                var ProcessRun = grid.getCell(rows[i], "ProcessRun");
                var Id = grid.getCell(rows[i], "Id");
                var IsFinish = grid.getCell(rows[i], "IsFinish");

                if (ProcessRun == 1) {
                    grid.jqGrid('setCell', rows[i], "Name_View", "", {
                        'background-color': '#05d405',
                        color: '#fff',
                        'font-size': '1.025em'
                    });
                }
                else {
                    grid.jqGrid('setCell', rows[i], "Name_View", "", {
                        'background-color': 'red',
                        color: '#fff',
                        'font-size': '1.025em'
                    });
                }
                if (IsFinish == '1') {

                    $("#id_actual" + Id).attr("checked", true);
                }
                else {
                    $("#id_actual" + Id).attr("checked", false);
                }
            }
        },
        onSelectRow: function () {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            actualId = grid.jqGrid("getGridParam", 'selrow');
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);
            $('#dialogCompositeId').val(row_id.ID);
            $('#dialogCompositeProcess').val(row_id.Name);
            $('#dialogCompositeDate').val(row_id.Date);
            $('#type_roll').val(row_id.RollCode);
            $("#mProcess").val(row_id.Name);
            $("#mQc").val(row_id.QCCode);
            $("#mRoll").val(row_id.RollCode);
            $("#check_qc").val(row_id.QCCode);
            $("#qc_item_vcd").val(row_id.QCCode);
            $("#qc_date").val(row_id.Date);
            $("#oqc_item_vcd").val(row_id.QCCode);
            $("#oqc_date").val(row_id.Date);
            var rowData = grid.getRowData(actualId);
            actualProcessCode = rowData.Name;
            $("#id_actual").val(rowData.Id);
            actualDate = rowData.Date;
            actualRollType = rowData.RollCode;


            $("#staff_id_s").val("");
            $("#StartDate").val("");
            $("#EndDate").val("");

            $('#list_staff').clearGridData();
            $('#list_staff').jqGrid('setGridParam', { search: true });
            var pdata = $('#list_staff').jqGrid('getGridParam', 'postData');
            getlist_staff(pdata);
        },
    });
}
function getCheck(e) {
    var r = confirm("Bạn có chắc muốn thay đổi không?");
    var checkbox = document.getElementById("id_actual" + e.value);
    var check = "";

    if (checkbox.checked === true) {
        check = "true";
    }
    else {
        check = "false";
    }
    if (r == true) {
       // debugger
        // Khai báo tham số
        $.ajax({
            url: "/TIMS/UpdateProcessIsFinish",
            type: "get",
            headers: {
                "Content-Type": "application/json"
            },
            data: {
                id_actual: e.value,
                IsFinished: check,
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
    }
    else {

        if (check === "Y") {
            document.getElementById("idMaterialBom" + e.value).checked = false;
        }
        else {
            document.getElementById("idMaterialBom" + e.value).checked = true;
        }
        return false;
    }
}
function _Date(cellvalue, options, rowObject) {
    if (cellvalue == null)
        return "";
    if (cellvalue.length == 14) {

        return cellvalue.replace(/^(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})$/, "$1-$2-$3 $4:$5:$6");
    }

    if (cellvalue.length > 8) {
        return cellvalue;
    }
    cellvalue = cellvalue.substr(0, 8);

    var reg = /(\d{4})(\d{2})(\d{2})/;
    if (reg.test(cellvalue))
        return cellvalue.replace(reg, "$1-$2-$3");
    else {
        reg = /(\d{4})(\d{2})\-(\d{2})/;
        if (reg.test(cellvalue))
            return cellvalue.replace(reg, "$1-$2-$3");
        else {
            reg = /(\d{4})\-(\d{2})(\d{2})/;
            if (reg.test(cellvalue))
                return cellvalue.replace(reg, "$1-$2-$3");
            else {
                reg = /(\d{4})\-(\d{2})\-(\d{2})/;
                if (reg.test(cellvalue))
                    return cellvalue.replace(reg, "$1-$2-$3");
                else {
                    reg = /(\d{4})(\d{2}).(\d{2})/;
                    if (reg.test(cellvalue))
                        return cellvalue.replace(reg, "$1-$2-$3");
                    else {
                        reg = /(\d{4}).(\d{2}).(\d{2})/;
                        if (reg.test(cellvalue))
                            return cellvalue.replace(reg, "$1-$2-$3");

                        else {
                            reg = /(\d{4})(\d{2})\\(\d{2})/;
                            if (reg.test(cellvalue))
                                return cellvalue.replace(reg, "$1-$2-$3");
                            else {
                                reg = /(\d{4})\\(\d{2})\\(\d{2})/;
                                if (reg.test(cellvalue))
                                    return cellvalue.replace(reg, "$1-$2-$3");
                                else {
                                    reg = /(\d{4})\\(\d{2})\.(\d{2})/;
                                    if (reg.test(cellvalue))
                                        return cellvalue.replace(reg, "$1-$2-$3");
                                    else {
                                        reg = /(\d{4})\.(\d{2})\\(\d{2})/;
                                        if (reg.test(cellvalue))
                                            return cellvalue.replace(reg, "$1-$2-$3");
                                        else
                                            return cellvalue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
function xoa_wactual(cellValue, options, rowdata, action) {
    html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + rowdata.Id + '"onclick="xoa_wactual_con(this);">X</button>';
    return html;
}
function xoa_wactual_con(e) {
    $.ajax({
        //url: '/TIMS/xoa_wactual_con?id=' + $(e).data("id"),
        url: '/TIMS/DeleteActualChild',
        type: "Delete",

        dataType: "json",
        data: {
            id: $(e).data("id"),
        },
        success: function (data) {
            if (data.result == true) {
                SuccessAlert("Delete Success");
                id = $('#id_actual').val();
                $("#list").jqGrid('delRowData', id);
            }
            else {
                ErrorAlert(data.message);
            }
        },
    });
}
$('#dialogWorkerMapping_mStartDate').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});
$('#dialogWorkerMapping_mEndDate').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});


$('#EndDate').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});
$('#StartDate').datetimepicker({
    format: 'YYYY-MM-DD HH:mm:ss',
});

$("#list_staff").jqGrid({
    mtype: 'Get',
    colModel: [
        { key: true, label: 'psid', name: 'psid', hidden: true },
        { key: false, label: 'ID', name: 'staff_id', align: 'center', width: 100 },
        { label: 'Name', name: 'uname', width: 150, sortable: true, },
        { label: 'Actual', name: 'ActualQty', sortable: true, width: 100, align: 'right', formatter: 'integer' },

        //{ label: 'Actual CNgay', name: 'actual_cn', sortable: true, width: 100, align: 'right', formatter: 'integer' },
        //{ label: 'Actual CDem', name: 'actual_cd', sortable: true, width: 100, align: 'right', formatter: 'integer' },

        { label: 'Defective', name: 'Defective', sortable: true, width: 100, align: 'right', formatter: 'integer' },
        { label: '', name: '', sortable: false, width: 100, align: 'center', formatter: CompositeBtn },
        { key: false, label: 'Start Date', name: 'start_dt', width: '200', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { key: false, label: 'End Date', name: 'end_dt', width: '200', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
        { key: false, label: 'Use', name: 'use_yn', hidden: true },
        { key: false, label: 'het_ca', name: 'het_ca', hidden: true },
    ],
    onSelectRow: function (rowid, selected, status, e) {
        $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
        var selectedRowId = $("#list_staff").jqGrid("getGridParam", 'selrow');
        rowData = $("#list_staff").getRowData(selectedRowId);
        $("#staff_id").val(rowData.staff_id);
        $("#psid").val(rowData.psid);

        $("#dialogWorkerMapping_mWorkerId").val(rowData.staff_id);
        $("#dialogWorkerMapping_mWorkerName").val(rowData.uname);
        $("#dialogWorkerMapping_mWorkerType").val(rowData.staff_tp);
        $("#dialogWorkerMapping_mStartDate").val(rowData.start_dt);
        $("#dialogWorkerMapping_mEndDate").val(rowData.end_dt);
        $("#dialogWorkerMapping_mIsUsed").val(rowData.use_yn);

        $("#dialogWorkerMapping_create_li").removeClass("active");
        $("#dialogWorkerMapping_modify_li").addClass("active");
        $("#dialogWorkerMapping_create_tab").removeClass("active");
        $("#dialogWorkerMapping_modify_tab").removeClass("hidden");
        $("#dialogWorkerMapping_create_tab").addClass("hidden");
        $("#dialogWorkerMapping_modify_tab").addClass("active");
        $("#dialogWorkerMapping_modifyBtn").attr("disabled", false);
        $("#dialogWorkerMapping_deleteBtn").attr("disabled", false);
    },

    footerrow: true,
    userDataOnFooter: true,
    pager: jQuery('#list_staffpage'),
    viewrecords: true,
    rowList: [50, 100, 200, 500, 1000],
    height: 300,
    width: $(".box-sx").width() - 5,
    autowidth: false,
    rowNum: 50,
    caption: '',
    loadtext: "Loading...",
    emptyrecords: "No data.",
    rownumbers: true,
    gridview: true,
    loadonce: false,
    subGrid: true,
    subGridRowExpanded: showChildGrid, // javascript function that will take care of showing the child grid
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
    gridComplete: function () {
        var rows = $("#list_staff").getDataIDs();
        for (var i = 0; i < rows.length; i++) {
            var het_ca = $("#list_staff").getCell(rows[i], "het_ca");
            if (het_ca == "QK") {
                $("#list_staff").jqGrid('setRowData', rows[i], false, { background: '#ccc' });
            }
        }
        //total

        //prevCellVal = { cellId: undefined, value: undefined };
        //var rowIds = $("#list_staff").getDataIDs();
        //rowIds.forEach(GridScanProduct_SetRowColor);

        //sum Quantity
        var ids = $("#list_staff").jqGrid('getDataIDs');
        var sum_Quantity = 0;
        var sum_DefQuantity = 0;
        var $self = $(this)
        for (var i = 0; i < ids.length; i++) {
            sum_Quantity += parseInt($("#list_staff").getCell(ids[i], "ActualQty"));
            sum_DefQuantity += parseInt($("#list_staff").getCell(ids[i], "Defective"));
        }

        $self.jqGrid("footerData", "set", { uname: "Total" });
        $self.jqGrid("footerData", "set", { ActualQty: sum_Quantity });
        $self.jqGrid("footerData", "set", { Defective: sum_DefQuantity });


    },
});
$("#dialogWorkerMapping_modify_li").click(function () {
    $("#dialogWorkerMapping_create_li").removeClass("active");
    $("#dialogWorkerMapping_modify_li").addClass("active");

    $("#dialogWorkerMapping_create_tab").removeClass("active");
    $("#dialogWorkerMapping_modify_tab").removeClass("hidden");

    $("#dialogWorkerMapping_create_tab").addClass("hidden");
    $("#dialogWorkerMapping_modify_tab").addClass("active");
});
$("#dialogWorkerMapping_create_li").click(function () {
    $("#dialogWorkerMapping_create_li").addClass("active");
    $("#dialogWorkerMapping_modify_li").removeClass("active");

    $("#dialogWorkerMapping_create_tab").addClass("active");
    $("#dialogWorkerMapping_modify_tab").addClass("hidden");

    $("#dialogWorkerMapping_create_tab").removeClass("hidden");
    $("#dialogWorkerMapping_modify_tab").removeClass("active");
});
$("#searchBtn_Staff").click(function () {
    $('#list_staff').clearGridData();
    $('#list_staff').jqGrid('setGridParam', { search: true });
    var pdata = $('#list_staff').jqGrid('getGridParam', 'postData');
    getlist_staff(pdata);
});
function getlist_staff(pdata) {
    var params = new Object();

    if ($("#list_staff").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.id_actual = $("#id_actual").val();
    params.staff_id = $("#staff_id_s").val();
    params.StartDate = $("#StartDate").val();
    params.EndDate = $("#EndDate").val();
    $("#list_staff").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: `/TIMS/GetListStaffTims`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (data.result) {
                var showing = $("#list_staff")[0];
                showing.addJSONData(data);
            }
            else {
                ErrorAlert(data.message);
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
    //get staff_id

    var rowData = $('#list_staff').jqGrid('getRowData', parentRowKey);
    var staff_id = rowData.staff_id;
    var psid = rowData.psid;

    $("#" + childGridID).jqGrid({
        url: `/TIMS/GetListActualDetailTims?id_actual=${$("#id_actual").val()}&psid=${psid}`,
        mtype: "GET",
        datatype: "json",
        //async: false,
        page: 1,
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'Register Date', name: 'reg_dt', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }},
            { label: 'Container', name: 'bb_no', width: 280, },
            { label: 'ML No', name: 'mt_cd', width: 300, sortable: true, },
            { label: 'Quantity', name: 'real_qty', width: 81, align: 'right', formatter: 'integer' },
            { label: 'Quantity Real', name: 'gr_qty', width: 81, align: 'right', formatter: 'integer' },
            { label: 'Shift', name: 'ShiftName', width: 70 },
        ],
        shrinkToFit: false,
        loadonce: false,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false, // set the subGrid property to true to show expand buttons for each row
        rowNum: 2000,
        rownumbers: true,
        loadonce: true,
        multiPageSelection: true,
        rowList: [60, 80, 100, 120],
        viewrecords: true,
        rowNum: 10,
        pager: "#" + childGridPagerID,
        jsonReader:
        {
            root: "data",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        onSelectRow: function (id, rowid, status, e) {
            //if (id)
            //{
            //    $("#" + childGridID).jqGrid('editRow', id, true);
            //}
        },
    });
}
function getData(pdata) {
    var params = new Object();

    if (grid.jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.at_no = $('#at_no').val();
    grid.jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: `/TIMS/GetInfoActualTims`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (reposive) {
            if (reposive.result){
                var showing = grid[0];
                showing.addJSONData(reposive);
            }
            else {
                ErrorAlert(reposive.message);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#saverecreate_actual").click(function () {

    var name = $("#cProcess").val();
    var Roll = $("#cRoll").val();
    if (name == "") {
        alert("PLease choose name");
        return;
    } if (Roll == "") {
        alert("PLease choose Roll");
        return;
    } else {
        var style_no = $("#mProduct").val();
        var at_no = $("#at_no").val();
        var name = $("#cProcess").val();
        var roll = $("#cRoll").val();
        $.ajax({
            //url: "/TIMS/Add_w_actual",
            url: "/TIMS/AddActualForTims",
            type: "POST",
            dataType: "json",
            data: {
                style_no: style_no,
                at_no: at_no,
                name: name,
                roll: roll,
            },
            success: function (data) {

                $('#dialogCreate_actual').dialog('close');

                if (data.result == true) {
                    var id = data.kq[0].id_actual;
                    $("#list").jqGrid('addRowData', id, data.kq[0], 'first');
                    $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    SuccessAlert("Success");
                } else {
                    ErrorAlert(data.message);
                }
            },
        });
    }
});



function CompositeBtn(cellValue, options, rowdata, action) {
    html = `<button class="btn btn-sm btn-primary button-srh compositeBtn_" id="compositeBtn_${rowdata.psid}" data-psid="${rowdata.psid}"  data-userId="${rowdata.staff_id}"  `;
    html += ` onclick="CompositePopup(this)">Composite</button>`;
    return html;
}
//#endregion
//#region MAPPING
//$("#change_gr").click(function () {
//    var myList = grid2.jqGrid('getDataIDs');
//    var id = [];
//    var gr_qty = [];
//    var value_all = [];
//    for (var i = 0; i < myList.length; i++) {
//        var rowData = grid2.jqGrid('getRowData', myList[i]);
//        var chuoi = "#" + myList[i] + "_gr_qty1";
//        var get_input = $(chuoi).val();
//        var value = "";
//        if (get_input == undefined) {
//            value = rowData.gr_qty;
//        } else {
//            value = get_input;
//        }
//        value_all.push(value);
//        id.push(myList[i]);
//    }
//    //đưa vào controller xử lí
//    $.ajax({
//        url: "/TIMS/change_gr_dv?value_new=" + value_all + "&wmtid=" + id,
//        type: "post",
//        success: function (data) {
//            if (data.result == false) {
//                ErrorAlert(data.message);
//                call_table1();
//            } else {
//                SuccessAlert("Success");
//                call_table1();
//            }
//            $("#result").html("<label class='text-success'>&nbsp;&nbsp;Modify: " + data.success + "(Tray)</label>" + "<label class='text-primary'>&nbsp;&nbsp;Has over the other process: " + data.dem_quacd + "(Lot)</label>" + "<label class='text-danger'>&nbsp;&nbsp;Fail: " + data.dontexits + "(Lot)</label>");
//        },
//    });
//});
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
        //url: "/TIMS/ChangeGroupDevide?value_new=" + value_all + "&value_old=" + gr_qty + "&wmtid=" + id + "&psid=" + $("#psid").val(),
        url: "/TIMS/ChangeGroupDevide",
        type: "POST",
        dataType: "json",
        data: {
            value_new: value_all,
            value_old: gr_qty,
            wmtid: id,
            psid: $("#psid").val(),
        },
        success: function (data) {
            if (data.result == false) {
                ErrorAlert(data.message);
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaTims?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
            } else {
                SuccessAlert("Success");
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaTims?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
            }
        },
    });
});
function Xoa_qr_con(e) {
    $.ajax({
        url: '/TIMS/Xoa_mt_pp_composite?id=' + $(e).data("id"),
        type: "get",
        dataType: "json",
        success: function (data) {
            if (data.result != false) {
                var id = $(e).data("id");
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('delRowData', id);
            } else {
                ErrorAlert(data.message)
            }
        },
    });
}
function Cancel_EACon(e) {
    $.ajax({
        url: '/TIMS/CancelEA',
        type: "POST",
        dataType: "json",
        data: {
            id: $(e).data("id"),
            psid: $("#psid").val(),
        },
        success: function (data) {
            if (data.result == true) {
                SuccessAlert(data.message);
                jQuery("#dialogCompositeRollNormal_MateritalGrid").jqGrid("clearGridData");
                var id = $(e).data("id");
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('delRowData', id);
            } else {
                ErrorAlert(data.message)
            }
        },
    });
}
//#region Merge
$(".PopupOK").dialog({
    width: '70%',
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
$("#dialogOK").jqGrid
    ({
        mtype: 'Get',
        colModel: [
            { key: true, name: 'wmtid', hidden: true },
            { label: 'Container', name: 'bb_no', width: 350, sortable: true },
            { label: 'ML No', name: 'mt_cd', width: 350, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 200, },
            { label: 'Quantity', name: 'gr_qty', },
            { label: 'PO NO', name: 'at_no', },
        ],
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#dialogOK").jqGrid("getGridParam", 'selrow');
            row_id = $("#dialogOK").getRowData(selectedRowId);
        },
        pager: jQuery('#dialogOKPage'),
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        sortable: true,
        loadonce: false,
        height: 200,
        multiselect: true,
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
$("#PopupOK_Search").click(function () {
    $('#dialogOK').clearGridData();
    $('#dialogOK').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogOK').jqGrid('getGridParam', 'postData');
    getData_Merge(pdata);
});
function MergeCon(e) {
    $("#mt_cd_merge").val($(e).data("mt_cd"));
    $('.PopupOK').dialog('open');
    $('#dialogOK').clearGridData();
    $('#dialogOK').jqGrid('setGridParam', { search: true });
    var pdata = $('#dialogOK').jqGrid('getGridParam', 'postData');

    getData_Merge(pdata);
}
function getData_Merge(pdata) {
    var params = new Object();

    if ($('#dialogOK').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var mt_cd = $('#mt_cd_merge_s').val().trim();
    var bb_no = $('#bb_no_merge_s').val().trim();
    var mt_no = $('#mt_no_ok').val().trim();
    var id_actual = $('#id_actual').val().trim();
    var mt_lot = $('#mt_cd_merge').val();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.mt_cd = mt_cd;
    params.mt_no = mt_no;
    params.mt_lot = mt_lot;
    params.bb_no = bb_no;
    params.id_actual = id_actual;

    $('#dialogOK').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: "/TIMS/GetOkeReasonTims",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#dialogOK')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#Gop_OK").click(function () {
    var so_luongmm = $("#GR_OK").val();
    var i, check_jqgridng = $("#dialogOK").jqGrid("getGridParam", "selarrrow"), n, rowData;
    if (so_luongmm == "") {
        alert("Please Enter Quantity");
    }
    if (check_jqgridng.length < 1) {
        alert("Please choose ML LOT");
    }
    else {
        var mt_cd_ok = $("#mt_cd_merge").val();
        var psid = $("#psid").val();
        var trang_thai = $("#trang_thai").val();
        if (trang_thai == "DV") {
            $.get("/TIMS/Gop_OK_DV?wmtid=" + check_jqgridng + "&soluong=" + so_luongmm + "&mt_cd=" + mt_cd_ok + "&psid=" + psid, function (data) {
                if (data.result == true) {
                    console.log(data);
                    SuccessAlert("Success");
                    $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaTims?" + "mt_cd=" + data.mt_cd, datatype: "json" }).trigger("reloadGrid");

                } else {
                    ErrorAlert(data.message);
                }
                $('.PopupOK').dialog('close');
            });
        } else {
            $.get("/TIMS/Gop_OK?wmtid=" + check_jqgridng + "&soluong=" + so_luongmm + "&mt_cd=" + mt_cd_ok + "&psid=" + psid, function (data) {
                if (data.result == true) {
                    SuccessAlert("Success");
                } else {
                    ErrorAlert(data.message);
                }
                $('.PopupOK').dialog('close');
                var value = $("#psid").val();
                call_table1(value);
            });
        }

    }
});
//#endregion
//OPEN COMPOSITE DIALOG
function total_cp(cellValue, options, rowdata, action) {
    var gr_qty = (rowdata.gr_qty != "" && rowdata.gr_qty != undefined && rowdata.gr_qty != null ? rowdata.gr_qty : 0);
    var Remain = (rowdata.Remain != "" && rowdata.Remain != undefined && rowdata.Remain != null ? rowdata.Remain : 0);
    var tong = parseInt(gr_qty) + parseInt(Remain);
    return tong;
}
function finish_vl(cellValue, options, rowdata, action) {
    var id = rowdata.wmmid;
    var use_yn = rowdata.use_yn;
    html = '<button  class="btn btn-sm btn-secondary button-srh" data-id="' + id + '" data-use_yn="' + use_yn + '"onclick="Finish_vlcon(this);">F</button>';
    return html;
}
function stop_cp(cellValue, options, rowdata, action) {
    var id = rowdata.wmmid;
    var html = "";
    if (rowdata.sts_share == "N") {
        html = '<button  class="btn btn-sm btn-danger button-srh">Đã Dừng</button>';
    } else {
        html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + id + '" onclick="stop_cpcon(this);">Dừng kế thừa</button>';
    }
    return html;
}
function stop_cpcon(e) {
    var id = $(e).data("id");
    var psid = $("#psid").val();
    $.get("/TIMS/StopCp?wmmid=" + id + "&psid=" + psid, function (data) {
        if (data.result == true) {
            SuccessAlert("");
            grid3.setRowData(id, false, { background: "#d0e9c6" });
        } else {
            ErrorAlert(data.message)
        }
    });
}
function Finish_vlcon(e) {
    var id = $(e).data("id");
    var use_yn = $(e).data("use_yn");
    if (use_yn == "N") {
        $("#thong_bao").html("Is A sure you want to return to the used state");
        $("#sts_creat").val("Finish_Back");
        $('#dialogAccept').dialog('open');
        $("#wmmid").val(id)
    }
    else {
        $.get("/TIMS/FinishBack?wmmid=" + id, function (data) {
            if (data.result == true) {
                SuccessAlert("");
                var rowData = grid3.jqGrid('getRowData', id);
                rowData.use_yn = data.use_yn;
                grid3.jqGrid('setRowData', id, rowData);
                grid3.setRowData(id, false, { background: "#d0e9c6" });
            } else {
                ErrorAlert(data.message)
            }
        });
    }
}
function Return_vl(cellValue, options, rowdata, action) {
    var id = rowdata.wmmid;
    var mt_cd = rowdata.mt_cd;
    var mt_lot = rowdata.mt_lot;
    html = '<button  class="btn btn-sm btn-success button-srh" data-id="' + id + '" data-mt_cd="' + mt_cd + '" data-mt_lot="' + mt_lot + '" onclick="Return_vlcon(this);">R</button>';
    return html;
}
function Return_vlcon(e) {
    $('#dialogReturn').dialog('open');
    $("#mt_cd_rt").val($(e).data("mt_cd"));
    $("#mt_lot_rt").val($(e).data("mt_lot"));
}
$("#savereturn").click(function () {
    var soluong = $("#return_length").val();
    var mt_cd = $("#mt_cd_rt").val();
    var mt_lot = $("#mt_lot_rt").val();
    $.get("/TIMS/savereturn_lot?soluong=" + soluong + "&mt_cd=" + mt_cd + "&mt_lot=" + mt_lot, function (data) {
        $('#dialogReturn').dialog('close');
        $("#return_length").val("0");
        if (data.result == true) {
            SuccessAlert("Success");
            console.log(data.kq[0]);
            var id = data.kq[0].wmmid;
            grid3.setRowData(id, data.kq[0], { background: "#d0e9c6" });
        }
        else {
            ErrorAlert(data.message);
        }
    });
});
function cancel(cellValue, options, rowdata, action) {
    var html = "";
    if (rowdata.use_yn == "Y") {
        var id = rowdata.wmmid;
        html = '<i class="fa fa-times-circle" aria-hidden="true" data-id="' + id + '"onclick = "Cancel_vl(this);"></i>';
    }
    return html;
}
function Cancel_vl(e) {
    var value = $("#psid").val();

    $.get("/TIMS/Cancel_mapping?wmmid=" + $(e).data("id") + "&psid=" + value, function (data) {
        call_table1(value);
        if (data.result == true) {
            var id = $(e).data("id");
            grid3.jqGrid('delRowData', id);
            SuccessAlert(data.message);
        }
        else {
            ErrorAlert(data.message);
        }
    });
}
$('#dialogCompositeRollNormalContainer_SearchBtn').on('click', function () {
    get_ds_bb();
});
$('#dialogCompositeRollNormalContainer_CreateBtn').on('click', function () {
    if (!bobbinNumber) {
        alert('Select 1 bobbin.');
        return;
    }
    if (bobbinMaterialCode) {
        alert('Bobbin had material.');
        return;
    }
    var staff_id = $("#staff_id").val();
    var psid = $("#psid").val();
    var params = new Object();
    params.bb_no = bobbinNumber;
    params.id_actual = actualId;
    params.name = actualProcessCode;
    params.psid = psid;
    params.staff_id = staff_id;
    params.style_no = actualProductCode;
    $.ajax({
        url: "/TIMS/InsertMLNoWithSelectedBobin",
        type: "Post",
        dataType: "json",
        data: params,
        success: function (data) {

            console.log(data);
            if (data.result == true) {
                $('.popupDialogComposite_RollNormal_Container').dialog('close');
                SuccessAlert("Success");
                var id = data.ds1.wmtid;
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('addRowData', id, data.ds1, 'first');
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").setRowData(id, false, { background: "#d0e9c6" });
            } else {
                ErrorAlert(data.message);
            }
        },
    });
    //$.ajax({
    //    url: `/TIMS/InsertMLNoWithSelectedBobin?id_actual=${actualId}&name=${actualProcessCode}&style_no=${actualProductCode}&bb_no=${bobbinNumber}&staff_id=${staff_id}&psid=${psid}`, // Hằng code
    //})
    //    .done(function (data) {
    //        $('.popupDialogComposite_RollNormal_Container').dialog('close');
    //        if (data.result == false) {
    //            ErrorAlert(data.message);
    //        } else {
    //            SuccessAlert("Success");
    //            var id = data.ds1.wmtid;
    //            $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('addRowData', id, data.ds1, 'first');
    //            $("#dialogCompositeRollNormal_MateritalCompositeGrid").setRowData(id, false, { background: "#d0e9c6" });
    //        }
    //    })
    //    .fail(function () {
    //        return;
    //    });
});

//COMPOSITE POPUP
function CompositePopup(e) {

    $("#staff_id").val($(e).data("userid"));
    //$("#result_oqc").html("");
    //roll:100
    //EA:200
    var type = $("#type_roll").val();
    var process = $("#dialogCompositeProcess").val();
    grid3.clearGridData();
    $("#cp_lot").val("");
    if (process != "OQC") {
        $('.popupDialogComposite_RollNormal').dialog('open');
        call_table1($(e).data("psid"));
        $("#psid").val($(e).data("psid"));
        var type = $("#type_roll").val();
        var check_author = $("#authoryty_readonly").val();
        if (check_author != "read") {

            if (type == "200")//dang EA
            {
                $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Stop_Copy"]);
                $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Finish"]);
                $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Cancel"]);
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('showCol', ["Cancel"]);
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["Delete"]);
                $("#scan_EA").removeClass("hidden");
                $("#save_bb").addClass("hidden");
                $("#dialogCompositeRollNormalContainer_CreateBtn").addClass("hidden");
                $("#pp_sta").removeClass("hidden");
            }
            else {
                $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Finish"]);
                $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('showCol', ["Cancel"]);
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["Cancel"]);
                $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('showCol', ["Delete"]);
                $("#pp_sta").addClass("hidden");
                $("#scan_EA").addClass("hidden");
                $("#save_bb").removeClass("hidden");
                $("#dialogCompositeRollNormalContainer_CreateBtn").removeClass("hidden");
            }
        }//dang ROLL
        else {
            $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Stop_Copy"]);
            $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Finish"]);
            $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["Cancel"]);
            $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["Cancel"]);
            $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["Delete"]);
            $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["merge"]);
            $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('hideCol', ["ADD"]);
            $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('hideCol', ["QC"]);
            $("#dialogCompositeRollNormalContainer_CreateBtn").addClass("hidden");
            $("#pp_sta").addClass("hidden");
            $("#scan_EA").addClass("hidden");
            $("#save_bb").addClass("hidden");
        }
    } else {
        //view oqc
        call_tableoqc();
        $('.dialog_composite4_OQC').dialog('open');
        $("#tb_mt_cd_oqc").jqGrid('hideCol', ["qc"]);
    }
}
function call_table1(value) {
    $.get("/TIMS/GetListMaterialWebAuto?id_actual=" + $("#id_actual").val() + "&psid=" + value, function (data) {

        $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
    });
}

$(".popupDialogComposite_RollNormal").dialog({
    width: '70%',
    height: 'auto',
    maxWidth: 1000,
    maxHeight: 1000,
    minWidth: '70%',
    minHeight: 500,
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
        $('#dialogCompositeRollNormal_MateritalCompositeGrid').jqGrid({
            url: ``,//Hằng code
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
                {
                    label: 'Tgian Tạo', name: 'date', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { label: 'ML No', name: 'mt_cd', width: 300, sortable: true, },
                {
                    label: 'Quantity Real', name: 'gr_qty1', width: 81, align: 'right'
                },
                { label: 'Quantity', name: 'gr_qty', width: 81, formatter: 'integer', hidden: true },
                { label: 'Quantity', name: 'real_qty', width: 81, formatter: 'integer', align: 'right' },
                { label: 'Container', name: 'bb_no', width: 300 },
                { label: 'Delete', name: 'Delete', width: 80, align: 'center', formatter: xoa_qr_cha },
                { label: 'Cancel', name: 'Cancel', width: 80, align: 'center', formatter: Cancel_EA },
                { label: 'Merge', name: 'merge', width: 80, align: 'center', formatter: Merge_OK },
                { label: '', name: 'ADD', align: 'center', width: 80, formatter: OK_NG },
            ],
            gridComplete: function () {
                var rows = $("#dialogCompositeRollNormal_MateritalCompositeGrid").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var gr_qty = $("#dialogCompositeRollNormal_MateritalCompositeGrid").getCell(rows[i], "sl_tru_ng");
                    if (gr_qty > 0) {
                        $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                    }
                }
            },
            onCellSelect: function (rowid) {
                var lastSel = "";
                if (rowid && rowid !== lastSel) {
                    grid2.restoreRow(lastSel);
                    lastSel = rowid;
                }
                grid2.editRow(rowid, true);
            },
            pager: '#dialogCompositeRollNormal_MateritalCompositePage',
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 150,
            width: null,
            loadonce: false,
            caption: 'Composite',
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
            gridComplete: function () {
                var rows = $('#dialogCompositeRollNormal_MateritalCompositeGrid').getDataIDs();
            },
            loadComplete: function () {
                var rows = $('#dialogCompositeRollNormal_MateritalCompositeGrid').getDataIDs();
            },
            onSelectRow: function () {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#dialogCompositeRollNormal_MateritalCompositeGrid").jqGrid("getGridParam", 'selrow');
                var rowData = $("#dialogCompositeRollNormal_MateritalCompositeGrid").getRowData(selectedRowId);
                $("#cp_lot").val(rowData.mt_cd);
                $("#dialogCompositeRollNormal_MateritalGrid").setGridParam({ url: "/TIMS/GetListMaterialMappingwTims?" + "mt_cd=" + rowData.mt_cd, datatype: "json" }).trigger("reloadGrid");
            },
        });

        function xoa_qr_cha(cellValue, options, rowdata, action) {
            if (rowdata.real_qty == 0 || rowdata.real_qty == undefined) {
                var id = rowdata.wmtid;
                html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + id + '"onclick="Xoa_qr_con(this);">Delete</button>';
                return html;
            }
            return "";
        }
        function Cancel_EA(cellValue, options, rowdata, action) {
            var id = rowdata.wmtid;
            html = '<button  class="btn btn-sm btn-danger button-srh" data-id="' + id + '"onclick="Cancel_EACon(this);">Cancel</button>';
            return html;
        }

        $('#dialogCompositeRollNormal_MateritalGrid').jqGrid({
            url: ``,//Hằng code
            mtype: 'GET',
            datatype: 'json',
            colModel: [
                { key: true, label: 'wmmid', name: 'wmmid', hidden: true },
                {
                    label: 'Tgian Mapping', name: 'mapping_dt', width: 150, align: 'center', formatter: "date",
                    formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
                },
                { label: '', name: 'Finish', width: 40, formatter: finish_vl, align: 'center' },
                { label: 'Dừng kế thừa', name: 'Stop_Copy', width: 120, formatter: stop_cp, align: 'center' },

                { label: '', name: 'Cancel', width: 50, formatter: cancel, align: 'center' },
                { label: 'Using', name: 'use_yn', width: 40, align: 'center' },
                {
                    label: 'Check QC', name: 'QC', width: 80, align: 'center', formatter: check_qc
                },
                { label: 'Quantity', name: 'real_qty', width: 50, formatter: 'integer', align: 'right' },
                { label: 'Real Quantity', name: 'gr_qty', width: 50, formatter: 'integer', align: 'right' },
                { label: 'mt_lot', name: 'mt_lot', hidden: true },
                { label: 'ML No', name: 'mt_cd', width: 300, sortable: true },
                { label: 'MT NO', name: 'mt_no', width: 200, hidden: true },
                { label: 'sts_share', name: 'sts_share', hidden: true },
                { label: 'Container', name: 'bb_no', width: 300, formatter: change_bobinEA },
            ],
            pager: '#dialogCompositeRollNormal_MateritalPage',
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            rownumbers: true,
            autowidth: true,
            shrinkToFit: false,
            viewrecords: true,
            height: 300,
            width: null,
            loadonce: false,
            caption: 'Material',
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
            gridComplete: function () {
                var rows = $("#dialogCompositeRollNormal_MateritalGrid").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var use_yn = $("#dialogCompositeRollNormal_MateritalGrid").getCell(rows[i], "use_yn");
                    if (use_yn == "N") {
                        $("#dialogCompositeRollNormal_MateritalGrid").jqGrid('setRowData', rows[i], false, { background: '#f39c12' });
                    }
                }
            },
            loadComplete: function () {
                var rows = $('#dialogCompositeRollNormal_MateritalGrid').getDataIDs();
            },
            onSelectRow: function () {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            },
        });
    },
    close: function () {
    }
});
function Merge_OK(cellValue, options, rowdata, action) {
    $("#trang_thai").val("");
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-info button-srh" data-mt_cd="' + rowdata.mt_cd + '" data-id="' + id + '"onclick="MergeCon(this);">Merge</button>';
    return html;
}
function Merge_OK_DV(cellValue, options, rowdata, action) {
    //$("#trang_thai").val("DV");
    $("#trang_thai").val("");// devide sử dụng chung hàm với merge ok luôn
    var id = rowdata.wmtid;
    html = '<button  class="btn btn-sm btn-info button-srh" data-mt_cd="' + rowdata.mt_cd + '" data-id="' + id + '"onclick="MergeCon(this);">Merge</button>';
    return html;
}
//#region  devide
$("#pp_sta").click(function () {
    $('.popupDialogComposite_RollNormal').dialog('close');
    $("#tb_mt_cd_sta").clearGridData();
    $('.dialog_composite3').dialog('open');
    $.get("/TIMS/GetListMaterialWebAuto?id_actual=" + $("#id_actual").val() + "&psid=" + $("#psid").val(), function (data) {
        $("#tb_mt_lot_sta").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: data }).trigger("reloadGrid");
    });
});
$("#back_sta").click(function () {
    $('.dialog_composite3').dialog('close');
    $('.popupDialogComposite_RollNormal').dialog('open');
});
$grid = $("#tb_mt_lot_sta").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { name: "RD", width: 60, align: "center", label: "", resizable: false, title: false, formatter: bntCellValue },
            { key: true, label: 'wmtid', name: 'wmtid', align: 'center', hidden: true },
            { label: 'Container', name: 'bb_no', width: 240, },
            { label: 'Quantity reality', name: 'gr_qty', width: 81, formatter: 'integer', align: 'right' },
            { label: 'ML No', name: 'mt_cd', width: 240, sortable: true, },
        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
        },
        onSelectRow: function (id, rowid, status, e) {
            var selectedRowId = $("#tb_mt_lot_sta").jqGrid("getGridParam", 'selrow');
            row_id = $("#tb_mt_lot_sta").getRowData(selectedRowId);
            $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaTims?" + "mt_cd=" + row_id.mt_cd, datatype: "json" }).trigger("reloadGrid");
            $('#mt_code_bb2').val(row_id.mt_cd);
            $('#bb_no_dv').val(row_id.bb_no);
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
function bntCellValue(cellValue, op1tions, rowdata, action) {
    if (rowdata.gr_qty == 0) {
        var mt_cd = rowdata.mt_cd;
        var html = `<button class="btn btn-xs btn-primary" data-mt_cd="${mt_cd}" onclick="destroy(this);">Redo</button>&nbsp;&nbsp;&nbsp;&nbsp;`;
        return html;
    }
    return "";
};
function destroy(e) {
    $.ajax({
        url: '/TIMS/DestroyDevideTims?mt_cd=' + $(e).data("mt_cd"),
        type: "get",
        dataType: "json",
        success: function (data) {

            if (data.result == true) {
                SuccessAlert(data.message);
                var id = data.wmtid;
                var rowData = $('#tb_mt_lot_sta').jqGrid('getRowData', id);
                rowData.gr_qty = data.gr_qty;
                $("#tb_mt_lot_sta").setRowData(id, rowData, { background: "#d0e9c6" });

            } else {
                //alert(data.kq);
                ErrorAlert(data.message);
            }
        },
    });
}
var edit_dv = true;
$("#tb_mt_cd_sta").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
        colModel: [
            { key: true, label: 'wmtid', name: 'wmtid', hidden: true },
            { label: 'mt_lot', name: 'mt_lot', hidden: true },
            { label: 'mtcdparent', name: 'material_code_parent', hidden: true },
            { label: 'mt_cd', name: 'mt_cd', hidden: true },
            { label: 'ML No', name: 'material_code', width: 350, sortable: true },
            { label: 'MT NO', name: 'mt_no', width: 150 },
            { label: 'Quantity', name: 'real_qty', align: 'right', formatter: 'integer', width: 100, editable: false, edittype: "number", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, },
            { label: 'Quantity Real', name: 'gr_qty', formatter: 'integer', align: 'right', width: 100 },
            { label: 'Container', name: 'bb_no', width: 240, align: 'center', formatter: change_bobin },
            { label: 'Merge', name: 'merge', width: 80, align: 'center', formatter: Merge_OK_DV },
        ],
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
        loadonce: false,
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
    var id = rowdata.wmtid;
    var name = "Add Container";
    if (cellValue != null && cellValue != "") { name = cellValue }

    var html = '<button style="color: dodgerblue;border: none;background: none;" data-sts="DV" data-id="' + id + '" onclick="change_container(this);">' + name + '</button>';
    return html;
}

function change_bobinEA(cellValue, options, rowdata, action) {
    var id = rowdata.wmmid;
    var html = "";
    var type = $("#type_roll").val();
    if (cellValue != null && type == "200") {
        html = '<button style="color: dodgerblue;border: none;background: none; " data-sts="EA" data-id="' + id + '"   onclick="change_container(this);">' + cellValue + '</button>';
    } else {
        html = cellValue;
    }
    return html;
}
function change_container(e) {
    $('.diologbobindv').dialog('open');
    $("#s_bb_nodv").val("");
    get_ds_bb_dv();
    var id = $(e).data("id");
    var sts = $(e).data("sts");
    $("#id_dv").val(id);
    $("#sts_change_bb").val(sts);
}
$("#device").click(function () {
    // console.log("chia tims ssssss")
    if (parseInt($('#so_lgr').val()) < 1) {
        alert("Quantity must > 0 ");
        return false;
    }
    if ($('#bb_no_dv').val() == "") {
        alert("Please choose Bobin need devide");
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
    if ((sobichia / sochia) > 400) {
        alert("Số lượng hộp nhỏ được chia ra không được quá 400");
        return false;
    }
    else {
        $.get("/TIMS/DevideStaTims?bb_no=" + $('#bb_no_dv').val() + "&number_dv=" + $("#number_dv").val() + "&psid=" + $("#psid").val(), function (data) {
            $('.diologbobin2').dialog('close');
            if (data.result == true) {
                SuccessAlert("Success");
                var id_dv = $("#id_dv").val();
                var rowData = $('#tb_mt_lot_sta').jqGrid('getRowData', id_dv);
                rowData.gr_qty = 0;
                $('#tb_mt_lot_sta').jqGrid('setRowData', id_dv, rowData);
                $("#tb_mt_lot_sta").setRowData(id_dv, false, { background: "#d0e9c6" });
                $("#tb_mt_cd_sta").setGridParam({ url: "/ActualWO/GetListMappingStaTims?" + "mt_cd=" + $('#mt_code_bb2').val(), datatype: "json" }).trigger("reloadGrid");
            } else {
                ErrorAlert(data.message);
            }
        });
    }
});
//#endregion
//#endregion
//#region OQC

//#endregion
//
//#region check QC
$('input#qc_ok_qty').keyup(function () {
    $("#qc_ng_qty").val("0");
    $("#qc_remain").val("0");

    var valueOK = parseInt(this.value);
    if (valueOK == 0) {
        var valueMax = (document.getElementById('qc_gr_qty').value == "") ? 0 : document.getElementById('qc_gr_qty').value;


        $("#qc_ok_qty").val(0);
        $("#qc_ng_qty").val(valueMax);
        return false;
    }
    var maxQuantity = $("#qc_gr_qty").val();
    if (parseInt(valueOK) > parseInt(maxQuantity)) {
        $("#qc_ok_qty").val(maxQuantity);
        return false;
    }
    var Valueng_qty = parseInt(maxQuantity) - parseInt(valueOK);
    if (Valueng_qty < 0) {
        var QuantityNg = parseInt(maxQuantity) - parseInt(valueOK);
        $("#qc_ng_qty").val(QuantityNg);
        return false;
    }
    $("#qc_ng_qty").val(Valueng_qty);
});

$('input#qc_ng_qty').keyup(function () {
    var valueNG = parseInt(this.value);
    if (valueNG < 0) {
        $("#qc_ng_qty").val("0");
        $("#qc_ok_qty").val("0");
        return false;
    }
    var valueMax = (document.getElementById('qc_gr_qty').value == "") ? 0 : document.getElementById('qc_gr_qty').value;
    var valueOK = (document.getElementById('qc_ok_qty').value == "") ? 0 : document.getElementById('qc_ok_qty').value;


    var Valueremain = parseInt(valueMax) - (parseInt(valueOK) + parseInt(valueNG));
    if (Valueremain < 0) {
        var QuantityNg = parseInt(valueMax) - parseInt(valueOK);
        $("#qc_ng_qty").val(QuantityNg);
        $("#qc_remain").val(0);
        return false;
    }
    $("#qc_remain").val(Valueremain);
});

function check_qcEA(cellValue, options, rowdata, action) {
    if (rowdata.gr_qty > 0) { }
    var item_vcd = $("#check_qc").val();

    var html = '<button style="color: dodgerblue;border: none;background: none;"  data-id="' + rowdata.wmtid + '"  onclick="item_qc_check(this);">' + item_vcd + '</button>';
    return html;
};
function check_qc(cellValue, options, rowdata, action) {
    if (rowdata.gr_qty > 0) { }
    //var item_vcd = $("#check_qc").val();
    var item_vcd = "TI000001A";
    var html = '<button style="color: dodgerblue;border: none;background: none;"  data-id="' + rowdata.wmmid + '"  onclick="item_qc_check(this);">' + item_vcd + '</button>';
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
    },
});
$("#p_SaveQcfacline").on("click", function () {
    var i, selRowIds = $('#list_facline_qc_value').jqGrid("getGridParam", "selarrrow"), n, rowData, check_qty = "";

    var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
    row_id = $("#list_facline_qc").getRowData(selectedRowId);

    var fqno = row_id.fqno;
    var fruits = [];
    for (i = 0, n = selRowIds.length; i < n; i++) {
        id = selRowIds[i];
        check_qty = getCellValue(selRowIds[i], 'check_qty');
        if (check_qty == undefined) {
            var ret = $("#list_facline_qc_value").jqGrid('getRowData', selRowIds[i]);
            var giatri = ret.check_qty;
            fruits.push(giatri);
        } else {
            fruits.push(check_qty);
        }
    }

    $.ajax({
        type: "get",
        dataType: 'json',
        url: '/TIMS/ModifyFaclineQcDetail?' + "id=" + selRowIds + "&" + "check_qty=" + fruits + "&" + "fqno=" + fqno,

        success: function (data) {
            if (data.result) {
                SuccessAlert("Success");
                $.each(data.list, function (key, item) {
                    var id = item.fqhno;
                    $("#list_facline_qc_value").setRowData(id, item, { background: "#d0e9c6" });
                    var id = item.fqno;
                    $("#list_facline_qc").setRowData(id, item, { background: "#d0e9c6" });
                });
            }
            else {
                ErrorAlert(data.message);
            }
        },
        error: function (data) {
            ErrorAlert("QR Barcode no exist!!!");
        },
    });
});
function item_qc_check(e) {
    //ktra xem total có > 0 không, nếu == 0 thì ko view pp check qc
    var total_qty = $(e).data("gr_qty");
    if (total_qty == 0) {
        alert("There is no quantity to Check QC");
        return false;
    }
    $('.dialog_qc_code_pp').dialog('open');

    var id = $(e).data("id")
    var rowData = grid3.jqGrid('getRowData', id);


    //$("#qc_gr_qty").val(rowData.real_qty);
    //$("#qc_remain").val("0");
    //$("#qc_ng_qty").val("0");
    $("#qc_mt_cd").val(rowData.mt_cd);
    var item_vcd = $("#check_qc").val();
    //table_facline_qc(item_vcd, rowData.mt_cd);
    //table_facline_qc_value();
    $("input#qc_ok_qty").attr({
        "max": rowData.gr_qty,        // substitute your own
    });
    $.ajax({
        url: '/TIMS/Getfacline_qc',
        type: "get",
        dataType: "json",
        data: {
            mt_cd: rowData.mt_cd,
            item_vcd: "TI000001A",
            mt_lot: $("#cp_lot").val(),

        },
        success: function (response) {
            if (response.result) {
                // SuccessAlert(data.message);

                var html = '';
                html += '<table class ="qc"><tr><th class="con">Work Date</th><th class="con">Check Qty</th><th class="con">Ok Qty</th><th class="con">NG Qty</th><th class="con">Remain Qty</th></tr>';
                html += '<tr class="con1">'
                //$.each(data, function (key, item) {
                //    html += ' <td>'+item.fq_no+'</td>';
                //    html += ' <td>' + item.work_dt+'</td>';
                //    html += ' <td>' + item.check_qty+'</td>';
                //    html += ' <td>' + item.ok_qty+'</td>';
                //    html += ' <td>' + item.defect_qty+'</td>';

                //});


                html += ' <td>' + response.data.work_dt + '</td>';
                html += ' <td>' + response.data.check_qty + '</td>';
                html += ' <td>' + response.data.ok_qty + '</td>';
                html += ' <td>' + response.data.ng_qty + '</td>';
                html += ' <td>' + response.data.remain_qty + '</td>';


                html += "</tr>"
                html += '</table>';
                $("#viewTableQc").html(html);

                $("#qc_gr_qty").val(response.data.check_qty);
                $("#qc_ok_qty").val(response.data.ok_qty);
                $("#qc_ng_qty").val(response.data.ng_qty);
                $("#qc_remain").val(response.data.remain_qty);


                return;
            }
            else {
                var html = '';
                html += '<table class ="qc"><tr><th class="con">Work Date</th><th class="con">Check Qty</th><th class="con">Ok Qty</th><th class="con">NG Qty</th><th class="con">Remain Qty</th></tr>';
                html += '<tr class="con1">'
                html += ' <td></td>';
                html += ' <td></td>';
                html += ' <td></td>';
                html += ' <td></td>';
                html += ' <td></td>';
                html += "</tr>"
                html += '</table>';
                $("#viewTableQc").html(html);

                $("#viewTableQc").html(html);



                $("#qc_gr_qty").val(rowData.gr_qty);
                $("#qc_ok_qty").val("0");
                $("#qc_remain").val("0");
                $("#qc_ng_qty").val("0");
            }
        },
        error: function () {
            $("#qc_ok_qty").val("0");
            $("#qc_remain").val("0");
            $("#qc_ng_qty").val("0");
        }
    });

}
function table_facline_qc(item_vcd, mt_cd) {
    $("#list_facline_qc").setGridParam({ url: "/TIMS/Getfacline_qc?" + "mt_cd=" + mt_cd + "&mt_lot=" + $("#cp_lot").val() + "&item_vcd=" + item_vcd, datatype: "json" }).trigger("reloadGrid");
    $("#list_facline_qc").jqGrid
        ({
            url: "/TIMS/Getfacline_qc?mt_cd=" + mt_cd + "&item_vcd=" + item_vcd,
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqno', name: 'fqno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'FQ NO', name: 'fq_no', sortable: true, width: '120', align: 'center' },
                { key: false, label: 'Work Date', name: 'work_dt', sortable: true, width: '150', align: 'center' },
                { key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', formatter: 'integer' },
                { key: false, label: 'Ok Qty', name: 'ok_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
                //{ key: false, label: 'Defect Qty', name: 'defect_qty', sortable: true, align: 'right', width: '80', formatter: 'integer' },
                {
                    label: 'Defect Qty', name: 'defect_qty', width: 81, align: 'right', formatter: 'integer', editable: true, edittype: "text", editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                    formatter: 'integer', editoptions: {
                        dataInit: function (element) {
                            $(element).keypress(function (e) {
                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                    return false;
                                }
                                defect_qty = parseInt(this.value);

                            });
                            $(element).keyup(function (e) {
                                defect_qty = parseInt(this.value);

                            });
                            $(element).keydown(function (e) {
                                var selectedRowId = $("#list_facline_qc").jqGrid("getGridParam", 'selrow');
                                var row_id = $("#list_facline_qc").getRowData(selectedRowId);

                                defect_qty = parseInt(this.value);
                                if (e.which == 13) {

                                    var chuoi = $(e).data("fqno") + "_defect_qty";
                                    var gr = $("#" + chuoi + "").val();
                                    var giatri = gr;
                                    if ((giatri == "") || (giatri == undefined)) {
                                        giatri = defect_qty;
                                    }

                                    $.ajax({
                                        url: '/Tims/update_Ng_qty?fqno=' + row_id.fqno + "&defect_qty=" + giatri,
                                        type: "get",
                                        dataType: "json",
                                        success: function (data) {
                                            if (data.result) {
                                                SuccessAlert(data.message);
                                                var id = data.data[0].fqno;
                                                $("#list_facline_qc").setRowData(id, data.data[0], { background: "#28a745", color: "#fff" });


                                            }
                                            else {
                                                alert('error');
                                            }


                                        },

                                    });
                                }
                                else {
                                    defect_qty = parseInt(this.value);
                                }
                            });
                        }
                    },
                },
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


                if (rowid && rowid !== lastSelection) {
                    jQuery(this).restoreRow(lastSelection);
                    lastSelection = rowid;
                }
                jQuery(this).editRow(rowid, true);
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
var lastSelection;
function editRow(id) {
    if (id && id !== lastSelection) {
        var grid = $("#list_facline_qc");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSelection = id;
    }
}
function table_facline_qc_value() {
    $("#list_facline_qc_value").jqGrid
        ({
            url: "/TIMS/Getfacline_qc_value",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'fqhno', name: 'fqhno', width: 80, align: 'center', hidden: true },
                { key: false, label: 'Subject', name: 'check_subject', sortable: true, width: '150', align: 'left' },
                { key: false, label: 'Check Value', name: 'check_value', sortable: true, width: '110', align: 'left' },
                {
                    key: false, label: 'Check Qty', name: 'check_qty', sortable: true, align: 'right', width: '100', editable: true,
                    editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true },
                    formatter: 'integer', editoptions: {
                        dataInit: function (element) {
                            $(element).keypress(function (e) {
                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                    return false;
                                }
                            });
                        }
                    },
                },
            ],

            onCellSelect: editRow_facline,
            onSelectRow: function (rowid, selected, status, e) {
                $("#p_SaveQcfacline").attr("disabled", false);
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
        })
}
function getViewItemQC() {
    $.get("/TIMS/PartialView_View_QC_WEB?item_vcd=" + $("#check_qc").val(), function (html) {
        $("#view_qc").html(html);
    });
}

$("#save_qc_code").on("click", function () {
    var sluong_max = 0;

    var okqty = $('#qc_ok_qty').val();
    var check_qty = $('#qc_gr_qty').val();
    if (check_qty == 0) {
        $('#qc_gr_qty').focus();
        return
    }
    $.ajax({
        url: "/TIMS/Insert_w_product_qc",
        type: "get",
        dataType: "json",
        data: {
            'item_vcd': $('#qc_item_vcd').val(),
            'check_qty': $('#qc_gr_qty').val(),
            'ok_qty': $('#qc_ok_qty').val(),
            'mt_cd': $('#qc_mt_cd').val(),
            'date': $('#qc_date').val(),
            'mt_lot': $("#cp_lot").val(),
            'check_qty_error': $('#qc_ng_qty').val(),
            'remain': $('#qc_remain').val(),
            'psid': $('#psid').val(),
        }
    })
        .done(function (data) {
            if (data.result) {
                if (data.remain > 0) {
                    alert("Còn số lượng chưa kiểm tra vui lòng chọn lại đồ đựng mới!");
                }
                else {
                    SuccessAlert("Success");
                }

                document.getElementById("qc_gr_qty").value = "0";
                document.getElementById("qc_ok_qty").value = "0";
                document.getElementById("qc_ng_qty").value = "0";
                document.getElementById("qc_remain").value = "0";


                $(".testnumber").val("0");
                var value = $("#psid").val();
                call_table1(value);

                grid3.setGridParam({ url: "/TIMS/GetListMaterialMappingwTims?" + "mt_cd=" + $("#cp_lot").val(), datatype: "json" }).trigger("reloadGrid");


                $.ajax({
                    url: '/TIMS/Getfacline_qc',
                    type: "get",
                    dataType: "json",
                    data: {
                        mt_cd: $('#qc_mt_cd').val(),
                        item_vcd: "TI000001A",
                        mt_lot: $("#cp_lot").val(),

                    },
                    success: function (response) {
                        if (response.result) {

                            var html = '';
                            html += '<table class ="qc"><tr><th class="con">Work Date</th><th class="con">Check Qty</th><th class="con">Ok Qty</th><th class="con">NG Qty</th><th class="con">Remain Qty</th></tr>';
                            html += '<tr class="con1">'

                            html += ' <td>' + response.data.work_dt + '</td>';
                            html += ' <td>' + response.data.check_qty + '</td>';
                            html += ' <td>' + response.data.ok_qty + '</td>';
                            html += ' <td>' + response.data.ng_qty + '</td>';
                            html += ' <td>' + response.data.remain_qty + '</td>';


                            html += "</tr>"
                            html += '</table>';
                            $("#viewTableQc").html(html);

                            $("#qc_gr_qty").val(response.data.check_qty);
                            $("#qc_ok_qty").val(response.data.ok_qty);
                            $("#qc_ng_qty").val(response.data.ng_qty);
                            $("#qc_remain").val(response.data.remain_qty);


                            return false;
                        }
                        else {
                            var html = '';
                            html += '<table class ="qc"><tr><th class="con">Work Date</th><th class="con">Check Qty</th><th class="con">Ok Qty</th><th class="con">NG Qty</th><th class="con">Remain Qty</th></tr>';
                            html += '<tr class="con1">'
                            html += ' <td></td>';
                            html += ' <td></td>';
                            html += ' <td></td>';
                            html += ' <td></td>';
                            html += ' <td></td>';
                            html += "</tr>"
                            html += '</table>';
                            $("#viewTableQc").html(html);

                            $("#viewTableQc").html(html);



                            $("#qc_gr_qty").val(rowData.gr_qty);
                            $("#qc_ok_qty").val("0");
                            $("#qc_remain").val("0");
                            $("#qc_ng_qty").val("0");
                            return false;
                        }
                    },
                    error: function () {
                        $("#qc_ok_qty").val("0");
                        $("#qc_remain").val("0");
                        $("#qc_ng_qty").val("0");
                        return false;
                    }
                });
            }
            else {
                ErrorAlert(data.message);
            }
        })
        .fail(function () {
            //return;
        });
});
function editRow_facline(id) {
    if (id && id !== lastSel) {
        var grid = $("#list_facline_qc_value");
        grid.jqGrid('editRow', id, { keys: true, focusField: 2 });
        lastSel = id;
    }
}
//#endregion
//VARIABLES
var grid = $('#list');
var grid2 = $('#dialogCompositeRollNormal_MateritalCompositeGrid');
var grid3 = $('#dialogCompositeRollNormal_MateritalGrid');
var actualId = '0';
var deleteType = '';
var actualRollType = '';
var actualProcessCode = '';
var actualProductCode = '';
var actualDate = '';
//DOCUMENT READY
$(document).ready(function (e) {
    GetTIMSProcesses();
    GetTIMSRolls();
    Grid();
    $("#sDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeYear: true
    });
    $("#eDate").datepicker({
        dateFormat: 'yy-mm-dd',
        changeYear: true
    });
});
$(".dialog_composite3").dialog({
    width: '70%',
    height: 800,
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
$(".dialog_composite4_OQC").dialog({
    width: '70%',
    height: 500,
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
//POPUP_COMPOSITE

//MATERIAL POPUP
$('.dialogMaterial').on('click', function () {
    $('.popupDialogMaterial').dialog('open');
});
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
$("#create_accept").click(function () {
    switch ($("#sts_creat").val()) {
        case "create_wk":
            $.ajax({
                url: "/ActualWO/Createprocessstaff_duplicate",
                type: "get",
                dataType: "json",
                data: {
                    staff_id: $('#dialogWorkerMapping_cWorkerId').val(),
                    id_actual: $('#id_actual').val(),
                    use_yn: $('#dialogWorkerMapping_cIsUsed').val(),
                    id_update: $('#id_update').val(),
                    start: $('#start_accept').val(),
                    end: $('#end_accept').val(),
                },
                success: function (data) {
                    $("#list_staff").jqGrid('addRowData', data.psid, data, 'first');
                    $("#list_staff").setRowData(data.psid, false, { background: "#d0e9c6" });
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
            $.get("/TIMS/FinishBack?wmmid=" + id, function (data) {
                if (data.result == true) {
                    SuccessAlert("");
                    var rowData = grid3.jqGrid('getRowData', id);
                    rowData.use_yn = data.use_yn;
                    grid3.jqGrid('setRowData', id, rowData);
                    grid3.setRowData(id, false, { background: "#d0e9c6" });
                } else {
                    ErrorAlert(data.message);
                }
            });
            $('#dialogAccept').dialog('close');
            break;
    }
});
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
function sl_tru_ng(cellValue, options, rowdata, action) {
    var html = parseInt(cellValue == null ? 0 : cellValue) + parseInt(rowdata.gr_qty);
    return html;
};
//#endregion

function PercentagePrimary(cellValue, options, rowdata, action) {
    if (rowdata.target == 0 || rowdata.totalTarget == undefined || rowdata.actual == undefined) {
        return 0.00 + ` %`;
    }
    return (((rowdata.actual / rowdata.target)) / rowdata.process_count * 100).toFixed(2) + ` %`;
}

function Percentage(cellValue, options, rowdata, action) {
    if (rowdata.target == 0 || rowdata.target == undefined || rowdata.ActualQty == undefined) {
        return 0.00 + ` %`;
    }
    return ((rowdata.ActualQty / rowdata.target) * 100).toFixed(2) + ` %`;
}
function ProcessCalculating(cellValue, options, rowdata, action) {
    var html = rowdata.process_count + "/" + rowdata.count_pr_w;
    return html;
}
function FinishPO(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-po="${rowdata.at_no}" onclick="FinishPOOnClick(this)">Finish</button>`;
}
var poToFinish = undefined;
function FinishPOOnClick(e) {
    poToFinish = undefined;
    poToFinish = e.dataset.po;
    $('#dialog_Finsh').dialog('open');
}

$('#closefinish').click(function () {
    $('#dialog_Finsh').dialog('close');
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
        url: `/TIMS/FinishPO?po=${poToFinish}`,
        type: "post",
        dataType: "json",
        success: function (response) {
            if (response.result == true) {
                $('#list').clearGridData();
                $("#list_primary").jqGrid('delRowData', response.id);
                SuccessAlert(``);
                document.getElementById("form1").reset();
                document.getElementById("form2").reset();
                $('#list_finish').jqGrid('setGridParam', { search: true });
                var pdata = $('#list_finish').jqGrid('getGridParam', 'postData');
                getData_Finished(pdata);
                return;
            }
            else {
                ErrorAlert(`System error.`);
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
        url: `/TIMS/GetFinishPO`,
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

            { label: 'Description', name: 'remark', width: 80 },
            { label: 'Target', name: 'target', width: 100, align: 'right', formatter: 'integer' },

            { label: 'Complete (%)', name: '', width: 100, align: 'center', formatter: PercentagePrimary },
            { label: 'Processing', name: '', width: 150, align: 'center', formatter: ProcessCalculating },
            { label: 'Redo', name: 'redo', width: 150, align: 'center', formatter: Redo },
        ],
        onSelectRow: function (rowid, selected, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#list_finish").jqGrid("getGridParam", 'selrow');
            var row_id = $("#list_finish").getRowData(selectedRowId);
            actualProductCode = row_id.product;
            $("#at_no").val(row_id.at_no);

            $("#id_actualpr").val(row_id.id_actualpr);
            $("#oqc_name").val(row_id.product);
            $("#dialogCompositeProduct").val(row_id.product);
            $("#mTarget").val(row_id.target);
            $("#mRemark").val(row_id.remark);
            $("#mProduct").val(row_id.product);
            $("#qc_name").val(row_id.product);

            $('#list').clearGridData();
            $('#list').jqGrid('setGridParam', { search: true });
            var pdata = $('#list').jqGrid('getGridParam', 'postData');
            getData(pdata);

            $('#list_SX').clearGridData();
            $('#list_SX').jqGrid('setGridParam', { search: true });
            var pdata = $('#list_SX').jqGrid('getGridParam', 'postData');
            getData_SX(pdata);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");
            $("#modifyBtn").attr("disabled", false);
            $("#deleteBtn").attr("disabled", false);
        },
        pager: jQuery('#jqGrid_finishPager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 300,
        width: $(".box-body1").width() - 5,
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
function Redo(cellValue, options, rowdata, action) {
    var html = "";
    html = `<button class="btn btn-xs btn-primary"  data-id_actualpr="${rowdata.id_actualpr}"  onclick="redo_con(this);">REDO</button>`;
    return html;
}
function redo_con(e) {
    $.get("/ActualWO/Redo_Finish?id_actualpr=" + $(e).data("id_actualpr"), function (data) {
        if (data.result == true) {
            var id_actualpr = parseInt($(e).data("id_actualpr"));
            $("#list_finish").jqGrid('delRowData', id_actualpr);
            SuccessAlert(``);
            $('#list_primary').jqGrid('setGridParam', { search: true });
            var pdata = $('#list_primary').jqGrid('getGridParam', 'postData');
            getData_primary(pdata);
        }
        else {
            ErrorAlert(data.message);
        }
    });
}
//function ProcessCalculating(cellValue, options, rowdata, action) {
//    var po = rowdata.at_no;
//    var html = ``;
//    $.ajax({
//        url: `/ActualWO/ProcessCalculating?po=${po}`,
//        type: "GET",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: false,
//        success: function (response) {
//            html += `${response} / ${rowdata.process_count}`;
//        },
//        error: function () {
//        }
//    });
//    return html;
//}


//view ViewDetailDatetime
function ViewDetailDatetime(cellValue, options, rowdata, action) {
    var html = "";
    html = '<button class="btn btn-xs btn-primary"  data-id_actual="' + rowdata.Id + '"  onclick="ClickViewDetailDatetime(this);">Detail</button>';
    return html;
}


function ClickViewDetailDatetime(e) {
    var id_actual = $(e).data("id_actual")
    var opt = {
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 950,
        title: 'Details'
    };

    var theDialog = $(".popup-dialog.dialog_Viewdetaildatetime").dialog(opt);
    theDialog.dialog("open");
   /* $('.popup-dialog.dialog_Viewdetaildatetime').dialog('open');*/


    $.get("/TIMS/PartialView_dialog_Viewdetaildatetime?id_actual=" + id_actual, function (html) {
        $("#PartialView_dialog_Viewdetaildatetime").html(html);
    });

}