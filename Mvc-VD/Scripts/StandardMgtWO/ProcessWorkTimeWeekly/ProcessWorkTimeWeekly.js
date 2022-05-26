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
        right: '',
    },
    buttonText: {
        today: 'today',
    },
    defaultView: 'basicWeek',
    aspectRatio: 1.5,
    //Random default events
    eventTextColor: '#fff',
    events: function (start, end, timezone, callback) {
        var start1 = window.moment(start._d).format('LL');
        var end1 = window.moment(end._d).format('LL');
        $.ajax({
            url: '/StandardMgtWO/GetWorkTimeWeek?start_dt=' + start1 + '&end_dt=' + end1,
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
    
        element.append("<i class='fa fa-pencil removebtn' data-id='" + event._id + "' onclick='updatetime(this)'></i>");
        //element.find(".removebtn").click(function () {

        //    $('.dialogDangerous').dialog('open');


        //    $("#hour").change(function () {
        //        var time = $(this).val();

        //        $.ajax({

        //            url: '/StandardMgtWO/updatetime?time2=' + time,

        //            data: '&id=' + event.id,

        //            type: "POST",

        //            success: function (json, id) {
        //                $('#calendar').fullCalendar('refetchEvents');
        //                var ngay = json.substring(0, 4) + "-" + json.substring(4, 6) + "-" + json.substring(6, 8);
        //                alert("You have just update day:" + ngay + "")
        //            },
        //            error: function (result) {
        //                $('#calendar').fullCalendar('refetchEvents');
        //                alert('Time past not change')
        //            }
        //        });
        //        $('.dialogDangerous').dialog('close');

        //    });
        //});
    },
});
function updatetime(e) {
    //var r = confirm("Are you make sure UPDATE Events?");
    //if (r === true) {
        $('.dialogDangerous').dialog('open');
        var id = $(e).data("id").toString();
        $("#hour").change(function () {
            var time = $(this).val();

            $.ajax({
                url: '/StandardMgtWO/updatetime?&id=' + id + '&time2=' + time,
                type: "POST",
                success: function (response) {
                    if (response.result) {
                        $('#calendar').fullCalendar('removeEvents', event._id);
                        $('#calendar').fullCalendar('refetchEvents');
                        var ngay = response.data.substring(0, 4) + "-" + response.data.substring(4, 6) + "-" + response.data.substring(6, 8);
                        alert("You have just update day:" + ngay + "")

                    } else {
                        $('#calendar').fullCalendar('refetchEvents');
                        alert('Time past not change')
                    }
                },
                error: function (response) {

                    $('#calendar').fullCalendar('refetchEvents');
                    alert('Time past not change')
                }
            });
            $('.dialogDangerous').dialog('close');
        });
    //}
}
$(document).ready(function () {
    _Gettime();
});
function _Gettime() {
    $.get("/StandardMgtWO/selectboxhour", function (data) {
        if (data != null && data != undefined && data.length) {
            var html = '';
            html += '<option value="" selected="selected">*Hour*</option>';
            $.each(data, function (key, item) {
                html += '<option value=' + item.policy_name + "," + item.work_hour + '>' + item.policy_name + '[' + item.work_hour + ']</option>';
            });
            $("#hour").html(html);
            $("#hour1").html(html);

        }
    });
}
$(".dialogDangerous").dialog({
    width: 350,
    height: 100,
    maxWidth: 350,
    maxHeight: 300,
    minWidth: 300,
    minHeight: 400,
    resizable: false,
    fluid: true,
    modal: true,
    autoOpen: false,
    classes: {
        "ui-dialog": "ui-dialog",
        "ui-dialog-titlebar": "ui-dialog ui-dialog-titlebar-sm",
    },
    resize: function (event, ui) {
        $('.ui-dialog-content').addClass('m-0 p-0');
    },
    open: function (event, ui) {

    },
});









