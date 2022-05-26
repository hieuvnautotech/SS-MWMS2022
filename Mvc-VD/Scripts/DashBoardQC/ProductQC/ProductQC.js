$(document).ready(function () {
    $('#container-pie').addClass("hidden");
    $('#container-bar').removeClass("active");
    //gettable();
    //gettable2();
    //$('#end').datepicker({
    //    dateFormat: 'yy-mm-dd',
    //}).datepicker('setDate', new Date().getDay);
    $('#end').datepicker({
        dateFormat: 'yy-mm-dd',
    }).datepicker('setDate', 'today');
    $('#start').datepicker({
        dateFormat: 'yy-mm-dd',
    }).datepicker('setDate', 'today');

    var start = $('#start').val();
    var end = $('#end').val();

    //start chart bar
    $.get("/DashBoardQC/_OQCDash_Bar?start=" + start + "&end=" + end, function (html) {
        $('#_OQCDash_Bar').html(html);
    });
    //start chart pie
    $.get("/DashBoardQC/_OQCDash_Pie?start=" + start + "&end=" + end, function (html) {
        $('#_OQCDash_Pie').html(html);
    });

});
$("#barchart").change(function () {
    var id = $(this).val();
    if (id == 1) {
        $("#_OQCDash_Bar").addClass("active");
        $("#_OQCDash_Bar").removeClass("hidden");

        $("#_OQCDash_Pie").addClass("hidden");
        $("#_OQCDash_Pie").removeClass("active");

    }
    else {
        $("#_OQCDash_Pie").addClass("active");
        $("#_OQCDash_Pie").removeClass("hidden");

        $("#_OQCDash_Bar").addClass("hidden");
        $("#_OQCDash_Bar").removeClass("active");

    }
});

$("#searchBtn").click(function () {
    var ml_no = $('#s_ml_no').val();
    var start = $('#start').val();
    var end = $('#end').val();
    var id = $('#barchart').val();
    if (id == 1) {
        //start chart
        $.get("/DashBoardQC/_OQCDash_Bar?ml_no=" + ml_no + "&start=" + start + "&end=" + end, function (html) {
            $('#_OQCDash_Bar').html(html);
        });
    }
    else {
        $.get("/DashBoardQC/_OQCDash_Pie?ml_no=" + ml_no + "&start=" + start + "&end=" + end, function (html) {
            $('#_OQCDash_Pie').html(html);
        });
    }
});
