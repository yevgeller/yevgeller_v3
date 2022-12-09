using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models;

namespace yevgeller_v3.Pages
{
    public class LinksModel : PageModel
    {
        [FromQuery(Name = "param")]
        public string Param { get; set; }

        List<Article> currentSelection = new List<Article>();
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

            var j = 1;
        }
    }
}
