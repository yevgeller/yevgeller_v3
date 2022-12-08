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
            //articles
            //    categories.Add(new Category { Name = ""}
            articles.Add(new Article("https://yevgellerdesignpatterns.azurewebsites.net/", "My review of Software Design Patterns", "trunk", "Links" ));
            articles.Add(new Article("https://github.com/rust-lang/rustlings", "Rustlings", "Rust resources", "Links"));
            articles.Add(new Article("https://nostarch.com/rust-programming-language-2nd-edition", "Rust book (No Starch Press)", "Rust resources", "Links"));
            articles.Add(new Article("https://javascript.plainenglish.io/top-javascript-one-liners-will-make-you-a-superhero-af1363083354", "UsefulOne-liners", "JavaScript resources", "Articles"));
        }

        public List<Article> GetAllArticles() => articles;
        public List<Article> GetArticlesByCategory(string category) => 
            articles.Where(x => x.Category.ToLower() == category.ToLower().Trim()).ToList();

        public List<string> GetDistinctCategories() => articles.Select(x=>x.Category).Distinct().ToList();
    }
}

