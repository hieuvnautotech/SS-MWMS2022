$(".dialog_menu").dialog({
    width: '100%',
    height: 500,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog2": "ui-dialog2",
        "ui-dialog2-titlebar": "ui-dialog2 ui-dialog2-titlebar-sm",
        "ui-dialog2-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog2-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {

        $("#popupmenu").jqGrid
        ({
            url: '/System/GetDataMenu',
            mtype: 'GET',
            datatype: 'json',
            colModel: [
            { label: 'ID', name: 'mnno', key: true, width: 60, align: 'right', hidden: true, key: true },
            { name: 'mn_cd', hidden: true },
            { name: 'up_mn_cd', hidden: true },
            { name: 'level_cd', hidden: true },
            { name: 'sub_yn', hidden: true },
            { name: 'mn_cd_full', hidden: true },
            { label: 'Menu Name', name: 'mn_nm', sortable: true, editable: true, editrules: { required: true, number: true }, width: 150, align: 'left' },
            { label: 'Full Name', name: 'mn_full', sortable: true, width: 350, editrules: { required: true, number: true }, align: 'left' },
            { label: 'URL', name: 'url_link', width: 220, align: 'left', sortable: true, editable: true, editrules: { required: true, number: true } },
            { label: 'USE YN', name: 'use_yn', width: 55, align: 'center', sortable: true, editrules: { required: true, number: true } },
            { label: 'Order No', name: 'order_no', width: 70, align: 'center', sortable: true, editable: true },
            { label: 'Reg Name', name: 'reg_id', sortable: true, },
            {
            label: 'Reg Date', name: 'reg_dt', sortable: true, formatter: "date",
            formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d H:i:s" }
            },

            ],
            onSelectRow: function (rowid, selected, status, e) {
                $('.ui-state-highlight').css({ 'border': '#AAAAAA' });
                var selectedRowId = $("#popupmenu").jqGrid("getGridParam", 'selrow');
                row_id = $("#popupmenu").getRowData(selectedRowId);
                if (row_id != null) {
                    $('#selectmenu').click(function () {
                        $('#c_mn_cd').val(row_id.mn_cd);
                        $('#c_title').val(row_id.mn_nm);
                        $('#m_mn_cd').val(row_id.mn_cd);
                        $('#m_title').val(row_id.mn_nm);
                        $('.dialog_menu').dialog('close');
                    });
                }
            },

            pager: jQuery('#pagermenu'),
            viewrecords: true,
            rowNum: 50,
            rowList: [50, 100, 200, 500, 1000],
            sortable: true,
            loadonce: true,
            height: 300,
            width: $(".boxMenu").width(),
            autowidth: false,
            loadtext: "Loading...",
            emptyrecords: "No data.",
            rownumbers: true,
            gridview: true,
            loadonce: true,
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
$(".poupdialogmenu").click(function () {
    $('.dialog_menu').dialog('open');
});

///////////////////////////////////////////////////////////////////////////////////////////////////////

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
        url: "/system/deleteManual",
        type: "Delete",
        dataType: "json",
        data: {
            bno: $('#s_bno').val(),
        },
        success: function (data) {
            if (data.result != 0) {
                jQuery("#noticeMgt").setGridParam({ datatype: "json" }).trigger('reloadGrid');
                SuccessAlert("Thành Công !");
            }
            else {
                ErrorAlert('Manual Information was not existing. Please check again');
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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
$(".dialog_CREATE").dialog({
    width: 1200,
    height: 850,
    maxWidth: 1200,
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
        $('#c_content').summernote({
            placeholder: 'Please give me content ....',
            tabsize: 2,
            height: 500,
            minHeight: 100,
            maxHeight: 800,
            focus: true,
            toolbar: [
              ['style', ['bold', 'italic', 'underline', 'clear']],
              ['font', ['strikethrough', 'superscript', 'subscript']],
              ['fontsize', ['fontsize']],
              ['color', ['color']],
              ['para', ['ul', 'ol', 'paragraph']],
              ['height', ['height']],
              ['table', ['table']],
              ['insert', ['link', 'picture', 'video']],
              ['view', ['fullscreen', 'codeview', 'help']],
            ],
            popover: {
                image: [
                  ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                  ['float', ['floatLeft', 'floatRight', 'floatNone']],
                  ['remove', ['removeMedia']]
                ],
                link: [
                  ['link', ['linkDialogShow', 'unlink']]
                ],
                table: [
                  ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
                  ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
                ],
                air: [
                  ['color', ['color']],
                  ['font', ['bold', 'underline', 'clear']],
                  ['para', ['ul', 'paragraph']],
                  ['table', ['table']],
                  ['insert', ['link', 'picture']]
                ]
            },
            codemirror: {
                theme: 'paper'
            }
        });
    },
    close: function (event, ui) {
        $('#c_content').summernote('destroy');
    },
});

$('#registerBtn').click(function () {
    $('.dialog_CREATE').dialog('open');
});

$('#c_saveBtn').click(function () {

debugger;
    if (($("#c_lng_cd").val() != "") && ($("#c_mn_cd").val() != "") && ($("#c_content").val() != ""))
    {          
        $.ajax({
            url: "/system/check_exist_manual",
            type: "Get",
            dataType: "json",
            data: {
                mn_cd: $("#c_mn_cd").val(),
                lng_cd: $("#c_lng_cd").val()
            },
            success: function (data) {
                if (data.result == 0) {                    
                    SuccessAlert('A manual page was created. Continue make another manual');
                    $("#c_notice").submit();
                } else {
                    ErrorAlert('Manual Information was existing. Please check again');
                }
            },
            error: function (result) { }
        });
    
    }
    else 
    {
        alert("Language, Title, Content should not blank");
    }       
});
//////////////////////////////////////////////////////////////////////////////////////////////////////////
$(".dialog_MODIFY").dialog({
    width: 1200,
    height: 850,
    maxWidth: 1200,
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
        $('#m_content').summernote({
            placeholder: 'Please give me content ....',
            tabsize: 2,
            height: 500,
            minHeight: 100,
            maxHeight: 800,
            focus: true,
            dialogsInBody: true,
            toolbar: [
              ['style', ['bold', 'italic', 'underline', 'clear']],
              ['font', ['strikethrough', 'superscript', 'subscript']],
              ['fontsize', ['fontsize']],
              ['color', ['color']],
              ['para', ['ul', 'ol', 'paragraph']],
              ['height', ['height']],
              ['table', ['table']],
              ['insert', ['link', 'picture', 'video']],
              ['view', ['fullscreen', 'codeview', 'help']],
            ],
            popover: {
                image: [
                  ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                  ['float', ['floatLeft', 'floatRight', 'floatNone']],
                  ['remove', ['removeMedia']]
                ],
                link: [
                  ['link', ['linkDialogShow', 'unlink']]
                ],
                table: [
                  ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
                  ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
                ],
                air: [
                  ['color', ['color']],
                  ['font', ['bold', 'underline', 'clear']],
                  ['para', ['ul', 'paragraph']],
                  ['table', ['table']],
                  ['insert', ['link', 'picture']]
                ]
            },
            codemirror: {
                theme: 'paper'
            }
        });       
    },
    close: function (event, ui) {
        $('#m_content').summernote('reset');
        $('#m_content').summernote('destroy');
       
    },
});

$('#m_saveBtn').click(function () {

debugger;
    if ($("#m_content").val() != "") {
        //($("#m_lng_cd").val() == "English") ? $("#m_lng_cd").val("EN") : $("#m_lng_cd").val("VN")
        $.ajax({
            url: "/system/check_exist_manual",
            type: "Get",
            dataType: "json",
            data: {
                mn_cd: $("#m_mn_cd").val(),
                lng_cd: ($('#m_lng_cd').val() == "English") ? "EN" : "VN",
            },
            success: function (data) {
                if (data.result != 0) {
                    alert('Manual updated. Good job');
                    $("#m_notice").submit();
                } else {
                    alert('A new manual was created. Good job');
                    $("#m_notice").submit();
                }
            },
            error: function (result) { }
        });

    }
    else {
        alert("Language, Title, Content should not blank");
    }
});

/////////////////////////////////////////////////////////////////////////////////////////////
$(".dialog3").dialog({
    width: 1150,
    height: 800,
    maxWidth: '100%',
    maxHeight: 800,
    minWidth: '50%',
    minHeight: 400,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
        //"ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },

});
$('#close3').click(function () {
    $('.dialog3').dialog('close');
});
