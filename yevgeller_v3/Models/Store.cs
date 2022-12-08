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
            articles
            //    categories.Add(new Category { Name = ""}
            //articles.Add(new Article { URL = "", Description = "", CategoryName = `});
        }
    }
}

