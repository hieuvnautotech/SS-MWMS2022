$(function () {
    $("#list").jqGrid
    ({
        url: "/DevManagement/getdata_tray_mgt",
        datatype: 'json',
        mtype: 'Get',
        colModel: [
             { key: true, label: 'trno', name: 'trno', width: 50, hidden: true },
             { label: 'Tray Code', name: 'tr_no', width: 110, align: 'center' },
             { label: 'Tray Name', name: 'tr_nm', width: 110, align: 'center' },
             { label: 'Product Qty', name: 'prd_qty', width: 100, align: 'right' },
             { label: 'User Y/N', name: 'use_yn', width: 100, align: 'center' },
             { label: 'Create User', name: 'reg_id', width: 110, align: 'left' },
             { label: 'Change User', name: 'chg_id', width: 180, align: 'left' },

             { label: 'Create Date', name: 'reg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },
             { label: 'Change Date', name: 'chg_dt', align: 'center', width: 150, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" } },

        ],
        formatter: {
            integer: { thousandsSeparator: ",", defaultValue: '0' },
            currency: { decimalSeparator: ".", defaultValue: '0.00', prefix: "", suffix: "", thousandsSeparator: ',', decimalPlaces: 2 },
            number: { decimalSeparator: ".", thousandsSeparator: " ", decimalPlaces: 2, defaultValue: '0.00' },
        },
        onSelectRow: function (rowid, status, e) {

            var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
            row_id = $("#list").getRowData(selectedRowId);

            $("#m_tr_no").val(row_id.tr_no);
            $("#m_tr_nm").val(row_id.tr_nm);
            $("#m_prd_qty").val(row_id.prd_qty);
            $("#m_trno").val(row_id.trno);
      
            $("#tab_1").removeClass("active");
            $("#tab_2").addClass("active");
            $("#tab_c1").removeClass("active");
            $("#tab_c2").removeClass("hidden");
            $("#tab_c1").addClass("hidden");
            $("#tab_c2").addClass("active");
            $("#tab_1").removeClass("active");
            $("#m_save_but").attr("disabled", false);
            $("#del_save_but").attr("disabled", false);
        },

        pager: jQuery('#gridpager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: "300",
        width: null,
        caption: 'Tray Management',
        rowNum: 50,
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        shrinkToFit: false,
        loadonce: true,
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
    $("#start").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    $("#end").datepicker({ dateFormat: 'yy-mm-dd' }).val();

    $("#tab_1").on("click", "a", function (event) {
        document.getElementById("form1").reset();
        $("#tab_2").removeClass("active");
        $("#tab_1").addClass("active");
        $("#tab_c2").removeClass("active");
        $("#tab_c1").addClass("active");
        $("#tab_c1").removeClass("hidden");
        $("#tab_c2").addClass("hidden");

    });
    $("#tab_2").on("click", "a", function (event) {

        document.getElementById("form2").reset();
        $("#tab_1").removeClass("active");
        $("#tab_2").addClass("active");
        $("#tab_c1").removeClass("active");
        $("#tab_c2").removeClass("hidden");
        $("#tab_c1").addClass("hidden");
        $("#tab_c2").addClass("active");
        $("#m_save_but").attr("disabled", true);
        $("#del_save_but").attr("disabled", true);
        $("#m_logo").empty();
    });

});


$("#c_save_but").click(function () {
    if ($("#form1").valid() == true) {
     
        var tr_no = $('#c_tr_no').val();
        var tr_nm = $('#c_tr_nm').val();
        var prd_qty = $('#c_prd_qty').val();

        $.ajax({
            url: "/DevManagement/insertTray_info",
            type: "get",
            dataType: "json",
            data: {
                tr_no: tr_no,
                tr_nm: tr_nm,
                prd_qty: prd_qty,
            },
            success: function (data) {
                var id = data.trno;
                $("#list").jqGrid('addRowData', id, data, 'first');
                $("#list").setRowData(id, false, { background: "#d0e9c6" });
            },
            error: function (data) {
                alert('The Code is the same. Please check again.');
            }
        });
    }
});

$("#m_save_but").click(function () {
    if ($("#form2").valid() == true) {
        var tr_no = $('#m_tr_no').val();
        var tr_nm = $('#m_tr_nm').val();
        var prd_qty = $('#m_prd_qty').val();
        var trno = $('#m_trno').val();
        $.ajax({
            url: "/DevManagement/updateTray_info",
            type: "get",
            dataType: "json",
            data: {
                tr_nm: tr_nm,
                prd_qty: prd_qty,
                trno: trno,
            },
            success: function (data) {
                var id = data.trno;
              
                $("#list").setRowData(id, data, { background: "#d0e9c6" });


            }
        });
    }
});

$("#searchBtn").click(function () {
    var tr_no = $('#tr_no').val().trim();
    var tr_nm = $('#tr_nm').val().trim();
    $.ajax({
        url: "/DevManagement/searchTrayInfo",
        type: "get",
        dataType: "json",
        data: {
            tr_no: tr_no,
            tr_nm: tr_nm,

        },
        success: function (result) {

            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { rowNum: 50, datatype: 'local', data: result }).trigger("reloadGrid");
        }
    });

});
$("#form1").validate({
    rules: {
        "prd_qty": {
            number: true,
        },

    },
});
$("#form2").validate({
    rules: {
        "prd_qty": {
            number: true,
        },

    },
});