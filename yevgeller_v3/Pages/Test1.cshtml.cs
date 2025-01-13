using Microsoft.AspNetCore.Http.HttpResults;
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

        public void OnPostEdit(string answer)
        {
            var j = answer;
            var a = 1;

            tq = repository.GetNextQuestion();
            //Message = "Edit handler fired";
        }

        //public async Task<IActionResult> OnPostProcessAsync(string answer)
        public IActionResult OnPostProcess(string answer)
        {
            var found = tq.Answers.FirstOrDefault(x=>x.Answer == answer);

            if (found == null) return NotFound();

            tq = repository.GetNextQuestion();

            return null;
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
