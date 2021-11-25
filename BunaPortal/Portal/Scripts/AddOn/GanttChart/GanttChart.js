








$(function () {

    "use strict";
    $('.gantt').gantt({
        source: function getdata(request, response) {
            $.ajax({
                type: 'Get',
                url: '@Url.Action("GetHitecJobGanttData", "Report")',
                dataType: 'json',
                async: false,
                success: function (data) {
                    response($.map(data.result, function (val, item) {
                        alert(stringify(val));
                        //return {
                        //    name: val.Name,
                        //    desc: "",
                        //    values: [{
                        //        from: val.From,
                        //        to: val.To,
                        //        label: val.Label,
                        //        customClass: "ganttRed",
                        //        dataObj: val.dataObj,
                        //    }]
                        //}
                    }))
                },
                error: function (data) {
                    alert("SomethingWrong" + data.result);

                }
            })
        },
        navigate: "scroll",
        scale: "weeks",
        maxScale: "months",
        minScale: "days",
        itemsPerPage: 10,
        onItemClick: function (data) {
            alert(data);
        },
        onAddClick: function (dt, rowId) {
            alert("Empty space clicked - add an item!");
        },
        onRender: function () {
            if (window.console && typeof console.log === "function") {
                console.log("chart rendered");
            }
        }
    });

    $(".gantt").popover({
        selector: ".bar",
        title: "I'm a popover",
        content: function () {
            return $(this).data('dataObj');
        },
        trigger: "hover"
    });

});

