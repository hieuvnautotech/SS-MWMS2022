var bien = 0;

$(function () {
    $("#TrayBoxGrid").jqGrid({
        datatype: function (postData) { getData(postData); },
        mtype: 'Get',
        colNames: ['bno', 'Type', 'Code', 'Name', 'Lot', 'PO', 'Purpose', 'BarCode', 'Description'],
        //url: '/DevManagement/searchTrayBox',
        //datatype: 'json',
        //mtype: 'GET',
        colModel: [
            { name: "bno", hidden: true, key: true },
            //{ key: false,  name: 'xa', width: 80, align: 'center', formatter: xa_container },
            { name: 'mc_type', width: 50, align: 'center' },
            { name: 'bb_no', width: 250, sort: true },
            { name: 'bb_nm', width: 150, align: 'left' },
            { name: 'mt_cd', width: 300, align: 'left' },
            { key: false,  name: 'at_no', width: 100, align: 'center' },
            { name: 'purpose', align: 'right', width: 150, },
            { name: 'barcode', width: 250, align: 'center' },
            { name: 're_mark', width: 200, align: 'center' },
        ],
        pager: jQuery('#TrayBoxGridPager'),
        viewrecords: true,
        rowList: [50, 100, 200, 500, 1000],
        height: 500,
        width: $(".box-body").width() - 5,
        autowidth: false,
        rowNum: 50,
        caption: '',
        loadtext: "Loading...",
        emptyrecords: "No data.",
        rownumbers: true,
        gridview: true,
        caption: 'Tray Box Management',
        loadonce: false,
        shrinkToFit: false,
        multiselect: true,
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


    $("#c_save_but").click(function () {
        if ($("#form1").valid() == true) {
            //var dpno = $('#c_dpno').val();
            var bb_no = $('#c_mt_cd').val();
            var bb_nm = $('#c_bb_nm').val();
            var purpose = $('#c_purpose').val();
            var description = $('#c_description').val();
            $.ajax({
                url: "/DevManagement/insertTrayBox",
                type: "get",
                dataType: "json",
                data: {
                    bb_no: bb_no,
                    bb_nm: bb_nm,
                    purpose: purpose,
                    description: description,
                    //dpno: dpno,
                },
                success: function (data) {
                    var id = data.bxno;
                    //$('#mt_remark_color').val(mt_cd);
                    //jQuery("#commonGrid").setGridParam({ datatype: "json" }).trigger('reloadGrid');

                    $("#TrayBoxGrid").jqGrid('addRowData', id, data, 'first');
                    $("#TrayBoxGrid").setRowData(id, false, { background: "#d0e9c6" });
                },
                error: function (data) {
                    alert('The Code is the same. Please check again.');
                }
            });
        }
    });

    $("#m_save_but").click(function () {
        if ($("#form2").valid() == true) {
            //var bx_no = $('#m_bx_no').val();
            var bb_nm = $('#m_bb_nm').val();
            var purpose = $('#m_purpose').val();
            var description = $('#m_description').val();
            var bno = $('#m_bno').val();
            $.ajax({
                url: "/DevManagement/updateTrayBox",
                type: "get",
                dataType: "json",
                data: {
                    bb_nm: bb_nm,
                    purpose: purpose,
                    description: description,
                    bno: bno,
                },
                success: function (data) {
                    var id = data.bno;
                    $("#TrayBoxGrid").setRowData(id, data, { background: "#d0e9c6" });
                }
            });
        }
    });
});

$("#tab_1").on("click", "a", function (event) {
    document.getElementById("form1").reset();
    $("#DeptGrid").trigger('reloadGrid');
    $("#tab_2").removeClass("active");
    $("#tab_1").addClass("active");
    $("#tab_c2").removeClass("active");
    $("#tab_c1").removeClass("hidden");
    $("#tab_c2").addClass("hidden");
    $("#tab_c1").addClass("active");
});
$("#tab_2").on("click", "a", function (event) {
    document.getElementById("form2").reset();
    $("#DeptGrid").trigger('reloadGrid');
    $("#tab_1").removeClass("active");
    $("#tab_2").addClass("active");
    $("#tab_c1").removeClass("active");
    $("#tab_c2").removeClass("hidden");
    $("#tab_c1").addClass("hidden");
    $("#tab_c2").addClass("active");

    $("#m_save_but").attr("disabled", true);
    $("#del_save_but").attr("disabled", true);

});

$('#excelBtn').click(function () {
    var bb_nm = $('#s_bb_nm').val().trim();
    var bb_no= $('#s_bb_no').val().trim();
    $('#exportData').attr('action', "/DevManagement/ExportToExcelTrayBox?bb_nm=" + bb_nm + "&bb_no=" + bb_no);
});

$('#htmlBtn').click(function (e) {
    $("#TrayBoxGrid").jqGrid('exportToHtml',
        options = {
            title: '',
            onBeforeExport: null,
            includeLabels: true,
            includeGroupHeader: true,
            includeFooter: true,
            tableClass: 'jqgridprint',
            autoPrint: false,
            topText: '',
            bottomText: '',
            fileName: "Department",
            returnAsString: false
        }
    );
});

$("#form1").validate({
    rules: {
        "c_mt_cd": {
            required: true,
        },
        "c_bb_nm": {
            required: true,
        },
    },
});

$("#form2").validate({
    rules: {
        "m_mt_cd": {
            required: true,
        },
        "m_bb_nm": {
            required: true,
        },
    },
});

function getData(pdata) {
    if (bien != 0) {
        $('.loading').show();
        var params = new Object();
        if (jQuery('#TrayBoxGrid').jqGrid('getGridParam', 'reccount') == 0) {
            params.page = 1;
        }
        else { params.page = pdata.page; }
        params.rows = pdata.rows;
        params.sidx = pdata.sidx;
        params.sord = pdata.sord;
        params.bb_no = $('#s_bb_no').val() == null ? "" : $('#s_bb_no').val().trim();
        params.bb_nm = $('#s_bb_nm').val() == null ? "" : $('#s_bb_nm').val().trim();
        $("#TrayBoxGrid").jqGrid('setGridParam', { search: true, postData: { searchString: $("#auto_complete_search").val() } });
        params._search = pdata._search;

        $.ajax({
            url: "/DevManagement/searchTrayBox",
            type: "Get",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            traditional: true,
            data: params,
            success: function (data, st) {
                if (st == "success") {
                    var grid = $("#TrayBoxGrid")[0];
                    grid.addJSONData(data);
                    $('.loading').hide();
                }
            }
        })
    }
};
//-----------------------------------------------------------------------------------
function xa_container(cellValue, options, rowdata, action) {

    var html = "";
    if (rowdata.mt_cd) {
        html = `<button class="btn btn-xs btn-primary" data-id="${rowdata.bno}" data-mt_cd="${rowdata.mt_cd}" data-bb_no="${rowdata.bb_no}" onclick="Xa_bb(this);">Xả</button>`;
    }
    return html;
}
function Xa_bb(e) {
    $("#bobin").val($(e).data("bb_no"));
    $("#mt_cd").val($(e).data("mt_cd"));
    $("#id_xa").val($(e).data("id"));
    $("#sts").val("xa_bb");
    $('#dialogDangerous').dialog('open');
}
$("#searchBtn").click(function () {
    $('.loading').show();
    bien = 1;
    var grid = $("#TrayBoxGrid");
    grid.jqGrid('setGridParam', { search: true });
    var pdata = grid.jqGrid('getGridParam', 'postData');

    getData(pdata);
});

$("#form4").validate({
    rules: {
        "qty_ge": {
            required: true,
            min: 1,
            max: 10,
            number: true,
        },
        "mc_type": {
            required: true,

        },
    },
});
function checkType(qty_ge, mc_type) {
    if (mc_type == "") {
        ErrorAlert("Please enter the Type.");
        $('#loading').hide();
        return false;
    }
    if (isNaN(qty_ge) || qty_ge == null || qty_ge == '' || qty_ge == 'undefined' || qty_ge.length == 0) {
        ErrorAlert("Please enter a number value between One and Ten. ");
        $('#loading').hide();
        return false;
    }
    if (parseInt(qty_ge) <= 0 || parseInt(qty_ge) > 10) {
        ErrorAlert("Please enter a value between One and Ten. ");
        $('#loading').hide();
        return false;
    }
    return true;
}



$("#autogeBtn").click(function () {
    //if ($("#form4").valid() == true) {
    //var bx_no = $('#m_bx_no').val();
    var qty_ge = $('#qty_ge').val().trim();
    var mc_type = $('#mc_type').val();
    if (mc_type.trim() != '') {

        if (!isNaN(qty_ge)) {
            $('#loading').show();
            if (parseInt(qty_ge) >= 1 && parseInt(qty_ge) <= 10) {
                $.ajax({
                    url: "/DevManagement/insertautoTrayBox",
                    type: "Post",
                    dataType: "json",
                    data: {
                        qty_ge: qty_ge,
                        mc_type: mc_type
                    },
                    success: function (response) {
                        debugger;
                        if (response.result == true) {
                            for (var i = 0; i < response.data.length; i++) {
                                var item = response.data[i];
                                $("#TrayBoxGrid").jqGrid('addRowData', item.bno, item, 'first');
                                $("#TrayBoxGrid").setRowData(item.bno, false, { background: "#28a745", color: "#fff" });
                            }
                            $('#loading').hide();
                        }
                        else {

                            ErrorAlert(response.message);
                            $('#loading').hide();
                        }
                    }
                });
            }
            else {
                ErrorAlert("Please enter a value between One and Ten. ");
                $('#loading').hide();
            }

        }
        else {
            ErrorAlert("Please enter a number value between One and Ten. ");
            $('#loading').hide();

        }
    }
    else {
        ErrorAlert("Please enter the Type.");
        $('#loading').hide();
    }

});


$("#c_print").click(function () {
    var i, selRowIds = $('#TrayBoxGrid').jqGrid("getGridParam", "selarrrow"), n, rowData;

    if (selRowIds.length > 0) {
        var mapForm = document.createElement("form");
        mapForm.target = "Map";
        mapForm.type = "hidden";
        mapForm.method = "POST"; // or "post" if appropriate
        mapForm.action = "/DevManagement/_Print";

        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = "qrcode";
        mapInput.value = selRowIds;

        mapForm.appendChild(mapInput);
        document.body.appendChild(mapForm);

        map = window.open(" ", "Map", "width=600, height=800, left=200, top=100, location=no, status=no,");

        if (map) {
            mapForm.submit();
        } else {
            ErrorAlert("You must allow popups for this map to work.'");
        }
    }

    else {
        ErrorAlert("Please Select Grid");
    }




});
//
$("#dialogDangerous").dialog({
    width: '20%',
    height: 100,
    maxWidth: '20%',
    maxHeight: 100,
    minWidth: '20%',
    minHeight: 100,
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

    },
});

$("#deletestyle").click(function () {
    $.ajax({
        url: "/ActualWO/DestroyBobbinMgt",
        type: "Get",
        dataType: "json",
        data: {
            bobin: $("#bobin").val(),
            mt_cd: $("#mt_cd").val(),
        },
        success: function (responsie) {
            if (responsie.result) {
                SuccessAlert(responsie.message);
                var id = $("#id_xa").val();

                var rowData = $('#TrayBoxGrid').jqGrid('getRowData', id);
                rowData.mt_cd = null;
                $('#TrayBoxGrid').jqGrid('setRowData', id, rowData);
                $("#TrayBoxGrid").setRowData(id_dv, false, { background: "#d0e9c6" });

            } else {
                ErrorAlert(responsie.message);
            }
        },
        error: function (result) { }
    });

    $('#dialogDangerous').dialog('close');
});

$('#closestyle').click(function () {
    $('#dialogDangerous').dialog('close');
});

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});
