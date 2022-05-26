function ini_events(ele) {
    ele.each(function () {

        // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
        // it doesn't need to have a start or end
        var eventObject = {
            title: $.trim($(this).text()) // use the element's text as the event title
        };

        // store the Event Object in the DOM element so we can get to it later
        $(this).data('eventObject', eventObject);

        // make the event draggable using jQuery UI
        $(this).draggable({
            zIndex: 1070,
            revert: true, // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });
}
//ini_events($('#external-events div.external-event'));


/* initialize the calendar
-----------------------------------------------------------------*/
//Date for the calendar events (dummy data)
var date = new Date();
var d = date.getDate(),
        m = date.getMonth(),
        y = date.getFullYear();
    
$('#calendar').fullCalendar({
    header: {
        left: 'prev,next',
        center: 'title',
        right: 'month',
    },
    buttonText: {
        today: 'today',
    },
    //Random default events
    eventTextColor: '#fff',
    events: function (start, end, timezone, callback) {
        var start1 = window.moment(start._d).format("LL");
        var end1 = window.moment(end._d).format("LL");
        $.ajax({
            url: '/StandardMgtWO/GetWoMgtRight?start_dt=' + start1 + '&end_dt=' + end1 ,
            type: "GET",
            dataType: "JSON",

            success: function (result) {
                var events = [];

                $.each(result, function (key, value) {
                    //alert(value.line_cd);
                    events.push(
                    {
                            
                        title: value.prounit_nm + "[" + value.work_time + ":00" + "]",
                        id: value.wtid,
                        start: moment(value.start_dt).format('YYYY-MM-DD'),
                        end: moment(value.end_dt).format('YYYY-MM-DD'),
                        backgroundColor: value.color,
                        borderColor: value.color,
                    });
                });
                callback(events);
            }
        });
    },

    selectable: true,
    contentHeight: 1100,
    defaultDate: new Date(),
    editable: true,
    droppable: true, // this allows things to be dropped onto the calendar !!!
    drop: function (date, allDay, event) { // this function is called when something is dropped

        var originalEventObject = $(this).data('eventObject');

        var copiedEventObject = $.extend({}, originalEventObject);

        copiedEventObject.start = date;
        copiedEventObject.end = date;
        copiedEventObject.allDay = allDay;

        copiedEventObject.backgroundColor = $(this).css("background-color");
        copiedEventObject.borderColor = $(this).css("border-color");

        fo_no: $('#m_fo_no').val();
        $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);
        var fo_no = $('#m_fo_no').val()
        $.ajax({
            url: '/StandardMgtWO/CreateColor',
            data: {
                title: originalEventObject.title,
                start: moment(copiedEventObject.start).format(),
                end: moment(copiedEventObject.end).format(),
                fo_no: fo_no
            },
            type: "get",
            dataType: "json",
            success: function (result) {
                location.reload();
            }

        });
    },

    eventRender: function (event, element) {

        element.append("<i class='fa fa-pencil removebtn' aria-hidden='true'></i>");
        element.find(".removebtn").click(function () {

            $('.dialogDangerous').dialog('open');

         
            $("#hour").change(function () {
                var time = $(this).val();
                 
                    $.ajax({

                        url: '/StandardMgtWO/updatetime?time2=' + time,

                        data: '&id=' + event.id,

                        type: "POST",

                        success: function (json, id) {
                            $('#calendar').fullCalendar('refetchEvents');
                            var ngay =json.substring(0,4)+"-"+json.substring(4,6)+"-"+json.substring(6,8);
                            alert("You have just update day:" + ngay + "")
                        },
                        error: function (result) {
                            $('#calendar').fullCalendar('refetchEvents');
                            alert('Time past not change')
                        }
                    });
                    $('.dialogDangerous').dialog('close');

            });
        });
    },
});

$(document).ready(function () {
    _Gettime();
});
function _Gettime() {
    $.get("/StandardMgtWO/selectboxhour", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Hour*</option>';
            $.each(data, function (key, item) {
                //html += '<option value=' + item.policy_name + "," + item.work_hour + '>' + item.policy_name + '[' + item.work_hour + ']</option>';
                var gtri = item.policy_name + "," + item.work_hour;
                var gtri2 = item.policy_name + "[" + item.work_hour +"]";
                html += "<option value ='" + gtri + "' >" + gtri2 + " </option> "
            });
            $("#hour").html(html);
            $("#hour1").html(html);

        }
    });
}

$("#modifi").click(function () {
    if ($("#hour1").val().trim() == "") {
        alert("Please select your hour");
        $("#hour1").val("");
        $("#hour1").focus();
        return false;
    }
    if ($("#thang").val().trim() == "") {
        alert("Please enter your time");
        $("#thang").val("");
        $("#thang").focus();
        return false;
    }
    else {
        var time = $('#hour1').val().trim();
        var thang = $('#thang').val().trim();
        $.ajax({
            url: "/StandardMgtWO/updateline",
            type: "get",
            dataType: "json",
            data: {
                time: time,
                thang: thang,
            },
            success: function (json, id) {
                $('#calendar').fullCalendar('refetchEvents');
                alert("You have just update all line in day :" + thang + " and time is:   " + time + " ");
            },
            error: function (result) {
                $('#calendar').fullCalendar('refetchEvents');
                alert('Time past not change')
            }
        });
    }
});


$(document).ready(function () {
    _Getmonth();
});
function _Getmonth() {
    var html = '';
    html += '<option selected="selected">*FROM MONTH*</option>';
    var bien = 0;
    var name = new Array();
    var month = new Array();
    name[1] = "January";
    name[2] = "February";
    name[3] = "March";
    name[4] = "April";
    name[5] = "May";
    name[6] = "June";
    name[7] = "July";
    name[8] = "August";
    name[9] = "September";
    name[10] = "October";
    name[11] = "November";
    name[12] = "December";
    //
    month[0] = 1;
    month[1] = 2;
    month[2] = 3;
    month[3] = 4;
    month[4] = 5;
    month[5] = 6;
    month[6] = 7;
    month[7] = 8;
    month[8] = 9;
    month[9] = 10;
    month[10] = 11;
    month[11] = 12;
    var ds_thang = [];
    var d = new Date();
    var n = name[d.getMonth()];
    var l = month[d.getMonth()];
    var year =parseInt(d.toLocaleDateString().substr(d.toLocaleDateString().toString().length-4,4));
    console.log(year);
    bien = l;
    var cong = 0;
    for (var i = 0; i < 12; i++) {
        if (bien == 12) {
            cong = 0;
        }
        if (bien == 1 && i != 0) {
            year = year + 1;
        }
        ds_thang.push(bien);
        html += '<option value=' + bien +year+ '>' + name[bien] + '</option>';
        bien = cong + i + 1;
    }
    $("#month").html(html);
}

$("#insert_thang").click(function () {
    if ($("#month").val().trim() == "") {
        alert("Please select your from month");
        $("#month").val("");
        $("#month").focus();
        return false;
    }
    if ($("#many").val().trim() == "") {
        alert("Please enter your NUMBER OF MONTHS CREATE");
        $("#many").val("");
        $("#many").focus();
        return false;
    }
    else {
        var month = $('#month').val();
        var many = $('#many').val();
        $.ajax({
            url: "/StandardMgtWO/insert_thang",
            type: "get",
            dataType: "json",
            data: {
                month: month,
                many: many,
            },
            success: function (json, id) {
                $('#calendar').fullCalendar('refetchEvents');
                alert("You have just update all line in day :" + thang + " and time is:   " + time + " ");
            },
            error: function (result) {
                $('#calendar').fullCalendar('refetchEvents');
                alert('Time past not change')
            }
        });
    }
});
