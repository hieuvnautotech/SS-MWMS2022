$(document).ready(function () {
   getdata_html();

});

function getdata_html() {
    var productCode = $('#s_style_no').val().trim();
    var po_no = $('#s_po_no').val().trim();
    var machineCode = $('#c_mc_no').val().trim();
    $.get("/DevManagement/PartialView_ResourceMgt"+
        "?productCode=" + productCode+
        "&po_no=" + po_no +
        "&machineCode=" + machineCode
        , function (data) {
            $("#html").html(data);
            var day = $("#day").val();
            if (day == 1) {
                var d = new Date();
                var day = d.getDate();
              //  document.getElementById("td_" + day).style.background = "#1bff00";
            }
    });
}
$("#searchBtn").click(function () {

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

