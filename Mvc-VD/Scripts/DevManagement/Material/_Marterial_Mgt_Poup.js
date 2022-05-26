$(".dialogSupli").dialog({
    width: '50%',
    height: 600,
    maxWidth: 1000,
    maxHeight: 600,
    minWidth: '50%',
    minHeight: 600,
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
        $("#popupsupllier").jqGrid
        ({
            url: '/DevManagement/getpopupsuplier',
            datatype: 'json',
            mtype: 'Get',
            colModel: [
                    { label: 'spno', name: 'spno', key: true, width: 80, align: 'left', hidden: true },
                { label: 'Supplier Code', name: 'sp_cd', width: 150, align: 'left' },
                { label: 'Supplier Name', name: 'sp_nm', width: 150, align: 'left' },
                  
                { label: 'Busines type', name: 'bsn_tp', width: 100, align: 'center' },
                { label: 'BSN', name: 'bsn_tp', width: 80, hidden: true, align: 'center' },
                { label: 'Changer', name: 'chg_nm', width: 80, align: 'center', },
                { label: 'Tel', name: 'phone_nb', width: 90, align: 'left' },
                { label: 'Cell', name: 'cell_nb', width: 90, align: 'left' },
                { label: 'Fax', name: 'fax_nb', width: 90, align: 'left' },
                { label: 'E-Mail', name: 'e_mail', width: 120, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                { label: 'Website', name: 'web_site', width: 120, align: 'left' },
                { label: 'Address', name: 'address', width: 150, align: 'left', sorttype: false, sortable: false, cellattr: function (rowId, cellValue, rowObject) { return ' title="' + cellValue + '"'; } },
                { label: 'Remark', name: 're_mark', width: 120, align: 'left' },
            ],
            onSelectRow: function (rowid, selected, status, e) {
                $("#savestyle_Supli").removeClass("disabled");
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupsupllier").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupsupllier").getRowData(selectedRowId);
                if (row_id != null) {
                    $("#savestyle_Supli").click(function () {
                        $('#c_supllier').val(row_id.sp_cd);
                        $('#m_supllier').val(row_id.sp_cd);
                        $('#pp_supllier').val(row_id.sp_cd);
                        $('.dialogSupli').dialog('close');
                    });
                }
            },

            pager: '#pagersupllier',
            loadonce: false,
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            height: 300,
            width:null,
            autowidth: true,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            shrinkToFit: false,
            multiselect: false,
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
$("#searchBtn_Buyer_popup").click(function () {
    var suplier_no = $('#suplier_no_popup').val().trim();
    var suplier_nm = $('#suplier_nm_popup').val().trim();
    $.ajax({
        url: '/DevManagement/getpopupsuplier',
        type: "get",
        dataType: "json",
        data: {
            suplier_no: suplier_no,
            suplier_nm: suplier_nm
        },
        success: function (result) {
            debugger;
            $("#popupsupllier").jqGrid('clearGridData').jqGrid('setGridParam', { datatype: 'local', data: result.rows }).trigger("reloadGrid");
        }
    });
});


$(".poupdialogsupli").click(function () {
    $('.dialogSupli').dialog('open');
});

$('#closestyle_Supli').click(function () {
    $('.dialogSupli').dialog('close');
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Popup Dangerous
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

$('#del_save_but').click(function () {
    $('#dialogDangerous').dialog('open');
});


$("#deletestyle").click(function () {
    $.ajax({
        url: "/DevManagement/deleteMaterial",
        type: "Delete",
        dataType: "json",
        data: {
            id: $('#m_mtid').val(),
        },
        success: function (data) {
            if (data.result == true) {
                $("#list").jqGrid('delRowData', data);
                jQuery('#list').trigger('reloadGrid');
                SuccessAlert(data.message);
            }
            else {
                ErrorAlert(data.message);
            }

        },
        error: function (result) { }
    });
    $('#dialogDangerous').dialog('close');
});

$('#closestyle').click(function () {
    $('#dialogDangerous').dialog('close');
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$(".dialogImg").dialog({
    width: '60%',
    height: 700,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '40%',
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

    },
});

$('#closeImg').click(function () {
    $('.dialogImg').dialog('close');
});

//copy_function
$(".Copy_material").dialog({
    width: '90%',
    height: 400,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '40%',
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

    },
    //close: function (event, ui) {

    //    document.getElementById("form3").reset();
    //},
});

$("#m_copy").click(function () {

    //$('.copy_consum option[value=val2]').attr('selected', 'selected');
    $('.Copy_material').dialog('open');
    $("#pp_mt_type").val("PMT");
});


$("#pp_save_but").click(function () {
    if ($("#form3").valid() == true) {

        var formData = new FormData();

        formData.append("mt_type", $('#pp_mt_type').val());
        formData.append("mt_no", $('#pp_mt_no').val().toUpperCase());
        formData.append("mt_nm", $('#pp_mt_nm').val());
        formData.append("sp_cd", $('#pp_supllier').val());

        if ($('#pp_width').val() == "") { formData.append("width", "0") } else { formData.append("width", $('#pp_width').val()); }
        if ($('#pp_spec').val() == "") { formData.append("spec", "0") } else { formData.append("spec", $('#pp_spec').val()); }
        formData.append("width_unit", $('#pp_width_unit').val());
        formData.append("spec_unit", $('#pp_spec_unit').val());
        formData.append("unit_cd", $('#pp_unit_cd').val());
        formData.append("bundle_unit", $('#pp_bundle_unit').val());
        formData.append("bundle_qty", $('#pp_bundle_qty').val());
        formData.append("barcode", $('#pp_barcode').val());
        formData.append("sts_create", "copy");

        $('#loading').show();
        $.ajax({
            type: 'POST',
            url: "/DevManagement/insertmaterial",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                /*debugger*/;
                $('#loading').hide();
                if (data.result == true) {
                    if (data.type != "PMT") {
                        var id = data.kq[0].mtid;
                        $("#list").jqGrid('addRowData', id, data.kq[0], 'first');
                        $("#list").setRowData(id, false, { background: "#d0e9c6" });
                    } else {
                        if (data.kq.length == 0) { alert("MT Code already exists"); }
                        for (var i = 0; i < data.kq.length; i++) {
                            var id = data.kq[i].mtid;
                            $("#list").jqGrid('addRowData', id, data.kq[i], 'first');
                            $("#list").setRowData(id, false, { background: "#d0e9c6" });
                        }
                    }
                    SuccessAlert(data.message);
                }
                else {
                    ErrorAlert(data.message);
                }
            },
            error: function (result) {
                $('.loading').hide();
                ErrorAlert('The Code is the same. Please check again');
            }
        })
    }
});