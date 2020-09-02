//  Highcharts

function drawWeeklyChart(subjectName, respData) {
    return Highcharts.chart('weeklyChart', {
        chart: {
            type: 'areaspline'
        },
        title: {
            text: `Weekly Attendance Summary ${subjectName}`
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            verticalAlign: 'top',
            x: 150,
            y: 100,
            floating: true,
            borderWidth: 1,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        xAxis: {
            categories: [
                'Monday',
                'Tuesday',
                'Wednesday',
                'Thursday',
                'Friday',
                'Saturday',
                'Sunday'
            ],
            plotBands: [{ // visualize the weekend
                from: 4.5,
                to: 6.5,
                color: 'rgba(68, 170, 213, .2)'
            }]
        },
        yAxis: {
            title: {
                text: 'Attendance Percentage (%)'
            }
        },
        tooltip: {
            shared: true,
            valueSuffix: ' %'
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            areaspline: {
                fillOpacity: 0.5
            }
        },
        series: respData
    });
}
    
function drawMonthlyChart(subjectName, respData) {

    return Highcharts.chart('monthlyViewChart', {
        chart: {
            type: 'area'
        },
        title: {
            text: `Monthly Attendance Summary for ${subjectName}`
        },
        subtitle: {
            text: 'Week # of semester # summary'
            //text: 'Sources: <a href="https://thebulletin.org/2006/july/global-nuclear-stockpiles-1945-2006">' +
            //'thebulletin.org</a> &amp; <a href="https://www.armscontrol.org/factsheets/Nuclearweaponswhohaswhat">' +
            //'armscontrol.org</a>'
        },
        xAxis: {
            allowDecimals: false,
            labels: {
                formatter: function () {
                    return this.value; // clean, unformatted number for year
                }
            }
        },
        yAxis: {
            title: {
                text: 'Overall Attendance Statistics (%)'
            },
            labels: {
                formatter: function () {
                    //return this.value / 1000 + 'k';
                    return Math.round((this.value / 70) * 100) + '%';
                }
            }
        },
        tooltip: {
            pointFormat: '{series.name} had attended <b>{point.y:,.0f}</b><br/> in {point.x}'
        },
        plotOptions: {
            area: {
                pointStart: 2018,
                marker: {
                    enabled: false,
                    symbol: 'circle',
                    radius: 2,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                }
            }
        },
        series: respData
    });

}