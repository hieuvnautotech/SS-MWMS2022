
$(function () {
    $("#list1").jqGrid
        ({
            url: '',
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true},
                { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
                { label: 'MT No', name: 'mt_no', width: 130, align: 'left' },
                //{ label: 'MT Type', name: 'mt_type_nm', width: 100, align: 'left' },
               /* { label: 'Lot No', name: 'lot_no', width: 100, align: 'left' },*/
                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Location', name: 'locationName', width: 130, align: 'center' },
                //{ label: 'Status', name: 'sts_nm', width: 120, align: 'center' },

                //{ label: 'Expiry Date', name: 'expiry_dt', width: 180, align: 'center', formatter: convertDate },
                //{ label: 'Date of Receipt', name: 'dt_of_receipt', width: 180, align: 'center', formatter: convertDate },
                //{ label: 'Expore Date', name: 'expore_dt', width: 180, align: 'center', formatter: convertDate },
                //{ name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete },
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
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-danger" onclick="Del_Row(' + wmtid + ');">Delete</button>';
    return html;
}
function Del_Row(wmtid) {

    $("#list1").jqGrid('delRowData', wmtid);

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

var ListMaterial = [];

$("#scan_mt_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var materialCode = e.target.value.trim();
        var IsExist = (ListMaterial.indexOf(materialCode));
        if (IsExist != -1 && ListMaterial.length > 0) {
            $("#scan_mt_cd").val("");
            document.getElementById("scan_mt_cd").focus();
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
            //ErrorAlert("Dữ liệu đã được scan rồi, vui lòng scan mã khác");
            
              return false;
        }
        $.ajax({
            url: "/ExportToMachine/GetRack_ScanMLQR_WIP",
            type: "Get",
            dataType: "json",
            data: {
                materialCode: materialCode
            },
            success: function (response) {
                if (response.result) {

                    ListMaterial.push(materialCode);
                    var id = response.data.materialid;
                    $("#list1").jqGrid('addRowData', id, response.data, 'first');
                    $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    $("#scan_mt_cd").val("");
                    document.getElementById("scan_mt_cd").focus();
                  

                }
                else {
                    $("#scan_mt_cd").val("");
                    document.getElementById("scan_mt_cd").focus();
                    $.blockUI({
                        message: response.message,
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
            ListMaterial = []
            $("#scan_mt_cd").val("");
            document.getElementById("scan_mt_cd").focus();
        }

    }
    else {
       // alert("Please Scan!!!")
        $.blockUI({
            message: "Bạn không có bất kì NVL nào để xóa, vui lòng quét NVL ở kệ",
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
  
    var SelectRack = $("#SelectRack").val().trim();
    if (SelectRack == "" || SelectRack == null || SelectRack == undefined) {
        //ErrorAlert("Bạn hãy chọn một kệ để tiếp tục");
        $.blockUI({
            message: "Bạn hãy chọn một kệ để tiếp tục",
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
        $("#scan_mt_cd").val("");
        document.getElementById("scan_mt_cd").focus();
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
        return false;
        //ErrorAlert("Dữ liệu rỗng, Vui lòng scan Nguyên Vật Liệu để tiếp tục");
     
    
    }

    $.ajax({
        url: "/ExportToMachine/UpdateChangeRackMaterialToMachine?ListId="+ rows,
        type: "Put",
        dataType: "json",
        data:
        {
            SelectRack: SelectRack
        },
        success: function (data) {
            if (data.result) {
                SuccessAlert(data.message);
                ListMaterial = []
                $("#list1").jqGrid('clearGridData');
            }
            else {
               // ErrorAlert(data.message);
                $.blockUI({
                    message: ErrorAlert(data.message),
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
        }
    });
    
});


//----- help to focus on input------------------//
setTimeout(function () { $("#scan_mt_cd").focus(); }, 1);