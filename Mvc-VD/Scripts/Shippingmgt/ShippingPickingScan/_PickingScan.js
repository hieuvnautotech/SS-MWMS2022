$(function () {
    $("#list").jqGrid
        ({
            url: "/ShippingMgt/GetPickingScan",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'sid', name: 'sid', width: 50, align: 'center', hidden: true },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'center', formatter: SD_popup },
                { label: 'SD NO', name: 'sd_no', width: 100, align: 'left', hidden: true },
                { label: 'SD Name', name: 'sd_nm', width: 300, align: 'left' },
                { label: 'Product', name: 'product_cd', width: 150, align: 'left' },
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



                $("#m_sid").val(rowData.sid);
                $("#m_sd_nm").val(rowData.sd_nm);
                $("#m_sd_no").val(rowData.sd_no);
                $("#m_style_no").val(rowData.product_cd);
                $("#m_remark").val(rowData.remark);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("show");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").addClass("active");

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);


                $("#list2").setGridParam({ url: "/ShippingMgt/Getshippingsdmaterial?sd_no=" + rowData.sd_no, datatype: "json" }).trigger("reloadGrid");


            },

            pager: jQuery('#listPager'),
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            loadonce: true, //tải lại dữ liệu
            viewrecords: true,
            rownumbers: true,
            hoverrows: false,
            caption: 'Picking Scan',
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
            gridComplete: function () {

                var id_no = $("#id_no").val();
                if (id_no != "") {
                    $("#list").jqGrid('setSelection', id_no, { background: "#28a745 !important", color: "#fff" });
                }
                var rows = $("#list").getDataIDs();

                for (var i = 0; i < rows.length; i++) {
                    var sd_sts_cd = $("#list").getCell(rows[i], "sd_sts_cd");
                    var alert = $("#list").getCell(rows[i], "alert");

                    if (alert == "2") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(244,177,131) ',
                        });
                    }else if (alert == "0") {
                        $("#list").jqGrid('setCell', rows[i], "sts_nm", "", {
                            'background-color': 'rgb(164, 204, 145)',
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

//-------INSERT------------//
$("#c_save_but").click(function () {
    debugger;
    var isValid = $('#form1').valid();
    if (isValid == true) {
        var sd_nm = $('#c_sd_nm').val();
        var remark = $('#c_remark').val();
        var product_cd = $('#c_style_no').val().trim();

        {
            $.ajax({
                url: "/ShippingMgt/InsertSDInfo",
                type: "Post",
                dataType: "json",
                data: {
                    sd_nm: sd_nm,
                    remark: remark,
                    product_cd: product_cd,
                },

                success: function (response) {
                    if (response.result) {
                        var id = response.data[0].sid;
                        $("#list").jqGrid('addRowData', id, response.data[0], 'first');
                        $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });
                        SuccessAlert(response.message)
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

    var sid = $('#m_sid').val();
    var sd_nm = $('#m_sd_nm').val();
    var remark = $('#m_remark').val();
    var product_cd = $('#m_style_no').val();


    $.ajax({
        url: "/ShippingMgt/UpdateSDInfo",
        type: "get",
        dataType: "json",
        data: {
            sid: sid,
            sd_nm: sd_nm,
            product_cd: product_cd,
            remark: remark

        },
        success: function (response) {
            if (response.result) {
                var id = response.data.sid;
                $("#list").setRowData(id, response.data, { background: "#28a745", color: "#fff" });
                SuccessAlert(response.message);

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
    var r = confirm("Bạn có chắc muốn Xóa không???");
    if (r === true) {
        var m_sid = $("#m_sid").val();
        var m_sd_no = $("#m_sd_no").val();
        $.ajax({
            url: "/ShippingMgt/DeleteSDInfo?sid=" + m_sid + "&sd_no=" + m_sd_no ,
            type: "POST",
            success: function (response) {
                if (response.result) {

                    $('#list').jqGrid('delRowData', m_sid);
                    document.getElementById("form2").reset();
                    document.getElementById("form1").reset();
                    $("#tab_2").removeClass("active");
                    $("#tab_1").addClass("active");
                    $("#tab_c2").removeClass("show");
                    $("#tab_c2").removeClass("active");
                    $("#tab_c1").addClass("active");

                    SuccessAlert(response.message);
                    $("#list2").setGridParam({ url: "/ShippingMgt/Getshippingsdmaterial?sd_no=" + m_sd_no, datatype: "json" }).trigger("reloadGrid");

                } else {
                    ErrorAlert(response.message);
                }
            }
        });
    }
});

//-------SEARCH------------//


$("#searchBtn").click(function () {
    debugger;
    var sd_no = $('#s_sd_no').val().trim();
    var sd_nm = $('#s_sd_nm').val().trim();
    var product_cd = $('#s_style_no').val().trim();
    var remark = $('#remark').val().trim();

    $.ajax({
        url: "/ShippingMgt/GetPickingScan",

        type: "get",
        dataType: "json",
        data: {
            sd_no: sd_no,
            sd_nm: sd_nm,
            product_cd: product_cd,
            remark: remark
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});



$(function () {
    $("#list1").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
                { label: 'Lot No', name: 'lot_no', width: 100, align: 'right' },
                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', fomatter: 'integer' },
                { label: 'Expiry Date', name: 'expiry_dt', width: 180, align: 'center', formatter: convertDate },
                { label: 'Date of Receipt', name: 'dt_of_receipt', width: 180, align: 'center', formatter: convertDate },
                { label: 'Expore Date', name: 'expore_dt', width: 180, align: 'center', formatter: convertDate },
                { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);


                $("#m_sid").val(rowData.sid);
                $("#m_sd_nm").val(rowData.sd_nm);
                $("#m_remark").val(rowData.remark);

                $("#tab_1").removeClass("active");
                $("#tab_2").addClass("active");
                $("#tab_c1").removeClass("show");
                $("#tab_c1").removeClass("active");
                $("#tab_c2").addClass("active");

                $("#m_save_but").attr("disabled", false);
                $("#del_save_but").attr("disabled", false);



            },

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
            footerrow: true,
            userDataOnFooter: true,
            gridComplete: function () {
                //prevCellVal = { cellId: undefined, value: undefined };
                //var rowIds = gridProduct.getDataIDs();
                //rowIds.forEach(GridScanProduct_SetRowColor);

                //sum Quantity
                var ids = $("#list1").jqGrid('getDataIDs');
                var sum_Quantity = 0;
                var $self = $(this)
                for (var i = 0; i < ids.length; i++) {
                    sum_Quantity += parseInt($("#list1").getCell(ids[i], "gr_qty"));
                }

                $self.jqGrid("footerData", "set", { lot_no: "Total" });
                $self.jqGrid("footerData", "set", { gr_qty: sum_Quantity });

                jQuery("#list1").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

            },

        });
});
$("#ml_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var ml_cd = e.target.value.trim();

        //var rows = $('#list1').jqGrid('getGridParam', 'data');
        var rows = $('#list1').jqGrid('getRowData');
        for (i = 0; i < rows.length; i++) {
            var ml_cd_view = rows[i].mt_cd;

            if (ml_cd == ml_cd_view) {
                document.getElementById("ml_cd").focus();
                $("#ml_cd").val("");
                ErrorAlert("Dữ liệu đã scan rồi !!!");
                return false;

            }
        }
        $.ajax({
            url: "/ShippingMgt/GetPickingScanMLQR",
            type: "get",
            dataType: "json",
            data: {
                ml_cd: ml_cd
            },
            success: function (response) {
                if (response.result) {
                    var id = response.data.wmtid;
                    $("#list1").jqGrid('addRowData', id, response.data, 'first');
                    $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    document.getElementById("ml_cd").focus();
                    $("#ml_cd").val("");
                } else
                {
                    document.getElementById("ml_cd").focus();
                    $("#ml_cd").val("");
                    ErrorAlert(response.message);
                }
                $("#ml_cd").val("");

            }
        });
    }
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
function bntDelete(cellValue, options, rowdata, action) {
    var wmtid = rowdata.wmtid;
    var html = '<button class="btn btn-xs btn-danger" onclick="Del_Row(' + wmtid + ');">Delete</button>';
    return html;
}
function Del_Row(wmtid) {
    $("#list1").jqGrid('delRowData', wmtid);
}

$("#Clear_all").on("click", function () {
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {
        $("#list1").jqGrid('clearGridData');
    }

});

$("#Save_completed").on("click", function () {

    var sd_no = $("#m_sd_no").val();
    if (sd_no == "" || sd_no == null || sd_no == undefined) {
        ErrorAlert("Vui lòng chọn mã SD!!!")
        return false;
    }

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        ErrorAlert("Vui lòng Scan!!!")
            return false;
    }

        $.ajax({
            url: "/ShippingMgt/InsertMTQRSDList?data=" + rows + "&sd_no=" + sd_no,
            type: "get",
            dataType: "json",

            success: function (data) {

                if (data.result) {
                    SuccessAlert(data.message);
                    $("#list1").jqGrid('clearGridData');
                }
                else {
                    ErrorAlert(data.message);
                }
            }
        });

});

//----- SD POPUP------------------//
function SD_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-sd_no="' + cellValue + '" onclick="ViewSDPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewSDPopup(e) {
    $('.popup-dialog.SD_Info_Popup').dialog('open');
    var sd_no = $(e).data('sd_no');


    $.get("/ShippingMgt/PartialView_SD_Info_Popup?" +
        "sd_no=" + sd_no + ""
        , function (html) {
            $("#PartialView_SD_Info_Popup").html(html);
        });
}

$("#Select_completed").on("click", function () {

    var sd_no = $("#m_sd_no").val();
    if (sd_no == "" || sd_no == null || sd_no == undefined) {
        ErrorAlert("Vui lòng chọn mã SD!!!")
        return false;
    }


    $('.popup-dialog.List_ML_NO_Info_Popup').dialog('open');



    $.get("/ShippingMgt/PartialView_List_ML_NO_Info_Popup?" +
        "sd_no=" + sd_no + ""
        , function (html) {
            $("#PartialView_List_ML_NO_Info_Popup").html(html);
        });


});

$("#Create_NVL").on("click", function () {

    var sd_no = $("#m_sd_no").val();
    if (sd_no == "" || sd_no == null || sd_no == undefined) {
        ErrorAlert("Vui lòng chọn mã SD!!!")
        return false;
    }


    $('.popup-dialog.Create_List_ML_NO_Info_Popup').dialog('open');



    $.get("/ShippingMgt/PartialView_Create_List_ML_NO_Info_Popup?" +
        "sd_no=" + sd_no + ""
        , function (html) {
            $("#PartialView_Create_List_ML_NO_Info_Popup").html(html);
        });


});
$("#PickingClick").on("click", function () {

    var sd_no = $("#m_sd_no").val();
    if (sd_no == "" || sd_no == null || sd_no == undefined) {
        ErrorAlert("Vui lòng chọn mã SD!!!")
        return false;
    }


    $('.popup-dialog.ListMaterialNo').dialog('open');



    $.get("/ShippingMgt/PartialView_ListMaterialNoPopup?" +
        "sd_no=" + sd_no + ""
        , function (html) {
            $("#PartialView_ListMaterialNoPopup").html(html);
        });


});

//VIEW LƯỚI 2

//$(function () {
//    $("#list2").jqGrid
//        ({
//            datatype: 'json',
//            mtype: 'Get',
//            colModel: [
//                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center',hidden:true },
//                { label: 'Mã nguyên vật liệu', name: 'mt_no', width: 300, align: 'left' },
//                { label: 'Số lượng cấp(Tem)', name: 'SoLuongCap', width: 150, align: 'right', formatter: 'integer' },
//                { label: 'Số Mét', name: 'meter', width: 150, align: 'right', formatter: 'integer' },
//                { label: 'Số lượng nhận', name: 'SoLuongNhanDuoc', width: 150, align: 'right', formatter: 'integer' },
//                { label: 'Còn lại', name: 'SoluongConLai', width: 150, align: 'right', formatter: 'integer' },

//            ],
//            onSelectRow: function (rowid, selected, status, e) {
//                var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
//                var rowData = $("#list2").getRowData(selectedRowId);

//            },

//            pager: jQuery('#listPager2'),
//            rowNum: 50,
//            rowList: [50, 100, 200, 500, 1000],
//            loadonce: true, //tải lại dữ liệu
//            viewrecords: true,
//            rownumbers: true,
//            hoverrows: false,
//            caption: '',
//            emptyrecords: "No data.",
//            height: 250,
//            width: null,
//            autowidth: false,
//            shrinkToFit: false,
//            footerrow: true,
//            userDataOnFooter: true,
//            gridComplete: function () {

//                //sum Quantity
//                var ids = $("#list2").jqGrid('getDataIDs');
//                var sum_Quantity = 0;
//                var sumMeter_Quantity = 0;
//                var sumSoluongNhan_Quantity = 0;
//                var sumSoluongconlai_Quantity = 0;
//                var $self = $(this)
//                for (var i = 0; i < ids.length; i++) {
//                    sum_Quantity += parseInt($("#list2").getCell(ids[i], "SoLuongCap"));
//                    sumMeter_Quantity += parseInt($("#list2").getCell(ids[i], "meter"));
//                    sumSoluongNhan_Quantity += parseInt($("#list2").getCell(ids[i], "SoLuongNhanDuoc"));
//                    sumSoluongconlai_Quantity += parseInt($("#list2").getCell(ids[i], "SoluongConLai"));

//                    var SoluongConLai = $("#list2").getCell(ids[i], "SoluongConLai");

//                    if (SoluongConLai == "0") {
//                        $("#list2").jqGrid('setCell', ids[i], "SoluongConLai", "", {
//                            'background-color': 'rgb(164, 204, 145)',
//                        });
//                    }
//                }

//                $self.jqGrid("footerData", "set", { mt_no: "Total" });
//                $self.jqGrid("footerData", "set", { SoLuongCap: sum_Quantity });
//                $self.jqGrid("footerData", "set", { meter: sumMeter_Quantity });
//                $self.jqGrid("footerData", "set", { SoLuongNhanDuoc: sumSoluongNhan_Quantity });
//                $self.jqGrid("footerData", "set", { SoluongConLai: sumSoluongconlai_Quantity });

//                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');

//            },
//            subGrid: true,
//            subGridRowExpanded: showChildGridMaterial,
//            jsonReader:
//            {
//                root: "rows",
//                page: "page",
//                total: "total",
//                records: "records",
//                repeatitems: false,
//                Id: "0"
//            },

//        });
//});
$(function () {
    $("#list2").jqGrid
        ({
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'Mã nguyên vật liệu', name: 'mt_no', width: 300, align: 'left' },
                { label: 'Số lượng cấp(Tem)', name: 'SoLuongCap', width: 150, align: 'right', formatter: 'integer' },
                { label: 'Số Mét', name: 'meter', width: 150, align: 'right', formatter: 'integer' },
                { label: 'Số lượng nhận', name: 'SoLuongNhanDuoc', width: 150, align: 'right', formatter: 'integer' },
                { label: 'Còn lại', name: 'SoluongConLai', width: 150, align: 'right', formatter: 'integer' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list2").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list2").getRowData(selectedRowId);

            },

            pager: jQuery('#listPager2'),
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
            footerrow: true,
            userDataOnFooter: true,
            gridComplete: function () {

                //sum Quantity
                var ids = $("#list2").jqGrid('getDataIDs');
                var sum_Quantity = 0;
                var sumMeter_Quantity = 0;
                var sumSoluongNhan_Quantity = 0;
                var sumSoluongconlai_Quantity = 0;
                var $self = $(this)
                for (var i = 0; i < ids.length; i++) {
                    sum_Quantity += parseInt($("#list2").getCell(ids[i], "SoLuongCap"));
                    sumMeter_Quantity += parseInt($("#list2").getCell(ids[i], "meter"));
                    sumSoluongNhan_Quantity += parseInt($("#list2").getCell(ids[i], "SoLuongNhanDuoc"));
                    sumSoluongconlai_Quantity += parseInt($("#list2").getCell(ids[i], "SoluongConLai"));

                    var SoluongConLai = $("#list2").getCell(ids[i], "SoluongConLai");

                    if (SoluongConLai == "0") {
                        $("#list2").jqGrid('setCell', ids[i], "SoluongConLai", "", {
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

            },
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

        });
});
json_object = "";
tempList = [];
$(`#excelupload_input`).change(function (evt) {
    var selectedFile = evt.target.files[0];
    var reader = new FileReader();
    var excelData = [];
    reader.onload = function (event) {
        var data = event.target.result;
        var workbook = XLSX.read(data, { type: 'binary' });
        workbook.SheetNames.forEach(function (sheetName) {
            var XL_row_object = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
            for (var i = 0; i < XL_row_object.length; i++) {
                var MaterialNo = ``;
                var Quantity = ``;
                var Meter = ``;
                if (XL_row_object[i]["MaterialNo"] != undefined) {
                    MaterialNo = XL_row_object[i]["MaterialNo"];
                    Quantity = XL_row_object[i]["Quantity(roll)"];
                    Meter = XL_row_object[i]["Meter"];
                    //debugger;
                    let Obj = {};
                    Obj.MaterialNo = MaterialNo;
                    Obj.Quantity = Quantity;
                    Obj.Meter = Meter;
                    //excelData.push(MaterialNo);
                    //excelData.push(Quantity);
                    //excelData.push(Meter);
                    excelData.push(Obj);
                }

            }
        });

        json_object = excelData;

    };

    reader.onerror = function (event) {
        console.error("File could not be read! Code " + event.target.error.code);
    };
    reader.readAsBinaryString(selectedFile);
});

//EXCEL UPLOAD
$(`#ExcelUpload`).on(`click`, function () {

    var sd_no = $("#m_sd_no").val();
    if (sd_no == "" || sd_no == null || sd_no == undefined) {
        ErrorAlert("Vui lòng chọn mã SD!!!")
        return false;
    }

    tempList = [];

    for (var item of json_object) {

        //debugger;

        data = {
            mt_no: item.MaterialNo,
            quantity: item.Quantity,
            meter: item.Meter,
            sd_no: sd_no
        };
        tempList.push(data);
    }
    $(`#excelupload_input`).val('');
    $.ajax({
        url: `/ShippingMgt/UploadExcel`,
        type: 'POST',
        async: true,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tempList),
        traditonal: true
    })
        .done(function (response) {
            if (response.result) {

                $("#list2").setGridParam({ url: "/ShippingMgt/Getshippingsdmaterial?sd_no=" + response.data, datatype: "json" }).trigger("reloadGrid");
                SuccessAlert(response.message);
                $(`#excelupload_input`).val(``);
                tempList = [];
                json_object = "";
                return;
            }
            else {
                ErrorAlert(response.message);

                return;
            }
        })
        .fail(function () {
            ErrorAlert(`Lỗi hệ thống.`);
        });
});
var childGridID = "";
function showChildGridMaterial(parentRowID, parentRowKey) {

    parentRowKey1 = parentRowKey;
    childGridID = parentRowID + "_table";
    var childGridPagerID = parentRowID + "_pager";
    $('#' + parentRowID).append('<table id=' + childGridID + '></table><div id=' + childGridPagerID + ' class=scroll></div>');

    var rowData = $('#list2').jqGrid('getRowData', parentRowKey);
    var sd_no = $("#m_sd_no").val();
    var mt_no = encodeURIComponent(rowData.mt_no);

    $("#" + childGridID).jqGrid({
        url: "/ShippingMgt/getdsMaterialDetail?mt_no=" + mt_no + "&sd_no=" + sd_no,
        mtype: "GET",
        datatype: "json",
        async: false,
        page: 1,
        colModel: [
            { key: true, label: 'id', name: 'id', width: 200, align: 'center',hidden:true },
            { key: false, label: 'Ngày tạo', name: 'reg_dt', align: 'center', width: 265, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
            {
                key: false, label: 'Số lượng cấp(Tem)', name: 'quantity', width: 150, align: 'right', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, editoptions: {
                    dataInit: function (element) {
                        $(element).keypress(function (e) {
                            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                return false;
                            }
                        });
                        $(element).keydown(function (e) {
                            if (e.which == 13) {
                                var XacNhan = confirm("Bạn có chắc chưa?");
                                if (XacNhan) {

                                    var Qty = this.value;

                                    if ((Qty == "") || (Qty == undefined) || Qty == 0) {
                                        ErrorAlert("Vui lòng nhập số lượng");
                                        return false;
                                    }

                                    var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
                                    row_id = $("#" + childGridID).getRowData(selectedRowId);
                                    $.ajax({
                                        url: '/ShippingMgt/updateSoCuon?id=' + row_id.id + "&Qty=" + Qty,
                                        type: "get",
                                        dataType: "json",
                                        success: function (data) {
                                            if (data.result) {
                                                SuccessAlert(data.message);
                                                var rowData = $("#" + childGridID).jqGrid('getRowData', row_id.id);
                                                rowData.quantity = Qty;
                                                $("#" + childGridID).setRowData(row_id.id, rowData, { background: "#d0e9c6" });
                                                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                            }
                                            else {
                                                ErrorAlert(data.message);
                                                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                            }

                                        },
                                        error: function () {
                                            ErrorAlert(data.message);
                                            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
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

            {
                key: false, label: 'Số Mét', name: 'meter', sortable: true, width: 150, align: 'right',formatter: 'integer', editable: true, editoptions: { size: 10, maxlength: 15 }, editrules: { integer: true, required: true }, editoptions: {
                dataInit: function (element) {
                    $(element).keypress(function (e) {
                        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                            return false;
                        }
                    });
                    $(element).keydown(function (e) {
                        if (e.which == 13) {
                            var XacNhan = confirm("Bạn có chắc chưa?");
                            if (XacNhan) {
                                 var Qty = this.value;

                                if ((Qty == "") || (Qty == undefined) || Qty == 0) {
                                    ErrorAlert("Vui lòng nhập số lượng");
                                return false;
                            }
                            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
                            row_id = $("#" + childGridID).getRowData(selectedRowId);
                                $.ajax({
                                    url: '/ShippingMgt/updateSoMeter?id=' + row_id.id + "&Qty=" + Qty,
                                    type: "get",
                                    dataType: "json",
                                    success: function (data) {
                                        if (data.result) {
                                            SuccessAlert(data.message);
                                            var rowData = $("#" + childGridID).jqGrid('getRowData', row_id.id);
                                            rowData.meter = Qty;
                                            $("#" + childGridID).setRowData(row_id.id, rowData, { background: "#d0e9c6" });
                                            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                        }
                                        else {
                                            ErrorAlert(data.message);
                                            jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                        }

                                    },
                                    error: function () {
                                        ErrorAlert(data.message);
                                        jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
                                    }

                                });
                            }
                            else {
                                return false;
                            }
                        }
                    });
                }
            }},


           { name: '', label: 'Delete', width: 100, align: 'center', formatter: DeleteMaterial, exportcol: false },

        ],


        onCellSelect: editRow,


        shrinkToFit: false,
        loadonce: false,
        rowNum: 50,
        rowList: [50, 100, 200, 500, 1000],
        width: null,
        height: '100%',
        subGrid: false,
        rownumbers: true,

        multiPageSelection: true,
        rowList: [50, 100, 200, 500, 1000],
        viewrecords: true,

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
        onSelectRow: function (rowid, status, e) {
            $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
            var selectedRowId = $("#" + childGridID).jqGrid("getGridParam", 'selrow');
            row_id = $("#" + childGridID).getRowData(selectedRowId);
            var lastSelection = "";

            if (rowid && rowid !== lastSelection) {
                var grid = $("#" + childGridID);
                grid.jqGrid('editRow', rowid, { keys: true, focusField: 2 });
                lastSelection = rowid;


            }

        },
        gridComplete: function () {

            var rows = jQuery("#" + childGridID).getDataIDs();
            for (var i = 0; i < rows.length; i++) {

            }


        }
    });
}
var lastSelection;
function editRow(id) {

    if (id && id !== lastSelection) {
        var grid = $("#" + childGridID);
        grid.jqGrid('editRow', id, { keys: true, focusField: 3 });
        lastSelection = id;
    }

    //var ret = "/SalesMgt/getLine2?bom_no=" + jQuery("#popups5").getRowData(id).bom_no;
    //jQuery("#popups5").setColProp("line_no", { editoptions: { dataUrl: ret } });
}
function DeleteMaterial(cellValue, options, rowdata, action) {
    return `<button  class="btn btn-sm btn-danger button-srh" data-id="${rowdata.id}" onclick="DeleteMaterialOnClick(this)">X</button>`;
}
function DeleteMaterialOnClick(e) {

    var XacNhan = confirm("Bạn có chắc chưa?");
    if (XacNhan) {
        var id = e.dataset.id;

        var settings = {
            "url": "/ShippingMgt/DelMaterialShippng",
            "method": "Delete",
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
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');


            }
            else {
                ErrorAlert(data.message);
                jQuery("#list2").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
            }
        });
    }
    else {
        return false;
    }

}
