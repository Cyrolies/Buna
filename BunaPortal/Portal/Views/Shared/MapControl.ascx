<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="MVCLocalization" %>
 <%--Export code--%>
 
 <%  Html.Telerik().Window().HtmlAttributes(new { @class = "Font" })
            .Name("MapWindow")
            .Title(Localizer.Current.GetString("Map"))
            .Content(() => {%>
                <div id="googleMap" style="width:1000px;height:600px;"></div>
                <div id="legend" style="visibility: hidden;">
                </div>
            <%})
            .Width(1000)
            .Draggable(true)
            .Modal(true)
            .Visible(false)
            .Render();
    %>
    
     
     <% Html.Telerik().ScriptRegistrar()
           .OnDocumentReady(() =>
           {%>
                // open the initially hidden window when the button is clicked
               $('.mapLink')
                    .click(function(e) {
                        e.preventDefault();

                        var shipmentNumber = $(this).html();

                        initMap(shipmentNumber);
                    });

                function initMap(x) {
                    
                    $('#MapWindow').data('tWindow').center().open();

                    google.maps.visualRefresh = true;

                    var params = { shipmentNumber: x};
                    var paramsStr = $.param(params);
                    var selectUrl = "<%= @Url.Action("GetMapLocations", "Delivery")%>"  + "?" + paramsStr;
                    console.log(selectUrl);
                    $.ajax({
                    url: selectUrl,
                    type: "GET",
                    success: function (result) 
                    {
                        var locations = JSON.parse(result);

                        if(locations.length > 0)
                        {
                            var firstLocation = locations[0];

                            var myLatLng = new google.maps.LatLng(firstLocation.Latitude, firstLocation.Longitude);

                            var mapOptions = {
                                zoom: 9,
                                center: myLatLng,
                                mapTypeId: google.maps.MapTypeId.ROADMAP
                            };

                            var map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);

                            map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(
                            document.getElementById('legend'));

                            var legend = document.getElementById('legend');

                            var divCancelled = document.createElement('div');
                            divCancelled.innerHTML = '<img src="https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=|FFFF00|000000"> ' + 'Cancelled';
                            legend.appendChild(divCancelled);

                            var divCompleted = document.createElement('div');
                            divCompleted.innerHTML = '<img src="https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=|00FF00|000000"> ' + 'Completed';
                            legend.appendChild(divCompleted);

                            var divPending = document.createElement('div');
                            divPending.innerHTML = '<img src="https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=|0000FF|000000"> ' + 'Pending';
                            legend.appendChild(divPending);

                            var infowindow = new google.maps.InfoWindow({
                                content: "..."
                            });

                            $.each(locations, function (i, item)
                            {
                                var marker = new google.maps.Marker({
                                'position': new google.maps.LatLng(item.Latitude, item.Longitude),
                                'map': map,

                                icon:'https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld='+ item.VisitNumber +'|00FF00|000000', 'title': item.Name})

                                if(item.DeliveryStatus == 'Completed')
                                {
                                    google.maps.event.addListener(marker, 'click', function () {

                                        var windowContent = "<b>" + item.VisitNumber + ". " + item.Name + " - " + item.CustomerId + "</b><br />";
                                        windowContent += "<p> Start Time : " + item.StartTime + "<br />";
                                        windowContent += "End Time : " + item.EndTime + "</p>";

                                        infowindow.setContent(windowContent);
                                        infowindow.open(map, this);
                                    });
                                }
                                else if(item.DeliveryStatus == 'Cancelled')
                                {
                                    marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=|FFFF00|000000');    
                                    
                                    google.maps.event.addListener(marker, 'click', function () {

                                        var windowContent = "<b>" + item.Name + " - " + item.CustomerId + "</b><br />";
                                        windowContent += "<p> Date Cancelled : " + item.DateCancelled + "</p>";

                                        infowindow.setContent(windowContent);
                                        infowindow.open(map, this);
                                    });                                
                                }
                                else
                                {
                                    marker.setIcon('https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=|0000FF|000000'); 
                                    
                                    google.maps.event.addListener(marker, 'click', function () {

                                        var windowContent = "<b>" + item.Name + " - " + item.CustomerId + "</b><br />";

                                        infowindow.setContent(windowContent);
                                        infowindow.open(map, this);
                                    });    
                                }
                            });

                        }
                    }
                });


                }

          <%}); %>

           