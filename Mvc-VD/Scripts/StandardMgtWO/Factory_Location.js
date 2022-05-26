$grid = $("#list").jqGrid({
    url: '/StandardMgtWO/GetWHfactory_wo',
    mtype: 'GET',
    datatype: 'json',
    colModel: [
        {label: 'ID', name: 'lctno', width: 60 ,hidden: true},
        { label: 'Factory Code', name: 'lct_cd', sortable: true, editable: true, editrules: { required: true, number: true }, width: 250, align: 'center' },
        { label: 'Name', name: 'lct_nm', sortable: true, editrules: { required: true, number: true }, width: 250, },
        { label: 'Level', name: 'level_cd', sortable: true, editrules: { required: true, number: true }, width: 60, align: 'center' },
        { label: 'use Y/N', name: 'use_yn', sortable: true, width: 60, },
        { label: 'Remark', name: 're_mark', width: 100, sortable: true, },
        { label: 'Creat User', name: 'reg_id', sortable: true, width: 70, },
        {label: 'Creat Date', width: 110, name: 'reg_dt', sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" }},
        { label: 'Change User', name: 'chg_id', sortable: true, width: 80, },
        { label: 'Change Date', name: 'chg_dt', width: 110, sortable: true, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
    ],
    loadonce: true,
    shrinkToFit: false,
    pager: '#jqGridPager1',
    rowNum: 2000,
    rownumbers: true,

    rowList: [60, 80, 100, 120],
    viewrecords: true,
    height: 360,
    jsonReader:
    {
        root: "rows",
        page: "page",
        total: "total",
        records: "records",
        repeatitems: false,
        Id: "0"
    },
    width: $(".box-body").width() - 5,
    autowidth: false,
    onSelectRow: function (rowid, status, e) {
        var selectedRowId = $("#list").jqGrid("getGridParam", 'selrow');
        row_id = $("#list").getRowData(selectedRowId);
        $('#m_lct_nm').val(row_id.lct_nm);
        $("#m_use_yn").val(row_id.use_yn);
        $('#m_re_mark').val(row_id.re_mark);
        $('#m_lctno').val(row_id.lctno);
        $('#c_lctno').val(row_id.lctno);

                  
    },
});
            

$("#c_save_but").click(function () {
    var selRowId = $('#list').jqGrid("getGridParam", "selrow");
    if (selRowId == null) {
        alert("Please select the top Factory on the grid.");
        return false;
    }
    if ($("#c_lct_nm").val().trim() == "") {
        alert("Please enter  Name");
        $("#c_lct_nm").val("");
        $("#c_lct_nm").focus();
        return false;
    }

    if ($("#c_re_mark").val() == "") {
        alert("Please enter Remark");
        $("#c_re_mark").val("");
        $("#c_re_mark").focus();
        return false;
    } else {
        $.ajax({
            url: "/StandardMgtWO/insertfactory_lct",
            type: "get",
            dataType: "json",
            data: {
                lct_nm: $('#c_lct_nm').val(),
                use_yn: $('#c_use_yn').val(),
                re_mark: $('#c_re_mark').val(),
                lctno: $('#c_lctno').val(),                            
            },
            success: function (data) {
                jQuery("#list").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
                document.getElementById("form2").reset();
            }

        });
    }
});
$("#m_save_but").click(function () {
    $.ajax({
        url: "/StandardMgtWO/updatefactory_lct",
        type: "get",
        dataType: "json",
        data: {
            lct_nm: $('#m_lct_nm').val(),
            use_yn: $('#m_use_yn').val(),
            re_mark: $('#m_re_mark').val(),
            lctno: $('#m_lctno').val(),
        },
        success: function (data) {
            jQuery("#list").setGridParam({ rowNum: 2000, datatype: "json" }).trigger('reloadGrid');
            document.getElementById("form1").reset();
        }
    });
});
$("#searchBtn").click(function () {
    $.ajax({
        url: "/StandardMgtWO/SearchFactory_Location",
        type: "get",
        dataType: "json",
        data: {
            lct_cd: $('#lct_cd').val().trim(),
        },
        success: function (result) {
            $("#list").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result }).trigger("reloadGrid");

        }
    });

});

$(document).ready(function () {
    $("#lct_cd").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/StandardMgtWO/Autolct_cd",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.lct_cd, value: item.lct_cd };
                    }))

                }
            })
        },
        messages: {
            noResults: '',
        }
    });
});

$("#c_reset_but").click(function () {
    document.getElementById("form2").reset();
});
   
