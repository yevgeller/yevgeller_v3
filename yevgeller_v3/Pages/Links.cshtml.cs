using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models;

namespace yevgeller_v3.Pages
{
    public class LinksModel : PageModel
    {
        [FromQuery(Name = "param")]
        public string Param { get; set; } = string.Empty;
        internal Store store;

        public LinksModel()
        {
            this.store = new Store();
        }

       internal List<Article> currentSelection = new List<Article>();
        internal List<string> groupHeaders = new List<string>();
        public void OnGet(string param)
        {
            Param = param;
            Store s = new Store();
            if (param != null)
            {
                currentSelection = s.GetArticlesByCategory(param);
            }
            else
            {
                currentSelection = s.GetAllArticles();
            }

            groupHeaders = s.GetDistinctGroupHeaders(param);
        }
    }
}
