function FinishEx2M(data) {
    var result = confirm("bạn muốn hoàn thành không ?");
    if (result == true) {
        if (data.id) {
            var post = $.ajax({
                url: '/ExportToMachine/FinishExtoMachine?id=' + data.id + '&IsFinish=true',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            });
            post.done(function (msg) {
                SuccessAlert("");
                $("[id=" + data.id + "]").remove();
                //add row to list finish
                var parameters =
                {
                    rowID: data.id,
                    initdata: {
                        ChangeDate: data.ChangeDate,
                        ChangeId: data.ChangeId,
                        CreateDate: data.CreateDate,
                        CreateId: data.CreateId,
                        Description: data.Description,
                        ExportCode: data.ExportCode,
                        IsFinish: data.IsFinish,
                        LocationCode: data.LocationCode,
                        MachineCode: data.MachineCode,
                        ProductCode: data.ProductCode,
                        ProductName: data.ProductName,
                        id: data.id
                    },
                    position: "first",
                    useDefValues: false,
                    useFormatter: false,
                    addRowParams: { extraparam: {} }
                };

                $("#list-finish").jqGrid('addRow', parameters);
            });

            post.fail(function (jqXHR, textStatus) {
                ErrorAlert("không hoàn thành được");
            });
            idFinishPO = null;
        } else {
            window.alert("chưa có PO nào click")
        }
    }
};
function Redo(data) {
    var result = confirm("bạn muốn hủy hoàn thành không ?");
    if (result == true) {
        if (data.id) {
            var post = $.ajax({
                url: '/ExportToMachine/FinishExtoMachine?id=' + data.id + '&IsFinish=false',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            });
            post.done(function (msg) {
                SuccessAlert("");
                $("[id=" + data.id + "]").remove();
                //add row to list processing
                var parameters =
                {
                    rowID: data.id,
                    initdata: {
                        ChangeDate: data.ChangeDate,
                        ChangeId: data.ChangeId,
                        CreateDate: data.CreateDate,
                        CreateId: data.CreateId,
                        Description: data.Description,
                        ExportCode: data.ExportCode,
                        IsFinish: data.IsFinish,
                        LocationCode: data.LocationCode,
                        MachineCode: data.MachineCode,
                        ProductCode: data.ProductCode,
                        ProductName: data.ProductName,
                        id: data.id
                    },
                    position: "first",
                    useDefValues: false,
                    useFormatter: false,
                    addRowParams: { extraparam: {} }
                };

                $("#list-processing").jqGrid('addRow', parameters);
            });

            post.fail(function (jqXHR, textStatus) {
                ErrorAlert("không hoàn thành được");
            });
            idFinishPO = null;
        } else {
            window.alert("chưa có PO nào click")
        }
    }
};
function getDataFinish(pdata) {
    var params = new Object();

    if ($("#list-finish").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.ExportCode = $('#s_ep_no').val().trim();
    params.ProductCode = $('#s_ProductCode').val().trim();
    params.ProductName = $('#s_ProductName').val().trim();
    params.Description = $('#s_Description').val().trim();

    $("#list-finish").jqGrid('setGridParam',
        {
            search: true,
            postData:
            {
                searchString: $("#auto_complete_search").val()
            }
        });
    $.ajax({
        url: '/ExportToMachine/listExportMaterial?IsFinish=true',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list-finish")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
function RedoTarget(cellValue, options, rowdata, action) {
    return '<button  class="btn btn-sm btn-primary button-srh text-center" onclick=\'Redo(' + JSON.stringify(rowdata) + ')\'>Redo</button>';
};

function loadGridFinish() {
    $("#list-finish").jqGrid({
        mtype: 'GET',
        datatype: function (postData) { getDataFinish(postData); },
        colModel: [
            { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true },
            { label: 'Export Code', name: 'ExportCode', width: 100, align: 'center', formatter: EP_popup },
            { label: 'Export Code', name: 'ExportCode', width: 150, align: 'left', hidden: true },
            { label: 'Product Code', name: 'ProductCode', width: 150, align: 'left' },
            { label: 'Product Name', name: 'ProductName', width: 250, align: 'left' },
            { label: 'Description', name: 'Description', width: 180, align: 'left' },
            { label: 'Finish', name: 'Finish', width: 80, align: 'center', formatter: RedoTarget },
        ],
        pager: '#gridpagerfinish',
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: 200,
        width: null,
        loadonce: true,
        caption: 'Export Infomation',
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
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list-finish").jqGrid("getGridParam", 'selrow');
            var rowData = $("#list-finish").getRowData(selectedRowId);

            $("#m_id").val(rowData.id);
            $("#modifyExportCode").val(rowData.ExportCode);
            //$("#ModifyMachineCode").val(rowData.MachineCode);
            $("#ModifyProductCode").val(rowData.ProductCode);
            $("#ModifyProductName").val(rowData.ProductName);
            $("#ModifyDescription").val(rowData.Description);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("show");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").addClass("active");

            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
        },
    });
}
$("a[href=#Finish]").on('click', function () {
    loadGridFinish();
})
//GRID
$(function () {
    function FinishEx(cellValue, options, rowdata, action) {
        return '<button  class="btn btn-sm btn-danger button-srh text-center" onclick=\'FinishEx2M(' + JSON.stringify(rowdata) + ')\'>Finish</button>';
    };
    $("#list-processing").jqGrid({
        mtype: 'GET',
        datatype: function (postData) { getData(postData); },
        colModel: [
            { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true },
            { label: 'Shipping Code', name: 'ShippingCode', width: 100, align: 'center', formatter: SP_popup },
            { label: 'Shipping Code', name: 'ShippingCode', width: 150, align: 'left', hidden: true },
            { label: 'Product Code', name: 'ProductCode', width: 150, align: 'left' },
            { label: 'Product Name', name: 'ProductName', width: 250, align: 'left' },
            { label: 'Description', name: 'Description', width: 180, align: 'left' },
            //{ label: 'Finish', name: 'Finish', width: 80, align: 'center', formatter: FinishEx },
        ],
        pager: '#gridpagerprocessing',
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        rownumbers: true,
        autowidth: true,
        shrinkToFit: false,
        viewrecords: true,
        height: 200,
        width: null,
        loadonce: true,
        caption: 'Shipping Infomation',
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
        onSelectRow: function (rowid, selected, status, e) {
            var selectedRowId = $("#list-processing").jqGrid("getGridParam", 'selrow');
            var rowData = $("#list-processing").getRowData(selectedRowId);

            $("#m_id").val(rowData.id);
            $("#modifyShippingCode").val(rowData.ShippingCode);
            $("#ModifyProductCode").val(rowData.ProductCode);
            $("#ModifyProductName").val(rowData.ProductName);
            $("#ModifyDescription").val(rowData.Description);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("show");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").addClass("active");

            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
        },
    });
});

function getData(pdata) {
    var params = new Object();

    if ($("#list-processing").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.ShippingCode = $('#s_sf_no').val().trim();
    params.ProductCode = $('#s_ProductCode').val().trim();
    params.ProductName = $('#s_ProductName').val().trim();
    params.Description = $('#s_Description').val().trim();

    $("#list-processing").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/fgwms/searchShippingSortingFG',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list-processing")[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
//-------SEARCH------------//
$("#searchBtn").click(function () {

    $("#list-processing").jqGrid('setGridParam', { search: true });
    var pdata = $("#list-processing").jqGrid('getGridParam', 'postData');
    getData(pdata);
});

//-------INSERT------------//
$("#c_save_but").click(function () {
    var isValid = $('#form1').valid();
    if (isValid == true) {
       
        var Description = $('#CreateDescription').val();
        var ProductCode = $('#CreateProductCode').val();
        var ProductName = $('#CreateProductName').val().trim();
        {
            $.ajax({
                url: "/fgwms/InsertShippingFGSorting",
                type: "get",
                dataType: "json",
                data: {
                    ProductCode: ProductCode,
                    ProductName: ProductName,
                    Description: Description,
                },
                success: function (response) {
                    if (response.result) {
                        var id = response.data.id;
                        $("#list-processing").jqGrid('addRowData', id, response.data, 'first');
                        $("#list-processing").setRowData(id, false, { background: "#28a745", color: "#fff" });
                        SuccessAlert("Thành công");
                    } else {
                        ErrorAlert('Data already exists, Please check again!!!');
                    }
                }
            });
        }
    }
});

//-------UPDATE------------//
$("#m_save_but").click(function () {
    var isValid = $('#form2').valid();
    if (isValid == false) {
        return false;
    }

    var id = $('#m_id').val();
    var ProductName = $('#ModifyProductName').val();
    var ShippingCode = $('#modifyShippingCode').val();
    var ProductCode = $('#ModifyProductCode').val();
    var Description = $('#ModifyDescription').val();


    $.ajax({
        url: "/fgwms/ModifyShippingFGSorting",
        type: "get",
        dataType: "json",
        data: {
            id: id,
            ShippingCode: ShippingCode,
            ProductName: ProductName,
            ProductCode: ProductCode,
            Description: Description

        },
        success: function (response) {
            if (response.result) {
                var id = response.data.id;
                $("#list-processing").setRowData(id, response.data, { background: "#28a745", color: "#fff" });
                SuccessAlert("Thành công");

            } else {
                ErrorAlert(response.message);
            }
        },

    });
});
$("#m_delete_but").click(function () {
    var isValid = $('#form2').valid();
    if (isValid == false) {
        return false;
    }
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {
        var selectedRowId = $("#list-processing").jqGrid("getGridParam", 'selrow');
        var rowData = $("#list-processing").getRowData(selectedRowId);
        var id = $("#m_id").val();
        var ShippingCode = rowData.ShippingCode;
        $.ajax({
            url: "/fgwms/DeleteExporttomachine",
            type: "POST",
            data:{
                id:id,
                ShippingCode: ShippingCode,
             },
            success: function (response) {
                if (response.result) {

                    $('#list-processing').jqGrid('delRowData', id);
                    document.getElementById("form2").reset();
                    document.getElementById("form1").reset();
                    $("#tab_2").removeClass("active");
                    $("#tab_1").addClass("active");
                    $("#tab_c2").removeClass("show");
                    $("#tab_c2").removeClass("active");
                    $("#tab_c1").addClass("active");
                    $("#m_id").val("");
                    $("#modifyExportCode").val("");

                    SuccessAlert(response.message);

                } else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});

$(function () {
    $("#list1").jqGrid
        ({
            url: '',
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true},
                { label: 'Buyer Code', name: 'buyer_qr', width: 300, align: 'left' },
                { label: 'Product Code', name: 'product_code', width: 130, align: 'left' },
                { label: 'Quantity', name: 'qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'Lot No', name: 'lot_no', width: 100, align: 'right', hidden: true },
                { label: 'DL No', name: 'dl_no', width: 130, align: 'center' },
                { label: 'Status', name: 'sts_cd', width: 130, align: 'center' },
              
            ],


            pager: jQuery('#listPager1'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: '',
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
            gridComplete: function () {


            }


        });
});
function bntDelete(cellValue, options, rowdata, action) {
    var wmtid = rowdata.id;
    var html = '<button class="btn btn-xs btn-danger" onclick="Del_Row(' + id + ');">Delete</button>';
    return html;
}
function Del_Row(id) {

    $("#list1").jqGrid('delRowData', id);

}


function convertDate(cellValue, options, rowdata, action) {

    if (cellValue != null) {

        a = cellValue.substr(0, 4);
        b = cellValue.substr(4, 2);
        c = cellValue.substr(6, 2);

        var html = a + "-" + b + "-" + c;
        return html;
    }
    else {
        var html = "";
        return html;
    }



};

var ListBuyer = [];

$("#scan_buyer_qr").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var BuyerCode = $("#scan_buyer_qr").val();
        var IsExist = (ListBuyer.indexOf(BuyerCode));
        if (IsExist != -1 && ListBuyer.length > 0) {
         
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
            $("#scan_buyer_qr").val("");
            document.getElementById("scan_buyer_qr").focus();
              return false;
        }
        $.ajax({
            url: "/fgwms/GetExport_ScanBuyer_FG",
            type: "get",
            dataType: "json",
            data: {
                BuyerCode: BuyerCode
            },
            success: function (response) {
                if (response.result) {

                    ListBuyer.push(BuyerCode);
                    var id = response.data.id;
                    $("#list1").jqGrid('addRowData', id, response.data, 'first');
                    $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();

                } else {
                   // ErrorAlert(response.message);
                    $.blockUI({
                        message: response.message,
                        timeout: 2000,
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
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();
                }

            }
        });
    }
});
$("#Clear_all").on("click", function () {

    var rows = $("#list1").getDataIDs();
    if (rows.length > 0) {
        var r = confirm("Are you make sure DELETE Events?");
        if (r === true) {
            $("#list1").jqGrid('clearGridData');
            ListBuyer = []
        }

    }
    else {
       // alert("Please Scan!!!")
        $.blockUI({
            message: "Bạn không có bất kì NVL nào để xóa",
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
        return false;
    }

});

$("#Save_completed").on("click", function () {
    var ShippingCode = $("#modifyShippingCode").val().trim();
    if (ShippingCode == "" || ShippingCode == null || ShippingCode == undefined) {
       // ErrorAlert("Bạn hãy chọn một phiếu xuất bất kỳ để tiếp tục");
        $.blockUI({
            message: "Bạn hãy chọn một phiếu xuất bất kỳ để tiếp tục",
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
        return false;
    }
   
       

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        //ErrorAlert("Dữ liệu rỗng, Vui lòng scan Nguyên Vật Liệu để tiếp tục");
        $.blockUI({
            message: "Dữ liệu rỗng, Vui lòng scan Nguyên Vật Liệu để tiếp tục",
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
        $("#scan_buyer_qr").val("");
        document.getElementById("scan_buyer_qr").focus();
        return false;
    }
    var r = confirm("Bạn có chắc muốn xuất tem gói vào PHIẾU " + ShippingCode);
    if (r === true) {
        $.ajax({
            url: "/fgwms/UpdateShippingSortingFG?ListId=" + rows,
            type: "get",
            dataType: "json",
            data:
            {
                ShippingCode: ShippingCode,
            },
            success: function (data) {
                if (data.result) {
                    SuccessAlert(data.message);
                    ListBuyer = []
                    $("#list1").jqGrid('clearGridData');
                    return;
                }
                else {
                    //ErrorAlert(data.message);
                    $.blockUI({
                        message: data.message,
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
                    return;
                }
            }
        });
    }
    return;
    
});


//----- SP POPUP------------------//
function SP_popup(cellValue) {
  
    var html = '<a class="css_pp_cilck" data-id="' + cellValue + '" onclick="ViewExPortPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewExPortPopup(e) {

        var opt = {
        autoOpen: false,
        modal: true,
        width: 1000,
        height: 950,
        title: 'Details'
    };

    var theDialog = $(".popup-dialog.SF_Info_Popup").dialog(opt);
    theDialog.dialog("open");
    /*$('.popup-dialog.SF_Info_Popup').dialog('open');*/
  
    var ShippingCode = $(e).data('id');
    $.get("/fgwms/PartialViewShippingFGSortingPP?" +
        "ShippingCode=" + ShippingCode + "&edit=true"
        , function (html) {
            $("#PartialViewShippingFGSortingPP").html(html);
        })
}



//----- help to focus on input------------------//
setTimeout(function () { $("#scan_mt_cd").focus(); }, 1);