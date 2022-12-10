using Microsoft.AspNetCore.Razor.TagHelpers;
using yevgeller_v3.Models;

namespace yevgeller_v3.TagHelpers
{
    public class ArticleAsListItemTagHelper : TagHelper
    {
        public Article Article { get; set; } = new Article();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";    // Replaces <email> with <a> tag
            output.Attributes.SetAttribute("class", "mb-2 is-size-5 is-large");
            output.Content.AppendHtml($@"<a href='{this.Article.URL}'>{this.Article.Description}</a>");
            if(!string.IsNullOrWhiteSpace(Article.Comment))
            {
                output.Content.AppendHtml($@"<br />{this.Article.Comment}");
            }
        }
    }
}
