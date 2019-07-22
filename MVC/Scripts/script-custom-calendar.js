$(document).ready(function () {
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
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: "/Calendar/GetCalendarData",
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    var events = [];

                    $.each(result, function (i, data) {
                        events.push(
                            {
                                id: data.id,
                                title: data.title,
                                description: data.discription,
                                start: moment(data.start).format('YYYY-MM-DD'),
                                end: moment(data.end).format('YYYY-MM-DD'),
                                color: 'darkBlue',
                                textColor: 'white'
                            });
                    });
                    
                    callback(events);
                }
            });
        },
        eventRender: function (event, element) {
            element.qtip(
                {
                    content: {
                        text: event.description,
                        title: "行程描述"
                    },
                    position: {
                        my: 'top left',  // Position my top left...   
                        at: 'bottom right', // at the bottom right of...   
                    },
                    hide: {
                        fixed: true,
                        delay: 300
                    },
                    style: {
                        classes: 'qtip-bootstrap'
                    }
                });
        },
        dayClick: function (date, jsEvent, view, resourceObj) {

            alert('Date: ' + date.format());
            alert('Resource ID: ' + resourceObj.id);

        },
        editable: false
    });
}); 