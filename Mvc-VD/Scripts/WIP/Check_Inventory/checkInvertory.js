$("#list").jqGrid
    ({
        datatype: function (postData) { getData_Inventory(postData); },
        mtype: 'GET',
        colModel: [
            { key: true, label: 'vno', name: 'vno', width: 50, hidden: true },
            { label: 'Code', name: 'vn_cd', width: 150, align: 'center' },
            { label: 'Inventory Name', name: 'vn_nm', width: 200, align: 'left' },
            { label: 'Start Date', name: 'start_dt', width: 200, align: 'center' },
            { label: 'End Date', name: 'end_dt', width: 200, align: 'center' },
            { label: 'Remark', name: 're_mark', width: 200, align: 'left' },
            { label: 'User Y/N', name: 'use_yn', width: 50, align: 'center' },
            { label: 'Create User', name: 'reg_id', index: 'reg_id', editable: true, width: '100px' },
            { label: 'Create date', name: 'reg_dt', index: 'reg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200' },
            { label: 'Change User', name: 'chg_id', index: 'chg_id', editable: true, width: '100px' },
            { label: 'Change Date', name: 'chg_dt', index: 'chg_dt', editable: true, align: 'center', formatter: 'date', formatoptions: { newformat: 'm-d-Y h:m:s' }, width: '200' },
        ],

        onSelectRow: function (rowid, status, e) {

            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            var vno = row_id.vno;
            var vn_cd = row_id.vn_cd;
            var vn_nm = row_id.vn_nm;
            var start_dt = row_id.start_dt;
            var end_dt = row_id.end_dt;
            var re_mark = row_id.re_mark;
            var use_yn = row_id.use_yn;

           

            $('#m_vno').val(vno);
            $('#m_vn_cd').val(vn_cd);
            $('#m_vn_nm').val(vn_nm);
            $('#m_start_dt').val(start_dt);
            $('#m_end_dt').val(end_dt);
            $('#m_re_mark').val(re_mark);
            $('#m_use_yn').val(use_yn);

            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");
            $("#tab_1").removeClass("active");
            $("#m_save_but").attr("disabled", false);

            $('#list1').clearGridData();
            $('#list1').jqGrid('setGridParam', { search: true });
            var pdata = $('#list1').jqGrid('getGridParam', 'postData');
            getData_Inventory_DT(pdata);
        },
        pager: "#listPager",
        pager: jQuery('#listPager'),
        viewrecords: true,
        rowList: [20, 50, 200, 500],
        height: "200",
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 20,
        caption: 'Invertory Schedule',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
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
    });
//hàng tồn kho trong hệ thống
$("#list1").jqGrid
    ({
        datatype: function (postData) { getData_Inventory_DT(postData); },
        mtype: 'GET',
        colModel: [
            { label: 'Status', name: 'sts_nm', width: 110, align: 'right' },
            { key: true, label: 'wmtid', name: 'wmtid', width: 50, align: 'right', hidden: true },
            { label: 'Composite Code', name: 'mt_cd', width: 400, align: 'left' },
            { label: 'Material', name: 'mt_nm', width: 400, align: 'left' },
            { label: 'QTy (Roll/EA)', name: 'qty', width: 150, align: 'right' },
            { label: '', name: 'bundle_unit', width: 150, align: 'right', hidden: true },
            { label: 'Lenght', name: 'lenght', width: 150, align: 'right' },
            { label: 'Size ', name: 'size', width: 150, align: 'right' },
        ],

        pager: "#listPager1",
        viewrecords: true,
        height: "300",
        width: $(".box-body").width() - 5,
        rowNum: 200,
        rowList: [200, 400, 600, 800, 2000],
        autowidth: true,
        caption: 'Invertory Schedule',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
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
    });
$("#list2").jqGrid
    ({
        mtype: 'GET',
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

        pager: "#listPager2",
        viewrecords: true,
        height: "300",
        autowidth: true,
        rowNum: 200,
        rowList: [200, 400, 600, 800, 2000],

        caption: 'Invertory Schedule',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
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
    });
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';
$('#list').jqGrid('setGridWidth', $(".box-body").width());
$(window).on("resize", function () {
    var newWidth = $("#list").closest(".ui-jqgrid").parent().width();
    $("#list").jqGrid("setGridWidth", newWidth, false);
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
$("#ml_cd").on("keydown", function (e) {
    if (e.keyCode === 13) {

        var ml_cd = e.target.value.trim();

        //var rows = $('#list1').jqGrid('getGridParam', 'data');
        var rows = $('#list2').jqGrid('getRowData');
        for (i = 0; i < rows.length; i++) {
            var ml_cd_view = rows[i].mt_cd;

            if (ml_cd == ml_cd_view) {
                ErrorAlert("Dữ liệu đã scan rồi !!!");
                return false;
            }
        }
        var vn_cd = $("#m_vn_cd").val();
        if (vn_cd == "" || vn_cd == null || vn_cd == undefined) {
            ErrorAlert("Vui lòng chọn mã Code Inventory!!!")
            return false;
        }

        $.ajax({
            url: "/WIP/GetPickingScanMLQR",
            type: "get",
            dataType: "json",
            data: {
                ml_cd: ml_cd,
                vn_cd: vn_cd
            },
            success: function (response) {
                if (response.result) {
                    var id = response.data.wmtid;
                    $("#list2").jqGrid('addRowData', id, response.data, 'first');
                    $("#list2").setRowData(id, false, { background: "#28a745", color: "#fff" });
                    document.getElementById("ml_cd").focus();
                } else {
                    ErrorAlert(response.message);
                }
                $("#ml_cd").val("");

            }
        });
    }
});

$("#Save_completed").on("click", function () {
    var vn_cd = $("#m_vn_cd").val();
    if (vn_cd == "" || vn_cd == null || vn_cd == undefined) {
        ErrorAlert("Vui lòng chọn mã Code Inventory!!!")
        return false;
    }
    var rows = $("#list2").getDataIDs();
    if (rows == "" || rows == null || rows == undefined) {
        ErrorAlert("Vui lòng Scan!!!")
        return false;
    }
    $.ajax({
        url: "/WIP/InsertMTQRIVList?data=" + rows + "&vn_cd=" + vn_cd,
        type: "get",
        dataType: "json",

        success: function (data) {
            if (data.result) {
                SuccessAlert(data.message);
                $("#list2").jqGrid('clearGridData');
                $('#list1').clearGridData();
                $('#list1').jqGrid('setGridParam', { search: true });
                var pdata = $('#list1').jqGrid('getGridParam', 'postData');
                getData_Inventory_DT(pdata);
            }
            else {
                ErrorAlert(data.message);
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
    $("#list2").jqGrid('delRowData', wmtid);
}

$("#Clear_all").on("click", function () {
    var r = confirm("Bạn có chắc chắn XÓA sự kiện?");
    if (r === true) {
        $("#list2").jqGrid('clearGridData');
    }

});
function getData_Inventory(pdata) {
    var params = new Object();

    if ($("#list").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    var vn_nm = $('#vn_nm').val().trim();
    var start = $('#start').val().trim();
    var end = $('#end').val().trim();

    params.vn_nm = vn_nm;
    params.start = vn_nm;
    params.end = end;
    console.log(params);
    $("#list").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: '/WIP/GetCheckInvenSchedule',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list")[0];
                grid.addJSONData(data);
            }
        }
    })
};
function fun_check1() {
    if ($("#c_vn_nm").val().trim() == "") {
        alert("Please enter Name");
        $("#c_vn_nm").val("");
        $("#c_vn_nm").focus();
        return false;
    }
    return true;
}
$("#c_save_but").click(function () {
    if (fun_check1() == true) {
        var vn_cd = $('#c_vn_cd').val();
        var vn_nm = $('#c_vn_nm').val();
        var start_dt = $('#c_start_dt').val();
        var end_dt = $('#c_end_dt').val();
        var use_yn = $('#c_use_yn').val();
        var re_mark = $('#c_re_mark').val();
        {
            $.ajax({
                url: "/WIP/insertSchedule",
                type: "get",
                dataType: "json",
                data: {
                    vn_cd: vn_cd,
                    vn_nm: vn_nm,
                    start_dt: start_dt,
                    end_dt: end_dt,
                    use_yn: use_yn,
                    re_mark: re_mark,
                },
                success: function (data) {
                    if (data.result == true) {
                        var id = data.kq.vno;
                        $("#list").jqGrid('addRowData', id, data.kq, 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                        SuccessAlert("");
                    } else {
                        //alert(data.kq);
                        ErrorAlert(data.kq);
                    }
                }
            });
        }
    }
});
function fun_check2() {
    if ($("#m_vn_nm").val().trim() == "") {
        alert("Please enter Name");
        $("#m_vn_nm").val("");
        $("#m_vn_nm").focus();
        return false;
    }
    return true;
}
$("#m_save_but").click(function () {
    if (fun_check2() == true) {
        var vno = $('#m_vno').val();
        var vn_cd = $('#m_vn_cd').val();
        var vn_nm = $('#m_vn_nm').val();
        var start_dt = $('#m_start_dt').val();
        var end_dt = $('#m_end_dt').val();
        var use_yn = $('#m_use_yn').val();
        var re_mark = $('#m_re_mark').val();
        $.ajax({
            url: "/WIP/updateSchedule",
            type: "get",
            dataType: "json",
            data: {
                vno: m_vno,
                vn_cd: vn_cd,
                vn_nm: vn_nm,
                start_dt: start_dt,
                end_dt: end_dt,
                use_yn: use_yn,
                re_mark: re_mark,
            },
            success: function (data) {
                if (data.result == true) {
                    var id = data.kq.vno;
                    $("#list").setRowData(id, data.kq, { background: "#d0e9c6" });
                    SuccessAlert("");
                } else {
                    ErrorAlert(data.kq);
                };
            }
        });
    };
});

$("#searchBtn").click(function () {
    $('#list').clearGridData();
    $('#list').jqGrid('setGridParam', { search: true });
    var pdata = $('#list').jqGrid('getGridParam', 'postData');
    getData_Inventory(pdata);
});
$("#tab_1").on("click", "a", function (event) {

    document.getElementById("form1").reset();
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");

});
$("#tab_2").on("click", "a", function (event) {
    $("#m_save_but").attr("disabled", true);
    document.getElementById("form2").reset();
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");
});

$("#c_start_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#c_end_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#m_start_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#m_end_dt").datepicker({
    dateFormat: 'yy-mm-dd',
});
$("#start").datepicker({
    dateFormat: 'yy-mm-dd',
});

$("#end").datepicker({
    dateFormat: 'yy-mm-dd',
});

function getData_Inventory_DT(pdata) {
    var params = new Object();

    if ($("#list1").jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.vd_cd = $("#m_vn_cd").val()
    $("#list1").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
    $.ajax({
        url: '/WIP/GetInventorySX',
        type: "Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var grid = $("#list1")[0];
                grid.addJSONData(data);
            }
        }
    })
};