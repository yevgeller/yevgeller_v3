using System.Runtime.CompilerServices;

namespace yevgeller_v3.Models
{
    public class Store
    {
        private List<Article> articles = new List<Article>();
        private List<Category> categories = new List<Category>();

        public List<Category> InitializeCategories()
        {
            List<Category> result = new List<Category>();
            result.Add(new Category { Name = "Links" });
            result.Add(new Category { Name = "Articles" });
            result.Add(new Category { Name = "Long reads" });
            result.Add(new Category { Name = "Products" });
            return result;
        }

        public Store()
        {
            articles.Add(new Article("https://yevgellerdesignpatterns.azurewebsites.net/", "My review of Software Design Patterns", "trunk", "Links" ));
            articles.Add(new Article("https://github.com/rust-lang/rustlings", "Rustlings", "Rust resources", "Links"));
            articles.Add(new Article("https://nostarch.com/rust-programming-language-2nd-edition", "Rust book (No Starch Press)", "Rust resources", "Links"));
            articles.Add(new Article("https://javascript.plainenglish.io/top-javascript-one-liners-will-make-you-a-superhero-af1363083354", "UsefulOne-liners", "JavaScript resources", "Articles"));
<<<<<<< HEAD
            articles.Add(new Article("#", "Design a lift system", "Project Ideas", "Project Ideas"));
            var art = new Article("#", "Design a vending machine -- should accept 1, 5, 10, 25 cents, 1, 2 dollar note", "Project Ideas", "Project Ideas");
            art.Comment = "Products: Candy(10), Snack (50), Nuts(90), Coke (25), Pepsi (35), Soda (45)";
            articles.Add(art);
            articles.Add(new Article("#", "Design a traffic controller system for a junction", "Project Ideas", "Project Ideas"));
=======
        //copy all other links here
>>>>>>> Interim commit
        }

        public List<Article> GetAllArticles() => articles;
        public List<Article> GetArticlesByCategory(string category) => 
            articles.Where(x => x.Category.ToLower() == category.ToLower().Trim()).ToList();

        public List<string> GetDistinctCategories() => articles.Select(x=>x.Category).Distinct().ToList();
    }
}

