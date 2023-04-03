using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VendorInvoicing.TagHelpers
{
    [HtmlTargetElement("last-action-message-undo")]
    public class LastActionMessageUndoTagHelper : TagHelper
    {
        [ViewContext()]
        [HtmlAttributeNotBound()]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("deleted-vendor-id")]
        public int DeletedVendorId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.TempData.ContainsKey("LastActionMessageUndo"))
            {
                // generate action URL:
                var linkGenerator = (LinkGenerator)ViewContext.HttpContext.RequestServices.GetService(typeof(LinkGenerator));
                var undoUrl = linkGenerator.GetPathByAction("UndoDelete", "Vendor", new { id = DeletedVendorId });

                // build outer div:
                var outerDiv = new TagBuilder("div");
                outerDiv.AddCssClass("alert alert-success alert-dismissible fade show");
                outerDiv.Attributes.Add("role", "alert");

                // build inner divs
                var col10Div = new TagBuilder("div");
                col10Div.AddCssClass("col-sm-10");

                // build message span:
                var messageSpan = new TagBuilder("span");
                messageSpan.InnerHtml.Append(ViewContext.TempData["LastActionMessageUndo"].ToString());

                // build form tag:
                var form = new TagBuilder("form");
                form.Attributes.Add("asp-controller", "Vendor");
                form.Attributes.Add("asp-action", "UndoDelete");
                form.Attributes.Add("method", "post");
                form.Attributes.Add("enctype", "application/x-www-form-urlencoded");

                // build route parameter for vendor id:
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
                // append form, message span to inner div secion 1:
                col10Div.InnerHtml.AppendHtml(messageSpan);
                col10Div.InnerHtml.AppendHtml(form);

                var col2Div = new TagBuilder("div");
                col2Div.AddCssClass("col-sm-2");

                // build close button:
                var closeBtn = new TagBuilder("button");
                closeBtn.Attributes.Add("type", "button");
                closeBtn.AddCssClass("btn-close");
                closeBtn.Attributes.Add("data-bs-dismiss", "alert");
                closeBtn.Attributes.Add("aria-label", "close");

                col2Div.InnerHtml.AppendHtml(closeBtn);

                outerDiv.InnerHtml.AppendHtml(col10Div);
                outerDiv.InnerHtml.AppendHtml(col2Div);

                form.Attributes.Add("action", undoUrl);

                // set output content to be the outer div:
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                //output.Content.AppendHtml(outerDiv);
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
