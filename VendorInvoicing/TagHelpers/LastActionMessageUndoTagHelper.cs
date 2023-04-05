using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using static System.Collections.Specialized.BitVector32;

namespace VendorInvoicing.TagHelpers
{
    [HtmlTargetElement("last-action-message-undo")]
    public class LastActionMessageUndoTagHelper : TagHelper
    {
        [ViewContext()]
        [HtmlAttributeNotBound()]
        public ViewContext ViewContext { get; set; }

        //Parameter to pass into taghelper
        [HtmlAttributeName("deleted-vendor-id")]
        public int DeletedVendorId { get; set; }
        //Generate button linked with vendorId
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.TempData.ContainsKey("LastActionMessageUndo"))
            {
                /*<div class="alert alert-success alert-dismissible fade show text-nowrap" role="alert">
                    <div class="align-items-center d-inline-flex">
                        <span class="p-1">This is a message</span>
                        <form asp-controller="Vendor" asp-action="UndoDelete" method="post" enctype="application/x-www-form-urlencoded">
                            <input type="hidden" name="id" value="@Model.DeletedVendorId" />
                            <button type="submit" class="btn btn-primary p-1" data-bs-dismiss="alert" aria-label="Undo">Undo</button>
                        </form>
                    </div>
                    <div><button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>
                </div>*/

                // generate action URL:
                var linkGenerator = (LinkGenerator)ViewContext.HttpContext.RequestServices.GetService(typeof(LinkGenerator));
                var undoUrl = linkGenerator.GetPathByAction("UndoDelete", "Vendor", new { id = DeletedVendorId });

                // build outer div:
                var outerDiv = new TagBuilder("div");
                //outerDiv.AddCssClass("alert alert-success alert-dismissible fade show text-nowrap");
                outerDiv.Attributes.Add("class", "alert alert-success alert-dismissible fade show text-nowrap");
                outerDiv.Attributes.Add("role", "alert");
                outerDiv.Attributes.Add("data-dismiss-timeout", "5000"); // 5000 milliseconds = 5 seconds

                // build message div
                var messageDiv = new TagBuilder("div");
                messageDiv.AddCssClass("align-items-center d-inline-flex");

                // build button div
                var closeButtonDiv = new TagBuilder("div");

                // build message span:
                var messageSpan = new TagBuilder("span");
                messageSpan.AddCssClass("p-1");
                messageSpan.InnerHtml.Append(ViewContext.TempData["LastActionMessageUndo"].ToString());

                // build form tag:
                var form = new TagBuilder("form");
                form.AddCssClass("container");
                form.Attributes.Add("asp-controller", "Vendor");
                form.Attributes.Add("asp-action", "UndoDelete");
                form.Attributes.Add("method", "post");
                form.Attributes.Add("enctype", "application/x-www-form-urlencoded");

                // build query route parameter for vendor id:
                var routeParam = new TagBuilder("input");
                routeParam.Attributes.Add("type", "hidden");
                routeParam.Attributes.Add("name", "id");
                routeParam.Attributes.Add("value", DeletedVendorId.ToString());

                // build undo button:
                var undoBtn = new TagBuilder("button");
                undoBtn.Attributes.Add("type", "submit");
                undoBtn.Attributes.Add("class", "btn btn-primary");
                undoBtn.Attributes.Add("data-bs-dismiss", "alert");
                undoBtn.Attributes.Add("aria-label", "Undo");
                undoBtn.InnerHtml.Append("Undo");

                form.InnerHtml.AppendHtml(routeParam);
                form.InnerHtml.AppendHtml(undoBtn);

                messageDiv.InnerHtml.AppendHtml(messageSpan);
                messageDiv.InnerHtml.AppendHtml(form);

                // build close button:
                var closeBtn = new TagBuilder("button");
                closeBtn.Attributes.Add("type", "button");
                closeBtn.AddCssClass("btn-close");
                closeBtn.Attributes.Add("data-bs-dismiss", "alert");
                closeBtn.Attributes.Add("aria-label", "close");

                closeButtonDiv.InnerHtml.AppendHtml(closeBtn);

                outerDiv.InnerHtml.AppendHtml(messageDiv);
                outerDiv.InnerHtml.AppendHtml(closeButtonDiv);

                form.Attributes.Add("action", undoUrl);

                // set output content to be the outer div:
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                //output.Content.AppendHtml(outerDiv); (didnt work, needed async call)
                output.Content.SetHtmlContent(await Task.FromResult(outerDiv));
            }
            else
            {
                // no output:
                output.SuppressOutput();
            }
        }
    }
}
