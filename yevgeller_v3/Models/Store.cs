using System.Runtime.CompilerServices;

namespace yevgeller_v3.Models
{
    public class Store
    {
        private List<Article> articles = new List<Article>();
        private List<Category> categories = new List<Category>();

        //public List<Category> InitializeCategories()
        //{
        //    List<Category> result = new List<Category>();
        //    result.Add(new Category { Name = "Links" });
        //    result.Add(new Category { Name = "Articles" });
        //    result.Add(new Category { Name = "Long reads" });
        //    result.Add(new Category { Name = "Products" });
        //    return result;
        //}

        public Store()
        {
            //Links
            articles.Add(new Article("https://yevgellerdesignpatterns.azurewebsites.net/", "My review of Software Design Patterns", "trunk", "Links"));
            articles.Add(new Article("https://yevgellerdesignpatterns.azurewebsites.net/Game1", "Mines game v2", "trunk", "Links"));
            articles.Add(new Article("https://github.com/rust-lang/rustlings", "Rustlings", "Rust resources", "Links"));
            articles.Add(new Article("https://nostarch.com/rust-programming-language-2nd-edition", "Rust book (No Starch Press)", "Rust resources", "Links"));
            //Exercises
            articles.Add(new Article("https://laracasts.com/topics/vue", "Laracasts", "Vue", "Exercises"));
            articles.Add(new Article("https://exercism.org/", "Exercism", "Exercises", "Exercises"));
            articles.Add(new Article("https://adventofcode.com/", "Advent of Code", "Exercises", "Exercises"));

            //Articles
            articles.Add(new Article("https://javascript.plainenglish.io/top-javascript-one-liners-will-make-you-a-superhero-af1363083354", "Useful One-liners", "JavaScript resources", "Articles"));
            articles.Add(new Article("https://deepu.tech/concurrency-in-modern-languages-js/", "Concurrency in JS", "JavaScript", "Articles"));
            articles.Add(new Article("https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net60&pivots=development-environment-vs", "Deploy an ASP.Net Web App", "Azure", "Articles"));

            //Long reads
            articles.Add(new Article("https://martinfowler.com/articles/evodb.html", "Evolutionary Database Design", "", "Long reads"));
            articles.Add(new Article("http://craftinginterpreters.com/contents.html", "Crafting Interpreters", "", "Long reads"));
            articles.Add(new Article("http://gameprogrammingpatterns.com/", "Game Programming Patterns", "", "Long reads"));

            //Project Ideas
            articles.Add(new Article("#", "Design a lift system", "Project Ideas", "Project Ideas"));
            articles.Add(new Article { URL = "#", Description = "Design a vending machine -- should accept 1, 5, 10, 25 cents, 1, 2 dollar note", GroupHeader = "Project Ideas", Category = "Project Ideas", Comment = "Products: Candy(10), Snack (50), Nuts(90), Coke (25), Pepsi (35), Soda (45)" });
            articles.Add(new Article("#", "Design a traffic controller system for a junction", "Project Ideas", "Project Ideas"));

            //Old
            articles.Add(new Article("/static/encoder.html", "JS text encoder that prevents machine recognition", "Old", "Old"));
            articles.Add(new Article("/static/jsClock.html", "JS canvas clock", "Old", "Old"));
            articles.Add(new Article("/static/mines.html", "Minesweeper v1", "Old", "Old"));
            articles.Add(new Article("/static/yevgeller_v1.html", "List of old projects", "Old", "Old"));
            //Random
            articles.Add(new Article("https://justjavascript.com/", "Just JavaScript", "JavaScript", "Random"));
        }

        public string GetProperlyCapitalizedCategoryName(string category)
        {
            var items = GetArticlesByCategory(category);
            if (items.Count > 0) return items[0].Category;

            return "No category";
        }

        public List<Article> GetAllArticles() => articles;
        public List<Article> GetArticlesByCategory(string category) =>
            articles.Where(x => category == null || x.Category.ToLower() == category.ToLower().Trim()).ToList();

        public List<string> GetDistinctGroupHeaders(string category)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                return this.GetArticlesByCategory(category)
                .Select(x => x.GroupHeader)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            }

            return this.GetAllArticles()
                .Select(x => x.GroupHeader)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        public List<string> GetDistinctCategories() => articles.Select(x => x.Category).Distinct().ToList();
    }
}

