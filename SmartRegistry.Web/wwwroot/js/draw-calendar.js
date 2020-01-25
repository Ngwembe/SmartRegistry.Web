$(function () {
    //$.fn.modal.Constructor.prototype.enforceFocus = function () { };    //  For bootstrap 3
    $.fn.modal.Constructor.prototype._enforceFocus = function () { };   //  For bootstrap 4
    
    var events = [];
    
    getSchedules = function () {
        $.ajax({
            type: "GET",
            url: "/Schedules/GetSchedules",
            success: function (data) {

                $.each(data,
                    function (i, event) {
                        console.log(event);
                        events.push({
                            //id: event.id,
                            //title: `${event.subject.code} AT ${event.lectureRoom}`,
                            //start: moment(event.scheduleFor,"yyyy-dd-mm HH:ii:ss"),
                            //end: event.scheduleTo != null ? moment(event.scheduleTo, "yyyy-dd-mm HH:ii:ss") : null,
                            //allDay: false,
                            //lectureRoom: event.lectureRoom,
                            //isConfirmed: event.isConfirmed
                            id: event.id,
                            title: `${event.subject.code} AT ${event.lectureRoom}`,
                            start: event.scheduleFor !== null ? moment(event.scheduleFor) : null, // moment(event.scheduleFor),
                            end: event.scheduleTo !== null ? moment(event.scheduleTo) : null,
                            allDay: false,
                            lectureRoom: event.lectureRoom,
                            isConfirmed: event.isConfirmed
                        });
                    });

                generateCalendar(events);
            },
            error: function (err) {
                alert(err);
            }
        });
    };

    getSchedules();

    var $selectedSchedule = null;

    function generateCalendar(eventData) {

        $('#calendar').fullCalendar('destroy');

        $('#calendar').fullCalendar({
            events: eventData,
            //weekends: true, //false // will hide Saturdays and Sundays
            businessHours: true,
            contentHeight: 400,
            dafaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            //default: { hours: 1, minutes: 30 },
            slotDuration: '01:30:00',
            allDaySlot: false,
            allDayDefault: false,
            nowIndicator: true,
            selectOverlap: false,
            eventStartEditable: true,
            eventDurationEditable: true,
            editable: true,
            eventResize: function (event) {
                var start = $.fullCalendar.formatDate(event.start, "DD-MM-YYYY HH:mm:ss a");
                var end = $.fullCalendar.formatDate(event.end, "DD-MM-YYYY HH:mm:ss a");

                var title = event.title;
                var id = event.id;
                var isConfirmed = event.isConfirmed;
                var lectureRoom = event.lectureRoom;

                var scheduler = {
                    id: id,
                    scheduleFor: /*start,*/ moment(event.start, 'DD-MMM-YYYY, hh:mm A').format('DD-MMM-YYYY hh:mm A'),
                    scheduleTo: /*end,*/ moment(event.end, 'DD-MMM-YYYY, hh:mm A').format('DD-MMM-YYYY hh:mm A'),
                    lectureRoom: lectureRoom,
                    subjectId: id,
                    isConfirmed: isConfirmed
                };

                $.ajax({
                    type: "POST",
                    url: "/Schedules/Edit",
                    data: { id: id, schedule: scheduler },
                    //data: JSON.stringify({
                    //    id: id,
                    //    scheduleFor: start,
                    //    scheduleTo: end,
                    //    lectureRoom: lectureRoom,
                    //    subjectId: id,
                    //    isConfirmed: isConfirmed
                    //}),
                    async: true,
                    success: function (response) {
                        $('#createEventViewModal').modal('hide');
                        $('#createSuccessModal').modal('hide');
                        $('#updateSuccessModal').modal('show');
                    }
                });
            },
            //duration: {hours: 1, minutes: 30},
            header: {
                left: 'prev,next,today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            selectable: true,
            selectHelper: true,
            select: function (start, end, allDay) {
                
                var selectedDate = {
                    startDate: new Date(start),
                    endDate: new Date(end) //,
                    //isAllDay: allDay,
                    //title: add,
                    //lectureRoom: lectureRoom
                };

                var startDate = new Date(start);
                var endDate = new Date(end);

                return $.ajax({
                    type: "POST",
                    url: "/Schedules/GetCreateModalPartialView",
                    data: {
                        scheduleFor: new Date(start).toJSON(),
                        scheduleTo: new Date(end).toJSON()
                        //,title: add,
                        //lectureRoom: lectureRoom
                    },

                    //JSON.stringify(selectedDate),//{startDate: selectedDate.startDate, endDate: selectedDate.endDate}, // //.serialize(),
                    success: function (data) {
                        $('#createModal').empty().html(data);

                        $('.date').datepicker({
                            beforeShowDay: $.datepicker.noWeekends,
                            dateFormat: "dd/MM/yyyy HH:mm"
                            //dateFormat: "dd/MM/yyyy HH:mm:ss.fff"
                        });

                        $('#createEventViewModal').modal('show');

                        $('.select2').select2();

                        $('#saveSchedule').on('click', function (e) {
                            e.preventDefault();

                            $('#createEventViewModal').modal('hide');

                            var scheduleForm = $('#add-patient').serializeArray();

                            $.ajax({
                                type: "POST",
                                url: '/Schedules/Create',
                                dataType: 'json',
                                contentType: 'application/json',
                                data: JSON.stringify(scheduleForm),
                                success: (e) => {
                                    e.preventDefault();

                                    $('#createEventViewModal').modal('hide');
                                    $('#createSuccessModal').modal('show');

                                    getSchedules();
                                }
                            });
                        });
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },
            eventLimit: true,
            eventColor: '#378006',
            eventClick: function (calEvent, jsEvent, view) {
                $selectedSchedule = calEvent;
                
                $.ajax({
                    type: "GET",
                    async: true,
                    url: "/Schedules/PullSchedule",
                    data: { scheduleId: $selectedSchedule.id },
                    success: function (data) {

                        // Create the chart
                        Highcharts.chart('eventDonut', {
                            chart: {
                                type: 'pie'
                            },
                            title: {
                                //text: 'Browser market shares. January, 2018'
                                text: `Student Attendance`
                            },
                            subtitle: {
                                //text: 'Click the slices to view versions. Source: <a href="http://statcounter.com" target="_blank">statcounter.com</a>'
                                text: `Lecture scheduled for <b>${calEvent.start.format('DD-MMM-YYYY HH:mm a')}</b> - <b>${calEvent.end.format('DD-MMM-YYYY HH:mm a')}</b>`
                            },
                            plotOptions: {
                                series: {
                                    dataLabels: {
                                        enabled: true,
                                        format: '{point.name}: {point.y:.1f}%'
                                    }
                                }
                            },

                            tooltip: {
                                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                            },

                            series: [
                                {
                                    name: "Student Attendance",
                                    colorByPoint: true,
                                    data: [
                                        {
                                            name: "Present",
                                            y: data.present,
                                            drilldown: "Chrome"
                                        },
                                        {
                                            name: "Absent",
                                            y: data.absent,
                                            drilldown: "Firefox"
                                        },
                                        //{
                                        //    name: "Internet Explorer",
                                        //    y: 7.23,
                                        //    drilldown: "Internet Explorer"
                                        //},
                                        //{
                                        //    name: "Safari",
                                        //    y: 5.58,
                                        //    drilldown: "Safari"
                                        //},
                                        //{
                                        //    name: "Edge",
                                        //    y: 4.02,
                                        //    drilldown: "Edge"
                                        //},
                                        //{
                                        //    name: "Opera",
                                        //    y: 1.92,
                                        //    drilldown: "Opera"
                                        //},
                                        //{
                                        //    name: "Other",
                                        //    y: 7.62,
                                        //    drilldown: null
                                        //}
                                    ]
                                }
                            ],
                            //drilldown: {
                            //    series: [
                            //        {
                            //            name: "Chrome",
                            //            id: "Chrome",
                            //            data: [
                            //                [
                            //                    "v65.0",
                            //                    0.1
                            //                ],
                            //                [
                            //                    "v64.0",
                            //                    1.3
                            //                ],
                            //                [
                            //                    "v63.0",
                            //                    53.02
                            //                ],
                            //                [
                            //                    "v62.0",
                            //                    1.4
                            //                ],
                            //                [
                            //                    "v61.0",
                            //                    0.88
                            //                ],
                            //                [
                            //                    "v60.0",
                            //                    0.56
                            //                ],
                            //                [
                            //                    "v59.0",
                            //                    0.45
                            //                ],
                            //                [
                            //                    "v58.0",
                            //                    0.49
                            //                ],
                            //                [
                            //                    "v57.0",
                            //                    0.32
                            //                ],
                            //                [
                            //                    "v56.0",
                            //                    0.29
                            //                ],
                            //                [
                            //                    "v55.0",
                            //                    0.79
                            //                ],
                            //                [
                            //                    "v54.0",
                            //                    0.18
                            //                ],
                            //                [
                            //                    "v51.0",
                            //                    0.13
                            //                ],
                            //                [
                            //                    "v49.0",
                            //                    2.16
                            //                ],
                            //                [
                            //                    "v48.0",
                            //                    0.13
                            //                ],
                            //                [
                            //                    "v47.0",
                            //                    0.11
                            //                ],
                            //                [
                            //                    "v43.0",
                            //                    0.17
                            //                ],
                            //                [
                            //                    "v29.0",
                            //                    0.26
                            //                ]
                            //            ]
                            //        },
                            //        {
                            //            name: "Firefox",
                            //            id: "Firefox",
                            //            data: [
                            //                [
                            //                    "v58.0",
                            //                    1.02
                            //                ],
                            //                [
                            //                    "v57.0",
                            //                    7.36
                            //                ],
                            //                [
                            //                    "v56.0",
                            //                    0.35
                            //                ],
                            //                [
                            //                    "v55.0",
                            //                    0.11
                            //                ],
                            //                [
                            //                    "v54.0",
                            //                    0.1
                            //                ],
                            //                [
                            //                    "v52.0",
                            //                    0.95
                            //                ],
                            //                [
                            //                    "v51.0",
                            //                    0.15
                            //                ],
                            //                [
                            //                    "v50.0",
                            //                    0.1
                            //                ],
                            //                [
                            //                    "v48.0",
                            //                    0.31
                            //                ],
                            //                [
                            //                    "v47.0",
                            //                    0.12
                            //                ]
                            //            ]
                            //        },
                            //        {
                            //            name: "Internet Explorer",
                            //            id: "Internet Explorer",
                            //            data: [
                            //                [
                            //                    "v11.0",
                            //                    6.2
                            //                ],
                            //                [
                            //                    "v10.0",
                            //                    0.29
                            //                ],
                            //                [
                            //                    "v9.0",
                            //                    0.27
                            //                ],
                            //                [
                            //                    "v8.0",
                            //                    0.47
                            //                ]
                            //            ]
                            //        },
                            //        {
                            //            name: "Safari",
                            //            id: "Safari",
                            //            data: [
                            //                [
                            //                    "v11.0",
                            //                    3.39
                            //                ],
                            //                [
                            //                    "v10.1",
                            //                    0.96
                            //                ],
                            //                [
                            //                    "v10.0",
                            //                    0.36
                            //                ],
                            //                [
                            //                    "v9.1",
                            //                    0.54
                            //                ],
                            //                [
                            //                    "v9.0",
                            //                    0.13
                            //                ],
                            //                [
                            //                    "v5.1",
                            //                    0.2
                            //                ]
                            //            ]
                            //        },
                            //        {
                            //            name: "Edge",
                            //            id: "Edge",
                            //            data: [
                            //                [
                            //                    "v16",
                            //                    2.6
                            //                ],
                            //                [
                            //                    "v15",
                            //                    0.92
                            //                ],
                            //                [
                            //                    "v14",
                            //                    0.4
                            //                ],
                            //                [
                            //                    "v13",
                            //                    0.1
                            //                ]
                            //            ]
                            //        },
                            //        {
                            //            name: "Opera",
                            //            id: "Opera",
                            //            data: [
                            //                [
                            //                    "v50.0",
                            //                    0.96
                            //                ],
                            //                [
                            //                    "v49.0",
                            //                    0.82
                            //                ],
                            //                [
                            //                    "v12.1",
                            //                    0.14
                            //                ]
                            //            ]
                            //        }
                            //    ]
                            //}
                        });
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });

                $('#eventView #eventViewHeader').text(calEvent.title);
                $('#eventViewVenue')
                    .text(calEvent.lectureRoom !== null ? calEvent.lectureRoom : 'Venue not confirmed');

                var $description = $('<div/>');

                $description.append($('<p/>')
                    .html(`<b>Start:</b> ${calEvent.start.format('DD-MMM-YYYY HH:mm a')} `));

                if (calEvent.end !== null) {
                    $description.append($('<p/>')
                        .html(`<b>End:</b> ${calEvent.end.format('DD-MMM-YYYY HH:mm a')} `));
                }
                //$description.append($('<p/>').html(`<b>Descrption:</b> ${calEvent.Description}`));
                if (calEvent.isConfirmed) {
                    $description.append($('<p/>')
                        .html(`<b class='text-center'>Schedule Confirmed:</b> ${calEvent.isConfirmed
                            } <i class='glyphicon glyphicon-ok-sign'></i>`));
                } else {
                    $description.append($('<p/>')
                        .html(`<b class='text-center'>Schedule Confirmed:</b> ${calEvent.isConfirmed
                            } <i class='glyphicon glyphicon-wrong'></i>`));
                }

                if ($selectedSchedule.isConfirmed)
                    $('#btnConfirmLecture').hide();
                else
                    $('#btnConfirmLecture').show();

                $('#eventView #eventViewBody').empty().html($description);

                //console.log($('#eventView #eventDonut'));


                //Morris.Donut({
                //    element: 'eventDonut',
                //    data: [
                //        {value: 70, label: 'foo'},
                //        {value: 15, label: 'bar'},
                //        {value: 10, label: 'baz'},
                //        {value: 5, label: 'A really really long label'}
                //    ]
                //    //, formatter: function (x) { return x + "%"; }
                //});

                //var testingDonut = Morris.Donut({
                //    element: 'testingDonut', //$('#eventView #eventDonut'),
                //    data: [
                //        { value: 70, label: 'foo' },
                //        { value: 15, label: 'bar' },
                //        { value: 10, label: 'baz' },
                //        { value: 5, label: 'A really really long label' }
                //    ]
                //    //, formatter: function (x) { return x + "%"; }
                //});
                
                //$('#eventView #eventDonut').empty().html($(testingDonut).html());
                //$("#eventDonut").empty().html($("#testingDonut").html());
                //$("#testingDonut").empty();

                $('#eventView').modal('show');
            }
        });
        //$('.draggable').data('duration', '01:30');
    }

    $('#btnConfirmLecture').on('click', function (e) {
        e.preventDefault();

        if (confirm("Are you sure you want to confirm the schedule?")) {
            $.ajax({
                type: "GET",
                url: "/Schedules/ConfirmSchedule",
                data: { scheduleId: $selectedSchedule.id },
                success: function (data) {
                    //TODO: Show confirmation success
                    $('#eventView').modal('hide');
                    getSchedules();
                },
                error: function (err) {
                    alert(err);
                }
            });
        }
    });
         
});

function pullSchedules(subjectId) {
    $.ajax({
        type: "GET",
        url: "/Schedules/PullSchedules",
        data: { subjectId: subjectId },
        success: function (data) {

            $.each(data,
                function (i, event) {

                    events.push({
                        id: event.id,
                        title: `${event.subject.code} AT ${event.lectureRoom}`,
                        start: event.scheduleFor !== null ? moment(event.scheduleFor) : null, // moment(event.scheduleFor),
                        end: event.scheduleTo !== null ? moment(event.scheduleTo) : null,
                        allDay: false,
                        lectureRoom: event.lectureRoom,
                        isConfirmed: event.isConfirmed
                    });
                });

            generateCalendar(events);
        },
        error: function (err) {
            alert(err);
        }
    });

    retrieveSubjectSchedules(subjectId);
}

function retrieveSubjectSchedules(subjectId) {

    $.ajax({
        url: '/Schedules/RetrieveSchedules',
        //async: true,
        type: 'GET',
        contentType: 'application/json',
        data: { subjectId: subjectId },
        success: function (data) {
            
            populateChart(data);
        },
        error: function (err) {
            console.log(err);
        }
    });

};

function populateChart(data) {

    let present = [];
    let absent = [];
    
    let dates = [];

    for (const schedule of data) {

        var total = (schedule.present + schedule.absent);

        let v = total > 0;

        if (v) {

            //  Creating present dataset
            present.push((parseFloat(schedule.present) / parseFloat(total)) * 100.0);

            //  Creating absent dataset
            absent.push((parseFloat(schedule.absent) / parseFloat(total)) * 100.0);            
        }
        else {
            //  Creating present dataset
            present.push(schedule.present * 100.0);

            //  Creating absent dataset
            absent.push(schedule.absent * 100.0);            
        }  

        ////  Creating present dataset
        //present.push(schedule.present);

        ////  Creating absent dataset
        //absent.push(schedule.absent);

        dates.push(new Date(schedule.date).getFullYear() + '-' + (new Date(schedule.date).getMonth() + 1) + '-' + new Date(schedule.date).getUTCDate() +
            ' ' + (new Date(schedule.date).getHours() < 10 ? '0' + new Date(schedule.date).getHours() : new Date(schedule.date).getHours()) +
            ':' + (new Date(schedule.date).getMinutes() < 30 ? '0' + new Date(schedule.date).getMinutes() : new Date(schedule.date).getMinutes()));
    }
    
    var chart = new Highcharts.Chart({
        chart: {
            renderTo: 'continuous-chart',
            type: 'line'
        },
        title: { text: 'Attendance for TPG111B 2019' },
        credits: { enabled: false },
        xAxis: {
            tickmarkPlacement: 'on',
            gridLineWidth: 0,
            maxPadding: 0,
            startOnTick: true,
            endOnTick: true,
            categories: dates
        },
        yAxis: {
            title: {
                text: 'Overall Attendance Statistics'
            },
            tickInterval: 10,
            max: 100    //  (absent[0] + present[0]) * 2
            //,labels: {
            //    formatter: function () {
            //        return Math.round((this.value / 70) * 100) + '%';
            //    }
            //}
        }
    });

    chart.addSeries({ name: 'Present', type: 'area', data: present });
    //chart.addSeries({ name: 'Present', type: 'spline', data: present });
    chart.addSeries({ name: 'Absent', type: 'area', data: absent });
    //chart.addSeries({ name: 'Absent', type: 'spline', data: absent });

    chart.redraw();
}