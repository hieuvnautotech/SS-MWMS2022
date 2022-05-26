$(".dialog1").dialog({
    width: '70%',
    height: 1100,
    maxWidth: 1000,
    maxHeight: 450,
    minWidth: '50%',
    minHeight: 450,
    zIndex: 1000,
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
    close: function (event, ui) {
        $('#popupbom').empty();
    },
});


$('#closebom1').click(function () {
    $('.dialog1').dialog('close');
    $('#popupbom').empty();
    //$("#list").setGridParam({ datatype: "json" }).trigger('reloadGrid');
});

$("#Print").on("click", function () {
    var bid = $("#pp_bid").val();
    window.open("/DevManagement/Printdetail_bom?" + "id=" + bid, "PRINT", "width=1100, height=800, left=0, top=100, location=no, status=no,")
});