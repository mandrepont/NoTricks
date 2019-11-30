using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace NoTrick.Web.TagHelps {
    [HtmlTargetElement(Attributes = PageAttributeName)]
    [HtmlTargetElement(Attributes = ClassAttributeName)]
    [HtmlTargetElement(Attributes = ExactAttributeName)]
    public class ActiveRouteTagHelper : TagHelper {
        private const string PageAttributeName = "asp-active-pages";
        private const string ClassAttributeName = "asp-active-class";
        private const string ExactAttributeName = "asp-active-exact";


        [HtmlAttributeName(PageAttributeName)]
        public string Page { get; set; }

        [HtmlAttributeName(ExactAttributeName)]
        public bool Exact { get; set; }

        [HtmlAttributeName(ClassAttributeName)]
        public string Class { get; set; } = "active";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            RouteValueDictionary routeValues = ViewContext.RouteData.Values;
            string currentPage = routeValues["page"].ToString();

            string[] acceptedPages = Page.Trim().Split(',').Distinct().ToArray();

            bool Compare(string x) {
                return Exact ? x == currentPage : currentPage.Contains(x, StringComparison.InvariantCultureIgnoreCase);
            }

            if (acceptedPages.Any(Compare)) {
                SetAttribute(output, "class", Class);
            }

            base.Process(context, output);
        }

        private void SetAttribute(TagHelperOutput output, string attributeName, string value, bool merge = true) {
            var v = value;
            TagHelperAttribute attribute;
            if (output.Attributes.TryGetAttribute(attributeName, out attribute)) {
                if (merge) {
                    v = $"{attribute.Value} {value}";
                }
            }
            output.Attributes.SetAttribute(attributeName, v);
        }
    }
}
