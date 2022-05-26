
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

    
            $("#m_sf_no").val(rowData.ShippingCode);
            

         
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
        url: '/TIMS/searchShippingSortingTIMS',
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

$("#scan_buyer_qr").on("keydown", function (e) {
    if (e.keyCode === 13) {
        $.blockUI({ message: '' });
        //kiem tra buyer_qr null ko

        //var buyer_qr = e.target.value.trim();
        var buyer_qr = $("#scan_buyer_qr").val().trim();

        if (buyer_qr == "" || buyer_qr == null || buyer_qr == undefined) {

            $("#scan_buyer_qr").val("");
            document.getElementById("scan_buyer_qr").focus();
            $.unblockUI();
            return false;
        }
        
        //ktra SF
        var ShippingCode = $("#m_sf_no").val();
        if (ShippingCode == "" || ShippingCode == null || ShippingCode == undefined) {
            $("#scan_buyer_qr").val("");
            document.getElementById("scan_buyer_qr").focus();
            $.blockUI({
                message: "Vui lòng chọn mã ST!!!",
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

        $.ajax({
           
            url: "/fgwms/ScanReceivingFGBuyer",
            type: "get",
            dataType: "json",
            data: {
                buyer_qr: buyer_qr,
                ShippingCode: ShippingCode
            },
            success: function (response) {
                if (response.result) {

                    SuccessAlert(response.message);

                   
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();

                    $.unblockUI();
                    return false;
                }
                else {
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();

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
                    return false;
                }
            }
        });
    }
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
   /* $('.popup-dialog.SF_Info_Popup').dialog('open');*/
  
    var ShippingCode = $(e).data('id');
    $.get("/TIMS/PartialViewShippingTIMSSortingPP?" +
        "ShippingCode=" + ShippingCode + "&edit=false"
        , function (html) {
            $("#PartialViewShippingFGSortingPP").html(html);
        })
}



//----- help to focus on input------------------//
setTimeout(function () { $("#scan_mt_cd").focus(); }, 1);