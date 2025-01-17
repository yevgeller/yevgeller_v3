using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models.BdpqTestingFramework;


namespace yevgeller_v3.Pages
{
    public class Test1Model : PageModel
    {
        public readonly IBdpqTestingFramework repo;
        public TestQuestion tq = new();

        public Test1Model(IBdpqTestingFramework repository)
        {
            this.repo = repository;
            tq = new();
        }

        public void OnGet()
        {
            tq = repo.ProcessAnswer(string.Empty);
        }
        public void OnPostEdit(string answer)
        {
            tq = repo.ProcessAnswer(answer);
        }
    }
}
