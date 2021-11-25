<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Common.ExportHelpers.ExportTemplate>" %>
<%@ Import Namespace="MVCLocalization" %>
<%@ Import Namespace="DALEFModel" %>
<%@ Import Namespace="EzFloManagerBLL" %>
 <%--Export code--%>

 <% 
     DriverManager drvMng = new DriverManager();
     List<sysUser> drivers = drvMng.GetsysUser(false).ToList();   
     
     SalesTargetManager salTrgMng = new SalesTargetManager();
     List<tblTargetTemplate> templates = salTrgMng.GettblTargetTemplate(false).ToList();
 %>

 <%  Html.Telerik().Window().HtmlAttributes(new { @class = "Font" })
            .Name("LinkWindow")
            .Title(Localizer.Current.GetString("Link Driver"))
            .Content(() => {%>
                
                <% using (Html.BeginForm("LinkDriver"+Model.ModelName,Model.ControllerName, FormMethod.Post, new { id =
                    "driver-link-form" }))
                   { %>
                        <body  id="exportbody" class="text-align: center;">
                        <table id="drvTable" width="50%" style="float:left;">
                            <tr>
                                <td><%= Html.Label(Localizer.Current.GetString("Drivers"))%></td>
                            </tr>
                             <% foreach(sysUser drv in drivers as List<sysUser>) { %>
                                 <tr><td><%= Html.CheckBox(drv.Id.ToString(), false, new {value = drv.Id, style ="width:150"})%><span><%= drv.UserName %></span></td></tr>
                                <% } %>
                        </table>
                        <table id="templateTbl" width="50%" style="float:left;">
                            <tr>
                                <td><%= Html.Label(Localizer.Current.GetString("Templates"))%></td>
                            </tr>
                                <% foreach (tblTargetTemplate tblTemplate in templates as List<tblTargetTemplate>){ %>
                                <tr><td><%= Html.CheckBox("chkDriver" + tblTemplate.Id, false, new { value = tblTemplate.Id, @class = "chkBox", style = "width:150", })%><span><%= tblTemplate.TemplateName%></span></td></tr>                           
                                <% } %>
                        </table>
                         <%= Html.Hidden("selectedTemplate") %>
                        <br /> <br />
                        <div class="form-actions">
                        <br /> <br />
                            <button id="btnLink" type="submit" class="t-button"><%= Localizer.Current.GetString("Link") %></button>
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
               $('#link-driver-open-button')
                    .click(function(e) {
                        e.preventDefault();
                        $('#LinkWindow').data('tWindow').center().open();
                    });
                
                //Submit button function gets grid filter/sortby and current page values
                $('#btnLink')
                    .click(function(e) {
                    var c = 0;
                  
                    $('.chkBox').each(function () 
                    {                      
                        if($(this).is(':checked'))
                        {
                            c = c + 1;
                            var $obj = $(this);
                            var val = $obj.val();
                            $('#selectedTemplate').val($obj.val());
                            
                        }                       
                    });
                    if(c > 1)
                    {
                        alert("Please only select one template");
                        e.preventDefault();
                    }
                   });


          <%}); %>

<%--End Export code--%>