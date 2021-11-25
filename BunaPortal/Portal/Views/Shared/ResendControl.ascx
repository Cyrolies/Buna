<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Common.ResendHelpers.ResendModel>" %>
<%@ Import Namespace="MVCLocalization" %>


 <%  Html.Telerik().Window().HtmlAttributes(new { @class = "Font" })
            .Name("ResendWindow")
            .Title(Localizer.Current.GetString(Model.PageTitleName))
            .Content(() => {%>
                
                <% using (Html.BeginForm("ResendData"+Model.ModelName,Model.ControllerName, FormMethod.Post, new { id =
                    "resend-form" }))
                   { %>
                        <body  id="resendbody" class="text-align: center;">
                            <table>
                                
                                <% if(Model.EventList != null) { %>
                                
                                <tr>
                                <td>
                                    <%= Html.Label(Localizer.Current.GetString("Event"))%>
                                </td>
                                <td>
                                <%= Html.DropDownList("eventName", new SelectList(Model.EventList.Select(x => new { Value = x, Text = x }),
                                    "Value",
                                    "Text"
                                ))%>
                                </td>
                                </tr>

                                <% } %>

                                <tr>
                                    <td><%= Html.Label(Localizer.Current.GetString("DateFrom"))%></td>
                                    <td><%= Html.Telerik().DatePicker().Name("dateFrom").Value(DateTime.Now)%></td>
                                </tr>
                                <tr>
                                    <td><%= Html.Label(Localizer.Current.GetString("DateTo"))%></td>
                                    <td><%= Html.Telerik().DatePicker().Name("dateTo").Value(DateTime.Now)%></td>
                                </tr>
                                <tr>
                                    <td><%= Html.Label(Localizer.Current.GetString("FromStatus"))%></td>
                                    <td><%=Html.DropDownList("fromStatus", new SelectList(Enum.GetValues(typeof(Common.Enumerations.EventStatus))))%></td>
                                    <td>
                                </td>
                                </tr>
                                <tr>
                                    <td><%= Html.Label(Localizer.Current.GetString("ToStatus"))%></td>
                                    <td><%=Html.DropDownList("toStatus", new SelectList(Enum.GetValues(typeof(Common.Enumerations.EventStatus))))%></td>
                                    <td>
                                </td>
                                </tr>                          
                        </table>     
                        
                        <div class="form-actions">
                            <button id="btnSubmit" type="submit" class="t-button"><%= Localizer.Current.GetString("Resend") %></button>
                        </div>           
                        </body>
                <% } %>
            <%})
            .Width(400)
            .Draggable(true)
            .Modal(true)
            .Visible(false)
            .Render();
    %>
    
     <% Html.Telerik().ScriptRegistrar()
           .OnDocumentReady(() =>
           {%>
                // open the initially hidden window when the button is clicked
               $('#resend-open-button')
                    .click(function(e) {
                        e.preventDefault();
                        $('#ResendWindow').data('tWindow').center().open();
                    });
                
          <%}); %>
