

var app = {};

var chartDom = document.getElementById('multibarechart');
var myChart = echarts.init(chartDom);
var option;

option = {
    legend: {},
    tooltip: {},
    dataset: {
        source: [
            ['Status', 'Open', 'Completed', 'Invoiced'],
            ['Jan', 43.3, 85.8, 93.7],
            ['Feb', 83.1, 73.4, 55.1],
            ['Mar', 86.4, 65.2, 82.5],
            ['Apr', 72.4, 53.9, 39.1],
            ['May', 72.4, 53.9, 39.1],
            ['Jun', 72.4, 53.9, 39.1],
            ['Jul', 72.4, 53.9, 39.1],
            ['Aug', 12.4, 3.9, 1],
        ]
    },
    xAxis: { type: 'category' },
    yAxis: {},
    // Declare several bar series, each will be mapped
    // to a column of dataset.source by default.
    series: [
        { type: 'bar' },
        { type: 'bar' },
        { type: 'bar' }
    ]
};

option && myChart.setOption(option);
