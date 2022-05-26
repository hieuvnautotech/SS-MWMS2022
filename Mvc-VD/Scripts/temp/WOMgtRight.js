   
    $(document).ready(function () {
           linecolor();
       });
    function linecolor() {
         
        $.get("/ProducePlan/GetWoMgtLeft", function (data) {
            if (data != null && data != undefined && data.length) {
                $.each(data, function (key, item) {
                    var event = $('<div />');
                    //event = '<div class="external-event" style="background:' + item.line_color + '; color:#fff">' + item.line_cd + '</div>';
                    event.css({ "color": "#fff", "background": item.line_color }).addClass("external-event");
                    event.html(item.line_cd);
                    $('#external-events').prepend(event);
                    ini_events($(event));
                });
            }
        });
    }
    /* initialize the external events
    //-----------------------------------------------------------------*/
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
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },
        //Random default events
        eventTextColor: '#fff',
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '/ProducePlan/GetWoMgtRight',
                type: "GET",
                dataType: "JSON",
                   
                success: function (result) {
                    var events = [];

                    $.each(result, function (key, value) {
                        //alert(value.line_cd);
                        events.push(
                    {
                        title: value.line_cd,
                        id: value.oldno,
                        start: moment(value.start_dt).format('YYYY-MM-DD'),
                        end: moment(value.end_dt).format('YYYY-MM-DD'),
                        backgroundColor: value.line_color,
                        borderColor: value.line_color,
                    });
                    });
                    callback(events);
                }
            });
        },
      
        selectable: true,
        contentHeight: 400,
        defaultDate: new Date(),
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function (date, allDay, event) { // this function is called when something is dropped

            // retrieve the dropped element's stored Event Object
            var originalEventObject = $(this).data('eventObject');

            // we need to copy it, so that multiple events don't have a reference to the same object
            var copiedEventObject = $.extend({}, originalEventObject);

            // assign it the date that was reported
            copiedEventObject.start = date;
            copiedEventObject.end = date;
            copiedEventObject.allDay = false;
            copiedEventObject.backgroundColor = $(this).css("background-color");
            copiedEventObject.borderColor = $(this).css("border-color");

            // render the event on the calendar
            // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
            $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            // is the "remove after drop" checkbox checked?
            if ($('#drop-remove').is(':checked')) {
                // if so, remove the element from the "Draggable Events" list
                $(this).remove();
            }

            $.ajax({

                url: '/ProducePlan/CreateColor',

                data: '&title=' + originalEventObject.title + '&start=' + moment(copiedEventObject.start).format() + '&end=' + moment(copiedEventObject.end).format() + '&id=' + copiedEventObject.id,

                type: "POST",

                success: function(json) {

                    $("#calendar").fullCalendar('renderEvent',

                    {

                        id: json.id,

                        title: title,

                        start: startTime,

                        end: endTime,

                    },

                    true);

                }
            });
        },
        eventResize: function(event) {  // resize to increase or decrease time of event

            $.ajax({

                url: '/ProducePlan/UpdateColor',

                data: '&title='+event.title+'&start='+moment(event.start).format()+'&end='+moment(event.end).format()+'&id='+event.id ,

                type: "POST",

                success: function(json) {

                }

            })
        },
        eventRender: function (event, element) {
            element.append("<span class='removebtn'>X</span>");
            element.find(".removebtn").click(function () {
           
                $(".removebtn").click(function () {
                    $('.dialogDangerous').dialog('open');
                });

                $('#closeWo').click(function () {
                    $('.dialogDangerous').dialog('close');
                });

                $("#deleteWo").click(function () {
                    $.ajax({

                        url: '/ProducePlan/DeleteColor',

                        data: '&title=' + event.title + '&start=' + moment(event.start).format() + '&end=' + moment(event.end).format() + '&id=' + event.id,

                        type: "POST",

                        success: function (json) {

                            $('#calendar').fullCalendar('removeEvents', event._id);

                        },

                        error: function (result) { }
                    });
                    $('.dialogDangerous').dialog('close');
                });
  
            });
        },
    });
    


    