<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<EzFloManagerOTIUI.Models.UserResetModel>>" %>
<%@ Import Namespace="MVCLocalization" %>

 <%  Html.Telerik().Window().HtmlAttributes(new { @class = "tWindow" })
            .Name("ResetWindow")
            .Title(Localizer.Current.GetString("Reset Password"))
            .Content(() => {%>
                

                <% using (Html.BeginForm("ResetPassword", "MobileUser", FormMethod.Post, new { id =
                    "reset-form" }))
                   { %>

                        <body  id="resendbody" class="text-align: center;">
                            <table>
                            <tr>
                            <td><input  type="checkbox" class = "checkall" /></td>
                            
                            </tr>

                            <% foreach (var item in Model)
                            {
                            %>

                            <tr>
                                <td>
                                    <%= Html.CheckBoxFor(m => Model[Model.IndexOf(item)].IsChecked)%>
                                </td>
                                <td>
                                <%= Html.DisplayFor(m => Model[Model.IndexOf(item)].Name)%>
                                <%= Html.HiddenFor(m => Model[Model.IndexOf(item)].UserID)%>
                                </td>
                            </tr>
                            <% 
                            } 
                            %>
                                                      
                            </table>     
                        
                            <div class="form-actions">
                                <button id="btnSubmit" class="t-button"><%= Localizer.Current.GetString("Reset") %></button>
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
               $('#reset-open-button')
                    .click(function(e) {
                        e.preventDefault();
                        $('#ResetWindow').data('tWindow').center().open();
                    });

                    $( "#reset-form" ).submit(function( event ) {

                      if( !confirm('Are you sure that you want to reset the passwords for the selected users? ') ) 
                        event.preventDefault();
                    });

                    $('.checkall').click(function(e){
                        var table= $(e.target).closest('table');
                        $('td input:checkbox',table).prop('checked',this.checked);
                    });
 
          <%}); %>
