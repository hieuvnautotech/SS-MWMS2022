//GRID
$(function () {
    $("#list").jqGrid({
        mtype: 'GET',
        datatype: function (postData) { getData(postData); },
        colModel: [
            { key: true, label: 'exid', name: 'exid', width: 50, align: 'center', hidden: true },
            { label: 'EX NO', name: 'ex_no', width: 100, align: 'center', formatter: EX_popup },
            { label: 'EX NO', name: 'ex_no', width: 100, align: 'left', hidden: true },
            { label: 'EX Name', name: 'ex_nm', width: 200, align: 'left' },
            { label: 'Status', name: 'sts_nm', width: 150, align: 'center', hidden: true },
            { label: 'Status', name: 'ex_sts_cd', width: 150, align: 'center', hidden: true },
            {
                label: 'Shipping Date', name: 'work_dt', width: 150, align: 'center' },

            { label: 'Remark', name: 'remark', width: 180, align: 'left' },
        ],
        pager: '#gridpager',
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
            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            var rowData = $("#list").getRowData(selectedRowId);


            $("#m_exid").val(rowData.exid);
            $("#m_ex_nm").val(rowData.ex_nm);
            $("#m_ex_no").val(rowData.ex_no);
            $("#m_remark").val(rowData.remark);

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

    if ($("#list").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;

    params.ex_no = $('#s_ex_no').val();
    params.ex_nm = $('#s_ex_nm').val();
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/wipwms/GetEXInfo',
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $("#list")[0];
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
   
    $("#list").jqGrid('setGridParam', { search: true });
    var pdata = $("#list").jqGrid('getGridParam', 'postData');
    getData(pdata);
});

//-------INSERT------------//
$("#c_save_but").click(function () {
    var isValid = $('#form1').valid();
    if (isValid == true) {
        var ex_nm = $('#c_ex_nm').val();
        var remark = $('#c_remark').val();

        {
            $.ajax({
                url: "/wipwms/InsertEXInfo",
                type: "get",
                dataType: "json",
                data: {
                    ex_nm: ex_nm,
                    remark: remark,
                },

                success: function (response) {
                    if (response.result) {

                        var id = response.data.exid;

                        $("#list").jqGrid('addRowData', id, response.data, 'first');
                        $("#list").setRowData(id, false, { background: "#28a745", color: "#fff" });

                        SuccessAlert(response.message);

                    } else {
                        ErrorAlert(response.message);
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

    var exid = $('#m_exid').val();
    var ex_nm = $('#m_ex_nm').val();
    var remark = $('#m_remark').val();


    $.ajax({
        url: "/wipwms/ModifyEXInfo",
        type: "get",
        dataType: "json",
        data: {
            exid: exid,
            ex_nm: ex_nm,
            remark: remark

        },
        success: function (response) {
            if (response.result) {
                var id = response.data.exid;
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
    var r = confirm("Are you make sure DELETE Events?");
    if (r === true) {
        var m_exid = $("#m_exid").val();
        $.ajax({
            url: "/wipwms/DeleteEXInfo?exid=" + m_exid,
            type: "POST",
            success: function (response) {
                if (response.result) {

                    $('#list').jqGrid('delRowData', m_exid);
                    document.getElementById("form2").reset();
                    document.getElementById("form1").reset();
                    $("#tab_2").removeClass("active");
                    $("#tab_1").addClass("active");
                    $("#tab_c2").removeClass("show");
                    $("#tab_c2").removeClass("active");
                    $("#tab_c1").addClass("active");

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
            url:'',
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                { key: true, label: 'id', name: 'id', width: 50, align: 'center', hidden: true },
                { key: false, label: 'wmtid', name: 'wmtid', width: 50, align: 'center', hidden: true },
                { label: 'ML NO', name: 'mt_cd', width: 300, align: 'left' },
                { label: 'MT No', name: 'mt_no', width: 100, align: 'left' },
                { label: 'MT Type', name: 'mt_type', width: 100, align: 'left' },
                { label: 'Lot No', name: 'lot_no', width: 100, align: 'left' },
                { label: 'Quantity', name: 'gr_qty', width: 100, align: 'right', formatter: 'integer' },
                { label: 'mt_sts_cd', name: 'mt_sts_cd', width: 100, align: 'right', hidden: true },
                { label: 'Status', name: 'sts_nm', width: 120, align: 'center' },

                { label: 'Expiry Date', name: 'expiry_dt', width: 180, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" }},
                { label: 'Date of Receipt', name: 'dt_of_receipt', width: 180, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" }},
                { label: 'Expore Date', name: 'expore_dt', width: 180, align: 'center', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d" } },
                { name: "", width: 50, align: "center", label: "Action", resizable: false, title: false, formatter: bntDelete },
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

$("#scan_mt_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var mt_cd = e.target.value.trim();

        var rows = $('#list1').jqGrid('getRowData');

        for (i = 0; i < rows.length; i++) {
            var mt_cd_view = rows[i].mt_cd;

            if (mt_cd == mt_cd_view) {
                ErrorAlert("Dữ liệu đã được scan rồi, vui lòng scan mã khác");

                $("#scan_mt_cd").val("");
                document.getElementById("scan_mt_cd").focus();

                return false;

            }
        }

        $.ajax({
            url: "/wipwms/GetShipping_ScanMLQR_WIP",
            type: "Get",
            dataType: "json",
            data: {
                mt_cd: mt_cd
            },
            success: function (response) {
                if (response.result == true) {

                    for (var i = 0; i < response.data.length; i++) {
                        var id = response.data[i].wmtid;
                        $("#list1").jqGrid('addRowData', id, response.data[i], 'first');
                        $("#list1").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    }
                    $("#scan_mt_cd").val("");
                    document.getElementById("scan_mt_cd").focus();
                    SuccessAlert(response.message)

                } else {
                    ErrorAlert(response.message);

                    $("#scan_mt_cd").val("");
                    document.getElementById("scan_mt_cd").focus();
                }

            }
        });
    }
});

$("#Save_completed").on("click", function () {

    var ex_no = $("#m_ex_no").val();
    if (ex_no == "" || ex_no == null || ex_no == undefined) {
        alert("Please Choose EX No!!!")
        return false;
    }

    var rows = $("#list1").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        alert("Please Scan!!!")
        return false;
    }

    $.ajax({
        url: "/wipwms/UpdateShipping_ScanMLQR_WIP?data=" + rows + "&ex_no=" + ex_no,
        type: "Put",
        dataType: "json",

        success: function (data) {
        debugger;

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




$("#Select_completed").on("click", function () {

    var ex_no = $("#m_ex_no").val();
    if (ex_no == "" || ex_no == null || ex_no == undefined) {
        alert("Please Choose EX No!!!")
        return false;
    }


    $('.popup-dialog.List_ML_NO_Info_Popup').dialog('open');



    $.get("/wipwms/PartialView_List_ML_NO_Info_Popup_WIP?" +
        "ex_no=" + ex_no + ""
        , function (html) {
            $("#PartialView_List_ML_NO_Info_Popup_WIP").html(html);
        });


});




//----- EX POPUP------------------//
function EX_popup(cellValue) {
    var html = '<a class="css_pp_cilck" data-ex_no="' + cellValue + '" onclick="ViewEXPopup(this);">' + cellValue + '</a>';
    return html;
};

function ViewEXPopup(e) {
    $('.popup-dialog.EX_Info_Popup').dialog('open');
    var ex_no = $(e).data('ex_no');


    $.get("/wipwms/PartialView_EX_Info_Popup?" +
        "ex_no=" + ex_no + ""
        , function (html) {
            $("#PartialView_EX_Info_Popup").html(html);
        });
}
$("#Create_NVL").on("click", function () {

    var ex_no = $("#m_ex_no").val();
    if (ex_no == "" || ex_no == null || ex_no == undefined) {
        alert("Please Choose EX No!!!")
        return false;
    }
    $('.popup-dialog.Create_List_Memory_Popup').dialog('open');

    $.get("/wipwms/PartialView_Create_List_Memory_Popup?" +
        "ex_no=" + ex_no + ""
        , function (html) {
            $("#PartialView_Create_List_Memory_Popup").html(html);
        });


});


//----- help to focus on input------------------//
setTimeout(function () { $("#scan_mt_cd").focus(); }, 1);