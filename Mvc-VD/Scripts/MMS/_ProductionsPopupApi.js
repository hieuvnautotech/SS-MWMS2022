
function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}
setInterval(function () {
    var d = new Date();
    var s = d.getSeconds();
    var m = d.getMinutes();
    var h = d.getHours();
    var year = d.getFullYear();
    var month = d.getMonth();
    var date = d.getDate();
    
    s = checkTime(s);
    m = checkTime(m);
    h = checkTime(h);
    year = checkTime(year);
    month = checkTime(month + 1);
    date = checkTime(date);
    haftday = (h > 12) ? "PM" : "AM";
    $('#daystatus').html(year + '/' + month + '/' + date );
    $('#timestatus').html(haftday + ' : ' + h + ' : ' + m + ' : ' + s);
}, 1000);

function getdataproduct() {
    $.ajax({
        url: "/MMS/ProductionsPopupAPI",
        type: "get",
        dataType: "json",
        data: {
            line: "line001",
        },
        success: function (data) {
            var line_name = data[0].line_cd;
            line_name = line_name.charAt(0).toUpperCase() + line_name.slice(1).toLowerCase();
            var avg_data = data[0].done_qty / data[0].prod_qty;
            $('#linename').html(line_name);
            $('#oldd_no').html(data[0].oldd_no);
            $('#prod_qty').html(':&nbsp;&nbsp;' + data[0].prod_qty + '  pcs');
            $('#done_qty').html(':&nbsp;&nbsp;' + data[0].done_qty + '  pcs');
            $('#avg_data').html(avg_data.toFixed(2) + ' %');
        },
    });
};

getdataproduct();
setInterval(function () {
    getdataproduct();
}, 3000);



//$ajax({
//    url: '/MMS/ProductionsPopupAPI',
//    type: 'get',
//    datatype: 'json',
//    data: {},
//    success: function (result) {

//    },
//});