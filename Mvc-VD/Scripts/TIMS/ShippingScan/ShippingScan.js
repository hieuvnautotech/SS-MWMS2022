$(function () {
    $("#list").jqGrid
        ({
            url: "/TIMS/GetEXTInfo",
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'extid', name: 'extid', width: 50, align: 'center', hidden: true },
                { label: 'EXT NO', name: 'ext_no', width: 100, align: 'center', formatter: EXT_popup },
                { label: 'EXT NO', name: 'ext_no', width: 100, align: 'left', hidden: true },
                { label: 'EXT Name', name: 'ext_nm', width: 200, align: 'left' },
                { label: 'Status', name: 'sts_nm', width: 150, align: 'center', hidden: true },
                { label: 'Status', name: 'ext_sts_cd', width: 150, align: 'center', hidden: true },
                { label: 'Shipping Date', name: 'work_dt', width: 150, align: 'center', formatter: convertDate },

                { label: 'Remark', name: 'remark', width: 180, align: 'left' },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
                var rowData = $("#list").getRowData(selectedRowId);


                $("#m_extid").val(rowData.extid);
                $("#m_ext_nm").val(rowData.ext_nm);
                $("#m_ext_no").val(rowData.ext_no);
                $("#m_remark").val(rowData.remark);

                $("#tab_1").removeClass("active");
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
            caption: 'Shipping Scan',
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

            }
        });
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

//-------INSERT------------//
$("#c_save_but").click(function () {
    var isValid = $('#form1').valid();
    if (isValid == true) {
        var ext_nm = $('#c_ext_nm').val();
        var remark = $('#c_remark').val();

        {
            $.ajax({
                url: "/TIMS/InsertEXTInfo",
                type: "get",
                dataType: "json",
                data: {
                    ext_nm: ext_nm,
                    remark: remark,
                },

                success: function (response) {
                    if (response.result) {

                        var id = response.data[0].extid;

                        $("#list").jqGrid('addRowData', id, response.data[0], 'first');
                        $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });

                    } else {
                        alert('Data already exists, Please check again!!!');
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

    var extid = $('#m_extid').val();
    var ext_nm = $('#m_ext_nm').val();
    var remark = $('#m_remark').val();


    $.ajax({
        url: "/TIMS/UpdateEXTInfo",
        type: "get",
        dataType: "json",
        data: {
            extid: extid,
            ext_nm: ext_nm,
            remark: remark

        },
        success: function (response) {
            if (response.result) {
                var id = response.data.extid;
                $("#list").setRowData(id, response.data, { background: "#28a745", color: "#fff" });


            } else {
                alert(response.message);
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
        var m_extid = $("#m_extid").val();
        $.ajax({
            url: "/TIMS/DeleteEXTInfo?extid=" + m_extid,
            type: "POST",
            success: function (response) {
                if (response.result) {

                    $('#list').jqGrid('delRowData', m_extid);
                    document.getElementById("form2").reset();
                    document.getElementById("form1").reset();
                    $("#tab_2").removeClass("active");
                    $("#tab_1").addClass("active");
                    $("#tab_c2").removeClass("show");
                    $("#tab_c2").removeClass("active");
                    $("#tab_c1").addClass("active");

                    alert(response.message);

                } else {
                    alert(response.message);
                }
            }
        });
    }
});

//-------SEARCH------------//
$("#searchBtn").click(function () {
    var ext_no = $('#s_ext_no').val().trim();
    var ext_nm = $('#s_ext_nm').val().trim();

    $.ajax({
        url: "/TIMS/GetEXTInfo",

        type: "get",
        dataType: "json",
        data: {
            ext_no: ext_no,
            ext_nm: ext_nm,

        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });
});

//----- EXT POPUP------------------//
function EXT_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-ext_no="' + cellValue + '" onclick="ViewEXTPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewEXTPopup(e) {
    $('.popup-dialog.EXT_Info_Popup').dialog('open');
    var ext_no = $(e).data('ext_no');


    $.get("/TIMS/PartialView_EXT_Info_Popup?" +
        "ext_no=" + ext_no + ""
        , function (html) {
            $("#PartialView_EXT_Info_Popup").html(html);
        });
}

$(function () {
    $("#list1").jqGrid
    ({
        datatype: 'json',
        mtype: 'Get',
      
            colModel: [
                { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 350, align: 'left' },
                { label: 'Bobbin No', name: 'bb_no', width: 120, align: 'left' },
                { label: 'Buyer QR', name: 'buyer_qr', width: 120, align: 'left' },

                { label: 'Material Type', name: 'mt_type_nm', width: 150, align: 'center' },
                { label: 'Material Type', name: 'mt_type', width: 180, align: 'center', hidden: true },

                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 100, align: 'center' },

                { label: 'Departure', name: 'from_lct_nm', width: 180, align: 'center' },
                { label: 'Departure Status', name: 'lct_sts_cd', width: 180, align: 'center' },
                { label: 'Receiving Date', name: 'recevice_dt_tims', width: 180, align: 'center', formatter: convertDate },
                { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete },
            ],

        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        pager: "#listPage1",
   
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 250,
        width: null,
        autowidth: false,
        rowNum: 50,
       
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        loadonce: true,
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
    })

});//grid1
$("#scan_buyer_qr").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var buyer_qr = e.target.value.trim();

        var rows = $('#list1').jqGrid('getRowData');

        for (i = 0; i < rows.length; i++) {
            var buyer_qr_view = rows[i].buyer_qr;

            if (buyer_qr == buyer_qr_view) {
                alert("Data Duplicated !!!");

                $("#scan_buyer_qr").val("");
                document.getElementById("scan_buyer_qr").focus();
                return false;

            }
        }

        $.ajax({
            url: "/TIMS/GetTimsShippingScanMLQR",

            type: "get",
            dataType: "json",
            data: {
                buyer_qr: buyer_qr
            },
            success: function (response) {
                if (response.result) {

                    for (var i = 0; i < response.data.length; i++) {
                        var id = response.data[i].wmtid;

                        $("#list1").jqGrid('addRowData', id, response.data[i], 'first');
                        $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });

                    }

                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();

                } else {
                    $("#scan_buyer_qr").val("");
                    document.getElementById("scan_buyer_qr").focus();
                    alert(response.message);
                }

            }
        });
    }
});


$("#Save_completed").on("click", function () {

    var ext_no = $("#m_ext_no").val();
    if (ext_no == "" || ext_no == null || ext_no == undefined) {
        alert("Please Choose EXT No!!!")
        return false;
    }

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        alert("Please Scan!!!")
        return false;
    }

    $.ajax({
        url: "/TIMS/UpdateMTQR_EXTList?data=" + rows + "&ext_no=" + ext_no,
        type: "get",
        dataType: "json",

        success: function (data) {

            if (data.result) {
                alert(data.message);
                $("#list1").jqGrid('clearGridData');
            }
            else {
                alert(data.message);
            }
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
$("#Clear_all").on("click", function () {

    var rows = $("#list1").getDataIDs();
    if (rows.length > 0) {
        var r = confirm("Are you make sure DELETE Events?");
        if (r === true) {
            $("#list1").jqGrid('clearGridData');
        }
    }
    else {
        alert("Please Scan!!!")
        return false;
    }

});
$("#Select_completed").on("click", function () {

    var ext_no = $("#m_ext_no").val();
    if (ext_no == "" || ext_no == null || ext_no == undefined) {
        alert("Please Choose EXT No!!!")
        return false;
    }

    $('.popup-dialog.List_ML_NO_Info_Popup').dialog('open');



    $.get("/TIMS/PartialView_GetList_ML_NO_Tims_Shipping_PP?" +
        "ext_no=" + ext_no + ""
        , function (html) {
            $("#PartialView_GetList_ML_NO_Tims_Shipping_PP").html(html);
        });


});

//----- help to focus on input------------------//
setTimeout(function () { $("#scan_buyer_qr").focus(); }, 1);







