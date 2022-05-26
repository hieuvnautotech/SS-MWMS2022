
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
    month = checkTime(month);
    date = checkTime(date);
    haftday = (h > 12) ? "PM" : "AM";
    $('#daystatus').html(year + '/' + month + '/' + date );
    $('#timestatus').html(haftday + ' : ' + h + ' : ' + m + ' : ' + s);
}, 1000);