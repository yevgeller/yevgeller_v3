using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using yevgeller_v3.Models.BdpqTestingFramework;


namespace yevgeller_v3.Pages
{
    public class Test1Model : PageModel
    {
        //public Repository repository = new Repository();
        public readonly IBdpqTestingRepository repo;
        public TestQuestion tq;

        public Test1Model(IBdpqTestingRepository repository)
        {
            this.repo = repository;
            tq = repo.GetNextQuestion();
        }

        public void OnGet()
        {
            tq = repo.GetNextQuestion();
        }

        public void OnPostEdit(string answer)
        {
            var j = answer;
            var a = 1;

            tq = repo.GetNextQuestion();
            //Message = "Edit handler fired";
        }

        //public async Task<IActionResult> OnPostProcessAsync(string answer)
        public IActionResult OnPostProcess(string answer)
        {
            var found = tq.Answers.FirstOrDefault(x=>x.Answer == answer);

            if (found == null) return NotFound();

            tq = repo.GetNextQuestion();

            //return null;
            //var contact = await _context.Customer.FindAsync(id);

            //if (contact != null)
            //{
            //    _context.Customer.Remove(contact);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToPage();
        }
    }
}
