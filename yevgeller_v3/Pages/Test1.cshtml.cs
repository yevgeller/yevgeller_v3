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

/* Other HTML:
 * 
 * 

        <!--
         <p> @Model.tq.Question</p>

        @foreach (var a in Model.tq.Answers)
        {
                     <div>
                         <input type="radio" id="answer@i" name="SelectedAnswer" value="@a.Answer" />
                         <label for="answer@i">@a.Answer</label>
                     </div>
        }
        -->
    </span>

    @foreach (var t in @Model.repo.GetTestItemsByCategory()
                               .OrderBy(x => x.QuestionType)
                               .ThenBy(x => x.QuestionCategory))
    {
        <div style="font-size:3rem;">@Html.Raw(t.IconAsHtml()) @t.ToString()</div>
    }

    <h1>Font Awesome Icons</h1>
    <i class="fas fa-bicycle"></i>
    <i class="fas fa-drum"></i>
    <i class="fas fa-parking"></i>
    <hr />
    <hr />

    <h1>Test Items:</h1>

    @foreach (var t in @Model.repo.GetTestItemsForATest()
                            .OrderBy(x => x.QuestionType)
                            .ThenBy(x => x.QuestionCategory))
    {
        <div style="font-size:3rem;">@Html.Raw(t.IconAsHtml()) @t.ToString()</div>
    }

 * 
 * 
 * 
 * 
 */