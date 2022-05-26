
$(function () {
    $(".dialog2").dialog({
        width: '100%',
        height: 450,
        maxWidth: 1000,
        maxHeight: 450,
        minWidth: '50%',
        minHeight: 450,
        resizable: false,
        fluid: true,
        modal: true,
        autoOpen: false,
        classes: {
            "ui-dialog": "ui-dialog",
            "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
            "ui-dialog-titlebar-close": "visibility: hidden",
        },
        resize: function (event, ui) {
            $('.ui-dialog-content').addClass('m-0 p-0');
        },
        open: function (event, ui) {

            $("#staff_popup").jqGrid
            ({
                datatype: function (postData) { getData_staff(postData); },
                mtype: 'Get',
                colModel: [
                   { key: true, label: 'Userid', name: 'userid', width: 80, align: 'left' },
                   { key: false, label: 'Name', name: 'uname', width: 150, align: 'left' },
                   { key: false, label: 'Mission', name: 'position_cd', width: 150, align: 'left' },
                   { key: false, label: 'Machine', name: 'mc_no', width: 150, align: 'left' },
                ],
                onSelectRow: function (rowid, selected, status, e) {
                    $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                    $("#selected").removeClass("disabled");
                    var selectedRowId = $("#staff_popup").jqGrid("getGridParam", 'selrow');
                    row_id = $("#staff_popup").getRowData(selectedRowId);
                    if (row_id != null) {
                        $('#close_staff').click(function () {
                            $('.dialog2').dialog('close');
                        });
                        $("#selected").click(function () {
                            $('#c_staff_id').val(row_id.userid);
                            $('#m_staff_id').val(row_id.userid);
                            $('.dialog2').dialog('close');
                        });
                    }
                },
                pager: jQuery('#page_staff'),
                viewrecords: true,
                rowList: [50, 100, 200, 500, 1000],
                height: 300,
                width: $(".boxStaff").width() -50,
                autowidth: false,
                rowNum: 50,
                caption: '',
                loadtext: "Loading...",
                emptyrecords: "No data.",
                rownumbers: true,
                gridview: true,
                loadonce: false,
                shrinkToFit: false,
                multiselect: false,
                subGrid: true,
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

        },
    });


    $(".poupdialogstaff").click(function () {
        $('.dialog2').dialog('open');
    });

    $('#close_staff').click(function () {
        $('.dialog2').dialog('close');
    });

});


function getData_staff(pdata) {

    var params = new Object();

    if ($('#staff_popup').jqGrid('getGridParam', 'reccount') == 0) {
        params.page = 1;
    }
    else { params.page = pdata.page; }
    var userid = $('#userid_pp').val().trim();
    var uname = $('#uname_pp').val().trim();
    var position_cd = $('#position_cd').val().trim();

    params.rows = pdata.rows;
    params.sidx = pdata.sidx;
    params.sord = pdata.sord;
    params._search = pdata._search;
    params.position_cd = position_cd;
    params.uname = uname;
    params.userid = userid;
    //params.processCode = $('#sMaterialCode').val();

    $('#staff_popup').jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });

    $.ajax({
        url: "/ActualWO/SearchStaffPp",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        traditional: true,
        data: params,
        success: function (data, st) {
            if (st == "success") {
                var showing = $('#staff_popup')[0];
                showing.addJSONData(data);
            }
        },
        error: function () {
            return;
        }
    });
};
$("#searchBtn_staff").click(function () {
    $('#staff_popup').clearGridData();
    $('#staff_popup').jqGrid('setGridParam', { search: true });
    var pdata = $('#staff_popup').jqGrid('getGridParam', 'postData');
    getData_staff(pdata);
});

$(document).ready(function () {
    _getposition();
});
function _getposition() {
    $.get("/ActualWO/Getposition_staff", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Mission*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            $("#position_cd").html(html);
        }
    });
}