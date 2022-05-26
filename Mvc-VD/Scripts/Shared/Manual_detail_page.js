$(".Manual_detail_page").dialog({
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
        "ui-dialog-titlebar-close": "visibility: hidden",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },

});
$('#close3').click(function () {
    $('.Manual_detail_page').dialog('close');
});


$('#i_EN').click(function () {
    var menu_nm = $("#current_menu").data("level_1") + ">" + $("#current_menu").data("level_2") + ">" + $("#current_menu").data("level_3");
    var lang_cd = $("#current_lang").val();
    $.get("/system/viewdetailmanual?menu_nm=" + menu_nm + "&lang_cd=EN", function (data) {
        if (data.content != "") {
            var html = '';
            html += '<h1>' + data.title + '</h1>';
            html += '<div id="content">' + data.content + '</div>';
            $("#info_detail").html(html);
            $('.Manual_detail_page').dialog('open');
        }

    });
});

$('#i_VN').click(function () {
    var menu_nm = $("#current_menu").data("level_1") + ">" + $("#current_menu").data("level_2") + ">" + $("#current_menu").data("level_3");
    var lang_cd = $("#current_lang").val();
    $.get("/system/viewdetailmanual?menu_nm=" + menu_nm + "&lang_cd=VN", function (data) {
        if (data.content != "") {
            var html = '';
            html += '<h1>' + data.title + '</h1>';
            html += '<div id="content">' + data.content + '</div>';
            $("#info_detail").html(html);
            $('.Manual_detail_page').dialog('open');
        }

    });
});

function lang_EN(){
    var menu_nm = $("#current_menu").data("level_1") + ">" + $("#current_menu").data("level_2") + ">" + $("#current_menu").data("level_3");
    $("#info_detail").empty();
    $.get("/system/viewdetailmanual?menu_nm=" + menu_nm + "&lang_cd=EN", function (data) {
        var html = '';
        
        html += '<h1>' + data.title + '</h1>';
        html += '<div id="content">' + data.content + '</div>';
        $("#info_detail").html(html);
    });
};

function lang_VN() {
    var menu_nm = $("#current_menu").data("level_1") + ">" + $("#current_menu").data("level_2") + ">" + $("#current_menu").data("level_3");
    $("#info_detail").empty();
    $.get("/system/viewdetailmanual?menu_nm=" + menu_nm + "&lang_cd=VN", function (data) {
        var html = '';        
        html += '<h1>' + data.title + '</h1>';
        html += '<div id="content">' + data.content + '</div>';
        $("#info_detail").html(html);
    });
};

