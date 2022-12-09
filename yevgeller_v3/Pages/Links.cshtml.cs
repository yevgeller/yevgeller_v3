using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace yevgeller_v3.Pages
{
    public class LinksModel : PageModel
    {
        [FromQuery(Name = "param")]
        public string Param { get; set; }
        public void OnGet(string param)
        {
            Param = param;
        }
    }
}
