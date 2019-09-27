$(document).ready(function () {
    $('#external-events .fc-event').each(function () {

        // store data so the calendar knows to render an event upon drop
        $(this).data('event', {
            title: $.trim($(this).text()), // use the element's text as the event title
            stick: true // maintain when user navigates (see docs on the renderEvent method)
        });

        // make the event draggable using jQuery UI
        $(this).draggable({
            zIndex: 999,
            revert: true,      // will cause the event to go back to its
            revertDuration: 0  //  original position after the drag
        });

    });
    $('#calendar').fullCalendar({
        header:
        {
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
        //events: function (start, end, timezone, callback) {
        //    $.ajax({
        //        url: "/Main/GetCalendarData",
        //        type: "GET",
        //        dataType: "JSON",

        //        success: function (result) {
        //            var events = [];

        //            $.each(result, function (i, data) {
        //                events.push(
        //                    {
        //                        id: data.id,
        //                        title: data.title,
        //                        description: data.discription,
        //                        start: moment(data.start).format('YYYY-MM-DD'),
        //                        end: moment(data.end).format('YYYY-MM-DD'),
        //                        color: 'darkBlue',
        //                        textColor: 'white'
        //                    });
        //            });
                    
        //            callback(events);
        //        }
        //    });
        //},
        eventRender: function (event, element) {
            element.qtip(
                {
                    content: {
                        text: event.description,
                        title: "行程描述"
                    },
              
                    hide: {
                        fixed: true,
                        delay: 300
                    },      position: {
                        my: 'top left',  // Position my top left...   
                        at: 'bottom right', // at the bottom right of...   
                    },
                    style: {
                        classes: 'qtip-bootstrap'
                    }
                });
        },
        dayClick: function (date, jsEvent, view, resourceObj) {
            alert('Date: ' + date.format());
        },
        editable: true,
        droppable: true,
        drop: function () {
            // is the "remove after drop" checkbox checked?
            if ($('#drop-remove').is(':checked')) {
                // if so, remove the element from the "Draggable Events" list
                $(this).remove();
            }

            //------------------------------//
            //--事件丟到行事曆後的資料寫入--//
            //------------------------------// 

        }, // this allows things to be dropped onto the calendar

        //----------------------------------------------------------------------------------------//
        //將行事曆的事件丟回 external_list//
        eventDragStop: function (event, jsEvent, ui, view) {

            if (isEventOverDiv(jsEvent.clientX, jsEvent.clientY)) {
                $('#calendar').fullCalendar('removeEvents', event._id);
                var el = $("<div class='fc-event'>").appendTo('#external-events-listing').text(event.title);
                el.draggable({
                    zIndex: 999,
                    revert: true,
                    revertDuration: 0
                });
                el.data('event', { title: event.title, id: event.id, stick: true });
                alert('Hello');
            }
        }
    });
    var isEventOverDiv = function (x, y) {

        var external_events = $('#external-events');
        var offset = external_events.offset();
        offset.right = external_events.width() + offset.left;
        offset.bottom = external_events.height() + offset.top;

        // Compare
        if (x >= offset.left
            && y >= offset.top
            && x <= offset.right
            && y <= offset.bottom) { return true; }
        return false;

    }
}); 