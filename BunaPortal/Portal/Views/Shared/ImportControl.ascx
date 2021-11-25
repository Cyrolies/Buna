<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Common.ExportHelpers.ExportTemplate>" %>
<%@ Import Namespace="MVCLocalization" %>
 <%--Export code--%>
 
 <%  Html.Telerik().Window().HtmlAttributes(new { @class = "Font" })
            .Name("UploadWindow")
            .Title(Localizer.Current.GetString("Upload Template"))
            .Content(() => {%>
                
                <% using (Html.BeginForm("ImportData" + Model.ModelName, Model.ControllerName, FormMethod.Post, new
                   {
                       id =
                    "upload-template-form", enctype = "multipart/form-data" }))
                   { %>
                        <body  id="exportbody" class="text-align: center;">
                              <label for="file"><%= Localizer.Current.GetString("Filename:") %></label>
                              <input type="file" name="file" id="file" />
                        <br /> <br />

                        <div class="form-actions">
                        <br /> <br />
                            <button id="btnSave" type="submit" class="t-button"><%= Localizer.Current.GetString("Import") %></button>
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
               $('#import-target-open-button')
                    .click(function(e) {
                        e.preventDefault();
                        $('#UploadWindow').data('tWindow').center().open();
                    });

                
                //Submit button function gets grid filter/sortby and current page values
                $('#btnSave')
                    .click(function(e) {
                   
                   });
                
             


          <%}); %>

           
<%--End Export code--%>