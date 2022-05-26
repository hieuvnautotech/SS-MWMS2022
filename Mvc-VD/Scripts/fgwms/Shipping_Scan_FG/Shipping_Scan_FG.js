function deliveryQtyFormat(cellValue)
{
    var temp = undefined;
    if (cellValue === undefined)
    {
        temp = `0`;
    } else
    {

        temp = cellValue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    }
    return temp;
};
var dlNo = ``;
var dlID = 0;
$(function ()
{
    $("#list").jqGrid
        ({
            datatype: function (postData) { getData(postData); },
            mtype: 'Get',
            colModel: [
                { key: true, label: 'dlid', name: 'dlid', width: 50, align: 'center', hidden: true },
                { label: 'Delivery No', name: 'dl_no1', width: 100, align: 'center', sortable: true, formatter: code_pp },
                { label: 'Delivery No', name: 'dl_no', width: 100, align: 'left', sortable: true,hidden: true },
                { label: 'Delivery Name', name: 'dl_nm', width: 200, sortable: true, align: 'left' },
                { label: 'Status', name: 'Status', width: 150, align: 'center', hidden: true },
                { label: 'Delivery Date', name: 'work_dt', width: 150, sortable: true, align: 'center' },
                { label: 'Delivery Quantity', name: 'quantity', width: 150, align: 'right', sortable: false,formatter: deliveryQtyFormat },
                { label: 'Remark', name: 'remark', width: 180, align: 'left', sortable: true, },
                //{ label: 'Cancel', name: '', width: 80, align: 'center', formatter: CancelFormat },

            ],
            onSelectRow: function (rowid, selected, status, e)
            {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);
                dlNo = rowData.dl_no;
                dlID = selectedRowId;
                $("#m_dlid").val(rowData.dlid);
                $("#m_dl_no").val(rowData.dl_no);
                $("#m_dl_nm2").val(rowData.dl_nm);
                $("#m_remark").val(rowData.remark);
                $("#m_date").val(rowData.work_dt);

                $("#tab_1").removeClass("active");

                $("#tab_1").removeClass("show");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("show");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").addClass("active");

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);



            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Delivery Infomation',
            emptyrecords: "No data.",
            height: 200,
            width: null,
            autowidth: false,
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
            gridComplete: function ()
            {
            },
            loadComplete: function ()
            {
                var ids = $("#list").getDataIDs();
                for (var item of ids)
                {
                    if (item == dlID)
                    {
                        $("#list").setRowData(item, false, { background: "#28a745", color: "#fff" });
                        break;
                    }
                }
                dlID = 0;
            }
        });
});
function getData(pdata)
{
    var params = new Object();

    if ($("#list").jqGrid('getGridParam', 'reccount') == 0)
    {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.dl_no = $('#s_dl_no').val().trim();
    params.dl_nm = $('#s_dl_nm').val().trim();
    params.productCode = $('#productCode').val() == null ? "" : $('#productCode').val().trim();
    params.start = $('#start').val() == null ? "" : $('#start').val().trim();
    params.end = $('#end').val() == null ? "" : $('#end').val().trim();
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/fgwms/GetDLInfo',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st)
        {
            if (st == "success")
            {
                var showing = $("#list")[0];
                showing.addJSONData(data);
            }
        },
        error: function ()
        {
            return;
        }
    });
};
//-------SEARCH------------//
$("#searchBtn").click(function ()
{
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getData(pdata);
});
function convertDate(cellValue, options, rowdata, action)
{
    if (cellValue != null)
    {
        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);

        var html = a + "-" + b + "-" + c;
        return html;
    }
    else
    {
        var html = "";
        return html;
    }
};

//-------INSERT------------//
$("#c_save_but").click(function ()
{
    debugger;
    var isValid = $('#form1').valid();
    if (isValid == true)
    {
        var dl_nm = $('#m_dl_nm').val();
        var remark = $('#c_remark').val();
        var c_date = $('#c_date').val();

        {
            $.ajax({
                url: "/fgwms/InsertDlInfo",
                type: "Post",
                dataType: "json",
                data: {
                    dl_nm: dl_nm,
                    remark: remark,
                    work_dt: c_date,
                },
                success: function (response)
                {
                    if (response.result == true)
                    {
                        var id = response.data.dlid;
                        $("#list").jqGrid('addRowData', id, response.data, 'first');
                        $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });
                        SuccessAlert(response.message);
                    } else
                    {
                        ErrorAlert(response.message);
                    }
                }
            });
        }
    }
});
//-------UPDATE------------//
$("#m_save_but").click(function ()
{
    var isValid = $('#form2').valid();
    if (isValid == false)
    {
        return false;
    }

    var dlid = $('#m_dlid').val();
    var dl_nm = $('#m_dl_nm2').val();
    var remark = $('#m_remark').val();
    var date = $('#m_date').val();

    $.ajax({
        url: "/fgwms/ModifyDlInfo",
        type: "Put",
        dataType: "json",
        data: {
            dlid: dlid,
            dl_nm: dl_nm,
            remark: remark,
            work_dt: date
        },
        success: function (response)
        {
            if (response.result)
            {
                var id = response.data.dlid;
                $("#list").setRowData(id, response.data, { background: "#28a745", color: "#fff" });
            } else
            {
                alert(response.message);
            }
        },
    });
});
//----- Delivery POPUP------------------//
function code_pp(cellValue)
{
    var html = `<a class="css_pp_cilck" data-dl_no="${cellValue}" onclick="ViewDLPopup(this);">${cellValue}</a>`;
    return html;
};

function ViewDLPopup(e)
{

    //var opt = {
    //    autoOpen: false,
    //    modal: true,
    //    width: 2000,
    //    height: 950,
    //    title: 'Details'
    //};

    var theDialog = $(".popup-dialog.DL_Info_Popup").dialog();
    theDialog.dialog("open");
    var dl_no = $(e).data('dl_no');

    $.get(`/fgwms/PartialView_DL_Info_Popup?dl_no=${dl_no}`
        , function (html)
        {
            $("#PartialView_DL_Info_Popup").html(html);
        });
}

//----- list1------------------//
var prevCellVal = { cellId: undefined, value: undefined };
$(function ()
{
    SetDatePicker();

    $("#list1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true },
                //{ label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
                //{ label: 'Bobbin No', name: 'bb_no', width: 220, align: 'center' },
                { label: 'Buyer QR', name: 'buyer_cd', width: 220, align: 'center', hidden: true },
                { label: 'Product Code', name: 'product', width: 220, align: 'center' },
                { name: 'TypeSystem', hidden: true  },
                { label: 'Box No', name: 'bx_no', width: 220, align: 'center' },

                { label: 'Material Type', name: 'mt_type_nm', width: 130, align: 'center', hidden: true },
                //{ label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'gr_qty', width: 80, align: 'right', formatter: 'integer'},
                //{ label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 100, align: 'center', hidden: true},

                //{ label: 'Departure', name: 'from_lct_nm', width: 100, align: 'center' },
                //{ label: 'Location', name: 'to_lct_nm', width: 100, align: 'center' },
                //{ label: 'Destination', name: 'to_lct_nm', width: 100, align: 'center' },
                //{ label: 'Location Status', name: 'lct_sts_cd', width: 100, align: 'center' },

                //{ label: 'Receiving Date', name: 'recevice_dt_tims', width: 130, align: 'center', formatter: convertDate },
                {
                    name: "", width: 100, align: "center", label: "Action", resizable: false, title: false,
                    cellattr: function (rowId, val, rawObject, cm, rdata)
                    {
                        var result;

                        if (prevCellVal.value == val)
                        {
                            result = ' style="" rowspanid="' + prevCellVal.cellId + '"';
                        }
                        else
                        {
                            var cellId = this.id + '_row_' + rowId + '_' + cm.name;

                            result = ` rowspan="${rawObject.rowpan}" id="${cellId}">  <button class="btn btn-xs btn-danger"  data-bx_no="${rawObject.bx_no}"  onclick="Del_Row(this);" > Delete </button`;

                            prevCellVal = { cellId: cellId, value: val };
                        }

                        return result;
                    }
                },
            ],
            footerrow: true,
            userDataOnFooter: true,
            pager: jQuery('#listPager1'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: `Tem thùng`,
            emptyrecords: "No data.",
            height: 250,
            width: null,
            autowidth: false,
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
            onSelectRow: function (rowid, status, e, iRow, iCol)
            {
                var selrowId = $("#list1").jqGrid(`getGridParam`, `selrow`);
                var rowData = $("#list1").getRowData(selrowId);
                boxCodeMapped = rowData.bx_no;
                if (boxCodeMapped)
                {
                    $("#list2").setGridParam({ url: `/fgwms/GetMappedProducts?boxCodeMapped=${boxCodeMapped}`, datatype: "json" }).trigger("reloadGrid");
                }
            },
            gridComplete: function ()
            {
                prevCellVal = { cellId: undefined, value: undefined };

                //sum Quantity
                var ids = $("#list1").jqGrid('getDataIDs');
                var sum_Quantity = 0;
                var $self = $(this)
                for (var i = 0; i < ids.length; i++) {
                    sum_Quantity += parseInt($("#list1").getCell(ids[i], "gr_qty"));
                }

                $self.jqGrid("footerData", "set", { bx_no: "Total" });
                $self.jqGrid("footerData", "set", { gr_qty: sum_Quantity });


                jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });
});
$("#list2").jqGrid
    ({
        mtype: 'Get',
        colModel: [
            { key: true, label: 'Id', name: 'Id', width: 50, align: 'center', hidden: true },
            { label: 'Buyer QR', name: 'BuyerCode', width: 220, align: 'center' },
            { label: 'Quantity', name: 'Quantity', width: 80, align: 'right', formatter: 'integer' },
            { label: 'Lot Date', name: 'lot_date', width: 80, align: 'center' },
        ],
        footerrow: true,
        userDataOnFooter: true,
        pager: jQuery('#listPage2'),
        width: null,
        rowNum: 500,
        rowList: [500, 1000, 2000, 10000],
        loadtext: `Loading...`,
        emptyrecords: `No data`,
        height: 250,
        rownumbers: true,
        shrinkToFit: false,
        autowidth: false,
        loadonce: true,
        multiselect: false,
        multiboxonly: false,
        autoResizing: true,
        viewrecords: true,
        caption: `Tem gói`,
        jsonReader:
        {
            root: `data`,
            page: `page`,
            total: `total`,
            records: `records`,
            repeatitems: false,
            Id: `0`
        },
        gridComplete: function ()
        {
            prevCellVal = { cellId: undefined, value: undefined };

            //sum Quantity
            var ids = $("#list2").jqGrid('getDataIDs');
            var sum_Quantity = 0;
            var $self = $(this);
            for (var i = 0; i < ids.length; i++)
            {
                sum_Quantity += parseInt($("#list2").getCell(ids[i], "Quantity"));
            }

            $self.jqGrid("footerData", "set", { BuyerCode: "Total" });
            $self.jqGrid("footerData", "set", { Quantity: sum_Quantity });


            //jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        }
    });



var ListBox = [];
$("#scan_bx_no").on("keydown", function (e) {
    if (e.keyCode === 13) {
        //kiem tra bx_no null ko
        //var bx_no = e.target.value.trim();
        var bx_no = $("#scan_bx_no").val().trim();

        if (bx_no == "" || bx_no == null || bx_no == undefined) {
            $("#scan_bx_no").val("");
            document.getElementById("scan_bx_no").focus();
            return false;
        }
        //ktra tren luoi co ton tai khong

        var IsExist = (ListBox.indexOf(bx_no));
        if (IsExist != -1 && ListBox.length > 0) {
            $.blockUI({
                message: ("Dữ liệu đã được scan rồi, vui lòng scan mã khác"),
                timeout: 500,
                css: {

                    border: 'none',
                    padding: '15px',
                    backgroundColor: 'red',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 1,
                    color: '#fff',
                    fontSize: '20px'
                }
            });
            $("#scan_bx_no").val("");
            document.getElementById("scan_bx_no").focus();
            return false;
        }


        $.ajax({
            url: "/fgwms/GetShipping_ScanMLQR_FG",
            type: "get",
            dataType: "json",
            data: {
                bx_no: bx_no
            },
            success: function (response) {
                if (response.result) {
                    ListBox.push(bx_no);

                    var id = response.data.id;
                    $("#list1").jqGrid('addRowData', id, response.data, 'first');
                    $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    SuccessAlert(response.message);
                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();

                } else {
                    ErrorAlert(response.message);
                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();
                }
            }
        });
    }
});


function Del_Row(e)
{
    var bx_no = e.dataset.bx_no;
    var rows = $('#list1').jqGrid('getRowData');

    for (var item of rows)
    {
        var id = item.id;
        var bx_no_view = item.bx_no;

        if (bx_no == bx_no_view)
        {
            $("#list1").jqGrid('delRowData', id);
            $("#list1").jqGrid('delRowData', id);
            $("#list2").jqGrid('clearGridData');
            ListBox = ListBox.filter(item => item !== bx_no)
        }
    }
}
//$("#Clear_all").on("click", function () {
//    var r = confirm("Are you make sure DELETE Events?");
//    if (r === true) {
//        $("#list1").jqGrid('clearGridData');
//    }
//});

$("#Clear_all_box").on("click", function ()
{
    var r = confirm("Chọn YES để xóa danh sách scan.");
    if (r === true)
    {
        $("#list1").jqGrid('clearGridData');
        ListBox = [];
    }
});

$("#Save_completed").on("click", function ()
{
    var dl_no = $("#m_dl_no").val();
    if (dl_no == "" || dl_no == null || dl_no == undefined)
    {
        ErrorAlert("Vui lòng chọn 1 danh sách chuyển hàng!!!");
        return false;
    }

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined)
    {
        ErrorAlert("Chưa scan mã tem nào!!!");
        return false;
    }

    var rowIDMes = [];
    var rowIDSap = [];
    var BoxCodeList = [];


    const beasts = ['ant', 'bison', 'camel', 'duck', 'bison'];

    console.log(beasts.indexOf('bison'));

    var rows = $("#list1").jqGrid('getRowData');
    for (i = 0; i < rows.length; i++) {
        var TypeSystem = rows[i].TypeSystem;

        if (TypeSystem == "MES") {
            rowIDMes.push(rows[i].wmtid);
        }
        if (TypeSystem == "SAP") {
            rowIDSap.push(rows[i].wmtid);
        }
        if (BoxCodeList.indexOf(rows[i].bx_no) == -1) {
            BoxCodeList.push(rows[i].bx_no);
        }
    }
    object = {};

    object['WmtidMES'] = rowIDMes;
    object['WmtidsAP'] = rowIDSap;
    object['DlNo'] = dl_no;
    object['ListBoxCode'] = BoxCodeList;
    var r = confirm("Bạn có chắc muốn xuất vào PHIẾU " + dl_no);
    if (r === true)
    {
        $.blockUI({ message: '<h4>Loading...</h4>' });
        $.ajax({
            url: "/fgwms/UpdateMTQR_DeliveryList",
            type: "Put",
            contentType: 'application/json',
            data: JSON.stringify(object),

            success: function (response)
            {
                $.unblockUI();
                if (response.result)
                {
                    ListBox = [];
                    SuccessAlert(response.message);
                    $("#list1").jqGrid('clearGridData');

                    var id = response.data[0].dlid;
                    $("#list").setRowData(id, response.data[0], { background: "#28a745", color: "#fff" });
                }
                else
                {
                    ErrorAlert(response.message);
                }
            },
            error: function ()
            {
                $.unblockUI();
                return;
            }
        });
    }
    else
    {
        return
    }
});

//----- help to focus on input------------------//
//setTimeout(function () { $("#scan_bx_no").focus(); }, 1);
//setTimeout(function () { $("#scan_bx_no").focus(); }, 1);

//$(`#m_delete_but`).on(`click`, function ()
//{
//    $('#dialogDangerous').dialog('open');
//});

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
    resize: function (event, ui)
    {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui)
    {
    },
});

$("#deletestyle").click(function ()
{
    $.ajax({
        url: `DeleteDL`,
        type: `get`,
        dataType: "json",
        data: {
            id: dlID
        },
        success: function (data)
        {
            if (data.result == true)
            {
                SuccessAlert(data.message);
                $("#list").jqGrid('delRowData', data.value);
            } else
            {
                ErrorAlert(data.message);
            }
        },
    });
    $('#dialogDangerous').dialog('close');
});

$(`#scan_buyer`).on("keydown", function (e)
{
    if (e.keyCode === 13)
    {
        //kiem tra bx_no null ko
        var buyerCode = e.target.value.trim();
        if (buyerCode == "" || buyerCode == null || buyerCode == undefined)
        {
            $("#scan_buyer").val("");
            document.getElementById("scan_buyer").focus();
            return false;
        }
        //ktra tren luoi co ton tai khong
        var rows = $('#list1').jqGrid('getRowData');

        for (i = 0; i < rows.length; i++)
        {
            var buyer_cd_view = rows[i].buyer_cd;

            if (buyerCode == buyer_cd_view)
            {
                alert("Data Duplicated !!!");

                $("#scan_buyer").val("");
                document.getElementById("scan_bx_no").focus();

                return false;
            }
        }

        $.ajax({
            url: "/fgwms/GetShipping_ScanBuyerQR_FG",
            type: "get",
            dataType: "json",
            data: {
                buyerCode: buyerCode
            },
            success: function (response)
            {
                if (response.result)
                {
                    for (var i = 0; i < response.data.length; i++)
                    {
                        var id = response.data[i].wmtid;

                        $("#list1").jqGrid('addRowData', id, response.data[i], 'first');
                        $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    }

                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();
                } else
                {
                    alert(response.message);

                    $("#scan_bx_no").val("");
                    document.getElementById("scan_bx_no").focus();
                }
            }
        });
    }
});

json_object = "";
tempList = [];
$(`#excelupload_input`).change(function (evt)
{
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event)
    {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName)
        {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            for (var i = 0; i < XL_row_object.length; i++)
            {
                var data_row_tmp = ``;
                if (XL_row_object[i]["CODE"] != undefined)
                {
                    data_row_tmp = XL_row_object[i]["CODE"];
                    excelData.push(data_row_tmp);
                }

            }
        });

        json_object = excelData;

    };

    reader.onerror = function (event)
    {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});

//EXCEL UPLOAD
$(`#ExcelUpload`).on(`click`, function ()
{
    tempList = [];

    for (var item of json_object)
    {
        data = {
            Code: item
        };
        tempList.push(data);
    }
    $(`#excelupload_input`).val('');
    $.ajax({
        url: `/fgwms/ShowingScanBoxTemporary`,
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tempList),
        traditonal: true
    })
        .done(function (response)
        {
            if (response.result && response.listData)
            {

                for (var item of response.listData)
                {
                    var id = item.wmtid;
                    $("#list1").jqGrid('addRowData', id, item, 'first');
                    $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                }
                $("#scan_bx_no").val(``);
                $("#scan_bx_no").focus();
                SuccessAlert(response.message);
                return;
            }

            ErrorAlert(response.message);
            $(`#scan_bx_no`).val(``);
            return;

        })
        .fail(function ()
        {
            ErrorAlert(`Lỗi hệ thống.`);
        });
});

function SetDatePicker()
{
    $(`#start`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());

    $(`#end`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());

    $(`#c_date`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());


    $(`#m_date`).datepicker({
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());
};
$('#excelBtn').click(function () {

    var i, n, rowData, selRowIds = $("#list").jqGrid("getGridParam", "selarrrow");

    if (selRowIds.length > 0) {
        var dlNoArray = [];
        for (var i of selRowIds) {
            dlNoArray.push(i);
        }
        $(`#exportData`).attr(`action`, `/fgwms/ExportToExcel?dlNo=${dlNoArray}`);

    }
    else {
        ErrorAlert(`Không có giá trị của danh sách xuất hàng.`);
        return;
    }

    //var dlNo = $("#s_dl_no").val() == undefined ? "" : $("#s_dl_no").val().trim();
    //var dlName = $("#s_dl_nm").val() == undefined ? "" : $("#s_dl_nm").val().trim();
    //var productCode = $("#productCode").val() == undefined ? "" : $("#productCode").val().trim();
    //var start = $("#start").val() == undefined ? "" : $("#start").val();
    //var end = $("#end").val() == undefined ? "" : $("#end").val();

    //$(`#exportData`).attr(`action`, `/fgwms/ExportToExcel?dlNo=${dlNo}&dlName=${dlName}&productCode=${productCode}&start=${start}&end=${end}`);
});


$('#excelBtn2').click(function () {
    var i, n, rowData, selRowIds = $("#list").jqGrid("getGridParam", "selarrrow");
    if (selRowIds.length > 0) {
        var dlNoArray = [];
        //var rowIds = $("#list").getDataIDs();
        //var length = rowIds.length;
        /*  if (length > 0) {*/
        for (var i of selRowIds) {
            dlNoArray.push(i);
        }

        $(`#exportData2`).attr(`action`, `/fgwms/ShippingExportToExcel?dlNo=${dlNoArray}`);
        //}
    }

    else {
        ErrorAlert(`Không có giá trị của danh sách xuất hàng.`);
        return;
    }


});
//----- Format cancel ------------------//
function CancelFormat(cellValue, options, rowdata, action) {
    //debugger
    var html = "";
    if (rowdata.dlid == null || rowdata.dlid == "") {
        return "";
    }
    else {
        html = '<button  class="btn btn-sm btn-danger button-srh"  data-dlid="' + rowdata.dlid + '"onclick="CancelDL(this);">X</button>';
        return html;

    }
};
function CancelDL(e) {
    var dlid = $(e).data("dlid");
    var confirmText = "Bạn có chắc muốn hủy không?";
    if (confirm(confirmText)) {
        $.ajax({
            type: 'POST',
            url: "/fgwms/CancelDeliveryAll?dlid=" + dlid,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',

            cache: false,
            processData: false,
            success: function (response) {
                if (response.result) {

                    $("#list").jqGrid('delRowData', dlid);
                    SuccessAlert(response.message);

                }
                else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
    return false;
}
//



function bntDelete(cellValue, options, rowdata, action)
{
    var bx_no = rowdata.bx_no;
    var id = rowdata.id;
    var html = '<button class="btn btn-xs btn-danger" data-id= ' + id + ' data-bx_no= ' + bx_no + ' onclick="Delelte_Row(this);">Delete</button>';
    return html;
}
function Delelte_Row(e)
{
    var bx_no = $(e).data('bx_no');;
    var id = $(e).data('id');

    $("#list1").jqGrid('delRowData', id);
    $("#list2").jqGrid('clearGridData');

    ListBox = ListBox.filter(item => item !== bx_no);


}


$('#m_delete_but').click(function ()
{
    $.blockUI({ message: $('#question'), css: { width: '275px' } });
});

$('#yes').click(function ()
{

    $.blockUI({ message: "<h4>Đang xóa,Vui lòng đợi...</h4>" });

    var settings = {
        "url": "/fgwms/DeleteDelivery",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "id": $('#m_dlid').val(),
        }),
    };
    $.ajax(settings).done(function (reposive)
    {
        if (reposive.result)
        {
            SuccessAlert(reposive.message);
            $("#list").jqGrid('delRowData', $('#m_dlid').val());
            $.unblockUI();
        }
        else
        {
            ErrorAlert(reposive.message);
            $.unblockUI();
        }
    });
});

$('#no').click(function ()
{
    $.unblockUI();
    return false;
});