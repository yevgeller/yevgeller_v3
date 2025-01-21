using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models.BdpqTestingFramework;


namespace yevgeller_v3.Pages
{
    public class Test1Model : PageModel
    {
        public readonly IBdpqTestingFramework repo;
        public TestQuestion tq = new();
        public List<TestItemStatistic> stats = new List<TestItemStatistic>();

        public Test1Model(IBdpqTestingFramework repository)
        {
            this.repo = repository;
            tq = new();
        }

        public void OnGet()
        {
            tq = repo.ProcessAnswer(string.Empty);
            stats = repo.GetTestItemStatistics().OrderBy(x => x.Item).ToList();
        }
        public void OnPostEdit(string answer)
        {
            tq = repo.ProcessAnswer(answer);
            stats = repo.GetTestItemStatistics().OrderBy(x => x.Item).ToList();
        }

        public void OnPostReset()
        {
            repo.ResetStatistics();
            tq = repo.ProcessAnswer(string.Empty);
        }

        public int TotalAnsweredQuestions()
        {
            if (stats == null) return 0;
            if(stats.Count == 0) return 0;
            return stats.Sum(x => x.Correct);
        }
    }
}