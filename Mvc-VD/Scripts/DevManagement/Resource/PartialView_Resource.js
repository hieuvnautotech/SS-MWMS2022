$(document).ready(function () {
    getdata_html();
    $.get("/Plan/get_mc_type", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '<select name="mc_type" id="mc_type_pp" class="form-control select2">';
            html += '<option value="" selected="selected">*Type*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.dt_cd + '>' + item.dt_nm + '</option>';
            });
            html += '</select>';
            $("#type_change").html(html);
        }
    });
});

function getdata_html() {
    var month = $("#prev_next").val();
    var po_no = $("#s_po_no").val();
    var product = $("#s_style_no").val();
    var type_kind = $("#type_kind").val();
    var value_code = $("#value_code").val();
    var value_nm = $("#value_nm").val();
    var mc_type = $("#mc_type_pp").val();
    $.get("/DevManagement/PartialView_Resource"+
        "?month=" + month+
         "&product=" + product +
        "&po_no=" + po_no +
         "&type_kind=" + type_kind+
         "&value_code=" + value_code+
         "&value_nm=" + value_nm +
         "&mc_type=" + mc_type 
        , function (data) {
            $("#html").html(data);
            var day = $("#day").val();
            if (day == 1) {
                var d = new Date();
                var day = d.getDate();
                document.getElementById("td_" + day).style.background = "#1bff00";
            }
    });
}
$("#searchBtn").click(function () {
    $("#day").val(0);
    getdata_html();

});
function prev() {
    var value = $("#prev_next").val();
    var kq =parseInt(value) - 1;
    $("#prev_next").val(kq);
    $("#day").val(0);
    getdata_html();
}
function next() {
    var value = $("#prev_next").val();
    var kq = parseInt(value) + 1;
    $("#prev_next").val(kq);
    $("#day").val(0);
    getdata_html();
}
function today() {
    $("#prev_next").val(0);
    $("#day").val(1);
    getdata_html();
}

