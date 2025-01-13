using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models.BdpqTestingFramework;


namespace yevgeller_v3.Pages
{
    public class Test1Model : PageModel
    {
        public Repository repository;
        public TestQuestion tq;
        public void OnGet()
        {
            if (repository == null) repository = new Repository();

            tq = repository.GetNextQuestion();
        }


    }
}
