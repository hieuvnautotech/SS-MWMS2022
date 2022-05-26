//---------------bntFinishMaterial------------------//
function bntFinishMaterial(cellValue, options, rowdata, action) {
    var html = "";
    if (rowdata.sd_sts_cd == "000") {
        html = '<button' +
            ' data-sid=' + rowdata.sid +
            ' data-sd_no=' + rowdata.sd_no +
            ' data-alert=' + rowdata.alert +
            ' class="btn btn-xs btn-success" onclick="FinishMaterial_Row(this)">F</button>';
    }
    return html;
}
function FinishMaterial_Row(e) {
    var confirmFinish = confirm("Bạn có muốn hoàn thành SD này !");
    if (confirmFinish) {
        var alert = e.dataset.alert;
        $.get("/wipwms/FinishMaterialWIP?" +
            "sid=" + $(e).data("sid") +
            "&sd_no=" + $(e).data("sd_no") ,
            function (res) {
                if (res.result != 0) {
                    $("#searchBtn").trigger("click");
                }
            });
    } else {
       
    }
  
}
$(function () {
    $("#list").jqGrid
        ({
            //url: "/wipwms/GetPickingScan",
            //datatype: 'json',
            datatype: function (postData) { getListGetPickingScan(postData); },
            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                //{ label: 'Finish', name: 'sd_no', width: 50, align: 'center', formatter: bntFinishMaterial },
                { name: "FB", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntMissingMaterial },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'center', formatter: SD_popup },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'left', hidden: true },
                { label: 'SD Name', name: 'sd_nm', width: 200, align: 'left' },
                { label: 'Product', name: 'product_cd', width: 200, align: 'left' },
                { label: 'Status', name: 'sts_nm', width: 150, align: 'center' },
                { label: 'Status', name: 'sd_sts_cd', width: 150, align: 'center', hidden: true },
                { label: 'Location', name: 'lct_nm', width: 150, align: 'left', hidden: true },
                { label: 'Location', name: 'lct_cd', width: 150, align: 'left', hidden: true },
                { label: 'alert', name: 'alert', width: 180, align: 'left', hidden: true },
                { label: 'Remark', name: 'remark', width: 180, align: 'left' },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);

                if (rowData.sd_sts_cd != "000") {
                    $("#Save_completed").attr("disabled", true);
                }
                else {
                    $("#Save_completed").attr("disabled", false);
                }
                //$("#list1").setGridParam({ url: "/wipwms/GetPickingScanMLQR?sd_no=" + rowData.sd_no, datatype: "json" }).trigger("reloadGrid");
                //bao add
                $("#list1").setGridParam({ url: "/wipwms/GetPickingScanMLQRTotal?sd_no=" + rowData.sd_no, datatype: "json" }).trigger("reloadGrid");
                $("#m_sd_no").val(rowData.sd_no);
                //if (rowData.sd_sts_cd == "000") {
                //    Update_Sd(rowData.sd_no);
                //}
            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: false, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            gridview: true,
            hoverrows: false,
            caption: 'SD Infomation',
            emptyrecords: "No data.",
            height: 200,
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
            gridComplete: function (result) {
                //debugger;
                var id_no = $("#id_no").val();
                if (id_no != "") {
                    $("#list").jqGrid('setSelection', id_no, { background: "#5cb85c !important", color: "#fff" });
                }
                var rows = $("#list").getDataIDs();
                for (var i = 0; i < rows.length; i++) {
                    var sd_sts_cd = $("#list").getCell(rows[i], "sd_sts_cd");
                    var alert = $("#list").getCell(rows[i], "alert");

                    //if (sd_sts_cd == "000") {

                    //    $("#list").jqGrid('setRowData', rows[i], false, { background: "rgb(244,177,131)" } );
                    //}
                    //else {
                    //    $("#list").jqGrid('setRowData', rows[i], false, { background: 'rgb(164,204,145)' });

                    //}
                    if (alert == "0") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(164, 204, 145)',
                        });
                    }
                    else if (alert == "2") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(244,177,131) ',
                        });
                    }
                    if (sd_sts_cd == "001") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            background: 'rgb(164,204,145)',
                        });
                    }

                }


            }
        });
});
function Update_Sd(e) {

    $.ajax({
        url: "/wipwms/UpStatusSd?sd_no=" + e,
        type: "get",
        dataType: "json",

        success: function (response) {
            if (response.result) {
                var id1 = response.data1[0].sid;


                $("#list").setRowData(id1, response.data1[0], { background: "#28a745", color: "#fff" });
                return false;
            }
            else {
                return false;
            }

        }
    });
};
$(function () {
    //$("#list1").jqGrid
    //    ({
    //        datatype: 'json',
    //        mtype: 'Get',
    //        colModel: [
    //            { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true },
    //            { key: false, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
    //            { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
    //            { label: 'Lot No', name: 'lot_no', width: 100, align: 'left' },
    //            { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
    //            { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
    //            { label: 'Status', name: 'sts_nm', width: 120, align: 'center' },

    //            { label: 'Expiry Date', name: 'expiry_dt', width: 180, align: 'center', formatter: convertDate },
    //            { label: 'Date of Receipt', name: 'dt_of_receipt', width: 180, align: 'center', formatter: convertDate },
    //            { label: 'Expore Date', name: 'expore_dt', width: 180, align: 'center', formatter: convertDate },
    //            { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete, hidden: true },
    //            { label: 'selected', name: 'selected', width: 100, align: 'right', fomatter: 'integer', formatter: selected, hidden: true },
    //        ],
    //        pager: jQuery('#listPager1'),
    //        rowNum: 500,
    //        rowList: [500, 1000],
    //        loadonce: true, //tải lại dữ liệu
    //        viewrecords: true,
    //        rownumbers: true,
    //        hoverrows: false,
    //        caption: '',
    //        emptyrecords: "No data.",
    //        height: 250,
    //        width: null,
    //        autowidth: false,
    //        shrinkToFit: false,
    //        jsonReader:
    //        {
    //            root: "rows",
    //            page: "page",
    //            total: "total",
    //            records: "records",
    //            repeatitems: false,
    //            Id: "0"
    //        },
    //        gridComplete: function () {

    //            document.getElementById("remain_qty").value = "Remaining: 0";
    //            document.getElementById("remain_qty1").value = 0;
    //            var rowcount = parseInt($("#list1").getGridParam("reccount"));
    //            if (rowcount != 0) {

    //                var recort_new = 0;

    //                var allRowsInGrid = $('#list1').jqGrid('getGridParam', 'data');
    //                for (var i = 0; i < allRowsInGrid.length; i++) {
    //                    var mt_sts_cd = allRowsInGrid[i].mt_sts_cd;
    //                    if (mt_sts_cd == "000") {
    //                        recort_new += 1;
    //                    }

    //                    if (mt_sts_cd == "000") {
    //                        $("#list1").jqGrid('setCell', allRowsInGrid[i].id, "sts_nm", "", {
    //                            'background-color': 'rgb(244,177,131) ',
    //                        });
    //                    }
    //                    if (mt_sts_cd == "001") {
    //                        $("#list1").jqGrid('setCell', allRowsInGrid[i].id, "sts_nm", "", {
    //                            'background-color': 'rgb(164,204,145)',
    //                        });
    //                    }
    //                }
    //                document.getElementById("remain_qty").value = "Remaining: " + recort_new;
    //                document.getElementById("remain_qty1").value = rowcount;
    //            }
    //        }
    //    });
    // comment lai để sửa theo cái grid của nga chỉ 
    //$("#list1").jqGrid
    //    ({
    //        datatype: 'json',
    //        mtype: 'Get',
    //        colModel: [
    //            { key: false, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
    //            { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
    //            { label: 'Lot No', name: 'lot_no', width: 100, align: 'left' },
    //            { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
    //            { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
    //            { label: 'Status', name: 'sts_nm', width: 120, align: 'center' },

    //            { label: 'Expiry Date', name: 'expiry_dt', width: 180, align: 'center', formatter: convertDate },
    //            { label: 'Date of Receipt', name: 'dt_of_receipt', width: 180, align: 'center', formatter: convertDate },
    //            { label: 'Expore Date', name: 'expore_dt', width: 180, align: 'center', formatter: convertDate },
    //            { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete, hidden: true },
    //            { label: 'selected', name: 'selected', width: 100, align: 'right', fomatter: 'integer', formatter: selected, hidden: true },
    //        ],
    //        pager: jQuery('#listPager1'),
    //        rowNum: 500,
    //        rowList: [500, 1000],
    //        loadonce: true, //tải lại dữ liệu
    //        viewrecords: true,
    //        rownumbers: true,
    //        hoverrows: false,
    //        caption: '',
    //        emptyrecords: "No data.",
    //        height: 250,
    //        width: null,
    //        autowidth: false,
    //        shrinkToFit: false,
    //        jsonReader:
    //        {
    //            root: "rows",
    //            page: "page",
    //            total: "total",
    //            records: "records",
    //            repeatitems: false,
    //            Id: "0"
    //        },
    //        gridComplete: function () {
    //            document.getElementById("remain_qty").value = "Remaining: 0";
    //            document.getElementById("remain_qty1").value = 0;
    //            var rowcount = parseInt($("#list1").getGridParam("reccount"));
    //            if (rowcount != 0) {

    //                var recort_new = 0;

    //                var allRowsInGrid = $('#list1').jqGrid('getGridParam', 'data');
    //                for (var i = 0; i < allRowsInGrid.length; i++) {
    //                    var mt_sts_cd = allRowsInGrid[i].mt_sts_cd;
    //                    if (mt_sts_cd == "000") {
    //                        recort_new += 1;
    //                    }

    //                    if (mt_sts_cd == "000") {
    //                        $("#list1").jqGrid('setCell', allRowsInGrid[i].id, "sts_nm", "", {
    //                            'background-color': 'rgb(244,177,131) ',
    //                        });
    //                    }
    //                    if (mt_sts_cd == "001") {
    //                        $("#list1").jqGrid('setCell', allRowsInGrid[i].id, "sts_nm", "", {
    //                            'background-color': 'rgb(164,204,145)',
    //                        });
    //                    }
    //                }
    //                document.getElementById("remain_qty").value = "Remaining: " + recort_new;
    //                document.getElementById("remain_qty1").value = rowcount;
    //            }
    //        }
    //    });
    //bao add
});
    $("#list1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: false, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { key: true, label: 'Mã Nguyên Liệu', name: 'mt_no', width: 300, align: 'left' },
                { label: 'Số Lượng Cấp(Tem)', name: 'SoLuongCap', width: 180, align: 'right' },
                { label: 'Số Mét', name: 'meter', width: 150, align: 'right', formatter: 'integer' },
                { label: 'Số Lượng Nhận', name: 'SoLuongNhanDuoc', width: 180, align: 'right' },
                { label: 'Còn Lại', name: 'SoluongConLai', width: 180, align: 'right' }
            ],
            pager: jQuery('#listPager1'),
            rowNum: 500,
            rowList: [500, 1000],
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
            footerrow: true,
            userDataOnFooter: true,
            gridComplete: function () {
                $('#list1').jqGrid('getGridParam', 'data');
               
                var rows = $("#list1").getDataIDs();
                var sum_Quantity = 0;
                var sumMeter_Quantity = 0;
                var sumSoluongNhan_Quantity = 0;
                var sumSoluongconlai_Quantity = 0;
                var $self = $(this)
                for (var i = 0; i < rows.length; i++) {
                    sum_Quantity += parseInt($("#list1").getCell(rows[i], "SoLuongCap"));
                    sumMeter_Quantity += parseInt($("#list1").getCell(rows[i], "meter")); 

                    var SoluongConLai = $("#list1").getCell(rows[i], "SoluongConLai");
                    sumSoluongNhan_Quantity += parseInt($("#list1").getCell(rows[i], "SoLuongNhanDuoc"));
                    sumSoluongconlai_Quantity += parseInt($("#list1").getCell(rows[i], "SoluongConLai")); 


                    if (SoluongConLai == "0") {
                        $("#list1").jqGrid('setCell', rows[i], "SoluongConLai", "", {
                            'background-color': 'rgb(164, 204, 145)',
                        });
                    }
                }
                $self.jqGrid("footerData", "set", { mt_no: "Total" });
                $self.jqGrid("footerData", "set", { SoLuongCap: sum_Quantity });
                $self.jqGrid("footerData", "set", { meter: sumMeter_Quantity });
                $self.jqGrid("footerData", "set", { SoLuongNhanDuoc: sumSoluongNhan_Quantity });
                $self.jqGrid("footerData", "set", { SoluongConLai: sumSoluongconlai_Quantity });
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }


        });



function bntDelete(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-danger" onclick="Del_Row(' + wmtid + ');">Delete</button>';
    return html;
}
function selected(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-danger" id="N-' + wmtid + '" name="' + wmtid + '" >fff</button>';
    return html;
}
//-------SEARCH------------//
$("#searchBtn").click(function () {
    //debugger;
    var pdata = $('#list').jqGrid('getGridParam', 'postData');
    getListGetPickingScan(pdata);
});
function getListGetPickingScan(pdata) {
    //debugger;
    var sd_no = $('#s_sd_no').val().trim();
    var sd_nm = $('#s_sd_nm').val().trim();
    var product_cd = $('#c_style_no').val().trim();
    let pages = 1;
    let rows = 50;
    var pdata = $('#list').jqGrid('getGridParam', 'postData');
    var params = new Object();
    if (pdata.page == 0) {
        pages = 1;
    }
    else {
        pages = pdata.page;
    }
    rows = pdata.rows;
    $.ajax({
        url: "/wipwms/GetPickingScan",
        type: "get",
        dataType: "json",
        data: {
            sd_no: sd_no,
            sd_nm: sd_nm,
            product_cd: product_cd,
            page: pages,
            rows: rows,
        },
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#list')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
        //success: function (result, st) {
        //    if (st == "success") {
        //        var showing = $('#list')[0];
        //        showing.addJSONData(result);
        //    }
        //},
        //error: function () {
        //    return;
        //}
        //success: function (result) {
        //    //$("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        //    var showing = $('#popupbobindv')[0];
        //    showing.addJSONData(result);
        //}
    });
}
//function Del_Row(wmtid) {

//    $("#list1").jqGrid('delRowData', wmtid);

//}

//$("#ml_cd").on("keydown", function (e) {
//    if (e.keyCode === 13) {

//        var ml_cd = e.target.value.trim();
//        var allRowsInGrid = $('#list1').jqGrid('getGridParam', 'data');



//        for (i = 0; i < allRowsInGrid.length; i++) {
//            var ml_cd_view = allRowsInGrid[i].mt_cd;

//            if (ml_cd == ml_cd_view) {
//                var id = allRowsInGrid[i].wmtid;

//                var val = document.getElementById('N-'+id+'').value;
//                if (val == 1) {
//                    $("#ml_cd").val("");
//                    document.getElementById("ml_cd").focus();
//                    alert(' ' + ml_cd_view +' has been chosen !')

//                    return false;

//                }


//                $("#list1").setRowData(id, allRowsInGrid[i], { background: "#28a745", color: "#fff" });

//                var remainQty = $("#remain_qty1").val();

//                document.getElementById("remain_qty1").value = remainQty - 1;
//                document.getElementById("remain_qty").value = "Remaining: " + (remainQty - 1);

//                $("[name='" + id + "']").val(1);

//                $("#ml_cd").val("");
//                document.getElementById("ml_cd").focus();

//                return false;
//            }

//        }
//        $("#ml_cd").val("");
//        document.getElementById("ml_cd").focus();

//        alert('Data does not exist !')

//    }
//});
//$("#ml_cd").on("keydown", function (e) {

//    if (e.keyCode === 13) {
//        //kiem tra ml_no null ko
//        var ml_cd = e.target.value.trim();
//        if (ml_cd == "" || ml_cd == null || ml_cd == undefined) {
//            return false;
//            $("#ml_cd").val("");
//            document.getElementById("ml_cd").focus();
//        }
//        //ktra sd_no
//        var sd_no = $("#m_sd_no").val();
//        if (sd_no == "" || sd_no == null || sd_no == undefined) {
//            ErrorAlert("Vui lòng chọn mã SD!!!")
//            return false;
//        }
//        //ktra tren luoi co ton tai khong
//        var rows = $('#list1').jqGrid('getGridParam', 'data');
//        var flag = false;
//        for (i = 0; i < rows.length; i++) {
//            var ml_cd_view = rows[i].mt_cd;
//            var wmtid = rows[i].wmtid;
//            var id = rows[i].id;
//            if (ml_cd == ml_cd_view) {
//                flag = true;
//                var val = rows[i].mt_sts_cd;

//                break;
//            }
//        }

//        if (flag) {


//            if (val != '000') {
//                $("#ml_cd").val("");
//                document.getElementById("ml_cd").focus();
//                ErrorAlert(' ' + ml_cd_view + 'Đã được đưa vào kho" !')

//                return false;

//            }

//            $.ajax({
//                url: "/wipwms/InsertMTQRSDList?data=" + wmtid + "&sd_no=" + sd_no,
//                type: "get",
//                dataType: "json",
//                data: {
//                    ml_cd: ml_cd
//                },
//                success: function (response) {
//                    if (response.result) {

//                        var id1 = response.data1[0].sid;
//                        var id2 = response.data2.id;

//                        $("#list").setRowData(id1, response.data1[0], { background: "#28a745", color: "#fff" });

//                        $("#list1").jqGrid('delRowData', id);

//                        $("#list1").jqGrid('addRowData', id2, response.data2, 'first');
//                        $("#list1").setRowData(id2, false, { background: "#28a745", color: "#fff" });

//                        document.getElementById("remain_qty1").value = response.kt_sd;
//                        document.getElementById("remain_qty").value = "Remaining: " + response.kt_sd;

//                        $("[name='" + id + "']").val(1);

//                        $("#ml_cd").val("");
//                        document.getElementById("ml_cd").focus();




//                        var timer = setTimeout(
//                            function () {
//                                $("#list1").jqGrid('delRowData', id2);
//                                clearTimeout(timer)
//                            }, 6000);


//                        return false;
//                    }
//                    else {
//                        ErrorAlert(response.message);
//                        $("#ml_cd").val("");
//                        document.getElementById("ml_cd").focus();
//                    }

//                }
//            });

//        }
//        else {
//            ErrorAlert('Dữ liệu không tồn tại');
//            $("#ml_cd").val("");
//            document.getElementById("ml_cd").focus();
//        }

//    }

//});

$("#ml_cd").on("keydown", function (e) {
    //debugger;
    if (e.keyCode === 13) {
        $.blockUI({ message: '' });
        //kiem tra ml_no null ko
        var ml_cd = e.target.value.trim();
        var LocationCode = $("#SelectRack").val().trim();
        if (ml_cd == "" || ml_cd == null || ml_cd == undefined) {

            $("#ml_cd").val("");
            document.getElementById("ml_cd").focus();
            $.unblockUI();
            return false;
        }
        if (LocationCode == "" || LocationCode == null || LocationCode == undefined) {

            $("#SelectRack").val("");
            document.getElementById("SelectRack").focus();
            ErrorAlert("Vui lòng chọn kệ");
            $.unblockUI();
            return false;
        }
        //ktra sd_no
        var sd_no = $("#m_sd_no").val();
        if (sd_no == "" || sd_no == null || sd_no == undefined) {
            $("#ml_cd").val("");
            document.getElementById("ml_cd").focus();
            $.blockUI({
                message: "Vui lòng chọn mã SD!!!",
                timeout: 5,
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

            //url: "/wipwms/InsertMTQRSDList?data=" + ml_cd + "&sd_no=" + sd_no,
            url: "/wipwms/ScanML_no_ReceiWIP2",
            type: "get",
            dataType: "json",
            data: {
                ml_no: ml_cd,
                sd_no: sd_no,
                LocationCode: LocationCode
            },
            success: function (response) {
                if (response.result) {
                    debugger;
                    $("#list1").jqGrid('delRowData', response.data[0].mt_no);
                    $("#list1").jqGrid('addRowData', response.data[0].mt_no, response.data[0], 'first');
                    $("#list1").setRowData(response.data[0].mt_no, false, { background: "#28a745", color: "#fff" });

                    SuccessAlert(response.message);
                    $("#ml_cd").val("");

                    document.getElementById("ml_cd").focus();

                    $.unblockUI();
                    return false;
                }
                else {

                    debugger;
                    $("#ml_cd").val("");
                    document.getElementById("ml_cd").focus();

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

//----- SD POPUP------------------//
function SD_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-sd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewSDPopup(e) {
    $('.popup-dialog.SD_Info_Popup').dialog('open');
    var sd_no = $(e).data('sd_no');


    $.get("/wipwms/PartialView_WIP_Recei_SD_Popup?" +
        "sd_no=" + sd_no + ""
        , function (html) {
            $("#PartialView_WIP_Recei_SD_Popup").html(html);
        });
}



//---------------bntMissingMaterial------------------//

function bntMissingMaterial(cellValue, options, rowdata, action) {
    var html = "";
    if (rowdata.sd_sts_cd == "000") {


        html = '<button' +
            ' data-sid=' + rowdata.sid +
            ' data-sd_no=' + rowdata.sd_no +
            ' data-alert=' + rowdata.alert +
            ' class="btn btn-xs btn-warning" onclick="MissMaterial_Row(this)">FB</button>';
    }

    return html;

}


function MissMaterial_Row(e) {


    var alert = e.dataset.alert;


    $.get("/wipwms/PartialView_Receving_Scan_Wip_Missing_M_Popup?" +
        "sid=" + $(e).data("sid") +
        "&sd_no=" + $(e).data("sd_no") +
        "&alert=" + alert,
        function (html) {
            $("#PartialView_Receving_Scan_Wip_Missing_M_Popup").html(html);
        });
    $('.popup-dialog.Receving_Scan_Wip_Missing_M_Popup').dialog('open');



}

//----- help to focus on input------------------//
//setTimeout(function () { $("#ml_cd").focus(); }, 1);