var canvas = $('#main_canvas'),
    //context = canvas.getContext('2d'),
    video = $('#main_video'),
    vendorUrl = window.URL || window.webkitURL;

navigator.getMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;

navigator.getMedia({
    video: true,
    audio: false
}, function (stream) {
    video.src = vendorUrl.createObjectURL("rtsp://admin:sy0630hi@192.168.100.139:1554/h264_stream");
    video.play();

}, function (error) {
    //error
});